using Syncfusion.XlsIO;
using System;
using System.Data;
using WBX.whiteOPS.Server.ReportingServices;
using static SiqConfigExport.Constants;
using SiqConfigReport;

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
            string nameRange = null;
                //ExcelUtilities.importTable(
                //    table,
                //    table.TableName,
                //    worksheet,
                //    tableRow + 1,
                //    tableColumn,
                //    autofitColumns,
                //    false,
                //    true,
                //    TableBuiltInStyles.TableStyleLight9,
                //    false,
                //    false
                //);
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
            worksheet.Name = WorksheetNames.Default.dbInfo;
            ImportTable(ds.Tables[Queries.Default.dbInfo1], worksheet, 1, 1, true);
            ImportTable(ds.Tables[Queries.Default.dbInfo2], worksheet, 5, 1, false);
            ImportTable(ds.Tables[Queries.Default.siqConfig1], worksheet, 8, 1, false);
            ImportTable(ds.Tables[Queries.Default.siqConfig2], worksheet, 13, 1, false);
            
            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.license);
            ImportTable(ds.Tables[Queries.Default.license], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.coreServices);
            ImportTable(ds.Tables[Queries.Default.installServer], worksheet, 1, 1, true);
            ImportTable(ds.Tables[Queries.Default.installService], worksheet, 3 + ds.Tables[Queries.Default.installServer].Rows.Count, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.idc);
            ImportTable(ds.Tables[Queries.Default.idc], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.wpc);
            ImportTable(ds.Tables[Queries.Default.wpc], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.apps);
            ImportTable(ds.Tables[Queries.Default.apps], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.appConfigs);
            ImportTable(ds.Tables[Queries.Default.appConfigs], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.tasks);
            ImportTable(ds.Tables[Queries.Default.tasks], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.taskResults);
            ImportTable(ds.Tables[Queries.Default.taskResults], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.healthCenter);
            ImportTable(ds.Tables[Queries.Default.healthCenter], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Create(WorksheetNames.Default.users);
            ImportTable(ds.Tables[Queries.Default.users], worksheet, 1, 1, true);
            
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
