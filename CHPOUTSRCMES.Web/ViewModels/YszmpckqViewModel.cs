using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class YszmpckqViewModel
    {
        public IEnumerable<SelectListItem> OrganizationNameItems { set; get; }
        [Display(Name = "組織")]
        public string SelectedOrganization { set; get; }
        
        
        public IEnumerable<SelectListItem> OspSubinventoryNameItems { set; get; }
        [Display(Name = "加工廠")]
        public string SelectedOspSubinventory { set; get; }
        
        public IEnumerable<SelectListItem> PstypNameItems { set; get; }
        [Display(Name = "紙別")]
        public string SelectedPstyp { set; get; }

        public YszmpckqDT YszmpckqDT { set; get; }
    }
}