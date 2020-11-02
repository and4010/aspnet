using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Managers;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NLog;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class IdentityUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

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
                var flag = user.Flag == null ? "" : user.Flag;
                if (user.Status == "停用" || flag == "D")
                {
                    resultModel.Code = -1;
                    resultModel.Success = false;
                    resultModel.Msg = "帳號已停用或刪除";
                    resultModel.Data = null;
                    return resultModel;
                }

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
                resultModel.Msg = "帳號或密碼輸入錯誤!!";
                resultModel.Data = null;
            }
            return resultModel;
        }

        public void SignOut(IAuthenticationManager authenticationManager)
        {
            authenticationManager.SignOut();
        }


        public async Task<ResultModel> ChangePassword(ManageUserViewModel manageUserViewModel, string UserId)
        {
            ResultModel resultModel = new ResultModel();
            IdentityResult result = await userManager.ChangePasswordAsync(UserId, manageUserViewModel.OldPassword, manageUserViewModel.NewPassword);
            if (result.Succeeded)
            {
                resultModel.Msg = "密碼變更成功";
                resultModel.Success = true;
            }
            else
            {
                resultModel.Msg = result.Errors.First();
                resultModel.Success = false;
            }
            return resultModel;
        }

        /// <summary>
        /// 建立帳號
        /// </summary>
        /// <param name="accountModel"></param>
        /// <param name="Subinventory"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<ResultModel> CreateUser(AccountModel accountModel, List<UserSubinventory> Subinventory, string UserId)
        {
            ResultModel resultModel = new ResultModel();
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {

                    var user = await userManager.FindByNameAsync(accountModel.Account);
                    if (user != null)
                    {
                        resultModel.Msg = "此使用者已存在!";
                        resultModel.Success = false;
                        return resultModel;
                    }

                    var createUser = new AppUser()
                    {
                        UserName = accountModel.Account,
                        DisplayName = accountModel.Name,
                        Email = accountModel.Email,
                        Status = "啟用"
                    };

                    IdentityResult createIdentityResult = await userManager.CreateAsync(createUser, accountModel.Password);
                    if (!createIdentityResult.Succeeded)
                    {
                        string error = "";
                        foreach (var errors in createIdentityResult.Errors)
                        {
                            error = errors;
                        }
                        resultModel.Msg = "使用者:" + accountModel.Account + "產生失敗!" + error;
                        resultModel.Success = false;
                    }
                    else
                    {
                        IdentityResult addRoleIdentityResult = await userManager.AddToRoleAsync(createUser.Id, accountModel.RoleName);
                        if (!addRoleIdentityResult.Succeeded)
                        {
                            resultModel.Msg = "加入角色:" + accountModel.RoleName + "失敗!";
                            resultModel.Success = false;
                        }
                        else
                        {
                            for (int i = 0; i < Subinventory.Count; i++)
                            {
                                USER_SUBINVENTORY_T uSER_SUBINVENTORY_T = new USER_SUBINVENTORY_T();
                                uSER_SUBINVENTORY_T.UserId = createUser.Id;
                                uSER_SUBINVENTORY_T.OrganizationId = Subinventory[i].ORGANIZATIONID;
                                uSER_SUBINVENTORY_T.SubinventoryCode = Subinventory[i].SUBINVENTORY_CODE;
                                uSER_SUBINVENTORY_T.CreatedBy = UserId;
                                uSER_SUBINVENTORY_T.CreationDate = DateTime.Now;
                                userSubinventoryTRepository.Create(uSER_SUBINVENTORY_T, true);
                            }
                            resultModel.Success = true;
                            resultModel.Msg = "新增成功";
                            transaction.Commit();
                        }
                    }

                    return resultModel;
                }


                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    transaction.Rollback();
                    return new ResultModel(false, e.Message);
                }
            }
        }

        public async Task<ResultModel> Edit(AccountModel accountModel, List<UserSubinventory> Subinventory, string UserId)
        {
            ResultModel resultModel = new ResultModel();
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    //先全部刪除再新增
                    var ust = userSubinventoryTRepository.Get(x => x.UserId == accountModel.Id).ToList();
                    foreach (USER_SUBINVENTORY_T data in ust)
                    {
                        userSubinventoryTRepository.Delete(data, true);
                    }

                    var user = await userManager.FindByIdAsync(accountModel.Id);
                    if (user == null)
                    {
                        return new ResultModel (false,"此使用者不存在!");
                    }

                    if (!await roleManager.RoleExistsAsync(accountModel.RoleName))
                    {
                        return new ResultModel(false, "角色:" + accountModel.RoleName + "不存在!");
                    }

                    user.DisplayName = accountModel.Name;
                    user.Email = accountModel.Email;

                    IdentityResult updateIdentityResult = await userManager.UpdateAsync(user);
                    if (!updateIdentityResult.Succeeded)
                    {
                        return new ResultModel (false,  "使用者:" + accountModel.Name + "更新失敗!");
                    }

                    var rolename = userManager.GetRoles(user.Id).FirstOrDefault();
                    if (rolename.CompareTo(accountModel.RoleName) != 0)
                    {
                        IdentityResult removeRoleIdentityResult = await userManager.RemoveFromRoleAsync(user.Id, rolename);
                        if (!removeRoleIdentityResult.Succeeded)
                        {
                            return new ResultModel (false,"移除角色:" + rolename + "失敗!");
                        }
                        IdentityResult addRoleIdentityResult = await userManager.AddToRoleAsync(user.Id, accountModel.RoleName);
                        if (!addRoleIdentityResult.Succeeded)
                        {
                            return new ResultModel (false,"加入角色:" + accountModel.RoleName + "失敗!" );
                        }
                    }
                    for (int i = 0; i < Subinventory.Count; i++)
                    {
                        USER_SUBINVENTORY_T uSER_SUBINVENTORY_T = new USER_SUBINVENTORY_T();
                        uSER_SUBINVENTORY_T.UserId = accountModel.Id;
                        uSER_SUBINVENTORY_T.OrganizationId = Subinventory[i].ORGANIZATIONID;
                        uSER_SUBINVENTORY_T.SubinventoryCode = Subinventory[i].SUBINVENTORY_CODE;
                        uSER_SUBINVENTORY_T.CreatedBy = UserId;
                        uSER_SUBINVENTORY_T.CreationDate = DateTime.Now;
                        userSubinventoryTRepository.Create(uSER_SUBINVENTORY_T, true);
                    }
                    transaction.Commit();
                    return new ResultModel(true, "更新成功");
                }

                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    transaction.Rollback();
                    return new ResultModel(false, e.Message);
                }
            }
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultModel> ResetPassword(string id)
        {
            IdentityResult result = null;
            try
            {
                string msg = "";
                bool hasPassword = HasPassword(id);
                if (hasPassword)
                {
                    result = await userManager.RemovePasswordAsync(id);

                    if (!result.Succeeded)
                    {
                        msg = "重設密碼失敗, ";
                        foreach (var error in result.Errors)
                        {
                            msg += error;
                        }
                        return new ResultModel(result.Succeeded, msg);
                    }
                }

                result = await userManager.AddPasswordAsync(id, "000000");
                if (!result.Succeeded)
                {
                    msg = "重設密碼失敗";
                    foreach (var error in result.Errors)
                    {
                        msg += error;
                    }
                }
                return new ResultModel(result.Succeeded, msg);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }

        }

        /// <summary>
        /// 註記帳號刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel Delete(string id)
        {
            try
            {
                var user = userManager.FindById(id);
                if (user != null)
                {
                    user.Flag = "D";
                    userManager.Update(user);
                    return new ResultModel(true, "成功");
                }
                return new ResultModel(false, "找不到ID");
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }

        }

        /// <summary>
        /// 帳號狀態
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel AccountDisable(string id)
        {
            try
            {
                var user = userManager.FindById(id);
                if (user != null)
                {
                    if (user.Status == "啟用")
                    {
                        user.Status = "停用";
                        userManager.Update(user);
                        return new ResultModel(true, "成功");
                    }
                    else
                    {
                        user.Status = "啟用";
                        userManager.Update(user);
                        return new ResultModel(true, "成功");
                    }
                }
                return new ResultModel(false, "找不到ID");
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }
        }

        /// <summary>
        /// 取得table
        /// </summary>
        /// <returns></returns>
        public List<AccountModel> GetTable()
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"
SELECT 
ut.Id as Id,
rt.Name as RoleName,
UserName as Account,
DisplayName as Name,
Email as Email,
ISNULL(Status,'啟用') AS Status,
(SELECT (COUNT(cast(SUBINVENTORY_CODE as nvarchar)))
FROM USER_SUBINVENTORY_T   
WHERE UserId = ut.Id) as SubinventoryCount
FROM USER_T ut
join USER_ROLE_T urt on urt.UserId = ut.Id
Join ROLE_T rt on rt.Id = urt.RoleId
where ISNULL(Flag,'') <> 'D'
");
                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        return mesContext.Database.SqlQuery<AccountModel>(commandText, sqlParameterList.ToArray()).ToList();
                    }
                    else
                    {
                        return mesContext.Database.SqlQuery<AccountModel>(commandText).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<AccountModel>();
            }
        }


        /// <summary>
        /// 取得倉庫管理帳號
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<UserSubinventory> GetSubinventories()
        {
            return subinventoryRepository
                       .GetAll()
                       .AsNoTracking()
                       .Where(x => x.ControlFlag != ControlFlag.Deleted && x.OspFlag == "Y")
                       .Join(organizationRepository.GetAll(), x => x.OrganizationId, y => y.OrganizationId, (x, y) => new UserSubinventory() {
                           ORGANIZATIONID = y.OrganizationId,
                           ORGANIZATION_CODE = y.OrganizationCode,
                           SUBINVENTORY_CODE = x.SubinventoryCode,
                           SUBINVENTORY_NAME = x.SubinventoryName
                       })
                       .ToList();
        }

        /// <summary>
        /// 取得角色
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetRoleNameList()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach (IdentityRole role in roleManager.Roles)
            {
                roles.Add(new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Name,
                    Selected = false,
                });
            }
            return roles;
        }

        /// <summary>
        /// 取得預設checkbox
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCheckboxValue(string id)
        {

            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT CAST(UST.ORGANIZATION_ID AS nvarchar) +'-'+ UST.SUBINVENTORY_CODE + ' ' + ST.SUBINVENTORY_NAME +',' AS [text()]
FROM USER_SUBINVENTORY_T UST
JOIN SUBINVENTORY_T ST ON ST.SUBINVENTORY_CODE = UST.SUBINVENTORY_CODE
WHERE UserId = @id
FOR XML PATH('')");
                    string commandText = string.Format(query.ToString());
                    return mesContext.Database.SqlQuery<string>(commandText, new SqlParameter("@id", id)).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return "";
            }


        }

        /// <summary>
        /// 取得預設user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountModel GetUserValue(string id)
        {
            var user = userManager.FindById(id);
            var rolename = userManager.GetRoles(id).FirstOrDefault();
            if (user != null)
            {
                AccountModel accountModel = new AccountModel();
                accountModel.RoleName = rolename;
                accountModel.Name = user.DisplayName;
                accountModel.Account = user.UserName;
                accountModel.Email = user.Email;
                return accountModel;
            }
            else
            {
                return new AccountModel();
            }
        }

        public List<String> ViewUserSub(string id)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
ot.ORGANIZATION_CODE +'-'+ UST.SUBINVENTORY_CODE + '-' + ST.SUBINVENTORY_NAME
FROM USER_SUBINVENTORY_T UST
JOIN SUBINVENTORY_T ST ON ST.SUBINVENTORY_CODE = UST.SUBINVENTORY_CODE
join ORGANIZATION_T ot on ot.ORGANIZATION_ID = ust.ORGANIZATION_ID
WHERE UserId = @id
order by ot.ORGANIZATION_CODE");
                    string commandText = string.Format(query.ToString());
                    return mesContext.Database.SqlQuery<string>(commandText, new SqlParameter("@id", id)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<string>();
            }
        }

        private bool HasPassword(string id)
        {
            var user = userManager.FindById(id);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        #region 測試資料產生

        public void ImportUserMisc(IWorkbook book)
        {//大量寫入資料時，請關閉AutoDetectChangesEnabled 功能來提高效能
            this.Context.Configuration.AutoDetectChangesEnabled = false;
            //高度相關作業資料處理時，請使用 交易來 確保資料完整性
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    ImportUser(book);
                    //產生條碼設定預設值
                    ImportBcdMisc(book);
                    ImportUserSubinventory(book);
                    ImportReason(book);
                    //成功時，提交所有處理
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    //失敗，取消所有處理動作
                    txn.Rollback();
                }
            }
            this.Context.Configuration.AutoDetectChangesEnabled = true;

        }

        public void Import(IWorkbook book)
        {
            //大量寫入資料時，請關閉AutoDetectChangesEnabled 功能來提高效能
            this.Context.Configuration.AutoDetectChangesEnabled = false;
            //高度相關作業資料處理時，請使用 交易來 確保資料完整性
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    generateRoles();
                    ImportUser(book);
                    ImportOrganization(book);
                    ImportSubinventory(book);
                    ImportLocater(book);
                    ImportItems(book);
                    ImprotRelated(book);
                    ImportYszmpckq(book);
                    ImprotMachinePaperType(book);
                    ImprotTransaction(book);
                    //產生條碼設定預設值
                    ImportBcdMisc(book);
                    ImportUserSubinventory(book);
                    ImportReason(book);
                    //成功時，提交所有處理
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    //失敗，取消所有處理動作
                    txn.Rollback();
                }
            }
            this.Context.Configuration.AutoDetectChangesEnabled = true;
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
            ISheet sheet = FindSheet(book, "使用者帳號");
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
                    string rolename = ExcelUtil.GetStringCellValue(row.GetCell(0)).Trim();
                    string password = ExcelUtil.GetStringCellValue(row.GetCell(2)).Trim();
                    user.UserName = ExcelUtil.GetStringCellValue(row.GetCell(1)).Trim();
                    user.DisplayName = ExcelUtil.GetStringCellValue(row.GetCell(3)).Trim();
                    user.Email = ExcelUtil.GetStringCellValue(row.GetCell(4)).Trim();
                    user.EmailConfirmed = false;

                    var appUser = userManager.FindByName(user.UserName);
                    if (appUser == null)
                    {
                        var result = userManager.Create(user, password);
                        if (result.Succeeded)
                        {
                            userManager.AddToRole(user.Id, rolename);
                        }
                    }
                }
            }
        }

        private void ImportUserSubinventory(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "使用者倉庫");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell userNameCell = null;
            ICell subinventoryCell = null;

            userNameCell = ExcelUtil.FindCell("帳號", sheet);
            if (userNameCell == null)
            {
                throw new Exception("找不到帳號欄位");
            }
            subinventoryCell = ExcelUtil.FindCell("倉庫", sheet);
            if (subinventoryCell == null)
            {
                throw new Exception("找不到倉庫欄位");
            }

            for (int j = userNameCell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var adm = this.appUserRepository.Get(x => x.UserName.CompareTo("adm") == 0).FirstOrDefault();
                    var userName = ExcelUtil.GetStringCellValue(j, userNameCell.ColumnIndex, sheet);
                    var subinvenotryCode = ExcelUtil.GetStringCellValue(j, subinventoryCell.ColumnIndex, sheet);
                    var user = this.appUserRepository.Get(x => x.UserName.CompareTo(userName) == 0).FirstOrDefault();
                    var subinventory = this.subinventoryRepository.Get(x => x.SubinventoryCode.CompareTo(subinvenotryCode) == 0).FirstOrDefault();
                    //搜尋未執行 SaveChanges 的資料
                    var userSubinventory = this.Context.ChangeTracker.Entries<USER_SUBINVENTORY_T>()
                        .Where(x => x.Entity.UserId == user.Id && x.Entity.SubinventoryCode == subinventory.SubinventoryCode).FirstOrDefault();

                    var userSubinventory1 = userSubinventoryTRepository.Get(x => x.UserId == user.Id && x.SubinventoryCode == subinvenotryCode).FirstOrDefault();


                    //搜尋已執行 SaveChanges 的資料
                    //var org = transactionTypeRepository.Get(x => x.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    if ((userSubinventory == null || userSubinventory.Entity.UserId.Length == 0)
                        && (userSubinventory1 == null || userSubinventory1.UserId.Length == 0))
                    {
                        var now = DateTime.Now;
                        USER_SUBINVENTORY_T userSubinventoryT = new USER_SUBINVENTORY_T();
                        userSubinventoryT.UserId = user.Id;
                        userSubinventoryT.OrganizationId = subinventory.OrganizationId;
                        userSubinventoryT.SubinventoryCode = subinventory.SubinventoryCode;
                        userSubinventoryT.CreatedBy = adm.Id;
                        userSubinventoryT.CreationDate = now;
                        userSubinventoryT.LastUpdateBy = adm.Id;
                        userSubinventoryT.LastUpdateDate = now;
                        userSubinventoryTRepository.Create(userSubinventoryT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImportBcdMisc(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "條碼前置碼");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell subinventoryCell = null;
            ICell prefixCell = null;
            ICell serialSizeCell = null;

            subinventoryCell = ExcelUtil.FindCell("倉庫", sheet);
            if (subinventoryCell == null)
            {
                throw new Exception("找不到帳號欄位");
            }
            prefixCell = ExcelUtil.FindCell("條碼前置碼", sheet);
            if (prefixCell == null)
            {
                throw new Exception("找不到條碼前置碼欄位");
            }
            serialSizeCell = ExcelUtil.FindCell("流水號長度", sheet);
            if (serialSizeCell == null)
            {
                throw new Exception("找不到流水號長度欄位");
            }

            for (int j = subinventoryCell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var adm = this.appUserRepository.Get(x => x.UserName.CompareTo("adm") == 0).FirstOrDefault();
                    var subinvenotryCode = ExcelUtil.GetStringCellValue(j, subinventoryCell.ColumnIndex, sheet);
                    var subinventory = this.subinventoryRepository.Get(x => x.SubinventoryCode.CompareTo(subinvenotryCode) == 0).FirstOrDefault();
                    //搜尋未執行 SaveChanges 的資料
                    var item = this.Context.ChangeTracker.Entries<BCD_MISC_T>()
                        .Where(x => x.Entity.SubinventoryCode == subinvenotryCode).FirstOrDefault();

                    var item1 = bcdMiscRepository.Get(x => x.SubinventoryCode == subinvenotryCode).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = transactionTypeRepository.Get(x => x.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    if ((item == null || item.Entity.SubinventoryCode.Length == 0)
                        && (item1 == null || item1.SubinventoryCode.Length == 0))
                    {
                        var now = DateTime.Now;
                        BCD_MISC_T bcdMisc = new BCD_MISC_T();
                        bcdMisc.OrganizationId = subinventory.OrganizationId;
                        bcdMisc.SubinventoryCode = subinventory.SubinventoryCode;
                        bcdMisc.PrefixCode = ExcelUtil.GetStringCellValue(j, prefixCell.ColumnIndex, sheet);
                        bcdMisc.SerialSize = ExcelUtil.GetInt32CellValue(j, serialSizeCell.ColumnIndex, sheet, 4);
                        bcdMisc.CreatedBy = adm.Id;
                        bcdMisc.CreationDate = now;
                        bcdMisc.LastUpdateBy = adm.Id;
                        bcdMisc.LastUpdateDate = now;
                        bcdMiscRepository.Create(bcdMisc);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImportItems(IWorkbook book)
        {

            ISheet sheet = FindSheet(book, "XXIFV050_ITEMS_FTY_V");

            if (sheet == null) return;


            var noOfRow = sheet.LastRowNum;
            ICell ORGANIZATION_CODE_cell = null;
            ICell InventoryItemId_cell = null;
            ICell Item_number_cell = null;
            ICell CategoryCodeInv_cell = null;
            ICell CategoryNameInv_cell = null;
            ICell CategoryCodeCost_cell = null;
            ICell CategoryNameCost_cell = null;
            ICell CategoryCodeControl_cell = null;
            ICell CategoryNameControl_cell = null;
            ICell ItemDescEng_cell = null;
            ICell ItemDescSch_cell = null;
            ICell ItemDescTch_cell = null;
            ICell PrimaryUomCode_cell = null;
            ICell SecondaryUomCode_cell = null;
            ICell InventoryItemStatusCode_cell = null;
            ICell ItemType_cell = null;
            ICell CatalogElemVal010_cell = null;
            ICell CatalogElemVal020_cell = null;
            ICell CatalogElemVal030_cell = null;
            ICell CatalogElemVal040_cell = null;
            ICell CatalogElemVal050_cell = null;
            ICell CatalogElemVal060_cell = null;
            ICell CatalogElemVal070_cell = null;
            ICell CatalogElemVal080_cell = null;
            ICell CatalogElemVal090_cell = null;
            ICell CatalogElemVal100_cell = null;
            ICell CatalogElemVal110_cell = null;
            ICell CatalogElemVal120_cell = null;
            ICell CatalogElemVal130_cell = null;
            ICell CatalogElemVal140_cell = null;
            ICell ControlFlag_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            ORGANIZATION_CODE_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (ORGANIZATION_CODE_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

            InventoryItemId_cell = ExcelUtil.FindCell("INVENTORY_ITEM_ID", sheet);
            if (InventoryItemId_cell == null)
            {
                throw new Exception("找不到INVENTORY_ITEM_ID欄位");
            }

            Item_number_cell = ExcelUtil.FindCell("ITEM_NUMBER", sheet);
            if (Item_number_cell == null)
            {
                throw new Exception("找不到ITEM_NUMBER欄位");
            }

            CategoryCodeInv_cell = ExcelUtil.FindCell("CATEGORY_CODE_INV", sheet);
            if (CategoryCodeInv_cell == null)
            {
                throw new Exception("找不到CATEGORY_CODE_INV欄位");
            }
            CategoryNameInv_cell = ExcelUtil.FindCell("CATEGORY_NAME_INV", sheet);
            if (CategoryNameInv_cell == null)
            {
                throw new Exception("找不到CATEGORY_NAME_INV欄位");
            }
            CategoryCodeCost_cell = ExcelUtil.FindCell("CATEGORY_CODE_COST", sheet);
            if (CategoryCodeCost_cell == null)
            {
                throw new Exception("找不到Category_Code_Cost欄位");
            }
            CategoryNameCost_cell = ExcelUtil.FindCell("CATEGORY_NAME_COST", sheet);
            if (CategoryNameCost_cell == null)
            {
                throw new Exception("找不到CATEGORY_NAME_COST欄位");
            }
            CategoryCodeControl_cell = ExcelUtil.FindCell("CATEGORY_CODE_CONTROL", sheet);
            if (CategoryCodeControl_cell == null)
            {
                throw new Exception("找不到CATEGORY_CODE_CONTROL欄位");
            }
            CategoryNameControl_cell = ExcelUtil.FindCell("CATEGORY_NAME_CONTROL", sheet);
            if (CategoryNameControl_cell == null)
            {
                throw new Exception("找不到CATEGORY_NAME_CONTROL欄位");
            }
            ItemDescEng_cell = ExcelUtil.FindCell("ITEM_DESC_ENG", sheet);
            if (ItemDescEng_cell == null)
            {
                throw new Exception("找不到ITEM_DESC_ENG欄位");
            }
            ItemDescSch_cell = ExcelUtil.FindCell("ITEM_DESC_SCH", sheet);
            if (ItemDescSch_cell == null)
            {
                throw new Exception("找不到ITEM_DESC_SCH欄位");
            }

            ItemDescTch_cell = ExcelUtil.FindCell("ITEM_DESC_TCH", sheet);
            if (ItemDescTch_cell == null)
            {
                throw new Exception("找不到ITEM_DESC_TCH欄位");
            }
            PrimaryUomCode_cell = ExcelUtil.FindCell("PRIMARY_UOM_CODE", sheet);
            if (PrimaryUomCode_cell == null)
            {
                throw new Exception("找不到PRIMARY_UOM_CODE欄位");
            }
            SecondaryUomCode_cell = ExcelUtil.FindCell("SECONDARY_UOM_CODE", sheet);
            if (SecondaryUomCode_cell == null)
            {
                throw new Exception("找不到SECONDARY_UOM_CODE欄位");
            }
            InventoryItemStatusCode_cell = ExcelUtil.FindCell("INVENTORY_ITEM_STATUS_CODE", sheet);
            if (InventoryItemStatusCode_cell == null)
            {
                throw new Exception("找不到INVENTORY_ITEM_STATUS_CODE欄位");
            }
            ItemType_cell = ExcelUtil.FindCell("ITEM_TYPE", sheet);
            if (ItemType_cell == null)
            {
                throw new Exception("找不到ITEM_TYPE欄位");
            }
            CatalogElemVal010_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_010", sheet);
            if (CatalogElemVal010_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_010欄位");
            }
            CatalogElemVal020_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_020", sheet);
            if (CatalogElemVal020_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_020欄位");
            }

            CatalogElemVal030_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_030", sheet);
            if (CatalogElemVal030_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_030欄位");
            }

            CatalogElemVal040_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_040", sheet);
            if (CatalogElemVal040_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_040欄位");
            }

            CatalogElemVal050_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_050", sheet);
            if (CatalogElemVal050_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_050欄位");
            }

            CatalogElemVal060_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_060", sheet);
            if (CatalogElemVal060_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_060欄位");
            }

            CatalogElemVal070_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_070", sheet);
            if (CatalogElemVal070_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_070欄位");
            }

            CatalogElemVal080_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_080", sheet);
            if (CatalogElemVal080_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_080欄位");
            }

            CatalogElemVal090_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_090", sheet);
            if (CatalogElemVal090_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_090欄位");
            }

            CatalogElemVal100_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_100", sheet);
            if (CatalogElemVal100_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_100欄位");
            }

            CatalogElemVal110_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_110", sheet);
            if (CatalogElemVal110_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_110欄位");
            }

            CatalogElemVal120_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_120", sheet);
            if (CatalogElemVal120_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_120欄位");
            }

            CatalogElemVal130_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_130", sheet);
            if (CatalogElemVal130_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_130欄位");
            }

            CatalogElemVal140_cell = ExcelUtil.FindCell("CATALOG_ELEM_VAL_140", sheet);
            if (CatalogElemVal140_cell == null)
            {
                throw new Exception("找不到CATALOG_ELEM_VAL_140欄位");
            }

            //ControlFlag_cell = ExcelUtil.FindCell("CONTROL_FLAG", sheet);
            //if (ControlFlag_cell == null)
            //{
            //    throw new Exception("找不到CONTROL_FLAG欄位");
            //}

            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATE_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATE_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }

            for (int j = InventoryItemId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var id = Int64.Parse(ExcelUtil.GetStringCellValue(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<ITEMS_T>().Where(x => x.Entity.InventoryItemId == id).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = itemsTRepository.Get(x => x.InventoryItemId == id).FirstOrDefault();
                    if (org == null || org.Entity.InventoryItemId <= 0)
                    {
                        ITEMS_T iTEMS_T = new ITEMS_T();
                        iTEMS_T.InventoryItemId = ExcelUtil.GetLongCellValue(j, InventoryItemId_cell.ColumnIndex, sheet);
                        iTEMS_T.ItemNumber = ExcelUtil.GetStringCellValue(j, Item_number_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CategoryCodeInv = ExcelUtil.GetStringCellValue(j, CategoryCodeInv_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CategoryNameInv = ExcelUtil.GetStringCellValue(j, CategoryNameInv_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CategoryCodeCost = ExcelUtil.GetStringCellValue(j, CategoryCodeCost_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CategoryNameCost = ExcelUtil.GetStringCellValue(j, CategoryNameCost_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CategoryCodeControl = ExcelUtil.GetStringCellValue(j, CategoryCodeControl_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CategoryNameControl = ExcelUtil.GetStringCellValue(j, CategoryNameControl_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.ItemDescEng = ExcelUtil.GetStringCellValue(j, ItemDescEng_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.ItemDescSch = ExcelUtil.GetStringCellValue(j, ItemDescSch_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.ItemDescTch = ExcelUtil.GetStringCellValue(j, ItemDescTch_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.PrimaryUomCode = ExcelUtil.GetStringCellValue(j, PrimaryUomCode_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.SecondaryUomCode = ExcelUtil.GetStringCellValue(j, SecondaryUomCode_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.InventoryItemStatusCode = ExcelUtil.GetStringCellValue(j, InventoryItemStatusCode_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.ItemType = ExcelUtil.GetStringCellValue(j, ItemType_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal010 = ExcelUtil.GetStringCellValue(j, CatalogElemVal010_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal020 = ExcelUtil.GetStringCellValue(j, CatalogElemVal020_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal030 = ExcelUtil.GetStringCellValue(j, CatalogElemVal030_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal040 = ExcelUtil.GetStringCellValue(j, CatalogElemVal040_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal050 = ExcelUtil.GetStringCellValue(j, CatalogElemVal050_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal060 = ExcelUtil.GetStringCellValue(j, CatalogElemVal060_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal070 = ExcelUtil.GetStringCellValue(j, CatalogElemVal070_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal080 = ExcelUtil.GetStringCellValue(j, CatalogElemVal080_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal090 = ExcelUtil.GetStringCellValue(j, CatalogElemVal090_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal100 = ExcelUtil.GetStringCellValue(j, CatalogElemVal100_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal110 = ExcelUtil.GetStringCellValue(j, CatalogElemVal110_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal120 = ExcelUtil.GetStringCellValue(j, CatalogElemVal120_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal130 = ExcelUtil.GetStringCellValue(j, CatalogElemVal130_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.CatalogElemVal140 = ExcelUtil.GetStringCellValue(j, CatalogElemVal140_cell.ColumnIndex, sheet).Trim();
                        iTEMS_T.ControlFlag = "";
                        iTEMS_T.CreatedBy = ExcelUtil.GetLongCellValue(j, CreatedBy_cell.ColumnIndex, sheet);
                        iTEMS_T.CreationDate = ExcelUtil.GetDateTimeCellValue(j, CreationDate_cell.ColumnIndex, sheet, DateTime.Now);
                        iTEMS_T.LastUpdateBy = ExcelUtil.GetLongCellValue(j, LastUpdateBy_cell.ColumnIndex, sheet);
                        iTEMS_T.LastUpdateDate = ExcelUtil.GetDateTimeCellValue(j, LastUpdateDate_cell.ColumnIndex, sheet, DateTime.Now);
                        itemsTRepository.Create(iTEMS_T);


                        ORG_ITEMS_T oRG_ITEMS_T = new ORG_ITEMS_T();
                        var oCode = ExcelUtil.GetStringCellValue(j, ORGANIZATION_CODE_cell.ColumnIndex, sheet).Trim();
                        //搜尋未執行 SaveChanges 的資料
                        var data = this.Context.ChangeTracker.Entries<ORGANIZATION_T>().Where(x => x.Entity.OrganizationCode == oCode).FirstOrDefault();
                        //搜尋已執行 SaveChanges 的資料
                        //var o = organizationRepository.Get(x => x.OrganizationCode == ocode).FirstOrDefault();
                        if (data != null)
                        {
                            var entity = data.Entity;
                            oRG_ITEMS_T.InventoryItemId = ExcelUtil.GetLongCellValue(j, InventoryItemId_cell.ColumnIndex, sheet);
                            oRG_ITEMS_T.OrganizationId = entity.OrganizationId;
                            orgItemRepository.Create(oRG_ITEMS_T);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("第 {0} 列 出現錯誤 {1}", j + 1, LogUtilities.BuildExceptionMessage(ex));
                }
            }

            this.SaveChanges();
        }

        private void ImportOrganization(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_SUBINVENTORY_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            int start_pos = 1;


            ICell organizationId_cell = null;
            ICell organizationCode_cell = null;
            ICell organizationName_cell = null;
            ICell orgId_cell = null;
            ICell orgName_cell = null;

            orgId_cell = ExcelUtil.FindCell("ORG_ID", sheet);
            if (orgId_cell == null)
            {
                throw new Exception("找不到ORG_ID欄位");
            }

            orgName_cell = ExcelUtil.FindCell("ORG_NAME", sheet);
            if (orgName_cell == null)
            {
                throw new Exception("找不到ORG_NAME欄位");
            }

            organizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (organizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }

            organizationCode_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (organizationCode_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

            organizationName_cell = ExcelUtil.FindCell("ORGANIZATION_NAME", sheet);
            if (organizationName_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_NAME欄位");
            }

            for (int rowIterator = start_pos; rowIterator <= noOfRow; rowIterator++)
            {
                IRow row = sheet.GetRow(rowIterator);

                if (row != null
                    && row.Cells.Count >= 5)
                {
                    try
                    {
                        var id = ExcelUtil.GetLongCellValue(row.GetCell(organizationId_cell.ColumnIndex));
                        var orgUnitId = ExcelUtil.GetLongCellValue(row.GetCell(orgId_cell.ColumnIndex));

                        var orgUnit = this.Context.ChangeTracker
                            .Entries<ORG_UNIT_T>()
                            .Where(x => x.Entity.OrgId == orgUnitId)
                            .FirstOrDefault();
                        if (orgUnit == null)
                        {
                            ORG_UNIT_T orgUnitT = new ORG_UNIT_T();
                            orgUnitT.OrgId = orgUnitId;
                            orgUnitT.OrgName = ExcelUtil.GetStringCellValue(row.GetCell(orgName_cell.ColumnIndex));
                            orgUnitT.ControlFlag = "";
                            orgUnitRepository.Create(orgUnitT);
                        }

                        //搜尋未執行 SaveChanges 的資料
                        var org = this.Context.ChangeTracker
                            .Entries<ORGANIZATION_T>()
                            .Where(x => x.Entity.OrganizationId == id)
                            .FirstOrDefault();
                        //搜尋已執行 SaveChanges 的資料
                        //var org = organizationRepository.Get(x => x.OrganizationID == id).FirstOrDefault();
                        if (org == null)
                        {
                            ORGANIZATION_T oRGANIZATION_T = new ORGANIZATION_T();
                            oRGANIZATION_T.OrganizationId = ExcelUtil.GetLongCellValue(row.GetCell(organizationId_cell.ColumnIndex));
                            oRGANIZATION_T.OrganizationCode = ExcelUtil.GetStringCellValue(row.GetCell(organizationCode_cell.ColumnIndex)).Trim();
                            oRGANIZATION_T.OrganizationName = ExcelUtil.GetStringCellValue(row.GetCell(organizationName_cell.ColumnIndex)).Trim();
                            oRGANIZATION_T.OrgUnitId = orgUnitId;
                            oRGANIZATION_T.ControlFlag = "";
                            organizationRepository.Create(oRGANIZATION_T);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    }
                }
            }

            this.SaveChanges();
        }

        private void ImportSubinventory(IWorkbook book)
        {

            ISheet sheet = FindSheet(book, "XXCINV_SUBINVENTORY_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;

            ICell organizationId_cell = null;
            ICell subinventoryCode_cell = null;
            ICell subinventoryName_cell = null;
            ICell ospFlag_cell = null;
            ICell LocatorType_cell = null;

            organizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (organizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }

            subinventoryCode_cell = ExcelUtil.FindCell("SUBINVENTORY_CODE", sheet);
            if (subinventoryCode_cell == null)
            {
                throw new Exception("找不到SUBINVENTORY_CODE欄位");
            }

            subinventoryName_cell = ExcelUtil.FindCell("SUBINVENTORY_NAME", sheet);
            if (subinventoryName_cell == null)
            {
                throw new Exception("找不到SUBINVENTORY_NAME欄位");
            }
            ospFlag_cell = ExcelUtil.FindCell("OSP_FLAG", sheet);
            if (ospFlag_cell == null)
            {
                throw new Exception("找不到OSP_FLAG欄位");
            }
            LocatorType_cell = ExcelUtil.FindCell("LOCATOR_TYPE", sheet);
            if (LocatorType_cell == null)
            {
                throw new Exception("找不到LOCATOR_TYPE欄位");
            }

            for (int j = organizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var id = Int64.Parse(ExcelUtil.GetStringCellValue(j, organizationId_cell.ColumnIndex, sheet).Trim());
                    var subCode = ExcelUtil.GetStringCellValue(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<SUBINVENTORY_T>().Where(x => x.Entity.SubinventoryCode == subCode).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    // var org = subinventoryRepository.Get(x => x.OrganizationID == id && x.SubinventoryCode == subCode).FirstOrDefault();
                    if (org == null)
                    {
                        SUBINVENTORY_T sUBINVENTORY_T = new SUBINVENTORY_T();
                        sUBINVENTORY_T.OrganizationId = ExcelUtil.GetLongCellValue(j, organizationId_cell.ColumnIndex, sheet);
                        sUBINVENTORY_T.SubinventoryCode = ExcelUtil.GetStringCellValue(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                        sUBINVENTORY_T.SubinventoryName = ExcelUtil.GetStringCellValue(j, subinventoryName_cell.ColumnIndex, sheet).Trim();
                        sUBINVENTORY_T.OspFlag = ExcelUtil.GetStringCellValue(j, ospFlag_cell.ColumnIndex, sheet).Trim();
                        sUBINVENTORY_T.ControlFlag = "";
                        sUBINVENTORY_T.LocatorType = ExcelUtil.GetLongCellValue(j, LocatorType_cell.ColumnIndex, sheet);
                        subinventoryRepository.Create(sUBINVENTORY_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }

            this.SaveChanges();
        }

        private void ImportLocater(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_SUBINVENTORY_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;

            ICell organizationId_cell = null;
            ICell subinventoryCode_cell = null;
            ICell LocatorId_cell = null;
            //ICell LocatorType_cell = null;
            ICell LocatorSegments_cell = null;
            ICell LocatorDesc_cell = null;
            ICell Segment1_cell = null;
            ICell Segment2_cell = null;
            ICell Segment3_cell = null;
            ICell Segment4_cell = null;
            ICell LocatorStatus_cell = null;
            ICell LocatorStatusCode_cell = null;
            ICell LocatorPickingOrder_cell = null;
            ICell LocatorDisableDate_cell = null;

            organizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (organizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }

            subinventoryCode_cell = ExcelUtil.FindCell("SUBINVENTORY_CODE", sheet);
            if (subinventoryCode_cell == null)
            {
                throw new Exception("找不到SUBINVENTORY_CODE欄位");
            }

            LocatorId_cell = ExcelUtil.FindCell("LOCATOR_ID", sheet);
            if (LocatorId_cell == null)
            {
                throw new Exception("找不到LOCATOR_ID欄位");
            }
            //LocatorType_cell = ExcelUtil.FindCell("LOCATOR_TYPE", sheet);
            //if (LocatorType_cell == null)
            //{
            //    throw new Exception("找不到LOCATOR_TYPE欄位");
            //}
            LocatorSegments_cell = ExcelUtil.FindCell("LOCATOR_SEGMENTS", sheet);
            if (LocatorSegments_cell == null)
            {
                throw new Exception("找不到LOCATOR_SEGMENTS欄位");
            }
            LocatorDesc_cell = ExcelUtil.FindCell("LOCATOR_DESC", sheet);
            if (LocatorDesc_cell == null)
            {
                throw new Exception("找不到LOCATOR_DESC欄位");
            }
            Segment1_cell = ExcelUtil.FindCell("SEGMENT1", sheet);
            if (Segment1_cell == null)
            {
                throw new Exception("找不到SEGMENT1欄位");
            }
            Segment2_cell = ExcelUtil.FindCell("SEGMENT2", sheet);
            if (Segment2_cell == null)
            {
                throw new Exception("找不到SEGMENT2欄位");
            }
            Segment3_cell = ExcelUtil.FindCell("SEGMENT3", sheet);
            if (Segment3_cell == null)
            {
                throw new Exception("找不到SEGMENT3欄位");
            }
            Segment4_cell = ExcelUtil.FindCell("SEGMENT4", sheet);
            if (Segment4_cell == null)
            {
                throw new Exception("找不到SEGMENT4欄位");
            }
            LocatorStatus_cell = ExcelUtil.FindCell("LOCATOR_STATUS", sheet);
            if (LocatorStatus_cell == null)
            {
                throw new Exception("找不到LOCATOR_STATUS欄位");
            }
            LocatorStatusCode_cell = ExcelUtil.FindCell("LOCATOR_STATUS_CODE", sheet);
            if (LocatorStatusCode_cell == null)
            {
                throw new Exception("找不到LOCATOR_STATUS_CODE欄位");
            }
            LocatorPickingOrder_cell = ExcelUtil.FindCell("LOCATOR_PICKING_ORDER", sheet);
            if (LocatorPickingOrder_cell == null)
            {
                throw new Exception("找不到LOCATOR_PICKING_ORDER欄位");
            }
            LocatorDisableDate_cell = ExcelUtil.FindCell("LOCATOR_DISABLE_DATE", sheet);
            if (LocatorDisableDate_cell == null)
            {
                throw new Exception("找不到LOCATOR_DISABLE_DATE欄位");
            }


            for (int j = organizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    string idString = ExcelUtil.GetStringCellValue(j, LocatorId_cell.ColumnIndex, sheet).Trim();
                    if (string.IsNullOrEmpty(idString)) continue;

                    var id = Int64.Parse(idString);
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<LOCATOR_T>().Where(x => x.Entity.LocatorId == id).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = locatorTRepository.Get(x => x.LocatorId == id).FirstOrDefault();
                    if (org == null || org.Entity.LocatorId <= 0)
                    {
                        LOCATOR_T lOCATOR_T = new LOCATOR_T();
                        lOCATOR_T.OrganizationId = ExcelUtil.GetLongCellValue(j, organizationId_cell.ColumnIndex, sheet);
                        lOCATOR_T.SubinventoryCode = ExcelUtil.GetStringCellValue(j, subinventoryCode_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.LocatorId = ExcelUtil.GetLongCellValue(j, LocatorId_cell.ColumnIndex, sheet);
                        //lOCATOR_T.LocatorType = Int64.Parse(ExcelUtil.GetCellString(j, LocatorType_cell.ColumnIndex, sheet).Trim());
                        lOCATOR_T.LocatorSegments = ExcelUtil.GetStringCellValue(j, LocatorSegments_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.LocatorDesc = ExcelUtil.GetStringCellValue(j, LocatorDesc_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment1 = ExcelUtil.GetStringCellValue(j, Segment1_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment2 = ExcelUtil.GetStringCellValue(j, Segment2_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment3 = ExcelUtil.GetStringCellValue(j, Segment3_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.Segment4 = ExcelUtil.GetStringCellValue(j, Segment4_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.ControlFlag = "";
                        lOCATOR_T.LocatorStatus = ExcelUtil.GetLongOrNullCellValue(j, LocatorStatus_cell.ColumnIndex, sheet);
                        lOCATOR_T.LocatorStatusCode = ExcelUtil.GetStringCellValue(j, LocatorStatusCode_cell.ColumnIndex, sheet).Trim();
                        lOCATOR_T.LocatorPickingOrder = ExcelUtil.GetLongOrNullCellValue(j, LocatorPickingOrder_cell.ColumnIndex, sheet);
                        lOCATOR_T.LocatorDisableDate = ExcelUtil.GetDateTimeOrNullCellValue(j, LocatorDisableDate_cell.ColumnIndex, sheet);
                        locatorTRepository.Create(lOCATOR_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImprotRelated(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_OSP_RELATED_ITEM_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell Related_id_cell = null;
            ICell InventoryItemId_cell = null;
            ICell ItemNumber_cell = null;
            ICell ItemDescription_cell = null;
            ICell RelatedItemId_cell = null;
            ICell RelatedItemNumber_cell = null;
            ICell RelatedItemDescription_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            //Related_id_cell = ExcelUtil.FindCell("RELATED_ITEM_ID", sheet);
            //if (Related_id_cell == null)
            //{
            //    throw new Exception("找不到RELATED_ITEM_ID欄位");
            //}
            ItemNumber_cell = ExcelUtil.FindCell("ITEM_NUMBER", sheet);
            if (ItemNumber_cell == null)
            {
                throw new Exception("找不到ITEM_NUMBER欄位");
            }

            InventoryItemId_cell = ExcelUtil.FindCell("INVENTORY_ITEM_ID", sheet);
            if (InventoryItemId_cell == null)
            {
                throw new Exception("找不到INVENTORY_ITEM_ID欄位");
            }

            ItemDescription_cell = ExcelUtil.FindCell("ITEM_DESCRIPTION", sheet);
            if (ItemDescription_cell == null)
            {
                throw new Exception("找不到ITEM_DESCRIPTION欄位");
            }
            RelatedItemNumber_cell = ExcelUtil.FindCell("RELATED_ITEM_NUMBER", sheet);
            if (RelatedItemNumber_cell == null)
            {
                throw new Exception("找不到RELATED_ITEM_NUMBER欄位");
            }
            RelatedItemId_cell = ExcelUtil.FindCell("RELATED_ITEM_ID", sheet);
            if (RelatedItemId_cell == null)
            {
                throw new Exception("找不到RELATED_ITEM_ID欄位");
            }

            RelatedItemDescription_cell = ExcelUtil.FindCell("RELATED_ITEM_DESCRIPTION", sheet);
            if (RelatedItemDescription_cell == null)
            {
                throw new Exception("找不到RELATED_ITEM_DESCRIPTION欄位");
            }

            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATE_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATE_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }

            for (int j = InventoryItemId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var id = Int64.Parse(ExcelUtil.GetStringCellValue(j, InventoryItemId_cell.ColumnIndex, sheet).Trim());
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<RELATED_T>().Where(x => x.Entity.InventoryItemId == id).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = relatedTRepository.Get(x => x.InventoryItemId == id).FirstOrDefault();
                    if (org == null || org.Entity.InventoryItemId <= 0)
                    {
                        RELATED_T rELATED_T = new RELATED_T();
                        rELATED_T.InventoryItemId = ExcelUtil.GetLongCellValue(j, InventoryItemId_cell.ColumnIndex, sheet);
                        rELATED_T.ItemNumber = ExcelUtil.GetStringCellValue(j, ItemNumber_cell.ColumnIndex, sheet).Trim();
                        rELATED_T.ItemDescription = ExcelUtil.GetStringCellValue(j, ItemDescription_cell.ColumnIndex, sheet).Trim();
                        rELATED_T.RelatedItemId = ExcelUtil.GetLongCellValue(j, RelatedItemId_cell.ColumnIndex, sheet);
                        rELATED_T.RelatedItemNumber = ExcelUtil.GetStringCellValue(j, RelatedItemNumber_cell.ColumnIndex, sheet).Trim();
                        rELATED_T.RelatedItemDescription = ExcelUtil.GetStringCellValue(j, RelatedItemDescription_cell.ColumnIndex, sheet).Trim();
                        rELATED_T.CreatedBy = ExcelUtil.GetLongCellValue(j, CreatedBy_cell.ColumnIndex, sheet);
                        rELATED_T.CreationDate = ExcelUtil.GetDateTimeCellValue(j, CreationDate_cell.ColumnIndex, sheet);
                        rELATED_T.LastUpdateBy = ExcelUtil.GetLongCellValue(j, LastUpdateBy_cell.ColumnIndex, sheet);
                        rELATED_T.LastUpdateDate = ExcelUtil.GetDateTimeCellValue(j, LastUpdateDate_cell.ColumnIndex, sheet);
                        rELATED_T.ControlFlag = "";
                        relatedTRepository.Create(rELATED_T);

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImportYszmpckq(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCOM_YSZMPCKQ_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell OrganizationId_cell = null;
            ICell OrganizationCode_cell = null;
            ICell OspSubinventory_cell = null;
            ICell Pstyp_cell = null;
            ICell Bwetup_cell = null;
            ICell Bwetdn_cell = null;
            ICell Rwtup_cell = null;
            ICell Rwtdn_cell = null;
            ICell Pckq_cell = null;
            ICell PaperQty_cell = null;
            ICell PiecesQty_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            OrganizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (OrganizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }
            OrganizationCode_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (OrganizationCode_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

            OspSubinventory_cell = ExcelUtil.FindCell("OSP_SUBINVENTORY", sheet);
            if (OspSubinventory_cell == null)
            {
                throw new Exception("找不到OSP_SUBINVENTORY欄位");
            }
            Pstyp_cell = ExcelUtil.FindCell("PSTYP", sheet);
            if (Pstyp_cell == null)
            {
                throw new Exception("找不到PSTYP欄位");
            }
            Bwetup_cell = ExcelUtil.FindCell("BWETUP", sheet);
            if (Bwetup_cell == null)
            {
                throw new Exception("找不到BWETUP欄位");
            }
            Bwetdn_cell = ExcelUtil.FindCell("BWETDN", sheet);
            if (Bwetdn_cell == null)
            {
                throw new Exception("找不到BWETDN欄位");
            }
            Rwtup_cell = ExcelUtil.FindCell("RWTUP", sheet);
            if (Rwtup_cell == null)
            {
                throw new Exception("找不到RWTUP欄位");
            }
            Rwtdn_cell = ExcelUtil.FindCell("RWTDN", sheet);
            if (Rwtdn_cell == null)
            {
                throw new Exception("找不到RWTDN欄位");
            }
            Pckq_cell = ExcelUtil.FindCell("PCKQ", sheet);
            if (Pckq_cell == null)
            {
                throw new Exception("找不到PCKQ欄位");
            }
            PaperQty_cell = ExcelUtil.FindCell("PAPER_QTY", sheet);
            if (PaperQty_cell == null)
            {
                throw new Exception("找不到PAPER_QTY欄位");
            }
            PiecesQty_cell = ExcelUtil.FindCell("PIECES_QTY", sheet);
            if (PiecesQty_cell == null)
            {
                throw new Exception("找不到PIECES_QTY欄位");
            }

            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATE_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATE_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }

            for (int j = OrganizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    //var id = Int64.Parse(ExcelUtil.GetCellString(j, OrganizationId_cell.ColumnIndex, sheet).Trim());
                    //var org = YszmpckqTRepository.Get(x => x.InventoryItemId == id);
                    //if (org == null || org.InventoryItemId <= 0)
                    //{
                    YSZMPCKQ_T ySZMPCKQ_T = new YSZMPCKQ_T();
                    ySZMPCKQ_T.OrganizationId = ExcelUtil.GetLongCellValue(j, OrganizationId_cell.ColumnIndex, sheet);
                    ySZMPCKQ_T.OrganizationCode = ExcelUtil.GetStringCellValue(j, OrganizationCode_cell.ColumnIndex, sheet).Trim();
                    ySZMPCKQ_T.OspSubinventory = ExcelUtil.GetStringCellValue(j, OspSubinventory_cell.ColumnIndex, sheet).Trim();
                    ySZMPCKQ_T.Pstyp = ExcelUtil.GetStringCellValue(j, Pstyp_cell.ColumnIndex, sheet).Trim();
                    ySZMPCKQ_T.Bwetup = ExcelUtil.GetDecimalCellValue(j, Bwetup_cell.ColumnIndex, sheet, 0m);
                    ySZMPCKQ_T.Bwetdn = ExcelUtil.GetDecimalCellValue(j, Bwetdn_cell.ColumnIndex, sheet, 0m);
                    ySZMPCKQ_T.Rwtup = ExcelUtil.GetDecimalCellValue(j, Rwtup_cell.ColumnIndex, sheet, 0m);
                    ySZMPCKQ_T.Rwtdn = ExcelUtil.GetDecimalCellValue(j, Rwtdn_cell.ColumnIndex, sheet, 0m);
                    ySZMPCKQ_T.Pckq = ExcelUtil.GetLongCellValue(j, Pckq_cell.ColumnIndex, sheet, 0);
                    ySZMPCKQ_T.PaperQty = ExcelUtil.GetLongCellValue(j, PaperQty_cell.ColumnIndex, sheet, 0);
                    ySZMPCKQ_T.PiecesQty = ExcelUtil.GetLongCellValue(j, PiecesQty_cell.ColumnIndex, sheet, 0);
                    ySZMPCKQ_T.CreatedBy = ExcelUtil.GetLongCellValue(j, CreatedBy_cell.ColumnIndex, sheet);
                    ySZMPCKQ_T.CreationDate = ExcelUtil.GetDateTimeCellValue(j, CreationDate_cell.ColumnIndex, sheet);
                    ySZMPCKQ_T.LastUpdateBy = ExcelUtil.GetLongCellValue(j, LastUpdateBy_cell.ColumnIndex, sheet);
                    ySZMPCKQ_T.LastUpdateDate = ExcelUtil.GetDateTimeCellValue(j, LastUpdateDate_cell.ColumnIndex, sheet);
                    ySZMPCKQ_T.ControlFlag = "";
                    yszmpckqTRepository.Create(ySZMPCKQ_T);
                    //}
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImprotMachinePaperType(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCPO_MACHINE_PAPER_TYPE_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell OrganizationId_cell = null;
            ICell OrganizationCode_cell = null;
            ICell MachineCode_cell = null;
            ICell MachineMeaning_cell = null;
            ICell Description_cell = null;
            ICell PaperType_cell = null;
            ICell MachineNum_cell = null;
            ICell SupplierNum_cell = null;
            ICell SupplierName_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            OrganizationId_cell = ExcelUtil.FindCell("ORGANIZATION_ID", sheet);
            if (OrganizationId_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_ID欄位");
            }
            OrganizationCode_cell = ExcelUtil.FindCell("ORGANIZATION_CODE", sheet);
            if (OrganizationCode_cell == null)
            {
                throw new Exception("找不到ORGANIZATION_CODE欄位");
            }

            MachineCode_cell = ExcelUtil.FindCell("MACHINE_CODE", sheet);
            if (MachineCode_cell == null)
            {
                throw new Exception("找不到MACHINE_CODE欄位");
            }
            MachineMeaning_cell = ExcelUtil.FindCell("MACHINE_MEANING", sheet);
            if (MachineMeaning_cell == null)
            {
                throw new Exception("找不到MACHINE_MEANING欄位");
            }
            Description_cell = ExcelUtil.FindCell("DESCRIPTION", sheet);
            if (Description_cell == null)
            {
                throw new Exception("找不到DESCRIPTION欄位");
            }
            PaperType_cell = ExcelUtil.FindCell("PAPER_TYPE", sheet);
            if (PaperType_cell == null)
            {
                throw new Exception("找不到PAPER_TYPE欄位");
            }
            MachineNum_cell = ExcelUtil.FindCell("MACHINE_NUM", sheet);
            if (MachineNum_cell == null)
            {
                throw new Exception("找不到MACHINE_NUM欄位");
            }
            SupplierNum_cell = ExcelUtil.FindCell("SUPPLIER_NUM", sheet);
            if (SupplierNum_cell == null)
            {
                throw new Exception("找不到SUPPLIER_NUM欄位");
            }
            SupplierName_cell = ExcelUtil.FindCell("VENDOR_NAME", sheet);
            if (SupplierName_cell == null)
            {
                throw new Exception("找不到VENDOR_NAME欄位");
            }
            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATED_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATION_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATED_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }

            for (int j = OrganizationId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var MachineCode = ExcelUtil.GetStringCellValue(j, MachineCode_cell.ColumnIndex, sheet).Trim();
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<MACHINE_PAPER_TYPE_T>().Where(x => x.Entity.MachineCode == MachineCode).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = machinePaperTypeRepository.Get(x => x.MachineCode == MachineCode).FirstOrDefault();
                    if (org == null || string.IsNullOrEmpty(org.Entity.MachineCode))
                    {
                        MACHINE_PAPER_TYPE_T mACHINE_PAPER_TYPE_T = new MACHINE_PAPER_TYPE_T();
                        mACHINE_PAPER_TYPE_T.OrganizationId = ExcelUtil.GetLongCellValue(j, OrganizationId_cell.ColumnIndex, sheet);
                        mACHINE_PAPER_TYPE_T.OrganizationCode = ExcelUtil.GetStringCellValue(j, OrganizationCode_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.MachineCode = ExcelUtil.GetStringCellValue(j, MachineCode_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.MachineMeaning = ExcelUtil.GetStringCellValue(j, MachineMeaning_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.Description = ExcelUtil.GetStringCellValue(j, Description_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.PaperType = ExcelUtil.GetStringCellValue(j, PaperType_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.MachineNum = ExcelUtil.GetStringCellValue(j, MachineNum_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.SupplierNum = ExcelUtil.GetStringCellValue(j, SupplierNum_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.SupplierName = ExcelUtil.GetStringCellValue(j, SupplierName_cell.ColumnIndex, sheet).Trim();
                        mACHINE_PAPER_TYPE_T.CreatedBy = ExcelUtil.GetLongCellValue(j, CreatedBy_cell.ColumnIndex, sheet);
                        mACHINE_PAPER_TYPE_T.CreationDate = ExcelUtil.GetDateTimeCellValue(j, CreationDate_cell.ColumnIndex, sheet);
                        mACHINE_PAPER_TYPE_T.LastUpdateBy = ExcelUtil.GetLongCellValue(j, LastUpdateBy_cell.ColumnIndex, sheet);
                        mACHINE_PAPER_TYPE_T.LastUpdateDate = ExcelUtil.GetDateTimeCellValue(j, LastUpdateDate_cell.ColumnIndex, sheet);
                        mACHINE_PAPER_TYPE_T.ControlFlag = "";
                        machinePaperTypeRepository.Create(mACHINE_PAPER_TYPE_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImprotTransaction(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "XXCINV_TRANSACTION_TYPE_V");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell TransactionTypeId_cell = null;
            ICell TransactionTypeName_cell = null;
            ICell Description_cell = null;
            ICell TransactionActionId_cell = null;
            ICell TransactionActionName_cell = null;
            ICell TransactionSourceTypeId_cell = null;
            ICell TransactionSourceTypeName_cell = null;
            ICell CreatedBy_cell = null;
            ICell CreationDate_cell = null;
            ICell LastUpdateBy_cell = null;
            ICell LastUpdateDate_cell = null;

            TransactionTypeId_cell = ExcelUtil.FindCell("TRANSACTION_TYPE_ID", sheet);
            if (TransactionTypeId_cell == null)
            {
                throw new Exception("找不到TRANSACTION_TYPE_ID欄位");
            }
            TransactionTypeName_cell = ExcelUtil.FindCell("TRANSACTION_TYPE_NAME", sheet);
            if (TransactionTypeName_cell == null)
            {
                throw new Exception("找不到TRANSACTION_TYPE_NAME欄位");
            }
            Description_cell = ExcelUtil.FindCell("DESCRIPTION", sheet);
            if (Description_cell == null)
            {
                throw new Exception("找不到DESCRIPTION欄位");
            }
            TransactionActionId_cell = ExcelUtil.FindCell("TRANSACTION_ACTION_ID", sheet);
            if (TransactionActionId_cell == null)
            {
                throw new Exception("找不到TRANSACTION_ACTION_ID欄位");
            }
            TransactionActionName_cell = ExcelUtil.FindCell("TRANSACTION_ACTION_NAME", sheet);
            if (TransactionActionName_cell == null)
            {
                throw new Exception("找不到TRANSACTION_ACTION_NAME欄位");
            }

            TransactionSourceTypeId_cell = ExcelUtil.FindCell("TRANSACTION_SOURCE_TYPE_ID", sheet);
            if (TransactionSourceTypeId_cell == null)
            {
                throw new Exception("找不到TRANSACTION_SOURCE_TYPE_ID欄位");
            }
            TransactionSourceTypeName_cell = ExcelUtil.FindCell("TRANSACTION_SOURCE_TYPE_NAME", sheet);
            if (TransactionSourceTypeName_cell == null)
            {
                throw new Exception("找不到TRANSACTION_SOURCE_TYPE_NAME欄位");
            }
            CreatedBy_cell = ExcelUtil.FindCell("CREATED_BY", sheet);
            if (CreatedBy_cell == null)
            {
                throw new Exception("找不到CREATED_BY欄位");
            }
            CreationDate_cell = ExcelUtil.FindCell("CREATION_DATE", sheet);
            if (CreationDate_cell == null)
            {
                throw new Exception("找不到CREATION_DATE欄位");
            }
            LastUpdateBy_cell = ExcelUtil.FindCell("LAST_UPDATED_BY", sheet);
            if (LastUpdateBy_cell == null)
            {
                throw new Exception("找不到LAST_UPDATED_BY欄位");
            }
            LastUpdateDate_cell = ExcelUtil.FindCell("LAST_UPDATE_DATE", sheet);
            if (LastUpdateDate_cell == null)
            {
                throw new Exception("找不到LAST_UPDATE_DATE欄位");
            }


            for (int j = TransactionTypeId_cell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var TransactionTypeId = Int64.Parse(ExcelUtil.GetStringCellValue(j, TransactionTypeId_cell.ColumnIndex, sheet).Trim());
                    //搜尋未執行 SaveChanges 的資料
                    var org = this.Context.ChangeTracker.Entries<TRANSACTION_TYPE_T>().Where(x => x.Entity.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    //搜尋已執行 SaveChanges 的資料
                    //var org = transactionTypeRepository.Get(x => x.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    if (org == null || org.Entity.TransactionTypeId <= 0)
                    {
                        TRANSACTION_TYPE_T tRANSACTION_TYPE_T = new TRANSACTION_TYPE_T();
                        tRANSACTION_TYPE_T.TransactionTypeId = ExcelUtil.GetLongCellValue(j, TransactionTypeId_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.TransactionTypeName = ExcelUtil.GetStringCellValue(j, TransactionTypeName_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.Description = ExcelUtil.GetStringCellValue(j, Description_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.TransactionActionId = ExcelUtil.GetLongCellValue(j, TransactionActionId_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.TransactionActionName = ExcelUtil.GetStringCellValue(j, TransactionActionName_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.TransactionSourceTypeId = ExcelUtil.GetLongCellValue(j, TransactionSourceTypeId_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.TransactionSourceTypeName = ExcelUtil.GetStringCellValue(j, TransactionSourceTypeName_cell.ColumnIndex, sheet).Trim();
                        tRANSACTION_TYPE_T.CreatedBy = ExcelUtil.GetLongCellValue(j, CreatedBy_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.CreationDate = ExcelUtil.GetDateTimeCellValue(j, CreationDate_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.LastUpdateBy = ExcelUtil.GetLongCellValue(j, LastUpdateBy_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.LastUpdateDate = ExcelUtil.GetDateTimeCellValue(j, LastUpdateDate_cell.ColumnIndex, sheet);
                        tRANSACTION_TYPE_T.ControlFlag = "";
                        transactionTypeRepository.Create(tRANSACTION_TYPE_T);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }

        private void ImportReason(IWorkbook book)
        {
            ISheet sheet = FindSheet(book, "貨故原因");

            if (sheet == null) return;

            var noOfRow = sheet.LastRowNum;
            ICell codeCell = null;
            ICell descCell = null;

            codeCell = ExcelUtil.FindCell("代碼", sheet);
            if (codeCell == null)
            {
                throw new Exception("找不到代碼欄位");
            }
            descCell = ExcelUtil.FindCell("說明", sheet);
            if (descCell == null)
            {
                throw new Exception("找不到說明欄位");
            }

            for (int j = codeCell.RowIndex + 1; j <= noOfRow; j++)
            {
                try
                {
                    var adm = this.appUserRepository.Get(x => x.UserName.CompareTo("adm") == 0).FirstOrDefault();
                    var code = ExcelUtil.GetStringCellValue(j, codeCell.ColumnIndex, sheet);
                    var desc = ExcelUtil.GetStringCellValue(j, descCell.ColumnIndex, sheet);

                    var reason = stkReasonTRepository.Get(x => x.ReasonCode == code).FirstOrDefault();

                    //搜尋已執行 SaveChanges 的資料
                    //var org = transactionTypeRepository.Get(x => x.TransactionTypeId == TransactionTypeId).FirstOrDefault();
                    if (reason == null && string.IsNullOrEmpty(reason.ReasonCode) && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(desc))
                    {
                        var now = DateTime.Now;
                        STK_REASON_T stkReasonT = new STK_REASON_T();
                        stkReasonT.ReasonCode = code;
                        stkReasonT.ReasonDesc = desc;
                        stkReasonT.CreatedBy = adm.Id;
                        stkReasonT.CreationDate = now;
                        stkReasonT.LastUpdateBy = adm.Id;
                        stkReasonT.LastUpdateDate = now;
                        stkReasonTRepository.Create(stkReasonT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.SaveChanges();
        }


        public ISheet FindSheet(IWorkbook book, string name)
        {
            ISheet sheet = null;

            if (book.NumberOfSheets == 0)
            {
                return sheet;
            }

            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                //獲取工作表(GetSheetAt)
                if (!book.GetSheetAt(i).SheetName.Contains(name))
                {
                    continue;
                }
                sheet = book.GetSheetAt(i);
            }
            return sheet;
        }


        #endregion 測試資料產生
    }
}