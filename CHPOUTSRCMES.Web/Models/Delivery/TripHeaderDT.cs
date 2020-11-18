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
using CHPOUTSRCMES.Web.Util;
using Microsoft.Reporting.WebForms;
using System.Drawing;

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

        [Display(Name = "主單位需求量總和")]
        public decimal RP_SUM { get; set; }

        [Display(Name = "次單位需求量總和")]
        public decimal RS_SUM { get; set; }
        
        public string REQUESTED_PRIMARY_UOM { get; set; }

        public string REQUESTED_SECONDARY_UOM { get; set; }
    }


    public class TripHeaderData
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
      

        public List<TripHeaderDT> DeliverySearch(DeliveryUOW uow, string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus, string userId)
        {
            return uow.DeliverySearch(TripActualShipBeginDate, TripActualShipEndDate, DeliveryName, SelectedSubinventory, SelectedTrip
                , TransactionDate, SelectedDeliveryStatus, userId);
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
        }

        public ResultModel PrintPickList(DeliveryUOW uow, List<long> ids, string userId, string userName)
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

            return uow.CancelTrip(updateDatas, userId, userName);
        }

        

        public ResultModel AddPickDT(DeliveryUOW uow, long dlvHeaderId, long dlvDetailId, string deliveryName, string barcode, string qty, string addUser, string addUserName)
        {
            if (qty != null)
            {
                var result = ConvertEx.StringToDecimal(qty);
                if (!result.Success) return new ResultModel(false, "令數須為數字");
                return uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, result.Data, addUser, addUserName);
            }
            else
            {
                return uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, null, addUser, addUserName);
            }
           
            //var addResult = uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, qty, addUser, addUserName, status);
            //if (!addResult.Success)
            //{
            //    return addResult;
            //}
            //return CheckPicked(uow, dlvHeaderId, addUser, addUserName);
        }


        public ResultModel DelPickDT(DeliveryUOW uow, List<long> dlvPickedId, string userId, string userName)
        {
            var pickedDataList = uow.GetDeliveryPickDataListFromPickedId(dlvPickedId);
            if (pickedDataList.Count == 0) return new ResultModel(false, "揀貨明細找不到資料");
            if (pickedDataList.Count != dlvPickedId.Count) return new ResultModel(false, "刪除揀貨明細數量比對錯誤");

            return uow.DelPickDT(pickedDataList, userId, userName);
        }

       

        public ResultModel UpdateTransactionAuthorizeDates(DeliveryUOW uow, TripDetailDTEditor data, string userId, string userNmae)
        {
            return uow.UpdateTransactionAuthorizeDates(data, userId, userNmae);
        }

        public ActionResult PritLabel(DeliveryUOW uow, List<long> PICKED_IDs, string userName)
        {
            var resultData = uow.GetLabels(PICKED_IDs, userName);
            return uow.PrintLabel(resultData.Data);
        }

        #region 報表

        public ResultDataModel<ReportViewer> LocalDeliveryPickingReportViewer(DeliveryUOW uow, string tripName)
        {
            try
            {
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("TRIP_NAME", tripName, false));

                var report = new ReportViewer();
                // Set the processing mode for the ReportViewer to Local  
                report.ProcessingMode = ProcessingMode.Local;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;
                
                LocalReport localReport = report.LocalReport;
                localReport.ReportPath = "Report/DeliveryPickingList.rdlc";

                var reportDataSourceResult = uow.GetPickingListReportDataSource(tripName);
                if (!reportDataSourceResult.Success) return new ResultDataModel<ReportViewer>(false, reportDataSourceResult.Msg, null);
                localReport.DataSources.Add(reportDataSourceResult.Data);

                // Set the report parameters for the report  
                localReport.SetParameters(paramList);

                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得備貨單報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得備貨單報表失敗:" + ex.Message, null);
            }
        }


        public ResultDataModel<ReportViewer> RemoteDeliveryPickingReportViewer(string tripName)
        {
            try
            {
                string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("TRIP_NAME", tripName, false));

                var report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;
                report.ServerReport.ReportPath = KeyName + "/DeliveryPickingList";
                report.ServerReport.ReportServerUrl = new Uri("http://rs.yfy.com/ReportServer");
                report.ServerReport.SetParameters(paramList);
                report.ServerReport.Refresh();                

                return new ResultDataModel<ReportViewer>(true, "取得備貨單報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得備貨單報表失敗:" + ex.Message, null);
            }
        }

        #endregion
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
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.FREIGHT_TERMS_NAME) : models.OrderBy(x => x.FREIGHT_TERMS_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRIP_CAR) : models.OrderBy(x => x.TRIP_CAR);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRIP_NAME) : models.OrderBy(x => x.TRIP_NAME);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DELIVERY_NAME) : models.OrderBy(x => x.DELIVERY_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DetailType) : models.OrderBy(x => x.DetailType);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DELIVERY_STATUS) : models.OrderBy(x => x.DELIVERY_STATUS);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CUSTOMER_NAME) : models.OrderBy(x => x.CUSTOMER_NAME);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUBINVENTORY_CODE) : models.OrderBy(x => x.SUBINVENTORY_CODE);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.NOTE) : models.OrderBy(x => x.NOTE);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SHIP_CUSTOMER_NAME) : models.OrderBy(x => x.SHIP_CUSTOMER_NAME);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRIP_ACTUAL_SHIP_DATE) : models.OrderBy(x => x.TRIP_ACTUAL_SHIP_DATE);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TRANSACTION_DATE) : models.OrderBy(x => x.TRANSACTION_DATE);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.AUTHORIZE_DATE) : models.OrderBy(x => x.AUTHORIZE_DATE);
                


            }
        }

        private static IOrderedEnumerable<TripHeaderDT> ThenBy(int column, string dir, IOrderedEnumerable<TripHeaderDT> models)
        {
            switch (column)
            {
                default:
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.FREIGHT_TERMS_NAME) : models.ThenBy(x => x.FREIGHT_TERMS_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRIP_CAR) : models.ThenBy(x => x.TRIP_CAR);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRIP_NAME) : models.ThenBy(x => x.TRIP_NAME);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DELIVERY_NAME) : models.ThenBy(x => x.DELIVERY_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DetailType) : models.ThenBy(x => x.DetailType);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DELIVERY_STATUS) : models.ThenBy(x => x.DELIVERY_STATUS);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CUSTOMER_NAME) : models.ThenBy(x => x.CUSTOMER_NAME);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUBINVENTORY_CODE) : models.ThenBy(x => x.SUBINVENTORY_CODE);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.NOTE) : models.ThenBy(x => x.NOTE);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SHIP_CUSTOMER_NAME) : models.ThenBy(x => x.SHIP_CUSTOMER_NAME);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRIP_ACTUAL_SHIP_DATE) : models.ThenBy(x => x.TRIP_ACTUAL_SHIP_DATE);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TRANSACTION_DATE) : models.ThenBy(x => x.TRANSACTION_DATE);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.AUTHORIZE_DATE) : models.ThenBy(x => x.AUTHORIZE_DATE);


            }
        }
    }

    public class TripDetailDTEditor
    {
        public string Action { get; set; }
        public List<TripHeaderDT> TripDetailDTList { get; set; }
    }

    public class PickDTEditor
    {
        public string Action { get; set; }
        public List<long> DlvPickedIdList { get; set; }
    }
}