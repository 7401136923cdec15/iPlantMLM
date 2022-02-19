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
    public partial class CapacityGradingPage : Page
    {
        public CapacityGradingPage()
        {
            InitializeComponent();
        }

        private FPCProduct mFPCProduct;
        public CapacityGradingPage(FPCProduct wFPCProduct)
        {
            InitializeComponent();
            try
            {
                mFPCProduct = wFPCProduct;
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
                LoadTable();

                //加载产品规格
                List<FPCProduct> wList = FPCProductDAO.Instance.GetProductList();
                Cbb_Role.ItemsSource = wList;
                Cbb_Role.SelectedIndex = 0;
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
                int wErrorCode = 0;
                List<IPTCapacityGrading> wList = IPTCapacityGradingDAO.Instance.IPT_QueryIPTCapacityGradingList(-1, "", out wErrorCode);
                if (mFPCProduct != null)
                    wList = wList.FindAll(p => p.ProductID == mFPCProduct.ProductID).ToList();
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

                List<IPTCapacityGrading> wList = e.Result as List<IPTCapacityGrading>;
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
                IPTCapacityGrading wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTCapacityGrading;
                if (wSelectedDisplay == null)
                    return;
                Cbb_Role.SelectedValue = wSelectedDisplay.ProductID;
                Tbx_UserName.Text = wSelectedDisplay.Gear;
                Tbx_WorkNo.Text = wSelectedDisplay.LowerLimitText;
                Tbx_Phone.Text = wSelectedDisplay.UpLimitText;
                Tbx_Explain.Text = wSelectedDisplay.Explain;
                mIPTCapacityGrading = wSelectedDisplay;
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
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 6004))
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
                List<IPTCapacityGrading> wUserList = new List<IPTCapacityGrading>();
                List<FPCProduct> wList = FPCProductDAO.Instance.GetProductList();
                foreach (DataRow wRowData in wDataTable.Rows)
                {
                    IPTCapacityGrading wUser = new IPTCapacityGrading();
                    wUser.ProductName = wRowData[0].ToString();
                    wUser.ProductID = wList.Exists(p => p.ProductName.Equals(wUser.ProductName)) ? wList.Find(p => p.ProductName.Equals(wUser.ProductName)).ProductID : 0;
                    wUser.Gear = wRowData[1].ToString();
                    double wLowerLimit = 0;
                    double.TryParse(wRowData[2].ToString(), out wLowerLimit);
                    wUser.LowerLimit = wLowerLimit;
                    wUser.ID = 0;
                    double wUpLimit = 0;
                    double.TryParse(wRowData[3].ToString(), out wUpLimit);
                    wUser.UpLimit = wUpLimit;
                    wUser.Explain = wRowData[4].ToString();
                    wUserList.Add(wUser);
                }
                int wErrorCode = 0;
                foreach (IPTCapacityGrading wBMSEmployee in wUserList)
                {
                    if (string.IsNullOrWhiteSpace(wBMSEmployee.Gear) || wBMSEmployee.ProductID <= 0)
                        continue;
                    List<IPTCapacityGrading> wEList = IPTCapacityGradingDAO.Instance.IPT_QueryIPTCapacityGradingList(wBMSEmployee.ProductID, wBMSEmployee.Gear, out wErrorCode);
                    if (wEList.Count > 0)
                        continue;
                    else
                        IPTCapacityGradingDAO.Instance.IPT_SaveIPTCapacityGrading(wBMSEmployee, out wErrorCode);
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

                LoadTable();
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
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 6005))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                int wErrorCode = ExcelOutPutTool.Intance.ExcelExportData("容量分档规则", Xdg_MainGrid);
                if (wErrorCode != 0)
                    MessageBox.Show("导出出错！错误码：" + wErrorCode.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 新增
        private IPTCapacityGrading mIPTCapacityGrading = new IPTCapacityGrading();
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 6001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //①产品规格
                int wProductID = (int)Cbb_Role.SelectedValue;
                //②档位
                string wGear = Tbx_UserName.Text;
                if (string.IsNullOrWhiteSpace(wGear))
                {
                    MessageBox.Show("档位不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //③容量下限
                double wLowerLimit = 0;
                if (!double.TryParse(Tbx_WorkNo.Text, out wLowerLimit))
                {
                    MessageBox.Show("容量下限输入值有误!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //④容量上限
                double wUpLimit = 0;
                if (!double.TryParse(Tbx_Phone.Text, out wUpLimit))
                {
                    MessageBox.Show("容量上限输入值有误!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //⑤说明
                string wExplain = Tbx_Explain.Text;

                mIPTCapacityGrading.ID = 0;
                mIPTCapacityGrading.ProductID = wProductID;
                mIPTCapacityGrading.Gear = wGear;
                mIPTCapacityGrading.LowerLimit = wLowerLimit;
                mIPTCapacityGrading.UpLimit = wUpLimit;
                mIPTCapacityGrading.Explain = wExplain;

                if (MessageBox.Show("确认新增吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Save_DoWork(s, exc, mIPTCapacityGrading);
                wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Save_DoWork(object s, DoWorkEventArgs e, IPTCapacityGrading wIPTCapacityGrading)
        {
            try
            {
                string wMsg = "";
                int wErrorCode = 0;
                if (wIPTCapacityGrading.ID <= 0)
                {
                    List<IPTCapacityGrading> wList = IPTCapacityGradingDAO.Instance.IPT_QueryIPTCapacityGradingList(wIPTCapacityGrading.ProductID, wIPTCapacityGrading.Gear, out wErrorCode);
                    if (wList.Count > 0)
                    {
                        wMsg = "该产品规格，该档位已存在!";
                        e.Result = wMsg;
                        return;
                    }
                }
                IPTCapacityGradingDAO.Instance.IPT_SaveIPTCapacityGrading(wIPTCapacityGrading, out wErrorCode);
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
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 6003))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                IPTCapacityGrading wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTCapacityGrading;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要删除的项!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                List<IPTCapacityGrading> wList = new List<IPTCapacityGrading>();
                wList.Add(wSelectedDisplay);
                IPTCapacityGradingDAO.Instance.IPT_DeleteIPTCapacityGradingList(wList);

                LoadTable();
                MessageBox.Show("删除成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 修改
        private void Cmd_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 6002))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                IPTCapacityGrading wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as IPTCapacityGrading;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中需要修改的项!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //①产品规格
                int wProductID = (int)Cbb_Role.SelectedValue;
                //②档位
                string wGear = Tbx_UserName.Text;
                if (string.IsNullOrWhiteSpace(wGear))
                {
                    MessageBox.Show("档位不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //③容量下限
                double wLowerLimit = 0;
                if (!double.TryParse(Tbx_WorkNo.Text, out wLowerLimit))
                {
                    MessageBox.Show("容量下限输入值有误!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //④容量上限
                double wUpLimit = 0;
                if (!double.TryParse(Tbx_Phone.Text, out wUpLimit))
                {
                    MessageBox.Show("容量上限输入值有误!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //⑤说明
                string wExplain = Tbx_Explain.Text;

                wSelectedDisplay.ProductID = wProductID;
                wSelectedDisplay.Gear = wGear;
                wSelectedDisplay.LowerLimit = wLowerLimit;
                wSelectedDisplay.UpLimit = wUpLimit;
                wSelectedDisplay.Explain = wExplain;

                if (MessageBox.Show("确认修改吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wSelectedDisplay);
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
    }
}
