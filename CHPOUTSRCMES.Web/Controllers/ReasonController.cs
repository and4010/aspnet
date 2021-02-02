using DataTables;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models.Information;
using Microsoft.AspNet.Identity;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class ReasonController : Controller
    {
        /// <summary>
        /// 基本資料-貨故原因 View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ReasonViewModel model = new ReasonViewModel();
            return View(model);
        }

        /// <summary>
        /// 貨故原因表格
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ReasonJson")]
        public JsonResult ReasonJson(DataTableAjaxPostViewModel data)
        {
           
         
            ReasonViewModel reasonView = new ReasonViewModel();
            List<ReasonModel> model = reasonView.GetReason();
            model = ReasonViewModel.ReasonModelDTOrder.Search(data, model);
            model = ReasonViewModel.ReasonModelDTOrder.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }


        /// <summary>
        /// 貨故原因表格資料編輯
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ReasonEditor"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Editor(DataTableAjaxPostViewModel data, ReasonViewModel.ReasonEditor ReasonEditor)
        {

            ResultModel resultModel = new ResultModel();

            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();

            ReasonViewModel reasonViewModel = new ReasonViewModel();
            resultModel = reasonViewModel.SetReasonValue(ReasonEditor, id, name);


             //var response = new Editor().Model<ReasonModel>().Field(new Field("Remark")).Process(request).Data();
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }



    }

}