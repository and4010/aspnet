using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Entity.Transfer;
using CHPOUTSRCMES.Web.DataModel.Entiy.Transfer;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Reporting.WebForms;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class TransferUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_HEADER_T> trfHeaderTRepository;
        private readonly IRepository<TRF_DETAIL_T> trfDetailTRepository;
        private readonly IRepository<TRF_DETAIL_HT> trfDetailHtRepository;
        private readonly IRepository<TRF_INBOUND_PICKED_T> trfInboundPickedTRepository;
        private readonly IRepository<TRF_INBOUND_PICKED_HT> trfInboundPickedHtRepository;
        private readonly IRepository<TRF_OUTBOUND_PICKED_T> trfOutboundPickedTRepository;
        private readonly IRepository<TRF_OUTBOUND_PICKED_HT> trfOutboundPickedHtRepository;
        private readonly IRepository<TRF_REASON_HEADER_T> trfReasonHeaderTRepository;
        private readonly IRepository<TRF_REASON_T> trfReasonTRepository;
        private readonly IRepository<TRF_REASON_HT> trfReasonHtRepository;
        private readonly IRepository<TRF_FILEINFO_T> trfFileInfoTRepository;
        private readonly IRepository<TRF_FILES_T> trfFilesTRepository;

        public TransferType transferType = new TransferType();
        public IDetail pickSatus = new PickStatus();
        public PalletStatusCode palletStatusCode = new PalletStatusCode();

        public TransferUOW(DbContext context)
           : base(context)
        {
            this.trfHeaderTRepository = new GenericRepository<TRF_HEADER_T>(this);
            this.trfDetailTRepository = new GenericRepository<TRF_DETAIL_T>(this);
            this.trfDetailHtRepository = new GenericRepository<TRF_DETAIL_HT>(this);
            this.trfInboundPickedTRepository = new GenericRepository<TRF_INBOUND_PICKED_T>(this);
            this.trfInboundPickedHtRepository = new GenericRepository<TRF_INBOUND_PICKED_HT>(this);
            this.trfOutboundPickedTRepository = new GenericRepository<TRF_OUTBOUND_PICKED_T>(this);
            this.trfOutboundPickedHtRepository = new GenericRepository<TRF_OUTBOUND_PICKED_HT>(this);
            this.trfReasonHeaderTRepository = new GenericRepository<TRF_REASON_HEADER_T>(this);
            this.trfReasonTRepository = new GenericRepository<TRF_REASON_T>(this);
            this.trfReasonHtRepository = new GenericRepository<TRF_REASON_HT>(this);
            this.trfFileInfoTRepository = new GenericRepository<TRF_FILEINFO_T>(this);
            this.trfFilesTRepository = new GenericRepository<TRF_FILES_T>(this);
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

        

        public class DataUpdateAuthority
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


       

       

        #region 儲位
        /// <summary>
        /// 取得儲位資料 給庫存異動用
        /// </summary>
        /// <param name="locatorId"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public LOCATOR_T GetLocatorForTransfer(long locatorId, DateTime now)
        {
            return locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == locatorId &&
          x.ControlFlag != ControlFlag.Deleted && (x.LocatorDisableDate == null || x.LocatorDisableDate > now));
        }

        #endregion

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

        public List<SelectListItem> GetOutBoundShipmentNumberDropDownList(long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            var transferTypeList = createDropDownList(DropDownListType.Add);
            transferTypeList.AddRange(GetShipmentNumberforOutbound(outOrganizationId, outSubinventoryCode, inOrganizationId, inSubinventoryCode));
            return transferTypeList;
        }

        public List<SelectListItem> GetShipmentNumberforOutbound(long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {

            string cmd = @"
SELECT CONVERT(varchar(10), TRANSFER_HEADER_ID) as Value,
SHIPMENT_NUMBER as Text
FROM TRF_HEADER_T
WHERE 
TRANSFER_TYPE = 'O'
AND SUBINVENTORY_CODE = @subCode AND ORGANIZATION_ID = @organId
AND TRANSFER_SUBINVENTORY_CODE = @trfSubCode AND TRANSFER_ORGANIZATION_ID = @trfOrganId";

            return this.Context.Database.SqlQuery<SelectListItem>(cmd,
                new SqlParameter("@subCode", outSubinventoryCode),
                new SqlParameter("@organId", outOrganizationId),
                new SqlParameter("@trfSubCode", inSubinventoryCode),
                new SqlParameter("@trfOrganId", inOrganizationId)).ToList();

        }

        #endregion


        public TRF_DETAIL_T GetTrfDetail(long transferHeaderId, long transferDetailId)
        {
            return trfDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransferHeaderId == transferHeaderId && x.TransferDetailId == transferDetailId);
        }

        public TRF_INBOUND_PICKED_T GetTrfInboundPickedDataFromBarcode(long transferHeaderId, string barcode)
        {
            return trfInboundPickedTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransferHeaderId == transferHeaderId && x.Barcode == barcode);
        }

        public TRF_HEADER_T GetTrfHeader(string shipmentNumber, string transferType)
        {
            return trfHeaderTRepository.GetAll().AsNoTracking().FirstOrDefault(x =>
            x.ShipmentNumber == shipmentNumber &&
            x.TransferType == transferType);
        }

        public TRF_HEADER_T GetTrfHeader(long transferHeaderId)
        {
            return trfHeaderTRepository.GetAll().AsNoTracking().FirstOrDefault(x =>
            x.TransferHeaderId == transferHeaderId);
        }

        public List<TRF_INBOUND_PICKED_T> GetTrfInboundPickedList(List<long> transferPickedIdList)
        {
            return trfInboundPickedTRepository.GetAll().AsNoTracking().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
        }

        public List<TRF_INBOUND_PICKED_T> GetTrfInboundPickedList2(List<long> transferPickedIdList)
        {
            return trfInboundPickedTRepository.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
        }

        /// <summary>
        /// 取得揀貨清單給入庫揀貨Datatables用
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <param name="numberStatus"></param>
        /// <returns></returns>
        public List<StockTransferBarcodeDT> GetTrfInboundPickedTList(long transferHeaderId, string numberStatus)
        {
            string cmd = "";
            if (numberStatus == NumberStatus.NotSaved)
            {
                cmd = @"
SELECT [TRANSFER_PICKED_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY p.[TRANSFER_PICKED_ID]) as SUB_ID
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
      ,p.PALLET_STATUS as PALLET_STATUS
  FROM [TRF_INBOUND_PICKED_T] p
  inner join TRF_DETAIL_T d on p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
  WHERE p.TRANSFER_HEADER_ID = @transferHeaderId";
            }
            else
            {
                cmd = @"
SELECT [TRANSFER_PICKED_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY p.[TRANSFER_PICKED_ID]) as SUB_ID
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
      ,p.PALLET_STATUS as PALLET_STATUS
  FROM [TRF_INBOUND_PICKED_HT] p
  inner join TRF_DETAIL_HT d on p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
  WHERE p.TRANSFER_HEADER_ID = @transferHeaderId";
            }
            return this.Context.Database.SqlQuery<StockTransferBarcodeDT>(cmd, new SqlParameter("@transferHeaderId", transferHeaderId)).ToList();
        }

        /// <summary>
        /// 取得明細清單給出庫明細Datatables用
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <param name="numberStatus"></param>
        /// <returns></returns>
        public List<StockTransferDT> GetTrfOutboundDetailTList(long transferHeaderId, string numberStatus)
        {
            string cmd = "";
            if (numberStatus == NumberStatus.NotSaved)
            {
                cmd = @"
SELECT d.[TRANSFER_DETAIL_ID] AS ID
      ,ROW_NUMBER() OVER(ORDER BY d.[TRANSFER_DETAIL_ID]) AS SUB_ID
      ,MIN(d.[ITEM_NUMBER]) AS [ITEM_NUMBER]
      ,MIN([PACKING_TYPE]) AS [PACKING_TYPE]
      ,MIN([REQUESTED_PRIMARY_UOM]) AS REQUESTED_QUANTITY_UOM
      ,MIN([REQUESTED_PRIMARY_QUANTITY]) AS REQUESTED_QUANTITY
	  ,SUM(ISNULL(p.PRIMARY_QUANTITY, 0))  AS PICKED_QUANTITY
      ,MIN([REQUESTED_SECONDARY_UOM]) AS REQUESTED_QUANTITY_UOM2
      ,MIN(ISNULL([REQUESTED_SECONDARY_QUANTITY], 0)) AS REQUESTED_QUANTITY2
	  ,SUM(ISNULL(p.SECONDARY_QUANTITY, 0))  AS PICKED_QUANTITY2
      ,MIN([ROLL_REAM_QTY]) AS [ROLL_REAM_QTY]
      ,MIN(CASE WHEN d.[ITEM_CATEGORY] = '平版' THEN '板' WHEN d.[ITEM_CATEGORY] = '捲筒' THEN '捲' ELSE '' END) AS ROLL_REAM_UOM
  FROM [TRF_DETAIL_T] d
  LEFT JOIN TRF_OUTBOUND_PICKED_T p on d.TRANSFER_HEADER_ID = p.TRANSFER_HEADER_ID AND d.TRANSFER_DETAIL_ID = p.TRANSFER_DETAIL_ID
  WHERE d.TRANSFER_HEADER_ID = @transferHeaderId
  GROUP BY d.TRANSFER_DETAIL_ID";
            }
            else
            {
                cmd = @"
SELECT d.[TRANSFER_DETAIL_ID] AS ID
      ,ROW_NUMBER() OVER(ORDER BY d.[TRANSFER_DETAIL_ID]) AS SUB_ID
      ,MIN(d.[ITEM_NUMBER]) AS [ITEM_NUMBER]
      ,MIN([PACKING_TYPE]) AS [PACKING_TYPE]
      ,MIN([REQUESTED_PRIMARY_UOM]) AS REQUESTED_QUANTITY_UOM
      ,MIN([REQUESTED_PRIMARY_QUANTITY]) AS REQUESTED_QUANTITY
	  ,SUM(ISNULL(p.PRIMARY_QUANTITY, 0))  AS PICKED_QUANTITY
      ,MIN([REQUESTED_SECONDARY_UOM]) AS REQUESTED_QUANTITY_UOM2
      ,MIN(ISNULL([REQUESTED_SECONDARY_QUANTITY], 0)) AS REQUESTED_QUANTITY2
	  ,SUM(ISNULL(p.SECONDARY_QUANTITY, 0))  AS PICKED_QUANTITY2
      ,MIN([ROLL_REAM_QTY]) AS [ROLL_REAM_QTY]
      ,MIN(CASE WHEN d.[ITEM_CATEGORY] = '平版' THEN '板' WHEN d.[ITEM_CATEGORY] = '捲筒' THEN '捲' ELSE '' END) AS ROLL_REAM_UOM
  FROM [TRF_DETAIL_HT] d
  LEFT JOIN TRF_OUTBOUND_PICKED_HT p on d.TRANSFER_HEADER_ID = p.TRANSFER_HEADER_ID AND d.TRANSFER_DETAIL_ID = p.TRANSFER_DETAIL_ID
  WHERE d.TRANSFER_HEADER_ID = @transferHeaderId
  GROUP BY d.TRANSFER_DETAIL_ID";
            }
            return this.Context.Database.SqlQuery<StockTransferDT>(cmd, new SqlParameter("@transferHeaderId", transferHeaderId)).ToList();
        }

        public List<StockTransferBarcodeDT> GetTrfOutboundPickedTList(long transferHeaderId, string numberStatus)
        {
            string cmd = "";
            if (numberStatus == NumberStatus.NotSaved)
            {
                cmd = @"
SELECT [TRANSFER_PICKED_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY p.[TRANSFER_PICKED_ID]) as SUB_ID
      ,p.[ITEM_NUMBER]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,ISNULL([SECONDARY_QUANTITY],0) as SECONDARY_QUANTITY
      ,[SECONDARY_UOM]
      ,[NOTE] as REMARK
	  ,d.PACKING_TYPE as PACKING_TYPE
      ,p.PALLET_STATUS as PALLET_STATUS
  FROM [TRF_OUTBOUND_PICKED_T] p
  inner join TRF_DETAIL_T d on p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
  WHERE p.TRANSFER_HEADER_ID = @transferHeaderId";
            }
            else
            {
                cmd = @"
SELECT [TRANSFER_PICKED_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY p.[TRANSFER_PICKED_ID]) as SUB_ID
      ,p.[ITEM_NUMBER]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,ISNULL([SECONDARY_QUANTITY],0) as SECONDARY_QUANTITY
      ,[SECONDARY_UOM]
      ,[NOTE] as REMARK
	  ,d.PACKING_TYPE as PACKING_TYPE
      ,p.PALLET_STATUS as PALLET_STATUS
  FROM [TRF_OUTBOUND_PICKED_HT] p
  inner join TRF_DETAIL_HT d on p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
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
        public ResultDataModel<string> GetShipmentNumber(string outSubinventoryCode, string inSubinventoryCode, DateTime now, string userId)
        {
            return GenerateShipmentNo(outSubinventoryCode, inSubinventoryCode, now, 3, userId);
            //Random rnd = new Random();
            //List<int> randomList = Enumerable.Range(1, 999)
            //    .OrderBy(x => rnd.Next()).Take(100).ToList();
            //return "(" + outSubinventoryCode + "-" + inSubinventoryCode + ")" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", randomList[0].ToString());
        }

        public ResultDataModel<int> InobundCheckShipmentNumberExist(string shipmentNumber)
        {
            try
            {
                var trfHeader = GetTrfHeader(shipmentNumber, TransferType.InBound);
                if (trfHeader == null)
                {
                    return new ResultDataModel<int>(true, "沒有此出貨編號資料", 0);
                }
                else
                {
                    return new ResultDataModel<int>(true, "有此出貨編號資料", 1);
                }
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<int>(false, "檢查出貨編號失敗:" + ex.Message, -1);
            }           
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
        /// <param name="rollReamWt">每件令數 捲筒為0</param>
        /// <param name="lotNumber"></param>
        /// <param name="createUser"></param>
        /// <param name="createUserName"></param>
        /// <returns></returns>
        public ResultDataModel<TRF_HEADER_T> InboundCreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId, string dataUpadteAuthority, string dataWriteType,
            decimal requestedQty, decimal rollReamWt, string lotNumber, string createUser, string createUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var result = InboundCreateDetailNoTransaction(shipmentNumber, transferType, itemNumber, outOrganizationId,
             outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId, dataUpadteAuthority, dataWriteType,
             requestedQty, rollReamWt, lotNumber, createUser, createUserName, "");

                    txn.Commit();
                    return new ResultDataModel<TRF_HEADER_T>(true, "新增明細成功", result.Data);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultDataModel<TRF_HEADER_T>(false, "新增明細失敗:" + ex.Message, null);
                }
            }
        }

        /// <summary>
        /// 新增明細 要加 Transaction
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
        public ResultDataModel<TRF_HEADER_T> InboundCreateDetailNoTransaction(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
          string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId, string dataUpadteAuthority, string dataWriteType,
          decimal requestedQty, decimal rollReamWt, string lotNumber, string createUser, string createUserName, string containerNo)
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
                if (itemNumberOrganizationIdList.Where(x => x == inOrganizationId).ToList().Count == 0) throw new Exception("入庫組織沒有此料號");
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
                var outLocator = GetLocatorForTransfer((long)outLocatorId, now);
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
                var inLocator = GetLocatorForTransfer((long)inLocatorId, now);
                if (inLocator == null) throw new Exception("找不到出庫儲位資料");
                transferLocatorCode = inLocator.LocatorSegments;
                inLocatorSegment3 = inLocator.Segment3;
            }

            var transactionType = GetTransactionType(transactionTypeId);
            if (transactionType == null) throw new Exception("找不到庫存交易類別資料");

            if (shipmentNumber == DropDownListTypeValue.Add)
            {
                var result = GetShipmentNumber(outSubinventoryCode, inSubinventoryCode, now, createUser);
                if (!result.Success) throw new Exception(result.Msg);
                shipmentNumber = result.Data;
            }

            var trfHeader = GetTrfHeader(shipmentNumber, TransferType.InBound);
            if (trfHeader == null)
            {
                trfHeader = new TRF_HEADER_T
                {
                    OrgId = outOrganization.OrgUnitId,
                    OrganizationId = outOrganizationId,
                    OrganizationCode = outOrganization.OrganizationCode,
                    ShipmentNumber = shipmentNumber,
                    TransferCatalog = transferCatalog,
                    TransferType = TransferType.InBound,
                    NumberStatus = NumberStatus.NotSaved,
                    IsMes = IsMes.No,
                    SubinventoryCode = outSubinventoryCode,
                    LocatorId = outLocatorId,
                    LocatorCode = locatorCode,
                    TransactionDate = now,
                    TransactionTypeId = transactionTypeId,
                    TransactionTypeName = transactionType.TransactionTypeName,
                    TransferOrgId = inOrganization.OrgUnitId,
                    TransferOrganizationId = inOrganizationId,
                    TransferOrganizationCode = inOrganization.OrganizationCode,
                    TransferSubinventoryCode = inSubinventoryCode,
                    TransferLocatorId = inLocatorId,
                    TransferLocatorCode = transferLocatorCode,
                    CreatedBy = createUser,
                    CreatedUserName = createUserName,
                    CreationDate = now,
                    LastUpdateBy = null,
                    LastUpdateUserName = null,
                    LastUpdateDate = null
                };
                trfHeaderTRepository.Create(trfHeader, true);
            }



            //if (shipmentNumber == DropDownListTypeValue.Add)
            //{
            //    var result = GetShipmentNumber(outSubinventoryCode, inSubinventoryCode, now, createUser);
            //    if (!result.Success) throw new Exception(result.Msg);
            //    shipmentNumber = result.Data;

            //    trfHeaderTRepository.Create(new TRF_HEADER_T
            //    {
            //        OrgId = outOrganization.OrgUnitId,
            //        OrganizationId = outOrganizationId,
            //        OrganizationCode = outOrganization.OrganizationCode,
            //        ShipmentNumber = shipmentNumber,
            //        TransferCatalog = transferCatalog,
            //        TransferType = TransferType.InBound,
            //        NumberStatus = NumberStatus.NotSaved,
            //        IsMes = IsMes.No,
            //        SubinventoryCode = outSubinventoryCode,
            //        LocatorId = outLocatorId,
            //        LocatorCode = locatorCode,
            //        TransactionDate = now,
            //        TransactionTypeId = transactionTypeId,
            //        TransactionTypeName = transactionType.TransactionTypeName,
            //        TransferOrgId = inOrganization.OrgUnitId,
            //        TransferOrganizationId = inOrganizationId,
            //        TransferOrganizationCode = inOrganization.OrganizationCode,
            //        TransferSubinventoryCode = inSubinventoryCode,
            //        TransferLocatorId = inLocatorId,
            //        TransferLocatorCode = transferLocatorCode,
            //        CreatedBy = createUser,
            //        CreatedUserName = createUserName,
            //        CreationDate = now,
            //        LastUpdateBy = null,
            //        LastUpdateUserName = null,
            //        LastUpdateDate = null
            //    }, true);
            //}



            //var trfHeader = GetTrfHeader(shipmentNumber, TransferType.InBound);
            //if (trfHeader == null) throw new Exception("找不到出貨編號資料");

            string headeroutOutLocatorSegment3 = "";
            if (trfHeader.LocatorId != null)
            {
                var headerOutLocator = GetLocatorForTransfer((long)trfHeader.LocatorId, now);
                if (headerOutLocator == null) throw new Exception("找不到出庫儲位資料");
                headeroutOutLocatorSegment3 = headerOutLocator.Segment3;
            }

            string headeroutInLocatorSegment3 = "";
            if (trfHeader.TransferLocatorId != null)
            {
                var headerInLocator = GetLocatorForTransfer((long)trfHeader.TransferLocatorId, now);
                if (headerInLocator == null) throw new Exception("找不到入庫儲位資料");
                headeroutInLocatorSegment3 = headerInLocator.Segment3;
            }

            if (trfHeader.OrganizationId != outOrganizationId) throw new Exception("出庫組織比對錯誤，請選擇此出庫倉庫" + trfHeader.SubinventoryCode);
            if (trfHeader.SubinventoryCode != outSubinventoryCode) throw new Exception("出庫倉庫比對錯誤，請選擇此出庫倉庫" + trfHeader.SubinventoryCode);
            if (trfHeader.LocatorId != outLocatorId) throw new Exception("出庫儲位比對錯誤，請選擇此出庫儲位" + headeroutOutLocatorSegment3);
            if (trfHeader.TransferOrganizationId != inOrganizationId) throw new Exception("入庫組織比對錯誤，請選擇此入庫倉庫" + trfHeader.TransferSubinventoryCode);
            if (trfHeader.TransferSubinventoryCode != inSubinventoryCode) throw new Exception("入庫倉庫比對錯誤，請選擇此入庫倉庫" + trfHeader.TransferSubinventoryCode);
            if (trfHeader.TransferLocatorId != inLocatorId) throw new Exception("入庫儲位比對錯誤，請選擇此入庫儲位" + headeroutInLocatorSegment3);

            //檢查捲號是否重複
            if (item.CatalogElemVal070 == ItemCategory.Roll)
            {
                var pick = trfInboundPickedTRepository.GetAll().FirstOrDefault(x => x.LotNumber == lotNumber);
                if (pick != null) throw new Exception("捲號不可重複");
                var stock = stockTRepository.GetAll().FirstOrDefault(x => x.LotNumber == lotNumber); //待確認 捲號庫存搜尋方式
                if (stock != null) throw new Exception("此捲號" + lotNumber + "已入庫");
            }

            //var trfDeatil = GetTrfDetail(trfHeader.TransferHeaderId, item.InventoryItemId);
            //if (trfDeatil != null) throw new Exception("料號重複輸入");

            //計算捲數/棧板數
            decimal rollReamQty = 0;
            if (item.CatalogElemVal070 == ItemCategory.Roll)
            {
                rollReamQty = 1; //捲筒固定為1
            }
            else if (item.CatalogElemVal070 == ItemCategory.Flat)
            {
                rollReamQty = Math.Ceiling(requestedQty / rollReamWt); //無條件進位 算出棧板數
            }
            else
            {
                throw new Exception("無法識別貨品類別");
            }


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

            TRF_DETAIL_T trfDeatil = new TRF_DETAIL_T
            {
                TransferHeaderId = trfHeader.TransferHeaderId,
                InventoryItemId = item.InventoryItemId,
                ItemNumber = itemNumber,
                ItemDescription = item.ItemDescTch,
                PackingType = item.CatalogElemVal110,
                ItemCategory = item.CatalogElemVal070,
                RequestedTransactionUom = item.PrimaryUomCode, //沒交易單位資料 改放主單位 待修正
                RequestedTransactionQuantity = priQty, //沒交易單位資料 改放主單位 待修正
                RequestedPrimaryUom = item.PrimaryUomCode,
                RequestedPrimaryQuantity = priQty,
                RequestedSecondaryUom = item.SecondaryUomCode,
                RequestedSecondaryQuantity = secQty,
                RollReamQty = rollReamQty,
                RollReamWt = rollReamWt,
                DataUpadteAuthority = dataUpadteAuthority,
                DataWriteType = dataWriteType,
                CreatedBy = createUser,
                CreatedUserName = createUserName,
                CreationDate = now,
                LastUpdateBy = null,
                LastUpdateUserName = null,
                LastUpdateDate = null
            };

            trfDetailTRepository.Create(trfDeatil, true);

            //var trfDeatil = GetTrfDetail(trfHeader.TransferHeaderId, item.InventoryItemId);
            //if (trfDeatil == null) throw new Exception("找不到此料號明細資料");

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


                trfInboundPickedTRepository.Create(new TRF_INBOUND_PICKED_T
                {
                    TransferDetailId = trfDeatil.TransferDetailId,
                    TransferHeaderId = trfHeader.TransferHeaderId,
                    InventoryItemId = item.InventoryItemId,
                    ItemNumber = itemNumber,
                    StockId = 0,
                    Barcode = generateBarcodesResult.Data[i],
                    PrimaryUom = item.PrimaryUomCode,
                    PrimaryQuantity = primaryQuantity,
                    SecondaryUom = item.SecondaryUomCode,
                    SecondaryQuantity = secondaryQuantity,
                    LotNumber = lotNumber,
                    LotQuantity = item.CatalogElemVal070 == ItemCategory.Roll ? primaryQuantity : default(decimal?),  //待確認理論重
                    Note = null,
                    Status = InboundStatus.WaitPrint,
                    PalletStatus = PalletStatusCode.All,
                    CreatedBy = createUser,
                    CreatedUserName = createUserName,
                    CreationDate = now,
                    LastUpdateBy = null,
                    LastUpdateUserName = null,
                    LastUpdateDate = null,
                    ContainerNo = containerNo
                });

            }

            trfInboundPickedTRepository.SaveChanges();
            return new ResultDataModel<TRF_HEADER_T>(true, "新增明細成功", trfHeader);
        }


        public ResultDataModel<TRF_HEADER_T> OutboundCreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
        string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId, string dataUpadteAuthority, string dataWriteType,
        decimal requestedQty, decimal rollReamQty, string createUser, string createUserName)
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
                        if (itemNumberOrganizationIdList.Where(x => x == outOrganizationId).ToList().Count == 0) throw new Exception("出庫組織沒有此料號");
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
                        var outLocator = GetLocatorForTransfer((long)outLocatorId, now);
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
                        var inLocator = GetLocatorForTransfer((long)inLocatorId, now);
                        if (inLocator == null) throw new Exception("找不到出庫儲位資料");
                        transferLocatorCode = inLocator.LocatorSegments;
                        inLocatorSegment3 = inLocator.Segment3;
                    }

                    var transactionType = GetTransactionType(transactionTypeId);
                    if (transactionType == null) throw new Exception("找不到庫存交易類別資料");

                    if (shipmentNumber == DropDownListTypeValue.Add)
                    {
                        var result = GetShipmentNumber(outSubinventoryCode, inSubinventoryCode, now, createUser);
                        if (!result.Success) throw new Exception(result.Msg);
                        shipmentNumber = result.Data;

                        trfHeaderTRepository.Create(new TRF_HEADER_T
                        {
                            OrgId = outOrganization.OrgUnitId,
                            OrganizationId = outOrganizationId,
                            OrganizationCode = outOrganization.OrganizationCode,
                            ShipmentNumber = shipmentNumber,
                            TransferCatalog = transferCatalog,
                            TransferType = TransferType.Outbound,
                            NumberStatus = NumberStatus.NotSaved,
                            IsMes = IsMes.Yes,
                            SubinventoryCode = outSubinventoryCode,
                            LocatorId = outLocatorId,
                            LocatorCode = locatorCode,
                            TransactionDate = now,
                            TransactionTypeId = transactionTypeId,
                            TransactionTypeName = transactionType.TransactionTypeName,
                            TransferOrgId = inOrganization.OrgUnitId,
                            TransferOrganizationId = inOrganizationId,
                            TransferOrganizationCode = inOrganization.OrganizationCode,
                            TransferSubinventoryCode = inSubinventoryCode,
                            TransferLocatorId = inLocatorId,
                            TransferLocatorCode = transferLocatorCode,
                            CreatedBy = createUser,
                            CreatedUserName = createUserName,
                            CreationDate = now,
                            LastUpdateBy = null,
                            LastUpdateUserName = null,
                            LastUpdateDate = null
                        }, true);
                    }

                    var trfHeader = GetTrfHeader(shipmentNumber, TransferType.Outbound);
                    if (trfHeader == null) throw new Exception("找不到出貨編號資料");

                    string headeroutOutLocatorSegment3 = "";
                    if (trfHeader.LocatorId != null)
                    {
                        var headerOutLocator = GetLocatorForTransfer((long)trfHeader.LocatorId, now);
                        if (headerOutLocator == null) throw new Exception("找不到出庫儲位資料");
                        headeroutOutLocatorSegment3 = headerOutLocator.Segment3;
                    }

                    string headeroutInLocatorSegment3 = "";
                    if (trfHeader.TransferLocatorId != null)
                    {
                        var headerInLocator = GetLocatorForTransfer((long)trfHeader.TransferLocatorId, now);
                        if (headerInLocator == null) throw new Exception("找不到入庫儲位資料");
                        headeroutInLocatorSegment3 = headerInLocator.Segment3;
                    }

                    if (trfHeader.OrganizationId != outOrganizationId) throw new Exception("出庫組織比對錯誤，請選擇此出庫倉庫" + trfHeader.SubinventoryCode);
                    if (trfHeader.SubinventoryCode != outSubinventoryCode) throw new Exception("出庫倉庫比對錯誤，請選擇此出庫倉庫" + trfHeader.SubinventoryCode);
                    if (trfHeader.LocatorId != outLocatorId) throw new Exception("出庫儲位比對錯誤，請選擇此出庫儲位" + headeroutOutLocatorSegment3);
                    if (trfHeader.TransferOrganizationId != inOrganizationId) throw new Exception("入庫組織比對錯誤，請選擇此入庫倉庫" + trfHeader.TransferSubinventoryCode);
                    if (trfHeader.TransferSubinventoryCode != inSubinventoryCode) throw new Exception("入庫倉庫比對錯誤，請選擇此入庫倉庫" + trfHeader.TransferSubinventoryCode);
                    if (trfHeader.TransferLocatorId != inLocatorId) throw new Exception("入庫儲位比對錯誤，請選擇此入庫儲位" + headeroutInLocatorSegment3);

                    //計算每件令數
                    decimal rollReamWt = 0;
                    if (item.CatalogElemVal070 == ItemCategory.Roll)
                    {
                        rollReamWt = 0; //捲筒每件令數為0
                    }
                    else if (item.CatalogElemVal070 == ItemCategory.Flat)
                    {
                        //rollReamWt = Math.Ceiling(requestedQty / rollReamQty); //每件令數 = 總令數 除以 棧板數 無條件進位 待確認是否正確
                        var yszmpckq = GetYszmpckq(outOrganization.OrganizationId, outOrganization.OrganizationCode, outSubinventoryCode, item.CatalogElemVal020, item.CatalogElemVal040);
                        if (yszmpckq == null) throw new Exception("找不到令重包數資料");
                        rollReamWt = yszmpckq.PiecesQty;
                        //var yszmpckq = GetYszmpckq(outOrganization.OrganizationId, outOrganization.OrganizationCode, outSubinventoryCode, item.CatalogElemVal020);
                        //if (yszmpckq == null)
                        //{
                        //    rollReamWt = Math.Ceiling(requestedQty / rollReamQty);
                        //}
                        //else
                        //{
                        //    rollReamWt = yszmpckq.PiecesQty;
                        //}
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

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

                    TRF_DETAIL_T trfDeatil = new TRF_DETAIL_T
                    {
                        TransferHeaderId = trfHeader.TransferHeaderId,
                        InventoryItemId = item.InventoryItemId,
                        ItemNumber = itemNumber,
                        ItemDescription = item.ItemDescTch,
                        PackingType = item.CatalogElemVal110,
                        ItemCategory = item.CatalogElemVal070,
                        RequestedTransactionUom = item.PrimaryUomCode, //沒交易單位資料 改放主單位 待修正
                        RequestedTransactionQuantity = priQty, //沒交易單位資料 改放主單位 待修正
                        RequestedPrimaryUom = item.PrimaryUomCode,
                        RequestedPrimaryQuantity = priQty,
                        RequestedSecondaryUom = item.SecondaryUomCode,
                        RequestedSecondaryQuantity = secQty,
                        RollReamQty = rollReamQty,
                        RollReamWt = rollReamWt,
                        DataUpadteAuthority = dataUpadteAuthority,
                        DataWriteType = dataWriteType,
                        CreatedBy = createUser,
                        CreatedUserName = createUserName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    };

                    trfDetailTRepository.Create(trfDeatil, true);
                    trfDeatil.OutboundTransferDetailId = trfDeatil.TransferDetailId;
                    trfDetailTRepository.Update(trfDeatil, true);

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

        public ResultModel OutboundCreatePick(long transferHeaderId, long transferDetailId, string barcode,
        decimal reamQty, string createUser, string createUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var header = GetTrfHeader(transferHeaderId);
                    if (header == null) throw new Exception("找不到出貨編號資料");

                    var detail = GetTrfDetail(transferHeaderId, transferDetailId);
                    if (detail == null) throw new Exception("找不到明細資料");

                    //檢查庫存
                    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.Barcode == barcode && x.LocatorId == header.LocatorId);
                    if (stock == null) throw new Exception("找不到庫存資料");
                    if (stock.StatusCode != StockStatusCode.InStock) throw new Exception("不在庫");
                    if (detail.InventoryItemId != stock.InventoryItemId) throw new Exception("料號不正確");
                    if (stock.ItemCategory == ItemCategory.Flat)
                    {
                        if (stock.SecondaryAvailableQty == null || stock.SecondaryAvailableQty == 0) throw new Exception("庫存次單位數量為0");
                    }
                    else if (stock.ItemCategory == ItemCategory.Roll)
                    {
                        if (stock.PrimaryAvailableQty == 0) throw new Exception("庫存主單位數量為0");
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

                    //檢查是否重複輸入條碼
                    if (stock.PackingType != PackingType.Ream)
                    {
                        //包裝方式不為令包時 要檢查條碼重複輸入
                        var pick = trfOutboundPickedTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.Barcode == barcode);
                        if (pick != null)
                        {

                            var tempHeader = GetTrfHeader(pick.TransferHeaderId);
                            if (tempHeader == null)
                            {
                                throw new Exception("找不到出貨編號資料");
                            }
                            else
                            {
                                throw new Exception("此條碼已存在出貨編號" + tempHeader.ShipmentNumber + "裡");
                            }
                        }
                    }


                    //判斷棧板狀態
                    string note = "";
                    string palletStatus = "";
                    if (stock.PackingType == PackingType.Ream)
                    {
                        if (reamQty == stock.SecondaryAvailableQty)
                        {
                            palletStatus = PalletStatusCode.All;
                        }
                        //if (reamQty == detail.RollReamWt)
                        //{
                        //    palletStatus = PalletStatusCode.All;
                        //}
                        else if (reamQty > stock.SecondaryAvailableQty)
                        {
                            throw new Exception("庫存量不足，僅剩:" + stock.SecondaryAvailableQty + " " + stock.SecondaryUomCode);
                        }
                        else
                        {
                            if (reamQty == 0)
                            {
                                throw new Exception("令包數不可為0");
                            }
                            palletStatus = PalletStatusCode.Split;
                            note = palletStatusCode.GetDesc(PalletStatusCode.Split);
                        }
                    }
                    else
                    {
                        //包裝方式非令包 為整板
                        palletStatus = PalletStatusCode.All;
                    }

                    //計算數量
                    decimal? pryQty = 0;
                    decimal? secQty = null;
                    if (stock.ItemCategory == ItemCategory.Flat)
                    {
                        if (palletStatus == PalletStatusCode.Split)
                        {
                            //拆板須轉換主單位
                            secQty = reamQty;
                            var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, reamQty, stock.SecondaryUomCode, stock.PrimaryUomCode); //次單位數量轉主單位數量
                            if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                            pryQty = uomConversionResult.Data;
                        }
                        else if (palletStatus == PalletStatusCode.All)
                        {
                            secQty = stock.SecondaryAvailableQty;
                            pryQty = stock.PrimaryAvailableQty;
                        }
                        else
                        {
                            throw new Exception("出庫不會有併板");
                        }
                    }
                    else if (stock.ItemCategory == ItemCategory.Roll)
                    {
                        pryQty = stock.PrimaryAvailableQty;
                    }
                    else
                    {
                        throw new Exception("無法識別貨品類別");
                    }

                    var now = DateTime.Now;
                    trfOutboundPickedTRepository.Create(new TRF_OUTBOUND_PICKED_T
                    {
                        TransferDetailId = transferDetailId,
                        TransferHeaderId = transferHeaderId,
                        InventoryItemId = stock.InventoryItemId,
                        ItemNumber = stock.ItemNumber,
                        StockId = stock.StockId,
                        Barcode = stock.Barcode,
                        PrimaryUom = stock.PrimaryUomCode,
                        PrimaryQuantity = pryQty == null ? 0 : (decimal)pryQty,
                        SecondaryUom = stock.SecondaryUomCode,
                        SecondaryQuantity = secQty,
                        LotNumber = stock.LotNumber,
                        LotQuantity = stock.LotQuantity,
                        Note = note,
                        Status = null,
                        PalletStatus = palletStatus,
                        CreatedBy = createUser,
                        CreatedUserName = createUserName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    }, true);

                    //扣除庫存
                    pryQty = -1 * pryQty;
                    secQty = -1 * secQty;
                    STK_TXN_T stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode, header.TransferLocatorId, CategoryCode.TransferOutbound, ActionCode.Picked, header.ShipmentNumber);
                    var updaeStockResult = UpdateStock(stock, stkTxnT, ref pryQty, ref secQty, pickSatus, PickStatus.Picked, createUser, now, true);
                    if (!updaeStockResult.Success) throw new Exception(updaeStockResult.Msg);

                    stockTRepository.SaveChanges();
                    stkTxnTRepository.SaveChanges();

                    txn.Commit();
                    return new ResultModel(true, "新增揀貨成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "新增揀貨失敗:" + ex.Message);
                }
            }


        }

        public ResultModel WaitPrintToWaitInbound(List<long> transferPickedIdList, string userId, string userName)
        {
            var pickList = trfInboundPickedTRepository.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
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
                    trfInboundPickedTRepository.SaveChanges();
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
            var pick = trfInboundPickedTRepository.GetAll().FirstOrDefault(x => x.Barcode == barcode && x.TransferHeaderId == transferHeaderId);
            if (pick == null) return new ResultModel(false, "找不到揀貨資料");
            if (pick.Status == InboundStatus.WaitPrint) return new ResultModel(false, "請先列印條碼");
            if (pick.Status == InboundStatus.AlreadyInbound) return new ResultModel(false, "已經入庫");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    UpdateInboundPickStatus(pick, InboundStatus.AlreadyInbound, userId, userName, DateTime.Now);
                    trfInboundPickedTRepository.SaveChanges();
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

        //public ResultModel ChangeToAlreadyInBoundForShipmnetNumber(string shipmnetNumber, string barcode, string userId, string userName)
        //{
        //    var pick = trfInboundPickedTRepository.GetAll().FirstOrDefault(x => x.Barcode == barcode && x.sh == shipmnetNumber);
        //    if (pick == null) return new ResultModel(false, "找不到揀貨資料");
        //    if (pick.Status == InboundStatus.WaitPrint) return new ResultModel(false, "請先列印條碼");
        //    if (pick.Status == InboundStatus.AlreadyInbound) return new ResultModel(false, "已經入庫");

        //    using (var txn = this.Context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            UpdateInboundPickStatus(pick, InboundStatus.AlreadyInbound, userId, userName, DateTime.Now);
        //            trfInboundPickedTRepository.SaveChanges();
        //            txn.Commit();
        //            return new ResultModel(true, "待列印轉待入庫成功");
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(LogUtilities.BuildExceptionMessage(ex));
        //            txn.Rollback();
        //            return new ResultModel(false, "待列印轉待入庫失敗:" + ex.Message);
        //        }
        //    }

        //}

        /// <summary>
        /// 更新入庫狀態 使用時要加Transaction
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
            trfInboundPickedTRepository.Update(pick);
        }

        public ResultModel UpdateInboundPickNote(List<long> transferPickedIdList, string note, string userId, string userName)
        {
            var pickList = trfInboundPickedTRepository.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
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
                        trfInboundPickedTRepository.Update(pick);
                    }

                    trfInboundPickedTRepository.SaveChanges();
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
            var pickList = trfInboundPickedTRepository.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
            if (pickList == null || pickList.Count == 0) return new ResultModel(false, "找不到揀貨資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DelInboundPickData(pickList, userId, userName);
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

        /// <summary>
        /// 刪除入庫Pick資料 要加Transaction
        /// </summary>
        /// <param name="pickList"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public void DelInboundPickData(List<TRF_INBOUND_PICKED_T> pickList, string userId, string userName)
        {
            var now = DateTime.Now;
            List<long> transferDetailIdList = pickList.GroupBy(x => x.TransferDetailId).Select(x => x.Key).ToList();

            List<TRF_DETAIL_T> trfDetailList = trfDetailTRepository.GetAll().Where(x => transferDetailIdList.Contains(x.TransferDetailId)).ToList();
            //List<TRF_DETAIL_T> trfDetailList = trfDetailTRepository.GetAll().Join(
            //    trfInboundPickedTRepository.GetAll(),
            //    d => d.TransferDetailId,
            //    p => p.TransferDetailId,
            //    (d, p) => d).Select(x => x).ToList();
            if (trfDetailList == null || trfDetailList.Count == 0) throw new Exception("找不到明細資料");
            foreach (TRF_INBOUND_PICKED_T pick in pickList)
            {
                trfInboundPickedTRepository.Delete(pick);
                //var remainPick = trfInboundPickedTRepository.GetAll().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                //var detail = trfDetailTRepository.GetAll().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                //if (detail == null) throw new Exception("找不到明細資料");
                //if (remainPick == null)
                //{
                //    //此Deail已沒有關聯的Pick則刪除此Detail
                //    trfDetailTRepository.Delete(detail);
                //}
                //else
                //{
                //    //更新Deatail數量  如果每筆轉換數量效率差則 待修改
                //    //detail.RequestedTransactionQuantity -= pick.PrimaryQuantity; //暫時用主單位數量代替 待修改
                //    //detail.RequestedPrimaryQuantity -= pick.PrimaryQuantity;
                //    //detail.RequestedSecondaryQuantity -= pick.SecondaryQuantity;
                //    detail.RequestedSecondaryQuantity -= pick.SecondaryQuantity;
                //    var uomConversionResult = uomConversion.Convert(pick.InventoryItemId, (decimal)detail.RequestedSecondaryQuantity, pick.SecondaryUom, pick.PrimaryUom); //副單位需求量 轉 主單位需求量
                //    if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                //    detail.RequestedTransactionQuantity = uomConversionResult.Data; //暫時用主單位數量代替 待修改
                //    detail.RequestedPrimaryQuantity = uomConversionResult.Data;

                //    detail.LastUpdateBy = userId;
                //    detail.LastUpdateUserName = userName;
                //    detail.LastUpdateDate = now;
                //    trfDetailTRepository.Update(detail);
                //}
            }
            trfInboundPickedTRepository.SaveChanges();
            foreach (TRF_DETAIL_T detail in trfDetailList)
            {
                var tempPickList = trfInboundPickedTRepository.GetAll().Where(x => x.TransferDetailId == detail.TransferDetailId).ToList();
                if (tempPickList == null || tempPickList.Count == 0)
                {
                    trfDetailTRepository.Delete(detail);
                }
                else
                {
                    if (detail.ItemCategory == ItemCategory.Roll)
                    {
                        decimal pryQty = 0;
                        foreach (TRF_INBOUND_PICKED_T pick in tempPickList)
                        {
                            pryQty += pick.PrimaryQuantity;
                        }
                        detail.RequestedTransactionQuantity = pryQty; //暫時用主單位數量代替 待修改
                        detail.RequestedPrimaryQuantity = pryQty;
                        detail.RequestedSecondaryQuantity = null;
                        detail.LastUpdateBy = userId;
                        detail.LastUpdateUserName = userName;
                        detail.LastUpdateDate = now;
                        trfDetailTRepository.Update(detail);
                    }
                    else
                    {
                        decimal secQty = 0;
                        foreach (TRF_INBOUND_PICKED_T pick in tempPickList)
                        {
                            secQty += (decimal)pick.SecondaryQuantity;
                        }
                        var uomConversionResult = uomConversion.Convert(tempPickList[0].InventoryItemId, secQty, tempPickList[0].SecondaryUom, tempPickList[0].PrimaryUom); //副單位需求量 轉 主單位需求量
                        if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);

                        detail.RequestedTransactionQuantity = uomConversionResult.Data; //暫時用主單位數量代替 待修改
                        detail.RequestedPrimaryQuantity = uomConversionResult.Data;
                        detail.RequestedSecondaryQuantity = secQty;
                        detail.LastUpdateBy = userId;
                        detail.LastUpdateUserName = userName;
                        detail.LastUpdateDate = now;
                        trfDetailTRepository.Update(detail);
                    }

                }
            }

            trfDetailTRepository.SaveChanges();
            //return new ResultModel(true, "刪除入庫揀貨資料成功");
        }

        public ResultModel UpdateOutboundPickNote(List<long> transferPickedIdList, string note, string userId, string userName)
        {
            var pickList = trfOutboundPickedTRepository.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
            if (pickList == null || pickList.Count == 0) return new ResultModel(false, "找不到揀貨資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    foreach (TRF_OUTBOUND_PICKED_T pick in pickList)
                    {
                        pick.Note = note;
                        pick.LastUpdateBy = userId;
                        pick.LastUpdateUserName = userName;
                        pick.LastUpdateDate = now;
                        trfOutboundPickedTRepository.Update(pick);
                    }

                    trfOutboundPickedTRepository.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "更新出庫揀貨備註成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "更新出庫揀貨備註失敗:" + ex.Message);
                }
            }
        }

        public ResultModel DelOutboundPickedData(List<long> transferPickedIdList, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    var pickList = trfOutboundPickedTRepository.GetAll().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
                    if (pickList == null || pickList.Count == 0) throw new Exception("找不到揀貨資料");
                    if (pickList.Count != transferPickedIdList.Count) throw new Exception("找不到部分揀貨資料");
                    var header = GetTrfHeader(pickList[0].TransferHeaderId);
                    if (header == null) throw new Exception("找不到出貨編號資料");

                    foreach (TRF_OUTBOUND_PICKED_T pick in pickList)
                    {
                        var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                        if (stock == null) throw new Exception("找不到庫存資料");
                        STK_TXN_T stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode, header.TransferLocatorId, CategoryCode.TransferOutbound, ActionCode.Deleted, header.ShipmentNumber);
                        decimal? pryQty = pick.PrimaryQuantity;
                        decimal? secQty = pick.SecondaryQuantity;
                        var updaeStockResult = UpdateStock(stock, stkTxnT, ref pryQty, ref secQty, pickSatus, PickStatus.Deleted, userId, now, true);
                        if (!updaeStockResult.Success) throw new Exception(updaeStockResult.Msg);
                        trfOutboundPickedTRepository.Delete(pick);
                    }
                    stockTRepository.SaveChanges();
                    stkTxnTRepository.SaveChanges();
                    trfOutboundPickedTRepository.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "刪除出庫揀貨資料成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除出庫揀貨資料失敗:" + ex.Message);
                }

            }
        }

        public ResultModel DelOutboundDetailData(List<long> transferDetailIdList, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    var detailList = trfDetailTRepository.GetAll().Where(x => transferDetailIdList.Contains(x.TransferDetailId)).ToList();
                    if (detailList == null || detailList.Count == 0) throw new Exception("找不到明細資料");
                    if (detailList.Count != transferDetailIdList.Count) throw new Exception("找不到部分明細資料");
                    var header = GetTrfHeader(detailList[0].TransferHeaderId);
                    if (header == null) throw new Exception("找不到出貨編號資料");

                    foreach (TRF_DETAIL_T detail in detailList)
                    {
                        var pickList = trfOutboundPickedTRepository.GetAll().Where(x => x.TransferDetailId == detail.TransferDetailId).ToList();
                        if (pickList != null && pickList.Count > 0)
                        {
                            foreach (TRF_OUTBOUND_PICKED_T pick in pickList)
                            {
                                var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                                if (stock == null) throw new Exception("找不到庫存資料");
                                STK_TXN_T stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode, header.TransferLocatorId, CategoryCode.TransferOutbound, ActionCode.Deleted, header.ShipmentNumber);
                                decimal? priQty = pick.PrimaryQuantity;
                                decimal? secQty = pick.SecondaryQuantity;
                                var updaeStockResult = UpdateStock(stock, stkTxnT, ref priQty, ref secQty, pickSatus, PickStatus.Deleted, userId, now, true);
                                if (!updaeStockResult.Success) throw new Exception(updaeStockResult.Msg);

                                trfOutboundPickedTRepository.Delete(pick);
                            }
                        }
                        trfDetailTRepository.Delete(detail);
                    }
                    stockTRepository.SaveChanges();
                    stkTxnTRepository.SaveChanges();
                    trfOutboundPickedTRepository.SaveChanges();
                    trfDetailTRepository.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "刪除出庫明細資料成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除出庫明細資料失敗:" + ex.Message);
                }
            }

        }


        public ResultDataModel<TRF_HEADER_T> InboundImportExcel(string shipmentNumber, string transferType, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId, List<InboundImportExcelModel> excelList,
            string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    if (excelList == null || excelList.Count == 0) throw new Exception("找不到Excel資料");
                    if (shipmentNumber != DropDownListTypeValue.Add)
                    {
                        //不是新增出貨編號時，要移除舊資料
                        var trfheader = GetTrfHeader(shipmentNumber, transferType);
                        if (trfheader == null) throw new Exception("找不到出貨編號資料");
                        //刪除入庫揀貨資料
                        string cmd = @"
  DELETE FROM [TRF_INBOUND_PICKED_T]
  WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", trfheader.TransferHeaderId)) < 0) //這裡用 < 0 表示原本就沒資料也可以通過刪除檢查 
                        {
                            throw new Exception("刪除入庫揀貨資料失敗");
                        }

                        //刪除入庫明細資料
                        cmd = @"
  DELETE FROM [TRF_DETAIL_T]
  WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", trfheader.TransferHeaderId)) < 0)
                        {
                            throw new Exception("刪除入庫明細資料失敗");
                        }

                        //                      //刪除入庫檔頭資料
                        //                      cmd = @"
                        //DELETE FROM [TRF_HEADER_T]
                        //WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                        //                      if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", trfheader.TransferHeaderId)) < 0)
                        //                      {
                        //                          throw new Exception("刪除入庫檔頭資料失敗");
                        //                      }
                    }

                    ResultDataModel<TRF_HEADER_T> result = new ResultDataModel<TRF_HEADER_T>(false, "找不到Excel資料", null);
                    foreach (InboundImportExcelModel excel in excelList)
                    {
                        result = InboundCreateDetailNoTransaction(shipmentNumber, transferType, excel.ItemNumber, outOrganizationId,
                 outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId,
                 TransferUOW.DataUpdateAuthority.Permit, TransferUOW.DataWriteType.ExcelImport, excel.Qty, excel.RollReamWt, excel.LotNumber, userId, userName, excel.ContainerNo);
                        if (result.Success)
                        {
                            if (shipmentNumber == DropDownListTypeValue.Add) shipmentNumber = result.Data.ShipmentNumber;  //如果是新增編號,則在第一筆新增明細成功後更新出貨編號
                        }
                        else
                        {
                            throw new Exception(result.Msg);
                        }
                    }
                    txn.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultDataModel<TRF_HEADER_T>(false, "入庫匯入Excel失敗:" + ex.Message, null);
                }
            }
        }

        /// <summary>
        /// 入庫存檔
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="checkStockStatus"></param>
        /// <returns></returns>
        public ResultModel InBoundSaveTransfer(long transferHeaderId, string userId, string userName, bool checkStockStatus)
        {

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var pickList = trfInboundPickedTRepository.GetAll().Where(x => x.TransferHeaderId == transferHeaderId).ToList();
                    if (pickList == null || pickList.Count == 0) throw new Exception("找不到揀貨資料");

                    if (checkStockStatus)
                    {
                        var notAlreadyInboundList = pickList.Where(x => x.Status != InboundStatus.AlreadyInbound).ToList();
                        if (notAlreadyInboundList != null && notAlreadyInboundList.Count > 0) throw new Exception("尚有條碼未入庫");
                    }
                    DateTime now = DateTime.Now;

                    var header = trfHeaderTRepository.GetAll().FirstOrDefault(x => x.TransferHeaderId == transferHeaderId);
                    if (header == null) throw new Exception("找不到出貨編號資料");
                    header.TransactionDate = now;
                    header.NumberStatus = NumberStatus.Saved;
                    header.LastUpdateBy = userId;
                    header.LastUpdateUserName = userName;
                    header.LastUpdateDate = now;
                    trfHeaderTRepository.SaveChanges();



                    if (header.IsMes == IsMes.Yes)
                    {
                        //對方是MES
                        foreach (TRF_INBOUND_PICKED_T pick in pickList)
                        {
                            if (pick.StockId == 0)
                            {
                                //為在入庫時另外新增的資料
                                if (pick.PalletStatus == PalletStatusCode.All)
                                {
                                    //整板 新增庫存
                                    var detail = trfDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                                    if (detail == null) throw new Exception("找不到明細資料");
                                    var item = GetItemNumber(pick.InventoryItemId);
                                    if (item == null) throw new Exception("找不到料號資料");

                                    STOCK_T stock = new STOCK_T();
                                    stock.OrganizationId = (long)header.TransferOrganizationId;
                                    stock.OrganizationCode = header.TransferOrganizationCode;
                                    stock.SubinventoryCode = header.TransferSubinventoryCode;
                                    stock.LocatorId = header.TransferLocatorId;
                                    stock.LocatorSegments = header.TransferLocatorCode;
                                    stock.InventoryItemId = detail.InventoryItemId;
                                    stock.ItemNumber = detail.ItemNumber;
                                    stock.ItemDescription = detail.ItemDescription;
                                    stock.ItemCategory = item.CatalogElemVal070;
                                    stock.PaperType = item.CatalogElemVal020;
                                    stock.BasicWeight = item.CatalogElemVal040;
                                    stock.ReamWeight = item.CatalogElemVal060;
                                    if (item.CatalogElemVal070 == ItemCategory.Flat)
                                    {
                                        stock.RollReamWt = detail.RollReamWt;
                                    }
                                    else
                                    {
                                        stock.RollReamWt = 0;
                                    }
                                    stock.Specification = item.CatalogElemVal050;
                                    stock.PackingType = detail.PackingType;
                                    stock.OspBatchNo = "";
                                    stock.LotNumber = pick.LotNumber;
                                    stock.LotQuantity = pick.LotQuantity;
                                    stock.Barcode = pick.Barcode;
                                    stock.PrimaryUomCode = pick.PrimaryUom;
                                    stock.PrimaryTransactionQty = pick.PrimaryQuantity;
                                    stock.PrimaryAvailableQty = pick.PrimaryQuantity;
                                    stock.PrimaryLockedQty = null;
                                    stock.SecondaryUomCode = pick.SecondaryUom;
                                    stock.SecondaryTransactionQty = pick.SecondaryQuantity;
                                    stock.SecondaryAvailableQty = pick.SecondaryQuantity;
                                    stock.SecondaryLockedQty = null;
                                    stock.ReasonCode = null;
                                    stock.ReasonDesc = null;
                                    stock.Note = pick.Note;
                                    stock.StatusCode = StockStatusCode.InStock;
                                    stock.CreatedBy = userId;
                                    stock.CreationDate = now;
                                    stock.LastUpdateBy = null;
                                    stock.LastUpdateDate = null;
                                    stockTRepository.Create(stock, true);

                                    //產生異動紀錄
                                    var stkTxnT = CreateStockRecord(stock, header.OrganizationId, header.OrganizationCode, header.SubinventoryCode, header.LocatorId,
                                        header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                    header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber,
                                    0, stock.PrimaryAvailableQty, stock.PrimaryAvailableQty, 0, stock.SecondaryAvailableQty, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                                    stkTxnTRepository.Create(stkTxnT);
                                }
                                else if (pick.PalletStatus == PalletStatusCode.Merge)
                                {
                                    //併版 更新庫存主次數量
                                    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                                    if (stock == null) throw new Exception("找不到庫存資料");

                                    //產生異動紀錄
                                    var stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                    header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber);

                                    stock.OrganizationId = (long)header.TransferOrganizationId;
                                    stock.OrganizationCode = header.TransferOrganizationCode;
                                    stock.SubinventoryCode = header.TransferSubinventoryCode;
                                    stock.LocatorId = header.TransferLocatorId;
                                    stock.LocatorSegments = header.TransferLocatorCode;
                                    if (string.IsNullOrEmpty(stock.Note))
                                    {
                                        stock.Note = pick.Note;
                                    }
                                    else
                                    {
                                        stock.Note = stock.Note + "," + pick.Note;
                                    }
                                    stkTxnT.Note = stock.Note;
                                    stock.StatusCode = StockStatusCode.InStock;
                                    stkTxnT.StatusCode = stock.StatusCode;
                                    stock.LastUpdateBy = userId;
                                    stock.LastUpdateDate = now;

                                    stkTxnT.PryBefQty = stock.PrimaryAvailableQty;
                                    stkTxnT.SecBefQty = stock.SecondaryAvailableQty;
                                    stkTxnT.PryChgQty = pick.PrimaryQuantity;
                                    stkTxnT.SecChgQty = pick.SecondaryQuantity;

                                    stock.SecondaryAvailableQty += pick.SecondaryQuantity;
                                    stkTxnT.SecAftQty = stock.SecondaryAvailableQty;
                                    //stock.SecondaryTransactionQty = stock.SecondaryAvailableQty;
                                    var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)stock.SecondaryAvailableQty, stock.SecondaryUomCode, stock.PrimaryUomCode); //副單位需求量 轉 主單位需求量
                                    if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                                    stock.PrimaryAvailableQty = uomConversionResult.Data;
                                    stkTxnT.PryAftQty = stock.PrimaryAvailableQty;
                                    //stock.PrimaryTransactionQty = stock.PrimaryAvailableQty;
                                    stock.RollReamWt = stock.RollReamWt;
                                    stkTxnT.CreatedBy = userId;
                                    stkTxnT.CreationDate = now;
                                    stkTxnT.LastUpdateBy = null;
                                    stkTxnT.LastUpdateDate = null;
                                    stockTRepository.Update(stock, true);
                                    stkTxnTRepository.Create(stkTxnT);
                                }
                                else
                                {
                                    //非MES入庫沒有拆板
                                }
                            }
                            else
                            {
                                //為MES出庫轉入庫的資料
                                if (pick.PalletStatus == PalletStatusCode.All)
                                {
                                    if (string.IsNullOrEmpty(pick.SplitFromBarcode))
                                    {
                                        //整版 非從拆板轉來 更新庫存位置
                                        var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                                        if (stock == null) throw new Exception("找不到庫存資料");

                                        //var detail = trfDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                                        //if (detail == null) throw new Exception("找不到明細資料");

                                        //產生異動紀錄
                                        stock.OrganizationId = (long)header.TransferOrganizationId;
                                        stock.OrganizationCode = header.TransferOrganizationCode;
                                        stock.SubinventoryCode = header.TransferSubinventoryCode;
                                        stock.LocatorId = header.TransferLocatorId;
                                        stock.LocatorSegments = header.TransferLocatorCode;
                                        if (string.IsNullOrEmpty(stock.Note))
                                        {
                                            stock.Note = pick.Note;
                                        }
                                        else
                                        {
                                            stock.Note = stock.Note + "," + pick.Note;
                                        }
                                        stock.StatusCode = StockStatusCode.InStock;
                                        stock.LastUpdateBy = userId;
                                        stock.LastUpdateDate = now;
                                        stockTRepository.Update(stock, true);

                                        var stkTxnT = CreateStockRecord(stock, header.OrganizationId, header.OrganizationCode, header.SubinventoryCode, header.LocatorId,
                                            header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                        header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber,
                                        stock.PrimaryAvailableQty, 0, stock.PrimaryAvailableQty, stock.SecondaryAvailableQty, 0, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                                        stkTxnTRepository.Create(stkTxnT);
                                    }
                                    else
                                    {
                                        //整版 從拆板轉來 必為平板(令包) 新增庫存
                                        var sourceStock = GetStock(pick.SplitFromBarcode);
                                        if (sourceStock == null) throw new Exception("找不到庫存資料");

                                        STOCK_T stock = new STOCK_T();
                                        stock.OrganizationId = (long)header.TransferOrganizationId;
                                        stock.OrganizationCode = header.TransferOrganizationCode;
                                        stock.SubinventoryCode = header.TransferSubinventoryCode;
                                        stock.LocatorId = header.TransferLocatorId;
                                        stock.LocatorSegments = header.TransferLocatorCode;
                                        stock.InventoryItemId = sourceStock.InventoryItemId;
                                        stock.ItemNumber = sourceStock.ItemNumber;
                                        stock.ItemDescription = sourceStock.ItemDescription;
                                        stock.ItemCategory = sourceStock.ItemCategory;
                                        stock.PaperType = sourceStock.PaperType;
                                        stock.BasicWeight = sourceStock.BasicWeight;
                                        stock.ReamWeight = sourceStock.ReamWeight;
                                        stock.RollReamWt = sourceStock.RollReamWt;
                                        stock.Specification = sourceStock.Specification;
                                        stock.PackingType = sourceStock.PackingType;
                                        stock.OspBatchNo = sourceStock.OspBatchNo;
                                        stock.LotNumber = sourceStock.LotNumber;
                                        stock.LotQuantity = sourceStock.LotQuantity;
                                        stock.Barcode = pick.Barcode;
                                        stock.PrimaryUomCode = pick.PrimaryUom;
                                        stock.PrimaryTransactionQty = pick.PrimaryQuantity;
                                        stock.PrimaryAvailableQty = pick.PrimaryQuantity;
                                        stock.PrimaryLockedQty = null;
                                        stock.SecondaryUomCode = pick.SecondaryUom;
                                        stock.SecondaryTransactionQty = pick.SecondaryQuantity;
                                        stock.SecondaryAvailableQty = pick.SecondaryQuantity;
                                        stock.SecondaryLockedQty = null;
                                        stock.ReasonCode = null;
                                        stock.ReasonDesc = null;
                                        stock.Note = pick.Note;
                                        stock.StatusCode = StockStatusCode.InStock;
                                        stock.CreatedBy = userId;
                                        stock.CreationDate = now;
                                        stock.LastUpdateBy = null;
                                        stock.LastUpdateDate = null;
                                        stockTRepository.Create(stock, true);

                                        //產生異動紀錄
                                        var stkTxnT = CreateStockRecord(stock, header.OrganizationId, header.OrganizationCode, header.SubinventoryCode, header.LocatorId,
                                            header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                        header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber,
                                        0, stock.PrimaryAvailableQty, stock.PrimaryAvailableQty, 0, stock.SecondaryAvailableQty, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                                        stkTxnTRepository.Create(stkTxnT);

                                    }
                                }
                                else if (pick.PalletStatus == PalletStatusCode.Merge)
                                {
                                    //併版 更新主次數量
                                    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                                    if (stock == null) throw new Exception("找不到庫存資料");

                                    //產生異動紀錄
                                    var stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                    header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber);

                                    stock.OrganizationId = (long)header.TransferOrganizationId;
                                    stock.OrganizationCode = header.TransferOrganizationCode;
                                    stock.SubinventoryCode = header.TransferSubinventoryCode;
                                    stock.LocatorId = header.TransferLocatorId;
                                    stock.LocatorSegments = header.TransferLocatorCode;
                                    if (string.IsNullOrEmpty(stock.Note))
                                    {
                                        stock.Note = pick.Note;
                                    }
                                    else
                                    {
                                        stock.Note = stock.Note + "," + pick.Note;
                                    }
                                    stkTxnT.Note = stock.Note;
                                    stock.StatusCode = StockStatusCode.InStock;
                                    stkTxnT.StatusCode = stock.StatusCode;
                                    stock.LastUpdateBy = userId;
                                    stock.LastUpdateDate = now;

                                    stkTxnT.PryBefQty = stock.PrimaryAvailableQty;
                                    stkTxnT.SecBefQty = stock.SecondaryAvailableQty;
                                    stkTxnT.PryChgQty = pick.PrimaryQuantity;
                                    stkTxnT.SecChgQty = pick.SecondaryQuantity;

                                    stock.SecondaryAvailableQty += pick.SecondaryQuantity;
                                    stkTxnT.SecAftQty = stock.SecondaryAvailableQty;
                                    //stock.SecondaryTransactionQty = stock.SecondaryAvailableQty;
                                    var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)stock.SecondaryAvailableQty, stock.SecondaryUomCode, stock.PrimaryUomCode); //副單位需求量 轉 主單位需求量
                                    if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                                    stock.PrimaryAvailableQty = uomConversionResult.Data;
                                    stkTxnT.PryAftQty = stock.PrimaryAvailableQty;
                                    //stock.PrimaryTransactionQty = stock.PrimaryAvailableQty;
                                    //stock.RollReamWt = stock.RollReamWt;
                                    stkTxnT.CreatedBy = userId;
                                    stkTxnT.CreationDate = now;
                                    stkTxnT.LastUpdateBy = null;
                                    stkTxnT.LastUpdateDate = null;
                                    stockTRepository.Update(stock, true);
                                    stkTxnTRepository.Create(stkTxnT);
                                }
                                else
                                {
                                    //MES入庫沒有拆板 已於出庫存檔時 把拆板轉為整板

                                }
                            }

                        }
                    }
                    else
                    {
                        //對方非MES 分 整板和併板
                        foreach (TRF_INBOUND_PICKED_T pick in pickList)
                        {
                            if (pick.PalletStatus == PalletStatusCode.All)
                            {
                                //整板 新增庫存
                                var detail = trfDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransferDetailId == pick.TransferDetailId);
                                if (detail == null) throw new Exception("找不到明細資料");
                                var item = GetItemNumber(pick.InventoryItemId);
                                if (item == null) throw new Exception("找不到料號資料");

                                STOCK_T stock = new STOCK_T();
                                stock.OrganizationId = (long)header.TransferOrganizationId;
                                stock.OrganizationCode = header.TransferOrganizationCode;
                                stock.SubinventoryCode = header.TransferSubinventoryCode;
                                stock.LocatorId = header.TransferLocatorId;
                                stock.LocatorSegments = header.TransferLocatorCode;
                                stock.InventoryItemId = detail.InventoryItemId;
                                stock.ItemNumber = detail.ItemNumber;
                                stock.ItemDescription = detail.ItemDescription;
                                stock.ItemCategory = item.CatalogElemVal070;
                                stock.PaperType = item.CatalogElemVal020;
                                stock.BasicWeight = item.CatalogElemVal040;
                                stock.ReamWeight = item.CatalogElemVal060;
                                if (item.CatalogElemVal070 == ItemCategory.Flat)
                                {
                                    stock.RollReamWt = detail.RollReamWt;
                                }
                                else
                                {
                                    stock.RollReamWt = 0;
                                }
                                stock.Specification = item.CatalogElemVal050;
                                stock.PackingType = detail.PackingType;
                                stock.OspBatchNo = "";
                                stock.LotNumber = pick.LotNumber;
                                stock.LotQuantity = pick.LotQuantity;
                                stock.Barcode = pick.Barcode;
                                stock.PrimaryUomCode = pick.PrimaryUom;
                                stock.PrimaryTransactionQty = pick.PrimaryQuantity;
                                stock.PrimaryAvailableQty = pick.PrimaryQuantity;
                                stock.PrimaryLockedQty = null;
                                stock.SecondaryUomCode = pick.SecondaryUom;
                                stock.SecondaryTransactionQty = pick.SecondaryQuantity;
                                stock.SecondaryAvailableQty = pick.SecondaryQuantity;
                                stock.SecondaryLockedQty = null;
                                stock.ReasonCode = null;
                                stock.ReasonDesc = null;
                                stock.Note = pick.Note;
                                stock.StatusCode = StockStatusCode.InStock;
                                stock.ContainerNo = pick.ContainerNo;
                                stock.CreatedBy = userId;
                                stock.CreationDate = now;
                                stock.LastUpdateBy = null;
                                stock.LastUpdateDate = null;
                                stockTRepository.Create(stock, true);

                                //產生異動紀錄
                                var stkTxnT = CreateStockRecord(stock, header.OrganizationId, header.OrganizationCode, header.SubinventoryCode, header.LocatorId,
                                    header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber,
                                0, stock.PrimaryAvailableQty, stock.PrimaryAvailableQty, 0, stock.SecondaryAvailableQty, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                                stkTxnTRepository.Create(stkTxnT);
                            }
                            else if (pick.PalletStatus == PalletStatusCode.Merge)
                            {
                                //併版 更新庫存主次數量
                                var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                                if (stock == null) throw new Exception("找不到庫存資料");

                                //產生異動紀錄
                                var stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                                header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.StockTransfer, header.ShipmentNumber);

                                stock.OrganizationId = (long)header.TransferOrganizationId;
                                stock.OrganizationCode = header.TransferOrganizationCode;
                                stock.SubinventoryCode = header.TransferSubinventoryCode;
                                stock.LocatorId = header.TransferLocatorId;
                                stock.LocatorSegments = header.TransferLocatorCode;
                                if (string.IsNullOrEmpty(stock.Note))
                                {
                                    stock.Note = pick.Note;
                                }
                                else
                                {
                                    stock.Note = stock.Note + "," + pick.Note;
                                }
                                stkTxnT.Note = stock.Note;
                                stock.StatusCode = StockStatusCode.InStock;
                                stkTxnT.StatusCode = stock.StatusCode;
                                stock.LastUpdateBy = userId;
                                stock.LastUpdateDate = now;

                                stkTxnT.PryBefQty = stock.PrimaryAvailableQty;
                                stkTxnT.SecBefQty = stock.SecondaryAvailableQty;
                                stkTxnT.PryChgQty = pick.PrimaryQuantity;
                                stkTxnT.SecChgQty = pick.SecondaryQuantity;

                                stock.SecondaryAvailableQty += pick.SecondaryQuantity;
                                stkTxnT.SecAftQty = stock.SecondaryAvailableQty;
                                //stock.SecondaryTransactionQty = stock.SecondaryAvailableQty;
                                var uomConversionResult = uomConversion.Convert(stock.InventoryItemId, (decimal)stock.SecondaryAvailableQty, stock.SecondaryUomCode, stock.PrimaryUomCode); //副單位需求量 轉 主單位需求量
                                if (!uomConversionResult.Success) throw new Exception(uomConversionResult.Msg);
                                stock.PrimaryAvailableQty = uomConversionResult.Data;
                                stkTxnT.PryAftQty = stock.PrimaryAvailableQty;
                                //stock.PrimaryTransactionQty = stock.PrimaryAvailableQty;
                                stock.RollReamWt = stock.RollReamWt;
                                stkTxnT.CreatedBy = userId;
                                stkTxnT.CreationDate = now;
                                stkTxnT.LastUpdateBy = null;
                                stkTxnT.LastUpdateDate = null;
                                stockTRepository.Update(stock, true);
                                stkTxnTRepository.Create(stkTxnT);
                            }
                            else
                            {
                                //非MES入庫沒有拆板
                            }
                        }

                        //    var stockList = trfDetailTRepository.GetAll().
                        //    Where(x => x.TransferHeaderId == transferHeaderId).
                        //    Join(
                        //itemsTRepository.GetAll(),
                        //d => d.InventoryItemId,
                        //i => i.InventoryItemId,
                        //(d, i) => new
                        //{
                        //    TransferDetailId = d.TransferDetailId,
                        //    InventoryItemId = d.InventoryItemId,
                        //    ItemNumber = d.ItemNumber,
                        //    ItemDescription = d.ItemDescription,
                        //    ItemCategory = i.CatalogElemVal070,
                        //    PaperType = i.CatalogElemVal020,
                        //    BasicWeight = i.CatalogElemVal040,
                        //    ReamWeight = i.CatalogElemVal060,
                        //    RollReamWt = d.RollReamQty,
                        //    Specification = i.CatalogElemVal050,
                        //    PackingType = d.PackingType,
                        //}).Join(
                        //trfInboundPickedTRepository.GetAll(),
                        //di => di.TransferDetailId,
                        //p => p.TransferDetailId,
                        //(di, p) => new
                        //{
                        //    InventoryItemId = di.InventoryItemId,
                        //    ItemNumber = di.ItemNumber,
                        //    ItemDescription = di.ItemDescription,
                        //    ItemCategory = di.ItemCategory,
                        //    PaperType = di.PaperType,
                        //    BasicWeight = di.BasicWeight,
                        //    ReamWeight = di.ReamWeight,
                        //    RollReamWt = di.RollReamWt,
                        //    Specification = di.Specification,
                        //    PackingType = di.PackingType,
                        //    LotNumber = p.LotNumber,
                        //    Barcode = p.Barcode,
                        //    PrimaryUomCode = p.PrimaryUom,
                        //    PrimaryTransactionQty = p.PrimaryQuantity,
                        //    PrimaryAvailableQty = p.PrimaryQuantity,
                        //    SecondaryUomCode = p.SecondaryUom,
                        //    SecondaryTransactionQty = p.SecondaryQuantity,
                        //    SecondaryAvailableQty = p.SecondaryQuantity,
                        //    Note = p.Note,
                        //}
                        //).ToList().Select(x => new STOCK_T
                        //{
                        //    OrganizationId = (long)header.TransferOrganizationId,
                        //    OrganizationCode = header.TransferOrganizationCode,
                        //    SubinventoryCode = header.TransferSubinventoryCode,
                        //    LocatorId = header.TransferLocatorId,
                        //    LocatorSegments = header.TransferLocatorCode,
                        //    InventoryItemId = x.InventoryItemId,
                        //    ItemNumber = x.ItemNumber,
                        //    ItemDescription = x.ItemDescription,
                        //    ItemCategory = x.ItemCategory,
                        //    PaperType = x.PaperType,
                        //    BasicWeight = x.BasicWeight,
                        //    ReamWeight = x.ReamWeight,
                        //    RollReamWt = x.RollReamWt,
                        //    Specification = x.Specification,
                        //    PackingType = x.PackingType,
                        //    OspBatchNo = "",
                        //    LotNumber = x.LotNumber,
                        //    Barcode = x.Barcode,
                        //    PrimaryUomCode = x.PrimaryUomCode,
                        //    PrimaryTransactionQty = x.PrimaryTransactionQty,
                        //    PrimaryAvailableQty = x.PrimaryAvailableQty,
                        //    PrimaryLockedQty = 0,
                        //    SecondaryUomCode = x.SecondaryUomCode,
                        //    SecondaryTransactionQty = x.SecondaryTransactionQty,
                        //    SecondaryAvailableQty = x.SecondaryAvailableQty,
                        //    SecondaryLockedQty = 0,
                        //    ReasonCode = "",
                        //    ReasonDesc = "",
                        //    Note = x.Note,
                        //    StatusCode = StockStatusCode.InStock,
                        //    CreatedBy = userId,
                        //    CreationDate = now,
                        //    LastUpdateBy = "",
                        //    LastUpdateDate = null
                        //}).ToList();

                        //if (stockList == null || stockList.Count == 0) throw new Exception("無法產生庫存資料");

                        //foreach (STOCK_T stock in stockList)
                        //{
                        //    //產生庫存資料
                        //    stockTRepository.Create(stock, true);
                        //    //產生庫存異動紀錄


                        //    //更新pick的STOCK_ID
                        //    var pick = trfInboundPickedTRepository.GetAll().FirstOrDefault(x => x.Barcode == stock.Barcode && x.TransferHeaderId == transferHeaderId);
                        //    if (pick == null) throw new Exception("找不到揀貨資料");
                        //    pick.StockId = stock.StockId;
                        //    trfInboundPickedTRepository.Update(pick);
                        //}
                        //trfInboundPickedTRepository.SaveChanges();
                    }

                    //複製入庫明細資料到入庫歷史明細
                    string cmd = @"
INSERT INTO TRF_DETAIL_HT
(
      [TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[ITEM_CATEGORY]
      ,[PACKING_TYPE]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[ROLL_REAM_QTY]
      ,[ROLL_REAM_WT]
      ,[DATA_UPADTE_AUTHORITY]
      ,[DATA_WRITE_TYPE]
      ,[OUTBOUND_TRANSFER_DETAIL_ID]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[ITEM_CATEGORY]
      ,[PACKING_TYPE]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[ROLL_REAM_QTY]
      ,[ROLL_REAM_WT]
      ,[DATA_UPADTE_AUTHORITY]
      ,[DATA_WRITE_TYPE]
      ,[OUTBOUND_TRANSFER_DETAIL_ID]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_DETAIL_T]
  WHERE [TRANSFER_HEADER_ID] = @TRANSFER_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("複製入庫明細資料到入庫歷史明細失敗");
                    }

                    //刪除入庫明細資料
                    cmd = @"
  DELETE FROM [TRF_DETAIL_T]
  WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("刪除入庫明細資料失敗");
                    }

                    //複製入庫揀貨資料到入庫歷史揀貨
                    cmd = @"
INSERT INTO [TRF_INBOUND_PICKED_HT]
(
      [TRANSFER_PICKED_ID]
      ,[TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[STOCK_ID]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[STATUS]
      ,[SPLIT_FROM_BARCODE]
      ,[PALLET_STATUS]
      ,[CONTAINER_NO]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_PICKED_ID]
      ,[TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[STOCK_ID]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[STATUS]
      ,[SPLIT_FROM_BARCODE]
      ,[PALLET_STATUS]
      ,[CONTAINER_NO]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_INBOUND_PICKED_T]
  WHERE [TRANSFER_HEADER_ID] = @TRANSFER_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("複製入庫揀貨資料到入庫歷史揀貨失敗");
                    }

                    //刪除入庫揀貨資料
                    cmd = @"
  DELETE FROM [TRF_INBOUND_PICKED_T]
  WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("刪除入庫揀貨資料失敗");
                    }


                    stkTxnTRepository.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "入庫存檔成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "入庫存檔失敗:" + ex.Message);
                }

            }

        }

        /// <summary>
        /// 出庫存檔
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="checkStockStatus"></param>
        /// <returns></returns>
        public ResultModel OutBoundSaveTransfer(long transferHeaderId, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var detail = trfDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransferHeaderId == transferHeaderId);
                    if (detail == null) return new ResultModel(true, "請輸入出庫明細");

                    //檢查出貨編號是否揀完
                    string cmd = @"
           SELECT 
d.TRANSFER_DETAIL_ID AS ID
FROM TRF_DETAIL_T d
JOIN TRF_HEADER_T h ON h.TRANSFER_HEADER_ID = d.TRANSFER_HEADER_ID
LEFT JOIN TRF_OUTBOUND_PICKED_T p ON p.TRANSFER_HEADER_ID = d.TRANSFER_HEADER_ID AND p.TRANSFER_DETAIL_ID = d.TRANSFER_DETAIL_ID
WHERE h.TRANSFER_HEADER_ID = @transferHeaderId
GROUP BY d.TRANSFER_DETAIL_ID
HAVING (SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) <> MIN(d.REQUESTED_PRIMARY_QUANTITY) AND MIN(d.ITEM_CATEGORY) = '捲筒' )
OR (SUM(ISNULL(p.SECONDARY_QUANTITY, 0)) <> MIN(d.REQUESTED_SECONDARY_QUANTITY) AND MIN(d.ITEM_CATEGORY) = '平版' )
OR MIN(d.ROLL_REAM_QTY) <> COUNT(p.TRANSFER_DETAIL_ID)";

                    var list = this.Context.Database.SqlQuery<long>(cmd, new SqlParameter("@transferHeaderId", transferHeaderId)).ToList();
                    if (list.Count != 0) return new ResultModel(true, "此出貨編號尚未揀完");

                    var pickList = trfOutboundPickedTRepository.GetAll().Where(x => x.TransferHeaderId == transferHeaderId).ToList();
                    if (pickList == null || pickList.Count == 0) throw new Exception("找不到揀貨資料");

                    DateTime now = DateTime.Now;

                    var header = trfHeaderTRepository.GetAll().FirstOrDefault(x => x.TransferHeaderId == transferHeaderId);
                    if (header == null) throw new Exception("找不到出貨編號資料");
                    header.TransactionDate = now;
                    header.NumberStatus = NumberStatus.Saved;
                    header.LastUpdateBy = userId;
                    header.LastUpdateDate = now;
                    header.LastUpdateUserName = userName;
                    trfHeaderTRepository.SaveChanges();

                    //更新庫存鎖定量和庫存備註
                    foreach (TRF_OUTBOUND_PICKED_T pick in pickList)
                    {
                        var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                        if (stock == null) throw new Exception("找不到庫存資料");
                        if (string.IsNullOrEmpty(stock.Note))
                        {
                            stock.Note = pick.Note;
                        }
                        else
                        {
                            stock.Note = stock.Note + "," + pick.Note;
                        }
                        STK_TXN_T stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode, header.TransferLocatorId, CategoryCode.TransferOutbound, ActionCode.StockTransfer, header.ShipmentNumber);
                        var updateStockLockQtyResult = UpdateStockLockQty(stock, stkTxnT, -1 * pick.PrimaryQuantity, -1 * pick.SecondaryQuantity, pickSatus, PickStatus.TransferOutOfStock, userId, now);
                        if (!updateStockLockQtyResult.Success) throw new Exception(updateStockLockQtyResult.Msg);
                    }
                    stkTxnTRepository.SaveChanges();
                    stockTRepository.SaveChanges();

                    //複製出庫明細資料到入庫歷史明細
                    cmd = @"
INSERT INTO TRF_DETAIL_HT
(
      [TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[ITEM_CATEGORY]
      ,[PACKING_TYPE]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[ROLL_REAM_QTY]
      ,[ROLL_REAM_WT]
      ,[DATA_UPADTE_AUTHORITY]
      ,[DATA_WRITE_TYPE]
      ,[OUTBOUND_TRANSFER_DETAIL_ID]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[ITEM_CATEGORY]
      ,[PACKING_TYPE]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[ROLL_REAM_QTY]
      ,[ROLL_REAM_WT]
      ,[DATA_UPADTE_AUTHORITY]
      ,[DATA_WRITE_TYPE]
      ,[OUTBOUND_TRANSFER_DETAIL_ID]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_DETAIL_T]
  WHERE [TRANSFER_HEADER_ID] = @TRANSFER_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("複製出庫明細資料到出庫歷史明細失敗");
                    }

                    //刪除出庫明細資料
                    cmd = @"
  DELETE FROM [TRF_DETAIL_T]
  WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("刪除出庫明細資料失敗");
                    }

                    //複製出庫揀貨資料到出庫歷史揀貨
                    cmd = @"
INSERT INTO [TRF_OUTBOUND_PICKED_HT]
(
      [TRANSFER_PICKED_ID]
      ,[TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[STOCK_ID]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[STATUS]
      ,[PALLET_STATUS]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_PICKED_ID]
      ,[TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[STOCK_ID]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[STATUS]
      ,[PALLET_STATUS]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_OUTBOUND_PICKED_T]
  WHERE [TRANSFER_HEADER_ID] = @TRANSFER_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("複製出庫揀貨資料到出庫歷史揀貨失敗");
                    }

                    //刪除入庫揀貨資料
                    cmd = @"
  DELETE FROM [TRF_OUTBOUND_PICKED_T]
  WHERE TRANSFER_HEADER_ID = @TRANSFER_HEADER_ID";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId)) <= 0)
                    {
                        throw new Exception("刪除出庫揀貨資料失敗");
                    }

                    txn.Commit();
                    return new ResultModel(true, "出庫存檔成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "出庫存檔失敗:" + ex.Message);
                }
            }
        }

        public ResultDataModel<TRF_HEADER_T> OutBoundToInbound(long transferHeaderId, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;

                    var header = trfHeaderTRepository.GetAll().FirstOrDefault(x => x.TransferHeaderId == transferHeaderId);
                    if (header == null) throw new Exception("找不到出貨編號資料");

                    TRF_HEADER_T newHeader = new TRF_HEADER_T();
                    newHeader.OrgId = header.OrgId;
                    newHeader.OrganizationId = header.OrganizationId;
                    newHeader.OrganizationCode = header.OrganizationCode;
                    newHeader.ShipmentNumber = header.ShipmentNumber;
                    newHeader.TransferCatalog = header.TransferCatalog;
                    newHeader.TransferType = TransferType.InBound;
                    newHeader.NumberStatus = NumberStatus.NotSaved;
                    newHeader.IsMes = header.IsMes;
                    newHeader.SubinventoryCode = header.SubinventoryCode;
                    newHeader.LocatorId = header.LocatorId;
                    newHeader.LocatorCode = header.LocatorCode;
                    newHeader.TransactionDate = now;
                    newHeader.TransactionTypeId = header.TransactionTypeId;
                    newHeader.TransactionTypeName = header.TransactionTypeName;
                    newHeader.TransferOrgId = header.TransferOrgId;
                    newHeader.TransferOrganizationId = header.TransferOrganizationId;
                    newHeader.TransferOrganizationCode = header.TransferOrganizationCode;
                    newHeader.TransferSubinventoryCode = header.TransferSubinventoryCode;
                    newHeader.TransferLocatorId = header.TransferLocatorId;
                    newHeader.TransferLocatorCode = header.TransferLocatorCode;
                    newHeader.CreatedBy = header.CreatedBy;
                    newHeader.CreatedUserName = header.CreatedUserName;
                    newHeader.CreationDate = header.CreationDate;
                    newHeader.LastUpdateBy = header.LastUpdateBy;
                    newHeader.LastUpdateUserName = header.LastUpdateUserName;
                    newHeader.LastUpdateDate = header.LastUpdateDate;
                    trfHeaderTRepository.Create(newHeader, true);


                    //出庫歷史明細轉入庫明細
                    string cmd = @"
INSERT INTO TRF_DETAIL_T
(
      [TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[ITEM_CATEGORY]
      ,[PACKING_TYPE]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[ROLL_REAM_QTY]
      ,[ROLL_REAM_WT]
      ,[DATA_UPADTE_AUTHORITY]
      ,[DATA_WRITE_TYPE]
      ,[OUTBOUND_TRANSFER_DETAIL_ID]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT
      @NEW_TRANSFER_HEADER_ID
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[ITEM_CATEGORY]
      ,[PACKING_TYPE]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[ROLL_REAM_QTY]
      ,[ROLL_REAM_WT]
      ,@DATA_UPADTE_AUTHORITY
      ,[DATA_WRITE_TYPE]
      ,[OUTBOUND_TRANSFER_DETAIL_ID]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_DETAIL_HT]
  WHERE [TRANSFER_HEADER_ID] = @TRANSFER_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId),
                        new SqlParameter("@NEW_TRANSFER_HEADER_ID", newHeader.TransferHeaderId),
                        new SqlParameter("@DATA_UPADTE_AUTHORITY", DataUpdateAuthority.Permit) //待確認MES出庫轉MES入庫是否要封鎖Detail修改權限
                        ) <= 0)
                    {
                        throw new Exception("出庫明細轉入庫明細失敗");
                    }

                    //出庫揀貨複製到入庫揀貨
                    cmd = @"
INSERT INTO [TRF_INBOUND_PICKED_T]
(
      
      [TRANSFER_DETAIL_ID]
      ,[TRANSFER_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[STOCK_ID]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,[STATUS]
      ,[PALLET_STATUS]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT 
      d.[TRANSFER_DETAIL_ID]
      ,d.[TRANSFER_HEADER_ID]
      ,p.[INVENTORY_ITEM_ID]
      ,p.[ITEM_NUMBER]
      ,[STOCK_ID]
      ,[BARCODE]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[NOTE]
      ,@STATUS
      ,[PALLET_STATUS]
      ,p.[CREATED_BY]
      ,p.[CREATED_USER_NAME]
      ,p.[CREATION_DATE]
      ,p.[LAST_UPDATE_BY]
      ,p.[LAST_UPDATE_USER_NAME]
      ,p.[LAST_UPDATE_DATE]
  FROM [TRF_OUTBOUND_PICKED_HT] p
  inner join TRF_DETAIL_T d on p.TRANSFER_DETAIL_ID = d.OUTBOUND_TRANSFER_DETAIL_ID
  WHERE p.[TRANSFER_HEADER_ID] = @TRANSFER_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_HEADER_ID", transferHeaderId),
                        new SqlParameter("@STATUS", InboundStatus.WaitInbound)
                        ) <= 0)
                    {
                        throw new Exception("出庫揀貨複製到入庫揀貨失敗");
                    }

                    //處理拆板
                    var pickList = trfInboundPickedTRepository.GetAll().Where(x => x.TransferHeaderId == newHeader.TransferHeaderId && x.PalletStatus == PalletStatusCode.Split).ToList();
                    if (pickList != null && pickList.Count > 0)
                    {
                        var generateBarcodesResult = GenerateBarcodes((long)newHeader.TransferOrganizationId, newHeader.TransferSubinventoryCode, pickList.Count, userId);
                        if (!generateBarcodesResult.Success) throw new Exception(generateBarcodesResult.Msg);

                        for (int i = 0; i < pickList.Count; i++)
                        {
                            //var detail = GetTrfDetail(pickList[i].TransferHeaderId, pickList[i].TransferDetailId);
                            //if (detail == null) throw new Exception("找不到明細資料");

                            //STOCK_T stock = new STOCK_T();
                            //stock.OrganizationId = (long)header.TransferOrganizationId;
                            //stock.OrganizationCode = header.TransferOrganizationCode;
                            //stock.SubinventoryCode = header.TransferSubinventoryCode;
                            //stock.LocatorId = header.TransferLocatorId;
                            //stock.LocatorSegments = header.TransferLocatorCode;
                            //stock.InventoryItemId = detail.InventoryItemId;
                            //stock.ItemNumber = detail.ItemNumber;
                            //stock.ItemDescription = detail.ItemDescription;
                            //stock.ItemCategory = item.CatalogElemVal070;
                            //stock.PaperType = item.CatalogElemVal020;
                            //stock.BasicWeight = item.CatalogElemVal040;
                            //stock.ReamWeight = item.CatalogElemVal060;
                            //if (item.CatalogElemVal070 == ItemCategory.Flat)
                            //{
                            //    stock.RollReamWt = detail.RollReamWt;
                            //}
                            //else
                            //{
                            //    stock.RollReamWt = 0;
                            //}
                            //stock.Specification = item.CatalogElemVal050;
                            //stock.PackingType = detail.PackingType;
                            //stock.OspBatchNo = "";
                            //stock.LotNumber = pick.LotNumber;
                            //stock.Barcode = pick.Barcode;
                            //stock.PrimaryUomCode = pick.PrimaryUom;
                            //stock.PrimaryTransactionQty = pick.PrimaryQuantity;
                            //stock.PrimaryAvailableQty = pick.PrimaryQuantity;
                            //stock.PrimaryLockedQty = 0;
                            //stock.SecondaryUomCode = pick.SecondaryUom;
                            //stock.SecondaryTransactionQty = pick.SecondaryQuantity;
                            //stock.SecondaryAvailableQty = pick.SecondaryQuantity;
                            //stock.SecondaryLockedQty = 0;
                            //stock.ReasonCode = "";
                            //stock.ReasonDesc = "";
                            //stock.Note = pick.Note;
                            //stock.StatusCode = StockStatusCode.InStock;
                            //stock.CreatedBy = userId;
                            //stock.CreationDate = now;
                            //stock.LastUpdateBy = "";
                            //stock.LastUpdateDate = null;
                            //stockTRepository.Create(stock, true);

                            ////產生異動紀錄
                            //var stkTxnT = CreateStockRecord(stock, header.TransferOrganizationId, header.TransferOrganizationCode, header.TransferSubinventoryCode,
                            //header.TransferLocatorId, CategoryCode.TransferInbound, ActionCode.InBoundSaveTransfer, header.ShipmentNumber,
                            //0, stock.PrimaryAvailableQty, stock.PrimaryAvailableQty, 0, stock.SecondaryAvailableQty, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                            //stkTxnTRepository.Create(stkTxnT);

                            pickList[i].SplitFromBarcode = pickList[i].Barcode;
                            pickList[i].Barcode = generateBarcodesResult.Data[i];
                            pickList[i].StockId = 0;
                            pickList[i].Status = InboundStatus.WaitPrint;
                            pickList[i].Note = "拆至" + pickList[i].SplitFromBarcode;
                            pickList[i].PalletStatus = PalletStatusCode.All;
                            pickList[i].LastUpdateBy = userId;
                            pickList[i].LastUpdateUserName = userName;
                            trfInboundPickedTRepository.Update(pickList[i]);
                        }
                        trfInboundPickedTRepository.SaveChanges();
                    }

                    txn.Commit();
                    return new ResultDataModel<TRF_HEADER_T>(true, "MES出庫轉入庫成功", newHeader);

                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultDataModel<TRF_HEADER_T>(false, "MES出庫轉入庫失敗:" + ex.Message, null);
                }
            }
        }

        public ResultModel MergeBarcode(STOCK_T mergeBarocdeData, List<TRF_INBOUND_PICKED_T> waitMergeBarcodeDataList, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    if (mergeBarocdeData == null) throw new Exception("找不到被併板條碼資料");
                    if (waitMergeBarcodeDataList == null || waitMergeBarcodeDataList.Count == 0) throw new Exception("找不到待併板條碼資料");

                    decimal waitMergePrimaryTotalQty = 0;
                    decimal waitMergeSecondaryTotalQty = 0;
                    string newNote = "";
                    DateTime now = DateTime.Now;

                    foreach (TRF_INBOUND_PICKED_T data in waitMergeBarcodeDataList)
                    {
                        waitMergeSecondaryTotalQty = waitMergeSecondaryTotalQty + (decimal)data.SecondaryQuantity;
                        newNote = newNote + data.Barcode + " ";
                    }

                    newNote = "併" + newNote;

                    var uomConversionResult = uomConversion.Convert(waitMergeBarcodeDataList[0].InventoryItemId, waitMergeSecondaryTotalQty, waitMergeBarcodeDataList[0].SecondaryUom, waitMergeBarcodeDataList[0].PrimaryUom); //副單位需求量 轉 主單位需求量
                    if (uomConversionResult.Success)
                    {
                        waitMergePrimaryTotalQty = uomConversionResult.Data;
                    }
                    else
                    {
                        throw new Exception(uomConversionResult.Msg);
                    }

                    TRF_INBOUND_PICKED_T pick = new TRF_INBOUND_PICKED_T();
                    pick.TransferDetailId = waitMergeBarcodeDataList[0].TransferDetailId;
                    pick.TransferHeaderId = waitMergeBarcodeDataList[0].TransferHeaderId;
                    pick.InventoryItemId = waitMergeBarcodeDataList[0].InventoryItemId;
                    pick.ItemNumber = waitMergeBarcodeDataList[0].ItemNumber;
                    pick.StockId = mergeBarocdeData.StockId;
                    pick.Barcode = mergeBarocdeData.Barcode;
                    pick.PrimaryUom = waitMergeBarcodeDataList[0].PrimaryUom;
                    pick.PrimaryQuantity = waitMergePrimaryTotalQty;
                    pick.SecondaryUom = waitMergeBarcodeDataList[0].SecondaryUom;
                    pick.SecondaryQuantity = waitMergeSecondaryTotalQty;
                    pick.LotNumber = null; //併版沒有捲號
                    pick.LotQuantity = null; //併版沒有理論重
                    pick.Note = newNote;
                    pick.Status = InboundStatus.WaitPrint;
                    pick.PalletStatus = PalletStatusCode.Merge;
                    pick.CreatedBy = userId;
                    pick.CreatedUserName = userName;
                    pick.CreationDate = now;
                    pick.LastUpdateBy = null;
                    pick.LastUpdateUserName = null;
                    pick.LastUpdateDate = null;
                    trfInboundPickedTRepository.Create(pick, true);

                    DelInboundPickData(waitMergeBarcodeDataList, userId, userName);

                    txn.Commit();
                    return new ResultModel(true, "併板成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "併板失敗:" + ex.Message);
                }
            }
        }


        #region 貨故

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
      ,ISNULL([SECONDARY_AVAILABLE_QTY],0) AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE
      ,[REASON_DESC] AS REASON_DESC
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
      ,[REASON_DESC] AS REASON_DESC
      ,s.[LOCATOR_ID] AS LOCATOR_ID
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

        public ResultModel SaveReason(HttpFileCollectionBase File, long stockId, string reasonCode, long? transferLocatorId, string note, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;

                    var trfLocator = locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == transferLocatorId);
                    if (trfLocator == null) throw new Exception("找不到目標儲位資料");

                    var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == stockId);
                    if (stock == null) throw new Exception("找不到庫存資料");

                    var locator = locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == stock.LocatorId);
                    if (locator == null) throw new Exception("找不到儲位資料");

                    var organization = GetOrganization(stock.OrganizationId);
                    if (organization == null) throw new Exception("找不到組織資料");

                    var trfOrganization = GetOrganization(trfLocator.OrganizationId);
                    if (trfOrganization == null) throw new Exception("找不到目標組織資料");

                    var reason = stkReasonTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.ReasonCode == reasonCode);
                    if (reason == null) throw new Exception("找不到貨故資料");

                    long transactionTypeId;
                    var transferCatalog = GetTransferCatalog(stock.OrganizationId, trfLocator.OrganizationId);
                    if (transferCatalog == TransferCatalog.OrgTransfer)
                    {
                        throw new Exception("貨故不可為組織間移轉");
                        //transactionTypeId = TransferUOW.TransactionTypeId.IntransitShipment;
                    }
                    else
                    {
                        transactionTypeId = TransferUOW.TransactionTypeId.Chp30;
                    }

                    var transactionType = GetTransactionType(transactionTypeId);
                    if (trfOrganization == null) throw new Exception("找不到庫存交易類別資料");


                    TRF_REASON_HEADER_T header = new TRF_REASON_HEADER_T
                    {
                        OrgId = organization.OrgUnitId,
                        OrganizationId = stock.OrganizationId,
                        OrganizationCode = stock.OrganizationCode,
                        ShipmentNumber = GetShipmentNumberGuid(),
                        SubinventoryCode = stock.SubinventoryCode,
                        LocatorId = stock.LocatorId,
                        LocatorCode = stock.LocatorSegments,
                        Segment3 = locator.Segment3,
                        TransactionDate = now,
                        TransactionTypeId = transactionTypeId,
                        TransactionTypeName = transactionType.TransactionTypeName,
                        TransferOrgId = trfOrganization.OrgUnitId,
                        TransferOrganizationId = trfLocator.OrganizationId,
                        TransferOrganizationCode = trfOrganization.OrganizationCode,
                        TransferSubinventoryCode = trfLocator.SubinventoryCode,
                        TransferLocatorId = transferLocatorId,
                        TransferLocatorCode = trfLocator.LocatorSegments,
                        NumberStatus = NumberStatus.Saved,
                        ToErp = ToErp.Yes,
                        CreatedBy = userId,
                        CreatedUserName = userName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null
                    };
                    trfReasonHeaderTRepository.Create(header, true);

                    TRF_REASON_T detail = new TRF_REASON_T
                    {
                        TransferReasonHeaderId = header.TransferReasonHeaderId,
                        InventoryItemId = stock.InventoryItemId,
                        ItemNumber = stock.ItemNumber,
                        ItemDescription = stock.ItemDescription,
                        Barcode = stock.Barcode,
                        StockId = stockId,
                        PrimaryUom = stock.PrimaryUomCode,
                        PrimaryQuantity = stock.PrimaryAvailableQty,
                        SecondaryUom = stock.SecondaryUomCode,
                        SecondaryQuantity = stock.SecondaryAvailableQty,
                        LotNumber = stock.LotNumber,
                        LotQuantity = stock.LotQuantity,
                        ReasonCode = reasonCode,
                        ReasonDesc = reason.ReasonDesc,
                        Note = note,
                        CreatedBy = userId,
                        CreatedUserName = userName,
                        CreationDate = now,
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null,
                    };
                    trfReasonTRepository.Create(detail, true);

                    if (File != null || File.Count != 0)
                    {
                        foreach (string i in File)
                        {
                            HttpPostedFileBase hpf = File[i] as HttpPostedFileBase;
                            var filebyte = VaryQualityLevel(hpf);
                            TRF_FILES_T tRF_FILES_T = new TRF_FILES_T();
                            tRF_FILES_T.FileInstance = filebyte;
                            trfFilesTRepository.Create(tRF_FILES_T, true);

                            TRF_FILEINFO_T tRF_FILEINFO_T = new TRF_FILEINFO_T();
                            tRF_FILEINFO_T.TransferReasonHeaderId = header.TransferReasonHeaderId;
                            tRF_FILEINFO_T.TransferReasonId = detail.TransferReasonId;
                            tRF_FILEINFO_T.TrfFileId = tRF_FILES_T.TrfFileId;
                            tRF_FILEINFO_T.FileType = hpf.ContentType;
                            tRF_FILEINFO_T.FileName = Path.GetFileName(hpf.FileName);
                            tRF_FILEINFO_T.Size = filebyte.Length;
                            tRF_FILEINFO_T.Seq = 1;
                            tRF_FILEINFO_T.CreatedBy = userId;
                            tRF_FILEINFO_T.CreatedUserName = userName;
                            tRF_FILEINFO_T.CreationDate = DateTime.Now;
                            trfFileInfoTRepository.Create(tRF_FILEINFO_T);

                        }
                    }

                    //更新庫存
                    stock.OrganizationId = trfLocator.OrganizationId;
                    stock.OrganizationCode = trfOrganization.OrganizationCode;
                    stock.SubinventoryCode = trfLocator.SubinventoryCode;
                    stock.LocatorId = transferLocatorId;
                    stock.LocatorSegments = trfLocator.LocatorSegments;
                    stock.ReasonCode = reasonCode;
                    stock.ReasonDesc = reason.ReasonDesc;
                    stock.StatusCode = StockStatusCode.InStock;
                    if (string.IsNullOrEmpty(stock.Note))
                    {
                        stock.Note = note;
                    }
                    else
                    {
                        stock.Note = stock.Note + "," + note;
                    }
                    stock.LastUpdateBy = userId;
                    stock.LastUpdateDate = now;
                    stockTRepository.Update(stock);

                    var stkTxnT = CreateStockRecord(stock, trfLocator.OrganizationId, trfOrganization.OrganizationCode, trfLocator.SubinventoryCode,
                        transferLocatorId, CategoryCode.TransferReason, ActionCode.StockTransfer, header.ShipmentNumber,
                                  stock.PrimaryAvailableQty, 0, stock.PrimaryAvailableQty, stock.SecondaryAvailableQty, 0, stock.SecondaryAvailableQty, StockStatusCode.InStock, userId, now);
                    stkTxnTRepository.Create(stkTxnT);

                    
                    //複製貨故明細資料到貨故歷史明細
                    string cmd = @"
INSERT INTO TRF_REASON_HT
(
      [TRANSFER_REASON_ID]
      ,[TRANSFER_REASON_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[BARCODE]
      ,[STOCK_ID]
      ,[PRIMARY_UOM]
      ,[PRIMARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[REASON_CODE]
      ,[REASON_DESC]
      ,[NOTE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_REASON_ID]
      ,[TRANSFER_REASON_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[BARCODE]
      ,[STOCK_ID]
      ,[PRIMARY_UOM]
      ,[PRIMARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[REASON_CODE]
      ,[REASON_DESC]
      ,[NOTE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_REASON_T]
  WHERE [TRANSFER_REASON_HEADER_ID] = @TRANSFER_REASON_HEADER_ID
";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_REASON_HEADER_ID", header.TransferReasonHeaderId)) <= 0)
                    {
                        throw new Exception("複製貨故明細資料到貨故歷史明細失敗");
                    }

                    //刪除貨故明細資料
                    cmd = @"
  DELETE FROM [TRF_REASON_T]
  WHERE TRANSFER_REASON_HEADER_ID = @TRANSFER_REASON_HEADER_ID";
                    if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_REASON_HEADER_ID", header.TransferReasonHeaderId)) <= 0)
                    {
                        throw new Exception("刪除貨故明細資料失敗");
                    }

                    this.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "貨故存檔成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "貨故存檔失敗:" + ex.Message);
                }
            }
        }

        #endregion

        #region 標籤
        public ResultDataModel<List<LabelModel>> GetInboundLabels(List<long> transferPickedIdList, string userName)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (transferPickedIdList == null || transferPickedIdList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                var pickDataList = trfInboundPickedTRepository.GetAll().AsNoTracking().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
                if (pickDataList == null || pickDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                if (pickDataList.Count != transferPickedIdList.Count) throw new Exception("找不到部分揀貨資料");
                var header = GetTrfHeader(pickDataList[0].TransferHeaderId);
                if (header == null) return new ResultDataModel<List<LabelModel>>(false, "找不到出貨編號資料", null);

                if (header.IsMes == IsMes.Yes)
                {
                    //對方為MES時
                    foreach (TRF_INBOUND_PICKED_T pick in pickDataList)
                    {
                        if (pick.StockId == 0)
                        {
                            //為入庫時另外新增的資料，不是MES出庫轉入庫的資料
                            var detail = GetTrfDetail(pick.TransferHeaderId, pick.TransferDetailId);
                            if (detail == null) return new ResultDataModel<List<LabelModel>>(false, "找不到明細資料", null);

                            StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,i.ITEM_DESC_TCH as BarocdeName
,i.CATALOG_ELEM_VAL_020 as PapaerType
,i.CATALOG_ELEM_VAL_040 as BasicWeight
,i.CATALOG_ELEM_VAL_050 as Specification
,'' as BatchNo");

                            if (pick.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                            {
                                //棧板狀態為拆板 在入庫不會遇到
                                throw new Exception("入庫棧板狀態不可為拆板");
                            }
                            else if (pick.PalletStatus == PalletStatusCode.All) //判斷是否整版
                            {
                                if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
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
                                else if (detail.ItemCategory == ItemCategory.Roll)
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
                                else
                                {
                                    throw new Exception("無法識別貨品類別");
                                }
                            }
                            else
                            {
                                //併板
                                if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
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
,FORMAT((s.SECONDARY_AVAILABLE_QTY + p.SECONDARY_QUANTITY),'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                                }
                                else if (detail.ItemCategory == ItemCategory.Roll)
                                {
                                    //併板 捲筒
                                    return new ResultDataModel<List<LabelModel>>(false, "捲筒不能併板", null);
                                }
                                else
                                {
                                    throw new Exception("無法識別貨品類別");
                                }
                            }


                            var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", pick.Barcode)).ToList();
                            if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                            labelModelList.Add(labelModel[0]);
                        }
                        else
                        {
                            //為MES出庫轉入庫的資料
                            var detail = GetTrfDetail(pick.TransferHeaderId, pick.TransferDetailId);
                            if (detail == null) return new ResultDataModel<List<LabelModel>>(false, "找不到明細資料", null);

                            StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as BatchNo");

                            if (pick.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                            {
                                //棧板狀態為拆板 在入庫不會遇到
                                throw new Exception("入庫棧板狀態不可為拆板");

                            }
                            else if (pick.PalletStatus == PalletStatusCode.All) //判斷是否整版
                            {
                                if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
                                {
                                    if (string.IsNullOrEmpty(pick.SplitFromBarcode))
                                    {
                                        //整板 平版 不是從拆板轉來 數量為揀貨的數量
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
                                        //整板 平版 從拆板轉來 數量為揀貨的數量
                                        cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.SPLIT_FROM_BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                                    }

                                }
                                else if (detail.ItemCategory == ItemCategory.Roll)
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
                                else
                                {
                                    throw new Exception("無法識別貨品類別");
                                }
                            }
                            else
                            {
                                //併板
                                if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
                                {
                                    //併板 平版(令包) 數量為庫存數量(被併板條碼原庫存) + 檢貨數量(待併板條碼數量總和)
                                    cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT((s.SECONDARY_AVAILABLE_QTY + p.SECONDARY_QUANTITY),'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                                }
                                else if (detail.ItemCategory == ItemCategory.Roll)
                                {
                                    //併板 捲筒
                                    return new ResultDataModel<List<LabelModel>>(false, "捲筒不能併板", null);
                                }
                                else
                                {
                                    throw new Exception("無法識別貨品類別");
                                }
                            }


                            var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", pick.Barcode)).ToList();
                            if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                            labelModelList.Add(labelModel[0]);
                        }
                    }
                    return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);
                }
                else
                {
                    //對方非MES時料號資料從主檔取 併板則從庫存取
                    foreach (TRF_INBOUND_PICKED_T pick in pickDataList)
                    {
                        var detail = GetTrfDetail(pick.TransferHeaderId, pick.TransferDetailId);
                        if (detail == null) return new ResultDataModel<List<LabelModel>>(false, "找不到明細資料", null);

                        StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,i.ITEM_DESC_TCH as BarocdeName
,i.CATALOG_ELEM_VAL_020 as PapaerType
,i.CATALOG_ELEM_VAL_040 as BasicWeight
,i.CATALOG_ELEM_VAL_050 as Specification
,'' as BatchNo");

                        if (pick.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                        {
                            //棧板狀態為拆板 在入庫不會遇到
                            throw new Exception("入庫棧板狀態不可為拆板");
                        }
                        else if (pick.PalletStatus == PalletStatusCode.All) //判斷是否整版
                        {
                            if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
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
                            else if (detail.ItemCategory == ItemCategory.Roll)
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
                            else
                            {
                                throw new Exception("無法識別貨品類別");
                            }
                        }
                        else
                        {
                            //併板
                            if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
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
,FORMAT((s.SECONDARY_AVAILABLE_QTY + p.SECONDARY_QUANTITY),'0.##########') as Qty
FROM [TRF_INBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                            }
                            else if (detail.ItemCategory == ItemCategory.Roll)
                            {
                                //併板 捲筒
                                return new ResultDataModel<List<LabelModel>>(false, "捲筒不能併板", null);
                            }
                            else
                            {
                                throw new Exception("無法識別貨品類別");
                            }
                        }


                        var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", pick.Barcode)).ToList();
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

        public ResultDataModel<List<LabelModel>> GetOutboundLabels(List<long> transferPickedIdList, string userName)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (transferPickedIdList == null || transferPickedIdList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                var pickDataList = trfOutboundPickedTRepository.GetAll().AsNoTracking().Where(x => transferPickedIdList.Contains(x.TransferPickedId)).ToList();
                if (pickDataList == null || pickDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                if (pickDataList.Count != transferPickedIdList.Count) throw new Exception("找不到部分揀貨資料");
                var header = GetTrfHeader(pickDataList[0].TransferHeaderId);
                if (header == null) return new ResultDataModel<List<LabelModel>>(false, "找不到出貨編號資料", null);

                foreach (TRF_OUTBOUND_PICKED_T pick in pickDataList)
                {
                    var detail = GetTrfDetail(pick.TransferHeaderId, pick.TransferDetailId);
                    if (detail == null) return new ResultDataModel<List<LabelModel>>(false, "找不到明細資料", null);

                    StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as BatchNo");

                    if (pick.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                    {
                        if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
                        {
                            //拆板 平版 數量為庫存數量(拆板後的剩餘數量)
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(s.SECONDARY_AVAILABLE_QTY,'0.##########') as Qty
FROM [TRF_OUTBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else if (detail.ItemCategory == ItemCategory.Roll)
                        {
                            //拆板 捲筒
                            return new ResultDataModel<List<LabelModel>>(false, "捲筒不能拆板", null);
                        }
                        else
                        {
                            throw new Exception("無法識別貨品類別");
                        }
                    }
                    else if (pick.PalletStatus == PalletStatusCode.All) //判斷是否整版
                    {
                        if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
                        {
                            //整板 平版 數量為揀貨的數量
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [TRF_OUTBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else if (detail.ItemCategory == ItemCategory.Roll)
                        {
                            //整板 捲筒
                            cmd.Append(@"
,s.PRIMARY_UOM_CODE as Unit
,FORMAT(p.PRIMARY_QUANTITY,'0.##########') as Qty
FROM [TRF_OUTBOUND_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else
                        {
                            throw new Exception("無法識別貨品類別");
                        }
                    }
                    else
                    {
                        //棧板狀態為併板 在出貨不會遇到
                        throw new Exception("出庫棧板狀態不可為併板");
                    }


                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", pick.Barcode)).ToList();
                    if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                    labelModelList.Add(labelModel[0]);
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

        #region 報表
        public ResultDataModel<ReportDataSource> GetOutboundPickingListReportDataSource(string shipmentNumber)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    ReportDataSource dataSource = new ReportDataSource();
                    DataSet dataset = new DataSet("dataset");
                    string cmd = "select * from OutboundPickingList(@SHIPMENT_NUMBER)";
                    SqlCommand command = new SqlCommand(cmd, connection);
                    command.Parameters.Add(new SqlParameter("@SHIPMENT_NUMBER", shipmentNumber));
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "Detail");
                    dataSource.Name = "Detail";
                    dataSource.Value = dataset.Tables["Detail"];

                    connection.Close();
                    return new ResultDataModel<ReportDataSource>(true, "取得庫存移轉-備貨單報表資料來源成功", dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    return new ResultDataModel<ReportDataSource>(false, "取得庫存移轉-備貨單報表資料來源失敗:" + ex.Message, null);
                }
            }

        }

        public ResultDataModel<ReportDataSource> GetInboundRollPickingListReportDataSource(string shipmentNumber)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    ReportDataSource dataSource = new ReportDataSource();
                    DataSet dataset = new DataSet("dataset");
                    string cmd = "select * from InboundRollPickingList(@SHIPMENT_NUMBER)";
                    SqlCommand command = new SqlCommand(cmd, connection);
                    command.Parameters.Add(new SqlParameter("@SHIPMENT_NUMBER", shipmentNumber));
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "Detail");
                    dataSource.Name = "Detail";
                    dataSource.Value = dataset.Tables["Detail"];

                    connection.Close();
                    return new ResultDataModel<ReportDataSource>(true, "取得庫存移轉-紙捲入庫單報表資料來源成功", dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    return new ResultDataModel<ReportDataSource>(false, "取得庫存移轉-紙捲入庫單報表資料來源失敗:" + ex.Message, null);
                }
            }

        }

        public ResultDataModel<ReportDataSource> GetInboundFlatPickingListReportDataSource(string shipmentNumber)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    ReportDataSource dataSource = new ReportDataSource();
                    DataSet dataset = new DataSet("dataset");
                    string cmd = "select * from InboundFlatPickingList(@SHIPMENT_NUMBER)";
                    SqlCommand command = new SqlCommand(cmd, connection);
                    command.Parameters.Add(new SqlParameter("@SHIPMENT_NUMBER", shipmentNumber));
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "Detail");
                    dataSource.Name = "Detail";
                    dataSource.Value = dataset.Tables["Detail"];

                    connection.Close();
                    return new ResultDataModel<ReportDataSource>(true, "取得庫存移轉-平張入庫單報表資料來源成功", dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    return new ResultDataModel<ReportDataSource>(false, "取得庫存移轉-平張入庫單報表資料來源失敗:" + ex.Message, null);
                }
            }

        }

        #endregion


        public List<SelectListItem> GetShipmnetNumberCreateTypeList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "自動新增", Value = "新增編號" });
            list.Add(new SelectListItem() { Text = "手動輸入", Value = "手動輸入" });
            return list;
        }
       
    }

}