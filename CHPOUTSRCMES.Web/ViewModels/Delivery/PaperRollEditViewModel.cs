using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.Models.Delivery;

namespace CHPOUTSRCMES.Web.ViewModels.Delivery
{
    public class PaperRollEditViewModel
    {
        [Display(Name = "交運單號")]
        public string DELIVERY_NAME { get; set; }

        //[Display(Name = "航程號")]
        //public string TRIP_NAME { get; set; }

        //[Display(Name = "訂單編號")]
        //public long ORDER_NUMBER { get; set; }

        //[Display(Name = "車次")]
        //public string TRIP_CAR { get; set; }

        //[Display(Name = "客戶名稱")]
        //public string CUSTOMER_NAME { get; set; }

        //[Display(Name = "送貨地點")]
        //public string CUSTOMER_LOCATION_CODE { get; set; }

        //[Display(Name = "送貨客戶名稱")]
        //public string SHIP_CUSTOMER_NAME { get; set; }

        //[Display(Name = "送貨客戶地點")]
        //public string SHIP_LOCATION_CODE { get; set; }

        //[Display(Name = "組車日")]
        //public String TRIP_ACTUAL_SHIP_DATE { get; set; }

        //[Display(Name = "備註")]
        //public string REMARK { get; set; }

        [Display(Name = "項次")]
        public string PaperRollEditDT_ID { get; set; }

        [Display(Name = "條碼號")]
        public string BARCODE { get; set; }

        [Display(Name = "訂單")]
        public string ORDER_NUMBER { get; set; }

        [Display(Name = "訂單行號")]
        public string ORDER_SHIP_NUMBER { get; set; }

        [Display(Name = "料號名稱")]
        public string ITEM_DESCRIPTION { get; set; }

        public DeliveryDetailViewHeader DeliveryDetailViewHeader { get; set; }
    }
}