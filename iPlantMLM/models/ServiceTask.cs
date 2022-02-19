using ShrisTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iPlantMLM
{
    public class ServiceTask
    {
        private System.Threading.Thread mThreadCycle;

        private static ServiceTask mInstance;

        public static ServiceTask Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new ServiceTask();

                return mInstance;
            }
        }

        private ServiceTask()
        {
            mThreadCycle = new System.Threading.Thread(ApplicationTask);
        }

        public bool LoadConfiguration()
        {
            bool wMESSuccess = false;
            try
            {
                string wAPPPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                Server.Configuration = wAPPPath + "Configurations\\";
                ApplicationConfig.ReadConfigFile(Server.Configuration + "Application.xml");

                LoggerTool.LoggerMode = int.Parse(ApplicationConfig.GetItemText("LoggerMode"));
                LoggerTool.ExceptionMode = int.Parse(ApplicationConfig.GetItemText("ExceptionMode"));

                #region ERP-SQLService
                Server.LoggerDays = int.Parse(ApplicationConfig.GetItemText("LoggerDays"));
                Server.ExceptionDays = int.Parse(ApplicationConfig.GetItemText("ExceptionDays"));
                Server.DebuggerDays = int.Parse(ApplicationConfig.GetItemText("DebuggerDays"));

                string wERPServerName = ApplicationConfig.GetItemText("ServerName");
                string wERPDBName = ApplicationConfig.GetItemText("DataBase");
                string wERPUserName = ApplicationConfig.GetItemText("UserName");
                string wERPPassword = ApplicationConfig.GetItemText("Password");
                int wERPPort = int.Parse(ApplicationConfig.GetItemText("Port"));

                Server.MySQLPool.SetConURL(wERPServerName, wERPDBName, wERPUserName, wERPPassword, wERPPort, 5);
                wMESSuccess = Server.MySQLPool.CreateConnection_Trial();
                if (wMESSuccess)
                    Server.MySQLPool.UpdateConnectionPool();
                else
                {
                    LoggerTool.SaveLogger("Database", "LoadConfiguration", "ERP CreateConnection_Trial start Success");
                }
                #endregion          
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wMESSuccess;
        }

        public void Start()
        {
            mThreadCycle.Start();
        }

        private void ApplicationTask()
        {
            int wTicks = 0;
            bool wExpiredFile = true;
            bool wERPDB_OK = true, wMESDB_OK = true;
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(1000);
                    if (wTicks % 3 == 0)
                      
                    if (wTicks % 10 == 0)
                    {
                        if (wERPDB_OK)
                            wERPDB_OK = Server.MySQLPool.UpdateConnectionPool();
                        if (wMESDB_OK)
                            wMESDB_OK = GUD.SQLPool.UpdateConnectionPool();
                    }
                    if (wTicks % 60 == 0)
                    {
                        if (!wERPDB_OK)
                            wERPDB_OK = Server.MySQLPool.CreateConnection_Trial();
                        if (!wMESDB_OK)
                            wMESDB_OK = GUD.SQLPool.CreateConnection_Trial();
                    }
                    if (wTicks % 1000 == 0)
                    {
                        if (DateTime.Now.Hour == 12)
                        {
                            if (wExpiredFile)
                            {
                                LoggerTool.Clear_ExpiredLoggerFile(Server.LoggerDays);
                                wExpiredFile = false;
                            }
                        }
                        else
                        {
                            wExpiredFile = true;
                        }
                        LoggerTool.SaveLogger("ServiceTask", "ApplicationTask", "System is runing");
                        wTicks = 0;
                    }
                    wTicks++;
                }
                catch (Exception ex)
                {
                    LoggerTool.SaveException("ServiceTask", "ApplicationTask", "Function Exception:" + ex.ToString());
                }
            }
        }
    }
}