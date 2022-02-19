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
    public class IPTStandardDAO
    {
        #region 单实例
        private IPTStandardDAO() { }
        private static IPTStandardDAO _Instance;

        public static IPTStandardDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IPTStandardDAO();
                return IPTStandardDAO._Instance;
            }
        }
        #endregion

        public int IPT_SaveIPTStandard(IPTStandard wIPTStandard, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTStandard.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO ipt_standard(ProductID,PartPointID,ItemID,PartID,Type,UpperLimit,LowerLimit,UnitText,DefaultValue,TextDescription,EditorID,EditTime,Active,OrderID) VALUES(@wProductID,@wPartPointID,@wItemID,@wPartID,@wType,@wUpperLimit,@wLowerLimit,@wUnitText,@wDefaultValue,@wTextDescription,@wEditorID,@wEditTime,@wActive,@wOrderID);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wIPTStandard.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wPartPointID", wIPTStandard.PartPointID);
                    wSqlCommand.Parameters.AddWithValue("@wItemID", wIPTStandard.ItemID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTStandard.PartID);
                    wSqlCommand.Parameters.AddWithValue("@wType", wIPTStandard.Type);
                    wSqlCommand.Parameters.AddWithValue("@wUpperLimit", wIPTStandard.UpperLimit);
                    wSqlCommand.Parameters.AddWithValue("@wLowerLimit", wIPTStandard.LowerLimit);
                    wSqlCommand.Parameters.AddWithValue("@wUnitText", wIPTStandard.UnitText);
                    wSqlCommand.Parameters.AddWithValue("@wDefaultValue", wIPTStandard.DefaultValue);
                    wSqlCommand.Parameters.AddWithValue("@wTextDescription", wIPTStandard.TextDescription);
                    wSqlCommand.Parameters.AddWithValue("@wEditorID", wIPTStandard.EditorID);
                    wSqlCommand.Parameters.AddWithValue("@wEditTime", wIPTStandard.EditTime);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wIPTStandard.Active);
                    wSqlCommand.Parameters.AddWithValue("@wOrderID", wIPTStandard.OrderID);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wIPTStandard.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE ipt_standard SET ProductID=@wProductID,PartPointID=@wPartPointID,ItemID=@wItemID,PartID=@wPartID,Type=@wType,UpperLimit=@wUpperLimit,LowerLimit=@wLowerLimit,UnitText=@wUnitText,DefaultValue=@wDefaultValue,TextDescription=@wTextDescription,EditorID=@wEditorID,EditTime=@wEditTime,Active=@wActive,OrderID=@wOrderID WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wIPTStandard.ID);
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wIPTStandard.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wPartPointID", wIPTStandard.PartPointID);
                    wSqlCommand.Parameters.AddWithValue("@wItemID", wIPTStandard.ItemID);
                    wSqlCommand.Parameters.AddWithValue("@wPartID", wIPTStandard.PartID);
                    wSqlCommand.Parameters.AddWithValue("@wType", wIPTStandard.Type);
                    wSqlCommand.Parameters.AddWithValue("@wUpperLimit", wIPTStandard.UpperLimit);
                    wSqlCommand.Parameters.AddWithValue("@wLowerLimit", wIPTStandard.LowerLimit);
                    wSqlCommand.Parameters.AddWithValue("@wUnitText", wIPTStandard.UnitText);
                    wSqlCommand.Parameters.AddWithValue("@wDefaultValue", wIPTStandard.DefaultValue);
                    wSqlCommand.Parameters.AddWithValue("@wTextDescription", wIPTStandard.TextDescription);
                    wSqlCommand.Parameters.AddWithValue("@wEditorID", wIPTStandard.EditorID);
                    wSqlCommand.Parameters.AddWithValue("@wEditTime", wIPTStandard.EditTime);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wIPTStandard.Active);
                    wSqlCommand.Parameters.AddWithValue("@wOrderID", wIPTStandard.OrderID);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wIPTStandard.ID;
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

        public int IPT_DeleteIPTStandardList(List<IPTStandard> wIPTStandardList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wIPTStandardList != null && wIPTStandardList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wIPTStandardList.Count; i++)
                    {
                        if (i == wIPTStandardList.Count - 1)
                            wStringBuilder.Append(wIPTStandardList[i].ID);
                        else
                            wStringBuilder.Append(wIPTStandardList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From ipt_standard WHERE ID in({0});", wStringBuilder.ToString());
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

        public List<IPTStandard> IPT_QueryIPTStandardList(int wID, int wProductID, int wPartPointID, int wItemID, int wPartID, int wType, int wActive, out int wErrorCode)
        {
            List<IPTStandard> wResultList = new List<IPTStandard>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM ipt_standard WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wProductID <=0 or ProductID= @wProductID)"
                    + " and(@wPartPointID <=0 or PartPointID= @wPartPointID)"
                    + " and(@wItemID <=0 or ItemID= @wItemID)"
                    + " and(@wPartID <=0 or PartID= @wPartID)"
                    + " and(@wType <=0 or Type= @wType)"
                    + " and(@wActive <=0 or Active= @wActive)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);
                wSqlCommand.Parameters.AddWithValue("@wPartPointID", wPartPointID);
                wSqlCommand.Parameters.AddWithValue("@wItemID", wItemID);
                wSqlCommand.Parameters.AddWithValue("@wPartID", wPartID);
                wSqlCommand.Parameters.AddWithValue("@wType", wType);
                wSqlCommand.Parameters.AddWithValue("@wActive", wActive);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();

                List<FPCProduct> wFPCProductList = FPCProductDAO.Instance.GetProductList();
                List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();
                List<IPTItem> wIPTItemList = IPTItemDAO.Instance.IPT_QueryIPTItemList(-1, "", "", out wErrorCode);
                List<BMSEmployee> wUserList = BMSEmployeeDAO.Instance.BMS_QueryBMSEmployeeList(-1, "", "", "", -1, out wErrorCode);
                while (wSqlDataReader.Read())
                {
                    IPTStandard wIPTStandard = new IPTStandard();
                    wIPTStandard.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wIPTStandard.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wIPTStandard.PartPointID = StringUtils.ParseInt(wSqlDataReader["PartPointID"]);
                    wIPTStandard.ItemID = StringUtils.ParseInt(wSqlDataReader["ItemID"]);
                    wIPTStandard.PartID = StringUtils.ParseInt(wSqlDataReader["PartID"]);
                    wIPTStandard.Type = StringUtils.ParseInt(wSqlDataReader["Type"]);
                    wIPTStandard.UpperLimit = StringUtils.ParseDouble(wSqlDataReader["UpperLimit"]);
                    wIPTStandard.LowerLimit = StringUtils.ParseDouble(wSqlDataReader["LowerLimit"]);
                    wIPTStandard.UnitText = StringUtils.ParseString(wSqlDataReader["UnitText"]);
                    wIPTStandard.DefaultValue = StringUtils.ParseString(wSqlDataReader["DefaultValue"]);
                    wIPTStandard.TextDescription = StringUtils.ParseString(wSqlDataReader["TextDescription"]);
                    wIPTStandard.EditorID = StringUtils.ParseInt(wSqlDataReader["EditorID"]);
                    wIPTStandard.EditTime = StringUtils.ParseDate(wSqlDataReader["EditTime"]);
                    wIPTStandard.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wIPTStandard.OrderID = StringUtils.ParseInt(wSqlDataReader["OrderID"]);

                    if (wIPTStandard.ProductID > 0)
                        wIPTStandard.ProductName = wFPCProductList.Find(p => p.ProductID == wIPTStandard.ProductID).ProductName;
                    if (wIPTStandard.ItemID > 0)
                    {
                        wIPTStandard.ItemCode = wIPTItemList.Find(p => p.ID == wIPTStandard.ItemID).Code;
                        wIPTStandard.ItemName = wIPTItemList.Find(p => p.ID == wIPTStandard.ItemID).Name;
                    }
                    if (wIPTStandard.PartID > 0)
                        wIPTStandard.PartName = wFPCPartList.Find(p => p.PartID == wIPTStandard.PartID).PartName;
                    wIPTStandard.TypeName = ((StandardType)wIPTStandard.Type).ToString();
                    if (wIPTStandard.EditorID > 0)
                        wIPTStandard.Editor = wUserList.Find(p => p.ID == wIPTStandard.EditorID).Name;
                    wIPTStandard.EditTimeText = wIPTStandard.EditTime.ToString("yyyy/MM/dd");
                    wIPTStandard.ActiveText = wIPTStandard.Active == 1 ? "激活" : "关闭";
                    if (wIPTStandard.LowerLimit > 0)
                        wIPTStandard.LowerLimitText = wIPTStandard.LowerLimit.ToString();
                    else
                        wIPTStandard.LowerLimitText = "";

                    if (wIPTStandard.UpperLimit > 0)
                        wIPTStandard.UpperLimitText = wIPTStandard.UpperLimit.ToString();
                    else
                        wIPTStandard.UpperLimitText = "";

                    wResultList.Add(wIPTStandard);
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

        /// <summary>
        /// 获取检验标准列表
        /// </summary>
        internal List<ExamineDataItem> IPT_QueryIPTStandardList(List<int> wPartIDList, int wProductID, bool wIsHistory)
        {
            List<ExamineDataItem> wResult = new List<ExamineDataItem>();
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                List<int> wCurrentPartIDList = new List<int>();
                foreach (string wItem in wConfig.CurrentPart.Split(','))
                    wCurrentPartIDList.Add(int.Parse(wItem));

                int wErrorCode = 0;
                List<IPTStandard> wList = new List<IPTStandard>();

                //当前工位
                List<IPTStandard> wCurrentList = new List<IPTStandard>();
                foreach (int wPartID in wCurrentPartIDList)
                    wCurrentList.AddRange(IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wProductID, -1, -1, wPartID, -1, 1, out wErrorCode));
                wCurrentList = wCurrentList.OrderBy(p => p.PartID).ThenBy(p => p.OrderID).ToList();
                //历史工位
                List<int> wHisIDList = wPartIDList.FindAll(p => !wCurrentPartIDList.Exists(q => q == p)).ToList();
                List<IPTStandard> wHisList = new List<IPTStandard>();
                foreach (int wPartID in wHisIDList)
                    wHisList.AddRange(IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wProductID, -1, -1, wPartID, -1, 1, out wErrorCode));
                wHisList = wHisList.OrderBy(p => p.PartID).ThenBy(p => p.OrderID).ToList();

                wList.AddRange(wCurrentList);
                wList.AddRange(wHisList);

                GUD.mIPTStandardList = wList;
                GUD.mPartIDList = wPartIDList;

                //包装工位去掉三串测试结果
                if (wCurrentPartIDList.Exists(p => p == 11 || p == 12))
                    wList.RemoveAll(p => p.PartID == 10);

                foreach (IPTStandard wIPTStandard in wList)
                {
                    if (!wCurrentPartIDList.Exists(p => p == 11 || p == 12) && !wCurrentPartIDList.Exists(p => p == wIPTStandard.PartID))
                        continue;

                    ExamineDataItem wExamineDataItem = new ExamineDataItem();

                    wExamineDataItem.ItemGroup = "";

                    wExamineDataItem.StandardID = 0;
                    wExamineDataItem.ItemID = (int)wIPTStandard.ID;
                    wExamineDataItem.ItemName = wIPTStandard.ItemName;
                    if (!string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                        wExamineDataItem.ItemName += "(" + wIPTStandard.UnitText + ")";
                    wExamineDataItem.OrderID = wIPTStandard.OrderID;
                    wExamineDataItem.TypeEnum = (ItemTypeEnum)wIPTStandard.Type;
                    wExamineDataItem.ItemDescription = "OK;NG";
                    wExamineDataItem.Active = 1;
                    wExamineDataItem.ActiveText = "激活";
                    wExamineDataItem.IsRequired = false;
                    wExamineDataItem.IsLinkControl = false;
                    wExamineDataItem.LinkRows = 0;
                    wExamineDataItem.RelationParameter = "";
                    wExamineDataItem.RelationRatio = "";
                    wExamineDataItem.IsEdit = true;
                    if (wCurrentPartIDList.All(p => p > wIPTStandard.PartID))
                        wExamineDataItem.IsEdit = false;
                    if (wIsHistory)
                        wExamineDataItem.IsEdit = false;
                    wExamineDataItem.IsPrimaryKey = false;
                    wExamineDataItem.IsRepeat = true;
                    wExamineDataItem.ValueLength = 0;
                    wExamineDataItem.DefalutValueType = 0;
                    wExamineDataItem.DefaultValueForm = wIPTStandard.DefaultValue;

                    wResult.Add(wExamineDataItem);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
    }
}

