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
using Microsoft.AspNet.Identity;
using CHPOUTSRCMES.Web.DataModel.Entity.Delivery;
using System.Security.Claims;
using CHPOUTSRCMES.Web.Util;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        TripHeaderData tripHeaderData = new TripHeaderData();


        /// <summary>
        /// 出貨首頁View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    //取得使用者角色
                    var userIdentity = (ClaimsIdentity)User.Identity;
                    var claims = userIdentity.Claims;
                    var roleClaimType = userIdentity.RoleClaimType;
                    var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                    DeliverySearchViewModel viewModel = tripHeaderData.GetDeliverySearchViewModel(uow, roles, id);
                    return View(viewModel);
                }
            }
        }

        /// <summary>
        /// 出貨查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="TripActualShipBeginDate"></param>
        /// <param name="TripActualShipEndDate"></param>
        /// <param name="DeliveryName"></param>
        /// <param name="SelectedSubinventory"></param>
        /// <param name="SelectedTrip"></param>
        /// <param name="TransactionDate"></param>
        /// <param name="SelectedDeliveryStatus"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeliverySearch")]
        public JsonResult DeliverySearch(DataTableAjaxPostViewModel data, string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus)
        {

            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    List<TripHeaderDT> model = tripHeaderData.DeliverySearch(uow, TripActualShipBeginDate, TripActualShipEndDate, DeliveryName, SelectedSubinventory, SelectedTrip, TransactionDate, SelectedDeliveryStatus, this.User.Identity.GetUserId());
                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => p.SUB_ID.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.FREIGHT_TERMS_NAME) && p.FREIGHT_TERMS_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.DELIVERY_NAME) && p.DELIVERY_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.CUSTOMER_NAME) && p.CUSTOMER_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.SHIP_CUSTOMER_NAME) && p.SHIP_CUSTOMER_NAME.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.TRIP_CAR) && p.TRIP_CAR.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                            || (p.TRIP_ACTUAL_SHIP_DATE.HasValue && p.TRIP_ACTUAL_SHIP_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.TRIP_NAME) && p.TRIP_NAME.ToLower().Contains(search.ToLower()))
                            || (p.TRANSACTION_DATE.HasValue && p.TRANSACTION_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.DetailType) && p.DetailType.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.DELIVERY_STATUS) && p.DELIVERY_STATUS.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.RP_SUM.ToString()) && p.RP_SUM.Normalize().ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.RS_SUM.ToString()) && p.RS_SUM.Normalize().ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.REQUESTED_PRIMARY_UOM) && p.REQUESTED_PRIMARY_UOM.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.REQUESTED_SECONDARY_UOM) && p.REQUESTED_SECONDARY_UOM.ToLower().Contains(search.ToLower()))
                            ).ToList();
                    }

                    var filteredCount = model.Count;
                    model = TripDetailDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();

                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

       
        /// <summary>
        /// 平張明細輸入View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FlatEdit(long id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    return View(tripHeaderData.GetFlatEditViewModel(uow, id));
                }
            }
        }

        
        /// <summary>
        /// 紙捲明細輸入View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RollEdit(long id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    return View(tripHeaderData.GetPaperRollEditViewModel(uow, id));
                }
            }
        }

        /// <summary>
        /// 紙捲需求表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="DlvHeaderId"></param>
        /// <param name="DELIVERY_STATUS_NAME"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetRollEdit")]
        public JsonResult GetRollEdit(DataTableAjaxPostViewModel data, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    PaperRollEditData paperRollEditData = new PaperRollEditData();
                    List<PaperRollEditDT> model = paperRollEditData.GetRollDetailDT(uow, DlvHeaderId, DELIVERY_STATUS_NAME);
                    var totalCount = model.Count;
                    string search = data.Search.Value;
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => p.SUB_ID.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.ORDER_NUMBER.ToString()) && p.ORDER_NUMBER.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ORDER_SHIP_NUMBER) && p.ORDER_SHIP_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
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
            }
        }

        /// <summary>
        /// 紙捲揀貨表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="DlvHeaderId"></param>
        /// <param name="DELIVERY_STATUS_NAME"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetRollEditBarcode")]
        public JsonResult GetRollEditBarcode(DataTableAjaxPostViewModel data, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    PaperRollEditBarcodeData paperRollEditBarcodeData = new PaperRollEditBarcodeData();
                    List<PaperRollEditBarcodeDT> model = paperRollEditBarcodeData.GetRollPickDT(uow, DlvHeaderId, DELIVERY_STATUS_NAME);
                    var totalCount = model.Count;
                    string search = data.Search.Value;
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => p.SUB_ID.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                            || p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                            || p.PRIMARY_UOM.ToString().ToLower().Contains(search.ToLower())
                            ).ToList();
                    }
                    var filteredCount = model.Count;
                    model = PaperRollEditBarcodeDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();

                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 紙捲新增揀貨條碼
        /// </summary>
        /// <param name="BARCODE"></param>
        /// <param name="DlvHeaderId"></param>
        /// <param name="DLV_DETAIL_ID"></param>
        /// <param name="DELIVERY_NAME"></param>
        /// <returns></returns>
        [HttpPost, ActionName("InputRollEditBarcode")]
        public ActionResult InputRollEditBarcode(string BARCODE, long DlvHeaderId, long DLV_DETAIL_ID, string DELIVERY_NAME)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = tripHeaderData.AddPickDT(uow, DlvHeaderId, DLV_DETAIL_ID, DELIVERY_NAME, BARCODE, null, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

       
        /// <summary>
        /// 平張需求表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="DlvHeaderId"></param>
        /// <param name="DELIVERY_STATUS_NAME"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetFlatEdit")]
        public JsonResult GetFlatEdit(DataTableAjaxPostViewModel data, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {

            
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    FlatEditData flatEditData = new FlatEditData();
                    List<FlatEditDT> model = flatEditData.GetFlatDetailDT(uow, DlvHeaderId, DELIVERY_STATUS_NAME);
                    var totalCount = model.Count;
                    string search = data.Search.Value;
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => p.SUB_ID.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.ORDER_NUMBER.ToString()) && p.ORDER_NUMBER.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ORDER_SHIP_NUMBER) && p.ORDER_SHIP_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.OSP_BATCH_NO) && p.OSP_BATCH_NO.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.REAM_WEIGHT) && p.REAM_WEIGHT.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                            || p.SRC_REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.SRC_REQUESTED_QUANTITY_UOM) && p.SRC_REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                            || p.REQUESTED_QUANTITY2.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM2) && p.REQUESTED_QUANTITY_UOM2.ToLower().Contains(search.ToLower()))
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
            }
        }


        /// <summary>
        /// 平張揀貨表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="DlvHeaderId"></param>
        /// <param name="DELIVERY_STATUS_NAME"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetFlatEditBarcode")]
        public JsonResult GetFlatEditBarcode(DataTableAjaxPostViewModel data, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    FlatEditBarcodeData flatEditBarcodeData = new FlatEditBarcodeData();
                    List<FlatEditBarcodeDT> model = flatEditBarcodeData.GetFlatPickDT(uow, DlvHeaderId, DELIVERY_STATUS_NAME);
                    var totalCount = model.Count;
                    string search = data.Search.Value;
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => p.SUB_ID.ToString().ToLower().Contains(search.ToLower())
                            || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.REAM_WEIGHT) && p.REAM_WEIGHT.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                            || p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                            || p.PRIMARY_UOM.ToString().ToLower().Contains(search.ToLower())
                            || p.SECONDARY_QUANTITY.ToString().ToLower().Contains(search.ToLower())
                            || p.SECONDARY_UOM.ToString().ToLower().Contains(search.ToLower())
                            ).ToList();
                    }
                    var filteredCount = model.Count;
                    model = FlatEditBarcodeDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();
                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 平張新增揀貨條碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("InputFlatEditBarcode")]
        //public ActionResult InputFlatEditBarcode(string BARCODE, decimal? SECONDARY_QUANTITY, long DlvHeaderId, long DLV_DETAIL_ID, string DELIVERY_NAME)
        public ActionResult InputFlatEditBarcode(FlatEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
            }
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {

                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = tripHeaderData.AddPickDT(uow, model.DLV_HEADER_ID, model.DLV_DETAIL_ID, model.DELIVERY_NAME, model.BARCODE, model.SECONDARY_QUANTITY, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }

        }

      

        /// <summary>
        /// 平張明細檢視View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FlatView(long id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    return View(tripHeaderData.GetFlatViewModel(uow, id));
                }
            }
        }

     
        /// <summary>
        /// 紙捲明細檢視View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RollView(long id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    return View(tripHeaderData.GetPaperRollViewModel(uow, id));
                }
            }
        }

        /// <summary>
        /// 出貨申請
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeliveryConfirm(List<long> id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.DeliveryConfirm(uow, id, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 取消出貨申請
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelConfirm(List<long> id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.CancelConfirm(uow, id, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 列印備貨單(變更交運單狀態 未印 => 待出)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintPickList(List<long> id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.PrintPickList(uow, id, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 出貨核准
        /// </summary>
        /// <param name="selectDatas"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeliveryAuthorize(TripDetailDTEditor selectDatas)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.DeliveryAuthorize(uow, selectDatas, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 取消出貨核准(未使用)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelAuthorize(List<long> id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.CancelAuthorize(uow, id, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 取消航程號
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelTrip(List<long> id)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.CancelTrip(uow, id, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 更新明細輸入抬頭
        /// </summary>
        /// <param name="DlvHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateDeliveryDetailViewHeader(long DlvHeaderId)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    TripHeaderData tripHeaderData = new TripHeaderData();
                    DeliveryDetailViewHeader viewModel = tripHeaderData.GetDeliveryDetailViewHeader(uow, Convert.ToInt32(DlvHeaderId));
                    return PartialView("_DeliveryPartial", viewModel);
                }
            }
        }

        /// <summary>
        /// 更新出貨核准日
        /// </summary>
        /// <param name="selectedData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateTransactionAuthorizeDates(TripDetailDTEditor selectedData)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者ID
                    var userId = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = tripHeaderData.UpdateTransactionAuthorizeDates(uow, selectedData, userId, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 揀貨表格資料編輯
        /// </summary>
        /// <param name="pickDTEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PickDTEditor(PickDTEditor pickDTEditor)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    if (pickDTEditor.Action == "remove")
                    {
                        //取得使用者ID
                        var id = this.User.Identity.GetUserId();
                        //取得使用者帳號
                        var name = this.User.Identity.GetUserName();
                        ResultModel result = tripHeaderData.DelPickDT(uow, pickDTEditor.DlvPickedIdList, id, name);
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }
                    else
                    {
                        return new JsonResult { Data = new { status = false, result = "Action無法辨識" } };
                    }
                }
            }
        }

        /// <summary>
        /// 列印標籤
        /// </summary>
        /// <param name="PICKED_ID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintLabel(List<long> PICKED_ID)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    return tripHeaderData.PritLabel(uow, PICKED_ID, name);
                }
            }
        }

        /// <summary>
        /// 出貨-備貨單
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ActionResult PickingReport(string tripName)
        {
            using (var context = new MesContext())
            {
                using (DeliveryUOW uow = new DeliveryUOW(context))
                {
#if BIOTECH
                    var result = tripHeaderData.LocalDeliveryPickingReportViewer(uow, tripName);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                    }

#else
                    
                    var result = tripHeaderData.RemoteDeliveryPickingReportViewer(tripName);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                    }

#endif
                }
            }
        }

        /// <summary>
        /// 取得進階功能狀態
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAdvancedStatus()
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            //取得使用者角色
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            bool Advanced = false;
            if (roles != null && roles.Count > 0)
            {
                foreach (Claim role in roles)
                {
                    if (role.Value == MasterUOW.UserRole.Adm || role.Value == MasterUOW.UserRole.ChpUser)
                    {
                        Advanced = true;
                        break;
                    }
                    else
                    {
                        Advanced = false;
                    }
                }
            }
            return new JsonResult { Data = new { status = Advanced, result = "" } };

        }
    }
}