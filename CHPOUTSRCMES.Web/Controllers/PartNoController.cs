﻿using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class PartNoController : Controller
    {


        // GET: PartNo
        public ActionResult Index()
        {
            PartNoViewModel model = new PartNoViewModel();
            ViewBag.Catalog_elem_val_050 = model.GetSpec();
            ViewBag.Catalog_elem_val_070 = model.Get070();
            ViewBag.Catalog_elem_val_020 = model.GetTypePaper();
            ViewBag.Organization_code = model.GetOrganization_code();
            return View();
        }



        [HttpPost, ActionName("PartNoJson")]
        public JsonResult PartNoJson(DataTableAjaxPostViewModel data,string Catalog_elem_val_050, string Catalog_elem_val_020,string Catalog_elem_val_070,string Organization_code)
        {
            PartNoViewModel partNoViewModel = new PartNoViewModel();
            List<PartNoModel> model = partNoViewModel.GetItemNo(Catalog_elem_val_050, Catalog_elem_val_020, Catalog_elem_val_070, Organization_code); 
            model = PartNoViewModel.Search(data, model);
            model = PartNoViewModel.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
        }
    }
}