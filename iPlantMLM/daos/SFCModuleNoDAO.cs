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
    public class SFCModuleNoDAO
    {
        #region 单实例
        private SFCModuleNoDAO() { }
        private static SFCModuleNoDAO _Instance;

        public static SFCModuleNoDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SFCModuleNoDAO();
                return SFCModuleNoDAO._Instance;
            }
        }
        #endregion

        public int SFC_SaveSFCModuleNo(SFCModuleNo wSFCModuleNo, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wSFCModuleNo.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO sfc_moduleno(ModuleNo,ShiftID,CreateTime) VALUES(@wModuleNo,@wShiftID,@wCreateTime);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wModuleNo", wSFCModuleNo.ModuleNo);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wSFCModuleNo.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wSFCModuleNo.CreateTime);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wSFCModuleNo.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE sfc_moduleno SET ModuleNo=@wModuleNo,ShiftID=@wShiftID,CreateTime=@wCreateTime WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wSFCModuleNo.ID);
                    wSqlCommand.Parameters.AddWithValue("@wModuleNo", wSFCModuleNo.ModuleNo);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wSFCModuleNo.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wSFCModuleNo.CreateTime);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wSFCModuleNo.ID;
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

        public int SFC_DeleteSFCModuleNoList(List<SFCModuleNo> wSFCModuleNoList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wSFCModuleNoList != null && wSFCModuleNoList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wSFCModuleNoList.Count; i++)
                    {
                        if (i == wSFCModuleNoList.Count - 1)
                            wStringBuilder.Append(wSFCModuleNoList[i].ID);
                        else
                            wStringBuilder.Append(wSFCModuleNoList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From sfc_moduleno WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<SFCModuleNo> SFC_QuerySFCModuleNoList(int wID, string wModuleNo, int wShiftID, out int wErrorCode)
        {
            List<SFCModuleNo> wResultList = new List<SFCModuleNo>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM sfc_moduleno WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wModuleNo is null or @wModuleNo = '' or ModuleNo= @wModuleNo)"
                    + " and(@wShiftID <=0 or ShiftID= @wShiftID)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wModuleNo", wModuleNo);
                wSqlCommand.Parameters.AddWithValue("@wShiftID", wShiftID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    SFCModuleNo wSFCModuleNo = new SFCModuleNo();
                    wSFCModuleNo.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCModuleNo.ModuleNo = StringUtils.ParseString(wSqlDataReader["ModuleNo"]);
                    wSFCModuleNo.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wSFCModuleNo.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);

                    wResultList.Add(wSFCModuleNo);
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

        internal bool CheckRepeat(string wCapNo, string wModuleNo)
        {
            bool wResult = false;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT count(*) Number FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo!=@CapacitorPackageNo and ModuleNumber=@ModuleNumber";

                wSqlCommand.Parameters.AddWithValue("@CapacitorPackageNo", wCapNo);
                wSqlCommand.Parameters.AddWithValue("@ModuleNumber", wModuleNo);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    int wNumber = StringUtils.ParseInt(wSqlDataReader["Number"].ToString());
                    if (wNumber > 0)
                        wResult = true;
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

