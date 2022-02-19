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
    public class IPTNumberValueDAO
    {
        #region 单实例
        private IPTNumberValueDAO() { }
        private static IPTNumberValueDAO _Instance;

        public static IPTNumberValueDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IPTNumberValueDAO();
                return IPTNumberValueDAO._Instance;
            }
        }
        #endregion

        public int IPT_SaveIPTNumberValue(IPTNumberValue wIPTNumberValue, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTNumberValue.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO ipt_numbervalue(SerialNumber,StandardID,Value,CreateID,CreateTime,ShiftID,PartID) VALUES(@wSerialNumber,@wStandardID,@wValue,@wCreateID,@wCreateTime,@wShiftID,@wPartID);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wIPTNumberValue.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wStandardID", wIPTNumberValue.StandardID);
                    wSqlCommand.Parameters.AddWithValue("@wValue", wIPTNumberValue.Value);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wIPTNumberValue.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wIPTNumberValue.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wIPTNumberValue.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTNumberValue.PartID);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wIPTNumberValue.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE ipt_numbervalue SET SerialNumber=@wSerialNumber,StandardID=@wStandardID,Value=@wValue,CreateID=@wCreateID,CreateTime=@wCreateTime,ShiftID=@wShiftID,PartID=@wPartID WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wIPTNumberValue.ID);
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wIPTNumberValue.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wStandardID", wIPTNumberValue.StandardID);
                    wSqlCommand.Parameters.AddWithValue("@wValue", wIPTNumberValue.Value);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wIPTNumberValue.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wIPTNumberValue.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wIPTNumberValue.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTNumberValue.PartID);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wIPTNumberValue.ID;
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

        public int IPT_DeleteIPTNumberValueList(List<IPTNumberValue> wIPTNumberValueList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTNumberValueList != null && wIPTNumberValueList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wIPTNumberValueList.Count; i++)
                    {
                        if (i == wIPTNumberValueList.Count - 1)
                            wStringBuilder.Append(wIPTNumberValueList[i].ID);
                        else
                            wStringBuilder.Append(wIPTNumberValueList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From ipt_numbervalue WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<IPTNumberValue> IPT_QueryIPTNumberValueList(int wID, string wSerialNumber, int wStandardID, int wShiftID, int wPartID, out int wErrorCode)
        {
            List<IPTNumberValue> wResultList = new List<IPTNumberValue>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM ipt_numbervalue WHERE 1=1"
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
                    IPTNumberValue wIPTNumberValue = new IPTNumberValue();
                    wIPTNumberValue.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wIPTNumberValue.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wIPTNumberValue.StandardID = StringUtils.ParseInt(wSqlDataReader["StandardID"]);
                    wIPTNumberValue.Value = StringUtils.ParseDouble(wSqlDataReader["Value"]);

                    wIPTNumberValue.Value = Math.Round(wIPTNumberValue.Value, 2);

                    wIPTNumberValue.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wIPTNumberValue.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wIPTNumberValue.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wIPTNumberValue.PartID = StringUtils.ParseInt(wSqlDataReader["PartID"]);

                    wResultList.Add(wIPTNumberValue);
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

        public double IPT_QueryIPTNumberValue(string wSerialNumber, string wItemName, int wProductID)
        {
            double wResult = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = string.Format("SELECT Value FROM iplantmlm.ipt_numbervalue where SerialNumber=@wSerialNumber and StandardID in (SELECT ID FROM iplantmlm.ipt_standard where ItemID in (SELECT ID FROM iplantmlm.ipt_item where Name like '%{0}%') and ProductID=@wProductID);", wItemName);

                wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseDouble(wSqlDataReader["Value"]);
                    wResult = Math.Round(wResult, 2);
                }
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

