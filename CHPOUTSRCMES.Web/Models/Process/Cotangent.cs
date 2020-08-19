using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class Cotangent
    {
        [Display(Name = "加工產出檢貨ID")]
        public long OspDetailOutId { set; get; }

        [Display(Name = "餘切id")]
        public long OspCotangentId { set; get; }

        public long OspHeaderId { set; get; }

        [Display(Name = "餘切條碼")]
        public string Barcode { set; get; }

        [Display(Name = "餘切令數")]
        public decimal SecondaryQuantity { set; get; }

        public string SecondaryUom { set; get; }

        [Display(Name = "餘切料號")]
        public string Related_item { set; get; }

        [Display(Name = "重量")]
        public decimal PrimaryQuantity { set; get; }

        public string PrimaryUom { set; get; }

        public long InventoryItemId { set; get; }

        [Display(Name = "狀態")]
        public string Status { set; get; }

    }

}