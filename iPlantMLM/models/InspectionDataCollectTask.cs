using MySql.Data.MySqlClient;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace iPlantMLM
{
    public class InspectionDataCollectTask
    {
        private static InspectionDataCollectTask mInstance;

        public static InspectionDataCollectTask Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new InspectionDataCollectTask();

                return mInstance;
            }
        }

        //#region 读取和上传线程
        ///// <summary>
        ///// 不断从数据库获取待上传数据的线程
        ///// </summary>
        //private Thread mThreadReadData;

        ///// <summary>
        ///// 上传数据到MES服务的线程
        ///// </summary>
        //private Thread mThreadUploadData;
        //#endregion

        #region Data
        /// <summary>
        /// 每次最多从数据库读取多少条记录
        /// 此参数必须大于已知的单条检验规程最大项点数量
        /// </summary>
        //private int mMaxReadRows = 1000;

        /// <summary>
        /// 从数据库获取的待上传数据临时缓存
        /// </summary>
        private List<ExamineDataItem> mDataItemListFromDB = new List<ExamineDataItem>();
        #endregion

        //public InspectionDataCollectTask()
        //{
        //    mThreadReadData = new Thread(new ThreadStart(ReadDataFromDBTask))
        //    {
        //        IsBackground = true
        //    };

        //    mThreadUploadData = new Thread(new ThreadStart(UploadDataToMESTask))
        //    {
        //        IsBackground = true
        //    };
        //}

        //public void StartTask()
        //{
        //    try
        //    {
        //        mThreadReadData.Start();
        //        mThreadUploadData.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        LoggerTool.SaveException(
        //            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //    }
        //}

        #region 读取数据

        //private void ReadDataFromDBTask()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            Thread.Sleep(10000);

        //            if (string.IsNullOrWhiteSpace(GUD.SQLConnectString) || (!DBManager.Instance.CheckSQLConnection()))
        //                continue;

        //            lock (mDataItemListFromDB)
        //            {
        //                mDataItemListFromDB = GetItemListFromDB();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LoggerTool.SaveException(
        //                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //                System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //            //throw ex;
        //        }
        //    }
        //}

        //private List<ExamineDataItem> GetItemListFromDB()
        //{
        //    List<ExamineDataItem> wResultList = new List<ExamineDataItem>();

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(GUD.SQLConnectString))
        //        {
        //            return wResultList;
        //        }

        //        using (MySqlConnection wConnection = new MySqlConnection(GUD.SQLConnectString))
        //        {
        //            wConnection.Open();

        //            using (MySqlCommand wCommand = new MySqlCommand())
        //            {
        //                wCommand.Connection = wConnection;
        //                wCommand.CommandText = string.Format(
        //                    "SELECT * FROM `netdatamanager`.`datacollect` WHERE ID > 0 " +
        //                    "AND `NeedUpload` = 1 AND `SFCTaskStepID` != 0 ORDER BY ID LIMIT {0};", mMaxReadRows);

        //                using (MySqlDataReader wReader = wCommand.ExecuteReader())
        //                {
        //                    wResultList = MyFunctionTool.ParseExamineDataFromReader(wReader);

        //                    wReader.Close();
        //                }
        //            }
        //        }

        //        // 剔除最后可能不完整的数据
        //        // 如果此次获取的行数不足最大获取行数，说明拿到的是最后一波数据，则不用剔除
        //        if (wResultList.Count == mMaxReadRows)
        //        {
        //            // 最后一条数据的Code(电容包编码)
        //            string wCode = wResultList[mMaxReadRows - 1].Code;
        //            string wStationID = wResultList[mMaxReadRows - 1].ItemGroup;
        //            wResultList.RemoveAll(p => (p.Code == wCode) && (p.ItemGroup == wStationID));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerTool.SaveException(
        //            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //        throw ex;
        //    }

        //    return wResultList;
        //}
        #endregion

        #region 上传数据
        //private void UploadDataToMESTask()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            Thread.Sleep(10000);

        //            UploadData();
        //        }
        //        catch (Exception ex)
        //        {
        //            LoggerTool.SaveException(
        //               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //               System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //            //throw ex;
        //        }
        //    }
        //}

        //private void UploadData()
        //{
        //    try
        //    {
        //        lock (mDataItemListFromDB)
        //        {
        //            if (mDataItemListFromDB.Count < 1)
        //            {
        //                return;
        //            }

        //            SubmitMESIPTBill(mDataItemListFromDB);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerTool.SaveException(
        //            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //        throw ex;
        //    }
        //}

        //public void SubmitMESIPTBill(List<ExamineDataItem> wExamineDataItemList)
        //{
        //    try
        //    {
        //        List<int> wIDSuccessList = new List<int>(); // 提交成功ID列表
        //        List<int> wFailedRecordIDList = new List<int>(); // 创建表单失败的ID列表

        //        // 所有出现的任务ID列表

        //        Dictionary<int, List<int>> wSFCTaskPartIDList =
        //            wExamineDataItemList.ToLookup(t => t.SFCTaskPartID, t => t.GroupEnum.MESStationID).ToDictionary(t => t.Key, t => t.Distinct().ToList());

        //        // 用任务ID列表换取表单ID列表
        //        Dictionary<int, List<int>> wBadTaskPartIDList = new Dictionary<int, List<int>>();

        //        Dictionary<int, Dictionary<int, SFCTaskIPT>> wTaskDic = GetSFCTaskIPTList(wSFCTaskPartIDList, out wBadTaskPartIDList);

        //        // 若创建的表单个数和任务ID不匹配，说明有创建表单失败的情况，将对应记录上传状态置为2，后续再操作
        //        if (wBadTaskPartIDList.Count > 0)
        //        {
        //            wFailedRecordIDList = wExamineDataItemList.FindAll(p =>
        //            wBadTaskPartIDList.ContainsKey(p.SFCTaskPartID)
        //            && wBadTaskPartIDList[p.SFCTaskPartID].Contains(p.GroupEnum.MESStationID)).Select(p => p.ID).ToList();
        //        }

        //        // 按照工位和电容包编码分组
        //        Dictionary<string, Dictionary<string, List<ExamineDataItem>>> wGroupByStationAndCodeResult =
        //            GroupDataByStationAndCode(wExamineDataItemList);

        //        // 上传
        //        SFCTaskIPT wSFCTaskIPT = null;
        //        foreach (string wStationID in wGroupByStationAndCodeResult.Keys)
        //        {
        //            foreach (string wCode in wGroupByStationAndCodeResult[wStationID].Keys)
        //            {
        //                if (wGroupByStationAndCodeResult[wStationID][wCode].Count < 1)
        //                {
        //                    continue;
        //                }
        //                int wTaskPartID = wGroupByStationAndCodeResult[wStationID][wCode][0].SFCTaskPartID;
        //                List<IPTValue> wIPTValueList = ExamineDataItem.ParseToIPTValueList(
        //                    wGroupByStationAndCodeResult[wStationID][wCode]);

        //                if (!wTaskDic.ContainsKey(wTaskPartID) 
        //                    || !wTaskDic[wTaskPartID].ContainsKey(wGroupByStationAndCodeResult[wStationID][wCode][0].GroupEnum.MESStationID))
        //                {
        //                    continue;
        //                }
        //                wSFCTaskIPT = wTaskDic[wTaskPartID][wGroupByStationAndCodeResult[wStationID][wCode][0].GroupEnum.MESStationID];

        //                //填充wTaskDic：电容包编码、模组编码、大箱箱号
        //                wSFCTaskIPT.WorkPartNo = wCode;//电容包编码

        //                ExamineDataItem wExamineDataItemModuleNo = wGroupByStationAndCodeResult[wStationID][wCode].Find(p => p.ItemName.Trim().Equals("模组编码"));
        //                if (wExamineDataItemModuleNo != null)
        //                {
        //                    wSFCTaskIPT.ModulePartNo = wExamineDataItemModuleNo.ItemValue;//大箱箱号
        //                }

        //                ExamineDataItem wExamineDataItemBoxNo = wGroupByStationAndCodeResult[wStationID][wCode].Find(p => p.ItemName.Trim().Equals("大箱箱号"));
        //                if (wExamineDataItemBoxNo != null)
        //                {
        //                    wSFCTaskIPT.BoxNo = wExamineDataItemBoxNo.ItemValue;//大箱箱号
        //                }

        //                APIResult wUploadResult = MyWebAPITool.Instance.NET_SaveValueList(
        //                    wSFCTaskIPT, wIPTValueList, (int)OperateType.提交);
        //                if ((wUploadResult != null) && (wUploadResult.resultCode == 1000))
        //                {
        //                    wIDSuccessList.AddRange(wGroupByStationAndCodeResult[wStationID][wCode].Select(p => p.ID).Distinct().ToList());
        //                }
        //                else//提交失败则标记此数据需后台上传
        //                {
        //                    wGroupByStationAndCodeResult[wStationID][wCode].ForEach((p) => p.NeedUpload = 1);
        //                }
        //            }
        //        }

        //        if (wIDSuccessList.Count > 0)
        //        {
        //            ChangeUploadedStatus(wIDSuccessList, 0);
        //        }
        //        if (wFailedRecordIDList.Count > 0)
        //        {
        //            ChangeUploadedStatus(wFailedRecordIDList, 2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerTool.SaveException(
        //                          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //                          System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// 分割记录(按照录入表格的一行为单位，作为一个表单对应的检验数据)
        /// </summary>
        /// <param name="wDataSource"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, List<ExamineDataItem>>> GroupDataByStationAndCode(
            List<ExamineDataItem> wDataSource)
        {
            Dictionary<string, Dictionary<string, List<ExamineDataItem>>> wResultDic =
                new Dictionary<string, Dictionary<string, List<ExamineDataItem>>>();

            try
            {
                // 先按工位分组
                IEnumerable<IGrouping<string, ExamineDataItem>> wGroupByStationResult = wDataSource.GroupBy(p => p.ItemGroup);
                foreach (IGrouping<string, ExamineDataItem> wItem in wGroupByStationResult)
                {
                    string wGroupID = wItem.Key;
                    List<ExamineDataItem> wItemListOfStation = wItem.ToList();

                    wResultDic.Add(wGroupID, new Dictionary<string, List<ExamineDataItem>>());

                    // 再按电容包编码分组
                    IEnumerable<IGrouping<string, ExamineDataItem>> wGroupByCodeResult = wItemListOfStation.GroupBy(p => p.Code);
                    foreach (var wItemByCode in wGroupByCodeResult)
                    {
                        string wCode = wItemByCode.Key;
                        List<ExamineDataItem> wItemListOfCode = wItemByCode.ToList();

                        wResultDic[wGroupID].Add(wCode, wItemListOfCode);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                throw ex;
            }

            return wResultDic;
        }

        /// <summary>
        /// 用任务ID列表创建表单列表
        /// </summary>
        /// <param name="wSFCTaskStepIDList"></param>
        /// <returns></returns>
        //public Dictionary<int, Dictionary<int, SFCTaskIPT>> GetSFCTaskIPTList(Dictionary<int, List<int>> wSFCTaskPartIDList, out Dictionary<int, List<int>> wBadTaskPartIDList)
        //{
        //    Dictionary<int, Dictionary<int, SFCTaskIPT>> wResultDic = new Dictionary<int, Dictionary<int, SFCTaskIPT>>();
        //    wBadTaskPartIDList = new Dictionary<int, List<int>>();
        //    try
        //    {
        //        foreach (int wID in wSFCTaskPartIDList.Keys)
        //        {
        //            Dictionary<int, SFCTaskIPT> wDic = new Dictionary<int, SFCTaskIPT>();
        //            foreach (int wStationID in wSFCTaskPartIDList[wID])
        //            {

        //                APIResult wResult = MyWebAPITool.Instance.NET_CreateBillWithoutRestrictions(
        //                BMSEmployee.MESSystemAdmin, wID, (int)IPTMode.生产自检, wStationID);
        //                if ((wResult == null) || (wResult.resultCode != 1000))
        //                {
        //                    LoggerTool.SaveException(
        //                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //                        System.Reflection.MethodBase.GetCurrentMethod().Name + "()",
        //                        wResult == null ? "创建生产自检单失败，API Error." : wResult.getMsg());

        //                    if (!wBadTaskPartIDList.ContainsKey(wID))
        //                        wBadTaskPartIDList.Add(wID, new List<int>());
        //                    wBadTaskPartIDList[wID].Add(wStationID);
        //                    continue;
        //                }

        //                List<IPTItem> wIPTItemList = wResult.List<IPTItem>();
        //                SFCTaskIPT wSFCTaskIPT = wResult.Info<SFCTaskIPT>();
        //                wSFCTaskIPT.ID = 0;

        //                wDic.Add(wStationID, wSFCTaskIPT);
        //            }
        //            if (wDic.Count > 0)
        //                wResultDic.Add(wID, wDic);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerTool.SaveException(
        //            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //        throw ex;
        //    }

        //    return wResultDic;
        //}

        //public void ChangeUploadedStatus(List<int> wIDList, int wNeedUploadStatus)
        //{
        //    try
        //    {
        //        if (wIDList.Count < 1)
        //            return;
        //        string wSQLString = string.Format(
        //            "UPDATE `netdatamanager`.`datacollect` SET `NeedUpload` = {0}, `DTUpload` = now() WHERE `ID` IN ({1})",
        //            wNeedUploadStatus, string.Join(",", wIDList));

        //        DBManager.ExecuteSqlTransaction(new List<string>() { wSQLString }, GUD.SQLConnectString);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerTool.SaveException(
        //            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //        throw ex;
        //    }
        //}
        #endregion
    }
}
