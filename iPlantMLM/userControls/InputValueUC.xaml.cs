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
    /// InputValueUC.xaml 的交互逻辑
    /// </summary>
    public partial class InputValueUC : UserControl
    {
        public InputValueUC()
        {
            InitializeComponent();
        }

        public InputValueUC(List<IPTStandard> wIPTStandardList)
        {
            InitializeComponent();
            try
            {
                Grid_Main.ColumnDefinitions.Clear();
                wIPTStandardList = wIPTStandardList.OrderBy(p => p.OrderID).ToList();
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
                    wBorder.Background = Brushes.LightBlue;
                    wBorder.BorderBrush = Brushes.White;
                    wBorder.BorderThickness = new Thickness(1);
                    Grid.SetColumn(wBorder, wIndex++);
                    Grid_Main.Children.Add(wBorder);

                    TextBlock wTextBlock = new TextBlock();
                    wTextBlock.Text = "value" + wIndex;
                    wTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    wTextBlock.FontSize = 20;
                    wTextBlock.FontWeight = FontWeights.Bold;
                    wTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    wTextBlock.Foreground = Brushes.Black;
                    wTextBlock.Margin = new Thickness(10);
                    wBorder.Child = wTextBlock;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        public SFCModuleRecord mSFCModuleRecord;
        public List<IPTStandard> mIPTStandardList;
        public List<IPTBoolValue> mIPTBoolValueList;
        public List<IPTNumberValue> mIPTNumberValueList;
        public List<IPTTextValue> mIPTTextValueList;

        public InputValueUC(SFCModuleRecord wSFCModuleRecord, List<IPTStandard> wIPTStandardList, List<IPTBoolValue> wIPTBoolValueList, List<IPTNumberValue> wIPTNumberValueList, List<IPTTextValue> wIPTTextValueList)
        {
            InitializeComponent();
            try
            {
                mSFCModuleRecord = wSFCModuleRecord;
                mIPTStandardList = wIPTStandardList;
                mIPTBoolValueList = wIPTBoolValueList;
                mIPTNumberValueList = wIPTNumberValueList;
                mIPTTextValueList = wIPTTextValueList;

                //电容包编号
                TB_CapacityNo.Text = wSFCModuleRecord.CapacitorPackageNo;
                if (wIPTStandardList.Count > 0 && wIPTStandardList[0].PartID > 7)
                    TB_CapacityNo.Text = wSFCModuleRecord.ModuleNumber;

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
                    wBorder.Background = Brushes.LightBlue;
                    wBorder.BorderBrush = Brushes.White;
                    wBorder.BorderThickness = new Thickness(1);
                    Grid.SetColumn(wBorder, wIndex++);
                    Grid_Main.Children.Add(wBorder);

                    TextBlock wTextBlock = new TextBlock();

                    switch ((StandardType)wIPTStandard.Type)
                    {
                        case StandardType.文本:
                            if (wIPTTextValueList.Exists(p => p.SerialNumber.Equals(wSFCModuleRecord.SerialNumber) && p.StandardID == wIPTStandard.ID))
                            {
                                wTextBlock.Text = wIPTTextValueList.Find(p => p.SerialNumber.Equals(wSFCModuleRecord.SerialNumber) && p.StandardID == wIPTStandard.ID).Value;
                            }
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
                            if (wIPTNumberValueList.Exists(p => p.SerialNumber.Equals(wSFCModuleRecord.SerialNumber) && p.StandardID == wIPTStandard.ID))
                            {
                                wTextBlock.Text = wIPTNumberValueList.Find(p => p.SerialNumber.Equals(wSFCModuleRecord.SerialNumber) && p.StandardID == wIPTStandard.ID).Value.ToString();
                            }
                            break;
                        case StandardType.单选:
                        case StandardType.是否:
                            if (wIPTBoolValueList.Exists(p => p.SerialNumber.Equals(wSFCModuleRecord.SerialNumber) && p.StandardID == wIPTStandard.ID))
                            {
                                int wValue = wIPTBoolValueList.Find(p => p.SerialNumber.Equals(wSFCModuleRecord.SerialNumber) && p.StandardID == wIPTStandard.ID).Value;
                                wTextBlock.Text = wValue == 1 ? "OK" : "NG";
                            }
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(wIPTStandard.UnitText) && !string.IsNullOrWhiteSpace(wTextBlock.Text))
                        wTextBlock.Text += wIPTStandard.UnitText;

                    wTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    wTextBlock.FontSize = 15;
                    wTextBlock.FontWeight = FontWeights.Bold;
                    wTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    wTextBlock.Foreground = Brushes.Black;
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
