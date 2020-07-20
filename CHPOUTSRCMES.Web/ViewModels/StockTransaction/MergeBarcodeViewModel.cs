using CHPOUTSRCMES.Web.Models.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.ViewModels.StockTransaction
{
    public class MergeBarcodeViewModel
    {
        [Display(Name = "條碼")]
        public string MergeBarcode { set; get; }

        //待併板條碼清單
        public List<StockTransferBarcodeDT> WaitMergeBarcodeList { set; get; }

        [Display(Name = "條碼")]
        public string OriginalBarcode { set; get; }

        [Display(Name = "單位")]
        public string OriginalUnit { set; get; }

        [Display(Name = "條碼")]
        public string AfterBarcode { set; get; }
    }
}