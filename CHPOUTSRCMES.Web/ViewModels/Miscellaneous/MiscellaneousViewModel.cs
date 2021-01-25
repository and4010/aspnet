using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.Miscellaneous
{
    public class MiscellaneousViewModel
    {
        public IEnumerable<SelectListItem> MiscellaneousItems { set; get; }
        [Display(Name = "異動類別")]
        public string SelectedMiscellaneous { set; get; }

        [Display(Name = "項次")]
        public string SUB_ID { set; get; }

        [Display(Name = "倉庫")]
        public string Subinventory { set; get; }

        [Display(Name = "儲位")]
        public string Locator { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "條碼號")]
        public string Barcode { set; get; }

        public string SearchUnit { set; get; }

        public string TransactionUnit { set; get; }

        public string SearchQty { set; get; }

        public string PercentageError { set; get; }

        public string Qty { set; get; }

        [Display(Name = "備註")]
        public string Note { set; get; }

        public IEnumerable<SelectListItem> SubinventoryItems { set; get; }
        [Display(Name = "倉庫")]
        public string SelectedSubinventory { set; get; }

        public IEnumerable<SelectListItem> LocatorItems { set; get; }
        [Display(Name = "儲位")]
        public string SelectedLocator { set; get; }

        public IEnumerable<SelectListItem> ItemNumberItems { set; get; }
        
    }
}