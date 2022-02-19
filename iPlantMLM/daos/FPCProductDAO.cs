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
    public class FPCProductDAO
    {
        #region 单实例
        private FPCProductDAO() { }
        private static FPCProductDAO _Instance;

        public static FPCProductDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FPCProductDAO();
                return FPCProductDAO._Instance;
            }
        }
        #endregion

        public List<FPCProduct> GetProductList()
        {
            List<FPCProduct> wResult = new List<FPCProduct>();
            try
            {
                wResult = XMLTool.Instance.GetXml<FPCProduct>(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "FPCProduct.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public void SetProductList(List<FPCProduct> wList)
        {
            try
            {
                XMLTool.Instance.SetXml<FPCProduct>(wList, System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Configurations\\" + "FPCProduct.xml");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}

