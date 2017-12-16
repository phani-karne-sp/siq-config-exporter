using System;
using WBX.whiteOPS.ServerCore;
using Microsoft.Win32;
using SecurityIQConfigExporter.Properties;
using WBX.whiteOPS.DAO.NHibernate;

namespace SecurityIQConfigExporter
{
    class Util
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetSiqHome()
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return GetEnvironmentVariable(Settings.Default.siqHomeEnvVar);
        }

        public static string GetSiqHomeLogs()
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return GetEnvironmentVariable(Settings.Default.siqHomeLogsEnvVar);
        }

        public static string GetEnvironmentVariable(string varName)
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            string var = Environment.GetEnvironmentVariable(varName);
            _log.DebugFormat("{0} value is {1}", varName, var);
            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return var;
        }

        /// <summary>
        /// confirms whether keys and values are present in the registry. note you need to look in the 64-bit area.
        /// a 32-bit program will not find the default keys!
        /// </summary>
        /// <returns></returns>
        public static bool IsSiqHibernateRegKeyPresent()
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            bool found = false;

            RegistryKey hibernateRegKey = Registry.LocalMachine.OpenSubKey(Constants.NHIBERNATE_REG_KEY, false);

            if (hibernateRegKey != null)
            {
                string hibernateInstallDir = (String)hibernateRegKey.GetValue(Constants.NHIBERNATE_IDIR_KEY);
                string hibernateConfigFile = (String)hibernateRegKey.GetValue(Constants.NHIBERNATE_CONFIG_KEY);

                if (!string.IsNullOrWhiteSpace(hibernateInstallDir) && !string.IsNullOrWhiteSpace(hibernateConfigFile))
                {
                    _log.DebugFormat("Got nhibernate registry key - installDir: {0}  |  config file: {1}", hibernateInstallDir, hibernateConfigFile);
                    found = true;
                }
                else
                {
                    _log.WarnFormat("either install dir or config file values were missing under HKLM:{0}", Constants.NHIBERNATE_REG_KEY);
                }
            }
            else
            {
                _log.WarnFormat("NHibernate registry key not found in HKLM:{0}", Constants.NHIBERNATE_REG_KEY);
            }

            _log.DebugFormat("reg keys and values found: {0}", found);

            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return found;
        }

        public static void PressAnyKey(bool isInteractive)
        {
            if (isInteractive)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        public static string GetDbName()
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            string dbName = string.Empty;
            string connString = NHibernateHelper.getConnectionString(NHibernateHelper.DatabaseType.Primary);
            _log.DebugFormat("connection string: {0}", connString);
            string[] connStringArray = connString.Split(';');
            foreach (var item in connStringArray)
            {
                if (item.StartsWith("initial catalog"))
                {
                    dbName = item.Substring(item.IndexOf("=") + 1);
                }
            }
            _log.DebugFormat("dbName: {0}", dbName);
            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return dbName;
        }
    }
}
