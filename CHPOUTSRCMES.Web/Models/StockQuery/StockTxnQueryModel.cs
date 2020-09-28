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

        [Display(Name = "時間")]
        public DateTime CreateDate { set; get; }


        public static List<StockTxnQueryModel> getModels(DataTableAjaxPostViewModel data,
            string subinventory, string locator, string itemCategory, string itemNo, string barcode, string userId)
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
, ISNULL(S.DOC, '') AS DocNumber
, ISNULL(S.ACTION, '') AS ActionName
, ISNULL(S.CATEGORY, '') AS Category
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

                builder.AppendLine(@" ORDER BY S.SUBINVENTORY_CODE, S.LOCATOR_ID, S.INVENTORY_ITEM_ID, S.BARCODE, S.CREATION_DATE");

                var models = mesContext.Database.SqlQuery<StockTxnQueryModel>(builder.ToString(), paramList.ToArray());
                return models.Skip(data.Start).Take(data.Length).ToList();

            }
            catch (Exception ex)
            {

            }

            return new List<StockTxnQueryModel>();
        }
    }
}