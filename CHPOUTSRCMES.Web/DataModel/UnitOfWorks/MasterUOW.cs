using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.Entiy.Information;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks.Interfaces;
using CHPOUTSRCMES.Web.Util;
using NLog;
using NPOI.SS.UserModel;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models.Information;
using System.Text;
using System.Data.SqlClient;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class MasterUOW: UnitOfWork
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 料號
        /// </summary>
        private readonly IItemsTRepository itemsTRepositiory;
        /// <summary>
        /// 組織料號
        /// </summary>
        private readonly IRepository<ORG_ITEMS_T> orgItemRepositityory;
        /// <summary>
        /// 組織
        /// </summary>
        private readonly IRepository<ORGANIZATION_T> organizationRepositiory;
        /// <summary>
        /// 倉庫
        /// </summary>
        private readonly IRepository<SUBINVENTORY_T> subinventoryRepositiory;
        /// <summary>
        /// 儲位
        /// </summary>
        private readonly IRepository<LOCATOR_T> locatorTRepositiory;
        /// <summary>
        /// 餘切規格
        /// </summary>
        private readonly IRepository<RELATED_T> relatedTRepositiory;
        /// <summary>
        /// 令重包數
        /// </summary>
        private readonly IRepository<YSZMPCKQ_T> yszmpckqTRepositiory;
        /// <summary>
        /// 機台紙別
        /// </summary>
        private readonly IRepository<MACHINE_PAPER_TYPE_T> machinePaperTypeRepositiory;
        /// <summary>
        /// 庫存交易類別
        /// </summary>
        private readonly IRepository<TRANSACTION_TYPE_T> transactionTypeRepositiory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public MasterUOW(DbContext context) : base(context)
        {
            this.itemsTRepositiory = new ItemsTRepository(this);
            this.orgItemRepositityory = new GenericRepository<ORG_ITEMS_T>(this);
            this.organizationRepositiory = new GenericRepository<ORGANIZATION_T>(this);
            this.subinventoryRepositiory = new GenericRepository<SUBINVENTORY_T>(this);
            this.locatorTRepositiory = new GenericRepository<LOCATOR_T>(this);
            this.relatedTRepositiory = new GenericRepository<RELATED_T>(this);
            this.yszmpckqTRepositiory = new GenericRepository<YSZMPCKQ_T>(this);
            this.machinePaperTypeRepositiory = new GenericRepository<MACHINE_PAPER_TYPE_T>(this);
            this.transactionTypeRepositiory = new GenericRepository<TRANSACTION_TYPE_T>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="itemsTRepository"></param>
        /// <param name="orgItemRepository"></param>
        /// <param name="organizationRepositiory"></param>
        /// <param name="subinventoryRepositiory"></param>
        /// <param name="locatorTRepositiory"></param>
        /// <param name="relatedTRepositiory"></param>
        /// <param name="yszmpckqTRepositiory"></param>
        /// <param name="machinePaperTypeRepositiory"></param>
        /// <param name="transactionTypeRepositiory"></param>
        public MasterUOW( DbContext context,
            IItemsTRepository itemsTRepository, IRepository<ORG_ITEMS_T> orgItemRepository,
            IRepository<ORGANIZATION_T> organizationRepositiory, IRepository<SUBINVENTORY_T> subinventoryRepositiory,
            IRepository<LOCATOR_T> locatorTRepositiory, IRepository<RELATED_T> relatedTRepositiory,
            IRepository<YSZMPCKQ_T> yszmpckqTRepositiory, IRepository<MACHINE_PAPER_TYPE_T> machinePaperTypeRepositiory,
            IRepository<TRANSACTION_TYPE_T> transactionTypeRepositiory
            ) : base(context)
        {
            this.itemsTRepositiory = itemsTRepository;
            this.orgItemRepositityory = orgItemRepository;
            this.organizationRepositiory = organizationRepositiory;
            this.subinventoryRepositiory = subinventoryRepositiory;
            this.locatorTRepositiory = locatorTRepositiory;
            this.relatedTRepositiory = relatedTRepositiory;
            this.yszmpckqTRepositiory = yszmpckqTRepositiory;
            this.machinePaperTypeRepositiory = machinePaperTypeRepositiory;
            this.transactionTypeRepositiory = transactionTypeRepositiory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        public void Import(IWorkbook book)
        {
            //大量寫入資料時，請關閉AutoDetectChangesEnabled 功能來提高效能
            this.Context.Configuration.AutoDetectChangesEnabled = false;
            //高度相關作業資料處理時，請使用 交易來 確保資料完整性
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    ImportOrganization(book);
                    ImportSubinventory(book);
                    ImportLocater(book);
                    ImportItems(book);
                    ImprotRelated(book);
                    ImportYszmpckq(book);
                    ImprotMachinePaperType(book);
                    ImprotTransaction(book);
                    //成功時，提交所有處理
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    //失敗，取消所有處理動作
                    txn.Rollback();
                }
            }
            this.Context.Configuration.AutoDetectChangesEnabled = true;
        }

        private void ImportItems(IWorkbook book)
        {

            ISheet sheet = FindSheet(book, "XXIFV050_ITEMS_FTY_V");

            if (sheet == null) return;


            var noOfRow = sheet.LastRowNum;
            ICell ORGANIZATION_CODE_cell = null;
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

            ORGANIZATION_CODE_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (ORGANIZATION_CODE_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

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
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<ITEMS_T>().Where(x => x.Entity.InventoryItemId == id).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = itemsTRepositiory.Get(x => x.InventoryItemId == id).FirstOrDefault();
                    if (org == null || org.Entity.InventoryItemId <= 0)
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
                        itemsTRepositiory.Create(iTEMS_T);


                        ORG_ITEMS_T oRG_ITEMS_T = new ORG_ITEMS_T();
                        var oCode = ExcelUtil.GetCellString(j, ORGANIZATION_CODE_cell.ColumnIndex, sheet).Trim();
                        //搜尋未執行 SaveChanges 的資料
                        var data = this.Context.ChangeTracker.Entries<ORGANIZATION_T>().Where(x => x.Entity.OrganizationCode == oCode).FirstOrDefault();
                        //搜尋已執行 SaveChanges 的資料
                        //var o = organizationRepositiory.Get(x => x.OrganizationCode == ocode).FirstOrDefault();
                        if (data != null)
                        {
                            var entity = data.Entity;
                            oRG_ITEMS_T.InventoryItemId = Int64.Parse(ExcelUtil.GetCellString(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                            oRG_ITEMS_T.OrganizationId = entity.OrganizationId;
                            orgItemRepositityory.Create(oRG_ITEMS_T);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("第 {0} 列 出現錯誤 {1}", j + 1, LogUtilities.BuildExceptionMessage(ex));
                }
            }

            this.SaveChanges();
        }

        private void ImportOrganization(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_SUBINVENTORY_V");

            if (sheet == null) return;

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
                        //搜尋未執行 SaveChanges 的資料
                        var org = this.Context.ChangeTracker.Entries<ORGANIZATION_T>().Where(x => x.Entity.OrganizationId == id).FirstOrDefault();
                        //搜尋已執行 SaveChanges 的資料
                        //var org = organizationRepositiory.Get(x => x.OrganizationID == id).FirstOrDefault();
                        if (org == null)
                        {
                            ORGANIZATION_T oRGANIZATION_T = new ORGANIZATION_T();
                            oRGANIZATION_T.OrganizationId = Int64.Parse(getCellString(row.GetCell(0)).Trim());
                            oRGANIZATION_T.OrganizationCode = getCellString(row.GetCell(1)).Trim();
                            oRGANIZATION_T.OrganizationName = getCellString(row.GetCell(2)).Trim();
                            oRGANIZATION_T.ControlFlag = "";
                            organizationRepositiory.Create(oRGANIZATION_T);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    }
                }
            }

            this.SaveChanges();
        }

        private void ImportSubinventory(IWorkbook book)
        {

            ISheet sheet = FindSheet(book, "XXCINV_SUBINVENTORY_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;

            ICell organizationId_cell = null;
            ICell subinventoryCode_cell = null;
            ICell subinventoryName_cell = null;
            ICell ospFlag_cell = null;
            ICell LocatorType_cell = null;

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
            LocatorType_cell = ExcelUtil.FindCell("LOCATOR_TYPE", sheet);
            if (LocatorType_cell == null)
            {
                throw new Exception("找不到LOCATOR_TYPE欄位");
            }

            for (int j = organizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var id = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                    var subCode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<SUBINVENTORY_T>().Where(x => x.Entity.SubinventoryCode == subCode).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                   // var org = subinventoryRepositiory.Get(x => x.OrganizationID == id && x.SubinventoryCode == subCode).FirstOrDefault();
                    if (org == null)
                    {
                        SUBINVENTORY_T sUBINVENTORY_T = new SUBINVENTORY_T();
                        sUBINVENTORY_T.OrganizationId = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                        sUBINVENTORY_T.SubinventoryCode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                        sUBINVENTORY_T.SubinventoryName = ExcelUtil.GetCellString(j, subinventoryName_cell.ColumnIndex, sheet).Trim();
                        sUBINVENTORY_T.OspFlag = ExcelUtil.GetCellString(j, ospFlag_cell.ColumnIndex, sheet).Trim();
                        sUBINVENTORY_T.ControlFlag = "";
                        sUBINVENTORY_T.LocatorType = Int64.Parse(ExcelUtil.GetCellString(j, LocatorType_cell.ColumnIndex, sheet).Trim());
                        subinventoryRepositiory.Create(sUBINVENTORY_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }

            this.SaveChanges();
        }

        private void ImportLocater(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_SUBINVENTORY_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;

            ICell organizationId_cell = null;
            ICell subinventoryCode_cell = null;
            ICell LocatorId_cell = null;
            //ICell LocatorType_cell = null;
            ICell LocatorSegments_cell = null;
            ICell LocatorDesc_cell = null;
            ICell Segment1_cell = null;
            ICell Segment2_cell = null;
            ICell Segment3_cell = null;
            ICell Segment4_cell = null;
            ICell LocatorStatus_cell = null;
            ICell LocatorStatusCode_cell = null;
            ICell LocatorPickingOrder_cell = null;
            ICell LocatorDisableDate_cell = null;

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
            //LocatorType_cell = ExcelUtil.FindCell("LOCATOR_TYPE", sheet);
            //if (LocatorType_cell == null)
            //{
            //    throw new Exception("找不到LOCATOR_TYPE欄位");
            //}
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
            LocatorStatus_cell = ExcelUtil.FindCell("LOCATOR_STATUS", sheet);
            if (LocatorStatus_cell == null)
            {
                throw new Exception("找不到LOCATOR_STATUS欄位");
            }
            LocatorStatusCode_cell = ExcelUtil.FindCell("LOCATOR_STATUS_CODE", sheet);
            if (LocatorStatusCode_cell == null)
            {
                throw new Exception("找不到LOCATOR_STATUS_CODE欄位");
            }
            LocatorPickingOrder_cell = ExcelUtil.FindCell("LOCATOR_PICKING_ORDER", sheet);
            if (LocatorPickingOrder_cell == null)
            {
                throw new Exception("找不到LOCATOR_PICKING_ORDER欄位");
            }
            LocatorDisableDate_cell = ExcelUtil.FindCell("LOCATOR_DISABLE_DATE", sheet);
            if (LocatorDisableDate_cell == null)
            {
                throw new Exception("找不到LOCATOR_DISABLE_DATE欄位");
            }


            for (int j = organizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    string idString = ExcelUtil.GetCellString(j, LocatorId_cell.ColumnIndex, sheet).Trim();
                    if (string.IsNullOrEmpty(idString)) continue;

                    var id = Int64.Parse(idString);
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<LOCATOR_T>().Where(x => x.Entity.LocatorId == id).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = locatorTRepositiory.Get(x => x.LocatorId == id).FirstOrDefault();
                    if (org == null || org.Entity.LocatorId <= 0)
                    {
                        LOCATOR_T lOCATOR_T = new LOCATOR_T();
                        lOCATOR_T.OrganizationId = Int64.Parse(ExcelUtil.GetCellString(j, organizationId_cell.ColumnIndex, sheet).Trim());
                        lOCATOR_T.SubinventoryCode = ExcelUtil.GetCellString(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.LocatorId = Int64.Parse(ExcelUtil.GetCellString(j, LocatorId_cell.ColumnIndex, sheet).Trim());
                        //lOCATOR_T.LocatorType = Int64.Parse(ExcelUtil.GetCellString(j, LocatorType_cell.ColumnIndex, sheet).Trim());
                        lOCATOR_T.LocatorSegments = ExcelUtil.GetCellString(j, LocatorSegments_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.LocatorDesc = ExcelUtil.GetCellString(j, LocatorDesc_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment1 = ExcelUtil.GetCellString(j, Segment1_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment2 = ExcelUtil.GetCellString(j, Segment2_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment3 = ExcelUtil.GetCellString(j, Segment3_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment4 = ExcelUtil.GetCellString(j, Segment4_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.ControlFlag = "";
                        lOCATOR_T.LocatorStatus = Int64.Parse(ExcelUtil.GetCellString(j, LocatorStatus_cell.ColumnIndex, sheet).Trim());
                        lOCATOR_T.LocatorStatusCode = ExcelUtil.GetCellString(j, LocatorStatusCode_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.LocatorPickingOrder = Int64.Parse(ExcelUtil.GetCellString(j, LocatorPickingOrder_cell.ColumnIndex, sheet).Trim());
                        lOCATOR_T.LocatorDisableDate = null;
                        locatorTRepositiory.Create(lOCATOR_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImprotRelated(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_OSP_RELATED_ITEM_V");

            if (sheet == null) return;

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
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<RELATED_T>().Where(x => x.Entity.InventoryItemId == id).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = relatedTRepositiory.Get(x => x.InventoryItemId == id).FirstOrDefault();
                    if (org == null || org.Entity.InventoryItemId <= 0)
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
                        rELATED_T.ControlFlag = "";
                        relatedTRepositiory.Create(rELATED_T);

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImportYszmpckq(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCOM_YSZMPCKQ_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell OrganizationId_cell = null;
            ICell OrganizationCode_cell = null;
            ICell OspSubinventory_cell = null;
            ICell Pstyp_cell = null;
            ICell Bwetup_cell = null;
            ICell Bwetdn_cell = null;
            ICell Rwtup_cell = null;
            ICell Rwtdn_cell = null;
            ICell Pckq_cell = null;
            ICell PaperQty_cell = null;
            ICell PiecesQty_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            OrganizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (OrganizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }
            OrganizationCode_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (OrganizationCode_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

            OspSubinventory_cell = ExcelUtil.FindCell("OSP_SUBINVENTORY", sheet);
            if (OspSubinventory_cell == null)
            {
                throw new Exception("找不到OSP_SUBINVENTORY欄位");
            }
            Pstyp_cell = ExcelUtil.FindCell("PSTYP", sheet);
            if (Pstyp_cell == null)
            {
                throw new Exception("找不到PSTYP欄位");
            }
            Bwetup_cell = ExcelUtil.FindCell("BWETUP", sheet);
            if (Bwetup_cell == null)
            {
                throw new Exception("找不到BWETUP欄位");
            }
            Bwetdn_cell = ExcelUtil.FindCell("BWETDN", sheet);
            if (Bwetdn_cell == null)
            {
                throw new Exception("找不到BWETDN欄位");
            }
            Rwtup_cell = ExcelUtil.FindCell("RWTUP", sheet);
            if (Rwtup_cell == null)
            {
                throw new Exception("找不到RWTUP欄位");
            }
            Rwtdn_cell = ExcelUtil.FindCell("RWTDN", sheet);
            if (Rwtdn_cell == null)
            {
                throw new Exception("找不到RWTDN欄位");
            }
            Pckq_cell = ExcelUtil.FindCell("PCKQ", sheet);
            if (Pckq_cell == null)
            {
                throw new Exception("找不到PCKQ欄位");
            }
            PaperQty_cell = ExcelUtil.FindCell("PAPER_QTY", sheet);
            if (PaperQty_cell == null)
            {
                throw new Exception("找不到PAPER_QTY欄位");
            }
            PiecesQty_cell = ExcelUtil.FindCell("PIECES_QTY", sheet);
            if (PiecesQty_cell == null)
            {
                throw new Exception("找不到PIECES_QTY欄位");
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

            for (int j = OrganizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    //var id = Int64.Parse(ExcelUtil.GetCellString(j, OrganizationId_cell.ColumnIndex, sheet).Trim());
                    //var org = YszmpckqTRepositiory.Get(x => x.InventoryItemId == id);
                    //if (org == null || org.InventoryItemId <= 0)
                    //{
                    YSZMPCKQ_T ySZMPCKQ_T = new YSZMPCKQ_T();
                    ySZMPCKQ_T.OrganizationId = Int64.Parse(ExcelUtil.GetCellString(j, OrganizationId_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.OrganizationCode = ExcelUtil.GetCellString(j, OrganizationCode_cell.ColumnIndex, sheet).Trim();
                    ySZMPCKQ_T.OspSubinventory = ExcelUtil.GetCellString(j, OspSubinventory_cell.ColumnIndex, sheet).Trim();
                    ySZMPCKQ_T.Pstyp = ExcelUtil.GetCellString(j, Pstyp_cell.ColumnIndex, sheet).Trim();
                    ySZMPCKQ_T.Bwetup = ExcelUtil.GetCellString(j, Bwetup_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Decimal.Parse(ExcelUtil.GetCellString(j, Bwetup_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.Bwetdn = ExcelUtil.GetCellString(j, Bwetdn_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Decimal.Parse(ExcelUtil.GetCellString(j, Bwetdn_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.Rwtup = ExcelUtil.GetCellString(j, Rwtup_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Decimal.Parse(ExcelUtil.GetCellString(j, Rwtup_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.Rwtdn = ExcelUtil.GetCellString(j, Rwtdn_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Decimal.Parse(ExcelUtil.GetCellString(j, Rwtdn_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.Pckq = ExcelUtil.GetCellString(j, Pckq_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Int64.Parse(ExcelUtil.GetCellString(j, Pckq_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.PaperQty = ExcelUtil.GetCellString(j, PaperQty_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Int64.Parse(ExcelUtil.GetCellString(j, PaperQty_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.PiecesQty = ExcelUtil.GetCellString(j, PiecesQty_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Int64.Parse(ExcelUtil.GetCellString(j, PiecesQty_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.CreatedBy = ExcelUtil.GetCellString(j, CreatedBy_cell.ColumnIndex, sheet).Trim() == "" ? 0 : Int64.Parse(ExcelUtil.GetCellString(j, CreatedBy_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.CreationDate = DateTime.Parse(ExcelUtil.GetCellString(j, CreationDate_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.LastUpdateBy = Int64.Parse(ExcelUtil.GetCellString(j, LastUpdateBy_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.LastUpdateDate = DateTime.Parse(ExcelUtil.GetCellString(j, LastUpdateDate_cell.ColumnIndex, sheet).Trim());
                    ySZMPCKQ_T.ControlFlag = "";
                    yszmpckqTRepositiory.Create(ySZMPCKQ_T);
                    //}
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImprotMachinePaperType(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCPO_MACHINE_PAPER_TYPE_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell OrganizationId_cell = null;
            ICell OrganizationCode_cell = null;
            ICell MachineCode_cell = null;
            ICell MachineMeaning_cell = null;
            ICell Description_cell = null;
            ICell PaperType_cell = null;
            ICell MachineNum_cell = null;
            ICell SupplierNum_cell = null;
            ICell SupplierName_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            OrganizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (OrganizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }
            OrganizationCode_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (OrganizationCode_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

            MachineCode_cell = ExcelUtil.FindCell("MACHINE_CODE", sheet);
            if (MachineCode_cell == null)
            {
                throw new Exception("找不到MACHINE_CODE欄位");
            }
            MachineMeaning_cell = ExcelUtil.FindCell("MACHINE_MEANING", sheet);
            if (MachineMeaning_cell == null)
            {
                throw new Exception("找不到MACHINE_MEANING欄位");
            }
            Description_cell = ExcelUtil.FindCell("DESCRIPTION", sheet);
            if (Description_cell == null)
            {
                throw new Exception("找不到DESCRIPTION欄位");
            }
            PaperType_cell = ExcelUtil.FindCell("PAPER_TYPE", sheet);
            if (PaperType_cell == null)
            {
                throw new Exception("找不到PAPER_TYPE欄位");
            }
            MachineNum_cell = ExcelUtil.FindCell("MACHINE_NUM", sheet);
            if (MachineNum_cell == null)
            {
                throw new Exception("找不到MACHINE_NUM欄位");
            }
            SupplierNum_cell = ExcelUtil.FindCell("SUPPLIER_NUM", sheet);
            if (SupplierNum_cell == null)
            {
                throw new Exception("找不到SUPPLIER_NUM欄位");
            }
            SupplierName_cell = ExcelUtil.FindCell("VENDOR_NAME", sheet);
            if (SupplierName_cell == null)
            {
                throw new Exception("找不到VENDOR_NAME欄位");
            }
            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATED_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATION_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATED_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }

            for (int j = OrganizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var MachineCode = ExcelUtil.GetCellString(j, MachineCode_cell.ColumnIndex, sheet).Trim();
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<MACHINE_PAPER_TYPE_T>().Where(x => x.Entity.MachineCode == MachineCode).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = machinePaperTypeRepositiory.Get(x => x.MachineCode == MachineCode).FirstOrDefault();
                    if (org == null || string.IsNullOrEmpty(org.Entity.MachineCode))
                    {
                        MACHINE_PAPER_TYPE_T mACHINE_PAPER_TYPE_T = new MACHINE_PAPER_TYPE_T();
                        mACHINE_PAPER_TYPE_T.OrganizationId = Int64.Parse(ExcelUtil.GetCellString(j, OrganizationId_cell.ColumnIndex, sheet).Trim());
                        mACHINE_PAPER_TYPE_T.OrganizationCode = ExcelUtil.GetCellString(j, OrganizationCode_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.MachineCode = ExcelUtil.GetCellString(j, MachineCode_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.MachineMeaning = ExcelUtil.GetCellString(j, MachineMeaning_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.Description = ExcelUtil.GetCellString(j, Description_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.PaperType = ExcelUtil.GetCellString(j, PaperType_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.MachineNum = ExcelUtil.GetCellString(j, MachineNum_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.SupplierNum = ExcelUtil.GetCellString(j, SupplierNum_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.SupplierName = ExcelUtil.GetCellString(j, SupplierName_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.CreatedBy = Int64.Parse(ExcelUtil.GetCellString(j, CreatedBy_cell.ColumnIndex, sheet).Trim());
                        mACHINE_PAPER_TYPE_T.CreationDate = DateTime.Parse(ExcelUtil.GetCellString(j, CreationDate_cell.ColumnIndex, sheet).Trim());
                        mACHINE_PAPER_TYPE_T.LastUpdateBy = Int64.Parse(ExcelUtil.GetCellString(j, LastUpdateBy_cell.ColumnIndex, sheet).Trim());
                        mACHINE_PAPER_TYPE_T.LastUpdateDate = DateTime.Parse(ExcelUtil.GetCellString(j, LastUpdateDate_cell.ColumnIndex, sheet).Trim());
                        mACHINE_PAPER_TYPE_T.ControlFlag = "";
                        machinePaperTypeRepositiory.Create(mACHINE_PAPER_TYPE_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImprotTransaction(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_TRANSACTION_TYPE_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell TransactionTypeId_cell = null;
            ICell TransactionTypeName_cell = null;
            ICell Description_cell = null;
            ICell TransactionActionId_cell = null;
            ICell TransactionActionName_cell = null;
            ICell TransactionSourceTypeId_cell = null;
            ICell TransactionSourceTypeName_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            TransactionTypeId_cell = ExcelUtil.FindCell("TRANSACTION_TYPE_ID", sheet);
            if (TransactionTypeId_cell == null)
            {
                throw new Exception("找不到TRANSACTION_TYPE_ID欄位");
            }
            TransactionTypeName_cell = ExcelUtil.FindCell("TRANSACTION_TYPE_NAME", sheet);
            if (TransactionTypeName_cell == null)
            {
                throw new Exception("找不到TRANSACTION_TYPE_NAME欄位");
            }
            Description_cell = ExcelUtil.FindCell("DESCRIPTION", sheet);
            if (Description_cell == null)
            {
                throw new Exception("找不到DESCRIPTION欄位");
            }
            TransactionActionId_cell = ExcelUtil.FindCell("TRANSACTION_ACTION_ID", sheet);
            if (TransactionActionId_cell == null)
            {
                throw new Exception("找不到TRANSACTION_ACTION_ID欄位");
            }
            TransactionActionName_cell = ExcelUtil.FindCell("TRANSACTION_ACTION_NAME", sheet);
            if (TransactionActionName_cell == null)
            {
                throw new Exception("找不到TRANSACTION_ACTION_NAME欄位");
            }

            TransactionSourceTypeId_cell = ExcelUtil.FindCell("TRANSACTION_SOURCE_TYPE_ID", sheet);
            if (TransactionSourceTypeId_cell == null)
            {
                throw new Exception("找不到TRANSACTION_SOURCE_TYPE_ID欄位");
            }
            TransactionSourceTypeName_cell = ExcelUtil.FindCell("TRANSACTION_SOURCE_TYPE_NAME", sheet);
            if (TransactionSourceTypeName_cell == null)
            {
                throw new Exception("找不到TRANSACTION_SOURCE_TYPE_NAME欄位");
            }
            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATED_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATION_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATED_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }

            
            for (int j = TransactionTypeId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var TransactionTypeId = Int64.Parse(ExcelUtil.GetCellString(j, TransactionTypeId_cell.ColumnIndex, sheet).Trim());
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<TRANSACTION_TYPE_T>().Where(x => x.Entity.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = transactionTypeRepositiory.Get(x => x.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    if (org == null || org.Entity.TransactionTypeId <= 0)
                    {
                        TRANSACTION_TYPE_T tRANSACTION_TYPE_T = new TRANSACTION_TYPE_T();
                        tRANSACTION_TYPE_T.TransactionTypeId = Int64.Parse(ExcelUtil.GetCellString(j, TransactionTypeId_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.TransactionTypeName = ExcelUtil.GetCellString(j, TransactionTypeName_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.Description = ExcelUtil.GetCellString(j, Description_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.TransactionActionId = Int64.Parse(ExcelUtil.GetCellString(j, TransactionActionId_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.TransactionActionName = ExcelUtil.GetCellString(j, TransactionActionName_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.TransactionSourceTypeId = Int64.Parse(ExcelUtil.GetCellString(j, TransactionSourceTypeId_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.TransactionSourceTypeName = ExcelUtil.GetCellString(j, TransactionSourceTypeName_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.CreatedBy = Int64.Parse(ExcelUtil.GetCellString(j, CreatedBy_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.CreationDate = DateTime.Parse(ExcelUtil.GetCellString(j, CreationDate_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.LastUpdateBy = Int64.Parse(ExcelUtil.GetCellString(j, LastUpdateBy_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.LastUpdateDate = DateTime.Parse(ExcelUtil.GetCellString(j, LastUpdateDate_cell.ColumnIndex, sheet).Trim());
                        tRANSACTION_TYPE_T.ControlFlag = "";
                        transactionTypeRepositiory.Create(tRANSACTION_TYPE_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }


        private ISheet FindSheet(IWorkbook book ,string name)
        {
            ISheet sheet = null;

            if (book.NumberOfSheets == 0)
            {
                return sheet;
            }

            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                //獲取工作表(GetSheetAt)
                if (!book.GetSheetAt(i).SheetName.Contains(name))
                {
                    continue;
                }
                sheet = book.GetSheetAt(i);
            }
            return sheet;
        }

        private string getCellString(ICell cell)
        {
            return getCellString(cell, cell.CellType);
        }

        private string getCellString(ICell cell, CellType type)
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

        private int ConvertInt32(string value)
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

        /// <summary>
        /// 產生下拉選單內容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> createDropDownList(DropDownListType type)
        {
            var organizationList = new List<SelectListItem>();
            switch (type)
            {
                case DropDownListType.All:
                    organizationList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                    break;
                case DropDownListType.Choice:
                    organizationList.Add(new SelectListItem() { Text = "請選擇", Value = "請選擇" });
                    break;
                default://無Header
                    break;
            }
            return organizationList;
        }

        /// <summary>
        /// 取得組織下拉式選單內容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetOrganizationDropDownList(DropDownListType type)
        {
            var organizationList = createDropDownList(type);
            organizationList.AddRange(getOrganizationList());
            return organizationList;
        }

        /// <summary>
        /// 取得倉庫下拉式選單內容
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventoryDropDownList(string ORGANIZATION_ID, DropDownListType type)
        {
            var subinventoryList = createDropDownList(type);
            subinventoryList.AddRange(getSubinventoryList(ORGANIZATION_ID));
            return subinventoryList;
        }

        /// <summary>
        /// 取得儲位下拉式選單內容
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocatorDropDownList(string ORGANIZATION_ID, string SUBINVENTORY_CODE, DropDownListType type)
        {
            var locatorList = createDropDownList(type);
            locatorList.AddRange(getLocatorList(ORGANIZATION_ID, SUBINVENTORY_CODE));
            return locatorList;
        }

        /// <summary>
        /// 取得組織SelectListItem
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> getOrganizationList()
        {
            var organizationList = new List<SelectListItem>();
            try
            {
                var tempList = organizationRepositiory
                            .GetAll().AsNoTracking()
                            .OrderBy(x => x.OrganizationCode)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.OrganizationCode,
                                Value = x.OrganizationId.ToString()
                            });
                organizationList.AddRange(tempList);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            

            //organizationList.AddRange(tempList);
            return organizationList;
        }

        /// <summary>
        /// 取得倉庫SelectListItem
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <returns></returns>
        private List<SelectListItem> getSubinventoryList(string ORGANIZATION_ID)
        {
            long organizationId = 0;
            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            var subinventoryList = new List<SelectListItem>();
            if (ORGANIZATION_ID == "*")
            {
                var tempList = subinventoryRepositiory
                           .GetAll().AsNoTracking()
                           .OrderBy(x => x.SubinventoryCode)
                           .Select(x => new SelectListItem()
                           {
                               Text = x.SubinventoryCode,
                               Value = x.SubinventoryCode
                           });
                subinventoryList.AddRange(tempList);
            }
            else
            {
                var tempList = subinventoryRepositiory
                           .GetAll().AsNoTracking()
                           .Where(x => x.OrganizationId == organizationId)
                           .OrderBy(x => x.SubinventoryCode)
                           .Select(x => new SelectListItem()
                           {
                               Text = x.SubinventoryCode,
                               Value = x.SubinventoryCode
                           });
                subinventoryList.AddRange(tempList);
            }
            return subinventoryList;
        }

        /// <summary>
        /// 取得儲位SelectListItem
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <returns></returns>
        private List<SelectListItem> getLocatorList(string ORGANIZATION_ID, string SUBINVENTORY_CODE)
        {
            long organizationId = 0;

            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            var locatorList = new List<SelectListItem>();
            
            if (ORGANIZATION_ID == "*" && SUBINVENTORY_CODE == "*")
            {
                var tempList = locatorTRepositiory.GetAll().AsNoTracking()
                          .Join(subinventoryRepositiory.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId
                          })
                          .Where(x => x.LocatorType == 2)
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }
            else if (ORGANIZATION_ID != "*" && SUBINVENTORY_CODE == "*")
            {
                var tempList = locatorTRepositiory.GetAll().AsNoTracking()
                          .Join(subinventoryRepositiory.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId
                          })
                          .Where(x => x.LocatorType == 2 && x.OrganizationId == organizationId)
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }
            else if (ORGANIZATION_ID == "*" && SUBINVENTORY_CODE != "*")
            {
                var tempList = locatorTRepositiory.GetAll().AsNoTracking()
                          .Join(subinventoryRepositiory.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId
                          })
                          .Where(x => x.LocatorType == 2 && x.SubinventoryCode == SUBINVENTORY_CODE)
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }
            else
            {
                var tempList = locatorTRepositiory.GetAll().AsNoTracking()
                          .Join(subinventoryRepositiory.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId
                          })
                          .Where(x => x.LocatorType == 2 && x.OrganizationId == organizationId && x.SubinventoryCode == SUBINVENTORY_CODE)
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }

            return locatorList;
        }

        /// <summary>
        /// 下拉選單OPTION種類
        /// </summary>
        public enum DropDownListType
        {
            NoHeader = 0, //沒有額外添加第一項
            All = 1, //添加第一項為全部
            Choice = 2 //添加第一項為請選擇
        }

        /// <summary>
        /// 取得 基本資料-庫存組織 查詢結果
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <param name="LOCATOR_ID"></param>
        /// <returns></returns>
        public List<OrgSubinventoryDT> OrgSubinventorySearch(string ORGANIZATION_ID, string SUBINVENTORY_CODE, string LOCATOR_ID)
        {
            try
            {
                long orgId = 0;
                long locId = 0;

                try
                {
                    if (!string.IsNullOrEmpty(ORGANIZATION_ID) && ORGANIZATION_ID != "*")
                    {
                        orgId = Convert.ToInt64(ORGANIZATION_ID);
                    }
                }
                catch
                {
                    ORGANIZATION_ID = "*";
                }

                try
                {
                    if (!string.IsNullOrEmpty(LOCATOR_ID) && LOCATOR_ID != "*")
                    {
                        locId = Convert.ToInt64(LOCATOR_ID);
                    }
                }
                catch
                {
                    LOCATOR_ID = "*";
                }

                
                List<string> cond = new List<string>();
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                string prefixCmd = @"
select o.ORGANIZATION_ID,
o.ORGANIZATION_CODE,
o.ORGANIZATION_NAME,
s.SUBINVENTORY_CODE,
s.SUBINVENTORY_NAME,
s.OSP_FLAG,
'A',
l.LOCATOR_ID,
s.LOCATOR_TYPE,
l.LOCATOR_SEGMENTS,
l.LOCATOR_DESC,
l.SEGMENT1,
l.SEGMENT2,
l.SEGMENT3,
l.SEGMENT4
from ORGANIZATION_T o
inner join SUBINVENTORY_T s
on o.ORGANIZATION_ID = s.ORGANIZATION_ID
inner join LOCATOR_T l
on s.ORGANIZATION_ID = l.ORGANIZATION_ID and s.SUBINVENTORY_CODE = l.SUBINVENTORY_CODE";

                if (ORGANIZATION_ID != "*")
                {
                    cond.Add("o.ORGANIZATION_ID = @ORGANIZATION_ID");
                    sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", orgId.ToString()));
                    
                }
                if (SUBINVENTORY_CODE != "*")
                {
                    cond.Add("s.SUBINVENTORY_CODE = @SUBINVENTORY_CODE");
                    sqlParameterList.Add(new SqlParameter("@SUBINVENTORY_CODE", SUBINVENTORY_CODE));
                }
                if (LOCATOR_ID != "*")
                {
                    cond.Add("l.LOCATOR_ID = @LOCATOR_ID");
                    sqlParameterList.Add(new SqlParameter("@LOCATOR_ID", locId.ToString()));
                }

                string commandText = string.Format(prefixCmd + "{0}{1}", cond.Count > 0 ? " WHERE " : "", string.Join(" AND ", cond.ToArray()));

                if (sqlParameterList.Count > 0)
                {
                    return this.Context.Database.SqlQuery<OrgSubinventoryDT>(commandText, sqlParameterList.ToArray()).ToList();
                    
                }
                else
                {
                    return this.Context.Database.SqlQuery<OrgSubinventoryDT>(commandText).ToList();
                }
                
                //var tempList = this.Context.Database.SqlQuery<OrgSubinventoryDT>(cmd.ToString(),
                //    new SqlParameter("@ORGANIZATION_ID", orgId),
                //    new SqlParameter("@SUBINVENTORY_CODE", SUBINVENTORY_CODE),
                //    new SqlParameter("@LOCATOR_ID", locId)).ToList();

                //var tempList = organizationRepositiory.GetAll().AsNoTracking()
                //    .Join(subinventoryRepositiory.GetAll().AsNoTracking(),
                //    o => new { o.OrganizationId },
                //    s => new { s.OrganizationId },
                //    (o, s) => new
                //    {
                //        o.OrganizationId,
                //        o.OrganizationCode,
                //        o.OrganizationName,
                //        s.SubinventoryCode,
                //        s.SubinventoryName,
                //        s.OspFlag,
                //        s.LocatorType
                //    })
                //    .Join(locatorTRepositiory.GetAll().AsNoTracking(),
                //    os => new { os.OrganizationId, os.SubinventoryCode },
                //    l => new { l.OrganizationId, l.SubinventoryCode },
                //    (os, l) => new {
                //        OrganizationId = os.OrganizationId,
                //        OrganizationCode = os.OrganizationCode,
                //        OrganizationName = os.OrganizationName,
                //        SubinventoryCode = os.SubinventoryCode,
                //        SubinventoryName = os.SubinventoryName,
                //        OspFlag = os.OspFlag,
                //        BarcodePrefixCode = "A",
                //        LocatorId = l.LocatorId,
                //        LocatorType = os.LocatorType,
                //        LocatorSegments = l.LocatorSegments,
                //        LocatorDesc = l.LocatorDesc,
                //        Segment1 = l.Segment1,
                //        Segment2 = l.Segment2,
                //        Segment3 = l.Segment3,
                //        Segment4 = l.Segment4
                //    })
                //        .Where(x => ORGANIZATION_ID == "*" ? true : x.OrganizationId == orgId &&
                //            SUBINVENTORY_CODE == "*" ? true : x.SubinventoryCode == SUBINVENTORY_CODE &&
                //            LOCATOR_ID == "*" ? true : x.LocatorId == locId)
                //            .Select(x => new OrgSubinventoryDT{
                //                ORGANIZATION_ID = x.OrganizationId,
                //                ORGANIZATION_CODE = x.OrganizationCode,
                //                ORGANIZATION_NAME = x.OrganizationName,
                //                SUBINVENTORY_CODE = x.SubinventoryCode,
                //                SUBINVENTORY_NAME = x.SubinventoryName,
                //                OSP_FLAG = x.OspFlag,
                //                BARCODE_PREFIX_CODE = x.BarcodePrefixCode,
                //                LOCATOR_ID = x.LocatorId,
                //                LOCATOR_TYPE = x.LocatorType,
                //                LOCATOR_SEGMENTS = x.LocatorSegments,
                //                LOCATOR_DESC = x.LocatorDesc,
                //                SEGMENT1 = x.Segment1,
                //                SEGMENT2 = x.Segment2,
                //                SEGMENT3 = x.Segment3,
                //                SEGMENT4 = x.Segment4
                //            });

                //return tempList;
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<OrgSubinventoryDT>();
            }
            //return result;
        }
    }
}