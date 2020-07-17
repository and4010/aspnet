using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class MachinePaperType
    {

        [Display(Name = "庫存組織ID")]
        public int Organization_id { set; get; }

        [Display(Name = "庫存組織")]
        public string Organization_code { set; get; }

        [Display(Name = "機台紙別代碼")]
        public string Machine_code { set; get; }

        [Display(Name = "機台紙別意義")]
        public string Machine_meaning { set; get; }

        [Display(Name = "機台紙別摘要")]
        public string Description { set; get; }

        [Display(Name = "紙別")]
        public string Paper_type { set; get; }

        [Display(Name = "機台")]
        public string Machine_num { set; get; }

        [Display(Name = "供應商編號")]
        public string Supplier_num { set; get; }

        [Display(Name = "供應商名稱")]
        public string Supplier_name { set; get; }

        [Display(Name = "建立人員ID")]
        public string Created_by { set; get; }

        [Display(Name = "建立日期")]
        public DateTime Creation_date { set; get; }

        [Display(Name = "最後更新人員ID")]
        public string Last_updated_by { set; get; }

        [Display(Name = "最後更新日期")]
        public DateTime Last_update_date { set; get; }
    }
}