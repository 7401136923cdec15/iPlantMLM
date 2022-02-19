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
    /// ModuleHistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SingleInfoWindow : Window
    {
        public SingleInfoWindow()
        {
            InitializeComponent();
        }


        public SingleInfoWindow(SFCModuleRecord wSFCModuleRecord)
        {
            InitializeComponent();
            try
            {
                if (string.IsNullOrWhiteSpace(wSFCModuleRecord.ModuleNumber))
                    TB_Title.Text = string.Format("{0} 单体信息", wSFCModuleRecord.CapacitorPackageNo);
                else
                    TB_Title.Text = string.Format("{0} 单体信息", wSFCModuleRecord.ModuleNumber);

                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => ModuleHistoryWindow_DoWork(s, exc, wSFCModuleRecord);
                wBW.RunWorkerCompleted += (s, exc) => ModuleHistoryWindow_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void ModuleHistoryWindow_DoWork(object s, DoWorkEventArgs e, SFCModuleRecord wSFCModuleRecord)
        {
            try
            {
                string wInfo = SFCModuleRecordDAO.Instance.GetSingleInfo(wSFCModuleRecord.CapacitorPackageNo);
                e.Result = wInfo;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void ModuleHistoryWindow_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                string wInfo = e.Result as string;
                if (string.IsNullOrWhiteSpace(wInfo))
                {
                    MessageBox.Show("未查询到单体信息!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    WP_Main.Children.Clear();
                    string[] wValues = wInfo.Split(',');
                    int wIndex = 1;
                    foreach (string wValue in wValues)
                    {
                        SingleUC wUC = new SingleUC("单体" + wIndex, wValue);
                        WP_Main.Children.Add(wUC);
                        wIndex++;
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
