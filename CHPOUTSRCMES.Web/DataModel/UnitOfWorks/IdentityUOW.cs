using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Managers;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NPOI.SS.UserModel;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class IdentityUOW : UnitOfWork
    {
        private AppUserManager userManager { get; set; }

        private AppRoleManager roleManager { get; set; }


        public IdentityUOW(MesContext context) : base(context)
        {
            userManager = new AppUserManager(new UserStore<AppUser>(context));
            roleManager = new AppRoleManager(new RoleStore<IdentityRole>(context));
        }

        public async Task<ResultDataModel<AppUser>> SignInAsync(IAuthenticationManager authenticationManager, LoginViewModel model)
        {
            ResultDataModel<AppUser> resultModel = new ResultDataModel<AppUser>();
            var user = await userManager.FindAsync(model.UserName, model.Password);
            if (user != null)
            {
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);

                resultModel.Code = 0;
                resultModel.Success = true;
                resultModel.Msg = "";
                resultModel.Data = user;
            }
            else
            {
                resultModel.Code = -1;
                resultModel.Success = false;
                resultModel.Msg ="帳號或密碼輸入錯誤!!";
                resultModel.Data = null;
            }
            return resultModel;
        }

        public void SignOut(IAuthenticationManager authenticationManager)
        {
            authenticationManager.SignOut();
        }


        public void generateRoles()
        {
            // RoleTypes is a class containing constant string values for different roles
            roleManager.Create(new IdentityRole("系統管理員"));
            roleManager.Create(new IdentityRole("華紙使用者"));
            roleManager.Create(new IdentityRole("使用者"));
        }

        public void ImportUser(IWorkbook book)
        {
            if (book.NumberOfSheets == 0)
            {
                return;
            }
            var errors = new List<string>();
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                if (!book.GetSheetAt(i).SheetName.Contains("(User)"))
                {
                    continue;
                }

                ISheet sheet = book.GetSheetAt(i);
                var noOfRow = sheet.LastRowNum;

                int start_pos = 1;
                for (int rowIterator = start_pos; rowIterator <= noOfRow; rowIterator++)
                {
                    IRow row = sheet.GetRow(rowIterator);
                    if (row != null
                        && row.Cells.Count >= 4
                        && row.GetCell(0) != null
                        && row.GetCell(1) != null
                        && row.GetCell(2) != null
                        && row.GetCell(3) != null)
                    {
                        // Add code to initialize context tables
                        AppUser user = new AppUser();
                        string rolename = ExcelUtil.GetCellString(row.GetCell(0)).Trim();
                        string password = ExcelUtil.GetCellString(row.GetCell(2)).Trim();
                        user.UserName = ExcelUtil.GetCellString(row.GetCell(1)).Trim();
                        user.DisplayName = ExcelUtil.GetCellString(row.GetCell(3)).Trim();
                        user.Email = ExcelUtil.GetCellString(row.GetCell(4)).Trim();
                        user.EmailConfirmed = false;
                        var result = userManager.Create(user, password);
                        if (result.Succeeded)
                        {
                            userManager.AddToRole(user.Id, rolename);
                        }
                    }
                }


            }

        }
    }
}