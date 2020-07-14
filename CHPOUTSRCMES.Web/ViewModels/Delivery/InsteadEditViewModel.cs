using CHPOUTSRCMES.Web.Models.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.ViewModels.Delivery
{
    public class InsteadEditViewModel
    {
        public DeliveryDetailViewHeader DeliveryDetailViewHeader { get; set; }

        [Display(Name = "條碼號")]
        public string BARCODE { get; set; }
    }
}