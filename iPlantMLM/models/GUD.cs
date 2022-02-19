using ShrisTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iPlantMLM
{
    public class GUD
    {
        /// <summary>
        /// MySQL连接池
        /// </summary>
        public static MySQLPool SQLPool = Server.MySQLPool;

        /// <summary>
        /// 全局登录用户
        /// </summary>
        public static BMSEmployee mLoginUser = new BMSEmployee();

        /// <summary>
        /// 将下拉框拼接字符串转下拉框绑定数据源
        /// </summary>
        public static List<ComboxBinding> GetComboxBindingByString(string wComboxString)
        {
            List<ComboxBinding> wComboxBindingList = new List<ComboxBinding>();
            try
            {
                if (string.IsNullOrWhiteSpace(wComboxString))
                {
                    return wComboxBindingList;
                }

                wComboxString = wComboxString.Replace("；", ";");

                string[] wArray = wComboxString.Split(';');
                for (int i = 0; i < wArray.Length; i++)
                {
                    wComboxBindingList.Add(new ComboxBinding { ID = i + 1, Text = wArray[i] });
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wComboxBindingList;
        }

        /// <summary>
        /// 全局标准列表
        /// </summary>
        public static List<IPTStandard> mIPTStandardList = new List<IPTStandard>();

        /// <summary>
        /// 全局工位
        /// </summary>
        public static List<int> mPartIDList = new List<int>();
    }
}