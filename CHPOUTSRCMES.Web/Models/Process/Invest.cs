using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class Invest
    {

        [Display(Name = "ID")] 
        public int Invest_Id { set; get; }

        [Display(Name = "加工明細ID")] //OSP_BATCH_ID
        public int Process_Detail_Id { set; get; }

        [Display(Name = "條碼編號")]
        public string Barcode { set; get; }

        [Display(Name = "殘捲")]
        public string Remnant { set; get; }

        [Display(Name = "原重量")]
        public string Original_Weight { set; get; }

        [Display(Name = "餘重(KG)")]
        public string Remaining_Weight { set; get; }

        [Display(Name = "基重")]
        public string Basic_Weight { set; get; }

        [Display(Name = "寬幅")]
        public string Specification { set; get; }

        [Display(Name = "捲號")]
        public string Lot_Number { set; get; }

        [Display(Name = "紙別")]
        public string Paper_Type { get; set; }

        [Display(Name = "料號")]
        public string Item_No { set; get; }

        [Display(Name = "令數")] //REAM_WT
        public string Ream_Qty { set; get; }



    }
}