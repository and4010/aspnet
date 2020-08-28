using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.ViewModels.StockTransaction;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.Jsons.Requests;
using System.IO;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using Microsoft.AspNet.Identity;
using CHPOUTSRCMES.Web.DataModel.Entiy.Transfer;
using System.Security.Claims;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class StockTransactionController : Controller
    {
        StockTransferData stockTransferData = new StockTransferData();
        StockTransferBarcodeData stockTransferBarcodeData = new StockTransferBarcodeData();
        Top top = new Top();

        //
        // GET: /StockTransaction/
        public ActionResult Index()
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    StockTransferViewModel viewModel = stockTransferData.GetViewModel(uow);
                    return View(viewModel);
                }
            }
        }

        public ActionResult _ImportBodyRoll()
        {
            return PartialView("~/Views/Purchase/_ImportBodyRoll.cshtml");
        }

        public ActionResult _ImportBodyFlat()
        {
            return PartialView("~/Views/Purchase/_ImportBodyFlat.cshtml");
        }

        [HttpPost]
        public ActionResult MergeBarcodeDialog(List<long> IDs)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    MergeBarcodeViewModel vieModel = stockTransferData.GetMergeBarcodeViewModel(uow, IDs);
                    return PartialView("_MergeBarcodePartial", vieModel);
                }
            }
        }



        //public ActionResult StockTransfer()
        //{
        //    StockTransferViewModel viewModel = StockTransferData.GetViewModel();
        //    return View(viewModel);
        //}

        public PartialViewResult GetTop()
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    //取得使用者帳號
                    //var name = this.User.Identity.GetUserName();
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    return PartialView("_TopPartial", top.GetViewModel(uow, stockTransferData.orgData, id));
                }
            }
        }

        public PartialViewResult GetContent(string TransferType)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    //var name = this.User.Identity.GetUserName();
                    //StockData.addDefault();
                    //取得使用者角色
                    var userIdentity = (ClaimsIdentity)User.Identity;
                    var claims = userIdentity.Claims;
                    var roleClaimType = userIdentity.RoleClaimType;
                    var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                    if (TransferType == TransferUOW.TransferType.Outbound)
                    {
                        return PartialView("_OutBoundPartial", stockTransferData.GetOutBoundViewModel(uow, id));
                    }
                    else if (TransferType == TransferUOW.TransferType.InBound)
                    {
                        return PartialView("_InBoundPartial", stockTransferData.GetInBoundViewModel(uow, roles, id));
                    }
                    else
                    {
                        return PartialView("_TransferReasonPartial", stockTransferData.GetTransferReasonViewModel(uow));
                    }
                }
            }
        }

        [HttpPost, ActionName("GetLocatorListForUserId")]
        public JsonResult GetLocatorList(string SUBINVENTORY_CODE)
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<SelectListItem> items = stockTransferData.GetLocatorListForUserId(uow, id, SUBINVENTORY_CODE, MasterUOW.DropDownListType.Choice).ToList();
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
        }
        
        [HttpPost, ActionName("GetInboundShipmentNumberList")]
        public JsonResult GetInboundShipmentNumberList(long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    return new JsonResult { Data = stockTransferData.GetInboundShipmentNumberList(uow, outOrganizationId, outSubinventoryCode, inOrganizationId, inSubinventoryCode) };
                }
            }
        }

        [HttpPost, ActionName("GetOutboundShipmentNumberList")]
        public JsonResult GetOutboundShipmentNumberList(long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    return new JsonResult { Data = stockTransferData.GetOutboundShipmentNumberList(uow, outOrganizationId, outSubinventoryCode, inOrganizationId, inSubinventoryCode) };
                }
            }
        }

        //[HttpPost, ActionName("AutoCompleteShipmentNumber")]
        //public JsonResult AutoCompleteShipmentNumber(string TransactionType, string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string Prefix)
        //{
        //    List<AutoCompletedItem> items = stockTransferData.AutoCompleteShipmentNumber(TransactionType, OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator, Prefix);
        //    return Json(items, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost, ActionName("InboundAutoCompleteShipmentNumber")]
        //public JsonResult InboundAutoCompleteShipmentNumber(string TransactionType, string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string Prefix)
        //{
        //    List<AutoCompletedItem> items = stockTransferData.AutoCompleteShipmentNumber(TransactionType, OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator, Prefix, "MES未出庫"); //不顯示MES未出庫的資料
        //    return Json(items, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost, ActionName("GetSubinventoryTransferNumberList")]
        public JsonResult GetSubinventoryTransferNumberList(string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator)
        {
            List<SelectListItem> items = stockTransferData.GetSubinventoryTransferNumberList(OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetItemNumberList")]
        public JsonResult GetItemNumberList(string SubinventoryCode, string Locator)
        {
            long locatorId;
            try
            {
                locatorId = Convert.ToInt64(Locator);
            }
            catch
            {
                locatorId = 0;
            }
            List<SelectListItem> items = StockData.GetItemNumberList(SubinventoryCode, locatorId).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutoCompleteItemNumber(string Prefix)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    List<AutoCompletedItem> items = stockTransferData.GetAutoCompleteItemNumberList(uow, Prefix);
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //[HttpPost]
        //public JsonResult InboundAutoCompleteItemNumber(string InSubinventoryCode, string Prefix)
        //{
        //    List<AutoCompletedItem> items = StockData.AutoCompleteItemNumber(InSubinventoryCode, Prefix);
        //    return Json(items, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult AutoCompleteItemNumber(string SubinventoryCode, string Locator, string Prefix)
        //{
        //    long locatorId;
        //    try
        //    {
        //        locatorId = Convert.ToInt64(Locator);
        //    }
        //    catch
        //    {
        //        locatorId = 0;
        //    }

        //    List<AutoCompletedItem> items = StockData.AutoCompleteItemNumber(SubinventoryCode, locatorId, Prefix);
        //    return Json(items, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost, ActionName("CheckTransactionType")]
        public JsonResult CheckTransactionType(string OutSubinventoryCode, string InSubinventoryCode)
        {
            ResultModel result = stockTransferData.CheckTransactionType(OutSubinventoryCode, InSubinventoryCode);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };

        }

        [HttpPost, ActionName("GetMergeBarocdeStatus")]
        public JsonResult GetMergeBarocdeStatus(string MergeBarocde, List<long> waitMergeIDs)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    return stockTransferData.GetMergeBarocdeStatus(uow, MergeBarocde, waitMergeIDs);
                }
            }
        }

        [HttpPost, ActionName("GetStockItemData")]
        public JsonResult GetStockItemData(string ITEM_NO)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {

                    return new JsonResult { Data = stockTransferData.GetItemNumberData(uow, ITEM_NO) };
                    //List<StockDT> list = StockData.GetStockItemData(SUBINVENTORY_CODE, ITEM_NO);
                    //if (list.Count > 0 && list[0].ITEM_CATEGORY == "平版")
                    //{
                    //    return new JsonResult { Data = new { STATUS = true, PACKING_TYPE = list[0].PACKING_TYPE, ITEM_CATEGORY = list[0].ITEM_CATEGORY, UNIT = list[0].SECONDARY_UOM_CODE } };
                    //}
                    //else if (list.Count > 0 && list[0].ITEM_CATEGORY == "捲筒")
                    //{
                    //    return new JsonResult { Data = new { STATUS = true, PACKING_TYPE = list[0].PACKING_TYPE, ITEM_CATEGORY = list[0].ITEM_CATEGORY, UNIT = list[0].PRIMARY_UOM_CODE } };
                    //}
                    //else
                    //{
                    //    return new JsonResult { Data = new { STATUS = false, PACKING_TYPE = "", ITEM_CATEGORY = "", UNIT = "" } };
                    //}
                }
            }
        }

        [HttpPost, ActionName("GetInboundStockItemData")]
        public JsonResult GetInboundStockItemData(long ORGANIZATION_ID, string ITEM_NO)
        {
            List<StockDT> list = StockData.GetInboundStockItemData(ORGANIZATION_ID, ITEM_NO);
            if (list.Count > 0 && list[0].ITEM_CATEGORY == "平版")
            {
                return new JsonResult { Data = new { STATUS = true, PACKING_TYPE = list[0].PACKING_TYPE, ITEM_CATEGORY = list[0].ITEM_CATEGORY, UNIT = list[0].SECONDARY_UOM_CODE } };
            }
            else if (list.Count > 0 && list[0].ITEM_CATEGORY == "捲筒")
            {
                return new JsonResult { Data = new { STATUS = true, PACKING_TYPE = list[0].PACKING_TYPE, ITEM_CATEGORY = list[0].ITEM_CATEGORY, UNIT = list[0].PRIMARY_UOM_CODE } };
            }
            else
            {
                return new JsonResult { Data = new { STATUS = false, PACKING_TYPE = "", ITEM_CATEGORY = "", UNIT = "" } };
            }


        }

        [HttpPost, ActionName("SearchStock")]
        public JsonResult SearchStock(DataTableAjaxPostViewModel data, string SubinventoryCode, string Locator, string ItemNumber)
        {
            long locatorId;
            try
            {
                locatorId = Convert.ToInt64(Locator);
            }
            catch
            {
                locatorId = 0;
            }

            List<StockDT> model = StockData.GetModel(SubinventoryCode, locatorId, ItemNumber);

            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (p.ID.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SEGMENT3) && p.SEGMENT3.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_NO) && p.ITEM_NO.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.REASON_DESC) && p.REASON_DESC.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetOutboundStockTransferDT")]
        public JsonResult GetOutboundStockTransferDT(DataTableAjaxPostViewModel data, long transferHeaderId, string numberStatus)
        {

            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {

                    List<StockTransferDT> model = stockTransferData.GetOutboundDetailData(uow, transferHeaderId, numberStatus);

            var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => (p.ID.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                            || (p.ROLL_REAM_QTY.ToString().ToLower().Contains(search.ToLower()))
                            || (p.REQUESTED_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                            || (p.PICKED_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM) && p.REQUESTED_QUANTITY_UOM.ToLower().Contains(search.ToLower()))
                            || (p.REQUESTED_QUANTITY2.ToString().ToLower().Contains(search.ToLower()))
                            || (p.PICKED_QUANTITY2.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.REQUESTED_QUANTITY_UOM2) && p.REQUESTED_QUANTITY_UOM2.ToLower().Contains(search.ToLower()))
                            ).ToList();
                    }

                    var filteredCount = model.Count;
                    model = StockTransferDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();

                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost, ActionName("GetShipmentNumberData")]
        public JsonResult GetShipmentNumberData(long transferHeaderId)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {

                    return new JsonResult { Data = stockTransferData.GetShipmentNumberData(uow, transferHeaderId) };
                }

            }
        }

        [HttpPost]
        public ActionResult InboundPickEditor(PickEditor pickEditor)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = stockTransferData.InboundPickEditor(uow, pickEditor, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost]
        public ActionResult OutboundDetailEditor(DetailEditor detailEditor)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = stockTransferData.OutboundDetailEditor(uow, detailEditor, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost]
        public ActionResult OutboundPickEditor(PickEditor pickEditor)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = stockTransferData.OutboundPickEditor(uow, pickEditor, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost, ActionName("GetNumberStatus")]
        public JsonResult GetNumberStatus(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string Number)
        {
            ResultModel result = stockTransferData.GetNumberStatus(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }



        [HttpPost, ActionName("CheckNumber")]
        public JsonResult CheckNumber(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string Number)
        {
            ResultModel result = stockTransferData.CheckNumber(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        //[HttpPost, ActionName("CheckCreateDetail")]
        //public JsonResult CheckCreateDetail(string shipmentNumber, string transferType, long outOrganizationId, string outSubinventoryCode, long outLocatorId, long inOrganizationId, string inSubinventoryCode, long inLocatorId)
        //{
        //    using (var context = new MesContext())
        //    {
        //        using (TransferUOW uow = new TransferUOW(context))
        //        {
        //            ResultModel result = stockTransferData.CheckNumber(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number);
        //            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        //        }
        //    }
        //}

        [HttpPost, ActionName("SaveStockTransferDT")]
        public JsonResult SaveStockTransferDT(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal PICKED_QTY, string UNIT, decimal ROLL_REAM_QTY)
        {
            ResultModel result = stockTransferData.SaveStockTransferDT(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID,
                Number, ITEM_NUMBER, REQUESTED_QTY, PICKED_QTY, UNIT, ROLL_REAM_QTY);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };

        }

        [HttpPost, ActionName("DeleteItemNumber")]
        public JsonResult DeleteItemNumber(long ID)
        {
            ResultModel result = stockTransferData.DeleteItemNumber(ID, false);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }


        [HttpPost, ActionName("GetOutboundStockTransferBarcodeDT")]
        public JsonResult GetOutboundStockTransferBarcodeDT(DataTableAjaxPostViewModel data, long transferHeaderId, string numberStatus)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {  
                    List<StockTransferBarcodeDT> model = stockTransferData.GetOutboundPickedData(uow, transferHeaderId, numberStatus);

                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => (p.SUB_ID.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                            //|| (!string.IsNullOrEmpty(p.LOT_NUMBER) && p.LOT_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                             || (p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                             || (!string.IsNullOrEmpty(p.PRIMARY_UOM) && p.PRIMARY_UOM.ToLower().Contains(search.ToLower()))
                             || (p.SECONDARY_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                             || (!string.IsNullOrEmpty(p.SECONDARY_UOM) && p.SECONDARY_UOM.ToLower().Contains(search.ToLower()))
                           || (!string.IsNullOrEmpty(p.REMARK) && p.SECONDARY_UOM.ToLower().Contains(search.ToLower()))
                            ).ToList();
                    }

                    var filteredCount = model.Count;
                    model = StockTransferBarcodeDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();

                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);

                }
            }


        }


        [HttpPost, ActionName("GetInboundStockTransferBarcodeDT")]
        public JsonResult GetInboundStockTransferBarcodeDT(DataTableAjaxPostViewModel data, long transferHeaderId, string numberStatus)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //List<StockTransferBarcodeDT> model;
                    //if (TransactionType == "出貨編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "MES未出庫")
                    //{
                    //    model = stockTransferBarcodeData.GetModelFromShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number); //不顯示MES未出庫的資料
                    //}
                    //else if (TransactionType == "移轉編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "MES未出庫")
                    //{
                    //    model = stockTransferBarcodeData.GetModelFromSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number); //不顯示MES未出庫的資料
                    //}
                    //else
                    //{
                    //    model = new List<StockTransferBarcodeDT>();
                    //}
                    List<StockTransferBarcodeDT> model = stockTransferData.GetInboundPickedData(uow, transferHeaderId, numberStatus);

                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => (p.SUB_ID.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.LOT_NUMBER) && p.LOT_NUMBER.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                             || (p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                             || (!string.IsNullOrEmpty(p.PRIMARY_UOM) && p.PRIMARY_UOM.ToLower().Contains(search.ToLower()))
                             || (p.SECONDARY_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                             || (!string.IsNullOrEmpty(p.SECONDARY_UOM) && p.SECONDARY_UOM.ToLower().Contains(search.ToLower()))
                           || (!string.IsNullOrEmpty(p.REMARK) && p.REMARK.ToLower().Contains(search.ToLower()))
                            ).ToList();
                    }

                    var filteredCount = model.Count;
                    model = InboundStockTransferBarcodeDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();

                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost, ActionName("SaveStockTransferBarcodeDT")]
        public JsonResult SaveStockTransferBarcodeDT(string TransactionType, string Number, long StockTransferDT_ID, string BARCODE, decimal? InputReamQty)
        {
            ResultModel result = stockTransferBarcodeData.SaveStockTransferBarcodeDT(TransactionType, Number, StockTransferDT_ID, BARCODE, InputReamQty);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost, ActionName("OutboundCreateDetail")]
        public JsonResult OutboundCreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId,
            decimal requestedQty, decimal rollReamQty)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();

                    ResultDataModel<TRF_HEADER_T> result = stockTransferData.OutboundCreateDetail(uow, shipmentNumber, transferType, itemNumber, outOrganizationId,
             outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId, requestedQty, rollReamQty, id, name);
                    if (result.Success)
                    {
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg, shipmentNumber = result.Data.ShipmentNumber, transferHeaderId = result.Data.TransferHeaderId } };
                    }
                    else
                    {
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }


                }
            }
        }

        [HttpPost, ActionName("OutboundCreatePick")]
        public JsonResult OutboundCreatePick(long transferHeaderId, long transferDetailId, string barcode, decimal reamQty)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();

                    ResultModel result = stockTransferData.OutboundCreatePick(uow, transferHeaderId, transferDetailId, barcode, reamQty, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }
        

        [HttpPost, ActionName("InboundCreateDetail")]
        public JsonResult InboundCreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId,
            decimal requestedQty, decimal rollReamWt, string lotNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();

                    ResultDataModel<TRF_HEADER_T> result = stockTransferData.InboundCreateDetail(uow, shipmentNumber, transferType, itemNumber, outOrganizationId,
             outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId, requestedQty, rollReamWt, lotNumber, id, name);
                    if (result.Success)
                    {
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg, shipmentNumber = result.Data.ShipmentNumber, transferHeaderId = result.Data.TransferHeaderId } };
                    }
                    else
                    {
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }
                    

                }
            }
        }



        [HttpPost, ActionName("CreateInboundBarcode")]
        public JsonResult CreateInboundBarcode(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal ROLL_REAM_WT, string LOT_NUMBER)
        {
            ResultModel result = stockTransferBarcodeData.CreateInboundBarcode(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID,
            ref Number, ITEM_NUMBER, REQUESTED_QTY, ROLL_REAM_WT, LOT_NUMBER, "非MES入庫手動新增");
            return new JsonResult { Data = new { status = result.Success, result = result.Msg, number = Number } };
        }

        [HttpPost, ActionName("InboundImportExcel")]
        public JsonResult InboundImportExcel(List<InboundImportExcelModel> excelList, string shipmentNumber, string transferType,
            long outOrganizationId, string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultDataModel<TRF_HEADER_T> result = stockTransferData.InboundImportExcel(uow, excelList, shipmentNumber, transferType,
             outOrganizationId, outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId, id, name);
                    if (result.Success)
                    {
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg, shipmentNumber = result.Data.ShipmentNumber, transferHeaderId = result.Data.TransferHeaderId } };
                    }
                    else
                    {
                        return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }
                }
            }
        }

        [HttpPost, ActionName("ImportRollInboundBarcode")]
        public JsonResult ImportRollInboundBarcode(ImportRollInboundBarcodeModel data)
        {
            string Number = data.Number;
            ResultModel result = stockTransferBarcodeData.ImportRollInboundBarcode(data, ref Number);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg, number = Number } };
        }

        [HttpPost, ActionName("ImportFlatInboundBarcode")]
        public JsonResult ImportFlatInboundBarcode(ImportFlatInboundBarcodeModel data)
        {
            string Number = data.Number;
            ResultModel result = stockTransferBarcodeData.ImportFlatInboundBarcode(data, ref Number);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg, number = Number } };
        }

        [HttpPost, ActionName("BarcodeInbound")]
        public JsonResult BarcodeInbound(long transferHeaderId, string barcode)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.ChangeToAlreadyInBound(uow, transferHeaderId, barcode, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost, ActionName("DeleteBarcode")]
        public JsonResult DeleteBarcode(List<long> IDs)
        {
            ResultModel result = stockTransferBarcodeData.DeleteBarcode(IDs, false);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost, ActionName("InboundDeleteBarcode")]
        public JsonResult InboundDeleteBarcode(List<long> IDs)
        {
            ResultModel result = stockTransferBarcodeData.DeleteBarcode(IDs, true);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost, ActionName("OutBoundSaveTransfer")]
        public JsonResult OutBoundSaveTransfer(string TransactionType, string Number)
        {
            ResultModel result = stockTransferData.OutBoundSaveTransfer(TransactionType, Number);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost, ActionName("InBoundSaveTransfer")]
        public JsonResult InBoundSaveTransfer(long transferHeaderId)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.InBoundSaveTransfer(uow, transferHeaderId, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        [HttpPost, ActionName("InBoundSaveTransferNoCheckStockStatus")]
        public JsonResult InBoundSaveTransferNoCheckStockStatus(long transferHeaderId)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.InBoundSaveTransferNoCheckStockStatus(uow, transferHeaderId, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }
        


        [HttpPost]
        public ActionResult UpdateRemark(StockTransferBarcodeDTEditor selectedData)
        {
            List<StockTransferBarcodeDT> data = stockTransferBarcodeData.UpdateRemark(selectedData);
            return new JsonResult { Data = new { data } };
        }

        [HttpPost, ActionName("SaveReason")]
        public JsonResult SaveReason(long ID, string REASON_CODE, string REASON_DESC, string Locator, string NOTE)
        {
            long locatorId;
            try
            {
                locatorId = Convert.ToInt64(Locator);
            }
            catch
            {
                locatorId = 0;
            }
            ResultModel result = StockData.SaveReason(ID, REASON_CODE, REASON_DESC, locatorId, NOTE, stockTransferData.orgData);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }



        //[HttpPost]
        //public ActionResult MergeBarcode(StockTransferBarcodeDTEditor selectedData)
        //{
        //    ResultModel result = stockTransferBarcodeData.MergeBarcode(selectedData);
        //    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        //}

        [HttpPost]
        public ActionResult MergeBarcode(string MergeBarocde, List<long> waitMergeIDs)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.MergeBarcode(uow, MergeBarocde, waitMergeIDs, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }



        [HttpPost]
        public JsonResult UploadFileRoll(HttpPostedFileBase file, string ddlInSubinventory, string ddlInLocator, DataTableAjaxPostViewModel data)
        {
            var result = new ResultModel();
            var detail = new List<StockTransferBarcodeDT>();
            if (file == null || file.ContentLength == 0)
            {
                result.Msg = "檔案不得空白";
                result.Success = false;
            }
            else
            {
                string extension = Path.GetExtension(file.FileName);
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    try
                    {
                        StockTransferBarcodeData stock = new StockTransferBarcodeData();
                        var papper = new ExcelImport();
                        papper.TransferPaperRoll(file, ref detail, ref result, ddlInSubinventory, ddlInLocator);
                        stock.importModel = detail;
                    }
                    catch (Exception e)
                    {
                        result.Msg = e.Message;
                        result.Success = false;
                    }

                }
                else
                {
                    result.Msg = "只能上傳excel文件";
                    result.Success = false;
                }
            }
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult UploadFileFlat(HttpPostedFileBase file, string ddlInSubinventory, string ddlInLocator, DataTableAjaxPostViewModel data)
        {
            var result = new ResultModel();
            var detail = new List<StockTransferDT>();
            if (file == null || file.ContentLength == 0)
            {
                result.Msg = "檔案不得空白";
                result.Success = false;
            }
            else
            {
                string extension = Path.GetExtension(file.FileName);
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    try
                    {
                        StockTransferData stock = new StockTransferData();
                        var papper = new ExcelImport();
                        papper.TransferFlat(file, ref detail, ref result, ddlInSubinventory, ddlInLocator);
                        stock.importModel = detail;
                    }
                    catch (Exception e)
                    {
                        result.Msg = e.Message;
                        result.Success = false;
                    }

                }
                else
                {
                    result.Msg = "只能上傳excel文件";
                    result.Success = false;
                }
            }
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }


        #region 標籤
        [HttpPost]
        public ActionResult PrintInboundLabel(List<long> ID)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    return stockTransferData.PrintInboundLabel(uow, ID, name);
                }
            }
        }

        [HttpPost]
        public ActionResult PrintOutboundLabel(List<long> ID)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    return stockTransferData.PrintOutboundLabel(uow, ID, name);
                }
            }
        }

        public JsonResult WaitPrintToWaitInbound(List<long> transferPickedIdList)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.WaitPrintToWaitInbound(uow, transferPickedIdList, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }
        #endregion
    }
}