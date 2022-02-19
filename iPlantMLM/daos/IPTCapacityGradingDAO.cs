using iPlantMLM;
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
    public class IPTCapacityGradingDAO
    {
        #region 单实例
        private IPTCapacityGradingDAO() { }
        private static IPTCapacityGradingDAO _Instance;

        public static IPTCapacityGradingDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IPTCapacityGradingDAO();
                return IPTCapacityGradingDAO._Instance;
            }
        }
        #endregion

        public int IPT_SaveIPTCapacityGrading(IPTCapacityGrading wIPTCapacityGrading, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTCapacityGrading.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO ipt_capacitygrading(ProductID,Gear,LowerLimit,UpLimit,`Explain`) VALUES(@wProductID,@wGear,@wLowerLimit,@wUpLimit,@wExplain);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wIPTCapacityGrading.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wGear", wIPTCapacityGrading.Gear);
                    wSqlCommand.Parameters.AddWithValue("@wLowerLimit", wIPTCapacityGrading.LowerLimit);
                    wSqlCommand.Parameters.AddWithValue("@wUpLimit", wIPTCapacityGrading.UpLimit);
                    wSqlCommand.Parameters.AddWithValue("@wExplain", wIPTCapacityGrading.Explain);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wIPTCapacityGrading.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE ipt_capacitygrading SET ProductID=@wProductID,Gear=@wGear,LowerLimit=@wLowerLimit,UpLimit=@wUpLimit,`Explain`=@wExplain WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wIPTCapacityGrading.ID);
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wIPTCapacityGrading.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wGear", wIPTCapacityGrading.Gear);
                    wSqlCommand.Parameters.AddWithValue("@wLowerLimit", wIPTCapacityGrading.LowerLimit);
                    wSqlCommand.Parameters.AddWithValue("@wUpLimit", wIPTCapacityGrading.UpLimit);
                    wSqlCommand.Parameters.AddWithValue("@wExplain", wIPTCapacityGrading.Explain);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wIPTCapacityGrading.ID;
                }
            }
            catch (Exception ex)
            {
                GUD.SQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wErrorCode = (int)9;
            }
            finally
            {
                GUD.SQLPool.FreeConnection(wCon);
            }
            return wResult;
        }

        public int IPT_DeleteIPTCapacityGradingList(List<IPTCapacityGrading> wIPTCapacityGradingList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTCapacityGradingList != null && wIPTCapacityGradingList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wIPTCapacityGradingList.Count; i++)
                    {
                        if (i == wIPTCapacityGradingList.Count - 1)
                            wStringBuilder.Append(wIPTCapacityGradingList[i].ID);
                        else
                            wStringBuilder.Append(wIPTCapacityGradingList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From ipt_capacitygrading WHERE ID in({0});", wStringBuilder.ToString());
                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                GUD.SQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wErrorCode = (int)9;
            }
            finally
            {
                GUD.SQLPool.FreeConnection(wCon);
            }
            return wErrorCode;
        }

        public List<IPTCapacityGrading> IPT_QueryIPTCapacityGradingList(int wProductID, string wGear, out int wErrorCode)
        {
            List<IPTCapacityGrading> wResultList = new List<IPTCapacityGrading>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM ipt_capacitygrading WHERE 1=1"
                    + " and(@wProductID <=0 or ProductID= @wProductID)"
                    + " and(@wGear is null or @wGear = '' or Gear= @wGear)";

                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);
                wSqlCommand.Parameters.AddWithValue("@wGear", wGear);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                List<FPCProduct> wFPCProductList = FPCProductDAO.Instance.GetProductList();
                while (wSqlDataReader.Read())
                {
                    IPTCapacityGrading wIPTCapacityGrading = new IPTCapacityGrading();

                    wIPTCapacityGrading.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wIPTCapacityGrading.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wIPTCapacityGrading.ProductName = wFPCProductList.Exists(p => p.ProductID == wIPTCapacityGrading.ProductID) ? wFPCProductList.Find(p => p.ProductID == wIPTCapacityGrading.ProductID).ProductName : "";
                    wIPTCapacityGrading.Gear = StringUtils.ParseString(wSqlDataReader["Gear"]);
                    wIPTCapacityGrading.LowerLimit = StringUtils.ParseDouble(wSqlDataReader["LowerLimit"]);
                    wIPTCapacityGrading.LowerLimitText = wIPTCapacityGrading.LowerLimit.ToString();
                    wIPTCapacityGrading.UpLimit = StringUtils.ParseDouble(wSqlDataReader["UpLimit"]);
                    wIPTCapacityGrading.UpLimitText = wIPTCapacityGrading.UpLimit.ToString();
                    wIPTCapacityGrading.Explain = StringUtils.ParseString(wSqlDataReader["Explain"]);

                    wResultList.Add(wIPTCapacityGrading);
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                GUD.SQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wErrorCode = (int)9;
            }
            finally
            {
                GUD.SQLPool.FreeConnection(wCon);
            }
            return wResultList;
        }
    }
}

