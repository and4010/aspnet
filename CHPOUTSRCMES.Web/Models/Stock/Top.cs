using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.ViewModels.StockTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class Top
    {
        //private OrgSubinventoryData orgData = new OrgSubinventoryData();

        //public Top(OrgSubinventoryData orgSubinventoryData)
        //{
        //    orgData = orgSubinventoryData;
        //}

        public TopViewModel GetViewModel(MasterUOW uow, OrgSubinventoryData orgData, string userId)
        {
            TopViewModel viewModel = new TopViewModel();

            viewModel.SubinventoryItems = orgData.GetSubinventoryListForUserId(uow, userId, MasterUOW.DropDownListType.Choice);

            //viewModel.LocatorItems = orgData.GetLocatorList(uow, "265", viewModel.SelectedSubinventory, MasterUOW.DropDownListType.Choice);
            viewModel.LocatorItems = new List<SelectListItem> { new SelectListItem { Text = "請選擇", Value = "請選擇" } };
            return viewModel;
        }
    }
}