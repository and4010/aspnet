using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models.Delivery;

namespace CHPOUTSRCMES.Web.ViewModels.Delivery
{
    public class DeliverySearchViewModel
    {
        [Display(Name = "組車日")]
        public string TripActualShipBeginDate { set; get; }
        public string TripActualShipEndDate { set; get; }
        [Display(Name = "交運單號")]
        public string DeliveryName { set; get; }

        public IEnumerable<SelectListItem> SubinventoryNameItems { set; get; }
        [Display(Name = "倉庫")]
        public string SelectedSubinventory { set; get; }

        public IEnumerable<SelectListItem> TripNameItems { set; get; }
        [Display(Name = "航程號")]
        public string SelectedTrip { set; get; }


        [Display(Name = "出貨申請日")]
        public string TransactionDate { set; get; }

        public IEnumerable<SelectListItem> DeliveryStatusItems { set; get; }
        [Display(Name = "狀態")]
        public string SelectedDeliveryStatus { set; get; }
    }
}