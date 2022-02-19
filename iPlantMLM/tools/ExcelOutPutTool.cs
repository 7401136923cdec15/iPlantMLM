using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.ExcelExporter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace iPlantMLM
{
    public class ExcelOutPutTool
    {
        #region 单实例
        private ExcelOutPutTool() { }
        private static ExcelOutPutTool _Intance;
        public static ExcelOutPutTool Intance
        {
            get
            {
                if (_Intance == null)
                    _Intance = new ExcelOutPutTool();
                return ExcelOutPutTool._Intance;
            }
        }
        #endregion
        public string GetDialogPath()
        {
            string defaultPath = "";
            try
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                //打开的文件夹浏览对话框上的描述  
                dialog.Description = "请选择一个文件夹";
                //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
                dialog.ShowNewFolderButton = false;
                //首次defaultPath为空，按FolderBrowserDialog默认设置（即桌面）选择  
                if (defaultPath != "")
                {
                    //设置此次默认目录为上一次选中目录  
                    dialog.SelectedPath = defaultPath;
                }
                //按下确定选择的按钮  
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //记录选中的目录  
                    defaultPath = dialog.SelectedPath;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return defaultPath;
        }
        public string SaveDialogPath(string wFileName)
        {
            string wPath = "";
            try
            {
                Microsoft.Win32.SaveFileDialog wSaveFileDialog = new Microsoft.Win32.SaveFileDialog();

                //设置文件类型
                //书写规则例如：txt files(*.txt)|*.txt
                wSaveFileDialog.Filter = "xlsx files(*.xlsx)|*.xlsx";
                //设置默认文件名（可以不设置）
                if (wFileName.Length < 1)
                    wSaveFileDialog.FileName = DateTime.Now.ToString("yyyyMMdd");
                else
                    //wSaveFileDialog.FileName = wFileName + "_" + DateTime.Now.ToString("yyyyMMdd");
                    wSaveFileDialog.FileName = wFileName;
                //主设置默认文件extension（可以不设置）
                wSaveFileDialog.DefaultExt = "xlsx";
                //获取或设置一个值，该值指示如果用户省略扩展名，文件对话框是否自动在文件名中添加扩展名。（可以不设置）
                wSaveFileDialog.AddExtension = true;

                //设置默认文件类型显示顺序（可以不设置）
                wSaveFileDialog.FilterIndex = 1;

                //保存对话框是否记忆上次打开的目录
                wSaveFileDialog.RestoreDirectory = true;

                // Show save file dialog box
                bool? wResult = wSaveFileDialog.ShowDialog();
                //点了保存按钮进入
                if (wResult == true)
                {
                    //获得文件路径
                    wPath = wSaveFileDialog.FileName.ToString();
                    //获取文件名，不带路径
                    //fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                    //获取文件路径，不带文件名
                    //FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return wPath;
        }
        public int ExcelExportData(string wFileName, Infragistics.Windows.DataPresenter.XamDataGrid wXamDataGrid)
        {
            int wErrorCode = 0;
            try
            {
                if (wXamDataGrid == null || wXamDataGrid.Records.Count < 1)
                    return 1000;//表内容为空
                string wPath = ExcelOutPutTool.Intance.SaveDialogPath(wFileName);
                if (!string.IsNullOrEmpty(wPath))
                {
                    DataPresenterExcelExporter wExporter = new DataPresenterExcelExporter();
                    Workbook wWorkbook = wExporter.Export(wXamDataGrid, wPath, WorkbookFormat.Excel2007);// 

                    Process wProcess = new Process();
                    wProcess.StartInfo.FileName = wPath;
                    wProcess.Start();
                }
            }
            catch (Exception)
            {
                wErrorCode = 1002;//逻辑异常
                throw;
            }
            return wErrorCode;
        }
        //获取实体类里面所有的名称、值、DESCRIPTION值
        public string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name; //名称
                object value = item.GetValue(t, null);  //值
                string des = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute))).Description;// 属性值

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1}:{2},", name, value, des);
                }
                else
                {
                    getProperties(value);
                }
            }
            return tStr;
        }
        public int ExcelExportDataSource<T>(string wFileName, List<T> wList)
        {
            int wErrorCode = 0;
            Infragistics.Windows.DataPresenter.XamDataGrid wXamDataGrid = new Infragistics.Windows.DataPresenter.XamDataGrid();
            try
            {
                wXamDataGrid.BeginInit();
                T wModel = System.Activator.CreateInstance<T>();                     //实例化一个T类型对象
                PropertyInfo[] wPropertyInfos = wModel.GetType().GetProperties();     //获取T对象的所有公共属性
                FieldLayout wFieldLayout = new FieldLayout();
                foreach (PropertyInfo wPropertyInfo in wPropertyInfos)
                {
                    Field wField = new Field();
                    wField.Name = wPropertyInfo.Name;
                    if (((DescriptionAttribute)Attribute.GetCustomAttribute(wPropertyInfo, typeof(DescriptionAttribute))).Description != null)
                        wField.Label = ((DescriptionAttribute)Attribute.GetCustomAttribute(wPropertyInfo, typeof(DescriptionAttribute))).Description;
                    else
                        wField.Label = wPropertyInfo.Name;

                    wField.Visibility = ((BindableAttribute)Attribute.GetCustomAttribute(wPropertyInfo, typeof(BindableAttribute))).Bindable == true ? Visibility.Visible : Visibility.Collapsed;
                    wField.Width = FieldLength.Auto;
                    wFieldLayout.Fields.Add(wField);
                }
                wXamDataGrid.FieldLayouts.Add(wFieldLayout);
                wXamDataGrid.EndInit();
                wXamDataGrid.DataSource = null;
                wXamDataGrid.DataSource = wList;
                if (wXamDataGrid == null)
                    return 1000;//表内容为空
                string wPath = ExcelOutPutTool.Intance.SaveDialogPath(wFileName);
                if (!string.IsNullOrEmpty(wPath))
                {
                    DataPresenterExcelExporter wExporter = new DataPresenterExcelExporter();
                    Workbook wWorkbook = wExporter.Export(wXamDataGrid, wPath, WorkbookFormat.Excel2007);// 

                    Process wProcess = new Process();
                    wProcess.StartInfo.FileName = wPath;
                    wProcess.Start();
                }
            }
            catch (Exception)
            {
                wErrorCode = 1002;//逻辑异常
                throw;
            }
            return wErrorCode;
        }
    }
}
