using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiqConfigReport.Properties;

namespace SiqConfigReport
{
    class Util
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetSiqHome()
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            string home = Environment.GetEnvironmentVariable(Settings.Default.siqHomeEnvVar);
            _log.DebugFormat("{0} value is {1}", Settings.Default.siqHomeEnvVar, home);

            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            return home;
        }
    }
}
