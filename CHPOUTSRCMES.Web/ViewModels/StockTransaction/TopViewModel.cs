using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.StockTransaction
{
    public class TopViewModel
    {
        public IEnumerable<SelectListItem> SubinventoryItems { set; get; }
        [Display(Name = "倉庫")]
        public string SelectedSubinventory { set; get; }

        public IEnumerable<SelectListItem> LocatorItems { set; get; }
        [Display(Name = "儲位")]
        public string SelectedLocator { set; get; }

        public IEnumerable<SelectListItem> ItemNumberItems { set; get; }
        [Display(Name = "料號")]
        public string ItemNumber { set; get; }
    }
}