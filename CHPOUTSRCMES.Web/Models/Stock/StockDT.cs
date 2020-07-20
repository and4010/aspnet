using CHPOUTSRCMES.Web.Jsons.Requests;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockDT
    {
        public long ID { set; get; }
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_NAME { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public string SUBINVENTORY_NAME { set; get; }
        public long LOCATOR_ID { set; get; }
        public long LOCATOR_TYPE { set; get; }
        public string SEGMENT3 { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_NO { set; get; }
        public string OSP_BATCH_NO { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
        public string ITEM_CATEGORY { set; get; }
        public string LOT_NUMBER { set; get; }
        public string BARCODE { set; get; }
        public string PapaerType { get; set; }
        public string BasicWeight { get; set; }
        public string Specification { get; set; }
        public string PACKING_TYPE { set; get; }
        public string PRIMARY_UOM_CODE { set; get; }
        public decimal PRIMARY_TRANSACTION_QTY { set; get; }
        public decimal PRIMARY_AVAILABLE_QTY { set; get; }
        public string SECONDARY_UOM_CODE { set; get; }
        public decimal SECONDARY_TRANSACTION_QTY { set; get; }
        public decimal SECONDARY_AVAILABLE_QTY { set; get; }
        public string REASON_CODE { set; get; }
        public string REASON_DESC { set; get; }
        public string NOTE { set; get; }
        public string STATUS_CODE { set; get; }
        public long CREATE_BY { set; get; }
        public string CREATE_BY_USERNAME { set; get; }
        public string CREATE_DATE { set; get; }
        public long LAST_UPDATE_BY { set; get; }
        public string LAST_UPDATE_BY_USERNAME { set; get; }
        public string LAST_UPDATE_DATE { get; set; }

        public StockDT(long ID, long ORGANIZATION_ID, string ORGANIZATION_NAME, string SUBINVENTORY_CODE, string SUBINVENTORY_NAME,
            long LOCATOR_ID, long LOCATOR_TYPE, string SEGMENT3, long INVENTORY_ITEM_ID, string ITEM_NO, string OSP_BATCH_NO, string ITEM_DESCRIPTION, string ITEM_CATEGORY,
            string LOT_NUMBER, string BARCODE, string PapaerType, string BasicWeight, string Specification,
            string PACKING_TYPE, string PRIMARY_UOM_CODE, decimal PRIMARY_TRANSACTION_QTY, decimal PRIMARY_AVAILABLE_QTY,
            string SECONDARY_UOM_CODE, decimal SECONDARY_TRANSACTION_QTY, decimal SECONDARY_AVAILABLE_QTY, string REASON_CODE, string REASON_DESC,
            string NOTE, string STATUS_CODE, long CREATE_BY, string CREATE_BY_USERNAME, string CREATE_DATE, long LAST_UPDATE_BY,
            string LAST_UPDATE_BY_USERNAME, string LAST_UPDATE_DATE)
        {
            this.ID = ID;
            this.ORGANIZATION_ID = ORGANIZATION_ID;
            this.ORGANIZATION_NAME = ORGANIZATION_NAME;
            this.SUBINVENTORY_CODE = SUBINVENTORY_CODE;
            this.SUBINVENTORY_NAME = SUBINVENTORY_NAME;
            this.LOCATOR_ID = LOCATOR_ID;
            this.LOCATOR_TYPE = LOCATOR_TYPE;
            this.SEGMENT3 = SEGMENT3;
            this.INVENTORY_ITEM_ID = INVENTORY_ITEM_ID;
            this.ITEM_NO = ITEM_NO;
            this.OSP_BATCH_NO = OSP_BATCH_NO;
            this.ITEM_DESCRIPTION = ITEM_DESCRIPTION;
            this.ITEM_CATEGORY = ITEM_CATEGORY;
            this.LOT_NUMBER = LOT_NUMBER;
            this.BARCODE = BARCODE;
            this.PapaerType = PapaerType;
            this.BasicWeight = BasicWeight;
            this.Specification = Specification;
            this.PACKING_TYPE = PACKING_TYPE;
            this.PRIMARY_UOM_CODE = PRIMARY_UOM_CODE;
            this.PRIMARY_TRANSACTION_QTY = PRIMARY_TRANSACTION_QTY;
            this.PRIMARY_AVAILABLE_QTY = PRIMARY_AVAILABLE_QTY;
            this.SECONDARY_UOM_CODE = SECONDARY_UOM_CODE;
            this.SECONDARY_TRANSACTION_QTY = SECONDARY_TRANSACTION_QTY;
            this.SECONDARY_AVAILABLE_QTY = SECONDARY_AVAILABLE_QTY;
            this.REASON_CODE = REASON_CODE;
            this.REASON_DESC = REASON_DESC;
            this.NOTE = NOTE;
            this.STATUS_CODE = STATUS_CODE;
            this.CREATE_BY = CREATE_BY;
            this.CREATE_BY_USERNAME = CREATE_BY_USERNAME;
            this.CREATE_DATE = CREATE_DATE;
            this.LAST_UPDATE_BY = LAST_UPDATE_BY;
            this.LAST_UPDATE_BY_USERNAME = LAST_UPDATE_BY_USERNAME;
            this.LAST_UPDATE_DATE = LAST_UPDATE_DATE;

        }


    }

    public class StockData
    {

        public static List<StockDT> source = new List<StockDT>();

        public static void resetData()
        {
            source = new List<StockDT>();
        }

        public static void addDefault()
        {
            if (source.Count == 0)
            {
                source.Add(new StockDT(1, 265, "INV_ORG_華紙總公司", "SFG", "中間倉", 22016, 2, "TB2",
                    504029, "4A003A01000310K266K", "P9B0288", "試抄紙品", "平版", "", "B2005060001", "A003", "01000", "310K266K",
                    "令包", "KG", 1200m, 1200m,
                    "RE", 120, 120, "", "", "", "", 4, "華紙管理員", "2020-05-26", 4, "華紙管理員", "2020-05-26"));
                source.Add(new StockDT(2, 265, "INV_ORG_華紙總公司", "TB2", "總倉-南崁", 0, 1, "",
                    504030, "4AB23P00699350K250K", "P2010087", "雪面銅版紙", "平版", "", "A2005060002", "AB23", "00699", "350K250K",
                    "無令打件", "KG", 1200m, 1200m,
                    "RE", 120, 120, "", "", "", "", 4, "華紙管理員", "2020-05-26", 4, "華紙管理員", "2020-05-26"));
                source.Add(new StockDT(3, 265, "INV_ORG_華紙總公司", "TB2", "總倉-南崁", 0, 1, "",
                    504031, "4FHIZA03000787RL00","", "捲筒金典銅西", "捲筒", "2618011282120406", "A2005060001", "FHIZ", "03000", "787RL00",
                    "", "KG", 1200m, 1200m,
                    "", 0, 0, "", "", "", "", 4, "華紙管理員", "2020-05-26", 4, "華紙管理員", "2020-05-26"));
                source.Add(new StockDT(4, 285, "INV_ORG_華紙新屋廠", "SA", "新屋內銷", 0, 1, "",
                  504032, "4DM00P03000310K446K", "P4010000", "全塗灰銅卡", "平版", "", "C2005060001", "DM00", "03000", "310K446K",
                  "無令打件", "KG", 1200m, 1200m,
                  "RE", 120, 120, "", "", "", "", 4, "華紙管理員", "2020-05-26", 4, "華紙管理員", "2020-05-26"));
            }

        }

        public static IEnumerable<SelectListItem> GetItemNumberList(string subInventoryCode, long locatorId)
        {


            List<ListItem> list = new List<ListItem>();

            list.Add(new ListItem("請選擇", "請選擇"));

            var query = from stockDT in source
                        where subInventoryCode == stockDT.SUBINVENTORY_CODE &&
                        locatorId == stockDT.LOCATOR_ID

                        group stockDT by new { stockDT.ITEM_NO } into g
                        select new ListItem
                        {
                            Text = g.Key.ITEM_NO,
                            Value = g.Key.ITEM_NO
                        };
            list.AddRange(query.ToList());

            return list.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public static List<AutoCompletedItem> AutoCompleteItemNumber(string subInventoryCode, long locatorId, string Prefix)
        {
            var query = from stockDT in source
                        where subInventoryCode == stockDT.SUBINVENTORY_CODE &&
                        locatorId == stockDT.LOCATOR_ID &&
                        stockDT.ITEM_NO.Contains(Prefix)
                        group stockDT by new { stockDT.ITEM_NO } into g
                        select new AutoCompletedItem
                        {
                            Description = g.Key.ITEM_NO,
                            Value = g.Key.ITEM_NO
                        };

            return query.ToList();
        }

        public static List<AutoCompletedItem> AutoCompleteItemNumber(string subInventoryCode, string Prefix)
        {
            var query = from stockDT in source
                        where subInventoryCode == stockDT.SUBINVENTORY_CODE &&
                        stockDT.ITEM_NO.Contains(Prefix)
                        group stockDT by new { stockDT.ITEM_NO } into g
                        select new AutoCompletedItem
                        {
                            Description = g.Key.ITEM_NO,
                            Value = g.Key.ITEM_NO
                        };

            return query.ToList();
        }

        public static List<StockDT> GetInboundStockItemData(long ORGANIZATION_ID, string ITEM_NO)
        {
            var query = from stockDT in source
                        where ORGANIZATION_ID == stockDT.ORGANIZATION_ID &&
                        ITEM_NO == stockDT.ITEM_NO
                        select stockDT;

            List<StockDT> list = query.ToList();
            return list;
        }

        public static List<StockDT> GetStockItemData(string SUBINVENTORY_CODE, string ITEM_NO)
        {
            var query = from stockDT in source
                        where SUBINVENTORY_CODE == stockDT.SUBINVENTORY_CODE &&
                        ITEM_NO == stockDT.ITEM_NO
                        select stockDT;

            List<StockDT> list = query.ToList();
            return list;
        }

        public static List<StockDT> GetStockData(string BARCODE)
        {
            var query = from stockDT in source
                        where BARCODE == stockDT.BARCODE
                        select stockDT;

            List<StockDT> list = query.ToList();
            return list;
        }

        public static List<StockDT> GetModel(string SubinventoryCode, long LocatorId, string ItemNumber)
        {
            var query = from stockDT in source
                        where SubinventoryCode == stockDT.SUBINVENTORY_CODE &&
                        LocatorId == stockDT.LOCATOR_ID &&
                        ItemNumber == stockDT.ITEM_NO
                        select stockDT;
            return query.ToList();
        }

        public static List<StockDT> GetModel(string SubinventoryCode, long LocatorId, string ItemNumber, decimal? PrimaryQty, decimal? PercentageError)
        {
            if (PrimaryQty == null)
            {
                PrimaryQty = 0;
            }
            if (PercentageError == null)
            {
                PercentageError = 0;
            }
            PercentageError = PercentageError * 0.01m;
            decimal errorQty = (decimal)PrimaryQty * (decimal)PercentageError;
            decimal maxQty = (decimal)PrimaryQty + errorQty;
            decimal minQty = (decimal)PrimaryQty - errorQty;

            var query = from stockDT in source
                        where SubinventoryCode == stockDT.SUBINVENTORY_CODE &&
                        LocatorId == stockDT.LOCATOR_ID &&
                        stockDT.PRIMARY_AVAILABLE_QTY >= minQty &&
                        stockDT.PRIMARY_AVAILABLE_QTY <= maxQty &&
                        ItemNumber == stockDT.ITEM_NO
                        select stockDT;
            return query.ToList();
        }

        public static ResultModel SaveReason(long ID, string REASON_CODE, string REASON_DESC, long LOCATOR_ID, string NOTE, OrgSubinventoryData orgData)
        {
            var stockQuery = from stockDT in source
                        where ID == stockDT.ID
                        select stockDT;
            var stockList = stockQuery.ToList();

            if (stockList.Count == 0)
            {
                return new ResultModel(false, "找不到庫存");
            }

            string SUBINVENTORY_CODE = stockList[0].SUBINVENTORY_CODE;
            var newOrgList = orgData.search(SUBINVENTORY_CODE, LOCATOR_ID);
            if (newOrgList.Count == 0)
            {
                return new ResultModel(false, "找不到儲位資料");
            }

            string SEGMENT3 = newOrgList[0].SEGMENT3;
            long LOCATOR_TYPE = newOrgList[0].LOCATOR_TYPE;

            foreach (StockDT stockDT in source)
            {
                if (stockDT.ID == stockList[0].ID)
                {
                    stockDT.REASON_CODE = REASON_CODE;
                    stockDT.REASON_DESC = REASON_DESC;
                    stockDT.LOCATOR_ID = LOCATOR_ID;
                    stockDT.SEGMENT3 = SEGMENT3;
                    stockDT.LOCATOR_TYPE = LOCATOR_TYPE;
                    stockDT.NOTE = NOTE;
                }

            }
            return new ResultModel(true, "貨故儲存成功");
        }


    }

    internal class StockDTOrder
    {
        public static IOrderedEnumerable<StockDT> Order(List<Order> orders, IEnumerable<StockDT> models)
        {
            IOrderedEnumerable<StockDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockDT> OrderBy(int column, string dir, IEnumerable<StockDT> models)
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

        private static IOrderedEnumerable<StockDT> ThenBy(int column, string dir, IOrderedEnumerable<StockDT> models)
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