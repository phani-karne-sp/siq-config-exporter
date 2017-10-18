using System;
using System.IO;
using System.Reflection;
using static SiqConfigExport.Constants;

namespace SiqConfigExport
{
    /// <summary>
    /// This class helps find and load assembilies dynamically.  The only assembily reference that must be copied local is log4net.
    /// </summary>
    public class AssemblyLoader
    {
        string reportingServiceFolder = null;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// A list of SecurityIQ assembles that need to be resolved dynamically
        string[] resolveTheseOnly = { "Syncfusion.XlsIO.Base", "Syncfusion.Compression.Base", "Syncfusion.Core", "WBXReportingService",
            "log4net", "Iesi.Collections", "NHibernate", "WBX.NHibernateDAO", "WBX.ServerCore", "WBX.Common" };
        
        /// <summary>
        /// When we create the assembily loader we let itself figureout where to get the files.  TODO: In later versions the path to files will get injected.
        /// </summary>
        public AssemblyLoader()
        {
            
            const string defaultPath = @"c:\Program Files\SailPoint\SecurityIQ\ReportingService\";
            
            if (Directory.Exists(defaultPath))
                reportingServiceFolder = defaultPath;
            else if (Directory.Exists(defaultPath.Replace("c:", "d:")))
                reportingServiceFolder = defaultPath.Replace("c:", "d:");
            else if (Directory.Exists(defaultPath.Replace("c:", "e:")))
                reportingServiceFolder = defaultPath.Replace("c:", "e:");
            else if (Directory.Exists(defaultPath.Replace("c:", "f:")))
                reportingServiceFolder = defaultPath.Replace("c:", "f:");
            else if (Directory.Exists(defaultPath.Replace("g:", "g:")))
                reportingServiceFolder = defaultPath.Replace("c:", "g:");

            #region developerHack
            //TODO: get rid of hack for developer
            const string developerPath = @"C:\Users\matt.shirilla\Documents\SourceCode\SiqLibs50\";
            if (Directory.Exists(developerPath))
                reportingServiceFolder = developerPath;
            #endregion developerHack
        }

        public string ReportingServiceFolder
        {
            get
            {
                return reportingServiceFolder;
            }
            
        }

        /// <summary>
        /// Get a list of SecurityIQ assembles that need to be resolved dynamically
        /// </summary>
        public string[] ResolveTheseOnly
        {
            get
            {
                return resolveTheseOnly;
            }
        }



        /// <summary>
        /// Given a path, this method will try to resolve the assembly.  If not found on that path try 
        /// to locate the assembly in other common locations.
        /// </summary>
        /// <param name="assemblyName">The file name of the assembly (Does not have to end in .dll, that will be added in the method if it is not there.)</param>
        /// <returns></returns>
        public Assembly ResolveOnServer(string assemblyName)
        {
            Assembly assembly = null;
            try
            {//try to load the library from the path in the config file
                assembly = Resolve(reportingServiceFolder, assemblyName);
            }
            catch (Exception e)
            {
                _log.Error("Could not load the assembly " + reportingServiceFolder + assemblyName);
                ExceptionHandler.HandleGeneralException(e);
            }
            return assembly;
        }


        /// <summary>
        /// Load an assembly
        /// </summary>
        /// <param name="basePath">The path to the folder where the assembly lives</param>
        /// <param name="assemblyName">The file name of the assembly (Does not have to end in .dll, that will be added in the method if it is not there.)</param>
        /// <returns></returns>
        internal Assembly Resolve(string basePath, string assemblyName)
        {
            _log.Debug("Base path: " + basePath);
            _log.Debug("Attempting to resolve assembly: " + assemblyName);
            if (!basePath.EndsWith("\\"))
                basePath = basePath + "\\";
            if (!assemblyName.EndsWith(DOT_DLL))
                assemblyName = assemblyName + DOT_DLL;
            Assembly assembly = null;
            assembly = Assembly.LoadFile(basePath + assemblyName);
            _log.Debug("Found assembly: " + assembly.FullName);
            return assembly;
        }

    }
}
