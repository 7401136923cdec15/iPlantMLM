using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace iPlantMLM
{
    /// <summary>
    /// HistoryWindow1.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryWindow1 : Window
    {
        public HistoryWindow1()
        {
            InitializeComponent();
        }

        #region 查询
        private void Btn_QueryByModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //产品
                int wProductID = (int)Cbb_ProductType.SelectedValue;
                //时间段
                DateTime wSTime = (DateTime)Dpk_StartDateModule.SelectedDate;
                wSTime = new DateTime(wSTime.Year, wSTime.Month, wSTime.Day, 0, 0, 0);
                DateTime wETime = (DateTime)Dpk_EndDateModule.SelectedDate;
                wETime = new DateTime(wETime.Year, wETime.Month, wETime.Day, 23, 59, 59);
                //模组或电容包编号
                string wCode = Txt_Module.Text.Trim();

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Window_Loaded_DoWork(s, exc, wProductID, wSTime, wETime, wCode);
                wBW.RunWorkerCompleted += (s, exc) => Window_Loaded_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 全局变量
        //表格实例
        private MEDataGrid mMEDataGrid = new MEDataGrid();
        #endregion

        #region 窗体加载
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dpk_StartDateModule.SelectedDate = DateTime.Now;
                Dpk_EndDateModule.SelectedDate = DateTime.Now;
                List<FPCProduct> wList = FPCProductDAO.Instance.GetProductList();
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                Cbb_ProductType.ItemsSource = wList;
                Cbb_ProductType.SelectedValue = wConfig.CurrentProduct;

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
                    0, wPartIDList, 0, wConfig.CurrentProduct, true);

                //添加到页面
                SV_DateList.Content = mMEDataGrid;

                //默认查询
                //产品
                int wProductID = (int)Cbb_ProductType.SelectedValue;
                //时间段
                DateTime wSTime = (DateTime)Dpk_StartDateModule.SelectedDate;
                wSTime = new DateTime(wSTime.Year, wSTime.Month, wSTime.Day, 0, 0, 0);
                DateTime wETime = (DateTime)Dpk_EndDateModule.SelectedDate;
                wETime = new DateTime(wETime.Year, wETime.Month, wETime.Day, 23, 59, 59);
                //模组或电容包编号
                string wCode = Txt_Module.Text.Trim();

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Window_Loaded_DoWork(s, exc, wProductID, wSTime, wETime, wCode);
                wBW.RunWorkerCompleted += (s, exc) => Window_Loaded_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Window_Loaded_DoWork(object s, DoWorkEventArgs e, int wProductID, DateTime wSTime, DateTime wETime, string wCode)
        {
            try
            {
                int wErrorCode;
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                string[] wParts = wConfig.CurrentPart.Split(',');
                List<int> wPartIDList = new List<int>();
                foreach (string wItem in wParts)
                    wPartIDList.Add(int.Parse(wItem));

                List<SFCModuleRecord> wList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(wProductID, wSTime, wETime, wCode, wPartIDList, out wErrorCode);

                //按照箱号排序
                if (wConfig.CurrentPart.Contains("11") || wConfig.CurrentPart.Contains("12"))
                    wList = wList.OrderBy(p => p.BarCode).ToList();

                List<Dictionary<int, string>> wDicList = new List<Dictionary<int, string>>();
                foreach (SFCModuleRecord wSFCModuleRecord in wList)
                {
                    List<IPTTextValue> wTextValueList = new List<IPTTextValue>();
                    List<IPTNumberValue> wIPTNumberValueList = new List<IPTNumberValue>();
                    List<IPTBoolValue> wIPTBoolValueList = new List<IPTBoolValue>();
                    wTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                    wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                    wIPTBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wSFCModuleRecord.SerialNumber, -1, -1, -1, out wErrorCode);
                    Dictionary<int, string> wValueDic = new Dictionary<int, string>();
                    wValueDic.Add(0, "");
                    if (wSFCModuleRecord.ID > 0)
                        wValueDic.Add(1, wSFCModuleRecord.CapacitorPackageNo);
                    else
                        wValueDic.Add(1, wCode);
                    int wIndex = 2;


                    if (wPartIDList[0] >= 8 && wPartIDList[0] <= 10 && !wConfig.CurrentPart.Split(',').ToList().Exists(p => p.Equals("11")))
                    {
                        wValueDic.Add(2, wSFCModuleRecord.ModuleNumber);
                        wIndex = 3;
                    }

                    //模组编码组合
                    if (wPartIDList[0] == 6 || wPartIDList[0] == 11 || wPartIDList[0] == 12)
                    {
                        wValueDic.Add(2, wSFCModuleRecord.Gear);
                        wIndex = 3;
                    }

                    for (int i = 0; i < GUD.mIPTStandardList.Count; i++)
                    {
                        IPTStandard wIPTStandard = GUD.mIPTStandardList[i];
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
                        if (string.IsNullOrWhiteSpace(wValue) && wIPTStandard.ItemName.Equals("静置时间"))
                            wValue = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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

        public List<Dictionary<int, string>> mDicList = new List<Dictionary<int, string>>();

        private void Window_Loaded_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<Dictionary<int, string>> wDicList = e.Result as List<Dictionary<int, string>>;

                mDicList = wDicList;

                mMEDataGrid.ClearAll(true);
                foreach (Dictionary<int, string> wItem in wDicList)
                    mMEDataGrid.AppendRow(wItem);
                if (wDicList.Count <= 0)
                    MessageBox.Show("当前工位暂无数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 导出
        private void Btn_ExportModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mDicList.Count <= 0)
                {
                    MessageBox.Show("暂无数据，请查询历史数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认导出吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                //创建文件夹
                string wDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                FileStream wFileStream = File.Create(wDesktopPath + "/工位导出数据.xlsx");

                List<List<string>> wRowList = new List<List<string>>();
                List<string> wList = new List<string>();
                foreach (string wItem in mMEDataGrid.mColNameCellItemTypeDic.Keys)
                    wList.Add(wItem);
                wRowList.Add(wList);

                int wIndex = 1;
                foreach (Dictionary<int, string> wDic in mDicList)
                {
                    wList = new List<string>();
                    int wFlag = 1;
                    foreach (int wItem in wDic.Keys)
                    {
                        if (wFlag > mMEDataGrid.mColNameCellItemTypeDic.Count)
                            break;

                        wList.Add(wDic[wItem]);
                        wFlag++;
                    }

                    wList[0] = wIndex.ToString();

                    wRowList.Add(wList);

                    wIndex++;
                }

                ExcelTool.Instance.ExportToExcel(wFileStream, wRowList, "工位导出数据");

                MessageBox.Show("导出成功，请到桌面查看!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败，请先关闭该Excel文件!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);

                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
