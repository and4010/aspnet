using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class PaperRollEditDT
    {
        public long ID { get; set; }

        public long DlvHeaderId { get; set; }

        //public string DT_RowId { get; set; }

        public long SUB_ID { get; set; }

        [Display(Name = "訂單編號")]
        public long ORDER_NUMBER { get; set; }

        [Display(Name = "訂單行號")]
        public string ORDER_SHIP_NUMBER { get; set; }


        [Display(Name = "工單號碼ID")]
        public long? OSP_BATCH_ID { get; set; }

        [Display(Name = "工單號碼")]
        public string OSP_BATCH_NO { get; set; }

        [Display(Name = "料號ID")]
        public long INVENTORY_ITEM_ID { get; set; }

        [Display(Name = "料號")]
        public string ITEM_NUMBER { get; set; }

        [Display(Name = "代紙料號ID")]
        public long? TMP_ITEM_ID { get; set; }

        [Display(Name = "代紙料號名稱")]
        public string TMP_ITEM_NUMBER { get; set; }

        [Display(Name = "紙別")]
        public string PAPER_TYPE { get; set; }

        [Display(Name = "基重")]
        public string BASIC_WEIGHT { get; set; }

        [Display(Name = "規格")]
        public string SPECIFICATION { get; set; }

        [Display(Name = "需求數量")] //預計出庫量 主要數量
        public decimal REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //主單位已揀數合計
        public decimal? PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //主單位
        public string REQUESTED_QUANTITY_UOM { get; set; }

        [Display(Name = "需求數量")] //訂單原始數量 交易數量
        public decimal SRC_REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //交易單位已揀數合計 由主單位已揀數合計 換算過來
        public decimal? SRC_PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //交易單位
        public string SRC_REQUESTED_QUANTITY_UOM { get; set; }

        [Display(Name = "備註")]
        public string REMARK { get; set; }//要刪掉

    }

    public class PaperRollEditData
    {
        public static List<PaperRollEditDT> model = new List<PaperRollEditDT>();
        private static long id = 0;
        public static void resetData()
        {
            id = 0;
            model = new List<PaperRollEditDT>();
        }

        public static void addDefault(long DlvHeaderId)
        {
            id++;
            PaperRollEditDT paperRollData = new PaperRollEditDT();
            paperRollData.ID = id;
            //paperRollData.DT_RowId = "row_1";
            paperRollData.DlvHeaderId = DlvHeaderId;
            paperRollData.ORDER_NUMBER = 1192006168;
            paperRollData.ORDER_SHIP_NUMBER = "1.1";
            paperRollData.OSP_BATCH_NO = "";
            paperRollData.ITEM_NUMBER = "4FHIZA03000787RL00";
            paperRollData.TMP_ITEM_NUMBER = "";
            paperRollData.PAPER_TYPE = "FHIZ";
            paperRollData.BASIC_WEIGHT = "03000";
            paperRollData.SPECIFICATION = "787";
            paperRollData.REQUESTED_QUANTITY = 1000;
            paperRollData.PICKED_QUANTITY = 0;
            paperRollData.REQUESTED_QUANTITY_UOM = "KG";
            paperRollData.SRC_REQUESTED_QUANTITY = 1;
            paperRollData.SRC_PICKED_QUANTITY = 0;
            paperRollData.SRC_REQUESTED_QUANTITY_UOM = "MT";
            paperRollData.REMARK = "";
            model.Add(paperRollData);

            id++;
            PaperRollEditDT paperRollData2 = new PaperRollEditDT();
            paperRollData2.BASIC_WEIGHT = "02500";
            paperRollData2.ID = id;
            //paperRollData2.DT_RowId = "row_2";
            paperRollData2.DlvHeaderId = DlvHeaderId;
            paperRollData2.ORDER_NUMBER = 1192006168;
            paperRollData2.ORDER_SHIP_NUMBER = "1.1";
            paperRollData2.OSP_BATCH_NO = "";
            paperRollData2.ITEM_NUMBER = "4FHIZA02500787RL00";
            paperRollData2.TMP_ITEM_NUMBER = "";
            paperRollData2.PAPER_TYPE = "FHIZ";
            paperRollData2.BASIC_WEIGHT = "02500";
            paperRollData2.SPECIFICATION = "787";
            paperRollData2.REQUESTED_QUANTITY = 1000;
            paperRollData2.PICKED_QUANTITY = 0;
            paperRollData2.REQUESTED_QUANTITY_UOM = "KG";
            paperRollData2.SRC_REQUESTED_QUANTITY = 1;
            paperRollData2.SRC_PICKED_QUANTITY = 0;
            paperRollData2.SRC_REQUESTED_QUANTITY_UOM = "MT";
            paperRollData2.REMARK = "";
            model.Add(paperRollData2);

            id++;
            PaperRollEditDT paperRollData3 = new PaperRollEditDT();
            paperRollData3.BASIC_WEIGHT = "03500";
            paperRollData3.ID = id;
            //paperRollData3.DT_RowId = "row_3";
            paperRollData3.DlvHeaderId = DlvHeaderId;
            paperRollData3.ORDER_NUMBER = 1192006169;
            paperRollData3.ORDER_SHIP_NUMBER = "1.2";
            paperRollData3.OSP_BATCH_NO = "";
            paperRollData3.ITEM_NUMBER = "4FHIZA02000787RL00";
            paperRollData3.TMP_ITEM_NUMBER = "";
            paperRollData3.PAPER_TYPE = "FHIZ";
            paperRollData3.BASIC_WEIGHT = "02000";
            paperRollData3.SPECIFICATION = "787";
            paperRollData3.REQUESTED_QUANTITY = 1000;
            paperRollData3.PICKED_QUANTITY = 0;
            paperRollData3.REQUESTED_QUANTITY_UOM = "KG";
            paperRollData3.SRC_REQUESTED_QUANTITY = 1;
            paperRollData3.SRC_PICKED_QUANTITY = 0;
            paperRollData3.SRC_REQUESTED_QUANTITY_UOM = "MT";
            paperRollData3.REMARK = "";
            model.Add(paperRollData3);


        }

        public static int getDataCount(long DlvHeaderId)
        {
            var query = from data in PaperRollEditData.model
                        where DlvHeaderId == data.DlvHeaderId
                        select data;
            List<PaperRollEditDT> list = query.ToList<PaperRollEditDT>();
            return list.Count;
        }

        public List<PaperRollEditDT> GetRollDetailDT(DeliveryUOW uow, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {

            return uow.GetRollDetailDT(DlvHeaderId, DELIVERY_STATUS_NAME);
            //var query = from data in PaperRollEditData.model
            //            where data.DlvHeaderId == DlvHeaderId
            //            select data;
            //List<PaperRollEditDT> list = query.ToList<PaperRollEditDT>();
            //return list;
        }

        //Barcode 123
        public static void updateA006(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = 1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }

        //Barcode 456
        public static void updateB001(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = 1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }

        //Barcode 130 跟 1230 不同訂單編號
        public static void updateA006s(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = 1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }


        public static void remove(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = -1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }

        public static void removeA006(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = -1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }

        public static void removeB001(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = -1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }

        public static void removeA006s(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            decimal addQty = -1000;
            foreach (PaperRollEditDT obj in model)
            {
                if (obj.ID == PaperRollEditDT_ID && obj.DlvHeaderId == DlvHeaderId)
                {
                    decimal newQty = Convert.ToDecimal(obj.PICKED_QUANTITY) + addQty;
                    obj.PICKED_QUANTITY = newQty;
                    obj.SRC_PICKED_QUANTITY = newQty / 1000;
                    break;
                }
            }
        }

        public static bool checkBarcodeItemDesc(long PaperRollEditDT_ID, string ITEM_DESCRIPTION)
        {
            var query = from data in PaperRollEditData.model
                        where PaperRollEditDT_ID == data.ID && ITEM_DESCRIPTION == data.ITEM_NUMBER
                        select data;
            List<PaperRollEditDT> list = query.ToList<PaperRollEditDT>();
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
        //public static bool checkDeliveryPickComplete(long DlvHeaderId)
        //{
        //    //var query = from data in PaperRollEditData.model
        //    //            where data.PICKED_QUANTITY == data.REQUESTED_QUANTITY
        //    //            select data;

        //    //List<PaperRollEditDT> list = query.ToList<PaperRollEditDT>();
        //    //if (list.Count > 0 && list.Count == model.Count)
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}

        //    var query = from data in PaperRollEditData.model
        //                where data.DlvHeaderId == DlvHeaderId
        //                group data by new { DlvHeaderId = data.DlvHeaderId } into g
        //                select g.Sum(p => p.PICKED_QUANTITY);

        //    var query2 = from data in TripHeaderData.source
        //                 where data.Id == DlvHeaderId
        //                 select data.REQUESTED_QUANTITY;

        //    if (query.ToList()[0] == query2.ToList()[0])
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //}
    }

    internal class PaperRollEditDTOrder
    {
        public static IOrderedEnumerable<PaperRollEditDT> Order(List<Order> orders, IEnumerable<PaperRollEditDT> models)
        {
            IOrderedEnumerable<PaperRollEditDT> orderedModel = null;
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


        private static IOrderedEnumerable<PaperRollEditDT> OrderBy(int column, string dir, IEnumerable<PaperRollEditDT> models)
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PAPER_TYPE) : models.OrderBy(x => x.PAPER_TYPE);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BASIC_WEIGHT) : models.OrderBy(x => x.BASIC_WEIGHT);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SPECIFICATION) : models.OrderBy(x => x.SPECIFICATION);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY) : models.OrderBy(x => x.REQUESTED_QUANTITY);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY) : models.OrderBy(x => x.PICKED_QUANTITY);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_REQUESTED_QUANTITY) : models.OrderBy(x => x.SRC_REQUESTED_QUANTITY);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_PICKED_QUANTITY) : models.OrderBy(x => x.SRC_PICKED_QUANTITY);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SRC_REQUESTED_QUANTITY_UOM) : models.OrderBy(x => x.SRC_REQUESTED_QUANTITY_UOM);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);

            }
        }

        private static IOrderedEnumerable<PaperRollEditDT> ThenBy(int column, string dir, IOrderedEnumerable<PaperRollEditDT> models)
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PAPER_TYPE) : models.ThenBy(x => x.PAPER_TYPE);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BASIC_WEIGHT) : models.ThenBy(x => x.BASIC_WEIGHT);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SPECIFICATION) : models.ThenBy(x => x.SPECIFICATION);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY) : models.ThenBy(x => x.REQUESTED_QUANTITY);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY) : models.ThenBy(x => x.PICKED_QUANTITY);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_REQUESTED_QUANTITY) : models.ThenBy(x => x.SRC_REQUESTED_QUANTITY);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_PICKED_QUANTITY) : models.ThenBy(x => x.SRC_PICKED_QUANTITY);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SRC_REQUESTED_QUANTITY_UOM) : models.ThenBy(x => x.SRC_REQUESTED_QUANTITY_UOM);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);

            }
        }
    }
}