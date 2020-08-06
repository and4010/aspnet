using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class MachinePaperTypeController : Controller
    {
        // GET: MachinePaperType
        public ActionResult Index()
        {
            MachinePaperTypeViewModel model = new MachinePaperTypeViewModel();
            ViewBag.Organization_code = model.GetOrganization_code();
            return View(model);
        }



        [HttpPost, ActionName("MachinePaperType")]
        public JsonResult MachinePaperType(DataTableAjaxPostViewModel data,string Organization_code)
        {
            MachinePaperTypeViewModel machinePaperTypeViewModel = new MachinePaperTypeViewModel();


            List<MachinePaperType> model = machinePaperTypeViewModel.GetMachinePaperTypes(Organization_code);


            model = MachinePaperTypeViewModel.Search(data, model);
            model = MachinePaperTypeViewModel.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
        }


    }
}