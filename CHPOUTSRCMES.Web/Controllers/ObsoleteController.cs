using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Obsolete;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class ObsoleteController : Controller
    {
        Top top = new Top();
        StockObsoleteData obsoleteData = new StockObsoleteData();
        OrgSubinventoryData orgData = new OrgSubinventoryData();
        //
        // GET: /Obsolete/
        public ActionResult Index()
        {
            StockData.addDefault();
            ObsoleteViewModel viewModel = obsoleteData.GetObsoleteViewModel();
            return View(viewModel);
        }

        public PartialViewResult GetTop()
        {
            using (var context = new MesContext())
            {
                using (MasterUOW uow = new MasterUOW(context))
                {
                    //取得使用者帳號
                    //var name = this.User.Identity.GetUserName();
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    return PartialView("_TopPartial", top.GetViewModel(uow, orgData, id));
                }
            }
        }

        [HttpPost, ActionName("SearchStock")]
        public JsonResult SearchStock(DataTableAjaxPostViewModel data, long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            using (var context = new MesContext())
            {
                using (ObsoleteUOW uow = new ObsoleteUOW(context))
                {

                    List<StockDT> model = obsoleteData.SearchStock(uow, organizationId, subinventoryCode, locatorId, itemNumber);

                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search   
                        model = model.Where(p => (p.ID.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.SEGMENT3) && p.SEGMENT3.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.ITEM_NO) && p.ITEM_NO.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                            || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                            || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                            || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                            ).ToList();
                    }

                    var filteredCount = model.Count;
                    model = StockDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();
                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost, ActionName("GetTransactionDetail")]
        public JsonResult GetTransactionDetail(DataTableAjaxPostViewModel data)
        {
            List<StockObsoleteDT> model = obsoleteData.GetModel();

            var totalCount = model.Count;
            string search = data.Search.Value;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search
                model = model.Where(p => (p.ID.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SUBINVENTORY_CODE) && p.SUBINVENTORY_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SEGMENT3) && p.SEGMENT3.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ITEM_NO) && p.ITEM_NO.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BARCODE) && p.BARCODE.ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.PRIMARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PRIMARY_UOM_CODE) && p.PRIMARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_TRANSACTION_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (p.SECONDARY_AVAILABLE_QTY.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SECONDARY_UOM_CODE) && p.SECONDARY_UOM_CODE.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.NOTE) && p.NOTE.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }

            var filteredCount = model.Count;
            model = StockObsoleteDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateRemark(StockObsoleteDTEditor selectedData)
        {
            List<StockObsoleteDT> data = obsoleteData.UpdateRemark(selectedData);
            return new JsonResult { Data = new { data } };
        }

        [HttpPost]
        public ActionResult AddTransactionDetail(long ID, decimal ObsoleteQty)
        {
            ResultModel result = obsoleteData.AddTransactionDetail(ID, ObsoleteQty);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult DelTransactionDetail(List<long> IDs)
        {
            ResultModel result = obsoleteData.DelTransactionDetail(IDs);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

        [HttpPost]
        public ActionResult SaveTransactionDetail()
        {
            ResultModel result = obsoleteData.SaveTransactionDetail();
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }


	}
}