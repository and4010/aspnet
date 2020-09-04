using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.ViewModels.StockInvetory;
using CHPOUTSRCMES.Web.ViewModels;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Text;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockInventoryDT
    {
        public long ID { set; get; }

        public long SUB_ID { set; get; }
        public long STOCK_ID { set; get; }

        public long ORGANIZATION_ID { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public long LOCATOR_ID { set; get; }
        public string SEGMENT3 { set; get; }
        public string ITEM_NO { set; get; }
        public string ITEM_CATEGORY { set; get; }
        public string BARCODE { set; get; }
        public string LOT_NUMBER { set; get; }
        public decimal PRIMARY_TRANSACTION_QTY { set; get; }
        public decimal PRIMARY_AVAILABLE_QTY { set; get; }
        public string PRIMARY_UOM_CODE { set; get; }
        public decimal SECONDARY_TRANSACTION_QTY { set; get; }
        public decimal SECONDARY_AVAILABLE_QTY { set; get; }
        public string SECONDARY_UOM_CODE { set; get; }
        public string NOTE { set; get; }
        public DateTime LAST_UPDATE_DATE { set; get; }
        public string STATUS { set; get; }
        public string PapaerType { get; set; }
        public string BasicWeight { get; set; }
        public string Specification { get; set; }
        public string OSP_BATCH_NO { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
    }

    public class StockInventoryData
    {
        public OrgSubinventoryData orgData = new OrgSubinventoryData();
        public static List<StockInventoryDT> profitModel = new List<StockInventoryDT>();
        public static List<StockInventoryDT> lossModel = new List<StockInventoryDT>();

        public static void resetData()
        {
            profitModel = new List<StockInventoryDT>();
            lossModel = new List<StockInventoryDT>();
        }

        public StockInventoryViewModel GetStockInvetoryViewModel(StockInventoryUOW uow)
        {
            StockInventoryViewModel viewModel = new StockInventoryViewModel();
            viewModel.TransactionTypeItems = uow.GetInventoryTypeDropDownList();
            return viewModel;
        }

        public List<StockDT> SearchStock(StockInventoryUOW uow, long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            return uow.GetStockTList(organizationId, subinventoryCode, locatorId, itemNumber);
        }

        public List<StockInventoryDT> GetsLossStockInventoryHistoryData(StockInventoryUOW uow, long organizationId, string subinventoryCode, long? locatorId, string itemNumber, string userId)
        {
            return uow.GetsLossStockInventoryHtList(organizationId, subinventoryCode, locatorId, itemNumber, userId);
        }

        public List<StockInventoryDT> GetsStockInventoryData(StockInventoryUOW uow, string userId, long transactionTypeId, bool fromHistoryData)
        {
            return uow.GetStockInventoryTList(userId, transactionTypeId, fromHistoryData);
        }

        public ResultModel CreateDetail(StockInventoryUOW uow, long transactionTypeId, long stockId, decimal mQty, string userId, string userName)
        {
            return uow.CreateDetail(transactionTypeId, stockId, mQty, userId, userName);
        }

        public ResultModel SaveTransactionDetail(StockInventoryUOW uow, long transactionTypeId, string userId, string userName)
        {
            return uow.SaveTransactionDetail(transactionTypeId, userId, userName);
        }

        public ResultModel DetailEditor(StockInventoryUOW uow, StockInventoryDTEditor editor, string userId, string userName)
        {
            if (editor.Action == "remove")
            {
                var ids = editor.StockInventoryDTList.Select(x => x.ID).ToList();
                return uow.DelDetailData(ids);
            }
            else if (editor.Action == "edit")
            {
                var ids = editor.StockInventoryDTList.Select(x => x.ID).ToList();
                string note = editor.StockInventoryDTList[0].NOTE;
                return uow.UpdateDetailNote(ids, note, userId, userName);
            }
            else if (editor.Action == "create")
            {
                if (editor.StockInventoryDTList == null || editor.StockInventoryDTList.Count == 0) return new ResultModel(false, "沒有資料可新增明細");
                return uow.CreateDetailForNoStock(editor.StockInventoryDTList[0], userId, userName);
            }
            else
            {
                return new ResultModel(false, "無法識別作業項目");
            }
        }

        #region 舊

        public List<StockInventoryDT> GetProfitModel()
        {
            var query = from data in profitModel
                        where data.STATUS != "已異動存檔"
                        select data;
            return query.ToList();
        }

        public List<StockInventoryDT> GetLossModel()
        {
            var query = from data in lossModel
                        where data.STATUS != "已異動存檔"
                        select data;
            return query.ToList();
        }

        public List<StockInventoryDT> GetLossModel(string SubinventoryCode, long Locator, string ItemNumber)
        {
            var query = from data in lossModel
                        where SubinventoryCode == data.SUBINVENTORY_CODE &&
                        Locator == data.LOCATOR_ID &&
                        ItemNumber == data.ITEM_NO &&
                        data.STATUS == "已異動存檔"
                        select data;
            return query.ToList();
        }

        public ResultModel AddLossDetail(long ID, decimal Qty)
        {
            decimal newQty = -Qty;
            var stockData = StockData.source.FirstOrDefault(d => d.ID == ID);
            if (stockData == null)
            {
                return new ResultModel(false, "找不到庫存");
            }
            else
            {
                if (stockData.ITEM_CATEGORY == "捲筒" && stockData.PRIMARY_AVAILABLE_QTY < Qty)
                {
                    return new ResultModel(false, "數量不可大於庫存量");
                }
                var detailData = lossModel.FirstOrDefault(d => d.BARCODE == stockData.BARCODE && d.STATUS == "未異動存檔");
                if (detailData != null)
                {
                    return new ResultModel(false, "異動記錄已存在");
                }

                StockInventoryDT addData = new StockInventoryDT();
                var highestId = lossModel.Any() ? lossModel.Select(x => x.ID).Max() : 0;
                addData.ID = highestId + 1;
                addData.STOCK_ID = stockData.ID;
                addData.BARCODE = stockData.BARCODE;
                addData.ITEM_NO = stockData.ITEM_NO;
                addData.LOT_NUMBER = stockData.LOT_NUMBER;
                addData.NOTE = stockData.NOTE;
                addData.ITEM_CATEGORY = stockData.ITEM_CATEGORY;
                if (stockData.ITEM_CATEGORY == "平版")
                {
                    addData.PRIMARY_TRANSACTION_QTY = newQty * 10;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + (newQty * 10);
                    addData.SECONDARY_TRANSACTION_QTY = newQty;
                    addData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY + newQty;
                }
                else if (stockData.ITEM_CATEGORY == "捲筒")
                {
                    addData.PRIMARY_TRANSACTION_QTY = newQty;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + newQty;
                    addData.SECONDARY_TRANSACTION_QTY = 0;
                    addData.SECONDARY_AVAILABLE_QTY = 0;
                }
                addData.PRIMARY_UOM_CODE = stockData.PRIMARY_UOM_CODE;
                addData.SECONDARY_UOM_CODE = stockData.SECONDARY_UOM_CODE;
                addData.LOCATOR_ID = stockData.LOCATOR_ID;
                addData.SEGMENT3 = stockData.SEGMENT3;
                addData.SUBINVENTORY_CODE = stockData.SUBINVENTORY_CODE;
                addData.LAST_UPDATE_DATE = DateTime.Now;
                addData.STATUS = "未異動存檔";
                addData.PapaerType = stockData.PapaerType;
                addData.BasicWeight = stockData.BasicWeight;
                addData.Specification = stockData.Specification;
                addData.OSP_BATCH_NO = stockData.OSP_BATCH_NO;
                addData.ITEM_DESCRIPTION = stockData.ITEM_DESCRIPTION;
                lossModel.Add(addData);
            }
            return new ResultModel(true, "新增明細成功");
        }

        public ResultModel AddProfitDetail(long ID, decimal Qty)
        {
            decimal newQty = Qty;
            var stockData = StockData.source.FirstOrDefault(d => d.ID == ID);
            if (stockData == null)
            {
                return new ResultModel(false, "找不到庫存");
            }
            else
            {
                var detailData = profitModel.FirstOrDefault(d => d.BARCODE == stockData.BARCODE);
                if (detailData != null)
                {
                    return new ResultModel(false, "異動記錄已存在");
                }

                StockInventoryDT addData = new StockInventoryDT();
                var highestId = profitModel.Any() ? profitModel.Select(x => x.ID).Max() : 0;
                addData.ID = highestId + 1;
                addData.STOCK_ID = stockData.ID;
                addData.BARCODE = stockData.BARCODE;
                addData.ITEM_NO = stockData.ITEM_NO;
                addData.LOT_NUMBER = stockData.LOT_NUMBER;
                addData.NOTE = stockData.NOTE;
                addData.ITEM_CATEGORY = stockData.ITEM_CATEGORY;
                if (stockData.ITEM_CATEGORY == "平版")
                {
                    addData.PRIMARY_TRANSACTION_QTY = newQty * 10;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + (newQty * 10);
                    addData.SECONDARY_TRANSACTION_QTY = newQty;
                    addData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY + newQty;
                }
                else if (stockData.ITEM_CATEGORY == "捲筒")
                {
                    addData.PRIMARY_TRANSACTION_QTY = newQty;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + newQty;
                    addData.SECONDARY_TRANSACTION_QTY = 0;
                    addData.SECONDARY_AVAILABLE_QTY = 0;
                }
                addData.PRIMARY_UOM_CODE = stockData.PRIMARY_UOM_CODE;
                addData.SECONDARY_UOM_CODE = stockData.SECONDARY_UOM_CODE;
                addData.LOCATOR_ID = stockData.LOCATOR_ID;
                addData.SEGMENT3 = stockData.SEGMENT3;
                addData.SUBINVENTORY_CODE = stockData.SUBINVENTORY_CODE;
                addData.LAST_UPDATE_DATE = DateTime.Now;
                addData.STATUS = "未異動存檔";
                addData.PapaerType = stockData.PapaerType;
                addData.BasicWeight = stockData.BasicWeight;
                addData.Specification = stockData.Specification;
                addData.OSP_BATCH_NO = stockData.OSP_BATCH_NO;
                addData.ITEM_DESCRIPTION = stockData.ITEM_DESCRIPTION;
                profitModel.Add(addData);
            }
            return new ResultModel(true, "新增明細成功");
        }

        public List<StockInventoryDT> CreateProfitDetail(List<StockInventoryDT> dataList)
        {
            List<StockInventoryDT> createList = new List<StockInventoryDT>();
            List<StockDT> itemList = StockData.GetStockItemData(dataList[0].SUBINVENTORY_CODE, dataList[0].ITEM_NO);
            if (itemList.Count == 0)
            {
                return createList;
            }

            StockInventoryDT addData = new StockInventoryDT();
            var highestId = profitModel.Any() ? profitModel.Select(x => x.ID).Max() : 0;
            addData.ID = highestId + 1;
            addData.STOCK_ID = 0;
            addData.BARCODE = GetNewBarcode(dataList[0].SUBINVENTORY_CODE);
            addData.ITEM_NO = dataList[0].ITEM_NO;
            addData.LOT_NUMBER = dataList[0].LOT_NUMBER;
            addData.NOTE = dataList[0].NOTE;
            if (itemList[0].ITEM_CATEGORY == "平版")
            {
                addData.PRIMARY_TRANSACTION_QTY = dataList[0].SECONDARY_TRANSACTION_QTY * 10;
                addData.PRIMARY_AVAILABLE_QTY = dataList[0].SECONDARY_TRANSACTION_QTY * 10;
                addData.SECONDARY_TRANSACTION_QTY = dataList[0].SECONDARY_TRANSACTION_QTY;
                addData.SECONDARY_AVAILABLE_QTY = dataList[0].SECONDARY_TRANSACTION_QTY;
            }
            else if (itemList[0].ITEM_CATEGORY == "捲筒")
            {
                addData.PRIMARY_TRANSACTION_QTY = dataList[0].PRIMARY_TRANSACTION_QTY;
                addData.PRIMARY_AVAILABLE_QTY = dataList[0].PRIMARY_TRANSACTION_QTY;
                addData.SECONDARY_TRANSACTION_QTY = 0;
                addData.SECONDARY_AVAILABLE_QTY = 0;
            }
            addData.PRIMARY_UOM_CODE = itemList[0].PRIMARY_UOM_CODE;
            addData.SECONDARY_UOM_CODE = itemList[0].SECONDARY_UOM_CODE;
            addData.LOCATOR_ID = dataList[0].LOCATOR_ID;
            addData.SEGMENT3 = dataList[0].SEGMENT3;
            addData.SUBINVENTORY_CODE = dataList[0].SUBINVENTORY_CODE;
            addData.LAST_UPDATE_DATE = DateTime.Now;
            addData.STATUS = "未異動存檔";
            addData.PapaerType = itemList[0].PapaerType;
            addData.BasicWeight = itemList[0].BasicWeight;
            addData.Specification = itemList[0].Specification;
            addData.OSP_BATCH_NO = itemList[0].OSP_BATCH_NO;
            addData.ITEM_DESCRIPTION = itemList[0].ITEM_DESCRIPTION;
            createList.Add(addData);
            profitModel.Add(addData);
            return createList;

        }

        public List<StockInventoryDT> DelLossDetail(List<StockInventoryDT> dataList)
        {
            List<StockInventoryDT> removeList = new List<StockInventoryDT>();
            foreach (StockInventoryDT data in dataList)
            {
                foreach (StockInventoryDT local in lossModel)
                {
                    if (data.ID == local.ID)
                    {
                        removeList.Add(local);
                    }
                }
            }
            lossModel.RemoveAll(x => removeList.Contains(x));
            return removeList;
        }

        public List<StockInventoryDT> DelProfitDetail(List<StockInventoryDT> dataList)
        {
            List<StockInventoryDT> removeList = new List<StockInventoryDT>();
            foreach (StockInventoryDT data in dataList)
            {
                foreach (StockInventoryDT local in profitModel)
                {
                    if (data.ID == local.ID)
                    {
                        removeList.Add(local);
                    }
                }
            }
            profitModel.RemoveAll(x => removeList.Contains(x));
            return removeList;
        }

        public List<StockInventoryDT> UpdateLossDetail(List<StockInventoryDT> dataList)
        {
            List<StockInventoryDT> updateList = new List<StockInventoryDT>();
            foreach (StockInventoryDT data in dataList)
            {
                foreach (StockInventoryDT local in lossModel)
                {
                    if (data.ID == local.ID)
                    {
                        local.NOTE = data.NOTE;
                        updateList.Add(local);
                    }
                }
            }
            return updateList;
        }

        public List<StockInventoryDT> UpdateProfitDetail(List<StockInventoryDT> dataList)
        {
            List<StockInventoryDT> updateList = new List<StockInventoryDT>();
            foreach (StockInventoryDT data in dataList)
            {
                foreach (StockInventoryDT local in profitModel)
                {
                    if (data.ID == local.ID)
                    {
                        local.NOTE = data.NOTE;
                        updateList.Add(local);
                    }
                }
            }
            return updateList;
        }

        public ResultModel SaveLossDetail()
        {
            foreach (StockInventoryDT data in lossModel)
            {
                foreach (StockDT stockData in StockData.source)
                {
                    if (data.STOCK_ID == stockData.ID && data.STATUS == "未異動存檔")
                    {
                        stockData.PRIMARY_AVAILABLE_QTY = data.PRIMARY_AVAILABLE_QTY;
                        stockData.SECONDARY_AVAILABLE_QTY = data.SECONDARY_AVAILABLE_QTY;
                        stockData.NOTE = data.NOTE;
                        data.STATUS = "已異動存檔";
                    }
                }
            }
            return new ResultModel(true, "異動存檔成功");
        }

        public ResultModel SaveProfitDetail()
        {
            List<StockDT> newStcokList = new List<StockDT>();
            foreach (StockInventoryDT data in profitModel)
            {
                foreach (StockDT stockData in StockData.source)
                {
                    if (data.STOCK_ID == stockData.ID && data.STATUS == "未異動存檔")
                    {
                        stockData.PRIMARY_AVAILABLE_QTY = data.PRIMARY_AVAILABLE_QTY;
                        stockData.SECONDARY_AVAILABLE_QTY = data.SECONDARY_AVAILABLE_QTY;
                        stockData.NOTE = data.NOTE;
                        data.STATUS = "已異動存檔";
                    }
                }
                if (data.STOCK_ID == 0)
                {
                    List<StockDT> stock = StockData.GetStockItemData(data.SUBINVENTORY_CODE, data.ITEM_NO);
                    var stockId = StockData.source.Any() ? StockData.source.Select(x => x.ID).Max() : 0;
                    StockDT newStcok = new StockDT(stockId + 1, stock[0].ORGANIZATION_ID, stock[0].ORGANIZATION_NAME, stock[0].SUBINVENTORY_CODE,
                        stock[0].SUBINVENTORY_NAME, stock[0].LOCATOR_ID, stock[0].LOCATOR_TYPE, stock[0].SEGMENT3, stock[0].INVENTORY_ITEM_ID, stock[0].ITEM_NO,
                        stock[0].OSP_BATCH_NO, stock[0].ITEM_DESCRIPTION, stock[0].ITEM_CATEGORY, data.LOT_NUMBER, data.BARCODE,
                        stock[0].PapaerType, stock[0].BasicWeight, stock[0].Specification, stock[0].PACKING_TYPE, stock[0].PRIMARY_UOM_CODE,
                        data.PRIMARY_TRANSACTION_QTY, data.PRIMARY_TRANSACTION_QTY, stock[0].SECONDARY_UOM_CODE, data.SECONDARY_TRANSACTION_QTY,
                        data.SECONDARY_TRANSACTION_QTY, "", "", data.NOTE, "", 4, "華紙管理員", "2020-05-26", 4, "華紙管理員", "2020-05-26");
                    newStcokList.Add(newStcok);
                    data.STATUS = "已異動存檔";
                }
            }
            if (newStcokList != null)
            {
                foreach (StockDT newStockData in newStcokList)
                {
                    StockData.source.Add(newStockData);
                }
            }
            return new ResultModel(true, "異動存檔成功");
        }

        public string GetNewBarcode(string IN_SUBINVENTORY_CODE)
        {
            var query = from data in profitModel
                        where data.STOCK_ID == 0
                        orderby data.BARCODE descending
                        select data.BARCODE;
            var barcodeList = query.ToList();
            if (barcodeList.Count > 0)
            {
                string barcode = barcodeList[0];
                int number = Convert.ToInt32(barcode.Substring(barcode.Length - 4));
                number = number + 1;
                string newNumber = number.ToString().PadLeft(4, '0');
                var barcodeBuilder = new StringBuilder(barcode);
                barcodeBuilder.Remove(7, 4);
                barcodeBuilder.Insert(7, newNumber);
                return barcodeBuilder.ToString();
            }



            var stockBarcodeQuery = from data in StockData.source
                                    where IN_SUBINVENTORY_CODE == data.SUBINVENTORY_CODE
                                    orderby data.BARCODE descending
                                    select data.BARCODE;
            var stockBarcodeList = query.ToList();
            if (stockBarcodeList.Count > 0)
            {
                string barcode = stockBarcodeList[0];
                int number = Convert.ToInt32(barcode.Substring(barcode.Length - 4));
                number = number + 1;
                string newNumber = number.ToString().PadLeft(4, '0');
                var barcodeBuilder = new StringBuilder(barcode);
                barcodeBuilder.Remove(7, 4);
                barcodeBuilder.Insert(7, newNumber);
                return barcodeBuilder.ToString();
            }
            else
            {
                string prefixBarcode = orgData.GetBarodePrefixCode(IN_SUBINVENTORY_CODE);
                return prefixBarcode + DateTime.Now.ToString("yyMMdd") + "0001";
            }

        }

        public List<StockInventoryDT> GetProfitBarcodeData(string BARCODE)
        {
            var query = from data in profitModel
                        where BARCODE == data.BARCODE
                        select data;

            return query.ToList();
        }

        #endregion

    }


    internal class StockInventoryDTOrder
    {
        public static IOrderedEnumerable<StockInventoryDT> Order(List<Order> orders, IEnumerable<StockInventoryDT> models)
        {
            IOrderedEnumerable<StockInventoryDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockInventoryDT> OrderBy(int column, string dir, IEnumerable<StockInventoryDT> models)
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_TRANSACTION_QTY) : models.OrderBy(x => x.PRIMARY_AVAILABLE_QTY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_AVAILABLE_QTY) : models.OrderBy(x => x.PRIMARY_AVAILABLE_QTY);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_UOM_CODE) : models.OrderBy(x => x.PRIMARY_UOM_CODE);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_TRANSACTION_QTY) : models.OrderBy(x => x.SECONDARY_TRANSACTION_QTY);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_AVAILABLE_QTY) : models.OrderBy(x => x.SECONDARY_AVAILABLE_QTY);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_UOM_CODE) : models.OrderBy(x => x.SECONDARY_UOM_CODE);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.NOTE) : models.OrderBy(x => x.NOTE);
            }
        }

        private static IOrderedEnumerable<StockInventoryDT> ThenBy(int column, string dir, IOrderedEnumerable<StockInventoryDT> models)
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_TRANSACTION_QTY) : models.ThenBy(x => x.PRIMARY_AVAILABLE_QTY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_AVAILABLE_QTY) : models.ThenBy(x => x.PRIMARY_AVAILABLE_QTY);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_UOM_CODE) : models.ThenBy(x => x.PRIMARY_UOM_CODE);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_TRANSACTION_QTY) : models.ThenBy(x => x.SECONDARY_TRANSACTION_QTY);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_AVAILABLE_QTY) : models.ThenBy(x => x.SECONDARY_AVAILABLE_QTY);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_UOM_CODE) : models.ThenBy(x => x.SECONDARY_UOM_CODE);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.NOTE) : models.ThenBy(x => x.NOTE);
            }
        }
    }

    public class StockInventoryDTEditor
    {
        public string Action { get; set; }
        public List<StockInventoryDT> StockInventoryDTList { get; set; }
    }
}