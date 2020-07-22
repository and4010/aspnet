using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Delivery;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class DeliveryUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<DLV_ORG_T> dlvOrgTRepositiory;
        private readonly IRepository<DLV_HEADER_T> dlvHeaderTRepositiory;
        private readonly IRepository<DLV_DETAIL_T> dlvDetailTRepositiory;
        private readonly IRepository<DLV_PICKED_T> dlvPickedTRepositiory;
        private readonly IRepository<DLV_PICKED_HT> dlvPickedHtRepositiory;

        //還有幾個TABLE未加入

                /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DeliveryUOW(DbContext context)
            : base(context)
        {
            this.dlvOrgTRepositiory = new GenericRepository<DLV_ORG_T>(this);
            this.dlvHeaderTRepositiory = new GenericRepository<DLV_HEADER_T>(this);
            this.dlvDetailTRepositiory = new GenericRepository<DLV_DETAIL_T>(this);
            this.dlvPickedTRepositiory = new GenericRepository<DLV_PICKED_T>(this);
            this.dlvPickedHtRepositiory = new GenericRepository<DLV_PICKED_HT>(this);
        }

        public void generateTestData()
        {
            try
            {
                //DliveryHeaderRepositiory.getContext().Configuration.AutoDetectChangesEnabled = false;
                DLV_HEADER_T mDLV_HEADER_T = new DLV_HEADER_T();
                mDLV_HEADER_T.DlvHeaderId = 1;
                mDLV_HEADER_T.OrgId = 1;
                mDLV_HEADER_T.OrgName = "1";
                mDLV_HEADER_T.OrganizationId = 265;
                mDLV_HEADER_T.OrganizationCode = "FTY";
                mDLV_HEADER_T.SubinventoryCode = "TB2";
                mDLV_HEADER_T.TripCar = "PN01";
                mDLV_HEADER_T.TripId = 1;
                mDLV_HEADER_T.TripName = "Y191226-1036357";
                mDLV_HEADER_T.TripActualShipDate = Convert.ToDateTime("2019-12-26");
                mDLV_HEADER_T.DeliveryId = 1;
                mDLV_HEADER_T.DeliveryName = "FTY1912000547";
                mDLV_HEADER_T.ItemCategory = "平版";
                mDLV_HEADER_T.CustomerId = 1;
                mDLV_HEADER_T.CustomerNumber = "1";
                mDLV_HEADER_T.CustomerName = "保吉";
                mDLV_HEADER_T.CustomerLocationCode = "福安印刷";
                mDLV_HEADER_T.ShipCustomerId = 1;
                mDLV_HEADER_T.ShipCustomerNumber = "1";
                mDLV_HEADER_T.ShipCustomerName = "保吉紙業有限公司";
                mDLV_HEADER_T.ShipLocationCode = "台南市安南區府安路5段119巷";
                mDLV_HEADER_T.Freight_Terms_Name = "台南";
                mDLV_HEADER_T.DeliveryStatusId = 1;
                mDLV_HEADER_T.DeliveryStatusName = "未印";
                mDLV_HEADER_T.TransactionBy = null;
                mDLV_HEADER_T.TransactionDate = null;
                mDLV_HEADER_T.Authorize_By = null;
                mDLV_HEADER_T.Authorize_Date = null;
                mDLV_HEADER_T.CreatedBy = 1;
                mDLV_HEADER_T.CreationDate = DateTime.Now;
                mDLV_HEADER_T.LastUpdateBy = 1;
                mDLV_HEADER_T.LastUpdateDate = DateTime.Now;
                dlvHeaderTRepositiory.Create(mDLV_HEADER_T, true);
                //DliveryHeaderRepositiory.getContext().Configuration.AutoDetectChangesEnabled = true;


                DLV_DETAIL_T mDLV_DETAIL_T = new DLV_DETAIL_T();
                mDLV_DETAIL_T.DlvDetailId = 1;
                mDLV_DETAIL_T.DlvHeaderId = mDLV_HEADER_T.DlvHeaderId;
                mDLV_DETAIL_T.OrderNumber = 1192006167;
                mDLV_DETAIL_T.OrderLineId = 1;
                mDLV_DETAIL_T.OrderShipNumber = "1.2";
                mDLV_DETAIL_T.PackingType = "令包";
                mDLV_DETAIL_T.InventoryItemId = 1;
                mDLV_DETAIL_T.ItemNumber = "4A003A01000310K266K";
                mDLV_DETAIL_T.ItemDescription = "123";
                mDLV_DETAIL_T.ReamWeight = "58.97";
                mDLV_DETAIL_T.ItemCategory = "平版";
                mDLV_DETAIL_T.PaperType = "A003";
                mDLV_DETAIL_T.BasicWeight = "01000";
                mDLV_DETAIL_T.Specification = "310K266K";
                mDLV_DETAIL_T.GrainDirection = "1";
                mDLV_DETAIL_T.LocatorId = null;
                mDLV_DETAIL_T.LocatorCode = null;
                mDLV_DETAIL_T.SrcRequestedQuantity = 1.33742M;
                mDLV_DETAIL_T.SrcRequestedQuantityUom = "MT";
                mDLV_DETAIL_T.RequestedQuantity = 1337.419M;
                mDLV_DETAIL_T.RequestedQuantityUom = "KG";
                mDLV_DETAIL_T.RequestedQuantity2 = 50;
                mDLV_DETAIL_T.RequestedQuantityUom2 = "RE";
                mDLV_DETAIL_T.OspBatchId = 1;
                mDLV_DETAIL_T.OspBatchNo = "P9B0288";
                mDLV_DETAIL_T.OspBatchType = "";
                mDLV_DETAIL_T.TmpItemId = null;
                mDLV_DETAIL_T.TmpItemNumber = "";
                mDLV_DETAIL_T.TmpItemDescription = "";
                mDLV_DETAIL_T.CreatedBy = 1;
                mDLV_DETAIL_T.CreationDate = DateTime.Now;
                mDLV_DETAIL_T.LastUpdateBy = 1;
                mDLV_DETAIL_T.LastUpdateDate = DateTime.Now;
                dlvDetailTRepositiory.Create(mDLV_DETAIL_T, true);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }


        }

    }
}