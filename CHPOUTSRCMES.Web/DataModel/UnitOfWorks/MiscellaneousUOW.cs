using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Miscellaneous;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class MiscellaneousUOW : TransferUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_MISCELLANEOUS_HEADER_T> trfMiscellaneousHeaderTRepositiory;
        private readonly IRepository<TRF_MISCELLANEOUS_T> trfMiscellaneousTRepositiory;
        private readonly IRepository<TRF_MISCELLANEOUS_HT> trfMiscellaneousHtRepositiory;

        public MiscellaneousUOW(DbContext context)
          : base(context)
        {
            this.trfMiscellaneousHeaderTRepositiory = new GenericRepository<TRF_MISCELLANEOUS_HEADER_T>(this);
            this.trfMiscellaneousTRepositiory = new GenericRepository<TRF_MISCELLANEOUS_T>(this);
            this.trfMiscellaneousHtRepositiory = new GenericRepository<TRF_MISCELLANEOUS_HT>(this);
        }

        MiscellaneousType miscellaneousType = new MiscellaneousType();

        /// <summary>
        /// 雜項異動類別
        /// </summary>
        public class MiscellaneousType
        {
            /// <summary>
            /// 雜收
            /// </summary>
            public const long Receive = TransactionTypeId.Chp37In;
            /// <summary>
            /// 雜發
            /// </summary>
            public const long Send = TransactionTypeId.Chp37Out;

            public string GetDesc(long type)
            {
                switch (type)
                {
                    case Receive:
                        return "雜收";
                    case Send:
                        return "雜發";
                    default:
                        return "";
                }
            }
        }

        #region 雜項異動類別下拉選單

        /// <summary>
        /// 取得雜項異動類別下拉選單
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetMiscellaneousTypeDropDownList()
        {
            var transferTypeList = createDropDownList(DropDownListType.Choice);
            transferTypeList.AddRange(getMiscellaneousTypeList());
            return transferTypeList;
        }

        private List<SelectListItem> getMiscellaneousTypeList()
        {
            var miscellaneousTypeList = new List<SelectListItem>();
            try
            {
                miscellaneousTypeList.Add(new SelectListItem() { Text = miscellaneousType.GetDesc(MiscellaneousType.Send), Value = MiscellaneousType.Send.ToString() });
                miscellaneousTypeList.Add(new SelectListItem() { Text = miscellaneousType.GetDesc(MiscellaneousType.Receive), Value = MiscellaneousType.Receive.ToString() });

                var a = miscellaneousTypeList.Select(x => new SelectListItem() { Text = x.Text, Value = x.Value }).Where(x => x.Value == "1");
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return miscellaneousTypeList;
        }

        #endregion


        public List<StockDT> GetStockTList(long organizationId, string subinventoryCode, long? locatorId, string itemNumber, decimal primaryQty, decimal percentageError)
        {
            try
            {
                percentageError = percentageError * 0.01m;
                decimal errorQty = primaryQty * percentageError;
                decimal maxQty = primaryQty + errorQty;
                decimal minQty = primaryQty - errorQty;

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
  AND s.PRIMARY_AVAILABLE_QTY >= @minQty
  AND s.PRIMARY_AVAILABLE_QTY <= @maxQty
  AND s.ITEM_NUMBER = @itemNumber
";

                    var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventoryCode);
                    var pMinQty = SqlParamHelper.GetDecimal("@minQty", minQty);
                    var pMaxQty = SqlParamHelper.GetDecimal("@maxQty", maxQty);
                    var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                    return this.Context.Database.SqlQuery<StockDT>(cmd, pOrg, pSub, pMinQty, pMaxQty, pItemNo).ToList();
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
  AND s.PRIMARY_AVAILABLE_QTY >= @minQty
  AND s.PRIMARY_AVAILABLE_QTY <= @maxQty
  AND s.ITEM_NUMBER = @itemNumber
";

                    var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                    var pSub = SqlParamHelper.R.SubinventoryCode("@subinventory", subinventoryCode);
                    var pLoc = SqlParamHelper.GetBigInt("@locatorId", (long)locatorId);
                    var pMinQty = SqlParamHelper.GetDecimal("@minQty", minQty);
                    var pMaxQty = SqlParamHelper.GetDecimal("@maxQty", maxQty);
                    var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                    return this.Context.Database.SqlQuery<StockDT>(cmd, pOrg, pSub, pLoc, pMinQty, pMaxQty, pItemNo).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockDT>();
            }
        }

        public List<StockMiscellaneousDT> GetStockMiscellaneousTList(string userId)
        {
            try
            {
                string cmd = @"
SELECT m.TRANSFER_MISCELLANEOUS_ID AS ID
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
  FROM TRF_MISCELLANEOUS_HEADER_T h
  INNER JOIN TRF_MISCELLANEOUS_T m on h.TRANSFER_MISCELLANEOUS_HEADER_ID = m.TRANSFER_MISCELLANEOUS_HEADER_ID 
  INNER JOIN USER_SUBINVENTORY_T u on h.ORGANIZATION_ID = u.ORGANIZATION_ID AND h.SUBINVENTORY_CODE = u.SUBINVENTORY_CODE
  WHERE u.UserId = @userId
";
                var pUserId = SqlParamHelper.GetNVarChar("@userId", userId);

                return this.Context.Database.SqlQuery<StockMiscellaneousDT>(cmd, pUserId).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<StockMiscellaneousDT>();
            }
        }


        public ResultModel CreateDetail(long transactionTypeId, long organizationId, string subinventoryCode, long? locatorId,
      long stockId, decimal mPrimaryQty, string note, string userId, string userName)
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
                        if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
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
                        if (aftPryQty < 0) return new ResultModel(false, "超過庫存數量:" + stock.PrimaryAvailableQty + stock.PrimaryUomCode);
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


        public ResultModel DelDetailData(List<long> ids)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var list = trfMiscellaneousTRepositiory.GetAll().Where(x => ids.Contains(x.TransferMiscellaneousId)).ToList();
                    if (list == null || list.Count == 0) return new ResultModel(false, "找不到明細資料");
                    if (list.Count != ids.Count) throw new Exception("找不到部分明細資料");

                    foreach (TRF_MISCELLANEOUS_T data in list)
                    {
                        var headerId = data.TransferMiscellaneousHeaderId;
                        trfMiscellaneousTRepositiory.Delete(data, true);
                        var detailList = trfMiscellaneousTRepositiory.GetAll().FirstOrDefault(x => x.TransferMiscellaneousHeaderId == headerId);
                        if (detailList == null)
                        {
                            var header = trfMiscellaneousHeaderTRepositiory.GetAll().FirstOrDefault(x => x.TransferMiscellaneousHeaderId == headerId);
                            if (header == null) throw new Exception("找不到檔頭資料");
                            trfMiscellaneousHeaderTRepositiory.Delete(header);
                        }
                    }
                    trfMiscellaneousHeaderTRepositiory.SaveChanges();

                    txn.Commit();
                    return new ResultModel(true, "刪除雜項異動明細成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除雜項異動明細失敗:" + ex.Message);
                }
            }
        }

        public ResultModel UpdateDetailNote(List<long> ids, string note, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var list = trfMiscellaneousTRepositiory.GetAll().Where(x => ids.Contains(x.TransferMiscellaneousId)).ToList();
                    if (list == null || list.Count == 0) return new ResultModel(false, "找不到明細資料");
                    if (list.Count != ids.Count) throw new Exception("找不到部分明細資料");

                    var now = DateTime.Now;
                    foreach (TRF_MISCELLANEOUS_T data in list)
                    {
                        data.Note = note;
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                        trfMiscellaneousTRepositiory.Update(data);
                    }

                    trfMiscellaneousTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "更新雜項異動備註成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "更新雜項異動備註失敗:" + ex.Message);
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

                    var headerIdList = trfMiscellaneousHeaderTRepositiory.GetAll().AsNoTracking().Join(
                        trfMiscellaneousTRepositiory.GetAll().AsNoTracking(),
                        h => new { h.TransferMiscellaneousHeaderId },
                        d => new { d.TransferMiscellaneousHeaderId },
                        (h, d) => new
                        {
                            HeaderId = h.TransferMiscellaneousHeaderId,
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
                .Select(x => x.HeaderId).ToList();

                    if (headerIdList == null || headerIdList.Count == 0) return new ResultModel(false, "沒有可存檔的資料");

                    foreach (long headerId in headerIdList)
                    {
                        var header = trfMiscellaneousHeaderTRepositiory.GetAll().FirstOrDefault(x => x.TransferMiscellaneousHeaderId == headerId);
                        if (header == null) throw new Exception("找不到檔頭資料");
                        header.NumberStatus = NumberStatus.Saved;
                        header.TransactionDate = now;
                        header.LastUpdateBy = userId;
                        header.LastUpdateDate = now;
                        header.LastUpdateUserName = userName;
                        trfMiscellaneousHeaderTRepositiory.Update(header);

                        var detailList = trfMiscellaneousTRepositiory.GetAll().Where(x => x.TransferMiscellaneousHeaderId == header.TransferMiscellaneousHeaderId).ToList();
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
                            null, CategoryCode.Miscellaneous, ActionCode.StockTransfer, header.ShipmentNumber,
                            detail.OriginalPrimaryQuantity,detail.TransferPrimaryQuantity, detail.AfterPrimaryQuantity, detail.OriginalSecondaryQuantity, 
                            detail.TransferSecondaryQuantity, detail.AfterSecondaryQuantity, StockStatusCode.InStock, userId, now);
                            stkTxnTRepositiory.Create(stkTxnT);
                        }

                      

                        //複製明細資料到歷史明細
                        string cmd = @"
INSERT INTO [TRF_MISCELLANEOUS_HT]
(
	[TRANSFER_MISCELLANEOUS_ID]
      ,[TRANSFER_MISCELLANEOUS_HEADER_ID]
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
SELECT [TRANSFER_MISCELLANEOUS_ID]
      ,[TRANSFER_MISCELLANEOUS_HEADER_ID]
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
  FROM [TRF_MISCELLANEOUS_T]
  WHERE [TRANSFER_MISCELLANEOUS_HEADER_ID] = @TRANSFER_MISCELLANEOUS_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_MISCELLANEOUS_HEADER_ID", header.TransferMiscellaneousHeaderId)) <= 0)
                        {
                            throw new Exception("複製雜項異動明細資料到雜項異動歷史明細失敗");
                        }

                        //刪除明細資料
                        cmd = @"
  DELETE FROM [TRF_MISCELLANEOUS_T]
  WHERE [TRANSFER_MISCELLANEOUS_HEADER_ID] = @TRANSFER_MISCELLANEOUS_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_MISCELLANEOUS_HEADER_ID", header.TransferMiscellaneousHeaderId)) <= 0)
                        {
                            throw new Exception("刪除雜項異動明細資料失敗");
                        }
                    }

                    trfMiscellaneousHeaderTRepositiory.SaveChanges();
                    stockTRepositiory.SaveChanges();
                    stkTxnTRepositiory.SaveChanges();

                    txn.Commit();

                    return new ResultModel(true, "雜項異動存檔成功");

                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "雜項異動存檔失敗:" + ex.Message);
                }



            }
        }
    }
}