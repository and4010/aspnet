﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Entiy.Transfer;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class TransferUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_HEADER_T> trfHeaderTRepositiory;
        private readonly IRepository<TRF_DETAIL_T> trfDetailTRepositiory;
        private readonly IRepository<TRF_DETAIL_HT> trfDetailHtRepositiory;
        private readonly IRepository<TRF_INBOUND_PICKED_T> trfInboundPickedTRepositiory;
        private readonly IRepository<TRF_INBOUND_PICKED_HT> trfInboundPickedHtRepositiory;
        private readonly IRepository<TRF_OUTBOUND_PICKED_T> trfOutboundPickedTRepositiory;
        private readonly IRepository<TRF_OUTBOUND_PICKED_HT> trfOutboundPickedHtRepositiory;

        public TransferType transferType = new TransferType();

        public TransferUOW(DbContext context)
           : base(context)
        {
            this.trfHeaderTRepositiory = new GenericRepository<TRF_HEADER_T>(this);
            this.trfDetailTRepositiory = new GenericRepository<TRF_DETAIL_T>(this);
            this.trfDetailHtRepositiory = new GenericRepository<TRF_DETAIL_HT>(this);
            this.trfInboundPickedTRepositiory = new GenericRepository<TRF_INBOUND_PICKED_T>(this);
            this.trfInboundPickedHtRepositiory = new GenericRepository<TRF_INBOUND_PICKED_HT>(this);
            this.trfOutboundPickedTRepositiory = new GenericRepository<TRF_OUTBOUND_PICKED_T>(this);
            this.trfOutboundPickedHtRepositiory = new GenericRepository<TRF_OUTBOUND_PICKED_HT>(this);

        }

        /// <summary>
        /// 庫存移轉類別
        /// </summary>
        public class TransferType : ICategory
        {
            /// <summary>
            /// 出庫
            /// </summary>
            public const string Outbound = "O";
            /// <summary>
            /// 入庫
            /// </summary>
            public const string InBound = "I";
            /// <summary>
            /// 貨故
            /// </summary>
            public const string Reason = "R";

            public string GetDesc(string statusCode)
            {
                switch (statusCode)
                {
                    case Outbound:
                        return "出庫";
                    case InBound:
                        return "入庫";
                    case Reason:
                        return "貨故";
                    default:
                        return "";
                }
            }


        }

        public class TransferCatalog
        {
            /// <summary>
            /// 組織間移轉
            /// </summary>
            public const string OrgTransfer = "ORG";
            /// <summary>
            /// 倉庫間移轉
            /// </summary>
            public const string InvTransfer = "INV";
        }



        public class IsMes
        {
            /// <summary>
            /// 對方非MES
            /// </summary>
            public const string No = "0";
            /// <summary>
            /// 對方是MES
            /// </summary>
            public const string Yes = "1";
        }

        public class DataUpadteAuthority
        {
            /// <summary>
            /// 拒絕
            /// </summary>
            public const string Denied = "0";
            /// <summary>
            /// 允許
            /// </summary>
            public const string Permit = "1";
        }

        public class DataWriteType
        {
            /// <summary>
            /// 手動輸入
            /// </summary>
            public const string KeyIn = "0";
            /// <summary>
            /// Excel匯入
            /// </summary>
            public const string ExcelImport = "1";
        }


        /// <summary>
        /// 出貨編號狀態
        /// </summary>
        public class NumberStatus
        {
            /// <summary>
            /// 未存檔
            /// </summary>
            public const string NotSaved = "0";
            /// <summary>
            /// 已存檔
            /// </summary>
            public const string Saved = "1";
        }

        public string GetTransferCatalog(string outSubinventoryCode, string inSubinventoryCode)
        {
            if (CompareOrganization(outSubinventoryCode, inSubinventoryCode))
            {
                return TransferCatalog.InvTransfer;
            }
            else
            {
                return TransferCatalog.OrgTransfer;
            }
        }

        public string GetTransferCatalog(long outOrganizationId, long inOrganizationId)
        {
            if (outOrganizationId == inOrganizationId)
            {
                return TransferCatalog.InvTransfer;
            }
            else
            {
                return TransferCatalog.OrgTransfer;
            }
        }

        

        #region 庫存移轉下拉選單

        /// <summary>
        /// 取得庫存移轉類別下拉選單
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetTransferTypeDropDownList()
        {
            var transferTypeList = createDropDownList(DropDownListType.Choice);
            transferTypeList.AddRange(getTransferTypeList());
            return transferTypeList;
        }

        private List<SelectListItem> getTransferTypeList()
        {
            var transferTypeList = new List<SelectListItem>();
            try
            {
                transferTypeList.Add(new SelectListItem() { Text = transferType.GetDesc(TransferType.Outbound), Value = TransferType.Outbound });
                transferTypeList.Add(new SelectListItem() { Text = transferType.GetDesc(TransferType.InBound), Value = TransferType.InBound });
                transferTypeList.Add(new SelectListItem() { Text = transferType.GetDesc(TransferType.Reason), Value = TransferType.Reason });
                var a = transferTypeList.Select(x => new SelectListItem() { Text = x.Text, Value = x.Value }).Where(x => x.Value == "1");
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return transferTypeList;
        }

        #endregion



        #region 出貨編號下拉選單

        public List<SelectListItem> GetOutBoundShipmentNumberDropDownList(string transferCatalog, string transferType, string outSubinventoryCode, string inSubinventoryCode)
        {
            var transferTypeList = createDropDownList(DropDownListType.Add);
            transferTypeList.AddRange(getShipmentNumberList(transferCatalog, transferType, outSubinventoryCode, inSubinventoryCode));
            return transferTypeList;
        }

        private List<SelectListItem> getShipmentNumberList(string transferCatalog, string transferType, string outSubinventoryCode, string inSubinventoryCode)
        {

            var shipmentNumberList = new List<SelectListItem>();
            try
            {
                shipmentNumberList =
                    trfHeaderTRepositiory.GetAll().AsNoTracking().Where(
                    x => x.TransferCatalog == transferCatalog &&
                    x.TransferType == transferType &&
                    x.SubinventoryCode == outSubinventoryCode &&
                    x.TransferSubinventoryCode == inSubinventoryCode)
                     .OrderBy(x => x.ShipmentNumber)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.ShipmentNumber,
                                Value = x.ShipmentNumber
                            }).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return shipmentNumberList;
        }

        #endregion

        public List<TRF_HEADER_T> GetTrfHeaderList(string transferCatalog, long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            return trfHeaderTRepositiory.GetAll().AsNoTracking().Where(
                   x => x.OrganizationId == outOrganizationId &&
                   x.TransferOrganizationId == inOrganizationId &&
                   x.TransferCatalog == transferCatalog &&
                   //x.TransferType == transferType &&
                   x.SubinventoryCode == outSubinventoryCode &&
                   x.TransferSubinventoryCode == inSubinventoryCode)
                    .OrderBy(x => x.ShipmentNumber).ToList();

        }

        public List<SelectListItem> GetInBoundShipmentNumberDropDownList(long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            var transferTypeList = createDropDownList(DropDownListType.Add);
            transferTypeList.AddRange(GetShipmentNumberforInbound(outOrganizationId, outSubinventoryCode, inOrganizationId, inSubinventoryCode));
            return transferTypeList;
        }

        /// <summary>
        /// 取得入庫的出貨編號
        /// </summary>
        /// <param name="outOrganizationId"></param>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="inOrganizationId"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <returns></returns>
        public List<SelectListItem> GetShipmentNumberforInbound(long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            
            string cmd = @"
           SELECT CONVERT(varchar(10), TRANSFER_HEADER_ID) as Value,
      SHIPMENT_NUMBER as Text

FROM TRF_HEADER_T
WHERE 
TRANSFER_TYPE = 'I'
AND SUBINVENTORY_CODE = @subCode AND ORGANIZATION_ID = @organId
AND TRANSFER_SUBINVENTORY_CODE = @trfSubCode AND TRANSFER_ORGANIZATION_ID = @trfOrganId

UNION 


SELECT
CONVERT(varchar(10), s.TRANSFER_HEADER_ID) as Value,
s.SHIPMENT_NUMBER as Text
FROM TRF_HEADER_T s
LEFT JOIN TRF_HEADER_T t ON s.SHIPMENT_NUMBER = t.SHIPMENT_NUMBER AND t.TRANSFER_TYPE = 'I'  
WHERE 
s.TRANSFER_TYPE = 'O'
AND s.SUBINVENTORY_CODE = @subCode AND s.ORGANIZATION_ID = @organId
AND s.TRANSFER_SUBINVENTORY_CODE = @trfSubCode AND s.TRANSFER_ORGANIZATION_ID = @trfOrganId

AND s.NUMBER_STATUS = '1'
AND t.SHIPMENT_NUMBER IS NULL";

            return this.Context.Database.SqlQuery<SelectListItem>(cmd,
                new SqlParameter("@subCode", outSubinventoryCode),
                new SqlParameter("@organId", outOrganizationId),
                new SqlParameter("@trfSubCode", inSubinventoryCode),
                new SqlParameter("@trfOrganId", inOrganizationId)).ToList();




        }


        public List<TRF_DETAIL_T> GetTrfDetailList(long transferHeaderId, string itemNumber)
        {
            return trfDetailTRepositiory.GetAll().AsNoTracking().Where(x =>
            x.TransferHeaderId == transferHeaderId &&
            x.ItemNumber == itemNumber).ToList();
        }

        public TRF_DETAIL_T GetTrfDetail(long transferHeaderId, long inventoryItemId)
        {
            return trfDetailTRepositiory.GetAll().AsNoTracking().FirstOrDefault(x =>
            x.TransferHeaderId == transferHeaderId &&
            x.InventoryItemId == inventoryItemId);
        }

        //public List<TRF_HEADER_T> GetTrfHeaderList(string shipmentNumber, string transferType)
        //{
        //    return trfHeaderTRepositiory.GetAll().AsNoTracking().Where(x =>
        //    x.ShipmentNumber == shipmentNumber &&
        //    x.TransferType == transferType).ToList();
        //}

        public TRF_HEADER_T GetTrfHeader(string shipmentNumber, string transferType)
        {
            return trfHeaderTRepositiory.GetAll().AsNoTracking().FirstOrDefault(x =>
            x.ShipmentNumber == shipmentNumber &&
            x.TransferType == transferType);
        }

        public TRF_HEADER_T GetTrfHeader(long transferHeaderId)
        {
            return trfHeaderTRepositiory.GetAll().AsNoTracking().FirstOrDefault(x =>
            x.TransferHeaderId == transferHeaderId);
        }

        public List<StockTransferBarcodeDT> GetTrfInboundPickedTList(long transferHeaderId, string numberStatus)
        {
            string cmd = "";
            if (numberStatus == NumberStatus.NotSaved)
            {
                cmd = @"
SELECT [TRANSFER_PICKED_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY p.[TRANSFER_DETAIL_ID]) as SUB_ID
      ,p.[TRANSFER_DETAIL_ID] as TransferDetailId
      ,p.[TRANSFER_HEADER_ID] as TransferHeaderId
      ,p.[ITEM_NUMBER]
      ,[STOCK_ID] as StockId
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,ISNULL([SECONDARY_QUANTITY],0) as SECONDARY_QUANTITY
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[NOTE] as REMARK
      ,[STATUS] as Status
	  ,d.PACKING_TYPE as PACKING_TYPE
  FROM [TRF_INBOUND_PICKED_T] p
  inner join TRF_DETAIL_T d on p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
  WHERE p.TRANSFER_HEADER_ID = @transferHeaderId";
            }
            else
            {
                cmd = @"
SELECT [TRANSFER_PICKED_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY p.[TRANSFER_DETAIL_ID]) as SUB_ID
      ,p.[TRANSFER_DETAIL_ID] as TransferDetailId
      ,p.[TRANSFER_HEADER_ID] as TransferHeaderId
      ,p.[ITEM_NUMBER]
      ,[STOCK_ID] as StockId
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,ISNULL([SECONDARY_QUANTITY],0) as SECONDARY_QUANTITY
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[NOTE] as REMARK
      ,[STATUS] as Status
	  ,d.PACKING_TYPE as PACKING_TYPE
  FROM [TRF_INBOUND_PICKED_HT] p
  inner join TRF_DETAIL_T d on p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
  WHERE p.TRANSFER_HEADER_ID = @transferHeaderId";
            }
            return this.Context.Database.SqlQuery<StockTransferBarcodeDT>(cmd, new SqlParameter("@transferHeaderId", transferHeaderId)).ToList();
        }

        /// <summary>
        /// 取出貨編號 待修正為從資料庫取
        /// </summary>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <returns></returns>
        public string GetShipmentNumber(string outSubinventoryCode, string inSubinventoryCode)
        {
            Random rnd = new Random();
            List<int> randomList = Enumerable.Range(1, 999)
                .OrderBy(x => rnd.Next()).Take(100).ToList();
            return "(" + outSubinventoryCode + "-" + inSubinventoryCode + ")" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", randomList[0].ToString());
        }


        public ResultDataModel<TRF_HEADER_T> CreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId, string dataUpadteAuthority, string dataWriteType,
            decimal requestedQty, decimal rollReamWt, string lotNumber, string createUser, string createUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var item = GetItemNumber(itemNumber);
                    if (item == null) throw new Exception("找不到料號資料");
                    long transactionTypeId;
                    var transferCatalog = GetTransferCatalog(outOrganizationId, inOrganizationId);
                    if (transferCatalog == TransferCatalog.OrgTransfer)
                    {
                        transactionTypeId = TransferUOW.TransactionTypeId.IntransitShipment;
                        //檢查料號所屬組織
                        var itemNumberOrganizationIdList = GetItemNumberOrganizationId(item.InventoryItemId);
                        if (itemNumberOrganizationIdList == null || itemNumberOrganizationIdList.Count == 0) throw new Exception("找不到料號所屬組織");

                        if (transferType == TransferUOW.TransferType.InBound)
                        {
                            if (itemNumberOrganizationIdList.Where(x => x == inOrganizationId).ToList().Count == 0) throw new Exception("入庫組織沒有此料號");
                        }
                        else
                        {
                            if (itemNumberOrganizationIdList.Where(x => x == outOrganizationId).ToList().Count == 0) throw new Exception("出庫組織沒有此料號");
                        }
                        
                    }
                    else
                    {
                        transactionTypeId = TransferUOW.TransactionTypeId.Chp30;
                    }
                  
                    var outOrganization = GetOrganization(outOrganizationId);
                    if (outOrganization == null) throw new Exception("找不到出庫組織資料");

                    string locatorCode = "";
                    string outLocatorSegment3 = "";
                    if (outLocatorId != null)
                    {
                        var outLocator = GetLocator(outOrganizationId, outSubinventoryCode);
                        if (outLocator == null) throw new Exception("找不到出庫儲位資料");
                        locatorCode = outLocator.LocatorSegments;
                        outLocatorSegment3 = outLocator.Segment3;
                    }
                    
                    var inOrganization = GetOrganization(inOrganizationId);
                    if (inOrganization == null) throw new Exception("找不到出庫組織資料");

                    string transferLocatorCode = "";
                    string inLocatorSegment3 = "";
                    if (inLocatorId != null)
                    {
                        var inLocator = GetLocator(inOrganizationId, inSubinventoryCode);
                        if (inLocator == null) throw new Exception("找不到出庫儲位資料");
                        transferLocatorCode = inLocator.LocatorSegments;
                        inLocatorSegment3 = inLocator.Segment3;
                    }
                  
                    var transactionType = GetTransactionType(transactionTypeId);
                    if (transactionType == null) throw new Exception("找不到庫存交易類別資料");

                    if (shipmentNumber == DropDownListTypeValue.Add)
                    {
                        shipmentNumber = GetShipmentNumber(outSubinventoryCode, inSubinventoryCode);
                        trfHeaderTRepositiory.Create(new TRF_HEADER_T
                        {
                            OrgId = outOrganization.OrganizationId, //待修正為ORG_ID
                            OrganizationId = outOrganizationId,
                            OrganizationCode = outOrganization.OrganizationCode,
                            ShipmentNumber = shipmentNumber,
                            TransferCatalog = transferCatalog,
                            TransferType = transferType,
                            NumberStatus = NumberStatus.NotSaved,
                            IsMes = IsMes.No,
                            SubinventoryCode = outSubinventoryCode,
                            LocatorId = outLocatorId,
                            LocatorCode = locatorCode,
                            TransactionDate = now,
                            TransactionTypeId = transactionTypeId,
                            TransactionTypeName = transactionType.TransactionTypeName,
                            TransferOrgId = inOrganization.OrganizationId, //待修正為ORG_ID
                            TransferOrganizationId = inOrganizationId,
                            TransferOrganizationCode = inOrganization.OrganizationCode,
                            TransferSubinventoryCode = inSubinventoryCode,
                            TransferLocatorId = inLocatorId,
                            TransferLocatorCode = transferLocatorCode,
                            CreatedBy = createUser,
                            CreatedUserName = createUserName,
                            CreationDate = now,
                            LastUpdateBy = createUser,
                            LastUpdateUserName = createUserName,
                            LastUpdateDate = now
                        }, true);
                    }

                    var trfHeader = GetTrfHeader(shipmentNumber, transferType);
                    if (trfHeader == null) throw new Exception("找不到出貨編號資料");
                    if (trfHeader.OrganizationId != outOrganizationId) throw new Exception("出庫組織比對錯誤，請選擇此出庫倉庫" + outSubinventoryCode);
                    if (trfHeader.SubinventoryCode != outSubinventoryCode) throw new Exception("出庫倉庫比對錯誤，請選擇此出庫倉庫" + outSubinventoryCode);
                    if (trfHeader.LocatorId != outLocatorId) throw new Exception("出庫儲位比對錯誤，請選擇此出庫儲位" + outLocatorSegment3);
                    if (trfHeader.TransferOrganizationId != inOrganizationId) throw new Exception("入庫組織比對錯誤，請選擇此入庫倉庫" + inSubinventoryCode);
                    if (trfHeader.TransferSubinventoryCode != inSubinventoryCode) throw new Exception("入庫倉庫比對錯誤，請選擇此入庫倉庫" + inSubinventoryCode);
                    if (trfHeader.TransferLocatorId != inLocatorId) throw new Exception("入庫儲位比對錯誤，請選擇此入庫儲位" + inLocatorSegment3);

                    var trfDeatil = GetTrfDetail(trfHeader.TransferHeaderId, item.InventoryItemId);
                    if (trfDeatil != null) throw new Exception("料號重複輸入");

                    decimal rollReamQty = Math.Ceiling(requestedQty / rollReamWt); //無條件進位 算出棧板數或捲數

                    decimal priQty = 0;
                    decimal? secQty = null;

                    if (item.CatalogElemVal070 == ItemCategory.Roll)
                    {
                        priQty = requestedQty;
                    }
                    else if (item.CatalogElemVal070 == ItemCategory.Flat)
                    {
                        var uomConversionResult = uomConversion.Convert(item.InventoryItemId, requestedQty, item.SecondaryUomCode, item.PrimaryUomCode); //副單位需求量 轉 主單位需求量
                        if (uomConversionResult.Success)
                        {
                            priQty = uomConversionResult.Data;
                            secQty = requestedQty;
                        }
                        else
                        {
                            throw new Exception(uomConversionResult.Msg); //單位換算失敗 回傳錯誤
                        }
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }


                    trfDetailTRepositiory.Create(new TRF_DETAIL_T
                    {
                        TransferHeaderId = trfHeader.TransferHeaderId,
                        InventoryItemId = item.InventoryItemId,
                        ItemNumber = itemNumber,
                        ItemDescription = item.ItemDescTch,
                        PackingType = item.CatalogElemVal110,
                        RequestedTransactionUom = item.PrimaryUomCode, //沒交易單位資料 改放主單位
                        RequestedTransactionQuantity = priQty, //沒交易單位資料 改放主單位
                        RequestedPrimaryUom = item.PrimaryUomCode,
                        RequestedPrimaryQuantity = priQty,
                        RequestedSecondaryUom = item.SecondaryUomCode,
                        RequestedSecondaryQuantity = secQty,
                        RollReamQty = rollReamQty,
                        DataUpadteAuthority = dataUpadteAuthority,
                        DataWriteType = dataWriteType,
                        CreatedBy = createUser,
                        CreatedUserName = createUserName,
                        CreationDate = now,
                        LastUpdateBy = createUser,
                        LastUpdateUserName = createUserName,
                        LastUpdateDate = now
                    }, true);

                    trfDeatil = GetTrfDetail(trfHeader.TransferHeaderId, item.InventoryItemId);
                    if (trfDeatil == null) throw new Exception("找不到此料號明細資料");

                    //產生條碼清單
                    var generateBarcodesResult = GenerateBarcodes(inOrganizationId, inSubinventoryCode, (int)rollReamQty, createUser);
                    if (!generateBarcodesResult.Success) throw new Exception(generateBarcodesResult.Msg);

                    
                    decimal rollReamWtForPriUom = 0; //每棧令數主單位數量
                    decimal priRemainder = 0; //最後一板(餘數)主單位數量
                    if (item.CatalogElemVal070 == ItemCategory.Flat && rollReamQty > 1)
                    {
                        //平版 且 棧板數大於1 須算出每棧令數的主單位數量
                        var rollReamWtToPriUomConverResult = uomConversion.Convert(item.InventoryItemId, rollReamWt, item.SecondaryUomCode, item.PrimaryUomCode); //每棧令數 轉 主單位需求量
                        if (rollReamWtToPriUomConverResult.Success)
                        {
                            rollReamWtForPriUom = rollReamWtToPriUomConverResult.Data;
                        }
                        else
                        {
                            throw new Exception(rollReamWtToPriUomConverResult.Msg); //單位換算失敗 回傳錯誤
                        }

                        decimal secRemainder = (decimal)secQty % rollReamWt; //最後一板(餘數)次單位數量
                        if (secRemainder != 0) //判斷最後一板是否需要轉換主單位數量
                        {
                            //算出最後一板(餘數)的主單位數量
                            var secRemainderToPriUomConverResult = uomConversion.Convert(item.InventoryItemId, secRemainder, item.SecondaryUomCode, item.PrimaryUomCode); //最後一板(餘數) 轉 主單位需求量
                            if (secRemainderToPriUomConverResult.Success)
                            {
                                priRemainder = secRemainderToPriUomConverResult.Data;
                            }
                            else
                            {
                                throw new Exception(secRemainderToPriUomConverResult.Msg); //單位換算失敗 回傳錯誤
                            }
                        }
                    }
                       

                    if (transferType == TransferType.InBound)
                    {
                        //新增入庫揀貨明細
                        for (int i = 0; i < rollReamQty; i++)
                        {
                            decimal primaryQuantity = 0;
                            decimal? secondaryQuantity = null;
                            if (item.CatalogElemVal070 == ItemCategory.Flat)
                            {
                                if (rollReamQty == 1) //全部只有一板
                                {
                                    primaryQuantity = priQty; 
                                    secondaryQuantity = secQty;
                                }
                                else if (i + 1 == rollReamQty) //最後一板
                                {
                                    decimal secRemainder = (decimal)secQty % rollReamWt; //最後一板(餘數)次單位數量
                                    if (secRemainder == 0)
                                    {
                                        //沒有餘數時 取每板數量
                                        primaryQuantity = rollReamWtForPriUom;
                                        secondaryQuantity = rollReamWt;
                                    }
                                    else
                                    {
                                        primaryQuantity = priRemainder;
                                        secondaryQuantity = secRemainder;
                                    }                                   
                                }
                                else //每板
                                {
                                    primaryQuantity = rollReamWtForPriUom;  
                                    secondaryQuantity = rollReamWt;
                                }
                            }
                            else
                            {
                                //捲筒
                                primaryQuantity = priQty;
                                secondaryQuantity = null;
                            }


                            trfInboundPickedTRepositiory.Create(new TRF_INBOUND_PICKED_T
                            {
                                TransferDetailId = trfDeatil.TransferDetailId,
                                TransferHeaderId = trfHeader.TransferHeaderId,
                                InventoryItemId = item.InventoryItemId,
                                ItemNumber = itemNumber,
                                StockId = 0, //待問
                                Barcode = generateBarcodesResult.Data[i],
                                PrimaryUom = item.PrimaryUomCode,
                                PrimaryQuantity = primaryQuantity,
                                SecondaryUom = item.SecondaryUomCode,
                                SecondaryQuantity = secondaryQuantity,
                                LotNumber = lotNumber,
                                LotQuantity = null,
                                Note = "",
                                Status = InboundStatus.WaitPrint,
                                CreatedBy = createUser,
                                CreatedUserName = createUserName,
                                CreationDate = now,
                                LastUpdateBy = createUser,
                                LastUpdateUserName = createUserName,
                                LastUpdateDate = now

                            });
                           
                        }
                    }
                    else
                    {
                        //新增出貨揀貨明細
                        for (int i = 0; i < rollReamQty; i++)
                        {
                            decimal primaryQuantity = 0;
                            decimal? secondaryQuantity = null;
                            if (item.CatalogElemVal070 == ItemCategory.Flat)
                            {
                                if (rollReamQty == 1) //全部只有一板
                                {
                                    primaryQuantity = priQty;
                                    secondaryQuantity = secQty;
                                }
                                else if (i + 1 == rollReamQty) //最後一板
                                {
                                    decimal secRemainder = (decimal)secQty % rollReamWt; //最後一板(餘數)次單位數量
                                    if (secRemainder == 0)
                                    {
                                        //沒有餘數時 取每板數量
                                        primaryQuantity = rollReamWtForPriUom;
                                        secondaryQuantity = rollReamWt;
                                    }
                                    else
                                    {
                                        primaryQuantity = priRemainder;
                                        secondaryQuantity = secRemainder;
                                    }
                                }
                                else //每板
                                {
                                    primaryQuantity = rollReamWtForPriUom;
                                    secondaryQuantity = rollReamWt;
                                }
                            }
                            else
                            {
                                //捲筒
                                primaryQuantity = priQty;
                                secondaryQuantity = null;
                            }


                            trfOutboundPickedTRepositiory.Create(new TRF_OUTBOUND_PICKED_T
                            {
                                TransferDetailId = trfDeatil.TransferDetailId,
                                TransferHeaderId = trfHeader.TransferHeaderId,
                                InventoryItemId = item.InventoryItemId,
                                ItemNumber = itemNumber,
                                StockId = 0, //待問
                                Barcode = generateBarcodesResult.Data[i],
                                PrimaryUom = item.PrimaryUomCode,
                                PrimaryQuantity = primaryQuantity,
                                SecondaryUom = item.SecondaryUomCode,
                                SecondaryQuantity = secondaryQuantity,
                                LotNumber = lotNumber,
                                LotQuantity = null,
                                Note = "",
                                Status = InboundStatus.WaitPrint,
                                CreatedBy = createUser,
                                CreatedUserName = createUserName,
                                CreationDate = now,
                                LastUpdateBy = createUser,
                                LastUpdateUserName = createUserName,
                                LastUpdateDate = now
                            });
                           
                        }
                    }

                    trfInboundPickedTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultDataModel<TRF_HEADER_T>(true, "新增明細成功", trfHeader);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultDataModel<TRF_HEADER_T>(false, "新增明細失敗:" + ex.Message, null);
                }
            }
        }


    }

}