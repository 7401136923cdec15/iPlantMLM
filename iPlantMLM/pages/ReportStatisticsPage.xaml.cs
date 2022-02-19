using Chart;
using Infragistics.Controls.Charts;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iPlantMLM
{
    /// <summary>
    /// ReportStatisticsPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReportStatisticsPage : Page
    {
        public ReportStatisticsPage()
        {
            InitializeComponent();
        }

        #region 页面加载
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime wTime = DateTime.Now;
                wTime = wTime.AddDays(-4);
                Dpk_StartDateModule.SelectedDate = wTime;
                Dpk_EndDateModule.SelectedDate = DateTime.Now;
                Dpk_StartDateModule1.SelectedDate = DateTime.Now;
                Dpk_EndDateModule1.SelectedDate = DateTime.Now;
                Dpk_StartDateModule2.SelectedDate = DateTime.Now;
                Dpk_EndDateModule2.SelectedDate = DateTime.Now;

                //标准条形图
                LoadChart();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 标准条形图
        private ObservableCollection<NomalColumnar> GetNomalColumnarOC_DL(List<ModuleChart> wList)
        {
            ObservableCollection<NomalColumnar> wResultOC = new ObservableCollection<NomalColumnar>();
            try
            {
                Random wRandom = new Random();

                Dictionary<string, double> wDataDic = new Dictionary<string, double>();
                foreach (ModuleChart wModuleChart in wList)
                    wDataDic.Add(wModuleChart.Date, wModuleChart.Total);

                foreach (string wLable in wDataDic.Keys)
                {
                    NomalColumnar wNomalColumnar = new NomalColumnar();
                    wNomalColumnar.Value = wDataDic[wLable];
                    wNomalColumnar.Label = wLable;
                    wResultOC.Add(wNomalColumnar);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return wResultOC;
        }
        private ObservableCollection<NomalColumnar> GetNomalColumnarOC1_DL(List<ModuleChart> wList)
        {
            ObservableCollection<NomalColumnar> wResultOC = new ObservableCollection<NomalColumnar>();
            try
            {
                Dictionary<string, double> wDataDic = new Dictionary<string, double>();
                foreach (ModuleChart wModuleChart in wList)
                    wDataDic.Add(wModuleChart.Date, wModuleChart.DTGood);

                foreach (string wLable in wDataDic.Keys)
                {
                    NomalColumnar wNomalColumnar = new NomalColumnar();
                    wNomalColumnar.Value = wDataDic[wLable];
                    wNomalColumnar.Label = wLable;
                    wResultOC.Add(wNomalColumnar);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return wResultOC;
        }
        private ObservableCollection<NomalColumnar> GetNomalColumnarOC2_DL(List<ModuleChart> wList)
        {
            ObservableCollection<NomalColumnar> wResultOC = new ObservableCollection<NomalColumnar>();
            try
            {
                Dictionary<string, double> wDataDic = new Dictionary<string, double>();
                foreach (ModuleChart wModuleChart in wList)
                    wDataDic.Add(wModuleChart.Date, wModuleChart.DTBad);

                foreach (string wLable in wDataDic.Keys)
                {
                    NomalColumnar wNomalColumnar = new NomalColumnar();
                    wNomalColumnar.Value = wDataDic[wLable];
                    wNomalColumnar.Label = wLable;
                    wResultOC.Add(wNomalColumnar);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return wResultOC;
        }
        private ObservableCollection<NomalColumnar> GetNomalColumnarOC3_DL(List<ModuleChart> wList)
        {
            ObservableCollection<NomalColumnar> wResultOC = new ObservableCollection<NomalColumnar>();
            try
            {
                Dictionary<string, double> wDataDic = new Dictionary<string, double>();
                foreach (ModuleChart wModuleChart in wList)
                    wDataDic.Add(wModuleChart.Date, wModuleChart.MZGood);

                foreach (string wLable in wDataDic.Keys)
                {
                    NomalColumnar wNomalColumnar = new NomalColumnar();
                    wNomalColumnar.Value = wDataDic[wLable];
                    wNomalColumnar.Label = wLable;
                    wResultOC.Add(wNomalColumnar);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return wResultOC;
        }
        private ObservableCollection<NomalColumnar> GetNomalColumnarOC4_DL(List<ModuleChart> wList)
        {
            ObservableCollection<NomalColumnar> wResultOC = new ObservableCollection<NomalColumnar>();
            try
            {
                Dictionary<string, double> wDataDic = new Dictionary<string, double>();
                foreach (ModuleChart wModuleChart in wList)
                    wDataDic.Add(wModuleChart.Date, wModuleChart.MZBad);

                foreach (string wLable in wDataDic.Keys)
                {
                    NomalColumnar wNomalColumnar = new NomalColumnar();
                    wNomalColumnar.Value = wDataDic[wLable];
                    wNomalColumnar.Label = wLable;
                    wResultOC.Add(wNomalColumnar);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return wResultOC;
        }

        private ColumnarProperty GetBarProperty_DL()
        {
            ColumnarProperty wBarProperty = new ColumnarProperty();
            try
            {
                //标题（在堆积柱状图不生效）
                wBarProperty.Title = "总投料数";
                //图实体
                wBarProperty.XamDataCharts = this.XDC_ColumnarDisplay;
                // 说明实体
                wBarProperty.Legends = this.Legend_ColumnarDisplay;
                //是否开启说明  默认不开启
                wBarProperty.LegendsFlag = true;
                //颜色（堆积柱状图不生效）
                wBarProperty.Color = Brushes.Blue;
                //是否自定义颜色（堆积柱状图不生效）默认不使用
                wBarProperty.ColorFlag = true;
                //颜色（堆积柱状图生效）
                wBarProperty.ColorLists = null;
                //是否自定义颜色（堆积柱状图生效）默认不使用
                wBarProperty.ColorListFlag = false;
                //X轴
                wBarProperty.NumericY = GetNumericX_DL();
                //Y轴
                wBarProperty.CategoryX = GetCategoryY_DL();
                //X轴刻度设置
                AxisLabelSettings wAxisLabelSettings_X = new AxisLabelSettings();
                wAxisLabelSettings_X.Foreground = Brushes.Black;
                wAxisLabelSettings_X.Angle = 0;
                wAxisLabelSettings_X.FontSize = 16;
                wAxisLabelSettings_X.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                wAxisLabelSettings_X.Location = AxisLabelsLocation.OutsideLeft;
                wBarProperty.XAxisLabelSettings = wAxisLabelSettings_X;
                //Y轴刻度设置
                AxisLabelSettings wAxisLabelSettings_Y = new AxisLabelSettings();
                wAxisLabelSettings_Y.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                wAxisLabelSettings_Y.FontSize = 16;
                wAxisLabelSettings_Y.Location = AxisLabelsLocation.OutsideBottom;
                wAxisLabelSettings_Y.Foreground = Brushes.Black;
                wBarProperty.YAxisLabelSettings = wAxisLabelSettings_Y;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wBarProperty;
        }
        private CategoryXAxis GetCategoryY_DL()
        {
            CategoryXAxis wCategoryXAxis = new CategoryXAxis();
            try
            {
                wCategoryXAxis.ItemsSource = null;
                wCategoryXAxis.Label = "{Label}";
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wCategoryXAxis;
        }
        private NumericYAxis GetNumericX_DL()
        {
            NumericYAxis wNumericYAxis = new NumericYAxis();
            try
            {
                wNumericYAxis.Label = "{}";
                wNumericYAxis.MinimumValue = 0;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wNumericYAxis;
        }
        #endregion

        #region 查询
        private void Btn_QueryByModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //①日期
                DateTime wSTime = Dpk_StartDateModule1.SelectedDate.Value;
                wSTime = new DateTime(wSTime.Year, wSTime.Month, wSTime.Day, 0, 0, 0);
                DateTime wETime = Dpk_EndDateModule1.SelectedDate.Value;
                wETime = new DateTime(wETime.Year, wETime.Month, wETime.Day, 23, 59, 59);
                //②编码
                String wCode = Txt_Module.Text;
                //③装箱条码
                string wZXCode = Txt_ZXCode.Text;
                //④装托条码
                string wZTCode = Txt_ZTCode.Text;
                //⑤档位
                string wGear = Txt_Gear.Text;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Btn_QueryByModule_Click_DoWork(s, exc, wSTime, wETime, wCode, wZXCode, wZTCode, wGear);
                wBW.RunWorkerCompleted += (s, exc) => Btn_QueryByModule_Click_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_QueryByModule_Click_DoWork(object s, DoWorkEventArgs e, DateTime wSTime, DateTime wETime, string wCode, string wZXCode, string wZTCode, string wGear)
        {
            try
            {
                List<SFCModuleRecord> wList = SFCModuleRecordDAO.Instance.SFC_QueryHistoryList(wSTime, wETime, wCode, wZXCode, wZTCode, 1, wGear);
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_QueryByModule_Click_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<SFCModuleRecord> wList = e.Result as List<SFCModuleRecord>;
                Xdg_MainGrid.DataSource = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 表格双击
        private void TableClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                    return;

                ModuleHistoryWindow wUI = new ModuleHistoryWindow(wSelectedDisplay);
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region Excel导出
        private void Btn_Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int wErrorCode = ExcelOutPutTool.Intance.ExcelExportData("模组检测数据统计", Xdg_MainGrid);
                if (wErrorCode != 0)
                    MessageBox.Show("导出出错！错误码：" + wErrorCode.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 不合格数据查询
        private void Btn_QueryByModule2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //①日期
                DateTime wSTime = Dpk_StartDateModule2.SelectedDate.Value;
                wSTime = new DateTime(wSTime.Year, wSTime.Month, wSTime.Day, 0, 0, 0);
                DateTime wETime = Dpk_EndDateModule2.SelectedDate.Value;
                wETime = new DateTime(wETime.Year, wETime.Month, wETime.Day, 23, 59, 59);
                //②编码
                String wCode = Txt_Module1.Text;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Btn_QueryByModule2_Click_DoWork(s, exc, wSTime, wETime, wCode);
                wBW.RunWorkerCompleted += (s, exc) => Btn_QueryByModule2_Click_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Btn_QueryByModule2_Click_DoWork(object s, DoWorkEventArgs e, DateTime wSTime, DateTime wETime, string wCode)
        {
            try
            {
                List<SFCModuleRecord> wList = SFCModuleRecordDAO.Instance.SFC_QueryHistoryList(wSTime, wETime, wCode, "", "", 2, "");
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_QueryByModule2_Click_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<SFCModuleRecord> wList = e.Result as List<SFCModuleRecord>;
                Xdg_MainGrid1.DataSource = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 不合格品导出
        private void Btn_Export1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int wErrorCode = ExcelOutPutTool.Intance.ExcelExportData("不合格数据统计", Xdg_MainGrid1);
                if (wErrorCode != 0)
                    MessageBox.Show("导出出错！错误码：" + wErrorCode.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 标定
        private void Btn_Sign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid1.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要标定的不合格数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (wSelectedDisplay.Active != 1)
                {
                    MessageBox.Show("请选中状态为“激活”的不合格数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认标定吗？标定后，系统查重将不检查该电容包编码!", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                wSelectedDisplay.Active = 2;
                int wErrorCode = 0;
                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSelectedDisplay, out wErrorCode);

                Btn_QueryByModule2_Click(null, null);
                MessageBox.Show("标定成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 产量查询
        private void Btn_QueryChart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadChart();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 加载图表
        /// </summary>
        private void LoadChart()
        {
            try
            {
                DateTime wSTime = Dpk_StartDateModule.SelectedDate.Value;
                DateTime wETime = Dpk_EndDateModule.SelectedDate.Value;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => LoadChart_DoWork(s, exc, wSTime, wETime);
                wBW.RunWorkerCompleted += (s, exc) => LoadChart_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadChart_DoWork(object s, DoWorkEventArgs e, DateTime wSTime, DateTime wETime)
        {
            try
            {
                List<ModuleChart> wList = new List<ModuleChart>();
                while (int.Parse(wSTime.ToString("yyyyMMdd")) <= int.Parse(wETime.ToString("yyyyMMdd")))
                {
                    int wShiftID = int.Parse(wSTime.ToString("yyyyMMdd"));

                    ModuleChart wModuleChart = SFCModuleRecordDAO.Instance.SFC_QueryModuleChart(wShiftID);
                    wModuleChart.Date = wSTime.ToString("MM/dd");
                    wList.Add(wModuleChart);

                    wSTime = wSTime.AddDays(1);
                }
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadChart_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<ModuleChart> wList = e.Result as List<ModuleChart>;

                ChartDisplay wChartDisplay_DL = new ChartDisplay();
                ColumnarProperty wColumnarProperty_DL = GetBarProperty_DL();
                wChartDisplay_DL.Nomal_Columnar_Display(GetNomalColumnarOC_DL(wList), wColumnarProperty_DL, true, false);

                wColumnarProperty_DL.Color = Brushes.Green;
                wColumnarProperty_DL.Title = "单体电压合格数";
                wChartDisplay_DL.Nomal_Columnar_Display(GetNomalColumnarOC1_DL(wList), wColumnarProperty_DL, false, false);

                wColumnarProperty_DL.Color = Brushes.Red;
                wColumnarProperty_DL.Title = "单体电压不合格数";
                wChartDisplay_DL.Nomal_Columnar_Display(GetNomalColumnarOC2_DL(wList), wColumnarProperty_DL, false, false);

                wColumnarProperty_DL.Color = Brushes.LimeGreen;
                wColumnarProperty_DL.Title = "模组检测合格数";
                wChartDisplay_DL.Nomal_Columnar_Display(GetNomalColumnarOC3_DL(wList), wColumnarProperty_DL, false, false);

                wColumnarProperty_DL.Color = Brushes.PaleVioletRed;
                wColumnarProperty_DL.Title = "模组检测不合格数";
                wChartDisplay_DL.Nomal_Columnar_Display(GetNomalColumnarOC4_DL(wList), wColumnarProperty_DL, false, false);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 不合格品双击
        private void TableClick1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid1.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                    return;

                ModuleHistoryWindow wUI = new ModuleHistoryWindow(wSelectedDisplay);
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 容量内阻导出
        private void Btn_CapacityExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime wSTime = Dpk_StartDateModule1.SelectedDate.Value;
                wSTime = new DateTime(wSTime.Year, wSTime.Month, wSTime.Day, 0, 0, 0);
                DateTime wETime = Dpk_EndDateModule1.SelectedDate.Value;
                wETime = new DateTime(wETime.Year, wETime.Month, wETime.Day, 23, 59, 59);

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Btn_CapacityExport_Click_DoWork(s, exc, wSTime, wETime);
                wBW.RunWorkerCompleted += (s, exc) => Btn_CapacityExport_Click_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Btn_CapacityExport_Click_DoWork(object s, DoWorkEventArgs e, DateTime wSTime, DateTime wETime)
        {
            try
            {
                List<SFCModuleRecord> wList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByTime(wSTime, wETime);
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_CapacityExport_Click_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<SFCModuleRecord> wList = e.Result as List<SFCModuleRecord>;
                if (wList == null || wList.Count <= 0)
                {
                    MessageBox.Show("所选日期范围内无数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //创建文件夹
                Microsoft.Win32.SaveFileDialog wSaveFileDialog = new Microsoft.Win32.SaveFileDialog();

                //设置文件类型
                //书写规则例如：txt files(*.txt)|*.txt
                wSaveFileDialog.Filter = "xlsx files(*.xlsx)|*.xlsx";
                //设置默认文件名（可以不设置）
                wSaveFileDialog.FileName = "容量内阻.xlsx";
                //主设置默认文件extension（可以不设置）
                wSaveFileDialog.DefaultExt = "xlsx";
                //获取或设置一个值，该值指示如果用户省略扩展名，文件对话框是否自动在文件名中添加扩展名。（可以不设置）
                wSaveFileDialog.AddExtension = true;

                //设置默认文件类型显示顺序（可以不设置）
                wSaveFileDialog.FilterIndex = 1;

                //保存对话框是否记忆上次打开的目录
                wSaveFileDialog.RestoreDirectory = true;

                // Show save file dialog box
                bool? wResult = wSaveFileDialog.ShowDialog();

                if (wResult == true)
                {
                    FileStream wFileStream = File.Create(wSaveFileDialog.FileName);

                    List<List<string>> wRowList = new List<List<string>>();

                    List<string> wColList1 = new List<string>();
                    wColList1.Add("序号");
                    wColList1.Add("电容包编号");
                    wColList1.Add("容量");
                    wColList1.Add("内阻");
                    wColList1.Add("档位");
                    wRowList.Add(wColList1);

                    int wIndex = 1;
                    foreach (SFCModuleRecord wItem in wList)
                    {
                        if (string.IsNullOrWhiteSpace(wItem.Gear))
                            continue;

                        List<string> wColList = new List<string>();
                        wColList.Add(wIndex.ToString());
                        wColList.Add(wItem.CapacitorPackageNo);
                        wColList.Add(wItem.Capacity);
                        wColList.Add(wItem.InternalResistance);
                        wColList.Add(wItem.Gear);
                        wRowList.Add(wColList);

                        wIndex++;
                    }

                    ExcelTool.Instance.ExportToExcel(wFileStream, wRowList, "容量内阻");

                    MessageBox.Show("导出成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 查看单体信息
        private void Btn_SeeSingle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中电容包信息!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SingleInfoWindow wUI = new SingleInfoWindow(wSelectedDisplay);
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 查看单体信息
        private void Btn_SeeSingleBad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid1.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中电容包信息!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SingleInfoWindow wUI = new SingleInfoWindow(wSelectedDisplay);
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 删除当前工位数据
        private void Btn_ClearData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 8001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中电容包信息!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认删除当前工位数据吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => DeleteData_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => DeleteData_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void DeleteData_DoWork(object s, DoWorkEventArgs e, SFCModuleRecord wSelectedDisplay)
        {
            try
            {
                int wErrorCode = 0;
                //①根据流水号和工位删除当前工位的数据
                List<IPTNumberValue> wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wSelectedDisplay.SerialNumber, -1, -1, wSelectedDisplay.CurrentPartID, out wErrorCode);
                if (wIPTNumberValueList.Count > 0)
                    IPTNumberValueDAO.Instance.IPT_DeleteIPTNumberValueList(wIPTNumberValueList);

                List<IPTTextValue> wIPTTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wSelectedDisplay.SerialNumber, -1, -1, wSelectedDisplay.CurrentPartID, out wErrorCode);
                if (wIPTTextValueList.Count > 0)
                    IPTTextValueDAO.Instance.IPT_DeleteIPTTextValueList(wIPTTextValueList);

                List<IPTBoolValue> wIPTBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wSelectedDisplay.SerialNumber, -1, -1, wSelectedDisplay.CurrentPartID, out wErrorCode);
                if (wIPTBoolValueList.Count > 0)
                    IPTBoolValueDAO.Instance.IPT_DeleteIPTBoolValueList(wIPTBoolValueList);
                //②判断是否是工位7-电容包装配，若是，需要删除模组编码数据
                if (wSelectedDisplay.CurrentPartID == 7)
                    wSelectedDisplay.ModuleNumber = "";
                wSelectedDisplay.IsQuality = 1;
                //③将当前工位-1，若等于5，再-1
                wSelectedDisplay.CurrentPartID -= 1;
                if (wSelectedDisplay.CurrentPartID == 5)
                    wSelectedDisplay.CurrentPartID -= 1;
                if (wSelectedDisplay.CurrentPartID < 1)
                    wSelectedDisplay.CurrentPartID = 1;
                SFCModuleRecordDAO.Instance.SFC_SaveSFCModuleRecord(wSelectedDisplay, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void DeleteData_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                this.Btn_QueryByModule_Click(null, null);

                MessageBox.Show("数据删除成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 删除当前工位数据
        private void Btn_ClearData_Bad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 8001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid1.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中电容包信息!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认删除当前工位数据吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => DeleteData_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => DeleteData_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
