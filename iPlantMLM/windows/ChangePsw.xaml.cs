using ShrisTool;
using System;
using System.Collections.Generic;
using System.Windows;

namespace iPlantMLM
{
    public delegate void ChangeFinish();
    /// <summary>
    /// ChangePsw.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePsw : Window
    {
        public ChangePsw()
        {
            InitializeComponent();
        }

        #region 全局变量
        private BMSEmployee mEmployee = new BMSEmployee();
        public delegate void DeleSave();
        public event DeleSave DelSave;
        #endregion

        public ChangePsw(BMSEmployee wEmployee)
        {
            InitializeComponent();

            try
            {
                mEmployee = wEmployee;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
             System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mEmployee != null)
                {
                    string wOldPsw = this.Tbx_OldPsw.Password.Trim();
                    string wNewPsw = this.Tbx_NewPsw.Password.Trim();
                    string wConfirmPsw = this.Tbx_ConfirmPsw.Password.Trim();
                    if (string.IsNullOrEmpty(wOldPsw))
                    {
                        MessageBox.Show("原密码未填写。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(wNewPsw))
                    {
                        MessageBox.Show("新密码未填写。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(wConfirmPsw))
                    {
                        MessageBox.Show("确认新密码未填写。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        if (!ShrisDES.EncryptDESString(wOldPsw, "shrismcis").Equals(mEmployee.Password))
                        {
                            MessageBox.Show("原密码输入错误。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else if (!wNewPsw.Equals(wConfirmPsw))
                        {
                            MessageBox.Show("确认新密码前后不一致。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else if ((wNewPsw.Trim().Length < 6) || (wNewPsw.Trim().Length > 12))
                        {
                            MessageBox.Show("密码长度须在6位~12位之间。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (MessageBox.Show("确认修改密码吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                            return;

                        string wPsw = ShrisDES.EncryptDESString(wConfirmPsw, "shrismcis");
                        mEmployee.Password = wPsw;
                        int wErrorCode = 0;
                        BMSEmployeeDAO.Instance.BMS_SaveBMSEmployee(mEmployee, out wErrorCode);

                        MessageBox.Show("密码修改成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        if (DelSave != null)
                            DelSave();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
             System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mEmployee != null)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
             System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

    }
}
