using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class AccountModel
    {
        public string Id { get; set; }

        [Display(Name = "角色ID")]
        public int RoleId { set; get; }


        [Display(Name = "角色")]
        public string RoleName { set; get; }

        [Required(ErrorMessage = "需輸入帳號")]
        [Display(Name = "帳號")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "帳號只能輸入英文和數字")]
        public string Account { get; set; }

        [Required(ErrorMessage = "需輸入密碼")]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required(ErrorMessage = "需輸入姓名")]
        [StringLength(6, ErrorMessage = "只能輸入{1}")]
        public string Name { get; set; }


        [Display(Name = "信箱")]
        [DataType(DataType.EmailAddress, ErrorMessage = "請輸入正確的電子郵件信箱")]
        public string Email { get; set; }

        [Display(Name = "狀態")]
        public string Status { get; set; }

        [Display(Name = "倉庫")]
        public List<UserSubinventory> Subinventory { get; set; }


        /// <summary>
        /// View使用
        /// </summary>
        public List<UserSubinventory> GetSubinventories { set; get; }
        public IEnumerable<SelectListItem> GetRoleNameList { set; get; }
    }

    public class UserSubinventory
    {
        public string ORGANIZATION_CODE { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public string SUBINVENTORY_NAME { set; get; }
        public long ORGANIZATIONID { set; get; }
    }

    public class SubinventoryDetail
    {
        //public string SubinventoryId { set; get; }
        public string SubinventoryName { set; get; }
    }
        
}