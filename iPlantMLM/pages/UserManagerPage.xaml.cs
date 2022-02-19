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
    public partial class UserManagerPage : Page
    {
        public UserManagerPage()
        {
            InitializeComponent();
        }

        #region 全局变量
        #endregion

        #region 页面加载
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                QueryUserAll();
                LoadSource();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadSource()
        {
            try
            {
                //激活状态
                Dictionary<int, string> wActiveDic = new Dictionary<int, string>();
                wActiveDic.Add(1, "激活");
                wActiveDic.Add(2, "关闭");
                Cbb_Active.ItemsSource = wActiveDic;
                Cbb_Active.SelectedIndex = 0;
                //角色渲染
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => LoadRole_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => LoadRole_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadRole_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                int wErrorCode = 0;
                List<MBSRole> wRoleList = MBSRoleDAO.Instance.MBS_QueryMBSRoleList(-1, 1, "", out wErrorCode);
                e.Result = wRoleList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadRole_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<MBSRole> wRoleList = e.Result as List<MBSRole>;
                Cbb_Role.ItemsSource = wRoleList;
                Cbb_Role.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void QueryUserAll()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => QueryUserAll_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => QueryUserAll_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void QueryUserAll_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                int wErrorCode = 0;
                List<BMSEmployee> wUserList = BMSEmployeeDAO.Instance.BMS_QueryBMSEmployeeList(-1, "", "", "", -1, out wErrorCode);
                e.Result = wUserList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void QueryUserAll_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<BMSEmployee> wUserList = e.Result as List<BMSEmployee>;
                Xdg_MainGrid.DataSource = wUserList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 创建用户
        private void Cmd_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 1001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Tbx_UserName.Text))
                {
                    MessageBox.Show("用户名不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Tbx_WorkNo.Text))
                {
                    MessageBox.Show("工号不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string wLoginID = Tbx_WorkNo.Text;

                //判断工号是否存在
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => LoginID_DoWork(s, exc, wLoginID);
                wBW.RunWorkerCompleted += (s, exc) => LoginID_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void LoginID_DoWork(object s, DoWorkEventArgs e, string wLoginID)
        {
            try
            {
                int wErrorCode = 0;
                List<BMSEmployee> wUserList = BMSEmployeeDAO.Instance.BMS_QueryBMSEmployeeList(-1, "", "", wLoginID, -1, out wErrorCode);
                if (wUserList.Count > 0)
                    e.Result = true;
                else
                    e.Result = false;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoginID_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                {
                    MessageBox.Show("该工号已存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认创建用户吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                string wLoginID = Tbx_WorkNo.Text;
                string wName = Tbx_UserName.Text;
                int wActive = (int)Cbb_Active.SelectedValue;
                string wPhone = Tbx_Phone.Text;
                string wPassword = ShrisDES.EncryptDESString("123456", "shrismcis");
                BMSEmployee wUser = new BMSEmployee();
                wUser.ID = 0;
                wUser.LoginName = wLoginID;
                wUser.LoginID = wLoginID;
                wUser.Name = wName;
                wUser.Active = wActive;
                wUser.Phone = wPhone;
                wUser.Password = wPassword;
                wUser.Operator = "";
                wUser.Email = "";
                wUser.WeiXin = "";
                wUser.Grad = (int)Cbb_Role.SelectedValue;
                wUser.PartPower = GetPartPower();

                MyLoading wMyLoading1 = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => AddUser_DoWork(s, exc, wUser);
                wBW.RunWorkerCompleted += (s, exc) => AddUser_RunWorkerCompleted(exc, wMyLoading1);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void AddUser_DoWork(object s, DoWorkEventArgs e, BMSEmployee wUser)
        {
            try
            {
                int wErrorCode = 0;
                BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wUser, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void AddUser_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                QueryUserAll();
                MessageBox.Show("创建用户成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 关闭用户
        private void Cmd_Disable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 1003))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BMSEmployee wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as BMSEmployee;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要关闭的用户!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认关闭该用户吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => CloseUser_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => CloseUser_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void CloseUser_DoWork(object s, DoWorkEventArgs e, BMSEmployee wSelectedDisplay)
        {
            try
            {
                wSelectedDisplay.Active = 2;
                int wErrorCode = 0;
                BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wSelectedDisplay, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void CloseUser_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                QueryUserAll();
                MessageBox.Show("关闭成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 激活用户
        private void Cmd_Active_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 1004))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BMSEmployee wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as BMSEmployee;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要激活的用户!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认激活该用户吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => ActiveUser_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => ActiveUser_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void ActiveUser_DoWork(object s, DoWorkEventArgs e, BMSEmployee wSelectedDisplay)
        {
            try
            {
                wSelectedDisplay.Active = 1;
                int wErrorCode = 0;
                BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wSelectedDisplay, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void ActiveUser_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                QueryUserAll();
                MessageBox.Show("激活成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 保存用户
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 1005))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Tbx_UserName.Text))
                {
                    MessageBox.Show("用户名不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Tbx_WorkNo.Text))
                {
                    MessageBox.Show("工号不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BMSEmployee wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as BMSEmployee;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要修改的用户!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //获取工位权限
                string wPartPower = GetPartPower();

                //判断工号是否和其他重复
                string wLoginID = Tbx_WorkNo.Text;
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => LoginIDJudge_DoWork(s, exc, wLoginID, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => LoginIDJudge_RunWorkerCompleted(exc, wMyLoading, wSelectedDisplay, wPartPower);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private string GetPartPower()
        {
            string wResult = "";
            try
            {
                List<string> wList = new List<string>();
                foreach (StackPanel wStackPanel in SP_StationPower.Children)
                {
                    CheckBox wCheckBox = wStackPanel.Children[0] as CheckBox;
                    if (wCheckBox.IsChecked == true)
                        wList.Add(wCheckBox.Tag as string);
                }
                wResult = string.Join(",", wList);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
        private void LoginIDJudge_DoWork(object s, DoWorkEventArgs e, string wLoginID, BMSEmployee wSelectedDisplay)
        {
            try
            {
                int wErrorCode;
                List<BMSEmployee> wUserList = BMSEmployeeDAO.Instance.BMS_QueryBMSEmployeeList(-1, "", "", wLoginID, -1, out wErrorCode);
                if (wUserList.Exists(p => p.ID != wSelectedDisplay.ID))
                    e.Result = true;
                else
                    e.Result = false;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoginIDJudge_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, BMSEmployee wSelectedDisplay, string wPartPower)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                {
                    MessageBox.Show("该工号已存在，不允许修改!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                string wLoginID = Tbx_WorkNo.Text;
                string wName = Tbx_UserName.Text;
                int wActive = (int)Cbb_Active.SelectedValue;
                string wPhone = Tbx_Phone.Text;
                wSelectedDisplay.LoginID = wLoginID;
                wSelectedDisplay.Name = wName;
                wSelectedDisplay.Active = wActive;
                wSelectedDisplay.Phone = wPhone;
                wSelectedDisplay.Grad = (int)Cbb_Role.SelectedValue;
                wSelectedDisplay.PartPower = wPartPower;

                MyLoading wMyLoading1 = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => UpdateUser_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => TUpdateUser_RunWorkerCompleted(exc, wMyLoading1);
                wBW.RunWorkerAsync();
                wMyLoading1.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void UpdateUser_DoWork(object s, DoWorkEventArgs e, BMSEmployee wUser)
        {
            try
            {
                int wErrorCode = 0;
                BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wUser, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void TUpdateUser_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                QueryUserAll();
                MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 重置密码
        private void Cmd_ResetPsw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 1006))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                BMSEmployee wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as BMSEmployee;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要重置的用户!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认重置该用户的密码吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => ResetUser_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => ResetUserr_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void ResetUser_DoWork(object s, DoWorkEventArgs e, BMSEmployee wSelectedDisplay)
        {
            try
            {
                wSelectedDisplay.Password = ShrisDES.EncryptDESString("123456", "shrismcis");
                int wErrorCode = 0;
                BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(wSelectedDisplay, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void ResetUserr_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                QueryUserAll();
                MessageBox.Show("重置成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                BMSEmployee wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as BMSEmployee;
                if (wSelectedDisplay == null)
                    return;
                Tbx_UserName.Text = wSelectedDisplay.Name;
                Tbx_WorkNo.Text = wSelectedDisplay.LoginID;
                Tbx_Phone.Text = wSelectedDisplay.Phone;
                Cbb_Active.SelectedIndex = wSelectedDisplay.Active == 1 ? 0 : 1;
                Cbb_Role.SelectedValue = wSelectedDisplay.Grad;
                //渲染右侧工位权限
                List<string> wPartIDList = wSelectedDisplay.PartPower.Split(',').ToList();
                foreach (StackPanel wStackPanel in SP_StationPower.Children)
                {
                    CheckBox wCheckBox = wStackPanel.Children[0] as CheckBox;
                    string wPartID = wCheckBox.Tag as string;
                    if (wPartIDList.Exists(p => p.Equals(wPartID)))
                        wCheckBox.IsChecked = true;
                    else
                        wCheckBox.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 导入用户
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
                    wUser.PartPower = "";
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

                QueryUserAll();
                MessageBox.Show("导入成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 导出用户
        private void Cmd_Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int wErrorCode = ExcelOutPutTool.Intance.ExcelExportData("用户列表", Xdg_MainGrid);
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
