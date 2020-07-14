using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class Cotangent
    {
        [Display(Name = "加工明細ID")] //OSP_BATCH_ID
        public int Process_Detail_Id { set; get; }

        [Display(Name = "餘切id")]
        public int Cotangent_Id { set; get; }

        [Display(Name = "餘切條碼")]
        public string Barcode { set; get; }

        [Display(Name = "餘切令數")]
        public string Cotangent_Ttl_Roll_Ream { set; get; }

        [Display(Name = "餘切料號")]
        public string Related_item { set; get; }

        [Display(Name = "公斤")]
        public string Kg { set; get; }

        [Display(Name = "狀態")]
        public string Status { set; get; }

    }

}