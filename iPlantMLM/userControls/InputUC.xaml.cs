using ShrisTool;
using System;
using System.Collections.Generic;
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
    /// InputUC.xaml 的交互逻辑
    /// </summary>
    public partial class InputUC : UserControl
    {
        public delegate void DeleReturn();
        public event DeleReturn DelReturn;
        public InputUC()
        {
            InitializeComponent();
        }

        private int mType = 0;
        public IPTStandard mIPTStandard;
        public InputUC(IPTStandard wIPTStandard)
        {
            InitializeComponent();
            try
            {
                mType = wIPTStandard.Type;
                mIPTStandard = wIPTStandard;
                switch ((StandardType)wIPTStandard.Type)
                {
                    case StandardType.文本:
                        Grid_TextType.Visibility = Visibility.Visible;
                        Grid_NumberType.Visibility = Visibility.Collapsed;
                        Grid_SingleSelectType.Visibility = Visibility.Collapsed;
                        TB_TextTitle.Text = wIPTStandard.ItemName + ":";
                        if (!string.IsNullOrWhiteSpace(wIPTStandard.DefaultValue))
                            TB_TextValue.Text = wIPTStandard.DefaultValue;
                        break;
                    case StandardType.全开区间:
                    case StandardType.全包区间:
                    case StandardType.右包区间:
                    case StandardType.左包区间:
                    case StandardType.小于:
                    case StandardType.大于:
                    case StandardType.小于等于:
                    case StandardType.大于等于:
                    case StandardType.等于:
                        Grid_NumberType.Visibility = Visibility.Visible;
                        Grid_TextType.Visibility = Visibility.Collapsed;
                        Grid_SingleSelectType.Visibility = Visibility.Collapsed;
                        TB_NumberTitle.Text = wIPTStandard.ItemName + ":";
                        TB_UnitText.Text = wIPTStandard.UnitText;
                        if (!string.IsNullOrWhiteSpace(wIPTStandard.DefaultValue))
                            TB_NumberValue.Text = wIPTStandard.DefaultValue;
                        break;
                    case StandardType.单选:
                    case StandardType.是否:
                        Grid_SingleSelectType.Visibility = Visibility.Visible;
                        Grid_TextType.Visibility = Visibility.Collapsed;
                        Grid_NumberType.Visibility = Visibility.Collapsed;
                        TB_Title.Text = wIPTStandard.ItemName + ":";

                        Dictionary<int, string> wDic = new Dictionary<int, string>();
                        wDic.Add(1, "OK");
                        wDic.Add(2, "NG");
                        CB_Value.ItemsSource = wDic;
                        CB_Value.SelectedIndex = 0;

                        if (!string.IsNullOrWhiteSpace(wIPTStandard.DefaultValue))
                            CB_Value.Text = wIPTStandard.DefaultValue;
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
        public void MyFocus()
        {
            try
            {
                switch ((StandardType)mType)
                {
                    case StandardType.文本:
                        TB_TextValue.Focus();
                        break;
                    case StandardType.全开区间:
                    case StandardType.全包区间:
                    case StandardType.右包区间:
                    case StandardType.左包区间:
                    case StandardType.小于:
                    case StandardType.大于:
                    case StandardType.小于等于:
                    case StandardType.大于等于:
                    case StandardType.等于:
                        TB_NumberValue.Focus();
                        break;
                    case StandardType.单选:
                    case StandardType.是否:
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

        public String GetWriteValue()
        {
            string wResult = "";
            try
            {
                switch ((StandardType)mType)
                {
                    case StandardType.文本:
                        wResult = TB_TextValue.Text;
                        break;
                    case StandardType.全开区间:
                    case StandardType.全包区间:
                    case StandardType.右包区间:
                    case StandardType.左包区间:
                    case StandardType.小于:
                    case StandardType.大于:
                    case StandardType.小于等于:
                    case StandardType.大于等于:
                    case StandardType.等于:
                        wResult = TB_NumberValue.Text;
                        break;
                    case StandardType.单选:
                    case StandardType.是否:
                        wResult = CB_Value.SelectedValue.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        private void TB_TextValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TB_TextValue.Text.Contains("\r\n"))
                {
                    TB_TextValue.Text = TB_TextValue.Text.Replace("\r\n", "");
                    if (string.IsNullOrWhiteSpace(TB_TextValue.Text))
                    {
                        MessageBox.Show("填写值不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (DelReturn != null)
                        DelReturn();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void TB_NumberValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TB_NumberValue.Text.Contains("\r\n"))
                {
                    TB_NumberValue.Text = TB_NumberValue.Text.Replace("\r\n", "");
                    if (string.IsNullOrWhiteSpace(TB_NumberValue.Text))
                    {
                        MessageBox.Show("填写值不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (DelReturn != null)
                        DelReturn();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        public void SetMyValue(string wValue)
        {
            try
            {
                switch ((StandardType)mIPTStandard.Type)
                {
                    case StandardType.文本:
                        TB_TextValue.Text = wValue;
                        break;
                    case StandardType.全开区间:
                    case StandardType.全包区间:
                    case StandardType.右包区间:
                    case StandardType.左包区间:
                    case StandardType.小于:
                    case StandardType.大于:
                    case StandardType.小于等于:
                    case StandardType.大于等于:
                    case StandardType.等于:
                        TB_NumberValue.Text = wValue;
                        break;
                    case StandardType.单选:
                    case StandardType.是否:
                        CB_Value.SelectedValue = int.Parse(wValue);
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
    }
}
