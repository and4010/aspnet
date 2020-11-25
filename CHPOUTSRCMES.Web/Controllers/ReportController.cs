using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.Report;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Process;
using CHPOUTSRCMES.Web.ViewModels.Report;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class ReportController : Controller
    {
        YieldQueryModel yieldQueryModel = new YieldQueryModel();
        // GET: Report
        public ActionResult Yield()
        {
            YieldViewModel yieldViewModel = new YieldViewModel();
            ProcessViewModel processViewModel = new ProcessViewModel();
            //yieldViewModel.BathNoList = processViewModel.GetBatchNo();
            yieldViewModel.MachineCodeList = processViewModel.GetManchine();
            return View(yieldViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult YieldQuery(DataTableAjaxPostViewModel data, string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum)
        {
            var userId = User.Identity.GetUserId();
            var models = yieldQueryModel.getModels(data, cuttingDateFrom, cuttingDateTo, batchNo, machineNum, userId);
            return Json(models, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OspYieldReport(string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    var userId = User.Identity.GetUserId();
#if DEBUG
                    var result = yieldQueryModel.LocalOspYieldReportViewer(uow, cuttingDateFrom, cuttingDateTo, batchNo, machineNum, userId);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return PartialView ("_ReportPartial");
    }
                    else
                    {
                        throw new Exception(result.Msg);
    //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
}

#else

                    var result = yieldQueryModel.RemoteOspYieldReportViewer(uow, cuttingDateFrom, cuttingDateTo, batchNo, machineNum, userId);
                    if (result.Success)
                    {
                        ViewBag.ReportViewer = result.Data;
                        return View("Report");
                    }
                    else
                    {
                        throw new Exception(result.Msg);
                        //return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                    }

#endif
                }
            }
        }
    }
}