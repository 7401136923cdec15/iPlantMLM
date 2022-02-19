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
    public class IPTTextValueDAO
    {
        #region 单实例
        private IPTTextValueDAO() { }
        private static IPTTextValueDAO _Instance;

        public static IPTTextValueDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IPTTextValueDAO();
                return IPTTextValueDAO._Instance;
            }
        }
        #endregion

        public int IPT_SaveIPTTextValue(IPTTextValue wIPTTextValue, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTTextValue.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO ipt_textvalue(SerialNumber,StandardID,Value,CreateID,CreateTime,ShiftID,PartID) VALUES(@wSerialNumber,@wStandardID,@wValue,@wCreateID,@wCreateTime,@wShiftID,@wPartID);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wIPTTextValue.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wStandardID", wIPTTextValue.StandardID);
                    wSqlCommand.Parameters.AddWithValue("@wValue", wIPTTextValue.Value);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wIPTTextValue.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wIPTTextValue.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wIPTTextValue.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTTextValue.PartID);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wIPTTextValue.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE ipt_textvalue SET SerialNumber=@wSerialNumber,StandardID=@wStandardID,Value=@wValue,CreateID=@wCreateID,CreateTime=@wCreateTime,ShiftID=@wShiftID,PartID=@wPartID WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wIPTTextValue.ID);
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wIPTTextValue.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wStandardID", wIPTTextValue.StandardID);
                    wSqlCommand.Parameters.AddWithValue("@wValue", wIPTTextValue.Value);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wIPTTextValue.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wIPTTextValue.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wIPTTextValue.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTTextValue.PartID);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wIPTTextValue.ID;
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

        public int IPT_DeleteIPTTextValueList(List<IPTTextValue> wIPTTextValueList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTTextValueList != null && wIPTTextValueList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wIPTTextValueList.Count; i++)
                    {
                        if (i == wIPTTextValueList.Count - 1)
                            wStringBuilder.Append(wIPTTextValueList[i].ID);
                        else
                            wStringBuilder.Append(wIPTTextValueList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From ipt_textvalue WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<IPTTextValue> IPT_QueryIPTTextValueList(int wID, string wSerialNumber, int wStandardID, int wShiftID, int wPartID, out int wErrorCode)
        {
            List<IPTTextValue> wResultList = new List<IPTTextValue>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM ipt_textvalue WHERE 1=1"
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
                    IPTTextValue wIPTTextValue = new IPTTextValue();
                    wIPTTextValue.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wIPTTextValue.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wIPTTextValue.StandardID = StringUtils.ParseInt(wSqlDataReader["StandardID"]);
                    wIPTTextValue.Value = StringUtils.ParseString(wSqlDataReader["Value"]);
                    wIPTTextValue.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wIPTTextValue.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wIPTTextValue.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wIPTTextValue.PartID = StringUtils.ParseInt(wSqlDataReader["PartID"]);

                    wResultList.Add(wIPTTextValue);
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

        public string IPT_QueryIPTTextValue(string wSerialNumber, string wItemName, int wProductID)
        {
            string wResult = "";
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = string.Format("SELECT Value FROM iplantmlm.ipt_textvalue where SerialNumber=@wSerialNumber and StandardID in (SELECT ID FROM iplantmlm.ipt_standard where ItemID in (SELECT ID FROM iplantmlm.ipt_item where Name like '%{0}%') and ProductID=@wProductID);", wItemName);

                wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                    wResult = StringUtils.ParseString(wSqlDataReader["Value"]);
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

