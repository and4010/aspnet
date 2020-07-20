﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Delivery;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class TripDetailDT
    {
        public long Id { get; set; }

        public long ORG_ID { get; set; }

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


        [Display(Name = "訂單主單位")]
        public string SRC_REQUESTED_QUANTITY_UOM { get; set; }


        [Display(Name = "預計出庫輔數量")]
        public decimal REQUESTED_QUANTITY2 { get; set; }


        [Display(Name = "輔單位(RE)")]
        public string SRC_REQUESTED_QUANTITY_UOM2 { get; set; }


        [Display(Name = "預計出庫量")]
        public decimal REQUESTED_QUANTITY { get; set; }


        [Display(Name = "庫存單位(KG)")]
        public string REQUESTED_QUANTITY_UOM { get; set; }


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
        public string REMARK { get; set; }


        [Display(Name = "交運單狀態")]
        public string DELIVERY_STATUS { get; set; }


        [Display(Name = "明細作業別")]
        public string DetailType { get; set; }

        [Display(Name = "車次")]
        public string TRIP_CAR { get; set; }
    }


    public class TripDetailData
    {
        public static List<TripDetailDT> source = new List<TripDetailDT>();
        //public static List<TripDetailDT> model = new List<TripDetailDT>();

        public static void resetData()
        {
            source = new List<TripDetailDT>();
            //model = new List<TripDetailDT>();
        }

        public static void AddDefaultData()
        {
            #region 產生資料
            source = new List<TripDetailDT>();
            #region 平版
            source.Add(new TripDetailDT()
            {
                Id = 1,
                ORG_ID = 1,
                CUSTOMER_LOCATION_CODE = "福安印刷",
                CUSTOMER_NAME = "保吉",
                SHIP_LOCATION_CODE = "台南市安南區府安路5段119巷",
                SHIP_CUSTOMER_NAME = "保吉紙業有限公司",
                TRIP_ID = 1,
                DELIVERY_NAME = "FTY1912000547",
                TRIP_CAR = "PN01",
                FREIGHT_TERMS_NAME = "台南",
                //ITEM_DESCRIPTION = "A001",
                //ORDER_NUMBER = 1,
                //ORDER_SHIP_NUMBER = "OSN001",
                DELIVERY_STATUS = "未印",
                DetailType = "平版",
                //PAPER_TYPE = "塗佈白紙板",
                //BASIC_WEIGHT = "03500",
                //SPECIFICATION = "214K512K",
                //GRAIN_DIRECTION = "L",
                //PACKING_TYPE = "無令打件",
                //訂單原始數量
                SRC_REQUESTED_QUANTITY = 1.33742M,
                //訂單主單位
                SRC_REQUESTED_QUANTITY_UOM = "MT",
                //預計出庫輔數量
                REQUESTED_QUANTITY2 = 50,
                //輔單位(RE)
                SRC_REQUESTED_QUANTITY_UOM2 = "RE",
                //預計出庫量
                REQUESTED_QUANTITY = 1337.419M,
                //庫存單位(KG)
                REQUESTED_QUANTITY_UOM = "KG",
                //出貨倉庫
                SUBINVENTORY_CODE = "TB2",
                //出貨倉庫名稱
                //SUBINVENTORY_NAME = "TB2",
                //組車日
                TRIP_ACTUAL_SHIP_DATE = Convert.ToDateTime("2019-12-26"),
                //航程號
                TRIP_NAME = "Y191226-1036357",
                //預計出貨確認日
                TRANSACTION_DATE = Convert.ToDateTime("2019-12-26"),
                //出貨核准日
                AUTHORIZE_DATE = "2019-12-26",
                //備註
                REMARK = "FT1.P9B0288",
            });
            #endregion

            #region 捲筒
            source.Add(new TripDetailDT()
            {
                Id = 2,
                ORG_ID = 7,
                CUSTOMER_LOCATION_CODE = "中華彩色",
                CUSTOMER_NAME = "中華彩色",
                SHIP_LOCATION_CODE = "新北市新店區寶橋路229號",
                SHIP_CUSTOMER_NAME = "中華彩色印刷股份有限公司",
                TRIP_ID = 2,
                DELIVERY_NAME = "FTY2001000140",
                TRIP_CAR = "PTB2",
                FREIGHT_TERMS_NAME = "台北",
                //ITEM_DESCRIPTION = "A006",
                //ORDER_NUMBER = 6,
                //ORDER_SHIP_NUMBER = "OSN006",
                DELIVERY_STATUS = "未印",
                DetailType = "平版",
                //PAPER_TYPE = "塗佈白紙板",
                //BASIC_WEIGHT = "03500",
                //SPECIFICATION = "214K512K",
                //GRAIN_DIRECTION = "L",
                //PACKING_TYPE = "無令打件",
                //訂單原始數量
                SRC_REQUESTED_QUANTITY = 0.37489M,
                //訂單主單位
                SRC_REQUESTED_QUANTITY_UOM = "MT",
                //預計出庫輔數量
                REQUESTED_QUANTITY2 = 19,
                //輔單位(RE)
                SRC_REQUESTED_QUANTITY_UOM2 = "RE",
                //預計出庫量
                REQUESTED_QUANTITY = 374.8945M,
                //庫存單位(KG)
                REQUESTED_QUANTITY_UOM = "KG",
                //出貨倉庫
                SUBINVENTORY_CODE = "TB2",
                //出貨倉庫名稱
                //SUBINVENTORY_NAME = "TB2",
                //組車日
                TRIP_ACTUAL_SHIP_DATE = Convert.ToDateTime("2020-01-09"),
                //航程號
                TRIP_NAME = "Y200109-1052058",
                //預計出貨確認日
                TRANSACTION_DATE = Convert.ToDateTime("2020-01-09"),
                //出貨核准日
                AUTHORIZE_DATE = "2020-01-09",
                //備註
                REMARK = "FT1.早上到X002010031大道季刊98期/P2010087",
            });

            #endregion

            #region 捲筒
            source.Add(new TripDetailDT()
            {
                Id = 3,
                ORG_ID = 8,
                CUSTOMER_LOCATION_CODE = "中華彩色",
                CUSTOMER_NAME = "中華彩色",
                SHIP_LOCATION_CODE = "新北市新店區寶橋路229號",
                SHIP_CUSTOMER_NAME = "中華彩色印刷股份有限公司",
                TRIP_ID = 3,
                DELIVERY_NAME = "FTY2001000152",
                TRIP_CAR = "PTB2",
                FREIGHT_TERMS_NAME = "台北",
                //ITEM_DESCRIPTION = "A006",
                //ORDER_NUMBER = 6,
                //ORDER_SHIP_NUMBER = "OSN006",
                DELIVERY_STATUS = "未印",
                DetailType = "捲筒",
                //PAPER_TYPE = "塗佈白紙板",
                //BASIC_WEIGHT = "03500",
                //SPECIFICATION = "214K512K",
                //GRAIN_DIRECTION = "L",
                //PACKING_TYPE = "無令打件",
                //訂單原始數量
                SRC_REQUESTED_QUANTITY = 3,
                //訂單主單位
                SRC_REQUESTED_QUANTITY_UOM = "MT",
                //預計出庫輔數量
                REQUESTED_QUANTITY2 = 0,
                //輔單位(RE)
                SRC_REQUESTED_QUANTITY_UOM2 = "",
                //預計出庫量
                REQUESTED_QUANTITY = 3000,
                //庫存單位(KG)
                REQUESTED_QUANTITY_UOM = "KG",
                //出貨倉庫
                SUBINVENTORY_CODE = "SFG",
                //出貨倉庫名稱
                //SUBINVENTORY_NAME = "SFG",
                //組車日
                TRIP_ACTUAL_SHIP_DATE = Convert.ToDateTime("2020-04-22"),
                //航程號
                TRIP_NAME = "Y200109-1052060",
                //預計出貨確認日
                TRANSACTION_DATE = Convert.ToDateTime("2020-04-22"),
                //出貨核准日
                AUTHORIZE_DATE = "2020-04-22",
                //備註
                REMARK = "",
            });

            #endregion

            #endregion

        }

        public static List<TripDetailDT> GetData(int id)
        {
            var query = from tripDetail in source
                        where tripDetail.Id == id
                        select tripDetail;
            return query.ToList<TripDetailDT>();
        }

        public static List<TripDetailDT> GetData()
        {
            var query = from tripDetail in source
                        select tripDetail;
            return query.ToList<TripDetailDT>();
        }

        public static List<TripDetailDT> Search(string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
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

        public static DeliverySearchViewModel GetViewModel()
        {
            DeliverySearchViewModel viewModel = new DeliverySearchViewModel();
            //viewModel.SelectedDeliveryStatus = "";
            //viewModel.DeliveryName = "";
            //viewModel.TransactionDate = "";
            //viewModel.TripActualShipBeginDate = "";
            //viewModel.TripActualShipEndDate = "";

            //viewModel.SelectedTrip = "*";
            List<ListItem> tripList = new List<ListItem>();
            tripList.Add(new ListItem("全部", "*"));
            tripList.Add(new ListItem("Y191226-1036357", "Y191226-1036357"));
            tripList.Add(new ListItem("Y200109-1052058", "Y200109-1052058"));
            tripList.Add(new ListItem("Y200109-1052060", "Y200109-1052060"));

            viewModel.TripNameItems = tripList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });

            //viewModel.SelectedSubinventory = "*";
            OrgSubinventoryData orgData = new OrgSubinventoryData();
            viewModel.SubinventoryNameItems = orgData.GetSubinventoryList("265", true);





            //viewModel.SelectedDeliveryStatus = "*";
            List<ListItem> deliveryStatusList = new List<ListItem>();
            deliveryStatusList.Add(new ListItem("全部", "*"));
            deliveryStatusList.Add(new ListItem("取消", "取消"));
            deliveryStatusList.Add(new ListItem("未印", "未印"));
            deliveryStatusList.Add(new ListItem("待出", "待出"));
            deliveryStatusList.Add(new ListItem("已揀", "已揀"));
            deliveryStatusList.Add(new ListItem("待核准", "待核准"));
            deliveryStatusList.Add(new ListItem("已出貨", "已出貨"));

            viewModel.DeliveryStatusItems = deliveryStatusList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });

            return viewModel;
        }


        public static ResultModel DeliveryConfirm(List<long> ids)
        {


            var query = from tripDetail in source
                        where ids.Contains(tripDetail.Id)
                        group tripDetail by new
                        {
                            tripDetail.TRIP_ID
                        } into g
                        select new
                        {
                            g.Key.TRIP_ID
                        };



            ResultModel result = new ResultModel(true, "出貨確認成功");

            foreach (TripDetailDT sourceData in source)
            {
                if (result.Success == false)
                {
                    break;
                }
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS != "已揀")
                        {
                            result.Success = false;
                            result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為已揀";
                            break;
                        }
                    }
                }
            }

            if (result.Success == false)
            {
                return result;
            }

            foreach (TripDetailDT sourceData in source)
            {
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS == "已揀")
                        {
                            sourceData.DELIVERY_STATUS = "待核准";
                            //obj.TRANSACTION_DATE = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }

            return result;


        }

        public static ResultModel CancelConfirm(List<long> ids)
        {

            var query = from tripDetail in source
                        where ids.Contains(tripDetail.Id)
                        group tripDetail by new
                        {
                            tripDetail.TRIP_ID
                        } into g
                        select new
                        {
                            g.Key.TRIP_ID
                        };

            ResultModel result = new ResultModel(true, "取消出貨確認成功");

            foreach (TripDetailDT sourceData in source)
            {
                if (result.Success == false)
                {
                    break;
                }
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS != "待核准")
                        {
                            result.Success = false;
                            result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為待核准";
                            break;
                        }
                    }
                }
            }

            if (result.Success == false)
            {
                return result;
            }

            foreach (TripDetailDT sourceData in source)
            {
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS == "待核准")
                        {
                            sourceData.DELIVERY_STATUS = "已揀";
                        }
                    }
                }
            }

            return result;

        }

        public static ResultModel PrintPickList(List<long> ids)
        {

            var query = from tripDetail in source
                        where ids.Contains(tripDetail.Id)
                        group tripDetail by new
                        {
                            tripDetail.TRIP_ID
                        } into g
                        select new
                        {
                            g.Key.TRIP_ID
                        };

            ResultModel result = new ResultModel(true, "列印備貨單成功");

            foreach (TripDetailDT obj in source)
            {
                //foreach (var q in query)
                //{
                //    if (obj.TRIP_ID == q.TRIP_ID)
                //    {
                //        if (obj.DELIVERY_STATUS != "未印")
                //        {
                //            result.Success = false;
                //            result.Msg = "交運單" + obj.DELIVERY_NAME + "狀態須為未印";
                //            break;
                //        }
                //    }
                //}

                //if (!result.Success)
                //{
                //    break;
                //}

                foreach (var q in query)
                {
                    if (obj.TRIP_ID == q.TRIP_ID)
                    {
                        if (obj.DELIVERY_STATUS == "未印")
                        {
                            obj.DELIVERY_STATUS = "待出";
                        }
                    }
                }
            }

            return result;
        }

        public static ResultModel DeliveryAuthorize(List<long> ids)
        {
            var query = from tripDetail in source
                        where ids.Contains(tripDetail.Id)
                        group tripDetail by new
                        {
                            tripDetail.TRIP_ID
                        } into g
                        select new
                        {
                            g.Key.TRIP_ID
                        };

            ResultModel result = new ResultModel(true, "出貨核准成功");

            foreach (TripDetailDT sourceData in source)
            {
                if (result.Success == false)
                {
                    break;
                }
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS != "待核准")
                        {
                            result.Success = false;
                            result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為待核准";
                            break;
                        }
                    }
                }
            }

            if (result.Success == false)
            {
                return result;
            }

            foreach (TripDetailDT sourceData in source)
            {
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS == "待核准")
                        {
                            sourceData.DELIVERY_STATUS = "已出貨";
                        }
                    }
                }
            }

            return result;

        }

        public static ResultModel CancelAuthorize(List<long> ids)
        {
            var query = from tripDetail in source
                        where ids.Contains(tripDetail.Id)
                        group tripDetail by new
                        {
                            tripDetail.TRIP_ID
                        } into g
                        select new
                        {
                            g.Key.TRIP_ID
                        };

            ResultModel result = new ResultModel(true, "取消出貨核准成功");

            foreach (TripDetailDT sourceData in source)
            {
                if (result.Success == false)
                {
                    break;
                }
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS != "待核准")
                        {
                            result.Success = false;
                            result.Msg = "交運單" + sourceData.DELIVERY_NAME + "狀態須為待核准";
                            break;
                        }
                    }
                }
            }

            if (result.Success == false)
            {
                return result;
            }

            foreach (TripDetailDT sourceData in source)
            {
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS == "待核准")
                        {
                            sourceData.DELIVERY_STATUS = "已揀";
                        }
                    }
                }
            }

            return result;

        }

        public static ResultModel CancelTrip(List<long> ids)
        {

            var query = from tripDetail in source
                        where ids.Contains(tripDetail.Id)
                        group tripDetail by new
                        {
                            tripDetail.TRIP_ID
                        } into g
                        select new
                        {
                            g.Key.TRIP_ID
                        };

            ResultModel result = new ResultModel(true, "取消航程號成功");

            foreach (TripDetailDT sourceData in source)
            {
                if (result.Success == false)
                {
                    break;
                }
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        if (sourceData.DELIVERY_STATUS == "取消")
                        {
                            result.Success = false;
                            result.Msg = "航程號" + sourceData.TRIP_NAME + "已取消";
                            break;
                        }
                    }
                }
            }

            foreach (TripDetailDT sourceData in source)
            {
                foreach (var q in query)
                {
                    if (sourceData.TRIP_ID == q.TRIP_ID)
                    {
                        sourceData.DELIVERY_STATUS = "取消";
                    }
                }
            }

            return result;

        }

        public static void ChangeDeliveryStatus(long TripDetailDT_ID, bool pickComplete)
        {

            foreach (TripDetailDT obj in source)
            {
                if (obj.Id == TripDetailDT_ID)
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

        public static List<TripDetailDT> UpdateTransactionAuthorizeDates(TripDetailDTEditor data)
        {
            List<TripDetailDT> result = new List<TripDetailDT>();

            foreach (var sourceTripDetailDT in source)
            {
                foreach (var selectedData in data.TripDetailDTList)
                {
                    if (sourceTripDetailDT.TRIP_ID == selectedData.TRIP_ID)
                    {
                        sourceTripDetailDT.AUTHORIZE_DATE = selectedData.AUTHORIZE_DATE;
                        result.Add(sourceTripDetailDT);
                    }
                }
            }

            return result;

        }
    }

    internal class TripDetailDTOrder
    {
        public static IOrderedEnumerable<TripDetailDT> Order(List<Order> orders, IEnumerable<TripDetailDT> models)
        {
            IOrderedEnumerable<TripDetailDT> orderedModel = null;
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


        private static IOrderedEnumerable<TripDetailDT> OrderBy(int column, string dir, IEnumerable<TripDetailDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);


            }
        }

        private static IOrderedEnumerable<TripDetailDT> ThenBy(int column, string dir, IOrderedEnumerable<TripDetailDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Id) : models.ThenBy(x => x.Id);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);


            }
        }
    }

    public class TripDetailDTEditor
    {
        public string Action { get; set; }
        //public List<long> TripDetailDT_IDs { get; set; }
        //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
        public List<TripDetailDT> TripDetailDTList { get; set; }
    }

}