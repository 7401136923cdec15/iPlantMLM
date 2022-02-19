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
    public class SFCModuleRecordDAO
    {
        #region 单实例
        private SFCModuleRecordDAO() { }
        private static SFCModuleRecordDAO _Instance;

        public static SFCModuleRecordDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SFCModuleRecordDAO();
                return SFCModuleRecordDAO._Instance;
            }
        }
        #endregion

        public int SFC_SaveSFCModuleRecord(SFCModuleRecord wSFCModuleRecord, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wSFCModuleRecord.TrustBarCode == null)
                    wSFCModuleRecord.TrustBarCode = "";
                if (wSFCModuleRecord.ID == 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "INSERT INTO sfc_modulerecord(SerialNumber,CapacitorPackageNo,OnlineTime,ModuleNumber,OfflineTime,CurrentPartID,Times,BarCode,Gear,ShiftID,CreateID,CreateTime,ProductID,TrustBarCode,Active,IsQuality) VALUES(@wSerialNumber,@wCapacitorPackageNo,@wOnlineTime,@wModuleNumber,@wOfflineTime,@wCurrentPartID,@wTimes,@wBarCode,@wGear,@wShiftID,@wCreateID,@wCreateTime,@wProductID,@wTrustBarCode,@wActive,@wIsQuality);SELECT LAST_INSERT_ID() as ID;";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSFCModuleRecord.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wCapacitorPackageNo", wSFCModuleRecord.CapacitorPackageNo);
                    wSqlCommand.Parameters.AddWithValue("@wOnlineTime", wSFCModuleRecord.OnlineTime);
                    wSqlCommand.Parameters.AddWithValue("@wModuleNumber", wSFCModuleRecord.ModuleNumber);
                    wSqlCommand.Parameters.AddWithValue("@wOfflineTime", wSFCModuleRecord.OfflineTime);
                    wSqlCommand.Parameters.AddWithValue("@wCurrentPartID", wSFCModuleRecord.CurrentPartID);
                    wSqlCommand.Parameters.AddWithValue("@wTimes", wSFCModuleRecord.Times);
                    wSqlCommand.Parameters.AddWithValue("@wBarCode", wSFCModuleRecord.BarCode);
                    wSqlCommand.Parameters.AddWithValue("@wGear", wSFCModuleRecord.Gear);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wSFCModuleRecord.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wSFCModuleRecord.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wSFCModuleRecord.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wSFCModuleRecord.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wTrustBarCode", wSFCModuleRecord.TrustBarCode);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wSFCModuleRecord.Active);
                    wSqlCommand.Parameters.AddWithValue("@wIsQuality", wSFCModuleRecord.IsQuality);

                    wSqlCommand.ExecuteNonQuery();
                    wResult = (int)wSqlCommand.LastInsertedId;
                }
                else if (wSFCModuleRecord.ID > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;
                    wSqlCommand.CommandText = "UPDATE sfc_modulerecord SET SerialNumber=@wSerialNumber,CapacitorPackageNo=@wCapacitorPackageNo,OnlineTime=@wOnlineTime,ModuleNumber=@wModuleNumber,OfflineTime=@wOfflineTime,CurrentPartID=@wCurrentPartID,Times=@wTimes,BarCode=@wBarCode,Gear=@wGear,ShiftID=@wShiftID,CreateID=@wCreateID,CreateTime=@wCreateTime,ProductID=@wProductID,TrustBarCode=@wTrustBarCode,Active=@wActive,IsQuality=@wIsQuality WHERE ID=@wID";
                    wSqlCommand.Parameters.Clear();
                    wSqlCommand.Parameters.AddWithValue("@wID", wSFCModuleRecord.ID);
                    wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSFCModuleRecord.SerialNumber);
                    wSqlCommand.Parameters.AddWithValue("@wCapacitorPackageNo", wSFCModuleRecord.CapacitorPackageNo);
                    wSqlCommand.Parameters.AddWithValue("@wOnlineTime", wSFCModuleRecord.OnlineTime);
                    wSqlCommand.Parameters.AddWithValue("@wModuleNumber", wSFCModuleRecord.ModuleNumber);
                    wSqlCommand.Parameters.AddWithValue("@wOfflineTime", wSFCModuleRecord.OfflineTime);
                    wSqlCommand.Parameters.AddWithValue("@wCurrentPartID", wSFCModuleRecord.CurrentPartID);
                    wSqlCommand.Parameters.AddWithValue("@wTimes", wSFCModuleRecord.Times);
                    wSqlCommand.Parameters.AddWithValue("@wBarCode", wSFCModuleRecord.BarCode);
                    wSqlCommand.Parameters.AddWithValue("@wGear", wSFCModuleRecord.Gear);
                    wSqlCommand.Parameters.AddWithValue("@wShiftID", wSFCModuleRecord.ShiftID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateID", wSFCModuleRecord.CreateID);
                    wSqlCommand.Parameters.AddWithValue("@wCreateTime", wSFCModuleRecord.CreateTime);
                    wSqlCommand.Parameters.AddWithValue("@wProductID", wSFCModuleRecord.ProductID);
                    wSqlCommand.Parameters.AddWithValue("@wTrustBarCode", wSFCModuleRecord.TrustBarCode);
                    wSqlCommand.Parameters.AddWithValue("@wActive", wSFCModuleRecord.Active);
                    wSqlCommand.Parameters.AddWithValue("@wIsQuality", wSFCModuleRecord.IsQuality);

                    int wSQL_Result = wSqlCommand.ExecuteNonQuery();
                    wResult = wSFCModuleRecord.ID;
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

        public int SFC_DeleteSFCModuleRecordList(List<SFCModuleRecord> wSFCModuleRecordList)
        {
            int wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wSFCModuleRecordList != null && wSFCModuleRecordList.Count > 0)
                {
                    MySqlCommand wSqlCommand = new MySqlCommand();
                    wSqlCommand.Connection = wCon;

                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wSFCModuleRecordList.Count; i++)
                    {
                        if (i == wSFCModuleRecordList.Count - 1)
                            wStringBuilder.Append(wSFCModuleRecordList[i].ID);
                        else
                            wStringBuilder.Append(wSFCModuleRecordList[i].ID + ",");
                    }
                    wSqlCommand.CommandText = string.Format("DELETE From sfc_modulerecord WHERE ID in({0});", wStringBuilder.ToString());
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

        public string SFC_CreateSerialNumber(out int wErrorCode)
        {
            string wResult = "";
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT count(*) as Number FROM iplantmlm.sfc_modulerecord where ShiftID=@ShiftID;";

                wSqlCommand.Parameters.AddWithValue("@ShiftID", wShiftID);

                int wNumber = Convert.ToInt32(wSqlCommand.ExecuteScalar());

                wResult = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMdd"), (wNumber + 1).ToString("0000"));
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

        public List<SFCModuleRecord> SFC_QuerySFCModuleRecordList(int wID, string wSerialNumber, string wCapacitorPackageNo, string wModuleNumber, int wCurrentPartID, string wBarCode, int wShiftID, out int wErrorCode)
        {
            List<SFCModuleRecord> wResultList = new List<SFCModuleRecord>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM sfc_modulerecord WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wSerialNumber is null or @wSerialNumber = '' or SerialNumber= @wSerialNumber)"
                    + " and(@wCapacitorPackageNo is null or @wCapacitorPackageNo = '' or CapacitorPackageNo= @wCapacitorPackageNo)"
                    + " and(@wModuleNumber is null or @wModuleNumber = '' or ModuleNumber= @wModuleNumber)"
                    + " and(@wCurrentPartID <=0 or CurrentPartID= @wCurrentPartID)"
                    + " and(@wBarCode is null or @wBarCode = '' or BarCode= @wBarCode)"
                    + " and(@wShiftID <=0 or ShiftID= @wShiftID)";

                wSqlCommand.Parameters.AddWithValue("@wID", wID);
                wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@wCapacitorPackageNo", wCapacitorPackageNo);
                wSqlCommand.Parameters.AddWithValue("@wModuleNumber", wModuleNumber);
                wSqlCommand.Parameters.AddWithValue("@wCurrentPartID", wCurrentPartID);
                wSqlCommand.Parameters.AddWithValue("@wBarCode", wBarCode);
                wSqlCommand.Parameters.AddWithValue("@wShiftID", wShiftID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();

                    wSFCModuleRecord.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCModuleRecord.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wSFCModuleRecord.CapacitorPackageNo = StringUtils.ParseString(wSqlDataReader["CapacitorPackageNo"]);
                    wSFCModuleRecord.OnlineTime = StringUtils.ParseDate(wSqlDataReader["OnlineTime"]);
                    wSFCModuleRecord.ModuleNumber = StringUtils.ParseString(wSqlDataReader["ModuleNumber"]);
                    wSFCModuleRecord.OfflineTime = StringUtils.ParseDate(wSqlDataReader["OfflineTime"]);
                    wSFCModuleRecord.CurrentPartID = StringUtils.ParseInt(wSqlDataReader["CurrentPartID"]);
                    wSFCModuleRecord.Times = StringUtils.ParseInt(wSqlDataReader["Times"]);
                    wSFCModuleRecord.BarCode = StringUtils.ParseString(wSqlDataReader["BarCode"]);
                    wSFCModuleRecord.Gear = StringUtils.ParseString(wSqlDataReader["Gear"]);
                    wSFCModuleRecord.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wSFCModuleRecord.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wSFCModuleRecord.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wSFCModuleRecord.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wSFCModuleRecord.TrustBarCode = StringUtils.ParseString(wSqlDataReader["TrustBarCode"]);
                    wSFCModuleRecord.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wSFCModuleRecord.ActiveText = wSFCModuleRecord.Active == 1 ? "激活" : "冻结";
                    wSFCModuleRecord.IsQuality = StringUtils.ParseInt(wSqlDataReader["IsQuality"]);
                    wSFCModuleRecord.IsQualityText = wSFCModuleRecord.IsQuality == 1 ? "合格" : "不合格";

                    wResultList.Add(wSFCModuleRecord);
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

        public List<SFCModuleRecord> SFC_QuerySFCModuleRecordList(int wProductID, DateTime wSTime, DateTime wEndTime, string wCode, List<int> wPartIDList, out int wErrorCode)
        {
            List<SFCModuleRecord> wResultList = new List<SFCModuleRecord>();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                if (wPartIDList == null || wPartIDList.Count <= 0)
                    return wResultList;

                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = string.Format("select * from iplantmlm.sfc_modulerecord where (@wCode='' or CapacitorPackageNo=@wCode or ModuleNumber=@wCode) and ProductID=@wProductID and (SerialNumber in (SELECT distinct SerialNumber FROM iplantmlm.ipt_boolvalue where CreateTime > @wSTime and CreateTime < @wEndTime and PartID in ({0})) or SerialNumber in (SELECT distinct SerialNumber FROM iplantmlm.ipt_numbervalue where CreateTime > @wSTime and CreateTime < @wEndTime and PartID in ({0})) or SerialNumber in (SELECT distinct SerialNumber FROM iplantmlm.ipt_textvalue where CreateTime > @wSTime and CreateTime < @wEndTime and PartID in ({0})));", string.Join(",", wPartIDList));

                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);
                wSqlCommand.Parameters.AddWithValue("@wSTime", wSTime);
                wSqlCommand.Parameters.AddWithValue("@wEndTime", wEndTime);
                wSqlCommand.Parameters.AddWithValue("@wProductID", wProductID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();
                    wSFCModuleRecord.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCModuleRecord.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wSFCModuleRecord.CapacitorPackageNo = StringUtils.ParseString(wSqlDataReader["CapacitorPackageNo"]);
                    wSFCModuleRecord.OnlineTime = StringUtils.ParseDate(wSqlDataReader["OnlineTime"]);
                    wSFCModuleRecord.ModuleNumber = StringUtils.ParseString(wSqlDataReader["ModuleNumber"]);
                    wSFCModuleRecord.OfflineTime = StringUtils.ParseDate(wSqlDataReader["OfflineTime"]);
                    wSFCModuleRecord.CurrentPartID = StringUtils.ParseInt(wSqlDataReader["CurrentPartID"]);
                    wSFCModuleRecord.Times = StringUtils.ParseInt(wSqlDataReader["Times"]);
                    wSFCModuleRecord.BarCode = StringUtils.ParseString(wSqlDataReader["BarCode"]);
                    wSFCModuleRecord.Gear = StringUtils.ParseString(wSqlDataReader["Gear"]);
                    wSFCModuleRecord.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wSFCModuleRecord.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wSFCModuleRecord.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wSFCModuleRecord.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wSFCModuleRecord.TrustBarCode = StringUtils.ParseString(wSqlDataReader["TrustBarCode"]);
                    wSFCModuleRecord.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wSFCModuleRecord.ActiveText = wSFCModuleRecord.Active == 1 ? "激活" : "冻结";
                    wSFCModuleRecord.IsQuality = StringUtils.ParseInt(wSqlDataReader["IsQuality"]);
                    wSFCModuleRecord.IsQualityText = wSFCModuleRecord.IsQuality == 1 ? "合格" : "不合格";

                    //容量、内阻
                    double wCapacity = StringUtils.ParseDouble(wSqlDataReader["Capacity"]);
                    wSFCModuleRecord.Capacity = Math.Round(wCapacity, 2).ToString();
                    wSFCModuleRecord.InternalResistance = Math.Round(StringUtils.ParseDouble(wSqlDataReader["InternalResistance"]), 2).ToString();

                    //交流内阻2
                    double wInternalImpedance = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "交流内阻2", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.InternalImpedance = wInternalImpedance > 0 ? wInternalImpedance.ToString() : "";

                    wResultList.Add(wSFCModuleRecord);
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

        public SFCModuleRecord SFC_QuerySFCModuleRecordByCode(string wCode, out int wErrorCode)
        {
            SFCModuleRecord wResult = new SFCModuleRecord();
            wErrorCode = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "select * from iplantmlm.sfc_modulerecord where ID in (SELECT max(ID) FROM iplantmlm.sfc_modulerecord where Active=1 and (CapacitorPackageNo=@wCode or ModuleNumber=@wCode));";

                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wResult.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wResult.CapacitorPackageNo = StringUtils.ParseString(wSqlDataReader["CapacitorPackageNo"]);
                    wResult.OnlineTime = StringUtils.ParseDate(wSqlDataReader["OnlineTime"]);
                    wResult.ModuleNumber = StringUtils.ParseString(wSqlDataReader["ModuleNumber"]);
                    wResult.OfflineTime = StringUtils.ParseDate(wSqlDataReader["OfflineTime"]);
                    wResult.CurrentPartID = StringUtils.ParseInt(wSqlDataReader["CurrentPartID"]);
                    wResult.Times = StringUtils.ParseInt(wSqlDataReader["Times"]);
                    wResult.BarCode = StringUtils.ParseString(wSqlDataReader["BarCode"]);
                    wResult.Gear = StringUtils.ParseString(wSqlDataReader["Gear"]);
                    wResult.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wResult.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wResult.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wResult.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wResult.TrustBarCode = StringUtils.ParseString(wSqlDataReader["TrustBarCode"]);
                    wResult.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wResult.ActiveText = wResult.Active == 1 ? "激活" : "冻结";
                    wResult.IsQuality = StringUtils.ParseInt(wSqlDataReader["IsQuality"]);
                    wResult.IsQualityText = wResult.IsQuality == 1 ? "合格" : "不合格";
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
            return wResult;
        }

        public List<SFCModuleRecord> SFC_QueryHistoryList(DateTime wSTime, DateTime wETime, string wCode, string wZXCode, string wZTCode, int wIsQuality, string wGear)
        {
            List<SFCModuleRecord> wResult = new List<SFCModuleRecord>();
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM sfc_modulerecord WHERE 1=1"
                    + " and(@wCode is null or @wCode = '' or ModuleNumber= @wCode or CapacitorPackageNo = @wCode)"
                    + " and(CreateTime>=@wSTime and CreateTime <= @wETime)"
                    + " and(@wIsQuality <=0 or IsQuality= @wIsQuality)"
                    + " and(@wZXCode is null or @wZXCode = '' or BarCode= @wZXCode)"
                    + " and(@wGear is null or @wGear = '' or Gear= @wGear)"
                    + " and(@wZTCode is null or @wZTCode = '' or TrustBarCode= @wZTCode)";

                wSqlCommand.Parameters.AddWithValue("@wSTime", wSTime);
                wSqlCommand.Parameters.AddWithValue("@wETime", wETime);
                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);
                wSqlCommand.Parameters.AddWithValue("@wZXCode", wZXCode);
                wSqlCommand.Parameters.AddWithValue("@wZTCode", wZTCode);
                wSqlCommand.Parameters.AddWithValue("@wIsQuality", wIsQuality);
                wSqlCommand.Parameters.AddWithValue("@wGear", wGear);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();
                while (wSqlDataReader.Read())
                {
                    SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();

                    wSFCModuleRecord.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCModuleRecord.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wSFCModuleRecord.CapacitorPackageNo = StringUtils.ParseString(wSqlDataReader["CapacitorPackageNo"]);
                    wSFCModuleRecord.OnlineTime = StringUtils.ParseDate(wSqlDataReader["OnlineTime"]);
                    wSFCModuleRecord.ModuleNumber = StringUtils.ParseString(wSqlDataReader["ModuleNumber"]);
                    wSFCModuleRecord.OfflineTime = StringUtils.ParseDate(wSqlDataReader["OfflineTime"]);
                    wSFCModuleRecord.CurrentPartID = StringUtils.ParseInt(wSqlDataReader["CurrentPartID"]);
                    wSFCModuleRecord.Times = StringUtils.ParseInt(wSqlDataReader["Times"]);
                    wSFCModuleRecord.BarCode = StringUtils.ParseString(wSqlDataReader["BarCode"]);
                    wSFCModuleRecord.Gear = StringUtils.ParseString(wSqlDataReader["Gear"]);
                    wSFCModuleRecord.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wSFCModuleRecord.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wSFCModuleRecord.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wSFCModuleRecord.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wSFCModuleRecord.TrustBarCode = StringUtils.ParseString(wSqlDataReader["TrustBarCode"]);
                    wSFCModuleRecord.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wSFCModuleRecord.ActiveText = wSFCModuleRecord.Active == 1 ? "激活" : "冻结";
                    wSFCModuleRecord.IsQuality = StringUtils.ParseInt(wSqlDataReader["IsQuality"]);
                    wSFCModuleRecord.IsQualityText = wSFCModuleRecord.IsQuality == 1 ? "合格" : "不合格";
                    wSFCModuleRecord.CurrentPartName = wFPCPartList.Exists(p => p.PartID == wSFCModuleRecord.CurrentPartID) ? wFPCPartList.Find(p => p.PartID == wSFCModuleRecord.CurrentPartID).PartName : "";

                    wResult.Add(wSFCModuleRecord);
                }
                wSqlDataReader.Close();

                //其他属性赋值
                foreach (SFCModuleRecord wSFCModuleRecord in wResult)
                {
                    //①容量
                    double wCapacity = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "容量", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.Capacity = wCapacity > 0 ? wCapacity.ToString() : "";
                    //②直流内阻
                    double wInternalResistance = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "直流内阻", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.InternalResistance = wInternalResistance > 0 ? wInternalResistance.ToString() : "";

                    //绝缘电阻1
                    double wInsulationInternalResistance1 = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "绝缘电阻1", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.InsulationInternalResistance1 = wInsulationInternalResistance1 > 0 ? wInsulationInternalResistance1.ToString() : "";
                    //交流内阻1
                    double wInternalImpedance1 = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "交流内阻1", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.InternalImpedance1 = wInternalImpedance1 > 0 ? wInternalImpedance1.ToString() : "";

                    //③绝缘电阻2
                    double wInsulationInternalResistance = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "绝缘电阻2", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.InsulationInternalResistance = wInsulationInternalResistance > 0 ? wInsulationInternalResistance.ToString() : "";
                    //④交流内阻2
                    double wInternalImpedance = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "交流内阻2", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.InternalImpedance = wInternalImpedance > 0 ? wInternalImpedance.ToString() : "";

                    //⑤外观质量
                    int wAppearanceQuality = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "外观质量", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.AppearanceQuality = wAppearanceQuality == 1 ? "OK" : wAppearanceQuality > 1 ? "NG" : "";
                    //⑥单体电压检测结果
                    int wMonomerVoltage = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "单体电压", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.MonomerVoltage = wMonomerVoltage == 1 ? "OK" : wMonomerVoltage > 1 ? "NG" : "";
                    //⑦耐压测试
                    int WithstandVoltageTest = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "耐压测试", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.WithstandVoltageTest = WithstandVoltageTest == 1 ? "OK" : WithstandVoltageTest > 1 ? "NG" : "";
                    //电容包装配完成度
                    int CapacityCompletion = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "电容包装配", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.CapacityCompletion = CapacityCompletion == 1 ? "OK" : CapacityCompletion > 1 ? "NG" : "";
                    //⑧装配完成度(模组)
                    int AssemblyCompletion = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "模组装配", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.AssemblyCompletion = AssemblyCompletion == 1 ? "OK" : AssemblyCompletion > 1 ? "NG" : "";

                    //⑨三串测试结果
                    int SCTestResult = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "三串测试结果", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.SCTestResult = SCTestResult == 1 ? "OK" : SCTestResult > 1 ? "NG" : "";

                    //PCB编号
                    wSFCModuleRecord.PCBNo = IPTTextValueDAO.Instance.IPT_QueryIPTTextValue(wSFCModuleRecord.SerialNumber, "PCB编号", wSFCModuleRecord.ProductID);
                    //单体编号
                    wSFCModuleRecord.SingleNo = IPTTextValueDAO.Instance.IPT_QueryIPTTextValue(wSFCModuleRecord.SerialNumber, "单体编号", wSFCModuleRecord.ProductID);
                    //R1阻值
                    double wR1Nuber = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "R61阻值", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.R1Number = wR1Nuber > 0 ? wR1Nuber.ToString() : "";
                    //R2阻值
                    double wR2Nuber = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "R62阻值", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.R2Number = wR2Nuber > 0 ? wR2Nuber.ToString() : "";
                    //静置电压
                    double wSV = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "静置电压", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.StandingVoltage = wSV > 0 ? wSV.ToString() : "";
                    //静置时间
                    double wSH = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValue(wSFCModuleRecord.SerialNumber, "静置时间", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.StandingHour = wSH > 0 ? wSH.ToString() : "";
                    //模组标签/档位信息
                    int wLable = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValue(wSFCModuleRecord.SerialNumber, "模组标签", wSFCModuleRecord.ProductID);
                    wSFCModuleRecord.LableInfo = wLable == 1 ? "OK" : wLable > 1 ? "NG" : "";
                }
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

        public ModuleChart SFC_QueryModuleChart(int wShiftID)
        {
            ModuleChart wResult = new ModuleChart();
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "select (SELECT count(distinct(CapacitorPackageNo)) FROM iplantmlm.sfc_modulerecord where ShiftID=@wShiftID) as Total,(SELECT count(distinct(CapacitorPackageNo)) FROM iplantmlm.sfc_modulerecord where ShiftID=@wShiftID and (  (CurrentPartID>=4 and IsQuality=1) or CurrentPartID>4  ) ) DTGood, (SELECT count(distinct(CapacitorPackageNo)) FROM iplantmlm.sfc_modulerecord where ShiftID=@wShiftID and CurrentPartID=4 and IsQuality=2) DTBad,(SELECT count(distinct(CapacitorPackageNo)) FROM iplantmlm.sfc_modulerecord where ShiftID=@wShiftID and ( (CurrentPartID>=9 and IsQuality=1) or CurrentPartID>9  )   ) MZGood,(SELECT count(distinct(CapacitorPackageNo)) FROM iplantmlm.sfc_modulerecord where ShiftID=@wShiftID and CurrentPartID=9 and IsQuality=2) MZBad;";

                wSqlCommand.Parameters.AddWithValue("@wShiftID", wShiftID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();

                while (wSqlDataReader.Read())
                {
                    wResult.Total = StringUtils.ParseInt(wSqlDataReader["Total"].ToString());
                    wResult.DTGood = StringUtils.ParseInt(wSqlDataReader["DTGood"].ToString());
                    wResult.DTBad = StringUtils.ParseInt(wSqlDataReader["DTBad"].ToString());
                    wResult.MZGood = StringUtils.ParseInt(wSqlDataReader["MZGood"].ToString());
                    wResult.MZBad = StringUtils.ParseInt(wSqlDataReader["MZBad"].ToString());
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

        /// <summary>
        /// 查询单体电压检测结果
        /// </summary>
        /// <param name="wCode">电容包编号或模组编码</param>
        /// <returns>单体电压检测结果</returns>
        public int QueryWithstandVoltageTestByCode(String wCode)
        {
            int wResult = 0;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT WithstandVoltageTest FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@wCode or ModuleNumber=@wCode;";

                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();

                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseInt(wSqlDataReader["WithstandVoltageTest"].ToString());
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

        /// <summary>
        /// 查询容量和直流内阻-设备检测结果
        /// </summary>
        /// <param name="wCode">电容包或模组编码</param>
        /// <param name="wCapacity">容量</param>
        /// <param name="wInternalResistance">直流内阻</param>
        public void QueryCapacityAndInternalResistanceByCode(string wCode, out double wCapacity, out double wInternalResistance)
        {
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            wCapacity = 0;
            wInternalResistance = 0;
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT Capacity,InternalResistance FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@wCode or ModuleNumber=@wCode;";

                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();

                while (wSqlDataReader.Read())
                {
                    wCapacity = StringUtils.ParseDouble(wSqlDataReader["Capacity"].ToString());
                    wInternalResistance = StringUtils.ParseDouble(wSqlDataReader["InternalResistance"].ToString());

                    //四舍五入，保留两位小数
                    wCapacity = Math.Round(wCapacity, 2);
                    wInternalResistance = Math.Round(wInternalResistance, 2);
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
        }

        public List<List<string>> QueryPrintList(int wShiftID, bool wIsJF)
        {
            List<List<string>> wResult = new List<List<string>>();

            List<string> wTitltList = new List<string>();
            wTitltList.Add("序号");
            wTitltList.Add("模组编码");
            wTitltList.Add("容量");
            wTitltList.Add("档位");
            wTitltList.Add("交流内阻2");
            if (wIsJF)
                wTitltList.Add("二维码");
            wResult.Add(wTitltList);

            DateTime wSTime = DateTime.Now;
            wSTime = new DateTime(wSTime.Year, wSTime.Month, wSTime.Day, 0, 0, 0);
            DateTime wETime = DateTime.Now;
            wETime = new DateTime(wETime.Year, wETime.Month, wETime.Day, 23, 59, 59);

            MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();

            FPCProduct wFPCProduct = FPCProductDAO.Instance.GetProductList().Find(p => p.ProductID == wConfig.CurrentProduct);

            int wErrorCode = 0;
            List<int> wPartList = new List<int>() { 9 };
            List<SFCModuleRecord> wSFCModuleRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(wConfig.CurrentProduct, wSTime, wETime, "", wPartList, out wErrorCode);
            int wIndex = 1;
            foreach (SFCModuleRecord wSFCModuleRecord in wSFCModuleRecordList)
            {
                List<string> wList = new List<string>();
                wList.Add(wIndex.ToString());
                wList.Add(StringUtils.ParseString(wSFCModuleRecord.ModuleNumber));
                double wCapacity = StringUtils.ParseDouble(wSFCModuleRecord.Capacity);
                wList.Add(Math.Round(wCapacity, 2).ToString());
                wList.Add(StringUtils.ParseString(wSFCModuleRecord.Gear));
                wList.Add(Math.Round(StringUtils.ParseDouble(wSFCModuleRecord.InternalImpedance), 2).ToString());

                if (wIsJF)
                    wList.Add(wFPCProduct.BarCodePrefix + wSFCModuleRecord.ModuleNumber);

                wResult.Add(wList);
                wIndex++;
            }

            //MySqlConnection wCon = GUD.SQLPool.GetConnection();
            //try
            //{
            //    MySqlCommand wSqlCommand = new MySqlCommand();
            //    wSqlCommand.Connection = wCon;
            //    //wSqlCommand.CommandText = "SELECT ModuleNumber,Capacity,Gear,InternalResistance FROM iplantmlm.sfc_modulerecord where ShiftID=@wShiftID and CurrentPartID>6;";
            //    //wSqlCommand.CommandText = "SELECT ModuleNumber,Capacity,Gear,InternalResistance FROM iplantmlm.sfc_modulerecord where CurrentPartID>6 and CurrentPartID < 9;";
            //    wSqlCommand.CommandText = "SELECT ModuleNumber,Capacity,Gear,InternalResistance FROM iplantmlm.sfc_modulerecord where CurrentPartID>6 and (SerialNumber in (SELECT distinct SerialNumber FROM iplantmlm.ipt_boolvalue where ShiftID=@wShiftID) or SerialNumber in (SELECT distinct SerialNumber FROM iplantmlm.ipt_numbervalue where ShiftID=@wShiftID) or SerialNumber in (SELECT distinct SerialNumber FROM iplantmlm.ipt_textvalue where ShiftID=@wShiftID));";

            //    wSqlCommand.Parameters.AddWithValue("@wShiftID", wShiftID);

            //    DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
            //    int wIndex = 1;
            //    while (wSqlDataReader.Read())
            //    {
            //        List<string> wList = new List<string>();
            //        wList.Add(wIndex.ToString());
            //        wList.Add(StringUtils.ParseString(wSqlDataReader["ModuleNumber"]));
            //        double wCapacity = StringUtils.ParseDouble(wSqlDataReader["Capacity"]);
            //        wList.Add(Math.Round(wCapacity, 2).ToString());
            //        wList.Add(StringUtils.ParseString(wSqlDataReader["Gear"]));
            //        wList.Add(Math.Round(StringUtils.ParseDouble(wSqlDataReader["InternalResistance"]), 2).ToString());

            //        wResult.Add(wList);

            //        wIndex++;
            //    }
            //    wSqlDataReader.Close();
            //}
            //catch (Exception ex)
            //{
            //    GUD.SQLPool.CloseConnection(wCon);
            //    LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            //}
            //finally
            //{
            //    GUD.SQLPool.FreeConnection(wCon);
            //}
            return wResult;
        }

        /// <summary>
        /// 判断当前工位是否填写了数据
        /// </summary>
        internal bool SFC_QueryPreIsDone(string wCode, int wPartID, out int wErrorCode)
        {
            bool wResult = false;
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            wErrorCode = 0;
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "select (select count(*) FROM iplantmlm.ipt_boolvalue where SerialNumber in (SELECT SerialNumber FROM iplantmlm.sfc_modulerecord where ID in (SELECT max(ID) FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@wCode or ModuleNumber=@wCode)) and PartID=@wPartID) FQTY1,(select count(*) FROM iplantmlm.ipt_numbervalue where SerialNumber in (SELECT SerialNumber FROM iplantmlm.sfc_modulerecord where ID in (SELECT max(ID) FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@wCode or ModuleNumber=@wCode)) and PartID=@wPartID) FQTY2,(select count(*) FROM iplantmlm.ipt_textvalue where SerialNumber in (SELECT SerialNumber FROM iplantmlm.sfc_modulerecord where ID in (SELECT max(ID) FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@wCode or ModuleNumber=@wCode)) and PartID=@wPartID) FQTY3;";

                wSqlCommand.Parameters.AddWithValue("@wCode", wCode);
                wSqlCommand.Parameters.AddWithValue("@wPartID", wPartID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();

                    int wFQTY1 = StringUtils.ParseInt(wSqlDataReader["FQTY1"].ToString());
                    int wFQTY2 = StringUtils.ParseInt(wSqlDataReader["FQTY2"].ToString());
                    int wFQTY3 = StringUtils.ParseInt(wSqlDataReader["FQTY3"].ToString());

                    if (wFQTY1 + wFQTY2 + wFQTY3 <= 0)
                        wResult = false;
                    else
                        wResult = true;
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
            return wResult;
        }

        internal List<SFCModuleRecord> SFC_QuerySFCModuleRecordByTime(DateTime wSTime, DateTime wETime)
        {
            List<SFCModuleRecord> wResultList = new List<SFCModuleRecord>();
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT * FROM sfc_modulerecord WHERE 1=1"
                    + " and(OnlineTime>=@wSTime and OnlineTime<=@wETime);";

                wSqlCommand.Parameters.AddWithValue("@wSTime", wSTime);
                wSqlCommand.Parameters.AddWithValue("@wETime", wETime);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();

                    wSFCModuleRecord.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCModuleRecord.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wSFCModuleRecord.CapacitorPackageNo = StringUtils.ParseString(wSqlDataReader["CapacitorPackageNo"]);
                    wSFCModuleRecord.OnlineTime = StringUtils.ParseDate(wSqlDataReader["OnlineTime"]);
                    wSFCModuleRecord.ModuleNumber = StringUtils.ParseString(wSqlDataReader["ModuleNumber"]);
                    wSFCModuleRecord.OfflineTime = StringUtils.ParseDate(wSqlDataReader["OfflineTime"]);
                    wSFCModuleRecord.CurrentPartID = StringUtils.ParseInt(wSqlDataReader["CurrentPartID"]);
                    wSFCModuleRecord.Times = StringUtils.ParseInt(wSqlDataReader["Times"]);
                    wSFCModuleRecord.BarCode = StringUtils.ParseString(wSqlDataReader["BarCode"]);
                    wSFCModuleRecord.Gear = StringUtils.ParseString(wSqlDataReader["Gear"]);
                    wSFCModuleRecord.ShiftID = StringUtils.ParseInt(wSqlDataReader["ShiftID"]);
                    wSFCModuleRecord.CreateID = StringUtils.ParseInt(wSqlDataReader["CreateID"]);
                    wSFCModuleRecord.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                    wSFCModuleRecord.CreateTime = StringUtils.ParseDate(wSqlDataReader["CreateTime"]);
                    wSFCModuleRecord.TrustBarCode = StringUtils.ParseString(wSqlDataReader["TrustBarCode"]);

                    double wCapacity = StringUtils.ParseDouble(wSqlDataReader["Capacity"].ToString());
                    wCapacity = Math.Round(wCapacity, 2);
                    wSFCModuleRecord.Capacity = wCapacity.ToString();

                    double wInternalResistance = StringUtils.ParseDouble(wSqlDataReader["InternalResistance"].ToString());
                    wInternalResistance = Math.Round(wInternalResistance, 2);
                    wSFCModuleRecord.InternalResistance = wInternalResistance.ToString();

                    wSFCModuleRecord.Active = StringUtils.ParseInt(wSqlDataReader["Active"]);
                    wSFCModuleRecord.ActiveText = wSFCModuleRecord.Active == 1 ? "激活" : "冻结";
                    wSFCModuleRecord.IsQuality = StringUtils.ParseInt(wSqlDataReader["IsQuality"]);
                    wSFCModuleRecord.IsQualityText = wSFCModuleRecord.IsQuality == 1 ? "合格" : "不合格";

                    wResultList.Add(wSFCModuleRecord);
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
            return wResultList;
        }

        internal void SFC_UpdateRL(int wID, double wRLValued, double wNZValued)
        {
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = string.Format("update sfc_modulerecord set Capacity=@wCapacity,InternalResistance=@wInternalResistance WHERE ID = @wID;");

                wSqlCommand.Parameters.AddWithValue("@wCapacity", wRLValued);
                wSqlCommand.Parameters.AddWithValue("@wInternalResistance", wNZValued);
                wSqlCommand.Parameters.AddWithValue("@wID", wID);

                int wSQL_Result = wSqlCommand.ExecuteNonQuery();
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
        }

        public string GetSingleInfo(string wCapacityNo)
        {
            string wResult = "";
            MySqlConnection wCon = GUD.SQLPool.GetConnection();
            try
            {
                MySqlCommand wSqlCommand = new MySqlCommand();
                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT NumArray FROM iplantmlm.ipt_singleinfo where CapacityNo=@wCapacityNo;";

                wSqlCommand.Parameters.AddWithValue("@wCapacityNo", wCapacityNo);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseString(wSqlDataReader["NumArray"]);
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

