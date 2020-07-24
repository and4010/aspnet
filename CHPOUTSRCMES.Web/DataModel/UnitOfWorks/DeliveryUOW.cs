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
using CHPOUTSRCMES.Web.Models.Delivery;
using System.Text;
using System.Data.SqlClient;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class DeliveryUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<DLV_ORG_T> dlvOrgTRepositiory;
        private readonly IRepository<DLV_HEADER_T> dlvHeaderTRepositiory;
        private readonly IRepository<DLV_DETAIL_T> dlvDetailTRepositiory;
        private readonly IRepository<DLV_DETAIL_HT> dlvDetailHtRepositiory;
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
            this.dlvDetailHtRepositiory = new GenericRepository<DLV_DETAIL_HT>(this);
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
                mDLV_HEADER_T.FreightTermsName = "台南";
                mDLV_HEADER_T.DeliveryStatusCode = "1";
                mDLV_HEADER_T.DeliveryStatusName = "未印";
                mDLV_HEADER_T.TransactionBy = null;
                mDLV_HEADER_T.TransactionDate = null;
                mDLV_HEADER_T.AuthorizeBy = null;
                mDLV_HEADER_T.AuthorizeDate = null;
                mDLV_HEADER_T.Note = "FT1.P9B0288";
                mDLV_HEADER_T.CreatedBy = "1";
                mDLV_HEADER_T.CreatedUserName = "華紙";
                mDLV_HEADER_T.CreationDate = DateTime.Now;
                mDLV_HEADER_T.LastUpdateBy = "1";
                mDLV_HEADER_T.LastUpdateUserName = "華紙";
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
                mDLV_DETAIL_T.CreatedBy = "1";
                mDLV_DETAIL_T.CreatedUserName = "華紙";
                mDLV_DETAIL_T.CreationDate = DateTime.Now;
                mDLV_DETAIL_T.LastUpdateBy = "1";
                mDLV_DETAIL_T.LastUpdateUserName = "華紙";
                mDLV_DETAIL_T.LastUpdateDate = DateTime.Now;
                dlvDetailTRepositiory.Create(mDLV_DETAIL_T, true);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }


        }



        public List<SelectListItem> GetTripNameDropDownList(DropDownListType type)
        {
            var tripNameList = createDropDownList(type);
            tripNameList.AddRange(getTripNameList());
            return tripNameList;
        }

        public List<SelectListItem> GetDeliveryStatusDropDownList(DropDownListType type)
        {
            var deliveryStatusList = createDropDownList(type);
            deliveryStatusList.AddRange(getDeliveryStatusList());
            return deliveryStatusList;
        }

        private List<SelectListItem> getDeliveryStatusList()
        {
            var deliveryStatusList = new List<SelectListItem>();
            try
            {
                deliveryStatusList.Add(new SelectListItem() { Text = "取消", Value = "0" });
                deliveryStatusList.Add(new SelectListItem() { Text = "未印", Value = "1" });
                deliveryStatusList.Add(new SelectListItem() { Text = "待出", Value = "2" });
                deliveryStatusList.Add(new SelectListItem() { Text = "已揀", Value = "3" });
                deliveryStatusList.Add(new SelectListItem() { Text = "待核准", Value = "4" });
                deliveryStatusList.Add(new SelectListItem() { Text = "已出貨", Value = "5" });
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return deliveryStatusList;
        }

        private List<SelectListItem> getTripNameList()
        {
            var tripNameList = new List<SelectListItem>();
            try
            {
                var tempList = dlvHeaderTRepositiory
                            .GetAll().AsNoTracking()
                            .OrderBy(x => x.TripName)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.TripName,
                                Value = x.TripId.ToString()
                            });
                tripNameList.AddRange(tempList);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return tripNameList;
        }

        public List<TripHeaderDT> DeliverySearch(string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus)
        {

            DateTime shipBeginDate = new DateTime();
            DateTime shipEndDate = new DateTime();
            DateTime tdate = new DateTime();

            bool shipBeginDateStatus = DateTime.TryParseExact(TripActualShipBeginDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out shipBeginDate);
            bool shipEndDateStatus = DateTime.TryParseExact(TripActualShipEndDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out shipEndDate);
            bool transactionDateStatus = DateTime.TryParseExact(TransactionDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out tdate);

            long tripId = 0;
            try
            {
                if (SelectedTrip != "*")
                {
                    tripId = Convert.ToInt64(SelectedTrip);
                }
            }
            catch
            {
                SelectedTrip = "*";
            }

            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            List<string> cond = new List<string>();
            string prefixCmd = @"
select h.AUTHORIZE_DATE,
h.CUSTOMER_LOCATION_CODE,
h.CUSTOMER_NAME,
h.DELIVERY_NAME,
h.DELIVERY_STATUS_NAME,
h.ITEM_CATEGORY as DetailType,
h.FREIGHT_TERMS_NAME,
h.DLV_HEADER_ID,
h.NOTE,
h.SUBINVENTORY_CODE,
h.SHIP_CUSTOMER_NAME,
h.SHIP_LOCATION_CODE,
h.TRIP_ACTUAL_SHIP_DATE,
h.TRANSACTION_DATE,
h.TRIP_CAR,
h.TRIP_ID,
h.TRIP_NAME,
d.REQUESTED_PRIMARY_QUANTITY,
d.REQUESTED_PRIMARY_UOM,
d.REQUESTED_SECONDARY_QUANTITY,
d.REQUESTED_SECONDARY_UOM,
d.REQUESTED_TRANSACTION_QUANTITY,
d.REQUESTED_TRANSACTION_UOM
from DLV_HEADER_T h
inner join DLV_DETAIL_T d
on h.DLV_HEADER_ID = d.DLV_HEADER_ID";

            if (shipBeginDateStatus != false)
            {
                cond.Add("@shipBeginDate <= h.TRIP_ACTUAL_SHIP_DATE");
                sqlParameterList.Add(new SqlParameter("@shipBeginDate", shipBeginDate) { SqlDbType = System.Data.SqlDbType.DateTime });
            }

            if (shipEndDateStatus != false)
            {
                cond.Add("h.TRIP_ACTUAL_SHIP_DATE <= @shipEndDate");
                sqlParameterList.Add(new SqlParameter("@shipEndDate", shipEndDate) { SqlDbType = System.Data.SqlDbType.DateTime });
            }
            if (DeliveryName != "")
            {
                cond.Add("h.DELIVERY_NAME = @DeliveryName");
                sqlParameterList.Add(new SqlParameter("@DeliveryName", DeliveryName));
            }
            if (SelectedSubinventory != "*")
            {
                cond.Add("h.SUBINVENTORY_CODE = @SelectedSubinventory");
                sqlParameterList.Add(new SqlParameter("@SelectedSubinventory", SelectedSubinventory));
            }
            if (SelectedTrip != "*")
            {
                cond.Add("h.TRIP_ID = @tripId");
                sqlParameterList.Add(new SqlParameter("@tripId", tripId));
            }
            if (transactionDateStatus != false)
            {
                cond.Add("h.TRANSACTION_DATE <= @tdate");
                sqlParameterList.Add(new SqlParameter("@tdate", tdate) { SqlDbType = System.Data.SqlDbType.DateTime });
            }
            if (SelectedDeliveryStatus != "*")
            {
                cond.Add("h.DELIVERY_STATUS_CODE <= @SelectedDeliveryStatus");
                sqlParameterList.Add(new SqlParameter("@SelectedDeliveryStatus", SelectedDeliveryStatus));
            }

            //string fullCmd = "";
            string commandText = string.Format(prefixCmd + "{0}{1}", cond.Count > 0 ? " WHERE " : "", string.Join(" AND ", cond.ToArray()));


            if (sqlParameterList.Count > 0)
            {
                return this.Context.Database.SqlQuery<TripHeaderDT>(commandText, sqlParameterList.ToArray()).ToList();

            }
            else
            {
                return this.Context.Database.SqlQuery<TripHeaderDT>(commandText).ToList();
            }
           
           

            //var query = dlvHeaderTRepositiory.GetAll().AsNoTracking()
            //    .Join(dlvDetailHtRepositiory.GetAll().AsNoTracking(),
            //    h => h.DlvHeaderId,
            //    d => d.DlvHeaderId,
            //     (h, d) => new TripHeaderDT
            //  {
            //      AUTHORIZE_DATE = h.AuthorizeDate != null ? ((DateTime)h.AuthorizeDate).ToString("yyyy-MM-dd") : "",
            //      CUSTOMER_LOCATION_CODE = h.CustomerLocationCode,
            //      CUSTOMER_NAME = h.CustomerName,
            //      DELIVERY_NAME = h.DeliveryName,
            //      DELIVERY_STATUS = h.DeliveryStatusName,
            //      DELIVERY_STATUS_CODE = h.DeliveryStatusCode,
            //      DetailType = h.ItemCategory,
            //      FREIGHT_TERMS_NAME = h.FreightTermsName,
            //      Id = h.DlvHeaderId,
            //      REMARK = h.Note,
            //      SUBINVENTORY_CODE = h.SubinventoryCode,
            //      SHIP_CUSTOMER_NAME = h.ShipCustomerName,
            //      SHIP_LOCATION_CODE = h.ShipLocationCode,
            //      TRIP_ACTUAL_SHIP_DATE = h.TripActualShipDate,
            //      TRANSACTION_DATE = h.TransactionDate,
            //      TRIP_CAR = h.TripCar,
            //      TRIP_ID = h.TripId,
            //      TRIP_NAME = h.TripName,
            //      REQUESTED_QUANTITY = d.RequestedQuantity,
            //      REQUESTED_QUANTITY_UOM = d.RequestedQuantityUom,
            //      REQUESTED_QUANTITY2 = d.RequestedQuantity2,
            //      REQUESTED_QUANTITY_UOM2 = d.RequestedQuantityUom2,
            //      SRC_REQUESTED_QUANTITY = d.SrcRequestedQuantity,
            //      SRC_REQUESTED_QUANTITY_UOM = d.SrcRequestedQuantityUom,
            //  })
            //  .Where(
            //  x =>
            //       (shipBeginDateStatus == false || shipBeginDate <= x.TRIP_ACTUAL_SHIP_DATE) &&
            //      (shipEndDateStatus == false || x.TRIP_ACTUAL_SHIP_DATE <= shipEndDate) &&
            //  (x.DELIVERY_NAME != null && x.DELIVERY_NAME.ToLower().Contains(DeliveryName.ToLower())) &&
            //  (SelectedSubinventory == "*" || x.SUBINVENTORY_CODE == SelectedSubinventory) &&
            //   (SelectedTrip == "*" || x.TRIP_ID == tripId) &&
            //    (transactionDateStatus == false || x.TRANSACTION_DATE == tdate) &&
            //     (SelectedDeliveryStatus == "*" || x.DELIVERY_STATUS_CODE == SelectedDeliveryStatus)
            //  );

            //return tempList;
        }
    }


}