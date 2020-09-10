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
        public JsonResult StockQuery(DataTableAjaxPostViewModel data, 
            string subinventory, string locatorId, string itemCategory, string itemNo)
        {

            var models = StockQueryModel.getModels(data, subinventory, locatorId, itemCategory, itemNo);

            return Json(new { draw = data.Draw, recordsFiltered = models.Count, recordsTotal = models.Count, data = models }, JsonRequestBehavior.AllowGet);
        }
	}
}