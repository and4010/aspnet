using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class FlatEditDT
    {
        public long ID { get; set; } //DELIVERY_DETAIL_ID

        public long SUB_ID { get; set; }

        [Display(Name = "訂單編號")]
        public long ORDER_NUMBER { get; set; }

        [Display(Name = "訂單行號")]
        public string ORDER_SHIP_NUMBER { get; set; }

        [Display(Name = "工單號碼ID")]
        public long? OSP_BATCH_ID { get; set; }

        [Display(Name = "工單號碼")]
        public string OSP_BATCH_NO { get; set; }

        [Display(Name = "代紙料號ID")]
        public long? TMP_ITEM_ID { get; set; }
        
        [Display(Name = "代紙料號")]
        public string TMP_ITEM_NUMBER { get; set; }

        [Display(Name = "料號ID")]
        public long INVENTORY_ITEM_ID { get; set; }
        
        [Display(Name = "料號")]
        public string ITEM_NUMBER { get; set; }

        [Display(Name = "令重")]
        public string REAM_WEIGHT { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { get; set; }

        [Display(Name = "需求數量")] //預計出庫量 主要數量
        public decimal REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //主單位已揀數合計
        public decimal? PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //主單位
        public string REQUESTED_QUANTITY_UOM { get; set; }

        [Display(Name = "需求數量")] //預計出庫輔數量 次要數量
        public decimal REQUESTED_QUANTITY2 { get; set; }

        [Display(Name = "已揀數量")] //出庫已揀輔數量
        public decimal? PICKED_QUANTITY2 { get; set; }

        [Display(Name = "單位")] //輔單位
        public string REQUESTED_QUANTITY_UOM2 { get; set; }

        [Display(Name = "需求數量")] //訂單原始數量 交易數量
        public decimal SRC_REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //交易單位已揀數合計 由主單位已揀數合計 換算過來
        public decimal? SRC_PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //交易單位
        public string SRC_REQUESTED_QUANTITY_UOM { get; set; }


        [Display(Name = "備註")]
        public string REMARK { get; set; }//要刪掉

        [Display(Name = "建立人員")]
        public long CREATED_BY { get; set; }
        [Display(Name = "建立日期")]
        public DateTime CREATION_DATE { get; set; }
        [Display(Name = "更新人員")]
        public long LAST_UPDATED_BY { get; set; }
        [Display(Name = "更新日期")]
        public DateTime LAST_UPDATE_DATE { get; set; }

        public string TRIP_NAME { get; set; }//要刪掉
        public string DELIVERY_NAME { get; set; }//要刪掉

    }

    public class FlatEditData
    {
        public static List<FlatEditDT> model = new List<FlatEditDT>();

        public static List<FlatEditDT> getModel(string DELIVERY_NAME, string TRIP_NAME) //改成用DELIVERY_DETAIL_ID SELECT
        {
            var query = from data in model
                        where DELIVERY_NAME == data.DELIVERY_NAME && TRIP_NAME == data.TRIP_NAME
                        select data;
            List<FlatEditDT> list = query.ToList<FlatEditDT>();
            return list;
        }

        public List<FlatEditDT> GetFlatDetailDT(DeliveryUOW uow, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            return uow.GetFlatDetailDT(DlvHeaderId, DELIVERY_STATUS_NAME);
            //var query = from data in PaperRollEditData.model
            //            where data.DlvHeaderId == DlvHeaderId
            //            select data;
            //List<PaperRollEditDT> list = query.ToList<PaperRollEditDT>();
            //return list;
        }

        public static void resetData()
        {
            model = new List<FlatEditDT>();
        }

        public static void addDefault()
        {
            FlatEditDT data = new FlatEditDT();
            data.ID = 1;
            data.TRIP_NAME = "Y191226-1036357";
            data.DELIVERY_NAME = "FTY1912000547";
            data.ORDER_NUMBER = 1192006167;
            data.ORDER_SHIP_NUMBER = "1.2";
            data.OSP_BATCH_NO = "P9B0288";
            data.TMP_ITEM_NUMBER = "";
            data.ITEM_NUMBER = "4A003A01000310K266K";
            data.REAM_WEIGHT = "58.97";
            data.PACKING_TYPE = "令包";
            data.REQUESTED_QUANTITY = 1337.419M;
            data.PICKED_QUANTITY = 0;
            data.REQUESTED_QUANTITY_UOM = "KG";
            data.REQUESTED_QUANTITY2 = 50;
            data.PICKED_QUANTITY2 = 0;
            data.REQUESTED_QUANTITY_UOM2 = "RE";
            data.SRC_REQUESTED_QUANTITY = 1.33742M;
            data.SRC_PICKED_QUANTITY = 0;
            data.SRC_REQUESTED_QUANTITY_UOM = "MT";
            data.CREATED_BY = 1;
            data.CREATION_DATE = DateTime.Now;
            data.LAST_UPDATED_BY = 1;
            data.LAST_UPDATE_DATE = DateTime.Now;
            data.REMARK = "";
            model.Add(data);

            FlatEditDT data2 = new FlatEditDT();
            data2.ID = 2;
            data2.TRIP_NAME = "Y200109-1052058";
            data2.DELIVERY_NAME = "FTY2001000140";
            data2.ORDER_NUMBER = 1202000114;
            data2.ORDER_SHIP_NUMBER = "1.1";
            data2.OSP_BATCH_NO = "P2010087";
            data2.TMP_ITEM_NUMBER = "";
            data2.ITEM_NUMBER = "4AB23P00699350K250K";
            data2.REAM_WEIGHT = "43.5";
            data2.PACKING_TYPE = "無令打件";
            data2.REQUESTED_QUANTITY = 374.8945M;
            data2.PICKED_QUANTITY = 0;
            data2.REQUESTED_QUANTITY_UOM = "KG";
            data2.REQUESTED_QUANTITY2 = 19;
            data2.PICKED_QUANTITY2 = 0;
            data2.REQUESTED_QUANTITY_UOM2 = "RE";
            data2.SRC_REQUESTED_QUANTITY = 0.37489M;
            data2.SRC_PICKED_QUANTITY = 0;
            data2.SRC_REQUESTED_QUANTITY_UOM = "MT";
            data2.CREATED_BY = 1;
            data2.CREATION_DATE = DateTime.Now;
            data2.LAST_UPDATED_BY = 1;
            data2.LAST_UPDATE_DATE = DateTime.Now;
            data2.REMARK = "";
            model.Add(data2);
        }

        //public static void addDefault(string DELIVERY_NAME, string TRIP_NAME)
        //{
        //    if (DELIVERY_NAME == "FTY1912000547" && TRIP_NAME == "Y191226-1036357")
        //    {
        //        if (!checkDefaultModel(DELIVERY_NAME, TRIP_NAME))
        //        {
        //            FlatEditDT data = new FlatEditDT();
        //            data.ID = 1;
        //            data.TRIP_NAME = "Y191226-1036357";
        //            data.DELIVERY_NAME = "FTY1912000547";
        //            data.ORDER_NUMBER = 1192006167;
        //            data.ORDER_SHIP_NUMBER = "1.2";
        //            data.OSP_BATCH_NO = "P9B0288";
        //            data.TMP_ITEM_NUMBER = "";
        //            data.ITEM_DESCRIPTION = "4A003A01000310K266K";
        //            data.REAM_WEIGHT = "58.97";
        //            data.PACKING_TYPE = "令包";
        //            data.REQUESTED_QUANTITY = 1337.419M;
        //            data.PICKED_QUANTITY = 0;
        //            data.REQUESTED_QUANTITY_UOM = "KG";
        //            data.REQUESTED_QUANTITY2 = 50;
        //            data.PICKED_QUANTITY2 = 0;
        //            data.SRC_REQUESTED_QUANTITY_UOM2 = "RE";
        //            data.SRC_REQUESTED_QUANTITY = 1.33742M;
        //            data.SRC_PICKED_QUANTITY = 0;
        //            data.SRC_REQUESTED_QUANTITY_UOM = "MT";
        //            data.CREATED_BY = 1;
        //            data.CREATION_DATE = DateTime.Now;
        //            data.LAST_UPDATED_BY = 1;
        //            data.LAST_UPDATE_DATE = DateTime.Now;
        //            data.REMARK = "";
        //            model.Add(data);
        //        }
        //    }

        //    if (DELIVERY_NAME == "FTY2001000140" && TRIP_NAME == "Y200109-1052058")
        //    {
        //        if (!checkDefaultModel(DELIVERY_NAME, TRIP_NAME))
        //        {
        //            FlatEditDT data2 = new FlatEditDT();
        //            data2.ID = 2;
        //            data2.TRIP_NAME = "Y200109-1052058";
        //            data2.DELIVERY_NAME = "FTY2001000140";
        //            data2.ORDER_NUMBER = 1202000114;
        //            data2.ORDER_SHIP_NUMBER = "1.1";
        //            data2.OSP_BATCH_NO = "P2010087";
        //            data2.TMP_ITEM_NUMBER = "";
        //            data2.ITEM_DESCRIPTION = "4AB23P00699350K250K";
        //            data2.REAM_WEIGHT = "43.5";
        //            data2.PACKING_TYPE = "無令打件";
        //            data2.REQUESTED_QUANTITY = 374.8945M;
        //            data2.PICKED_QUANTITY = 0;
        //            data2.REQUESTED_QUANTITY_UOM = "KG";
        //            data2.REQUESTED_QUANTITY2 = 19;
        //            data2.PICKED_QUANTITY2 = 0;
        //            data2.SRC_REQUESTED_QUANTITY_UOM2 = "RE";
        //            data2.SRC_REQUESTED_QUANTITY = 0.37489M;
        //            data2.SRC_PICKED_QUANTITY = 0;
        //            data2.SRC_REQUESTED_QUANTITY_UOM = "MT";
        //            data2.CREATED_BY = 1;
        //            data2.CREATION_DATE = DateTime.Now;
        //            data2.LAST_UPDATED_BY = 1;
        //            data2.LAST_UPDATE_DATE = DateTime.Now;
        //            data2.REMARK = "";
        //            model.Add(data2);
        //        }
        //    }

        //    //FlatEditDT data3 = new FlatEditDT();
        //    //data3.ID = 3;
        //    //data3.ORDER_NUMBER = 1;
        //    //data3.ORDER_SHIP_NUMBER = "OSN002";
        //    //data3.OSP_BATCH_NO = "P960009";
        //    //data3.TMP_ITEM_NUMBER = "";
        //    //data3.ITEM_DESCRIPTION = "4DM00P03000297K476K";
        //    //data3.REAM_WEIGHT = "304.35";
        //    //data3.PACKING_TYPE = "無令打件";
        //    //data3.REQUESTED_QUANTITY = 1200;
        //    //data3.PICKED_QUANTITY = 0;
        //    //data3.REQUESTED_QUANTITY_UOM = "KG";
        //    //data3.REQUESTED_QUANTITY2 = 120;
        //    //data3.PICKED_QUANTITY2 = 0;
        //    //data3.SRC_REQUESTED_QUANTITY_UOM2 = "RE";
        //    //data3.SRC_REQUESTED_QUANTITY = 1.2M;
        //    //data3.SRC_PICKED_QUANTITY = 0;
        //    //data3.SRC_REQUESTED_QUANTITY_UOM = "MT";
        //    //data3.CREATED_BY = 1;
        //    //data3.CREATION_DATE = DateTime.Now;
        //    //data3.LAST_UPDATED_BY = 1;
        //    //data3.LAST_UPDATE_DATE = DateTime.Now;
        //    //data.REMARK = "";
        //    //model.Add(data3);
        //}



        public static bool checkDefaultModel(string DELIVERY_NAME, string TRIP_NAME)
        {

            var query = from data in model
                        where DELIVERY_NAME == data.DELIVERY_NAME && TRIP_NAME == data.TRIP_NAME
                        select data;
            List<FlatEditDT> list = query.ToList<FlatEditDT>();

            if (list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        //public static bool remove(List<long> PICKED_ID)
        //{
        //    bool result = false;
        //    var query = from barcode in FlatEditBarcodeData.model
        //                where PICKED_ID.Contains(barcode.PICKED_ID)
        //                select barcode;
        //    List<FlatEditBarcodeDT> removeList = query.ToList<FlatEditBarcodeDT>();
        //    foreach (FlatEditBarcodeDT barcode in removeList)
        //    {
        //        result = model.Remove(barcode);
        //    }
        //    return result;

        //}

        //Barcode 123
        public static void updateF001(decimal qty)
        {
            foreach (FlatEditDT obj in model)
            {
                if (obj.ID == 1)
                {
                    decimal newQty = (decimal)obj.PICKED_QUANTITY + qty;
                    obj.PICKED_QUANTITY2 = newQty;
                    obj.PICKED_QUANTITY = newQty * 26.74838M;
                    obj.SRC_PICKED_QUANTITY = Math.Round(newQty * 26.74838M / 1000, 5, MidpointRounding.AwayFromZero);
                    break;
                }
            }
        }


        //Barcode 456
        public static void updateF002()
        {
            decimal addQty = 120;
            foreach (FlatEditDT obj in model)
            {
                if (obj.ID == 2)
                {
                    //decimal newQty = obj.PICKED_QUANTITY + addQty;
                    //obj.PICKED_QUANTITY2 = newQty;
                    //obj.PICKED_QUANTITY = newQty * 10;
                    //obj.SRC_PICKED_QUANTITY = newQty * 10 / 1000;
                    obj.PICKED_QUANTITY2 = 19M;
                    obj.PICKED_QUANTITY = 374.8945M;
                    obj.SRC_PICKED_QUANTITY = 0.37489M;
                    break;
                }
            }
        }

        //Barcode 130
        public static void updateF003()
        {
            decimal addQty = 120;
            foreach (FlatEditDT obj in model)
            {
                if (obj.ID == 3)
                {
                    decimal newQty = (decimal)obj.PICKED_QUANTITY + addQty;
                    obj.PICKED_QUANTITY2 = newQty;
                    obj.PICKED_QUANTITY = newQty * 10;
                    obj.SRC_PICKED_QUANTITY = newQty * 10 / 1000;
                    break;
                }
            }
        }

        public static void remove(List<FlatEditBarcodeDT> barcodeList)
        {
            foreach (FlatEditBarcodeDT barcodeData in barcodeList)
            {
                foreach (FlatEditDT obj in model)
                {
                    if (obj.ID == barcodeData.DLV_DETAIL_ID)
                    {
                        decimal newQty = (decimal)obj.PICKED_QUANTITY2 - barcodeData.SECONDARY_QUANTITY;
                        obj.PICKED_QUANTITY = newQty * 10;
                        obj.PICKED_QUANTITY2 = newQty;
                        obj.SRC_PICKED_QUANTITY = newQty * 10 / 1000;
                    }
                }
            }
        }

        public static bool checkBarcodeItemDesc(long FlatEditDT_ID, string ITEM_DESCRIPTION)
        {
            var query = from data in FlatEditData.model
                        where FlatEditDT_ID == data.ID && ITEM_DESCRIPTION == data.ITEM_NUMBER
                        select data;
            List<FlatEditDT> list = query.ToList<FlatEditDT>();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //檢查交運單是否揀完
        public static bool checkDeliveryPickComplete(long DlvHeaderId)
        {
            var query = from data in FlatEditData.model
                        where data.PICKED_QUANTITY == data.REQUESTED_QUANTITY &&
                        data.PICKED_QUANTITY2 == data.REQUESTED_QUANTITY2 &&
                        data.SRC_PICKED_QUANTITY == data.SRC_REQUESTED_QUANTITY
                        && DlvHeaderId == data.ID
                        select data;

            List<FlatEditDT> list = query.ToList<FlatEditDT>();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public static void removeF001(decimal qty)
        //{

        //    foreach (FlatEditDT obj in model)
        //    {
        //        if (obj.ID == 1)
        //        {
        //            decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + qty;
        //            obj.PICKED_QUANTITY = Convert.ToString(newQty);
        //            obj.SRC_PICKED_QUANTITY = Convert.ToString(newQty / 1000);
        //            break;
        //        }
        //    }
        //}

        //public static void removeF002()
        //{
        //    decimal addQty = -100;
        //    foreach (FlatEditDT obj in model)
        //    {
        //        if (obj.ID == 1)
        //        {
        //            decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
        //            obj.PICKED_QUANTITY = Convert.ToString(newQty);
        //            obj.SRC_PICKED_QUANTITY = Convert.ToString(newQty / 1000);
        //            break;
        //        }
        //    }
        //}
    }

    internal class FlatEditDTOrder
    {
        public static IOrderedEnumerable<FlatEditDT> Order(List<Order> orders, IEnumerable<FlatEditDT> models)
        {
            IOrderedEnumerable<FlatEditDT> orderedModel = null;
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


        private static IOrderedEnumerable<FlatEditDT> OrderBy(int column, string dir, IEnumerable<FlatEditDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ID) : models.OrderBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ORDER_NUMBER) : models.OrderBy(x => x.ORDER_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ORDER_SHIP_NUMBER) : models.OrderBy(x => x.ORDER_SHIP_NUMBER);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OSP_BATCH_NO) : models.OrderBy(x => x.OSP_BATCH_NO);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TMP_ITEM_NUMBER) : models.OrderBy(x => x.TMP_ITEM_NUMBER);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REAM_WEIGHT) : models.OrderBy(x => x.REAM_WEIGHT);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PACKING_TYPE) : models.OrderBy(x => x.PACKING_TYPE);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY) : models.OrderBy(x => x.REQUESTED_QUANTITY);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY) : models.OrderBy(x => x.PICKED_QUANTITY);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY2) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY2) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY_UOM2) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_REQUESTED_QUANTITY) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_PICKED_QUANTITY) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_REQUESTED_QUANTITY_UOM) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);

            }
        }

        private static IOrderedEnumerable<FlatEditDT> ThenBy(int column, string dir, IOrderedEnumerable<FlatEditDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ID) : models.ThenBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ORDER_NUMBER) : models.ThenBy(x => x.ORDER_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ORDER_SHIP_NUMBER) : models.ThenBy(x => x.ORDER_SHIP_NUMBER);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OSP_BATCH_NO) : models.ThenBy(x => x.OSP_BATCH_NO);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.TMP_ITEM_NUMBER) : models.ThenBy(x => x.TMP_ITEM_NUMBER);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REAM_WEIGHT) : models.ThenBy(x => x.REAM_WEIGHT);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PACKING_TYPE) : models.ThenBy(x => x.PACKING_TYPE);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY) : models.ThenBy(x => x.REQUESTED_QUANTITY);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY) : models.ThenBy(x => x.PICKED_QUANTITY);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY2) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY2) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY_UOM2) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_REQUESTED_QUANTITY) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_PICKED_QUANTITY) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_REQUESTED_QUANTITY_UOM) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);

            }
        }
    }
}