using Microsoft.Win32;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace iPlantMLM
{
    /// <summary>
    /// ManualEntryPage.xaml 的交互逻辑
    /// </summary>
    public partial class ManualEntry030Page : Page
    {
        public ManualEntry030Page()
        {
            InitializeComponent();

            InitializeForm();

            TB_WriteNo.Focus();
        }

        #region 全局变量
        //表格实例
        private MEDataGrid mMEDataGrid = new MEDataGrid();
        #endregion

        #region 初始化

        /// <summary>
        /// 渲染页面
        /// </summary>
        private void RenderPage()
        {
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();

                if (string.IsNullOrWhiteSpace(wConfig.CurrentPart) || wConfig.CurrentProduct <= 0)
                {
                    MessageBox.Show("当前工位或产品型号未设置!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //配置表格
                List<string> wPartsList = wConfig.CurrentPart.Split(',').ToList();
                List<int> wPartIDList = new List<int>();
                foreach (string wParts in wPartsList)
                    wPartIDList.Add(int.Parse(wParts));

                List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();
                wFPCPartList = wFPCPartList.FindAll(p => p.PartID < int.Parse(wConfig.CurrentPart.Split(',')[0])).ToList();
                wPartIDList.AddRange(wFPCPartList.Select(p => p.PartID).ToList());
                wPartIDList.OrderBy(p => p).ToList();

                mMEDataGrid.ConfigDataGrid(
                    0, wPartIDList, 0, wConfig.CurrentProduct, false);

                //添加到页面
                Scv_Grid.Content = mMEDataGrid;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                throw ex;
            }
        }
        private void InitializeForm()
        {
            try
            {
                this.mMEDataGrid.mSaveDataHandler += mMEDataGrid_mSaveDataHandler;

                //渲染页面
                this.RenderPage();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                MessageBox.Show("界面初始化失败。\n\n" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region 事件
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int wRow = 0;

                //wRow = Convert.ToInt32(this.Txt_Rows.Text.Trim().ToString());
                if (wRow <= 0)
                {
                    MessageBox.Show("行数设置错误:设置行数必须大于0", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                for (int i = 0; i < wRow; i++)
                {
                    mMEDataGrid.AppendRow(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("行数设置错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
             System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mMEDataGrid.ClearSelectRows();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
             System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("清空前请确认所有需要的数据均已保存。是否清空表格？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning) != MessageBoxResult.OK)
                    return;
                mMEDataGrid.ClearAll(true);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                MessageBox.Show("操作失败。\n\n" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Add:
                        this.mMEDataGrid_mSaveDataHandler();
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 保存
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 4001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                string[] wCurPartList = wConfig.CurrentPart.Split(',');
                foreach (string wItem in wCurPartList)
                {
                    if (!GUD.mLoginUser.PartPower.Split(',').ToList().Exists(p => p.Equals(wItem)))
                    {
                        MessageBox.Show("当前用户无当前工位授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                mMEDataGrid_mSaveDataHandler();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                MessageBox.Show("操作失败。\n\n" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void mMEDataGrid_mSaveDataHandler()
        {
            try
            {
                string wError = "";
                this.Lbl_SaveResult.Content = "";

                Dictionary<int, Dictionary<string, string>> wDicData = mMEDataGrid.GetAllData(out wError);
                if (!string.IsNullOrEmpty(wError))
                {
                    MessageBox.Show(wError, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (wDicData.Count < 1)
                {
                    MessageBox.Show("保存数据为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                foreach (int wKey in wDicData.Keys)
                {
                    if (wDicData[wKey] == null)
                    {
                        MessageBox.Show(string.Format("保存错误，第{0}行有数据未填写完整！", wKey), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                //校验数据
                foreach (int wRow in wDicData.Keys)
                {
                    Dictionary<string, string> wDic = wDicData[wRow];
                    foreach (string wTitle in wDic.Keys)
                    {
                        string wValue = wDic[wTitle];
                        if (string.IsNullOrWhiteSpace(wValue))
                        {
                            string wMsg = string.Format("保存失败，第【{0}】行数据未填写完整，【{1}】未填写!", wRow, wTitle);
                            MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    foreach (IPTStandard wIPTStandard in GUD.mIPTStandardList)
                    {
                        string wHead = "";
                        if (string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                            wHead = wIPTStandard.ItemName;
                        else
                            wHead = wIPTStandard.ItemName + "(" + wIPTStandard.UnitText + ")";

                        if (!wDic.ContainsKey(wHead))
                            continue;

                        string wValue = wDic[wHead];
                        switch ((StandardType)wIPTStandard.Type)
                        {
                            case StandardType.文本:
                                break;
                            case StandardType.单选:
                                break;
                            case StandardType.全开区间:
                            case StandardType.全包区间:
                            case StandardType.右包区间:
                            case StandardType.左包区间:
                            case StandardType.小于:
                            case StandardType.大于:
                            case StandardType.小于等于:
                            case StandardType.大于等于:
                            case StandardType.等于:
                                double wRes = 0;
                                if (!double.TryParse(wValue, out wRes))
                                {
                                    string wMsg = string.Format("保存失败，第【{0}】行数据有误，【{1}】输入值不合法!", wRow, wIPTStandard.ItemName);
                                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                                break;
                            case StandardType.多选:
                                break;
                            case StandardType.是否:
                                break;
                            default:
                                break;
                        }
                    }
                }
                //保存提示
                if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                //容量内阻测试工位-验证前面工序已填的数据是否合格,不合格提示不合格信息
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("6")))
                {
                    MyLoading wMyLoading = new MyLoading();
                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, exc) => IsQuality_DoWork(s, exc, wDicData);
                    wBW.RunWorkerCompleted += (s, exc) => IsQuality_RunWorkerCompleted(exc, wMyLoading, wDicData);
                    wBW.RunWorkerAsync();
                    wMyLoading.ShowDialog();
                }
                else
                {
                    MyLoading wMyLoading = new MyLoading();
                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wDicData);
                    wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading);
                    wBW.RunWorkerAsync();
                    wMyLoading.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                throw ex;
            }
        }
        private void Save_DoWork(object s, DoWorkEventArgs e, Dictionary<int, Dictionary<string, string>> wDicData)
        {
            try
            {
                int wErrorCode = 0;
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                List<string> wParts = wConfig.CurrentPart.Split(',').ToList();
                List<int> wPartIDList = new List<int>();
                foreach (string wPartStr in wParts)
                {
                    int wPartID = int.Parse(wPartStr);
                    wPartIDList.Add(wPartID);
                }
                List<IPTStandard> wIPTStandardList = GUD.mIPTStandardList.FindAll(p => wPartIDList.Exists(q => q == p.PartID)).ToList();

                foreach (int wRow in wDicData.Keys)
                {
                    Dictionary<string, string> wValueDic = wDicData[wRow];
                    string wCapacityCode = wValueDic["电容包编号"];
                    //若没有记录，则创建记录,首工位存模组信息
                    SFCModuleRecord wSFCModuleRecord = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByCode(wCapacityCode, out wErrorCode);
                    if (wSFCModuleRecord.ID <= 0 && wParts.Exists(p => p.Equals("1")))
                    {
                        string wSerialNumber = SFCModuleRecordDAO.Instance.SFC_CreateSerialNumber(out wErrorCode);
                        wSFCModuleRecord = new SFCModuleRecord();
                        wSFCModuleRecord.ID = 0;
                        wSFCModuleRecord.Gear = "";
                        wSFCModuleRecord.BarCode = "";
                        wSFCModuleRecord.CapacitorPackageNo = wCapacityCode;
                        wSFCModuleRecord.CreateID = GUD.mLoginUser.ID;
                        wSFCModuleRecord.CreateTime = DateTime.Now;
                        wSFCModuleRecord.CurrentPartID = int.Parse(wParts[wParts.Count - 1]);
                        wSFCModuleRecord.ModuleNumber = "";
                        wSFCModuleRecord.OnlineTime = DateTime.Now;
                        wSFCModuleRecord.SerialNumber = wSerialNumber;
                        wSFCModuleRecord.ShiftID = wShiftID;
                        wSFCModuleRecord.ProductID = wConfig.CurrentProduct;
                        wSFCModuleRecord.Active = 1;
                        wSFCModuleRecord.IsQuality = 1;
                        wSFCModuleRecord.Times = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacityCode, "", -1, "", -1, out wErrorCode).Count + 1;
                        wSFCModuleRecord.ID = SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);

                        //存特殊值
                        SpecialValueSave(wErrorCode, wConfig, wIPTStandardList, wValueDic, wSFCModuleRecord);

                        //依次存值
                        SaveValue(wErrorCode, wShiftID, wIPTStandardList, wValueDic, wCapacityCode);
                    }
                    else
                    {
                        //判断是修改还是保存
                        List<IPTBoolValue> wBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                        List<IPTTextValue> wIPTTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                        List<IPTNumberValue> wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                        //可修改
                        bool wIsUpdate = false;
                        foreach (IPTStandard wIPTStandard in wIPTStandardList)
                            if (wBoolValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTTextValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTNumberValueList.Exists(p => p.StandardID == wIPTStandard.ID))
                                wIsUpdate = true;
                        if (wIsUpdate)
                        {
                            //保存特别值，写死的
                            wErrorCode = SpecialValueSave(wErrorCode, wConfig, wIPTStandardList, wValueDic, wSFCModuleRecord);
                            //修改填写值
                            foreach (IPTStandard wIPTStandard in wIPTStandardList)
                            {
                                string wHead = "";
                                if (string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                                    wHead = wIPTStandard.ItemName;
                                else
                                    wHead = wIPTStandard.ItemName + "(" + wIPTStandard.UnitText + ")";
                                string wValue = wValueDic[wHead];
                                switch ((StandardType)wIPTStandard.Type)
                                {
                                    case StandardType.文本:
                                        IPTTextValue wIPTTextValue = wIPTTextValueList.Find(p => p.StandardID == wIPTStandard.ID);
                                        wIPTTextValue.Value = wValue;
                                        wIPTTextValue.CreateID = GUD.mLoginUser.ID;
                                        wIPTTextValue.CreateTime = DateTime.Now;
                                        wIPTTextValue.ShiftID = wShiftID;
                                        IPTTextValueDAO.Instance.IPT_SaveIPTTextValue(wIPTTextValue, out wErrorCode);
                                        break;
                                    case StandardType.单选:
                                        IPTBoolValue wIPTBoolValue = wBoolValueList.Find(p => p.StandardID == wIPTStandard.ID);
                                        wIPTBoolValue.Value = wValue.Equals("OK") ? 1 : 2;
                                        wIPTBoolValue.CreateID = GUD.mLoginUser.ID;
                                        wIPTBoolValue.CreateTime = DateTime.Now;
                                        wIPTBoolValue.ShiftID = wShiftID;
                                        IPTBoolValueDAO.Instance.IPT_SaveIPTBoolValue(wIPTBoolValue, out wErrorCode);
                                        break;
                                    case StandardType.全开区间:
                                    case StandardType.全包区间:
                                    case StandardType.右包区间:
                                    case StandardType.左包区间:
                                    case StandardType.小于:
                                    case StandardType.大于:
                                    case StandardType.小于等于:
                                    case StandardType.大于等于:
                                    case StandardType.等于:
                                        IPTNumberValue wIPTNumberValue = wIPTNumberValueList.Find(p => p.StandardID == wIPTStandard.ID);
                                        wIPTNumberValue.Value = double.Parse(wValue);
                                        wIPTNumberValue.CreateID = GUD.mLoginUser.ID;
                                        wIPTNumberValue.CreateTime = DateTime.Now;
                                        wIPTNumberValue.ShiftID = wShiftID;
                                        IPTNumberValueDAO.Instance.IPT_SaveIPTNumberValue(wIPTNumberValue, out wErrorCode);
                                        break;
                                    case StandardType.多选:
                                        break;
                                    case StandardType.是否:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            //判断是否合格，需要更新
                            List<SFCModuleRecord> wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacityCode, "", -1, "", -1, out wErrorCode);
                            wMRecordList = wMRecordList.OrderByDescending(p => p.ID).ToList();
                            int wIsQuality = 1;
                            wIsQuality = JudgeIsQuality(wIPTStandardList, wValueDic);
                            if (wIsQuality == 2)
                            {
                                wMRecordList[0].IsQuality = 2;
                                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                            }
                            else
                            {
                                wMRecordList[0].IsQuality = 1;
                                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                            }
                        }
                        else
                        {
                            //保存特别值，写死的
                            wErrorCode = SpecialValueSave(wErrorCode, wConfig, wIPTStandardList, wValueDic, wSFCModuleRecord);
                            //②保存当前工位信息
                            wSFCModuleRecord.CurrentPartID = wPartIDList[wPartIDList.Count - 1];
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                            //保存值
                            wErrorCode = SaveValue(wErrorCode, wShiftID, wIPTStandardList, wValueDic, wCapacityCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 特殊值保存
        /// </summary>
        private static int SpecialValueSave(int wErrorCode, MESConfig wConfig, List<IPTStandard> wIPTStandardList, Dictionary<string, string> wValueDic, SFCModuleRecord wSFCModuleRecord)
        {
            try
            {
                //电容包装配工位-绑定模组编码
                if (wConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("7")))
                {
                    foreach (IPTStandard wIPTStandard in wIPTStandardList)
                    {
                        if (wIPTStandard.ItemName.Equals("模组编码"))
                        {
                            wSFCModuleRecord.ModuleNumber = wValueDic["模组编码"];
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }
                    }
                }
                //模组检测记录档位信息
                if (wConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("6")))
                {
                    IPTStandard wRLStandard = GUD.mIPTStandardList.Find(p => p.ItemName.Contains("容量"));
                    string wRLValue = wValueDic["容量(F)"];
                    List<IPTCapacityGrading> wIPTCapacityGradingList = IPTCapacityGradingDAO.Instance.IPT_QueryIPTCapacityGradingList(wConfig.CurrentProduct, "", out wErrorCode);
                    double wRLValued = double.Parse(wRLValue);
                    if (wIPTCapacityGradingList.Exists(p => wRLValued >= p.LowerLimit && wRLValued <= p.UpLimit))
                    {
                        IPTCapacityGrading wIPTCapacityGrading = wIPTCapacityGradingList.Find(p => wRLValued >= p.LowerLimit && wRLValued <= p.UpLimit);
                        wSFCModuleRecord.Gear = wIPTCapacityGrading.Gear;
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                    }

                    //容量和直流内阻更新
                    string wNZValue = wValueDic["直流内阻(mΩ)"];
                    double wNZValued = double.Parse(wNZValue);
                    SFCModuleRecordDAO.Instance.SFC_UpdateRL(wSFCModuleRecord.ID, wRLValued, wNZValued);
                }
                //包装记录条码信息和末工位下线时间
                if (wConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("11")))
                {
                    foreach (IPTStandard wIPTStandard in wIPTStandardList)
                    {
                        if (wIPTStandard.ItemName.Equals("装箱条形码"))
                        {
                            wSFCModuleRecord.BarCode = wValueDic["装箱条形码"];
                            wSFCModuleRecord.OfflineTime = DateTime.Now;
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }
                        if (wIPTStandard.ItemName.Equals("装托条形码"))
                        {
                            wSFCModuleRecord.TrustBarCode = wValueDic["装托条形码"];
                            wSFCModuleRecord.OfflineTime = DateTime.Now;
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wErrorCode;
        }

        /// <summary>
        /// 依次存值
        /// </summary>
        private static int SaveValue(int wErrorCode, int wShiftID, List<IPTStandard> wIPTStandardList, Dictionary<string, string> wValueDic, string wCapacityCode)
        {
            try
            {
                //根据电容包编号获取最新的记录流水号
                List<SFCModuleRecord> wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacityCode, "", -1, "", -1, out wErrorCode);
                wMRecordList = wMRecordList.OrderByDescending(p => p.ID).ToList();
                //依次保存填写值
                int wIsQuality = 1;
                wIsQuality = JudgeIsQuality(wIPTStandardList, wValueDic);
                if (wIsQuality == 2)
                {
                    wMRecordList[0].IsQuality = 2;
                    SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                }
                else
                {
                    wMRecordList[0].IsQuality = 1;
                    SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                }

                foreach (IPTStandard wIPTStandard in wIPTStandardList)
                {
                    string wHead = "";
                    if (string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                        wHead = wIPTStandard.ItemName;
                    else
                        wHead = wIPTStandard.ItemName + "(" + wIPTStandard.UnitText + ")";
                    string wValue = wValueDic[wHead];
                    switch ((StandardType)wIPTStandard.Type)
                    {
                        case StandardType.文本:
                            IPTTextValue wIPTTextValue = new IPTTextValue();
                            wIPTTextValue.ID = 0;
                            wIPTTextValue.CreateID = GUD.mLoginUser.ID;
                            wIPTTextValue.CreateTime = DateTime.Now;
                            wIPTTextValue.SerialNumber = wMRecordList[0].SerialNumber;
                            wIPTTextValue.ShiftID = wShiftID;
                            wIPTTextValue.StandardID = wIPTStandard.ID;
                            wIPTTextValue.PartID = wIPTStandard.PartID;
                            wIPTTextValue.Value = wValue;
                            IPTTextValueDAO.Instance.IPT_SaveIPTTextValue(wIPTTextValue, out wErrorCode);
                            break;
                        case StandardType.全开区间:
                        case StandardType.全包区间:
                        case StandardType.右包区间:
                        case StandardType.左包区间:
                        case StandardType.小于:
                        case StandardType.大于:
                        case StandardType.小于等于:
                        case StandardType.大于等于:
                        case StandardType.等于:
                            IPTNumberValue wIPTNumberValue = new IPTNumberValue();
                            wIPTNumberValue.ID = 0;
                            wIPTNumberValue.CreateID = GUD.mLoginUser.ID;
                            wIPTNumberValue.CreateTime = DateTime.Now;
                            wIPTNumberValue.SerialNumber = wMRecordList[0].SerialNumber;
                            wIPTNumberValue.ShiftID = wShiftID;
                            wIPTNumberValue.StandardID = wIPTStandard.ID;
                            wIPTNumberValue.PartID = wIPTStandard.PartID;
                            wIPTNumberValue.Value = double.Parse(wValue);
                            IPTNumberValueDAO.Instance.IPT_SaveIPTNumberValue(wIPTNumberValue, out wErrorCode);
                            break;
                        case StandardType.单选:
                        case StandardType.是否:
                            IPTBoolValue wIPTBoolValue = new IPTBoolValue();
                            wIPTBoolValue.ID = 0;
                            wIPTBoolValue.CreateID = GUD.mLoginUser.ID;
                            wIPTBoolValue.CreateTime = DateTime.Now;
                            wIPTBoolValue.SerialNumber = wMRecordList[0].SerialNumber;
                            wIPTBoolValue.ShiftID = wShiftID;
                            wIPTBoolValue.StandardID = wIPTStandard.ID;
                            wIPTBoolValue.PartID = wIPTStandard.PartID;
                            wIPTBoolValue.Value = wValue.Equals("OK") ? 1 : 2;
                            IPTBoolValueDAO.Instance.IPT_SaveIPTBoolValue(wIPTBoolValue, out wErrorCode);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wErrorCode;
        }
        /// <summary>
        /// 判断是否合格
        /// </summary>
        private static int JudgeIsQuality(List<IPTStandard> wIPTStandardList, Dictionary<string, string> wValueDic)
        {
            int wResult = 1;
            try
            {
                foreach (IPTStandard wIPTStandard in wIPTStandardList)
                {
                    string wHead = "";
                    if (string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                        wHead = wIPTStandard.ItemName;
                    else
                        wHead = wIPTStandard.ItemName + "(" + wIPTStandard.UnitText + ")";
                    string wValue = wValueDic[wHead];
                    switch ((StandardType)wIPTStandard.Type)
                    {
                        case StandardType.文本:
                            break;
                        case StandardType.单选:
                            if (wValue.Equals("NG"))
                            {
                                wResult = 2;
                                return wResult;
                            }
                            break;
                        case StandardType.全开区间:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) <= wIPTStandard.LowerLimit || double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.全包区间:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) < wIPTStandard.LowerLimit || double.Parse(wValue) > wIPTStandard.UpperLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.右包区间:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) <= wIPTStandard.LowerLimit || double.Parse(wValue) > wIPTStandard.UpperLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.左包区间:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) < wIPTStandard.LowerLimit || double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.小于:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.大于:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) <= wIPTStandard.LowerLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.小于等于:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) > wIPTStandard.UpperLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.大于等于:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) < wIPTStandard.LowerLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.等于:
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) != wIPTStandard.LowerLimit)
                                {
                                    wResult = 2;
                                    return wResult;
                                }
                            }
                            break;
                        case StandardType.多选:
                            break;
                        case StandardType.是否:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
        private void Save_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                mMEDataGrid.ClearAll(true);
                TB_WriteNo.Text = "";
                MessageBox.Show("保存成功，请到历史记录查看!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void IsQuality_DoWork(object s, DoWorkEventArgs e, Dictionary<int, Dictionary<string, string>> wDicData)
        {
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                int wCurPartID = int.Parse(wConfig.CurrentPart.Split(',')[0]);
                List<int> wPartIDList = GUD.mPartIDList.FindAll(p => p < wCurPartID);

                string wMsg = "";
                foreach (int wRow in wDicData.Keys)
                {
                    Dictionary<string, string> wDic = wDicData[wRow];
                    string wCapacityCode = wDic["电容包编号"];

                    List<IPTStandard> wIPTStandardList = GUD.mIPTStandardList.FindAll(p => wPartIDList.Exists(q => q == p.PartID)).ToList();
                    foreach (IPTStandard wIPTStandard in wIPTStandardList)
                    {
                        string wHead = "";
                        if (string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                            wHead = wIPTStandard.ItemName;
                        else
                            wHead = wIPTStandard.ItemName + "(" + wIPTStandard.UnitText + ")";
                        string wValue = wDic[wHead];
                        switch ((StandardType)wIPTStandard.Type)
                        {
                            case StandardType.文本:
                                break;
                            case StandardType.单选:
                                if (wValue.Equals("NG"))
                                {
                                    wMsg = string.Format("第【{3}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值为NG!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wRow);
                                    e.Result = wMsg;
                                    return;
                                }
                                break;
                            case StandardType.全开区间:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) <= wIPTStandard.LowerLimit || double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<n<{4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.全包区间:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) < wIPTStandard.LowerLimit || double.Parse(wValue) > wIPTStandard.UpperLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<=n<={4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.右包区间:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) <= wIPTStandard.LowerLimit || double.Parse(wValue) > wIPTStandard.UpperLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<n<={4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.左包区间:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) < wIPTStandard.LowerLimit || double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<=n<{4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.小于:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【n<{4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.大于:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) <= wIPTStandard.LowerLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<n】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.小于等于:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) > wIPTStandard.UpperLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【n<={4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.大于等于:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) < wIPTStandard.LowerLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<=n】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.等于:
                                if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                                {
                                    if (double.Parse(wValue) != wIPTStandard.LowerLimit)
                                    {
                                        wMsg = string.Format("第【{7}】行数据验证不通过，【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【n={3}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCapacityCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString(), wRow);
                                        e.Result = wMsg;
                                        return;
                                    }
                                }
                                break;
                            case StandardType.多选:
                                break;
                            case StandardType.是否:
                                break;
                            default:
                                break;
                        }
                    }

                }
                e.Result = wMsg;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void IsQuality_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, Dictionary<int, Dictionary<string, string>> wDicData)
        {
            try
            {
                wMyLoading.Close();

                string wMsg = e.Result as string;
                if (!string.IsNullOrWhiteSpace(wMsg))
                {
                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MyLoading wMyLoading1 = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wDicData);
                wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading1);
                wBW.RunWorkerAsync();
                wMyLoading1.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 电容包或模组扫码
        private void TB_WriteNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string wText = TB_WriteNo.Text;
                if (wText.Contains("\r\n"))
                {
                    TB_WriteNo.Text = TB_WriteNo.Text.Replace("\r\n", "");
                    if (string.IsNullOrWhiteSpace(TB_WriteNo.Text))
                    {
                        MessageBox.Show("电容包编号或模组编码不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        TB_WriteNo.Text = "";
                        return;
                    }
                    String wCode = TB_WriteNo.Text;

                    MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();

                    //判断当前产品的电容包编码是否合格
                    FPCProduct wProduct = FPCProductDAO.Instance.GetProductList().Find(p => p.ProductID == wConfig.CurrentProduct);
                    if (int.Parse(wConfig.CurrentPart.Split(',')[0]) <= 7 && !string.IsNullOrWhiteSpace(wProduct.PackagePrefix) && !TB_WriteNo.Text.Contains(wProduct.PackagePrefix))
                    {
                        MessageBox.Show("当前产品的电容包编号前缀为：" + wProduct.PackagePrefix, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        TB_WriteNo.Text = "";
                        return;
                    }

                    //若有放行权限，可不检查直接放行
                    if (MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 4002))
                    {
                        //根据电容包或模组编码查询最新的流水号
                        MyLoading wMyLoading = new MyLoading();
                        BackgroundWorker wBW = new BackgroundWorker();
                        wBW.DoWork += (s, exc) => TB_WriteNo_DoWork(s, exc, wCode);
                        wBW.RunWorkerCompleted += (s, exc) => TB_WriteNo_RunWorkerCompleted(exc, wMyLoading, wCode);
                        wBW.RunWorkerAsync();
                        wMyLoading.ShowDialog();
                    }
                    else
                    {
                        //检查是否合格
                        MyLoading wMyLoading = new MyLoading();
                        BackgroundWorker wBW = new BackgroundWorker();
                        wBW.DoWork += (s, exc) => EnterIsQuality_DoWork(s, exc, wCode);
                        wBW.RunWorkerCompleted += (s, exc) => EnterIsQuality_RunWorkerCompleted(exc, wMyLoading, wCode);
                        wBW.RunWorkerAsync();
                        wMyLoading.ShowDialog();
                    }

                    //MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                    //if (wConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("6") || p.Equals("7") || p.Equals("8") || p.Equals("9") || p.Equals("10") || p.Equals("11")))
                    //{
                    //    MyLoading wMyLoading = new MyLoading();
                    //    BackgroundWorker wBW = new BackgroundWorker();
                    //    wBW.DoWork += (s, exc) => EnterIsQuality_DoWork(s, exc, wCode);
                    //    wBW.RunWorkerCompleted += (s, exc) => EnterIsQuality_RunWorkerCompleted(exc, wMyLoading, wCode);
                    //    wBW.RunWorkerAsync();
                    //    wMyLoading.ShowDialog();
                    //}
                    //else
                    //{
                    //    //根据电容包或模组编码查询最新的流水号
                    //    MyLoading wMyLoading = new MyLoading();
                    //    BackgroundWorker wBW = new BackgroundWorker();
                    //    wBW.DoWork += (s, exc) => TB_WriteNo_DoWork(s, exc, wCode);
                    //    wBW.RunWorkerCompleted += (s, exc) => TB_WriteNo_RunWorkerCompleted(exc, wMyLoading, wCode);
                    //    wBW.RunWorkerAsync();
                    //    wMyLoading.ShowDialog();
                    //}
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private SFCModuleRecord mHisModuleRecord;
        private void EnterIsQuality_DoWork(object s, DoWorkEventArgs e, string wCode)
        {
            try
            {
                string wMsg = "";
                int wErrorCode = 0;
                SFCModuleRecord wSFCModuleRecord = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByCode(wCode, out wErrorCode);
                if (wSFCModuleRecord.ID <= 0)
                {
                    e.Result = wMsg;
                    return;
                }

                //查询测试数据
                List<IPTTextValue> wTextValueList = new List<IPTTextValue>();
                List<IPTNumberValue> wIPTNumberValueList = new List<IPTNumberValue>();
                List<IPTBoolValue> wIPTBoolValueList = new List<IPTBoolValue>();
                wTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                wIPTBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);

                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                int wCurPartID = int.Parse(wConfig.CurrentPart.Split(',')[0]);
                List<int> wPartIDList = GUD.mPartIDList.FindAll(p => p < wCurPartID);

                List<IPTStandard> wIPTStandardList = GUD.mIPTStandardList.FindAll(p => wPartIDList.Exists(q => q == p.PartID)).ToList();
                mHisModuleRecord = wSFCModuleRecord;
                foreach (IPTStandard wIPTStandard in wIPTStandardList)
                {
                    string wValue = "";
                    switch ((StandardType)wIPTStandard.Type)
                    {
                        case StandardType.文本:
                            break;
                        case StandardType.单选:
                            wValue = wIPTBoolValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value == 1 ? "OK" : "NG";
                            if (wValue.Equals("NG"))
                            {
                                wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值为NG!", wIPTStandard.PartName, wIPTStandard.ItemName, wSFCModuleRecord.CapacitorPackageNo);
                                e.Result = wMsg;
                                return;
                            }
                            break;
                        case StandardType.全开区间:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) <= wIPTStandard.LowerLimit || double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合格，标准范围为【{3}{5}<n<{4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.全包区间:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) < wIPTStandard.LowerLimit || double.Parse(wValue) > wIPTStandard.UpperLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<=n<={4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.右包区间:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) <= wIPTStandard.LowerLimit || double.Parse(wValue) > wIPTStandard.UpperLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<n<={4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.左包区间:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) < wIPTStandard.LowerLimit || double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<=n<{4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.小于:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) >= wIPTStandard.UpperLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【n<{4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.大于:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) <= wIPTStandard.LowerLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<n】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.小于等于:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) > wIPTStandard.UpperLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【n<={4}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.大于等于:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) < wIPTStandard.LowerLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}<=n】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.等于:
                            wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            if (wIPTStandard.LowerLimit != 0 || wIPTStandard.UpperLimit != 0)
                            {
                                if (double.Parse(wValue) != wIPTStandard.LowerLimit)
                                {
                                    wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【n={3}{5}】，实际填写值为【{6}{5}】!", wIPTStandard.PartName, wIPTStandard.ItemName, wCode, wIPTStandard.LowerLimit.ToString(), wIPTStandard.UpperLimit.ToString(), wIPTStandard.UnitText, wValue.ToString());
                                    e.Result = wMsg;
                                    return;
                                }
                            }
                            break;
                        case StandardType.多选:
                            break;
                        case StandardType.是否:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void EnterIsQuality_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, string wCode)
        {
            try
            {
                wMyLoading.Close();

                string wMsg = e.Result as string;
                if (!string.IsNullOrWhiteSpace(wMsg))
                {
                    MessageBox.Show(wMsg + "，若需重新检测，请相关人员进行查重标记!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    TB_WriteNo.Text = "";
                    return;
                }
                else
                {
                    //根据电容包或模组编码查询最新的流水号
                    MyLoading wMyLoading1 = new MyLoading();
                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, exc) => TB_WriteNo_DoWork(s, exc, wCode);
                    wBW.RunWorkerCompleted += (s, exc) => TB_WriteNo_RunWorkerCompleted(exc, wMyLoading1, wCode);
                    wBW.RunWorkerAsync();
                    wMyLoading1.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void TB_WriteNo_DoWork(object s, DoWorkEventArgs e, string wCode)
        {
            try
            {
                int wErrorCode = 0;
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                //①查询序列号
                SFCModuleRecord wSFCModuleRecord = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByCode(wCode, out wErrorCode);
                //②查询填写值
                List<IPTTextValue> wTextValueList = new List<IPTTextValue>();
                List<IPTNumberValue> wIPTNumberValueList = new List<IPTNumberValue>();
                List<IPTBoolValue> wIPTBoolValueList = new List<IPTBoolValue>();
                if (wSFCModuleRecord.ID > 0 && GUD.mPartIDList.Max() < wSFCModuleRecord.CurrentPartID)
                {
                    List<object> wObjectList1 = new List<object>();
                    wObjectList1.Add(string.Format("【{0}】该电容包或模组数据已录入!", wCode));
                    e.Result = wObjectList1;
                    return;
                }
                if (wSFCModuleRecord.ID > 0)
                {
                    wTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                    wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                    wIPTBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                    //判断前工位是否都已录入
                    List<string> wParts = wMESConfig.CurrentPart.Split(',').ToList();
                    List<int> wPartIDList = new List<int>();
                    foreach (string wItem in wParts)
                        wPartIDList.Add(int.Parse(wItem));
                    List<int> wPrePartIDList = GUD.mPartIDList.FindAll(p => wPartIDList.All(q => q > p)).ToList();
                    wPrePartIDList.RemoveAll(p => !GUD.mIPTStandardList.Exists(q => q.PartID == p));
                    foreach (int wPartID in wPrePartIDList)
                    {
                        if (!wTextValueList.Exists(p => p.PartID == wPartID) && !wIPTNumberValueList.Exists(p => p.PartID == wPartID) && !wIPTBoolValueList.Exists(p => p.PartID == wPartID))
                        {
                            List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();

                            List<object> wObjectList1 = new List<object>();
                            wObjectList1.Add(string.Format("【{0}】工位未录入【{1}】数据!", wFPCPartList.Find(p => p.PartID == wPartID).PartName, wCode));
                            e.Result = wObjectList1;
                            return;
                        }
                    }
                }


                //①首工位未录入
                if (!wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("1")) && wSFCModuleRecord.ID <= 0)
                {
                    List<object> wObjectList1 = new List<object>();
                    wObjectList1.Add(string.Format("【{0}】首工位未录入该电容包或模组数据!", wCode));
                    e.Result = wObjectList1;
                    return;
                }

                List<int> wCurrentPartIDList = new List<int>();
                foreach (string wItem in wMESConfig.CurrentPart.Split(','))
                    wCurrentPartIDList.Add(int.Parse(wItem));

                //③组合Value集合返回
                Dictionary<int, string> wValueDic = new Dictionary<int, string>();
                wValueDic.Add(0, "");
                if (wSFCModuleRecord.ID > 0)
                    wValueDic.Add(1, wSFCModuleRecord.CapacitorPackageNo);
                else
                    wValueDic.Add(1, wCode);

                int wIndex = 2;
                //档位
                if (wCurrentPartIDList[0] == 6)
                {
                    wValueDic.Add(2, wSFCModuleRecord.Gear);
                    wIndex = 3;
                }

                for (int i = 0; i < GUD.mIPTStandardList.Count; i++)
                {
                    IPTStandard wIPTStandard = GUD.mIPTStandardList[i];

                    if (!wCurrentPartIDList.Exists(p => p == 11) && !wCurrentPartIDList.Exists(p => p == wIPTStandard.PartID))
                        continue;

                    string wValue = "";
                    switch ((StandardType)wIPTStandard.Type)
                    {
                        case StandardType.文本:
                            if (!string.IsNullOrWhiteSpace(wIPTStandard.DefaultValue))
                                wValue = wIPTStandard.DefaultValue;
                            if (wTextValueList.Exists(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID))
                                wValue = wTextValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value;
                            break;
                        case StandardType.全开区间:
                        case StandardType.全包区间:
                        case StandardType.右包区间:
                        case StandardType.左包区间:
                        case StandardType.小于:
                        case StandardType.大于:
                        case StandardType.小于等于:
                        case StandardType.大于等于:
                        case StandardType.等于:
                            if (!string.IsNullOrWhiteSpace(wIPTStandard.DefaultValue))
                                wValue = wIPTStandard.DefaultValue;
                            if (wIPTNumberValueList.Exists(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID))
                                wValue = wIPTNumberValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value.ToString();
                            break;
                        case StandardType.单选:
                        case StandardType.是否:
                            if (!string.IsNullOrWhiteSpace(wIPTStandard.DefaultValue))
                                wValue = wIPTStandard.DefaultValue;
                            if (wIPTBoolValueList.Exists(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID))
                                wValue = wIPTBoolValueList.Find(p => p.PartID == wIPTStandard.PartID && p.StandardID == wIPTStandard.ID).Value == 1 ? "OK" : "NG";
                            break;
                        default:
                            break;
                    }

                    //查询单体电压检测结果
                    if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("4")) && wIPTStandard.ItemName.Contains("单体电压检测"))
                    {
                        int wTestResult = SFCModuleRecordDAO.Instance.QueryWithstandVoltageTestByCode(wCode);
                        if (wTestResult <= 0)
                        {
                            List<object> wObjectList1 = new List<object>();
                            wObjectList1.Add(string.Format("【{0}】该电容包单体电压未检测!", wCode));
                            e.Result = wObjectList1;
                            return;
                        }
                        wValue = wTestResult == 1 ? "OK" : "NG";
                    }
                    //查询容量和直流内阻
                    else if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("6")) && wIPTStandard.ItemName.Contains("容量"))
                    {
                        double wCapacity = 0;
                        double wInternalResistance = 0;
                        SFCModuleRecordDAO.Instance.QueryCapacityAndInternalResistanceByCode(wCode, out wCapacity, out wInternalResistance);
                        if (wCapacity <= 0)
                        {
                            List<object> wObjectList1 = new List<object>();
                            wObjectList1.Add(string.Format("【{0}】该电容包容量未检测!", wCode));
                            e.Result = wObjectList1;
                            return;
                        }
                        wValue = wCapacity.ToString();
                    }
                    else if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("6")) && wIPTStandard.ItemName.Contains("直流内阻"))
                    {
                        double wCapacity = 0;
                        double wInternalResistance = 0;
                        SFCModuleRecordDAO.Instance.QueryCapacityAndInternalResistanceByCode(wCode, out wCapacity, out wInternalResistance);
                        if (wInternalResistance <= 0)
                        {
                            List<object> wObjectList1 = new List<object>();
                            wObjectList1.Add(string.Format("【{0}】该电容包直流内阻未检测!", wCode));
                            e.Result = wObjectList1;
                            return;
                        }
                        wValue = wInternalResistance.ToString();
                    }
                    //if (string.IsNullOrWhiteSpace(wValue) && wIPTStandard.ItemName.Equals("静置时间"))
                    //    wValue = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    wValueDic.Add(wIndex, wValue);
                    wIndex++;
                }

                List<object> wObjectList = new List<object>();
                wObjectList.Add("");
                wObjectList.Add(wValueDic);
                e.Result = wObjectList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void TB_WriteNo_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, string wCode)
        {
            try
            {
                wMyLoading.Close();

                List<object> wObjectList = e.Result as List<object>;
                string wMsg = wObjectList[0] as string;
                if (!string.IsNullOrWhiteSpace(wMsg))
                {
                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    TB_WriteNo.Text = "";
                    return;
                }

                string wError = "";
                Dictionary<int, Dictionary<string, string>> wDicData = mMEDataGrid.GetAllData(out wError);
                foreach (int wRow in wDicData.Keys)
                {
                    foreach (string wTitle in wDicData[wRow].Keys)
                    {
                        string wValue = wDicData[wRow][wTitle];
                        if (wCode.Equals(wValue))
                        {
                            MessageBox.Show("该编码已添加到表格!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            TB_WriteNo.Text = "";
                            return;
                        }
                    }
                }

                Dictionary<int, string> wValueDic = wObjectList[1] as Dictionary<int, string>;
                mMEDataGrid.AppendRow(wValueDic);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 历史记录查询
        private void Btn_History_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HistoryWindow1 wHistoryWindow1 = new HistoryWindow1();
                wHistoryWindow1.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 参数设置
        private void Cmd_Active_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParamSetting wParamSetting = new ParamSetting();
                wParamSetting.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 刷新数据
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TB_WriteNo.Text))
                {
                    MessageBox.Show("请扫描电容包编号!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //查询容量和直流内阻
                double wCapacity;
                double wInternalResistance;
                SFCModuleRecordDAO.Instance.QueryCapacityAndInternalResistanceByCode(TB_WriteNo.Text, out wCapacity, out wInternalResistance);

                if (wCapacity <= 0)
                {
                    MessageBox.Show(string.Format("【{0}】该电容包容量未检测!", TB_WriteNo.Text), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (wInternalResistance <= 0)
                {
                    MessageBox.Show(string.Format("【{0}】该电容包直流内阻未检测!", TB_WriteNo.Text), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                List<IPTStandard> wStandard = GUD.mIPTStandardList;
                //①找到输入电容包编号的所在行
                int wRow = 0;
                foreach (UIElement wUIElement in mMEDataGrid.Grd_Body.Children)
                {
                    if (wUIElement is TextBox)
                    {
                        TextBox wTextBox = wUIElement as TextBox;
                        if (wTextBox.Text.Equals(TB_WriteNo.Text))
                            wRow = (int)wTextBox.GetValue(Grid.RowProperty);
                    }
                }
                //②找到“容量”的列
                //③找到“直流内阻”的列
                int wRLCol = 0;
                int wNZCol = 0;
                for (int i = 0; i < wStandard.Count; i++)
                {
                    if (wStandard[i].ItemName.Contains("容量"))
                        wRLCol = i + 2;
                    if (wStandard[i].ItemName.Contains("直流内阻"))
                        wNZCol = i + 2;
                }
                //④赋值即可
                foreach (UIElement wUIElement in mMEDataGrid.Grd_Body.Children)
                {
                    if (wUIElement is TextBox)
                    {
                        TextBox wTextBox = wUIElement as TextBox;
                        int wCurRow = (int)wTextBox.GetValue(Grid.RowProperty);
                        int wCurCol = (int)wTextBox.GetValue(Grid.ColumnProperty);
                        //容量
                        if (wCurRow == wRow && wCurCol == wRLCol)
                            wTextBox.Text = wCapacity.ToString();
                        //直流内阻
                        else if (wCurRow == wRow && wCurCol == wNZCol)
                            wTextBox.Text = wInternalResistance.ToString();
                    }
                }

                MessageBox.Show("刷新成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 查看文件
        private void Btn_See_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //①获取当前工位集合
                int wErrorCode = 0;
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                List<string> wList = wConfig.CurrentPart.Split(',').ToList();
                //②遍历显示文件查看
                bool wHas = false;
                foreach (string wItem in wList)
                {
                    int wPartID = int.Parse(wItem);
                    List<SFCUploadSOP> wSOPList = SFCUploadSOPDAO.Instance.SFC_QuerySFCUploadSOPList(-1, wConfig.CurrentProduct, wPartID, -1, 1, out wErrorCode);
                    if (wSOPList.Count > 0)
                        wHas = true;
                    if (wSOPList.Count <= 0)
                        continue;
                    SOPFileLookWindow wUI = new SOPFileLookWindow(wSOPList[0]);
                    wUI.Show();
                }
                if (!wHas)
                    MessageBox.Show("当前产品规格当前工位暂无作业指导文件!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 数据导入
        private void Btn_Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 4001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                string[] wCurPartList = wConfig.CurrentPart.Split(',');
                foreach (string wItem in wCurPartList)
                {
                    if (!GUD.mLoginUser.PartPower.Split(',').ToList().Exists(p => p.Equals(wItem)))
                    {
                        MessageBox.Show("当前用户无当前工位授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                OpenFileDialog wOpenFileDialog = new OpenFileDialog();
                wOpenFileDialog.Filter = "Excel (.xls,.xlsx,.XLS,.XLSX)|*.xls;*.xlsx;*.XLS;*.XLSX";
                wOpenFileDialog.Multiselect = false;
                if (wOpenFileDialog.ShowDialog() != true)
                    return;

                if (MessageBox.Show("确认导入吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                string wFilePath = wOpenFileDialog.FileName;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Import_DoWork(s, exc, wFilePath);
                wBW.RunWorkerCompleted += (s, exc) => Import_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Import_DoWork(object s, DoWorkEventArgs e, string wFilePath)
        {
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                int wPartID = int.Parse(wConfig.CurrentPart.Split(',')[0]);
                int wErrorCode = 0;
                List<string> wParts = wConfig.CurrentPart.Split(',').ToList();
                List<int> wPartIDList = new List<int>();
                foreach (string wPartStr in wParts)
                    wPartIDList.Add(int.Parse(wPartStr));
                List<IPTStandard> wIPTStandardList = GUD.mIPTStandardList.FindAll(p => wPartIDList.Exists(q => q == p.PartID)).ToList();
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

                DataTable wDataTable = ExcelTool.Instance.ExcelToDatatableWithTitle(wFilePath);
                int wRow = 1;
                foreach (DataRow wRowData in wDataTable.Rows)
                {
                    if (wRow == 1)
                    {
                        wRow++;
                        continue;
                    }
                    //编码
                    string wCode = wRowData.ItemArray[0].ToString();

                    Dictionary<string, string> wValueDic = new Dictionary<string, string>();
                    for (int i = 0; i < wRowData.ItemArray.Length; i++)
                        wValueDic.Add(wDataTable.Rows[0].ItemArray[i].ToString(), wRowData.ItemArray[i].ToString());

                    if (wPartID == 1)
                    {
                        //判断数据是否已导入
                        SFCModuleRecord wSFCModuleRecord = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByCode(wCode, out wErrorCode);
                        if (wSFCModuleRecord != null && wSFCModuleRecord.ID > 0)
                        {
                            //首工位数据已录入
                            e.Result = string.Format("第【{0}】行数据导入失败，【{1}】该工位已录入该电容包数据!", wRow, wCode);
                            return;
                        }

                        string wSerialNumber = SFCModuleRecordDAO.Instance.SFC_CreateSerialNumber(out wErrorCode);
                        wSFCModuleRecord = new SFCModuleRecord();
                        wSFCModuleRecord.ID = 0;
                        wSFCModuleRecord.Gear = "";
                        wSFCModuleRecord.BarCode = "";
                        wSFCModuleRecord.CapacitorPackageNo = wCode;
                        wSFCModuleRecord.CreateID = GUD.mLoginUser.ID;
                        wSFCModuleRecord.CreateTime = DateTime.Now;
                        wSFCModuleRecord.CurrentPartID = int.Parse(wParts[wParts.Count - 1]);
                        wSFCModuleRecord.ModuleNumber = "";
                        wSFCModuleRecord.OnlineTime = DateTime.Now;
                        wSFCModuleRecord.SerialNumber = wSerialNumber;
                        wSFCModuleRecord.ShiftID = wShiftID;
                        wSFCModuleRecord.ProductID = wConfig.CurrentProduct;
                        wSFCModuleRecord.Active = 1;
                        wSFCModuleRecord.IsQuality = 1;
                        wSFCModuleRecord.Times = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCode, "", -1, "", -1, out wErrorCode).Count + 1;
                        wSFCModuleRecord.ID = SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);

                        //存特殊值
                        SpecialValueSave(wErrorCode, wConfig, wIPTStandardList, wValueDic, wSFCModuleRecord);

                        //依次存值
                        SaveValue(wErrorCode, wShiftID, wIPTStandardList, wValueDic, wCode);
                    }
                    else
                    {
                        //判断前工位是否已录入
                        int wPrePartID = GUD.mIPTStandardList.FindAll(p => p.PartID < wPartID).Max(p => p.PartID);
                        bool wCheckResult = SFCModuleRecordDAO.Instance.SFC_QueryPreIsDone(wCode, wPrePartID, out wErrorCode);
                        if (!wCheckResult)
                        {
                            List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();
                            //前工位未录入数据
                            e.Result = string.Format("第【{0}】行数据导入失败,【{2}】【{1}】工位数据未录入!", wRow, wFPCPartList.Find(p => p.PartID == wPrePartID).PartName, wCode);
                            return;
                        }

                        SFCModuleRecord wSFCModuleRecord = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByCode(wCode, out wErrorCode);

                        int wIsQuality = 1;
                        wIsQuality = JudgeIsQuality(wIPTStandardList, wValueDic);
                        if (wIsQuality == 2)
                        {
                            wSFCModuleRecord.IsQuality = 2;
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }
                        else
                        {
                            wSFCModuleRecord.IsQuality = 1;
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }

                        //判断是修改还是保存
                        List<IPTBoolValue> wBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                        List<IPTTextValue> wIPTTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                        List<IPTNumberValue> wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                        //可修改
                        bool wIsUpdate = false;
                        foreach (IPTStandard wIPTStandard in wIPTStandardList)
                            if (wBoolValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTTextValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTNumberValueList.Exists(p => p.StandardID == wIPTStandard.ID))
                                wIsUpdate = true;
                        if (wIsUpdate)
                        {
                            //保存特别值，写死的
                            wErrorCode = SpecialValueSave(wErrorCode, wConfig, wIPTStandardList, wValueDic, wSFCModuleRecord);
                            //修改填写值
                            foreach (IPTStandard wIPTStandard in wIPTStandardList)
                            {
                                string wHead = "";
                                if (string.IsNullOrWhiteSpace(wIPTStandard.UnitText))
                                    wHead = wIPTStandard.ItemName;
                                else
                                    wHead = wIPTStandard.ItemName + "(" + wIPTStandard.UnitText + ")";
                                string wValue = wValueDic[wHead];
                                switch ((StandardType)wIPTStandard.Type)
                                {
                                    case StandardType.文本:
                                        IPTTextValue wIPTTextValue = wIPTTextValueList.Find(p => p.StandardID == wIPTStandard.ID);
                                        wIPTTextValue.Value = wValue;
                                        wIPTTextValue.CreateID = GUD.mLoginUser.ID;
                                        wIPTTextValue.CreateTime = DateTime.Now;
                                        wIPTTextValue.ShiftID = wShiftID;
                                        IPTTextValueDAO.Instance.IPT_SaveIPTTextValue(wIPTTextValue, out wErrorCode);
                                        break;
                                    case StandardType.单选:
                                        IPTBoolValue wIPTBoolValue = wBoolValueList.Find(p => p.StandardID == wIPTStandard.ID);
                                        wIPTBoolValue.Value = wValue.Equals("OK") ? 1 : 2;
                                        wIPTBoolValue.CreateID = GUD.mLoginUser.ID;
                                        wIPTBoolValue.CreateTime = DateTime.Now;
                                        wIPTBoolValue.ShiftID = wShiftID;
                                        IPTBoolValueDAO.Instance.IPT_SaveIPTBoolValue(wIPTBoolValue, out wErrorCode);
                                        break;
                                    case StandardType.全开区间:
                                    case StandardType.全包区间:
                                    case StandardType.右包区间:
                                    case StandardType.左包区间:
                                    case StandardType.小于:
                                    case StandardType.大于:
                                    case StandardType.小于等于:
                                    case StandardType.大于等于:
                                    case StandardType.等于:
                                        IPTNumberValue wIPTNumberValue = wIPTNumberValueList.Find(p => p.StandardID == wIPTStandard.ID);
                                        wIPTNumberValue.Value = double.Parse(wValue);
                                        wIPTNumberValue.CreateID = GUD.mLoginUser.ID;
                                        wIPTNumberValue.CreateTime = DateTime.Now;
                                        wIPTNumberValue.ShiftID = wShiftID;
                                        IPTNumberValueDAO.Instance.IPT_SaveIPTNumberValue(wIPTNumberValue, out wErrorCode);
                                        break;
                                    case StandardType.多选:
                                        break;
                                    case StandardType.是否:
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            //保存特别值，写死的
                            wErrorCode = SpecialValueSave(wErrorCode, wConfig, wIPTStandardList, wValueDic, wSFCModuleRecord);
                            //②保存当前工位信息
                            wSFCModuleRecord.CurrentPartID = wPartIDList[wPartIDList.Count - 1];
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                            //保存值
                            wErrorCode = SaveValue(wErrorCode, wShiftID, wIPTStandardList, wValueDic, wCode);
                        }
                    }
                }

                e.Result = "";
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Import_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                string wMsg = e.Result as string;
                if (string.IsNullOrWhiteSpace(wMsg))
                    MessageBox.Show("导入成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
