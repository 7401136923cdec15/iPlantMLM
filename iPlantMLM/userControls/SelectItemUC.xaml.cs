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

namespace Shris.NewEnergy.iPlant.Device
{
    /// <summary>
    /// SelectItemUC.xaml 的交互逻辑
    /// </summary>
    public partial class SelectItemUC : UserControl
    {
        public delegate void DeleObject(Object wObject, bool wShow);
        public event DeleObject DelObject;

        public SelectItemUC()
        {
            InitializeComponent();
        }

        public SelectItemUC(object wObject, SelectType wSelectType)
        {
            InitializeComponent();
            try
            {
                IniteUI(wObject, wSelectType);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        public void IniteUI(object wObject, SelectType wSelectType)
        {
            try
            {
                //初始化数据
                IniteData(wObject, wSelectType);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 清除所有TextBlock的背景颜色,并修改前景色
        /// </summary>
        private void ClearAllBackground()
        {
            try
            {
                foreach (UIElement wItem in SP_Main.Children)
                {
                    if (!(wItem is StackPanel))
                        continue;
                    StackPanel wStackPanel = wItem as StackPanel;
                    if (!(wStackPanel.Children[0] is TextBlock))
                        continue;
                    TextBlock wTextBlock = wStackPanel.Children[0] as TextBlock;
                    wTextBlock.Background = Brushes.White;
                    wTextBlock.Foreground = Brushes.Black;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 改变控件的颜色
        /// </summary>
        private void ChangeTextBlockColor(TextBlock wTextBlock, Brush wColor)
        {
            try
            {
                if (wColor == null || wColor == Brushes.White)
                {
                    wTextBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C51D1D"));
                    wTextBlock.Foreground = Brushes.White;
                    if (DelObject != null)
                        DelObject(wTextBlock.Tag, true);
                }
                else
                {
                    wTextBlock.Background = Brushes.White;
                    wTextBlock.Foreground = Brushes.Black;
                    if (DelObject != null)
                        DelObject(wTextBlock.Tag, false);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="wObject"></param>
        private void IniteData(object wObject, SelectType wSelectType)
        {
            try
            {
                if (wObject == null)
                    return;
                SP_Main.Children.Clear();


                #region 枚举翻译列表
                if (wObject is List<EnumItem>)
                {
                    List<EnumItem> wList = wObject as List<EnumItem>;
                    for (int i = 0; i < wList.Count; i++)
                    {
                        if (i < wList.Count - 1)
                        {
                            StackPanel wStackPanel = new StackPanel();


                            TextBlock wTextBlock = new TextBlock();
                            wTextBlock.Cursor = Cursors.Hand;
                            wTextBlock.Text = string.Format("{0}", wList[i].Description);
                            wTextBlock.FontSize = 25;

                            wTextBlock.Background = Brushes.White;
                            wTextBlock.Padding = new Thickness(10);
                            wTextBlock.Tag = wList[i];
                            wTextBlock.TextWrapping = TextWrapping.Wrap;

                            Border wBorder = new Border();
                            wBorder.BorderThickness = new Thickness(0, 0, 0, 1);
                            wBorder.BorderBrush = Brushes.Gray;
                            wBorder.Margin = new Thickness(10, 0, 0, 0);

                            wStackPanel.Children.Add(wTextBlock);
                            wStackPanel.Children.Add(wBorder);

                            SP_Main.Children.Add(wStackPanel);

                            //注册事件
                            switch (wSelectType)
                            {
                                case SelectType.Default:
                                    break;
                                case SelectType.SingleSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_SingleClick;
                                    break;
                                case SelectType.MultiSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_MultiClick;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            StackPanel wStackPanel = new StackPanel();


                            TextBlock wTextBlock = new TextBlock();
                            wTextBlock.Cursor = Cursors.Hand;
                            wTextBlock.Text = string.Format("{0}", wList[i].Description);
                            wTextBlock.FontSize = 25;

                            wTextBlock.Background = Brushes.White;
                            wTextBlock.Padding = new Thickness(10);
                            wTextBlock.Tag = wList[i];
                            wTextBlock.TextWrapping = TextWrapping.Wrap;

                            Border wBorder = new Border();
                            wBorder.BorderThickness = new Thickness(0, 0, 0, 1);
                            wBorder.BorderBrush = Brushes.Gray;
                            wBorder.Margin = new Thickness(10, 0, 0, 0);
                            wBorder.Visibility = Visibility.Collapsed;

                            wStackPanel.Children.Add(wTextBlock);
                            wStackPanel.Children.Add(wBorder);

                            SP_Main.Children.Add(wStackPanel);

                            //注册事件
                            switch (wSelectType)
                            {
                                case SelectType.Default:
                                    break;
                                case SelectType.SingleSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_SingleClick;
                                    break;
                                case SelectType.MultiSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_MultiClick;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion
                #region 普通字符串
                else if (wObject is List<string>)
                {
                    List<string> wStringList = wObject as List<string>;
                    for (int i = 0; i < wStringList.Count; i++)
                    {
                        if (i < wStringList.Count - 1)
                        {
                            StackPanel wStackPanel = new StackPanel();


                            TextBlock wTextBlock = new TextBlock();
                            wTextBlock.Cursor = Cursors.Hand;
                            wTextBlock.Text = wStringList[i];
                            wTextBlock.FontSize = 25;

                            wTextBlock.Padding = new Thickness(10);
                            wTextBlock.Tag = wStringList[i];
                            wTextBlock.TextWrapping = TextWrapping.Wrap;

                            Border wBorder = new Border();
                            wBorder.BorderThickness = new Thickness(0, 0, 0, 1);
                            wBorder.BorderBrush = Brushes.Gray;
                            wBorder.Margin = new Thickness(10, 0, 0, 0);

                            wStackPanel.Children.Add(wTextBlock);
                            wStackPanel.Children.Add(wBorder);

                            SP_Main.Children.Add(wStackPanel);

                            //注册事件
                            switch (wSelectType)
                            {
                                case SelectType.Default:
                                    break;
                                case SelectType.SingleSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_SingleClick;
                                    break;
                                case SelectType.MultiSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_MultiClick;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            StackPanel wStackPanel = new StackPanel();


                            TextBlock wTextBlock = new TextBlock();
                            wTextBlock.Cursor = Cursors.Hand;
                            wTextBlock.Text = wStringList[i];
                            wTextBlock.FontSize = 25;

                            wTextBlock.Padding = new Thickness(10);
                            wTextBlock.Tag = wStringList[i];
                            wTextBlock.TextWrapping = TextWrapping.Wrap;

                            Border wBorder = new Border();
                            wBorder.BorderThickness = new Thickness(0, 0, 0, 1);
                            wBorder.BorderBrush = Brushes.Gray;
                            wBorder.Margin = new Thickness(10, 0, 0, 0);
                            wBorder.Visibility = Visibility.Collapsed;

                            wStackPanel.Children.Add(wTextBlock);
                            wStackPanel.Children.Add(wBorder);

                            SP_Main.Children.Add(wStackPanel);

                            //注册事件
                            switch (wSelectType)
                            {
                                case SelectType.Default:
                                    break;
                                case SelectType.SingleSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_SingleClick;
                                    break;
                                case SelectType.MultiSelect:
                                    wTextBlock.PreviewMouseLeftButtonUp += TextBlock_MultiClick;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void TextBlock_MultiClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock wTextBlock = sender as TextBlock;
                ChangeTextBlockColor(wTextBlock, wTextBlock.Background);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void TextBlock_SingleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock wTextBlock = sender as TextBlock;
                Brush wCurColor = wTextBlock.Background;
                ClearAllBackground();
                ChangeTextBlockColor(wTextBlock, wCurColor);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        public List<T> GetSelectedList<T>()
        {
            List<T> wResult = new List<T>();
            try
            {
                foreach (UIElement wItem in SP_Main.Children)
                {
                    if (((wItem as StackPanel).Children[0] as TextBlock).Tag is T && ((wItem as StackPanel).Children[0] as TextBlock).Background != Brushes.White)
                    {
                        T wT = (T)((wItem as StackPanel).Children[0] as TextBlock).Tag;
                        wResult.Add(wT);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public void SelectDefaultItemList(List<EnumItem> wList)
        {
            try
            {
                //去除空对象
                wList.RemoveAll(p => p == null);

                foreach (UIElement wItem in SP_Main.Children)
                {
                    TextBlock wTextBlock = (wItem as StackPanel).Children[0] as TextBlock;
                    if (wTextBlock != null && wTextBlock.Tag is EnumItem)
                    {
                        EnumItem wTemp = wTextBlock.Tag as EnumItem;
                        if (wTemp == null)
                            continue;
                        if (wList != null && wList.Count > 0 && wList.Exists(p => p.ID == wTemp.ID))
                            ChangeTextBlockColor(wTextBlock, wTextBlock.Background);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        public void AllSelect()
        {
            try
            {
                foreach (UIElement wItem in SP_Main.Children)
                {
                    TextBlock wTextBlock = (wItem as StackPanel).Children[0] as TextBlock;
                    if (wTextBlock != null && wTextBlock.Tag is EnumItem)
                    {
                        EnumItem wTemp = wTextBlock.Tag as EnumItem;
                        if (wTemp == null)
                            continue;
                        ChangeTextBlockColor(wTextBlock, wTextBlock.Background);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
