using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace iPlantMLM
{
    public class ExamineDataItem
    {
        [Category("基础")]
        [DisplayName("编号")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public int ID { get; set; }

        [Category("基础")]
        [DisplayName("电容包编码")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string Code { get; set; }

        [Category("基础")]
        [DisplayName("行号")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string ItemRowID { get; set; }

        [Category("基础")]
        [DisplayName("工位ID")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string ItemGroup { get; set; }

        [Category("基础")]
        [DisplayName("工位名")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string ItemGroupName { get; set; }

        [Category("基础")]
        [DisplayName("参数值")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string ItemValue { get; set; }

        [Category("基础")]
        [DisplayName("参数类型")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public string ItemType { get; set; }

        [Category("基础")]
        [DisplayName("创建时间")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public DateTime SessionTime { get; set; }

        [Category("基础")]
        [DisplayName("编号")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public int ShowMode { get; set; }

        [Category("基础")]
        [DisplayName("编号")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public int Active { get; set; }

        [Category("基础")]
        [DisplayName("编号")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(false)]
        public bool IsCheck { get; set; }

        //显示字段
        [Category("基础")]
        [DisplayName("检验规程ID")]
        [DefaultValue(0)]
        [ReadOnly(true)]
        [Browsable(true)]
        public int StandardID { get; set; }

        //显示字段
        [Category("基础")]
        [DisplayName("项A:参数编号")]
        [DefaultValue(0)]
        [ReadOnly(true)]
        [Browsable(true)]
        public int ItemID { get; set; }

        [Category("基础")]
        [DisplayName("项B:参数名")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string ItemName { get; set; }

        [Category("基础")]
        [DisplayName("项C:顺序")]
        [DefaultValue("")]
        [ReadOnly(true)]
        [Browsable(true)]
        public int OrderID { get; set; }

        [Category("基础")]
        [DisplayName("项D:参数类型")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public ItemTypeEnum TypeEnum { get; set; }

        //[Category("基础")]
        //[DisplayName("项E:参数组")]
        //[DefaultValue("")]
        //[ReadOnly(true)]
        //[Browsable(true)]
        //public StationMappingInfoItem GroupEnum { get; set; }
        //public string GroupEnumText
        //{
        //    get
        //    {
        //        return GroupEnum.LocalStationName;
        //    }
        //}

        [Category("基础")]
        [DisplayName("项F:参数描述")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string ItemDescription { get; set; }

        [Category("基础")]
        [DisplayName("项G:激活状态")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string ActiveText { get; set; }

        [Category("基础")]
        [DisplayName("项H:是否必填")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public bool IsRequired { get; set; }

        [Category("基础")]
        [DisplayName("项I:是否联动")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public bool IsLinkControl { get; set; }

        [Category("基础")]
        [DisplayName("项J:联动行数")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public int LinkRows { get; set; }

        [Category("基础")]
        [DisplayName("项K:是否主键")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public bool IsPrimaryKey { get; set; }


        [Category("基础")]
        [DisplayName("项L:是否可编辑")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public bool IsEdit { get; set; }

        [Category("基础")]
        [DisplayName("项M:是否可重复")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public bool IsRepeat { get; set; }

        [Category("基础")]
        [DisplayName("项N:默认值类型")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public DefaultValueTypeEnum DefalutValueType { get; set; }

        [Category("基础")]
        [DisplayName("项O:默认值格式")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string DefaultValueForm { get; set; }

        [Category("基础")]
        [DisplayName("项P:关联字段")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string RelationParameter { get; set; }

        [Category("基础")]
        [DisplayName("项Q:关联字段系数")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string RelationRatio { get; set; }

        [Category("基础")]
        [DisplayName("项R:值长度")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public int ValueLength { get; set; }

        [Category("基础")]
        [DisplayName("项S:过滤")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string FilterChar { get; set; }

        [Category("基础")]
        [DisplayName("任务ID")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public int SFCTaskStepID { get; set; }


        [Category("基础")]
        [DisplayName("工序任务ID")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public int SFCTaskPartID { get; set; }

        [Category("基础")]
        [DisplayName("产品型号")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string ProductNo { get; set; }

        [Category("基础")]
        [DisplayName("工单号")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public string OrderNo { get; set; }

        [Category("基础")]
        [DisplayName("是否上传")]
        [DefaultValue("")]
        [ReadOnly(false)]
        [Browsable(true)]
        public int NeedUpload { get; set; }
        public ExamineDataItem()
        {
            Code = "";
            ItemRowID = "";
            ItemGroup = "";
            ItemGroupName = "";
            ItemValue = "";
            ItemType = "";
            SessionTime = new DateTime(2000, 1, 1);
            ItemName = "";
            TypeEnum = ItemTypeEnum.文本;
            //GroupEnum = new StationMappingInfoItem();
            IsEdit = true;
            IsRepeat = true;
            FilterChar = "";
            ItemDescription = "";
            ActiveText = "";
            RelationParameter = "";
            NeedUpload = 0;
            DefalutValueType = DefaultValueTypeEnum.无默认值;
            DefaultValueForm = "";
            RelationRatio = "";

            ProductNo = "";
            OrderNo = "";
        }

        /// <summary>
        /// 虚拟的一个电容包编码项点
        /// </summary>
        /// <returns></returns>
        public static ExamineDataItem SimulateCodeItem()
        {
            return new ExamineDataItem()
            {
                ItemName = "电容包编码"
            };
        }

        //public static List<ExamineDataItem> ParseListFromIPTList(int wStadardID, List<IPTItem> wIPTList, string wItemGroup)
        //{
        //    List<ExamineDataItem> wResultList = new List<ExamineDataItem>();

        //    try
        //    {
        //        foreach (IPTItem wIPTItem in wIPTList)
        //        {
        //            if (wIPTItem.ItemType == 4)
        //            {
        //                continue;
        //            }

        //            ExamineDataItem wExamineDataItem = new ExamineDataItem();

        //            wExamineDataItem.ItemGroup = wItemGroup;

        //            wExamineDataItem.StandardID = wStadardID;
        //            wExamineDataItem.ItemID = (int)wIPTItem.ID;
        //            wExamineDataItem.ItemName = wIPTItem.Text;
        //            wExamineDataItem.OrderID = wIPTItem.OrderID;
        //            wExamineDataItem.TypeEnum = (ItemTypeEnum)wIPTItem.StandardType;
        //            wExamineDataItem.ItemDescription = wIPTItem.Details;
        //            if (wExamineDataItem.TypeEnum == ItemTypeEnum.单选)
        //            {
        //                wIPTItem.ValueSource = wIPTItem.ValueSource == null ? new List<string>() : wIPTItem.ValueSource;
        //                wExamineDataItem.ItemDescription = string.Join(";", wIPTItem.ValueSource);
        //            }
        //            wExamineDataItem.Active = wIPTItem.Active;
        //            wExamineDataItem.ActiveText = (wIPTItem.Active == 1) ? "激活" : "关闭";
        //            wExamineDataItem.IsRequired = wIPTItem.IsWriteFill == 1;
        //            wExamineDataItem.IsLinkControl = wIPTItem.IsManufactorFill == 1;
        //            wExamineDataItem.LinkRows = wIPTItem.IsModalFill;
        //            wExamineDataItem.RelationParameter = wIPTItem.CheckPoint;
        //            wExamineDataItem.RelationRatio = wIPTItem.DefaultNumber;
        //            wExamineDataItem.IsEdit = wIPTItem.IsNumberFill == 1;
        //            wExamineDataItem.IsPrimaryKey = wIPTItem.IsPeriodChange == 1;
        //            wExamineDataItem.IsRepeat = wIPTItem.IsQuality == 1;
        //            wExamineDataItem.ValueLength = wIPTItem.OtherValue;
        //            wExamineDataItem.DefalutValueType = (DefaultValueTypeEnum)StringUtils.ParseInt(wIPTItem.DefaultManufactor);
        //            wExamineDataItem.DefaultValueForm = wIPTItem.DefaultModal;

        //            wResultList.Add(wExamineDataItem);
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

        //public static List<IPTValue> ParseToIPTValueList(List<ExamineDataItem> wDataSource)
        //{
        //    List<IPTValue> wResultList = new List<IPTValue>();

        //    try
        //    {
        //        foreach (ExamineDataItem wItem in wDataSource)
        //        {
        //            if (wItem.ItemName.Equals("合格") || wItem.ItemName.Equals("备注"))
        //                continue;

        //            IPTValue wIPTValue = new IPTValue();

        //            wIPTValue.StandardID = wItem.StandardID;
        //            wIPTValue.IPTItemID = wItem.ItemID;
        //            wIPTValue.IPTItemName = wItem.ItemName;
        //            wIPTValue.Value = wItem.ItemValue;
        //            wIPTValue.Remark = wItem.ItemDescription;
        //            wIPTValue.Result = 1; // 0-默认，1-合格，2-不合格
        //            wIPTValue.TaskID = wItem.SFCTaskStepID;
        //            wIPTValue.IPTMode = (int)IPTMode.生产自检;
        //            wIPTValue.ItemType = 2; // 2 - 项，4 - 组

        //            wIPTValue.Status = (int)OperateType.提交;

        //            wResultList.Add(wIPTValue);
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

        /// <summary>
        /// 数据检查：检查保存数据code是否为null或空、各项参数值是否为null
        /// </summary>
        /// <param name="wSaveDataList"></param>
        /// <returns></returns>
        public static string CheckSaveData(List<ExamineDataItem> wSaveDataList, out List<ExamineDataItem> wSaveDataFilterList)
        {
            string wErrorMsg = "";
            wSaveDataFilterList = new List<ExamineDataItem>();
            try
            {
                Dictionary<string, string> wDicError = new Dictionary<string, string>();

                foreach (ExamineDataItem wExamineDataItem in wSaveDataList)
                {
                    if (string.IsNullOrWhiteSpace(wExamineDataItem.Code) && !wDicError.ContainsKey(wExamineDataItem.ItemRowID))
                    {
                        wErrorMsg += string.Format("第{0}行Code值为空\r\n", wExamineDataItem.ItemRowID);
                        wDicError.Add(wExamineDataItem.ItemRowID, wExamineDataItem.ItemRowID);
                    }

                    if (string.IsNullOrWhiteSpace(wExamineDataItem.OrderNo) && !wDicError.ContainsKey(wExamineDataItem.ItemRowID))
                    {
                        wErrorMsg += string.Format("第{0}行OrderNo值为空\r\n", wExamineDataItem.ItemRowID);
                    }
                }

                if (wErrorMsg.Length > 0)
                {
                    return wErrorMsg;
                }
                // 按照工位和电容包编码分组
                Dictionary<string, Dictionary<string, List<ExamineDataItem>>> wGroupByStationAndCodeResult =
                    InspectionDataCollectTask.Instance.GroupDataByStationAndCode(wSaveDataList);

                foreach (string wStationID in wGroupByStationAndCodeResult.Keys)
                {
                    foreach (string wCode in wGroupByStationAndCodeResult[wStationID].Keys)
                    {
                        if (wGroupByStationAndCodeResult[wStationID][wCode].Count < 1)
                        {
                            continue;
                        }
                        //查询电容包编码对应数据
                        List<ExamineDataItem> wExamineDataItemDBList = new List<ExamineDataItem>();
                        //检查数据是否改变
                        bool wIsChange = ExamineDataItem.CheckValueChange(wGroupByStationAndCodeResult[wStationID][wCode], wExamineDataItemDBList);

                        if (wIsChange)
                        {
                            //提交自检单(此处提交不合适，此时还未弹窗人工确认是否保存)
                            //MyFunctionTool.SaveTaskIPT(wGroupByStationAndCodeResult[wStationID][wCode]);
                            wSaveDataFilterList.AddRange(wGroupByStationAndCodeResult[wStationID][wCode]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                throw ex;
            }
            return wErrorMsg;
        }
        /// <summary>
        /// 检查要保存数据与数据库数据是否一致
        /// </summary>
        /// <param name="wSaveList">要保存数据</param>
        /// <param name="wDBDataList">数据库存储数据</param>
        /// <returns></returns>
        public static bool CheckValueChange(List<ExamineDataItem> wSaveList, List<ExamineDataItem> wDBDataList)
        {
            bool wIsChage = false;
            try
            {
                if (wDBDataList == null || wDBDataList.Count < 1)
                    return true;

                foreach (ExamineDataItem wItem in wSaveList)
                {
                    ExamineDataItem wResult = wDBDataList.Find(p => p.ItemName.Equals(wItem.ItemName));
                    if (wResult != null)
                    {
                        if (!wItem.ItemValue.Equals(wResult.ItemValue))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
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

            return wIsChage;
        }
    }
}
