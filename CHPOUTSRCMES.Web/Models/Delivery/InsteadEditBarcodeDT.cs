using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class InsteadEditBarcodeDT
    {
        public long PICKED_ID { get; set; }

        [Display(Name = "料號名稱")]
        public string ITEM_DESCRIPTION { get; set; }

        [Display(Name = "條碼號")]
        public string BARCODE { get; set; }

        [Display(Name = "數量")] //主要數量
        public string PRIMARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //主要單位
        public string PRIMARY_UOM { get; set; }
    }
}