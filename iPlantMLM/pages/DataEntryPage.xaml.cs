using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iPlantMLM
{
    /// <summary>
    /// DataEntryPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataEntryPage : Page
    {
        public DataEntryPage()
        {
            InitializeComponent();
        }

        #region 页面加载
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TB_CapacitorPackageNo.Focus();

                //加载表头
                LoadHead();

                //渲染左侧表格
                LoadTable();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 加载表格
        private MESConfig mMESConfig;
        private void LoadTable()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => LoadTable_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => LoadTable_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void LoadTable_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                mMESConfig = wConfig;
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int wErrorCode = 0;
                List<string> wPartsList = wConfig.CurrentPart.Split(',').ToList();
                List<IPTBoolValue> wIPTBoolValueList = new List<IPTBoolValue>();
                List<IPTNumberValue> wIPTNumberValueList = new List<IPTNumberValue>();
                List<IPTTextValue> wIPTTextValueList = new List<IPTTextValue>();
                foreach (string wParts in wPartsList)
                {
                    int wPartID = int.Parse(wParts);
                    wIPTBoolValueList.AddRange(IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, "", -1, wShiftID, wPartID, out wErrorCode));
                    wIPTNumberValueList.AddRange(IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, "", -1, wShiftID, wPartID, out wErrorCode));
                    wIPTTextValueList.AddRange(IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, "", -1, wShiftID, wPartID, out wErrorCode));
                }
                List<FPCPart> wPartList = FPCPartDAO.Instance.GetPartList();
                wPartList = wPartList.FindAll(p => p.PartID < int.Parse(wPartsList[0])).ToList();
                foreach (FPCPart wFPCPart in wPartList)
                {
                    wIPTBoolValueList.AddRange(IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, "", -1, wShiftID, wFPCPart.PartID, out wErrorCode));
                    wIPTNumberValueList.AddRange(IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, "", -1, wShiftID, wFPCPart.PartID, out wErrorCode));
                    wIPTTextValueList.AddRange(IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, "", -1, wShiftID, wFPCPart.PartID, out wErrorCode));
                }
                //查询不同的测试流水号
                List<string> wSerialNumberList = new List<string>();
                foreach (IPTBoolValue wIPTBoolValue in wIPTBoolValueList)
                {
                    if (wSerialNumberList.Exists(p => p.Equals(wIPTBoolValue.SerialNumber)))
                        continue;
                    wSerialNumberList.Add(wIPTBoolValue.SerialNumber);
                }
                foreach (IPTNumberValue wIPTNumberValue in wIPTNumberValueList)
                {
                    if (wSerialNumberList.Exists(p => p.Equals(wIPTNumberValue.SerialNumber)))
                        continue;
                    wSerialNumberList.Add(wIPTNumberValue.SerialNumber);
                }
                foreach (IPTTextValue wIPTTextValue in wIPTTextValueList)
                {
                    if (wSerialNumberList.Exists(p => p.Equals(wIPTTextValue.SerialNumber)))
                        continue;
                    wSerialNumberList.Add(wIPTTextValue.SerialNumber);
                }

                List<SFCModuleRecord> wSFCModuleRecordList = new List<SFCModuleRecord>();
                foreach (string wSerialNumber in wSerialNumberList)
                    wSFCModuleRecordList.AddRange(SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, wSerialNumber, "", "", -1, "", -1, out wErrorCode));

                //获取标准
                List<IPTStandard> wList = new List<IPTStandard>();
                foreach (string wPartIDs in wPartsList)
                {
                    int wPartID = int.Parse(wPartIDs);
                    if (wPartID <= 0)
                        continue;
                    wList.AddRange(IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wConfig.CurrentProduct, -1, -1, wPartID, -1, 1, out wErrorCode));
                }
                foreach (FPCPart wFPCPart in wPartList)
                {
                    wList.AddRange(IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wConfig.CurrentProduct, -1, -1, wFPCPart.PartID, -1, 1, out wErrorCode));
                }
                wList = wList.OrderBy(p => p.PartID).ThenBy(p => p.OrderID).ToList();
                //返回数据整理
                List<object> wObjectList = new List<object>();
                wObjectList.Add(wSFCModuleRecordList);
                wObjectList.Add(wIPTBoolValueList);
                wObjectList.Add(wIPTNumberValueList);
                wObjectList.Add(wIPTTextValueList);
                wObjectList.Add(wList);
                e.Result = wObjectList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadTable_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<object> wObjectList = e.Result as List<object>;
                List<SFCModuleRecord> wSFCModuleRecordList = wObjectList[0] as List<SFCModuleRecord>;
                List<IPTBoolValue> wIPTBoolValueList = wObjectList[1] as List<IPTBoolValue>;
                List<IPTNumberValue> wIPTNumberValueList = wObjectList[2] as List<IPTNumberValue>;
                List<IPTTextValue> wIPTTextValueList = wObjectList[3] as List<IPTTextValue>;
                List<IPTStandard> wIPTStandardList = wObjectList[4] as List<IPTStandard>;

                //开始循环渲染
                SP_ValueList.Children.Clear();
                wSFCModuleRecordList = wSFCModuleRecordList.OrderByDescending(p => p.CreateTime).ToList();
                foreach (SFCModuleRecord wSFCModuleRecord in wSFCModuleRecordList)
                {
                    InputValueUC wVUC = new InputValueUC(wSFCModuleRecord, wIPTStandardList, wIPTBoolValueList, wIPTNumberValueList, wIPTTextValueList);
                    wVUC.MouseLeftButtonUp += (s, ex) => InputValueUCClick(wSFCModuleRecord, wIPTStandardList, wIPTBoolValueList, wIPTNumberValueList, wIPTTextValueList);
                    SP_ValueList.Children.Add(wVUC);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 表格行单击事件
        /// </summary>
        private void InputValueUCClick(SFCModuleRecord wSFCModuleRecord, List<IPTStandard> wIPTStandardList, List<IPTBoolValue> wIPTBoolValueList, List<IPTNumberValue> wIPTNumberValueList, List<IPTTextValue> wIPTTextValueList)
        {
            try
            {
                //①电容包编号
                TB_CapacitorPackageNo.Text = wSFCModuleRecord.CapacitorPackageNo;
                if (wIPTStandardList.Count > 0 && wIPTStandardList[wIPTStandardList.Count - 1].PartID > 7)
                    TB_CapacitorPackageNo.Text = wSFCModuleRecord.ModuleNumber;
                //②动态值
                foreach (InputUC wInputUC in SP_DynamicInput.Children)
                {
                    switch ((StandardType)wInputUC.mIPTStandard.Type)
                    {
                        case StandardType.文本:
                            if (wIPTTextValueList.Exists(p => p.StandardID == wInputUC.mIPTStandard.ID && p.SerialNumber == wSFCModuleRecord.SerialNumber))
                            {
                                string wValue = wIPTTextValueList.Find(p => p.StandardID == wInputUC.mIPTStandard.ID && p.SerialNumber == wSFCModuleRecord.SerialNumber).Value;
                                wInputUC.SetMyValue(wValue);
                            }
                            else
                                wInputUC.SetMyValue("");
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
                            if (wIPTNumberValueList.Exists(p => p.StandardID == wInputUC.mIPTStandard.ID && p.SerialNumber == wSFCModuleRecord.SerialNumber))
                            {
                                double wValue = wIPTNumberValueList.Find(p => p.StandardID == wInputUC.mIPTStandard.ID && p.SerialNumber == wSFCModuleRecord.SerialNumber).Value;
                                wInputUC.SetMyValue(wValue.ToString());
                            }
                            else
                                wInputUC.SetMyValue("");
                            break;
                        case StandardType.单选:
                        case StandardType.是否:
                            if (wIPTBoolValueList.Exists(p => p.StandardID == wInputUC.mIPTStandard.ID && p.SerialNumber == wSFCModuleRecord.SerialNumber))
                            {
                                int wValue = wIPTBoolValueList.Find(p => p.StandardID == wInputUC.mIPTStandard.ID && p.SerialNumber == wSFCModuleRecord.SerialNumber).Value;
                                wInputUC.SetMyValue(wValue.ToString());
                            }
                            else
                                wInputUC.SetMyValue("");
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
        #endregion

        private void LoadHead()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => LoadHead_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => LoadHead_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void LoadHead_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                int wErrorCode = 0;
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                mMESConfig = wMESConfig;
                string[] wParts = wMESConfig.CurrentPart.Split(',');
                List<IPTStandard> wList = new List<IPTStandard>();

                List<FPCPart> wPartList = FPCPartDAO.Instance.GetPartList();
                wPartList = wPartList.FindAll(p => p.PartID < int.Parse(wParts[0])).ToList();
                foreach (FPCPart wFPCPart in wPartList)
                {
                    wList.AddRange(IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wMESConfig.CurrentProduct, -1, -1, wFPCPart.PartID, -1, 1, out wErrorCode));
                }

                List<IPTStandard> wInputIPTStandardList = new List<IPTStandard>();
                foreach (string wPartIDs in wParts)
                {
                    int wPartID = int.Parse(wPartIDs);
                    if (wPartID <= 0)
                        continue;
                    List<IPTStandard> wTempList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wMESConfig.CurrentProduct, -1, -1, wPartID, -1, 1, out wErrorCode);
                    wList.AddRange(wTempList);
                    wInputIPTStandardList.AddRange(wTempList);
                }

                wList = wList.OrderBy(p => p.PartID).ThenBy(p => p.OrderID).ToList();
                wInputIPTStandardList = wInputIPTStandardList.OrderBy(p => p.PartID).ThenBy(p => p.OrderID).ToList();

                List<object> wObjectList = new List<object>();
                wObjectList.Add(wList);
                wObjectList.Add(wInputIPTStandardList);
                e.Result = wObjectList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void LoadHead_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<object> wObjectList = e.Result as List<object>;
                List<IPTStandard> wList = wObjectList[0] as List<IPTStandard>;
                List<IPTStandard> wInputIPTStandardList = wObjectList[1] as List<IPTStandard>;

                InputHeadUC wUC = new InputHeadUC(wList);
                Grid_Head.Children.Add(wUC);

                SP_DynamicInput.Children.Clear();
                foreach (IPTStandard wIPTStandard in wInputIPTStandardList)
                {
                    InputUC wInputUC = new InputUC(wIPTStandard);
                    wInputUC.DelReturn += () => wInputUC_DelReturn(wIPTStandard);
                    SP_DynamicInput.Children.Add(wInputUC);
                }

                if (int.Parse(mMESConfig.CurrentPart.Split(',')[0]) > 7)
                {
                    TB_Title.Text = "模组编号:";
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        void wInputUC_DelReturn(IPTStandard wIPTStandard)
        {
            try
            {
                bool wFlag = false;
                foreach (InputUC wInputUC in SP_DynamicInput.Children)
                {
                    if (wInputUC.mIPTStandard.OrderID > wIPTStandard.OrderID && (wInputUC.mIPTStandard.Type == 1 || wInputUC.mIPTStandard.Type == 2))
                    {
                        wInputUC.MyFocus();
                        return;
                    }
                }
                if (!wFlag)
                {
                    Cmd_Save_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 保存
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 4001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //数据验证
                if (string.IsNullOrWhiteSpace(TB_CapacitorPackageNo.Text))
                {
                    MessageBox.Show("电容包编号填写值错误!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                foreach (InputUC wInputUC in SP_DynamicInput.Children)
                {
                    if (wInputUC.mIPTStandard.Type == 1 || wInputUC.mIPTStandard.Type == 2)
                    {
                        string wWriteValue = wInputUC.GetWriteValue();
                        if (string.IsNullOrWhiteSpace(wWriteValue))
                        {
                            MessageBox.Show(wInputUC.mIPTStandard.ItemName + "填写值错误!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (wInputUC.mIPTStandard.Type == 2)
                        {
                            double wTemp = 0;
                            if (!double.TryParse(wWriteValue, out wTemp))
                            {
                                MessageBox.Show(wInputUC.mIPTStandard.ItemName + "填写值错误!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                }

                string wCapacitorPackageNo = TB_CapacitorPackageNo.Text;

                //容量内阻测试工位-验证前面工序已填的数据是否合格,不合格提示不合格信息
                if (mMESConfig.CurrentPart.Split(',')[0].Equals("6"))
                {
                    MyLoading wMyLoading = new MyLoading();
                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, exc) => IsQuality_DoWork(s, exc, wCapacitorPackageNo);
                    wBW.RunWorkerCompleted += (s, exc) => IsQuality_RunWorkerCompleted(exc, wMyLoading, wCapacitorPackageNo);
                    wBW.RunWorkerAsync();
                    wMyLoading.ShowDialog();
                }
                else
                {
                    if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                        MessageBoxImage.Question) != MessageBoxResult.OK)
                        return;

                    //获取填写值
                    Dictionary<IPTStandard, string> wDic = GetValueDic();

                    MyLoading wMyLoading = new MyLoading();
                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wCapacitorPackageNo, wDic);
                    wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading);
                    wBW.RunWorkerAsync();
                    wMyLoading.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void IsQuality_DoWork(object s, DoWorkEventArgs e, string wCapacitorPackageNo)
        {
            try
            {
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                string wMsg = "";
                int wErrorCode = 0;
                List<SFCModuleRecord> wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacitorPackageNo, "", -1, "", -1, out wErrorCode);
                if (int.Parse(wMESConfig.CurrentPart.Split(',')[0]) > 7)
                    wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", "", wCapacitorPackageNo, -1, "", -1, out wErrorCode);

                if (wMRecordList.Count > 0)
                {
                    wMRecordList = wMRecordList.OrderByDescending(p => p.ID).ToList();
                    List<IPTBoolValue> wBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    List<IPTTextValue> wIPTTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    List<IPTNumberValue> wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);

                    foreach (IPTBoolValue wIPTBoolValue in wBoolValueList)
                    {
                        if (wIPTBoolValue.Value != 1)
                        {
                            List<IPTStandard> wStandardList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(wIPTBoolValue.StandardID, -1, -1, -1, -1, -1, -1, out wErrorCode);
                            wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值为NG!", wStandardList[0].PartName, wStandardList[0].ItemName, wCapacitorPackageNo);
                            e.Result = wMsg;
                            return;
                        }
                    }
                    foreach (IPTNumberValue wIPTNumberValue in wIPTNumberValueList)
                    {
                        List<IPTStandard> wStandardList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(wIPTNumberValue.StandardID, -1, -1, -1, -1, -1, -1, out wErrorCode);
                        if (wStandardList[0].LowerLimit != 0 || wStandardList[0].UpperLimit != 0)
                        {
                            if (wIPTNumberValue.Value < wStandardList[0].LowerLimit || wIPTNumberValue.Value > wStandardList[0].UpperLimit)
                            {
                                wMsg = string.Format("【{2}】-【{0}】-【{1}】填写值不合法，标准范围为【{3}{5}-{4}{5}】，实际填写值为【{6}{5}】!", wStandardList[0].PartName, wStandardList[0].ItemName, wCapacitorPackageNo, wStandardList[0].LowerLimit.ToString(), wStandardList[0].UpperLimit.ToString(), wStandardList[0].UnitText, wIPTNumberValue.Value.ToString());
                                e.Result = wMsg;
                                return;
                            }
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

        private void IsQuality_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, string wCapacitorPackageNo)
        {
            try
            {
                wMyLoading.Close();

                string wMsg = e.Result as string;
                if (string.IsNullOrWhiteSpace(wMsg))
                {
                    if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                        MessageBoxImage.Question) != MessageBoxResult.OK)
                        return;

                    //获取填写值
                    Dictionary<IPTStandard, string> wDic = GetValueDic();

                    MyLoading wMyLoading1 = new MyLoading();
                    BackgroundWorker wBW = new BackgroundWorker();
                    wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wCapacitorPackageNo, wDic);
                    wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading1);
                    wBW.RunWorkerAsync();
                    wMyLoading1.ShowDialog();
                }
                else
                {
                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private Dictionary<IPTStandard, string> GetValueDic()
        {
            Dictionary<IPTStandard, string> wResult = new Dictionary<IPTStandard, string>();
            try
            {
                foreach (InputUC wUC in SP_DynamicInput.Children)
                {
                    string wValue = wUC.GetWriteValue();
                    IPTStandard wIPTStandard = wUC.mIPTStandard;
                    wResult.Add(wIPTStandard, wValue);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        private void Save_DoWork(object s, DoWorkEventArgs e, string wCapacitorPackageNo, Dictionary<IPTStandard, string> wDic)
        {
            try
            {
                int wErrorCode = 0;
                int wShiftID = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                //①获取系统配置
                MESConfig wMESConfig = MESConfigDAO.Instance.GetMESConfig();
                List<string> wPartsList = wMESConfig.CurrentPart.Split(',').ToList();

                string wFlag = "";
                //根据电容包编号获取最新的记录流水号
                List<SFCModuleRecord> wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacitorPackageNo, "", -1, "", -1, out wErrorCode);
                if (int.Parse(wMESConfig.CurrentPart.Split(',')[0]) > 7)
                    wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", "", wCapacitorPackageNo, -1, "", -1, out wErrorCode);
                if (wMRecordList.Count > 0)
                {
                    wMRecordList = wMRecordList.OrderByDescending(p => p.ID).ToList();
                    List<IPTBoolValue> wBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    List<IPTTextValue> wIPTTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    List<IPTNumberValue> wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);

                    //判断是否满足修改条件
                    if (wPartsList.Exists(p => p.Equals(wMRecordList[0].CurrentPartID.ToString())))
                    {
                        //可修改
                        bool wIsUpdate = false;
                        foreach (IPTStandard wIPTStandard in wDic.Keys)
                            if (wBoolValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTTextValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTNumberValueList.Exists(p => p.StandardID == wIPTStandard.ID))
                                wIsUpdate = true;
                        if (wIsUpdate)
                        {
                            //修改填写值
                            UpdateWriteValue(wMRecordList[0], wDic, wBoolValueList, wIPTTextValueList, wIPTNumberValueList);
                            wFlag = "已修改";
                        }
                    }
                    else
                    {
                        foreach (IPTStandard wIPTStandard in wDic.Keys)
                            if (wBoolValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTTextValueList.Exists(p => p.StandardID == wIPTStandard.ID) || wIPTNumberValueList.Exists(p => p.StandardID == wIPTStandard.ID))
                                wFlag = "保存失败，该电容包已流转到其他工位!";
                    }
                }
                else
                {
                    if (!wPartsList.Exists(p => p.Equals("1")))
                        wFlag = "保存失败，首工位未记录该电容包编号!";
                }

                if (!string.IsNullOrWhiteSpace(wFlag))
                    e.Result = wFlag;
                else
                {
                    //②首工位存模组配置
                    if (wPartsList.Exists(p => p.Equals("1")))
                    {
                        string wSerialNumber = SFCModuleRecordDAO.Instance.SFC_CreateSerialNumber(out wErrorCode);
                        SFCModuleRecord wSFCModuleRecord = new SFCModuleRecord();
                        wSFCModuleRecord.ID = 0;
                        wSFCModuleRecord.Gear = "";
                        wSFCModuleRecord.BarCode = "";
                        wSFCModuleRecord.CapacitorPackageNo = wCapacitorPackageNo;
                        wSFCModuleRecord.CreateID = GUD.mLoginUser.ID;
                        wSFCModuleRecord.CreateTime = DateTime.Now;
                        wSFCModuleRecord.CurrentPartID = int.Parse(wPartsList[0]);
                        wSFCModuleRecord.ModuleNumber = "";
                        wSFCModuleRecord.OnlineTime = DateTime.Now;
                        wSFCModuleRecord.SerialNumber = wSerialNumber;
                        wSFCModuleRecord.ShiftID = wShiftID;
                        wSFCModuleRecord.ProductID = wMESConfig.CurrentProduct;
                        wSFCModuleRecord.Times = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacitorPackageNo, "", -1, "", -1, out wErrorCode).Count + 1;
                        wSFCModuleRecord.Active = 1;
                        wSFCModuleRecord.IsQuality = 1;
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSFCModuleRecord, out wErrorCode);
                    }
                    //电容包装配工位-绑定模组编码
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("7"))
                    {
                        foreach (IPTStandard wIPTStandard in wDic.Keys)
                        {
                            if (wIPTStandard.ItemName.Equals("模组编码") && wMRecordList.Count > 0)
                            {
                                wMRecordList[0].ModuleNumber = wDic[wIPTStandard];
                                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                            }
                        }
                    }
                    //模组检测记录档位信息
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("9"))
                    {
                        foreach (IPTStandard wIPTStandard in wDic.Keys)
                        {
                            if (wIPTStandard.ItemName.Equals("模组档位") && wMRecordList.Count > 0)
                            {
                                wMRecordList[0].Gear = wDic[wIPTStandard];
                                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                            }
                        }
                    }
                    //包装记录条码信息和末工位下线时间
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("11"))
                    {
                        foreach (IPTStandard wIPTStandard in wDic.Keys)
                        {
                            if (wIPTStandard.ItemName.Equals("装箱条形码") && wMRecordList.Count > 0)
                            {
                                wMRecordList[0].BarCode = wDic[wIPTStandard];
                                wMRecordList[0].OfflineTime = DateTime.Now;
                                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                            }
                            if (wIPTStandard.ItemName.Equals("装托条形码") && wMRecordList.Count > 0)
                            {
                                wMRecordList[0].TrustBarCode = wDic[wIPTStandard];
                                wMRecordList[0].OfflineTime = DateTime.Now;
                                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                            }
                        }
                    }
                    //②保存当前工位信息
                    if (wMRecordList.Count > 0)
                    {
                        wMRecordList[0].CurrentPartID = int.Parse(wPartsList[0]);
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wMRecordList[0], out wErrorCode);
                    }
                    //③存填写值
                    //根据电容包编号获取最新的记录流水号
                    wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacitorPackageNo, "", -1, "", -1, out wErrorCode);
                    if (int.Parse(wMESConfig.CurrentPart.Split(',')[0]) > 7)
                        wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", "", wCapacitorPackageNo, -1, "", -1, out wErrorCode);
                    if (wMRecordList.Count > 0)
                    {
                        wMRecordList = wMRecordList.OrderByDescending(p => p.ID).ToList();
                        foreach (IPTStandard wIPTStandard in wDic.Keys)
                        {
                            string wValue = wDic[wIPTStandard];
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
                                    wIPTBoolValue.Value = int.Parse(wValue);
                                    IPTBoolValueDAO.Instance.IPT_SaveIPTBoolValue(wIPTBoolValue, out wErrorCode);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    e.Result = "";
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 更新填写值
        /// </summary>
        private void UpdateWriteValue(SFCModuleRecord sFCModuleRecord, Dictionary<IPTStandard, string> wDic, List<IPTBoolValue> wBoolValueList, List<IPTTextValue> wIPTTextValueList, List<IPTNumberValue> wIPTNumberValueList)
        {
            try
            {
                int wErrorCode = 0;
                foreach (IPTStandard wIPTStandard in wDic.Keys)
                {
                    switch ((StandardType)wIPTStandard.Type)
                    {
                        case StandardType.文本:
                            if (wIPTTextValueList.Exists(p => p.SerialNumber == sFCModuleRecord.SerialNumber && p.StandardID == wIPTStandard.ID))
                            {
                                IPTTextValue wIPTTextValue = wIPTTextValueList.Find(p => p.SerialNumber == sFCModuleRecord.SerialNumber && p.StandardID == wIPTStandard.ID);
                                wIPTTextValue.Value = wDic[wIPTStandard];
                                IPTTextValueDAO.Instance.IPT_SaveIPTTextValue(wIPTTextValue, out wErrorCode);
                            }
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
                            if (wIPTNumberValueList.Exists(p => p.SerialNumber == sFCModuleRecord.SerialNumber && p.StandardID == wIPTStandard.ID))
                            {
                                IPTNumberValue wIPTNumberValue = wIPTNumberValueList.Find(p => p.SerialNumber == sFCModuleRecord.SerialNumber && p.StandardID == wIPTStandard.ID);
                                wIPTNumberValue.Value = double.Parse(wDic[wIPTStandard]);
                                IPTNumberValueDAO.Instance.IPT_SaveIPTNumberValue(wIPTNumberValue, out wErrorCode);
                            }
                            break;
                        case StandardType.单选:
                        case StandardType.是否:
                            if (wBoolValueList.Exists(p => p.SerialNumber == sFCModuleRecord.SerialNumber && p.StandardID == wIPTStandard.ID))
                            {
                                IPTBoolValue wIPTBoolValue = wBoolValueList.Find(p => p.SerialNumber == sFCModuleRecord.SerialNumber && p.StandardID == wIPTStandard.ID);
                                wIPTBoolValue.Value = int.Parse(wDic[wIPTStandard]);
                                IPTBoolValueDAO.Instance.IPT_SaveIPTBoolValue(wIPTBoolValue, out wErrorCode);
                            }
                            break;
                        default:
                            break;
                    }
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("7") && wIPTStandard.ItemName.Equals("模组编码"))
                    {
                        sFCModuleRecord.ModuleNumber = wDic[wIPTStandard];
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(sFCModuleRecord, out wErrorCode);
                    }
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("9") && wIPTStandard.ItemName.Equals("模组档位"))
                    {
                        sFCModuleRecord.Gear = wDic[wIPTStandard];
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(sFCModuleRecord, out wErrorCode);
                    }
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("11") && wIPTStandard.ItemName.Equals("装箱条形码"))
                    {
                        sFCModuleRecord.BarCode = wDic[wIPTStandard];
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(sFCModuleRecord, out wErrorCode);
                    }
                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("11") && wIPTStandard.ItemName.Equals("装托条形码"))
                    {
                        sFCModuleRecord.TrustBarCode = wDic[wIPTStandard];
                        SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(sFCModuleRecord, out wErrorCode);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Save_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                string wResult = (string)e.Result;
                if (!string.IsNullOrWhiteSpace(wResult) && !wResult.Equals("已修改"))
                {
                    MessageBox.Show(wResult, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    //渲染左侧表格
                    LoadTable();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 电容包编码回车事件监听
        private void TB_CapacitorPackageNo_TextInput(object sender, TextChangedEventArgs e)
        {
            try
            {
                string wText = TB_CapacitorPackageNo.Text;
                if (wText.Contains("\r\n"))
                {
                    TB_CapacitorPackageNo.Text = TB_CapacitorPackageNo.Text.Replace("\r\n", "");
                    if (string.IsNullOrWhiteSpace(TB_CapacitorPackageNo.Text))
                    {
                        MessageBox.Show("电容包编号错误!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        TB_CapacitorPackageNo.Text = "";
                        return;
                    }

                    if (mMESConfig.CurrentPart.Split(',')[0].Equals("6"))
                    {
                        wText = wText.Replace("\r\n", "");
                        MyLoading wMyLoading = new MyLoading();
                        BackgroundWorker wBW = new BackgroundWorker();
                        wBW.DoWork += (s, exc) => IsQuality_DoWork(s, exc, wText);
                        wBW.RunWorkerCompleted += (s, exc) => IsQuality_RunWorkerCompleted1(exc, wMyLoading, wText);
                        wBW.RunWorkerAsync();
                        wMyLoading.ShowDialog();
                    }
                    else
                    {
                        //选中第一个动态输入框
                        bool wFlag = false;
                        foreach (InputUC wInputUC in SP_DynamicInput.Children)
                        {
                            if (wInputUC.mIPTStandard.Type == 1 || wInputUC.mIPTStandard.Type == 2)
                            {
                                wInputUC.MyFocus();

                                //电容包静置默认自动填充时间
                                if (mMESConfig.CurrentPart.Split(',')[0].Equals("3"))
                                    wInputUC.SetMyValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                wFlag = true;
                                return;
                            }
                        }
                        if (!wFlag)
                        {
                            Cmd_Save_Click(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void IsQuality_RunWorkerCompleted1(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, string wCapacitorPackageNo)
        {
            try
            {
                wMyLoading.Close();

                string wMsg = e.Result as string;
                if (string.IsNullOrWhiteSpace(wMsg))
                {
                    //选中第一个动态输入框
                    bool wFlag = false;
                    foreach (InputUC wInputUC in SP_DynamicInput.Children)
                    {
                        if (wInputUC.mIPTStandard.Type == 1 || wInputUC.mIPTStandard.Type == 2)
                        {
                            wInputUC.MyFocus();

                            //电容包静置默认自动填充时间
                            if (mMESConfig.CurrentPart.Split(',')[0].Equals("3"))
                                wInputUC.SetMyValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            wFlag = true;
                            return;
                        }
                    }
                    if (!wFlag)
                    {
                        Cmd_Save_Click(null, null);
                    }
                }
                else
                {
                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 历史记录查询
        private void Cmd_History_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TB_CapacitorPackageNo.Text))
                {
                    MessageBox.Show("电容包编号或模组编码未录入!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                HistoryRecordWindow wWindow = new HistoryRecordWindow(TB_CapacitorPackageNo.Text);
                wWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
