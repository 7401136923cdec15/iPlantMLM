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
    /// ConfigManager.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigManager : Page
    {
        public ConfigManager()
        {
            InitializeComponent();
        }

        private FPCProduct mInputFPCProduct;
        public ConfigManager(FPCProduct wFPCProduct)
        {
            InitializeComponent();
            try
            {
                mInputFPCProduct = wFPCProduct;
                //PaintTree();
                //if (mFPCPart == null)
                //    Xdg_MainGrid.DataSource = new List<IPTStandard>();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 页面加载
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PaintTree();

                if (mFPCPart == null)
                    Xdg_MainGrid.DataSource = new List<IPTStandard>();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void PaintTree()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => PaintTree_DoWork(s, exc, mInputFPCProduct);
                wBW.RunWorkerCompleted += (s, exc) => PaintTree_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void PaintTree_DoWork(object s, DoWorkEventArgs e, FPCProduct wFPCProduct)
        {
            try
            {
                int wErrorCode = 0;
                List<FPCProduct> wProductList = FPCProductDAO.Instance.GetProductList();
                if (wFPCProduct != null)
                    wProductList = wProductList.FindAll(p => p.ProductID == wFPCProduct.ProductID).ToList();

                List<FPCPart> wPartList = FPCPartDAO.Instance.GetPartList();
                List<IPTStandard> wStandardList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, -1, -1, -1, -1, -1, 1, out wErrorCode);
                List<object> wObjectList = new List<object>();
                wObjectList.Add(wProductList);
                wObjectList.Add(wPartList);
                wObjectList.Add(wStandardList);
                e.Result = wObjectList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void PaintTree_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<object> wObjectList = e.Result as List<object>;
                List<FPCProduct> wProductList = wObjectList[0] as List<FPCProduct>;
                List<FPCPart> wPartList = wObjectList[1] as List<FPCPart>;
                List<IPTStandard> wStandardList = wObjectList[2] as List<IPTStandard>;

                TV_Main.Children.Clear();
                int wIndex = 1;
                foreach (FPCProduct wFPCProduct in wProductList)
                {
                    MyTreeViewItem wTreeViewItem = null;
                    if (wIndex++ == 1)
                        wTreeViewItem = new MyTreeViewItem(wFPCProduct.ProductName, true);
                    else
                        wTreeViewItem = new MyTreeViewItem(wFPCProduct.ProductName, false);
                    StackPanel wStackPanel = new StackPanel();
                    wStackPanel.Orientation = Orientation.Vertical;
                    wTreeViewItem.Grid_Main.Children.Add(wStackPanel);
                    foreach (FPCPart wFPCPart in wPartList)
                    {
                        TextBlock wTextBlock = new TextBlock();
                        wTextBlock.MouseLeftButtonUp += (s, ex) => ItemClick(wTextBlock, wFPCProduct, wFPCPart, ex);
                        wTextBlock.Cursor = Cursors.Hand;
                        wTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                        wTextBlock.Text = string.Format("({1}){0}", wFPCPart.PartName, wFPCPart.PartCode);
                        wTextBlock.Margin = new Thickness(5, 5, 0, 5);

                        if (wStandardList.Exists(p => p.ProductID == wFPCProduct.ProductID && p.PartID == wFPCPart.PartID))
                        {
                            wTextBlock.Background = Brushes.Green;
                            wTextBlock.Foreground = Brushes.White;
                        }

                        wStackPanel.Children.Add(wTextBlock);
                    }
                    TV_Main.Children.Add(wTreeViewItem);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private FPCProduct mFPCProduct;
        private FPCPart mFPCPart;
        private void ItemClick(TextBlock wTextBlock, FPCProduct wFPCProduct, FPCPart wFPCPart, MouseButtonEventArgs e)
        {
            try
            {
                TB_Title.Text = string.Format("{0}-{1}", wFPCProduct.ProductName, wFPCPart.PartName);
                mFPCProduct = wFPCProduct;
                mFPCPart = wFPCPart;

                LoadTable();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 渲染中间表格
        /// </summary>
        private void LoadTable()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => RefreshStandard_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => RefreshStandard_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }

        }
        private void RefreshStandard_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                int wErrorCode = 0;
                List<IPTStandard> wList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, mFPCProduct.ProductID, -1, -1, mFPCPart.PartID, -1, -1, out wErrorCode);
                wList = wList.OrderBy(p => p.OrderID).ToList();
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void RefreshStandard_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<IPTStandard> wList = e.Result as List<IPTStandard>;
                Xdg_MainGrid.DataSource = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 新增
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 3001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (mFPCPart == null)
                {
                    MessageBox.Show("请选中产品-工位!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                AddStandardWindow wUI = new AddStandardWindow(mFPCProduct, mFPCPart);
                wUI.DelSave += wUI_DelSave;
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        void wUI_DelSave()
        {
            try
            {
                PaintTree();
                LoadTable();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 上移
        private void Btn_Up_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 3002))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                IPTStandard wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTStandard;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要上移的数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (wSelectedDisplay.OrderID == 1)
                {
                    MessageBox.Show("到顶了!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                int wOrderID = wSelectedDisplay.OrderID;

                List<IPTStandard> wList = this.Xdg_MainGrid.DataSource as List<IPTStandard>;
                IPTStandard wStandard = wList.Find(p => p.OrderID == wSelectedDisplay.OrderID - 1);
                wStandard.OrderID = wOrderID;
                wSelectedDisplay.OrderID = wOrderID - 1;
                wStandard.Editor = GUD.mLoginUser.Name;
                wStandard.EditTimeText = DateTime.Now.ToString("yyyy/MM/dd");
                wSelectedDisplay.Editor = GUD.mLoginUser.Name;
                wSelectedDisplay.EditTimeText = DateTime.Now.ToString("yyyy/MM/dd");

                wList = wList.OrderBy(p => p.OrderID).ToList();
                Xdg_MainGrid.DataSource = null;
                Xdg_MainGrid.DataSource = wList;
                Xdg_MainGrid.SelectedDataItem = wSelectedDisplay;
                Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem = wSelectedDisplay;

                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Move_DoWork(s, exc, wStandard, wSelectedDisplay);
                wBW.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Move_DoWork(object s, DoWorkEventArgs e, IPTStandard wIPTStandard, IPTStandard wSelectedDisplay)
        {
            try
            {
                int wErrorCode = 0;
                wIPTStandard.EditorID = GUD.mLoginUser.ID;
                wIPTStandard.EditTime = DateTime.Now;
                IPTStandardDAO.Instance.IPT_SaveIPTStandard(wIPTStandard, out wErrorCode);

                wSelectedDisplay.EditorID = GUD.mLoginUser.ID;
                wSelectedDisplay.EditTime = DateTime.Now;
                IPTStandardDAO.Instance.IPT_SaveIPTStandard(wSelectedDisplay, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Move_DoWork_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                LoadTable();


            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 下移
        private void Btn_Down_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 3003))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                IPTStandard wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTStandard;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要下移的数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                List<IPTStandard> wList = this.Xdg_MainGrid.DataSource as List<IPTStandard>;
                int wMaxOrderID = wList.Max(p => p.OrderID);

                if (wSelectedDisplay.OrderID == wMaxOrderID)
                {
                    MessageBox.Show("到底了!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                int wOrderID = wSelectedDisplay.OrderID;

                IPTStandard wStandard = wList.Find(p => p.OrderID == wSelectedDisplay.OrderID + 1);
                wStandard.OrderID = wOrderID;
                wSelectedDisplay.OrderID = wOrderID + 1;
                wStandard.Editor = GUD.mLoginUser.Name;
                wStandard.EditTimeText = DateTime.Now.ToString("yyyy/MM/dd");
                wSelectedDisplay.Editor = GUD.mLoginUser.Name;
                wSelectedDisplay.EditTimeText = DateTime.Now.ToString("yyyy/MM/dd");

                wList = wList.OrderBy(p => p.OrderID).ToList();
                Xdg_MainGrid.DataSource = null;
                Xdg_MainGrid.DataSource = wList;
                Xdg_MainGrid.SelectedDataItem = wSelectedDisplay;
                Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem = wSelectedDisplay;

                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Move_DoWork(s, exc, wStandard, wSelectedDisplay);
                wBW.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 保存
        //private void Btn_Save_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
        //    }
        //}
        #endregion

        #region 激活
        private void Btn_Active_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 3004))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                IPTStandard wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTStandard;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要激活的数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认激活吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                wSelectedDisplay.Active = 1;
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Active_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => Active_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Active_DoWork(object s, DoWorkEventArgs e, IPTStandard wSelectedDisplay)
        {
            try
            {
                int wErrorCode = 0;
                wSelectedDisplay.EditorID = GUD.mLoginUser.ID;
                wSelectedDisplay.EditTime = DateTime.Now;
                IPTStandardDAO.Instance.IPT_SaveIPTStandard(wSelectedDisplay, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Active_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                PaintTree();
                LoadTable();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 关闭
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 3005))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                IPTStandard wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTStandard;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要关闭的数据!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认关闭吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                wSelectedDisplay.Active = 2;
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Active_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => Active_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 表格单击
        private void TableClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                IPTStandard wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTStandard;
                if (wSelectedDisplay == null)
                    return;

                AddStandardWindow wUI = new AddStandardWindow(wSelectedDisplay);
                wUI.DelSave += wUI_DelSave;
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
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 3006))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (mFPCPart == null)
                {
                    MessageBox.Show("请选择产品-工位!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                int wErrorCode = ExcelOutPutTool.Intance.ExcelExportData(TB_Title.Text + "-检测规程", Xdg_MainGrid);
                if (wErrorCode != 0)
                    MessageBox.Show("导出出错！错误码：" + wErrorCode.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
