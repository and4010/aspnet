using NPOI.SS.UserModel;
using System;

namespace CHPOUTSRCMES.Web.Util
{
    public class ExcelUtil
    {

        public static ICell FindCell(string contain, ISheet sheet, bool partialContain = false)
        {
            return FindCell(0, 0, contain, sheet, partialContain);
        }

        public static ICell FindCell(int start_y, int start_x, string contain, ISheet sheet, bool partialContain)
        {
            try
            {
                int noOfRow = sheet.LastRowNum;
                for (int y_pos = start_y; y_pos < noOfRow; y_pos++)
                {
                    IRow row = sheet.GetRow(y_pos);
                    for (int x_pos = start_x; x_pos < 100; x_pos++)
                    {
                        if (!partialContain && row.GetCell(x_pos) != null && string.Compare(GetStringCellValue(row.GetCell(x_pos)),contain) == 0)
                        {
                            return row.GetCell(x_pos);
                        } 
                        else if (partialContain && row.GetCell(x_pos) != null && GetStringCellValue(row.GetCell(x_pos)).Contains(contain))
                        {
                            return row.GetCell(x_pos);
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public static System.DateTime GetDateTimeCellValue(int row_index, int col_index, ISheet sheet, DateTime defaultValue = new DateTime())
        {
            var cellValue = GetDateTimeOrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
            return cellValue ?? defaultValue;
        }

        public static System.DateTime? GetDateTimeOrNullCellValue(int row_index, int col_index, ISheet sheet)
        {
            return GetDateTimeOrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
        }

        public static System.DateTime? GetDateTimeOrNullCellValue(ICell cell)
        {
            DateTime? datetime = null;
            try
            {
                datetime = cell.DateCellValue;
            }
            catch
            {

            }
            return datetime;
        }

        public static long GetLongCellValue(ICell cell)
        {
            var cellValue = GetLongOrNullCellValue(cell);
            return cellValue ?? 0;
        }

        public static long GetLongCellValue(int row_index, int col_index, ISheet sheet, long value = 0)
        {
            var cellValue = GetLongOrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
            return cellValue ?? value;
        }

        public static long? GetLongOrNullCellValue(int row_index, int col_index, ISheet sheet)
        {
            return GetLongOrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
        }

        public static long? GetLongOrNullCellValue(ICell cell)
        {
            long? value = null;
            try
            {
                value = long.Parse(GetStringCellValue(cell));
            }
            catch
            {

            }
            return value;
        }

        public static int GetInt32CellValue(ICell cell)
        {
            var cellValue = GetInt32OrNullCellValue(cell);
            return cellValue ?? 0;
        }

        public static int GetInt32CellValue(int row_index, int col_index, ISheet sheet, int value = 0)
        {
            var cellValue = GetInt32OrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
            return cellValue ?? value;
        }

        public static int? GetInt32OrNullCellValue(int row_index, int col_index, ISheet sheet)
        {
            return GetInt32OrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
        }

        public static int? GetInt32OrNullCellValue(ICell cell)
        {
            int? value = null;
            try
            {
                value = int.Parse(GetStringCellValue(cell));
            }
            catch
            {

            }
            return value;
        }

        public static decimal GetDecimalCellValue(int row_index, int col_index, ISheet sheet, decimal value = 0m)
        {
            var cellValue = GetDecimalOrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
            return cellValue ?? value;
        }

        public static decimal? GetDecimalOrNullCellValue(int row_index, int col_index, ISheet sheet)
        {
            return GetDecimalOrNullCellValue(sheet.GetRow(row_index).GetCell(col_index));
        }

        public static decimal? GetDecimalOrNullCellValue(ICell cell)
        {
            decimal? value = 0;
            try
            {
                value = decimal.Parse(GetStringCellValue(cell));
            }
            catch
            {

            }
            return value;
        }

        public static string GetStringCellValue(int row_index, int col_index, ISheet sheet)
        {
            return GetStringCellValue(sheet.GetRow(row_index).GetCell(col_index));
        }

        public static string GetStringCellValue(ICell cell)
        {
            if (cell == null)
            {
                return "";
            }
            return GetStringCellValue(cell, cell.CellType);
        }
        /// <summary>
        /// 依型別獲取單元格內容
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string GetStringCellValue(ICell cell, CellType type)
        {
            string Cellvalue;
            if (cell == null)
                return null;
            switch (type)
            {
                case CellType.Blank: //BLANK:  
                    Cellvalue = "";
                    break;
                case CellType.Boolean: //BOOLEAN:  
                    Cellvalue = cell.BooleanCellValue ? "是" : "否";
                    break;
                case CellType.Numeric: //NUMERIC:  
                    Cellvalue = cell.NumericCellValue.ToString();
                    break;
                case CellType.String: //STRING:  
                    Cellvalue = cell.StringCellValue;
                    break;
                case CellType.Formula: //FORMULA:  
                    Cellvalue = GetStringCellValue(cell, cell.CachedFormulaResultType);
                    break;
               
                default:
                    Cellvalue = cell.ToString();
                    break;
                  
            }
            return Cellvalue;
        }





    }
}