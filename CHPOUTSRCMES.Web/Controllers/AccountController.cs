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
using CHPOUTSRCMES.Web.DataModel.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using Microsoft.Owin.Security;
using CHPOUTSRCMES.Web.DataModel;
using System.Security.Claims;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IdentityUOW identityUOW { set; get; }

        private IAuthenticationManager authenticationManager
        {
            get { return this.HttpContext.GetOwinContext().Authentication; }
        }

        public AccountController()
        {
            identityUOW = new IdentityUOW(new MesContext());
        }


        //
        // GET: /Account/
        [Authorize(Roles="系統管理員, 華紙使用者")]
        public ActionResult Index()
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            //取得使用者角色
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var loginViewModel = CheckLoginCookie();
            ViewBag.ReturnUrl = returnUrl;
            //return View("Login", "_Layout2", loginViewModel);
            return View(loginViewModel);
        }

        private LoginViewModel CheckLoginCookie()
        {
            var loginViewModel = new LoginViewModel();
            if (Request.Cookies.Get("username") != null)
            {
                loginViewModel.UserName = Request.Cookies["username"].Value;
                return loginViewModel;
            }
            return loginViewModel;
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await identityUOW.SignInAsync(authenticationManager, model);

                if (!result.Success)
                {
                    ModelState.AddModelError("", "帳號或密碼有誤!");
                }
                else
                {
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("username") { Value = result.Data.UserName });
                        Response.Cookies.Add(new HttpCookie("displayname") { Value = result.Data.DisplayName });
                    }
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LogOff()
        {
            identityUOW.SignOut(authenticationManager);
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return Json(new { status = true, message = "登出成功!!" });
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
        [Authorize(Roles ="系統管理員")]
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