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
    /// InputHeadUC.xaml 的交互逻辑
    /// </summary>
    public partial class InputHeadUC : UserControl
    {
        public InputHeadUC()
        {
            InitializeComponent();
        }

        public InputHeadUC(List<IPTStandard> wIPTStandardList)
        {
            try
            {
                InitializeComponent();

                if (wIPTStandardList.Count > 0 && wIPTStandardList[0].PartID > 7)
                    TB_Title.Text = "模组编号";

                Grid_Main.ColumnDefinitions.Clear();
                wIPTStandardList = wIPTStandardList.OrderBy(p => p.PartID).ThenBy(p => p.OrderID).ToList();
                ColumnDefinition wColumnDefinition = new ColumnDefinition();
                GridLength wGridLength = new GridLength(1d, GridUnitType.Star);
                wColumnDefinition.Width = wGridLength;
                this.Grid_Main.ColumnDefinitions.Add(wColumnDefinition);
                int wIndex = 1;
                foreach (IPTStandard wIPTStandard in wIPTStandardList)
                {
                    wColumnDefinition = new ColumnDefinition();
                    wGridLength = new GridLength(1d, GridUnitType.Star);
                    wColumnDefinition.Width = wGridLength;
                    this.Grid_Main.ColumnDefinitions.Add(wColumnDefinition);
                    //加标题
                    Border wBorder = new Border();
                    wBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#365995"));
                    wBorder.BorderThickness = new Thickness(0);
                    Grid.SetColumn(wBorder, wIndex++);
                    Grid_Main.Children.Add(wBorder);

                    TextBlock wTextBlock = new TextBlock();
                    wTextBlock.Text = wIPTStandard.ItemName;
                    wTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    wTextBlock.FontSize = 15;
                    wTextBlock.FontWeight = FontWeights.Bold;
                    wTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    wTextBlock.Foreground = Brushes.White;
                    wTextBlock.Margin = new Thickness(10);
                    wTextBlock.TextWrapping = TextWrapping.Wrap;
                    wTextBlock.TextAlignment = TextAlignment.Center;
                    wBorder.Child = wTextBlock;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
