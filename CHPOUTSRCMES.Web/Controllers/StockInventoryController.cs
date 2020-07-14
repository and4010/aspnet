using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Models.StockInventory;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Miscellaneous;
using CHPOUTSRCMES.Web.ViewModels.StockInvetory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class StockInventoryController : Controller
    {
        StockInventoryData stockInventoryData = new StockInventoryData();
        Top top = new Top();
       

        // GET: StockInventory
        public ActionResult Index()
        {
            //ViewBag.TypeItem = GetTypeItem();
            //ViewBag.ProfitItem = GetProfitItem();
            //ViewBag.SubinventoryItem = GetSubinventoryItem();
            //ViewBag.LocatorItem = GetLocatorItem();
            //ViewBag.ItemNoItem = GetItemNoItem();

            StockInventoryViewModel viewModel = stockInventoryData.GetStockInvetoryViewModel();
            return View(viewModel);
        }




        public PartialViewResult GetTop()
        {
            return PartialView("_TopPartial", top.GetViewModel(stockInventoryData.orgData));
        }

        public PartialViewResult GetContent(string TransactionType)
        {
            StockData.addDefault();
            if (TransactionType == "盤虧")
            {
                return PartialView("_LossPartial");
            }
            else
            {
                return PartialView("_ProfitPartial");
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
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadHeaderDataTables(DataTableAjaxPostViewModel data, string SubinventoryCode, string Locator, string ItemNumber, string Pay)
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

            if (Pay == "盤虧")
            {
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
                        || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                        ).ToList();
                }

                var filteredCount = model.Count;
                model = StockDTOrder.Order(data.Order, model).ToList();
                model = model.Skip(data.Start).Take(data.Length).ToList();
                return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<StockInventoryDT> model = stockInventoryData.GetLossModel();

                var totalCount = model.Count;
                string search = data.Search.Value;

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    model = model.Where(p => (p.ID.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SEGMENT3) && p.SEGMENT3.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_NO) && p.ITEM_NO.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                     || (p.PRIMARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                     || (p.SECONDARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
                }

                var filteredCount = model.Count;
                model = StockInventoryDTOrder.Order(data.Order, model).ToList();
                model = model.Skip(data.Start).Take(data.Length).ToList();
                return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost, ActionName("GetProfitDetail")]
        public JsonResult GetProfitDetail(DataTableAjaxPostViewModel data)
        {
            List<StockInventoryDT> model = stockInventoryData.GetProfitModel();

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
                     || (p.PRIMARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                     || (p.SECONDARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockInventoryDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetLossDetailForLoss")]
        public JsonResult GetLossDetailForProfit(DataTableAjaxPostViewModel data)
        {

            List<StockInventoryDT> model = stockInventoryData.GetLossModel();

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
                     || (p.PRIMARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                     || (p.SECONDARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockInventoryDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetLossDetailForProfit")]
        public JsonResult GetLossDetailForProfit(DataTableAjaxPostViewModel data, string SubinventoryCode, string Locator, string ItemNumber)
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

            List<StockInventoryDT> model = stockInventoryData.GetLossModel(SubinventoryCode, locatorId, ItemNumber);

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
                     || (p.PRIMARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                     || (p.SECONDARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockInventoryDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadBodyDataTables(DataTableAjaxPostViewModel data, string SubinventoryCode, string Locator, string ItemNumber, string Pay)
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

            if (Pay == "盤盈")
            {
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
                        || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.REASON_DESC) && p.REASON_DESC.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                        ).ToList();
                }

                var filteredCount = model.Count;
                model = StockDTOrder.Order(data.Order, model).ToList();
                model = model.Skip(data.Start).Take(data.Length).ToList();
                return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<StockInventoryModel> model = StockInventoryViewModel.GetModel(SubinventoryCode, locatorId, ItemNumber);
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
                        || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.REASON_DESC) && p.REASON_DESC.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                        ).ToList();
                }

                var filteredCount = model.Count;
                model = StockInventoryModelDTOrder.Order(data.Order, model).ToList();
                model = model.Skip(data.Start).Take(data.Length).ToList();
                return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult LossEditor(StockInventoryDTEditor stockInventoryDTEditor)
        {
            List<StockInventoryDT> data = new List<StockInventoryDT>();
            if (stockInventoryDTEditor.Action == "remove")
            {
                data = stockInventoryData.DelLossDetail(stockInventoryDTEditor.StockInventoryDTList);
            }
            else if (stockInventoryDTEditor.Action == "create")
            {

            }
            else if (stockInventoryDTEditor.Action == "edit")
            {
                data = stockInventoryData.UpdateLossDetail(stockInventoryDTEditor.StockInventoryDTList);
            }
            return new JsonResult { Data = new { data } };
        }

        [HttpPost]
        public JsonResult ProfitEditor(StockInventoryDTEditor stockInventoryDTEditor)
        {
            List<StockInventoryDT> data = new List<StockInventoryDT>();
            if (stockInventoryDTEditor.Action == "remove")
            {
                data = stockInventoryData.DelProfitDetail(stockInventoryDTEditor.StockInventoryDTList);
            }
            else if (stockInventoryDTEditor.Action == "create")
            {
                data = stockInventoryData.CreateProfitDetail(stockInventoryDTEditor.StockInventoryDTList);
            }
            else if (stockInventoryDTEditor.Action == "edit")
            {
                data = stockInventoryData.UpdateProfitDetail(stockInventoryDTEditor.StockInventoryDTList);
            }
            return new JsonResult { Data = new { data } };
        }


        [HttpPost]
        public JsonResult EditorBody(StockInventoryEditor stockInventoryEditor)
        {

            if (stockInventoryEditor.Action == "remove")
            {
                if (stockInventoryEditor.Pay == "盤盈")
                {
                    var id = StockData.source.FirstOrDefault(r => r.ID == stockInventoryEditor.StockInventoryModel.ID);
                    if (id != null)
                    {
                        StockData.source.Remove(id);
                    }
                }
                else
                {
                    var id = StockInventoryViewModel.model.FirstOrDefault(r => r.ID == stockInventoryEditor.StockInventoryModel.ID);
                    if (id != null)
                    {
                        StockInventoryViewModel.model.Remove(id);
                    }
                }

            }

            if (stockInventoryEditor.Action == "create")
            {
                List<StockDT> stockDTs = StockData.source;
                var se = stockInventoryEditor.StockInventoryModel;
                stockDTs.Add(new StockDT(stockDTs.Count + 1, 265, "INV_ORG_華紙總公司", se.SUBINVENTORY_CODE, "總倉-南崁", 0, 1, "",
                   504029, se.ITEM_NO, "P2010087", "雪面銅版紙", "平版", "", "A2005060003", "AB23", "00699", "350K250K",
                   "無令打件", "KG", se.PRIMARY_AVAILABLE_QTY * 5, se.PRIMARY_AVAILABLE_QTY * 5,
                   "RE", se.PRIMARY_AVAILABLE_QTY, se.PRIMARY_AVAILABLE_QTY, "", "", se.NOTE, "", 4, "華紙管理員", "2020-05-26", 4, "華紙管理員", "2020-05-26"));

            }

            if (stockInventoryEditor.Action == "edit")
            {

                if (stockInventoryEditor.Pay == "盤盈")
                {
                    var id = StockData.source.FirstOrDefault(r => r.ID == stockInventoryEditor.StockInventoryModel.ID);
                    if (id != null)
                    {
                        id.NOTE = stockInventoryEditor.StockInventoryModel.NOTE;
                    }
                }
                else
                {
                    var id = StockInventoryViewModel.model.FirstOrDefault(r => r.ID == stockInventoryEditor.StockInventoryModel.ID);
                    if (id != null)
                    {
                        id.NOTE = stockInventoryEditor.StockInventoryModel.NOTE;
                    }
                }
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddLossDetail(long ID, decimal Qty)
        {
            ResultModel result = stockInventoryData.AddLossDetail(ID, Qty);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult AddProfitDetail(long ID, decimal Qty)
        {
            ResultModel result = stockInventoryData.AddProfitDetail(ID, Qty);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult SaveProfitDetail()
        {
            ResultModel result = stockInventoryData.SaveProfitDetail();
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult SaveLossDetail()
        {
            ResultModel result = stockInventoryData.SaveLossDetail();
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }


        [HttpPost] //0盤盈 1盤虧
        public JsonResult SaveQtyBodyDataTables(DataTableAjaxPostViewModel data, string Id, string Qty, string Pay)
        {
            ResultModel resultModel = new ResultModel();
            StockInventoryViewModel viewModel = new StockInventoryViewModel();
            StockData stockData = new StockData();
            if (Pay == "盤虧")
            {
                var FindData = StockData.source.FirstOrDefault(r => r.ID.ToString() == Id);
                var St = StockInventoryViewModel.model.FirstOrDefault(r => r.ID.ToString() == Id);
                if (St == null)
                {
                    StockInventoryViewModel.model.Add(new StockInventoryModel
                    {
                        ID = FindData.ID,
                        ORGANIZATION_ID = FindData.ORGANIZATION_ID,
                        ORGANIZATION_NAME = FindData.ORGANIZATION_NAME,
                        SUBINVENTORY_CODE = FindData.SUBINVENTORY_CODE,
                        SUBINVENTORY_NAME = FindData.SUBINVENTORY_NAME,
                        LOCATOR_ID = FindData.LOCATOR_ID,
                        LOCATOR_TYPE = FindData.LOCATOR_TYPE,
                        SEGMENT3 = FindData.SEGMENT3,
                        INVENTORY_ITEM_ID = FindData.INVENTORY_ITEM_ID,
                        ITEM_NO = FindData.ITEM_NO,
                        OSP_BATCH_NO = FindData.OSP_BATCH_NO,
                        ITEM_DESCRIPTION = FindData.ITEM_DESCRIPTION,
                        ITEM_CATEGORY = FindData.ITEM_CATEGORY,
                        LOT_NUMBER = FindData.LOT_NUMBER,
                        BARCODE = FindData.BARCODE,
                        PapaerType = FindData.PapaerType,
                        BasicWeight = FindData.BasicWeight,
                        Specification = FindData.Specification,
                        PACKING_TYPE = FindData.PACKING_TYPE,

                        PRIMARY_UOM_CODE = FindData.PRIMARY_UOM_CODE,
                        PRIMARY_TRANSACTION_QTY = int.Parse(Qty),
                        PRIMARY_AVAILABLE_QTY = FindData.PRIMARY_AVAILABLE_QTY - (int.Parse(Qty) * 10),
                        SECONDARY_UOM_CODE = FindData.SECONDARY_UOM_CODE,
                        SECONDARY_TRANSACTION_QTY = int.Parse(Qty),
                        SECONDARY_AVAILABLE_QTY = FindData.SECONDARY_AVAILABLE_QTY == 0 ? 0 : FindData.SECONDARY_AVAILABLE_QTY - int.Parse(Qty),
                        REASON_CODE = FindData.REASON_CODE,
                        REASON_DESC = FindData.REASON_DESC,
                        NOTE = FindData.NOTE,
                        STATUS_CODE = FindData.STATUS_CODE,
                        CREATE_BY = 1,
                        CREATE_BY_USERNAME = "一力星",
                        CREATE_DATE = DateTime.Now.ToString("yyyy/MM/dd"),
                        LAST_UPDATE_BY = 1,
                        LAST_UPDATE_BY_USERNAME = "一力星",
                        LAST_UPDATE_DATE = DateTime.Now.ToString("yyyy/MM/dd"),
                    });


                    if (FindData != null)
                    {
                        FindData.PRIMARY_UOM_CODE = FindData.PRIMARY_UOM_CODE;
                        FindData.PRIMARY_TRANSACTION_QTY = int.Parse(Qty);
                        FindData.PRIMARY_AVAILABLE_QTY = FindData.PRIMARY_AVAILABLE_QTY - (int.Parse(Qty) * 10);
                        FindData.SECONDARY_UOM_CODE = FindData.SECONDARY_UOM_CODE;
                        FindData.SECONDARY_TRANSACTION_QTY = int.Parse(Qty);
                        FindData.SECONDARY_AVAILABLE_QTY = FindData.SECONDARY_AVAILABLE_QTY == 0 ? 0 : FindData.SECONDARY_AVAILABLE_QTY - int.Parse(Qty);

                    }
                    resultModel.Success = true;
                }
                else
                {
                    resultModel.Success = false;
                    resultModel.Msg = "資料已存在";
                }


                List<StockInventoryModel> model = StockInventoryViewModel.model;
                //model = StockInventoryModelDTOrder.Order(data.Order, model).ToList();
                model = model.Skip(data.Start).Take(data.Length).ToList();
                var model1 = model.Skip(data.Start).Take(data.Length).ToList();

                return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1, resultModel }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var St = StockData.source.FirstOrDefault(r => r.ID.ToString() == Id);

                if (St != null)
                {

                    St.PRIMARY_UOM_CODE = St.PRIMARY_UOM_CODE;
                    St.PRIMARY_TRANSACTION_QTY = int.Parse(Qty);
                    St.PRIMARY_AVAILABLE_QTY = St.PRIMARY_AVAILABLE_QTY == 0 ? 0 : St.PRIMARY_AVAILABLE_QTY + (int.Parse(Qty) * 10);
                    St.SECONDARY_UOM_CODE = St.SECONDARY_UOM_CODE;
                    St.SECONDARY_TRANSACTION_QTY = int.Parse(Qty);
                    St.SECONDARY_AVAILABLE_QTY = St.SECONDARY_AVAILABLE_QTY == 0 ? 0 : St.SECONDARY_AVAILABLE_QTY + int.Parse(Qty);
                    St.CREATE_DATE = DateTime.Now.ToString("yyyy/MM/dd");
                    St.LAST_UPDATE_DATE = DateTime.Now.ToString("yyyy/MM/dd");

                    resultModel.Success = true;
                }
                else
                {
                    resultModel.Success = false;
                    resultModel.Msg = "資料已存在";
                }


                List<StockDT> model = StockData.source;

                //model = StockDTOrder.Order(data.Order, model).ToList();
                model = model.Skip(data.Start).Take(data.Length).ToList();
                var model1 = model.Skip(data.Start).Take(data.Length).ToList();

                return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1, resultModel }, JsonRequestBehavior.AllowGet);

            }



        }


        private List<SelectListItem> GetTypeItem()
        {
            List<SelectListItem> GetTypeItem = new List<SelectListItem>();
            GetTypeItem.Add(new SelectListItem()
            {
                Text = "紙捲",
                Value = "紙捲",
                Selected = false,
            });
            GetTypeItem.Add(new SelectListItem()
            {
                Text = "平張",
                Value = "平張",
                Selected = false,
            });
            return GetTypeItem;
        }

        private List<SelectListItem> GetProfitItem()
        {
            List<SelectListItem> GetProfitItem = new List<SelectListItem>();
            GetProfitItem.Add(new SelectListItem()
            {
                Text = "請選擇",
                Value = "請選擇",
                Selected = false,
            });
            GetProfitItem.Add(new SelectListItem()
            {
                Text = "盤盈",
                Value = "盤盈",
                Selected = false,
            });
            GetProfitItem.Add(new SelectListItem()
            {
                Text = "盤虧",
                Value = "盤虧",
                Selected = false,
            });
            return GetProfitItem;
        }

        private List<SelectListItem> GetSubinventoryItem()
        {
            List<SelectListItem> GetTypeItem = new List<SelectListItem>();
            GetTypeItem.Add(new SelectListItem()
            {
                Text = "TB2",
                Value = "TB2",
                Selected = false,
            });
            return GetTypeItem;
        }

        private List<SelectListItem> GetLocatorItem()
        {
            List<SelectListItem> GetTypeItem = new List<SelectListItem>();
            GetTypeItem.Add(new SelectListItem()
            {
                Text = "SFG",
                Value = "SFG",
                Selected = false,
            });
            return GetTypeItem;
        }

        private List<SelectListItem> GetItemNoItem()
        {
            List<SelectListItem> GetTypeItem = new List<SelectListItem>();
            GetTypeItem.Add(new SelectListItem()
            {
                Text = "4FHIZA025500635RL00",
                Value = "4FHIZA025500635RL00",
                Selected = false,
            });
            GetTypeItem.Add(new SelectListItem()
            {
                Text = "4FHIZA03000787RL00",
                Value = "4FHIZA03000787RL00",
                Selected = false,
            });
            return GetTypeItem;
        }


        public class StockInventoryEditor
        {
            public string Pay { get; set; }
            public string Action { get; set; }
            public StockInventoryModel StockInventoryModel { get; set; }
        }

    }
}