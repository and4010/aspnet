using CHPOUTSRCMES.Web.Models.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.Report
{
    public class YieldViewModel
    {

        [Display(Name = "裁切完工日期")]
        public DateTime? CuttingDateFrom { set; get; }
        public DateTime? CuttingDateTo { set; get; }

        [Display(Name = "工單號")]
        public string BatchNo { set; get; }

        [Display(Name = "機台")]
        public string MachineNum { set; get; }

        [Display(Name = "倉庫")]
        public string Subinventory { set; get; }

        public YieldQueryModel Fields { set; get; }

        public IEnumerable<SelectListItem> BathNoList { set; get; }

        public IEnumerable<SelectListItem> MachineCodeList { set; get; }

        public IEnumerable<SelectListItem> SubinventoryList { set; get; }
    }
}