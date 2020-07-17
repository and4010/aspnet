using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Models.Subinventory;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost, ActionName("AccountJson")]
        public JsonResult AccountJson(DataTableAjaxPostViewModel data)
        {

            if (AccountViewModel.model.Count == 0)
            {
                AccountViewModel.GetAccount();

            }

            List<AccountModel> model = AccountViewModel.model;
            model = AccountViewModel.Search(data, model);
            model = AccountViewModel.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subinventory()
        {
            List<SUBINVENTORY> model = new List<SUBINVENTORY>();
            model.Add(new SUBINVENTORY
            {
                ORGANIZATION_ID = 1,
                SUBINVENTORY_CODE = "TB2",
                SUBINVENTORY_NAME = "測試倉庫",
            });
            model.Add(new SUBINVENTORY
            {
                ORGANIZATION_ID = 2,
                SUBINVENTORY_CODE = "FTA",
                SUBINVENTORY_NAME = "成品倉",
            });
            return Json(new { model }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult Create([Bind(Include = "RoleName,Account, Password, Name, Email")] AccountModel accountModel, string[] Subinventory)
        //{
        //    var result = new ResultModel();
        //    var model = AccountViewModel.model;
        //    string content = "";
        //    string User = "";
        //    if (ModelState.IsValid)
        //    {

        //        for (int i = 0; i <= Subinventory.Length - 1; i++)
        //        {
        //            content = Subinventory[i] + " " + content;
        //        }

        //        if (accountModel.RoleName == "1")
        //        {
        //            User = "使用者";
        //        }
        //        else if (accountModel.RoleName == "2")
        //        {
        //            User = "華紙使用者";
        //        }
        //        else
        //        {
        //            User = "系統管理員";
        //        }



        //        model.Add(new AccountModel()
        //        {

        //            Id = model.Count + 1,
        //            RoleId = 1,
        //            RoleName = User,
        //            Account = accountModel.Account,
        //            Password = accountModel.Password,
        //            Name = accountModel.Name,
        //            Email = accountModel.Email,
        //            Status = "啟用",
        //            Subinventory = "1",

        //        });
        //        result.Success = true;
        //        result.Msg = "成功";
        //        return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        //    }

        //    return View();

        //}


        [HttpPost]
        public ActionResult AccountDisable(string id)
        {

            var model = AccountViewModel.model;
            var result = new ResultModel();
            var ID = model.First(r => r.Id.ToString() == id);

            if (ID != null)
            {
                if (ID.Status == "啟用")
                {
                    ID.Status = "停用";
                    result.Success = true;
                    result.Msg = "成功";
                }
                else
                {
                    ID.Status = "啟用";
                    result.Success = true;
                    result.Msg = "成功";
                }

            }
            else
            {
                result.Success = false;
                result.Msg = "失敗";
            }


            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }



        [HttpPost]
        public ActionResult DeafultPassword(string id)
        {

            var model = AccountViewModel.model;
            var result = new ResultModel();
            var ID = model.First(r => r.Id.ToString() == id);
            if (ID != null)
            {
                ID.Password = "00000";
                result.Success = true;
                result.Msg = "成功";
            }
            else
            {
                result.Success = false;
                result.Msg = "失敗";
            }

            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            var model = AccountViewModel.model;
            var result = new ResultModel();
            try
            {
                var ID = model.First(r => r.Id.ToString() == id);
                model.Remove(ID);
                result.Success = true;
                result.Msg = "成功";
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Msg = e.Message;
            }

            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }

        //[HttpPost]
        //public ActionResult Edit(string id, string strUser, string Name, string[] Subinventory)
        //{
        //    var result = new ResultModel();
        //    var model = AccountViewModel.model;
        //    string content = "";
        //    string User = "";
        //    var ID = model.First(r => r.Id.ToString() == id);
        //    if (ID != null)
        //    {
        //        for (int i = 0; i <= Subinventory.Length - 1; i++)
        //        {
        //            content = Subinventory[i] + " " + content;
        //        }

        //        if (strUser == "1")
        //        {
        //            User = "使用者";
        //        }
        //        else if (strUser == "2")
        //        {
        //            User = "華紙使用者";
        //        }
        //        else
        //        {
        //            User = "系統管理員";
        //        }

        //        ID.RoleName = User;
        //        ID.Name = Name;
        //        ID.Subinventory = content;
        //        result.Success = true;
        //        result.Msg = "成功";

        //    }
        //    else
        //    {
        //        result.Success = true;
        //        result.Msg = "失敗";

        //    }


        //    return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        //}


        [HttpPost]
        public JsonResult EditorAccount(AccountEditor AccountEditor)
        {

            var result = new ResultModel();
            var model = AccountViewModel.model;
            string User = "";

            List<SubinventoryDetail> subinventoryDetails = new List<SubinventoryDetail>();
            for (int i = 0; i < AccountEditor.AccountModel.Subinventory.Count; i++)
            {
                subinventoryDetails.Add(new SubinventoryDetail
                {
        
                    SubinventoryName = AccountEditor.AccountModel.Subinventory[i].SubinventoryName,
                });
            }


            if (AccountEditor.Action == "create")
            {
                
                if (AccountEditor.AccountModel.RoleId == 1)
                {
                    User = "使用者";
                }
                else if (AccountEditor.AccountModel.RoleId == 2)
                {
                    User = "華紙使用者";
                }
                else
                {
                    User = "系統管理員";
                }

                model.Add(new AccountModel()
                {

                    Id = model.Count + 1,
                    RoleId = 1,
                    RoleName = User,
                    Account = AccountEditor.AccountModel.Account,
                    Password = AccountEditor.AccountModel.Password,
                    Name = AccountEditor.AccountModel.Name,
                    Email = AccountEditor.AccountModel.Email,
                    Status = "啟用",
                    Subinventory = subinventoryDetails,

                });
                result.Success = true;
                result.Msg = "成功";
            }


            if (AccountEditor.Action == "edit")
            {
                var ID = model.First(r => r.Id == AccountEditor.AccountModel.Id);
                if (ID != null)
                {

                    if (AccountEditor.AccountModel.RoleId == 1)
                    {
                        User = "使用者";
                    }
                    else if (AccountEditor.AccountModel.RoleId == 2)
                    {
                        User = "華紙使用者";
                    }
                    else
                    {
                        User = "系統管理員";
                    }

                    ID.RoleName = User;
                    ID.Name = AccountEditor.AccountModel.Name;
                    ID.Subinventory = subinventoryDetails;
                    result.Success = true;
                    result.Msg = "成功";

                }
                else
                {
                    result.Success = true;
                    result.Msg = "失敗";

                }
            }



            return Json(new { status = result.Success, message = result.Msg }, JsonRequestBehavior.AllowGet);

        }


        public class AccountEditor
        {
            public string Action { get; set; }
            //public List<long> TripDetailDT_IDs { get; set; }
            //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
            public AccountModel AccountModel { get; set; }
        }


    }
}