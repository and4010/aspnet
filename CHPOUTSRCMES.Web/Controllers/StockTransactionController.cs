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
        

        /// <summary>
        /// 庫存移轉首頁View
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 紙捲匯入View
        /// </summary>
        /// <returns></returns>
        public ActionResult _ImportBodyRoll()
        {
            return PartialView("~/Views/StockTransaction/_InBoundRollImport.cshtml");
        }

        /// <summary>
        /// 平張匯入View
        /// </summary>
        /// <returns></returns>
        public ActionResult _ImportBodyFlat()
        {
            return PartialView("~/Views/Purchase/_ImportBodyFlat.cshtml");
        }

        /// <summary>
        /// 併板View
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MergeBarcodeDialog(List<long> IDs)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    MergeBarcodeViewModel viewModel = stockTransferData.GetMergeBarcodeViewModel(uow, IDs);
                    return PartialView("_MergeBarcodePartial", viewModel);
                }
            }
        }

        /// <summary>
        /// 取得庫存移轉內容View(分為出庫、入庫、貨故)
        /// </summary>
        /// <param name="TransferType"></param>
        /// <returns></returns>
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
                        return PartialView("_TransferReasonPartial", stockTransferData.GetTransferReasonViewModel(uow, id));
                    }
                }
            }
        }

        /// <summary>
        /// 取得使用者倉庫下拉選單
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetSubinventoryListForUserId")]
        public JsonResult GetSubinventoryListForUserId(string ORGANIZATION_ID)
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<SelectListItem> items = stockTransferData.getSubinventoryListForUserId(uow, id, ORGANIZATION_ID, MasterUOW.DropDownListType.Choice).ToList();
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 取得倉庫下拉選單
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetSubinventoryList")]
        public JsonResult GetSubinventoryList(string ORGANIZATION_ID)
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<SelectListItem> items = stockTransferData.getSubinventoryList(uow, ORGANIZATION_ID, MasterUOW.DropDownListType.Choice).ToList();
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 取得使用者儲位下拉選單
        /// </summary>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得儲位下拉選單
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetLocatorList")]
        public JsonResult GetLocatorList(string ORGANIZATION_ID, string SUBINVENTORY_CODE)
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    List<SelectListItem> items = stockTransferData.GetLocatorList(uow, ORGANIZATION_ID, SUBINVENTORY_CODE, MasterUOW.DropDownListType.Choice).ToList();
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 取得入庫出貨編號清單
        /// </summary>
        /// <param name="outOrganizationId"></param>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="inOrganizationId"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得出庫出貨編號清單
        /// </summary>
        /// <param name="outOrganizationId"></param>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="inOrganizationId"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <returns></returns>
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

        

        /// <summary>
        /// 取得自動完成料號清單
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 取得併板後結果
        /// </summary>
        /// <param name="MergeBarocde"></param>
        /// <param name="waitMergeIDs"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得料號資料
        /// </summary>
        /// <param name="ITEM_NO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetStockItemData")]
        public JsonResult GetStockItemData(string ITEM_NO)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    return new JsonResult { Data = stockTransferData.GetItemNumberData(uow, ITEM_NO) };
                }
            }
        }


        /// <summary>
        /// 庫存查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="organizationId"></param>
        /// <param name="subinventoryCode"></param>
        /// <param name="locatorId"></param>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        [HttpPost, ActionName("SearchStock")]
        public JsonResult SearchStock(DataTableAjaxPostViewModel data, long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {

                    List<StockDT> model = stockTransferData.SearchStock(uow, organizationId, subinventoryCode, locatorId, itemNumber);

                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => (p.SUB_ID.ToString().ToLower().Contains(search.ToLower()))
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
            }
        }

        /// <summary>
        /// 出庫備貨單產生表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="transferHeaderId"></param>
        /// <param name="numberStatus"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得出貨編號資料
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得出貨編號資料
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetShipmentNumberDataForShipmentNumber")]
        public JsonResult GetShipmentNumberData(string shipmentNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {

                    return new JsonResult { Data = stockTransferData.GetShipmentNumberData(uow, shipmentNumber) };
                }

            }
        }

        /// <summary>
        /// 庫存移轉出庫轉入庫
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <returns></returns>
        [HttpPost, ActionName("OutBoundToInbound")]
        public JsonResult OutBoundToInbound(long transferHeaderId)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    return new JsonResult { Data = stockTransferData.OutBoundToInbound(uow, transferHeaderId, id, name) };
                }

            }
        }

        /// <summary>
        /// 入庫揀貨表格資料編輯
        /// </summary>
        /// <param name="pickEditor"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 出庫備貨單表格資料編輯
        /// </summary>
        /// <param name="detailEditor"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 出庫揀貨表格資料編輯
        /// </summary>
        /// <param name="pickEditor"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 出庫條碼驗收表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="transferHeaderId"></param>
        /// <param name="numberStatus"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 入庫條碼驗收表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="transferHeaderId"></param>
        /// <param name="numberStatus"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetInboundStockTransferBarcodeDT")]
        public JsonResult GetInboundStockTransferBarcodeDT(DataTableAjaxPostViewModel data, long transferHeaderId, string numberStatus)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
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


        /// <summary>
        /// 出庫新增備貨單明細
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
        /// <param name="requestedQty"></param>
        /// <param name="rollReamQty"></param>
        /// <returns></returns>
        [HttpPost, ActionName("OutboundCreateDetail")]
        public JsonResult OutboundCreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId,
            string requestedQty, string rollReamQty)
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

        /// <summary>
        /// 出庫新增條碼驗收明細
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <param name="transferDetailId"></param>
        /// <param name="barcode"></param>
        /// <param name="reamQty"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 入庫新增入庫單明細
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
        /// <param name="requestedQty"></param>
        /// <param name="rollReamWt"></param>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        [HttpPost, ActionName("InboundCreateDetail")]
        public JsonResult InboundCreateDetail(string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId,
            string requestedQty, string rollReamWt, string lotNumber)
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


        /// <summary>
        /// 入庫Excel匯入
        /// </summary>
        /// <param name="excelList"></param>
        /// <param name="shipmentNumber"></param>
        /// <param name="transferType"></param>
        /// <param name="outOrganizationId"></param>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="outLocatorId"></param>
        /// <param name="inOrganizationId"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <param name="inLocatorId"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 轉已入庫
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [HttpPost, ActionName("BarcodeInboundForShipmentNumber")]
        public JsonResult BarcodeInbound(string shipmentNumber, string barcode)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.ChangeToAlreadyInBoundForShipmentNumber(uow, shipmentNumber, barcode, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 出庫存檔
        /// </summary>
        /// <param name="transferHeaderId"></param>
        /// <returns></returns>
        [HttpPost, ActionName("OutBoundSaveTransfer")]
        public JsonResult OutBoundSaveTransfer(long transferHeaderId)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.OutBoundSaveTransfer(uow, transferHeaderId, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

       

        /// <summary>
        /// 入庫存檔
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <returns></returns>
        [HttpPost, ActionName("InBoundSaveTransferForShipmentNumber")]
        public JsonResult InBoundSaveTransfer(string shipmentNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.InBoundSaveTransferForShipmentNumber(uow, shipmentNumber, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 直接入庫存檔
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <returns></returns>
        [HttpPost, ActionName("InBoundSaveTransferNoCheckStockStatusForShipmentNumber")]
        public JsonResult InBoundSaveTransferNoCheckStockStatus(string shipmentNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockTransferData.InBoundSaveTransferNoCheckStockStatusForShipmentNumber(uow, shipmentNumber, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 貨故異動存檔
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost, ActionName("SaveReason")]
        public JsonResult SaveReason(FormCollection formCollection)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var files = Request.Files;
                 
                    ResultModel result = stockTransferData.SaveReason(uow, files, Int64.Parse(formCollection["stockId"]),
                        formCollection["reasonCode"],
                        formCollection["transferLocatorId"] == null ? default(long?) : Int64.Parse(formCollection["transferLocatorId"]),
                        formCollection["note"], id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


        /// <summary>
        /// 併板
        /// </summary>
        /// <param name="MergeBarocde"></param>
        /// <param name="waitMergeIDs"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 紙捲Excel匯入資料預覽表格
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ddlInSubinventory"></param>
        /// <param name="ddlInLocator"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 平張Excel匯入資料預覽表格
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ddlInSubinventory"></param>
        /// <param name="ddlInLocator"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 列印入庫標籤
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 列印出庫標籤
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 待列印轉待入庫
        /// </summary>
        /// <param name="transferPickedIdList"></param>
        /// <returns></returns>
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

        #region 報表
        /// <summary>
        /// 備貨單
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <returns></returns>
        public ActionResult OutboundPickingReport(string shipmentNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
#if DEBUG
                    var result = stockTransferData.LocalOutboundPickingReportViewer (uow, shipmentNumber);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#else
                    
                    var result = stockTransferData.RemoteOutboundPickingReportViewer(shipmentNumber);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#endif
                }
            }
        }

        /// <summary>
        /// 入庫單
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <returns></returns>
        public ActionResult InboundRollPickingReport(string shipmentNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
#if DEBUG
                    var result = stockTransferData.LocalInboundRollPickingReportViewer(uow, shipmentNumber);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#else
                    
                    var result = stockTransferData.RemoteInboundRollPickingReportViewer(shipmentNumber);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#endif
                }
            }
        }

        /// <summary>
        /// 入庫單
        /// </summary>
        /// <param name="shipmentNumber"></param>
        /// <returns></returns>
        public ActionResult InboundFlatPickingReport(string shipmentNumber)
        {
            using (var context = new MesContext())
            {
                using (TransferUOW uow = new TransferUOW(context))
                {
#if DEBUG
                    var result = stockTransferData.LocalInboundFlatPickingReportViewer(uow, shipmentNumber);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#else
                    
                    var result = stockTransferData.RemoteInboundFlatPickingReportViewer(shipmentNumber);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#endif
                }
            }
        }

        #endregion

        #region Excell匯入範例下載

        /// <summary>
        /// 庫存移轉入庫範例
        /// </summary>
        public void DownloadExcelSampleFile()
        {
            //用戶端的物件
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] file = null;
            string filepath = Server.MapPath("../File/庫存移轉入庫範例.zip");
            try
            {
                //用戶端下載檔案到byte陣列
                file = wc.DownloadData(filepath);
            }
            catch (Exception ex)
            {
                Response.Write("ASP.net禁止下載此敏感檔案(通常為：.cs、.vb、微軟資料庫mdb、mdf和config組態檔等)。<br/>檔案路徑：" + filepath + "<br/>錯誤訊息：" + ex.ToString());
                return;
            }
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;
            string fileName = System.IO.Path.GetFileName(filepath);
            //跳出視窗，讓用戶端選擇要儲存的地方                         //使用Server.UrlEncode()編碼中文字才不會下載時，檔名為亂碼
            Response.AddHeader("Content-Disposition", "Attachment;FileName=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            //設定MIME類型為二進位檔案
            Response.ContentType = "Application/xls";

            try
            {
                //檔案有各式各樣，所以用BinaryWrite
                Response.BinaryWrite(file);

            }
            catch (Exception ex)
            {
                Response.Write("檔案輸出有誤，您可以在瀏覽器的URL網址貼上以下路徑嘗試看看。<br/>檔案路徑：" + filepath + "<br/>錯誤訊息：" + ex.ToString());
                return;
            }

            //這是專門寫文字的
            //HttpContext.Current.Response.Write();
            Response.End();
        }

        /// <summary>
        /// 期初開帳Excel範例檔
        /// </summary>
        public void DownloadExcelSampleFile2()
        {
            //用戶端的物件
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] file = null;
            string filepath = Server.MapPath("../File/期初開帳範例.zip");
            try
            {
                //用戶端下載檔案到byte陣列
                file = wc.DownloadData(filepath);
            }
            catch (Exception ex)
            {
                Response.Write("ASP.net禁止下載此敏感檔案(通常為：.cs、.vb、微軟資料庫mdb、mdf和config組態檔等)。<br/>檔案路徑：" + filepath + "<br/>錯誤訊息：" + ex.ToString());
                return;
            }
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;
            string fileName = System.IO.Path.GetFileName(filepath);
            //跳出視窗，讓用戶端選擇要儲存的地方                         //使用Server.UrlEncode()編碼中文字才不會下載時，檔名為亂碼
            Response.AddHeader("Content-Disposition", "Attachment;FileName=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            //設定MIME類型為二進位檔案
            Response.ContentType = "Application/xls";

            try
            {
                //檔案有各式各樣，所以用BinaryWrite
                Response.BinaryWrite(file);

            }
            catch (Exception ex)
            {
                Response.Write("檔案輸出有誤，您可以在瀏覽器的URL網址貼上以下路徑嘗試看看。<br/>檔案路徑：" + filepath + "<br/>錯誤訊息：" + ex.ToString());
                return;
            }

            //這是專門寫文字的
            Response.End();
        }

        #endregion
    }
}