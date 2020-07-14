using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Models.StockInventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.StockInvetory
{
    public class StockInventoryViewModel
    {
        public IEnumerable<SelectListItem> TransactionTypeItems { set; get; }
        [Display(Name = "異動類別")]
        public string SelectedTransactionType { set; get; }

        public static List<StockInventoryModel> model = new List<StockInventoryModel>();


        public static List<StockInventoryModel> GetModel(string SubinventoryCode, long LocatorId, string ItemNumber)
        {
            var query = from StockInventoryModel in model
                        where SubinventoryCode == StockInventoryModel.SUBINVENTORY_CODE &&
                        LocatorId == StockInventoryModel.LOCATOR_ID &&
                        ItemNumber == StockInventoryModel.ITEM_NO
                        select StockInventoryModel;
            return query.ToList();
        }




    }

    internal class StockInventoryModelDTOrder
    {
        public static IOrderedEnumerable<StockInventoryModel> Order(List<Order> orders, IEnumerable<StockInventoryModel> models)
        {
            IOrderedEnumerable<StockInventoryModel> orderedModel = null;
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


        private static IOrderedEnumerable<StockInventoryModel> OrderBy(int column, string dir, IEnumerable<StockInventoryModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ID) : models.OrderBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUBINVENTORY_CODE) : models.OrderBy(x => x.SUBINVENTORY_CODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SEGMENT3) : models.OrderBy(x => x.SEGMENT3);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NO) : models.OrderBy(x => x.ITEM_NO);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BARCODE) : models.OrderBy(x => x.BARCODE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_AVAILABLE_QTY) : models.OrderBy(x => x.PRIMARY_AVAILABLE_QTY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_UOM_CODE) : models.OrderBy(x => x.PRIMARY_UOM_CODE);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_AVAILABLE_QTY) : models.OrderBy(x => x.SECONDARY_AVAILABLE_QTY);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_UOM_CODE) : models.OrderBy(x => x.SECONDARY_UOM_CODE);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REASON_DESC) : models.OrderBy(x => x.REASON_DESC);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.NOTE) : models.OrderBy(x => x.NOTE);
            }
        }

        private static IOrderedEnumerable<StockInventoryModel> ThenBy(int column, string dir, IOrderedEnumerable<StockInventoryModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ID) : models.ThenBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUBINVENTORY_CODE) : models.ThenBy(x => x.SUBINVENTORY_CODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SEGMENT3) : models.ThenBy(x => x.SEGMENT3);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NO) : models.ThenBy(x => x.ITEM_NO);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BARCODE) : models.ThenBy(x => x.BARCODE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_AVAILABLE_QTY) : models.ThenBy(x => x.PRIMARY_AVAILABLE_QTY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_UOM_CODE) : models.ThenBy(x => x.PRIMARY_UOM_CODE);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_AVAILABLE_QTY) : models.ThenBy(x => x.SECONDARY_AVAILABLE_QTY);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_UOM_CODE) : models.ThenBy(x => x.SECONDARY_UOM_CODE);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REASON_DESC) : models.ThenBy(x => x.REASON_DESC);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.NOTE) : models.ThenBy(x => x.NOTE);
            }
        }
    }
}