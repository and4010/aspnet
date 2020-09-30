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

namespace CHPOUTSRCMES.Web.Models.SoaQuery
{
    public class SoaDetailQueryModel
    {
        [Display(Name = "單號")]
        public string DocNumber { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "狀態")]
        public string StatusCode { set; get; }

        [Display(Name = "錯誤訊息")]
        public string ErrorMsg { set; get; }

        [Display(Name = "主單位數量")]
        public decimal? PrimaryQuantity { set; get; }

        [Display(Name = "單位")]
        public string PrimaryUom { set; get; }

        [Display(Name = "次單位數量")]
        public decimal? SecondaryQuantity { set; get; }

        [Display(Name = "次單位")]
        public string SecondaryUom { set; get; }

        public static DataTableJsonResultModel<SoaDetailQueryModel> getModels(DataTableAjaxPostViewModel data,
            string processCode, string serverCode, string batchId)
        {
            var paramList = new List<SqlParameter>();
            using var mesContext = new MesContext();
            
            try
            {
                string cmd = getSelectCommand(processCode);
                paramList.Add(SqlParamHelper.GetVarChar("@processCode", processCode, 20));
                paramList.Add(SqlParamHelper.GetVarChar("@serverCode", serverCode, 20));
                paramList.Add(SqlParamHelper.GetVarChar("@batchId", batchId, 20));

                var models = mesContext.Database.SqlQuery<SoaDetailQueryModel>(cmd, paramList.ToArray()).ToList();
                //var count = models.Count();

                var list = Search(models, data.Search.Value);
                list = Order(data.Order, models);

                list = list.Skip(data.Start).Take(data.Length);

                return new DataTableJsonResultModel<SoaDetailQueryModel>(data.Draw, models.Count, list.ToList());
            }
            catch (Exception ex)
            {

            }

            return new DataTableJsonResultModel<SoaDetailQueryModel>(data.Draw, 0, new List<SoaDetailQueryModel>());
        }

        public static IOrderedEnumerable<SoaDetailQueryModel> Order(List<Order> orders, IEnumerable<SoaDetailQueryModel> models)
        {
            IOrderedEnumerable<SoaDetailQueryModel> orderedModel = null;

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

        private static IOrderedEnumerable<SoaDetailQueryModel> OrderBy(int column, string dir, IEnumerable<SoaDetailQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DocNumber) : models.OrderBy(x => x.DocNumber);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemId) : models.OrderBy(x => x.InventoryItemId);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ItemNumber) : models.OrderBy(x => x.ItemNumber);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StatusCode) : models.OrderBy(x => x.StatusCode);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ErrorMsg) : models.OrderBy(x => x.ErrorMsg);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryUom) : models.OrderBy(x => x.SecondaryUom);
            }
        }

        private static IOrderedEnumerable<SoaDetailQueryModel> ThenBy(int column, string dir, IOrderedEnumerable<SoaDetailQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DocNumber) : models.ThenBy(x => x.DocNumber);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.InventoryItemId) : models.ThenBy(x => x.InventoryItemId);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ItemNumber) : models.ThenBy(x => x.ItemNumber);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.StatusCode) : models.ThenBy(x => x.StatusCode);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ErrorMsg) : models.ThenBy(x => x.ErrorMsg);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PrimaryQuantity) : models.ThenBy(x => x.PrimaryQuantity);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PrimaryUom) : models.ThenBy(x => x.PrimaryUom);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SecondaryQuantity) : models.ThenBy(x => x.SecondaryQuantity);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SecondaryUom) : models.ThenBy(x => x.SecondaryUom);
            }
        }

        private static IEnumerable<SoaDetailQueryModel> Search(IEnumerable<SoaDetailQueryModel> models, string search)
        {
            if (string.IsNullOrEmpty(search)) return models;

            return models.Where(x =>
                    (!string.IsNullOrEmpty(x.DocNumber) && x.DocNumber.Contains(search))
                    || (!string.IsNullOrEmpty(x.ItemNumber) && x.ItemNumber.Contains(search))
                );
        }


        private static string getSelectCommand(string processCode)
        {
            string cmd = "";
            switch(processCode)
            {
                case "XXIFP217":
                    cmd = @"
SELECT 
CONTAINER_NO AS DocNumber
, SHIP_ITEM_NUMBER AS ItemNumber
, INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, PRIMARY_QUANTITY AS PrimaryQuantity
, PRIMARY_UOM AS PrimaryUom
, SECONDARY_QUANTITY AS SecondaryQuantity
, SECONDARY_UOM AS SecondaryUom
FROM XXIF_CHP_P217_CONTAINER_ST C";
                    break;
                case "XXIFP218":
                    cmd = @"
SELECT 
H.CONTAINER_NO AS DocNumber
, SHIP_ITEM_NUMBER AS ItemNumber
, INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, PRIMARY_QUANTITY AS PrimaryQuantity
, PRIMARY_UOM AS PrimaryUom
, SECONDARY_QUANTITY AS SecondaryQuantity
, SECONDARY_UOM AS SecondaryUom
FROM XXIF_CHP_P218_CONTAINER_RV_ST C
JOIN CTR_SOA_T S ON S.PROCESS_CODE = C.PROCESS_CODE AND S.SERVER_CODE = C.SERVER_CODE AND S.BATCH_ID = C.BATCH_ID 
JOIN CTR_HEADER_T H ON H.CTR_HEADER_ID = S.CTR_HEADER_ID ";
                    break;
                case "XXIFP219":
                    cmd = @"
SELECT 
C.BATCH_NO AS DocNumber
, INVENTORY_ITEM_NUMBER AS ItemNumber
, INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, PRIMARY_QUANTITY AS PrimaryQuantity
, PRIMARY_UOM AS PrimaryUom
, SECONDARY_QUANTITY AS SecondaryQuantity
, SECONDARY_UOM AS SecondaryUom
FROM XXIF_CHP_P219_OSP_BATCH_ST C ";
                    break;
                case "XXIFP210":
                    cmd = @"
SELECT 
C.BATCH_NO AS DocNumber
, C.ITEM_NO AS ItemNumber
, I.INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, TRANSACTION_QUANTITY AS PrimaryQuantity
, TRANSACTION_UOM AS PrimaryUom
, SECONDARY_TRANSACTION_QUANTITY AS SecondaryQuantity
, C.SECONDARY_UOM_CODE AS SecondaryUom
FROM XXIF_CHP_P210_IN_MMT_INGR_ST C
JOIN ITEMS_T I ON I.ITEM_NUMBER = C.ITEM_NO ";
                    break;
                case "XXIFP211":
                    cmd = @"
SELECT 
C.BATCH_NO AS DocNumber
, C.ITEM_NO AS ItemNumber
, CAST(0 AS BIGINT) AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, TRANSACTION_QUANTITY AS PrimaryQuantity
, TRANSACTION_UOM AS PrimaryUom
, SECONDARY_TRANSACTION_QUANTITY AS SecondaryQuantity
, SECONDARY_UOM_CODE AS SecondaryUom
FROM XXIF_CHP_P211_IN_MMT_PROD_ST C ";
                    break;
                case "XXIFP213":
                    cmd = @"
SELECT 
C.PROCESS_CODE, C.SERVER_CODE, C.BATCH_ID,
C.BATCH_NO AS DocNumber
, '' AS ItemNumber
, CAST(0 AS BIGINT) AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, CAST(0 AS DECIMAL(30,10)) AS PrimaryQuantity
, '' AS PrimaryUom
, CAST(0 AS DECIMAL(30,10)) AS SecondaryQuantity
, '' AS SecondaryUom
FROM XXIF_CHP_P213_IN_BATCH_COM_ST C";
                    break;
                case "XXIFP220":
                    cmd = @"
SELECT 
C.TRIP_NAME AS DocNumber
, C.ITEM_NUMBER AS ItemNumber
, C.INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, REQUESTED_QUANTITY AS PrimaryQuantity
, REQUESTED_QUANTITY_UOM AS PrimaryUom
, REQUESTED_QUANTITY2 AS SecondaryQuantity
, REQUESTED_QUANTITY_UOM2 AS SecondaryUom
FROM XXIF_CHP_P220_DELIVERY_ST C ";
                    break;
                case "XXIFP221":
                    cmd = @"
SELECT 
(SELECT TOP 1 TRIP_NAME FROM DLV_HEADER_T WHERE TRIP_ID = C.TRIP_ID) AS  DocNumber
, I.ITEM_NUMBER AS ItemNumber
, C.INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, PRIMARY_QUANTITY AS PrimaryQuantity
, PRIMARY_UOM AS PrimaryUom
, SECONDARY_QUANTITY AS SecondaryQuantity
, SECONDARY_UOM AS SecondaryUom
FROM XXIF_CHP_P221_SHIP_CONFIRM_ST C
JOIN DLV_SOA_T S ON S.PROCESS_CODE = C.PROCESS_CODE AND S.SERVER_CODE = C.SERVER_CODE AND S.BATCH_ID = C.BATCH_ID 
JOIN DLV_HEADER_T H ON H.TRIP_ID = S.TRIP_ID
JOIN ITEMS_T I ON I.INVENTORY_ITEM_ID = C.INVENTORY_ITEM_ID ";
                    break;
                case "XXIFP222":
                    cmd = @"
SELECT 
C.SHIPMENT_NUMBER AS  DocNumber
, C.ITEM_NUMBER AS ItemNumber
, C.INVENTORY_ITEM_ID AS InventoryItemId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, PRIMARY_QUANTITY AS PrimaryQuantity
, PRIMARY_UOM AS PrimaryUom
, SECONDARY_QUANTITY AS SecondaryQuantity
, SECONDARY_UOM_CODE AS SecondaryUom
FROM XXIF_CHP_P222_SUB_TRANSFER_ST C ";
                    break;
            }

            return cmd + @" WHERE C.PROCESS_CODE = @processCode AND C.SERVER_CODE = @serverCode AND C.BATCH_ID = @batchId";


        }

    }
}