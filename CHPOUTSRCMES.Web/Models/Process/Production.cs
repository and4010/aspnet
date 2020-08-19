using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class Production
    {

        [Display(Name = "OspPickedOutId")]
        public long OspPickedOutId { set; get; }

        [Display(Name = "加工明細ID")]
        public long OspDetailOutId { set; get; }

        [Display(Name = "加工檔頭ID")]
        public long OspHeaderId { set; get; }

        [Display(Name = "庫存ID")]
        public long? StockId { set; get; }

        [Display(Name = "條碼編號")]
        public string Barcode { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "產出料號")]
        public string Product_Item { set; get; }

        [Display(Name = "基重")]
        public string BasicWeight { set; get; }

        [Display(Name = "寬幅")]
        public string Specification { set; get; }

        [Display(Name = "捲號")]
        public string LotNumber { set; get; }

        [Display(Name = "餘切")]
        public string Cotangent { set; get; }

        [Display(Name = "餘切ID")]
        public long Cotangent_Id { set; get; }

        public string PaperType { set; get; }

        /// <summary>
        /// 主要數量
        /// </summary>
        /// 
        public decimal PrimaryQuantity { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        public string PrimaryUom { set; get; }

        /// <summary>
        /// 次要數量
        /// </summary>
        /// 
        public decimal SecondaryQuantity { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        public string SecondaryUom { set; get; }

        [Display(Name = "狀態")]
        public string Status { set; get; }

        [Display(Name = "棧板數")]
        public string Roll_Ream_Qty { set; get; }

        [Display(Name = "每板令數")] //SECONDARY_QUANTITY
        public string Roll_Ream_Wt { set; get; }

    }
}