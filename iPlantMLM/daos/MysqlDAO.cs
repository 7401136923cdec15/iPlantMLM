using iPlantMLM;
using MySql.Data.MySqlClient;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class MysqlDAO
    {
        #region 单实例
        private MysqlDAO() { }
        private static MysqlDAO _Instance;

        public static MysqlDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MysqlDAO();
                return _Instance;
            }
        }
        #endregion

        private string mysqlString = ConfigurationManager.AppSettings["mysql"];

        //插入数据
        public void InsertItem(SFCModuleRecord wSFCModuleRecord, double wValue, int wStandardID)
        {
            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "insert into iplantmlm.ipt_numbervalue(SerialNumber,StandardID,Value,CreateID,CreateTime,ShiftID,PartID) values(@wSerialNumber,@wStandardID,@wValue,1,now(),@wShiftID,6);";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@wSerialNumber", wSFCModuleRecord.SerialNumber);
                wSqlCommand.Parameters.AddWithValue("@wStandardID", wStandardID);
                wSqlCommand.Parameters.AddWithValue("@wValue", wValue);
                wSqlCommand.Parameters.AddWithValue("@wShiftID", wShiftID);

                wSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
        }

        //删除数据
        public void DeleteItem(string wSerialNumber, int wStandardID)
        {
            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "delete from iplantmlm.ipt_boolvalue where SerialNumber=@SerialNumber and StandardID=@StandardID and ID>0;";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@SerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@StandardID", wStandardID);

                wSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
        }

        //删除数据
        public void DeleteItem_Number(string wSerialNumber, int wStandardID)
        {
            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "delete from iplantmlm.ipt_numbervalue where SerialNumber=@SerialNumber and StandardID=@StandardID and ID>0;";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@SerialNumber", wSerialNumber);
                wSqlCommand.Parameters.AddWithValue("@StandardID", wStandardID);

                wSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
        }

        public string GetSerialNumber(string wCapacitorPackageNo, out int wProductID)
        {
            string wResult = "";

            MySqlConnection wCon = getMySqlConn();
            wProductID = 0;
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "select SerialNumber,ProductID FROM iplantmlm.sfc_modulerecord where ID in (SELECT max(ID) FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@CapacitorPackageNo);";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@CapacitorPackageNo", wCapacitorPackageNo);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"]);
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
            return wResult;
        }

        public void UpdateModuleRecord(string wGear, int wID)
        {
            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "update iplantmlm.sfc_modulerecord set CurrentPartID=6,Gear=@wGear where ID=@wID;";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@wGear", wGear);
                wSqlCommand.Parameters.AddWithValue("@wID", wID);

                wSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
        }

        /// <summary>
        /// 根据产品和容量获取档位
        /// </summary>
        public string GetGear(int wProductID, double wCapacity)
        {
            string wResult = "";

            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT Gear FROM iplantmlm.ipt_capacitygrading where ProductID=@ProductID and LowerLimit<=@wCapacity and @wCapacity <=UpLimit;";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@ProductID", wProductID);
                wSqlCommand.Parameters.AddWithValue("@wCapacity", wCapacity);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseString(wSqlDataReader["Gear"]);
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
            return wResult;
        }

        public List<SFCModuleRecord> GetSFCModuleRecordList()
        {
            List<SFCModuleRecord> wResult = new List<SFCModuleRecord>();
            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "Select ID,SerialNumber,CapacitorPackageNo,Capacity,InternalResistance,ProductID from iplantmlm.sfc_modulerecord where Gear='' and Active=1 and Capacity>0;";

                wSqlCommand.Parameters.Clear();

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();

                    wSFCModuleRecord.ID = StringUtils.ParseInt(wSqlDataReader["ID"]);
                    wSFCModuleRecord.SerialNumber = StringUtils.ParseString(wSqlDataReader["SerialNumber"]);
                    wSFCModuleRecord.CapacitorPackageNo = StringUtils.ParseString(wSqlDataReader["CapacitorPackageNo"]);
                    wSFCModuleRecord.Capacity = StringUtils.ParseString(wSqlDataReader["Capacity"].ToString());
                    wSFCModuleRecord.InternalResistance = StringUtils.ParseString(wSqlDataReader["InternalResistance"].ToString());
                    wSFCModuleRecord.ProductID = StringUtils.ParseInt(wSqlDataReader["ProductID"].ToString());

                    wResult.Add(wSFCModuleRecord);
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
            return wResult;
        }

        /// <summary>
        /// 查询标准ID
        /// </summary>
        public int GetStandardID(int wProductID, int wItemID)
        {
            int wResult = 0;

            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT ID FROM iplantmlm.ipt_standard where ProductID=@ProductID and ItemID=@wItemID;";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@ProductID", wProductID);
                wSqlCommand.Parameters.AddWithValue("@wItemID", wItemID);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseInt(wSqlDataReader["ID"]);
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
            return wResult;
        }

        public int GetCurrentPartID(string wPacageNo)
        {
            int wResult = 0;

            MySqlConnection wCon = getMySqlConn();
            try
            {
                wCon.Open();

                MySqlCommand wSqlCommand = new MySqlCommand();

                wSqlCommand.Connection = wCon;
                wSqlCommand.CommandText = "SELECT CurrentPartID FROM iplantmlm.sfc_modulerecord where ID in (SELECT max(ID) FROM iplantmlm.sfc_modulerecord where CapacitorPackageNo=@CapacitorPackageNo);";

                wSqlCommand.Parameters.Clear();
                wSqlCommand.Parameters.AddWithValue("@CapacitorPackageNo", wPacageNo);

                DbDataReader wSqlDataReader = wSqlCommand.ExecuteReader();
                while (wSqlDataReader.Read())
                {
                    wResult = StringUtils.ParseInt(wSqlDataReader["CurrentPartID"].ToString());
                }
                wSqlDataReader.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                wCon.Close();
            }
            finally
            {
                wCon.Close();
            }
            return wResult;
        }

        //建立mysql数据库链接
        public MySqlConnection getMySqlConn()
        {
            string constr = mysqlString;
            MySqlConnection mycon = new MySqlConnection(constr);
            return mycon;
        }
    }
}

