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
    public class MBSRoleFunctionDAO
    {
        #region 单实例
        private MBSRoleFunctionDAO() { }
        private static MBSRoleFunctionDAO _Instance;

        public static MBSRoleFunctionDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MBSRoleFunctionDAO();
                return MBSRoleFunctionDAO._Instance;
            }
        }
        #endregion

        public int MBS_SaveMBSRoleFunction(MBSRoleFunction wMBSRoleFunction, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wMBSRoleFunction.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO mbs_rolefunction(RoleID,FunctionID) VALUES(@wRoleID,@wFunctionID);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wRoleID", wMBSRoleFunction.RoleID);
                    wSqlCommand.Parameters.AddWithValue("@wFunctionID", wMBSRoleFunction.FunctionID);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wMBSRoleFunction.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE mbs_rolefunction SET RoleID=@wRoleID,FunctionID=@wFunctionID WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wMBSRoleFunction.ID);
                    wSqlCommand.Parameters.AddWithValue("@wRoleID", wMBSRoleFunction.RoleID);
                    wSqlCommand.Parameters.AddWithValue("@wFunctionID", wMBSRoleFunction.FunctionID);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wMBSRoleFunction.ID;
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

        public int MBS_DeleteMBSRoleFunctionList(List<MBSRoleFunction> wMBSRoleFunctionList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wMBSRoleFunctionList != null && wMBSRoleFunctionList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMBSRoleFunctionList.Count; i++)
                    {
                        if (i == wMBSRoleFunctionList.Count - 1)
                            wStringBuilder.Append(wMBSRoleFunctionList[i].ID);
                        else
                            wStringBuilder.Append(wMBSRoleFunctionList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From mbs_rolefunction WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<MBSRoleFunction> MBS_QueryMBSRoleFunctionList(int wID, int wRoleID, int wFunctionID, out int wErrorCode)
        {
            List<MBSRoleFunction> wResultList = new List<MBSRoleFunction>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM mbs_rolefunction WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wRoleID <=0 or RoleID= @wRoleID)"
                    + " and(@wFunctionID <=0 or FunctionID= @wFunctionID)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wRoleID", wRoleID);
                wSqlCommand.Parameters.AddWithValue("@wFunctionID", wFunctionID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    MBSRoleFunction wMBSRoleFunction = new MBSRoleFunction();
                    wMBSRoleFunction.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wMBSRoleFunction.RoleID = StringUtils.ParseInt(wSqlDataReader["RoleID"]);
                    wMBSRoleFunction.FunctionID = StringUtils.ParseInt(wSqlDataReader["FunctionID"]);

                    wResultList.Add(wMBSRoleFunction);
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

