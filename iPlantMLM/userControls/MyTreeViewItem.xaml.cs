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
    /// MyTreeViewItem.xaml 的交互逻辑
    /// </summary>
    public partial class MyTreeViewItem : UserControl
    {
        public MyTreeViewItem()
        {
            InitializeComponent();
        }

        public MyTreeViewItem(string wHeader, bool wIsShow)
        {
            try
            {
                InitializeComponent();
                Header.Text = wHeader;
                if (wIsShow)
                {
                    Image_Show.Visibility = Visibility.Visible;
                    Image_Hide.Visibility = Visibility.Collapsed;
                    Grid_Main.Visibility = Visibility.Visible;
                }
                else
                {
                    Image_Show.Visibility = Visibility.Collapsed;
                    Image_Hide.Visibility = Visibility.Visible;
                    Grid_Main.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 展开
        private void Image_Hide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image_Show.Visibility = Visibility.Visible;
                Image_Hide.Visibility = Visibility.Collapsed;
                Grid_Main.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 收起
        private void Image_Show_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image_Show.Visibility = Visibility.Collapsed;
                Image_Hide.Visibility = Visibility.Visible;
                Grid_Main.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
