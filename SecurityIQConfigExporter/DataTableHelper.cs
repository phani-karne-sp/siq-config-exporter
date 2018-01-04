using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WBX.whiteOPS.DAO.NHibernate;
using SecurityIQConfigExporter.Properties;

namespace SecurityIQConfigExporter
{
    public class DataTableHelper
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataSet GetDataFromSIQdb()
        {
            DataSet ds = new DataSet();

            //we utilize the session's connection and abuse it as a sqlConnection here
            using (SqlConnection dbConn = NHibernateHelper.getSession().Connection as SqlConnection)
            {
                foreach (SettingsProperty item in Queries.Default.Properties)
                {
                    _log.DebugFormat("working on item {0} with value {1}", item.Name, item.DefaultValue);
                    //note we need to format a couple strings here that contain {0} for db name
                    string query = (string)item.DefaultValue;
                    query = string.Format(query, Util.GetDbName());
                    if (!Settings.Default.enableTableRowCounts && string.Equals(item.Name, Settings.Default.rowCountQueryName))
                    {
                        //if we disabled row counts, don't run that query!
                        CreateFakeRowCountTable(ds, item);
                        continue;
                    }
                    DataTable dt = getDataTable(dbConn, item.Name, query);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        private static void CreateFakeRowCountTable(DataSet ds, SettingsProperty item)
        {
            _log.DebugFormat("row counts disabled, skipping query {0}", item.Name);
            DataTable table = new DataTable(item.Name);
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Notes", typeof(string));
            table.Rows.Add("enableTableRowCounts is false",
                "Row counts skipped. This may be acceptable, as row counts can use considerable resources in large installations. If you really want row counts and are willing to accept the potential performance impact, change enableTableRowCounts in the program's XML config to 'true' and re-run the tool.");
            ds.Tables.Add(table);
        }

        private DataTable getDataTable(SqlConnection dbConn, string tableName, string query)
        {
            DataTable t = null;

            try
            {
                _log.DebugFormat("attempting to run query {0} and put in data table with name {1}", query, tableName);
                using (SqlDataAdapter a = new SqlDataAdapter(query, dbConn))
                {
                    t = new DataTable(tableName);
                    a.Fill(t);
                }
                _log.DebugFormat("query ran ok!");
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
    }
}
