using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class Production
    {
        [Display(Name = "ID")]
        public int Production_Id { set; get; }

        [Display(Name = "加工明細ID")] //OSP_BATCH_ID
        public int Process_Detail_Id { set; get; }

        [Display(Name = "產出料號")]
        public string Product_Item { set; get; }

        [Display(Name = "棧板數")]
        public string Roll_Ream_Qty { set; get; }

        [Display(Name = "每板令數")] //SECONDARY_QUANTITY
        public string Roll_Ream_Wt { set; get; }

        [Display(Name = "餘切")]
        public string Cotangent { set; get; }

        [Display(Name = "餘切ID")]
        public int Cotangent_Id { set; get; }

        [Display(Name = "基重")]
        public string Basic_Weight { set; get; }

        [Display(Name = "寬幅")]
        public string Specification { set; get; }

        [Display(Name = "捲號")]
        public string Lot_Number { set; get; }

        [Display(Name = "條碼")]
        public string Barcode { set; get; }

        [Display(Name = "料號")]
        public string Item_No { set; get; }

        [Display(Name = "重量")] //PRIMARY_QUANTITY
        public string Weight { set; get; }

        [Display(Name = "狀態")]
        public string Status { set; get; }

        [Display(Name = "損耗量")]
        public string Loss { set; get; }

    }
}