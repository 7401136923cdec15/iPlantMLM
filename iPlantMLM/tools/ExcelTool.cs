using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ShrisTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class ExcelTool
    {
        #region 单实例
        private ExcelTool() { }
        private static ExcelTool _Instance;

        public static ExcelTool Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ExcelTool();
                return ExcelTool._Instance;
            }
        }
        #endregion

        /// <summary>
        /// 封装Fun：将Excel表格转换为DataTable
        /// </summary>
        public DataTable ExcelToDatatable(string wPathExcel)
        {
            DataTable wDataTable = new DataTable();
            try
            {
                string wFileExtension = wPathExcel.Substring(wPathExcel.LastIndexOf('.') + 1);
                if (wFileExtension == "xlsx")
                {
                    FileStream wFile = new FileStream(wPathExcel, FileMode.Open, FileAccess.Read);
                    XSSFWorkbook wXSSFWorkbook = new XSSFWorkbook(wFile);
                    XSSFSheet wXSSFSheet = wXSSFWorkbook.GetSheetAt(0) as XSSFSheet;
                    XSSFRow wXSSFRow = wXSSFSheet.GetRow(0) as XSSFRow;
                    int wCellCount = wXSSFRow.LastCellNum;

                    for (int i = wXSSFRow.FirstCellNum; i < wCellCount; i++)
                    {
                        DataColumn wDataColumn = new DataColumn(wXSSFRow.GetCell(i).StringCellValue);
                        wDataTable.Columns.Add(wDataColumn);
                    }
                    //int rowCount = sheet.LastRowNum;
                    for (int i = wXSSFSheet.FirstRowNum + 1; i < wXSSFSheet.LastRowNum + 1; i++)
                    {
                        XSSFRow wXSSFRow_ = wXSSFSheet.GetRow(i) as XSSFRow;
                        if (wXSSFRow_ == null)
                            continue;
                        DataRow wDataRow_ = wDataTable.NewRow();
                        for (int j = wXSSFRow_.FirstCellNum; j < wCellCount; j++)
                        {
                            ICell wICell = wXSSFRow_.GetCell(j);
                            object wValue = null;
                            if (wICell != null && wICell.CellType != CellType.Blank)
                            {
                                switch (wICell.CellType)
                                {
                                    case CellType.String:
                                        string wString = wICell.StringCellValue;
                                        wValue = !string.IsNullOrEmpty(wString) ? wString.ToString() : string.Empty;
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(wICell))
                                            wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                        else
                                            wValue = wICell.NumericCellValue;
                                        break;
                                    case CellType.Boolean:
                                        wValue = wICell.BooleanCellValue;
                                        break;
                                    case CellType.Formula:
                                        {
                                            switch (wICell.CachedFormulaResultType)
                                            {
                                                case CellType.Boolean:
                                                    wValue = wICell.BooleanCellValue;
                                                    break;
                                                case CellType.Error:
                                                    wValue = ErrorEval.GetText(wICell.ErrorCellValue);
                                                    break;
                                                case CellType.Numeric:
                                                    if (DateUtil.IsCellDateFormatted(wICell))
                                                        wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                                    else
                                                        wValue = wICell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    string cellstr = wICell.StringCellValue;
                                                    wValue = !string.IsNullOrEmpty(cellstr) ? cellstr.ToString() : null;
                                                    break;
                                                case CellType.Unknown:
                                                case CellType.Blank:
                                                    break;
                                                default:
                                                    wValue = string.Empty;
                                                    break;
                                            }
                                            break;
                                        }
                                    default:
                                        wValue = wICell.StringCellValue;
                                        break;
                                }
                                wDataRow_[j] = wValue;
                            }
                        }
                        wDataTable.Rows.Add(wDataRow_);
                    }
                    wXSSFWorkbook = null;
                    wXSSFSheet = null;
                }
                else
                {
                    FileStream wFileStream = new FileStream(wPathExcel, FileMode.Open, FileAccess.Read);
                    HSSFWorkbook wHSSFWorkbook = new HSSFWorkbook(wFileStream);
                    HSSFSheet wHSSFSheet = wHSSFWorkbook.GetSheetAt(0) as HSSFSheet;
                    HSSFRow wHSSFRow = wHSSFSheet.GetRow(0) as HSSFRow;
                    int wCellCount = wHSSFRow.LastCellNum;

                    for (int i = wHSSFRow.FirstCellNum; i < wCellCount; i++)
                    {
                        DataColumn wDataColumn = new DataColumn(wHSSFRow.GetCell(i).StringCellValue);
                        wDataTable.Columns.Add(wDataColumn);
                    }
                    //int rowCount = sheet.LastRowNum;
                    for (int i = (wHSSFSheet.FirstRowNum + 1); i < wHSSFSheet.LastRowNum + 1; i++)
                    {
                        HSSFRow wHSSFRow_ = wHSSFSheet.GetRow(i) as HSSFRow;
                        if (wHSSFRow_ == null)
                            continue;
                        DataRow wDataRow_ = wDataTable.NewRow();
                        for (int j = wHSSFRow_.FirstCellNum; j < wCellCount; j++)
                        {
                            ICell wICell = wHSSFRow_.GetCell(j);
                            object wValue = null;
                            if (wICell != null && wICell.CellType != CellType.Blank)
                            {
                                switch (wICell.CellType)
                                {
                                    case CellType.String:
                                        string wStr = wICell.StringCellValue;
                                        wValue = !string.IsNullOrEmpty(wStr) ? wStr.ToString() : string.Empty;
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(wICell))
                                            wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                        else
                                            wValue = wICell.NumericCellValue;
                                        break;
                                    case CellType.Boolean:
                                        wValue = wICell.BooleanCellValue;
                                        break;
                                    case CellType.Formula:
                                        {
                                            switch (wICell.CachedFormulaResultType)
                                            {
                                                case CellType.Boolean:
                                                    wValue = wICell.BooleanCellValue;
                                                    break;
                                                case CellType.Error:
                                                    wValue = ErrorEval.GetText(wICell.ErrorCellValue);
                                                    break;
                                                case CellType.Numeric:
                                                    if (DateUtil.IsCellDateFormatted(wICell))
                                                        wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                                    else
                                                        wValue = wICell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    string cellstr = wICell.StringCellValue;
                                                    wValue = !string.IsNullOrEmpty(cellstr) ? cellstr.ToString() : null;
                                                    break;
                                                case CellType.Unknown:
                                                case CellType.Blank:
                                                    break;
                                                default:
                                                    wValue = string.Empty;
                                                    break;
                                            }
                                            break;
                                        }
                                    default:
                                        wValue = wICell.StringCellValue;
                                        break;
                                }
                                wDataRow_[j] = wValue;
                            }
                        }
                        wDataTable.Rows.Add(wDataRow_);
                    }
                    wHSSFWorkbook = null;
                    wHSSFSheet = null;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wDataTable;
        }

        /// <summary>
        /// 封装Fun：将Excel表格转换为DataTable
        /// </summary>
        public DataTable ExcelToDatatableWithTitle(string wPathExcel)
        {
            DataTable wDataTable = new DataTable();
            try
            {
                string wFileExtension = wPathExcel.Substring(wPathExcel.LastIndexOf('.') + 1);
                if (wFileExtension == "xlsx")
                {
                    FileStream wFile = new FileStream(wPathExcel, FileMode.Open, FileAccess.Read);
                    XSSFWorkbook wXSSFWorkbook = new XSSFWorkbook(wFile);
                    XSSFSheet wXSSFSheet = wXSSFWorkbook.GetSheetAt(0) as XSSFSheet;
                    XSSFRow wXSSFRow = wXSSFSheet.GetRow(0) as XSSFRow;
                    int wCellCount = wXSSFRow.LastCellNum;

                    for (int i = wXSSFRow.FirstCellNum; i < wCellCount; i++)
                    {
                        DataColumn wDataColumn = new DataColumn(wXSSFRow.GetCell(i).StringCellValue);
                        wDataTable.Columns.Add(wDataColumn);
                    }
                    //int rowCount = sheet.LastRowNum;
                    for (int i = wXSSFSheet.FirstRowNum; i < wXSSFSheet.LastRowNum + 1; i++)
                    {
                        XSSFRow wXSSFRow_ = wXSSFSheet.GetRow(i) as XSSFRow;
                        if (wXSSFRow_ == null)
                            continue;
                        DataRow wDataRow_ = wDataTable.NewRow();
                        for (int j = wXSSFRow_.FirstCellNum; j < wCellCount; j++)
                        {
                            ICell wICell = wXSSFRow_.GetCell(j);
                            object wValue = null;
                            if (wICell != null && wICell.CellType != CellType.Blank)
                            {
                                switch (wICell.CellType)
                                {
                                    case CellType.String:
                                        string wString = wICell.StringCellValue;
                                        wValue = !string.IsNullOrEmpty(wString) ? wString.ToString() : string.Empty;
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(wICell))
                                            wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                        else
                                            wValue = wICell.NumericCellValue;
                                        break;
                                    case CellType.Boolean:
                                        wValue = wICell.BooleanCellValue;
                                        break;
                                    case CellType.Formula:
                                        {
                                            switch (wICell.CachedFormulaResultType)
                                            {
                                                case CellType.Boolean:
                                                    wValue = wICell.BooleanCellValue;
                                                    break;
                                                case CellType.Error:
                                                    wValue = ErrorEval.GetText(wICell.ErrorCellValue);
                                                    break;
                                                case CellType.Numeric:
                                                    if (DateUtil.IsCellDateFormatted(wICell))
                                                        wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                                    else
                                                        wValue = wICell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    string cellstr = wICell.StringCellValue;
                                                    wValue = !string.IsNullOrEmpty(cellstr) ? cellstr.ToString() : null;
                                                    break;
                                                case CellType.Unknown:
                                                case CellType.Blank:
                                                    break;
                                                default:
                                                    wValue = string.Empty;
                                                    break;
                                            }
                                            break;
                                        }
                                    default:
                                        wValue = wICell.StringCellValue;
                                        break;
                                }
                                wDataRow_[j] = wValue;
                            }
                        }
                        wDataTable.Rows.Add(wDataRow_);
                    }
                    wXSSFWorkbook = null;
                    wXSSFSheet = null;
                }
                else
                {
                    FileStream wFileStream = new FileStream(wPathExcel, FileMode.Open, FileAccess.Read);
                    HSSFWorkbook wHSSFWorkbook = new HSSFWorkbook(wFileStream);
                    HSSFSheet wHSSFSheet = wHSSFWorkbook.GetSheetAt(0) as HSSFSheet;
                    HSSFRow wHSSFRow = wHSSFSheet.GetRow(0) as HSSFRow;
                    int wCellCount = wHSSFRow.LastCellNum;

                    for (int i = wHSSFRow.FirstCellNum; i < wCellCount; i++)
                    {
                        DataColumn wDataColumn = new DataColumn(wHSSFRow.GetCell(i).StringCellValue);
                        wDataTable.Columns.Add(wDataColumn);
                    }
                    //int rowCount = sheet.LastRowNum;
                    for (int i = (wHSSFSheet.FirstRowNum); i < wHSSFSheet.LastRowNum + 1; i++)
                    {
                        HSSFRow wHSSFRow_ = wHSSFSheet.GetRow(i) as HSSFRow;
                        if (wHSSFRow_ == null)
                            continue;
                        DataRow wDataRow_ = wDataTable.NewRow();
                        for (int j = wHSSFRow_.FirstCellNum; j < wCellCount; j++)
                        {
                            ICell wICell = wHSSFRow_.GetCell(j);
                            object wValue = null;
                            if (wICell != null && wICell.CellType != CellType.Blank)
                            {
                                switch (wICell.CellType)
                                {
                                    case CellType.String:
                                        string wStr = wICell.StringCellValue;
                                        wValue = !string.IsNullOrEmpty(wStr) ? wStr.ToString() : string.Empty;
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(wICell))
                                            wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                        else
                                            wValue = wICell.NumericCellValue;
                                        break;
                                    case CellType.Boolean:
                                        wValue = wICell.BooleanCellValue;
                                        break;
                                    case CellType.Formula:
                                        {
                                            switch (wICell.CachedFormulaResultType)
                                            {
                                                case CellType.Boolean:
                                                    wValue = wICell.BooleanCellValue;
                                                    break;
                                                case CellType.Error:
                                                    wValue = ErrorEval.GetText(wICell.ErrorCellValue);
                                                    break;
                                                case CellType.Numeric:
                                                    if (DateUtil.IsCellDateFormatted(wICell))
                                                        wValue = wICell.DateCellValue.ToString("yyyy-MM-dd");
                                                    else
                                                        wValue = wICell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    string cellstr = wICell.StringCellValue;
                                                    wValue = !string.IsNullOrEmpty(cellstr) ? cellstr.ToString() : null;
                                                    break;
                                                case CellType.Unknown:
                                                case CellType.Blank:
                                                    break;
                                                default:
                                                    wValue = string.Empty;
                                                    break;
                                            }
                                            break;
                                        }
                                    default:
                                        wValue = wICell.StringCellValue;
                                        break;
                                }
                                wDataRow_[j] = wValue;
                            }
                        }
                        wDataTable.Rows.Add(wDataRow_);
                    }
                    wHSSFWorkbook = null;
                    wHSSFSheet = null;
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wDataTable;
        }

        public void ExportToExcel(FileStream wFileStream, List<List<string>> wRowList, string wSheetName)
        {
            try
            {
                //创建excel工作簿
                IWorkbook wIWorkbook = new XSSFWorkbook();
                //创建excel单元格样式
                ICellStyle wICellStyle = wIWorkbook.CreateCellStyle();
                wICellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                wICellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                wICellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                wICellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                //水平对齐
                wICellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                //垂直对齐
                wICellStyle.VerticalAlignment = VerticalAlignment.Center;

                //设置字体
                IFont wFont = wIWorkbook.CreateFont();
                wFont.FontHeightInPoints = 18;
                //wFont.FontName = "微软雅黑";
                wICellStyle.SetFont(wFont);

                //创建表
                ISheet wISheet = wIWorkbook.CreateSheet(wSheetName);

                for (int i = 0; i < wRowList.Count; i++)
                {
                    //创建第一行
                    IRow wIRow = wISheet.CreateRow(i);
                    for (int j = 0; j < wRowList[i].Count; j++)
                    {
                        //设置第一列的宽度
                        wISheet.SetColumnWidth(j, 25 * 256);

                        ICell wICell = wIRow.CreateCell(j);
                        wICell.SetCellValue(wRowList[i][j]);
                        wICell.CellStyle = wICellStyle;
                    }
                }

                wIWorkbook.Write(wFileStream);

                wFileStream.Close();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
        }
    }
}
