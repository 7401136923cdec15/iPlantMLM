using ShrisTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace iPlantMLM
{
    /// <summary>
    /// BasicConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class BasicConfigPage : Page
    {
        public delegate void DeleSave();
        public event DeleSave DelSave;

        public BasicConfigPage()
        {
            InitializeComponent();

            InitializeForm();
        }

        #region 初始化
        private void InitializeForm()
        {
            try
            {
                List<FPCProduct> wProductList = FPCProductDAO.Instance.GetProductList();
                Cbb_ChangeFocusType.ItemsSource = wProductList;
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();

                for (int i = 0; i < wProductList.Count; i++)
                {
                    if (wConfig.CurrentProduct == wProductList[i].ProductID)
                        Cbb_ChangeFocusType.SelectedIndex = i;
                }

                List<FPCPart> wPartList = FPCPartDAO.Instance.GetPartList();
                foreach (FPCPart wFPCPart in wPartList)
                {
                    List<string> wPartIDList = wConfig.CurrentPart.Split(',').ToList();
                    if (wPartIDList.Exists(p => p.Equals(wFPCPart.PartID.ToString())))
                    {
                        PartUC wUC = new PartUC(wFPCPart.PartName, true);
                        wUC.mPart = wFPCPart;
                        SP_PartList.Children.Add(wUC);
                    }
                    else
                    {
                        PartUC wUC = new PartUC(wFPCPart.PartName, false);
                        wUC.mPart = wFPCPart;
                        SP_PartList.Children.Add(wUC);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion

        #region 保存
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //①获取选中的产品型号
                FPCProduct wProduct = Cbb_ChangeFocusType.SelectedItem as FPCProduct;
                //②获取选中的工位
                List<FPCPart> wList = new List<FPCPart>();
                foreach (PartUC wPartUC in SP_PartList.Children)
                {
                    if (wPartUC.mIsChecked)
                        wList.Add(wPartUC.mPart);
                }
                if (wList.Count <= 0)
                {
                    MessageBox.Show("请勾选当前工位!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
                    return;

                string wPartIDs = string.Join(",", wList.Select(p => p.PartID));
                MESConfig wMESConfig = new MESConfig();
                wMESConfig.CurrentPart = wPartIDs;
                wMESConfig.CurrentProduct = wProduct.ProductID;
                MESConfigDAO.Instance.SaveMESConfig(wMESConfig);

                if (DelSave != null)
                    DelSave();

                MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
        #endregion
    }
}
