using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class OrgSubinventoryController : Controller
    {
        OrgSubinventoryData orgSubinventoryData = new OrgSubinventoryData();
        //
        // GET: /OrgSubinventory/
        public ActionResult Index()
        {
            OrgSubinventoryViewModel viewModel = orgSubinventoryData.getViewModel();
            return View(viewModel);
        }

        [HttpPost, ActionName("GetOrgSubinventoryDT")]
        public JsonResult GetOrgSubinventoryDT(DataTableAjaxPostViewModel data, string ORGANIZATION_ID, string SUBINVENTORY_CODE, string LOCATOR_ID)
        {
            List<OrgSubinventoryDT> model = orgSubinventoryData.search(ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_ID);
            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.ORGANIZATION_CODE) && p.ORGANIZATION_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ORGANIZATION_NAME) && p.ORGANIZATION_NAME.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SUBINVENTORY_NAME) && p.SUBINVENTORY_NAME.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.OSP_FLAG) && p.OSP_FLAG.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LOCATOR_SEGMENTS) && p.LOCATOR_SEGMENTS.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LOCATOR_DESC) && p.LOCATOR_DESC.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.CREATED_BY_NAME) && p.CREATED_BY_NAME.ToLower().Contains(search.ToLower()))
                    || (p.CREATION_DATE.HasValue && p.CREATION_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LAST_UPDATED_BY_NAME) && p.LAST_UPDATED_BY_NAME.ToLower().Contains(search.ToLower()))
                    || (p.LAST_UPDATE_DATE.HasValue && p.LAST_UPDATE_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = OrgSubinventoryDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost, ActionName("Search")]
        //public JsonResult Search(string ORGANIZATION_ID, string SUBINVENTORY_CODE, string LOCATOR_ID)
        //{
        //    ResultModel result = orgSubinventoryData.search(ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_ID);
        //    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        //}

        [HttpPost, ActionName("GetSubinventoryList")]
        public JsonResult GetSubinventoryList(string ORGANIZATION_ID)
        {
            List<SelectListItem> items = orgSubinventoryData.GetSubinventoryList(ORGANIZATION_ID, true).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetLocatorList")]
        public JsonResult GetLocatorList(string ORGANIZATION_ID, string SUBINVENTORY_CODE)
        {
            List<SelectListItem> items = orgSubinventoryData.GetLocatorList(ORGANIZATION_ID, SUBINVENTORY_CODE, true).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
	}
}