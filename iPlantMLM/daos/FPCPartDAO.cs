using MySql.Data.MySqlClient;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class FPCPartDAO
    {
        #region 单实例
        private FPCPartDAO() { }
        private static FPCPartDAO _Instance;

        public static FPCPartDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FPCPartDAO();
                return FPCPartDAO._Instance;
            }
        }
        #endregion

        public List<FPCPart> GetPartList()
        {
            List<FPCPart> wResult = new List<FPCPart>();
            try
            {
                wResult = XMLTool.Instance.GetXml<FPCPart>(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "FPCPart.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
    }
}

