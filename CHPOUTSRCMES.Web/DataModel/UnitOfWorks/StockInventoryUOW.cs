using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Inventory;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using NLog;


namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class StockInventoryUOW : TransferUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_INVENTORY_HEADER_T> trfInventoryHeaderTRepository;
        private readonly IRepository<TRF_INVENTORY_T> trfInventoryTRepository;
        private readonly IRepository<TRF_INVENTORY_HT> trfInventoryHtRepository;

        public StockInventoryUOW(DbContext context)
        : base(context)
        {
            this.trfInventoryHeaderTRepository = new GenericRepository<TRF_INVENTORY_HEADER_T>(this);
            this.trfInventoryTRepository = new GenericRepository<TRF_INVENTORY_T>(this);
            this.trfInventoryHtRepository = new GenericRepository<TRF_INVENTORY_HT>(this);
        }

        InventoryType inventoryType = new InventoryType();
        /// <summary>
        /// 盤點異動類別
        /// </summary>
        public class InventoryType
        {
            /// <summary>
            /// 盤盈
            /// </summary>
            public const long profit = TransactionTypeId.Chp16In;
            /// <summary>
            /// 盤虧
            /// </summary>
            public const long loss = TransactionTypeId.Chp16Out;

            public string GetDesc(long type)
            {
                switch (type)
                {
                    case profit:
                        return "盤盈";
                    case loss:
                        return "盤虧";
                    default:
                        return "";
                }
            }
        }

        #region 盤點類別下拉選單

        /// <summary>
        /// 取得盤點類別下拉選單
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetInventoryTypeDropDownList()
        {
            var transferTypeList = createDropDownList(DropDownListType.Choice);
            transferTypeList.AddRange(getInventoryTypeList());
            return transferTypeList;
        }

        private List<SelectListItem> getInventoryTypeList()
        {
            var inventoryTypeList = new List<SelectListItem>();
            try
            {
                inventoryTypeList.Add(new SelectListItem() { Text = inventoryType.GetDesc(InventoryType.loss), Value = InventoryType.loss.ToString() });
                inventoryTypeList.Add(new SelectListItem() { Text = inventoryType.GetDesc(InventoryType.profit), Value = InventoryType.profit.ToString() });

                var a = inventoryTypeList.Select(x => new SelectListItem() { Text = x.Text, Value = x.Value }).Where(x => x.Value == "1");
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return inventoryTypeList;
        }

        #endregion

        public List<StockDT> GetStockTList(long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            try
            {
                if (locatorId == null)
                {
                    string cmd = @"
SELECT [STOCK_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY [STOCK_ID]) AS SUB_ID
      ,s.[ORGANIZATION_ID] AS ORGANIZATION_ID
	  ,'' AS SEGMENT3
      ,s.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
      ,[ITEM_NUMBER] AS ITEM_NO
      ,[BARCODE] AS BARCODE
      ,[PRIMARY_UOM_CODE] AS PRIMARY_UOM_CODE
      ,[PRIMARY_AVAILABLE_QTY] AS PRIMARY_AVAILABLE_QTY
      ,[SECONDARY_UOM_CODE] AS SECONDARY_UOM_CODE
      ,ISNULL([SECONDARY_AVAILABLE_QTY], 0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE,
      ,[ITEM_CATEGORY] AS ITEM_CATEGORY
  FROM [STOCK_T] s
  WHERE s.ORGANIZATION_ID = @organizationId 
  AND s.SUBINVENTORY_CODE = @subinventoryCode
  AND s.ITEM_NUMBER = @itemNumber
";

                    var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventoryCode);
                    var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                    return this.Context.Database.SqlQuery<StockDT>(cmd, pOrg, pSub, pItemNo).ToList();
                }
                else
                {
                    string cmd = @"
SELECT [STOCK_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY [STOCK_ID]) AS SUB_ID
      ,s.[ORGANIZATION_ID] AS ORGANIZATION_ID
	  ,l.SEGMENT3 AS SEGMENT3
      ,s.[SUBINVENTORY_CODE]  AS SUBINVENTORY_CODE
      ,[ITEM_NUMBER] AS ITEM_NO
      ,[BARCODE] AS BARCODE
      ,[PRIMARY_UOM_CODE] AS PRIMARY_UOM_CODE
      ,[PRIMARY_AVAILABLE_QTY] AS PRIMARY_AVAILABLE_QTY
      ,[SECONDARY_UOM_CODE] AS SECONDARY_UOM_CODE
      ,ISNULL([SECONDARY_AVAILABLE_QTY],0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE
      ,[ITEM_CATEGORY] AS ITEM_CATEGORY
  FROM [STOCK_T] s
  LEFT JOIN [LOCATOR_T] l on s.ORGANIZATION_ID = l.ORGANIZATION_ID 
  AND s.SUBINVENTORY_CODE = l.SUBINVENTORY_CODE 
  AND s.LOCATOR_ID = l.LOCATOR_ID
  WHERE s.ORGANIZATION_ID = @organizationId 
  AND s.SUBINVENTORY_CODE = @subinventoryCode 
  AND s.LOCATOR_ID = @locatorId 
  AND s.ITEM_NUMBER = @itemNumber
";

                    var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventoryCode);
                    var pLoc = SqlParamHelper.GetBigInt("@locatorId", (long)locatorId);
                    var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                    return this.Context.Database.SqlQuery<StockDT>(cmd, pOrg, pSub, pLoc, pItemNo).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockDT>();
            }
        }

        public ResultModel CreateDetail(long transactionTypeId, long stockId, decimal mQty, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == stockId);
                    if (stock == null) throw new Exception("找不到庫存資料");

                    var organizationId = stock.OrganizationId;
                    var subinventoryCode = stock.SubinventoryCode;
                    long? locatorId = stock.LocatorId;

                    var header = trfInventoryHeaderTRepository.GetAll().FirstOrDefault(x => x.TransactionTypeId == transactionTypeId &&
                    x.OrganizationId == organizationId && x.SubinventoryCode == subinventoryCode && x.LocatorId == locatorId && x.NumberStatus == NumberStatus.NotSaved);

                    if (header == null)
                    {
                        //產生header資料
                        var organization = GetOrganization(organizationId);
                        if (organization == null) throw new Exception("找不到庫存組織資料");

                        var transactionType = GetTransactionType(transactionTypeId);
                        if (transactionType == null) throw new Exception("找不到庫存交易類別資料");

                        string locatorCode = null;
                        string segment3 = null;
                        if (locatorId != null)
                        {
                            var outLocator = GetLocatorForTransfer((long)locatorId, now);
                            if (outLocator == null) throw new Exception("找不到庫存儲位資料");
                            locatorCode = outLocator.LocatorSegments;
                            segment3 = outLocator.Segment3;
                        }

                        header = new TRF_INVENTORY_HEADER_T()
                        {
                            OrgId = organization.OrgUnitId,
                            OrganizationId = organizationId,
                            OrganizationCode = organization.OrganizationCode,
                            ShipmentNumber = GetShipmentNumberGuid(),
                            SubinventoryCode = subinventoryCode,
                            LocatorId = locatorId,
                            LocatorCode = locatorCode,
                            Segment3 = segment3,
                            NumberStatus = NumberStatus.NotSaved,
                            TransactionDate = now,
                            TransactionTypeId = transactionTypeId,
                            TransactionTypeName = transactionType.TransactionTypeName,
                            TransferOrgId = null,
                            TransferOrganizationId = null,
                            TransferOrganizationCode = null,
                            TransferSubinventoryCode = null,
                            TransferLocatorId = null,
                            TransferLocatorCode = null,
                            CreatedBy = userId,
                            CreatedUserName = userName,
                            CreationDate = now,
                            LastUpdateBy = null,
                            LastUpdateUserName = null,
                            LastUpdateDate = null
                        };

                        trfInventoryHeaderTRepository.Create(header, true);
                    }

                    var detail = trfInventoryTRepository.GetAll().FirstOrDefault(x =>
                    x.TransferInventoryHeaderId == header.TransferInventoryHeaderId &&
                    x.StockId == stockId);
                    if (detail != null) return new ResultModel(false, "已存在此條碼:" + detail.Barcode + "異動紀錄");



                    //處理異動量
                    mQty = Math.Abs(mQty); //轉為正數
                    if (transactionTypeId == TransactionTypeId.Chp16In)
                    {
                        //盤盈為正數不用處理
                    }
                    else if (transactionTypeId == TransactionTypeId.Chp16Out)
                    {
                        mQty = -1 * mQty;
                    }
                    else
                    {
                        throw new Exception("異動型態Id錯誤");
                    }

                    if (stock.ItemCategory == ItemCategory.Flat)
                    {
                        decimal aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : (decimal)stock.SecondaryAvailableQty) + mQty;
                        if (aftSecQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                    }
                    else if (stock.ItemCategory == ItemCategory.Roll)
                    {
                        decimal aftPryQty = stock.PrimaryAvailableQty + mQty;
                        if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

                        ////計算異動後的數量
                        //decimal aftPryQty = 0; //主單位異動後數量
                        //decimal? aftSecQty = null; //次單位異動後數量
                        //decimal mPrimaryQty = 0; //主單位異動量
                        //decimal? mSecondaryQty = null; //次單位異動量
                        //if (stock.ItemCategory == ItemCategory.Flat)
                        //{
                        //    mSecondaryQty = mQty;
                        //    aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mQty;
                        //    if (aftSecQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                        //    var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode); //主單位數量轉次單位數量
                        //    if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                        //    aftPryQty = uomConversionResult.Data;

                        //    //轉換主單位異動量
                        //    var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                        //    if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                        //    mPrimaryQty = uomConversionResult2.Data;
                        //}
                        //else if (stock.ItemCategory == ItemCategory.Roll)
                        //{
                        //    mPrimaryQty = mQty;
                        //    aftPryQty = stock.PrimaryAvailableQty + mQty;
                        //    if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                        //    aftSecQty = null;
                        //    mSecondaryQty = null;
                        //}
                        //else
                        //{
                        //    throw new Exception("無法識別貨品類別");
                        //}

                        //產生盤點明細
                        detail = new TRF_INVENTORY_T()
                    {
                        TransferInventoryHeaderId = header.TransferInventoryHeaderId,
                        InventoryItemId = stock.InventoryItemId,
                        ItemNumber = stock.ItemNumber,
                        ItemDescription = stock.ItemDescription,
                        Barcode = stock.Barcode,
                        StockId = stockId,
                        PrimaryUom = stock.PrimaryUomCode,
                        TransferPrimaryQuantity = stock.ItemCategory == ItemCategory.Roll ? mQty : 0,
                        OriginalPrimaryQuantity = 0,
                        AfterPrimaryQuantity = 0,
                        SecondaryUom = stock.SecondaryUomCode,
                        TransferSecondaryQuantity = stock.ItemCategory == ItemCategory.Flat ? mQty : default(decimal?),
                        OriginalSecondaryQuantity = null,
                        AfterSecondaryQuantity = null,
                        LotNumber = stock.LotNumber,
                        LotQuantity = stock.LotQuantity,
                        Note = null,
                        CreatedBy = userId,
                        CreatedUserName = userName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    };
                    trfInventoryTRepository.Create(detail, true);

                    txn.Commit();
                    return new ResultModel(true, "新增明細成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "新增明細失敗:" + ex.Message);
                }

            }
        }

        public List<StockInventoryDT> GetStockInventoryTList(string userId, long transactionTypeId, bool fromHistoryData)
        {
            try
            {
                string cmd = "";
                if (fromHistoryData)
                {
                    cmd = @"
SELECT m.TRANSFER_INVENTORY_ID AS ID
      ,m.[STOCK_ID] AS STOCK_ID
	  ,ROW_NUMBER() OVER(ORDER BY [TRANSFER_INVENTORY_ID]) AS SUB_ID
      ,h.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
	  ,h.[SEGMENT3] AS SEGMENT3
      ,m.[ITEM_NUMBER] AS ITEM_NO
      ,m.[BARCODE] AS BARCODE
      ,m.[PRIMARY_UOM] AS PRIMARY_UOM_CODE
      ,m.TRANSFER_PRIMARY_QUANTITY AS PRIMARY_TRANSACTION_QTY
	  ,m.AFTER_PRIMARY_QUANTITY AS PRIMARY_AVAILABLE_QTY
      ,m.SECONDARY_UOM AS SECONDARY_UOM_CODE
      ,ISNULL(m.TRANSFER_SECONDARY_QUANTITY,0) AS SECONDARY_TRANSACTION_QTY
	  ,ISNULL(m.AFTER_SECONDARY_QUANTITY,0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
  FROM TRF_INVENTORY_HEADER_T h
  INNER JOIN TRF_INVENTORY_HT m on h.TRANSFER_INVENTORY_HEADER_ID = m.TRANSFER_INVENTORY_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  WHERE u.UserId = @userId AND h.TRANSACTION_TYPE_ID = @transactionTypeId
";
                }
                else
                {
                    cmd = @"
SELECT m.TRANSFER_INVENTORY_ID AS ID
      ,m.[STOCK_ID] AS STOCK_ID
	  ,ROW_NUMBER() OVER(ORDER BY [TRANSFER_INVENTORY_ID]) AS SUB_ID
      ,h.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
	  ,h.[SEGMENT3] AS SEGMENT3
      ,m.[ITEM_NUMBER] AS ITEM_NO
      ,m.[BARCODE] AS BARCODE
      ,m.[PRIMARY_UOM] AS PRIMARY_UOM_CODE
      ,m.TRANSFER_PRIMARY_QUANTITY AS PRIMARY_TRANSACTION_QTY
	  ,m.AFTER_PRIMARY_QUANTITY AS PRIMARY_AVAILABLE_QTY
      ,m.SECONDARY_UOM AS SECONDARY_UOM_CODE
      ,ISNULL(m.TRANSFER_SECONDARY_QUANTITY,0) AS SECONDARY_TRANSACTION_QTY
	  ,ISNULL(m.AFTER_SECONDARY_QUANTITY,0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
  FROM TRF_INVENTORY_HEADER_T h
  INNER JOIN TRF_INVENTORY_T m on h.TRANSFER_INVENTORY_HEADER_ID = m.TRANSFER_INVENTORY_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  WHERE u.UserId = @userId AND h.TRANSACTION_TYPE_ID = @transactionTypeId
";
                }

                var pUserId = SqlParamHelper.GetNVarChar("@userId", userId);
                var pTypeId = SqlParamHelper.GetBigInt("@transactionTypeId", transactionTypeId);

                var list = this.Context.Database.SqlQuery<StockInventoryDT>(cmd, pUserId, pTypeId).ToList();

                if (list.Count == 0) return list;

                foreach(StockInventoryDT data in list)
                {
                    if (data.STOCK_ID == 0) continue; //新增條碼不必轉換數量
                    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == data.STOCK_ID);
                    if (stock == null) throw new Exception("找不到庫存資料");

                    //計算異動後的數量
                    decimal aftPryQty = 0; //主單位異動後數量
                    decimal? aftSecQty = null; //次單位異動後數量
                    decimal mPrimaryQty = 0; //主單位異動量
                    decimal? mSecondaryQty = null; //次單位異動量
                    if (stock.ItemCategory == ItemCategory.Flat)
                    {
                        mSecondaryQty = data.SECONDARY_TRANSACTION_QTY;
                        aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mSecondaryQty;
                        if (aftSecQty < 0) throw new Exception("超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                        var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                        aftPryQty = uomConversionResult.Data;

                        //轉換主單位異動量
                        var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                        if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                        mPrimaryQty = uomConversionResult2.Data;
                    }
                    else if (stock.ItemCategory == ItemCategory.Roll)
                    {
                        mPrimaryQty = data.PRIMARY_TRANSACTION_QTY;
                        aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                        if (aftPryQty < 0) throw new Exception("超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                        aftSecQty = null;
                        mSecondaryQty = null;
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }
                   
                    data.PRIMARY_TRANSACTION_QTY = mPrimaryQty;
                    data.PRIMARY_AVAILABLE_QTY = aftPryQty;
                    data.SECONDARY_TRANSACTION_QTY = mSecondaryQty == null ? 0 : (decimal)mSecondaryQty;
                    data.SECONDARY_AVAILABLE_QTY = aftSecQty == null ? 0 : (decimal)aftSecQty;  
                }
                return list;
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockInventoryDT>();
            }
        }

        /// <summary>
        /// 取得盤虧歷史紀錄
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="subinventoryCode"></param>
        /// <param name="locatorId"></param>
        /// <param name="itemNumber"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<StockInventoryDT> GetsLossStockInventoryHtList(long organizationId, string subinventoryCode, long? locatorId, string itemNumber, string userId)
        {
            try
            {
                if (locatorId == null)
                {
                    string cmd = @"
SELECT m.TRANSFER_INVENTORY_ID AS ID
      ,m.[STOCK_ID] AS STOCK_ID
	  ,ROW_NUMBER() OVER(ORDER BY [TRANSFER_INVENTORY_ID]) AS SUB_ID
      ,h.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
	  ,h.[SEGMENT3] AS SEGMENT3
      ,m.[ITEM_NUMBER] AS ITEM_NO
      ,m.[BARCODE] AS BARCODE
      ,m.[PRIMARY_UOM] AS PRIMARY_UOM_CODE
      ,m.TRANSFER_PRIMARY_QUANTITY AS PRIMARY_TRANSACTION_QTY
	  ,m.AFTER_PRIMARY_QUANTITY AS PRIMARY_AVAILABLE_QTY
      ,m.SECONDARY_UOM AS SECONDARY_UOM_CODE
      ,ISNULL(m.TRANSFER_SECONDARY_QUANTITY,0) AS SECONDARY_TRANSACTION_QTY
	  ,ISNULL(m.AFTER_SECONDARY_QUANTITY,0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,i.[CATALOG_ELEM_VAL_070] AS ITEM_CATEGORY
  FROM TRF_INVENTORY_HEADER_T h
  INNER JOIN TRF_INVENTORY_HT m on h.TRANSFER_INVENTORY_HEADER_ID = m.TRANSFER_INVENTORY_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  INNER JOIN ITEMS_T i on m.INVENTORY_ITEM_ID = i.INVENTORY_ITEM_ID
  WHERE u.UserId = @userId AND h.TRANSACTION_TYPE_ID = @transactionTypeId 
  AND h.ORGANIZATION_ID = @organizationId 
  AND h.SUBINVENTORY_CODE = @subinventoryCode 
  AND m.ITEM_NUMBER = @itemNumber
";
                    var pUserId = SqlParamHelper.GetNVarChar("@userId", userId);
                    var pTypeId = SqlParamHelper.GetBigInt("@transactionTypeId", TransactionTypeId.Chp16Out);
                    var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventoryCode);
                    var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                    return this.Context.Database.SqlQuery<StockInventoryDT>(cmd, pUserId, pTypeId, pOrg, pSub, pItemNo).ToList();
                }
                else
                {
                    string cmd = @"
SELECT m.TRANSFER_INVENTORY_ID AS ID
      ,m.[STOCK_ID] AS STOCK_ID
	  ,ROW_NUMBER() OVER(ORDER BY [TRANSFER_INVENTORY_ID]) AS SUB_ID
      ,h.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
	  ,h.[SEGMENT3] AS SEGMENT3
      ,m.[ITEM_NUMBER] AS ITEM_NO
      ,m.[BARCODE] AS BARCODE
      ,m.[PRIMARY_UOM] AS PRIMARY_UOM_CODE
      ,m.TRANSFER_PRIMARY_QUANTITY AS PRIMARY_TRANSACTION_QTY
	  ,m.AFTER_PRIMARY_QUANTITY AS PRIMARY_AVAILABLE_QTY
      ,m.SECONDARY_UOM AS SECONDARY_UOM_CODE
      ,ISNULL(m.TRANSFER_SECONDARY_QUANTITY,0) AS SECONDARY_TRANSACTION_QTY
	  ,ISNULL(m.AFTER_SECONDARY_QUANTITY,0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,i.[CATALOG_ELEM_VAL_070] AS ITEM_CATEGORY
  FROM TRF_INVENTORY_HEADER_T h
  INNER JOIN TRF_INVENTORY_HT m on h.TRANSFER_INVENTORY_HEADER_ID = m.TRANSFER_INVENTORY_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  INNER JOIN ITEMS_T i on m.INVENTORY_ITEM_ID = i.INVENTORY_ITEM_ID
  WHERE u.UserId = @userId AND h.TRANSACTION_TYPE_ID = @transactionTypeId 
  AND h.ORGANIZATION_ID = @organizationId 
  AND h.SUBINVENTORY_CODE = @subinventoryCode 
  AND h.LOCATOR_ID = @locatorId 
  AND m.ITEM_NUMBER = @itemNumber
";
                    var pUserId = SqlParamHelper.GetNVarChar("@userId", userId);
                    var pTypeId = SqlParamHelper.GetBigInt("@transactionTypeId", TransactionTypeId.Chp16Out);
                    var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventoryCode);
                    var pLoc = SqlParamHelper.GetBigInt("@locatorId", (long)locatorId);
                    var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                    return this.Context.Database.SqlQuery<StockInventoryDT>(cmd, pUserId, pTypeId, pOrg, pSub, pLoc, pItemNo).ToList();
                    //var list = this.Context.Database.SqlQuery<StockInventoryDT>(cmd, pUserId, pTypeId, pOrg, pSub, pLoc, pItemNo).ToList();

                    //if (list.Count == 0) return list;

                    ////轉換數量
                    //foreach (StockInventoryDT data in list)
                    //{
                    //    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == data.STOCK_ID);
                    //    if (stock == null) throw new Exception("找不到庫存資料");

                    //    //計算異動後的數量
                    //    decimal aftPryQty = 0; //主單位異動後數量
                    //    decimal? aftSecQty = null; //次單位異動後數量
                    //    decimal mPrimaryQty = 0; //主單位異動量
                    //    decimal? mSecondaryQty = null; //次單位異動量
                    //    if (stock.ItemCategory == ItemCategory.Flat)
                    //    {
                    //        mSecondaryQty = data.SECONDARY_TRANSACTION_QTY;
                    //        aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mSecondaryQty;
                    //        if (aftSecQty < 0) throw new Exception("超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                    //        var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                    //        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                    //        aftPryQty = uomConversionResult.Data;

                    //        //轉換主單位異動量
                    //        var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                    //        if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                    //        mPrimaryQty = uomConversionResult2.Data;
                    //    }
                    //    else if (stock.ItemCategory == ItemCategory.Roll)
                    //    {
                    //        mPrimaryQty = data.PRIMARY_TRANSACTION_QTY;
                    //        aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                    //        if (aftPryQty < 0) throw new Exception("超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                    //        aftSecQty = null;
                    //        mSecondaryQty = null;
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("無法識別貨品類別");
                    //    }

                    //    data.PRIMARY_TRANSACTION_QTY = mPrimaryQty;
                    //    data.PRIMARY_AVAILABLE_QTY = aftPryQty;
                    //    data.SECONDARY_TRANSACTION_QTY = mSecondaryQty == null ? 0 : (decimal)mSecondaryQty;
                    //    data.SECONDARY_AVAILABLE_QTY = aftSecQty == null ? 0 : (decimal)aftSecQty;
                    //}
                    //return list;

                }

            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockInventoryDT>();
            }
        }


        public ResultModel DelDetailData(List<long> ids)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var list = trfInventoryTRepository.GetAll().Where(x => ids.Contains(x.TransferInventoryId)).ToList();
                    if (list == null || list.Count == 0) return new ResultModel(false, "找不到明細資料");
                    if (list.Count != ids.Count) throw new Exception("找不到部分明細資料");

                    foreach (TRF_INVENTORY_T data in list)
                    {
                        var headerId = data.TransferInventoryHeaderId;
                        trfInventoryTRepository.Delete(data, true);
                        var detailList = trfInventoryTRepository.GetAll().FirstOrDefault(x => x.TransferInventoryHeaderId == headerId);
                        if (detailList == null)
                        {
                            var header = trfInventoryHeaderTRepository.GetAll().FirstOrDefault(x => x.TransferInventoryHeaderId == headerId);
                            if (header == null) throw new Exception("找不到檔頭資料");
                            trfInventoryHeaderTRepository.Delete(header);
                        }
                    }
                    trfInventoryHeaderTRepository.SaveChanges();

                    txn.Commit();
                    return new ResultModel(true, "刪除盤點明細成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除盤點明細失敗:" + ex.Message);
                }
            }
        }

        public ResultModel UpdateDetailNote(List<long> ids, string note, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var list = trfInventoryTRepository.GetAll().Where(x => ids.Contains(x.TransferInventoryId)).ToList();
                    if (list == null || list.Count == 0) return new ResultModel(false, "找不到明細資料");
                    if (list.Count != ids.Count) throw new Exception("找不到部分明細資料");

                    var now = DateTime.Now;
                    foreach (TRF_INVENTORY_T data in list)
                    {
                        data.Note = note;
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                        trfInventoryTRepository.Update(data);
                    }

                    trfInventoryTRepository.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "更新盤點備註成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "更新盤點備註失敗:" + ex.Message);
                }
            }
        }

        public ResultModel SaveTransactionDetail(long transactionTypeId, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var headerIdList = trfInventoryHeaderTRepository.GetAll().AsNoTracking().Join(
                        trfInventoryTRepository.GetAll().AsNoTracking(),
                        h => new { h.TransferInventoryHeaderId },
                        d => new { d.TransferInventoryHeaderId },
                        (h, d) => new
                        {
                            HeaderId = h.TransferInventoryHeaderId,
                            TransactionTypeId = h.TransactionTypeId,
                            OrganizationId = h.OrganizationId,
                            SubinventoryCode = h.SubinventoryCode
                        })
                        .Join(
                userSubinventoryTRepository.GetAll().AsNoTracking(),
                x => new { x.OrganizationId, x.SubinventoryCode },
                u => new { u.OrganizationId, u.SubinventoryCode },
                (h, u) => new
                {
                    HeaderId = h.HeaderId,
                    TransactionTypeId = h.TransactionTypeId,
                    UserId = u.UserId
                })
                .Where(x => x.UserId == userId && x.TransactionTypeId == transactionTypeId)
                .GroupBy(x => new { x.HeaderId })
                .Select(x => x.Key.HeaderId).ToList();

                    if (headerIdList == null || headerIdList.Count == 0) return new ResultModel(false, "沒有可存檔的資料");

                    foreach (long headerId in headerIdList)
                    {
                        var header = trfInventoryHeaderTRepository.GetAll().FirstOrDefault(x => x.TransferInventoryHeaderId == headerId);
                        if (header == null) throw new Exception("找不到檔頭資料");
                        header.NumberStatus = NumberStatus.Saved;
                        header.TransactionDate = now;
                        header.LastUpdateBy = userId;
                        header.LastUpdateDate = now;
                        header.LastUpdateUserName = userName;
                        trfInventoryHeaderTRepository.Update(header);

                        var detailList = trfInventoryTRepository.GetAll().Where(x => x.TransferInventoryHeaderId == header.TransferInventoryHeaderId).ToList();
                        if (detailList == null || detailList.Count == 0) throw new Exception("找不到明細資料");

                        foreach (var detail in detailList)
                        {
                            if (detail.StockId == 0)
                            {
                                //產生新庫存
                                var item = GetItemNumber(detail.ItemNumber);
                                if (item == null) throw new Exception("找不到料號資料");

                                //取得每件令數
                                decimal rollReamWt = 0;
                                if (item.CatalogElemVal070 == ItemCategory.Flat)
                                {
                                    //rollReamWt = Math.Ceiling(requestedQty / rollReamQty); //每件令數 = 總令數 除以 棧板數 無條件進位 待確認是否正確
                                    //rollReamWt = 0; //測試完 待改成下方正確的
                                    var yszmpckq = GetYszmpckq(header.OrganizationId, header.OrganizationCode, header.SubinventoryCode, item.CatalogElemVal020, item.CatalogElemVal040);
                                    if (yszmpckq == null) throw new Exception("找不到令重包數資料");
                                    rollReamWt = yszmpckq.PiecesQty;
                                }
                                else if (item.CatalogElemVal070 == ItemCategory.Roll)
                                {
                                    rollReamWt = 0;
                                }
                                else
                                {
                                    throw new Exception("無法識別貨品類別");
                                }

                                STOCK_T stock = new STOCK_T();
                                stock.OrganizationId = header.OrganizationId;
                                stock.OrganizationCode = header.OrganizationCode;
                                stock.SubinventoryCode = header.SubinventoryCode;
                                stock.LocatorId = header.LocatorId == 0 ? null : header.LocatorId;
                                stock.LocatorSegments = header.LocatorCode;
                                stock.InventoryItemId = detail.InventoryItemId;
                                stock.ItemNumber = detail.ItemNumber;
                                stock.ItemDescription = detail.ItemDescription;
                                stock.ItemCategory = item.CatalogElemVal070;
                                stock.PaperType = item.CatalogElemVal020;
                                stock.BasicWeight = item.CatalogElemVal040;
                                stock.ReamWeight = item.CatalogElemVal060;
                                stock.RollReamWt = rollReamWt;
                                stock.Specification = item.CatalogElemVal050;
                                stock.PackingType = item.CatalogElemVal110;
                                stock.OspBatchNo = null;
                                stock.LotNumber = detail.LotNumber;
                                stock.LotQuantity = detail.LotQuantity;
                                stock.Barcode = detail.Barcode;
                                stock.PrimaryUomCode = item.PrimaryUomCode;
                                stock.PrimaryTransactionQty = detail.TransferPrimaryQuantity;
                                stock.PrimaryAvailableQty = detail.TransferPrimaryQuantity;
                                stock.PrimaryLockedQty = null;
                                stock.SecondaryUomCode = item.SecondaryUomCode;
                                stock.SecondaryTransactionQty = detail.TransferSecondaryQuantity;
                                stock.SecondaryAvailableQty = detail.TransferSecondaryQuantity;
                                stock.SecondaryLockedQty = null;
                                stock.ReasonCode = null;
                                stock.ReasonDesc = null;
                                stock.Note = detail.Note;
                                stock.StatusCode = StockStatusCode.InStock;
                                stock.CreatedBy = userId;
                                stock.CreationDate = now;
                                stock.LastUpdateBy = null;
                                stock.LastUpdateDate = null;
                                stockTRepository.Create(stock, true);
                                
                                //產生異動紀錄
                                var stkTxnT = CreateStockRecord(stock, null, null, null,
                                null, CategoryCode.InventoryProfit, ActionCode.StockTransfer, header.ShipmentNumber,
                                0, stock.PrimaryAvailableQty, stock.PrimaryAvailableQty, 0, stock.SecondaryAvailableQty, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                                stkTxnTRepository.Create(stkTxnT);
                            }
                            else
                            {
                                var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == detail.StockId);
                                if (stock == null) throw new Exception("找不到庫存資料");

                                //計算異動後的數量
                                decimal aftPryQty = 0; //主單位異動後數量
                                decimal? aftSecQty = null; //次單位異動後數量
                                decimal mPrimaryQty = 0; //主單位異動量
                                decimal? mSecondaryQty = null; //次單位異動量
                                var stockStatusCode = ""; //庫存狀態
                                if (stock.ItemCategory == ItemCategory.Flat)
                                {
                                    mSecondaryQty = detail.TransferSecondaryQuantity;
                                    aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mSecondaryQty;
                                    if (aftSecQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                                    var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                                    if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                                    aftPryQty = uomConversionResult.Data;

                                    //轉換主單位異動量
                                    var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                                    if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                                    mPrimaryQty = uomConversionResult2.Data;

                                    if (aftSecQty == 0)
                                    {
                                        stockStatusCode = StockStatusCode.TransferNoneInStock;
                                    }
                                    else
                                    {
                                        stockStatusCode = StockStatusCode.InStock;
                                    }
                                }
                                else if (stock.ItemCategory == ItemCategory.Roll)
                                {
                                    mPrimaryQty = detail.TransferPrimaryQuantity;
                                    aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                                    if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                                    aftSecQty = null;
                                    mSecondaryQty = null;

                                    if (aftPryQty == 0)
                                    {
                                        stockStatusCode = StockStatusCode.TransferNoneInStock;
                                    }
                                    else
                                    {
                                        stockStatusCode = StockStatusCode.InStock;
                                    }
                                }
                                else
                                {
                                    throw new Exception("無法識別貨品類別");
                                }

                                //更新明細
                                detail.TransferPrimaryQuantity = mPrimaryQty;
                                detail.OriginalPrimaryQuantity = stock.PrimaryAvailableQty;
                                detail.AfterPrimaryQuantity = aftPryQty;
                                detail.TransferSecondaryQuantity = mSecondaryQty;
                                detail.OriginalSecondaryQuantity = stock.SecondaryAvailableQty;
                                detail.AfterSecondaryQuantity = aftSecQty;
                                detail.LastUpdateBy = userId;
                                detail.LastUpdateUserName = userName;
                                detail.LastUpdateDate = now;
                                trfInventoryTRepository.Update(detail, true);

                                //更新庫存
                                stock.PrimaryAvailableQty = aftPryQty;
                                stock.SecondaryAvailableQty = aftSecQty;
                                if (string.IsNullOrEmpty(stock.Note))
                                {
                                    stock.Note = detail.Note;
                                }
                                else
                                {
                                    stock.Note = stock.Note + "," + detail.Note;
                                }
                                stock.LastUpdateBy = userId;
                                stock.LastUpdateDate = now;
                                stock.StatusCode = stockStatusCode;
                                stockTRepository.Update(stock);

                                //產生異動紀錄
                                if (header.TransactionTypeId == TransactionTypeId.Chp16In)
                                {
                                    var stkTxnT = CreateStockRecord(stock, null, null, null,
                                null, CategoryCode.InventoryProfit, ActionCode.StockTransfer, header.ShipmentNumber,
                                detail.OriginalPrimaryQuantity, mPrimaryQty, aftPryQty, detail.OriginalSecondaryQuantity,
                                mSecondaryQty, aftSecQty, stockStatusCode, userId, now);
                                    stkTxnTRepository.Create(stkTxnT);
                                }
                                else if (header.TransactionTypeId == TransactionTypeId.Chp16Out)
                                {
                                    var stkTxnT = CreateStockRecord(stock, null, null, null,
                               null, CategoryCode.InventoryLoss, ActionCode.StockTransfer, header.ShipmentNumber,
                               detail.OriginalPrimaryQuantity, mPrimaryQty, aftPryQty, detail.OriginalSecondaryQuantity,
                               mSecondaryQty, aftSecQty, stockStatusCode, userId, now);
                                    stkTxnTRepository.Create(stkTxnT);
                                }
                                else
                                {
                                    throw new Exception("異動型態Id錯誤");
                                }
                                ////更新其它尚未儲存的明細數量
                                //if (header.TransactionTypeId == TransactionTypeId.Chp16In)
                                //{
                                //    var tempDetail = trfInventoryTRepository.GetAll().Join(
                                //        trfInventoryHeaderTRepository.GetAll().Where(x => x.TransactionTypeId == TransactionTypeId.Chp16Out && x.NumberStatus == NumberStatus.NotSaved),
                                //        d => new { d.TransferInventoryHeaderId },
                                //        h => new { h.TransferInventoryHeaderId },
                                //        (d,h) => d)
                                //        .FirstOrDefault(x => x.StockId == stock.StockId);
                                //    if (tempDetail == null) continue;

                                //    if (stock.ItemCategory == ItemCategory.Flat)
                                //    {
                                //        mSecondaryQty = tempDetail.TransferSecondaryQuantity;
                                //        aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mSecondaryQty;
                                //        if (aftSecQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                                //        var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                                //        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                                //        aftPryQty = uomConversionResult.Data;

                                //        //轉換主單位異動量
                                //        var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                                //        if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                                //        mPrimaryQty = uomConversionResult2.Data;
                                //    }
                                //    else if (stock.ItemCategory == ItemCategory.Roll)
                                //    {
                                //        mPrimaryQty = tempDetail.TransferPrimaryQuantity;
                                //        aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                                //        if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                                //        aftSecQty = null;
                                //        mSecondaryQty = null;
                                //    }
                                //    else
                                //    {
                                //        throw new Exception("無法識別貨品類別");
                                //    }

                                //    tempDetail.TransferPrimaryQuantity = mPrimaryQty;
                                //    tempDetail.OriginalPrimaryQuantity = stock.PrimaryAvailableQty;
                                //    tempDetail.AfterPrimaryQuantity = aftPryQty;
                                //    tempDetail.TransferSecondaryQuantity = mSecondaryQty;
                                //    tempDetail.OriginalSecondaryQuantity = stock.SecondaryAvailableQty;
                                //    tempDetail.AfterSecondaryQuantity = aftSecQty;
                                //    trfInventoryTRepository.Update(tempDetail);

                                //}

                                ////更新其它尚未儲存的明細數量
                                //if (header.TransactionTypeId == TransactionTypeId.Chp16Out)
                                //{
                                //    var tempDetail = trfInventoryTRepository.GetAll().Join(
                                //        trfInventoryHeaderTRepository.GetAll().Where(x => x.TransactionTypeId == TransactionTypeId.Chp16In && x.NumberStatus == NumberStatus.NotSaved),
                                //        d => new { d.TransferInventoryHeaderId },
                                //        h => new { h.TransferInventoryHeaderId },
                                //        (d, h) => d)
                                //        .FirstOrDefault(x => x.StockId == stock.StockId);
                                //    if (tempDetail == null) continue;

                                //    if (stock.ItemCategory == ItemCategory.Flat)
                                //    {
                                //        mSecondaryQty = tempDetail.TransferSecondaryQuantity;
                                //        aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mSecondaryQty;
                                //        if (aftSecQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                                //        var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                                //        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                                //        aftPryQty = uomConversionResult.Data;

                                //        //轉換主單位異動量
                                //        var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                                //        if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                                //        mPrimaryQty = uomConversionResult2.Data;
                                //    }
                                //    else if (stock.ItemCategory == ItemCategory.Roll)
                                //    {
                                //        mPrimaryQty = tempDetail.TransferPrimaryQuantity;
                                //        aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                                //        if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                                //        aftSecQty = null;
                                //        mSecondaryQty = null;
                                //    }
                                //    else
                                //    {
                                //        throw new Exception("無法識別貨品類別");
                                //    }

                                //    tempDetail.TransferPrimaryQuantity = mPrimaryQty;
                                //    tempDetail.OriginalPrimaryQuantity = stock.PrimaryAvailableQty;
                                //    tempDetail.AfterPrimaryQuantity = aftPryQty;
                                //    tempDetail.TransferSecondaryQuantity = mSecondaryQty;
                                //    tempDetail.OriginalSecondaryQuantity = stock.SecondaryAvailableQty;
                                //    tempDetail.AfterSecondaryQuantity = aftSecQty;
                                //    trfInventoryTRepository.Update(tempDetail);

                                //}


                            }

                        }



                        //複製明細資料到歷史明細
                        string cmd = @"
INSERT INTO [TRF_INVENTORY_HT]
(
	[TRANSFER_INVENTORY_ID]
      ,[TRANSFER_INVENTORY_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[BARCODE]
      ,[STOCK_ID]
      ,[PRIMARY_UOM]
      ,[TRANSFER_PRIMARY_QUANTITY]
      ,[ORIGINAL_PRIMARY_QUANTITY]
      ,[AFTER_PRIMARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[TRANSFER_SECONDARY_QUANTITY]
      ,[ORIGINAL_SECONDARY_QUANTITY]
      ,[AFTER_SECONDARY_QUANTITY]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_INVENTORY_ID]
      ,[TRANSFER_INVENTORY_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[BARCODE]
      ,[STOCK_ID]
      ,[PRIMARY_UOM]
      ,[TRANSFER_PRIMARY_QUANTITY]
      ,[ORIGINAL_PRIMARY_QUANTITY]
      ,[AFTER_PRIMARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[TRANSFER_SECONDARY_QUANTITY]
      ,[ORIGINAL_SECONDARY_QUANTITY]
      ,[AFTER_SECONDARY_QUANTITY]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_INVENTORY_T]
  WHERE [TRANSFER_INVENTORY_HEADER_ID] = @TRANSFER_INVENTORY_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_INVENTORY_HEADER_ID", header.TransferInventoryHeaderId)) <= 0)
                        {
                            throw new Exception("複製盤點明細資料到盤點歷史明細失敗");
                        }

                        //刪除明細資料
                        cmd = @"
  DELETE FROM [TRF_INVENTORY_T]
  WHERE [TRANSFER_INVENTORY_HEADER_ID] = @TRANSFER_INVENTORY_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_INVENTORY_HEADER_ID", header.TransferInventoryHeaderId)) <= 0)
                        {
                            throw new Exception("刪除盤點明細資料失敗");
                        }
                    }

                    this.SaveChanges();
                    //trfInventoryHeaderTRepository.SaveChanges();
                    //stockTRepository.SaveChanges();
                    //stkTxnTRepository.SaveChanges();

                    txn.Commit();

                    return new ResultModel(true, "盤點異動存檔成功");

                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "盤點異動存檔失敗:" + ex.Message);
                }



            }
        }
        /// <summary>
        /// 新增明細 用於沒有庫存時
        /// </summary>
        /// <param name="stockInventoryDT"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultModel CreateDetailForNoStock(StockInventoryDT stockInventoryDT, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    if (stockInventoryDT == null) return new ResultModel(false, "沒有資料可新增明細");
                    if (string.IsNullOrEmpty(stockInventoryDT.ITEM_NO)) throw new Exception("沒有料號");
                    if (stockInventoryDT.ORGANIZATION_ID == 0) throw new Exception("沒有組織ID");
                    if (string.IsNullOrEmpty(stockInventoryDT.SUBINVENTORY_CODE)) throw new Exception("沒有倉庫");

                    string locatorSegments = null;
                    if (stockInventoryDT.LOCATOR_ID > 0) //判斷是否有儲位
                    {
                        if (string.IsNullOrEmpty(stockInventoryDT.SEGMENT3)) throw new Exception("沒有儲位第三節段");
                        var locator = GetLocatorForTransfer(stockInventoryDT.LOCATOR_ID, now);
                        if (locator == null) throw new Exception("找不到儲位資料");
                        locatorSegments = locator.LocatorSegments;
                    }


                    var item = GetItemNumber(stockInventoryDT.ITEM_NO);
                    if (item == null) throw new Exception("找不到料號資料");

                    if (item.CatalogElemVal070 == ItemCategory.Roll)
                    {
                        if (string.IsNullOrEmpty(stockInventoryDT.LOT_NUMBER)) return new ResultModel(false, "請輸入捲號");
                        var tempDetail = trfInventoryTRepository.GetAll().FirstOrDefault(x => x.LotNumber == stockInventoryDT.LOT_NUMBER);
                        if (tempDetail != null) throw new Exception("捲號不可重複");
                        var stock = stockTRepository.GetAll().FirstOrDefault(x => x.LotNumber == stockInventoryDT.LOT_NUMBER); //待確認 捲號庫存搜尋方式
                        if (stock != null) throw new Exception("此捲號" + stockInventoryDT.LOT_NUMBER + "已入庫");
                    }

                    var organization = GetOrganization(stockInventoryDT.ORGANIZATION_ID);
                    if (organization == null) throw new Exception("找不到組織資料");

                   
                   
                    //產生條碼清單
                    var generateBarcodesResult = GenerateBarcodes(stockInventoryDT.ORGANIZATION_ID, stockInventoryDT.SUBINVENTORY_CODE, 1, userId);
                    if (!generateBarcodesResult.Success) throw new Exception(generateBarcodesResult.Msg);



                    //計算數量
                    decimal aftPryQty = 0; //主單位異動後數量
                    decimal? aftSecQty = null; //次單位異動後數量
                    decimal mPrimaryQty = 0; //主單位異動量
                    decimal? mSecondaryQty = null; //次單位異動量
                    if (item.CatalogElemVal070 == ItemCategory.Flat)
                    {
                        if (stockInventoryDT.SECONDARY_TRANSACTION_QTY == 0) return new ResultModel(false, "次單位異動量輸入錯誤");
                        mSecondaryQty = stockInventoryDT.SECONDARY_TRANSACTION_QTY;
                        aftSecQty = mSecondaryQty;
                        var uomConversionResult = uomConversion.Convert(item.InventoryItemId, (decimal)mSecondaryQty, item.SecondaryUomCode, item.PrimaryUomCode); //次單位數量轉主單位數量
                        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                        mPrimaryQty = uomConversionResult.Data;
                        aftPryQty = mPrimaryQty;
                    }
                    else if (item.CatalogElemVal070 == ItemCategory.Roll)
                    {
                        if (stockInventoryDT.PRIMARY_TRANSACTION_QTY == 0) return new ResultModel(false, "主單位異動量輸入錯誤");
                        mPrimaryQty = stockInventoryDT.PRIMARY_TRANSACTION_QTY;
                        aftPryQty = mPrimaryQty;
                        mSecondaryQty = null;
                        aftSecQty = null;
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

                    var transactionType = GetTransactionType(TransactionTypeId.Chp16In);
                    if (transactionType == null) throw new Exception("找不到庫存交易類別資料");


                    var header = trfInventoryHeaderTRepository.GetAll().FirstOrDefault(x => x.TransactionTypeId == TransactionTypeId.Chp16In &&
                    x.OrganizationId == stockInventoryDT.ORGANIZATION_ID && x.SubinventoryCode == stockInventoryDT.SUBINVENTORY_CODE && x.LocatorId == stockInventoryDT.LOCATOR_ID && x.NumberStatus == NumberStatus.NotSaved);

                    if (header == null)
                    {
                        header = new TRF_INVENTORY_HEADER_T()
                        {
                            OrgId = organization.OrgUnitId,
                            OrganizationId = stockInventoryDT.ORGANIZATION_ID,
                            OrganizationCode = organization.OrganizationCode,
                            ShipmentNumber = GetShipmentNumberGuid(),
                            SubinventoryCode = stockInventoryDT.SUBINVENTORY_CODE,
                            LocatorId = stockInventoryDT.LOCATOR_ID == 0 ? default(long?) : stockInventoryDT.LOCATOR_ID,
                            LocatorCode = locatorSegments,
                            Segment3 = stockInventoryDT.SEGMENT3 == "" ? null : stockInventoryDT.SEGMENT3,
                            NumberStatus = NumberStatus.NotSaved,
                            TransactionDate = now,
                            TransactionTypeId = TransactionTypeId.Chp16In,
                            TransactionTypeName = transactionType.TransactionTypeName,
                            TransferOrgId = null,
                            TransferOrganizationId = null,
                            TransferOrganizationCode = null,
                            TransferSubinventoryCode = null,
                            TransferLocatorId = null,
                            TransferLocatorCode = null,
                            CreatedBy = userId,
                            CreatedUserName = userName,
                            CreationDate = now,
                            LastUpdateBy = null,
                            LastUpdateUserName = null,
                            LastUpdateDate = null
                        };
                        trfInventoryHeaderTRepository.Create(header, true);
                    }

                    //產生盤點明細
                    var detail = new TRF_INVENTORY_T()
                    {
                        TransferInventoryHeaderId = header.TransferInventoryHeaderId,
                        InventoryItemId = item.InventoryItemId,
                        ItemNumber = stockInventoryDT.ITEM_NO,
                        ItemDescription = item.ItemDescTch,
                        Barcode = generateBarcodesResult.Data[0],
                        StockId = 0,
                        PrimaryUom = item.PrimaryUomCode,
                        TransferPrimaryQuantity = mPrimaryQty,
                        OriginalPrimaryQuantity = aftPryQty,
                        AfterPrimaryQuantity = aftPryQty,
                        SecondaryUom = item.SecondaryUomCode,
                        TransferSecondaryQuantity = mSecondaryQty,
                        OriginalSecondaryQuantity = aftSecQty,
                        AfterSecondaryQuantity = aftSecQty,
                        LotNumber = stockInventoryDT.LOT_NUMBER,
                        LotQuantity = aftPryQty, //待確認理論重
                        Note = stockInventoryDT.NOTE,
                        CreatedBy = userId,
                        CreatedUserName = userName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    };
                    trfInventoryTRepository.Create(detail, true);

                    txn.Commit();
                    return new ResultModel(true, "新增庫存明細成功");

                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "新增庫存明細失敗:" + ex.Message);
                }
            }
        }


        #region 標籤
        public ResultDataModel<List<LabelModel>> GetProfitLabels(List<long> trfInventoryIdList, string userName)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (trfInventoryIdList == null || trfInventoryIdList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到明細資料", null);
                var detailDataList = trfInventoryTRepository.GetAll().AsNoTracking().Where(x => trfInventoryIdList.Contains(x.TransferInventoryId)).ToList();
                if (detailDataList == null || detailDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                if (detailDataList.Count != trfInventoryIdList.Count) throw new Exception("找不到部分揀貨資料");

                //對方為MES時料號資料從庫存取
                foreach (TRF_INVENTORY_T detail in detailDataList)
                {
                    StringBuilder cmd = null;
                    //var header = GetTrfHeader(detailDataList[0].TransferHeaderId);
                    //if (header == null) return new ResultDataModel<List<LabelModel>>(false, "找不到出貨編號資料", null);

                    if (detail.StockId == 0)
                    {
                        var item = GetItemNumber(detail.InventoryItemId);
                        if (item == null) throw new Exception("找不到料號資料");

                        //為新增庫存 料號資料從資料庫取 數量從明細取
                        cmd = new StringBuilder(@"
SELECT d.BARCODE as Barocde
,@userName as PrintBy
,i.ITEM_DESC_TCH as BarocdeName
,i.CATALOG_ELEM_VAL_020 as PapaerType
,i.CATALOG_ELEM_VAL_040 as BasicWeight
,i.CATALOG_ELEM_VAL_050 as Specification
,'' as BatchNo");

                        if (item.CatalogElemVal070 == ItemCategory.Flat) //判斷是否為平版
                        {
                            //平板 數量為明細的次單位異動量
                            cmd.Append(@"
,i.SECONDARY_UOM_CODE as Unit
,FORMAT(d.TRANSFER_SECONDARY_QUANTITY,'0.##########') as Qty
,d.LOT_NUMBER as LotNumber
FROM [TRF_INVENTORY_T] d 
INNER JOIN ITEMS_T i ON d.INVENTORY_ITEM_ID = i.INVENTORY_ITEM_ID
WHERE d.BARCODE = @Barcode
");
                        }
                        else if (item.CatalogElemVal070 == ItemCategory.Roll)
                        {
                            //捲筒 數量為明細的主單位異動量
                            cmd.Append(@"
,i.PRIMARY_UOM_CODE as Unit
,FORMAT(d.TRANSFER_PRIMARY_QUANTITY,'0.##########') as Qty
,d.LOT_NUMBER as LotNumber
FROM [TRF_INVENTORY_T] d
INNER JOIN ITEMS_T i ON d.INVENTORY_ITEM_ID = i.INVENTORY_ITEM_ID
WHERE d.BARCODE = @Barcode
");
                        }
                        else
                        {
                            throw new Exception("無法識別貨品類別");
                        }
                    }
                    else
                    {
                        var item = GetItemNumber(detail.InventoryItemId);
                        if (item == null) throw new Exception("找不到料號資料");

                        //為更改舊庫存 料號資料從庫存取 數量從明細取異動後的數量
                        cmd = new StringBuilder(@"
SELECT s.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as BatchNo");

                        if (item.CatalogElemVal070 == ItemCategory.Flat) //判斷是否為平版
                        {
                            //平板 數量為明細的次單位異動量
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(d.AFTER_SECONDARY_QUANTITY,'0.##########') as Qty
,d.LOT_NUMBER as LotNumber
FROM [TRF_INVENTORY_T] d
INNER JOIN TRF_INVENTORY_HEADER_T h ON d.TRANSFER_INVENTORY_HEADER_ID = h.TRANSFER_INVENTORY_HEADER_ID
INNER JOIN STOCK_T s ON d.INVENTORY_ITEM_ID = s.INVENTORY_ITEM_ID
WHERE d.BARCODE = @Barcode  AND h.TRANSACTION_TYPE_ID = @TransactionTypeId
");
                        }
                        else if (item.CatalogElemVal070 == ItemCategory.Roll)
                        {
                            //捲筒 數量為明細的主單位異動量
                            cmd.Append(@"
,s.PRIMARY_UOM_CODE as Unit
,FORMAT(d.AFTER_PRIMARY_QUANTITY,'0.##########') as Qty
,d.LOT_NUMBER as LotNumber
FROM [TRF_INVENTORY_T] d
INNER JOIN TRF_INVENTORY_HEADER_T h ON d.TRANSFER_INVENTORY_HEADER_ID = h.TRANSFER_INVENTORY_HEADER_ID
INNER JOIN STOCK_T s ON d.INVENTORY_ITEM_ID = s.INVENTORY_ITEM_ID
WHERE d.BARCODE = @Barcode  AND h.TRANSACTION_TYPE_ID = @TransactionTypeId
");
                        }
                        else
                        {
                            throw new Exception("無法識別貨品類別");
                        }

                    }

                    if (detail.StockId == 0)
                    {
                        var pUserName = SqlParamHelper.GetNVarChar("@userName", userName);
                        var pBarcode = SqlParamHelper.R.Barcode("@Barcode", detail.Barcode);
                        var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), pUserName, pBarcode).ToList();
                        if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                        labelModelList.Add(labelModel[0]);
                    }
                    else
                    {
                        var pUserName = SqlParamHelper.GetNVarChar("@userName", userName);
                        var pBarcode = SqlParamHelper.R.Barcode("@Barcode", detail.Barcode);
                        var pTypeId = SqlParamHelper.GetBigInt("@transactionTypeId", TransactionTypeId.Chp16In);
                        var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), pUserName, pBarcode, pTypeId).ToList();
                        if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                        labelModelList.Add(labelModel[0]);
                    }
                }
                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);

            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }

        }

        #endregion

    }
}