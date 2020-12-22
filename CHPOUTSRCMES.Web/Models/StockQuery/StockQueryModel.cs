using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.StockQuery
{
    public class StockQueryModel
    {
        [Display(Name = "倉庫")]
        public string SubinventoryCode { set; get; }

        [Display(Name = "儲位ID")]
        public long LocatorId { set; get; }

        [Display(Name = "儲位")]
        public string LocatorSegments { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "捲筒/平版")]
        public string ItemCategory { set; get; }

        [Display(Name = "KG")]
        public string PrimaryUomCode { set; get; }

        [Display(Name = "重量(KG)")]
        public decimal PrimaryAvailableQty { set; get; }

        [Display(Name = "總重量(KG)")]
        public decimal PrimarySumQty { set; get; }

        [Display(Name = "RE")]
        public string SecondaryUomCode { set; get; }

        [Display(Name = "令數(RE)")]
        public decimal? SecondaryAvailableQty { set; get; }

        [Display(Name = "總令數(RE)")]
        public decimal? SecondarySumQty { set; get; }

        public static DataTableJsonResultModel<StockQueryModel> getModels(DataTableAjaxPostViewModel data,
            string subinventory, string locator, string itemCategory, string itemNo, string userId)
        {
            var paramList = new List<SqlParameter>();
            using var mesContext = new MesContext();
            try
            {
                StringBuilder builder = new StringBuilder(@"
SELECT 
ISNULL(S.SUBINVENTORY_CODE, '') AS SubinventoryCode
, ISNULL(S.LOCATOR_ID, 0) AS LocatorId
, MIN(ISNULL(S.LOCATOR_SEGMENTS, '')) AS LocatorSegments
, ISNULL(S.INVENTORY_ITEM_ID, 0) AS InventoryItemId
, MIN(ISNULL(S.ITEM_NUMBER, '')) AS ItemNumber
, MIN(ISNULL(S.ITEM_CATEGORY, '')) AS ItemCategory
, CASE MIN(ISNULL(S.ITEM_CATEGORY, '')) WHEN '平版' THEN '' ELSE MIN(ISNULL(S.PRIMARY_UOM_CODE, '')) END AS PrimaryUomCode
, CASE MIN(ISNULL(S.ITEM_CATEGORY, '')) WHEN '平版' THEN 0 ELSE SUM(ISNULL(S.PRIMARY_AVAILABLE_QTY, 0)) END AS PrimaryAvailableQty
, CASE MIN(ISNULL(S.ITEM_CATEGORY, '')) WHEN '平版' THEN 0 ELSE SUM(ISNULL(S.PRIMARY_AVAILABLE_QTY, 0)) + SUM(ISNULL(S.PRIMARY_LOCKED_QTY, 0)) END AS PrimarySumQty
, MIN(ISNULL(S.SECONDARY_UOM_CODE, '')) AS SecondaryUomCode
, SUM(ISNULL(S.SECONDARY_AVAILABLE_QTY, 0)) AS SecondaryAvailableQty
, SUM(ISNULL(S.SECONDARY_AVAILABLE_QTY, 0)) + SUM(ISNULL(S.SECONDARY_LOCKED_QTY, 0)) AS SecondarySumQty
FROM STOCK_T S
JOIN USER_SUBINVENTORY_T U ON U.SUBINVENTORY_CODE = S.SUBINVENTORY_CODE
WHERE U.UserId =  @userId AND S.STATUS_CODE = @statusCode
");
                paramList.Add(SqlParamHelper.GetVarChar("@userId", userId, 128));
                paramList.Add(SqlParamHelper.GetVarChar("@statusCode", MasterUOW.StockStatusCode.InStock, 10));
                if (!long.TryParse(locator, out long locatorId))
                {
                    locatorId = 0;
                }

                if (!string.IsNullOrEmpty(subinventory) && subinventory.CompareTo("*") != 0)
                {
                    builder.AppendLine(" AND S.SUBINVENTORY_CODE= @subinventoryCode");
                    paramList.Add(SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventory));
                }

                if (locatorId > 0)
                {
                    builder.AppendLine(" AND S.LOCATOR_ID= @locatorId");
                    paramList.Add(SqlParamHelper.GetBigInt("@locatorId", locatorId));
                }

                if (!string.IsNullOrEmpty(itemCategory) && itemCategory.CompareTo("*") != 0)
                {
                    builder.AppendLine(" AND S.ITEM_CATEGORY= @itemCategory");
                    paramList.Add(SqlParamHelper.GetVarChar("@itemCategory", itemCategory, 10, System.Data.ParameterDirection.Input));
                }

                if (!string.IsNullOrEmpty(itemNo) && itemNo.CompareTo("*") != 0)
                {
                    builder.AppendLine(" AND S.ITEM_NUMBER= @itemNumber");
                    paramList.Add(SqlParamHelper.GetVarChar("@itemNumber", itemNo, 40, System.Data.ParameterDirection.Input));
                }

                builder.AppendLine(@"
 GROUP BY S.SUBINVENTORY_CODE, S.LOCATOR_ID, S.INVENTORY_ITEM_ID
 ORDER BY S.SUBINVENTORY_CODE, S.LOCATOR_ID, S.INVENTORY_ITEM_ID");

                var models = mesContext.Database.SqlQuery<StockQueryModel>(builder.ToString(), paramList.ToArray()).ToList();
                int totalCount = models.Count;

                var list = Search(models, data.Search.Value);
                int filteredCount = list.Count();

                list = Order(data.Order, list);

                list = list.Skip(data.Start).Take(data.Length);

                return new DataTableJsonResultModel<StockQueryModel>(data.Draw, filteredCount, totalCount, list.ToList());

            }
            catch (Exception ex)
            {

            }

            return new DataTableJsonResultModel<StockQueryModel>(data.Draw, 0, new List<StockQueryModel>());
        }

        public static IOrderedEnumerable<StockQueryModel> Order(List<Order> orders, IEnumerable<StockQueryModel> models)
        {
            IOrderedEnumerable<StockQueryModel> orderedModel = null;

            for (int i = 0; i < orders.Count(); i++)
            {
                if (i == 0)
                {
                    orderedModel = OrderBy(orders[i].Column, orders[i].Dir, models);
                }
                else
                {
                    orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
                }
            }
            return orderedModel;
        }

        private static IOrderedEnumerable<StockQueryModel> OrderBy(int column, string dir, IEnumerable<StockQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SubinventoryCode) : models.OrderBy(x => x.SubinventoryCode);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LocatorSegments) : models.OrderBy(x => x.LocatorSegments);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ItemNumber) : models.OrderBy(x => x.ItemNumber);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ItemCategory) : models.OrderBy(x => x.ItemCategory);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryAvailableQty) : models.OrderBy(x => x.PrimaryAvailableQty);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryAvailableQty) : models.OrderBy(x => x.SecondaryAvailableQty);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimarySumQty) : models.OrderBy(x => x.PrimarySumQty);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondarySumQty) : models.OrderBy(x => x.SecondarySumQty);
            }
        }

        private static IOrderedEnumerable<StockQueryModel> ThenBy(int column, string dir, IOrderedEnumerable<StockQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SubinventoryCode) : models.ThenBy(x => x.SubinventoryCode);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LocatorSegments) : models.ThenBy(x => x.LocatorSegments);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ItemNumber) : models.ThenBy(x => x.ItemNumber);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ItemCategory) : models.ThenBy(x => x.ItemCategory);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PrimaryAvailableQty) : models.ThenBy(x => x.PrimaryAvailableQty);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SecondaryAvailableQty) : models.ThenBy(x => x.SecondaryAvailableQty);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PrimarySumQty) : models.ThenBy(x => x.PrimarySumQty);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SecondarySumQty) : models.ThenBy(x => x.SecondarySumQty);
            }
        }

        private static IEnumerable<StockQueryModel> Search(IEnumerable<StockQueryModel> models, string search)
        {
            if (string.IsNullOrEmpty(search)) return models;

            return models.Where(x =>
                    (!string.IsNullOrEmpty(x.SubinventoryCode) && x.SubinventoryCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.LocatorSegments) && x.LocatorSegments.Contains(search))
                    || (!string.IsNullOrEmpty(x.ItemNumber) && x.ItemNumber.Contains(search))
                );
        }
    }
}