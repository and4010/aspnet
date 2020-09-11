using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CHPOUTSRCMES.Web.Jsons.Requests;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class QueryViewModel
    {
        public StockQueryModel Fields { set; get; }

        public IEnumerable<SelectListItem> SubinvenotoryList { set; get; }

        public IEnumerable<SelectListItem> ItemCategoryList { set; get; }

        public IEnumerable<SelectListItem> locatorList { set; get; }



        public static IEnumerable<SelectListItem> getSubinvenotoryList(string userId)
        {

            using var mesContext = new CHPOUTSRCMES.Web.DataModel.MesContext();

            using var masterUow = new CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW(mesContext);
            try
            {
                return masterUow.GetSubinventoryDropDownListForUserId(userId, MasterUOW.DropDownListType.All);
            }
            catch (Exception ex)
            {

            }

            return new List<SelectListItem>();
        }

        public static IEnumerable<SelectListItem> getItemCategoryList(string userId)
        {

            using var mesContext = new CHPOUTSRCMES.Web.DataModel.MesContext();

            using var masterUow = new CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW(mesContext);
            try
            {
                var list = masterUow.itemsTRepository.GetAll().GroupBy(x => x.CatalogElemVal070).Select(x => new SelectListItem()
                {
                    Value = x.Key,
                    Text = x.Key
                }).ToList();
                list.Insert(0, new SelectListItem() { Value = "*", Text = "全部" });
                return list;
            }
            catch (Exception ex)
            {

            }

            return new List<SelectListItem>();
        }

        public static IEnumerable<SelectListItem> getLocatorList(string userId, string subinventory)
        {

            using var mesContext = new CHPOUTSRCMES.Web.DataModel.MesContext();

            using var masterUow = new CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW(mesContext);
            try
            {
                return masterUow.GetLocatorDropDownListForUserId(userId, "", MasterUOW.DropDownListType.All);
            }
            catch (Exception ex)
            {

            }

            return new List<SelectListItem>();
        }

        public static IEnumerable<AutoCompletedItem> getItemNumbers(string itemNo)
        {

            using var mesContext = new CHPOUTSRCMES.Web.DataModel.MesContext();

            using var masterUow = new CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW(mesContext);
            try
            {
                return masterUow.GetAutoCompleteItemNumberList(itemNo);
            }
            catch (Exception ex)
            {

            }

            return new List<AutoCompletedItem>();
        }


    }
}