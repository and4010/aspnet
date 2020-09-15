using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.StockQuery;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.StockQuery;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        //
        // GET: /Stock/
        public ActionResult Index()
        {
            return View();
        }

        //
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

        //
        // GET: /Stock/Detail
        public ActionResult Detail(string subinventoryCode, long locatorId, long itemId)
        {
            var model = QueryDetailViewModel.getModel(subinventoryCode, locatorId, itemId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockQuery(DataTableAjaxPostViewModel data, 
            string subinventory, string locatorId, string itemCategory, string itemNo)
        {

            var userId = User.Identity.GetUserId();
            var models = StockQueryModel.getModels(data, subinventory, locatorId, itemCategory, itemNo, userId);

            return Json(new { draw = data.Draw, recordsFiltered = models.Count, recordsTotal = models.Count, data = models }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockDetailQuery(DataTableAjaxPostViewModel data,
            string subinventory, long locatorId, long itemId)
        {

            var userId = User.Identity.GetUserId();
            var models = StockDetailQueryModel.getModels(data, subinventory, locatorId, itemId, userId);

            return Json(new { draw = data.Draw, recordsFiltered = models.Count, recordsTotal = models.Count, data = models }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetLocators(string subinventory)
        {

            var items = QueryViewModel.getLocatorList(this.User.Identity.GetUserId(), subinventory);

            return this.Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetItemNumbers(string itemNo)
        {

            var items = QueryViewModel.getItemNumbers(itemNo);

            return this.Json(items, JsonRequestBehavior.AllowGet);
        }

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