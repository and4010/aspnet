using CHPOUTSRCMES.Web.Data;
using CHPOUTSRCMES.Web.Db.Entiy;
using CHPOUTSRCMES.Web.Db.Entiy.Interfaces;
using CHPOUTSRCMES.Web.Db.Entiy.Repositorys;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Extensions.Logging;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db
{
    public class ModelInitializer : DropCreateDatabaseIfModelChanges<MesContext>
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Seed(MesContext context)
        {
            readFromXls(context);
            base.Seed(context);

        }


        internal static void readFromXls(MesContext context)
        {
            var baseDir = AppDomain.CurrentDomain
                               .BaseDirectory
                               .Replace("\\bin", string.Empty) + "Data\\Excel";

            string initialFile = baseDir + "\\ERP0193164.xlsx";

            if (!string.IsNullOrEmpty(initialFile) && File.Exists(initialFile))
            {
                using (FileStream fs = new FileStream(initialFile, FileMode.Open))
                {
                    IWorkbook workbook = null;
                    if (fs.Length > 0 && initialFile.Substring(initialFile.LastIndexOf(".")).Equals(".xls"))
                    {
                        //把xls文件中的数据写入wk中
                        workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        //把xlsx文件中的数据写入wk中
                        workbook = new XSSFWorkbook(fs);
                    }
                    importOrganization(workbook, context);
                    ImportSub(workbook, context);
                }
            }
        }



        private static void importOrganization(IWorkbook book, MesContext context)
        {
            {
                IRepository<ORGANIZATION_T> OrganizationRepositiory = new GenericRepository<ORGANIZATION_T>(context);
                if (book.NumberOfSheets == 0)
                {
                    return;
                }

                for (int i = 0; i < book.NumberOfSheets; i++)
                {
                    //獲取工作表(GetSheetAt)
                    if (!book.GetSheetAt(i).SheetName.Contains("XXCINV_SUBINVENTORY_V"))
                    {
                        continue;
                    }

                    ISheet sheet = book.GetSheetAt(i);
                    var noOfRow = sheet.LastRowNum;
                    int start_pos = 1;
                    for (int rowIterator = start_pos; rowIterator <= noOfRow; rowIterator++)
                    {
                        IRow row = sheet.GetRow(rowIterator);

                        if (row != null
                            && row.Cells.Count >= 3
                                && row.GetCell(0) != null && !string.IsNullOrEmpty(getCellString(row.GetCell(0)))
                                && row.GetCell(1) != null && !string.IsNullOrEmpty(getCellString(row.GetCell(1))))
                        {
                            try
                            {
                                var id = Int64.Parse(getCellString(row.GetCell(0)).Trim());
                                var org = OrganizationRepositiory.Get(x => x.OrganizationID == id);
                                if (org == null || org.OrganizationID <= 0)
                                {
                                    ORGANIZATION_T oRGANIZATION_T = new ORGANIZATION_T();
                                    oRGANIZATION_T.OrganizationID = Int64.Parse(getCellString(row.GetCell(0)).Trim());
                                    oRGANIZATION_T.OrganizationCode = getCellString(row.GetCell(1)).Trim();
                                    oRGANIZATION_T.OrganizationName = getCellString(row.GetCell(2)).Trim();
                                    OrganizationRepositiory.Create(oRGANIZATION_T);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                            }
                        }
                    }
                }
            }
        }


        private static void ImportSub(IWorkbook book, MesContext context)
        {
            IRepository<SUBINVENTORY_T> SubinventoryRepositiory = new GenericRepository<SUBINVENTORY_T>(context);
            if (book.NumberOfSheets == 0)
            {
                return;
            }
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                //獲取工作表(GetSheetAt)
                if (!book.GetSheetAt(i).SheetName.Contains("XXCINV_SUBINVENTORY_V"))
                {
                    continue;
                }

                ISheet sheet = book.GetSheetAt(i);
                var noOfRow = sheet.LastRowNum;
                int start_pos = 1;

                ICell organizationId_cell = null;
                ICell subinventoryCode_cell = null;
                ICell subinventoryName_cell = null;
                ICell ospFlag_cell = null;

                organizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
                if (organizationId_cell == null)
                {
                    throw new Exception("找不到ORGANIZATION_ID欄位");
                }

                subinventoryCode_cell = ExcelUtil.FindCell("SUBINVENTORY_CODE", sheet);
                if (subinventoryCode_cell == null)
                {
                    throw new Exception("找不到SUBINVENTORY_CODE欄位");
                }

                subinventoryName_cell = ExcelUtil.FindCell("SUBINVENTORY_NAME", sheet);
                if (subinventoryName_cell == null)
                {
                    throw new Exception("找不到SUBINVENTORY_NAME欄位");
                }
                ospFlag_cell = ExcelUtil.FindCell("OSP_FLAG", sheet);
                if (ospFlag_cell == null)
                {
                    throw new Exception("找不到OSP_FLAG欄位");
                }

                for (int rowIterator = start_pos; organizationId_cell.RowIndex + 1 <= noOfRow; rowIterator++)
                {
                    IRow row = sheet.GetRow(rowIterator);
                    if (row != null
                                && row.Cells.Count >= 14
                                && row.GetCell(0) != null && !string.IsNullOrEmpty(getCellString(row.GetCell(0)))
                                && row.GetCell(1) != null && !string.IsNullOrEmpty(getCellString(row.GetCell(1))))
                    {

                        try
                        {
                            var id = Int64.Parse(ExcelUtil.GetCellString(rowIterator, organizationId_cell.ColumnIndex, sheet).Trim());
                            var subcode = ExcelUtil.GetCellString(rowIterator, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                            var org = SubinventoryRepositiory.Get(x => x.OrganizationID == id && x.SsubinventoryCode == subcode);
                            if (org == null || org.OrganizationID <= 0)
                            {
                                SUBINVENTORY_T sUBINVENTORY_T = new SUBINVENTORY_T();
                                sUBINVENTORY_T.OrganizationID = Int64.Parse(ExcelUtil.GetCellString(rowIterator, organizationId_cell.ColumnIndex, sheet).Trim());
                                sUBINVENTORY_T.SsubinventoryCode = ExcelUtil.GetCellString(rowIterator, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                                sUBINVENTORY_T.SubinventoryName = ExcelUtil.GetCellString(rowIterator, subinventoryName_cell.ColumnIndex, sheet).Trim();
                                sUBINVENTORY_T.OspFlag = ExcelUtil.GetCellString(rowIterator, ospFlag_cell.ColumnIndex, sheet).Trim();
                                SubinventoryRepositiory.Create(sUBINVENTORY_T);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(LogUtilities.BuildExceptionMessage(ex));
                        }
                    }

                }

            }

        }


        private static string getCellString(ICell cell)
        {
            return getCellString(cell, cell.CellType);
        }

        private static string getCellString(ICell cell, CellType type)
        {
            string cellvalue = "";
            switch (type)
            {
                case CellType.String:
                    cellvalue = cell.StringCellValue;
                    break;
                case CellType.Numeric:
                    cellvalue = cell.NumericCellValue.ToString();
                    break;
                case CellType.Boolean:
                    cellvalue = cell.BooleanCellValue ? "是" : "否";
                    break;
                case CellType.Formula:
                    cellvalue = getCellString(cell, cell.CachedFormulaResultType);
                    break;
                default:
                    cellvalue = cell.ToString();
                    break;
            }
            return cellvalue;
        }

        private static int ConvertInt32(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

    }
}