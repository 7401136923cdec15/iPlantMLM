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
    /// PartUC.xaml 的交互逻辑
    /// </summary>
    public partial class PartUC : UserControl
    {
        public PartUC()
        {
            InitializeComponent();
        }

        public bool mIsChecked = false;
        public FPCPart mPart = null;

        public PartUC(string wName, bool wIsChecked)
        {
            try
            {
                InitializeComponent();

                TB_Name.Text = wName;
                CB_Check.IsChecked = wIsChecked;
                mIsChecked = wIsChecked;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void CB_Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mIsChecked = !mIsChecked;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
