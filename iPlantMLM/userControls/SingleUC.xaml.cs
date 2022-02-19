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
    /// SingleUC.xaml 的交互逻辑
    /// </summary>
    public partial class SingleUC : UserControl
    {
        public SingleUC()
        {
            InitializeComponent();
        }

        public SingleUC(string wTitle,string wContent)
        {
            InitializeComponent();
            try
            {
                Title.Content = wTitle;
                Content.Text = wContent;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
