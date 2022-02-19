using ShrisTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace iPlantMLM
{
    /// <summary>
    /// MEDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class MEDataGrid : UserControl
    {
        public MEDataGrid()
        {
            InitializeComponent();
        }

        #region 全局变量
        //选中行集合
        private List<int> mSelectRowList = new List<int>();

        private List<int> mSelectRowListTemp = new List<int>();
        //单元格类型配置
        public Dictionary<int, ExamineDataItem> mColNoCellItemTypeDic = new Dictionary<int, ExamineDataItem>();
        public Dictionary<string, ExamineDataItem> mColNameCellItemTypeDic = new Dictionary<string, ExamineDataItem>();
        #endregion

        public delegate void SaveDataHandler();
        public event SaveDataHandler mSaveDataHandler;
        private bool IsRouteContinue = false;//禁止事件继续触发
        /// <summary>
        /// 设置表头
        /// </summary>
        /// <param name="wHead">表头集合</param>
        public void SetHead(List<string> wHead)
        {
            try
            {
                if (wHead == null || wHead.Count < 1)
                    return;

                this.Grd_Body.RowDefinitions.Clear();
                this.Grd_Body.ColumnDefinitions.Clear();
                this.Grd_Body.Children.Clear();
                //添加新的行定义
                RowDefinition wRowDef = new RowDefinition();
                wRowDef.Height = GridLength.Auto;
                this.Grd_Body.RowDefinitions.Add(wRowDef);

                //依据传进来的的表头个数(+1 因为序号)定义列数，并设置宽度为自动
                for (int i = 0; i < wHead.Count; i++)
                {
                    ColumnDefinition wColDef = new ColumnDefinition();
                    wColDef.Width = GridLength.Auto;
                    this.Grd_Body.ColumnDefinitions.Add(wColDef);
                }

                //添加表头
                for (int i = 0; i < wHead.Count; i++)
                {
                    Label wLbl = new Label();
                    wLbl.Content = wHead[i];
                    wLbl.FontSize = 20;
                    wLbl.FontWeight = FontWeights.Bold;
                    wLbl.BorderBrush = Brushes.Black;
                    wLbl.BorderThickness = new Thickness(1);
                    wLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                    if (i > 0 && mColNoCellItemTypeDic.ContainsKey(i))
                    {
                        if (mColNoCellItemTypeDic[i].IsRequired)
                            wLbl.Content = string.Format("{0}(*)", wHead[i]);
                    }
                    Grid.SetRow(wLbl, 0);
                    Grid.SetColumn(wLbl, i);
                    this.Grd_Body.Children.Add(wLbl);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        //设置输入单元格类型（以列计算）
        private void SetCellDataType(int wCol, ExamineDataItem wExamineDataItem)
        {
            try
            {
                if (!mColNoCellItemTypeDic.ContainsKey(wCol))
                    mColNoCellItemTypeDic.Add(wCol, wExamineDataItem);

                if (!mColNameCellItemTypeDic.ContainsKey(wExamineDataItem.ItemName))
                    mColNameCellItemTypeDic.Add(wExamineDataItem.ItemName, wExamineDataItem);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        //表头参数配置
        private Dictionary<string, ExamineDataItem> GetTableHeadConfig(int wLineID, List<int> wPartIDList, int wStationID, int wProductID, bool wIsHistory)
        {
            Dictionary<string, ExamineDataItem> wDicTableHead = new Dictionary<string, ExamineDataItem>();

            try
            {
                //获取表头
                List<ExamineDataItem> wHeadEdiList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(wPartIDList, wProductID, wIsHistory);

                if (!wDicTableHead.ContainsKey("序号"))
                    wDicTableHead.Add("序号", new ExamineDataItem { ItemName = "序号", TypeEnum = ItemTypeEnum.文本 });//表序号列

                if (!wDicTableHead.ContainsKey("电容包编号"))
                    wDicTableHead.Add("电容包编号", new ExamineDataItem { ItemName = "电容包编号", TypeEnum = ItemTypeEnum.文本, IsEdit = false, IsRepeat = true });

                if (wPartIDList[0] >= 8 && wPartIDList[0] <= 10 && !wPartIDList.Exists(p => p == 11))
                {
                    if (!wDicTableHead.ContainsKey("模组编码"))
                        wDicTableHead.Add("模组编码", new ExamineDataItem { ItemName = "模组编码", TypeEnum = ItemTypeEnum.文本, IsEdit = false, IsRepeat = true });
                }

                if (wPartIDList[0] == 6 || wPartIDList[0] == 11 || wPartIDList[0] == 12)
                    if (!wDicTableHead.ContainsKey("档位"))
                        wDicTableHead.Add("档位", new ExamineDataItem { ItemName = "档位", TypeEnum = ItemTypeEnum.文本, IsEdit = false, IsRepeat = true });

                foreach (ExamineDataItem item in wHeadEdiList)
                {
                    if (!wDicTableHead.ContainsKey(item.ItemName)
                        && !item.ItemName.Equals("备注")
                        && !item.ItemName.Equals("合格"))
                        wDicTableHead.Add(item.ItemName, item);
                }

                if (wDicTableHead.Count < 1)
                {
                    MessageBox.Show("表头获取失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                throw ex;
            }
            return wDicTableHead;
        }

        //配置表格
        public Dictionary<string, ExamineDataItem> ConfigDataGrid(int wLineID, List<int> wPartIDList, int wStationID, int wProductID, bool wIsHistory)
        {
            Dictionary<string, ExamineDataItem> wDicHead = new Dictionary<string, ExamineDataItem>();
            try
            {
                mColNoCellItemTypeDic.Clear();
                mColNameCellItemTypeDic.Clear();
                wDicHead = GetTableHeadConfig(wLineID, wPartIDList, wStationID, wProductID, wIsHistory);

                int wCol = 0;
                foreach (string wKey in wDicHead.Keys)
                {
                    SetCellDataType(wCol, wDicHead[wKey]);
                    wCol++;
                }

                SetHead(wDicHead.Keys.ToList());
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                throw ex;
            }

            return wDicHead;
        }

        /// <summary>
        /// 添加新空白行
        /// </summary>
        public void AppendRow(Dictionary<int, string> wDicDefaultValue)
        {
            try
            {
                if (this.Grd_Body.ColumnDefinitions.Count < 1)
                {
                    return;
                }

                //添加新的行定义
                RowDefinition wRowDef = new RowDefinition();
                wRowDef.Height = GridLength.Auto;
                this.Grd_Body.RowDefinitions.Add(wRowDef);
                //添加新的行中不同列的数据
                int wColNum = this.Grd_Body.ColumnDefinitions.Count;
                for (int i = 0; i < wColNum; i++)
                {
                    TextBox wTbx = null;
                    ComboBox wCbb = null;
                    int wRowNum = this.Grd_Body.RowDefinitions.Count;

                    if (mColNoCellItemTypeDic.ContainsKey(i))
                    {
                        switch (mColNoCellItemTypeDic[i].TypeEnum)
                        {
                            case ItemTypeEnum.全包区间:
                            case ItemTypeEnum.全开区间:
                            case ItemTypeEnum.右包区间:
                            case ItemTypeEnum.大于:
                            case ItemTypeEnum.大于等于:
                            case ItemTypeEnum.小于:
                            case ItemTypeEnum.小于等于:
                            case ItemTypeEnum.左包区间:
                            case ItemTypeEnum.等于:
                            case ItemTypeEnum.文本:
                                wTbx = new TextBox();
                                wTbx.Text = "";
                                wTbx.Name = string.Format("Txtbox_{0}_{1}", wRowNum - 1, i);

                                wTbx.FontSize = 20;
                                wTbx.Background = Brushes.White;
                                wTbx.HorizontalContentAlignment = HorizontalAlignment.Center;
                                wTbx.VerticalContentAlignment = VerticalAlignment.Center;
                                wTbx.Padding = new Thickness(6, 0, 6, 0);

                                if (!mColNoCellItemTypeDic[i].IsEdit)
                                    wTbx.IsReadOnly = true;

                                if (mColNoCellItemTypeDic[i].ValueLength > 0)
                                    wTbx.MinWidth = mColNoCellItemTypeDic[i].ValueLength;

                                if (!mColNoCellItemTypeDic[i].IsRepeat)
                                    System.Windows.Input.InputMethod.SetIsInputMethodEnabled(wTbx, false);


                                switch (mColNoCellItemTypeDic[i].DefalutValueType)
                                {
                                    case DefaultValueTypeEnum.操作员:
                                        wTbx.Text = GUD.mLoginUser.Name;
                                        break;
                                    case DefaultValueTypeEnum.时间:
                                        wTbx.Text = GetDefaultTime(mColNoCellItemTypeDic[i].DefaultValueForm);
                                        break;
                                    case DefaultValueTypeEnum.工单号:
                                        wTbx.Text = GetDefaultOrderNo();
                                        break;
                                    default:
                                        //默认值

                                        break;
                                }

                                if (i != 0 && wDicDefaultValue != null && wDicDefaultValue.ContainsKey(i) && !string.IsNullOrEmpty(wDicDefaultValue[i]))
                                    wTbx.Text = wDicDefaultValue[i];

                                wTbx.TextChanged += TextBox_TextChanged;
                                wTbx.PreviewKeyDown += wTbx_PreviewKeyDown;
                                wTbx.KeyDown += KeyDonwEvent;
                                if (i == 0)//序号
                                {
                                    wTbx.Text = "" + (wRowNum - 1);
                                    wTbx.GotMouseCapture += Row_GotMouseCapture;
                                    wTbx.IsReadOnly = true;
                                }
                                Grid.SetRow(wTbx, wRowNum - 1);
                                Grid.SetColumn(wTbx, i);
                                this.Grd_Body.Children.Add(wTbx);
                                break;
                            case ItemTypeEnum.是否:
                                Border wBorder = new Border();
                                wBorder.Name = string.Format("Border_{0}_{1}", wRowNum - 1, i);
                                wBorder.BorderThickness = new Thickness(0.5);
                                wBorder.BorderBrush = Brushes.Gray;
                                wBorder.Padding = new Thickness(6, 0, 6, 0);
                                StackPanel wStackPanelNew = new StackPanel();
                                wStackPanelNew.HorizontalAlignment = HorizontalAlignment.Center;
                                wStackPanelNew.Orientation = Orientation.Horizontal;
                                RadioButton RBT_Yes = new RadioButton();
                                RBT_Yes.Content = "是";
                                RBT_Yes.GroupName = string.Format("Choose_{0}_{1}", wRowNum - 1, i);
                                RBT_Yes.FontSize = 20;
                                RBT_Yes.VerticalContentAlignment = VerticalAlignment.Center;
                                RBT_Yes.Checked += new RoutedEventHandler((sender, e) => RadioButton_Checked(sender, e));
                                wStackPanelNew.Children.Add(RBT_Yes);

                                RadioButton RBT_No = new RadioButton();
                                RBT_No.Content = "否";
                                RBT_No.GroupName = string.Format("Choose_{0}_{1}", wRowNum - 1, i);
                                RBT_No.FontSize = 20;
                                RBT_No.VerticalContentAlignment = VerticalAlignment.Center;
                                RBT_No.Checked += new RoutedEventHandler((sender, e) => RadioButton_Checked(sender, e));

                                //默认值
                                if (wDicDefaultValue != null && wDicDefaultValue.ContainsKey(i) && !string.IsNullOrEmpty(wDicDefaultValue[i]))
                                {
                                    if (wDicDefaultValue[i].Equals("是"))
                                        RBT_Yes.IsChecked = true;
                                    else if (wDicDefaultValue[i].Equals("否"))
                                        RBT_No.IsChecked = true;
                                }

                                wStackPanelNew.Children.Add(RBT_No);
                                wBorder.Child = wStackPanelNew;
                                wBorder.PreviewKeyDown += wBorder_PreviewKeyDown;

                                if (!mColNoCellItemTypeDic[i].IsEdit)
                                    wBorder.IsEnabled = false;

                                Grid.SetRow(wBorder, wRowNum - 1);
                                Grid.SetColumn(wBorder, i);
                                this.Grd_Body.Children.Add(wBorder);
                                break;
                            case ItemTypeEnum.单选:
                                wCbb = new ComboBox();
                                wCbb.Name = string.Format("C_{0}_{1}", wRowNum - 1, i);
                                List<ComboxBinding> wComboxBindingList = GUD.GetComboxBindingByString(mColNoCellItemTypeDic[i].ItemDescription);
                                wCbb.DisplayMemberPath = "Text";
                                wCbb.SelectedValuePath = "ID";
                                wCbb.ItemsSource = wComboxBindingList;
                                wCbb.SelectedIndex = 0;
                                wCbb.FontSize = 20;
                                wCbb.Background = Brushes.White;
                                wCbb.DropDownClosed += Cbb_DropDownClosed;
                                wCbb.PreviewKeyDown += wCbb_PreviewKeyDown;
                                //默认值
                                if (wDicDefaultValue != null && wDicDefaultValue.ContainsKey(i) && !string.IsNullOrEmpty(wDicDefaultValue[i]))
                                {
                                    if (wComboxBindingList.Exists(p => p.Text.Equals(wDicDefaultValue[i])))
                                        wCbb.Text = wDicDefaultValue[i];
                                }

                                if (!mColNoCellItemTypeDic[i].IsEdit)
                                    wCbb.IsEnabled = false;

                                Grid.SetRow(wCbb, wRowNum - 1);
                                Grid.SetColumn(wCbb, i);
                                this.Grd_Body.Children.Add(wCbb);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }


        //选中行事件
        private void Row_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement wUie = sender as UIElement;
                int wRow = Grid.GetRow(wUie);
                if (this.mSelectRowList.Contains(Grid.GetRow(wUie)))
                {
                    mSelectRowList.Remove(Grid.GetRow(wUie));
                    this.ChangeRowOrColColor(wRow, 0, false, false, Brushes.White);
                }
                else
                {
                    mSelectRowList.Add(Grid.GetRow(wUie));
                    this.ChangeRowOrColColor(wRow, 0, false, false, Brushes.Gold);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void wCbb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsRouteContinue)
                {
                    IsRouteContinue = false;
                    return;
                }
                ComboBox wComboBox = (ComboBox)sender;
                int wRow = 0, wCol = 0;
                if (!string.IsNullOrEmpty(wComboBox.Name))
                {
                    this.GetControlRowCol(wComboBox.Name, out wRow, out wCol);
                }
                switch (e.Key)
                {
                    case Key.Add:
                        e.Handled = true;
                        this.mSaveDataHandler();
                        break;
                    case Key.Up:
                    case Key.Down:
                    case Key.Left:
                    case Key.Right:
                        if (wRow == 0 || wCol == 0)
                            return;
                        this.MoveRowOrCol(wRow, wCol, e.Key);
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void wBorder_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsRouteContinue)
                {
                    IsRouteContinue = false;
                    return;
                }
                Border wBorder = (Border)sender;
                int wRow = 0, wCol = 0;
                if (!string.IsNullOrEmpty(wBorder.Name))
                {
                    this.GetControlRowCol(wBorder.Name, out wRow, out wCol);
                }
                switch (e.Key)
                {
                    case Key.Add:
                        e.Handled = true;
                        this.mSaveDataHandler();
                        break;
                    case Key.Up:
                    case Key.Down:
                    case Key.Left:
                    case Key.Right:
                        if (wRow == 0 || wCol == 0)
                            return;
                        this.MoveRowOrCol(wRow, wCol, e.Key);
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void wTbx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsRouteContinue)
                {
                    IsRouteContinue = false;
                    return;
                }
                TextBox wTextBox = (TextBox)sender;
                int wRow = 0, wCol = 0;
                if (!string.IsNullOrEmpty(wTextBox.Name))
                {
                    this.GetControlRowCol(wTextBox.Name, out wRow, out wCol);
                }
                switch (e.Key)
                {
                    case Key.Add:
                        e.Handled = true;
                        this.mSaveDataHandler();
                        break;
                    case Key.Up:
                    case Key.Down:
                    case Key.Left:
                    case Key.Right:
                        if (wRow == 0 || wCol == 0)
                            return;
                        this.MoveRowOrCol(wRow, wCol, e.Key);
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 获取默认时间
        /// </summary>
        /// <param name="wForm">格式</param>
        /// <returns></returns>
        private string GetDefaultTime(string wForm)
        {
            string wDefaultTime = "";
            try
            {
                if (string.IsNullOrEmpty(wForm))
                    return string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (wForm.Contains("年"))
                    wDefaultTime += "" + DateTime.Now.Year;
                if (wForm.Contains("月"))
                    wDefaultTime += "/" + DateTime.Now.Month;
                if (wForm.Contains("日"))
                    wDefaultTime += "/" + DateTime.Now.Day + " ";

                if (wForm.Contains("时"))
                    wDefaultTime += "" + DateTime.Now.Hour;
                if (wForm.Contains("分"))
                    wDefaultTime += ":" + DateTime.Now.Minute;
                if (wForm.Contains("秒"))
                    wDefaultTime += ":" + DateTime.Now.Second;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wDefaultTime;
        }
        /// <summary>
        /// 获取当前工单号
        /// </summary>
        /// <returns></returns>
        private string GetDefaultOrderNo()
        {
            return "";
        }
        /// <summary>
        /// 设置列联动，编辑行改变后此列所有行变为同样内容
        /// </summary>
        /// <param name="wSourceCol"></param>
        /// <param name="wValue"></param>
        private void LinkageControl(int wCRow, int wSourceCol, string wValue, int wLinkRows)
        {
            try
            {
                if (wValue == null)
                    return;
                int wCount = 0;
                foreach (UIElement wItem in this.Grd_Body.Children)
                {
                    int wCol = Grid.GetColumn(wItem);
                    int wRow = Grid.GetRow(wItem);
                    //序号
                    if (wCol == 0 || wRow == 0)
                        continue;
                    if (wSourceCol != wCol)
                        continue;

                    if (wLinkRows > 0)
                    {
                        if (wRow <= wCRow + wCount)
                            continue;

                        if (wCount >= wLinkRows - 1)
                            break;
                    }
                    wCount++;
                    if (wItem is TextBox)
                    {
                        TextBox wTbx = wItem as TextBox;

                        //获取对象
                        if (wTbx != null && !wTbx.Text.Equals(wValue))
                        {
                            wTbx.TextChanged -= TextBox_TextChanged;
                            wTbx.Text = wValue;
                            wTbx.TextChanged += TextBox_TextChanged;
                        }
                    }
                    else if (wItem is Border)
                    {
                        Border wBorder = wItem as Border;
                        if (wBorder == null)
                            continue;
                        StackPanel wStackPanel = wBorder.Child as StackPanel;
                        if (wStackPanel == null)
                            continue;
                        if (wStackPanel.Children.Count > 0)
                        {
                            if (wStackPanel.Children[0] is RadioButton)
                            {
                                RadioButton wRbt_Yes = wStackPanel.Children[0] as RadioButton;
                                RadioButton wRbt_No = wStackPanel.Children[1] as RadioButton;
                                if (wValue.Equals("是") && wRbt_Yes.IsChecked == false)
                                    wRbt_Yes.IsChecked = true;
                                else if (wValue.Equals("否") && wRbt_No.IsChecked == false)
                                    wRbt_No.IsChecked = true;
                            }
                        }
                    }
                    else if (wItem is ComboBox)
                    {
                        ComboBox wCbb = wItem as ComboBox;
                        if (wCbb != null && !wCbb.Text.Equals(wValue))
                        {
                            wCbb.Text = wValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 改变关联参数值
        /// </summary>
        /// <param name="wCurrentValue"></param>
        /// <param name="wCol"></param>
        private void RelateCellValueChange(bool wIsScan, string wCurrentValue, int wCurrentRow, int wCurrentCol)
        {
            try
            {
                Dictionary<string, string> wDicRelationParmValue = new Dictionary<string, string>();

                if (!mColNoCellItemTypeDic.ContainsKey(wCurrentCol) || wCurrentValue == null)
                    return;
                if (string.IsNullOrEmpty(mColNoCellItemTypeDic[wCurrentCol].RelationParameter))
                    return;

                string wParmString = mColNoCellItemTypeDic[wCurrentCol].RelationParameter;
                string wValueString = mColNoCellItemTypeDic[wCurrentCol].RelationRatio;
                wParmString = wParmString.Replace("；", ";");
                string[] wParmArray = wParmString.Split(';');
                foreach (string wItem in wParmArray)
                {
                    if (!wDicRelationParmValue.ContainsKey(wItem))
                        wDicRelationParmValue.Add(wItem, "");
                }

                if (!string.IsNullOrEmpty(wValueString))
                {
                    wValueString = wValueString.Replace("；", ";");
                    string[] wValueArray = wValueString.Split(';');
                    int wCount = 0;
                    foreach (string wItem in wDicRelationParmValue.Keys.ToList())
                    {
                        if (wCount + 1 > wValueArray.Length)
                            break;
                        wDicRelationParmValue[wItem] = wValueArray[wCount];
                        wCount++;
                    }
                }
                foreach (string wParm in wDicRelationParmValue.Keys)
                {
                    if (string.IsNullOrEmpty(wDicRelationParmValue[wParm]) && wIsScan)//数据库查找关联数据
                    {
                        string wItemParm = wParm, wCItemParm = wParm;
                        if (wParm.Contains("跨工位"))//跨工位进行取值：跨工位_容量
                        {
                            string[] wArray = wParm.Split('_');
                            if (wArray.Length < 3)
                                return;
                            wItemParm = wArray[1];
                            wCItemParm = wArray[2];
                        }
                        //ExamineDataItem wExamineDataItem = DBManager.Instance.QueryExamineDataItemByItemName(
                        //    mColNoCellItemTypeDic[wCurrentCol].ItemName, wCurrentValue, wItemParm, "");
                        ExamineDataItem wExamineDataItem = new ExamineDataItem();
                        if (!string.IsNullOrEmpty(wExamineDataItem.Code))
                        {
                            this.ChangeRelationCellValue(wCurrentRow, wCItemParm, wExamineDataItem.ItemValue);
                        }
                    }
                    else//值引起关联值变化
                    {
                        if (wDicRelationParmValue[wParm].Contains("时间段"))
                        {
                            DateTime wNow = DateTime.Now;
                            DateTime wStart = wNow.AddMinutes(-30);
                            DateTime wEnd = wStart.AddHours(1);
                            string wText = wStart.ToString("HH:mm") + "~" + wEnd.ToString("HH:mm");
                            this.ChangeRelationCellValue(wCurrentRow, wParm, wText);
                        }
                        else if (wDicRelationParmValue[wParm].Contains("="))
                        {
                            this.ChangeRelationCellValue(wCurrentRow, wParm, wCurrentValue);
                        }
                        else if (wDicRelationParmValue[wParm].Contains("取值"))
                        {
                            string wValue = wDicRelationParmValue[wParm].Substring(wDicRelationParmValue[wParm].IndexOf("值") + 1);
                            if (wCurrentValue.Length == mColNoCellItemTypeDic[wCurrentCol].ValueLength)
                            {
                                string wReationCellValue = wCurrentValue.Substring(Convert.ToInt32(wValue) - 1, 1);
                                this.ChangeRelationCellValue(wCurrentRow, wParm, wReationCellValue);
                            }
                        }

                        else
                        {
                            try
                            {
                                float wValue = 0, wRatio = 0;
                                wValue = (float)Convert.ToDouble(wCurrentValue);
                                wRatio = (float)Convert.ToDouble(wDicRelationParmValue[wParm]);
                                string wDestValue = Math.Round(((wValue * wRatio)), 2).ToString();
                                this.ChangeRelationCellValue(wCurrentRow, wParm, wDestValue);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void ChangeRelationCellValue(int wCurrentRow, string wItemName, string wValue)
        {
            try
            {
                foreach (UIElement wItem in this.Grd_Body.Children)
                {
                    int wCol = Grid.GetColumn(wItem);
                    int wRow = Grid.GetRow(wItem);
                    //序号
                    if (wCol == 0)
                        continue;
                    if (wRow != wCurrentRow)
                        continue;
                    if (!mColNoCellItemTypeDic.ContainsKey(wCol))
                        continue;

                    if (!mColNoCellItemTypeDic[wCol].ItemName.Trim().Equals(wItemName.Trim()))
                        continue;
                    if (wItem is TextBox)
                    {
                        TextBox wTbx = wItem as TextBox;
                        //获取对象
                        if (wTbx != null && !wTbx.Text.Equals(wValue))
                        {
                            wTbx.Text = wValue;
                        }
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 换行或换列
        /// </summary>
        /// <param name="wCurrentRow">当前行数</param>
        /// <param name="wCurrentCol">当前列数</param>
        private void MoveRowOrCol(int wCurrentRow, int wCurrentCol, int wLinkRows)
        {
            try
            {
                int wNexRow = 0, wNextCol = 0;

                //switch ((RowColTypeEnum)GUD.CurrentStation.MESPort)
                switch ((RowColTypeEnum)1)
                {
                    case RowColTypeEnum.Col:
                    //wNexRow = wCurrentRow;
                    //wNextCol = wCurrentCol + 1;
                    //break;
                    case RowColTypeEnum.Row:
                        wNexRow = wCurrentRow + wLinkRows;
                        wNextCol = wCurrentCol;
                        break;
                }
                if (wNexRow > 0 && wNextCol > 0)
                {
                    foreach (UIElement wItem in this.Grd_Body.Children)
                    {
                        int wCol = Grid.GetColumn(wItem);
                        int wRow = Grid.GetRow(wItem);
                        //序号
                        if (wCol == 0)
                            continue;

                        if (wRow != wNexRow || wCol != wNextCol)//不为当前行列
                            continue;

                        if (wItem is TextBox)
                        {
                            Keyboard.Focus(wItem);
                            break;
                        }
                        else
                        {
                            this.MoveRowOrCol(wNexRow, wNextCol, wLinkRows);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 换行或换列
        /// </summary>
        /// <param name="wCurrentRow">当前行数</param>
        /// <param name="wCurrentCol">当前列数</param>
        private void MoveRowOrCol(int wCurrentRow, int wCurrentCol, Key wKey)
        {
            try
            {
                int wNexRow = 0, wNextCol = 0;

                switch (wKey)
                {
                    case Key.Up:
                        wNexRow = wCurrentRow - 1;
                        wNextCol = wCurrentCol;
                        break;
                    case Key.Down:
                        wNexRow = wCurrentRow + 1;
                        wNextCol = wCurrentCol;
                        break;
                    case Key.Left:
                        wNextCol = wCurrentCol - 1;
                        wNexRow = wCurrentRow;
                        break;
                    case Key.Right:
                        wNextCol = wCurrentCol + 1;
                        wNexRow = wCurrentRow;
                        break;
                }
                if (wNexRow < 1 || wCurrentCol < 1)
                    return;

                int wRows = this.Grd_Body.RowDefinitions.Count;
                int wCols = this.Grd_Body.ColumnDefinitions.Count;
                if (wNexRow > wRows - 1 || wNextCol > wCols - 1)
                    return;
                if (wNexRow > 0 && wNextCol > 0)
                {
                    foreach (UIElement wItem in this.Grd_Body.Children)
                    {
                        int wCol = Grid.GetColumn(wItem);
                        int wRow = Grid.GetRow(wItem);
                        //序号
                        if (wCol == 0)
                            continue;

                        if (wRow != wNexRow || wCol != wNextCol)//不为当前行列
                            continue;

                        if (wItem is Border)
                        {
                            StackPanel wStackPanel = ((Border)wItem).Child as StackPanel;
                            Keyboard.Focus(wStackPanel.Children[0]);
                        }
                        else if (wItem is ComboBox)
                        {
                            Keyboard.Focus(wItem);
                        }
                        else
                            Keyboard.Focus(wItem);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 选中行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// 删除选中行
        /// </summary>
        public void ClearSelectRows()
        {
            try
            {
                if (mSelectRowList == null || mSelectRowList.Count < 1)
                {
                    MessageBox.Show("没有选中的数据。请先选中数据行后重试。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认删除选中行？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                List<int> wSelectRowList = mSelectRowList;
                List<UIElement> wSelectToDelItemList = new List<UIElement>();
                List<RowDefinition> wSelectToDelRowList = new List<RowDefinition>();

                //记录要删除的子元素
                foreach (UIElement item in this.Grd_Body.Children)
                {
                    int wRow = Grid.GetRow(item);

                    if (this.mSelectRowList.Contains(Grid.GetRow(item)))
                    {
                        wSelectToDelItemList.Add(item);
                    }
                }

                //删除子元素
                foreach (UIElement item in wSelectToDelItemList)
                {
                    this.Grd_Body.Children.Remove(item);
                }

                wSelectToDelItemList.Clear();

                //记录要删除行定义
                for (int i = 0; i < this.Grd_Body.RowDefinitions.Count; i++)
                {
                    if (this.mSelectRowList.Contains(i))
                    {
                        wSelectToDelRowList.Add(this.Grd_Body.RowDefinitions[i]);
                    }
                }

                //删除行定义
                for (int i = 0; i < wSelectToDelRowList.Count; i++)
                {
                    this.Grd_Body.RowDefinitions.Remove(wSelectToDelRowList[i]);
                }

                //重定义元素行位置
                int wNewRow = 0, wPreViewRow = 0;
                foreach (UIElement item in this.Grd_Body.Children)
                {
                    //当前元素设定行数
                    int wRow = Grid.GetRow(item);
                    if (wRow > 0 && wRow != wPreViewRow)
                    {
                        wNewRow++;
                        wPreViewRow = wRow;
                    }
                    Grid.SetRow(item, wNewRow);
                }
                this.UpdateControlName();
                mSelectRowList.Clear();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 清除所有行
        /// </summary>
        /// <param name="NeedNotice">是否需要用户确认</param>
        public void ClearAll(bool NeedNotice)
        {
            try
            {
                List<UIElement> wSelectToDelItemList = new List<UIElement>();
                List<RowDefinition> wSelectToDelRowList = new List<RowDefinition>();
                foreach (UIElement item in this.Grd_Body.Children)
                {
                    int wRow = Grid.GetRow(item);
                    if (wRow > 0)
                    {
                        wSelectToDelItemList.Add(item);
                    }
                }

                if ((wSelectToDelItemList.Count > 0) && NeedNotice)
                {
                    // 弹人工确认窗口
                    //if (MessageBox.Show(
                    //    "清空前请确认所有需要的数据均已保存。是否清空表格？",
                    //    "警告",
                    //    MessageBoxButton.YesNo,
                    //    MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    //{
                    //    return;
                    //}
                    //else
                    //{
                    // 删除子元素
                    foreach (UIElement item in wSelectToDelItemList)
                    {
                        this.Grd_Body.Children.Remove(item);
                    }
                    //}
                }

                //记录要删除行定义
                for (int i = 0; i < this.Grd_Body.RowDefinitions.Count; i++)
                {
                    if (i > 0)
                    {
                        wSelectToDelRowList.Add(this.Grd_Body.RowDefinitions[i]);
                    }
                }

                for (int i = 0; i < wSelectToDelRowList.Count; i++)
                {
                    this.Grd_Body.RowDefinitions.Remove(wSelectToDelRowList[i]);
                }

            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        /// <summary>
        /// 改变行或列颜色
        /// </summary>
        /// <param name="wRow">行</param>
        /// <param name="wCol">列</param>
        /// <param name="wIsRow">是否是行,否则改变列颜色</param>
        /// <param name="wColor">颜色</param>
        /// <param name="wIsChoosed">是否选中</param>
        public void ChangeRowOrColColor(int wCRow, int wCCol, bool wIsRow, bool wIsChoosed, Brush wColor)
        {
            try
            {
                foreach (UIElement wUIElement in this.Grd_Body.Children)
                {
                    if (Grid.GetRow(wUIElement) != wCRow)
                        continue;

                    int wCol = Grid.GetColumn(wUIElement);
                    if (wIsChoosed && wCol == 0)//为选中改变颜色，序号列颜色不变
                        continue;
                    if (wIsRow)//行模式
                    {
                        this.SetUIElementColor(wUIElement, wColor);
                    }
                    else if (wCCol == wCol)//列模式
                    {
                        this.SetUIElementColor(wUIElement, wColor);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 获取界面所有行数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, string>> GetAllData(out string wError)
        {
            Dictionary<int, Dictionary<string, string>> wDicData = new Dictionary<int, Dictionary<string, string>>();
            wError = "";
            try
            {
                int wRowNum = this.Grd_Body.RowDefinitions.Count;
                for (int i = 1; i < wRowNum; i++)
                {
                    Dictionary<string, string> wRowDataList = this.GetRowData(i);
                    if (!wDicData.ContainsKey(i))
                    {
                        if (wRowDataList == null || (wRowDataList != null && wRowDataList.Count > 0))
                            wDicData.Add(i, wRowDataList);
                    }
                }
                wError = this.CheckColSameValue(wDicData);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wDicData;
        }
        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="wRow"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRowData(int wRow)
        {
            //实例化集合，用以存储一行各个单元格的数据
            Dictionary<string, string> wDicDataList = new Dictionary<string, string>();
            try
            {
                //初始化赋值，确定集合长度，排除序号
                for (int i = 1; i < this.Grd_Body.ColumnDefinitions.Count; i++)
                {
                    if (mColNoCellItemTypeDic.ContainsKey(i))
                        wDicDataList.Add(mColNoCellItemTypeDic[i].ItemName, null);
                }
                List<string> wIsRequiredList = new List<string>();//必填项数据集合
                foreach (UIElement wUIElement in this.Grd_Body.Children)
                {
                    if (Grid.GetRow(wUIElement) != wRow)
                        continue;
                    int wCol = Grid.GetColumn(wUIElement);
                    //序号
                    if (wCol == 0 || !mColNoCellItemTypeDic.ContainsKey(wCol))
                        continue;

                    wDicDataList[mColNoCellItemTypeDic[wCol].ItemName] = GetUIElementValue(wUIElement);

                    if (mColNoCellItemTypeDic.ContainsKey(wCol) && mColNoCellItemTypeDic[wCol].IsRequired)//检查此列是否设置为必填项
                    {
                        wIsRequiredList.Add(wDicDataList[mColNoCellItemTypeDic[wCol].ItemName]);
                    }
                    else if (wDicDataList[mColNoCellItemTypeDic[wCol].ItemName] == null)
                    {
                        wDicDataList[mColNoCellItemTypeDic[wCol].ItemName] = "";
                        if (mColNoCellItemTypeDic[wCol].TypeEnum == ItemTypeEnum.是否)
                            wDicDataList[mColNoCellItemTypeDic[wCol].ItemName] = "是";
                    }
                }
                if (wIsRequiredList.Exists(p => p == null) && wIsRequiredList.Exists(p => p != null))//必填项同时存在为null和不为null情况
                    return null;

                else if (wIsRequiredList.Count > 0 && !wIsRequiredList.Exists(p => p != null))//必填值全为null
                    return new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wDicDataList;
        }
        /// <summary>
        /// 获取控件值
        /// </summary>
        /// <param name="wUIElement"></param>
        /// <returns></returns>
        private string GetUIElementValue(UIElement wUIElement)
        {
            string wValue = null;
            try
            {
                if (wUIElement is TextBox)
                {
                    TextBox wTbx = wUIElement as TextBox;
                    //获取对象
                    if (wTbx != null && !string.IsNullOrEmpty(wTbx.Text))
                    {
                        wValue = wTbx.Text;
                    }
                }
                else if (wUIElement is Border)
                {
                    Border wBorder = wUIElement as Border;
                    StackPanel wStackPanel = wBorder.Child as StackPanel;
                    RadioButton wRbt_Yes = wStackPanel.Children[0] as RadioButton;
                    RadioButton wRbt_No = wStackPanel.Children[1] as RadioButton;

                    if (wRbt_Yes.IsChecked == false && wRbt_No.IsChecked == false)//未勾选
                    {
                        wValue = null;
                    }
                    else
                    {
                        if (wRbt_Yes.IsChecked == true)
                            wValue = "是";
                        else
                            wValue = "否";
                    }
                }
                else if (wUIElement is ComboBox)
                {
                    ComboBox wCbb = wUIElement as ComboBox;
                    if (wCbb.Text != null && !string.IsNullOrEmpty(wCbb.Text))
                        wValue = wCbb.Text.ToString();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wValue;
        }
        /// <summary>
        /// 设置控件颜色
        /// </summary>
        /// <param name="wUIElement"></param>
        /// <returns></returns>
        private string SetUIElementColor(UIElement wUIElement, Brush wColor)
        {
            string wValue = null;
            try
            {
                if (wUIElement is TextBox)
                {
                    TextBox wTbx = wUIElement as TextBox;
                    //获取对象
                    if (wTbx != null)
                        wTbx.Background = wColor;
                }
                else if (wUIElement is Border)
                {
                    Border wBorder = wUIElement as Border;
                    StackPanel wStackPanel = wBorder.Child as StackPanel;
                    RadioButton wRbt_Yes = wStackPanel.Children[0] as RadioButton;
                    RadioButton wRbt_No = wStackPanel.Children[1] as RadioButton;
                    wRbt_Yes.Background = wColor;
                    wRbt_No.Background = wColor;
                }
                else if (wUIElement is ComboBox)
                {
                    ComboBox wCbb = wUIElement as ComboBox;
                    if (wCbb != null)
                        wCbb.Background = wColor;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wValue;
        }
        /// <summary>
        /// 获取控件当前列所有数据
        /// </summary>
        /// <param name="wCurrentCol">当前列编号</param>
        /// <returns></returns>
        private Dictionary<int, string> GetColData(int wCurrentCol, out string wError)
        {
            Dictionary<int, string> wDicData = new Dictionary<int, string>();
            wError = "";
            try
            {
                foreach (UIElement wUIElement in this.Grd_Body.Children)
                {

                    int wRow = Grid.GetRow(wUIElement);
                    if (wRow == 0)//过滤表头
                        continue;

                    int wCol = Grid.GetColumn(wUIElement);

                    if (wCol != wCurrentCol)
                        continue;

                    string wValue = GetUIElementValue(wUIElement);
                    if (!string.IsNullOrEmpty(wValue))
                    {
                        if (!wDicData.ContainsKey(wRow))
                            wDicData.Add(wRow, wValue);
                    }
                }
            }
            catch (Exception ex)
            {
                wError = "888";
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wDicData;
        }
        /// <summary>
        ///检查下一行是否存空行
        /// </summary>
        /// <param name="wCRow"></param>
        /// <returns></returns>
        private bool CheckHaveNextRow(int wCRow, int wCCol)
        {
            bool wExist = false;
            try
            {
                foreach (UIElement wUIElement in this.Grd_Body.Children)
                {

                    int wRow = Grid.GetRow(wUIElement);
                    if (wRow == 0)
                        continue;

                    int wCol = Grid.GetColumn(wUIElement);
                    if (wRow != wCRow + 1)
                        continue;

                    if (wCCol != wCol)
                        continue;

                    wExist = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wExist;
        }
        /// <summary>
        /// 检查当前单元格数据是否重复
        /// </summary>
        /// <param name="wCCol"></param>
        /// <param name="wValue"></param>
        /// <returns></returns>
        private bool CheckIsRepeat(int wCRow, int wCCol, string wValue)
        {
            bool wResult = false;
            try
            {
                string wError = "";
                Dictionary<int, string> wDicData = this.GetColData(wCCol, out wError);

                wDicData.Remove(wCRow);

                wResult = wDicData.Values.Contains(wValue);

            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
        /// <summary>
        /// 如果当前列不允许重复，则查询当前填入数据是否与其它行此列数据相同
        /// </summary>
        /// <param name="wDicData">列表数据集合</param>
        /// <returns></returns>
        private string CheckColSameValue(Dictionary<int, Dictionary<string, string>> wTableData)
        {
            string wError = "";
            try
            {
                if (wTableData == null || wTableData.Count < 1)
                    return wError;

                Dictionary<string, List<string>> wDicColData = new Dictionary<string, List<string>>();
                foreach (int wKey in wTableData.Keys)
                {
                    if (wTableData[wKey] == null || wTableData[wKey].Count < 1)
                        continue;
                    foreach (string wItemName in wTableData[wKey].Keys)
                    {
                        if (!mColNameCellItemTypeDic.ContainsKey(wItemName)
                            || mColNameCellItemTypeDic[wItemName].IsRepeat || string.IsNullOrEmpty(wTableData[wKey][wItemName]))
                            continue;

                        if (!wDicColData.ContainsKey(wItemName))
                            wDicColData.Add(wItemName, new List<string> { wTableData[wKey][wItemName] });
                        else
                            wDicColData[wItemName].Add(wTableData[wKey][wItemName]);
                    }
                }

                foreach (string wKey in wDicColData.Keys)
                {
                    bool wHaveDuplicates = wDicColData[wKey].GroupBy(i => i).Where(g => g.Count() > 1).Count() >= 1;
                    if (wHaveDuplicates)
                    {
                        wError += string.Format("{0}列存在重复数据\r\n", wKey);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wError;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int wRow = 0, wCol = 0;
                TextBox wTextBox = (TextBox)sender;

                if (wTextBox != null && wTextBox.Text != null)
                {
                    this.GetControlRowCol(wTextBox.Name, out wRow, out wCol);
                    this.ChangeRowOrColColor(wRow, wCol, false, true, Brushes.White);
                    if (!mColNoCellItemTypeDic[wCol].IsEdit)
                        return;
                    if (mColNoCellItemTypeDic.ContainsKey(wCol))
                    {
                        if (mColNoCellItemTypeDic[wCol].ValueLength > 0)
                        {
                            if (wTextBox.Text.Length >= mColNoCellItemTypeDic[wCol].ValueLength)
                            {

                                if (!string.IsNullOrEmpty(mColNoCellItemTypeDic[wCol].RelationParameter))
                                    RelateCellValueChange(false, wTextBox.Text, wRow, wCol);

                                if (mColNoCellItemTypeDic[wCol].IsLinkControl)
                                    LinkageControl(wRow, wCol, wTextBox.Text, mColNoCellItemTypeDic[wCol].LinkRows);
                            }
                        }
                        else
                        {
                            if (mColNoCellItemTypeDic[wCol].IsLinkControl)
                                LinkageControl(wRow, wCol, wTextBox.Text, mColNoCellItemTypeDic[wCol].LinkRows);

                            if (!string.IsNullOrEmpty(mColNoCellItemTypeDic[wCol].RelationParameter))
                                RelateCellValueChange(false, wTextBox.Text, wRow, wCol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void KeyDonwEvent(object sender, KeyEventArgs e)
        {
            try
            {
                int wRow = 0, wCol = 0;
                IsRouteContinue = true;
                if (e.Key == Key.Enter)
                {
                    TextBox wTextBox = (TextBox)sender;
                    if (wTextBox != null && !string.IsNullOrEmpty(wTextBox.Text))
                    {
                        this.GetControlRowCol(wTextBox.Name, out wRow, out wCol);

                        this.ChangeRowOrColColor(wRow, wCol, false, true, Brushes.White);
                        this.ReplaceSpecialChar(wTextBox.Text.ToString(), wCol, wTextBox);
                        if (!mColNoCellItemTypeDic[wCol].IsEdit)
                            return;
                        if (mColNoCellItemTypeDic.ContainsKey(wCol) && wTextBox.Text.Length > 0)
                        {
                            if (mColNoCellItemTypeDic[wCol].IsLinkControl)
                                LinkageControl(wRow, wCol, wTextBox.Text, mColNoCellItemTypeDic[wCol].LinkRows);

                            if (!string.IsNullOrEmpty(mColNoCellItemTypeDic[wCol].RelationParameter))
                                RelateCellValueChange(true, wTextBox.Text, wRow, wCol);

                            if (!mColNoCellItemTypeDic[wCol].IsRepeat)
                            {
                                if (CheckIsRepeat(wRow, wCol, wTextBox.Text))
                                    this.ChangeRowOrColColor(wRow, wCol, false, true, Brushes.Yellow);

                                //bool wIsExist = DBManager.Instance.CheckItemValueIsExist(
                                //    mColNoCellItemTypeDic[wCol].ItemGroup, mColNoCellItemTypeDic[wCol].ItemName, wTextBox.Text);
                                bool wIsExist = false;
                                if (wIsExist)
                                    this.ChangeRowOrColColor(wRow, wCol, false, true, Brushes.Red);
                            }
                        }
                        //if (!CheckHaveNextRow(wRow + mColNoCellItemTypeDic[wCol].LinkRows, wCol))
                        //{
                        //    if (mColNoCellItemTypeDic[wCol].LinkRows > 0)
                        //    {
                        //        for (int i = 0; i < mColNoCellItemTypeDic[wCol].LinkRows + 1; i++)
                        //            AppendRow(null);
                        //    }
                        //    else
                        //        AppendRow(null);
                        //}

                        MoveRowOrCol(wRow, wCol, mColNoCellItemTypeDic[wCol].LinkRows);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                int wRow = 0, wCol = 0;
                RadioButton wRadioButton = (RadioButton)sender;
                this.GetControlRowCol(wRadioButton.Name, out wRow, out wCol);
                if (mColNoCellItemTypeDic.ContainsKey(wCol) && mColNoCellItemTypeDic[wCol].IsLinkControl)
                {
                    if (wRadioButton.Name.Contains("Yes"))
                        LinkageControl(wRow, wCol, "是", mColNoCellItemTypeDic[wCol].LinkRows);
                    else
                        LinkageControl(wRow, wCol, "否", mColNoCellItemTypeDic[wCol].LinkRows);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                               System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Cbb_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                int wRow = 0, wCol = 0;
                ComboBox wComboBox = (ComboBox)sender;
                if (wComboBox != null)
                {
                    this.GetControlRowCol(wComboBox.Name, out wRow, out wCol);
                    if (mColNoCellItemTypeDic.ContainsKey(wCol) && mColNoCellItemTypeDic[wCol].IsLinkControl)
                    {
                        LinkageControl(wRow, wCol, wComboBox.Text, mColNoCellItemTypeDic[wCol].LinkRows);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void UpdateControlName()
        {
            try
            {
                foreach (UIElement wUIElement in this.Grd_Body.Children)
                {
                    int wRow = Grid.GetRow(wUIElement);
                    int wCol = Grid.GetColumn(wUIElement);

                    if (wUIElement is TextBox)
                    {
                        TextBox wTbx = wUIElement as TextBox;
                        //为序号
                        if (wCol == 0)
                        {
                            wTbx.Name = string.Format("Txtbox_{0}_{1}", wRow, wCol);
                            wTbx.Text = wRow.ToString();
                        }
                        //获取对象
                        else if (wTbx != null)
                        {
                            wTbx.Name = string.Format("Txtbox_{0}_{1}", wRow, wCol);
                        }
                    }
                    else if (wUIElement is Border)
                    {
                        Border wBorder = wUIElement as Border;
                        wBorder.Name = string.Format("Border_{0}_{1}", wRow, wCol);
                        StackPanel wStackPanel = wBorder.Child as StackPanel;
                        RadioButton wRbt_Yes = wStackPanel.Children[0] as RadioButton;
                        RadioButton wRbt_No = wStackPanel.Children[1] as RadioButton;

                        wRbt_Yes.GroupName = string.Format("Choose_{0}_{1}", wRow, wCol);
                        wRbt_No.GroupName = string.Format("Choose_{0}_{1}", wRow, wCol);
                    }
                    else if (wUIElement is ComboBox)
                    {
                        ComboBox wCbb = wUIElement as ComboBox;
                        if (wCbb != null)
                            wCbb.Name = string.Format("C_{0}_{1}", wRow, wCol);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        /// <summary>
        /// 获取当前聚焦控件行列信息
        /// </summary>
        /// <param name="wControlName"></param>
        /// <param name="wRow"></param>
        /// <param name="wCol"></param>
        /// <returns></returns>
        private string GetControlRowCol(string wControlName, out int wRow, out int wCol)
        {
            string wResult = "";
            wRow = 0;
            wCol = 0;
            try
            {
                if (string.IsNullOrEmpty(wControlName))
                    return "控件名称为空！";

                string[] wArray = wControlName.Split('_');
                wRow = Convert.ToInt32(wArray[1]);
                wCol = Convert.ToInt32(wArray[2]);
            }
            catch (Exception ex)
            {
                wResult = ex.ToString();
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
        /// <summary>
        /// 正则表达式
        /// </summary>
        /// <param name="wInputValue">空间输入数据</param>
        /// <param name="wCol">控件列ID</param>
        /// <returns></returns>
        private string ReplaceSpecialChar(string wInputValue, int wCol, TextBox wTextBox)
        {
            string wValue = wInputValue;
            try
            {
                if (string.IsNullOrEmpty(wValue))
                    return wValue;

                if (mColNoCellItemTypeDic.ContainsKey(wCol)
                    && !string.IsNullOrEmpty(mColNoCellItemTypeDic[wCol].FilterChar))
                {
                    wValue = Regex.Replace(wInputValue, mColNoCellItemTypeDic[wCol].FilterChar, "");

                    if (!mColNoCellItemTypeDic[wCol].IsLinkControl)//是否联动
                    {
                        wTextBox.TextChanged -= TextBox_TextChanged;
                        wTextBox.Text = wValue;
                        wTextBox.TextChanged += TextBox_TextChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wValue;
        }
    }
}
