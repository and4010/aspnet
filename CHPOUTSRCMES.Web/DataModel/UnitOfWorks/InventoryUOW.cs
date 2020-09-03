using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Inventory;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using NLog;


namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class InventoryUOW : TransferUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_INVENTORY_HEADER_T> trfInventoryHeaderTRepositiory;
        private readonly IRepository<TRF_INVENTORY_T> trfInventoryTRepositiory;
        private readonly IRepository<TRF_INVENTORY_HT> trfInventoryHtRepositiory;

        public InventoryUOW(DbContext context)
        : base(context)
        {
            this.trfInventoryHeaderTRepositiory = new GenericRepository<TRF_INVENTORY_HEADER_T>(this);
            this.trfInventoryTRepositiory = new GenericRepository<TRF_INVENTORY_T>(this);
            this.trfInventoryHtRepositiory = new GenericRepository<TRF_INVENTORY_HT>(this);
        }

        InventoryType inventoryType = new InventoryType();
        /// <summary>
        /// 雜項異動類別
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
      ,[SECONDARY_AVAILABLE_QTY] AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE
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
      ,[SECONDARY_AVAILABLE_QTY] AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE
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
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventory", subinventoryCode);
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

        public ResultModel CreateDetail(long transactionTypeId, long organizationId, string subinventoryCode, long? locatorId,
   long stockId, decimal mQty, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var header = trfInventoryHeaderTRepositiory.GetAll().FirstOrDefault(x => x.TransactionTypeId == transactionTypeId &&
                    x.OrganizationId == organizationId && x.SubinventoryCode == subinventoryCode && x.LocatorId == locatorId && x.NumberStatus == NumberStatus.NotSaved);

                    if (header == null)
                    {
                        //產生header資料
                        var organization = GetOrganization(organizationId);
                        if (organization == null) throw new Exception("找不到出庫組織資料");

                        var transactionType = GetTransactionType(transactionTypeId);
                        if (transactionType == null) throw new Exception("找不到庫存交易類別資料");

                        string locatorCode = null;
                        string segment3 = null;
                        if (locatorId != null)
                        {
                            var outLocator = GetLocatorForTransfer(organizationId, subinventoryCode, now);
                            if (outLocator == null) throw new Exception("找不到出庫儲位資料");
                            locatorCode = outLocator.LocatorSegments;
                            segment3 = outLocator.LocatorSegments;
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

                        trfInventoryHeaderTRepositiory.Create(header, true);
                    }

                    var detail = trfInventoryTRepositiory.GetAll().FirstOrDefault(x =>
                    x.TransferInventoryHeaderId == header.TransferInventoryHeaderId &&
                    x.StockId == stockId);
                    if (detail != null) return new ResultModel(false, "已存在此條碼:" + detail.Barcode + "異動紀錄");

                    var stock = stockTRepositiory.GetAll().FirstOrDefault(x => x.StockId == stockId);
                    if (stock == null) throw new Exception("找不到庫存資料");

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

                    //計算異動後的數量
                    decimal aftPryQty = 0; //主單位異動後數量
                    decimal? aftSecQty = null; //次單位異動後數量
                    decimal mPrimaryQty = 0; //主單位異動量
                    decimal? mSecondaryQty = null; //次單位異動量
                    if (stock.ItemCategory == ItemCategory.Flat)
                    {
                        mSecondaryQty = mQty;
                        aftSecQty = (stock.SecondaryAvailableQty == null ? 0 : stock.SecondaryAvailableQty) + mQty;
                        if (aftSecQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.SecondaryAvailableQty + stock.SecondaryUomCode);
                        var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)aftSecQty, stock.SecondaryUomCode, stock.PrimaryUomCode); //主單位數量轉次單位數量
                        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                        aftPryQty = uomConversionResult.Data;

                        //轉換主單位異動量
                        var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, (decimal)mSecondaryQty, stock.SecondaryUomCode, stock.PrimaryUomCode);
                        if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                        mPrimaryQty = uomConversionResult2.Data;
                    }
                    else if (stock.ItemCategory == ItemCategory.Roll)
                    {
                        mPrimaryQty = mQty;
                        aftPryQty = stock.PrimaryAvailableQty + mQty;
                        if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
                        aftSecQty = null;
                        mSecondaryQty = null;
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

                    //產生雜項異動明細
                    detail = new TRF_INVENTORY_T()
                    {
                        TransferInventoryHeaderId = header.TransferInventoryHeaderId,
                        InventoryItemId = stock.InventoryItemId,
                        ItemNumber = stock.ItemNumber,
                        ItemDescription = stock.ItemDescription,
                        Barcode = stock.Barcode,
                        StockId = stockId,
                        PrimaryUom = stock.PrimaryUomCode,
                        TransferPrimaryQuantity = mPrimaryQty,
                        OriginalPrimaryQuantity = stock.PrimaryAvailableQty,
                        AfterPrimaryQuantity = aftPryQty,
                        SecondaryUom = stock.SecondaryUomCode,
                        TransferSecondaryQuantity = mSecondaryQty,
                        OriginalSecondaryQuantity = stock.SecondaryAvailableQty,
                        AfterSecondaryQuantity = aftSecQty,
                        LotNumber = stock.LotNumber,
                        LotQuantity = null,
                        Note = null,
                        CreatedBy = userId,
                        CreatedUserName = userName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    };
                    trfInventoryTRepositiory.Create(detail, true);

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

        public List<StockInventoryDT> GetStockInventoryTList(string userId)
        {
            try
            {
                string cmd = @"
SELECT m.TRANSFER_INVENTORY_ID AS ID
      ,m.[STOCK_ID] AS STOCK_ID
	  ,ROW_NUMBER() OVER(ORDER BY [STOCK_ID]) AS SUB_ID
      ,h.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
	  ,h.[SEGMENT3] AS SEGMENT3
      ,m.[ITEM_NUMBER] AS ITEM_NO
      ,m.[BARCODE] AS BARCODE
      ,m.[PRIMARY_UOM] AS PRIMARY_UOM_CODE
      ,m.TRANSFER_PRIMARY_QUANTITY AS PRIMARY_TRANSACTION_QTY
	  ,m.AFTER_PRIMARY_QUANTITY AS PRIMARY_AVAILABLE_QTY
      ,m.SECONDARY_UOM AS SECONDARY_UOM_CODE
      ,m.TRANSFER_SECONDARY_QUANTITY AS SECONDARY_TRANSACTION_QTY
	  ,m.AFTER_SECONDARY_QUANTITY AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
  FROM TRF_INVENTORY_HEADER_T h
  INNER JOIN TRF_INVENTORY_T m on h.TRANSFER_INVENTORY_HEADER_ID = m.TRANSFER_INVENTORY_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  WHERE u.UserId = @userId
";
                var pUserId = SqlParamHelper.GetNVarChar("@userId", userId);

                return this.Context.Database.SqlQuery<StockInventoryDT>(cmd, pUserId).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockInventoryDT>();
            }
        }
    }
}