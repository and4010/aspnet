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

namespace CHPOUTSRCMES.Web.Controllers
{
    public class ReasonController : Controller
    {
        // GET: Reason
        public ActionResult Index()
        {
            ReasonViewModel model = new ReasonViewModel();
            return View(model);
        }

        [HttpPost, ActionName("ReasonJson")]
        public JsonResult ReasonJson(DataTableAjaxPostViewModel data)
        {
            List<ReasonModel> model;
         
            ReasonViewModel reasonView = new ReasonViewModel();
            if(ReasonViewModel.models.Count == 0)
            {
                reasonView.GetReason();
            }

            model = ReasonViewModel.models;
            model = ReasonViewModel.ReasonModelDTOrder.Search(data, model);
            model = ReasonViewModel.ReasonModelDTOrder.Order(data.Order, model).ToList();
            model = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }



        [HttpPost]
        public ActionResult Editor(DataTableAjaxPostViewModel data)
        {

            var boolean = false;
            var msg = "";
            var model = ReasonViewModel.models;

            var request = System.Web.HttpContext.Current.Request.Form;
            string[] name1 = request.AllKeys;

            var getReason_code = name1.GetValue(0).ToString().Trim();
            var Reason_code = request[getReason_code].ToString().Trim();

            var getReason_desc = name1.GetValue(1).ToString().Trim();
            var Reason_desc = request[getReason_desc].ToString().Trim();

            var getAction = name1.GetValue(2).ToString().Trim();
            if(getAction != "action")
            {
                getAction = name1.GetValue(6).ToString().Trim();
            }
            var Action = request[getAction].ToString().Trim();



            if (Reason_code.Length == 0 && Reason_desc.Length == 0)
            {
                boolean = false;
            }
            else
            {

                if (Action == "edit")
                {
                    var ID = model.First(r => r.Reason_code.ToString() == Reason_code);

                    if (ID != null)
                    {
                        ID.Reason_desc = Reason_desc;
                        boolean = true;
                    }

                }

                if (Action == "create")
                {
                   var d = model.FirstOrDefault(r => r.Reason_code.ToString() == Reason_code);
                    if(d == null)
                    {
                        ReasonViewModel.models.Add(new ReasonModel
                        {
                            Reason_code = Reason_code,
                            Reason_desc = Reason_desc,
                            Create_by = "華紙管理員",
                            Create_date = DateTime.Now,
                            Last_update_by = "華紙管理員",
                            Last_Create_date = DateTime.Now,
                        });
                        boolean = true;
                    }
                    else
                    {
                        msg = "代碼已存在";
                    }
             

                }

                if (Action == "remove")
                {
                    var ID = model.First(r => r.Reason_code.ToString() == Reason_code);
                    model.Remove(ID);
                    boolean = true;
                }
            }








           // var response = new Editor().Model<ReasonModel>().Field(new Field("Remark")).Process(request).Data();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model , boolean ,msg }, JsonRequestBehavior.AllowGet);
        }



    }

}