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
    public class MESParamsDAO
    {
        #region 单实例
        private MESParamsDAO() { }
        private static MESParamsDAO _Instance;
        /// <summary>
        /// 单实例
        /// </summary>
        public static MESParamsDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MESParamsDAO();
                return MESParamsDAO._Instance;
            }
        }
        #endregion

        public MESParams GetMESParams()
        {
            MESParams wResult = new MESParams();
            try
            {
                wResult = XMLTool.Instance.GetXmlItem<MESParams>(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "MESParams.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public void SaveMESParams(MESParams wMESParams)
        {
            try
            {
                XMLTool.Instance.SetXmlItem<MESParams>(wMESParams, System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "MESParams.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}

