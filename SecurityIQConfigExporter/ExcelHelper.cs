using System;
using System.Data;
using WBX.whiteOPS.Server.ReportingServices;
using Aspose.Cells;
using Aspose.Cells.Tables;

namespace SecurityIQConfigExporter
{
    public class ExcelHelper
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
        private void ImportTable(
            DataTable table,
            Worksheet worksheet,
            int tableRow,
            int tableColumn,
            bool autofitColumns
        )
        {
            ExcelUtilities.importTable(
                                table, 
                                table.TableName, 
                                worksheet, 
                                tableRow + 1, 
                                tableColumn, 
                                autofitColumns, 
                                false, 
                                true, 
                                TableStyleType.TableStyleLight9);
        }


        /// <summary>
        /// Create an Excel spreadsheet and add all the tables in the dataset to it.
        /// </summary>
        /// <param name="ds">A .NET DataSet</param>
        public void CreateExcel(DataSet ds)
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            //_log.Debug("creating excel application");
            //IApplication application = excelEngine.Excel;
            //application.DefaultVersion = ExcelVersion.Excel2013;
            //IWorkbook workbook = application.Workbooks.Create(1);
            //_log.Debug("done creating excel application");

            Workbook workbook = new Workbook();
            //this initializes the aspose license for us!
            ExcelUtilities.createNewWorkbook(1, ref workbook);

            _log.Debug("creating db info worksheet");
            //IWorksheet worksheet = workbook.Worksheets[0];
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = WorksheetNames.Default.dbInfo;
            ImportTable(ds.Tables["dbInfo1"], worksheet, 1, 1, true);
            ImportTable(ds.Tables["dbInfo2"], worksheet, 5, 1, false);
            ImportTable(ds.Tables["siqConfig1"], worksheet, 8, 1, false);
            ImportTable(ds.Tables["siqConfig2"], worksheet, 13, 1, false);
            _log.Debug("done creating db info worksheet");

            _log.Debug("creating license worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.license);
            ImportTable(ds.Tables["license"], worksheet, 1, 1, true);
            _log.Debug("done creating license worksheet");

            //_log.Debug("creating core service worksheet");
            //worksheet = workbook.Worksheets.Add(WorksheetNames.Default.coreServices);
            //ImportTable(ds.Tables["installServer"], worksheet, 1, 1, true);
            //ImportTable(ds.Tables["installService"], worksheet, 3 + ds.Tables["installServer"].Rows.Count, 1, true);
            //_log.Debug("done creating core service worksheet");

            _log.Debug("creating idc worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.idc);
            ImportTable(ds.Tables["idc"], worksheet, 1, 1, true);
            _log.Debug("done creating idc worksheet");

            //_log.Debug("creating wpc worksheet");
            //worksheet = workbook.Worksheets.Add(WorksheetNames.Default.wpc);
            //ImportTable(ds.Tables["wpc"], worksheet, 1, 1, true);
            //_log.Debug("done creating wpc worksheet");

            //_log.Debug("creating apps worksheet");
            //worksheet = workbook.Worksheets.Add(WorksheetNames.Default.apps);
            //ImportTable(ds.Tables["apps"], worksheet, 1, 1, true);
            //_log.Debug("done creating apps worksheet");

            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.appConfigs);
            ImportTable(ds.Tables["appConfigs"], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.tasks);
            ImportTable(ds.Tables["tasks"], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.taskResults);
            ImportTable(ds.Tables["taskResults"], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.healthCenter);
            ImportTable(ds.Tables["healthCenter"], worksheet, 1, 1, true);

            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.users);
            ImportTable(ds.Tables["users"], worksheet, 1, 1, true);

            workbook.Save(GetExcelFileName(), SaveFormat.Xlsx);

            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        
        public string GetExcelFileName()
        {
            string fileName = "SecurityIQConfigExport_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            _log.DebugFormat("file name is {0}", fileName);
            return fileName;
        }
    }
}
