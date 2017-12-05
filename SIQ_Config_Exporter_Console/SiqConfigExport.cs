using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using static SiqConfigExport.Constants;
using ILog = log4net.ILog;
using SiqConfigReport.Properties;

/// <summary>
/// v0.1 - The first version built for SIQ 4x
/// v5.0 - The version built for initial SIQ 5.0 release.  Recompiled on new 5.0 libs, minor chnages to text
/// </summary>
namespace SiqConfigExport
{
    
    /// <summary>
    /// The only assembily that must be included in the class path is the log4net.  The other dependencies must be in the reporting services folser
    /// </summary>
    class SiqConfigExport
    {   
        static AssemblyLoader assemblyLoader = null;
        static string hibernateFilePath = null;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// We need to hook the Hibernate assembily even before the main method.
        /// </summary>
        static SiqConfigExport() {
            _log.Info("intitializing...");
            _log.Debug("getting NHibernate path...");
            hibernateFilePath = GetHibernateCfgFilePathFromRegistry();
            _log.Debug("Hibernate file: " + hibernateFilePath);
            _log.Debug("getting assembily loader...");
            global::SiqConfigExport.SiqConfigExport.assemblyLoader = new AssemblyLoader();
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            _log.Info("intitialization complete");
        }
        

        public static void Export_SIQ_Config()
        {

            _log.Debug("Hibernate file: " + hibernateFilePath);
            _log.Debug("ReportingService Folder: " + global::SiqConfigExport.SiqConfigExport.assemblyLoader.ReportingServiceFolder);
            ConsoleReader consoleReader = new ConsoleReader();

            // Get the connection string from the NHibernate.config.xml
            NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration().Configure(hibernateFilePath);
            string hibernateConnectionString = null;
            hibernateConnectionString = config.GetProperty(global::NHibernate.Cfg.Environment.ConnectionString);
            
            ConnStringer connStringer = new ConnStringer(hibernateConnectionString, consoleReader);
            _log.Debug("Hibernate connection string: " + connStringer.HibernateConnectionString);


            bool useHibernate = true;
            Console.Write("Enter 'Y' to use the nHibernate credentials, or 'N' to use provide a different set of credentials [Y]:");
            string userInput = consoleReader.ReadLine();
            if (!String.IsNullOrEmpty(userInput) && userInput.ToLower().StartsWith("n"))
            {
                useHibernate = false;
            }
            
            connStringer.GenerateConnectionString(useHibernate);
            
            _log.Info("Getting data...");
            
            //Get the data, quit if there is a problem
            DataSet ds = new DataSet();
            try
            {
                ds = Data.GetDataFromSIQdb(connStringer.GeneratedConnectionString);
            }
            catch (SqlException e)
            {
                ExceptionHandler.HandleSqlException(e);
                ConsoleReader.PauseAndExit("There was a problem getting the data from the database, hit <enter> to exit.");
                System.Environment.Exit(-1);
            }

            _log.Info("Generating configuration report...");
            //Create the spreadsheet, quit if there is a problem
            try
            {
                ExcelUtils.CreateExcel(ds);
                ConsoleReader.PauseAndExit("The report was successfully created.  Hit <enter> to exit.");
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleGeneralException(e);
                ConsoleReader.PauseAndExit("The data was retrieved fom the database, but there was a problem creating the Excel file.  Hit <enter> to exit.");
            }
            
        }
        
        /*** This method helps load the SecurityIQ assembilies. They are likely not in a subfolder of 
         *  this executable.  Expect this type of error if the assembly is not found:
         *      "Could not load file or assembly 'Iesi.Collections, Version=1.0.1.0, Culture=neutral, 
         *      PublicKeyToken=aa95f207798dfdb4' or one of its dependencies." */
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            string assembilyName = args.Name.Split(',')[0];
            //only load it if it is in the pre-defined list.
            if (Array.Exists(assemblyLoader.ResolveTheseOnly, element => element == assembilyName))
                assembly = assemblyLoader.ResolveOnServer(assembilyName + Settings.Default.dllAssemblyFileExtension);
            return assembly;
        }
        


        /// <summary>
        /// Get the Hibernate file path from the registry.
        /// </summary>
        /// <returns>The full path to the Hibernate file from the 2 keys in the registry. It will probably be like this: C:\Program Files\SailPoint\NHibernate\hibernate.cfg.xml </returns>
        static string GetHibernateCfgFilePathFromRegistry()
        {
            // The name of the key must include a valid root.
            const string userRoot = "HKEY_LOCAL_MACHINE";
            const string subkey = @"SOFTWARE\whiteboxSecurity\whiteOPS\NHibernate";
            const string keyName = userRoot + "\\" + subkey;

            string hibernateFolder = (string)Registry.GetValue(keyName,
            "installDir",
            "NOT_EXIST");

            string hibernateCfgFile = (string)Registry.GetValue(keyName,
            "hibernateConfig",
            "NOT_EXIST");

            string hibernateFilePath = hibernateFolder + hibernateCfgFile;

            #region developerHack
            //TODO: Get rid of this developer hack
            if (!File.Exists(hibernateFilePath))
                hibernateFilePath = @"C:\Users\matt.shirilla\Documents\Visual Studio 2015\Projects\SIQ_Install_Report_5.0\ConsoleApplication1\resources\hibernate.cfg.xml";

            if (!File.Exists(hibernateFilePath))
                hibernateFilePath = null;
            #endregion developerHack

            return hibernateFilePath;
        }

    }
}
