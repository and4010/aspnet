using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class DeliveryHeader
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DELIVERY_HEADER_ID { set; get; }
        
        public long ORG_ID { set; get; }
        public string ORG_NAME { set; get; }
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_CODE { set; get; }
        public string TRIP_CAR { set; get; }
        public long TRIP_ID { set; get; }
        public string TRIP_NAME { set; get; }
        public DateTime TRIP_ACTUAL_SHIP_DATE { set; get; }
        public long DELIVERY_ID { set; get; }
        public string DELIVERY_NAME { set; get; }
        public long CUSTOMER_ID { set; get; }
        public string CUSTOMER_NUMBER { set; get; }
        public string CUSTOMER_NAME { set; get; }
        public string CUSTOMER_LOCATION_CODE { set; get; }
        public long SHIP_CUSTOMER_ID { set; get; }
        public string SHIP_CUSTOMER_NUMBER { set; get; }
        public string SHIP_CUSTOMER_NAME { set; get; }
        public string SHIP_LOCATION_CODE { set; get; }
        public string FREIGHT_TERMS_NAME { set; get; }
        public long ORDER_HEADER_ID { set; get; }
        public long ORDER_NUMBER { set; get; }
        public long ORDER_LINE_ID { set; get; }
        public string ORDER_SHIP_NUMBER { set; get; }
        public string DELIVERY_STATUS { set; get; }
        public long TRANSACTION_BY { set; get; }
        public DateTime TRANSACTION_DATE { set; get; }
        public long AUTHORIZE_BY { set; get; }
        public DateTime AUTHORIZE_DATE { set; get; }
        public string REMARK { set; get; }
        public long CREATED_BY { set; get; }
        public DateTime CREATION_DATE { set; get; }
        public long LAST_UPDATED_BY { set; get; }
        public DateTime LAST_UPDATE_DATE { set; get; }

    }

    
}