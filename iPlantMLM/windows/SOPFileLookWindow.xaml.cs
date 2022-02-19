using MoonPdfLib;
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
using System.Windows.Shapes;

namespace iPlantMLM
{
    /// <summary>
    /// SOPFileLookWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SOPFileLookWindow : Window
    {
        public SOPFileLookWindow()
        {
            InitializeComponent();
        }

        private SFCUploadSOP mSFCUploadSOP;
        public SOPFileLookWindow(SFCUploadSOP wUploadSOP)
        {
            InitializeComponent();
            try
            {
                mSFCUploadSOP = wUploadSOP;
                Lable_Title.Content = wUploadSOP.FileName;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (mSFCUploadSOP.Type)
                {
                    case 1://pdf
                        moonPdfPanel.OpenFile(mSFCUploadSOP.FilePath);
                        moonPdfPanel.Zoom(2.0);
                        break;
                    case 2://图片
                        moonPdfPanel.Visibility = Visibility.Collapsed;
                        SV_DateList.Visibility = Visibility.Visible;

                        Image wImage = new Image();
                        wImage.Source = new BitmapImage(new Uri(mSFCUploadSOP.FilePath));
                        this.SV_DateList.Content = wImage;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
