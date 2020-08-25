using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.Process;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Process;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class ProcessController : Controller
    {
        //
        // GET: /Process/
        public ActionResult Index()
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            ViewBag.Process_Status = viewModel.GetBatchStatusDesc();
            ViewBag.Process_Batch_no = viewModel.GetBatchNo();
            ViewBag.Manchine_Num = viewModel.GetManchine();
            ViewBag.Subinventory = viewModel.GetSubinventory();
            return View();
        }

        public ActionResult Schedule(long id)
        {
            //取得使用者角色
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            ViewBag.RemnantItem = procesViewModel.GetRemnantItem();
            ViewBag.CotangentItem = procesViewModel.GetCotangentItem();
            procesViewModel.CHP_PROCESS_T = procesViewModel.GetViewModel(id);
            procesViewModel.YieldVariance = procesViewModel.GetRate(id);
            procesViewModel.Authority = procesViewModel.GetAuthority(roles);
            return View(procesViewModel);
        }

        public ActionResult Edit(long id)
        {
            ProcessViewModel procesViewModel = new ProcessViewModel();
            procesViewModel.CHP_PROCESS_T = procesViewModel.GetViewModel(id);
            procesViewModel.YieldVariance = procesViewModel.GetRate(id);
            return View(procesViewModel);
        }

        public JsonResult EditSave(long OspHeaderId, string Note)
        {
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SetEditNote(OspHeaderId, Note);
            return Json(new { resultModel },JsonRequestBehavior.AllowGet);
        }

        public ActionResult Flat(long id)
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            procesViewModel.CHP_PROCESS_T = procesViewModel.GetViewModel(id);
            procesViewModel.YieldVariance = procesViewModel.GetRate(id);
            procesViewModel.Authority = procesViewModel.GetAuthority(roles);
            return View(procesViewModel);
        }

        public ActionResult PaperRoll(long id)
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            procesViewModel.CHP_PROCESS_T = procesViewModel.GetViewModel(id);
            procesViewModel.YieldVariance = procesViewModel.GetRate(id);
            procesViewModel.Authority = procesViewModel.GetAuthority(roles);
            return View(procesViewModel);
        }


        [HttpPost]
        public ActionResult _ProcessIndex()
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            ViewBag.MachineItems = viewModel.GetManchine();
            return PartialView();
        }

        [HttpPost]
        public ActionResult _Locator(long OspDetailOutId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            ViewBag.LocatorItem = viewModel.GetLocator(OspDetailOutId);
            return PartialView();
        }

        /// <summary>
        /// 存檔入庫用
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Locator"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeHeaderStauts(long OspHeaderId, long Locator)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.ChangeHeaderStauts(OspHeaderId, Locator, Userid, name);
            
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 完工紀錄編輯更改狀態
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Locator"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditHeaderStauts(long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.EditHeaderStauts(OspHeaderId, Userid, name);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "系統管理員, 華紙使用者")]
        [HttpPost]
        public ActionResult ApproveHeaderStauts(long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.ChangeHeaderStauts(OspHeaderId, 0, Userid, name);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _BtnDailogChangStatusCutDate(long OspHeaderId, DateTime Dialog_CuttingDateFrom, DateTime Dialog_CuttingDateTo, string Dialog_MachineNum, string BtnStatus)
        {
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SetStatusAndCutDate(OspHeaderId, Dialog_CuttingDateFrom, Dialog_CuttingDateTo, Dialog_MachineNum, BtnStatus);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TableResult(DataTableAjaxPostViewModel data, string Status, string BatchNo, 
            string MachineNum, string DueDate, string CuttingDateFrom, string CuttingDateTo, string Subinventory)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            List<CHP_PROCESS_T> model = viewModel.Search(Status, BatchNo, MachineNum, DueDate, CuttingDateFrom, CuttingDateTo, Subinventory);
            model = ProcessViewModel.ChpProcessModelDTOrder.Search(data, model);
            model = ProcessViewModel.ChpProcessModelDTOrder.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckBatchNo(string BatchNo,long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel  = viewModel.CheckBatchNo(BatchNo, OspHeaderId);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvestLoadTable(DataTableAjaxPostViewModel data, long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            List<Invest> model = viewModel.GetPicketIn(OspHeaderId);
            model = ProcessViewModel.InvestModelDTOrder.Search(data, model);
            model = ProcessViewModel.InvestModelDTOrder.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckStockBarcode(string Barcode, string OspDetailInId)
        {

            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultDataModel = procesViewModel.CheckStockBarcode(Barcode, OspDetailInId);
            return Json(new { resultDataModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveInvestBarcode(string Barcode, string Remnant, string Remaining_Weight, long OspDetailInId)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SavePickIn(Barcode, Remnant, Remaining_Weight, OspDetailInId, Userid, name);

    
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvestEdit(ProcessUOW.DetailDTEditor InvestDTList)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SetEditor(InvestDTList, Userid, name);
          
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionLoadDataTables(DataTableAjaxPostViewModel data, long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            List<Production> model = viewModel.GetPicketOut(OspHeaderId);
            model = ProcessViewModel.ProductionModelDTOrder.Search(data, model);
            model = ProcessViewModel.ProductionModelDTOrder.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 儲存產出條碼
        /// </summary>
        /// <param name="Production_Roll_Ream_Qty"></param>
        /// <param name="Production_Roll_Ream_Wt"></param>
        /// <param name="Cotangent"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateProduction(string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var UserName = this.User.Identity.GetUserName();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.CreateProduction(Userid, UserName,Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId);
            
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加工Editor
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProductionEdit(ProcessUOW.ProductionDTEditor ProductionDTEditor)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel =  viewModel.SetProductionEditor(ProductionDTEditor, id, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 代紙紙捲Editor
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaperRollerProductionEdit(ProcessUOW.ProductionDTEditor ProductionDTEditor)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.SetPaperRollerProductionEditor(ProductionDTEditor, id, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionChangeStatus(string Production_Barcode, long OspDetailOutId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.ProductionChangeStatus(Production_Barcode, OspDetailOutId, id, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CotangentDataTables(DataTableAjaxPostViewModel data, long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            List<Cotangent> model = viewModel.GetCotangents(OspHeaderId);
            model = ProcessViewModel.CotangentModelDTOrder.Search(data, model);
            model = ProcessViewModel.CotangentModelDTOrder.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CotangentEdit(ProcessUOW.CotangentDTEditor cotangentDTEditor)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.SetCotangentEditor(cotangentDTEditor, id, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult CotangentChangeStatus(string CotangentBarcode,long OspDetailOutId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.CotangentChangeStatus(CotangentBarcode, OspDetailOutId, id, name);
           
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RecordCotangentDataTables(DataTableAjaxPostViewModel data)
        {
            List<Cotangent> model = new List<Cotangent>();
            //model = ProcessViewModel.ListCotangent;
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Loss(long OspDetailInId ,long OspDetailOutId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultDataModel = viewModel.Loss(OspDetailInId, OspDetailOutId, id, name);
            return Json(new { resultDataModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 完工紀錄編輯
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FinisheEdit(string BatchNo, long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel = viewModel.FinisheEdit(BatchNo, OspHeaderId);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 代紙紙捲產出明細
        /// </summary>
        /// <param name="PaperRoll_Basic_Weight"></param>
        /// <param name="PaperRoll_Specification"></param>
        /// <param name="PaperRoll_Lot_Number"></param>
        /// <param name="Process_Detail_Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaperRollCreateProduction(string PaperRoll_Basic_Weight, string PaperRoll_Specification, string PaperRoll_Lot_Number, long OspDetailOutId)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var UserName = this.User.Identity.GetUserName();
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel = viewModel.PaperRollCreateProduction(PaperRoll_Basic_Weight, PaperRoll_Specification, PaperRoll_Lot_Number, OspDetailOutId, Userid, UserName);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GeProductLabels(List<long> OspPickedOutId,List<long> OspCotangentId)
        {
            //取得使用者帳號
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var name = this.User.Identity.GetUserName();
            return procesViewModel.GeProductLabels(OspPickedOutId, name, OspCotangentId);
        }

        /// <summary>
        /// 列印代紙紙捲條碼
        /// </summary>
        /// <param name="OspPickedOutId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GePaperRollerProductLabels(List<long> OspPickedOutId)
        {
            //取得使用者帳號
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var name = this.User.Identity.GetUserName();
            return procesViewModel.GePaperRollerProductLabels(OspPickedOutId, name);
        }


        [HttpPost]
        public ActionResult RePrintLabel(List<long> StockId, string Status)
        {
            //取得使用者帳號
            ProcessViewModel viewModel = new ProcessViewModel();
            var name = this.User.Identity.GetUserName();
            return viewModel.RePrintLabel(StockId, name, Status);
        }






    }
}