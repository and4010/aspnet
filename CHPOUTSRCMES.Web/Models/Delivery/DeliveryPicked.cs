using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class DeliveryPicked
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PICKED_ID { set; get; }

        public long DELIVERY_DETAIL_ID { set; get; }
        public string STATUS { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string BARCODE { set; get; }
        public decimal PRIMARY_QUANTITY { set; get; }
        public string PRIMARY_UOM { set; get; }
        public decimal SECONDARY_QUANTITY { set; get; }
        public string SECONDARY_UOM { set; get; }
        public string LOT_NUMBER { set; get; }
        public decimal LOT_QUANTITY { set; get; }
        public long LOCATOR_ID { set; get; }
        public string LOCATOR_CODE { set; get; }
        public long CREATED_BY { set; get; }
        public DateTime CREATION_DATE { set; get; }
        public long LAST_UPDATED_BY { set; get; }
        public DateTime LAST_UPDATE_DATE { set; get; }
    }
}