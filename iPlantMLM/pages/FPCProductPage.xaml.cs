using Microsoft.Win32;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
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
    /// UserManagerPage.xaml 的交互逻辑
    /// </summary>
    public partial class FPCProductPage : Page
    {
        /// <summary>
        /// 检测规程跳转
        /// </summary>
        /// <param name="wFPCProduct"></param>
        public delegate void DeleCheckRule(FPCProduct wFPCProduct);
        public event DeleCheckRule DelCheckRule;

        /// <summary>
        /// 分档规则跳转
        /// </summary>
        /// <param name="wFPCProduct"></param>
        public delegate void DeleCapacityRule(FPCProduct wFPCProduct);
        public event DeleCheckRule DelCapacityRule;

        /// <summary>
        /// 作业指导书跳转
        /// </summary>
        /// <param name="wFPCProduct"></param>
        public delegate void DeleSOP(FPCProduct wFPCProduct);
        public event DeleSOP DelSOP;

        public FPCProductPage()
        {
            InitializeComponent();
        }

        #region 页面加载
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadTable();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadTable()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Page_Loaded_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => Page_Loaded_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Page_Loaded_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                List<FPCProduct> wList = FPCProductDAO.Instance.GetProductList();
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Page_Loaded_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<FPCProduct> wList = e.Result as List<FPCProduct>;
                Xdg_MainGrid.DataSource = wList;
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
                FPCProduct wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as FPCProduct;
                if (wSelectedDisplay == null)
                    return;
                mFPCProduct = wSelectedDisplay;
                Tbx_UserName.Text = wSelectedDisplay.ProductName;
                Tbx_ProName.Text = wSelectedDisplay.Name;
                Tbx_Des.Text = wSelectedDisplay.DescribeInfo;
                Tbx_Model.Text = wSelectedDisplay.Model;
                Tbx_ProductCode.Text = wSelectedDisplay.ProductCode;
                Tbx_CodePrefix.Text = wSelectedDisplay.BarCodePrefix;
                Tbx_PackagePrefix.Text = wSelectedDisplay.PackagePrefix;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region Excel导入
        private void Cmd_Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 1002))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
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
                wBW.DoWork += (s, exc) => ImportUser_DoWork(s, exc, wFilePath);
                wBW.RunWorkerCompleted += (s, exc) => ImportUser_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void ImportUser_DoWork(object s, DoWorkEventArgs e, string wFilePath)
        {
            try
            {
                DataTable wDataTable = ExcelTool.Instance.ExcelToDatatable(wFilePath);
                List<BMSEmployee> wUserList = new List<BMSEmployee>();
                foreach (DataRow wRowData in wDataTable.Rows)
                {
                    BMSEmployee wUser = new BMSEmployee();
                    wUser.LoginID = wRowData[0].ToString();
                    wUser.Name = wRowData[1].ToString();
                    wUser.Phone = wRowData[2].ToString();
                    wUser.ID = 0;
                    wUser.Active = 1;
                    wUser.Password = ShrisDES.EncryptDESString("123456", "shrismcis");
                    wUser.Operator = "";
                    wUser.Email = "";
                    wUser.WeiXin = "";
                    wUser.Grad = 2;
                    wUser.LoginName = wRowData[0].ToString();
                    wUserList.Add(wUser);
                }
                int wErrorCode = 0;
                foreach (BMSEmployee wBMSEmployee in wUserList)
                {
                    if (string.IsNullOrWhiteSpace(wBMSEmployee.LoginID))
                        continue;
                    List<BMSEmployee> wEList = BMSEmployeeDAO.Instance.BMS_QueryBMSEmployeeList(-1, "", "", wBMSEmployee.LoginID, -1, out wErrorCode);
                    if (wEList.Count > 0)
                    {
                        foreach (BMSEmployee wMyUser in wEList)
                        {
                            wMyUser.Name = wBMSEmployee.Name;
                            wMyUser.Phone = wBMSEmployee.Phone;
                            BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wMyUser, out wErrorCode);
                        }
                    }
                    else
                        BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wBMSEmployee, out wErrorCode);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void ImportUser_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                MessageBox.Show("导入成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region Excel导出
        private void Cmd_ResetPsw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 5004))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                int wErrorCode = ExcelOutPutTool.Intance.ExcelExportData("产品规格", Xdg_MainGrid);
                if (wErrorCode != 0)
                    MessageBox.Show("导出出错！错误码：" + wErrorCode.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 修改
        private FPCProduct mFPCProduct = new FPCProduct();
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 5002))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //①产品规格
                string wGear = Tbx_UserName.Text;
                if (string.IsNullOrWhiteSpace(wGear))
                {
                    MessageBox.Show("产品规格不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (mFPCProduct.ProductID <= 0)
                {
                    MessageBox.Show("请选中需要修改的产品规格!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                mFPCProduct.ProductName = Tbx_UserName.Text;
                mFPCProduct.Name = Tbx_ProName.Text;
                mFPCProduct.DescribeInfo = Tbx_Des.Text;
                mFPCProduct.Model = Tbx_Model.Text;
                mFPCProduct.ProductCode = Tbx_ProductCode.Text;
                mFPCProduct.BarCodePrefix = Tbx_CodePrefix.Text;
                mFPCProduct.PackagePrefix = Tbx_PackagePrefix.Text;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Save_DoWork(s, exc, mFPCProduct);
                wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Save_DoWork(object s, DoWorkEventArgs e, FPCProduct wFPCProduct)
        {
            try
            {
                List<FPCProduct> wList = FPCProductDAO.Instance.GetProductList();

                string wMsg = "";
                if (wFPCProduct.ProductID <= 0)
                {
                    if (wList.Exists(p => p.ProductName.Equals(wFPCProduct.ProductName)))
                    {
                        e.Result = "该产品规格已存在!";
                        return;
                    }
                    wFPCProduct.ProductID = wList.Max(p => p.ProductID) + 1;
                    wList.Add(wFPCProduct);
                }
                else
                {
                    foreach (FPCProduct wItem in wList)
                    {
                        if (wItem.ProductID != wFPCProduct.ProductID)
                            continue;
                        wItem.ProductName = wFPCProduct.ProductName;
                        wItem.Name = wFPCProduct.Name;
                        wItem.Model = wFPCProduct.Model;
                        wItem.DescribeInfo = wFPCProduct.DescribeInfo;
                        wItem.ProductCode = wFPCProduct.ProductCode;
                        wItem.BarCodePrefix = wFPCProduct.BarCodePrefix;
                        wItem.PackagePrefix = wFPCProduct.PackagePrefix;
                    }
                }

                FPCProductDAO.Instance.SetProductList(wList);
                e.Result = wMsg;
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

                string wMsg = e.Result as string;
                if (string.IsNullOrWhiteSpace(wMsg))
                {
                    LoadTable();
                    MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show(wMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 删除
        private void Cmd_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 5003))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (mFPCProduct.ProductID <= 0)
                {
                    MessageBox.Show("请选中需要删除的产品规格!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                List<FPCProduct> wList = FPCProductDAO.Instance.GetProductList();
                wList.RemoveAll(p => p.ProductID == mFPCProduct.ProductID);
                FPCProductDAO.Instance.SetProductList(wList);

                LoadTable();
                MessageBox.Show("删除成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 新增
        private void Cmd_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 5001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //①产品规格
                string wGear = Tbx_UserName.Text;
                if (string.IsNullOrWhiteSpace(wGear))
                {
                    MessageBox.Show("产品规格不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认新增吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                FPCProduct wFPCProduct = new FPCProduct();
                wFPCProduct.ProductName = Tbx_UserName.Text;
                wFPCProduct.Name = Tbx_ProName.Text;
                wFPCProduct.DescribeInfo = Tbx_Des.Text;
                wFPCProduct.Model = Tbx_Model.Text;
                wFPCProduct.ProductCode = Tbx_ProductCode.Text;
                wFPCProduct.BarCodePrefix = Tbx_CodePrefix.Text;
                wFPCProduct.PackagePrefix = Tbx_PackagePrefix.Text;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wFPCProduct);
                wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 分档规则
        private void Cmd_CapacityRule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FPCProduct wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as FPCProduct;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中产品规格!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (DelCapacityRule != null)
                    DelCapacityRule(wSelectedDisplay);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 检测规程
        private void Cmd_CheckRule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FPCProduct wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as FPCProduct;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中产品规格!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (DelCheckRule != null)
                    DelCheckRule(wSelectedDisplay);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 作业指导书
        private void Cmd_SOP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FPCProduct wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as FPCProduct;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中产品规格!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (DelSOP != null)
                    DelSOP(wSelectedDisplay);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
