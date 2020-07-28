using NPOI.SS.UserModel;


namespace CHPOUTSRCMES.Web.Util
{
    public class ExcelUtil
    {

        public static ICell FindCell(string contain, ISheet sheet)
        {
            return FindCell(0, 0, contain, sheet);
        }


        public static ICell FindCell(int start_y, int start_x, string contain, ISheet sheet)
        {
            try
            {
                int noOfRow = sheet.LastRowNum;
                for (int y_pos = start_y; y_pos < noOfRow; y_pos++)
                {
                    IRow row = sheet.GetRow(y_pos);
                    for (int x_pos = start_x; x_pos < 100; x_pos++)
                    {
                        if (row.GetCell(x_pos) != null && GetCellString(row.GetCell(x_pos)).Contains(contain))
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


        public static string GetCellString(int row_index, int col_index, ISheet sheet)
        {
            return GetCellString(sheet.GetRow(row_index).GetCell(col_index));
        }

        public static string GetCellString(ICell cell)
        {
            if(cell == null)
            {
                return "";
            }
            return GetCellString(cell, cell.CellType);
        }

        /// <summary>
        /// 依型別獲取單元格內容
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string GetCellString(ICell cell, CellType type)
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
                    Cellvalue = GetCellString(cell, cell.CachedFormulaResultType);
                    break;
                default:
                    Cellvalue = cell.ToString();
                    break;
                  
            }
            return Cellvalue;
        }





    }
}