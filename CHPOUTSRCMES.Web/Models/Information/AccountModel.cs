using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class AccountModel
    {
        public int Id { get; set; }

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

        [Required]
        [Display(Name = "信箱")]
        [DataType(DataType.EmailAddress, ErrorMessage = "請輸入正確的電子郵件信箱")]
        public string Email { get; set; }

        [Display(Name = "狀態")]
        public string Status { get; set; }

        [Display(Name = "倉庫")]
        public List<SubinventoryDetail> Subinventory { get; set; }
    }

    public class SubinventoryDetail
    {
        //public string SubinventoryId { set; get; }
        public string SubinventoryName { set; get; }
    }
}