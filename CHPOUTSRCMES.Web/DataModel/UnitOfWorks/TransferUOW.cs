using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// 新增明細
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <param name="transferType"></param>
        /// <param name="itemNumber"></param>
        /// <param name="outOrganizationId"></param>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="outLocatorId"></param>
        /// <param name="inOrganizationId"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <param name="inLocatorId"></param>
        /// <param name="dataUpadteAuthority"></param>
        /// <param name="dataWriteType"></param>
        /// <param name="requestedQty"></param>
        /// <param name="rollReamWt"></param>
        /// <param name="lotNumber"></param>
        /// <param name="createUser"></param>
        /// <param name="createUserName"></param>
        /// <returns></returns>
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

                    //var trfDeatil = GetTrfDetail(trfHeader.TransferHeaderId, item.InventoryItemId);
                    //if (trfDeatil != null) throw new Exception("料號重複輸入");

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
                        RequestedTransactionUom = item.PrimaryUomCode, //沒交易單位資料 改放主單位 待修正
                        RequestedTransactionQuantity = priQty, //沒交易單位資料 改放主單位 待修正
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

                    var trfDeatil = GetTrfDetail(trfHeader.TransferHeaderId, item.InventoryItemId);
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
                                PalletStatus = PalletStatusCode.All,
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
                                PalletStatus = PalletStatusCode.All,
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
       

        public ResultModel WaitPrintToWaitInbound(List<long> transferPickedIdList, string userId, string userName)
        {
            var pickList = trfInboundPickedTRepositiory.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
            if (pickList == null || pickList.Count == 0) return new ResultModel(false, "找不到揀貨資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    foreach (TRF_INBOUND_PICKED_T pick in pickList)
                    {
                        if (pick.Status == InboundStatus.WaitPrint)
                        {
                            //入庫狀態為待列印才改待入庫
                            UpdateInboundPickStatus(pick, InboundStatus.WaitInbound, userId, userName, now);
                        }
                    }
                    trfInboundPickedTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "待列印轉待入庫成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "待列印轉待入庫失敗:" + ex.Message);
                }
            }
        }

        public ResultModel ChangeToAlreadyInBound(long transferHeaderId, string barcode, string userId, string userName)
        {
            var pick = trfInboundPickedTRepositiory.GetAll().FirstOrDefault(x => x.Barcode == barcode && x.TransferHeaderId == transferHeaderId);
            if (pick == null) return new ResultModel(false, "找不到揀貨資料");
            if (pick.Status == InboundStatus.WaitPrint) return new ResultModel(false, "請先列印條碼");
            if (pick.Status == InboundStatus.AlreadyInbound) return new ResultModel(false, "已經入庫");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    UpdateInboundPickStatus(pick, InboundStatus.AlreadyInbound, userId, userName, DateTime.Now);
                    trfInboundPickedTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "待列印轉待入庫成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "待列印轉待入庫失敗:" + ex.Message);
                }
            }

        }

        /// <summary>
        /// 更新入庫狀態
        /// </summary>
        /// <param name="pick"></param>
        /// <param name="inboundStatus"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="now"></param>
        public void UpdateInboundPickStatus(TRF_INBOUND_PICKED_T pick, string inboundStatus, string userId, string userName, DateTime now)
        {
            if (pick == null) throw new Exception("找不到揀貨資料");
            pick.Status = inboundStatus;
            pick.LastUpdateBy = userId;
            pick.LastUpdateUserName = userName;
            pick.LastUpdateDate = now;
            trfInboundPickedTRepositiory.Update(pick);
        }

        public ResultModel UpdateInboundPickNote(List<long> transferPickedIdList, string note, string userId, string userName)
        {
            var pickList = trfInboundPickedTRepositiory.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
            if (pickList == null || pickList.Count == 0) return new ResultModel(false, "找不到揀貨資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    foreach (TRF_INBOUND_PICKED_T pick in pickList)
                    {
                        pick.Note = note;
                        pick.LastUpdateBy = userId;
                        pick.LastUpdateUserName = userName;
                        pick.LastUpdateDate = now;
                        trfInboundPickedTRepositiory.Update(pick);
                    }

                    trfInboundPickedTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "更新入庫揀貨備註成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "更新入庫揀貨備註失敗:" + ex.Message);
                }
            }
        }


        public ResultModel DelInboundPickData(List<long> transferPickedIdList, string userId, string userName)
        {
            var pickList = trfInboundPickedTRepositiory.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
            if (pickList == null || pickList.Count == 0) return new ResultModel(false, "找不到揀貨資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    foreach (TRF_INBOUND_PICKED_T pick in pickList)
                    {
                        trfInboundPickedTRepositiory.Delete(pick, true);
                        var remainPick = trfInboundPickedTRepositiory.GetAll().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                        var detail = trfDetailTRepositiory.GetAll().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                        if (detail == null) throw new Exception("找不到明細資料");
                        if (remainPick == null)
                        {
                            //此Deail已沒有關聯的Pick則刪除此Detail
                            trfDetailTRepositiory.Delete(detail);
                        }
                        else
                        {
                            //更新Deatail數量
                            detail.RequestedTransactionQuantity -= pick.PrimaryQuantity; //暫時用主單位數量代替 待修改
                            detail.RequestedPrimaryQuantity -= pick.PrimaryQuantity;
                            detail.RequestedSecondaryQuantity -= pick.SecondaryQuantity;

                            detail.LastUpdateBy = userId;
                            detail.LastUpdateUserName = userName;
                            detail.LastUpdateDate = now;
                            trfDetailTRepositiory.Update(detail);
                        }
                    }

                    //trfInboundPickedTRepositiory.SaveChanges();
                    trfDetailTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "刪除入庫揀貨資料成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除入庫揀貨資料失敗:" + ex.Message);
                }


            }
        }

        #region 標籤
        public ResultDataModel<List<LabelModel>> GetInboundLabels(List<long> transferPickedIdList, string userName)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (transferPickedIdList == null || transferPickedIdList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                var pickDataList = trfInboundPickedTRepositiory.GetAll().AsNoTracking().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
                if (pickDataList == null || pickDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);

                var header = GetTrfHeader(pickDataList[0].TransferHeaderId);
                if (header == null) return new ResultDataModel<List<LabelModel>>(false, "找不到出貨編號資料", null);
                
                if (header.IsMes == IsMes.Yes)
                {
                    //對方為MES時料號資料從庫存取
                    foreach (TRF_INBOUND_PICKED_T data in pickDataList)
                    {
                        StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as BatchNo");

                        if (data.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                        {
                            //棧板狀態為拆板 在入庫不會遇到
                            throw new Exception("入庫棧板狀態不可為拆板");
                        }
                        else if (data.PalletStatus == PalletStatusCode.All) //判斷是否整版
                        {
                            if (data.SecondaryQuantity != null) //判斷是否為平版
                            {
                                //整板 平版(令包) 數量為揀貨的數量
                                cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                            }
                            else
                            {
                                //整板 捲筒
                                cmd.Append(@"
,s.PRIMARY_UOM_CODE as Unit
,FORMAT(p.PRIMARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                            }
                        }
                        else
                        {
                            //併板
                            if (data.SecondaryQuantity != null) //判斷是否為平版
                            {
                                //併板 平版(令包) 數量為庫存數量(被併板條碼原庫存) + 檢貨數量(待併板條碼數量總和)
                                cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(s.SECONDARY_AVAILABLE_QTY,'0.##########') + FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                            }
                            else
                            {
                                //併板 捲筒
                                return new ResultDataModel<List<LabelModel>>(false, "捲筒不能併板", null);
                            }
                        }


                        var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", data.Barcode)).ToList();
                        if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                        labelModelList.Add(labelModel[0]);
                    }
                    return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);
                }
                else
                {
                    //對方非MES時料號資料從主檔取 併板則從庫存取
                    foreach (TRF_INBOUND_PICKED_T data in pickDataList)
                    {
                        StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,i.ITEM_DESC_TCH as BarocdeName
,i.CATALOG_ELEM_VAL_020 as PapaerType
,i.CATALOG_ELEM_VAL_040 as BasicWeight
,i.CATALOG_ELEM_VAL_050 as Specification
,'' as BatchNo");

                        if (data.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                        {
                            //棧板狀態為拆板 在入庫不會遇到
                            throw new Exception("入庫棧板狀態不可為拆板");
                        }
                        else if (data.PalletStatus == PalletStatusCode.All) //判斷是否整版
                        {
                            if (data.SecondaryQuantity != null) //判斷是否為平版
                            {
                                //整板 平版(令包) 數量為揀貨的數量
                                cmd.Append(@"
,i.SECONDARY_UOM_CODE as Unit
,FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN ITEMS_T i ON p.INVENTORY_ITEM_ID = i.INVENTORY_ITEM_ID
WHERE p.BARCODE = @Barcode
");
                            }
                            else
                            {
                                //整板 捲筒
                                cmd.Append(@"
,i.PRIMARY_UOM_CODE as Unit
,FORMAT(p.PRIMARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN ITEMS_T i ON p.INVENTORY_ITEM_ID = i.INVENTORY_ITEM_ID
WHERE p.BARCODE = @Barcode
");
                            }
                        }
                        else
                        {
                            //併板
                            if (data.SecondaryQuantity != null) //判斷是否為平版
                            {
                                //併板 平版(令包) 數量為庫存數量(被併板條碼原庫存) + 檢貨數量(待併板條碼數量總和)
                                cmd.Clear();
                                cmd.Append(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as BatchNo
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(s.SECONDARY_AVAILABLE_QTY,'0.##########') + FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                            }
                            else
                            {
                                //併板 捲筒
                                return new ResultDataModel<List<LabelModel>>(false, "捲筒不能併板", null);
                            }
                        }


                        var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", data.Barcode)).ToList();
                        if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                        labelModelList.Add(labelModel[0]);
                    }
                    return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);
                }

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