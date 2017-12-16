using System;
using System.Data;
using WBX.whiteOPS.Server.ReportingServices;
using Aspose.Cells;
using Aspose.Cells.Tables;
using System.Text;

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
            bool autofitColumns = true,
            bool applyFormatting = true
        )
        {
            _log.DebugFormat("table row: {0} | table col {1} | dataTableRows {2} | dt name {3}", tableRow, tableColumn, table.Rows.Count, table.TableName);

            //if (_log.IsDebugEnabled)
            //{
            //    foreach (DataRow dataRow in table.Rows)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        foreach (var item in dataRow.ItemArray)
            //        {
            //            sb.Append(item + " | ");
            //        }
            //        _log.Debug(sb.ToString());
            //    }
            //}

            //padd with 1 for row & col...we can trim it off later...not padding will throw error...
            ExcelUtilities.importTable(
                                table,
                                table.TableName,
                                worksheet,
                                tableRow + 1,
                                tableColumn + 1,
                                autofitColumns,
                                false,
                                true,
                                TableStyleType.TableStyleMedium9,
                                false,
                                null,
                                applyFormatting);
        }


        /// <summary>
        /// Create an Excel spreadsheet and add all the tables in the dataset to it.
        /// </summary>
        /// <param name="ds">A .NET DataSet</param>
        public void CreateExcel(DataSet ds)
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Workbook workbook = new Workbook();
            //this initializes the aspose license for us!
            ExcelUtilities.createNewWorkbook(1, ref workbook);

            _log.Debug("creating db info worksheet");
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = WorksheetNames.Default.dbInfo;
            ImportTable(ds.Tables["dbInfo1"], worksheet, 1, 1);
            ImportTable(ds.Tables["dbInfo2"], worksheet, 3 + ds.Tables["dbInfo1"].Rows.Count, 1);
            ImportTable(ds.Tables["dbInfo3"], worksheet, 5 + ds.Tables["dbInfo2"].Rows.Count + ds.Tables["dbInfo1"].Rows.Count, 1);
            worksheet.Cells.Columns[2].Width = 75; //fix auto size issue
            TrimRowsAndColumns(worksheet); //fix trim issue for row/column padding to make utility method call work

            _log.Debug("created misc config worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.siqConfig);
            ImportTable(ds.Tables["siqConfig1"], worksheet, 1, 1);
            ImportTable(ds.Tables["siqConfig2"], worksheet, 3 + ds.Tables["siqConfig1"].Rows.Count, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating license worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.license);
            ImportTable(ds.Tables["license"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating core service worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.coreServices);
            ImportTable(ds.Tables["installServer"], worksheet, 1, 1);
            ImportTable(ds.Tables["installService"], worksheet, 3 + ds.Tables["installServer"].Rows.Count, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating idc worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.idc);
            ImportTable(ds.Tables["idc"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating wpc worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.wpc);
            ImportTable(ds.Tables["wpc"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating apps worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.apps);
            ImportTable(ds.Tables["apps"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating appConfigs worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.appConfigs);
            ImportTable(ds.Tables["appConfigs"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating tasks worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.tasks);
            ImportTable(ds.Tables["tasks"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating taskResults worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.taskResults);
            ImportTable(ds.Tables["taskResults"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating health worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.healthCenter);
            ImportTable(ds.Tables["healthCenter"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("creating users worksheet");
            worksheet = workbook.Worksheets.Add(WorksheetNames.Default.users);
            ImportTable(ds.Tables["users"], worksheet, 1, 1);
            TrimRowsAndColumns(worksheet);

            _log.Debug("saving workbook");
            workbook.Save(GetExcelFileName(), SaveFormat.Xlsx);

            _log.InfoFormat("exit {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        
        public string GetExcelFileName()
        {
            string fileName = "SecurityIQConfigExport_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            _log.DebugFormat("file name is {0}", fileName);
            return fileName;
        }

        //we need to add padding to make the excel utilities work...then we need to trim it off.
        private void TrimRowsAndColumns(Worksheet sheet)
        {
            sheet.Cells.DeleteRow(0); //leave for title?
            sheet.Cells.DeleteColumn(0);
        }
    }
}
