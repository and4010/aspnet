using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models.Information;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class OrgSubinventoryViewModel
    {
        public List<SelectListItem> OrganizationNameItems { set; get; }
        [Display(Name = "組織")]
        public string SelectedOrganization { set; get; }

        public IEnumerable<SelectListItem> SubinventoryNameItems { set; get; }
        [Display(Name = "倉庫")]
        public string SelectedSubinventory { set; get; }
        
        public IEnumerable<SelectListItem> LocatorNameItems { set; get; }
        [Display(Name = "儲位")]
        public string SelectedLocator { set; get; }

        public OrgSubinventoryDT OrgSubinventoryDT { set; get; }
        
    }
}