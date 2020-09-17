using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Obsolete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockObsoleteDT
    {
        public long ID { set; get; }

        public long SUB_ID { set; get; }
        public long STOCK_ID { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public string SEGMENT3 { set; get; }
        public string ITEM_NO { set; get; }
        public string BARCODE { set; get; }
        public decimal PRIMARY_TRANSACTION_QTY { set; get; }
        public decimal PRIMARY_AVAILABLE_QTY { set; get; }
        public string PRIMARY_UOM_CODE { set; get; }
        public decimal SECONDARY_TRANSACTION_QTY { set; get; }
        public decimal SECONDARY_AVAILABLE_QTY { set; get; }
        public string SECONDARY_UOM_CODE { set; get; }
        public string NOTE { set; get; }
        public DateTime LAST_UPDATE_DATE { set; get; }
        public string STATUS { set; get; }
    }

    public class StockObsoleteData
    {
        public static List<StockObsoleteDT> model = new List<StockObsoleteDT>();

        public static void resetData()
        {
            model = new List<StockObsoleteDT>();
        }

        public ObsoleteViewModel GetObsoleteViewModel()
        {
            ObsoleteViewModel viewModel = new ObsoleteViewModel();
            viewModel.Unit = "";
            viewModel.Qty = "";
            return viewModel;
        }

        public List<StockObsoleteDT> GetModel()
        {
            var query = from data in model
                        select data;
            return query.ToList();
        }

        public List<StockDT> SearchStock(ObsoleteUOW uow, long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            return uow.GetStockTList(organizationId, subinventoryCode, locatorId, itemNumber);
        }

        public ResultModel CreateDetail(ObsoleteUOW uow, long stockId, string mQty, string userId, string userName)
        {
            var result = ConvertEx.StringToDecimal(mQty);
            if (!result.Success) return new ResultModel(false, "數量須為數字");
            return uow.CreateDetail( stockId, result.Data, userId, userName);
        }

        public List<StockObsoleteDT> GetObsoleteData(ObsoleteUOW uow, string userId)
        {
            return uow.GetStockObsoleteTList(userId);
        }

        public ResultModel AddTransactionDetail(long ID, decimal ObsoleteQty)
        {
            decimal newObsoleteQty = -ObsoleteQty;
            var stockData = StockData.source.FirstOrDefault(d => d.ID == ID);
            if (stockData == null)
            {
                return new ResultModel(false, "找不到庫存");
            }
            else
            {
                var detailData = model.FirstOrDefault(d => d.BARCODE == stockData.BARCODE && d.STATUS == "未異動存檔");
                if (detailData != null)
                {
                    return new ResultModel(false, "異動記錄已存在");
                }

                StockObsoleteDT addData = new StockObsoleteDT();
                var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                addData.ID = highestId + 1;
                addData.STOCK_ID = stockData.ID;
                addData.BARCODE = stockData.BARCODE;
                addData.ITEM_NO = stockData.ITEM_NO;
                addData.NOTE = stockData.NOTE;
                if (stockData.ITEM_CATEGORY == "平版")
                {
                    addData.PRIMARY_TRANSACTION_QTY = newObsoleteQty * 10;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + (newObsoleteQty * 10);
                    addData.SECONDARY_TRANSACTION_QTY = newObsoleteQty;
                    addData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY + newObsoleteQty;
                }
                else if (stockData.ITEM_CATEGORY == "捲筒")
                {
                    addData.PRIMARY_TRANSACTION_QTY = newObsoleteQty;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + newObsoleteQty;
                    addData.SECONDARY_TRANSACTION_QTY = 0;
                    addData.SECONDARY_AVAILABLE_QTY = 0;
                }
                addData.PRIMARY_UOM_CODE = stockData.PRIMARY_UOM_CODE;
                addData.SECONDARY_UOM_CODE = stockData.SECONDARY_UOM_CODE;
                addData.SEGMENT3 = stockData.SEGMENT3;
                addData.SUBINVENTORY_CODE = stockData.SUBINVENTORY_CODE;
                addData.LAST_UPDATE_DATE = DateTime.Now;
                addData.STATUS = "未異動存檔";
                model.Add(addData);
            }
            return new ResultModel(true, "新增明細成功");
        }

        public ResultModel SaveTransactionDetail(ObsoleteUOW uow, string userId, string userName)
        {
            return uow.SaveTransactionDetail(userId, userName);
        }

        public List<StockObsoleteDT> UpdateRemark(StockObsoleteDTEditor data)
        {
            List<StockObsoleteDT> result = new List<StockObsoleteDT>();

            foreach (var barcodeData in model)
            {
                foreach (var selectedData in data.StockObsoleteDTList)
                {
                    if (barcodeData.ID == selectedData.ID)
                    {
                        barcodeData.NOTE = selectedData.NOTE;
                        result.Add(barcodeData);
                    }
                }
            }
            return result;
        }

        public ResultModel DelTransactionDetail(List<long> IDs)
        {
            if (IDs == null || IDs.Count == 0)
            {
                return new ResultModel(false, "請選擇要刪除的異動明細");
            }
            if (model.RemoveAll(x => IDs.Contains(x.ID)) > 0)
            {
                return new ResultModel(true, "異動明細刪除成功");
            }
            else
            {
                return new ResultModel(false, "異動明細刪除失敗");
            }
        }

        public ResultModel DetailEditor(ObsoleteUOW uow, StockObsoleteDTEditor editor, string userId, string userName)
        {
            if (editor.Action == "remove")
            {
                var ids = editor.StockObsoleteDTList.Select(x => x.ID).ToList();
                return uow.DelDetailData(ids);
            }
            else if (editor.Action == "edit")
            {
                var ids = editor.StockObsoleteDTList.Select(x => x.ID).ToList();
                string note = editor.StockObsoleteDTList[0].NOTE;
                return uow.UpdateDetailNote(ids, note, userId, userName);
            }
            else
            {
                return new ResultModel(false, "無法識別作業項目");
            }
        }
    }

    internal class StockObsoleteDTOrder
    {
        public static IOrderedEnumerable<StockObsoleteDT> Order(List<Order> orders, IEnumerable<StockObsoleteDT> models)
        {
            IOrderedEnumerable<StockObsoleteDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockObsoleteDT> OrderBy(int column, string dir, IEnumerable<StockObsoleteDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
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

        private static IOrderedEnumerable<StockObsoleteDT> ThenBy(int column, string dir, IOrderedEnumerable<StockObsoleteDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
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

    public class StockObsoleteDTEditor
    {
        public string Action { get; set; }

        public List<StockObsoleteDT> StockObsoleteDTList { get; set; }
    }
}