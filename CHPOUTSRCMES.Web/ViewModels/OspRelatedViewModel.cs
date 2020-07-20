using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models.Information;


namespace CHPOUTSRCMES.Web.ViewModels
{
    public class OspRelatedViewModel
    {
        public IEnumerable<SelectListItem> OrganizationNameItems { set; get; }
        [Display(Name = "組織")]
        public string SelectedOrganization { set; get; }

        public IEnumerable<SelectListItem> InventoryItemNumberItems { set; get; }
        [Display(Name = "組成成分料號")]
        public string SelectedInventoryItemNumber { set; get; }

        public IEnumerable<SelectListItem> RelatedItemNumberItems { set; get; }
        [Display(Name = "餘切料號")]
        public string SelectedRelatedItemNumber { set; get; }


        public OspRelatedDT OspRelatedDT { set; get; }
    }
}