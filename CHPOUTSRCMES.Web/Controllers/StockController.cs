using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CHPOUTSRCMES.Web.ViewModels;
using Microsoft.AspNet.Identity;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockQuery(DataTableAjaxPostViewModel data, 
            string subinventory, string locatorId, string itemCategory, string itemNo)
        {

            var userId = this.User.Identity.GetUserId();
            var models = StockQueryModel.getModels(data, subinventory, locatorId, itemCategory, itemNo, userId);

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
    }
}