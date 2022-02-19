using Shris.NewEnergy.iPlant.Device;
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
using System.Windows.Shapes;

namespace iPlantMLM
{
    /// <summary>
    /// AddStandardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddStandardWindow : Window
    {
        public delegate void DeleSave();
        public event DeleSave DelSave;
        public AddStandardWindow()
        {
            InitializeComponent();
        }

        public AddStandardWindow(FPCProduct wFPCProduct, FPCPart wFPCPart)
        {
            InitializeComponent();
            try
            {
                Tbx_UserName.Text = wFPCProduct.ProductName;
                Tbx_UserName.Tag = wFPCProduct.ProductID;
                Tbx_PartName.Text = wFPCPart.PartName;
                Tbx_PartName.Tag = wFPCPart.PartID;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private IPTStandard mIPTStandard;
        public AddStandardWindow(IPTStandard wIPTStandard)
        {
            InitializeComponent();
            try
            {
                LoadUI(wIPTStandard);
                mIPTStandard = wIPTStandard;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void LoadUI(IPTStandard wIPTStandard)
        {
            try
            {
                Dictionary<int, string> wTypeDic = new Dictionary<int, string>();
                wTypeDic.Add(0, "文本");
                wTypeDic.Add(1, "单选");
                wTypeDic.Add(2, "全开区间");
                wTypeDic.Add(3, "全包区间");
                wTypeDic.Add(4, "右包区间");
                wTypeDic.Add(5, "左包区间");
                wTypeDic.Add(6, "小于");
                wTypeDic.Add(7, "大于");
                wTypeDic.Add(8, "小于等于");
                wTypeDic.Add(9, "大于等于");
                wTypeDic.Add(10, "等于");
                wTypeDic.Add(15, "是否");

                Cbb_Type.ItemsSource = wTypeDic;

                Cmd_Select.Visibility = Visibility.Collapsed;
                Tbx_UserName.Text = wIPTStandard.ProductName;
                Tbx_PartName.Text = wIPTStandard.PartName;
                Tbx_ItemCode.Text = wIPTStandard.ItemCode;
                Tbx_ItemCode.IsEnabled = false;
                Tbx_ItemName.Text = wIPTStandard.ItemName;
                Tbx_ItemName.IsEnabled = false;
                Cbb_Type.SelectedValue = wIPTStandard.Type;
                Tbx_DownLimit.Text = wIPTStandard.LowerLimit.ToString();
                Tbx_UpLimit.Text = wIPTStandard.UpperLimit.ToString();
                Tbx_UnitText.Text = wIPTStandard.UnitText;
                Tbx_DefaultValue.Text = wIPTStandard.DefaultValue;
                Tbx_TextExplain.Text = wIPTStandard.TextDescription;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        #region 选择检验项点
        private void Cmd_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Select_DoWork(s, exc);
                wBW.RunWorkerCompleted += (s, exc) => Select_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Select_DoWork(object s, DoWorkEventArgs e)
        {
            try
            {
                int wErrorCode = 0;
                List<IPTItem> wList = IPTItemDAO.Instance.IPT_QueryIPTItemList(-1, "", "", out wErrorCode);
                e.Result = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Select_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<IPTItem> wIPTItemList = e.Result as List<IPTItem>;
                List<EnumItem> wEnumItemList = new List<EnumItem>();
                if (wIPTItemList != null && wIPTItemList.Count > 0)
                {
                    for (int i = 0; i < wIPTItemList.Count; i++)
                    {
                        EnumItem wEnumItem = new EnumItem();
                        wEnumItem.ID = wIPTItemList[i].ID;
                        wEnumItem.Name = wIPTItemList[i].Name;
                        wEnumItem.Description = wIPTItemList[i].Code + "-" + wIPTItemList[i].Name;
                        wEnumItemList.Add(wEnumItem);
                    }
                }
                else
                {
                    MessageBox.Show("未查询到检测项点,请手动添加!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                List<EnumItem> wList = new List<EnumItem>();
                EnumItem wSelecttem = new EnumItem();

                ExcSelectUI wRROSelectUI = new ExcSelectUI(wEnumItemList, wList, SelectType.SingleSelect, "检测项点选择", true);
                wRROSelectUI.DelSave += (s) => CheckItemDelSave(s, wRROSelectUI);
                wRROSelectUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void CheckItemDelSave(List<EnumItem> wEnumItemList, ExcSelectUI wRROSelectUI)
        {
            try
            {
                int wProductID = (int)Tbx_UserName.Tag;
                int wPartID = (int)Tbx_PartName.Tag;

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => CheckItemDelSave_DoWork(s, exc, wProductID, wPartID, wEnumItemList[0].ID);
                wBW.RunWorkerCompleted += (s, exc) => CheckItemDelSave_RunWorkerCompleted(exc, wMyLoading, wEnumItemList, wRROSelectUI);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void CheckItemDelSave_DoWork(object s, DoWorkEventArgs e, int wProductID, int wPartID, int wItemID)
        {
            try
            {
                int wErrorCode = 0;
                List<IPTStandard> wList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wProductID, -1, wItemID, wPartID, -1, -1, out wErrorCode);
                if (wList.Count > 0)
                    e.Result = true;
                else
                    e.Result = false;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void CheckItemDelSave_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading, List<EnumItem> wEnumItemList, ExcSelectUI wRROSelectUI)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                    MessageBox.Show("该项点已存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    Tbx_ItemCode.Text = wEnumItemList[0].Description.Split('-')[0];
                    Tbx_ItemName.Text = wEnumItemList[0].Name;
                    Tbx_ItemCode.Tag = wEnumItemList[0].ID;
                    wRROSelectUI.Close();
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 保存
        private void Cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(Tbx_ItemCode.Text))
                //{
                //    MessageBox.Show("检验项点编号必填!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(Tbx_ItemName.Text))
                {
                    MessageBox.Show("检验项点名称必填!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                IPTStandard wIPTStandard = new IPTStandard();
                if (mIPTStandard == null)
                {
                    //①产品规格
                    wIPTStandard.ProductID = (int)Tbx_UserName.Tag;
                    wIPTStandard.ProductName = Tbx_UserName.Text;
                    //②工位名称
                    wIPTStandard.PartID = (int)Tbx_PartName.Tag;
                    wIPTStandard.PartName = Tbx_PartName.Text;
                    //③检验项点编号
                    //wIPTStandard.ItemCode = Tbx_ItemCode.Text;
                    //④检验项点名称
                    wIPTStandard.ItemName = Tbx_ItemName.Text;
                    //⑤检验项点ID
                    if (Tbx_ItemCode.Tag != null)
                        wIPTStandard.ItemID = (int)Tbx_ItemCode.Tag;
                    else
                        wIPTStandard.ItemID = 0;
                    //⑥数值类型
                    wIPTStandard.Type = (int)Cbb_Type.SelectedValue;
                    //⑦数值上限
                    double wUpperLimit = 0;
                    double.TryParse(Tbx_UpLimit.Text, out wUpperLimit);
                    wIPTStandard.UpperLimit = wUpperLimit;
                    //⑧数值下限
                    double wLowerLimit = 0;
                    double.TryParse(Tbx_DownLimit.Text, out wLowerLimit);
                    wIPTStandard.LowerLimit = wLowerLimit;
                    //⑨数值单位
                    wIPTStandard.UnitText = Tbx_UnitText.Text;
                    //⑩默认值
                    wIPTStandard.DefaultValue = Tbx_DefaultValue.Text;
                    //文本说明
                    wIPTStandard.TextDescription = Tbx_TextExplain.Text;
                    wIPTStandard.Active = 1;
                    wIPTStandard.ID = 0;
                    wIPTStandard.EditorID = GUD.mLoginUser.ID;
                    wIPTStandard.EditTime = DateTime.Now;
                }
                else
                {
                    wIPTStandard = mIPTStandard;
                    //⑥数值类型
                    wIPTStandard.Type = (int)Cbb_Type.SelectedValue;
                    //⑦数值上限
                    double wUpperLimit = 0;
                    double.TryParse(Tbx_UpLimit.Text, out wUpperLimit);
                    wIPTStandard.UpperLimit = wUpperLimit;
                    //⑧数值下限
                    double wLowerLimit = 0;
                    double.TryParse(Tbx_DownLimit.Text, out wLowerLimit);
                    wIPTStandard.LowerLimit = wLowerLimit;
                    //⑨数值单位
                    wIPTStandard.UnitText = Tbx_UnitText.Text;
                    //⑩默认值
                    wIPTStandard.DefaultValue = Tbx_DefaultValue.Text;
                    //文本说明
                    wIPTStandard.TextDescription = Tbx_TextExplain.Text;
                    wIPTStandard.EditorID = GUD.mLoginUser.ID;
                    wIPTStandard.EditTime = DateTime.Now;
                }

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => Save_DoWork(s, exc, wIPTStandard);
                wBW.RunWorkerCompleted += (s, exc) => Save_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        private void Save_DoWork(object s, DoWorkEventArgs e, IPTStandard wIPTStandard)
        {
            try
            {
                int wErrorCode = 0;
                //根据产品和工位查询个数
                List<IPTStandard> wList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wIPTStandard.ProductID, -1, -1, wIPTStandard.PartID, -1, -1, out wErrorCode);
                if (mIPTStandard == null)
                    wIPTStandard.OrderID = wList.Count + 1;

                //若项点没有，先保存项点
                if (wIPTStandard.ItemID <= 0)
                {
                    int wItemID = 0;
                    List<IPTItem> wItemList = IPTItemDAO.Instance.IPT_QueryIPTItemList(-1, wIPTStandard.ItemName, "", out wErrorCode);
                    if (wItemList.Count > 0)
                        wItemID = wItemList[0].ID;
                    else
                    {
                        IPTItem wIPTItem = new IPTItem();
                        wIPTItem.ID = 0;
                        wIPTItem.Code = IPTItemDAO.Instance.GetNewCode();
                        wIPTItem.Name = wIPTStandard.ItemName;

                        wItemID = IPTItemDAO.Instance.IPT_SaveIPTItem(wIPTItem, out wErrorCode);
                    }
                    wIPTStandard.ItemID = wItemID;
                }

                List<IPTStandard> wSList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wIPTStandard.ProductID, -1, wIPTStandard.ItemID, wIPTStandard.PartID, -1, -1, out wErrorCode);
                if (wSList.Count > 0 && mIPTStandard == null)
                    e.Result = false;
                else
                {
                    e.Result = true;
                    IPTStandardDAO.Instance.IPT_SaveIPTStandard(wIPTStandard, out wErrorCode);
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Save_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                bool wResult = (bool)e.Result;
                if (wResult)
                {
                    this.Close();
                    if (DelSave != null)
                        DelSave();
                }
                else
                    MessageBox.Show("保存失败，该检验项点已存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 窗体加载
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<int, string> wTypeDic = new Dictionary<int, string>();
                wTypeDic.Add(0, "文本");
                wTypeDic.Add(1, "单选");
                wTypeDic.Add(2, "全开区间");
                wTypeDic.Add(3, "全包区间");
                wTypeDic.Add(4, "右包区间");
                wTypeDic.Add(5, "左包区间");
                wTypeDic.Add(6, "小于");
                wTypeDic.Add(7, "大于");
                wTypeDic.Add(8, "小于等于");
                wTypeDic.Add(9, "大于等于");
                wTypeDic.Add(10, "等于");
                wTypeDic.Add(15, "是否");
                Cbb_Type.ItemsSource = wTypeDic;
                if (mIPTStandard == null)
                    Cbb_Type.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
