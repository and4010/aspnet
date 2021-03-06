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
        StockObsoleteData obsoleteData = new StockObsoleteData();
        OrgSubinventoryData orgData = new OrgSubinventoryData();
        
        /// <summary>
        /// 存貨報廢 View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (var context = new MesContext())
            {
                using (ObsoleteUOW uow = new ObsoleteUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    ObsoleteViewModel viewModel = obsoleteData.GetObsoleteViewModel(uow, orgData, id);
                    return View(viewModel);
                }
            }
                   
        }


        /// <summary>
        /// 庫存查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="organizationId"></param>
        /// <param name="subinventoryCode"></param>
        /// <param name="locatorId"></param>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
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
                        model = model.Where(p => (p.SUB_ID.ToString().ToLower().Contains(search.ToLower()))
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

        /// <summary>
        /// 異動明細表格
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GetTransactionDetail")]
        public JsonResult GetTransactionDetail(DataTableAjaxPostViewModel data)
        {
            using (var context = new MesContext())
            {
                using (ObsoleteUOW uow = new ObsoleteUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<StockObsoleteDT> model = obsoleteData.GetObsoleteData(uow, id);

                    var totalCount = model.Count;
                    string search = data.Search.Value;

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        // Apply search
                        model = model.Where(p => (p.SUB_ID.ToString().ToLower().Contains(search.ToLower()))
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
            }
        }

        /// <summary>
        /// 異動明細表格資料編輯
        /// </summary>
        /// <param name="detailEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DetailEditor(StockObsoleteDTEditor detailEditor)
        {
            using (var context = new MesContext())
            {
                using (ObsoleteUOW uow = new ObsoleteUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = obsoleteData.DetailEditor(uow, detailEditor, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 新增異動明細
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="mQty"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTransactionDetail( long stockId, string mQty)
        {
            using (var context = new MesContext())
            {
                using (ObsoleteUOW uow = new ObsoleteUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = obsoleteData.CreateDetail(uow, stockId, mQty, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        /// <summary>
        /// 異動存檔
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveTransactionDetail()
        {
            using (var context = new MesContext())
            {
                using (ObsoleteUOW uow = new ObsoleteUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = obsoleteData.SaveTransactionDetail(uow, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }


	}
}