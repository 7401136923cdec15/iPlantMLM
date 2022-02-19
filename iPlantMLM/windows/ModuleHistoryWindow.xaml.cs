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
    public partial class ModuleHistoryWindow : Window
    {
        public ModuleHistoryWindow()
        {
            InitializeComponent();
        }


        public ModuleHistoryWindow(SFCModuleRecord wSFCModuleRecord)
        {
            InitializeComponent();
            try
            {
                if (string.IsNullOrWhiteSpace(wSFCModuleRecord.ModuleNumber))
                    TB_Title.Text = string.Format("{0} 历史记录", wSFCModuleRecord.CapacitorPackageNo);
                else
                    TB_Title.Text = string.Format("{0} 历史记录", wSFCModuleRecord.ModuleNumber);

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
                DateTime wSBaseTime = new DateTime(2000, 1, 1);
                DateTime wEBaseTime = new DateTime(5000, 1, 1);
                List<SFCModuleRecord> wList = SFCModuleRecordDAO.Instance.SFC_QueryHistoryList(wSBaseTime, wEBaseTime, wSFCModuleRecord.CapacitorPackageNo, "", "", -1, "");
                e.Result = wList;
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

                List<SFCModuleRecord> wList = e.Result as List<SFCModuleRecord>;
                Xdg_MainGrid.DataSource = wList;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Xdg_MainGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SFCModuleRecord wSelectedDisplay = Xdg_MainGrid.SelectedItems.DataPresenter.ActiveDataItem as SFCModuleRecord;
                if (wSelectedDisplay == null)
                    return;

                HistoryRecordWindow wUI = new HistoryRecordWindow(wSelectedDisplay.CapacitorPackageNo);
                wUI.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
