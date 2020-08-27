using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.StockTransaction
{
    public class OutBoundViewModel
    {
        public IEnumerable<SelectListItem> OutSubinventoryItems { set; get; }
        [Display(Name = "發貨倉庫")]
        public string SelectedOutSubinventory { set; get; }

        public IEnumerable<SelectListItem> OutLocatorItems { set; get; }
        [Display(Name = "發貨儲位")]
        public string SelectedOutLocator { set; get; }

        public IEnumerable<SelectListItem> InSubinventoryItems { set; get; }
        [Display(Name = "收貨倉庫")]
        public string SelectedInSubinventory { set; get; }

        public IEnumerable<SelectListItem> InLocatorItems { set; get; }
        [Display(Name = "收貨儲位")]
        public string SelectedInLocator { set; get; }

        //public bool DisplayDetail { set; get; }

        //public bool DisplayShipmentNumberArea { set; get; }

        public IEnumerable<SelectListItem> ShipmentNumberItems { set; get; }
        [Display(Name = "選擇出貨編號")]
        public string SelectedShipmentNumber { set; get; }

        [Display(Name = "出貨編號")]
        public string ShipmentNumber { set; get; }

        //public bool DisplaySubinventoryTransferNumberArea { set; get; }

        public IEnumerable<SelectListItem> SubinventoryTransferNumberItems { set; get; }
        [Display(Name = "選擇移轉編號")]
        public string SelectedSubinventoryTransferNumber { set; get; }

        [Display(Name = "移轉編號")]
        public string SubinventoryTransferNumber { set; get; }

        public IEnumerable<SelectListItem> ItemNumberItems { set; get; }
        [Display(Name = "料號")]
        public string SelectedItemNumber { set; get; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { set; get; }

        [Display(Name = "數量")] //輸入的主次單位數量
        public string InputTransactionQty { get; set; }

        [Display(Name = "單位")]
        public string Unit { get; set; }

        [Display(Name = "捲數/板數")]
        public string ROLL_REAM_QTY { get; set; }

        [Display(Name = "項次")]
        public string DetailSubId { get; set; }

        [Display(Name = "料號")]
        public string SelectedItemNumber2 { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE2 { get; set; }

        [Display(Name = "條碼")]
        public string BARCODE { get; set; }

        [Display(Name = "數量")] //輸入令包的數量
        public string InputReamQty { get; set; }

        [Display(Name = "每棧令數")] //每件令數
        public string ROLL_REAM_WT { get; set; }

        
    }
}