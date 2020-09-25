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
    public class StockTxnQueryModel
    {
        [Display(Name = "組織ID")]
        public long OrganizationId { set; get; }

        [Display(Name = "組織")]
        public string OrganizationCode { set; get; }

        [Display(Name = "倉庫")]
        public string SubinventoryCode { set; get; }

        [Display(Name = "儲位ID")]
        public long LocatorId { set; get; }

        [Display(Name = "儲位")]
        public string LocatorSegments { set; get; }

        [Display(Name = "移轉組織ID")]
        public long TrfOrganizationId { set; get; }

        [Display(Name = "移轉組織")]
        public string TrfOrganizationCode { set; get; }

        [Display(Name = "移轉倉庫")]
        public string TrfSubinventoryCode { set; get; }

        [Display(Name = "移轉儲位ID")]
        public long TrfLocatorId { set; get; }

        [Display(Name = "移轉儲位")]
        public string TrfLocatorSegments { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "捲筒/平版")]
        public string ItemCategory { set; get; }

        [Display(Name = "主單位")]
        public string PrimaryUomCode { set; get; }

        public decimal PrimaryAvailableQty { set; get; }

        public decimal PrimaryChangedQty { set; get; }

        [Display(Name = "次單位")]
        public string SecondaryUomCode { set; get; }

        public decimal? SecondaryAvailableQty { set; get; }

        public decimal SecondaryChangedQty { set; get; }

        public string Category { set; get; }

        public string DocNumber { set; get; }

        public string Action { set; get; }


        public static List<StockQueryModel> getModels(DataTableAjaxPostViewModel data,
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
, CASE MIN(ISNULL(S.ITEM_CATEGORY, '')) WHEN '平版' THEN 0 ELSE MIN(ISNULL(S.PRIMARY_AVAILABLE_QTY, 0)) END AS PrimaryAvailableQty
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

                var models = mesContext.Database.SqlQuery<StockQueryModel>(builder.ToString(), paramList.ToArray());
                return models.OrderBy(x => x.SubinventoryCode).ThenBy(x=>x.LocatorSegments).ThenBy(x=>x.ItemNumber).Skip(data.Start).Take(data.Length).ToList();

            }
            catch (Exception ex)
            {

            }

            return new List<StockQueryModel>();
        }
    }
}