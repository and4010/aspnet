using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.StockQuery;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.StockQuery;
using CHPOUTSRCMES.Web.ViewModels.StockTxnQuery;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        /// <summary>
        /// 庫存查詢-首頁 View
        /// </summary>
        /// <returns></returns>
        // GET: /Stock/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 照片檢視 View
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ItemNumber"></param>
        /// <returns></returns>
        public ActionResult PhotoView(string Id , string ItemNumber)
        {
            StockDetailQueryModel stockDetailQueryModel = new StockDetailQueryModel();
            stockDetailQueryModel.StockId = System.Int64.Parse(Id);
            stockDetailQueryModel.ItemNumber = ItemNumber;
            return View(stockDetailQueryModel);
        }

        /// <summary>
        /// 取得照片清單
        /// </summary>
        /// <param name="STOCK_ID"></param>
        /// <returns></returns>
        public JsonResult GetPhoto(long STOCK_ID)
        {
            StockDetailQueryModel stockDetailQueryModel = new StockDetailQueryModel();
            var photo = stockDetailQueryModel.GetPhoto(STOCK_ID);
            return Json(new { photo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 庫存查詢
        /// </summary>
        /// <returns></returns>
        // GET: /Stock/Query
        public ActionResult Query()
        {
            var model = new QueryViewModel();
            model.SubinvenotoryList = QueryViewModel.getSubinvenotoryList(this.User.Identity.GetUserId());
            model.ItemCategoryList = QueryViewModel.getItemCategoryList(this.User.Identity.GetUserId());
            model.locatorList = QueryViewModel.getLocatorList(this.User.Identity.GetUserId(), "");
            model.Fields = new StockQueryModel();
            return View(model);
        }

        /// <summary>
        /// 庫存查詢明細
        /// </summary>
        /// <param name="subinventoryCode"></param>
        /// <param name="locatorId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        // GET: /Stock/Detail
        public ActionResult Detail(string subinventoryCode, long locatorId, long itemId)
        {
            var model = QueryDetailViewModel.getModel(subinventoryCode, locatorId, itemId);

            return View(model);
        }

        /// <summary>
        /// 庫存交易記錄查詢
        /// </summary>
        /// <returns></returns>
        // GET: /Stock/Transaction
        public ActionResult Transaction()
        {
            var model = new StockTxnQueryViewModel();
            model.SubinvenotoryList = StockTxnQueryViewModel.getSubinvenotoryList(this.User.Identity.GetUserId());
            model.ItemCategoryList = StockTxnQueryViewModel.getItemCategoryList(this.User.Identity.GetUserId());
            model.locatorList = StockTxnQueryViewModel.getLocatorList(this.User.Identity.GetUserId(), "");
            model.ReasonList = StockTxnQueryViewModel.getStkTxnReasonList(this.User.Identity.GetUserId());
            model.Fields = new StockTxnQueryModel();
            return View(model);
        }


        /// <summary>
        /// 庫存總量查詢
        /// </summary>
        /// <param name="data">DataTableAjaxPostViewModel</param>
        /// <param name="subinventory">倉庫代號</param>
        /// <param name="locatorId">儲位ID</param>
        /// <param name="itemCategory">平版/紙捲</param>
        /// <param name="itemNo">料號</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockQuery(DataTableAjaxPostViewModel data, 
            string subinventory, string locatorId, string itemCategory, string itemNo)
        {

            var userId = User.Identity.GetUserId();
            var models = StockQueryModel.getModels(data, subinventory, locatorId, itemCategory, itemNo, userId);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 庫存明細查詢
        /// </summary>
        /// <param name="data">DataTableAjaxPostViewModel</param>
        /// <param name="subinventory">倉庫代號</param>
        /// <param name="locatorId">儲位ID</param>
        /// <param name="itemId">料號ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockDetailQuery(DataTableAjaxPostViewModel data,
            string subinventory, long locatorId, long itemId)
        {

            var userId = User.Identity.GetUserId();
            var models = StockDetailQueryModel.getModels(data, subinventory, locatorId, itemId, userId);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 庫存異動記錄查詢
        /// </summary>
        /// <param name="data">DataTableAjaxPostViewModel</param>
        /// <param name="subinventory">倉庫代號</param>
        /// <param name="locatorId">儲位ID</param>
        /// <param name="itemCategory">平版/紙捲</param>
        /// <param name="itemNo">料號</param>
        /// <param name="barcode">條碼</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockTxnQuery(DataTableAjaxPostViewModel data,
            string subinventory, string locatorId, string itemCategory, string itemNo, string barcode,string dateFrom,string dateTo, string reason)
        {

            var userId = User.Identity.GetUserId();
            var models = StockTxnQueryModel.getModels(data, subinventory, locatorId, itemCategory, itemNo, barcode, dateFrom, dateTo, reason, userId);

            return Json(models, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 取得儲位清單
        /// </summary>
        /// <param name="subinventory">倉庫代號</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetLocators(string subinventory)
        {

            var items = QueryViewModel.getLocatorList(this.User.Identity.GetUserId(), subinventory);

            return this.Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得料號清單
        /// </summary>
        /// <param name="itemNo">料號前置</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetItemNumbers(string itemNo)
        {

            var items = QueryViewModel.getItemNumbers(itemNo);

            return this.Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 列印標籤
        /// </summary>
        /// <param name="StockId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintLabel(List<long> StockId)
        {
            using var context = new DataModel.MesContext();
            using var uow = new MasterUOW(context);
            var label = uow.GetStockLabels(StockId, User.Identity.GetUserId());

            return uow.PrintLabel(label.Data);
        }
    }
}