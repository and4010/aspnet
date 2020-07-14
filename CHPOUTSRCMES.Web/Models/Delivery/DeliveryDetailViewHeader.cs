using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class DeliveryDetailViewHeader
    {
        public long TripDetailDT_ID { get; set; }

        [Display(Name = "交運單號")]
        public string DELIVERY_NAME { get; set; }

        [Display(Name = "航程號")]
        public string TRIP_NAME { get; set; }

        [Display(Name = "訂單編號")]
        public string ORDER_NUMBER { get; set; }

        [Display(Name = "車次")]
        public string TRIP_CAR { get; set; }

        [Display(Name = "客戶名稱")]
        public string CUSTOMER_NAME { get; set; }

        [Display(Name = "送貨地點")]
        public string CUSTOMER_LOCATION_CODE { get; set; }

        [Display(Name = "送貨客戶名稱")]
        public string SHIP_CUSTOMER_NAME { get; set; }

        [Display(Name = "送貨客戶地點")]
        public string SHIP_LOCATION_CODE { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "組車日")]
        public DateTime? TRIP_ACTUAL_SHIP_DATE { get; set; }

        [Display(Name = "備註")]
        public string REMARK { get; set; }

        [Display(Name = "狀態")]
        public string DELIVERY_STATUS { get; set; }
    }
}