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
using System.Reflection;
using CHPOUTSRCMES.Web.Util;
using System.Diagnostics;

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

        /// <summary>
        /// 變更密碼Dailog
        /// </summary>
        /// <returns></returns>
        public ActionResult _ChangePassword()
        {
            return PartialView();
        }

        /// <summary>
        /// 新增使用者View
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            AccountModel accountModel = new AccountModel();
            accountModel.GetSubinventories = identityUOW.GetSubinventories();
            accountModel.GetRoleNameList = identityUOW.GetRoleNameList();
            return View(accountModel);
        }

        /// <summary>
        /// 編輯使用者View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            AccountModel accountModel = new AccountModel();
            accountModel.Id = id;
            accountModel.GetSubinventories = identityUOW.GetSubinventories();
            accountModel.GetRoleNameList = identityUOW.GetRoleNameList();
            return View(accountModel);
        }

        /// <summary>
        /// 使用者管理首頁View
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="系統管理員")]
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

        /// <summary>
        /// 使用者登入View
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var loginViewModel = CheckLoginCookie();
            ViewBag.ReturnUrl = returnUrl;
            return View(loginViewModel);
        }

        /// <summary>
        /// 檢查登入Cookie
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
                    ModelState.AddModelError("", result.Msg);
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

        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 使用者管理表格
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得倉庫設定清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Subinventory()
        {
            List<UserSubinventory> model = identityUOW.GetSubinventories();

            return Json(new { model }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="manageUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ChangPasswordAsync(ManageUserViewModel manageUserViewModel)
        {
            var resultModel = new ResultModel();
            resultModel = await identityUOW.ChangePassword(manageUserViewModel, this.User.Identity.GetUserId());
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增使用者帳號
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="userSubinventory"></param>
        /// <returns></returns>
        [Authorize(Roles = "系統管理員")]
        [HttpPost]
        public async Task<JsonResult> CreateUser([Bind(Include = "RoleName,Account, Password, Name, Email")] AccountModel accountModel, List<UserSubinventory> userSubinventory)
        {
            var resultModel = new ResultModel();
            resultModel = await identityUOW.CreateUser(accountModel, userSubinventory, this.User.Identity.GetUserId());
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 使用者帳號啟用或停用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AccountDisable(string id)
        {

            var result = new ResultModel();
            result = identityUOW.AccountDisable(id);
            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }

        /// <summary>
        /// 恢復使用者密碼至預設密碼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="系統管理員")]
        public async Task<JsonResult> DeafultPassword(string id)
        {
            var result = new ResultModel();
            result = await identityUOW.ResetPassword(id);
            return new JsonResult { Data = new { status = result.Success, message = result.Msg } };
        }

        /// <summary>
        /// 刪除使用者帳號
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
        /// 取得編輯使用者時的倉庫設定checkbox設定值
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
        /// 取得編輯使用者時的使用者資料
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


        /// <summary>
        /// 取得使用者所屬倉庫(在倉庫設定中有被勾選的倉庫)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetViewUserSub(string id)
        {
            var UserSub =  identityUOW.ViewUserSub(id);
            return new JsonResult { Data = new { message = UserSub } };
        }
       


        public class AccountEditor
        {
            public string Action { get; set; }
            public AccountModel AccountModel { get; set; }
        }


    }
}