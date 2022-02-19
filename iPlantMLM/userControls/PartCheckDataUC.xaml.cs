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
    /// PartCheckDataUC.xaml 的交互逻辑
    /// </summary>
    public partial class PartCheckDataUC : UserControl
    {
        public PartCheckDataUC()
        {
            InitializeComponent();
        }

        public PartCheckDataUC(string wPartName, List<IPTStandard> wIPTStandardList, SFCModuleRecord wSFCModuleRecord, List<IPTBoolValue> wIPTBoolValueList, List<IPTNumberValue> wIPTNumberValueList, List<IPTTextValue> wIPTTextValueList)
        {
            InitializeComponent();
            try
            {
                SP_Main.Children.Clear();
                //加载工位
                Lable_Title.Content = wPartName;
                //加载表头
                InputHeadUC wInputHeadUC = new InputHeadUC(wIPTStandardList);
                SP_Main.Children.Add(wInputHeadUC);
                //加载表体
                InputValueUC wInputValueUC = new InputValueUC(wSFCModuleRecord, wIPTStandardList, wIPTBoolValueList, wIPTNumberValueList, wIPTTextValueList);
                SP_Main.Children.Add(wInputValueUC);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
