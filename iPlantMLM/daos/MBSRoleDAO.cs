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
    public class MBSRoleDAO
    {
        #region 单实例
        private MBSRoleDAO() { }
        private static MBSRoleDAO _Instance;

        public static MBSRoleDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MBSRoleDAO();
                return MBSRoleDAO._Instance;
            }
        }
        #endregion

        public int MBS_SaveMBSRole(MBSRole wMBSRole, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wMBSRole.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO mbs_role(Name,OwnerID,ExplainText,Active,CreateTime) VALUES(@wName,@wOwnerID,@wExplainText,@wActive,@wCreateTime);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wName", wMBSRole.Name);
                    wSqlCommand.Parameters.AddWithValue("@wOwnerID", wMBSRole.OwnerID);
                    wSqlCommand.Parameters.AddWithValue("@wExplainText", wMBSRole.ExplainText);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wMBSRole.Active);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wMBSRole.CreateTime);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wMBSRole.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE mbs_role SET Name=@wName,OwnerID=@wOwnerID,ExplainText=@wExplainText,Active=@wActive,CreateTime=@wCreateTime WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wMBSRole.ID);
                    wSqlCommand.Parameters.AddWithValue("@wName", wMBSRole.Name);
                    wSqlCommand.Parameters.AddWithValue("@wOwnerID", wMBSRole.OwnerID);
                    wSqlCommand.Parameters.AddWithValue("@wExplainText", wMBSRole.ExplainText);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wMBSRole.Active);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wMBSRole.CreateTime);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wMBSRole.ID;
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

        public int MBS_DeleteMBSRoleList(List<MBSRole> wMBSRoleList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wMBSRoleList != null && wMBSRoleList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMBSRoleList.Count; i++)
                    {
                        if (i == wMBSRoleList.Count - 1)
                            wStringBuilder.Append(wMBSRoleList[i].ID);
                        else
                            wStringBuilder.Append(wMBSRoleList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From mbs_role WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<MBSRole> MBS_QueryMBSRoleList(int wID, int wActive, string wName, out int wErrorCode)
        {
            List<MBSRole> wResultList = new List<MBSRole>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM mbs_role WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wName is null or @wName = '' or Name= @wName)"
                    + " and(@wActive <0 or Active= @wActive)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wName", wName);
                wSqlCommand.Parameters.AddWithValue("@wActive", wActive);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    MBSRole wMBSRole = new MBSRole();
                    wMBSRole.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wMBSRole.Name = StringUtils.ParseString(wSqlDataReader["Name"]);
                    wMBSRole.OwnerID = StringUtils.ParseInt(wSqlDataReader["OwnerID"]);
                    wMBSRole.ExplainText = StringUtils.ParseString(wSqlDataReader["ExplainText"]);
                    wMBSRole.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wMBSRole.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wMBSRole.ActiveText = wMBSRole.Active == 1 ? "激活" : "关闭";
                    wMBSRole.CreateTimeText = wMBSRole.CreateTime.ToString("yyyy/MM/dd HH:mm:ss");

                    wResultList.Add(wMBSRole);
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

        public bool IsPower(BMSEmployee wLoginUser, int wFunctionID)
        {
            bool wResult = false;
            try
            {
                int wErrorCode = 0;
                List<MBSRoleFunction> wList = MBSRoleFunctionDAO.Instance.MBS_QueryMBSRoleFunctionList(-1, wLoginUser.Grad, wFunctionID, out wErrorCode);
                if (wList.Count > 0)
                    return true;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
    }
}

