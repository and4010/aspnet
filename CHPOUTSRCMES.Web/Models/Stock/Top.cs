using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.ViewModels.StockTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class Top
    {
        //private OrgSubinventoryData orgData = new OrgSubinventoryData();

        //public Top(OrgSubinventoryData orgSubinventoryData)
        //{
        //    orgData = orgSubinventoryData;
        //}

        public TopViewModel GetViewModel(OrgSubinventoryData orgData)
        {

            TopViewModel viewModel = new TopViewModel();

            viewModel.SubinventoryItems = orgData.GetSubinventoryList("265", false);

            viewModel.LocatorItems = orgData.GetLocatorList("265", viewModel.SelectedSubinventory, false);

            return viewModel;
        }
    }
}