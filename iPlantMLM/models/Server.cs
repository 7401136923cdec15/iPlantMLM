using ShrisTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iPlantMLM
{
    public class Server
    {
        public static MySQLPool MySQLPool = new MySQLPool();

        public static string Configuration = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\";

        public static bool ERPEnable = false;

        public static int LoggerDays = 120;
        public static int ExceptionDays = 120;
        public static int DebuggerDays = 1;
    }
}