using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.StockTransaction
{
    public class TransferReasonViewModel
    {
        public IEnumerable<SelectListItem> ReasonItems { set; get; }
        [Display(Name = "異常原因")]
        public string Reason { set; get; }

        public IEnumerable<SelectListItem> LocatorItems { set; get; }
        [Display(Name = "儲位")]
        public string Locator { set; get; }

        [Display(Name = "備註")]
        public string Remark { set; get; }

       

    }
}