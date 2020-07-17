using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
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

namespace CHPOUTSRCMES.Web.DataModel
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
                    ImportLocater(workbook, context);
                    ImprotItemCd(workbook, context);
                    ImprotRelated(workbook, context);
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

                for (int j = organizationId_cell.RowIndex + 1; j <= noOfRow; j++)
                {
                    try
                    {
                        var id = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                        var subcode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                        var org = SubinventoryRepositiory.Get(x => x.OrganizationID == id && x.SubinventoryCode == subcode);
                        if (org == null || org.OrganizationID <= 0)
                        {
                            SUBINVENTORY_T sUBINVENTORY_T = new SUBINVENTORY_T();
                            sUBINVENTORY_T.OrganizationID = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                            sUBINVENTORY_T.SubinventoryCode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                            sUBINVENTORY_T.SubinventoryName = ExcelUtil.GetCellString(j, subinventoryName_cell.ColumnIndex, sheet).Trim();
                            sUBINVENTORY_T.OspFlag = ExcelUtil.GetCellString(j, ospFlag_cell.ColumnIndex, sheet).Trim();
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


        private static void ImportLocater(IWorkbook book, MesContext context)
        {
            IRepository<LOCATOR_T> LocatorTRepositiory = new GenericRepository<LOCATOR_T>(context);
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

                ICell organizationId_cell = null;
                ICell subinventoryCode_cell = null;
                ICell LocatorId_cell = null;
                ICell LocatorType_cell = null;
                ICell LocatorSegments_cell = null;
                ICell LocatorDesc_cell = null;
                ICell Segment1_cell = null;
                ICell Segment2_cell = null;
                ICell Segment3_cell = null;
                ICell Segment4_cell = null;

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

                LocatorId_cell = ExcelUtil.FindCell("LOCATOR_ID", sheet);
                if (LocatorId_cell == null)
                {
                    throw new Exception("找不到LOCATOR_ID欄位");
                }
                LocatorType_cell = ExcelUtil.FindCell("LOCATOR_TYPE", sheet);
                if (LocatorType_cell == null)
                {
                    throw new Exception("找不到LOCATOR_TYPE欄位");
                }
                LocatorSegments_cell = ExcelUtil.FindCell("LOCATOR_SEGMENTS", sheet);
                if (LocatorSegments_cell == null)
                {
                    throw new Exception("找不到LOCATOR_SEGMENTS欄位");
                }
                LocatorDesc_cell = ExcelUtil.FindCell("LOCATOR_DESC", sheet);
                if (LocatorDesc_cell == null)
                {
                    throw new Exception("找不到LOCATOR_DESC欄位");
                }
                Segment1_cell = ExcelUtil.FindCell("SEGMENT1", sheet);
                if (Segment1_cell == null)
                {
                    throw new Exception("找不到SEGMENT1欄位");
                }
                Segment2_cell = ExcelUtil.FindCell("SEGMENT2", sheet);
                if (Segment2_cell == null)
                {
                    throw new Exception("找不到SEGMENT2欄位");
                }
                Segment3_cell = ExcelUtil.FindCell("SEGMENT3", sheet);
                if (Segment3_cell == null)
                {
                    throw new Exception("找不到SEGMENT3欄位");
                }
                Segment4_cell = ExcelUtil.FindCell("SEGMENT4", sheet);
                if (Segment4_cell == null)
                {
                    throw new Exception("找不到SEGMENT4欄位");
                }

                for (int j = organizationId_cell.RowIndex + 1; j <= noOfRow; j++)
                {
                    try
                    {
                        var id = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                        var subcode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                        var org = LocatorTRepositiory.Get(x => x.LocatorId == id);
                        if (org == null || org.LocatorId <= 0)
                        {
                            LOCATOR_T lOCATOR_T = new LOCATOR_T();
                            lOCATOR_T.OrganizationId = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                            lOCATOR_T.SubinventoryCode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                            lOCATOR_T.LocatorId = Int64.Parse(ExcelUtil.GetCellString(j, LocatorId_cell.ColumnIndex, sheet).Trim());
                            lOCATOR_T.LocatorType = Int64.Parse(ExcelUtil.GetCellString(j, LocatorType_cell.ColumnIndex, sheet).Trim());
                            lOCATOR_T.LocatorSegments = ExcelUtil.GetCellString(j, LocatorSegments_cell.ColumnIndex, sheet).Trim();
                            lOCATOR_T.LocatorDesc = ExcelUtil.GetCellString(j, LocatorDesc_cell.ColumnIndex, sheet).Trim();
                            lOCATOR_T.Segment1 = ExcelUtil.GetCellString(j, Segment1_cell.ColumnIndex, sheet).Trim();
                            lOCATOR_T.Segment2 = ExcelUtil.GetCellString(j, Segment2_cell.ColumnIndex, sheet).Trim();
                            lOCATOR_T.Segment3 = ExcelUtil.GetCellString(j, Segment3_cell.ColumnIndex, sheet).Trim();
                            lOCATOR_T.Segment4 = ExcelUtil.GetCellString(j, Segment4_cell.ColumnIndex, sheet).Trim();
                            LocatorTRepositiory.Create(lOCATOR_T);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    }
                }
            }
        }


        private static void ImprotItemCd(IWorkbook book, MesContext context)
        {
            IRepository<ITEMS_T> ItemsTRepositiory = new GenericRepository<ITEMS_T>(context);
            if (book.NumberOfSheets == 0)
            {
                return;
            }

            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                //獲取工作表(GetSheetAt)
                if (!book.GetSheetAt(i).SheetName.Contains("XXIFV050_ITEMS_FTY_V"))
                {
                    continue;
                }

                ISheet sheet = book.GetSheetAt(i);
                var noOfRow = sheet.LastRowNum;
                ICell InventoryItemId_cell = null;
                ICell Item_number_cell = null;
                ICell CategoryCodeInv_cell = null;
                ICell CategoryNameInv_cell = null;
                ICell CategoryCodeCost_cell = null;
                ICell CategoryNameCost_cell = null;
                ICell CategoryCodeControl_cell = null;
                ICell CategoryNameControl_cell = null;
                ICell ItemDescEng_cell = null;
                ICell ItemDescSch_cell = null;
                ICell ItemDescTch_cell = null;
                ICell PrimaryUomCode_cell = null;
                ICell SecondaryUomCode_cell = null;
                ICell InventoryItemStatusCode_cell = null;
                ICell ItemType_cell = null;
                ICell CatalogElemVal010_cell = null;
                ICell CatalogElemVal020_cell = null;
                ICell CatalogElemVal030_cell = null;
                ICell CatalogElemVal040_cell = null;
                ICell CatalogElemVal050_cell = null;
                ICell CatalogElemVal060_cell = null;
                ICell CatalogElemVal070_cell = null;
                ICell CatalogElemVal080_cell = null;
                ICell CatalogElemVal090_cell = null;
                ICell CatalogElemVal100_cell = null;
                ICell CatalogElemVal110_cell = null;
                ICell CatalogElemVal120_cell = null;
                ICell CatalogElemVal130_cell = null;
                ICell CatalogElemVal140_cell = null;
                ICell ControlFlag_cell = null;
                ICell CreatedBy_cell = null;
                ICell CreationDate_cell = null;
                ICell LastUpdateBy_cell = null;
                ICell LastUpdateDate_cell = null;

                InventoryItemId_cell = ExcelUtil.FindCell("INVENTORY_ITEM_ID", sheet);
                if (InventoryItemId_cell == null)
                {
                    throw new Exception("找不到INVENTORY_ITEM_ID欄位");
                }

                Item_number_cell = ExcelUtil.FindCell("ITEM_NUMBER", sheet);
                if (Item_number_cell == null)
                {
                    throw new Exception("找不到ITEM_NUMBER欄位");
                }

                CategoryCodeInv_cell = ExcelUtil.FindCell("CATEGORY_CODE_INV", sheet);
                if (CategoryCodeInv_cell == null)
                {
                    throw new Exception("找不到CATEGORY_CODE_INV欄位");
                }
                CategoryNameInv_cell = ExcelUtil.FindCell("CATEGORY_NAME_INV", sheet);
                if (CategoryNameInv_cell == null)
                {
                    throw new Exception("找不到CATEGORY_NAME_INV欄位");
                }
                CategoryCodeCost_cell = ExcelUtil.FindCell("CATEGORY_CODE_COST", sheet);
                if (CategoryCodeCost_cell == null)
                {
                    throw new Exception("找不到Category_Code_Cost欄位");
                }
                CategoryNameCost_cell = ExcelUtil.FindCell("CATEGORY_NAME_COST", sheet);
                if (CategoryNameCost_cell == null)
                {
                    throw new Exception("找不到CATEGORY_NAME_COST欄位");
                }
                CategoryCodeControl_cell = ExcelUtil.FindCell("CATEGORY_CODE_CONTROL", sheet);
                if (CategoryCodeControl_cell == null)
                {
                    throw new Exception("找不到CATEGORY_CODE_CONTROL欄位");
                }
                CategoryNameControl_cell = ExcelUtil.FindCell("CATEGORY_NAME_CONTROL", sheet);
                if (CategoryNameControl_cell == null)
                {
                    throw new Exception("找不到CATEGORY_NAME_CONTROL欄位");
                }
                ItemDescEng_cell = ExcelUtil.FindCell("ITEM_DESC_ENG", sheet);
                if (ItemDescEng_cell == null)
                {
                    throw new Exception("找不到ITEM_DESC_ENG欄位");
                }
                ItemDescSch_cell = ExcelUtil.FindCell("ITEM_DESC_SCH", sheet);
                if (ItemDescSch_cell == null)
                {
                    throw new Exception("找不到ITEM_DESC_SCH欄位");
                }

                ItemDescTch_cell = ExcelUtil.FindCell("ITEM_DESC_TCH", sheet);
                if (ItemDescTch_cell == null)
                {
                    throw new Exception("找不到ITEM_DESC_TCH欄位");
                }
                PrimaryUomCode_cell = ExcelUtil.FindCell("PRIMARY_UOM_CODE", sheet);
                if (PrimaryUomCode_cell == null)
                {
                    throw new Exception("找不到PRIMARY_UOM_CODE欄位");
                }
                SecondaryUomCode_cell = ExcelUtil.FindCell("SECONDARY_UOM_CODE", sheet);
                if (SecondaryUomCode_cell == null)
                {
                    throw new Exception("找不到SECONDARY_UOM_CODE欄位");
                }
                InventoryItemStatusCode_cell = ExcelUtil.FindCell("INVENTORY_ITEM_STATUS_CODE", sheet);
                if (InventoryItemStatusCode_cell == null)
                {
                    throw new Exception("找不到INVENTORY_ITEM_STATUS_CODE欄位");
                }
                ItemType_cell = ExcelUtil.FindCell("ITEM_TYPE", sheet);
                if (ItemType_cell == null)
                {
                    throw new Exception("找不到ITEM_TYPE欄位");
                }
                CatalogElemVal010_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_010", sheet);
                if (CatalogElemVal010_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_010欄位");
                }
                CatalogElemVal020_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_020", sheet);
                if (CatalogElemVal020_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_020欄位");
                }

                CatalogElemVal030_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_030", sheet);
                if (CatalogElemVal030_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_030欄位");
                }

                CatalogElemVal040_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_040", sheet);
                if (CatalogElemVal040_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_040欄位");
                }

                CatalogElemVal050_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_050", sheet);
                if (CatalogElemVal050_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_050欄位");
                }

                CatalogElemVal060_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_060", sheet);
                if (CatalogElemVal060_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_060欄位");
                }

                CatalogElemVal070_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_070", sheet);
                if (CatalogElemVal070_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_070欄位");
                }

                CatalogElemVal080_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_080", sheet);
                if (CatalogElemVal080_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_080欄位");
                }

                CatalogElemVal090_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_090", sheet);
                if (CatalogElemVal090_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_090欄位");
                }

                CatalogElemVal100_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_100", sheet);
                if (CatalogElemVal100_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_100欄位");
                }

                CatalogElemVal110_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_110", sheet);
                if (CatalogElemVal110_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_110欄位");
                }

                CatalogElemVal120_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_120", sheet);
                if (CatalogElemVal120_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_120欄位");
                }

                CatalogElemVal130_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_130", sheet);
                if (CatalogElemVal130_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_130欄位");
                }

                CatalogElemVal140_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_140", sheet);
                if (CatalogElemVal140_cell == null)
                {
                    throw new Exception("找不到CATALOG_ELEM_VAL_140欄位");
                }

                //ControlFlag_cell = ExcelUtil.FindCell("CONTROL_FLAG", sheet);
                //if (ControlFlag_cell == null)
                //{
                //    throw new Exception("找不到CONTROL_FLAG欄位");
                //}

                CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
                if (CreatedBy_cell == null)
                {
                    throw new Exception("找不到CREATE_BY欄位");
                }
                CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
                if (CreationDate_cell == null)
                {
                    throw new Exception("找不到CREATE_DATE欄位");
                }
                LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
                if (LastUpdateBy_cell == null)
                {
                    throw new Exception("找不到LAST_UPDATE_BY欄位");
                }
                LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
                if (LastUpdateDate_cell == null)
                {
                    throw new Exception("找不到LAST_UPDATE_DATE欄位");
                }



                for (int j = InventoryItemId_cell.RowIndex + 1; j <= noOfRow; j++)
                {
                    try
                    {
                        var id = Int64.Parse(ExcelUtil.GetCellString(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                        var org = ItemsTRepositiory.Get(x => x.InventoryItemId == id);
                        if (org == null || org.InventoryItemId <= 0)
                        {
                            ITEMS_T iTEMS_T = new ITEMS_T();
                            iTEMS_T.InventoryItemId = Int64.Parse(ExcelUtil.GetCellString(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                            iTEMS_T.ItemNumber = ExcelUtil.GetCellString(j, Item_number_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CategoryCodeInv = ExcelUtil.GetCellString(j, CategoryCodeInv_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CategoryNameInv = ExcelUtil.GetCellString(j, CategoryNameInv_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CategoryCodeCost = ExcelUtil.GetCellString(j, CategoryCodeCost_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CategoryNameCost = ExcelUtil.GetCellString(j, CategoryNameCost_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CategoryCodeControl = ExcelUtil.GetCellString(j, CategoryCodeControl_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CategoryNameControl = ExcelUtil.GetCellString(j, CategoryNameControl_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.ItemDescEng = ExcelUtil.GetCellString(j, ItemDescEng_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.ItemDescSch = ExcelUtil.GetCellString(j, ItemDescSch_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.ItemDescTch = ExcelUtil.GetCellString(j, ItemDescTch_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.PrimaryUomCode = ExcelUtil.GetCellString(j, PrimaryUomCode_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.SecondaryUomCode = ExcelUtil.GetCellString(j, SecondaryUomCode_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.InventoryItemStatusCode = ExcelUtil.GetCellString(j, InventoryItemStatusCode_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.ItemType = ExcelUtil.GetCellString(j, ItemType_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal010 = ExcelUtil.GetCellString(j, CatalogElemVal010_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal020 = ExcelUtil.GetCellString(j, CatalogElemVal020_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal030 = ExcelUtil.GetCellString(j, CatalogElemVal030_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal040 = ExcelUtil.GetCellString(j, CatalogElemVal040_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal050 = ExcelUtil.GetCellString(j, CatalogElemVal050_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal060 = ExcelUtil.GetCellString(j, CatalogElemVal060_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal070 = ExcelUtil.GetCellString(j, CatalogElemVal070_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal080 = ExcelUtil.GetCellString(j, CatalogElemVal080_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal090 = ExcelUtil.GetCellString(j, CatalogElemVal090_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal100 = ExcelUtil.GetCellString(j, CatalogElemVal100_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal110 = ExcelUtil.GetCellString(j, CatalogElemVal110_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal120 = ExcelUtil.GetCellString(j, CatalogElemVal120_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal130 = ExcelUtil.GetCellString(j, CatalogElemVal130_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.CatalogElemVal140 = ExcelUtil.GetCellString(j, CatalogElemVal140_cell.ColumnIndex, sheet).Trim();
                            iTEMS_T.ControlFlag = "";
                            iTEMS_T.CreatedBy = Int64.Parse(ExcelUtil.GetCellString(j, CreatedBy_cell.ColumnIndex, sheet).Trim());
                            iTEMS_T.CreationDate = DateTime.Parse(ExcelUtil.GetCellString(j, CreationDate_cell.ColumnIndex, sheet).Trim());
                            iTEMS_T.LastUpdateBy = Int64.Parse(ExcelUtil.GetCellString(j, LastUpdateBy_cell.ColumnIndex, sheet).Trim());
                            iTEMS_T.LastUpdateDate = DateTime.Parse(ExcelUtil.GetCellString(j, LastUpdateDate_cell.ColumnIndex, sheet).Trim());
                            ItemsTRepositiory.Create(iTEMS_T);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    }
                }
            }

        }

        private static void ImprotRelated(IWorkbook book, MesContext context)
        {
            IRepository<RELATED_T> RelatedTRepositiory = new GenericRepository<RELATED_T>(context);
            if (book.NumberOfSheets == 0)
            {
                return;
            }
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                //獲取工作表(GetSheetAt)
                if (!book.GetSheetAt(i).SheetName.Contains("XXCINV_OSP_RELATED_ITEM_V"))
                {
                    continue;
                }

                ISheet sheet = book.GetSheetAt(i);
                var noOfRow = sheet.LastRowNum;
                ICell Related_id_cell = null;
                ICell InventoryItemId_cell = null;
                ICell ItemNumber_cell = null;
                ICell ItemDescription_cell = null;
                ICell RelatedItemId_cell = null;
                ICell RelatedItemNumber_cell = null;
                ICell RelatedItemDescription_cell = null;
                ICell CreatedBy_cell = null;
                ICell CreationDate_cell = null;
                ICell LastUpdateBy_cell = null;
                ICell LastUpdateDate_cell = null;


                //Related_id_cell = ExcelUtil.FindCell("RELATED_ITEM_ID", sheet);
                //if (Related_id_cell == null)
                //{
                //    throw new Exception("找不到RELATED_ITEM_ID欄位");
                //}

                InventoryItemId_cell = ExcelUtil.FindCell("INVENTORY_ITEM_ID", sheet);
                if (InventoryItemId_cell == null)
                {
                    throw new Exception("找不到INVENTORY_ITEM_ID欄位");
                }
                ItemNumber_cell = ExcelUtil.FindCell("ITEM", sheet);
                if (ItemNumber_cell == null)
                {
                    throw new Exception("找不到ITEM欄位");
                }

                ItemDescription_cell = ExcelUtil.FindCell("ITEM_DESCRIPTION", sheet);
                if (ItemDescription_cell == null)
                {
                    throw new Exception("找不到ITEM_DESCRIPTION欄位");
                }
                RelatedItemId_cell = ExcelUtil.FindCell("RELATED_ITEM_ID", sheet);
                if (RelatedItemId_cell == null)
                {
                    throw new Exception("找不到RELATED_ITEM_ID欄位");
                }
                RelatedItemNumber_cell = ExcelUtil.FindCell("RELATED_ITEM", sheet);
                if (RelatedItemNumber_cell == null)
                {
                    throw new Exception("找不到RELATED_ITEM欄位");
                }
                RelatedItemDescription_cell = ExcelUtil.FindCell("RELATED_ITEM_DESCRIPTION", sheet);
                if (RelatedItemDescription_cell == null)
                {
                    throw new Exception("找不到RELATED_ITEM_DESCRIPTION欄位");
                }

                CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
                if (CreatedBy_cell == null)
                {
                    throw new Exception("找不到CREATE_BY欄位");
                }
                CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
                if (CreationDate_cell == null)
                {
                    throw new Exception("找不到CREATE_DATE欄位");
                }
                LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
                if (LastUpdateBy_cell == null)
                {
                    throw new Exception("找不到LAST_UPDATE_BY欄位");
                }
                LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
                if (LastUpdateDate_cell == null)
                {
                    throw new Exception("找不到LAST_UPDATE_DATE欄位");
                }



                for (int j = InventoryItemId_cell.RowIndex + 1; j <= noOfRow; j++)
                {
                    try
                    {
                        var id = Int64.Parse(ExcelUtil.GetCellString(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                        var org = RelatedTRepositiory.Get(x => x.InventoryItemId == id);
                        if (org == null || org.InventoryItemId <= 0)
                        {
                            RELATED_T rELATED_T = new RELATED_T();
                            rELATED_T.InventoryItemId = Int64.Parse(ExcelUtil.GetCellString(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                            rELATED_T.ItemNumber = ExcelUtil.GetCellString(j, ItemNumber_cell.ColumnIndex, sheet).Trim();
                            rELATED_T.ItemDescription = ExcelUtil.GetCellString(j, ItemDescription_cell.ColumnIndex, sheet).Trim();
                            rELATED_T.RelatedItemId = Int64.Parse(ExcelUtil.GetCellString(j, RelatedItemId_cell.ColumnIndex, sheet).Trim());
                            rELATED_T.RelatedItemNumber = ExcelUtil.GetCellString(j, RelatedItemNumber_cell.ColumnIndex, sheet).Trim();
                            rELATED_T.RelatedItemDescription = ExcelUtil.GetCellString(j, RelatedItemDescription_cell.ColumnIndex, sheet).Trim();
                            rELATED_T.CreatedBy = Int64.Parse(ExcelUtil.GetCellString(j, CreatedBy_cell.ColumnIndex, sheet).Trim());
                            rELATED_T.CreationDate = DateTime.Parse(ExcelUtil.GetCellString(j, CreationDate_cell.ColumnIndex, sheet).Trim());
                            rELATED_T.LastUpdateBy = Int64.Parse(ExcelUtil.GetCellString(j, LastUpdateBy_cell.ColumnIndex, sheet).Trim());
                            rELATED_T.LastUpdateDate = DateTime.Parse(ExcelUtil.GetCellString(j, LastUpdateDate_cell.ColumnIndex, sheet).Trim());
                            RelatedTRepositiory.Create(rELATED_T);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(LogUtilities.BuildExceptionMessage(ex));
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