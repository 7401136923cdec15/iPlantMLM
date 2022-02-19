using Microsoft.Win32;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
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
    public partial class ManualEntryPage : Page
    {
        public ManualEntryPage()
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

                //显示装箱或装托信息
                ShowZXOrZTBox();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                MessageBox.Show("界面初始化失败。\n\n" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowZXOrZTBox()
        {
            try
            {
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();

                if (wMESConfig.CurrentPart.Contains("7") || wMESConfig.CurrentPart.Contains("8") || wMESConfig.CurrentPart.Contains("9"))
                    CB_JF.Visibility = Visibility.Visible;

                //装箱
                if (wMESConfig.CurrentPart.Contains("11"))
                {
                    this.Grd_Main.RowDefinitions.Clear();

                    RowDefinition wRowDefinition = new RowDefinition();
                    GridLength wGridLength = new GridLength(80, GridUnitType.Pixel);
                    wRowDefinition.Height = wGridLength;
                    this.Grd_Main.RowDefinitions.Add(wRowDefinition);

                    wRowDefinition = new RowDefinition();
                    wGridLength = new GridLength(14d, GridUnitType.Star);
                    wRowDefinition.Height = wGridLength;
                    this.Grd_Main.RowDefinitions.Add(wRowDefinition);

                    SP_ZX.Visibility = Visibility.Visible;
                }
                //装托
                else if (wMESConfig.CurrentPart.Contains("12"))
                {
                    this.Grd_Main.RowDefinitions.Clear();

                    RowDefinition wRowDefinition = new RowDefinition();
                    GridLength wGridLength = new GridLength(80, GridUnitType.Pixel);
                    wRowDefinition.Height = wGridLength;
                    this.Grd_Main.RowDefinitions.Add(wRowDefinition);

                    wRowDefinition = new RowDefinition();
                    wGridLength = new GridLength(14d, GridUnitType.Star);
                    wRowDefinition.Height = wGridLength;
                    this.Grd_Main.RowDefinitions.Add(wRowDefinition);

                    SP_ZX.Visibility = Visibility.Visible;
                    SP_ZX.Margin = new Thickness(5, 5, 0, 0);
                    SP_ZT.Visibility = Visibility.Visible;
                    SP_ZT.Margin = new Thickness(5, 5, 0, 0);
                    SP_Main.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
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
                mXHList = new List<string>();
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

                //ctrl+s保存
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
                    this.mMEDataGrid_mSaveDataHandler();
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

                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();

                //电容包装配工位需要查重模组编码
                if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("7")))
                {
                    foreach (int wRow in wDicData.Keys)
                    {
                        Dictionary<string, string> wDic = wDicData[wRow];
                        //电容包编号
                        string wCapNo = wDic["电容包编号"];
                        //模组编码
                        string wModuleNo = wDic["模组编码"];

                        bool wCheckResult = SFCModuleNoDAO.Instance.CheckRepeat(wCapNo, wModuleNo);
                        if (wCheckResult)
                        {
                            MessageBox.Show(string.Format("第【{1}】行数据错误，【{0}】该模组编码已使用!", wModuleNo, wRow), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                //保存提示
                if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                //容量内阻测试工位-验证前面工序已填的数据是否合格,不合格提示不合格信息

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
                        if (wIPTStandard.ItemName.Contains("装箱信息"))
                        {
                            wSFCModuleRecord.BarCode = wValueDic["装箱信息"];
                            wSFCModuleRecord.OfflineTime = DateTime.Now;
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }
                        if (wIPTStandard.ItemName.Contains("装托信息"))
                        {
                            wSFCModuleRecord.TrustBarCode = wValueDic["装托信息"];
                            wSFCModuleRecord.OfflineTime = DateTime.Now;
                            SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                        }
                    }
                }
                //包装记录条码信息和末工位下线时间
                if (wConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("12")))
                {
                    foreach (IPTStandard wIPTStandard in wIPTStandardList)
                    {
                        if (wIPTStandard.ItemName.Contains("装托信息"))
                        {
                            wSFCModuleRecord.TrustBarCode = wValueDic["装托信息"];
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

                    if (!wValueDic.ContainsKey(wHead))
                        continue;

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
                mXHList = new List<string>();

                TB_WriteNo.Text = "";
                TB_ZXNo.Text = "";
                TB_ZTNo.Text = "";
                MessageBox.Show("保存成功，请到历史记录查看!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                //输出模组编码打印文件
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("9")))
                {
                    bool wIsJF = CB_JF.IsChecked.Value;

                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, ex) => BW_PrintBQ_DoWork(s, ex, wIsJF);
                    wBW.RunWorkerCompleted += wBWBQ_RunWorkerCompleted;
                    wBW.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void wBWBQ_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                bool wResult = (bool)e.Result;
                if (wResult)
                    MessageBox.Show("模组标签打印文件已输出到桌面，请及时打印!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("模组标签打印文件输出失败，文件被占用!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void BW_PrintBQ_DoWork(object sender, DoWorkEventArgs e, bool wIsJF)
        {
            try
            {
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                //创建文件夹
                string wDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string wFoler = string.Format("{0}/MES模组标签/{1}", wDesktopPath, wShiftID);
                if (!Directory.Exists(wFoler))
                    Directory.CreateDirectory(wFoler);
                FileStream wFileStream = File.Create(wFoler + "/模组标签.xlsx");

                List<List<string>> wRowList = SFCModuleRecordDAO.Instance.QueryPrintList(wShiftID, wIsJF);

                ExcelTool.Instance.ExportToExcel(wFileStream, wRowList, "模组标签");

                e.Result = true;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                e.Result = false;
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
        private SFCModuleRecord mHisModuleRecord;
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

                    MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();

                    //判断当前产品的电容包编码是否合格
                    FPCProduct wProduct = FPCProductDAO.Instance.GetProductList().Find(p => p.ProductID == wConfig.CurrentProduct);
                    if (int.Parse(wConfig.CurrentPart.Split(',')[0]) <= 7 && !string.IsNullOrWhiteSpace(wProduct.PackagePrefix) && !TB_WriteNo.Text.Contains(wProduct.PackagePrefix))
                    {
                        MessageBox.Show("当前产品的电容包编号前缀为：" + wProduct.PackagePrefix, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        TB_WriteNo.Text = "";
                        return;
                    }

                    //替换金风前缀为空
                    if (!string.IsNullOrWhiteSpace(wProduct.BarCodePrefix))
                        TB_WriteNo.Text = TB_WriteNo.Text.Replace(wProduct.BarCodePrefix, "");

                    String wCode = TB_WriteNo.Text;

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
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
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

                    //移除三串测试工位检测
                    wPrePartIDList.Remove(10);

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
                if (wCurrentPartIDList[0] == 6 || wCurrentPartIDList[0] == 11 || wCurrentPartIDList[0] == 12)
                {
                    wValueDic.Add(2, wSFCModuleRecord.Gear);
                    wIndex = 3;
                }

                //模组编码组合
                if (wCurrentPartIDList[0] >= 8 && wCurrentPartIDList[0] <= 10 && !wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("11")))
                {
                    wValueDic.Add(2, wSFCModuleRecord.ModuleNumber);
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

                //输出模组编码打印文件
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("7")))
                {
                    if (MessageBox.Show("确定创建新模组编码？", "提示", MessageBoxButton.OKCancel,
                        MessageBoxImage.Question) != MessageBoxResult.OK)
                        return;

                    bool wIsJF = CB_JF.IsChecked.Value;

                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, ex) => BW_Print_DoWork(s, ex, wIsJF);
                    wBW.RunWorkerCompleted += wBW_RunWorkerCompleted;
                    wBW.RunWorkerAsync();
                }

                //输出模组标签打印文件
                //if (wMESConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("8") || p.Equals("9")))
                //{
                //    bool wIsJF = CB_JF.IsChecked.Value;

                //    BackgroundWorker wBW = new BackgroundWorker();
                //    wBW.DoWork += (s, ex) => BW_PrintBQ_DoWork(s, ex, wIsJF);
                //    wBW.RunWorkerCompleted += wBWBQ_RunWorkerCompleted;
                //    wBW.RunWorkerAsync();
                //}
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MessageBox.Show("模组编码打印文件已输出到桌面，请打印模组编码!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void BW_Print_DoWork(object sender, DoWorkEventArgs e, bool wIsJF)
        {
            try
            {
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int wErrorCode = 0;
                List<SFCModuleNo> wSFCModuleNoList = SFCModuleNoDAO.Instance.SFC_QuerySFCModuleNoList(-1, "", wShiftID, out wErrorCode);

                if (wIsJF)
                    wSFCModuleNoList = wSFCModuleNoList.FindAll(p => p.ModuleNo.Contains("CRRC")).ToList();
                else
                    wSFCModuleNoList = wSFCModuleNoList.FindAll(p => !p.ModuleNo.Contains("CRRC")).ToList();

                int wNewID = wSFCModuleNoList.Count + 1;

                string wModelNo = string.Format("CRRC{0}{1}", wShiftID, wNewID.ToString("000"));

                if (!wIsJF)
                    wModelNo = GetModelNo(wNewID);

                SFCModuleNo wSFCModuleNo = new SFCModuleNo();
                wSFCModuleNo.CreateTime = DateTime.Now;
                wSFCModuleNo.ID = 0;
                wSFCModuleNo.ModuleNo = wModelNo;
                wSFCModuleNo.ShiftID = wShiftID;
                SFCModuleNoDAO.Instance.SFC_SaveSFCModuleNo(wSFCModuleNo, out wErrorCode);

                List<SFCModuleNo> wNewList = SFCModuleNoDAO.Instance.SFC_QuerySFCModuleNoList(-1, "", wShiftID, out wErrorCode);
                //创建文件夹
                string wDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string wFoler = string.Format("{0}/MES模组编码/{1}", wDesktopPath, wShiftID);
                if (!Directory.Exists(wFoler))
                    Directory.CreateDirectory(wFoler);
                FileStream wFileStream = File.Create(wFoler + "/模组编码.xlsx");

                List<List<string>> wRowList = new List<List<string>>();
                foreach (SFCModuleNo wItem in wNewList)
                {
                    List<string> wColList = new List<string>();
                    wColList.Add(wItem.ModuleNo);
                    wRowList.Add(wColList);
                }

                ExcelTool.Instance.ExportToExcel(wFileStream, wRowList, "模组编码");
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 根据邵宇提供的模组编码规则生成模组编码
        /// </summary>
        private string GetModelNo(int wNewID)
        {
            string wResult = "";
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                List<FPCProduct> wFPCProductList = FPCProductDAO.Instance.GetProductList();
                FPCProduct wFPCProduct = wFPCProductList.Find(p => p.ProductID == wConfig.CurrentProduct);
                //①第一层  产品生产阶段代码
                string wCode1 = wFPCProduct.ProductCode;
                //②第二层  内部代码，与产品码相同
                string wCode2 = "";
                if (wFPCProduct.ProductName.Contains("160V10F"))
                    wCode2 = "J0";
                else if (wFPCProduct.ProductName.Contains("160V12F"))
                    wCode2 = "JG";
                //③第三层  年份代码，A代表2019，后面以此类推
                string wCode3 = "";
                int wFix = DateTime.Now.Year - 2019;
                char wA = 'A';
                int wAscii = (int)wA;
                wAscii = wAscii + wFix;
                wCode3 = ((char)wAscii).ToString();
                //④第四层  月份代码，用16进制数表示，1,2,3,4,5,6,7,8,9,A
                string wCode4 = "";
                wCode4 = DateTime.Now.Month.ToString("X");
                //⑤第五层  日期代码，用两位数字表示
                string wCode5 = "";
                wCode5 = DateTime.Now.Day.ToString("00");
                //⑥第六层  产品流水号，用四位数字表示
                string wCode6 = wNewID.ToString("0000");
                //⑦第七层  特殊标记，特殊标记为模组型号的尾缀
                string wCode7 = wFPCProduct.Model.Substring(wFPCProduct.Model.Length - 1);

                wResult = string.Format("{0}{1}{2}{3}{4}{5}{6}", wCode1, wCode2, wCode3, wCode4, wCode5, wCode6, wCode7);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
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

                    if (string.IsNullOrWhiteSpace(wCode))
                    {
                        wRow++;
                        continue;
                    }

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

        #region 装箱回车事件
        private void TB_ZXNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string wText = TB_ZXNo.Text;
                if (wText.Contains("\r\n"))
                {
                    TB_ZXNo.Text = TB_ZXNo.Text.Replace("\r\n", "");
                    if (string.IsNullOrWhiteSpace(TB_ZXNo.Text))
                    {
                        MessageBox.Show("装箱信息不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        TB_ZXNo.Text = "";
                        return;
                    }

                    MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                    if (wConfig.CurrentPart.Contains("11"))
                    {
                        //赋值所有装箱信息
                        List<IPTStandard> wStandard = GUD.mIPTStandardList;
                        for (int wRow = 1; wRow < mMEDataGrid.Grd_Body.Children.Count; wRow++)
                        {
                            //②找到“容量”的列
                            //③找到“直流内阻”的列
                            int wRLCol = 0;
                            for (int i = 0; i < wStandard.Count; i++)
                            {
                                if (wStandard[i].ItemName.Contains("装箱信息"))
                                    wRLCol = i + 3;
                            }
                            //④赋值即可
                            foreach (UIElement wUIElement in mMEDataGrid.Grd_Body.Children)
                            {
                                if (wUIElement is TextBox)
                                {
                                    TextBox wTextBox = wUIElement as TextBox;
                                    int wCurRow = (int)wTextBox.GetValue(Grid.RowProperty);
                                    int wCurCol = (int)wTextBox.GetValue(Grid.ColumnProperty);
                                    //装箱信息
                                    if (wCurRow == wRow && wCurCol == wRLCol)
                                        wTextBox.Text = TB_ZXNo.Text;
                                }
                            }
                        }
                    }
                    else if (wConfig.CurrentPart.Contains("12"))
                    {
                        //根据装箱信息查找模组列表
                        string wZXText = TB_ZXNo.Text;
                        if (mXHList.Exists(p => p.Equals(wZXText)))
                        {
                            MessageBox.Show("该箱号已添加!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        MyLoading wMyLoading = new MyLoading();
                        BackgroundWorker wBW = new BackgroundWorker();
                        wBW.DoWork += (s, exc) => ZX_DoWork(s, exc, wZXText);
                        wBW.RunWorkerCompleted += (s, exc) => ZX_RunWorkerCompleted(exc, wMyLoading);
                        wBW.RunWorkerAsync();
                        wMyLoading.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void ZX_DoWork(object s, DoWorkEventArgs e, string wZXText)
        {
            try
            {
                int wErrorCode = 0;
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();

                DateTime wSTime = new DateTime(2000, 1, 1);
                DateTime wETime = new DateTime(3000, 1, 1);
                List<SFCModuleRecord> wList = SFCModuleRecordDAO.Instance.SFC_QueryHistoryList(wSTime, wETime, "", wZXText, "", -1, "");

                List<Dictionary<int, string>> wDicList = new List<Dictionary<int, string>>();
                foreach (SFCModuleRecord wSFCModuleRecord in wList)
                {
                    //②查询填写值
                    List<IPTTextValue> wTextValueList = new List<IPTTextValue>();
                    List<IPTNumberValue> wIPTNumberValueList = new List<IPTNumberValue>();
                    List<IPTBoolValue> wIPTBoolValueList = new List<IPTBoolValue>();

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

                        //移除三串测试工位检测
                        wPrePartIDList.Remove(10);
                    }

                    List<int> wCurrentPartIDList = new List<int>();
                    foreach (string wItem in wMESConfig.CurrentPart.Split(','))
                        wCurrentPartIDList.Add(int.Parse(wItem));

                    //③组合Value集合返回
                    Dictionary<int, string> wValueDic = new Dictionary<int, string>();
                    wValueDic.Add(0, "");
                    wValueDic.Add(1, wSFCModuleRecord.CapacitorPackageNo);

                    int wIndex = 2;
                    //档位
                    if (wCurrentPartIDList[0] == 6 || wCurrentPartIDList[0] == 11 || wCurrentPartIDList[0] == 12)
                    {
                        wValueDic.Add(2, wSFCModuleRecord.Gear);
                        wIndex = 3;
                    }

                    for (int i = 0; i < GUD.mIPTStandardList.Count; i++)
                    {
                        IPTStandard wIPTStandard = GUD.mIPTStandardList[i];

                        if (!wCurrentPartIDList.Exists(p => p == 11 || p == 12) && !wCurrentPartIDList.Exists(p => p == wIPTStandard.PartID))
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

                        wValueDic.Add(wIndex, wValue);
                        wIndex++;
                    }
                    wDicList.Add(wValueDic);
                }

                e.Result = wDicList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        public List<string> mXHList = new List<string>();

        private void ZX_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<Dictionary<int, string>> wDicList = e.Result as List<Dictionary<int, string>>;

                if (wDicList.Count > 0)
                {
                    mXHList.Add(TB_ZXNo.Text);
                    //mMEDataGrid.ClearAll(true);
                    //TB_ZTNo.Focus();
                }
                else
                {
                    MessageBox.Show("未查询到装箱数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    TB_ZXNo.Text = "";
                    return;
                }

                foreach (Dictionary<int, string> wValueDic in wDicList)
                    mMEDataGrid.AppendRow(wValueDic);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 装托回车事件
        private void TB_ZTNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string wText = TB_ZTNo.Text;
                if (wText.Contains("\r\n"))
                {
                    TB_ZTNo.Text = TB_ZTNo.Text.Replace("\r\n", "");
                    if (string.IsNullOrWhiteSpace(TB_ZTNo.Text))
                    {
                        MessageBox.Show("装托信息不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        TB_ZTNo.Text = "";
                        return;
                    }

                    //赋值所有装托信息
                    List<IPTStandard> wStandard = GUD.mIPTStandardList;
                    for (int wRow = 1; wRow < mMEDataGrid.Grd_Body.Children.Count; wRow++)
                    {
                        //②找到“容量”的列
                        //③找到“直流内阻”的列
                        int wRLCol = 0;
                        for (int i = 0; i < wStandard.Count; i++)
                        {
                            if (wStandard[i].ItemName.Contains("装托信息"))
                                wRLCol = i + 3;
                        }
                        //④赋值即可
                        foreach (UIElement wUIElement in mMEDataGrid.Grd_Body.Children)
                        {
                            if (wUIElement is TextBox)
                            {
                                TextBox wTextBox = wUIElement as TextBox;
                                int wCurRow = (int)wTextBox.GetValue(Grid.RowProperty);
                                int wCurCol = (int)wTextBox.GetValue(Grid.ColumnProperty);
                                //装箱信息
                                if (wCurRow == wRow && wCurCol == wRLCol)
                                    wTextBox.Text = TB_ZTNo.Text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
