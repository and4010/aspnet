﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

        public ResultModel CreateDetail(long stockId, decimal mQty, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var stock = stockTRepositiory.GetAll().FirstOrDefault(x => x.StockId == stockId);
                    if (stock == null) throw new Exception("找不到庫存資料");

                    var organizationId = stock.OrganizationId;
                    var subinventoryCode = stock.SubinventoryCode;
                    long? locatorId = stock.LocatorId;

                    var header = trfObsoleteHeaderTRepositiory.GetAll().FirstOrDefault(x =>
                    x.OrganizationId == organizationId && x.SubinventoryCode == subinventoryCode && x.LocatorId == locatorId && x.NumberStatus == NumberStatus.NotSaved);

                    if (header == null)
                    {
                           //產生header資料
                           var organization = GetOrganization(organizationId);
                        if (organization == null) throw new Exception("找不到庫存組織資料");

                        var transactionType = GetTransactionType(TransactionTypeId.Chp26Out);
                        if (transactionType == null) throw new Exception("找不到庫存交易類別資料");

                        string locatorCode = null;
                        string segment3 = null;
                        if (locatorId != null)
                        {
                            var outLocator = GetLocatorForTransfer(organizationId, subinventoryCode, now);
                            if (outLocator == null) throw new Exception("找不到庫存儲位資料");
                            locatorCode = outLocator.LocatorSegments;
                            segment3 = outLocator.Segment3;
                        }

                        header = new TRF_OBSOLETE_HEADER_T()
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
                            TransactionTypeId = TransactionTypeId.Chp26Out,
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

                        trfObsoleteHeaderTRepositiory.Create(header, true);
                    }

                    var detail = trfObsoleteTRepositiory.GetAll().FirstOrDefault(x =>
                    x.TransferObsoleteHeaderId == header.TransferObsoleteHeaderId &&
                    x.StockId == stockId);
                    if (detail != null) return new ResultModel(false, "已存在此條碼:" + detail.Barcode + "異動紀錄");

                   

                    //處理異動量
                    mQty = -1 * Math.Abs(mQty);

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
                    detail = new TRF_OBSOLETE_T()
                    {
                        TransferObsoleteHeaderId = header.TransferObsoleteHeaderId,
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
                    trfObsoleteTRepositiory.Create(detail, true);

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


        public List<StockObsoleteDT> GetStockObsoleteTList(string userId)
        {
            try
            {
                string cmd = @"
SELECT m.TRANSFER_OBSOLETE_ID AS ID
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
  FROM TRF_OBSOLETE_HEADER_T h
  INNER JOIN TRF_OBSOLETE_T m on h.TRANSFER_OBSOLETE_HEADER_ID = m.TRANSFER_OBSOLETE_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  WHERE u.UserId = @userId
";
                var pUserId = SqlParamHelper.GetNVarChar("@userId", userId);

                return this.Context.Database.SqlQuery<StockObsoleteDT>(cmd, pUserId).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockObsoleteDT>();
            }
        }


        public ResultModel DelDetailData(List<long> ids)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var list = trfObsoleteTRepositiory.GetAll().Where(x => ids.Contains(x.TransferObsoleteId)).ToList();
                    if (list == null || list.Count == 0) return new ResultModel(false, "找不到明細資料");
                    if (list.Count != ids.Count) throw new Exception("找不到部分明細資料");

                    foreach (TRF_OBSOLETE_T data in list)
                    {
                        var headerId = data.TransferObsoleteHeaderId;
                        trfObsoleteTRepositiory.Delete(data, true);
                        var detailList = trfObsoleteTRepositiory.GetAll().FirstOrDefault(x => x.TransferObsoleteHeaderId == headerId);
                        if (detailList == null)
                        {
                            var header = trfObsoleteHeaderTRepositiory.GetAll().FirstOrDefault(x => x.TransferObsoleteHeaderId == headerId);
                            if (header == null) throw new Exception("找不到檔頭資料");
                            trfObsoleteHeaderTRepositiory.Delete(header);
                        }
                    }
                    trfObsoleteHeaderTRepositiory.SaveChanges();

                    txn.Commit();
                    return new ResultModel(true, "刪除存貨報廢明細成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除存貨報廢明細失敗:" + ex.Message);
                }
            }
        }

        public ResultModel UpdateDetailNote(List<long> ids, string note, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var list = trfObsoleteTRepositiory.GetAll().Where(x => ids.Contains(x.TransferObsoleteId)).ToList();
                    if (list == null || list.Count == 0) return new ResultModel(false, "找不到明細資料");
                    if (list.Count != ids.Count) throw new Exception("找不到部分明細資料");

                    var now = DateTime.Now;
                    foreach (TRF_OBSOLETE_T data in list)
                    {
                        data.Note = note;
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                        trfObsoleteTRepositiory.Update(data);
                    }

                    trfObsoleteTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "更新存貨報廢備註成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "更新存貨報廢備註失敗:" + ex.Message);
                }
            }
        }

        public ResultModel SaveTransactionDetail(string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var headerIdList = trfObsoleteHeaderTRepositiory.GetAll().AsNoTracking().Join(
                        trfObsoleteTRepositiory.GetAll().AsNoTracking(),
                        h => new { h.TransferObsoleteHeaderId },
                        d => new { d.TransferObsoleteHeaderId },
                        (h, d) => new
                        {
                            HeaderId = h.TransferObsoleteHeaderId,
                            OrganizationId = h.OrganizationId,
                            SubinventoryCode = h.SubinventoryCode
                        })
                        .Join(
                userSubinventoryTRepositiory.GetAll().AsNoTracking(),
                x => new { x.OrganizationId, x.SubinventoryCode },
                u => new { u.OrganizationId, u.SubinventoryCode },
                (h, u) => new
                {
                    HeaderId = h.HeaderId,
                    UserId = u.UserId
                })
                .Where(x => x.UserId == userId)
                .GroupBy(x => new { x.HeaderId })
                .Select(x => x.Key.HeaderId).ToList();

                    if (headerIdList == null || headerIdList.Count == 0) return new ResultModel(false, "沒有可存檔的資料");

                    foreach (long headerId in headerIdList)
                    {
                        var header = trfObsoleteHeaderTRepositiory.GetAll().FirstOrDefault(x => x.TransferObsoleteHeaderId == headerId);
                        if (header == null) throw new Exception("找不到檔頭資料");
                        header.NumberStatus = NumberStatus.Saved;
                        header.TransactionDate = now;
                        header.LastUpdateBy = userId;
                        header.LastUpdateDate = now;
                        header.LastUpdateUserName = userName;
                        trfObsoleteHeaderTRepositiory.Update(header);

                        var detailList = trfObsoleteTRepositiory.GetAll().Where(x => x.TransferObsoleteHeaderId == header.TransferObsoleteHeaderId).ToList();
                        if (detailList == null || detailList.Count == 0) throw new Exception("找不到明細資料");

                        foreach (var detail in detailList)
                        {
                            //更新庫存
                            var stock = stockTRepositiory.GetAll().FirstOrDefault(x => x.StockId == detail.StockId);
                            if (stock == null) throw new Exception("找不到庫存資料");
                            stock.PrimaryAvailableQty = detail.AfterPrimaryQuantity;
                            stock.SecondaryAvailableQty = detail.AfterSecondaryQuantity;
                            stock.LastUpdateBy = userId;
                            stock.LastUpdateDate = now;
                            stockTRepositiory.Update(stock);

                            //產生異動紀錄
                            var stkTxnT = CreateStockRecord(stock, null, null, null,
                            null, CategoryCode.Obsolete, ActionCode.StockTransfer, header.ShipmentNumber,
                            detail.OriginalPrimaryQuantity, detail.TransferPrimaryQuantity, detail.AfterPrimaryQuantity, detail.OriginalSecondaryQuantity,
                            detail.TransferSecondaryQuantity, detail.AfterSecondaryQuantity, StockStatusCode.InStock, userId, now);
                            stkTxnTRepositiory.Create(stkTxnT);
                        }



                        //複製明細資料到歷史明細
                        string cmd = @"
INSERT INTO [TRF_OBSOLETE_HT]
(
	[TRANSFER_OBSOLETE_ID]
      ,[TRANSFER_OBSOLETE_HEADER_ID]
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
SELECT [TRANSFER_OBSOLETE_ID]
      ,[TRANSFER_OBSOLETE_HEADER_ID]
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
  FROM [TRF_OBSOLETE_T]
  WHERE [TRANSFER_OBSOLETE_HEADER_ID] = @TRANSFER_OBSOLETE_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_OBSOLETE_HEADER_ID", header.TransferObsoleteHeaderId)) <= 0)
                        {
                            throw new Exception("複製存貨報廢明細資料到存貨報廢歷史明細失敗");
                        }

                        //刪除明細資料
                        cmd = @"
  DELETE FROM [TRF_OBSOLETE_T]
  WHERE [TRANSFER_OBSOLETE_HEADER_ID] = @TRANSFER_OBSOLETE_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_OBSOLETE_HEADER_ID", header.TransferObsoleteHeaderId)) <= 0)
                        {
                            throw new Exception("刪除存貨報廢明細資料失敗");
                        }
                    }

                    trfObsoleteHeaderTRepositiory.SaveChanges();
                    stockTRepositiory.SaveChanges();
                    stkTxnTRepositiory.SaveChanges();

                    txn.Commit();

                    return new ResultModel(true, "存貨報廢存檔成功");

                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "存貨報廢存檔失敗:" + ex.Message);
                }



            }
        }


    }
}