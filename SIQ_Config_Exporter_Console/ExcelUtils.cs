using Syncfusion.XlsIO;
using System;
using System.Data;
using WBX.whiteOPS.Server.ReportingServices;
using static SiqConfigExport.Constants;

namespace SiqConfigExport
{

    public class ExcelUtils
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Puts some data into a worksheet and applies an Excel table style to it.
        /// </summary>
        /// <param name="table">The DataTable</param>
        /// <param name="worksheet">Worksheet object</param>
        /// <param name="tableRow">The starting row number</param>
        /// <param name="tableColumn">The starting column number</param>
        /// <param name="autofitColumns">Automatically size the Excel columns?</param>
        /// <returns></returns>
        public static string ImportTable(
            DataTable table,
            IWorksheet worksheet,
            int tableRow,
            int tableColumn,
            bool autofitColumns
        )
        {
            //Call the SIQ Excel utility
            string nameRange =
                ExcelUtilities.importTable(
                    table,
                    table.TableName,
                    worksheet,
                    tableRow + 1,
                    tableColumn,
                    autofitColumns,
                    false,
                    true,
                    TableBuiltInStyles.TableStyleLight9,
                    false,
                    false
                );
            return nameRange;
        }


        /// <summary>
        /// Create an Excel spreadsheet and add all the tables in the dataset to it.
        /// </summary>
        /// <param name="ds">A .NET DataSet</param>
        public static void CreateExcel(System.Data.DataSet ds)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2013;
            IWorkbook workbook = application.Workbooks.Create(1);

            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = SheetNames.DB_INFO;
            ImportTable(ds.Tables[SqlStmtKeys.DB_INFO1], worksheet, 1, 1, true);
            ImportTable(ds.Tables[SqlStmtKeys.DB_INFO2], worksheet, 5, 1, false);
            ImportTable(ds.Tables[SqlStmtKeys.SIQ_CONFIG1], worksheet, 8, 1, false);
            ImportTable(ds.Tables[SqlStmtKeys.SIQ_CONFIG2], worksheet, 13, 1, false);
            
            worksheet = workbook.Worksheets.Create(SheetNames.LICENSE);
            ImportTable(ds.Tables[SqlStmtKeys.LICENSE], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.CORE_SERVICES);
            ImportTable(ds.Tables[SqlStmtKeys.INSTALL_SERVER], worksheet, 1, 1, true);
            ImportTable(ds.Tables[SqlStmtKeys.INSTALL_SERVICE], worksheet, 3 + ds.Tables[SqlStmtKeys.INSTALL_SERVER].Rows.Count, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.IDC);
            ImportTable(ds.Tables[SqlStmtKeys.IDC], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.WPC);
            ImportTable(ds.Tables[SqlStmtKeys.WPC], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.APPS);
            ImportTable(ds.Tables[SqlStmtKeys.APPS], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.APP_CONFIGS);
            ImportTable(ds.Tables[SqlStmtKeys.APP_CONFIGS], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.TASKS);
            ImportTable(ds.Tables[SqlStmtKeys.TASKS], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.TASK_RESULTS);
            ImportTable(ds.Tables[SqlStmtKeys.TASK_RESULTS], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.HEALTH_CENTER);
            ImportTable(ds.Tables[SqlStmtKeys.HEALTH_CENTER], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(SheetNames.USERS);
            ImportTable(ds.Tables[SqlStmtKeys.USERS], worksheet, 1, 1, true);
            
            workbook.SaveAs(GetExcelFileNameString());
            workbook.Close();
            excelEngine.Dispose();
        }
        
        static string GetExcelFileNameString()
        {
            DateTime now = DateTime.Now;
            string strNow = "SIQ_" + now.ToString("yyyyMMddHHmm") + ".xlsx";
            return strNow;
        }
    }

    

}
