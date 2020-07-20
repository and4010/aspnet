using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels.StockTransaction;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.Jsons.Requests;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockTransferDT
    {
        public long ID { get; set; }


        [Display(Name = "料號")]
        public string ITEM_NUMBER { get; set; }

        //[Display(Name = "基重")]
        //public string Base_Weight { get; set; }

        //[Display(Name = "紙別")]
        //public string PAPERTYPE { get; set; }

        //[Display(Name = "規格")]
        //public string Specification { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { get; set; }

        [Display(Name = "捲數/板數")]
        public decimal ROLL_REAM_QTY { get; set; }

        [Display(Name = "捲板單位")]
        public string ROLL_REAM_UOM { get; set; }

        [Display(Name = "需求數量")] //預計出庫量 主要數量
        public decimal REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //主單位已揀數合計
        public decimal PICKED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //入庫主單位已揀數合計
        public decimal INBOUND_PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //主單位
        public string REQUESTED_QUANTITY_UOM { get; set; }

        [Display(Name = "需求數量")] //預計出庫輔數量 次要數量
        public decimal REQUESTED_QUANTITY2 { get; set; }

        [Display(Name = "已揀數量")] //出庫已揀輔數量
        public decimal PICKED_QUANTITY2 { get; set; }

        [Display(Name = "已揀數量")] //入庫已揀輔數量
        public decimal INBOUND_PICKED_QUANTITY2 { get; set; }

        [Display(Name = "單位")] //輔單位
        public string SRC_REQUESTED_QUANTITY_UOM2 { get; set; }

        [Display(Name = "每棧令數")]
        public decimal ROLL_REAM_WT { get; set; }


        //[Display(Name = "需求數量")] //訂單原始數量 交易數量
        //public decimal SRC_REQUESTED_QUANTITY { get; set; }

        //[Display(Name = "已揀數量")] //交易單位已揀數合計 由主單位已揀數合計 換算過來
        //public decimal SRC_PICKED_QUANTITY { get; set; }

        //[Display(Name = "單位")] //交易單位
        //public string SRC_REQUESTED_QUANTITY_UOM { get; set; }

        //[Display(Name = "備註")]
        //public string REMARK { get; set; }



        [Display(Name = "建立人員ID")]
        public long CREATED_BY { get; set; }
        [Display(Name = "建立人員名稱")]
        public string CREATE_BY_USERNAME { set; get; }
        [Display(Name = "建立日期")]
        public DateTime CREATION_DATE { get; set; }


        [Display(Name = "更新人員ID")]
        public long LAST_UPDATED_BY { get; set; }
        [Display(Name = "更新人員名稱")]
        public long LAST_UPDATED_BY_USERNAME { get; set; }
        [Display(Name = "更新日期")]
        public DateTime LAST_UPDATE_DATE { get; set; }

        [Display(Name = "出貨編號")]
        public string SHIPMENT_NUMBER { get; set; }

        [Display(Name = "移轉編號")]
        public string SUBINVENTORY_TRANSFER_NUMBER { get; set; }

        [Display(Name = "編號狀態")]
        public string NUMBER_STATUS { get; set; }

        [Display(Name = "發貨倉庫")]
        public string OUT_SUBINVENTORY_CODE { get; set; }

        [Display(Name = "發貨儲位")]
        public string OUT_LOCATOR_ID { get; set; }

        [Display(Name = "收貨倉庫")]
        public string IN_SUBINVENTORY_CODE { get; set; }

        [Display(Name = "收貨儲位")]
        public string IN_LOCATOR_ID { get; set; }
    }

    public class StockTransferData
    {
        public static List<StockTransferDT> model = new List<StockTransferDT>();
        public List<StockTransferDT> importModel = new List<StockTransferDT>();
        public OrgSubinventoryData orgData = new OrgSubinventoryData();


        public static void resetData()
        {
            model = new List<StockTransferDT>();
        }


        public static StockTransferViewModel GetViewModel()
        {
            StockTransferViewModel viewModel = new StockTransferViewModel();
            viewModel.SelectedTransferType = "請選擇";
            List<ListItem> transferTypeList = new List<ListItem>();
            transferTypeList.Add(new ListItem("請選擇", "請選擇"));
            transferTypeList.Add(new ListItem("出庫", "出庫"));
            transferTypeList.Add(new ListItem("入庫", "入庫"));
            transferTypeList.Add(new ListItem("貨故", "貨故"));
            viewModel.TransferTypeItems = transferTypeList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });

            //viewModel.SelectedOutSubinventory = "";


            //viewModel.OutSubinventoryItems = orgData.GetSubinventoryList("265", false);


            //viewModel.OutLocatorItems = orgData.GetLocatorList("265", viewModel.SelectedOutSubinventory, false);

            return viewModel;
        }

        public OutBoundViewModel GetOutBoundViewModel()
        {
            OutBoundViewModel viewModel = new OutBoundViewModel();

            viewModel.OutSubinventoryItems = orgData.GetSubinventoryList("265", false);

            viewModel.OutLocatorItems = orgData.GetLocatorList("265", viewModel.SelectedOutSubinventory, false);

            viewModel.InSubinventoryItems = orgData.GetSubinventoryList("*", false);

            viewModel.InLocatorItems = orgData.GetLocatorList("*", viewModel.SelectedInSubinventory, false);

            viewModel.ShipmentNumberItems = GetShipmentNumberList(viewModel.SelectedOutSubinventory, viewModel.SelectedOutLocator, viewModel.SelectedInSubinventory, viewModel.SelectedInLocator);

            viewModel.SubinventoryTransferNumberItems = GetSubinventoryTransferNumberList(viewModel.SelectedOutSubinventory, viewModel.SelectedOutLocator, viewModel.SelectedInSubinventory, viewModel.SelectedInLocator);

            viewModel.ItemNumberItems = StockData.GetItemNumberList(viewModel.SelectedOutSubinventory, Convert.ToInt64(viewModel.SelectedOutLocator));

            //ResultModel result = CheckTransactionType(viewModel.SelectedOutSubinventory, viewModel.SelectedInSubinventory);
            //if (result.Success == false)
            //{
            //    viewModel.DisplayDetail = false;
            //    viewModel.DisplayShipmentNumberArea = false;
            //    viewModel.DisplaySubinventoryTransferNumberArea = false;
            //}
            //else
            //{
            //    viewModel.DisplayDetail = true;
            //    if (result.Msg == "倉庫間移轉")
            //    {
            //        viewModel.DisplayShipmentNumberArea = false;
            //        viewModel.DisplaySubinventoryTransferNumberArea = true;
            //    }
            //    else
            //    {
            //        viewModel.DisplayShipmentNumberArea = true;
            //        viewModel.DisplaySubinventoryTransferNumberArea = false;
            //    }
            //}

            return viewModel;
        }

        public InBoundViewModel GetInBoundViewModel()
        {
            InBoundViewModel viewModel = new InBoundViewModel();

            viewModel.OutSubinventoryItems = orgData.GetSubinventoryList("*", false);

            viewModel.OutLocatorItems = orgData.GetLocatorList("*", viewModel.SelectedOutSubinventory, false);

            viewModel.InSubinventoryItems = orgData.GetSubinventoryList("265", false);

            viewModel.InLocatorItems = orgData.GetLocatorList("265", viewModel.SelectedInSubinventory, false);

            viewModel.ShipmentNumberItems = GetShipmentNumberList(viewModel.SelectedOutSubinventory, viewModel.SelectedOutLocator, viewModel.SelectedInSubinventory, viewModel.SelectedInLocator);



            return viewModel;
        }

        public TransferReasonViewModel GetTransferReasonViewModel()
        {

            TransferReasonViewModel viewModel = new TransferReasonViewModel();
            viewModel.ReasonItems = orgData.GetReasonList();
            viewModel.LocatorItems = orgData.GetLocatorList("265", viewModel.Locator, false);

            return viewModel;
        }



        public IEnumerable<SelectListItem> GetLocatorList(string SUBINVENTORY_CODE, bool needAll)
        {
            return orgData.GetLocatorList("265", SUBINVENTORY_CODE, needAll);
        }

        public ResultModel CheckTransactionType(string outSubinventory, string inSubinventory)
        {
            if (outSubinventory == null || outSubinventory == "請選擇") return new ResultModel(false, "");
            if (inSubinventory == null || inSubinventory == "請選擇") return new ResultModel(false, "");

            if (orgData.getORGANIZATION_CODE(outSubinventory) == orgData.getORGANIZATION_CODE(inSubinventory))
            {
                return new ResultModel(true, "倉庫間移轉"); //倉庫間移轉
            }
            else
            {
                return new ResultModel(true, "組織間移轉"); //組織間移轉
            }
        }

        public List<StockTransferDT> GetModelFromShipmentNumber(string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string shipmentNumber)
        {
            var query = from stockTransferDT in model
                        where OutSubinventoryCode == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OutLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        InSubinventoryCode == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        InLocator == stockTransferDT.IN_LOCATOR_ID &&
                        shipmentNumber == stockTransferDT.SHIPMENT_NUMBER
                        select stockTransferDT;

            return query.ToList();
        }

        public List<StockTransferDT> GetModelFromSubinventoryTransferNumber(string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string subinventoryTransferNumber)
        {
            var query = from stockTransferDT in model
                        where OutSubinventoryCode == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OutLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        InSubinventoryCode == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        InLocator == stockTransferDT.IN_LOCATOR_ID &&
                        subinventoryTransferNumber == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                        select stockTransferDT;

            return query.ToList();
        }

        public IEnumerable<SelectListItem> GetShipmentNumberList(string outSubInventory, string outLocator, string inSubInventory, string inLocator)
        {
            List<ListItem> list = new List<ListItem>();

            list.Add(new ListItem("新增編號", "新增編號"));

            var query = from stockTransferDT in model
                        where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        inLocator == stockTransferDT.IN_LOCATOR_ID
                        group stockTransferDT by new { stockTransferDT.SHIPMENT_NUMBER } into g
                        select new ListItem
                        {
                            Text = g.Key.SHIPMENT_NUMBER,
                            Value = g.Key.SHIPMENT_NUMBER
                        };
            list.AddRange(query.ToList());

            return list.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public List<AutoCompletedItem> AutoCompleteShipmentNumber(string TransactionType, string outSubInventory, string outLocator, string inSubInventory, string inLocator, string Prefix)
        {
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                            inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            inLocator == stockTransferDT.IN_LOCATOR_ID &&
                            "非MES入庫手動新增" != stockTransferDT.NUMBER_STATUS &&
                            "非MES入庫檔案匯入" != stockTransferDT.NUMBER_STATUS &&
                            "非MES已入庫" != stockTransferDT.NUMBER_STATUS &&
                            stockTransferDT.SHIPMENT_NUMBER.Contains(Prefix)
                            group stockTransferDT by new { stockTransferDT.SHIPMENT_NUMBER } into g
                            select new AutoCompletedItem
                            {
                                Description = g.Key.SHIPMENT_NUMBER,
                                Value = g.Key.SHIPMENT_NUMBER
                            };

                return query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                            inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            inLocator == stockTransferDT.IN_LOCATOR_ID &&
                            "非MES入庫手動新增" != stockTransferDT.NUMBER_STATUS &&
                            "非MES入庫檔案匯入" != stockTransferDT.NUMBER_STATUS &&
                            "非MES已入庫" != stockTransferDT.NUMBER_STATUS &&
                            stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER.Contains(Prefix)
                            group stockTransferDT by new { stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER } into g
                            select new AutoCompletedItem
                            {
                                Description = g.Key.SUBINVENTORY_TRANSFER_NUMBER,
                                Value = g.Key.SUBINVENTORY_TRANSFER_NUMBER
                            };

                return query.ToList();
            }
            else
            {
                return new List<AutoCompletedItem>();
            }

        }

        public List<AutoCompletedItem> AutoCompleteShipmentNumber(string TransactionType, string outSubInventory, string outLocator, string inSubInventory, string inLocator, string Prefix, string NumberStatus)
        {
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                            inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            inLocator == stockTransferDT.IN_LOCATOR_ID &&
                            NumberStatus != stockTransferDT.NUMBER_STATUS &&
                            stockTransferDT.SHIPMENT_NUMBER.Contains(Prefix)
                            group stockTransferDT by new { stockTransferDT.SHIPMENT_NUMBER } into g
                            select new AutoCompletedItem
                            {
                                Description = g.Key.SHIPMENT_NUMBER,
                                Value = g.Key.SHIPMENT_NUMBER
                            };

                return query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                            inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            inLocator == stockTransferDT.IN_LOCATOR_ID &&
                            NumberStatus != stockTransferDT.NUMBER_STATUS &&
                            stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER.Contains(Prefix)
                            group stockTransferDT by new { stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER } into g
                            select new AutoCompletedItem
                            {
                                Description = g.Key.SUBINVENTORY_TRANSFER_NUMBER,
                                Value = g.Key.SUBINVENTORY_TRANSFER_NUMBER
                            };

                return query.ToList();
            }
            else
            {
                return new List<AutoCompletedItem>();
            }

        }



        public IEnumerable<SelectListItem> GetSubinventoryTransferNumberList(string outSubInventory, string outLocator, string inSubInventory, string inLocator)
        {
            List<ListItem> list = new List<ListItem>();

            list.Add(new ListItem("新增編號", "新增編號"));

            var query = from stockTransferDT in model
                        where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        inLocator == stockTransferDT.IN_LOCATOR_ID
                        group stockTransferDT by new { stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER } into g
                        select new ListItem
                        {
                            Text = g.Key.SUBINVENTORY_TRANSFER_NUMBER,
                            Value = g.Key.SUBINVENTORY_TRANSFER_NUMBER
                        };
            list.AddRange(query.ToList());

            return list.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public ResultModel CheckNumber(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number)
        {
            //檢查編號是否重複
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SHIPMENT_NUMBER
                            select stockTransferDT;
                List<StockTransferDT> datalist = query.ToList();
                if (datalist.Count > 0)
                {
                    if (!(datalist[0].OUT_SUBINVENTORY_CODE == OUT_SUBINVENTORY_CODE &&
                        datalist[0].OUT_LOCATOR_ID == OUT_LOCATOR_ID &&
                        datalist[0].IN_SUBINVENTORY_CODE == IN_SUBINVENTORY_CODE &&
                        datalist[0].IN_LOCATOR_ID == IN_LOCATOR_ID))
                    {
                        return new ResultModel(false, "出貨編號不可重複輸入");
                    }
                    else
                    {
                        return new ResultModel(true, "通過檢查");
                    }
                }
                else
                {
                    return new ResultModel(true, "是否新增編號?");
                }
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                            select stockTransferDT;
                List<StockTransferDT> datalist = query.ToList();
                if (datalist.Count > 0)
                {
                    if (!(datalist[0].OUT_SUBINVENTORY_CODE == OUT_SUBINVENTORY_CODE &&
                        datalist[0].OUT_LOCATOR_ID == OUT_LOCATOR_ID &&
                        datalist[0].IN_SUBINVENTORY_CODE == IN_SUBINVENTORY_CODE &&
                        datalist[0].IN_LOCATOR_ID == IN_LOCATOR_ID))
                    {
                        return new ResultModel(false, "移轉編號不可重複輸入");
                    }
                    else
                    {
                        return new ResultModel(true, "通過檢查");
                    }

                }
                else
                {
                    return new ResultModel(true, "是否新增編號?");
                }
            }
            else
            {
                return new ResultModel(false, "無法識別異動類別");
            }
        }


        public string GetShipmentNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID)
        {
            var query = from stockTransferDT in model
                        where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                        IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID
                        orderby stockTransferDT.SHIPMENT_NUMBER descending
                        select stockTransferDT.SHIPMENT_NUMBER;
            var list = query.ToList();
            int lastNumber = 0;
            if (list.Count > 0)
            {
                lastNumber = Convert.ToInt32(list[0].Substring((list[0].Length - 3) > 0 ? list[0].Length - 3 : 0)); //取後三碼流水號
            }
            lastNumber = lastNumber + 1;

            return "(" + OUT_SUBINVENTORY_CODE + "-" + IN_SUBINVENTORY_CODE + ")" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", lastNumber);
        }

        public string GetSubinventoryTransferNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID)
        {
            var query = from stockTransferDT in model
                        where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                        IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID
                        orderby stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER descending
                        select stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER;
            var list = query.ToList();
            int lastNumber = 0;
            if (list.Count > 0)
            {
                lastNumber = Convert.ToInt32(list[0].Substring((list[0].Length - 3) > 0 ? list[0].Length - 3 : 0)); //取後三碼流水號
            }
            lastNumber = lastNumber + 1;

            return "(" + OUT_SUBINVENTORY_CODE + "-" + IN_SUBINVENTORY_CODE + ")" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", lastNumber);
        }

        public ResultModel SaveStockTransferDT(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal PICKED_QTY, string UNIT, decimal ROLL_REAM_QTY)
        {
            ////檢查編號是否重複
            //if (TransactionType == "出貨編號")
            //{
            //    var query = from stockTransferDT in model
            //                where Number == stockTransferDT.SHIPMENT_NUMBER 
            //                select stockTransferDT;
            //    List<StockTransferDT> datalist = query.ToList();
            //    if (datalist.Count > 0)
            //    {
            //        if (!(datalist[0].OUT_SUBINVENTORY_CODE == OUT_SUBINVENTORY_CODE &&
            //            datalist[0].OUT_LOCATOR_ID == OUT_LOCATOR_ID &&
            //            datalist[0].IN_SUBINVENTORY_CODE == IN_SUBINVENTORY_CODE &&
            //            datalist[0].IN_LOCATOR_ID == IN_LOCATOR_ID))
            //        {
            //            return new ResultModel(false, "出貨編號不可重複輸入");
            //        }
            //    }
            //    else
            //    {
            //        return new ResultModel(true, "是否新增出貨編號?");
            //    }
            //}
            if (Number == "新增編號" && TransactionType == "出貨編號")
            {
                Number = GetShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }
            else if (Number == "新增編號" && TransactionType == "移轉編號")
            {
                Number = GetSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }

            List<StockTransferDT> list = new List<StockTransferDT>();

            //檢查備貨單的料號是否存在
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SHIPMENT_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }

            if (list.Count > 0)
            {
                return new ResultModel(false, "此料號已存在備貨單中");
                //foreach(StockTransferDT stockTransferDT in model)
                //{
                //    if (list[0].ID == stockTransferDT.ID)
                //    {
                //        if (UNIT == stockTransferDT.REQUESTED_QUANTITY_UOM)
                //        {
                //            stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                //            stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                //        }
                //        else if (UNIT == stockTransferDT.SRC_REQUESTED_QUANTITY_UOM2)
                //        {
                //            stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY;
                //            stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY;
                //        }
                //        stockTransferDT.ROLL_REAM_QTY = ROLL_REAM_QTY;
                //    }
                //}
            }
            else
            {
                List<StockDT> itemList = StockData.GetStockItemData(OUT_SUBINVENTORY_CODE, ITEM_NUMBER);
                if (itemList.Count == 0)
                {
                    return new ResultModel(false, "搜尋不到料號" + ITEM_NUMBER + "資料");
                }

                var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                StockTransferDT stockTransferDT = new StockTransferDT();
                stockTransferDT.ID = highestId + 1;
                stockTransferDT.OUT_SUBINVENTORY_CODE = OUT_SUBINVENTORY_CODE;
                stockTransferDT.OUT_LOCATOR_ID = OUT_LOCATOR_ID;
                stockTransferDT.IN_SUBINVENTORY_CODE = IN_SUBINVENTORY_CODE;
                stockTransferDT.IN_LOCATOR_ID = IN_LOCATOR_ID;
                if (TransactionType == "出貨編號")
                {
                    stockTransferDT.SHIPMENT_NUMBER = Number;
                }
                else if (TransactionType == "移轉編號")
                {
                    stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER = Number;
                }
                stockTransferDT.ITEM_NUMBER = ITEM_NUMBER;
                stockTransferDT.PACKING_TYPE = itemList[0].PACKING_TYPE;
                if (UNIT == "KG")
                {
                    if (itemList[0].ITEM_CATEGORY == "捲筒")
                    {
                        stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                        stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.REQUESTED_QUANTITY2 = 0;
                        stockTransferDT.PICKED_QUANTITY2 = 0;
                        stockTransferDT.INBOUND_PICKED_QUANTITY2 = 0;
                        stockTransferDT.ROLL_REAM_UOM = "捲";
                    }
                    else if (itemList[0].ITEM_CATEGORY == "平版")
                    {
                        stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                        stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY / 10;
                        stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY / 10;
                        stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY / 10;
                        stockTransferDT.ROLL_REAM_UOM = "板";
                    }
                }
                else if (UNIT == "RE")
                {
                    stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY * 10;
                    stockTransferDT.PICKED_QUANTITY = PICKED_QTY * 10;
                    stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY * 10;
                    stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY;
                    stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY;
                    stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY;
                    stockTransferDT.ROLL_REAM_UOM = "板";
                }
                stockTransferDT.REQUESTED_QUANTITY_UOM = itemList[0].PRIMARY_UOM_CODE;
                stockTransferDT.SRC_REQUESTED_QUANTITY_UOM2 = itemList[0].SECONDARY_UOM_CODE;
                stockTransferDT.ROLL_REAM_QTY = ROLL_REAM_QTY;
                stockTransferDT.NUMBER_STATUS = "MES未出庫";
                model.Add(stockTransferDT);
                return new ResultModel(true, Number);
            }
        }

        public ResultModel CreateInbound(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
          ref string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal PICKED_QTY, string UNIT, decimal ROLL_REAM_QTY, string ITEM_CATEGORY, string NUMBER_STATUS)
        {
            if (Number == "新增編號" && TransactionType == "出貨編號")
            {
                Number = GetShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }
            else if (Number == "新增編號" && TransactionType == "移轉編號")
            {
                Number = GetSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }
            List<StockTransferDT> list = new List<StockTransferDT>();

            string newNumber = Number;
            //檢查入庫單的料號是否存在
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where newNumber == stockTransferDT.SHIPMENT_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where newNumber == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }

            if (list.Count > 0)
            {
                var data = model.FirstOrDefault(d => d.ID == list[0].ID);
                if (data != null)
                {
                    if (UNIT == "KG")
                    {
                        if (ITEM_CATEGORY == "捲筒")
                        {
                            data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + REQUESTED_QTY;
                            //data.INBOUND_PICKED_QUANTITY = data.INBOUND_PICKED_QUANTITY + PICKED_QTY;
                            //data.PICKED_QUANTITY = PICKED_QTY;
                            //data.REQUESTED_QUANTITY2 = 0;
                            //data.PICKED_QUANTITY2 = 0;
                            //data.INBOUND_PICKED_QUANTITY2 = 0;
                        }
                        else if (ITEM_CATEGORY == "平版")
                        {
                            data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + REQUESTED_QTY;
                            //stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                            //data.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                            data.REQUESTED_QUANTITY2 = data.REQUESTED_QUANTITY2 + (REQUESTED_QTY / 10);
                            //data.PICKED_QUANTITY2 = PICKED_QTY / 10;
                            //data.INBOUND_PICKED_QUANTITY2 = PICKED_QTY / 10;
                        }
                    }
                    else if (UNIT == "RE")
                    {
                        data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + (REQUESTED_QTY * 10);
                        //data.PICKED_QUANTITY = PICKED_QTY * 10;
                        //data.INBOUND_PICKED_QUANTITY = PICKED_QTY * 10;
                        data.REQUESTED_QUANTITY2 = data.REQUESTED_QUANTITY2 + REQUESTED_QTY;
                        //data.PICKED_QUANTITY2 = PICKED_QTY;
                        //data.INBOUND_PICKED_QUANTITY2 = PICKED_QTY;
                    }
                    data.ROLL_REAM_QTY = data.ROLL_REAM_QTY + ROLL_REAM_QTY;
                    //data.NUMBER_STATUS = "非MES入庫手動新增";
                    data.NUMBER_STATUS = NUMBER_STATUS;
                    return new ResultModel(true, list[0].ID.ToString());
                }
                else
                {
                    return new ResultModel(false, "無法取得入庫單料號");
                }
            }
            else
            {
                List<StockDT> itemList = StockData.GetStockItemData(IN_SUBINVENTORY_CODE, ITEM_NUMBER);
                if (itemList.Count == 0)
                {
                    return new ResultModel(false, "搜尋不到料號" + ITEM_NUMBER + "資料");
                }
                else
                {
                    var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                    StockTransferDT stockTransferDT = new StockTransferDT();
                    stockTransferDT.ID = highestId + 1;
                    stockTransferDT.OUT_SUBINVENTORY_CODE = OUT_SUBINVENTORY_CODE;
                    stockTransferDT.OUT_LOCATOR_ID = OUT_LOCATOR_ID;
                    stockTransferDT.IN_SUBINVENTORY_CODE = IN_SUBINVENTORY_CODE;
                    stockTransferDT.IN_LOCATOR_ID = IN_LOCATOR_ID;
                    if (TransactionType == "出貨編號")
                    {
                        stockTransferDT.SHIPMENT_NUMBER = Number;
                    }
                    else if (TransactionType == "移轉編號")
                    {
                        stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER = Number;
                    }
                    stockTransferDT.ITEM_NUMBER = ITEM_NUMBER;
                    stockTransferDT.PACKING_TYPE = itemList[0].PACKING_TYPE;
                    if (UNIT == "KG")
                    {
                        if (itemList[0].ITEM_CATEGORY == "捲筒")
                        {
                            stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                            stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.REQUESTED_QUANTITY2 = 0;
                            stockTransferDT.PICKED_QUANTITY2 = 0;
                            stockTransferDT.INBOUND_PICKED_QUANTITY2 = 0;
                        }
                        else if (itemList[0].ITEM_CATEGORY == "平版")
                        {
                            stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                            stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY / 10;
                            stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY / 10;
                            stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY / 10;
                        }
                    }
                    else if (UNIT == "RE")
                    {
                        stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY * 10;
                        stockTransferDT.PICKED_QUANTITY = PICKED_QTY * 10;
                        stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY * 10;
                        stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY;
                        stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY;
                        stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY;
                    }
                    stockTransferDT.REQUESTED_QUANTITY_UOM = itemList[0].PRIMARY_UOM_CODE;
                    stockTransferDT.SRC_REQUESTED_QUANTITY_UOM2 = itemList[0].SECONDARY_UOM_CODE;
                    stockTransferDT.ROLL_REAM_QTY = ROLL_REAM_QTY;
                    //stockTransferDT.NUMBER_STATUS = "非MES入庫手動新增";
                    stockTransferDT.NUMBER_STATUS = NUMBER_STATUS;
                    model.Add(stockTransferDT);
                    return new ResultModel(true, stockTransferDT.ID.ToString());
                }
            }

        }

        public ResultModel UpdateStockTransferDT(long ID, decimal PRIMARY_QUANTITY, decimal SECONDARY_QUANTITY, bool remove, bool isInbound, string barcodeStatus)
        {
            foreach (StockTransferDT stockTransferDT in model)
            {
                if (isInbound)
                {
                    if (stockTransferDT.ID == ID)
                    {
                        stockTransferDT.REQUESTED_QUANTITY = stockTransferDT.REQUESTED_QUANTITY + PRIMARY_QUANTITY;
                        stockTransferDT.REQUESTED_QUANTITY2 = stockTransferDT.REQUESTED_QUANTITY2 + SECONDARY_QUANTITY;

                        if (barcodeStatus == "已入庫")
                        {
                            stockTransferDT.INBOUND_PICKED_QUANTITY = stockTransferDT.INBOUND_PICKED_QUANTITY + PRIMARY_QUANTITY;
                            stockTransferDT.INBOUND_PICKED_QUANTITY2 = stockTransferDT.INBOUND_PICKED_QUANTITY2 + SECONDARY_QUANTITY;
                        }
                        if (remove)
                        {
                            if (stockTransferDT.INBOUND_PICKED_QUANTITY == 0 && stockTransferDT.INBOUND_PICKED_QUANTITY2 == 0)
                            {
                                model.Remove(stockTransferDT);
                            }
                        }

                        //更新棧板數
                        if (PRIMARY_QUANTITY > 0)
                        {
                            stockTransferDT.ROLL_REAM_QTY = stockTransferDT.ROLL_REAM_QTY + 1;
                        }
                        else
                        {
                            stockTransferDT.ROLL_REAM_QTY = stockTransferDT.ROLL_REAM_QTY - 1;
                        }
                        return new ResultModel(true, "更新入庫單成功");
                    }
                }
                else
                {
                    if (stockTransferDT.ID == ID)
                    {
                        stockTransferDT.PICKED_QUANTITY = stockTransferDT.PICKED_QUANTITY + PRIMARY_QUANTITY;
                        stockTransferDT.PICKED_QUANTITY2 = stockTransferDT.PICKED_QUANTITY2 + SECONDARY_QUANTITY;
                        if (remove)
                        {
                            if (stockTransferDT.PICKED_QUANTITY == 0 && stockTransferDT.PICKED_QUANTITY2 == 0)
                            {
                                model.Remove(stockTransferDT);
                            }
                        }

                        return new ResultModel(true, "更新備貨單成功");
                    }
                }
            }
            return new ResultModel(false, "找不到備貨單更新");
        }

        public ResultModel DeleteNumber(string TransactionType, string Number)
        {
            if (TransactionType == "出貨編號")
            {
                var stockTransferDTquery = from stockTransferDT in model
                                           where Number == stockTransferDT.SHIPMENT_NUMBER
                                           select stockTransferDT;
                var stockTransferDTList = stockTransferDTquery.ToList();

                if (stockTransferDTList.Count == 0)
                {
                    return new ResultModel(true, "沒有編號資料可以刪除");
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    StockTransferBarcodeData.model.RemoveAll((x) => x.StockTransferDT_ID == data.ID);
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    model.Remove(data);
                }

                return new ResultModel(true, "此編號所有資料刪除成功");
            }
            else if (TransactionType == "移轉編號")
            {
                var stockTransferDTquery = from stockTransferDT in model
                                           where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                                           select stockTransferDT;
                var stockTransferDTList = stockTransferDTquery.ToList();

                if (stockTransferDTList.Count == 0)
                {
                    return new ResultModel(true, "沒有編號資料可以刪除");
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    StockTransferBarcodeData.model.RemoveAll((x) => x.StockTransferDT_ID == data.ID);
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    model.Remove(data);
                }

                return new ResultModel(true, "此編號所有資料刪除成功");
            }
            else
            {
                return new ResultModel(false, "無法識別異動類別");
            }
        }

        public ResultModel DeleteItemNumber(long ID, bool isInbound)
        {
            var query = from data in StockTransferBarcodeData.model
                        where ID == data.StockTransferDT_ID
                        select data.ID;
            var removeBarcodeIDs = query.ToList();
            if (removeBarcodeIDs.Count > 0)
            {
                bool removeResult = false;
                bool removeResult2 = false;
                var barcodeQuery = from data in StockTransferBarcodeData.model
                                   where removeBarcodeIDs.Contains(data.ID)
                                   select data;
                var removeList = barcodeQuery.ToList();
                if (removeList.Count == 0)
                {
                    return new ResultModel(false, "找不到條碼刪除");
                }

                foreach (StockTransferBarcodeDT barcode in removeList)
                {
                    removeResult = UpdateStockTransferDT(barcode.StockTransferDT_ID, -barcode.PRIMARY_QUANTITY, -barcode.SECONDARY_QUANTITY, true, isInbound, barcode.Status).Success;
                    removeResult2 = StockTransferBarcodeData.model.Remove(barcode);
                }
                if (removeResult && removeResult2)
                {
                    if (model.Count > 0)
                    {
                        return new ResultModel(true, "刪除料號成功");
                    }
                    else
                    {
                        return new ResultModel(true, "刪除料號成功，備貨單已沒任何資料");
                    }

                }
                else
                {
                    return new ResultModel(false, "刪除料號失敗");
                }
            }
            else
            {
                var itemQuery = from data in model
                                where ID == data.ID
                                select data;
                var removeList = itemQuery.ToList();
                if (removeList.Count > 0)
                {
                    if (model.Remove(removeList[0]))
                    {
                        if (model.Count > 0)
                        {
                            return new ResultModel(true, "刪除料號成功");
                        }
                        else
                        {
                            return new ResultModel(true, "刪除料號成功，備貨單已沒任何資料");
                        }
                    }
                    else
                    {
                        return new ResultModel(false, "料號刪除失敗");
                    }
                }
                else
                {
                    return new ResultModel(false, "料號刪除失敗");
                }
            }


        }

        public ResultModel GetNumberStatus(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string Number)
        {
            List<StockTransferDT> list = new List<StockTransferDT>();

            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                            IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&
                            Number == stockTransferDT.SHIPMENT_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                            IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&
                            Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else
            {
                return new ResultModel(false, "無法識別異動類別");
            }

            if (list.Count > 0)
            {
                return new ResultModel(true, list[0].NUMBER_STATUS);
            }
            else
            {
                return new ResultModel(true, "新增"); //沒編號資料回傳狀態回新增
            }

        }


        public List<StockTransferDT> GetStockTransferData(long ID)
        {
            var query = from stockTransferDT in model
                        where ID == stockTransferDT.ID
                        select stockTransferDT;
            return query.ToList();
        }

        //儲位異動出庫存檔
        public ResultModel OutBoundSaveTransfer(string TransactionType, string Number)
        {
            List<StockTransferDT> list = new List<StockTransferDT>();
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SHIPMENT_NUMBER
                            select stockTransferDT;
                list = query.ToList();
                if (list.Count == 0)
                {
                    return new ResultModel(false, "出貨編號" + Number + "沒有資料");
                }
            }

            if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                            select stockTransferDT;
                list = query.ToList();
                if (list.Count == 0)
                {
                    return new ResultModel(false, "移轉編號" + Number + "沒有資料");
                }
            }

            //棧板數/捲數檢查
            foreach (StockTransferDT data in list)
            {
                var query = from barcodeData in StockTransferBarcodeData.model
                            where data.ID == barcodeData.StockTransferDT_ID
                            select barcodeData.BARCODE;
                decimal pickedCount = Convert.ToDecimal(query.ToList().Count);

                if (pickedCount != data.ROLL_REAM_QTY)
                {
                    return new ResultModel(false, "料號" + data.ITEM_NUMBER + "的棧板數、捲數和輸入的條碼筆數不同");
                }
            }


            //已揀數量檢查
            foreach (StockTransferDT data in list)
            {
                if (data.REQUESTED_QUANTITY != data.PICKED_QUANTITY)
                {
                    return new ResultModel(false, "料號" + data.ITEM_NUMBER + "主單位已揀數量不等於需求數量");
                }
                if (data.REQUESTED_QUANTITY2 != data.PICKED_QUANTITY2)
                {
                    return new ResultModel(false, "料號" + data.ITEM_NUMBER + "次單位已揀數量不等於需求數量");
                }
            }

            //庫存扣除
            foreach (StockTransferBarcodeDT barcodeData in StockTransferBarcodeData.model)
            {
                foreach (StockDT stockData in StockData.source)
                {
                    if (TransactionType == "出貨編號")
                    {
                        if (barcodeData.SHIPMENT_NUMBER == Number && stockData.BARCODE == barcodeData.BARCODE)
                        {
                            stockData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY - barcodeData.PRIMARY_QUANTITY;
                            stockData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY - barcodeData.SECONDARY_QUANTITY;
                        }
                    }

                    if (TransactionType == "移轉編號")
                    {
                        if (barcodeData.SUBINVENTORY_TRANSFER_NUMBER == Number && stockData.BARCODE == barcodeData.BARCODE)
                        {
                            stockData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY - barcodeData.PRIMARY_QUANTITY;
                            stockData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY - barcodeData.SECONDARY_QUANTITY;
                        }
                    }
                }
            }

            //變更編號狀態
            foreach (StockTransferDT data in model)
            {
                if (TransactionType == "出貨編號")
                {
                    if (data.SHIPMENT_NUMBER == Number)
                    {
                        data.NUMBER_STATUS = "MES已出庫";
                    }
                }
                if (TransactionType == "移轉編號")
                {
                    if (data.SUBINVENTORY_TRANSFER_NUMBER == Number)
                    {
                        data.NUMBER_STATUS = "MES已出庫";
                    }
                }
            }

            return new ResultModel(true, "出庫存檔成功");
        }

        //儲位異動入庫存檔
        public ResultModel InBoundSaveTransfer(string TransactionType, string Number)
        {
            List<StockTransferDT> list = new List<StockTransferDT>();
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SHIPMENT_NUMBER
                            select stockTransferDT;
                list = query.ToList();
                if (list.Count == 0)
                {
                    return new ResultModel(false, "出貨編號" + Number + "沒有資料");
                }
            }

            if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                            select stockTransferDT;
                list = query.ToList();
                if (list.Count == 0)
                {
                    return new ResultModel(false, "移轉編號" + Number + "沒有資料");
                }
            }

            //棧板數/捲數檢查
            foreach (StockTransferDT data in list)
            {
                var query = from barcodeData in StockTransferBarcodeData.model
                            where data.ID == barcodeData.StockTransferDT_ID
                            select barcodeData.BARCODE;
                decimal pickedCount = Convert.ToDecimal(query.ToList().Count);

                if (pickedCount != data.ROLL_REAM_QTY)
                {
                    return new ResultModel(false, "料號" + data.ITEM_NUMBER + "的棧板數、捲數和輸入的條碼筆數不同");
                }
            }


            //已揀數量檢查
            foreach (StockTransferDT data in list)
            {
                if (data.REQUESTED_QUANTITY != data.INBOUND_PICKED_QUANTITY)
                {
                    //return new ResultModel(false, "料號" + data.ITEM_NUMBER + "入庫主單位已揀數量不等於需求數量");
                    return new ResultModel(false, "料號" + data.ITEM_NUMBER + "未揀完");
                }
                if (data.REQUESTED_QUANTITY2 != data.INBOUND_PICKED_QUANTITY2)
                {
                    return new ResultModel(false, "料號" + data.ITEM_NUMBER + "未揀完");
                }
            }

            //條碼入庫狀態檢查
            foreach (StockTransferDT data in list)
            {
                var query = from barcodeData in StockTransferBarcodeData.model
                            where data.ID == barcodeData.StockTransferDT_ID &&
                            barcodeData.Status != "已入庫"
                            select barcodeData.BARCODE;
                var barcodeList = query.ToList();
                if (barcodeList.Count > 0)
                {
                    return new ResultModel(false, "條碼" + barcodeList[0] + "未揀完");
                }
            }


            //庫存增加
            foreach (StockTransferBarcodeDT barcodeData in StockTransferBarcodeData.model)
            {
                foreach (StockDT stockData in StockData.source)
                {
                    if (TransactionType == "出貨編號")
                    {
                        if (barcodeData.SHIPMENT_NUMBER == Number && stockData.BARCODE == barcodeData.BARCODE)
                        {
                            stockData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + barcodeData.PRIMARY_QUANTITY;
                            stockData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY + barcodeData.SECONDARY_QUANTITY;
                        }
                    }

                    if (TransactionType == "移轉編號")
                    {
                        if (barcodeData.SUBINVENTORY_TRANSFER_NUMBER == Number && stockData.BARCODE == barcodeData.BARCODE)
                        {
                            stockData.PRIMARY_AVAILABLE_QTY = stockData.PRIMARY_AVAILABLE_QTY + barcodeData.PRIMARY_QUANTITY;
                            stockData.SECONDARY_AVAILABLE_QTY = stockData.SECONDARY_AVAILABLE_QTY + barcodeData.SECONDARY_QUANTITY;
                        }
                    }
                }
            }

            //變更編號狀態
            foreach (StockTransferDT data in model)
            {
                if (TransactionType == "出貨編號")
                {
                    if (data.SHIPMENT_NUMBER == Number)
                    {
                        if (data.NUMBER_STATUS == "非MES入庫手動新增" || data.NUMBER_STATUS == "非MES入庫檔案匯入")
                        {
                            data.NUMBER_STATUS = "非MES已入庫";
                        }
                        else if (data.NUMBER_STATUS == "MES已出庫")
                        {
                            data.NUMBER_STATUS = "MES已入庫";
                        }
                    }
                }
                if (TransactionType == "移轉編號")
                {
                    if (data.SUBINVENTORY_TRANSFER_NUMBER == Number)
                    {
                        if (data.NUMBER_STATUS == "非MES入庫手動新增" || data.NUMBER_STATUS == "非MES入庫檔案匯入")
                        {
                            data.NUMBER_STATUS = "非MES已入庫";
                        }
                        else if (data.NUMBER_STATUS == "MES已出庫")
                        {
                            data.NUMBER_STATUS = "MES已入庫";
                        }
                    }
                }
            }

            return new ResultModel(true, "入庫存檔成功");
        }

        public ResultModel MergeBarcode(long ID, decimal PRIMARY_QUANTITY, decimal SECONDARY_QUANTITY, decimal addROLL_REAM_QTY)
        {
            var data = model.FirstOrDefault(d => d.ID == ID);
            if (data != null)
            {
                data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + PRIMARY_QUANTITY;
                data.REQUESTED_QUANTITY2 = data.REQUESTED_QUANTITY2 + SECONDARY_QUANTITY;
                data.ROLL_REAM_QTY = data.ROLL_REAM_QTY + addROLL_REAM_QTY;

                return new ResultModel(true, "更改入庫單數量成功");
            }
            else
            {
                return new ResultModel(true, "更改入庫單數量失敗，找不到ID");
            }
        }

        //public ResultModel DelROLL_REAM_QTY(long ID, decimal PRIMARY_QUANTITY, decimal SECONDARY_QUANTITY, bool addNewBarcode)
        //{

        //}
    }


    internal class StockTransferDTOrder
    {
        public static IOrderedEnumerable<StockTransferDT> Order(List<Order> orders, IEnumerable<StockTransferDT> models)
        {
            IOrderedEnumerable<StockTransferDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockTransferDT> OrderBy(int column, string dir, IEnumerable<StockTransferDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ID) : models.OrderBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PACKING_TYPE) : models.OrderBy(x => x.PACKING_TYPE);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ROLL_REAM_QTY) : models.OrderBy(x => x.ROLL_REAM_QTY);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY) : models.OrderBy(x => x.REQUESTED_QUANTITY);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY) : models.OrderBy(x => x.PICKED_QUANTITY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY2) : models.OrderBy(x => x.REQUESTED_QUANTITY2);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY2) : models.OrderBy(x => x.PICKED_QUANTITY2);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_REQUESTED_QUANTITY_UOM2) : models.OrderBy(x => x.SRC_REQUESTED_QUANTITY_UOM2);

            }
        }

        private static IOrderedEnumerable<StockTransferDT> ThenBy(int column, string dir, IOrderedEnumerable<StockTransferDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ID) : models.ThenBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PACKING_TYPE) : models.ThenBy(x => x.PACKING_TYPE);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ROLL_REAM_QTY) : models.ThenBy(x => x.ROLL_REAM_QTY);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY) : models.ThenBy(x => x.REQUESTED_QUANTITY);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY) : models.ThenBy(x => x.PICKED_QUANTITY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY2) : models.ThenBy(x => x.REQUESTED_QUANTITY2);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY2) : models.ThenBy(x => x.PICKED_QUANTITY2);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_REQUESTED_QUANTITY_UOM2) : models.ThenBy(x => x.SRC_REQUESTED_QUANTITY_UOM2);

            }
        }
    }

}