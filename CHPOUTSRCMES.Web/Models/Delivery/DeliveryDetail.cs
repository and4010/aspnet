using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class DeliveryDetail
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DELIVERY_DETAIL_ID { set; get; }


        public long DELIVERY_HEADER_ID { set; get; }
        public string PACKING_TYPE { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_NUMBER { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
        public string PAPER_TYPE { set; get; }
        public string BASIC_WEIGHT { set; get; }
        public string SPECIFICATION { set; get; }
        public string GRAIN_DIRECTION { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public long LOCATOR_ID { set; get; }
        public string LOCATOR_CODE { set; get; }
        public decimal SRC_REQUESTED_QUANTITY { set; get; }
        public string SRC_REQUESTED_QUANTITY_UOM { set; get; }
        public decimal REQUESTED_QUANTITY { set; get; }
        public string REQUESTED_QUANTITY_UOM { set; get; }
        public decimal REQUESTED_QUANTITY2 { set; get; }
        public string SRC_REQUESTED_QUANTITY_UOM2 { set; get; }
        public long OSP_BATCH_ID { set; get; }
        public string OSP_BATCH_NO { set; get; }
        public string OSP_BATCH_TYPE { set; get; }
        public long TMP_INVENTORY_ITEM_ID { set; get; }
        public string TMP_ITEM_NUMBER { set; get; }
        public string TMP_ITEM_DESCRIPTION { set; get; }
        public long CREATED_BY { set; get; }
        public DateTime CREATION_DATE { set; get; }
        public long LAST_UPDATED_BY { set; get; }
        public DateTime LAST_UPDATE_DATE { set; get; }

    }
}