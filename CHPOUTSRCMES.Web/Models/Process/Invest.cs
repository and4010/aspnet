using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class Invest
    {

        [Display(Name = "OspPickedInId")] 
        public long OspPickedInId { set; get; }

        [Display(Name = "加工明細ID")]
        public long OspDetailInId { set; get; }

        [Display(Name = "加工檔頭ID")]
        public long OspHeaderId { set; get; }

        [Display(Name = "庫存ID")]
        public long StockId { set; get; }

        [Display(Name = "條碼編號")]
        public string Barcode { set; get; }

        [Display(Name = "殘捲")]
        public string HasRemaint { set; get; }

        [Display(Name = "原重量")]
        public decimal PrimaryQuantity { set; get; }

        [Display(Name = "餘重(KG)")]
        public decimal? RemainingQuantity { set; get; }

        [Display(Name = "基重")]
        public string BasicWeight { set; get; }

        [Display(Name = "規格")]
        public string Specification { set; get; }

        [Display(Name = "捲號")]
        public string LotNumber { set; get; }

        [Display(Name = "紙別")]
        public string PaperType { get; set; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string InventoryItemNumber { set; get; }

        [Display(Name = "令數")]
        public decimal? SecondaryQuantity { set; get; }



    }
}