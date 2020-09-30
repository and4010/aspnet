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
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.StockQuery
{
    public class StockDetailQueryModel
    {
        [Display(Name = "庫存ID")]
        public long StockId { set; get; }

        [Display(Name = "倉庫代碼")]
        public string SubinventoryCode { set; get; }
        
        [Display(Name = "倉庫名稱")]
        public string SubinventoryName { set; get; }

        [Display(Name = "儲位ID")]
        public long LocatorId { set; get; }

        [Display(Name = "儲位")]
        public string LocatorSegments { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "KG")]
        public string PrimaryUomCode { set; get; }

        [Display(Name = "重量(KG)")]
        public decimal PrimaryAvailableQty { set; get; }

        [Display(Name = "RE")]
        public string SecondaryUomCode { set; get; }

        [Display(Name = "令數(RE)")]
        public decimal? SecondaryAvailableQty { set; get; }

        [Display(Name = "基重")]
        public string BasicWeight { set; get; }

        [Display(Name = "令重")]
        public string ReamWeight { set; get; }

        [Display(Name = "規格")]
        public string Specification { set; get; }

        [Display(Name = "條碼")]
        public string Barcode { set; get; }

        [Display(Name = "捲號")]
        public string LotNumber { set; get; }

        [Display(Name = "紙別")]
        public string PaperType { set; get; }

        [Display(Name = "包裝方式")]
        public string PackingType { set; get; }

        public static DataTableJsonResultModel<StockDetailQueryModel> getModels(DataTableAjaxPostViewModel data,
            string subinventory, long locatorId, long itemId, string userId)
        {
            var paramList = new List<SqlParameter>();
            using var mesContext = new MesContext();

            try
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder(@"
SELECT 
S.STOCK_ID AS StockId
, ISNULL(S.SUBINVENTORY_CODE, '') AS SubinventoryCode
, ISNULL(B.SUBINVENTORY_NAME, '') AS SubinventoryName
, ISNULL(S.LOCATOR_ID, 0) AS LocatorId
, ISNULL(S.LOCATOR_SEGMENTS, '') AS LocatorSegments
, ISNULL(S.INVENTORY_ITEM_ID, 0) AS InventoryItemId
, ISNULL(S.ITEM_NUMBER, '') AS ItemNumber
, ISNULL(S.ITEM_CATEGORY, '') AS ItemCategory
, ISNULL(S.BARCODE, '') AS Barcode
, ISNULL(S.PAPER_TYPE, '') AS PaperType
, ISNULL(S.BASIC_WEIGHT, '') AS BasicWeight
, ISNULL(S.REAM_WEIGHT, '') AS ReamWeight
, ISNULL(S.LOT_NUMBER, '') AS LotNumber
, ISNULL(S.PACKING_TYPE, '') AS PackingType
, ISNULL(S.SPECIFICATION, '') AS Specification
, CASE ISNULL(S.ITEM_CATEGORY, '') WHEN '平版' THEN '' ELSE ISNULL(S.PRIMARY_UOM_CODE, '') END AS PrimaryUomCode
, CASE ISNULL(S.ITEM_CATEGORY, '') WHEN '平版' THEN 0 ELSE ISNULL(S.PRIMARY_AVAILABLE_QTY, 0) END AS PrimaryAvailableQty
, ISNULL(S.SECONDARY_UOM_CODE, '') AS SecondaryUomCode
, ISNULL(S.SECONDARY_AVAILABLE_QTY, 0) AS SecondaryAvailableQty
FROM STOCK_T S
JOIN USER_SUBINVENTORY_T U ON U.SUBINVENTORY_CODE = S.SUBINVENTORY_CODE
JOIN SUBINVENTORY_T B ON B.SUBINVENTORY_CODE = S.SUBINVENTORY_CODE AND B.ORGANIZATION_ID = S.ORGANIZATION_ID
WHERE U.UserId =  @userId 
AND S.SUBINVENTORY_CODE= @subinventoryCode
AND S.LOCATOR_ID= @locatorId
AND S.INVENTORY_ITEM_ID= @itemId
AND S.STATUS_CODE = @statusCode
ORDER BY S.SUBINVENTORY_CODE, S.LOCATOR_ID, S.INVENTORY_ITEM_ID
");
                paramList.Add(SqlParamHelper.GetVarChar("@userId", userId, 128));
                paramList.Add(SqlParamHelper.GetVarChar("@statusCode", MasterUOW.StockStatusCode.InStock, 10));
                paramList.Add(SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventory));
                paramList.Add(SqlParamHelper.GetBigInt("@locatorId", locatorId));
                paramList.Add(SqlParamHelper.GetBigInt("@itemId", itemId));
                var models = mesContext.Database.SqlQuery<StockDetailQueryModel>(builder.ToString(), paramList.ToArray()).ToList();
                //var count = models.Count();

                var list = Search(models, data.Search.Value);
                list = Order(data.Order, models);

                list = list.Skip(data.Start).Take(data.Length);

                return new DataTableJsonResultModel<StockDetailQueryModel>(data.Draw, 0, list.ToList());
            }
            catch (Exception ex)
            {

            }

            return new DataTableJsonResultModel<StockDetailQueryModel>(data.Draw, 0, new List<StockDetailQueryModel>());
        }

        public static IOrderedEnumerable<StockDetailQueryModel> Order(List<Order> orders, IEnumerable<StockDetailQueryModel> models)
        {
            IOrderedEnumerable<StockDetailQueryModel> orderedModel = null;

            for (int i = 0; i < orders.Count(); i++)
            {
                if(i == 0)
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

        private static IOrderedEnumerable<StockDetailQueryModel> OrderBy(int column, string dir, IEnumerable<StockDetailQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StockId) : models.OrderBy(x => x.StockId);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SubinventoryCode) : models.OrderBy(x => x.SubinventoryCode);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LocatorId) : models.OrderBy(x => x.LocatorId);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LocatorSegments) : models.OrderBy(x => x.LocatorSegments);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemId) : models.OrderBy(x => x.InventoryItemId);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ItemNumber) : models.OrderBy(x => x.ItemNumber);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryAvailableQty) : models.OrderBy(x => x.PrimaryAvailableQty);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUomCode) : models.OrderBy(x => x.PrimaryUomCode);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryAvailableQty) : models.OrderBy(x => x.SecondaryAvailableQty);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryUomCode) : models.OrderBy(x => x.SecondaryUomCode);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BasicWeight) : models.OrderBy(x => x.BasicWeight);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWeight) : models.OrderBy(x => x.ReamWeight);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
            }
        }

        private static IOrderedEnumerable<StockDetailQueryModel> ThenBy(int column, string dir, IOrderedEnumerable<StockDetailQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.StockId) : models.ThenBy(x => x.StockId);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SubinventoryCode) : models.ThenBy(x => x.SubinventoryCode);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LocatorId) : models.ThenBy(x => x.LocatorId);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LocatorSegments) : models.ThenBy(x => x.LocatorSegments);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.InventoryItemId) : models.ThenBy(x => x.InventoryItemId);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ItemNumber) : models.ThenBy(x => x.ItemNumber);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Barcode) : models.ThenBy(x => x.Barcode);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LotNumber) : models.ThenBy(x => x.LotNumber);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PrimaryAvailableQty) : models.ThenBy(x => x.PrimaryAvailableQty);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PrimaryUomCode) : models.ThenBy(x => x.PrimaryUomCode);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SecondaryAvailableQty) : models.ThenBy(x => x.SecondaryAvailableQty);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SecondaryUomCode) : models.ThenBy(x => x.SecondaryUomCode);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PaperType) : models.ThenBy(x => x.PaperType);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BasicWeight) : models.ThenBy(x => x.BasicWeight);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Specification) : models.ThenBy(x => x.Specification);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ReamWeight) : models.ThenBy(x => x.ReamWeight);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PackingType) : models.ThenBy(x => x.PackingType);
            }
        }

        private static IEnumerable<StockDetailQueryModel> Search(IEnumerable<StockDetailQueryModel> models, string search)
        {
            if (string.IsNullOrEmpty(search)) return models;

            return models.Where(x =>
                    (!string.IsNullOrEmpty(x.SubinventoryCode) && x.SubinventoryCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.LocatorSegments) && x.LocatorSegments.Contains(search))
                    || (!string.IsNullOrEmpty(x.ItemNumber) && x.ItemNumber.Contains(search))
                    || (!string.IsNullOrEmpty(x.BasicWeight) && x.BasicWeight.Contains(search))
                    || (!string.IsNullOrEmpty(x.ReamWeight) && x.ReamWeight.Contains(search))
                    || (!string.IsNullOrEmpty(x.Specification) && x.Specification.Contains(search))
                    || (!string.IsNullOrEmpty(x.Barcode) && x.Barcode.Contains(search))
                    || (!string.IsNullOrEmpty(x.LotNumber) && x.LotNumber.Contains(search))
                    || (!string.IsNullOrEmpty(x.PaperType) && x.PaperType.Contains(search))
                    || (!string.IsNullOrEmpty(x.PackingType) && x.PackingType.Contains(search))
                );
        }
        

        
    }
}