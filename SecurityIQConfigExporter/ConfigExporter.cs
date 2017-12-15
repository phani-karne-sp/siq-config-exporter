using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SecurityIQConfigExporter
{
    class ConfigExporter
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string exportConfig   = "exportconfig";
        const string prereqsServers  = "prereqservers";
        const string exit = "exit";

        public static bool IsInteractiveRun = false;

        static void Main(string[] args)
        {
            Console.Title = string.Format("SecurityIQ Configurtion Export Utility - {0}", Assembly.GetExecutingAssembly().GetName().Version);
            _log.InfoFormat("SailPoint Professional Services SecurityIQ configuration export tool - version {0}.", Assembly.GetExecutingAssembly().GetName().Version);

            ConfigExporter ce = new ConfigExporter();

            string theCommand = String.Empty;
            if (args.Length == 0 || String.IsNullOrWhiteSpace(args[0]))
            {
                _log.Debug("running program interactively");
                IsInteractiveRun = true;

                string key = string.Empty;
                while (key != "0")
                {
                    ce.PrintMainMenuToConsole();
                    key = Console.ReadLine();

                    if (key == "1")
                    {
                        theCommand = exportConfig;
                        break;
                    }
                    else if (key == "0")
                    {
                        theCommand = exit;
                    }
                    else
                    {
                        _log.ErrorFormat("Command {0} unrecognized. Please try a valid menu option.", key);
                    }

                    _log.DebugFormat("theCommand is {0}", theCommand);
                    Console.WriteLine();
                }
            }
            else
            {
                theCommand = args[0].ToLower();
                _log.DebugFormat("running program via cmd line with arg {0}", theCommand);
            }

            switch (theCommand)
            {
                case exportConfig:
                    ce.ExportConfigCmd();
                    break;
                case exit:
                    ce.ExitCmd();
                    break;
                default:
                    ce.DefaultCmd(theCommand);
                    break;
            }

            Console.ReadKey();
        }

        private void PrintMainMenuToConsole()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Export SecurityIQ config");
            Console.WriteLine();
            Console.Write("=>");
        }

        private void ExportConfigCmd()
        {
            _log.InfoFormat("{0} chosen", exportConfig);
            if (Util.IsSiqHibernateRegKeyPresent())
            {
                RunExportProcess();
            }
            else
            {
                _log.Error("This tool must be run on a SecurityIQ server, as it uses the hibernate config file of the SecurityIQ installtion to connect to the database.");
                Util.PressAnyKey(IsInteractiveRun);
                Environment.Exit(-1);
            }
        }

        private void ExitCmd()
        {
            _log.Info("User chose to exit tool...");
            Environment.Exit(0);
        }

        private void DefaultCmd(string theCommand)
        {
            _log.InfoFormat("Invalid command selection of {0}, exiting tool...", theCommand);
            Environment.Exit(-1);
        }

        private void RunExportProcess()
        {
            _log.InfoFormat("enter {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DataTableHelper dataTableHelper = new DataTableHelper();

            _log.Info("Filling data set for report generation...");
            DataSet ds = new DataSet();
            try
            {
                ds = dataTableHelper.GetDataFromSIQdb();
            }
            catch (SqlException e)
            {
                _log.Error("There was a problem getting the data from the database.", e);
                Util.PressAnyKey(IsInteractiveRun);
                System.Environment.Exit(-1);
            }

            _log.Info("Generating configuration report...");
            try
            {
                ExcelHelper excelHelper = new ExcelHelper();
                excelHelper.CreateExcel(ds);
                _log.Info("The report was successfully created.");
                Util.PressAnyKey(IsInteractiveRun);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                _log.Error("The data was retrieved fom the database, but there was a problem creating the Excel file.", e);
                Util.PressAnyKey(IsInteractiveRun);
                System.Environment.Exit(-2);
            }
        }
    }
}
