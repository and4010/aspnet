using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.StockQuery
{
    public class StockTxnQueryModel
    {
        [Display(Name = "條碼")]
        public string Barcode { set; get; }
        [Display(Name = "組織ID")]
        public long OrganizationId { set; get; }

        [Display(Name = "組織")]
        public string OrganizationCode { set; get; }

        [Display(Name = "組織名稱")]
        public string OrganizationName { set; get; }

        [Display(Name = "倉庫")]
        public string SubinventoryCode { set; get; }

        [Display(Name = "倉庫名稱")]
        public string SubinventoryName { set; get; }

        [Display(Name = "儲位ID")]
        public long LocatorId { set; get; }

        [Display(Name = "儲位")]
        public string LocatorSegment3 { set; get; }

        [Display(Name = "組織ID")]
        public long TrfOrganizationId { set; get; }

        [Display(Name = "組織")]
        public string TrfOrganizationCode { set; get; }

        [Display(Name = "組織名稱")]
        public string TrfOrganizationName { set; get; }

        [Display(Name = "倉庫")]
        public string TrfSubinventoryCode { set; get; }

        [Display(Name = "倉庫名稱")]
        public string TrfSubinventoryName { set; get; }

        [Display(Name = "儲位ID")]
        public long TrfLocatorId { set; get; }

        [Display(Name = "儲位")]
        public string TrfLocatorSegment3 { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "捲筒/平版")]
        public string ItemCategory { set; get; }

        [Display(Name = "單位")]
        public string UomCode { set; get; }

        [Display(Name = "剩餘量")]
        public decimal AvailableQty { set; get; }

        [Display(Name = "變動量")]
        public decimal ChangedQty { set; get; }

        [Display(Name = "作業")]
        public string Category { set; get; }

        [Display(Name = "單號")]
        public string DocNumber { set; get; }

        [Display(Name = "原因")]
        public string ActionName { set; get; }

        [Display(Name = "備註")]
        public string Note { set; get; }

        [Display(Name = "時間")]
        public DateTime CreateDate { set; get; }


        public static DataTableJsonResultModel<StockTxnQueryModel> getModels(DataTableAjaxPostViewModel data,
            string subinventory, string locator, string itemCategory, string itemNo, string barcode, string date,string userId)
        {
            var paramList = new List<SqlParameter>();
            using var mesContext = new MesContext();
            try
            {
                StringBuilder builder = new StringBuilder(@"
SELECT 
ISNULL(S.BARCODE, '') AS Barcode
, ISNULL(S.ORGANIZATION_ID, '') AS OrganizationId
, ISNULL(S.ORGANIZATION_CODE, '') AS OrganizationCode
, ISNULL(O.ORGANIZATION_NAME, '') AS OrganizationName
, ISNULL(S.SUBINVENTORY_CODE, '') AS SubinventoryCode
, ISNULL(B.SUBINVENTORY_NAME, '') AS SubinventoryName
, ISNULL(S.LOCATOR_ID, 0) AS LocatorId
, ISNULL(L.SEGMENT3, '') AS LocatorSegment3
, ISNULL(S.DST_ORGANIZATION_ID, '') AS TrfOrganizationId
, ISNULL(S.DST_ORGANIZATION_CODE, '') AS TrfOrganizationCode
, ISNULL(RO.ORGANIZATION_NAME, '') AS TrfOrganizationName
, ISNULL(S.DST_SUBINVENTORY_CODE, '') AS TrfSubinventoryCode
, ISNULL(TB.SUBINVENTORY_NAME, '') AS TrfSubinventoryName
, ISNULL(S.DST_LOCATOR_ID, 0) AS TrfLocatorId
, ISNULL(TL.SEGMENT3, '') AS TrfLocatorSegment3
, ISNULL(S.INVENTORY_ITEM_ID, 0) AS InventoryItemId
, ISNULL(S.ITEM_NUMBER, '') AS ItemNumber
, ISNULL(S.ITEM_CATEGORY, '') AS ItemCategory
, CASE ISNULL(S.ITEM_CATEGORY, '') WHEN '平版' THEN ISNULL(S.SEC_UOM_CODE, '') ELSE ISNULL(S.PRY_UOM_CODE, '') END AS UomCode
, CASE ISNULL(S.ITEM_CATEGORY, '') WHEN '平版' THEN ISNULL(S.SEC_AFT_QTY, 0) ELSE ISNULL(S.PRY_AFT_QTY, 0) END AS AvailableQty
, CASE ISNULL(S.ITEM_CATEGORY, '') WHEN '平版' THEN ISNULL(S.SEC_CHG_QTY, 0) ELSE ISNULL(S.PRY_CHG_QTY, 0) END AS ChangedQty
, ISNULL(
	CASE S.CATEGORY 
	WHEN '庫存移轉-貨故' THEN '' 
	WHEN '盤點-盤盈' THEN ''  
	WHEN '盤點-盤虧' THEN '' 
	WHEN '雜項異動-雜發' THEN ''  
	WHEN '雜項異動-雜收' THEN '' 
	WHEN '存貨報廢' THEN ''
	ELSE S.DOC END, '') AS DocNumber
, ISNULL(S.ACTION, '') AS ActionName
, ISNULL(S.CATEGORY, '') AS Category
, ISNULL(S.NOTE, '') AS Note 
, ISNULL(S.CREATION_DATE, '') AS CreateDate
FROM STK_TXN_T S
LEFT JOIN ORGANIZATION_T O ON O.ORGANIZATION_ID = S.ORGANIZATION_ID
LEFT JOIN ORGANIZATION_T RO ON RO.ORGANIZATION_ID = S.DST_ORGANIZATION_ID
LEFT JOIN SUBINVENTORY_T B ON B.SUBINVENTORY_CODE = S.SUBINVENTORY_CODE
LEFT JOIN SUBINVENTORY_T TB ON TB.SUBINVENTORY_CODE = S.DST_SUBINVENTORY_CODE
LEFT JOIN LOCATOR_T L ON L.LOCATOR_ID = S.LOCATOR_ID
LEFT JOIN LOCATOR_T TL ON TL.LOCATOR_ID = S.DST_LOCATOR_ID
WHERE 1=1 
AND ( S.SUBINVENTORY_CODE IN (SELECT SUBINVENTORY_CODE FROM USER_SUBINVENTORY_T WHERE UserId = @userId) 
OR S.DST_SUBINVENTORY_CODE IN (SELECT SUBINVENTORY_CODE FROM USER_SUBINVENTORY_T WHERE UserId = @userId) )
");
                paramList.Add(SqlParamHelper.GetVarChar("@userId", userId, 128));
               
                if (!long.TryParse(locator, out long locatorId))
                {
                    locatorId = 0;
                }

                if (!string.IsNullOrEmpty(subinventory) && subinventory.CompareTo("*") != 0)
                {
                    builder.AppendLine(" AND ( S.SUBINVENTORY_CODE= @subinventoryCode OR S.DST_SUBINVENTORY_CODE = @subinventoryCode )");
                    paramList.Add(SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventory));
                }

                if (locatorId > 0)
                {
                    builder.AppendLine(" AND ( S.LOCATOR_ID= @locatorId OR S.DST_LOCATOR_ID = @locatorId )");
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

                if (!string.IsNullOrEmpty(barcode))
                {
                    builder.AppendLine(" AND S.BARCODE= @barcode");
                    paramList.Add(SqlParamHelper.R.Barcode("@barcode", barcode));
                }

                if (!string.IsNullOrEmpty(date))
                {
                    builder.AppendLine("AND S.CREATION_DATE BETWEEN '' and @date");
                    paramList.Add(SqlParamHelper.R.Barcode("@date", date));
                }

                var models = mesContext.Database.SqlQuery<StockTxnQueryModel>(builder.ToString(), paramList.ToArray()).ToList();

                var list = Search(models, data.Search.Value);

                list = Order(data.Order, models);

                list = list.Skip(data.Start).Take(data.Length);

                return new DataTableJsonResultModel<StockTxnQueryModel>(data.Draw, models.Count, list.ToList());

            }
            catch (Exception ex)
            {

            }

            return new DataTableJsonResultModel<StockTxnQueryModel>(data.Draw, 0, new List<StockTxnQueryModel>());
        }


        public static IOrderedEnumerable<StockTxnQueryModel> Order(List<Order> orders, IEnumerable<StockTxnQueryModel> models)
        {
            IOrderedEnumerable<StockTxnQueryModel> orderedModel = null;

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

        private static IOrderedEnumerable<StockTxnQueryModel> OrderBy(int column, string dir, IEnumerable<StockTxnQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CreateDate) : models.OrderBy(x => x.CreateDate);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemId) : models.OrderBy(x => x.InventoryItemId);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ItemNumber) : models.OrderBy(x => x.ItemNumber);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ItemCategory) : models.OrderBy(x => x.ItemCategory);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrganizationId) : models.OrderBy(x => x.OrganizationId);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrganizationCode) : models.OrderBy(x => x.OrganizationCode);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrganizationName) : models.OrderBy(x => x.OrganizationName);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SubinventoryCode) : models.OrderBy(x => x.SubinventoryCode);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SubinventoryName) : models.OrderBy(x => x.SubinventoryName);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LocatorId) : models.OrderBy(x => x.LocatorId);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LocatorSegment3) : models.OrderBy(x => x.LocatorSegment3);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfOrganizationId) : models.OrderBy(x => x.TrfOrganizationId);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfOrganizationCode) : models.OrderBy(x => x.TrfOrganizationCode);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfOrganizationName) : models.OrderBy(x => x.TrfOrganizationName);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfSubinventoryCode) : models.OrderBy(x => x.TrfSubinventoryCode);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfSubinventoryName) : models.OrderBy(x => x.TrfSubinventoryName);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfLocatorId) : models.OrderBy(x => x.TrfLocatorId);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TrfLocatorSegment3) : models.OrderBy(x => x.TrfLocatorSegment3);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.AvailableQty) : models.OrderBy(x => x.AvailableQty);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ChangedQty) : models.OrderBy(x => x.ChangedQty);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.UomCode) : models.OrderBy(x => x.UomCode);
                case 22:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ActionName) : models.OrderBy(x => x.ActionName);
                case 23:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Note) : models.OrderBy(x => x.Note);
                case 24:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category) : models.OrderBy(x => x.Category);
                case 25:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DocNumber) : models.OrderBy(x => x.DocNumber);
                
            }
        }

        private static IOrderedEnumerable<StockTxnQueryModel> ThenBy(int column, string dir, IOrderedEnumerable<StockTxnQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CreateDate) : models.ThenBy(x => x.CreateDate);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Barcode) : models.ThenBy(x => x.Barcode);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.InventoryItemId) : models.ThenBy(x => x.InventoryItemId);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ItemNumber) : models.ThenBy(x => x.ItemNumber);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ItemCategory) : models.ThenBy(x => x.ItemCategory);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OrganizationId) : models.ThenBy(x => x.OrganizationId);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OrganizationCode) : models.ThenBy(x => x.OrganizationCode);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OrganizationName) : models.ThenBy(x => x.OrganizationName);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SubinventoryCode) : models.ThenBy(x => x.SubinventoryCode);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SubinventoryName) : models.ThenBy(x => x.SubinventoryName);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LocatorId) : models.ThenBy(x => x.LocatorId);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LocatorSegment3) : models.ThenBy(x => x.LocatorSegment3);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfOrganizationId) : models.ThenBy(x => x.TrfOrganizationId);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfOrganizationCode) : models.ThenBy(x => x.TrfOrganizationCode);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfOrganizationName) : models.ThenBy(x => x.TrfOrganizationName);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfSubinventoryCode) : models.ThenBy(x => x.TrfSubinventoryCode);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfSubinventoryName) : models.ThenBy(x => x.TrfSubinventoryName);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfLocatorId) : models.ThenBy(x => x.TrfLocatorId);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TrfLocatorSegment3) : models.ThenBy(x => x.TrfLocatorSegment3);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.AvailableQty) : models.ThenBy(x => x.AvailableQty);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ChangedQty) : models.ThenBy(x => x.ChangedQty);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.UomCode) : models.ThenBy(x => x.UomCode);
                case 22:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ActionName) : models.ThenBy(x => x.ActionName);
                case 23:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Note) : models.ThenBy(x => x.Note);
                case 24:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Category) : models.ThenBy(x => x.Category);
                case 25:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DocNumber) : models.ThenBy(x => x.DocNumber);
            }
        }

        private static IEnumerable<StockTxnQueryModel> Search(IEnumerable<StockTxnQueryModel> models, string search)
        {
            if (string.IsNullOrEmpty(search)) return models;

            return models.Where(x =>
                    (!string.IsNullOrEmpty(x.Barcode) && x.Barcode.Contains(search))
                    || (!string.IsNullOrEmpty(x.ItemNumber) && x.ItemNumber.Contains(search))
                    || (!string.IsNullOrEmpty(x.ItemCategory) && x.ItemCategory.Contains(search))
                    || (!string.IsNullOrEmpty(x.ItemCategory) && x.ItemCategory.Contains(search))
                    || (!string.IsNullOrEmpty(x.OrganizationCode) && x.OrganizationCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.SubinventoryCode) && x.SubinventoryCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.LocatorSegment3) && x.LocatorSegment3.Contains(search))
                    || (!string.IsNullOrEmpty(x.TrfOrganizationCode) && x.TrfOrganizationCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.TrfSubinventoryCode) && x.TrfSubinventoryCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.TrfLocatorSegment3) && x.TrfLocatorSegment3.Contains(search))
                    || (!string.IsNullOrEmpty(x.ActionName) && x.ActionName.Contains(search))
                    || (!string.IsNullOrEmpty(x.Category) && x.Category.Contains(search))
                    || (!string.IsNullOrEmpty(x.DocNumber) && x.DocNumber.Contains(search))
                    || (!string.IsNullOrEmpty(x.Note) && x.Note.Contains(search))
                );
        }
    }
}