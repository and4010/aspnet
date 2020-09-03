using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Obsolete;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class ObsoleteUOW : TransferUOW
    {

        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_OBSOLETE_HEADER_T> trfObsoleteHeaderTRepositiory;
        private readonly IRepository<TRF_OBSOLETE_T> trfObsoleteTRepositiory;
        private readonly IRepository<TRF_OBSOLETE_HT> trfObsoleteHtRepositiory;

        public ObsoleteUOW(DbContext context)
        : base(context)
        {
            this.trfObsoleteHeaderTRepositiory = new GenericRepository<TRF_OBSOLETE_HEADER_T>(this);
            this.trfObsoleteTRepositiory = new GenericRepository<TRF_OBSOLETE_T>(this);
            this.trfObsoleteHtRepositiory = new GenericRepository<TRF_OBSOLETE_HT>(this);
        }


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
    long stockId, decimal mQty, string note, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var header = trfMiscellaneousHeaderTRepositiory.GetAll().FirstOrDefault(x => x.TransactionTypeId == transactionTypeId &&
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

                        header = new TRF_MISCELLANEOUS_HEADER_T()
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

                        trfMiscellaneousHeaderTRepositiory.Create(header, true);
                    }

                    var detail = trfMiscellaneousTRepositiory.GetAll().FirstOrDefault(x =>
                    x.TransferMiscellaneousHeaderId == header.TransferMiscellaneousHeaderId &&
                    x.StockId == stockId);
                    if (detail != null) return new ResultModel(false, "已存在此條碼:" + detail.Barcode + "異動紀錄");

                    var stock = stockTRepositiory.GetAll().FirstOrDefault(x => x.StockId == stockId);
                    if (stock == null) throw new Exception("找不到庫存資料");

                    //處理異動量
                    mPrimaryQty = Math.Abs(mPrimaryQty); //轉為正數
                    if (mPrimaryQty > 1) return new ResultModel(false, " 超過最大數量限制1" + stock.PrimaryUomCode);

                    if (transactionTypeId == TransactionTypeId.Chp37Out)
                    {
                        mPrimaryQty = -1 * mPrimaryQty;
                    }
                    else if (transactionTypeId == TransactionTypeId.Chp37In)
                    {
                        //雜收為正數不用處理
                    }
                    else
                    {
                        throw new Exception("異動型態Id錯誤");
                    }

                    //計算異動後的數量
                    decimal aftPryQty = 0; //主單位異動後數量
                    decimal? aftSecQty = null; //次單位異動後數量
                    decimal? mSecondaryQty = null; //次單位異動量
                    if (stock.ItemCategory == ItemCategory.Flat)
                    {
                        aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                        var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, aftPryQty, stock.PrimaryUomCode, stock.SecondaryUomCode); //主單位數量轉次單位數量
                        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                        aftSecQty = uomConversionResult.Data;

                        //轉換次單位異動量
                        var uomConversionResult2 = uomConversion.Convert(stock.InventoryItemId, mPrimaryQty, stock.PrimaryUomCode, stock.SecondaryUomCode);
                        if (!uomConversionResult2.Success) throw new Exception(uomConversionResult.Msg);
                        mSecondaryQty = uomConversionResult2.Data;
                    }
                    else if (stock.ItemCategory == ItemCategory.Roll)
                    {
                        aftPryQty = stock.PrimaryAvailableQty + mPrimaryQty;
                        aftSecQty = null;
                        mSecondaryQty = null;
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

                    //產生雜項異動明細
                    detail = new TRF_MISCELLANEOUS_T()
                    {
                        TransferMiscellaneousHeaderId = header.TransferMiscellaneousHeaderId,
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
                        Note = note,
                        CreatedBy = userId,
                        CreatedUserName = userName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    };
                    trfMiscellaneousTRepositiory.Create(detail, true);

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

    }
}