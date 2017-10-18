using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static SiqConfigExport.Constants;

namespace SiqConfigExport
{
    public class Data
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static DataTable getDataTable(SqlConnection sqlCn, string tableName, string strSqlStatement)
        {
            DataTable t = null;
            try
            {
                //using (sqlCn)
                //{
                    //TODO: refactor, we assume the connection is open
                    using (SqlDataAdapter a = new SqlDataAdapter(strSqlStatement, sqlCn))
                    {
                        t = new DataTable(tableName);
                        a.Fill(t);
                    }
                //}
            }
            catch (System.Exception e)
            {
                t = new DataTable(tableName);
                DataColumn column;
                DataRow row;
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "error";
                t.Columns.Add(column);
                row = t.NewRow();
                row["error"] = e.Message;
                t.Rows.Add(row);
            }
            return t;
        }


        public static DataSet GetDataFromSIQdb(string connectionString)
        {
            DataSet ds = new DataSet();
            #region UsingSqlConnection
            using (SqlConnection sqlCn = new SqlConnection(connectionString))
            {
                sqlCn.Open();
                ds.Tables.Add(getDataTable(sqlCn, 
                       SqlStmtKeys.DB_INFO1,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.DB_INFO1)));

                ds.Tables.Add(getDataTable(sqlCn,
                       SqlStmtKeys.DB_INFO2,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.DB_INFO2)));
                
                ds.Tables.Add(getDataTable(sqlCn,
                       SqlStmtKeys.SIQ_CONFIG1,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.SIQ_CONFIG1)));

                ds.Tables.Add(getDataTable(sqlCn,
                       SqlStmtKeys.SIQ_CONFIG2,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.SIQ_CONFIG2)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.EMAIL,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.EMAIL)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.LICENSE,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.LICENSE)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.INSTALL_SERVER,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.INSTALL_SERVER)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.INSTALL_SERVICE,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.INSTALL_SERVICE)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.IDC,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.IDC)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.WPC,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.WPC)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.APPS,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.APPS)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.APP_CONFIGS,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.APP_CONFIGS)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.TASKS,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.TASKS)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.TASK_RESULTS,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.TASK_RESULTS)));

                ds.Tables.Add(getDataTable(sqlCn,
                       Constants.SqlStmtKeys.HEALTH_CENTER,
                       ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.HEALTH_CENTER)));

                ds.Tables.Add(getDataTable(sqlCn,
                    Constants.SqlStmtKeys.USERS,
                    ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.USERS)));
            }
            #endregion
            return ds;
        }

        /// <summary>
        /// Get the data needed for the report from the SecurityIQ database.Let the calling method will decide what to do if there is an error.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns></returns>
        public static DataSet GetDataFromSIQdb_OLD(string connectionString)
        {
            DataSet ds = new DataSet();
            DataTable t;
            #region UsingSqlConnection
            using (SqlConnection sqlCn = new SqlConnection(connectionString))
            {
                sqlCn.Open();

                //TODO: refactor:loop the statements, try catch each
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.DB_INFO1), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.DB_INFO1);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }

                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.DB_INFO2), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.DB_INFO2);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.SIQ_CONFIG1), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.SIQ_CONFIG1);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.SIQ_CONFIG2), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.SIQ_CONFIG2);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.EMAIL), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.EMAIL);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.LICENSE), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.LICENSE);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.INSTALL_SERVER), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.INSTALL_SERVER);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.INSTALL_SERVICE), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.INSTALL_SERVICE);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.IDC), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.IDC);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.WPC), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.WPC);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.APPS), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.APPS);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.APP_CONFIGS), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.APP_CONFIGS);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.TASKS), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.TASKS);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.TASK_RESULTS), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.TASK_RESULTS);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.HEALTH_CENTER), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.HEALTH_CENTER);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
                using (SqlDataAdapter a = new SqlDataAdapter(ConfigurationManager.AppSettings.Get(Constants.SqlStmtKeys.USERS), sqlCn))
                {
                    t = new DataTable(SqlStmtKeys.USERS);
                    a.Fill(t);
                    ds.Tables.Add(t);
                }
            }
            # endregion

            return ds;
        }

    }
}
