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
using CHPOUTSRCMES.Web.DataModel.Entity.Information;

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

        public ActionResult _ChangePassword()
        {
            return PartialView();
        }

        public ActionResult Create()
        {
            AccountModel accountModel = new AccountModel();
            accountModel.GetSubinventories = identityUOW.GetSubinventories();
            accountModel.GetRoleNameList = identityUOW.GetRoleNameList();
            return View(accountModel);
        }

        public ActionResult Edit(string id)
        {
            AccountModel accountModel = new AccountModel();
            accountModel.Id = id;
            accountModel.GetSubinventories = identityUOW.GetSubinventories();
            accountModel.GetRoleNameList = identityUOW.GetRoleNameList();
            return View(accountModel);
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

        [Authorize(Roles = "系統管理員")]
        [HttpPost]
        public JsonResult LoadTable(DataTableAjaxPostViewModel data)
        {

            List<AccountModel> model = identityUOW.GetTable();
            model = AccountViewModel.Search(data, model);
            model = AccountViewModel.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Subinventory()
        {
            List<UserSubinventory> model = identityUOW.GetSubinventories();

            return Json(new { model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ChangPasswordAsync(ManageUserViewModel manageUserViewModel)
        {
            var resultModel = new ResultModel();
            resultModel = await identityUOW.ChangePassword(manageUserViewModel, this.User.Identity.GetUserId());
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "系統管理員")]
        [HttpPost]
        public async Task<JsonResult> CreateUser([Bind(Include = "RoleName,Account, Password, Name, Email")] AccountModel accountModel, List<UserSubinventory> userSubinventory)
        {
            var resultModel = new ResultModel();
            resultModel = await identityUOW.CreateUser(accountModel, userSubinventory, this.User.Identity.GetUserId());
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult AccountDisable(string id)
        {

            var result = new ResultModel();
            result = identityUOW.AccountDisable(id);
            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }


        [HttpPost]
        [Authorize(Roles ="系統管理員")]
        public async Task<JsonResult> DeafultPassword(string id)
        {
            var result = new ResultModel();
            result = await identityUOW.ResetPassword(id);
            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }

        /// <summary>
        /// 刪除註記
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = new ResultModel();
            result = identityUOW.Delete(id);
            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }

        /// <summary>
        /// 取得預設checkbox
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCheckboxValue(string id)
        {
            var result = new ResultModel();
            var checkboxValue = identityUOW.GetCheckboxValue(id);
            return new JsonResult { Data = new { message = checkboxValue } };
        }

        /// <summary>
        /// 取得預設使用者
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserValue(string id)
        {
            var result = new ResultModel();
            var checkboxValue = identityUOW.GetUserValue(id);
            return new JsonResult { Data = new { message = checkboxValue } };
        }

        /// <summary>
        /// 編輯使用者
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="userSubinventory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(AccountModel accountModel, List<UserSubinventory> userSubinventory)
        {
            var result = new ResultModel();
            result = await identityUOW.Edit(accountModel, userSubinventory, this.User.Identity.GetUserId());
            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }


        [HttpPost]
        public JsonResult EditorAccount(AccountEditor AccountEditor)
        {

            var result = new ResultModel();
            var model = AccountViewModel.model;
            string User = "";


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

                    Id = "",
                    RoleId = 1,
                    RoleName = User,
                    Account = AccountEditor.AccountModel.Account,
                    Password = AccountEditor.AccountModel.Password,
                    Name = AccountEditor.AccountModel.Name,
                    Email = AccountEditor.AccountModel.Email,
                    Status = "啟用",
                    //Subinventory = subinventoryDetails,

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
                    //ID.Subinventory = subinventoryDetails;
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