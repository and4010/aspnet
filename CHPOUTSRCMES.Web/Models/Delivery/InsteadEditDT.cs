using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class InsteadEditDT
    {
        public long DELIVERY_HEADER_ID { get; set; }

        [Display(Name = "訂單編號")]
        public long ORDER_NUMBER { get; set; }
        [Display(Name = "工單號碼")]
        public string OSP_BATCH_NO { get; set; }
        [Display(Name = "料號名稱")]
        public string ITEM_DESCRIPTION { get; set; }
        [Display(Name = "代紙料號名稱")]
        public string TMP_ITEM_DESCRIPTION { get; set; }

        //[Display(Name = "令重")]
        //public string REAM_WEIGHT { get; set; }
        //[Display(Name = "包裝方式")]
        //public string PACKING_TYPE { get; set; }
        [Display(Name = "紙別")]
        public string PAPER_TYPE { get; set; }

        [Display(Name = "基重")]
        public string BASIC_WEIGHT { get; set; }

        [Display(Name = "規格")]
        public string SPECIFICATION { get; set; }


        [Display(Name = "需求數量")] //預計出庫量 主要數量
        public string REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //主單位已揀數合計
        public string PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //主單位
        public string REQUESTED_QUANTITY_UOM { get; set; }

        //[Display(Name = "需求數量")] //預計出庫輔數量 次要數量
        //public string REQUESTED_QUANTITY2 { get; set; }

        //[Display(Name = "已揀數量")] //出庫已揀輔數量
        //public string PICKED_QUANTITY2 { get; set; }

        //[Display(Name = "單位")] //輔單位
        //public string SRC_REQUESTED_QUANTITY_UOM2 { get; set; }

        //[Display(Name = "需求數量")] //訂單原始數量 交易數量
        //public string SRC_REQUESTED_QUANTITY { get; set; }

        //[Display(Name = "已揀數量")] //交易單位已揀數合計 由主單位已揀數合計 換算過來
        //public string SRC_PICKED_QUANTITY { get; set; }

        //[Display(Name = "單位")] //交易單位
        //public string SRC_REQUESTED_QUANTITY_UOM { get; set; }

        [Display(Name = "建立人員")]
        public long CREATED_BY { get; set; }
        [Display(Name = "建立日期")]
        public DateTime CREATION_DATE { get; set; }
        [Display(Name = "更新人員")]
        public long LAST_UPDATED_BY { get; set; }
        [Display(Name = "更新日期")]
        public DateTime LAST_UPDATE_DATE { get; set; }
    }
}