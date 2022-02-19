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
    public class BMSEmployeeDAO
    {
        #region 单实例
        private BMSEmployeeDAO() { }
        private static BMSEmployeeDAO _Instance;

        public static BMSEmployeeDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BMSEmployeeDAO();
                return BMSEmployeeDAO._Instance;
            }
        }
        #endregion

        public int BMS_SaveBMSEmployee(BMSEmployee wBMSEmployee, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wBMSEmployee.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO mbs_user(Name,LoginName,Password,CreateDate,Operator,Active,DepartmentID,Grad,Manager,Phone,Email,WeiXin,Position,PhoneMAC,Online,OnLineTime,DepartureDate,LastOnLineTime,DutyID,LoginID,Type,SuperiorID,PartPower) VALUES(@wName,@wLoginName,@wPassword,@wCreateDate,@wOperator,@wActive,@wDepartmentID,@wGrad,@wManager,@wPhone,@wEmail,@wWeiXin,@wPosition,@wPhoneMAC,@wOnline,@wOnLineTime,@wDepartureDate,@wLastOnLineTime,@wDutyID,@wLoginID,@wType,@wSuperiorID,@wPartPower);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wName", wBMSEmployee.Name);
                    wSqlCommand.Parameters.AddWithValue("@wLoginName", wBMSEmployee.LoginName);
                    wSqlCommand.Parameters.AddWithValue("@wPassword", wBMSEmployee.Password);
                    wSqlCommand.Parameters.AddWithValue("@wCreateDate", wBMSEmployee.CreateDate);
                    wSqlCommand.Parameters.AddWithValue("@wOperator", wBMSEmployee.Operator);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wBMSEmployee.Active);
                    wSqlCommand.Parameters.AddWithValue("@wDepartmentID", wBMSEmployee.DepartmentID);
                    wSqlCommand.Parameters.AddWithValue("@wGrad", wBMSEmployee.Grad);
                    wSqlCommand.Parameters.AddWithValue("@wManager", wBMSEmployee.Manager);
                    wSqlCommand.Parameters.AddWithValue("@wPhone", wBMSEmployee.Phone);
                    wSqlCommand.Parameters.AddWithValue("@wEmail", wBMSEmployee.Email);
                    wSqlCommand.Parameters.AddWithValue("@wWeiXin", wBMSEmployee.WeiXin);
                    wSqlCommand.Parameters.AddWithValue("@wPosition", wBMSEmployee.Position);
                    wSqlCommand.Parameters.AddWithValue("@wPhoneMAC", wBMSEmployee.PhoneMAC);
                    wSqlCommand.Parameters.AddWithValue("@wOnline", wBMSEmployee.Online);
                    wSqlCommand.Parameters.AddWithValue("@wOnLineTime", wBMSEmployee.OnLineTime);
                    wSqlCommand.Parameters.AddWithValue("@wDepartureDate", wBMSEmployee.DepartureDate);
                    wSqlCommand.Parameters.AddWithValue("@wLastOnLineTime", wBMSEmployee.LastOnLineTime);
                    wSqlCommand.Parameters.AddWithValue("@wDutyID", wBMSEmployee.DutyID);
                    wSqlCommand.Parameters.AddWithValue("@wLoginID", wBMSEmployee.LoginID);
                    wSqlCommand.Parameters.AddWithValue("@wType", wBMSEmployee.Type);
                    wSqlCommand.Parameters.AddWithValue("@wSuperiorID", wBMSEmployee.SuperiorID);
                    wSqlCommand.Parameters.AddWithValue("@wPartPower", wBMSEmployee.PartPower);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wBMSEmployee.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE mbs_user SET Name=@wName,LoginName=@wLoginName,Password=@wPassword,CreateDate=@wCreateDate,Operator=@wOperator,Active=@wActive,DepartmentID=@wDepartmentID,Grad=@wGrad,Manager=@wManager,Phone=@wPhone,Email=@wEmail,WeiXin=@wWeiXin,Position=@wPosition,PhoneMAC=@wPhoneMAC,Online=@wOnline,OnLineTime=@wOnLineTime,DepartureDate=@wDepartureDate,LastOnLineTime=@wLastOnLineTime,DutyID=@wDutyID,LoginID=@wLoginID,Type=@wType,SuperiorID=@wSuperiorID,PartPower=@wPartPower WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wBMSEmployee.ID);
                    wSqlCommand.Parameters.AddWithValue("@wName", wBMSEmployee.Name);
                    wSqlCommand.Parameters.AddWithValue("@wLoginName", wBMSEmployee.LoginName);
                    wSqlCommand.Parameters.AddWithValue("@wPassword", wBMSEmployee.Password);
                    wSqlCommand.Parameters.AddWithValue("@wCreateDate", wBMSEmployee.CreateDate);
                    wSqlCommand.Parameters.AddWithValue("@wOperator", wBMSEmployee.Operator);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wBMSEmployee.Active);
                    wSqlCommand.Parameters.AddWithValue("@wDepartmentID", wBMSEmployee.DepartmentID);
                    wSqlCommand.Parameters.AddWithValue("@wGrad", wBMSEmployee.Grad);
                    wSqlCommand.Parameters.AddWithValue("@wManager", wBMSEmployee.Manager);
                    wSqlCommand.Parameters.AddWithValue("@wPhone", wBMSEmployee.Phone);
                    wSqlCommand.Parameters.AddWithValue("@wEmail", wBMSEmployee.Email);
                    wSqlCommand.Parameters.AddWithValue("@wWeiXin", wBMSEmployee.WeiXin);
                    wSqlCommand.Parameters.AddWithValue("@wPosition", wBMSEmployee.Position);
                    wSqlCommand.Parameters.AddWithValue("@wPhoneMAC", wBMSEmployee.PhoneMAC);
                    wSqlCommand.Parameters.AddWithValue("@wOnline", wBMSEmployee.Online);
                    wSqlCommand.Parameters.AddWithValue("@wOnLineTime", wBMSEmployee.OnLineTime);
                    wSqlCommand.Parameters.AddWithValue("@wDepartureDate", wBMSEmployee.DepartureDate);
                    wSqlCommand.Parameters.AddWithValue("@wLastOnLineTime", wBMSEmployee.LastOnLineTime);
                    wSqlCommand.Parameters.AddWithValue("@wDutyID", wBMSEmployee.DutyID);
                    wSqlCommand.Parameters.AddWithValue("@wLoginID", wBMSEmployee.LoginID);
                    wSqlCommand.Parameters.AddWithValue("@wType", wBMSEmployee.Type);
                    wSqlCommand.Parameters.AddWithValue("@wSuperiorID", wBMSEmployee.SuperiorID);
                    wSqlCommand.Parameters.AddWithValue("@wPartPower", wBMSEmployee.PartPower);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wBMSEmployee.ID;
                }
            }
            catch (Exception ex)
            {
                Server.MySQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wErrorCode = (int)9;
            }
            finally
            {
                Server.MySQLPool.FreeConnection(wCon);
            }
            return wResult;
        }

        public int BMS_DeleteBMSEmployeeList(List<BMSEmployee> wBMSEmployeeList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = Server.MySQLPool.GetConnection();
            try
            {
                if (wBMSEmployeeList != null && wBMSEmployeeList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wBMSEmployeeList.Count; i++)
                    {
                        if (i == wBMSEmployeeList.Count - 1)
                            wStringBuilder.Append(wBMSEmployeeList[i].ID);
                        else
                            wStringBuilder.Append(wBMSEmployeeList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From mbs_user WHERE ID in({0});", wStringBuilder.ToString());
                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Server.MySQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wErrorCode = (int)9;
            }
            finally
            {
                Server.MySQLPool.FreeConnection(wCon);
            }
            return wErrorCode;
        }

        public List<BMSEmployee> BMS_QueryBMSEmployeeList(int wID, string wName, string wPassword, string wLoginID, int wActive, out int wErrorCode)
        {
            List<BMSEmployee> wResultList = new List<BMSEmployee>();
            wErrorCode = 0;
            MySqlConnection wCon = Server.MySQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM mbs_user WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wName is null or @wName = '' or Name= @wName)"
                    + " and(@wPassword is null or @wPassword = '' or Password= @wPassword)"
                    + " and(@wActive <0 or Active= @wActive)"
                    + " and(@wLoginID is null or @wLoginID = '' or LoginID= @wLoginID)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wName", wName);
                wSqlCommand.Parameters.AddWithValue("@wPassword", wPassword);
                wSqlCommand.Parameters.AddWithValue("@wLoginID", wLoginID);
                wSqlCommand.Parameters.AddWithValue("@wActive", wActive);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                List<MBSRole> wRoleList = MBSRoleDAO.Instance.MBS_QueryMBSRoleList(-1, -1, "", out wErrorCode);
                List<FPCPart> wPartList = FPCPartDAO.Instance.GetPartList();
                while (wSqlDataReader.Read())
                {
                    BMSEmployee wBMSEmployee = new BMSEmployee();
                    wBMSEmployee.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wBMSEmployee.Name = StringUtils.ParseString(wSqlDataReader["Name"]);
                    wBMSEmployee.LoginName = StringUtils.ParseString(wSqlDataReader["LoginName"]);
                    wBMSEmployee.Password = StringUtils.ParseString(wSqlDataReader["Password"]);
                    wBMSEmployee.CreateDate = StringUtils.ParseDate(wSqlDataReader["CreateDate"]);
                    wBMSEmployee.Operator = StringUtils.ParseString(wSqlDataReader["Operator"]);
                    wBMSEmployee.PartPower = StringUtils.ParseString(wSqlDataReader["PartPower"]);
                    wBMSEmployee.PartPowerName = GetPartPowerName(wPartList, wBMSEmployee.PartPower);
                    wBMSEmployee.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wBMSEmployee.DepartmentID = StringUtils.ParseInt(wSqlDataReader["DepartmentID"]);
                    wBMSEmployee.Grad = StringUtils.ParseInt(wSqlDataReader["Grad"]);
                    wBMSEmployee.Manager = StringUtils.ParseInt(wSqlDataReader["Manager"]);
                    wBMSEmployee.Phone = StringUtils.ParseString(wSqlDataReader["Phone"]);
                    wBMSEmployee.Email = StringUtils.ParseString(wSqlDataReader["Email"]);
                    wBMSEmployee.WeiXin = StringUtils.ParseString(wSqlDataReader["WeiXin"]);
                    wBMSEmployee.Position = StringUtils.ParseInt(wSqlDataReader["Position"]);
                    wBMSEmployee.PhoneMAC = StringUtils.ParseLong(wSqlDataReader["PhoneMAC"]);
                    wBMSEmployee.Online = StringUtils.ParseInt(wSqlDataReader["Online"]);
                    wBMSEmployee.OnLineTime = StringUtils.ParseDate(wSqlDataReader["OnLineTime"]);
                    wBMSEmployee.DepartureDate = StringUtils.ParseDate(wSqlDataReader["DepartureDate"]);
                    wBMSEmployee.LastOnLineTime = StringUtils.ParseDate(wSqlDataReader["LastOnLineTime"]);
                    wBMSEmployee.DutyID = StringUtils.ParseInt(wSqlDataReader["DutyID"]);
                    wBMSEmployee.LoginID = StringUtils.ParseString(wSqlDataReader["LoginID"]);
                    wBMSEmployee.Type = StringUtils.ParseInt(wSqlDataReader["Type"]);
                    wBMSEmployee.SuperiorID = StringUtils.ParseInt(wSqlDataReader["SuperiorID"]);

                    if (wRoleList.Exists(p => p.ID == wBMSEmployee.Grad))
                        wBMSEmployee.GradName = wRoleList.Find(p => p.ID == wBMSEmployee.Grad).Name;

                    wBMSEmployee.ActiveText = wBMSEmployee.Active == 1 ? "激活" : "关闭";

                    wResultList.Add(wBMSEmployee);
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                Server.MySQLPool.CloseConnection(wCon);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wErrorCode = (int)9;
            }
            finally
            {
                Server.MySQLPool.FreeConnection(wCon);
            }
            return wResultList;
        }

        private string GetPartPowerName(List<FPCPart> wPartList, string wPartPower)
        {
            string wResult = "";
            try
            {
                if (string.IsNullOrWhiteSpace(wPartPower))
                    return wResult;

                string[] wPartsList = wPartPower.Split(',');
                List<string> wNameList = new List<string>();
                foreach (string wItem in wPartsList)
                {
                    int wID = int.Parse(wItem);
                    if (wPartList.Exists(p => p.PartID == wID))
                    {
                        string wName = wPartList.Find(p => p.PartID == wID).PartName;
                        wNameList.Add(wName);
                    }
                }
                wResult = string.Join(",", wNameList);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public BMSEmployee BMS_Login(string wLoginID, string wPassword)
        {
            BMSEmployee wResult = new BMSEmployee();
            try
            {
                string wPass = ShrisDES.EncryptDESString(wPassword, "shrismcis");
                int wErrorCode = 0;
                List<BMSEmployee> wUserList = BMS_QueryBMSEmployeeList(-1, "", wPass, wLoginID, 1, out wErrorCode);
                if (wUserList.Count > 0)
                    wResult = wUserList[0];
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
    }
}

