using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Models.StockInventory;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Miscellaneous;
using CHPOUTSRCMES.Web.ViewModels.StockInvetory;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class StockInventoryController : Controller
    {
        StockInventoryData stockInventoryData = new StockInventoryData();

        /// <summary>
        /// 盤點-首頁 View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    StockInventoryViewModel viewModel = stockInventoryData.GetStockInvetoryViewModel(uow);
                    return View(viewModel);
                }
            }
        }

        /// <summary>
        /// 取得盤點內容View(分為盤虧、盤盈)
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        public PartialViewResult GetContent(long inventoryType)
        {
            //StockData.addDefault();
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    if (inventoryType == StockInventoryUOW.InventoryType.loss)
                    {
                        //取得使用者ID
                        var id = this.User.Identity.GetUserId();
                        LossViewModel viewModel = stockInventoryData.GetLossViewModel(uow, id);
                        return PartialView("_LossPartial", viewModel);
                    }
                    else
                    {
                        var id = this.User.Identity.GetUserId();
                        ProfitViewModel viewModel = stockInventoryData.GetProfitViewModel(uow, id);
                        return PartialView("_ProfitPartial", viewModel);
                    }
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
        public JsonResult SearchStock(DataTableAjaxPostViewModel data,long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {

                    List<StockDT> model = stockInventoryData.SearchStock(uow, organizationId, subinventoryCode, locatorId, itemNumber);

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
        /// 異動明細表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="transactionTypeId"></param>
        /// <param name="fromHistoryData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetTransactionDetail")]
        public JsonResult GetTransactionDetail(DataTableAjaxPostViewModel data, long transactionTypeId, bool fromHistoryData)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<StockInventoryDT> model = stockInventoryData.GetsStockInventoryData(uow, id, transactionTypeId, fromHistoryData);

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
        }

        /// <summary>
        /// 盤虧記錄表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="organizationId"></param>
        /// <param name="subinventoryCode"></param>
        /// <param name="locatorId"></param>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetTransactionDetailForLossHistory")]
        public JsonResult GetTransactionDetailForLossHistory (DataTableAjaxPostViewModel data, long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<StockInventoryDT> model = stockInventoryData.GetsLossStockInventoryHistoryData(uow, organizationId, subinventoryCode, locatorId, itemNumber, id);

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
        }

        /// <summary>
        /// 新增異動明細
        /// </summary>
        /// <param name="transactionTypeId"></param>
        /// <param name="stockId"></param>
        /// <param name="mQty"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTransactionDetail(long transactionTypeId, long stockId, string mQty)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockInventoryData.CreateDetail(uow, transactionTypeId, stockId, mQty, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 異動存檔
        /// </summary>
        /// <param name="transactionTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveTransactionDetail(long transactionTypeId)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = stockInventoryData.SaveTransactionDetail(uow, transactionTypeId, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 異動明細表格資料編輯
        /// </summary>
        /// <param name="detailEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DetailEditor(StockInventoryDTEditor detailEditor)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = stockInventoryData.DetailEditor(uow, detailEditor, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 標籤列印
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintProfitLabel(List<long> ID)
        {
            using (var context = new MesContext())
            {
                using (StockInventoryUOW uow = new StockInventoryUOW(context))
                {
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    return stockInventoryData.PrintProfitLabel(uow, ID, name);
                }
            }
        }


       

       


        


      


       



        


    }
}