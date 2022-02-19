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
    /// HistoryRecordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryRecordWindow : Window
    {
        public HistoryRecordWindow()
        {
            InitializeComponent();
        }

        public HistoryRecordWindow(string wCapacitorPackageNo)
        {
            InitializeComponent();
            try
            {
                MyLoading wMyLoading = new MyLoading();
                BackgroundWorker wBW = new BackgroundWorker();
                wBW.DoWork += (s, exc) => HistoryRecordWindow_DoWork(s, exc, wCapacitorPackageNo);
                wBW.RunWorkerCompleted += (s, exc) => HistoryRecordWindow_RunWorkerCompleted(exc, wMyLoading);
                wBW.RunWorkerAsync();
                wMyLoading.ShowDialog();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void HistoryRecordWindow_DoWork(object s, DoWorkEventArgs e, string wCapacitorPackageNo)
        {
            try
            {
                int wErrorCode = 0;
                MESConfig wConfig = MESConfigDAO.Instance.GetMESConfig();
                List<FPCPart> wFPCPartList = FPCPartDAO.Instance.GetPartList();
                wFPCPartList = wFPCPartList.OrderBy(p => p.PartID).ToList();

                SFCModuleRecord wSFCModuleRecord = null;
                List<SFCModuleRecord> wMRecordList = SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordList(-1, "", wCapacitorPackageNo, "", -1, "", -1, out wErrorCode);
                if (int.Parse(wConfig.CurrentPart.Split(',')[0]) > 7)
                {
                    wMRecordList = new List<SFCModuleRecord>();
                    wMRecordList.Add(SFCModuleRecordDAO.Instance.SFC_QuerySFCModuleRecordByCode(wCapacitorPackageNo, out wErrorCode));
                }
                if (wMRecordList.Count > 0)
                {
                    wMRecordList = wMRecordList.OrderByDescending(p => p.ID).ToList();
                    wSFCModuleRecord = wMRecordList[0];
                }
                if (wSFCModuleRecord != null)
                {
                    List<IPTBoolValue> wBoolValueList = IPTBoolValueDAO.Instance.IPT_QueryIPTBoolValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    List<IPTTextValue> wIPTTextValueList = IPTTextValueDAO.Instance.IPT_QueryIPTTextValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    List<IPTNumberValue> wIPTNumberValueList = IPTNumberValueDAO.Instance.IPT_QueryIPTNumberValueList(-1, wMRecordList[0].SerialNumber, -1, -1, -1, out wErrorCode);
                    //标准查询
                    Dictionary<FPCPart, List<IPTStandard>> wDic = new Dictionary<FPCPart, List<IPTStandard>>();
                    foreach (FPCPart wFPCPart in wFPCPartList)
                    {
                        List<IPTStandard> wList = IPTStandardDAO.Instance.IPT_QueryIPTStandardList(-1, wConfig.CurrentProduct, -1, -1, wFPCPart.PartID, -1, 1, out wErrorCode);
                        wDic.Add(wFPCPart, wList);
                    }
                    //返回数据整合
                    List<object> wObjectList = new List<object>();
                    wObjectList.Add(wFPCPartList);
                    wObjectList.Add(wBoolValueList);
                    wObjectList.Add(wIPTTextValueList);
                    wObjectList.Add(wIPTNumberValueList);
                    wObjectList.Add(wDic);
                    wObjectList.Add(wSFCModuleRecord);
                    e.Result = wObjectList;
                }
                else
                    e.Result = new List<object>();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void HistoryRecordWindow_RunWorkerCompleted(RunWorkerCompletedEventArgs e, MyLoading wMyLoading)
        {
            try
            {
                wMyLoading.Close();

                List<object> wObjectList = e.Result as List<object>;
                if (wObjectList.Count <= 0)
                {
                    MessageBox.Show("未查询到历史记录!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    List<FPCPart> wFPCPartList = wObjectList[0] as List<FPCPart>;
                    List<IPTBoolValue> wBoolValueList = wObjectList[1] as List<IPTBoolValue>;
                    List<IPTTextValue> wIPTTextValueList = wObjectList[2] as List<IPTTextValue>;
                    List<IPTNumberValue> wIPTNumberValueList = wObjectList[3] as List<IPTNumberValue>;
                    Dictionary<FPCPart, List<IPTStandard>> wDic = wObjectList[4] as Dictionary<FPCPart, List<IPTStandard>>;
                    SFCModuleRecord wSFCModuleRecord = wObjectList[5] as SFCModuleRecord;

                    SP_Main.Children.Clear();
                    foreach (FPCPart wFPCPart in wFPCPartList)
                    {
                        List<IPTStandard> wIPTStandardList = wDic[wFPCPart];
                        if (wIPTStandardList.Count <= 0)
                            continue;
                        if (!wBoolValueList.Exists(p => p.PartID == wFPCPart.PartID) && !wIPTTextValueList.Exists(p => p.PartID == wFPCPart.PartID) && !wIPTNumberValueList.Exists(p => p.PartID == wFPCPart.PartID))
                            continue;
                        PartCheckDataUC wPartCheckDataUC = new PartCheckDataUC(wFPCPart.PartName, wIPTStandardList, wSFCModuleRecord, wBoolValueList, wIPTNumberValueList, wIPTTextValueList);
                        SP_Main.Children.Add(wPartCheckDataUC);
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
