using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class OspRelatedController : Controller
    {
        OspRelatedData ospRelatedData = new OspRelatedData();
        //
        // GET: /OspRelated/
        public ActionResult Index()
        {
            var id = this.User.Identity.GetUserId();
            OspRelatedViewModel viewModel = ospRelatedData.getViewModel(id);
            return View(viewModel);
        }

        [HttpPost, ActionName("GetOspRelatedDT")]
        public JsonResult GetOspRelatedDT(DataTableAjaxPostViewModel data, string ORGANIZATION_ID, string INVENTORY_ITEM_ID, string RELATED_ITEM_ID)
        {
            List<OspRelatedDT> model = ospRelatedData.search(ORGANIZATION_ID, INVENTORY_ITEM_ID, RELATED_ITEM_ID);
            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.ITEM_NUMBER) && p.ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_DESCRIPTION) && p.ITEM_DESCRIPTION.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.RELATED_ITEM_NUMBER) && p.RELATED_ITEM_NUMBER.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.RELATED_ITEM_DESCRIPTION) && p.RELATED_ITEM_DESCRIPTION.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.CREATED_BY_NAME) && p.CREATED_BY_NAME.ToLower().Contains(search.ToLower()))
                    || (p.CREATION_DATE.HasValue && p.CREATION_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LAST_UPDATED_BY_NAME) && p.LAST_UPDATED_BY_NAME.ToLower().Contains(search.ToLower()))
                    || (p.LAST_UPDATE_DATE.HasValue && p.LAST_UPDATE_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = OspRelatedDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost, ActionName("Search")]
        //public JsonResult Search(string INVENTORY_ITEM_ID, string RELATED_ITEM_ID)
        //{
        //    ResultModel result = ospRelatedData.search(INVENTORY_ITEM_ID, RELATED_ITEM_ID);
        //    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        //}

        [HttpPost, ActionName("GetInventoryItemList")]
        public JsonResult GetInventoryItemList(string ORGANIZATION_ID)
        {
            List<SelectListItem> items = ospRelatedData.GetInventoryItemList(ORGANIZATION_ID).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetRelatedItemList")]
        public JsonResult GetRelatedItemList(string ORGANIZATION_ID, string INVENTORY_ITEM_ID)
        {
            List<SelectListItem> items = ospRelatedData.GetRelatedItemList(ORGANIZATION_ID, INVENTORY_ITEM_ID).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
	}
}