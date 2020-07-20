using CHPOUTSRCMES.Web.Models.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    public class LabelModel
    {
        //public int Id { get; set; }

        public string Barocde { get; set; }

        public string BarocdeName { get; set; }

        public string PapaerType { get; set; }

        public string BasicWeight { get; set; }

        public string Specification { get; set; }

        public string Qty { get; set; }

        public string Unit { get; set; }

        public string OspBatchNo { get; set; }

        public string PrintBy { get; set; }

        public LabelModel()
        {
        }

        public LabelModel(string BarocdeName, string PapaerType, string BasicWeight, string Specification, string Qty, string Unit, string OspBatchNo, string PrintBy, string Barocde)
        {
            this.Barocde = Barocde;
            this.BarocdeName = BarocdeName;
            this.PapaerType = PapaerType;
            this.BasicWeight = BasicWeight;
            this.Specification = Specification;
            this.Qty = Qty;
            this.Unit = Unit;
            this.OspBatchNo = OspBatchNo;
            this.PrintBy = PrintBy;
        }

        public static List<LabelModel> GetLabels(List<string> id)
        {
            List<LabelModel> lables = new List<LabelModel>();

            for (int i = 1; i <= 1; i++)
            {
                LabelModel lable1 = new LabelModel();
                lable1.Barocde = "W1907250010001";
                lable1.BarocdeName = "捲筒金典銅西";
                lable1.PapaerType = "FHIZ";
                lable1.BasicWeight = "250";
                lable1.Specification = "787MM";
                lable1.Qty = "947";
                lable1.Unit = "KG";
                lable1.OspBatchNo = "";
                lable1.PrintBy = "OY001";

                LabelModel lable2 = new LabelModel();
                lable2.Barocde = "W1907350010002";
                lable2.BarocdeName = "特級雪面銅版紙(N)";
                lable2.PapaerType = "ADNX";
                lable2.BasicWeight = "80";
                lable2.Specification = "300K210K";
                lable2.Qty = "30";
                lable2.Unit = "令";
                lable2.OspBatchNo = "P9A0021";
                lable2.PrintBy = "OY001";

                lables.Add(lable1);
                lables.Add(lable2);
            }

            return lables;
        }
    }

    public class LabelData
    {
        private List<LabelModel> testSource = new List<LabelModel>();

        public LabelData()
        {
            testSource.Add(new LabelModel("全塗灰銅卡", "DM00", "01000", "310K266K", "1000", "令", "P9B0288", "OY001", "P2005060001"));
            testSource.Add(new LabelModel("全塗灰銅卡", "DM00", "01000", "310K266K", "1000", "令", "P9B0288", "OY001", "P2005060002"));
            testSource.Add(new LabelModel("全塗灰銅卡", "DM00", "01000", "310K266K", "1000", "令", "P9B0288", "OY001", "P2005060003"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FU0Z", "02550", "635", "421", "KG", "", "OY001", "W2005060001"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FU0Z", "02200", "635", "440", "KG", "", "OY001", "W2005060002"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FU0Z", "02200", "889", "889", "KG", "", "OY001", "W2005060003"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "03000", "787", "1000", "KG", "", "OY001", "W2005060004"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02500", "787", "1000", "KG", "", "OY001", "W2005060005"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02000", "787", "1000", "KG", "", "OY001", "W2005060006"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2006060001"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2006060002"));
            testSource.Add(new LabelModel("全塗灰銅卡", "DM00", "02300", "294K476K", "120", "令", "P960009", "OY001", "P2007060001"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2008060001"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060001"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060002"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060003"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060004"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060005"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060006"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060007"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060008"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2010060009"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P20100600010"));
            testSource.Add(new LabelModel("試抄紙品", "DM00", "03000", "590", "120", "令", "F2010087", "OY001", "P2009060001"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2010060001"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2010060002"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2010060003"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2010060004"));
            testSource.Add(new LabelModel("捲筒金典銅西", "FHIZ", "02300", "590", "1000", "KG", "", "OY001", "W2010060005"));


        }

        public List<LabelModel> GetLabels(List<string> Barcode)
        {
            var query = from lable in testSource
                        where Barcode.Contains(lable.Barocde)
                        select lable;
            return query.ToList();
        }

        public List<LabelModel> GetLabels2(List<string> Barcode, StockTransferBarcodeData stockTransferBarcodeData)
        {
            var lableList = new List<LabelModel>();
            foreach (string barcode in Barcode)
            {
                var stockTransferBarcodeDTList = stockTransferBarcodeData.GetBarcodeData(barcode);

                //var stockData = StockData.GetStockData(barcode);
                if (stockTransferBarcodeDTList.Count > 0)
                {
                    if (stockTransferBarcodeDTList[0].ITEM_CATEGORY == "平版")
                    {
                        lableList.Add(new LabelModel(stockTransferBarcodeDTList[0].ITEM_DESCRIPTION, stockTransferBarcodeDTList[0].PAPERTYPE, stockTransferBarcodeDTList[0].Base_Weight,
                       stockTransferBarcodeDTList[0].Specification, stockTransferBarcodeDTList[0].SECONDARY_QUANTITY.ToString(), stockTransferBarcodeDTList[0].SECONDARY_UOM,
                       stockTransferBarcodeDTList[0].OSP_BATCH_NO, "OY001", barcode));
                    }
                    else
                    {
                        lableList.Add(new LabelModel(stockTransferBarcodeDTList[0].ITEM_DESCRIPTION, stockTransferBarcodeDTList[0].PAPERTYPE, stockTransferBarcodeDTList[0].Base_Weight,
                      stockTransferBarcodeDTList[0].Specification, stockTransferBarcodeDTList[0].PRIMARY_QUANTITY.ToString(), stockTransferBarcodeDTList[0].PRIMARY_UOM,
                      stockTransferBarcodeDTList[0].OSP_BATCH_NO, "OY001", barcode));
                    }
                    stockTransferBarcodeDTList[0].Status = "待入庫";
                }
            }
            return lableList;
        }


        public List<LabelModel> GetLabels3(List<string> Barcode)
        {

            StockInventoryData stockInventoryData = new StockInventoryData();
            var lableList = new List<LabelModel>();
            foreach (string barcode in Barcode)
            {
                List<StockInventoryDT> StockInventoryList = stockInventoryData.GetProfitBarcodeData(barcode);
                //List<StockDT> StockInventoryList = StockData.GetStockBarcodeData(barcode);
                //var stockData = StockData.GetStockData(barcode);
                if (StockInventoryList.Count > 0)
                {
                    if (StockInventoryList[0].ITEM_CATEGORY == "平版")
                    {
                        lableList.Add(new LabelModel(StockInventoryList[0].ITEM_DESCRIPTION, StockInventoryList[0].PapaerType, StockInventoryList[0].BasicWeight,
                        StockInventoryList[0].Specification, StockInventoryList[0].SECONDARY_AVAILABLE_QTY.ToString(), StockInventoryList[0].SECONDARY_UOM_CODE,
                        StockInventoryList[0].OSP_BATCH_NO, "OY001", barcode));
                    }
                    else
                    {
                        lableList.Add(new LabelModel(StockInventoryList[0].ITEM_DESCRIPTION, StockInventoryList[0].PapaerType, StockInventoryList[0].BasicWeight,
                      StockInventoryList[0].Specification, StockInventoryList[0].PRIMARY_AVAILABLE_QTY.ToString(), StockInventoryList[0].PRIMARY_UOM_CODE,
                      StockInventoryList[0].OSP_BATCH_NO, "OY001", barcode));
                    }
                }
            }
            return lableList;
        }
    }
}