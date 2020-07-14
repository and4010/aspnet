using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class InBoundDT
    {
        public long ID { get; set; }


        [Display(Name = "出貨編號")]
        public string SHIPMENT_NUMBER { get; set; }

        [Display(Name = "移轉編號")]
        public string SUBINVENTORY_TRANSFER_NUMBER { get; set; }

        [Display(Name = "條碼")]
        public string BARCODE { get; set; }

        [Display(Name = "料號")]
        public string ITEM_NUMBER { get; set; }

        [Display(Name = "捲號")]
        public string LOT_NUMBER { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { get; set; }
        [Display(Name = "數量")] //主要數量
        public decimal PRIMARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //主要單位
        public string PRIMARY_UOM { get; set; }

        [Display(Name = "數量")] //次要數量
        public decimal SECONDARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //次要單位
        public string SECONDARY_UOM { get; set; }

        [Display(Name = "備註")]
        public string REMARK { get; set; }

        [Display(Name = "建立人員ID")]
        public long CREATED_BY { get; set; }
        [Display(Name = "建立人員名稱")]
        public string CREATE_BY_USERNAME { set; get; }
        [Display(Name = "建立日期")]
        public DateTime CREATION_DATE { get; set; }


        [Display(Name = "更新人員ID")]
        public long LAST_UPDATED_BY { get; set; }
        [Display(Name = "更新人員名稱")]
        public long LAST_UPDATED_BY_USERNAME { get; set; }
        [Display(Name = "更新日期")]
        public DateTime LAST_UPDATE_DATE { get; set; }
    }


    public class InBoundData
    {
        public static List<InBoundDT> model = new List<InBoundDT>();

        public static void resetData()
        {
            model.Clear();
        }

    }
}