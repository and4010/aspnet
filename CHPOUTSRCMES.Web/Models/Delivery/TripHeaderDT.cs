using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Delivery;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.DataModel.Entity.Delivery;
using static CHPOUTSRCMES.Web.DataModel.UnitOfWorks.DeliveryUOW;
using System.Security.Claims;
using NLog;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class TripHeaderDT
    {
        public long Id { get; set; }

        public long SUB_ID { get; set; }

        [Display(Name = "內銷區域別")]
        public string FREIGHT_TERMS_NAME { get; set; }


        [Display(Name = "交運單名稱")]
        public string DELIVERY_NAME { get; set; }


        [Display(Name = "客戶名稱")]
        public string CUSTOMER_NAME { get; set; }


        [Display(Name = "送貨地點")]
        public string CUSTOMER_LOCATION_CODE { get; set; }


        [Display(Name = "送貨客戶名稱")]
        public string SHIP_CUSTOMER_NAME { get; set; }


        [Display(Name = "送貨客戶地點")]
        public string SHIP_LOCATION_CODE { get; set; }


        //[Display(Name = "訂單編號")]
        //public long ORDER_NUMBER { get; set; }


        //[Display(Name = "訂單行號")]
        //public string ORDER_SHIP_NUMBER { get; set; }


        //[Display(Name = "料號名稱")]
        //public string ITEM_DESCRIPTION { get; set; }


        //[Display(Name = "紙別")]
        //public string PAPER_TYPE { get; set; }


        //[Display(Name = "基重")]
        //public string BASIC_WEIGHT { get; set; }


        //[Display(Name = "規格")]
        //public string SPECIFICATION { get; set; }


        //[Display(Name = "絲向")]
        //public string GRAIN_DIRECTION { get; set; }


        //[Display(Name = "包裝方式")]
        //public string PACKING_TYPE { get; set; }


        [Display(Name = "訂單原始數量")]
        public decimal SRC_REQUESTED_QUANTITY { get; set; }


        //[Display(Name = "訂單主單位")]
        //public string SRC_REQUESTED_QUANTITY_UOM { get; set; }


        //[Display(Name = "預計出庫輔數量")]
        //public decimal REQUESTED_QUANTITY2 { get; set; }


        //[Display(Name = "輔單位(RE)")]
        //public string REQUESTED_QUANTITY_UOM2 { get; set; }


        //[Display(Name = "預計出庫量")]
        //public decimal REQUESTED_QUANTITY { get; set; }


        //[Display(Name = "庫存單位(KG)")]
        //public string REQUESTED_QUANTITY_UOM { get; set; }


        [Display(Name = "出貨倉庫")]
        public string SUBINVENTORY_CODE { get; set; }


        //[Display(Name = "出貨倉庫名稱")]
        //public string SUBINVENTORY_NAME { get; set; }


        [Display(Name = "組車日")]
        public DateTime? TRIP_ACTUAL_SHIP_DATE { get; set; }

        [Display(Name = "航程號ID")]
        public long TRIP_ID { get; set; }


        [Display(Name = "航程號")]
        public string TRIP_NAME { get; set; }


        [Display(Name = "出貨申請日")]
        public DateTime? TRANSACTION_DATE { get; set; }

        [Display(Name = "出貨核准日")]
        public string AUTHORIZE_DATE { get; set; }


        [Display(Name = "備註")]
        public string NOTE { get; set; }


        [Display(Name = "交運單狀態")]
        public string DELIVERY_STATUS { get; set; }

        [Display(Name = "交運單狀態Code")]
        public string DELIVERY_STATUS_CODE { get; set; }

        [Display(Name = "明細作業別")]
        public string DetailType { get; set; }

        [Display(Name = "車次")]
        public string TRIP_CAR { get; set; }
    }


    public class TripHeaderData
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public static List<TripHeaderDT> source = new List<TripHeaderDT>();
        //public static List<TripDetailDT> model = new List<TripDetailDT>();

        public static void resetData()
        {
            source = new List<TripHeaderDT>();
            //model = new List<TripDetailDT>();
        }

        //public static void AddDefaultData()
        //{
        //    #region 產生資料
        //    source = new List<TripHeaderDT>();
        //    #region 平版
        //    source.Add(new TripHeaderDT()
        //    {
        //        Id = 1,
        //        //ORG_ID = 1,
        //        CUSTOMER_LOCATION_CODE = "福安印刷",
        //        CUSTOMER_NAME = "保吉",
        //        SHIP_LOCATION_CODE = "台南市安南區府安路5段119巷",
        //        SHIP_CUSTOMER_NAME = "保吉紙業有限公司",
        //        TRIP_ID = 1,
        //        DELIVERY_NAME = "FTY1912000547",
        //        TRIP_CAR = "PN01",
        //        FREIGHT_TERMS_NAME = "台南",
        //        //ITEM_DESCRIPTION = "A001",
        //        //ORDER_NUMBER = 1,
        //        //ORDER_SHIP_NUMBER = "OSN001",
        //        DELIVERY_STATUS = "未印",
        //        DetailType = "平版",
        //        //PAPER_TYPE = "塗佈白紙板",
        //        //BASIC_WEIGHT = "03500",
        //        //SPECIFICATION = "214K512K",
        //        //GRAIN_DIRECTION = "L",
        //        //PACKING_TYPE = "無令打件",
        //        //訂單原始數量
        //        SRC_REQUESTED_QUANTITY = 1.33742M,
        //        //訂單主單位
        //        SRC_REQUESTED_QUANTITY_UOM = "MT",
        //        //預計出庫輔數量
        //        REQUESTED_QUANTITY2 = 50,
        //        //輔單位(RE)
        //        REQUESTED_QUANTITY_UOM2 = "RE",
        //        //預計出庫量
        //        REQUESTED_QUANTITY = 1337.419M,
        //        //庫存單位(KG)
        //        REQUESTED_QUANTITY_UOM = "KG",
        //        //出貨倉庫
        //        SUBINVENTORY_CODE = "TB2",
        //        //出貨倉庫名稱
        //        //SUBINVENTORY_NAME = "TB2",
        //        //組車日
        //        TRIP_ACTUAL_SHIP_DATE = Convert.ToDateTime("2019-12-26"),
        //        //航程號
        //        TRIP_NAME = "Y191226-1036357",
        //        //預計出貨確認日
        //        TRANSACTION_DATE = Convert.ToDateTime("2019-12-26"),
        //        //出貨核准日
        //        AUTHORIZE_DATE = "2019-12-26",
        //        //備註
        //        NOTE = "FT1.P9B0288",
        //    });
        //    #endregion

        //    #region 捲筒
        //    source.Add(new TripHeaderDT()
        //    {
        //        Id = 2,
        //        //ORG_ID = 7,
        //        CUSTOMER_LOCATION_CODE = "中華彩色",
        //        CUSTOMER_NAME = "中華彩色",
        //        SHIP_LOCATION_CODE = "新北市新店區寶橋路229號",
        //        SHIP_CUSTOMER_NAME = "中華彩色印刷股份有限公司",
        //        TRIP_ID = 2,
        //        DELIVERY_NAME = "FTY2001000140",
        //        TRIP_CAR = "PTB2",
        //        FREIGHT_TERMS_NAME = "台北",
        //        //ITEM_DESCRIPTION = "A006",
        //        //ORDER_NUMBER = 6,
        //        //ORDER_SHIP_NUMBER = "OSN006",
        //        DELIVERY_STATUS = "未印",
        //        DetailType = "平版",
        //        //PAPER_TYPE = "塗佈白紙板",
        //        //BASIC_WEIGHT = "03500",
        //        //SPECIFICATION = "214K512K",
        //        //GRAIN_DIRECTION = "L",
        //        //PACKING_TYPE = "無令打件",
        //        //訂單原始數量
        //        SRC_REQUESTED_QUANTITY = 0.37489M,
        //        //訂單主單位
        //        SRC_REQUESTED_QUANTITY_UOM = "MT",
        //        //預計出庫輔數量
        //        REQUESTED_QUANTITY2 = 19,
        //        //輔單位(RE)
        //        REQUESTED_QUANTITY_UOM2 = "RE",
        //        //預計出庫量
        //        REQUESTED_QUANTITY = 374.8945M,
        //        //庫存單位(KG)
        //        REQUESTED_QUANTITY_UOM = "KG",
        //        //出貨倉庫
        //        SUBINVENTORY_CODE = "TB2",
        //        //出貨倉庫名稱
        //        //SUBINVENTORY_NAME = "TB2",
        //        //組車日
        //        TRIP_ACTUAL_SHIP_DATE = Convert.ToDateTime("2020-01-09"),
        //        //航程號
        //        TRIP_NAME = "Y200109-1052058",
        //        //預計出貨確認日
        //        TRANSACTION_DATE = Convert.ToDateTime("2020-01-09"),
        //        //出貨核准日
        //        AUTHORIZE_DATE = "2020-01-09",
        //        //備註
        //        NOTE = "FT1.早上到X002010031大道季刊98期/P2010087",
        //    });

        //    #endregion

        //    #region 捲筒
        //    source.Add(new TripHeaderDT()
        //    {
        //        Id = 3,
        //        //ORG_ID = 8,
        //        CUSTOMER_LOCATION_CODE = "中華彩色",
        //        CUSTOMER_NAME = "中華彩色",
        //        SHIP_LOCATION_CODE = "新北市新店區寶橋路229號",
        //        SHIP_CUSTOMER_NAME = "中華彩色印刷股份有限公司",
        //        TRIP_ID = 3,
        //        DELIVERY_NAME = "FTY2001000152",
        //        TRIP_CAR = "PTB2",
        //        FREIGHT_TERMS_NAME = "台北",
        //        //ITEM_DESCRIPTION = "A006",
        //        //ORDER_NUMBER = 6,
        //        //ORDER_SHIP_NUMBER = "OSN006",
        //        DELIVERY_STATUS = "未印",
        //        DetailType = "捲筒",
        //        //PAPER_TYPE = "塗佈白紙板",
        //        //BASIC_WEIGHT = "03500",
        //        //SPECIFICATION = "214K512K",
        //        //GRAIN_DIRECTION = "L",
        //        //PACKING_TYPE = "無令打件",
        //        //訂單原始數量
        //        SRC_REQUESTED_QUANTITY = 3,
        //        //訂單主單位
        //        SRC_REQUESTED_QUANTITY_UOM = "MT",
        //        //預計出庫輔數量
        //        REQUESTED_QUANTITY2 = 0,
        //        //輔單位(RE)
        //        REQUESTED_QUANTITY_UOM2 = "",
        //        //預計出庫量
        //        REQUESTED_QUANTITY = 3000,
        //        //庫存單位(KG)
        //        REQUESTED_QUANTITY_UOM = "KG",
        //        //出貨倉庫
        //        SUBINVENTORY_CODE = "SFG",
        //        //出貨倉庫名稱
        //        //SUBINVENTORY_NAME = "SFG",
        //        //組車日
        //        TRIP_ACTUAL_SHIP_DATE = Convert.ToDateTime("2020-04-22"),
        //        //航程號
        //        TRIP_NAME = "Y200109-1052060",
        //        //預計出貨確認日
        //        TRANSACTION_DATE = Convert.ToDateTime("2020-04-22"),
        //        //出貨核准日
        //        AUTHORIZE_DATE = "2020-04-22",
        //        //備註
        //        NOTE = "",
        //    });

        //    #endregion

        //    #endregion

        //}

        public static List<TripHeaderDT> GetData(int id)
        {
            var query = from tripDetail in source
                        where tripDetail.Id == id
                        select tripDetail;
            return query.ToList<TripHeaderDT>();
        }

        public static List<TripHeaderDT> GetData()
        {
            var query = from tripDetail in source
                        select tripDetail;
            return query.ToList<TripHeaderDT>();
        }

        public List<TripHeaderDT> DeliverySearch(DeliveryUOW uow, string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus, string userId)
        {
            return uow.DeliverySearch(TripActualShipBeginDate, TripActualShipEndDate, DeliveryName, SelectedSubinventory, SelectedTrip
                , TransactionDate, SelectedDeliveryStatus, userId);
        }

        public static List<TripHeaderDT> Search(string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus)
        {


            DateTime shipBeginDate = new DateTime();
            DateTime shipEndDate = new DateTime();
            DateTime tdate = new DateTime();

            bool shipBeginDateStatus = DateTime.TryParseExact(TripActualShipBeginDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out shipBeginDate);
            bool shipEndDateStatus = DateTime.TryParseExact(TripActualShipEndDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out shipEndDate);
            bool transactionDateStatus = DateTime.TryParseExact(TransactionDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out tdate);



            //var query = from tripDetail in source
            //            where
            //            ((DeliveryName != null && !DeliveryName.Equals("")) && DeliveryName.ToLower().Contains(tripDetail.DELIVERY_NAME.ToLower())) &&
            //            ((SelectedSubinventory != null && !SelectedSubinventory.Equals("")) && SelectedSubinventory.ToLower().Contains(tripDetail.SUBINVENTORY_CODE.ToLower())) &&
            //            ((SelectedTrip != null && !SelectedTrip.Equals("")) && SelectedTrip.ToLower().Contains(tripDetail.TRIP_NAME.ToLower())) &&
            //            ((TransactionDate != null && !TransactionDate.Equals("")) && tdate == tripDetail.TRANSACTION_DATE) &&
            //            ((SelectedDeliveryStatus != null && !SelectedDeliveryStatus.Equals("")) && SelectedDeliveryStatus.ToLower().Contains(tripDetail.DELIVERY_STATUS.ToLower()))
            //            select tripDetail;
            //
            //var query = from tripDetail in source 
            //            where ((DeliveryName != null && !DeliveryName.Equals("")) || DeliveryName == tripDetail.DELIVERY_NAME)
            //            select tripDetail;

            var query = source.Where(
              x =>
                   (shipBeginDateStatus == false || shipBeginDate <= x.TRIP_ACTUAL_SHIP_DATE) &&
                  (shipEndDateStatus == false || x.TRIP_ACTUAL_SHIP_DATE <= shipEndDate) &&
              (x.DELIVERY_NAME != null && x.DELIVERY_NAME.ToLower().Contains(DeliveryName.ToLower())) &&
              (SelectedSubinventory == "*" || x.SUBINVENTORY_CODE == SelectedSubinventory) &&
               (SelectedTrip == "*" || x.TRIP_NAME == SelectedTrip) &&
                (transactionDateStatus == false || x.TRANSACTION_DATE == tdate) &&
                 (SelectedDeliveryStatus == "*" || x.DELIVERY_STATUS == SelectedDeliveryStatus)
              ).ToList();



            //if (!string.IsNullOrEmpty(DeliveryName))
            //{
            //    query = query.Where(DeliveryName == tripDetail.DELIVERY_NAME);
            //}

            //            where
            //            ((DeliveryName != null && !DeliveryName.Equals("")) && DeliveryName == tripDetail.DELIVERY_NAME) &&
            //            ((SelectedSubinventory != null && !SelectedSubinventory.Equals("")) && SelectedSubinventory == tripDetail.SUBINVENTORY_CODE) &&
            //            ((SelectedTrip != null && !SelectedTrip.Equals("")) && SelectedTrip == tripDetail.TRIP_NAME) &&
            //            ((TransactionDate != null && !TransactionDate.Equals("")) && tdate == tripDetail.TRANSACTION_DATE) &&
            //            ((SelectedDeliveryStatus != null && !SelectedDeliveryStatus.Equals("")) && SelectedDeliveryStatus == tripDetail.DELIVERY_STATUS)

            //            select tripDetail;


            return query;
        }

        public DeliverySearchViewModel GetDeliverySearchViewModel(DeliveryUOW uow, List<Claim> roles, string userId)
        {
            DeliverySearchViewModel viewModel = new DeliverySearchViewModel();
            //viewModel.SelectedDeliveryStatus = "";
            //viewModel.DeliveryName = "";
            //viewModel.TransactionDate = "";
            //viewModel.TripActualShipBeginDate = "";
            //viewModel.TripActualShipEndDate = "";

            //viewModel.SelectedTrip = "*";
            //List<ListItem> tripList = new List<ListItem>();
            //tripList.Add(new ListItem("全部", "*"));
            //tripList.Add(new ListItem("Y191226-1036357", "Y191226-1036357"));
            //tripList.Add(new ListItem("Y200109-1052058", "Y200109-1052058"));
            //tripList.Add(new ListItem("Y200109-1052060", "Y200109-1052060"));

            //viewModel.TripNameItems = tripList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });

            viewModel.TripNameItems = uow.GetTripNameDropDownList(MasterUOW.DropDownListType.All, userId);
            //viewModel.SelectedSubinventory = "*";
            OrgSubinventoryData orgData = new OrgSubinventoryData();
            viewModel.SubinventoryNameItems = orgData.GetSubinventoryListForUserId(uow, userId, MasterUOW.DropDownListType.All);
          


            ////viewModel.SelectedDeliveryStatus = "*";
            //List<ListItem> deliveryStatusList = new List<ListItem>();
            //deliveryStatusList.Add(new ListItem("全部", "*"));
            //deliveryStatusList.Add(new ListItem("取消", "取消"));
            //deliveryStatusList.Add(new ListItem("未印", "未印"));
            //deliveryStatusList.Add(new ListItem("待出", "待出"));
            //deliveryStatusList.Add(new ListItem("已揀", "已揀"));
            //deliveryStatusList.Add(new ListItem("待核准", "待核准"));
            //deliveryStatusList.Add(new ListItem("已出貨", "已出貨"));

            //viewModel.DeliveryStatusItems = deliveryStatusList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });

            viewModel.DeliveryStatusItems = uow.GetDeliveryStatusDropDownList(MasterUOW.DropDownListType.All);

            if (roles != null && roles.Count > 0)
            {
                foreach (Claim role in roles)
                {
                    if (role.Value == MasterUOW.UserRole.Adm || role.Value == MasterUOW.UserRole.ChpUser)
                    {
                        viewModel.Advanced = true;
                        break;
                    }
                    else
                    {
                        viewModel.Advanced = false;
                    }
                }
            }

            return viewModel;
        }

        public DeliveryDetailViewHeader GetDeliveryDetailViewHeader(DeliveryUOW uow, long dlvHeaderId)
        {
            DeliveryDetailViewHeader viewModel = new DeliveryDetailViewHeader();
            var headerDataList = uow.GetDeliveryHeaderDataListFromHeaderId(dlvHeaderId);
            if (headerDataList.Count > 0)
            {
                viewModel.CUSTOMER_LOCATION_CODE = headerDataList[0].CustomerLocationCode;
                viewModel.CUSTOMER_NAME = headerDataList[0].CustomerName;
                viewModel.DELIVERY_NAME = headerDataList[0].DeliveryName;
                viewModel.DELIVERY_STATUS = headerDataList[0].DeliveryStatusName;
                viewModel.DlvHeaderId = headerDataList[0].DlvHeaderId;
                viewModel.REMARK = headerDataList[0].Note;
                viewModel.SHIP_CUSTOMER_NAME = headerDataList[0].ShipCustomerName;
                viewModel.SHIP_LOCATION_CODE = headerDataList[0].ShipLocationCode;
                viewModel.TRIP_ACTUAL_SHIP_DATE = headerDataList[0].TripActualShipDate;
                viewModel.TRIP_CAR = headerDataList[0].TripCar;
                viewModel.TRIP_NAME = headerDataList[0].TripName;
            }
            return viewModel;
        }

        public FlatEditViewModel GetFlatEditViewModel(DeliveryUOW uow, long dlvHeaderId)
        {
            FlatEditViewModel viewModel = new FlatEditViewModel();
            viewModel.DeliveryDetailViewHeader = GetDeliveryDetailViewHeader(uow, dlvHeaderId);
            return viewModel;
        }

        public FlatViewModel GetFlatViewModel(DeliveryUOW uow, long dlvHeaderId)
        {
            FlatViewModel viewModel = new FlatViewModel();
            viewModel.DeliveryDetailViewHeader = GetDeliveryDetailViewHeader(uow, dlvHeaderId);
            return viewModel;
        }

        public PaperRollEditViewModel GetPaperRollEditViewModel(DeliveryUOW uow, long dlvHeaderId)
        {

            PaperRollEditViewModel viewModel = new PaperRollEditViewModel();
            viewModel.DeliveryDetailViewHeader = GetDeliveryDetailViewHeader(uow, dlvHeaderId);
            return viewModel;
        }

        public PaperRollViewModel GetPaperRollViewModel(DeliveryUOW uow, long dlvHeaderId)
        {
            PaperRollViewModel viewModel = new PaperRollViewModel();
            viewModel.DeliveryDetailViewHeader = GetDeliveryDetailViewHeader(uow, dlvHeaderId);
            return viewModel;
        }

        //public ResultModel DeliveryConfirm(DeliveryUOW uow, List<long> ids, string userId, string userName)
        //{
        //   var result =  ChangeDeliveryConfirm(uow, ids, DeliveryStatusCode.UnAuthorized, userId, userName);
        //    if (result.Success)
        //    {
        //        return new ResultModel(true, "出貨申請成功");
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}

        public ResultModel DeliveryConfirm(DeliveryUOW uow, List<long> ids, string userId, string userName)
        {
            List<long> tripIds = uow.GetTripIdList(ids);
            if (tripIds.Count == 0)
            {
                return new ResultModel(false, "找不到航程號資料 ");
            }

            var updateDatas = uow.GetDeliveryHeaderDataListFromTripId(tripIds);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到航程號資料");

            //檢查交運單狀態
            foreach (DLV_HEADER_T data in updateDatas)
            {
                if (data.DeliveryStatusCode != DeliveryUOW.DeliveryStatusCode.Picked)
                {
                    return new ResultModel(false, "交運單" + data.DeliveryName + "狀態須為" + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.Picked));
                }
            }

            ResultModel result = uow.ChangeDeliveryConfirm(updateDatas, DeliveryStatusCode.UnAuthorized, userId, userName);
            if (result.Success)
            {
                return new ResultModel(true, "出貨申請成功");
            }
            else
            {
                return result;
            }

        }

        public ResultModel CancelConfirm(DeliveryUOW uow, List<long> ids, string userId, string userName)
        {
            List<long> tripIds = uow.GetTripIdList(ids);
            if (tripIds.Count == 0)
            {
                return new ResultModel(false, "找不到航程號資料 ");
            }

            var updateDatas = uow.GetDeliveryHeaderDataListFromTripId(tripIds);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到航程號資料");

            //檢查交運單狀態
            foreach (DLV_HEADER_T data in updateDatas)
            {
                if (data.DeliveryStatusCode != DeliveryUOW.DeliveryStatusCode.UnAuthorized)
                {
                    return new ResultModel(false, "交運單" + data.DeliveryName + "狀態須為" + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.UnAuthorized));
                }
            }

            ResultModel result = uow.ChangeDeliveryConfirm(updateDatas, DeliveryUOW.DeliveryStatusCode.Picked, userId, userName);
            if (result.Success)
            {
                return new ResultModel(true, "取消申請成功");
            }
            else
            {
                return result;
            }


            //var query = from tripDetail in source
            //            where ids.Contains(tripDetail.Id)
            //            group tripDetail by new
            //            {
            //                tripDetail.TRIP_ID
            //            } into g
            //            select new
            //            {
            //                g.Key.TRIP_ID
            //            };

            //ResultModel result = new ResultModel(true, "取消出貨確認成功");

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    if (result.Success == false)
            //    {
            //        break;
            //    }
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS != "待核准")
            //            {
            //                result.Success = false;
            //                result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為待核准";
            //                break;
            //            }
            //        }
            //    }
            //}

            //if (result.Success == false)
            //{
            //    return result;
            //}

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS == "待核准")
            //            {
            //                sourceData.DELIVERY_STATUS = "已揀";
            //            }
            //        }
            //    }
            //}

            //return result;

        }

        public ResultModel PrintPickList(DeliveryUOW uow, List<long> ids, string userId, string userName)
        {
            //列印備貨單 待寫

            //列印完後更新狀態
            List<long> tripIds = uow.GetTripIdList(ids);
            if (tripIds.Count == 0)
            {
                return new ResultModel(false, "找不到航程號資料 ");
            }

            var updateDatas = uow.GetDeliveryHeaderDataListFromTripId(tripIds);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到航程號資料");

            //檢查交運單狀態
            foreach (DLV_HEADER_T data in updateDatas)
            {
                if (!(data.DeliveryStatusCode == DeliveryUOW.DeliveryStatusCode.Unprinted || data.DeliveryStatusCode == DeliveryStatusCode.UnPicked))
                {
                    return new ResultModel(false, "交運單" + data.DeliveryName + "狀態須為" + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.Unprinted) + "或" + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.UnPicked));
                }
            }

            ResultModel result = uow.UpdateDeliveryStatus(updateDatas, DeliveryUOW.DeliveryStatusCode.UnPicked, userId, userName);
            if (result.Success)
            {
                return new ResultModel(true, "列印備貨單成功");
            }
            else
            {
                return result;
            }


            //var query = from tripDetail in source
            //            where ids.Contains(tripDetail.Id)
            //            group tripDetail by new
            //            {
            //                tripDetail.TRIP_ID
            //            } into g
            //            select new
            //            {
            //                g.Key.TRIP_ID
            //            };

            //ResultModel result = new ResultModel(true, "列印備貨單成功");

            //foreach (TripHeaderDT obj in source)
            //{
            //    //foreach (var q in query)
            //    //{
            //    //    if (obj.TRIP_ID == q.TRIP_ID)
            //    //    {
            //    //        if (obj.DELIVERY_STATUS != "未印")
            //    //        {
            //    //            result.Success = false;
            //    //            result.Msg = "交運單" + obj.DELIVERY_NAME + "狀態須為未印";
            //    //            break;
            //    //        }
            //    //    }
            //    //}

            //    //if (!result.Success)
            //    //{
            //    //    break;
            //    //}

            //    foreach (var q in query)
            //    {
            //        if (obj.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (obj.DELIVERY_STATUS == "未印")
            //            {
            //                obj.DELIVERY_STATUS = "待出";
            //            }
            //        }
            //    }
            //}

            //return result;
        }

        public ResultModel DeliveryAuthorize(DeliveryUOW uow, TripDetailDTEditor selectDatas, string userId, string userName)
        {
            List<long> tripIds = selectDatas.TripDetailDTList.Select(x => x.TRIP_ID).ToList();
            if (tripIds.Count == 0)
            {
                return new ResultModel(false, "找不到航程號資料 ");
            }
            var updateDatas = uow.GetDeliveryHeaderDataListFromTripId(tripIds);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到航程號資料");

            //檢查交運單狀態
            foreach (DLV_HEADER_T data in updateDatas)
            {
                if (data.DeliveryStatusCode != DeliveryUOW.DeliveryStatusCode.UnAuthorized)
                {
                    return new ResultModel(false, "交運單" + data.DeliveryName + "狀態須為" + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.UnAuthorized));
                }
            }

            DateTime AuthorizeDate = Convert.ToDateTime(selectDatas.TripDetailDTList[0].AUTHORIZE_DATE);

            return uow.DeliveryAuthorize(updateDatas, AuthorizeDate, userId, userName);




            //var query = from tripDetail in source
            //            where ids.Contains(tripDetail.Id)
            //            group tripDetail by new
            //            {
            //                tripDetail.TRIP_ID
            //            } into g
            //            select new
            //            {
            //                g.Key.TRIP_ID
            //            };

            //ResultModel result = new ResultModel(true, "出貨核准成功");

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    if (result.Success == false)
            //    {
            //        break;
            //    }
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS != "待核准")
            //            {
            //                result.Success = false;
            //                result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為待核准";
            //                break;
            //            }
            //        }
            //    }
            //}

            //if (result.Success == false)
            //{
            //    return result;
            //}

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS == "待核准")
            //            {
            //                sourceData.DELIVERY_STATUS = "已出貨";
            //            }
            //        }
            //    }
            //}

            //return result;

        }

        public ResultModel CancelAuthorize(DeliveryUOW uow, List<long> ids, string userId, string userName)
        {
            List<long> tripIds = uow.GetTripIdList(ids);
            if (tripIds.Count == 0)
            {
                return new ResultModel(false, "找不到航程號資料 ");
            }

            var updateDatas = uow.GetDeliveryHeaderDataListFromTripId(tripIds);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到航程號資料");

            //檢查交運單狀態
            foreach (DLV_HEADER_T data in updateDatas)
            {
                if (data.DeliveryStatusCode != DeliveryUOW.DeliveryStatusCode.UnAuthorized)
                {
                    return new ResultModel(false, "交運單" + data.DeliveryName + "狀態須為" + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.UnAuthorized));
                }
            }

            ResultModel result = uow.UpdateDeliveryStatus(updateDatas, DeliveryUOW.DeliveryStatusCode.Picked, userId, userName);
            if (result.Success)
            {
                return new ResultModel(true, "取消核准成功");
            }
            else
            {
                return result;
            }

            //var query = from tripDetail in source
            //            where ids.Contains(tripDetail.Id)
            //            group tripDetail by new
            //            {
            //                tripDetail.TRIP_ID
            //            } into g
            //            select new
            //            {
            //                g.Key.TRIP_ID
            //            };

            //ResultModel result = new ResultModel(true, "取消出貨核准成功");

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    if (result.Success == false)
            //    {
            //        break;
            //    }
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS != "待核准")
            //            {
            //                result.Success = false;
            //                result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為待核准";
            //                break;
            //            }
            //        }
            //    }
            //}

            //if (result.Success == false)
            //{
            //    return result;
            //}

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS == "待核准")
            //            {
            //                sourceData.DELIVERY_STATUS = "已揀";
            //            }
            //        }
            //    }
            //}

            //return result;

        }

        public ResultModel CancelTrip(DeliveryUOW uow, List<long> ids, string userId, string userName)
        {
            List<long> tripIds = uow.GetTripIdList(ids);
            if (tripIds.Count == 0)
            {
                return new ResultModel(false, "找不到航程號資料 ");
            }

            var updateDatas = uow.GetDeliveryHeaderDataListFromTripId(tripIds);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到航程號資料");

            //檢查交運單狀態
            foreach (DLV_HEADER_T data in updateDatas)
            {
                if (data.DeliveryStatusCode == DeliveryUOW.DeliveryStatusCode.Canceled)
                {
                    return new ResultModel(false, "航程號" + data.DeliveryName + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.Canceled));
                }
                if (data.DeliveryStatusCode == DeliveryUOW.DeliveryStatusCode.Shipped)
                {
                    return new ResultModel(false, "航程號" + data.DeliveryName + uow.deliveryStatusCode.GetDesc(DeliveryUOW.DeliveryStatusCode.Shipped));
                }

            }

            ResultModel result = uow.UpdateDeliveryStatus(updateDatas, DeliveryUOW.DeliveryStatusCode.Canceled, userId, userName);
            if (result.Success)
            {
                return new ResultModel(true, "取消航程號成功");
            }
            else
            {
                return result;
            }


            //var query = from tripDetail in source
            //            where ids.Contains(tripDetail.Id)
            //            group tripDetail by new
            //            {
            //                tripDetail.TRIP_ID
            //            } into g
            //            select new
            //            {
            //                g.Key.TRIP_ID
            //            };

            //ResultModel result = new ResultModel(true, "取消航程號成功");

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    if (result.Success == false)
            //    {
            //        break;
            //    }
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            if (sourceData.DELIVERY_STATUS == "取消")
            //            {
            //                result.Success = false;
            //                result.Msg = "航程號" + sourceData.TRIP_NAME + "已取消";
            //                break;
            //            }
            //        }
            //    }
            //}

            //foreach (TripHeaderDT sourceData in source)
            //{
            //    foreach (var q in query)
            //    {
            //        if (sourceData.TRIP_ID == q.TRIP_ID)
            //        {
            //            sourceData.DELIVERY_STATUS = "取消";
            //        }
            //    }
            //}

            //return result;

        }

        public static void ChangeDeliveryStatus(long DlvHeaderId, bool pickComplete)
        {

            foreach (TripHeaderDT obj in source)
            {
                if (obj.Id == DlvHeaderId)
                {
                    if (pickComplete)
                    {
                        if (obj.DELIVERY_STATUS == "待出")
                        {
                            obj.DELIVERY_STATUS = "已揀";
                        }
                    }
                    else
                    {
                        if (obj.DELIVERY_STATUS == "已揀")
                        {
                            obj.DELIVERY_STATUS = "待出";
                        }
                    }
                }
            }

        }

        public ResultModel AddPickDT(DeliveryUOW uow, long dlvHeaderId, long dlvDetailId, string deliveryName, string barcode, decimal? qty, string addUser, string addUserName)
        {
            return uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, qty, addUser, addUserName);
            //var addResult = uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, qty, addUser, addUserName, status);
            //if (!addResult.Success)
            //{
            //    return addResult;
            //}
            //return CheckPicked(uow, dlvHeaderId, addUser, addUserName);
        }

        public ResultModel CheckPicked(DeliveryUOW uow, long dlvHeaderId, string userId, string userName)
        {
            var pickedResult = uow.CheckPicked(dlvHeaderId);
            if (!pickedResult.Success)
            {
                return pickedResult;
            }
            var updateDatas = uow.GetDeliveryHeaderDataListFromHeaderId(dlvHeaderId);
            if (updateDatas.Count == 0) return new ResultModel(false, "找不到交運單資料");

            return uow.UpdateDeliveryStatus(updateDatas, pickedResult.Msg, userId, userName);
        }

        public ResultModel DelPickDT(DeliveryUOW uow, List<long> dlvPickedId, string userId, string userName)
        {
            var pickedDataList = uow.GetDeliveryPickDataListFromPickedId(dlvPickedId);
            if (pickedDataList.Count == 0) return new ResultModel(false, "揀貨明細找不到資料");
            if (pickedDataList.Count != dlvPickedId.Count) return new ResultModel(false, "刪除揀貨明細數量比對錯誤");

            return uow.DelPickDT(pickedDataList, userId, userName);
            //var delResult = uow.DelPickDT(pickedDataList, userId, userName);

            //if (!delResult.Success)
            //{
            //    return delResult;
            //}
            //return CheckPicked(uow, pickedDataList[0].DlvHeaderId, userId, userName);
        }

        //public static ResultModel UpdateTransactionAuthorizeDates(TripDetailDTEditor data)
        //{
        //    if (data == null)
        //    {
        //        return new ResultModel(false, "更改出貨核准日失敗，資料來源為空");
        //    }

        //    int updatedCount = 0;

        //    foreach (var sourceTripDetailDT in source)
        //    {
        //        foreach (var selectedData in data.TripDetailDTList){
        //            if (sourceTripDetailDT.Id == selectedData.Id){
        //                sourceTripDetailDT.TRANSACTION_AUTHORIZE_DATE = selectedData.TRANSACTION_AUTHORIZE_DATE;
        //                updatedCount++;
        //            }
        //        }
        //    }

        //    if (updatedCount == 0)
        //    {
        //        return new ResultModel(false, "更改出貨核准日失敗，全部資料比對不到");
        //    }
        //    else if (updatedCount != data.TripDetailDTList.Count)
        //    {
        //        return new ResultModel(false, "更改出貨核准日失敗，部分資料比對不到");
        //    }
        //    else
        //    {
        //        return new ResultModel(true, "更改出貨核准日成功");
        //    }

        //}

        public ResultModel UpdateTransactionAuthorizeDates(DeliveryUOW uow, TripDetailDTEditor data, string userId, string userNmae)
        {
            return uow.UpdateTransactionAuthorizeDates(data, userId, userNmae);

            //List<TripHeaderDT> result = new List<TripHeaderDT>();

            //foreach (var sourceTripDetailDT in source)
            //{
            //    foreach (var selectedData in data.TripDetailDTList)
            //    {
            //        if (sourceTripDetailDT.TRIP_ID == selectedData.TRIP_ID)
            //        {
            //            sourceTripDetailDT.AUTHORIZE_DATE = selectedData.AUTHORIZE_DATE;
            //            result.Add(sourceTripDetailDT);
            //        }
            //    }
            //}

            //return result;

        }

        public ActionResult PritLabel(DeliveryUOW uow, List<long> PICKED_IDs, string userName)
        {
            var resultData = uow.GetLabels(PICKED_IDs, userName);
            return uow.PrintLabel(resultData.Data);
        }
    }

    internal class TripDetailDTOrder
    {
        public static IOrderedEnumerable<TripHeaderDT> Order(List<Order> orders, IEnumerable<TripHeaderDT> models)
        {
            IOrderedEnumerable<TripHeaderDT> orderedModel = null;
            if (orders.Count() > 0)
            {
                orderedModel = OrderBy(orders[0].Column, orders[0].Dir, models);
            }

            for (int i = 1; i < orders.Count(); i++)
            {
                orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
            }
            return orderedModel;
        }


        private static IOrderedEnumerable<TripHeaderDT> OrderBy(int column, string dir, IEnumerable<TripHeaderDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.FREIGHT_TERMS_NAME) : models.OrderBy(x => x.FREIGHT_TERMS_NAME);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRIP_NAME) : models.OrderBy(x => x.TRIP_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DELIVERY_NAME) : models.OrderBy(x => x.DELIVERY_NAME);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DetailType) : models.OrderBy(x => x.DetailType);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DELIVERY_STATUS) : models.OrderBy(x => x.DELIVERY_STATUS);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CUSTOMER_NAME) : models.OrderBy(x => x.CUSTOMER_NAME);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CUSTOMER_LOCATION_CODE) : models.OrderBy(x => x.CUSTOMER_LOCATION_CODE);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUBINVENTORY_CODE) : models.OrderBy(x => x.SUBINVENTORY_CODE);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRIP_ACTUAL_SHIP_DATE) : models.OrderBy(x => x.TRIP_ACTUAL_SHIP_DATE);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRANSACTION_DATE) : models.OrderBy(x => x.TRANSACTION_DATE);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.AUTHORIZE_DATE) : models.OrderBy(x => x.AUTHORIZE_DATE);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.NOTE) : models.OrderBy(x => x.NOTE);


            }
        }

        private static IOrderedEnumerable<TripHeaderDT> ThenBy(int column, string dir, IOrderedEnumerable<TripHeaderDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.FREIGHT_TERMS_NAME) : models.ThenBy(x => x.FREIGHT_TERMS_NAME);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRIP_NAME) : models.ThenBy(x => x.TRIP_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DELIVERY_NAME) : models.ThenBy(x => x.DELIVERY_NAME);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DetailType) : models.ThenBy(x => x.DetailType);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DELIVERY_STATUS) : models.ThenBy(x => x.DELIVERY_STATUS);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CUSTOMER_NAME) : models.ThenBy(x => x.CUSTOMER_NAME);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CUSTOMER_LOCATION_CODE) : models.ThenBy(x => x.CUSTOMER_LOCATION_CODE);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUBINVENTORY_CODE) : models.ThenBy(x => x.SUBINVENTORY_CODE);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRIP_ACTUAL_SHIP_DATE) : models.ThenBy(x => x.TRIP_ACTUAL_SHIP_DATE);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRANSACTION_DATE) : models.ThenBy(x => x.TRANSACTION_DATE);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.AUTHORIZE_DATE) : models.ThenBy(x => x.AUTHORIZE_DATE);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.NOTE) : models.ThenBy(x => x.NOTE);


            }
        }
    }

    public class TripDetailDTEditor
    {
        public string Action { get; set; }
        //public List<long> TripDetailDT_IDs { get; set; }
        //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
        public List<TripHeaderDT> TripDetailDTList { get; set; }
    }

    public class PickDTEditor
    {
        public string Action { get; set; }
        public List<long> DlvPickedIdList { get; set; }
    }
}