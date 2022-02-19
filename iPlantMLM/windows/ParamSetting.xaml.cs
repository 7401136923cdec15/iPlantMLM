using ShrisTool;
using System;
using System.Windows;

namespace iPlantMLM
{
    /// <summary>
    /// ChangePsw.xaml 的交互逻辑
    /// </summary>
    public partial class ParamSetting : Window
    {
        public ParamSetting()
        {
            InitializeComponent();
            try
            {
                MESParams wMESParams = MESParamsDAO.Instance.GetMESParams();
                if (wMESParams.ChargingTime > 0)
                    Tbx_NewPsw.Text = wMESParams.ChargingTime.ToString();
                if (wMESParams.ChargingVoltage > 0)
                    Tbx_OldPsw.Text = wMESParams.ChargingVoltage.ToString();
                if (wMESParams.DischargeDuration > 0)
                    Tbx_ConfirmPsw.Text = wMESParams.DischargeDuration.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //①充电电压
                if (string.IsNullOrWhiteSpace(Tbx_OldPsw.Text))
                {
                    MessageBox.Show("充电电压输入值不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                double wChargingVoltage = 0;
                if (!double.TryParse(Tbx_OldPsw.Text, out wChargingVoltage))
                {
                    MessageBox.Show("充电电压输入值不合法!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //②充电时长
                if (string.IsNullOrWhiteSpace(Tbx_NewPsw.Text))
                {
                    MessageBox.Show("充电时长输入值不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                double wChargingTime = 0;
                if (!double.TryParse(Tbx_NewPsw.Text, out wChargingTime))
                {
                    MessageBox.Show("充电时长输入值不合法!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //③放电时长
                if (string.IsNullOrWhiteSpace(Tbx_ConfirmPsw.Text))
                {
                    MessageBox.Show("放电时长输入值不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                double wDischargeDuration = 0;
                if (!double.TryParse(Tbx_ConfirmPsw.Text, out wDischargeDuration))
                {
                    MessageBox.Show("放电时长输入值不合法!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MESParams wMESParams = new MESParams();
                wMESParams.ChargingVoltage = wChargingVoltage;
                wMESParams.ChargingTime = wChargingTime;
                wMESParams.DischargeDuration = wDischargeDuration;
                MESParamsDAO.Instance.SaveMESParams(wMESParams);

                this.Close();
                MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                this.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
             System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

    }
}
