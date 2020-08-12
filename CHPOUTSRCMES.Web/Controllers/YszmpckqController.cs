using CHPOUTSRCMES.Web.Models;
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
    public class YszmpckqController : Controller
    {
        YszmpckqData yszmpckqDataData = new YszmpckqData();
         

        //
        // GET: /Yszmpckq/
        public ActionResult Index()
        {
            var id = this.User.Identity.GetUserId();
            YszmpckqViewModel viewModel = yszmpckqDataData.getViewModel(id);
            return View(viewModel);
        }

        [HttpPost, ActionName("GetYszmpckqDT")]
        public JsonResult GetYszmpckqDT(DataTableAjaxPostViewModel data, string ORGANIZATION_ID, string OSP_SUBINVENTORY, string PSTYP)
        {
            //List<YszmpckqDT> model = yszmpckqDataData.GetYszmpckqDT();
            List<YszmpckqDT> model = yszmpckqDataData.search(ORGANIZATION_ID, OSP_SUBINVENTORY, PSTYP);
            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.ORGANIZATION_CODE) && p.ORGANIZATION_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ORGANIZATION_NAME) && p.ORGANIZATION_NAME.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.OSP_SUBINVENTORY) && p.OSP_SUBINVENTORY.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.OSP_SUBINVENTORY_NAME) && p.OSP_SUBINVENTORY_NAME.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PSTYP) && p.PSTYP.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PSTYP_CHT_NAME) && p.PSTYP_CHT_NAME.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PSTYP_ENG_NAME) && p.PSTYP_ENG_NAME.ToLower().Contains(search.ToLower()))
                    || ((p.BWETUP != null) && p.BWETUP.ToString().ToLower().Contains(search.ToLower()))
                    || ((p.BWETDN != null) && p.BWETDN.ToString().ToLower().Contains(search.ToLower()))
                    || ((p.RWTUP != null) && p.RWTUP.ToString().ToLower().Contains(search.ToLower()))
                    || ((p.RWTDN != null) && p.RWTDN.ToString().ToLower().Contains(search.ToLower()))
                    || ((p.PCKQ != null) && p.PCKQ.ToString().ToLower().Contains(search.ToLower()))
                    || ((p.PAPER_QTY != null) && p.PAPER_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || ((p.PIECES_QTY != null) && p.PIECES_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.CREATED_BY_NAME) && p.CREATED_BY_NAME.ToLower().Contains(search.ToLower()))
                    || (p.CREATION_DATE.HasValue && p.CREATION_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LAST_UPDATED_BY_NAME) && p.LAST_UPDATED_BY_NAME.ToLower().Contains(search.ToLower()))
                    || (p.LAST_UPDATE_DATE.HasValue && p.LAST_UPDATE_DATE.Value.ToString("yyyy-MM-dd").ToLower().Contains(search.ToLower()))

                    ).ToList();
            }

            var filteredCount = model.Count;
            model = YszmpckqDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost, ActionName("ClearYszmpckqDT")]
        //public JsonResult ClearYszmpckqDT()
        //{
        //    ResultModel result = yszmpckqDataData.ClearYszmpckqDT();
        //    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        //}

        //[HttpPost, ActionName("Search")]
        //public JsonResult Search(string ORGANIZATION_ID, string OSP_SUBINVENTORY, string PSTYP)
        //{
        //    ResultModel result = yszmpckqDataData.search(ORGANIZATION_ID, OSP_SUBINVENTORY, PSTYP);
        //    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        //}

        [HttpPost, ActionName("GetOspSubinventoryList")]
        public JsonResult GetOspSubinventoryList(string ORGANIZATION_ID)
        {
            List<SelectListItem> items = yszmpckqDataData.GetOspSubinventoryList(ORGANIZATION_ID).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("GetPstypList")]
        public JsonResult GetPstypList(string ORGANIZATION_ID, string OSP_SUBINVENTORY_ID)
        {
            List<SelectListItem> items = yszmpckqDataData.GetPstypList(ORGANIZATION_ID ,OSP_SUBINVENTORY_ID).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
	}
}