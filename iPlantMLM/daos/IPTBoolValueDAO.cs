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
    public class IPTBoolValueDAO
    {
        #region 单实例
        private IPTBoolValueDAO() { }
        private static IPTBoolValueDAO _Instance;

        public static IPTBoolValueDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IPTBoolValueDAO();
                return IPTBoolValueDAO._Instance;
            }
        }
        #endregion

        public int IPT_SaveIPTBoolValue(IPTBoolValue wIPTBoolValue, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTBoolValue.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO ipt_boolvalue(SerialNumber,StandardID,Value,CreateID,CreateTime,ShiftID,PartID) VALUES(@wSerialNumber,@wStandardID,@wValue,@wCreateID,@wCreateTime,@wShiftID,@wPartID);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wIPTBoolValue.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wStandardID", wIPTBoolValue.StandardID);
                    wSqlCommand.Parameters.AddWithValue("@wValue", wIPTBoolValue.Value);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wIPTBoolValue.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wIPTBoolValue.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wIPTBoolValue.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTBoolValue.PartID);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wIPTBoolValue.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE ipt_boolvalue SET SerialNumber=@wSerialNumber,StandardID=@wStandardID,Value=@wValue,CreateID=@wCreateID,CreateTime=@wCreateTime,ShiftID=@wShiftID,PartID=@wPartID WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wIPTBoolValue.ID);
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wIPTBoolValue.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wStandardID", wIPTBoolValue.StandardID);
                    wSqlCommand.Parameters.AddWithValue("@wValue", wIPTBoolValue.Value);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wIPTBoolValue.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wIPTBoolValue.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wIPTBoolValue.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTBoolValue.PartID);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wIPTBoolValue.ID;
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

        public int IPT_DeleteIPTBoolValueList(List<IPTBoolValue> wIPTBoolValueList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTBoolValueList != null && wIPTBoolValueList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wIPTBoolValueList.Count; i++)
                    {
                        if (i == wIPTBoolValueList.Count - 1)
                            wStringBuilder.Append(wIPTBoolValueList[i].ID);
                        else
                            wStringBuilder.Append(wIPTBoolValueList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From ipt_boolvalue WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<IPTBoolValue> IPT_QueryIPTBoolValueList(int wID, string wSerialNumber, int wStandardID, int wShiftID, int wPartID, out int wErrorCode)
        {
            List<IPTBoolValue> wResultList = new List<IPTBoolValue>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM ipt_boolvalue WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wSerialNumber is null or @wSerialNumber = '' or SerialNumber= @wSerialNumber)"
                    + " and(@wStandardID <=0 or StandardID= @wStandardID)"
                    + " and(@wPartID <=0 or PartID= @wPartID)"
                    + " and(@wShiftID <=0 or ShiftID= @wShiftID)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@wStandardID", wStandardID);
                wSqlCommand.Parameters.AddWithValue("@wShiftID", wShiftID);
                wSqlCommand.Parameters.AddWithValue("@wPartID", wPartID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    IPTBoolValue wIPTBoolValue = new IPTBoolValue();
                    wIPTBoolValue.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wIPTBoolValue.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wIPTBoolValue.StandardID = StringUtils.ParseInt(wSqlDataReader["StandardID"]);
                    wIPTBoolValue.Value = StringUtils.ParseInt(wSqlDataReader["Value"]);
                    wIPTBoolValue.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wIPTBoolValue.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wIPTBoolValue.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wIPTBoolValue.PartID = StringUtils.ParseInt(wSqlDataReader["PartID"]);

                    wResultList.Add(wIPTBoolValue);
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

        public int IPT_QueryIPTBoolValue(string wSerialNumber, string wItemName, int wProductID)
        {
            int wResult = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = string.Format("SELECT Value FROM iplantmlm.ipt_boolvalue where SerialNumber=@wSerialNumber and StandardID in (SELECT ID FROM iplantmlm.ipt_standard where ItemID in (SELECT ID FROM iplantmlm.ipt_item where Name like '%{0}%') and ProductID=@wProductID);", wItemName);

                wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                    wResult = StringUtils.ParseInt(wSqlDataReader["Value"]);
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                GUD.SQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            finally
            {
                GUD.SQLPool.FreeConnection(wCon);
            }
            return wResult;
        }
    }
}

