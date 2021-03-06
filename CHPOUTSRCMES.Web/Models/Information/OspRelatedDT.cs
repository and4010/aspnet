using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class OspRelatedDT
    {
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_CODE { set; get; }
        public string ORGANIZATION_NAME { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_NUMBER { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
        public long RELATED_ITEM_ID { set; get; }
        public string RELATED_ITEM_NUMBER { set; get; }
        public string RELATED_ITEM_DESCRIPTION { set; get; }
        public long CREATED_BY { get; set; }
        public string CREATED_BY_NAME { get; set; }
        public DateTime? CREATION_DATE { get; set; }
        public long LAST_UPDATED_BY { get; set; }
        public string LAST_UPDATED_BY_NAME { get; set; }
        public DateTime? LAST_UPDATE_DATE { get; set; }

    }

    public class OspRelatedData
    {


        private List<SelectListItem> getOrganizationList(string userId)
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetOrganizationDropDownListForUserId(userId, DropDownListType.All);
            }
        }

        public List<SelectListItem> GetInventoryItemList(string ORGANIZATION_ID)
        {
            using (var context = new MesContext())
            {
                return new RelatedUOW(context).GetInventoryItemNumber(ORGANIZATION_ID);
            }
        }




        public List<SelectListItem> GetRelatedItemList(string ORGANIZATION_ID, string INVENTORY_ITEM_ID)
        {
            using (var context = new MesContext())
            {
                return new RelatedUOW(context).GetRelatedItem(ORGANIZATION_ID, INVENTORY_ITEM_ID);
            }
        }





        public List<OspRelatedDT> search(string ORGANIZATION_ID, string INVENTORY_ITEM_ID, string RELATED_ITEM_ID)
        {
            using (var context = new MesContext())
            {
                return new RelatedUOW(context).search(ORGANIZATION_ID, INVENTORY_ITEM_ID, RELATED_ITEM_ID);
            }
        }

        public OspRelatedViewModel getViewModel(string userId)
        {
            OspRelatedViewModel viewModel = new OspRelatedViewModel();
            viewModel.SelectedOrganization = "*";
            viewModel.SelectedInventoryItemNumber = "*";
            viewModel.SelectedRelatedItemNumber = "*";

            viewModel.OrganizationNameItems = getOrganizationList(userId);
            viewModel.InventoryItemNumberItems = GetInventoryItemList("*");
            viewModel.RelatedItemNumberItems = GetRelatedItemList("*", "*");

            return viewModel;
        }
    }

    internal class OspRelatedDTOrder
    {
        public static IOrderedEnumerable<OspRelatedDT> Order(List<Order> orders, IEnumerable<OspRelatedDT> models)
        {
            IOrderedEnumerable<OspRelatedDT> orderedModel = null;
            if (orders.Count() > 0)
            {
                orderedModel = OrderBy(orders[0].Column, orders[0].Dir, models);
            }

            for (int i = 1; i < orders.Count(); i++)
            {
                orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
            }
            return orderedModel;
        }


        private static IOrderedEnumerable<OspRelatedDT> OrderBy(int column, string dir, IEnumerable<OspRelatedDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_DESCRIPTION) : models.OrderBy(x => x.ITEM_DESCRIPTION);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RELATED_ITEM_NUMBER) : models.OrderBy(x => x.RELATED_ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RELATED_ITEM_DESCRIPTION) : models.OrderBy(x => x.RELATED_ITEM_DESCRIPTION);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATED_BY_NAME) : models.OrderBy(x => x.CREATED_BY_NAME);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATION_DATE) : models.OrderBy(x => x.CREATION_DATE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATED_BY_NAME) : models.OrderBy(x => x.LAST_UPDATED_BY_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATE_DATE) : models.OrderBy(x => x.LAST_UPDATE_DATE);
            }
        }

        private static IOrderedEnumerable<OspRelatedDT> ThenBy(int column, string dir, IOrderedEnumerable<OspRelatedDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_DESCRIPTION) : models.ThenBy(x => x.ITEM_DESCRIPTION);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RELATED_ITEM_NUMBER) : models.ThenBy(x => x.RELATED_ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RELATED_ITEM_DESCRIPTION) : models.ThenBy(x => x.RELATED_ITEM_DESCRIPTION);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATED_BY_NAME) : models.ThenBy(x => x.CREATED_BY_NAME);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATION_DATE) : models.ThenBy(x => x.CREATION_DATE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATED_BY_NAME) : models.ThenBy(x => x.LAST_UPDATED_BY_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATE_DATE) : models.ThenBy(x => x.LAST_UPDATE_DATE);
            }
        }
    }
}