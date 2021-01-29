using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Miscellaneous;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class MiscellaneousController : Controller
    {
        //Top top = new Top();
        StockMiscellaneousData miscellaneousData = new StockMiscellaneousData();
        OrgSubinventoryData orgData = new OrgSubinventoryData();
        //
        // GET: /Miscellaneous/
        public ActionResult Index()
        {
            using (var context = new MesContext())
            {
                using (MiscellaneousUOW uow = new MiscellaneousUOW(context))
                {
                    //StockData.addDefault();
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    MiscellaneousViewModel viewModel = miscellaneousData.GetMiscellaneousViewModel(uow, orgData, id);
                    return View(viewModel);
                }
            }
        }

        //public PartialViewResult GetTop()
        //{
        //    using (var context = new MesContext())
        //    {
        //        using (MasterUOW uow = new MasterUOW(context))
        //        {
        //            //取得使用者帳號
        //            //var name = this.User.Identity.GetUserName();
        //            //取得使用者ID
        //            var id = this.User.Identity.GetUserId();
        //            return PartialView("_TopPartial", top.GetViewModel(uow, orgData, id));
        //        }
        //    }
        //}

        //public PartialViewResult GetContent(string Miscellaneous)
        //{
        //    StockData.addDefault();
        //    if (Miscellaneous == "雜發")
        //    {
        //        return PartialView("_SendPartial", miscellaneousData.GetMiscellaneousSendViewModel());
        //    }
        //    else
        //    {
        //        return PartialView("_ReceivePartial", miscellaneousData.GetMiscellaneousReceiveViewModel());
        //    }
            
        //}

        [HttpPost, ActionName("GetStockItemData")]
        public JsonResult GetStockItemData(string SUBINVENTORY_CODE, string ITEM_NO)
        {
            List<StockDT> list = StockData.GetStockItemData(SUBINVENTORY_CODE, ITEM_NO);
            if (list.Count > 0 && list[0].ITEM_CATEGORY == "平版")
            {
                return new JsonResult { Data = new { STATUS = true, PACKING_TYPE = list[0].PACKING_TYPE, ITEM_CATEGORY = list[0].ITEM_CATEGORY, UNIT = list[0].PRIMARY_UOM_CODE } };
            }
            else if (list.Count > 0 && list[0].ITEM_CATEGORY == "捲筒")
            {
                return new JsonResult { Data = new { STATUS = true, PACKING_TYPE = list[0].PACKING_TYPE, ITEM_CATEGORY = list[0].ITEM_CATEGORY, UNIT = list[0].PRIMARY_UOM_CODE } };
            }
            else
            {
                return new JsonResult { Data = new { STATUS = false, PACKING_TYPE = "", ITEM_CATEGORY = "", UNIT = "" } };
            }
        }

        [HttpPost, ActionName("SearchStock")]
        //public JsonResult SearchStock(DataTableAjaxPostViewModel data, long organizationId, string subinventoryCode, long? locatorId, string itemNumber, decimal primaryQty, decimal percentageError)
        public JsonResult SearchStock(DataTableAjaxPostViewModel data, long organizationId, string subinventoryCode, long? locatorId, string itemNumber, decimal primaryQty, decimal percentageError)
        {
            using (var context = new MesContext())
            {
                using (MiscellaneousUOW uow = new MiscellaneousUOW(context))
                {
                    
                    List<StockDT> model = miscellaneousData.SearchStock(uow,  organizationId,  subinventoryCode,   locatorId,  itemNumber,  primaryQty,  percentageError);

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

        [HttpPost, ActionName("GetTransactionDetail")]
        public JsonResult GetTransactionDetail(DataTableAjaxPostViewModel data, long transactionTypeId)
        {

            using (var context = new MesContext())
            {
                using (MiscellaneousUOW uow = new MiscellaneousUOW(context))
                {
                    var id = this.User.Identity.GetUserId();
                    List<StockMiscellaneousDT> model = miscellaneousData.GetMiscellaneousData(uow, id, transactionTypeId);

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
                    model = StockMiscellaneousDTOrder.Order(data.Order, model).ToList();
                    model = model.Skip(data.Start).Take(data.Length).ToList();
                    return Json(new { draw = data.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = model }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult DetailEditor(StockMiscellaneousDTEditor detailEditor)
        {
            using (var context = new MesContext())
            {
                using (MiscellaneousUOW uow = new MiscellaneousUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    var result = miscellaneousData.DetailEditor(uow, detailEditor, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateRemark(StockMiscellaneousDTEditor selectedData)
        {
            List<StockMiscellaneousDT> data = miscellaneousData.UpdateRemark(selectedData);
            return new JsonResult { Data = new { data } };
        }

        [HttpPost]
        public ActionResult AddTransactionDetail(long transactionTypeId,
      long stockId, string mPrimaryQty, string note)
        {
            using (var context = new MesContext())
            {
                using (MiscellaneousUOW uow = new MiscellaneousUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result =  miscellaneousData.CreateDetail(uow, transactionTypeId, stockId, mPrimaryQty, note, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }

        [HttpPost]
        public ActionResult DelTransactionDetail(List<long> IDs)
        {
            ResultModel result = miscellaneousData.DelTransactionDetail(IDs);
            return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
        }

          [HttpPost]
        public ActionResult SaveTransactionDetail(long transactionTypeId)
        {
            using (var context = new MesContext())
            {
                using (MiscellaneousUOW uow = new MiscellaneousUOW(context))
                {
                    //取得使用者ID
                    var id = this.User.Identity.GetUserId();
                    //取得使用者帳號
                    var name = this.User.Identity.GetUserName();
                    ResultModel result = miscellaneousData.SaveTransactionDetail(uow, transactionTypeId, id, name);
                    return new JsonResult { Data = new { status = result.Success, result = result.Msg } };
                }
            }
        }
        
	}
}