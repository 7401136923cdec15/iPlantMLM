using ShrisTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace iPlantMLM
{
    /// <summary>
    /// LoginUI.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUI : Window
    {
        public delegate void DeleUser(BMSEmployee wUser);
        public event DeleUser DelUser;

        /// <summary>
        /// 最多保存N条用户登录信息
        /// </summary>
        private const int mMaxUserCacheCount = 10;

        public LoginUI()
        {
            InitializeComponent();

            InitializeForm();
        }

        #region 初始化
        private void InitializeForm()
        {
            try
            {
                //PwdTextBox.Focus();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 点击事件

        /// <summary>
        /// 窗体可拖拽
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        private void BT_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MixLogIn();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// Enter键触发登录
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        MixLogIn();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void BT_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 综合登录功能
        /// </summary>
        private void MixLogIn()
        {
            try
            {
                //①工号
                string wLoginID = UserTextBox.Text.Trim();
                //②密码
                string wPass = PwdTextBox.Text.Trim();
                //前端验证
                if (string.IsNullOrWhiteSpace(wLoginID) || string.IsNullOrWhiteSpace(wPass))
                {
                    MessageBox.Show("工号或密码不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //③登录测试
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => MixLogIn_DoWork(s, exc, wLoginID, wPass);
                wBW.RunWorkerCompleted += (s, exc) => MixLogIn_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void MixLogIn_DoWork(object s, DoWorkEventArgs e, string wLoginID, string wPassword)
        {
            try
            {
                BMSEmployee wResult = BMSEmployeeDAO.Instance.BMS_Login(wLoginID, wPassword);
                e.Result = wResult;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void MixLogIn_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                BMSEmployee wResult = e.Result as BMSEmployee;
                if (wResult.ID > 0)
                {
                    if (DelUser != null)
                        DelUser(wResult);
                    this.Close();
                }
                else
                    MessageBox.Show("工号或密码错误,请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 光标定位
        private void UserTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBox cmBox = (System.Windows.Controls.ComboBox)sender;
                var textBox = (cmBox.Template.FindName("PART_EditableTextBox",
                               cmBox) as TextBox);
                if (textBox != null)
                {
                    textBox.Focus();
                    textBox.SelectionStart = textBox.Text.Length;
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
