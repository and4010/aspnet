using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
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
        /// <summary>
        /// 加工首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            ProcessViewModel viewModel = new ProcessViewModel();
            ViewBag.Process_Status = viewModel.GetBatchStatusDesc();
            ViewBag.Process_Batch_no = viewModel.GetBatchNo();
            ViewBag.Manchine_Num = viewModel.GetManchine();
            ViewBag.Subinventory = viewModel.GetSubinventory(id,"Y");
            return View();
        }

        /// <summary>
        /// 加工-平張頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 加工-編輯備註頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long id)
        {
            ProcessViewModel procesViewModel = new ProcessViewModel();
            procesViewModel.CHP_PROCESS_T = procesViewModel.GetViewModel(id);
            procesViewModel.YieldVariance = procesViewModel.GetRate(id);
            return View(procesViewModel);
        }

        /// <summary>
        /// 加工-編輯備註儲存
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        public JsonResult EditSave(long OspHeaderId, string Note)
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SetEditNote(OspHeaderId, Note, id);
            return Json(new { resultModel },JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 代紙-平張頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 代紙-紙捲頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 加工頁面裁切日期dialog
        /// </summary>
        /// <param name="PaperType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ProcessIndex(string PaperType)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            ViewBag.MachineItems = viewModel.GetSelectMachine(PaperType);
            return PartialView();
        }

        /// <summary>
        /// 儲位dialog
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _Locator(long OspDetailOutId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            ViewBag.LocatorItem = viewModel.GetLocator(OspDetailOutId, Userid);
            return PartialView();
        }

        /// <summary>
        /// 存檔入庫用
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Locator"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeHeaderStatus(long OspHeaderId, string Locator)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.ChangeHeaderStatus(OspHeaderId, Locator, Userid, name);
            
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 完工紀錄-允許按鈕
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Locator"></param>
        /// <returns></returns>
        [Authorize(Roles = "系統管理員, 華紙使用者")]
        [HttpPost]
        public ActionResult ApproveHeaderStauts(long OspHeaderId, string Locator)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.ChangeHeaderStatus(OspHeaderId, Locator, Userid, name);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更改裁切日期按鈕
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Dialog_CuttingDateFrom"></param>
        /// <param name="Dialog_CuttingDateTo"></param>
        /// <param name="Dialog_MachineNum"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _BtnDailogChangStatusCutDate(long OspHeaderId, DateTime Dialog_CuttingDateFrom, DateTime Dialog_CuttingDateTo, string Dialog_MachineNum, string BtnStatus)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SetStatusAndCutDate(OspHeaderId, Dialog_CuttingDateFrom, Dialog_CuttingDateTo, Dialog_MachineNum, BtnStatus, Userid);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 關帳按鈕
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public ActionResult SetClose(long OspHeaderId, string BtnStatus)
        {
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.SetClose(OspHeaderId, BtnStatus);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加工表格單查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Status"></param>
        /// <param name="BatchNo"></param>
        /// <param name="MachineNum"></param>
        /// <param name="DueDateFrom"></param>
        /// <param name="DueDateTo"></param>
        /// <param name="CuttingDateFrom"></param>
        /// <param name="CuttingDateTo"></param>
        /// <param name="PlanStartDateFrom"></param>
        /// <param name="PlanStartDateTo"></param>
        /// <param name="Subinventory"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TableResult(DataTableAjaxPostViewModel data, string Status, string BatchNo, 
            string MachineNum, string DueDateFrom, string DueDateTo, string CuttingDateFrom, string CuttingDateTo, string PlanStartDateFrom, string PlanStartDateTo, string Subinventory)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            var Userid = this.User.Identity.GetUserId();
            List<CHP_PROCESS_T> model = viewModel.Search(Status, BatchNo, MachineNum, DueDateFrom, DueDateTo, CuttingDateFrom, CuttingDateTo, Subinventory, PlanStartDateFrom, PlanStartDateTo, Userid);
            model = ProcessViewModel.ChpProcessModelDTOrder.Search(data, model);
            model = ProcessViewModel.ChpProcessModelDTOrder.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 檢查批號
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckBatchNo(string BatchNo,long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel  = viewModel.CheckBatchNo(BatchNo, OspHeaderId);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 投入表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 檢查庫存條碼
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckStockBarcode(string Barcode, long OspDetailInId)
        {

            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultDataModel = procesViewModel.CheckStockBarcode(Barcode, OspDetailInId);
            return Json(new { resultDataModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 入庫儲存條碼
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="Remnant"></param>
        /// <param name="Remaining_Weight"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 投入條碼編輯修改Editor
        /// </summary>
        /// <param name="InvestDTList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 刪除全部條碼
        /// </summary>
        /// <param name="OspPickedInId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AllDelete(long[] OspPickedInId)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.AllDelete(OspPickedInId, Userid);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 產出表單資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
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
        public JsonResult CreateProduction(string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId, long OspDetailInId, long OspHeaderId)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var UserName = this.User.Identity.GetUserName();
            ProcessViewModel procesViewModel = new ProcessViewModel();
            var resultModel = procesViewModel.CreateProduction(Userid, UserName,Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId, OspDetailInId, OspHeaderId);
            
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加工產出編輯Editor
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
        /// 加工產出選取刪除
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProductionChooseDelete(long[] OspPickedOutId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = viewModel.ProductionChooseDelete(OspPickedOutId);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加工餘切選擇刪除
        /// </summary>
        /// <param name="OspCotangentId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CotangentChooseDelete(long[] OspCotangentId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel = viewModel.CotangentChooseDelete(OspCotangentId);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 代紙紙捲編輯Editor
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

        /// <summary>
        /// 產出條碼入庫
        /// </summary>
        /// <param name="Production_Barcode"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得餘切表單資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 餘切編輯Editor
        /// </summary>
        /// <param name="cotangentDTEditor"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 儲存餘切條碼資料
        /// </summary>
        /// <param name="CotangentBarcode"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 刪除得率計算
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRate(long OspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            viewModel.DeleteRate(OspHeaderId);
            return Json(new {  }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 計算損耗
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
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
        public JsonResult FinishedEdit(string BatchNo, long OspHeaderId)
        {
            var userId = this.User.Identity.GetUserId();
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel = viewModel.FinishedEdit(BatchNo, OspHeaderId, userId);

            return Json(resultModel, JsonRequestBehavior.AllowGet);
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
        public JsonResult PaperRollCreateProduction(string PaperRoll_Weight, string PaperRoll_Lot_Number, long OspDetailOutId)
        {
            //取得使用者ID
            var Userid = this.User.Identity.GetUserId();
            //取得使用者帳號
            var UserName = this.User.Identity.GetUserName();
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel = viewModel.PaperRollCreateProduction(PaperRoll_Weight, PaperRoll_Lot_Number, OspDetailOutId, Userid, UserName);

            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 列印標籤成品平版
        /// </summary>
        /// <param name="OspPickedOutId"></param>
        /// <param name="OspCotangentId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 補印標籤
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RePrintLabel(List<long> StockId, string Status)
        {
            //取得使用者帳號
            ProcessViewModel viewModel = new ProcessViewModel();
            var name = this.User.Identity.GetUserName();
            return viewModel.RePrintLabel(StockId, name, Status);
        }

        /// <summary>
        /// 又裁又代 須檢查來源裁切工單是否完工
        /// </summary>
        /// <param name="SrcOspHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckInsteadPaperOrderProcess(long SrcOspHeaderId)
        {
            ProcessViewModel viewModel = new ProcessViewModel();
            var resultModel = viewModel.CheckInsteadPaperOrderProcess(SrcOspHeaderId);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

    }
}