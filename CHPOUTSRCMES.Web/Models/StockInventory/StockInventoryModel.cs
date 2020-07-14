using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.StockInventory
{
    public class StockInventoryModel
    {
        
        public long ID { set; get; }
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_NAME { set; get; }

        [Display(Name = "倉庫")]
        public string SUBINVENTORY_CODE { set; get; }
        public string SUBINVENTORY_NAME { set; get; }
        public long LOCATOR_ID { set; get; }
        public long LOCATOR_TYPE { set; get; }

        [Display(Name = "儲位")]
        public string SEGMENT3 { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_NO { set; get; }
        public string OSP_BATCH_NO { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
        public string ITEM_CATEGORY { set; get; }
        public string LOT_NUMBER { set; get; }
        public string BARCODE { set; get; }

        [Display(Name = "紙別")]
        public string PapaerType { get; set; }

        [Display(Name = "基重")]
        public string BasicWeight { get; set; }

        [Display(Name = "規格")]
        public string Specification { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { set; get; }

        [Display(Name = "主單位")]
        public string PRIMARY_UOM_CODE { set; get; }

        [Display(Name = "主交易數量")]
        public decimal PRIMARY_TRANSACTION_QTY { set; get; }

        [Display(Name = "主數量")]
        public decimal PRIMARY_AVAILABLE_QTY { set; get; }

        [Display(Name = "次要單位")]
        public string SECONDARY_UOM_CODE { set; get; }

        [Display(Name = "次要交易數量")]
        public decimal SECONDARY_TRANSACTION_QTY { set; get; }

        [Display(Name = "次要數量")]
        public decimal SECONDARY_AVAILABLE_QTY { set; get; }


        public string REASON_CODE { set; get; }
        public string REASON_DESC { set; get; }

        [Display(Name = "備註")]
        public string NOTE { set; get; }
        public string STATUS_CODE { set; get; }
        public long CREATE_BY { set; get; }
        public string CREATE_BY_USERNAME { set; get; }
        public string CREATE_DATE { set; get; }
        public long LAST_UPDATE_BY { set; get; }
        public string LAST_UPDATE_BY_USERNAME { set; get; }
        public string LAST_UPDATE_DATE { get; set; }

    }
}