using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiqConfigReport
{
    class Main1
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string exportConfig   = "exportconfig";
        const string prereqsServers  = "prereqservers";

        static void Main(string[] args)
        {

            PrintWelcomeBanner();
            
            switch (args[0])
            {
                case exportConfig:
                    _log.Info(exportConfig);
                    SiqConfigExport.SiqConfigExport.Export_SIQ_Config();
                    break;
                case prereqsServers:
                    _log.Info(prereqsServers);
                    break;
                default:
                    _log.Info("Invalid selection.");
                    Console.ReadLine();
                    System.Environment.Exit(-1);
                    break;
            }

            SiqConfigExport.SiqConfigExport.Export_SIQ_Config();



        }



        static void PrintWelcomeBanner()
        {
            const string SAILPOINT_COPYRIGHT = "SecurityIQ Professional Services Utility v5.1.0.0  Copyright (c) 2017 - SailPoint";
            const string SAILPOINT_BANNER =    "*********************************************************************************";

            Console.Title = "SecurityIQ Professional Services Utility";

            _log.Info(SAILPOINT_BANNER);
            _log.Info(SAILPOINT_COPYRIGHT);
            _log.Info(SAILPOINT_BANNER);
        }

    }


}
