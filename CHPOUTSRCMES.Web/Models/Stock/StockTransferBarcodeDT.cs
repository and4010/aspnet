using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels.StockTransaction;
using CHPOUTSRCMES.Web.ViewModels;
using System.Text;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entiy.Transfer;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockTransferBarcodeDT
    {
        public long ID { get; set; }

        public long SUB_ID { get; set; }

        public long TransferHeaderId { get; set; }

        public long TransferDetailId { get; set; }

        [Display(Name = "出貨編號")]
        public string SHIPMENT_NUMBER { get; set; }

        [Display(Name = "移轉編號")]
        public string SUBINVENTORY_TRANSFER_NUMBER { get; set; }

        //[Display(Name = "編號狀態")]
        //public string NUMBER_STATUS { get; set; }

        [Display(Name = "條碼")]
        public string BARCODE { get; set; }

        public long StockId { get; set; }

        [Display(Name = "工單號碼")]
        public string OSP_BATCH_NO { get; set; }

        [Display(Name = "入庫狀態")]
        public string Status { get; set; }

        [Display(Name = "捲號")]
        public string LOT_NUMBER { get; set; }

        [Display(Name = "倉庫")]
        public string Subinventory { get; set; }

        [Display(Name = "儲位")]
        public string Locator { get; set; }

        [Display(Name = "基重")]
        public string Base_Weight { get; set; }

        [Display(Name = "紙別")]
        public string PAPERTYPE { get; set; }

        [Display(Name = "規格")]
        public string Specification { get; set; }

        [Display(Name = "料號")]
        public string ITEM_NUMBER { get; set; }

        public string ITEM_DESCRIPTION { get; set; }

        public string ITEM_CATEGORY { get; set; }


        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { get; set; }

        [Display(Name = "每件令數")] //每件令數
        public decimal ROLL_REAM_WT { get; set; }

        //[Display(Name = "棧板數")]
        //public decimal ROLL_REAM_QTY { get; set; }
        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "數量")] //主要數量
        public decimal PRIMARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //主要單位
        public string PRIMARY_UOM { get; set; }

        [DisplayFormat(DataFormatString = "{0}")]
        [Display(Name = "數量")] //次要數量
        public decimal SECONDARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //次要單位
        public string SECONDARY_UOM { get; set; }

        [Display(Name = "備註")]
        public string REMARK { get; set; }

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


        [Display(Name = "棧板狀態")]
        public string PALLET_STATUS { get; set; }

        [Display(Name = "櫃號")]
        public string CONTAINER_NO { get; set; }
    }



    public class StockTransferBarcodeData
    {
        StockTransferData stockTransferData = new StockTransferData();
        public static List<StockTransferBarcodeDT> model = new List<StockTransferBarcodeDT>();
        public List<StockTransferBarcodeDT> importModel = new List<StockTransferBarcodeDT>();

        public static void resetData()
        {
            model = new List<StockTransferBarcodeDT>();
        }

       

        public List<StockTransferBarcodeDT> GetModelFromShipmentNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string shipmentNumber)
        {
            var query = from stockTransferBarcodeDT in model
                        join stockTransferDT in StockTransferData.model
                        on stockTransferBarcodeDT.TransferDetailId equals stockTransferDT.ID
                        where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                            IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&
                        shipmentNumber == stockTransferBarcodeDT.SHIPMENT_NUMBER
                        select stockTransferBarcodeDT;

            return query.ToList();
        }

        public List<StockTransferBarcodeDT> GetModelFromSubinventoryTransferNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string subinventoryTransferNumber)
        {
            var query = from stockTransferBarcodeDT in model
                        join stockTransferDT in StockTransferData.model
                        on stockTransferBarcodeDT.TransferDetailId equals stockTransferDT.ID
                        where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                            IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&
                        subinventoryTransferNumber == stockTransferBarcodeDT.SUBINVENTORY_TRANSFER_NUMBER
                        select stockTransferBarcodeDT;

            return query.ToList();
        }

        //public List<StockTransferBarcodeDT> GetModelFromShipmentNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string shipmentNumber)
        //{
        //    var query = from stockTransferBarcodeDT in model
        //                join stockTransferDT in StockTransferData.model
        //                on stockTransferBarcodeDT.StockTransferDT_ID equals stockTransferDT.ID
        //                where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
        //                    OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
        //                    IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
        //                    IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&

        //                shipmentNumber == stockTransferBarcodeDT.SHIPMENT_NUMBER
        //                select stockTransferBarcodeDT;

        //    return query.ToList();
        //}

        //public List<StockTransferBarcodeDT> GetModelFromSubinventoryTransferNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string subinventoryTransferNumber)
        //{
        //    var query = from stockTransferBarcodeDT in model
        //                join stockTransferDT in StockTransferData.model
        //                on stockTransferBarcodeDT.StockTransferDT_ID equals stockTransferDT.ID
        //                where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
        //                    OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
        //                    IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
        //                    IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&

        //                subinventoryTransferNumber == stockTransferBarcodeDT.SUBINVENTORY_TRANSFER_NUMBER
        //                select stockTransferBarcodeDT;

        //    return query.ToList();
        //}

        public bool checkBarcodeExist(string BARCODE)
        {
            List<StockTransferBarcodeDT> list = new List<StockTransferBarcodeDT>();
            var query = from data in model
                        where BARCODE == data.BARCODE
                        select data;
            list = query.ToList();

            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            //List<StockTransferBarcodeDT> list = new List<StockTransferBarcodeDT>();
            //if (TransactionType == "出貨編號")
            //{
            //    var query = from data in model
            //                where Number == data.SHIPMENT_NUMBER &&
            //                BARCODE == data.BARCODE
            //                select data;
            //    list = query.ToList();
            //}
            //else
            //{
            //    var query = from data in model
            //                where Number == data.SUBINVENTORY_TRANSFER_NUMBER &&
            //                BARCODE == data.BARCODE
            //                select data;
            //    list = query.ToList();
            //}

            //if (list.Count > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public List<StockTransferBarcodeDT> GetBarcodeData(string BARCODE)
        {
            var query = from data in model
                        where BARCODE == data.BARCODE
                        select data;

            return query.ToList();
        }

        public ResultModel SaveStockTransferBarcodeDT(string TransactionType, string Number, long StockTransferDT_ID, string BARCODE, decimal? InputReamQty)
        {
            //檢查備貨單
            List<StockTransferDT> stockTransferDTList = stockTransferData.GetStockTransferData(StockTransferDT_ID);
            if (stockTransferDTList.Count == 0)
            {
                return new ResultModel(false, "找不到備貨單資料");
            }


            //檢查庫存
            List<StockDT> stockDataList = StockData.GetStockData(BARCODE);
            if (stockDataList.Count == 0)
            {
                return new ResultModel(false, "找不到此條碼庫存");
            }
            if (stockTransferDTList[0].ITEM_NUMBER != stockDataList[0].ITEM_NO)
            {
                return new ResultModel(false, "此條碼不符合已選擇的料號");
            }
            if (stockDataList[0].PRIMARY_AVAILABLE_QTY == 0)
            {
                return new ResultModel(false, "此條碼重沒庫存");
            }
            if (stockDataList[0].PACKING_TYPE == "令包")
            {
                if (InputReamQty > stockDataList[0].SECONDARY_AVAILABLE_QTY)
                {
                    return new ResultModel(false, "此條碼庫存" + stockDataList[0].SECONDARY_AVAILABLE_QTY + stockDataList[0].SECONDARY_UOM_CODE + "不足出貨");
                }
            }

            if (checkBarcodeExist(BARCODE))
            {
                return new ResultModel(false, "條碼重複輸入");
            }


            //條碼清單新增條碼
            var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
            StockTransferBarcodeDT stockTransferBarcodeDT = new StockTransferBarcodeDT();
            stockTransferBarcodeDT.ID = highestId + 1;
            stockTransferBarcodeDT.TransferDetailId = StockTransferDT_ID;
            stockTransferBarcodeDT.Subinventory = stockTransferDTList[0].OUT_SUBINVENTORY_CODE;
            if (TransactionType == "出貨編號")
            {
                stockTransferBarcodeDT.SHIPMENT_NUMBER = Number;
            }
            else
            {
                stockTransferBarcodeDT.SUBINVENTORY_TRANSFER_NUMBER = Number;
            }
            stockTransferBarcodeDT.LOT_NUMBER = stockDataList[0].LOT_NUMBER;
            stockTransferBarcodeDT.BARCODE = BARCODE;
            stockTransferBarcodeDT.Status = "待入庫";
            stockTransferBarcodeDT.ITEM_NUMBER = stockDataList[0].ITEM_NO;
            stockTransferBarcodeDT.OSP_BATCH_NO = stockDataList[0].OSP_BATCH_NO;
            stockTransferBarcodeDT.ITEM_DESCRIPTION = stockDataList[0].ITEM_DESCRIPTION;
            stockTransferBarcodeDT.ITEM_CATEGORY = stockDataList[0].ITEM_CATEGORY;
            stockTransferBarcodeDT.PAPERTYPE = stockDataList[0].PapaerType;
            stockTransferBarcodeDT.Base_Weight = stockDataList[0].BasicWeight;
            stockTransferBarcodeDT.Specification = stockDataList[0].Specification;
            stockTransferBarcodeDT.PACKING_TYPE = stockDataList[0].PACKING_TYPE;
            if (stockDataList[0].PACKING_TYPE == "令包")
            {
                stockTransferBarcodeDT.PRIMARY_QUANTITY = (decimal)InputReamQty * 10;
                stockTransferBarcodeDT.SECONDARY_QUANTITY = (decimal)InputReamQty;
            }
            else
            {
                stockTransferBarcodeDT.PRIMARY_QUANTITY = stockDataList[0].PRIMARY_AVAILABLE_QTY;
                stockTransferBarcodeDT.SECONDARY_QUANTITY = stockDataList[0].SECONDARY_AVAILABLE_QTY;
            }
            stockTransferBarcodeDT.PRIMARY_UOM = stockDataList[0].PRIMARY_UOM_CODE;
            stockTransferBarcodeDT.SECONDARY_UOM = stockDataList[0].SECONDARY_UOM_CODE;
            stockTransferBarcodeDT.REMARK = stockDataList[0].NOTE;
            stockTransferBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            model.Add(stockTransferBarcodeDT);

            //備貨單清單更新
            if (stockDataList[0].PACKING_TYPE == "令包")
            {
                stockTransferData.UpdateStockTransferDT(StockTransferDT_ID, (decimal)InputReamQty * 10, (decimal)InputReamQty, false, false, stockTransferBarcodeDT.Status);
            }
            else
            {
                stockTransferData.UpdateStockTransferDT(StockTransferDT_ID, stockDataList[0].PRIMARY_AVAILABLE_QTY, stockDataList[0].SECONDARY_AVAILABLE_QTY, false, false, stockTransferBarcodeDT.Status);
            }
            return new ResultModel(true, "條碼新增成功");
        }

        public ResultModel CreateInboundBarcode(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            ref string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal ROLL_REAM_WT, string LOT_NUMBER, string NUMBER_STATUS)
        {
            List<StockDT> itemList = StockData.GetStockItemData(IN_SUBINVENTORY_CODE, ITEM_NUMBER);
            if (itemList.Count == 0)
            {
                return new ResultModel(false, "搜尋不到料號" + ITEM_NUMBER + "資料");
            }
            string ITEM_CATEGORY = itemList[0].ITEM_CATEGORY;
            string Unit = "";
            if (itemList[0].ITEM_CATEGORY == "平版")
            {
                Unit = itemList[0].SECONDARY_UOM_CODE;
            }
            else
            {
                Unit = itemList[0].PRIMARY_UOM_CODE;
            }

            //ROLL_REAM_WT 每捲或棧板數量
            decimal ROLL_REAM_QTY = Math.Ceiling(REQUESTED_QTY / ROLL_REAM_WT); //無條件進位 算出棧板數或捲數

            ResultModel saveStockTransferDTResult = stockTransferData.CreateInbound(TransactionType, OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID, ref Number, ITEM_NUMBER, REQUESTED_QTY, 0, Unit, ROLL_REAM_QTY, ITEM_CATEGORY, NUMBER_STATUS);

            if (!saveStockTransferDTResult.Success)
            {
                return saveStockTransferDTResult;
            }

            long StockTransferDT_ID = Convert.ToInt64(saveStockTransferDTResult.Msg);

            decimal totalQty = REQUESTED_QTY;
            for (int i = 0; i < (int)ROLL_REAM_QTY; i++)
            {
                var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                //if (totalQty - ROLL_REAM_WT > 0)
                //{
                totalQty = totalQty - ROLL_REAM_WT;
                //}
                StockTransferBarcodeDT stockTransferBarcodeDT = new StockTransferBarcodeDT();
                stockTransferBarcodeDT.ID = highestId + 1;
                stockTransferBarcodeDT.TransferDetailId = StockTransferDT_ID;
                stockTransferBarcodeDT.Subinventory = IN_SUBINVENTORY_CODE;
                if (TransactionType == "出貨編號")
                {
                    stockTransferBarcodeDT.SHIPMENT_NUMBER = Number;
                }
                else
                {
                    stockTransferBarcodeDT.SUBINVENTORY_TRANSFER_NUMBER = Number;
                }
                stockTransferBarcodeDT.LOT_NUMBER = LOT_NUMBER;
                stockTransferBarcodeDT.BARCODE = GetInboundBarcode(StockTransferDT_ID, IN_SUBINVENTORY_CODE);
                stockTransferBarcodeDT.Status = "待列印";
                stockTransferBarcodeDT.ITEM_NUMBER = ITEM_NUMBER;
                stockTransferBarcodeDT.OSP_BATCH_NO = itemList[0].OSP_BATCH_NO;
                stockTransferBarcodeDT.ITEM_DESCRIPTION = itemList[0].ITEM_DESCRIPTION;
                stockTransferBarcodeDT.ITEM_CATEGORY = itemList[0].ITEM_CATEGORY;
                stockTransferBarcodeDT.PAPERTYPE = itemList[0].PapaerType;
                stockTransferBarcodeDT.Base_Weight = itemList[0].BasicWeight;
                stockTransferBarcodeDT.Specification = itemList[0].Specification;
                stockTransferBarcodeDT.PACKING_TYPE = itemList[0].PACKING_TYPE;
                if (itemList[0].ITEM_CATEGORY == "平版")
                {
                    stockTransferBarcodeDT.PRIMARY_QUANTITY = totalQty >= 0 ? ROLL_REAM_WT * 10 : (totalQty + ROLL_REAM_WT) * 10;
                    stockTransferBarcodeDT.SECONDARY_QUANTITY = totalQty >= 0 ? ROLL_REAM_WT : totalQty + ROLL_REAM_WT;
                }
                else if (itemList[0].ITEM_CATEGORY == "捲筒")
                {
                    stockTransferBarcodeDT.PRIMARY_QUANTITY = totalQty >= 0 ? ROLL_REAM_WT : totalQty + ROLL_REAM_WT;
                    stockTransferBarcodeDT.SECONDARY_QUANTITY = 0;
                }
                stockTransferBarcodeDT.PRIMARY_UOM = itemList[0].PRIMARY_UOM_CODE;
                stockTransferBarcodeDT.SECONDARY_UOM = itemList[0].SECONDARY_UOM_CODE;
                stockTransferBarcodeDT.REMARK = "";
                stockTransferBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
                model.Add(stockTransferBarcodeDT);
            }

            return new ResultModel(true, "入庫新增條碼成功");

        }

        public string GetInboundBarcode(long StockTransferDT_ID, string IN_SUBINVENTORY_CODE)
        {
            var query = from data in model
                        where IN_SUBINVENTORY_CODE == data.Subinventory
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
                string prefixBarcode = stockTransferData.orgData.GetBarodePrefixCode(IN_SUBINVENTORY_CODE);
                return prefixBarcode + DateTime.Now.ToString("yyMMdd") + "0001";
            }

        }

        public ResultModel BarcodeInbound(string TransactionType, string Number, string BARCODE)
        {
            List<StockTransferBarcodeDT> barcodeList = new List<StockTransferBarcodeDT>();
            if (TransactionType == "出貨編號")
            {
                var barcodeQuery = from data in model
                                   where Number == data.SHIPMENT_NUMBER &&
                                   BARCODE == data.BARCODE
                                   select data;
                barcodeList = barcodeQuery.ToList();
            }
            if (TransactionType == "移轉編號")
            {
                var barcodeQuery = from data in model
                                   where Number == data.SUBINVENTORY_TRANSFER_NUMBER &&
                                   BARCODE == data.BARCODE
                                   select data;
                barcodeList = barcodeQuery.ToList();
            }

            if (barcodeList.Count == 0)
            {
                return new ResultModel(false, "找不到條碼");
            }

            if (barcodeList[0].Status == "待列印")
            {
                return new ResultModel(false, "請列印條碼標籤");
            }

            if (barcodeList[0].Status == "已入庫")
            {
                return new ResultModel(false, "此條碼已入庫");
            }

            foreach (StockTransferBarcodeDT barcodeData in model)
            {
                if (barcodeData.BARCODE == BARCODE)
                {
                    barcodeData.Status = "已入庫";
                }
            }

            foreach (StockTransferDT data in StockTransferData.model)
            {
                if (data.ID == barcodeList[0].TransferDetailId)
                {
                    data.INBOUND_PICKED_QUANTITY = data.INBOUND_PICKED_QUANTITY + barcodeList[0].PRIMARY_QUANTITY;
                    data.INBOUND_PICKED_QUANTITY2 = data.INBOUND_PICKED_QUANTITY2 + barcodeList[0].SECONDARY_QUANTITY;
                }
            }
            return new ResultModel(true, "條碼入庫成功");
        }



        public ResultModel DeleteBarcode(List<long> IDs, bool isInbound)
        {
            bool result = false;
            var query = from data in model
                        where IDs.Contains(data.ID)
                        select data;
            var removeList = query.ToList();

            if (removeList.Count == 0)
            {
                return new ResultModel(false, "找不到條碼刪除");
            }

            foreach (StockTransferBarcodeDT barcode in removeList)
            {
                stockTransferData.UpdateStockTransferDT(barcode.TransferDetailId, -barcode.PRIMARY_QUANTITY, -barcode.SECONDARY_QUANTITY, false, isInbound, barcode.Status);
                result = model.Remove(barcode);

                if (model.Count == 0)
                {
                    stockTransferData.DeleteItemNumber(barcode.TransferDetailId, isInbound);
                }
            }


            if (result)
            {
                return new ResultModel(true, "刪除條碼成功");
            }
            else
            {
                return new ResultModel(true, "刪除條碼失敗");
            }
        }

        //public List<long> GetBarcodeIDs(long StockTransferDT_ID)
        //{
        //    var query = from data in model
        //                where StockTransferDT_ID == data.StockTransferDT_ID
        //                select data.ID;
        //    return query.ToList();
        //}

        public List<StockTransferBarcodeDT> UpdateRemark(StockTransferBarcodeDTEditor data)
        {
            List<StockTransferBarcodeDT> result = new List<StockTransferBarcodeDT>();

            foreach (var barcodeData in model)
            {
                foreach (var selectedData in data.StockTransferBarcodeDTList)
                {
                    if (barcodeData.ID == selectedData.ID)
                    {
                        barcodeData.REMARK = selectedData.REMARK;
                        result.Add(barcodeData);
                    }
                }
            }
            return result;
        }

        public List<StockTransferBarcodeDT> GetModelFromID(long ID)
        {
            var query = from stockTransferBarcodeDT in model
                        where ID == stockTransferBarcodeDT.ID
                        select stockTransferBarcodeDT;
            return query.ToList();
        }


        //public MergeBarcodeViewModel GetMergeBarcodeViewModel(TransferUOW List<long> IDs)
        //{
        //    MergeBarcodeViewModel vieModel = new MergeBarcodeViewModel();
        //    vieModel.WaitMergeBarcodeList = new List<TRF_INBOUND_PICKED_T>();

        //    var query = from data in model
        //                where IDs.Contains(data.ID)
        //                select data;

        //    var waitMergeBarcodeList = query.ToList();

        //    if (waitMergeBarcodeList.Count != 0)
        //    {
        //        vieModel.WaitMergeBarcodeList = waitMergeBarcodeList;
        //    }
        //    return vieModel;

        //}

        public JsonResult GetMergeBarocdeStatus(string MergeBarocde, List<long> waitMergeIDs)
        {
            List<StockDT> mergeBarocdeDataList = StockData.GetStockData(MergeBarocde);
            var queryWaitMergeBarcodeData = from data in model
                                            where waitMergeIDs.Contains(data.ID)
                                            select data;
            var waitMergeBarcodeDataList = queryWaitMergeBarcodeData.ToList();

            ResultModel checkResult = checkMergeBarcode(mergeBarocdeDataList, waitMergeBarcodeDataList);
            if (!checkResult.Success)
            {
                return new JsonResult { Data = new { status = checkResult.Success, result = checkResult.Msg } };
            }

            decimal waitMergeTotalQty = 0;
            foreach (StockTransferBarcodeDT data in waitMergeBarcodeDataList)
            {
                waitMergeTotalQty = waitMergeTotalQty + data.SECONDARY_QUANTITY;
            }

            return new JsonResult
            {
                Data = new
                {
                    status = true,
                    OriginalBarcode = mergeBarocdeDataList[0].BARCODE,
                    OriginalQty = mergeBarocdeDataList[0].SECONDARY_AVAILABLE_QTY,
                    OriginalUnit = mergeBarocdeDataList[0].SECONDARY_UOM_CODE,
                    AfterBarcode = mergeBarocdeDataList[0].BARCODE,
                    AfterQty = mergeBarocdeDataList[0].SECONDARY_AVAILABLE_QTY + waitMergeTotalQty,
                    AfterUnit = mergeBarocdeDataList[0].SECONDARY_UOM_CODE
                }
            };

        }

        public ResultModel checkMergeBarcode(List<StockDT> mergeBarocdeDataList, List<StockTransferBarcodeDT> waitMergeBarcodeDataList)
        {
            if (mergeBarocdeDataList.Count == 0)
            {
                return new ResultModel(false, "此條碼不在庫存");
            }
            if (waitMergeBarcodeDataList.Count == 0)
            {
                return new ResultModel(false, "找不到待併板條碼資料");
            }

            if (mergeBarocdeDataList[0].ITEM_CATEGORY == "捲筒")
            {
                return new ResultModel(false, "捲筒不可併板");
            }

            if (mergeBarocdeDataList[0].PACKING_TYPE != "令包")
            {
                return new ResultModel(false, "打包方式不是令包不可併板");
            }

            foreach (StockTransferBarcodeDT data in waitMergeBarcodeDataList)
            {
                if (data.ITEM_NUMBER != mergeBarocdeDataList[0].ITEM_NO)
                {
                    return new ResultModel(false, "併板料號須相同");
                }
            }

            return new ResultModel(true, "併板條碼檢查成功");
        }

        public ResultModel MergeBarcode(string MergeBarocde, List<long> waitMergeIDs)
        {
            List<StockDT> mergeBarocdeDataList = StockData.GetStockData(MergeBarocde);
            var queryWaitMergeBarcodeData = from data in model
                                            where waitMergeIDs.Contains(data.ID)
                                            select data;
            var waitMergeBarcodeDataList = queryWaitMergeBarcodeData.ToList();

            ResultModel checkResult = checkMergeBarcode(mergeBarocdeDataList, waitMergeBarcodeDataList);
            if (!checkResult.Success)
            {
                return checkResult;
            }

            decimal waitMergePrimaryTotalQty = 0;
            decimal waitMergeSecondaryTotalQty = 0;
            string newNote = "";

            foreach (StockTransferBarcodeDT data in waitMergeBarcodeDataList)
            {
                waitMergePrimaryTotalQty = waitMergePrimaryTotalQty + data.PRIMARY_QUANTITY;
                waitMergeSecondaryTotalQty = waitMergeSecondaryTotalQty + data.SECONDARY_QUANTITY;
                newNote = newNote + data.BARCODE + " ";
            }


            var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
            StockTransferBarcodeDT stockTransferBarcodeDT = new StockTransferBarcodeDT();
            stockTransferBarcodeDT.BARCODE = mergeBarocdeDataList[0].BARCODE;
            stockTransferBarcodeDT.ID = highestId + 1;
            stockTransferBarcodeDT.Subinventory = mergeBarocdeDataList[0].SUBINVENTORY_CODE;
            stockTransferBarcodeDT.ITEM_NUMBER = mergeBarocdeDataList[0].ITEM_NO;
            stockTransferBarcodeDT.OSP_BATCH_NO = mergeBarocdeDataList[0].OSP_BATCH_NO;
            stockTransferBarcodeDT.ITEM_DESCRIPTION = mergeBarocdeDataList[0].ITEM_DESCRIPTION;
            stockTransferBarcodeDT.ITEM_CATEGORY = mergeBarocdeDataList[0].ITEM_CATEGORY;
            stockTransferBarcodeDT.PAPERTYPE = mergeBarocdeDataList[0].PapaerType;
            stockTransferBarcodeDT.Base_Weight = mergeBarocdeDataList[0].BasicWeight;
            stockTransferBarcodeDT.Specification = mergeBarocdeDataList[0].Specification;
            stockTransferBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            stockTransferBarcodeDT.LOT_NUMBER = mergeBarocdeDataList[0].LOT_NUMBER;
            stockTransferBarcodeDT.PACKING_TYPE = mergeBarocdeDataList[0].PACKING_TYPE;
            stockTransferBarcodeDT.PRIMARY_QUANTITY = waitMergePrimaryTotalQty;
            stockTransferBarcodeDT.PRIMARY_UOM = mergeBarocdeDataList[0].PRIMARY_UOM_CODE;
            stockTransferBarcodeDT.REMARK = mergeBarocdeDataList[0].NOTE + " " + newNote;
            stockTransferBarcodeDT.SECONDARY_QUANTITY = waitMergeSecondaryTotalQty;
            stockTransferBarcodeDT.SECONDARY_UOM = mergeBarocdeDataList[0].SECONDARY_UOM_CODE;
            stockTransferBarcodeDT.SHIPMENT_NUMBER = waitMergeBarcodeDataList[0].SHIPMENT_NUMBER;
            stockTransferBarcodeDT.Status = "待列印";
            stockTransferBarcodeDT.TransferDetailId = waitMergeBarcodeDataList[0].TransferDetailId;
            stockTransferBarcodeDT.SUBINVENTORY_TRANSFER_NUMBER = waitMergeBarcodeDataList[0].SUBINVENTORY_TRANSFER_NUMBER;
            model.Add(stockTransferBarcodeDT);
            stockTransferData.MergeBarcode(waitMergeBarcodeDataList[0].TransferDetailId, waitMergePrimaryTotalQty, waitMergeSecondaryTotalQty, 1);

            //刪除舊條碼
            foreach (StockTransferBarcodeDT data in waitMergeBarcodeDataList)
            {
                model.Remove(data);
                stockTransferData.MergeBarcode(data.TransferDetailId, -data.PRIMARY_QUANTITY, -data.SECONDARY_QUANTITY, -1);
            }
            return new ResultModel(true, "併板成功");
        }

        public ResultModel MergeBarcode(StockTransferBarcodeDTEditor data)
        {
            if (data.StockTransferBarcodeDTList.Count == 0)
            {
                return new ResultModel(false, "請選擇條碼");
            }

            //檢查被併版的條碼庫存
            string newBarcode = data.StockTransferBarcodeDTList[0].BARCODE;
            var storckList = StockData.GetStockData(newBarcode);
            if (storckList.Count == 0)
            {
                return new ResultModel(false, "此條碼" + newBarcode + "沒有庫存");
            }

            if (storckList[0].PACKING_TYPE != "令包")
            {
                return new ResultModel(false, "此條碼" + storckList[0].BARCODE + "不是令包不可併板");
            }

            //檢查併板料號是否相同
            foreach (var selectedData in data.StockTransferBarcodeDTList)
            {
                var oldBarcodeList = GetModelFromID(selectedData.ID);
                if (oldBarcodeList.Count == 0)
                {
                    return new ResultModel(false, "找不到條碼資料");
                }
                if (storckList[0].ITEM_NO != oldBarcodeList[0].ITEM_NUMBER)
                {
                    return new ResultModel(false, "併板料號須相同");
                }
            }


            foreach (var selectedData in data.StockTransferBarcodeDTList)
            {
                var oldBarcodeList = GetModelFromID(selectedData.ID);
                if (oldBarcodeList.Count == 0)
                {
                    return new ResultModel(false, "找不到條碼資料");
                }

                if (oldBarcodeList[0].BARCODE == newBarcode)
                {
                    continue;
                }

                var barcode = model.FirstOrDefault(d => d.BARCODE == newBarcode);
                if (barcode != null)
                {
                    barcode.PRIMARY_QUANTITY = barcode.PRIMARY_QUANTITY + oldBarcodeList[0].PRIMARY_QUANTITY;
                    barcode.SECONDARY_QUANTITY = barcode.SECONDARY_QUANTITY + oldBarcodeList[0].SECONDARY_QUANTITY;
                    stockTransferData.MergeBarcode(oldBarcodeList[0].TransferDetailId, oldBarcodeList[0].PRIMARY_QUANTITY, oldBarcodeList[0].SECONDARY_QUANTITY, 0);
                }
                else
                {
                    var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                    StockTransferBarcodeDT stockTransferBarcodeDT = new StockTransferBarcodeDT();
                    stockTransferBarcodeDT.BARCODE = storckList[0].BARCODE;
                    stockTransferBarcodeDT.ID = highestId + 1;
                    stockTransferBarcodeDT.Subinventory = storckList[0].SUBINVENTORY_CODE;
                    stockTransferBarcodeDT.ITEM_NUMBER = storckList[0].ITEM_NO;
                    stockTransferBarcodeDT.OSP_BATCH_NO = storckList[0].OSP_BATCH_NO;
                    stockTransferBarcodeDT.ITEM_DESCRIPTION = storckList[0].ITEM_DESCRIPTION;
                    stockTransferBarcodeDT.ITEM_CATEGORY = storckList[0].ITEM_CATEGORY;
                    stockTransferBarcodeDT.PAPERTYPE = storckList[0].PapaerType;
                    stockTransferBarcodeDT.Base_Weight = storckList[0].BasicWeight;
                    stockTransferBarcodeDT.Specification = storckList[0].Specification;
                    stockTransferBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
                    stockTransferBarcodeDT.LOT_NUMBER = storckList[0].LOT_NUMBER;
                    stockTransferBarcodeDT.PACKING_TYPE = storckList[0].PACKING_TYPE;
                    stockTransferBarcodeDT.PRIMARY_QUANTITY = storckList[0].PRIMARY_AVAILABLE_QTY + oldBarcodeList[0].PRIMARY_QUANTITY;
                    stockTransferBarcodeDT.PRIMARY_UOM = storckList[0].PRIMARY_UOM_CODE;
                    stockTransferBarcodeDT.REMARK = storckList[0].NOTE;
                    stockTransferBarcodeDT.SECONDARY_QUANTITY = storckList[0].SECONDARY_AVAILABLE_QTY + oldBarcodeList[0].SECONDARY_QUANTITY;
                    stockTransferBarcodeDT.SECONDARY_UOM = storckList[0].SECONDARY_UOM_CODE;
                    stockTransferBarcodeDT.SHIPMENT_NUMBER = oldBarcodeList[0].SHIPMENT_NUMBER;
                    stockTransferBarcodeDT.Status = oldBarcodeList[0].Status;
                    stockTransferBarcodeDT.TransferDetailId = oldBarcodeList[0].TransferDetailId;
                    stockTransferBarcodeDT.SUBINVENTORY_TRANSFER_NUMBER = oldBarcodeList[0].SUBINVENTORY_TRANSFER_NUMBER;
                    model.Add(stockTransferBarcodeDT);
                    stockTransferData.MergeBarcode(oldBarcodeList[0].TransferDetailId, storckList[0].PRIMARY_AVAILABLE_QTY + oldBarcodeList[0].PRIMARY_QUANTITY, storckList[0].SECONDARY_AVAILABLE_QTY + oldBarcodeList[0].SECONDARY_QUANTITY, 1);
                }

                //stockTransferData.MergeBarcode(oldBarcodeList[0].StockTransferDT_ID, oldBarcodeList[0].PRIMARY_QUANTITY, oldBarcodeList[0].SECONDARY_QUANTITY, addNewBarcode);

                //刪除舊條碼
                model.Remove(oldBarcodeList[0]);
                stockTransferData.MergeBarcode(oldBarcodeList[0].TransferDetailId, -oldBarcodeList[0].PRIMARY_QUANTITY, -oldBarcodeList[0].SECONDARY_QUANTITY, -1);
            }
            return new ResultModel(true, "併板成功");
        }

        public ResultModel ImportRollInboundBarcode(ImportRollInboundBarcodeModel data, ref string Number)
        {
            if (data.StockTransferBarcodeDTList == null || data.StockTransferBarcodeDTList.Count == 0)
            {
                return new ResultModel(false, "沒有資料可以匯入");
            }


            //刪除舊編號資料
            ResultModel deleteResult = stockTransferData.DeleteNumber(data.TransactionType, data.Number);
            if (!deleteResult.Success)
            {
                return deleteResult;
            }


            foreach (StockTransferBarcodeDT stockTransferBarcodeDT in data.StockTransferBarcodeDTList)
            {

                ResultModel result = CreateInboundBarcode(data.TransactionType, data.OUT_SUBINVENTORY_CODE, data.OUT_LOCATOR_ID, data.IN_SUBINVENTORY_CODE, data.IN_LOCATOR_ID, ref Number,
                    stockTransferBarcodeDT.ITEM_NUMBER, stockTransferBarcodeDT.PRIMARY_QUANTITY, stockTransferBarcodeDT.PRIMARY_QUANTITY, stockTransferBarcodeDT.LOT_NUMBER, "非MES入庫檔案匯入");
                if (!result.Success)
                {
                    return result;
                }
            }
            return new ResultModel(true, "捲筒匯入成功");
        }

        public ResultModel ImportFlatInboundBarcode(ImportFlatInboundBarcodeModel data, ref string Number)
        {
            if (data.StockTransferDTList == null || data.StockTransferDTList.Count == 0)
            {
                return new ResultModel(false, "沒有資料可以匯入");
            }


            //刪除舊編號資料
            ResultModel deleteResult = stockTransferData.DeleteNumber(data.TransactionType, data.Number);
            if (!deleteResult.Success)
            {
                return deleteResult;
            }


            foreach (StockTransferDT stockTransferBarcodeDT in data.StockTransferDTList)
            {
                ResultModel result = CreateInboundBarcode(data.TransactionType, data.OUT_SUBINVENTORY_CODE, data.OUT_LOCATOR_ID, data.IN_SUBINVENTORY_CODE, data.IN_LOCATOR_ID, ref Number,
                    stockTransferBarcodeDT.ITEM_NUMBER, stockTransferBarcodeDT.REQUESTED_QUANTITY2, stockTransferBarcodeDT.ROLL_REAM_WT, "", "非MES入庫檔案匯入");
                if (!result.Success)
                {
                    return result;
                }
            }
            return new ResultModel(true, "平版匯入成功");
        }

    }

    internal class StockTransferBarcodeDTOrder
    {
        public static IOrderedEnumerable<StockTransferBarcodeDT> Order(List<Order> orders, IEnumerable<StockTransferBarcodeDT> models)
        {
            IOrderedEnumerable<StockTransferBarcodeDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockTransferBarcodeDT> OrderBy(int column, string dir, IEnumerable<StockTransferBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BARCODE) : models.OrderBy(x => x.BARCODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PACKING_TYPE) : models.OrderBy(x => x.PACKING_TYPE);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_QUANTITY) : models.OrderBy(x => x.PRIMARY_QUANTITY);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_UOM) : models.OrderBy(x => x.PRIMARY_UOM);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_QUANTITY) : models.OrderBy(x => x.SECONDARY_QUANTITY);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_UOM) : models.OrderBy(x => x.SECONDARY_UOM);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);


            }
        }

        private static IOrderedEnumerable<StockTransferBarcodeDT> ThenBy(int column, string dir, IOrderedEnumerable<StockTransferBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BARCODE) : models.ThenBy(x => x.BARCODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PACKING_TYPE) : models.ThenBy(x => x.PACKING_TYPE);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_QUANTITY) : models.ThenBy(x => x.PRIMARY_QUANTITY);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_UOM) : models.ThenBy(x => x.PRIMARY_UOM);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_QUANTITY) : models.ThenBy(x => x.SECONDARY_QUANTITY);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_UOM) : models.ThenBy(x => x.SECONDARY_UOM);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);

            }
        }
    }

    internal class InboundStockTransferBarcodeDTOrder
    {
        public static IOrderedEnumerable<StockTransferBarcodeDT> Order(List<Order> orders, IEnumerable<StockTransferBarcodeDT> models)
        {
            IOrderedEnumerable<StockTransferBarcodeDT> orderedModel = null;
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


        private static IOrderedEnumerable<StockTransferBarcodeDT> OrderBy(int column, string dir, IEnumerable<StockTransferBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BARCODE) : models.OrderBy(x => x.BARCODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LOT_NUMBER) : models.OrderBy(x => x.LOT_NUMBER);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PACKING_TYPE) : models.OrderBy(x => x.PACKING_TYPE);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_QUANTITY) : models.OrderBy(x => x.PRIMARY_QUANTITY);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_UOM) : models.OrderBy(x => x.PRIMARY_UOM);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_QUANTITY) : models.OrderBy(x => x.SECONDARY_QUANTITY);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_UOM) : models.OrderBy(x => x.SECONDARY_UOM);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);


            }
        }

        private static IOrderedEnumerable<StockTransferBarcodeDT> ThenBy(int column, string dir, IOrderedEnumerable<StockTransferBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BARCODE) : models.ThenBy(x => x.BARCODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Status) : models.ThenBy(x => x.Status);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LOT_NUMBER) : models.ThenBy(x => x.LOT_NUMBER);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PACKING_TYPE) : models.ThenBy(x => x.PACKING_TYPE);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_QUANTITY) : models.ThenBy(x => x.PRIMARY_QUANTITY);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_UOM) : models.ThenBy(x => x.PRIMARY_UOM);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_QUANTITY) : models.ThenBy(x => x.SECONDARY_QUANTITY);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_UOM) : models.ThenBy(x => x.SECONDARY_UOM);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);

            }
        }
    }

    public class StockTransferBarcodeDTEditor
    {
        public string Action { get; set; }

        public List<StockTransferBarcodeDT> StockTransferBarcodeDTList { get; set; }
    }



    public class ImportRollInboundBarcodeModel
    {
        public string TransactionType { get; set; }
        public string OUT_SUBINVENTORY_CODE { get; set; }
        public string OUT_LOCATOR_ID { get; set; }
        public string IN_SUBINVENTORY_CODE { get; set; }
        public string IN_LOCATOR_ID { get; set; }
        public string Number { get; set; }
        public List<StockTransferBarcodeDT> StockTransferBarcodeDTList { get; set; }
    }

    public class ImportFlatInboundBarcodeModel
    {
        public string TransactionType { get; set; }
        public string OUT_SUBINVENTORY_CODE { get; set; }
        public string OUT_LOCATOR_ID { get; set; }
        public string IN_SUBINVENTORY_CODE { get; set; }
        public string IN_LOCATOR_ID { get; set; }
        public string Number { get; set; }
        public List<StockTransferDT> StockTransferDTList { get; set; }
    }
}