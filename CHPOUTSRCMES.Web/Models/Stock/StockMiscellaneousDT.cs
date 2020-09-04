using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Miscellaneous;
using CHPOUTSRCMES.Web.ViewModels.StockInvetory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockMiscellaneousDT
    {
        public long ID { set; get; } //TRANSFER_MISCELLANEOUS_ID
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

    public class StockMiscellaneousData
    {
        public static List<StockMiscellaneousDT> model = new List<StockMiscellaneousDT>();

        public static void resetData()
        {
            model = new List<StockMiscellaneousDT>();
        }

        public MiscellaneousViewModel GetMiscellaneousViewModel(MiscellaneousUOW uow)
        {
            MiscellaneousViewModel viewModel = new MiscellaneousViewModel();
            viewModel.SelectedMiscellaneous = "請選擇";
            //List<ListItem> miscellaneousList = new List<ListItem>();
            //miscellaneousList.Add(new ListItem("請選擇", "請選擇"));
            //miscellaneousList.Add(new ListItem("雜發", "雜發"));
            //miscellaneousList.Add(new ListItem("雜收", "雜收"));

            viewModel.MiscellaneousItems = uow.GetMiscellaneousTypeDropDownList();

            viewModel.SearchQty = "";
            viewModel.PercentageError = "";
            viewModel.Qty = "";


            return viewModel;
        }

        //public MiscellaneousSendViewModel GetMiscellaneousSendViewModel()
        //{
        //    return new MiscellaneousSendViewModel();
        //}

        //public MiscellaneousReceiveViewModel GetMiscellaneousReceiveViewModel()
        //{
        //    return new MiscellaneousReceiveViewModel();
        //}

        public List<StockDT> SearchStock(MiscellaneousUOW uow,long organizationId, string subinventoryCode, long? locatorId, string itemNumber, decimal primaryQty, decimal percentageError)
        {
            return uow.GetStockTList(organizationId, subinventoryCode, locatorId, itemNumber, primaryQty, percentageError);
        }

        public List<StockMiscellaneousDT> GetMiscellaneousData(MiscellaneousUOW uow, string userId, long transactionTypeId)
        {
            return uow.GetStockMiscellaneousTList(userId, transactionTypeId);
        }

        public List<StockMiscellaneousDT> GetModel()
        {
            var query = from data in model
                        select data;
            return query.ToList();
        }

        public ResultModel CreateDetail(MiscellaneousUOW uow, long transactionTypeId,
      long stockId, decimal mPrimaryQty, string note, string userId, string userName)
        {
            return uow.CreateDetail(transactionTypeId, stockId, mPrimaryQty, note, userId, userName);
        }

        public ResultModel AddTransactionDetail(long ID, string Miscellaneous, decimal PrimaryQty, string Note)
        {
            if (PrimaryQty > 1)
            {
                return new ResultModel(false, "超過最大數量限制1KG");
            }
            if (Miscellaneous == "雜發")
            {
                PrimaryQty = -PrimaryQty;
            }
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

                StockMiscellaneousDT addData = new StockMiscellaneousDT();
                var highestId = model.Any() ? model.Select(x => x.SUB_ID).Max() : 0;
                addData.SUB_ID = highestId + 1;
                addData.STOCK_ID = stockData.ID;
                addData.BARCODE = stockData.BARCODE;
                addData.ITEM_NO = stockData.ITEM_NO;
                addData.NOTE = Note;
                if (stockData.ITEM_CATEGORY == "平版")
                {
                    addData.PRIMARY_TRANSACTION_QTY = PrimaryQty;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + PrimaryQty;
                    addData.SECONDARY_TRANSACTION_QTY = PrimaryQty / 10;
                    addData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY + (PrimaryQty / 10);
                }
                else if (stockData.ITEM_CATEGORY == "捲筒")
                {
                    addData.PRIMARY_TRANSACTION_QTY = PrimaryQty;
                    addData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + PrimaryQty;
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

        public ResultModel SaveTransactionDetail(MiscellaneousUOW uow, long transactionTypeId, string userId, string userName)
        {
            return uow.SaveTransactionDetail(transactionTypeId, userId, userName);

            //foreach (StockMiscellaneousDT detailData in model)
            //{
            //    foreach (StockDT stockData in StockData.source)
            //    {
            //        if (detailData.STOCK_ID == stockData.ID && detailData.STATUS == "未異動存檔")
            //        {
            //            stockData.PRIMARY_AVAILABLE_QTY = detailData.PRIMARY_AVAILABLE_QTY;
            //            stockData.SECONDARY_AVAILABLE_QTY = detailData.SECONDARY_AVAILABLE_QTY;
            //            //stockData.NOTE = detailData.NOTE;
            //            detailData.STATUS = "已異動存檔";
            //        }
            //    }
            //}
            //resetData();
            //return new ResultModel(true, "異動存檔成功");
        }

        public List<StockMiscellaneousDT> UpdateRemark(StockMiscellaneousDTEditor data)
        {
            List<StockMiscellaneousDT> result = new List<StockMiscellaneousDT>();

            foreach (var barcodeData in model)
            {
                foreach (var selectedData in data.StockMiscellaneousDTList)
                {
                    if (barcodeData.SUB_ID == selectedData.SUB_ID)
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
            if (model.RemoveAll(x => IDs.Contains(x.SUB_ID)) > 0)
            {
                return new ResultModel(true, "異動明細刪除成功");
            }
            else
            {
                return new ResultModel(false, "異動明細刪除失敗");
            }

        }

        public ResultModel DetailEditor(MiscellaneousUOW uow, StockMiscellaneousDTEditor editor, string userId, string userName)
        {
            if (editor.Action == "remove")
            {
                var ids = editor.StockMiscellaneousDTList.Select(x => x.ID).ToList();
                return uow.DelDetailData(ids);
            }
            else if (editor.Action == "edit")
            {
                var ids = editor.StockMiscellaneousDTList.Select(x => x.ID).ToList();
                string note = editor.StockMiscellaneousDTList[0].NOTE;
                return uow.UpdateDetailNote(ids, note, userId, userName);
            }
            else
            {
                return new ResultModel(false, "無法識別作業項目");
            }
        }
    }

    internal class StockMiscellaneousDTOrder
    {
        public static IOrderedEnumerable<StockMiscellaneousDT> Order(List<Order> orders, IEnumerable<StockMiscellaneousDT> models)
        {
            IOrderedEnumerable<StockMiscellaneousDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockMiscellaneousDT> OrderBy(int column, string dir, IEnumerable<StockMiscellaneousDT> models)
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

        private static IOrderedEnumerable<StockMiscellaneousDT> ThenBy(int column, string dir, IOrderedEnumerable<StockMiscellaneousDT> models)
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

    public class StockMiscellaneousDTEditor
    {
        public string Action { get; set; }

        public List<StockMiscellaneousDT> StockMiscellaneousDTList { get; set; }
    }
}