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
    public class SFCUploadSOPDAO
    {
        #region 单实例
        private SFCUploadSOPDAO() { }
        private static SFCUploadSOPDAO _Instance;

        public static SFCUploadSOPDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SFCUploadSOPDAO();
                return SFCUploadSOPDAO._Instance;
            }
        }
        #endregion

        public int SFC_SaveSFCUploadSOP(SFCUploadSOP wSFCUploadSOP, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wSFCUploadSOP.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO sfc_uploadsop(ProductID,PartID,FileName,FilePath,Type,UploadTime,OperatorID,ValidTime,Active) VALUES(@wProductID,@wPartID,@wFileName,@wFilePath,@wType,@wUploadTime,@wOperatorID,@wValidTime,@wActive);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wSFCUploadSOP.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wSFCUploadSOP.PartID);
                    wSqlCommand.Parameters.AddWithValue("@wFileName", wSFCUploadSOP.FileName);
                    wSqlCommand.Parameters.AddWithValue("@wFilePath", wSFCUploadSOP.FilePath);
                    wSqlCommand.Parameters.AddWithValue("@wType", wSFCUploadSOP.Type);
                    wSqlCommand.Parameters.AddWithValue("@wUploadTime", wSFCUploadSOP.UploadTime);
                    wSqlCommand.Parameters.AddWithValue("@wOperatorID", wSFCUploadSOP.OperatorID);
                    wSqlCommand.Parameters.AddWithValue("@wValidTime", wSFCUploadSOP.ValidTime);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wSFCUploadSOP.Active);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wSFCUploadSOP.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE sfc_uploadsop SET ProductID=@wProductID,PartID=@wPartID,FileName=@wFileName,FilePath=@wFilePath,Type=@wType,UploadTime=@wUploadTime,OperatorID=@wOperatorID,ValidTime=@wValidTime,Active=@wActive WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wSFCUploadSOP.ID);
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wSFCUploadSOP.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wSFCUploadSOP.PartID);
                    wSqlCommand.Parameters.AddWithValue("@wFileName", wSFCUploadSOP.FileName);
                    wSqlCommand.Parameters.AddWithValue("@wFilePath", wSFCUploadSOP.FilePath);
                    wSqlCommand.Parameters.AddWithValue("@wType", wSFCUploadSOP.Type);
                    wSqlCommand.Parameters.AddWithValue("@wUploadTime", wSFCUploadSOP.UploadTime);
                    wSqlCommand.Parameters.AddWithValue("@wOperatorID", wSFCUploadSOP.OperatorID);
                    wSqlCommand.Parameters.AddWithValue("@wValidTime", wSFCUploadSOP.ValidTime);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wSFCUploadSOP.Active);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wSFCUploadSOP.ID;
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

        public int SFC_DeleteSFCUploadSOPList(List<SFCUploadSOP> wSFCUploadSOPList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wSFCUploadSOPList != null && wSFCUploadSOPList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wSFCUploadSOPList.Count; i++)
                    {
                        if (i == wSFCUploadSOPList.Count - 1)
                            wStringBuilder.Append(wSFCUploadSOPList[i].ID);
                        else
                            wStringBuilder.Append(wSFCUploadSOPList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From sfc_uploadsop WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<SFCUploadSOP> SFC_QuerySFCUploadSOPList(int wID, int wProductID, int wPartID, int wType, int wActive, out int wErrorCode)
        {
            List<SFCUploadSOP> wResultList = new List<SFCUploadSOP>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM sfc_uploadsop WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wProductID <=0 or ProductID= @wProductID)"
                    + " and(@wPartID <=0 or PartID= @wPartID)"
                    + " and(@wType <=0 or Type= @wType)"
                    + " and(@wActive <=0 or Active= @wActive)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);
                wSqlCommand.Parameters.AddWithValue("@wPartID", wPartID);
                wSqlCommand.Parameters.AddWithValue("@wType", wType);
                wSqlCommand.Parameters.AddWithValue("@wActive", wActive);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                List<FPCProduct> wFPCProductList = FPCProductDAO.Instance.GetProductList();
                List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();
                List<BMSEmployee> wUserList = BMSEmployeeDAO.Instance.BMS_QueryBMSEmployeeList(-1, "", "", "", -1, out wErrorCode);
                while (wSqlDataReader.Read())
                {
                    SFCUploadSOP wSFCUploadSOP = new SFCUploadSOP();
                    wSFCUploadSOP.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCUploadSOP.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wSFCUploadSOP.PartID = StringUtils.ParseInt(wSqlDataReader["PartID"]);
                    wSFCUploadSOP.FileName = StringUtils.ParseString(wSqlDataReader["FileName"]);
                    wSFCUploadSOP.FilePath = StringUtils.ParseString(wSqlDataReader["FilePath"]);
                    wSFCUploadSOP.Type = StringUtils.ParseInt(wSqlDataReader["Type"]);
                    wSFCUploadSOP.UploadTime = StringUtils.ParseDate(wSqlDataReader["UploadTime"]);
                    wSFCUploadSOP.OperatorID = StringUtils.ParseInt(wSqlDataReader["OperatorID"]);
                    wSFCUploadSOP.ValidTime = StringUtils.ParseDate(wSqlDataReader["ValidTime"]);
                    wSFCUploadSOP.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);

                    wSFCUploadSOP.ProductName = wFPCProductList.Exists(p => p.ProductID == wSFCUploadSOP.ProductID) ? wFPCProductList.Find(p => p.ProductID == wSFCUploadSOP.ProductID).ProductName : "";
                    wSFCUploadSOP.PartName = wFPCPartList.Exists(p => p.PartID == wSFCUploadSOP.PartID) ? wFPCPartList.Find(p => p.PartID == wSFCUploadSOP.PartID).PartName : "";
                    wSFCUploadSOP.UploadTimeText = wSFCUploadSOP.UploadTime.ToString("yyyy/MM/dd HH:mm:ss");
                    wSFCUploadSOP.TypeText = wSFCUploadSOP.Type == 1 ? "PDF" : "图片";
                    wSFCUploadSOP.Operator = wUserList.Exists(p => p.ID == wSFCUploadSOP.OperatorID) ? wUserList.Find(p => p.ID == wSFCUploadSOP.OperatorID).Name : "";
                    if (wSFCUploadSOP.Active == 1)
                        wSFCUploadSOP.ValidTimeText = wSFCUploadSOP.ValidTime.ToString("yyyy/MM/dd HH:mm:ss");
                    else
                        wSFCUploadSOP.ValidTimeText = "";
                    wSFCUploadSOP.ActiveText = wSFCUploadSOP.Active == 1 ? "激活" : "关闭";

                    wResultList.Add(wSFCUploadSOP);
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

