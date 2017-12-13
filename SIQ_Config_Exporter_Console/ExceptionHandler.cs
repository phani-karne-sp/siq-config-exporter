using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SiqConfigExport
{
    /// <summary>
    /// Some helpers for our errors
    /// </summary>
    class ExceptionHandler
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void HandleSqlException(SqlException e)
        {
            _log.ErrorFormat("SQL Exception occurred with number: {0}", e.Number);
            _log.Error(e.Message);
            _log.Error(e.StackTrace);
        }

        public static void HandleGeneralException(Exception e)
        {
            _log.Error("An exception occurred.");
            _log.Error(e.Message);
            _log.Error(e.StackTrace);
        }
    }
}
