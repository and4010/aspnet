using CHPOUTSRCMES.Web.Models.Delivery;
using CHPOUTSRCMES.Web.ViewModels.Delivery;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models;
using DataTables;
using System.Collections.Specialized;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class DeliveryController : Controller
    {
        TripHeaderData tripHeaderData = new TripHeaderData();
        //
        // GET: /Delivery/
        public ActionResult Index()
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    DeliverySearchViewModel viewModel = tripHeaderData.GetViewModel(uow);
                    return View(viewModel);
                }
            }
        }

        [HttpPost, ActionName("GetTripDetail")]
        public JsonResult GetTripDetail(DataTableAjaxPostViewModel data, string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus)
        {
            //if (TripDetailData.source.Count == 0)
            //{
            //    TripDetailData.AddDefaultData();
            //}

            //List<TripDetailDT> model = TripDetailData.model;

            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    List<TripHeaderDT> model = tripHeaderData.DeliverySearch(uow, TripActualShipBeginDate, TripActualShipEndDate, DeliveryName, SelectedSubinventory, SelectedTrip, TransactionDate, SelectedDeliveryStatus);
                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => (!string.IsNullOrEmpty(p.FREIGHT_TERMS_NAME) && p.FREIGHT_TERMS_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.DELIVERY_NAME) && p.DELIVERY_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.CUSTOMER_NAME) && p.CUSTOMER_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.CUSTOMER_LOCATION_CODE) && p.CUSTOMER_LOCATION_CODE.ToLower().Contains(search.ToLower()))
                            || p.SRC_REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.SRC_REQUESTED_QUANTITY_UOM) && p.SRC_REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                            || p.REQUESTED_QUANTITY2.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM2) && p.REQUESTED_QUANTITY_UOM2.ToLower().Contains(search.ToLower()))
                            || p.REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM) && p.REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                            || (p.TRIP_ACTUAL_SHIP_DATE.HasValue && p.TRIP_ACTUAL_SHIP_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.TRIP_NAME) && p.TRIP_NAME.ToLower().Contains(search.ToLower()))
                            || (p.TRANSACTION_DATE.HasValue && p.TRANSACTION_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.DetailType) && p.DetailType.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.DELIVERY_STATUS) && p.DELIVERY_STATUS.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                            ).ToList();
                    }

                    var filteredCount = model.Count;
                    model = TripDetailDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();

                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //[HttpPost, ActionName("Search")]
        //public JsonResult Search(string TripActualShipBeginDate,string TripActualShipEndDate,string DeliveryName, string SelectedSubinventory,
        //    string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus)
        //{
        //    TripDetailData.model = TripDetailData.Search(TripActualShipBeginDate, TripActualShipEndDate, DeliveryName, SelectedSubinventory, SelectedTrip, TransactionDate, SelectedDeliveryStatus);

        //    return new JsonResult { Data = new { status = true, result = "搜尋成功" } };
        //}

        public ActionResult FlatEdit(string id)
        {
            TripHeaderDT detailData = null;
            if (string.IsNullOrEmpty(id))
            {
                detailData = TripHeaderData.GetData()[0];
            }
            else
            {
                detailData = TripHeaderData.GetData(Convert.ToInt32(id))[0];
            }

            FlatEditViewModel model = new FlatEditViewModel();
            model.DeliveryDetailViewHeader = new DeliveryDetailViewHeader();
            model.DeliveryDetailViewHeader.CUSTOMER_LOCATION_CODE = detailData.CUSTOMER_LOCATION_CODE;
            model.DeliveryDetailViewHeader.CUSTOMER_NAME = detailData.CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_NAME = detailData.DELIVERY_NAME;
            //model.DeliveryDetailViewHeader.ORDER_NUMBER = Convert.ToString(detailData.ORDER_NUMBER);
            model.DeliveryDetailViewHeader.REMARK = detailData.NOTE;
            model.DeliveryDetailViewHeader.SHIP_CUSTOMER_NAME = detailData.SHIP_CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.SHIP_LOCATION_CODE = detailData.SHIP_LOCATION_CODE;
            model.DeliveryDetailViewHeader.TRIP_ACTUAL_SHIP_DATE = detailData.TRIP_ACTUAL_SHIP_DATE;
            model.DeliveryDetailViewHeader.TRIP_CAR = detailData.TRIP_CAR;
            model.DeliveryDetailViewHeader.TRIP_NAME = detailData.TRIP_NAME;
            model.DeliveryDetailViewHeader.TripDetailDT_ID = detailData.Id;
            model.DeliveryDetailViewHeader.DELIVERY_STATUS = detailData.DELIVERY_STATUS;
            return View(model);
        }

        [HttpGet]
        public ActionResult InsteadEdit(string id)
        {
            InsteadEditViewModel model = new InsteadEditViewModel();
            DeliveryDetailViewHeader deliveryDetailViewHeader = new DeliveryDetailViewHeader();
            deliveryDetailViewHeader.TRIP_NAME = "1234";
            model.DeliveryDetailViewHeader = deliveryDetailViewHeader;
            return View(model);
        }

        [HttpGet]
        public ActionResult RollEdit(string id)
        {
            TripHeaderDT detailData = null;
            if (string.IsNullOrEmpty(id))
            {
                detailData = TripHeaderData.GetData()[0];
            }
            else
            {
                detailData = TripHeaderData.GetData(Convert.ToInt32(id))[0];
            }
            PaperRollEditViewModel model = new PaperRollEditViewModel();
            model.DeliveryDetailViewHeader = new DeliveryDetailViewHeader();
            model.DeliveryDetailViewHeader.TripDetailDT_ID = detailData.Id;
            model.DeliveryDetailViewHeader.CUSTOMER_LOCATION_CODE = detailData.CUSTOMER_LOCATION_CODE;
            model.DeliveryDetailViewHeader.CUSTOMER_NAME = detailData.CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_NAME = detailData.DELIVERY_NAME;
            //model.DeliveryDetailViewHeader.ORDER_NUMBER = Convert.ToString(detailData.ORDER_NUMBER);
            model.DeliveryDetailViewHeader.REMARK = detailData.NOTE;
            model.DeliveryDetailViewHeader.SHIP_CUSTOMER_NAME = detailData.SHIP_CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.SHIP_LOCATION_CODE = detailData.SHIP_LOCATION_CODE;
            model.DeliveryDetailViewHeader.TRIP_ACTUAL_SHIP_DATE = detailData.TRIP_ACTUAL_SHIP_DATE;
            model.DeliveryDetailViewHeader.TRIP_CAR = detailData.TRIP_CAR;
            model.DeliveryDetailViewHeader.TRIP_NAME = detailData.TRIP_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_STATUS = detailData.DELIVERY_STATUS;

            //DeliveryDetailViewHeader deliveryDetailViewHeader = new DeliveryDetailViewHeader();
            //deliveryDetailViewHeader.TRIP_NAME = "Y191226-1036357";
            //deliveryDetailViewHeader.DELIVERY_NAME = "FTY1910000150";
            //model.DeliveryDetailViewHeader = deliveryDetailViewHeader;

            return View(model);
        }

        [HttpPost, ActionName("GetRollEdit")]
        public JsonResult GetRollEdit(DataTableAjaxPostViewModel data, long TripDetailDT_ID)
        {
            if (PaperRollEditData.getDataCount(TripDetailDT_ID) == 0)
            {
                PaperRollEditData.addDefault(TripDetailDT_ID);
            }

            List<PaperRollEditDT> model = PaperRollEditData.getDataList(TripDetailDT_ID);
            var totalCount = model.Count;
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.ORDER_NUMBER.ToString()) && p.ORDER_NUMBER.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ORDER_SHIP_NUMBER) && p.ORDER_SHIP_NUMBER.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_DESCRIPTION) && p.ITEM_DESCRIPTION.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PAPER_TYPE) && p.PAPER_TYPE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BASIC_WEIGHT) && p.BASIC_WEIGHT.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SPECIFICATION) && p.SPECIFICATION.ToLower().Contains(search.ToLower()))
                    || p.SRC_REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(p.SRC_REQUESTED_QUANTITY_UOM) && p.SRC_REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                    || p.REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM) && p.REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.REMARK) && p.REMARK.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = PaperRollEditDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetRollEditBarcode")]
        public JsonResult GetRollEditBarcode(DataTableAjaxPostViewModel data, long TripDetailDT_ID)
        {
            List<PaperRollEditBarcodeDT> model = PaperRollEditBarcodeData.getDataList(TripDetailDT_ID);
            var totalCount = model.Count;
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.ITEM_DESCRIPTION) && p.ITEM_DESCRIPTION.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                    || p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || p.PRIMARY_UOM.ToString().ToLower().Contains(search.ToLower())
                   || (!string.IsNullOrEmpty(p.REMARK) && p.REMARK.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            var filteredCount = model.Count;
            model = PaperRollEditBarcodeDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("InputRollEditBarcode")]
        public ActionResult InputRollEditBarcode(string BARCODE, long TripDetailDT_ID, long PaperRollEditDT_ID)
        {

            if (BARCODE == "W2005060001" || BARCODE == "W2005060002" || BARCODE == "W2005060003" || BARCODE == "W2005060004" || BARCODE == "W2005060005" || BARCODE == "W2005060006")
            {
                if (!PaperRollEditBarcodeData.checkBarcodeExist(BARCODE))
                {
                    string ITEM_DESCRIPTION = "";
                    if (BARCODE == "W2005060001")
                    {
                        ITEM_DESCRIPTION = "4FHIZA03000787RL00";
                        if (!PaperRollEditData.checkBarcodeItemDesc(PaperRollEditDT_ID, ITEM_DESCRIPTION))
                        {
                            return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                        }
                        PaperRollEditBarcodeData.addBarcode123(PaperRollEditDT_ID, TripDetailDT_ID);
                        PaperRollEditData.updateA006(PaperRollEditDT_ID, TripDetailDT_ID);
                    }
                    else if (BARCODE == "W2005060004")
                    {
                        ITEM_DESCRIPTION = "4FHIZA03000787RL00";
                        if (!PaperRollEditData.checkBarcodeItemDesc(PaperRollEditDT_ID, ITEM_DESCRIPTION))
                        {
                            return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                        }
                        PaperRollEditBarcodeData.addBarcode124(PaperRollEditDT_ID, TripDetailDT_ID);
                        PaperRollEditData.updateA006(PaperRollEditDT_ID, TripDetailDT_ID);
                    }
                    else if (BARCODE == "W2005060002")
                    {
                        ITEM_DESCRIPTION = "4FHIZA02500787RL00";
                        if (!PaperRollEditData.checkBarcodeItemDesc(PaperRollEditDT_ID, ITEM_DESCRIPTION))
                        {
                            return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                        }
                        PaperRollEditBarcodeData.addBarcode456(PaperRollEditDT_ID, TripDetailDT_ID);
                        PaperRollEditData.updateB001(PaperRollEditDT_ID, TripDetailDT_ID);
                    }
                    else if (BARCODE == "W2005060005")
                    {
                        ITEM_DESCRIPTION = "4FHIZA02500787RL00";
                        if (!PaperRollEditData.checkBarcodeItemDesc(PaperRollEditDT_ID, ITEM_DESCRIPTION))
                        {
                            return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                        }
                        PaperRollEditBarcodeData.addBarcode457(PaperRollEditDT_ID, TripDetailDT_ID);
                        PaperRollEditData.updateB001(PaperRollEditDT_ID, TripDetailDT_ID);
                    }
                    else if (BARCODE == "W2005060003")
                    {
                        ITEM_DESCRIPTION = "4FHIZA02000787RL00";
                        if (!PaperRollEditData.checkBarcodeItemDesc(PaperRollEditDT_ID, ITEM_DESCRIPTION))
                        {
                            return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                        }
                        PaperRollEditBarcodeData.addBarcode130(PaperRollEditDT_ID, TripDetailDT_ID);
                        PaperRollEditData.updateA006s(PaperRollEditDT_ID, TripDetailDT_ID);
                    }
                    else if (BARCODE == "W2005060006")
                    {
                        ITEM_DESCRIPTION = "4FHIZA02000787RL00";
                        if (!PaperRollEditData.checkBarcodeItemDesc(PaperRollEditDT_ID, ITEM_DESCRIPTION))
                        {
                            return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                        }
                        PaperRollEditBarcodeData.addBarcode131(PaperRollEditDT_ID, TripDetailDT_ID);
                        PaperRollEditData.updateA006s(PaperRollEditDT_ID, TripDetailDT_ID);
                    }


                    TripHeaderData.ChangeDeliveryStatus(TripDetailDT_ID, PaperRollEditData.checkDeliveryPickComplete(TripDetailDT_ID));
                    return new JsonResult { Data = new { status = true, result = "條碼儲存成功" } };
                }
                else
                {
                    return new JsonResult { Data = new { status = false, result = "條碼重複輸入" } };
                }

            }
            else
            {
                return new JsonResult { Data = new { status = false, result = "找不到條碼資料" } };
            }
        }

        [HttpPost, ActionName("DeleteRollEditBarcode")]
        public ActionResult DeleteRollEditBarcode(List<long> PICKED_ID, long TripDetailDT_ID)
        {
            bool result = false;
            List<PaperRollEditBarcodeDT> items = PaperRollEditBarcodeData.getItemList(PICKED_ID);
            if (items.Count > 0)
            {
                result = PaperRollEditBarcodeData.remove(PICKED_ID);
                if (result)
                {
                    foreach (PaperRollEditBarcodeDT item in items)
                    {
                        PaperRollEditData.remove(item.PaperRollEditDT_ID, TripDetailDT_ID);
                        //if (item.ITEM_DESCRIPTION == "A006" && item.PaperRollEditDT_ID == 1)
                        //{
                        //    PaperRollEditData.removeA006(item.PaperRollEditDT_ID, TripDetailDT_ID);
                        //}
                        //else if (item.ITEM_DESCRIPTION == "B001")
                        //{
                        //    PaperRollEditData.removeB001(item.PaperRollEditDT_ID, TripDetailDT_ID);
                        //}
                        //else if (item.ITEM_DESCRIPTION == "A006" && item.PaperRollEditDT_ID == 3)
                        //{
                        //    PaperRollEditData.removeA006s(item.PaperRollEditDT_ID, TripDetailDT_ID);
                        //}
                    }
                    TripHeaderData.ChangeDeliveryStatus(TripDetailDT_ID, PaperRollEditData.checkDeliveryPickComplete(TripDetailDT_ID));
                    return new JsonResult { Data = new { status = true, result = "條碼刪除成功" } };
                }
                else
                {
                    return new JsonResult { Data = new { status = false, result = "條碼刪除失敗" } };
                }
            }
            else
            {
                return new JsonResult { Data = new { status = false, result = "條碼刪除失敗" } };
            }

        }

        [HttpPost, ActionName("GetFlatEdit")]
        public JsonResult GetFlatEdit(DataTableAjaxPostViewModel data, string DELIVERY_NAME, string TRIP_NAME)
        {
            if (FlatEditData.getModel(DELIVERY_NAME, TRIP_NAME).Count == 0)
            {
                FlatEditData.addDefault();
            }

            List<FlatEditDT> model = FlatEditData.getModel(DELIVERY_NAME, TRIP_NAME);
            var totalCount = model.Count;
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.ORDER_NUMBER.ToString()) && p.ORDER_NUMBER.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ORDER_SHIP_NUMBER) && p.ORDER_SHIP_NUMBER.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.OSP_BATCH_NO) && p.OSP_BATCH_NO.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_DESCRIPTION) && p.ITEM_DESCRIPTION.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.REAM_WEIGHT) && p.REAM_WEIGHT.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                    || p.SRC_REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(p.SRC_REQUESTED_QUANTITY_UOM) && p.SRC_REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                    || p.REQUESTED_QUANTITY2.ToString().ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(p.SRC_REQUESTED_QUANTITY_UOM2) && p.SRC_REQUESTED_QUANTITY_UOM2.ToLower().Contains(search.ToLower()))
                    || p.REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM) && p.REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.REMARK) && p.REMARK.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = FlatEditDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetFlatEditBarcode")]
        public JsonResult GetFlatEditBarcode(DataTableAjaxPostViewModel data, long TripDetailDT_ID)
        {
            List<FlatEditBarcodeDT> model = FlatEditBarcodeData.getModel(TripDetailDT_ID);
            var totalCount = model.Count;
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_DESCRIPTION) && p.ITEM_DESCRIPTION.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.REAM_WEIGHT) && p.REAM_WEIGHT.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                    || p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || p.PRIMARY_UOM.ToString().ToLower().Contains(search.ToLower())
                    || p.SECONDARY_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                    || p.SECONDARY_UOM.ToString().ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(p.REMARK) && p.REMARK.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            var filteredCount = model.Count;
            model = FlatEditBarcodeDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("InputFlatEditBarcode")]
        public ActionResult InputFlatEditBarcode(string BARCODE, decimal? SECONDARY_QUANTITY, long TripDetailDT_ID, long FlatEditDT_ID)
        {
            //搜尋條碼資料

            if (FlatEditBarcodeData.checkBarcodeExist(BARCODE))
            {
                return new JsonResult { Data = new { status = false, result = "條碼重複輸入" } };
            }
            string ITEM_DESCRIPTION = "";
            if (BARCODE == "P2005060001")//平張令包
            {
                if (SECONDARY_QUANTITY == null)
                {
                    ITEM_DESCRIPTION = "4A003A01000310K266K";
                    if (!FlatEditData.checkBarcodeItemDesc(FlatEditDT_ID, ITEM_DESCRIPTION))
                    {
                        return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                    }
                    return new JsonResult { Data = new { status = true, result = "令包" } };
                }
                else
                {
                    //儲存條碼資料
                    FlatEditBarcodeData.addBarcode123((decimal)SECONDARY_QUANTITY);
                    FlatEditData.updateF001((decimal)SECONDARY_QUANTITY);
                    //FlatEditBarcodeData.add(BARCODE, (decimal)SECONDARY_QUANTITY, PACKING_TYPE);
                    TripHeaderData.ChangeDeliveryStatus(TripDetailDT_ID, FlatEditData.checkDeliveryPickComplete(TripDetailDT_ID));
                    return new JsonResult { Data = new { status = true, result = "令包_條碼儲存成功" } };
                }
            }
            else if (BARCODE == "P2005060002") //平張打件
            {
                ITEM_DESCRIPTION = "4AB23P00699350K250K";
                if (!FlatEditData.checkBarcodeItemDesc(FlatEditDT_ID, ITEM_DESCRIPTION))
                {
                    return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                }
                //儲存條碼資料
                FlatEditBarcodeData.addBarcode456();
                FlatEditData.updateF002();
                //FlatEditBarcodeData.add(BARCODE, 9, PACKING_TYPE);
                TripHeaderData.ChangeDeliveryStatus(TripDetailDT_ID, FlatEditData.checkDeliveryPickComplete(TripDetailDT_ID));
                return new JsonResult { Data = new { status = true, result = "無令打件_條碼儲存成功" } };
            }
            else if (BARCODE == "P2005060003") //平張打件
            {
                ITEM_DESCRIPTION = "4DM00P03000297K476K";
                if (!FlatEditData.checkBarcodeItemDesc(FlatEditDT_ID, ITEM_DESCRIPTION))
                {
                    return new JsonResult { Data = new { status = false, result = "此條碼不符合已選擇的料號" } };
                }
                //儲存條碼資料
                FlatEditBarcodeData.addBarcode130();
                FlatEditData.updateF003();
                //FlatEditBarcodeData.add(BARCODE, 9, PACKING_TYPE);
                TripHeaderData.ChangeDeliveryStatus(TripDetailDT_ID, FlatEditData.checkDeliveryPickComplete(TripDetailDT_ID));
                return new JsonResult { Data = new { status = true, result = "無令打件_條碼儲存成功" } };
            }
            else
            {
                //找不到條碼資料
                return new JsonResult { Data = new { status = false, result = "找不到條碼資料" } };
            }
        }

        [HttpPost, ActionName("DeleteFlatEditBarcode")]
        public ActionResult DeleteFlatEditBarcode(List<long> PICKED_ID, long TripDetailDT_ID)
        {
            List<FlatEditBarcodeDT> removeBarcodeList = FlatEditBarcodeData.getRemoveBarcodeList(PICKED_ID);
            if (removeBarcodeList.Count > 0)
            {
                FlatEditData.remove(removeBarcodeList);
                if (FlatEditBarcodeData.remove(PICKED_ID))
                {
                    TripHeaderData.ChangeDeliveryStatus(TripDetailDT_ID, FlatEditData.checkDeliveryPickComplete(TripDetailDT_ID));
                    return new JsonResult { Data = new { status = true, result = "條碼刪除成功" } };
                }
                else
                {
                    return new JsonResult { Data = new { status = false, result = "條碼刪除失敗" } };
                }
            }
            else
            {
                return new JsonResult { Data = new { status = false, result = "請選擇要刪除的條碼" } };
            }

        }


        [HttpGet, ActionName("GetInsteadEdit")]
        public JsonResult GetInsteadEdit(DataTableAjaxPostViewModel data, string DELIVERY_NAME, string TRIP_NAME)
        {

            List<InsteadEditDT> model = new List<InsteadEditDT>();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet, ActionName("GetInsteadEditBarcode")]
        public JsonResult GetInsteadEditBarcode(DataTableAjaxPostViewModel data, string DELIVERY_DETAIL_ID)
        {

            List<InsteadEditBarcodeDT> model = new List<InsteadEditBarcodeDT>();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult FlatView(string id)
        {
            TripHeaderDT detailData = null;
            if (string.IsNullOrEmpty(id))
            {
                detailData = TripHeaderData.GetData()[0];
            }
            else
            {
                detailData = TripHeaderData.GetData(Convert.ToInt32(id))[0];
            }
            FlatViewModel model = new FlatViewModel();
            model.DeliveryDetailViewHeader = new DeliveryDetailViewHeader();
            model.DeliveryDetailViewHeader.TripDetailDT_ID = detailData.Id;
            model.DeliveryDetailViewHeader.CUSTOMER_LOCATION_CODE = detailData.CUSTOMER_LOCATION_CODE;
            model.DeliveryDetailViewHeader.CUSTOMER_NAME = detailData.CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_NAME = detailData.DELIVERY_NAME;
            //model.DeliveryDetailViewHeader.ORDER_NUMBER = Convert.ToString(detailData.ORDER_NUMBER);
            model.DeliveryDetailViewHeader.REMARK = detailData.NOTE;
            model.DeliveryDetailViewHeader.SHIP_CUSTOMER_NAME = detailData.SHIP_CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.SHIP_LOCATION_CODE = detailData.SHIP_LOCATION_CODE;
            model.DeliveryDetailViewHeader.TRIP_ACTUAL_SHIP_DATE = detailData.TRIP_ACTUAL_SHIP_DATE;
            model.DeliveryDetailViewHeader.TRIP_CAR = detailData.TRIP_CAR;
            model.DeliveryDetailViewHeader.TRIP_NAME = detailData.TRIP_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_STATUS = detailData.DELIVERY_STATUS;

            return View(model);
        }

        public ActionResult InsteadView(string id)
        {
            return View();
        }

        public ActionResult RollView(string id)
        {
            TripHeaderDT detailData = null;
            if (string.IsNullOrEmpty(id))
            {
                detailData = TripHeaderData.GetData()[0];
            }
            else
            {
                detailData = TripHeaderData.GetData(Convert.ToInt32(id))[0];
            }
            PaperRollViewModel model = new PaperRollViewModel();
            model.DeliveryDetailViewHeader = new DeliveryDetailViewHeader();
            model.DeliveryDetailViewHeader.TripDetailDT_ID = detailData.Id;
            model.DeliveryDetailViewHeader.CUSTOMER_LOCATION_CODE = detailData.CUSTOMER_LOCATION_CODE;
            model.DeliveryDetailViewHeader.CUSTOMER_NAME = detailData.CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_NAME = detailData.DELIVERY_NAME;
            //model.DeliveryDetailViewHeader.ORDER_NUMBER = Convert.ToString(detailData.ORDER_NUMBER);
            model.DeliveryDetailViewHeader.REMARK = detailData.NOTE;
            model.DeliveryDetailViewHeader.SHIP_CUSTOMER_NAME = detailData.SHIP_CUSTOMER_NAME;
            model.DeliveryDetailViewHeader.SHIP_LOCATION_CODE = detailData.SHIP_LOCATION_CODE;
            model.DeliveryDetailViewHeader.TRIP_ACTUAL_SHIP_DATE = detailData.TRIP_ACTUAL_SHIP_DATE;
            model.DeliveryDetailViewHeader.TRIP_CAR = detailData.TRIP_CAR;
            model.DeliveryDetailViewHeader.TRIP_NAME = detailData.TRIP_NAME;
            model.DeliveryDetailViewHeader.DELIVERY_STATUS = detailData.DELIVERY_STATUS;

            //DeliveryDetailViewHeader deliveryDetailViewHeader = new DeliveryDetailViewHeader();
            //deliveryDetailViewHeader.TRIP_NAME = "Y191226-1036357";
            //deliveryDetailViewHeader.DELIVERY_NAME = "FTY1910000150";
            //model.DeliveryDetailViewHeader = deliveryDetailViewHeader;

            return View(model);
        }

        [HttpPost]
        public ActionResult DeliveryConfirm(List<long> id)
        {
            ResultModel result = TripHeaderData.DeliveryConfirm(id);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };

            //if (result.Success)
            //{
            //    return new JsonResult { Data = new { status = true, result = "出貨確認成功" } };
            //}
            //else
            //{
            //    return new JsonResult { Data = new { status = false, result = "交運單狀態須為已揀" } };
            //}
        }



        [HttpPost]
        public ActionResult CancelConfirm(List<long> id)
        {
            ResultModel result = TripHeaderData.CancelConfirm(id);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult PrintPickList(List<long> id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    ResultModel result = tripHeaderData.PrintPickList(uow, id);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost]
        public ActionResult DeliveryAuthorize(List<long> id)
        {
            ResultModel result = TripHeaderData.DeliveryAuthorize(id);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult CancelAuthorize(List<long> id)
        {
            ResultModel result = TripHeaderData.CancelAuthorize(id);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult CancelTrip(List<long> id)
        {
            ResultModel result = TripHeaderData.CancelTrip(id);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult UpdateDeliveryDetailViewHeader(long TripDetailDT_ID)
        {
            DeliveryDetailViewHeader model = new DeliveryDetailViewHeader();
            TripHeaderDT detailData = TripHeaderData.GetData(Convert.ToInt32(TripDetailDT_ID))[0];
            model.CUSTOMER_LOCATION_CODE = detailData.CUSTOMER_LOCATION_CODE;
            model.CUSTOMER_NAME = detailData.CUSTOMER_NAME;
            model.DELIVERY_NAME = detailData.DELIVERY_NAME;
            //model.ORDER_NUMBER = Convert.ToString(detailData.ORDER_NUMBER);
            model.REMARK = detailData.NOTE;
            model.SHIP_CUSTOMER_NAME = detailData.SHIP_CUSTOMER_NAME;
            model.SHIP_LOCATION_CODE = detailData.SHIP_LOCATION_CODE;
            model.TRIP_ACTUAL_SHIP_DATE = detailData.TRIP_ACTUAL_SHIP_DATE;
            model.TRIP_CAR = detailData.TRIP_CAR;
            model.TRIP_NAME = detailData.TRIP_NAME;
            model.TripDetailDT_ID = detailData.Id;
            model.DELIVERY_STATUS = detailData.DELIVERY_STATUS;
            return PartialView("_DeliveryPartial", model);
        }


        [HttpPost]
        public ActionResult UpdateTransactionAuthorizeDates(TripDetailDTEditor selectedData)
        {
            List<TripHeaderDT> data = TripHeaderData.UpdateTransactionAuthorizeDates(selectedData);
            return new JsonResult { Data = new { data } };
        }
    }
}