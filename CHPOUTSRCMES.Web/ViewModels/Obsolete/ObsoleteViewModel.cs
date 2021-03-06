using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.Obsolete
{
    public class ObsoleteViewModel
    {

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

        [Display(Name = "單位")]
        public string Unit { set; get; }

        public string Qty { set; get; }

        public IEnumerable<SelectListItem> SubinventoryItems { set; get; }
        [Display(Name = "倉庫")]
        public string SelectedSubinventory { set; get; }

        public IEnumerable<SelectListItem> LocatorItems { set; get; }
        [Display(Name = "儲位")]
        public string SelectedLocator { set; get; }

        public IEnumerable<SelectListItem> ItemNumberItems { set; get; }

    }
}