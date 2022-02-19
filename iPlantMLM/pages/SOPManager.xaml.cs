using Microsoft.Win32;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    public partial class SOPManager : Page
    {
        public SOPManager()
        {
            InitializeComponent();
        }

        private FPCProduct mInputFPCProduct;
        public SOPManager(FPCProduct wFPCProduct)
        {
            InitializeComponent();
            try
            {
                mInputFPCProduct = wFPCProduct;

                TB_Title.Text = wFPCProduct.ProductName + "-工位未选择";
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
                    Xdg_MainGrid.DataSource = new List<SFCUploadSOP>();
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
                List<SFCUploadSOP> wSFCUploadSOPList = SFCUploadSOPDAO.Instance.SFC_QuerySFCUploadSOPList(-1, wFPCProduct.ProductID, -1, -1, 1, out wErrorCode);
                List<object> wObjectList = new List<object>();
                wObjectList.Add(wProductList);
                wObjectList.Add(wPartList);
                wObjectList.Add(wSFCUploadSOPList);
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
                List<SFCUploadSOP> wSFCUploadSOPList = wObjectList[2] as List<SFCUploadSOP>;

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

                        if (wSFCUploadSOPList.Exists(p => p.ProductID == wFPCProduct.ProductID && p.PartID == wFPCPart.PartID))
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
                List<SFCUploadSOP> wList = SFCUploadSOPDAO.Instance.SFC_QuerySFCUploadSOPList(-1, mFPCProduct.ProductID, mFPCPart.PartID, -1, -1, out wErrorCode);

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

                List<SFCUploadSOP> wList = e.Result as List<SFCUploadSOP>;
                Xdg_MainGrid.DataSource = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 上传
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 7001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (mFPCPart == null)
                {
                    MessageBox.Show("工位未选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                OpenFileDialog wOpenFileDialog = new OpenFileDialog();
                wOpenFileDialog.Filter = "PDF,JPG (.pdf,.jpg,.jpeg,.JPG)|*.pdf;*.jpg;*.jpeg;*.JPG";
                wOpenFileDialog.Multiselect = false;
                if (wOpenFileDialog.ShowDialog() != true)
                    return;

                //①文件名
                string wFileName = wOpenFileDialog.FileName.Substring(wOpenFileDialog.FileName.LastIndexOf(@"\") + 1);
                //②文件路径
                string wFilePath = wOpenFileDialog.FileName;
                //③类型
                int wType = 1;
                if (wFileName.Contains("jpg") || wFileName.Contains("JPG") || wFileName.Contains("jpeg"))
                    wType = 2;
                //④产品
                int wProductID = mFPCProduct.ProductID;
                //⑤工位
                int wPartID = mFPCPart.PartID;

                SFCUploadSOP wSFCUploadSOP = new SFCUploadSOP();
                wSFCUploadSOP.Active = 0;
                wSFCUploadSOP.FileName = wFileName;
                wSFCUploadSOP.FilePath = wFilePath;
                wSFCUploadSOP.ID = 0;
                wSFCUploadSOP.OperatorID = GUD.mLoginUser.ID;
                wSFCUploadSOP.PartID = wPartID;
                wSFCUploadSOP.ProductID = wProductID;
                wSFCUploadSOP.Type = wType;
                wSFCUploadSOP.UploadTime = DateTime.Now;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Btn_Add_Click_DoWork(s, exc, wSFCUploadSOP);
                wBW.RunWorkerCompleted += (s, exc) => Btn_Add_Click_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Btn_Add_Click_DoWork(object s, DoWorkEventArgs e, SFCUploadSOP wSFCUploadSOP)
        {
            try
            {
                string wSharePath = ConfigurationManager.AppSettings["ShareFolerPath"];
                string wShareUser = ConfigurationManager.AppSettings["ShareFolerUserName"];
                string wSharePass = ConfigurationManager.AppSettings["ShareFolerPassword"];
                bool wFlag = FileTool.Instance.UpLoadFile2(wSFCUploadSOP.FilePath, wSharePath, wShareUser, wSharePass, 1);

                if (wFlag)
                {
                    int wErrorCode = 0;
                    wSFCUploadSOP.FilePath = wSharePath + @"\" + wSFCUploadSOP.FileName;
                    SFCUploadSOPDAO.Instance.SFC_SaveSFCUploadSOP(wSFCUploadSOP, out wErrorCode);
                }

                e.Result = wFlag;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_Add_Click_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                {
                    LoadTable();
                    MessageBox.Show("上传成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("上传失败，请联系管理员!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                SFCUploadSOP wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCUploadSOP;
                if (wSelectedDisplay == null)
                    return;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 激活
        private void Btn_Active_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 7002))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SFCUploadSOP wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCUploadSOP;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要激活的指导文件!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认激活吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                List<SFCUploadSOP> wList = Xdg_MainGrid.DataSource as List<SFCUploadSOP>;
                int wErrorCode = 0;
                foreach (SFCUploadSOP wSFCUploadSOP in wList)
                {
                    wSFCUploadSOP.Active = 0;
                    SFCUploadSOPDAO.Instance.SFC_SaveSFCUploadSOP(wSFCUploadSOP, out wErrorCode);
                }

                wSelectedDisplay.Active = 1;
                wSelectedDisplay.ValidTime = DateTime.Now;
                SFCUploadSOPDAO.Instance.SFC_SaveSFCUploadSOP(wSelectedDisplay, out wErrorCode);

                LoadTable();
                PaintTree();
                MessageBox.Show("激活成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 7003))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SFCUploadSOP wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCUploadSOP;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要关闭的指导文件!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认关闭吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                int wErrorCode = 0;
                wSelectedDisplay.Active = 0;
                SFCUploadSOPDAO.Instance.SFC_SaveSFCUploadSOP(wSelectedDisplay, out wErrorCode);

                LoadTable();
                PaintTree();
                MessageBox.Show("关闭成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 查看
        private void Btn_See_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 7004))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SFCUploadSOP wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCUploadSOP;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SOPFileLookWindow wUI = new SOPFileLookWindow(wSelectedDisplay);
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
