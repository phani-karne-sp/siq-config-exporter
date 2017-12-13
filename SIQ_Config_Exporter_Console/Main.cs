using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiqConfigReport
{
    class Main1
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string exportConfig   = "exportconfig";
        const string prereqsServers  = "prereqservers";
        const string exit = "exit";

        static void Main(string[] args)
        {
            Console.Title = "SIQ Config Exporter";
            _log.InfoFormat("SailPoint Professional Services SecurityIQ configuration export tool - version {0}.", Assembly.GetExecutingAssembly().GetName().Version);

            string theCommand = String.Empty;
            if (args.Length == 0 || String.IsNullOrWhiteSpace(args[0]))
            {
                _log.Debug("running program interactively");
                string key = string.Empty;
                while (key != "0")
                {
                    PrintMenu_1();
                    key = Console.ReadLine();

                    if (key == "1")
                    {
                        theCommand = exportConfig;
                    }
                    else if (key == "0")
                    {
                        theCommand = exit;
                    }
                    else
                    {
                        _log.ErrorFormat("Command {0} unrecognized. Please try a valid menu option.", key);
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                theCommand = args[0];
                _log.DebugFormat("running program via cmd line with arg {0}", theCommand);
            }

            switch (theCommand)
            {
                case exportConfig:
                    _log.Info(exportConfig);
                    SiqConfigExport.SiqConfigExport.Export_SIQ_Config();
                    break;
                case exit:
                    _log.Info("User chose to exit tool...");
                    Environment.Exit(0);
                    break;
                default:
                    _log.InfoFormat("Invalid selection of {0}, exiting tool...", theCommand);
                    Environment.Exit(-1);
                    break;
            }

            Console.ReadKey();
        }

        static void PrintMenu_1()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Export SecurityIQ config");
            Console.WriteLine();
            Console.Write("=>");
        }
    }
}
