using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class PartNoModel
    {

        public int Id { get; set; }

        [Display(Name = "庫存組織CODE")]
        public long Organization_code { set; get; } 

        [Display(Name = "存貨分類")]
        public string Category_code_inv { set; get; }

        [Display(Name = "存貨分類摘要")]
        public string Category_name_inv { set; get; }

        [Display(Name = "成本分類")]
        public string Category_code_cost { set; get; }

        [Display(Name = "成本分類摘要")]
        public string Category_name_cost { set; get; }

        [Display(Name = "控制分類")]
        public string Category_code_control { set; get; }

        [Display(Name = "控制分類摘要")]
        public string Category_name_control { set; get; }

        [Display(Name = "料號")]
        public string Item_number { set; get; }

        [Display(Name = "料號ID")]
        public long Inventory_item_id { set; get; }

        [Display(Name = "英文摘要")]
        public string Item_desc_eng { set; get; }

        [Display(Name = "簡體中文摘要")]
        public string Item_desc_sch { set; get; }

        [Display(Name = "繁體中文摘要")]
        public string Item_desc_tch { set; get; }

        [Display(Name = "主要單位")]
        public string Primary_uom_code { set; get; }

        [Display(Name = "次要單位")]
        public string Secondary_uom_code { set; get; }

        [Display(Name = "料號狀態")]
        public string Inventory_item_status_code { set; get; }

        [Display(Name = "料號型態")]
        public string Item_type { set; get; }

        [Display(Name = "大紙別")]
        public string Catalog_elem_val_010 { set; get; }

        [Display(Name = "紙別代碼")]
        public string Catalog_elem_val_020 { set; get; }

        [Display(Name = "料件等級")]
        public string Catalog_elem_val_030 { set; get; }

        [Display(Name = "基重")]
        public string Catalog_elem_val_040 { set; get; }

        [Display(Name = "規格")]
        public string Catalog_elem_val_050 { set; get; }

        [Display(Name = "令重")]
        public string Catalog_elem_val_060 { set; get; }

        [Display(Name = "捲筒/平版")]
        public string Catalog_elem_val_070 { set; get; }

        [Display(Name = "FSC")]
        public string Catalog_elem_val_080 { set; get; }

        [Display(Name = "市場常規")]
        public string Catalog_elem_val_090 { set; get; }

        [Display(Name = "絲向")]
        public string Catalog_elem_val_100 { set; get; }

        [Display(Name = "令包/無令打件")]
        public string Catalog_elem_val_110 { set; get; }

        [Display(Name = "市銷/常規")]
        public string Catalog_elem_val_120 { set; get; }

        [Display(Name = "紙別中文名稱")]
        public string Catalog_elem_val_130 { set; get; }

        [Display(Name = "紙別英文名稱")]
        public string Catalog_elem_val_140 { set; get; }

        [Display(Name = "建立人員ID")]
        public long Created_by { set; get; }

        [Display(Name = "建立日期")]
        public DateTime Creation_Date { set; get; }

        [Display(Name = "最後更新人員ID")]
        public long Last_Updated_by { set; get; }

        [Display(Name = "最後更新日期")]
        public DateTime? Last_Update_Date { set; get; }
    }
}