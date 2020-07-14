using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Inventory
{
    public class Inventory
    {
        public PaperRollModel paperRollModel { set; get; }

        public FlatModel flatModel { set; get; }

        public class PaperRollModel
        {
            public int Id { get; set; }

            [Display(Name = "倉庫")]
            public string Subinventory { get; set; }

            [Display(Name = "儲位")]
            public string Locator { set; get; }

            [Display(Name = "條碼號")]
            public string Barcode { get; set; }

            [Display(Name = "料號")]
            public string Item_No { get; set; }

            [Display(Name = "紙別")]
            public string PaperType { get; set; }

            [Display(Name = "基重")]
            public string BaseWeight { get; set; }

            [Display(Name = "規格")]
            public string Specification { get; set; }

            [Display(Name = "理論重(KG)")]
            public string TheoreticalWeight { get; set; }

            [Display(Name = "捲號")]
            public string LotNumber { set; get; }

            [Display(Name = "在庫")]
            public string Stock { set; get; }

            [Display(Name = "實盤在庫")]
            public string FirmStock { set; get; }

            [Display(Name = "異動數量(KG)")]
            public string TransactionQuantity { get; set; }

            [Display(Name = "異動原因")]
            public string Reason { get; set; }

            [Display(Name = "異動人員ID")]
            public string Created_by { set; get; }

            [Display(Name = "異動時間")]
            public DateTime Last_Updated_Date { set; get; }

            [Display(Name = "動作")]
            public string Panying { get; set; }

        }


        public class FlatModel
        {

            public int Id { get; set; }

            [Display(Name = "倉庫")]
            public string Subinventory { get; set; }

            [Display(Name = "儲位")]
            public string Locator { get; set; }

            [Display(Name = "條碼號")]
            public string Barcode { get; set; }

            [Display(Name = "料號")]
            public string Item_No { get; set; }

            [Display(Name = "令重")]
            public string ReamWeight { get; set; }

            [Display(Name = "包裝方式")]
            public string PackingType { get; set; }

            [Display(Name = "令數")]
            public string Ream_Qty { get; set; }

            [Display(Name = "異動數量(令)")]
            public string StockReam_Qty { get; set; }

            [Display(Name = "異動原因")]
            public string Reason { get; set; }

            [Display(Name = "異動人員ID")]
            public string Created_by { set; get; }

            [Display(Name = "異動時間")]
            public DateTime Last_Updated_Date { set; get; }

            [Display(Name = "動作")]
            public string Panying { get; set; }
        }
    }
}