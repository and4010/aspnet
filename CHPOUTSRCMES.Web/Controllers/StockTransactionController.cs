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

namespace CHPOUTSRCMES.Web.Controllers
{

    public class StockTransactionController : Controller
    {
        StockTransferData stockTransferData = new StockTransferData();
        StockTransferBarcodeData stockTransferBarcodeData = new StockTransferBarcodeData();
        Top top = new Top();

        //
        // GET: /StockTransaction/
        public ActionResult Index()
        {
            StockTransferViewModel viewModel = StockTransferData.GetViewModel();
            return View(viewModel);
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
            MergeBarcodeViewModel vieModel = stockTransferBarcodeData.GetMergeBarcodeViewModel(IDs);
            return PartialView("_MergeBarcodePartial", vieModel);
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
                    return PartialView("_TopPartial", top.GetViewModel(uow, stockTransferData.orgData));
                }
            }
        }

        public PartialViewResult GetContent(string TransferType)
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    StockData.addDefault();
                    if (TransferType == "出庫")
                    {
                        return PartialView("_OutBoundPartial", stockTransferData.GetOutBoundViewModel(uow));
                    }
                    else if (TransferType == "入庫")
                    {
                        return PartialView("_InBoundPartial", stockTransferData.GetInBoundViewModel(uow));
                    }
                    else
                    {
                        return PartialView("_TransferReasonPartial", stockTransferData.GetTransferReasonViewModel(uow));
                    }
                }
            }
        }

        [HttpPost, ActionName("GetLocatorList")]
        public JsonResult GetLocatorList(string SUBINVENTORY_CODE)
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    List<SelectListItem> items = stockTransferData.GetLocatorList(uow, SUBINVENTORY_CODE, MasterUOW.DropDownListType.Choice).ToList();
                    return Json(items, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost, ActionName("GetShipmentNumberList")]
        public JsonResult GetShipmentNumberList(string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator)
        {
            List<SelectListItem> items = stockTransferData.GetShipmentNumberList(OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("AutoCompleteShipmentNumber")]
        public JsonResult AutoCompleteShipmentNumber(string TransactionType, string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string Prefix)
        {
            List<AutoCompletedItem> items = stockTransferData.AutoCompleteShipmentNumber(TransactionType, OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator, Prefix);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("InboundAutoCompleteShipmentNumber")]
        public JsonResult InboundAutoCompleteShipmentNumber(string TransactionType, string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string Prefix)
        {
            List<AutoCompletedItem> items = stockTransferData.AutoCompleteShipmentNumber(TransactionType, OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator, Prefix, "MES未出庫"); //不顯示MES未出庫的資料
            return Json(items, JsonRequestBehavior.AllowGet);
        }

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
        public JsonResult InboundAutoCompleteItemNumber(string InSubinventoryCode, string Prefix)
        {
            List<AutoCompletedItem> items = StockData.AutoCompleteItemNumber(InSubinventoryCode, Prefix);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutoCompleteItemNumber(string SubinventoryCode, string Locator, string Prefix)
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

            List<AutoCompletedItem> items = StockData.AutoCompleteItemNumber(SubinventoryCode, locatorId, Prefix);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("CheckTransactionType")]
        public JsonResult CheckTransactionType(string OutSubinventoryCode, string InSubinventoryCode)
        {
            ResultModel result = stockTransferData.CheckTransactionType(OutSubinventoryCode, InSubinventoryCode);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };

        }

        [HttpPost, ActionName("GetMergeBarocdeStatus")]
        public JsonResult GetMergeBarocdeStatus(string MergeBarocde, List<long> waitMergeIDs)
        {
            return stockTransferBarcodeData.GetMergeBarocdeStatus(MergeBarocde, waitMergeIDs);
        }

        [HttpPost, ActionName("GetStockItemData")]
        public JsonResult GetStockItemData(string SUBINVENTORY_CODE, string ITEM_NO)
        {
            List<StockDT> list = StockData.GetStockItemData(SUBINVENTORY_CODE, ITEM_NO);
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

        [HttpPost, ActionName("GetStockTransferDT")]
        public JsonResult GetStockTransferDT(DataTableAjaxPostViewModel data, string TransactionType, string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string Number, string NumberStatus)
        {
            List<StockTransferDT> model;
            if (TransactionType == "出貨編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "非MES入庫手動新增" && NumberStatus != "非MES入庫檔案匯入" && NumberStatus != "非MES已入庫")
            {
                model = stockTransferData.GetModelFromShipmentNumber(OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator, Number);
            }
            else if (TransactionType == "移轉編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "非MES入庫手動新增" && NumberStatus != "非MES入庫檔案匯入" && NumberStatus != "非MES已入庫")
            {
                model = stockTransferData.GetModelFromSubinventoryTransferNumber(OutSubinventoryCode, OutLocator, InSubinventoryCode, InLocator, Number);
            }
            else
            {
                model = new List<StockTransferDT>();
            }

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
                    || (!string.IsNullOrEmpty(p.SRC_REQUESTED_QUANTITY_UOM2) && p.SRC_REQUESTED_QUANTITY_UOM2.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockTransferDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
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


        [HttpPost, ActionName("GetStockTransferBarcodeDT")]
        public JsonResult GetStockTransferBarcodeDT(DataTableAjaxPostViewModel data, string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string Number, string NumberStatus)
        {
            List<StockTransferBarcodeDT> model;
            if (TransactionType == "出貨編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "非MES入庫手動新增" && NumberStatus != "非MES入庫檔案匯入" && NumberStatus != "非MES已入庫")
            {
                model = stockTransferBarcodeData.GetModelFromShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number);
            }
            else if (TransactionType == "移轉編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "非MES入庫手動新增" && NumberStatus != "非MES入庫檔案匯入" && NumberStatus != "非MES已入庫")
            {
                model = stockTransferBarcodeData.GetModelFromSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number);
            }
            else
            {
                model = new List<StockTransferBarcodeDT>();
            }

            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (p.StockTransferDT_ID.ToString().ToLower().Contains(search.ToLower()))
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


        [HttpPost, ActionName("GetInboundStockTransferBarcodeDT")]
        public JsonResult GetInboundStockTransferBarcodeDT(DataTableAjaxPostViewModel data, string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string Number, string NumberStatus)
        {
            List<StockTransferBarcodeDT> model;
            if (TransactionType == "出貨編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "MES未出庫")
            {
                model = stockTransferBarcodeData.GetModelFromShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number); //不顯示MES未出庫的資料
            }
            else if (TransactionType == "移轉編號" && !string.IsNullOrEmpty(Number) && NumberStatus != "MES未出庫")
            {
                model = stockTransferBarcodeData.GetModelFromSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, Number); //不顯示MES未出庫的資料
            }
            else
            {
                model = new List<StockTransferBarcodeDT>();
            }

            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (p.StockTransferDT_ID.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LOT_NUMBER) && p.LOT_NUMBER.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PACKING_TYPE) && p.PACKING_TYPE.ToLower().Contains(search.ToLower()))
                     || (p.PRIMARY_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                     || (!string.IsNullOrEmpty(p.PRIMARY_UOM) && p.PRIMARY_UOM.ToLower().Contains(search.ToLower()))
                     || (p.SECONDARY_QUANTITY.ToString().ToLower().Contains(search.ToLower()))
                     || (!string.IsNullOrEmpty(p.SECONDARY_UOM) && p.SECONDARY_UOM.ToLower().Contains(search.ToLower()))
                   || (!string.IsNullOrEmpty(p.REMARK) && p.SECONDARY_UOM.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = InboundStockTransferBarcodeDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("SaveStockTransferBarcodeDT")]
        public JsonResult SaveStockTransferBarcodeDT(string TransactionType, string Number, long StockTransferDT_ID, string BARCODE, decimal? InputReamQty)
        {
            ResultModel result = stockTransferBarcodeData.SaveStockTransferBarcodeDT(TransactionType, Number, StockTransferDT_ID, BARCODE, InputReamQty);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost, ActionName("CreateInboundBarcode")]
        public JsonResult CreateInboundBarcode(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal ROLL_REAM_WT, string LOT_NUMBER)
        {
            ResultModel result = stockTransferBarcodeData.CreateInboundBarcode(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID,
            ref Number, ITEM_NUMBER, REQUESTED_QTY, ROLL_REAM_WT, LOT_NUMBER, "非MES入庫手動新增");
            return new JsonResult { Data = new { status = result.Success, result = result.Msg, number = Number } };
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
        public JsonResult BarcodeInbound(string TransactionType, string Number, string BARCODE)
        {
            ResultModel result = stockTransferBarcodeData.BarcodeInbound(TransactionType, Number, BARCODE);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
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
        public JsonResult InBoundSaveTransfer(string TransactionType, string Number)
        {
            ResultModel result = stockTransferData.InBoundSaveTransfer(TransactionType, Number);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
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
            ResultModel result = stockTransferBarcodeData.MergeBarcode(MergeBarocde, waitMergeIDs);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
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
    }
}