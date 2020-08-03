using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class FlatEditBarcodeDT
    {
        public long PICKED_ID { get; set; }

        public long SUB_ID { get; set; }

        public long DLV_DETAIL_ID { get; set; } //DLV_DETAIL_ID

        public long DlvHeaderId { get; set; } //DLV_HEADER_ID

        [Display(Name = "條碼號")]
        public string BARCODE { get; set; }

        [Display(Name = "料號名稱")]
        public string ITEM_NUMBER { get; set; }

        [Display(Name = "令重")]
        public string REAM_WEIGHT { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { get; set; }

        [Display(Name = "數量")] //主要數量
        public decimal PRIMARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //主要單位
        public string PRIMARY_UOM { get; set; }

        [Display(Name = "數量")] //次要數量
        public decimal SECONDARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //次要單位
        public string SECONDARY_UOM { get; set; }

        [Display(Name = "建立人員")]
        public long CREATED_BY { get; set; }
        [Display(Name = "建立日期")]
        public DateTime CREATION_DATE { get; set; }
        [Display(Name = "更新人員")]
        public long LAST_UPDATED_BY { get; set; }
        [Display(Name = "更新日期")]
        public DateTime LAST_UPDATE_DATE { get; set; }
        //[Display(Name = "備註")]
        //public string REMARK { get; set; }
    }

  


    public class FlatEditBarcodeData
    {
        public static List<FlatEditBarcodeDT> model = new List<FlatEditBarcodeDT>();

        public static List<FlatEditBarcodeDT> getModel(long DlvHeaderId) //DELIVERY_HEADER_ID
        {
            var query = from data in model
                        where DlvHeaderId == data.DLV_DETAIL_ID
                        select data;
            List<FlatEditBarcodeDT> list = query.ToList<FlatEditBarcodeDT>();
            return list;
        }

        public List<FlatEditBarcodeDT> GetFlatPickDT(DeliveryUOW uow, long DlvHeaderId)
        {
            return uow.GetFlatPickDT(DlvHeaderId);
        }

        public static void resetData()
        {
            model = new List<FlatEditBarcodeDT>();
        }

        public ResultModel AddPickDT(DeliveryUOW uow, long dlvHeaderId, long dlvDetailId, string deliveryName, string barcode, decimal? qty, string addUser, string addUserName, string status, string transactionUomCode)
        {
            return uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, qty, addUser, addUserName, status, transactionUomCode);
        }

            public static void addBarcode123(decimal qty)
        {
            FlatEditBarcodeDT barcodeData = new FlatEditBarcodeDT();
            barcodeData.BARCODE = "P2005060001";
            barcodeData.SECONDARY_QUANTITY = qty;
            barcodeData.SECONDARY_UOM = "RE";
            barcodeData.DLV_DETAIL_ID = 1;
            barcodeData.PICKED_ID = 1;
            barcodeData.ITEM_NUMBER = "4A003A01000310K266K";
            barcodeData.PACKING_TYPE = "令包";
            barcodeData.PRIMARY_QUANTITY = qty * 26.74838M;
            barcodeData.PRIMARY_UOM = "KG";
            barcodeData.REAM_WEIGHT = "58.97";
            barcodeData.CREATED_BY = 1;
            barcodeData.CREATION_DATE = DateTime.Now;
            barcodeData.LAST_UPDATED_BY = 1;
            barcodeData.LAST_UPDATE_DATE = DateTime.Now;
            //barcodeData.REMARK = "";
            model.Add(barcodeData);
        }

        public static void addBarcode456()
        {
            FlatEditBarcodeDT barcodeData = new FlatEditBarcodeDT();
            barcodeData.BARCODE = "P2005060002";
            barcodeData.SECONDARY_QUANTITY = 19;
            barcodeData.SECONDARY_UOM = "RE";
            barcodeData.DLV_DETAIL_ID = 2;
            barcodeData.PICKED_ID = 2;
            barcodeData.ITEM_NUMBER = "4AB23P00699350K250K";
            barcodeData.PACKING_TYPE = "無令打件";
            barcodeData.PRIMARY_QUANTITY = 374.8945M;
            barcodeData.PRIMARY_UOM = "KG";
            barcodeData.REAM_WEIGHT = "43.5";
            barcodeData.CREATED_BY = 1;
            barcodeData.CREATION_DATE = DateTime.Now;
            barcodeData.LAST_UPDATED_BY = 1;
            barcodeData.LAST_UPDATE_DATE = DateTime.Now;
            //barcodeData.REMARK = "";
            model.Add(barcodeData);
        }

        public static void addBarcode130()
        {
            FlatEditBarcodeDT barcodeData = new FlatEditBarcodeDT();
            barcodeData.BARCODE = "P2005060003";
            barcodeData.SECONDARY_QUANTITY = 120;
            barcodeData.SECONDARY_UOM = "RE";
            barcodeData.DLV_DETAIL_ID = 3;
            barcodeData.PICKED_ID = 3;
            barcodeData.ITEM_NUMBER = "4DM00P03000297K476K";
            barcodeData.PACKING_TYPE = "無令打件";
            barcodeData.PRIMARY_QUANTITY = 120 * 10;
            barcodeData.PRIMARY_UOM = "KG";
            barcodeData.REAM_WEIGHT = "304.35";
            barcodeData.CREATED_BY = 1;
            barcodeData.CREATION_DATE = DateTime.Now;
            barcodeData.LAST_UPDATED_BY = 1;
            barcodeData.LAST_UPDATE_DATE = DateTime.Now;
            //barcodeData.REMARK = "";
            model.Add(barcodeData);
        }

        public static void add(string barcode, decimal qty, string packingType)
        {
            
            FlatEditBarcodeDT barcodeData = new FlatEditBarcodeDT();
            barcodeData.BARCODE = barcode;
            barcodeData.SECONDARY_QUANTITY = qty;
            barcodeData.SECONDARY_UOM = "RE";
            barcodeData.PICKED_ID = model.Count;
            barcodeData.ITEM_NUMBER = "123";
            barcodeData.PACKING_TYPE = packingType;
            barcodeData.PRIMARY_QUANTITY = qty * 2;
            barcodeData.PRIMARY_UOM = "KG";
            barcodeData.REAM_WEIGHT = "10";
            barcodeData.CREATED_BY = 1;
            barcodeData.CREATION_DATE = DateTime.Now;
            barcodeData.LAST_UPDATED_BY = 1;
            barcodeData.LAST_UPDATE_DATE = DateTime.Now;
            //barcodeData.REMARK = "";
            model.Add(barcodeData);
        }

        public static bool remove(List<long> PICKED_ID){
            bool result = false;
             var query = from barcode in FlatEditBarcodeData.model
                         where PICKED_ID.Contains(barcode.PICKED_ID)
                        select barcode;
            List<FlatEditBarcodeDT> removeList =  query.ToList<FlatEditBarcodeDT>();
            foreach(FlatEditBarcodeDT barcode in removeList){
               result = model.Remove(barcode);
            }
            return result;
           
        }

        public static long getLastPickedID(string itemDesc)
        {
            var query = from barcodeData in FlatEditBarcodeData.model
                        where itemDesc == barcodeData.ITEM_NUMBER
                        select barcodeData;
            List<FlatEditBarcodeDT> list = query.ToList<FlatEditBarcodeDT>();
            if (list.Count > 0)
            {
                return list[0].PICKED_ID;
            }
            else
            {
                return 0;
            }
        }

        public static long getLastPickedID()
        {
            var query = from barcodeData in FlatEditBarcodeData.model
                        orderby barcodeData.LAST_UPDATE_DATE descending
                        select barcodeData;
            List<FlatEditBarcodeDT> list = query.ToList<FlatEditBarcodeDT>();
            if (list.Count > 0)
            {
                return list[0].PICKED_ID;
            }
            else
            {
                return 0;
            }
        }

        //public static FlatEditBarcodeDT getBarcodeData(string barcode)
        //{
        //    var query = from data in FlatEditBarcodeData.model
        //                where barcode == data.BARCODE
        //                select data;
        //    List<FlatEditBarcodeDT> list = query.ToList();
        //    if (list.Count > 0)
        //    {
        //        return list[0];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public static bool checkBarcodeExist(string barcode)
        {
            var query = from barcideData in FlatEditBarcodeData.model
                        where barcode == barcideData.BARCODE
                        select barcideData;
            List<FlatEditBarcodeDT> list = query.ToList();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<FlatEditBarcodeDT> getRemoveBarcodeList(List<long> PICKED_ID)
        {
            var query = from barcode in FlatEditBarcodeData.model
                        where PICKED_ID.Contains(barcode.PICKED_ID)
                        select barcode;
            
            return query.ToList();
        }

    }

    internal class FlatEditBarcodeDTOrder
    {
        public static IOrderedEnumerable<FlatEditBarcodeDT> Order(List<Order> orders, IEnumerable<FlatEditBarcodeDT> models)
        {
            IOrderedEnumerable<FlatEditBarcodeDT> orderedModel = null;
            if (orders.Count() > 0)
            {
                orderedModel = OrderBy(orders[0].Column , orders[0].Dir, models);
            }

            for (int i = 1; i < orders.Count(); i++)
            {
                orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
            }
            return orderedModel;
        }


        private static IOrderedEnumerable<FlatEditBarcodeDT> OrderBy(int column, string dir, IEnumerable<FlatEditBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DLV_DETAIL_ID) : models.OrderBy(x => x.DLV_DETAIL_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BARCODE) : models.OrderBy(x => x.BARCODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REAM_WEIGHT) : models.OrderBy(x => x.REAM_WEIGHT);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PACKING_TYPE) : models.OrderBy(x => x.PACKING_TYPE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_QUANTITY) : models.OrderBy(x => x.PRIMARY_QUANTITY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_UOM) : models.OrderBy(x => x.PRIMARY_UOM);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_QUANTITY) : models.OrderBy(x => x.SECONDARY_QUANTITY);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SECONDARY_UOM) : models.OrderBy(x => x.SECONDARY_UOM);
                //case 10:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATE_DATE) : models.OrderBy(x => x.LAST_UPDATE_DATE);
            }
        }

        private static IOrderedEnumerable<FlatEditBarcodeDT> ThenBy(int column, string dir, IOrderedEnumerable<FlatEditBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DLV_DETAIL_ID) : models.ThenBy(x => x.DLV_DETAIL_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BARCODE) : models.ThenBy(x => x.BARCODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REAM_WEIGHT) : models.ThenBy(x => x.REAM_WEIGHT);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PACKING_TYPE) : models.ThenBy(x => x.PACKING_TYPE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_QUANTITY) : models.ThenBy(x => x.PRIMARY_QUANTITY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_UOM) : models.ThenBy(x => x.PRIMARY_UOM);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_QUANTITY) : models.ThenBy(x => x.SECONDARY_QUANTITY);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SECONDARY_UOM) : models.ThenBy(x => x.SECONDARY_UOM);
                //case 10:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATE_DATE) : models.ThenBy(x => x.LAST_UPDATE_DATE);
            }
        }
    }
}