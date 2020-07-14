using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class STOCK_T
    {
        public long STOCK_ID { set; get; }
        public long ORGANIZATION_ID { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public long LOCATOR_ID { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_NO { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
        public string ITEM_CATEGORY { set; get; }
        public string LOT_NUMBER { set; get; }
        public string BARCODE { set; get; }
        public string PRIMARY_UOM_CODE { set; get; }
        public decimal PRIMARY_TRANSACTION_QTY { set; get; }
        public decimal PRIMARY_AVAILABLE_QTY { set; get; }
        public string SECONDARY_UOM_CODE { set; get; }
        public decimal SECONDARY_TRANSACTION_QTY { set; get; }
        public decimal SECONDARY_AVAILABLE_QTY { set; get; } 
        public string REASON_CODE { set; get; }
        public string REASON_DESC { set; get; }
        public string STATUS_CODE { set; get; }
        public long CREATE_BY { set; get; }
        public DateTime? CREATE_DATE { set; get; }
        public long LAST_UPDATE_BY { set; get; }
        public DateTime? LAST_UPDATE_DATE { get; set; }

    }

   
}