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

        [Display(Name = "主單位")]
        public string PrimaryUomCode { set; get; }

        [Display(Name = "主可用量")]
        public decimal PrimaryAvailableQty { set; get; }

        [Display(Name = "次單位")]
        public string SecondaryUomCode { set; get; }

        [Display(Name = "次可用量")]
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

        public static List<StockDetailQueryModel> getModels(DataTableAjaxPostViewModel data,
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
, ISNULL(S.BASIC_WEIGHT, '') AS BasicWeight
, ISNULL(S.REAM_WEIGHT, '') AS ReamWeight
, ISNULL(S.LOT_NUMBER, '') AS LotNumber
, ISNULL(S.PACKING_TYPE, '') AS PackingType
, ISNULL(S.SPECIFICATION, '') AS Specification
, ISNULL(S.PRIMARY_UOM_CODE, '') AS PrimaryUomCode
, ISNULL(S.PRIMARY_AVAILABLE_QTY, 0) AS PrimaryAvailableQty
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
                return models.OrderBy(x => x.SubinventoryCode).ThenBy(x => x.LocatorSegments).ThenBy(x => x.ItemNumber).Skip(data.Start).Take(data.Length).ToList();
            }
            catch (Exception ex)
            {

            }

            return new List<StockDetailQueryModel>();
        }
    }
}