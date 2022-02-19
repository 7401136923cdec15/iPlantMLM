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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace iPlantMLM
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 退出
        private void CmdCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定退出程序吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                this.Close();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 修改密码
        private void CmdChangePwd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangePsw wChangePsw = new ChangePsw(GUD.mLoginUser);
                wChangePsw.DelSave += wChangePsw_DelSave;
                wChangePsw.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wChangePsw_DelSave()
        {
            try
            {
                //①隐藏主界面
                this.Hide();
                //②清空已打开的菜单栏
                TC_Main.Items.Clear();
                //③显示登陆界面
                LoginUI wLoginUI = new LoginUI();
                wLoginUI.DelUser += wLoginUI_DelUser;
                wLoginUI.ShowDialog();
                this.Show();

                InputData();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 注销
        private void CmdLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确认注销吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                //①隐藏主界面
                this.Hide();
                //②清空已打开的菜单栏
                TC_Main.Items.Clear();
                //③显示登陆界面
                LoginUI wLoginUI = new LoginUI();
                wLoginUI.DelUser += wLoginUI_DelUser;
                wLoginUI.ShowDialog();
                this.Show();

                InputData();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 设置
        private void CmdSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if ("设置".Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                BasicConfigPage wPage = new BasicConfigPage();
                wPage.DelSave += wPage_DelSave;
                AddTabItem("设置", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wPage_DelSave()
        {
            try
            {
                LoadConfig();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 左侧导航菜单鼠标移入移出特效
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                StackPanel wStackPanel = sender as StackPanel;
                TextBlock wTextBlock = wStackPanel.Children[1] as TextBlock;
                wTextBlock.Foreground = Brushes.Yellow;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                StackPanel wStackPanel = sender as StackPanel;
                TextBlock wTextBlock = wStackPanel.Children[1] as TextBlock;
                wTextBlock.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 显示导航栏
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Grid_Navigation.Visibility = Visibility.Visible;
                Image_Navigation.Visibility = Visibility.Collapsed;

                Grid_Main.ColumnDefinitions.Clear();

                ColumnDefinition wColumnDefinition = new ColumnDefinition();
                GridLength wGridLength = new GridLength(160, GridUnitType.Pixel);
                wColumnDefinition.Width = wGridLength;
                this.Grid_Main.ColumnDefinitions.Add(wColumnDefinition);

                wColumnDefinition = new ColumnDefinition();
                wGridLength = new GridLength(1, GridUnitType.Star);
                wColumnDefinition.Width = wGridLength;
                this.Grid_Main.ColumnDefinitions.Add(wColumnDefinition);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 隐藏导航栏
        private void StackPanel_Navigation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Grid_Navigation.Visibility = Visibility.Hidden;
                Image_Navigation.Visibility = Visibility.Visible;

                Grid_Main.ColumnDefinitions.Clear();

                ColumnDefinition wColumnDefinition = new ColumnDefinition();
                GridLength wGridLength = new GridLength(0, GridUnitType.Pixel);
                wColumnDefinition.Width = wGridLength;
                this.Grid_Main.ColumnDefinitions.Add(wColumnDefinition);

                wColumnDefinition = new ColumnDefinition();
                wGridLength = new GridLength(1, GridUnitType.Star);
                wColumnDefinition.Width = wGridLength;
                this.Grid_Main.ColumnDefinitions.Add(wColumnDefinition);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 关闭子菜单
        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image wImage = sender as Image;
                StackPanel wStackPanel = wImage.Parent as StackPanel;
                TextBlock wTextBlock = wStackPanel.Children[0] as TextBlock;

                TabItem wRemoveItem = null;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    if (wHeaderUC.TB_Title.Text.Equals(wTextBlock.Text))
                        wRemoveItem = wTabItem;
                }
                TC_Main.Items.Remove(wRemoveItem);
                if (TC_Main.Items == null || TC_Main.Items.Count <= 0)
                    TC_Main.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 人工录入
        private void SP_PersonInput_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                InputData();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 人工录入
        /// </summary>
        private void InputData()
        {
            try
            {
                //添加TabItem
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                if (wConfig.CurrentPart.Contains("6"))
                {
                    ManualEntry030Page wPage = new ManualEntry030Page();
                    AddTabItem("人工录入", wPage);
                }
                else
                {
                    ManualEntryPage wPage = new ManualEntryPage();
                    AddTabItem("人工录入", wPage);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void AddTabItem(string wTitle, object wObject)
        {
            try
            {
                TabItem wTabItem = new TabItem();
                HeaderUC wHeaderUC = new HeaderUC(wTitle);
                wHeaderUC.Image_Close.MouseLeftButtonUp += Image_MouseLeftButtonUp_1;
                wTabItem.Header = wHeaderUC;

                Frame wFrame = new Frame();
                wFrame.Content = wObject;
                wTabItem.Content = wFrame;

                TC_Main.Visibility = Visibility.Visible;
                TC_Main.Items.Add(wTabItem);
                TC_Main.SelectedItem = wTabItem;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 窗体加载
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Window_Loaded_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => Window_Loaded_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();

                //定时器修改容量、内阻
                DispatcherTimer wTimer = new DispatcherTimer();
                wTimer.Interval = TimeSpan.FromMilliseconds(1000);
                wTimer.Tick += Timer_Tick;
                wTimer.Start();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 定时器
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                List<SFCModuleRecord> wList = MysqlDAO.Instance.GetSFCModuleRecordList();
                foreach (SFCModuleRecord wSFCModuleRecord in wList)
                {
                    string wGear = MysqlDAO.Instance.GetGear(wSFCModuleRecord.ProductID, Math.Round(StringUtils.ParseDouble(wSFCModuleRecord.Capacity), 2));
                    if (string.IsNullOrWhiteSpace(wGear))
                        continue;
                    MysqlDAO.Instance.UpdateModuleRecord(wGear, wSFCModuleRecord.ID);

                    int wStandardID = MysqlDAO.Instance.GetStandardID(wSFCModuleRecord.ProductID, 7);
                    if (wStandardID <= 0)
                        continue;
                    //容量
                    MysqlDAO.Instance.DeleteItem_Number(wSFCModuleRecord.SerialNumber, wStandardID);
                    MysqlDAO.Instance.InsertItem(wSFCModuleRecord, StringUtils.ParseDouble(wSFCModuleRecord.Capacity), wStandardID);
                    //内阻
                    wStandardID = MysqlDAO.Instance.GetStandardID(wSFCModuleRecord.ProductID, 8);
                    if (wStandardID <= 0)
                        continue;
                    MysqlDAO.Instance.DeleteItem_Number(wSFCModuleRecord.SerialNumber, wStandardID);
                    MysqlDAO.Instance.InsertItem(wSFCModuleRecord, StringUtils.ParseDouble(wSFCModuleRecord.InternalResistance), wStandardID);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        private void Window_Loaded_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                //启动连接池
                RunConfiguration();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Window_Loaded_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                LoginUI wLoginUI = new LoginUI();
                wLoginUI.DelUser += wLoginUI_DelUser;
                wLoginUI.ShowDialog();

                this.Show();

                InputData();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wLoginUI_DelUser(BMSEmployee wUser)
        {
            try
            {
                //①登录人渲染
                LB_User.Content = wUser.Name;
                //②全局用户渲染
                GUD.mLoginUser = wUser;
                //③加载配置
                LoadConfig();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 加载配置
        private void LoadConfig()
        {
            try
            {
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += LoadConfig_DoWork;
                wBW.RunWorkerCompleted += LoadConfig_RunWorkerCompleted;
                wBW.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadConfig_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                List<object> wObjectList = e.Result as List<object>;
                string wPartNames = wObjectList[0] as string;
                string wProductName = wObjectList[1] as string;

                LblStation.Content = wPartNames;
                Txb_ProductType.Content = wProductName;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void LoadConfig_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                List<FPCPart> wPartList = FPCPartDAO.Instance.GetPartList();
                List<FPCProduct> wProductList = FPCProductDAO.Instance.GetProductList();
                List<string> wPartIDS = wConfig.CurrentPart.Split(',').ToList();
                wPartList = wPartList.FindAll(p => wPartIDS.Exists(q => q.Equals(p.PartID.ToString())));
                string wPartNames = string.Join(",", wPartList.Select(p => p.PartName));
                FPCProduct wFPCProduct = wProductList.Find(p => p.ProductID == wConfig.CurrentProduct);

                List<object> wObjectList = new List<object>();
                wObjectList.Add(wPartNames);
                wObjectList.Add(wFPCProduct.ProductName);
                e.Result = wObjectList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        private void RunConfiguration()
        {
            try
            {
                string wAPPPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                LoggerTool.Init_LoggerPath(this.GetType().Assembly.FullName, wAPPPath, 15, 1, 1);

                LoggerTool.SaveLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", "iPlantMLM has started");

                ServiceTask.Instance.LoadConfiguration();
                ServiceTask.Instance.Start();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 统计报表
        private void SP_StatisticalReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                ReportStatisticsPage wPage = new ReportStatisticsPage();
                AddTabItem("统计报表", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 用户管理
        private void SP_UserManage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                UserManagerPage wPage = new UserManagerPage();
                AddTabItem("用户管理", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 权限管理
        private void SP_AuthorityManagement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                AuthorityManagePage wPage = new AuthorityManagePage();
                AddTabItem("权限管理", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 全局键盘事件监听
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    if (MessageBox.Show("确定退出程序吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                        return;

                    this.Close();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 检测规程
        private void SP_Standard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                ConfigManager wPage = new ConfigManager();
                AddTabItem("检测规程", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 分档规则
        private void SP_CapacityGrading_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                CapacityGradingPage wPage = new CapacityGradingPage();
                AddTabItem("分档规则", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 产品管理
        private void SP_ProductManage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel wMyStackPanel = sender as StackPanel;
                TextBlock wMyTextBlock = wMyStackPanel.Children[1] as TextBlock;

                bool wIsExist = false;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if (wMyTextBlock.Text.Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    return;

                //添加TabItem
                FPCProductPage wPage = new FPCProductPage();
                wPage.DelCheckRule += wPage_DelCheckRule;
                wPage.DelCapacityRule += wPage_DelCapacityRule;
                wPage.DelSOP += wPage_DelSOP;
                AddTabItem("产品管理", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wPage_DelSOP(FPCProduct wFPCProduct)
        {
            try
            {
                bool wIsExist = false;
                TabItem wItem = null;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if ("作业指导书".Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        wItem = wTabItem;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    TC_Main.Items.Remove(wItem);

                //添加TabItem
                SOPManager wPage = new SOPManager(wFPCProduct);
                AddTabItem("作业指导书", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wPage_DelCapacityRule(FPCProduct wFPCProduct)
        {
            try
            {
                bool wIsExist = false;
                TabItem wItem = null;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if ("分档规则".Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        wItem = wTabItem;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    TC_Main.Items.Remove(wItem);

                //添加TabItem
                CapacityGradingPage wPage = new CapacityGradingPage(wFPCProduct);
                AddTabItem("分档规则", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wPage_DelCheckRule(FPCProduct wFPCProduct)
        {
            try
            {
                bool wIsExist = false;
                TabItem wItem = null;
                foreach (TabItem wTabItem in TC_Main.Items)
                {
                    HeaderUC wHeaderUC = wTabItem.Header as HeaderUC;
                    TextBlock wTextBlock = wHeaderUC.TB_Title;
                    if ("检测规程".Equals(wTextBlock.Text))
                    {
                        wIsExist = true;
                        wItem = wTabItem;
                        TC_Main.SelectedItem = wTabItem;
                    }
                }
                if (wIsExist)
                    TC_Main.Items.Remove(wItem);

                //添加TabItem
                ConfigManager wPage = new ConfigManager(wFPCProduct);
                AddTabItem("检测规程", wPage);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 最小化
        private void CmdMin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
