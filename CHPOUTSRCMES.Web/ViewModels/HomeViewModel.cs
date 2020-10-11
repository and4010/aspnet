using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class HomeViewModel
    {

        public int OspPendingCount { set; get; }

        public int DlvPendingCount { set; get; }
        
        public int CtrPendingCount { set; get; }
        
    }
}