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
    public class IPTItemDAO
    {
        #region 单实例
        private IPTItemDAO() { }
        private static IPTItemDAO _Instance;

        public static IPTItemDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IPTItemDAO();
                return IPTItemDAO._Instance;
            }
        }
        #endregion

        public int IPT_SaveIPTItem(IPTItem wIPTItem, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTItem.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO ipt_item(Name,Code) VALUES(@wName,@wCode);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wName", wIPTItem.Name);
                    wSqlCommand.Parameters.AddWithValue("@wCode", wIPTItem.Code);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wIPTItem.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE ipt_item SET Name=@wName,Code=@wCode WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wIPTItem.ID);
                    wSqlCommand.Parameters.AddWithValue("@wName", wIPTItem.Name);
                    wSqlCommand.Parameters.AddWithValue("@wCode", wIPTItem.Code);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wIPTItem.ID;
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

        public int IPT_DeleteIPTItemList(List<IPTItem> wIPTItemList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTItemList != null && wIPTItemList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wIPTItemList.Count; i++)
                    {
                        if (i == wIPTItemList.Count - 1)
                            wStringBuilder.Append(wIPTItemList[i].ID);
                        else
                            wStringBuilder.Append(wIPTItemList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From ipt_item WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<IPTItem> IPT_QueryIPTItemList(int wID, string wName, string wCode, out int wErrorCode)
        {
            List<IPTItem> wResultList = new List<IPTItem>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM ipt_item WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wName is null or @wName = '' or Name= @wName)"
                    + " and(@wCode is null or @wCode = '' or Code= @wCode)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wName", wName);
                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    IPTItem wIPTItem = new IPTItem();
                    wIPTItem.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wIPTItem.Name = StringUtils.ParseString(wSqlDataReader["Name"]);
                    wIPTItem.Code = StringUtils.ParseString(wSqlDataReader["Code"]);

                    wResultList.Add(wIPTItem);
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

        internal string GetNewCode()
        {
            string wResult = "";
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT max(ID) as ID FROM iplantmlm.ipt_item;";

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    int wID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wResult = (wID + 1).ToString("0000");
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

