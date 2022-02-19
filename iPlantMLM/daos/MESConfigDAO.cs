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
    public class MESConfigDAO
    {
        #region 单实例
        private MESConfigDAO() { }
        private static MESConfigDAO _Instance;

        public static MESConfigDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MESConfigDAO();
                return MESConfigDAO._Instance;
            }
        }
        #endregion

        public MESConfig GetMESConfig()
        {
            MESConfig wResult = new MESConfig();
            try
            {
                wResult = XMLTool.Instance.GetXmlItem<MESConfig>(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "MESConfig.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public void SaveMESConfig(MESConfig wMESConfig)
        {
            try
            {
                XMLTool.Instance.SetXmlItem<MESConfig>(wMESConfig, System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "MESConfig.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}

