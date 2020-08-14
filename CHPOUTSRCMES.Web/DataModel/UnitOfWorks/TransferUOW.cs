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

        public List<SelectListItem> GetShipmentNumberDropDownList(string transferCatalog, string transferType, string outSubinventoryCode, string inSubinventoryCode)
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
           SELECT TRANSFER_HEADER_ID as Value,
      SHIPMENT_NUMBER as Text,
'false' as Disabled,
null as 'Group',
'false' as Selected

FROM TRF_HEADER_T
WHERE 
TRANSFER_TYPE = 1
AND SUBINVENTORY_CODE = @subCode AND ORGANIZATION_ID = @organId
AND TRANSFER_SUBINVENTORY_CODE = @trfSubCode AND TRANSFER_ORGANIZATION_ID = @trfOrganId

UNION 


SELECT
s.TRANSFER_HEADER_ID as Value,
s.SHIPMENT_NUMBER as Text,
'false' as Disabled,
null as 'Group',
'false' as Selected
FROM TRF_HEADER_T s
LEFT JOIN TRF_HEADER_T t ON s.SHIPMENT_NUMBER = t.SHIPMENT_NUMBER AND t.TRANSFER_TYPE = 1  
WHERE 
s.TRANSFER_TYPE = 0
AND s.SUBINVENTORY_CODE = @subCode AND s.ORGANIZATION_ID = @organId
AND s.TRANSFER_SUBINVENTORY_CODE = @trfSubCode AND s.TRANSFER_ORGANIZATION_ID = @trfOrganId

AND s.NUMBER_STATUS = 1
AND t.SHIPMENT_NUMBER IS NULL";

                return  this.Context.Database.SqlQuery<SelectListItem>(cmd,
                    new SqlParameter("@subCode", outSubinventoryCode),
                    new SqlParameter("@organId", outOrganizationId),
                    new SqlParameter("@trfSubCode", inSubinventoryCode),
                    new SqlParameter("@trfOrganId", inOrganizationId)).ToList();


             

        }

        
        public List<TRF_HEADER_T> GetTrfHeaderList(string shipmentNumber, string transferType)
        {
            return trfHeaderTRepositiory.GetAll().AsNoTracking().Where(x =>
            x.ShipmentNumber == shipmentNumber &&
            x.TransferType == transferType).ToList();
        }
    }

}