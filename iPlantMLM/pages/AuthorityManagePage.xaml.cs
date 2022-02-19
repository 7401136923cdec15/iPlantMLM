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
    /// AuthorityManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class AuthorityManagePage : Page
    {
        public AuthorityManagePage()
        {
            InitializeComponent();
        }

        #region 页面加载
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                QueryRoleAll();
                LoadSource();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
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
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void QueryRoleAll()
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => QueryRoleAll_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => QueryRoleAll_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void QueryRoleAll_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                int wErrorCode = 0;
                List<MBSRole> wRoleList = MBSRoleDAO.Instance.MBS_QueryMBSRoleList(-1, -1, "", out wErrorCode);
                e.Result = wRoleList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void QueryRoleAll_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<MBSRole> wRoleList = e.Result as List<MBSRole>;
                Xdg_MainGrid.DataSource = wRoleList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 创建角色
        private void Cmd_ResetPsw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 2001))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Tbx_UserName.Text))
                {
                    MessageBox.Show("角色名称不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string wRoleName = Tbx_UserName.Text;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => JudgeRoleName_DoWork(s, exc, wRoleName);
                wBW.RunWorkerCompleted += (s, exc) => JudgeRoleName_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void JudgeRoleName_DoWork(object s, DoWorkEventArgs e, string wRoleName)
        {
            try
            {
                int wErrorCode = 0;
                List<MBSRole> wRoleList = MBSRoleDAO.Instance.MBS_QueryMBSRoleList(-1, -1, wRoleName, out wErrorCode);
                if (wRoleList.Count > 0)
                    e.Result = true;
                else
                    e.Result = false;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void JudgeRoleName_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                {
                    MessageBox.Show("该角色名已存在，不能重复创建!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认创建该角色吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                string wRoleName = Tbx_UserName.Text;
                string wExplain = Tbx_WorkNo.Text;
                int wActive = (int)Cbb_Active.SelectedValue;
                MBSRole wRole = new MBSRole();
                wRole.ID = 0;
                wRole.ExplainText = wExplain;
                wRole.Name = wRoleName;
                wRole.OwnerID = 0;
                wRole.CreateTime = DateTime.Now;

                MyLoading wMyLoading1 = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => AddRole_DoWork(s, exc, wRole);
                wBW.RunWorkerCompleted += (s, exc) => AddRole_RunWorkerCompleted(exc, wMyLoading1);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void AddRole_DoWork(object s, DoWorkEventArgs e, MBSRole wRole)
        {
            try
            {
                int wErrorCode = 0;
                MBSRoleDAO.Instance.MBS_SaveMBSRole(wRole, out wErrorCode);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void AddRole_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                QueryRoleAll();
                MessageBox.Show("创建角色成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 修改角色
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 2002))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Tbx_UserName.Text))
                {
                    MessageBox.Show("角色名称不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MBSRole wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as MBSRole;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要修改的角色!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //判断角色名称是否和其他重复
                string wLoginID = Tbx_UserName.Text;
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => RoleNameJudge_DoWork(s, exc, wLoginID, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => RoleNameJudge_RunWorkerCompleted(exc, wMyLoading, wSelectedDisplay);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void RoleNameJudge_DoWork(object s, DoWorkEventArgs e, string wLoginID, MBSRole wSelectedDisplay)
        {
            try
            {
                int wErrorCode;
                List<MBSRole> wUserList = MBSRoleDAO.Instance.MBS_QueryMBSRoleList(-1, -1, wLoginID, out wErrorCode);
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

        private void RoleNameJudge_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, MBSRole wSelectedDisplay)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                {
                    MessageBox.Show("该角色名称已存在，不允许修改!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认修改吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                string wExplainText = Tbx_WorkNo.Text;
                string wName = Tbx_UserName.Text;
                int wActive = (int)Cbb_Active.SelectedValue;
                wSelectedDisplay.Name = wName;
                wSelectedDisplay.ExplainText = wExplainText;
                wSelectedDisplay.Active = wActive;

                MyLoading wMyLoading1 = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => UpdateUser_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => TUpdateUser_RunWorkerCompleted(exc, wMyLoading1);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void UpdateUser_DoWork(object s, DoWorkEventArgs e, MBSRole wUser)
        {
            try
            {
                int wErrorCode = 0;
                MBSRoleDAO.Instance.MBS_SaveMBSRole(wUser, out wErrorCode);
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

                QueryRoleAll();
                MessageBox.Show("修改成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 关闭角色
        private void Cmd_Disable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 2003))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MBSRole wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as MBSRole;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要关闭的角色!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void CloseUser_DoWork(object s, DoWorkEventArgs e, MBSRole wSelectedDisplay)
        {
            try
            {
                wSelectedDisplay.Active = 2;
                int wErrorCode = 0;
                MBSRoleDAO.Instance.MBS_SaveMBSRole(wSelectedDisplay, out wErrorCode);
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

                QueryRoleAll();
                MessageBox.Show("关闭成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 激活角色
        private void Cmd_Active_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 2004))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MBSRole wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as MBSRole;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要激活的角色!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认激活该角色吗？", "提示", MessageBoxButton.OKCancel,
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
        private void ActiveUser_DoWork(object s, DoWorkEventArgs e, MBSRole wSelectedDisplay)
        {
            try
            {
                wSelectedDisplay.Active = 1;
                int wErrorCode = 0;
                MBSRoleDAO.Instance.MBS_SaveMBSRole(wSelectedDisplay, out wErrorCode);
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

                QueryRoleAll();
                MessageBox.Show("激活成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 更新权限
        private void Cmd_Function_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MBSRoleDAO.Instance.IsPower(GUD.mLoginUser, 2005))
                {
                    MessageBox.Show("无授权!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MBSRole wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as MBSRole;
                if (wSelectedDisplay == null)
                {
                    MessageBox.Show("请选中要更新权限的角色!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认更新吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                //获取选中的权限
                List<int> wFunctionList = GetCheckedFunctionList();
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => UpdateFunction_DoWork(s, exc, wSelectedDisplay, wFunctionList);
                wBW.RunWorkerCompleted += (s, exc) => UpdateFunction_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void UpdateFunction_DoWork(object s, DoWorkEventArgs e, MBSRole wSelectedDisplay, List<int> wFunctionList)
        {
            try
            {
                int wErrorCode = 0;
                List<MBSRoleFunction> wRoleList = MBSRoleFunctionDAO.Instance.MBS_QueryMBSRoleFunctionList(-1, wSelectedDisplay.ID, -1, out wErrorCode);
                if (wRoleList.Count > 0)
                    MBSRoleFunctionDAO.Instance.MBS_DeleteMBSRoleFunctionList(wRoleList);
                foreach (int wFunctionID in wFunctionList)
                {
                    MBSRoleFunction wRoleFunction = new MBSRoleFunction();
                    wRoleFunction.ID = 0;
                    wRoleFunction.RoleID = wSelectedDisplay.ID;
                    wRoleFunction.FunctionID = wFunctionID;
                    MBSRoleFunctionDAO.Instance.MBS_SaveMBSRoleFunction(wRoleFunction, out wErrorCode);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void UpdateFunction_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                MessageBox.Show("更新成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private List<int> GetCheckedFunctionList()
        {
            List<int> wResult = new List<int>();
            try
            {
                foreach (TreeViewItem wTreeViewItem in TV_Role.Items)
                {
                    StackPanel wStackPanel = wTreeViewItem.Items[0] as StackPanel;
                    foreach (StackPanel wMyStackPanel in wStackPanel.Children)
                    {
                        CheckBox wCheckBox = wMyStackPanel.Children[0] as CheckBox;
                        if (wCheckBox.IsChecked == true)
                            wResult.Add(int.Parse((string)wCheckBox.Tag));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
        #endregion

        #region 表格单击
        private void TableClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MBSRole wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as MBSRole;
                if (wSelectedDisplay == null)
                    return;
                Tbx_UserName.Text = wSelectedDisplay.Name;
                Tbx_WorkNo.Text = wSelectedDisplay.ExplainText;
                Cbb_Active.SelectedIndex = wSelectedDisplay.Active == 1 ? 0 : 1;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => PaintRoleTree_DoWork(s, exc, wSelectedDisplay);
                wBW.RunWorkerCompleted += (s, exc) => PaintRoleTree_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void PaintRoleTree_DoWork(object s, DoWorkEventArgs e, MBSRole wSelectedDisplay)
        {
            try
            {
                int wErrorCode = 0;
                List<MBSRoleFunction> wRoleFunctionList = MBSRoleFunctionDAO.Instance.MBS_QueryMBSRoleFunctionList(-1, wSelectedDisplay.ID, -1, out wErrorCode);
                e.Result = wRoleFunctionList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void PaintRoleTree_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<MBSRoleFunction> wRoleFunctionList = e.Result as List<MBSRoleFunction>;
                PaintRoleTreeTree(wRoleFunctionList);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void PaintRoleTreeTree(List<MBSRoleFunction> wRoleFunctionList)
        {
            try
            {
                foreach (TreeViewItem wTreeViewItem in TV_Role.Items)
                {
                    StackPanel wStackPanel = wTreeViewItem.Items[0] as StackPanel;
                    foreach (StackPanel wMyStackPanel in wStackPanel.Children)
                    {
                        CheckBox wCheckBox = wMyStackPanel.Children[0] as CheckBox;
                        string wFunctionID = wCheckBox.Tag as string;
                        if (wRoleFunctionList.Exists(p => p.FunctionID.ToString().Equals(wFunctionID)))
                            wCheckBox.IsChecked = true;
                        else
                            wCheckBox.IsChecked = false;
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
