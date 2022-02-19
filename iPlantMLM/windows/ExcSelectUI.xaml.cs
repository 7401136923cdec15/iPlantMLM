using Infragistics.Controls.Interactions;
using Infragistics.Windows.DataPresenter;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Shris.NewEnergy.iPlant.Device
{
    /// <summary>
    /// SettingDeviceConfigxaml.xaml 的交互逻辑
    /// </summary>
    public partial class ExcSelectUI : Window
    {
        public delegate void DeleSave(List<EnumItem> wEnumItemList);
        public event DeleSave DelSave;

        public delegate void DeleSearch(String wText);
        public event DeleSearch DelSearch;

        public ExcSelectUI()
        {
            InitializeComponent();
        }

        public ExcSelectUI(List<EnumItem> wList, List<EnumItem> wSelectedItemList, SelectType wSelectType, string wTitle, bool wShowSearchBox)
        {
            InitializeComponent();
            try
            {
                TB_Title.Text = wTitle;
                SIU_Items.IniteUI(wList, wSelectType);
                if (wSelectedItemList != null && wSelectedItemList.Count > 0)
                    SIU_Items.SelectDefaultItemList(wSelectedItemList);
                this.TE_CallContent.TextChanged += (s, e) => ValueChanged(wList, wSelectType);
                if (wList == null || wList.Count <= 0)
                    this.Grid_SearchTip.Visibility = Visibility.Visible;
                else
                    this.Grid_SearchTip.Visibility = Visibility.Collapsed;
                if (wShowSearchBox)
                {
                    TE_CallContent.Visibility = Visibility.Visible;
                }
                else
                {
                    TE_CallContent.Visibility = Visibility.Collapsed;
                }

                //多选时放出全选
                if (wSelectType == SelectType.MultiSelect)
                {
                    Border_AllSelect.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 搜索框文本改变事件
        /// </summary>
        /// <param name="wStringList"></param>
        private void ValueChanged(List<EnumItem> wList, SelectType wSelectType)
        {
            try
            {
                string wValue = this.TE_CallContent.Text;
                if (string.IsNullOrWhiteSpace(wValue))
                {
                    TB_Search.Visibility = Visibility.Visible;

                    SIU_Items.IniteUI(wList, wSelectType);
                    if (wList == null || wList.Count <= 0)
                        this.Grid_SearchTip.Visibility = Visibility.Visible;
                    else
                        this.Grid_SearchTip.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TB_Search.Visibility = Visibility.Collapsed;

                    List<EnumItem> wTempList = wList.FindAll(p => p.Name.Contains(wValue));
                    SIU_Items.IniteUI(wTempList, wSelectType);
                    if (wTempList == null || wTempList.Count <= 0)
                        this.Grid_SearchTip.Visibility = Visibility.Visible;
                    else
                        this.Grid_SearchTip.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void BT_Save_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                List<EnumItem> wList = SIU_Items.GetSelectedList<EnumItem>();

                if (wList != null && wList.Count > 0)
                {
                    if (DelSave != null)
                        DelSave(wList);
                }
                else
                {
                    if (DelSave != null)
                        DelSave(null);
                }

                if (DelSearch != null)
                    DelSearch(TE_CallContent.Text);

                //this.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 返回
        private void TextBlock_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 全选
        private void Border_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SIU_Items.AllSelect();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
