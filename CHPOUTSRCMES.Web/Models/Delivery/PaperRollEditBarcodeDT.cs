using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class PaperRollEditBarcodeDT
    {
        public long PICKED_ID { get; set; }

        public long SUB_ID { get; set; }

        public long DlvHeaderId { get; set; } //DLV_HEADER_ID

        public long DLV_DETAIL_ID { get; set; } //DLV_DETAIL_ID

        [Display(Name = "料號名稱")]
        public string ITEM_NUMBER { get; set; }

        [Display(Name = "條碼號")]
        public string BARCODE { get; set; }

        [Display(Name = "數量")] //主要數量
        public decimal PRIMARY_QUANTITY { get; set; }

        [Display(Name = "單位")] //主要單位
        public string PRIMARY_UOM { get; set; }

        //[Display(Name = "是否代紙")]
        //public string ISTMP { get; set; } //待判斷是否刪掉

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


    public class PaperRollEditBarcodeData
    {
        public static List<PaperRollEditBarcodeDT> model = new List<PaperRollEditBarcodeDT>();

        public static void resetData()
        {
            model = new List<PaperRollEditBarcodeDT>();
        }

        public static List<PaperRollEditBarcodeDT> getDataList(long DlvHeaderId)
        {
            var query = from data in PaperRollEditBarcodeData.model
                        where data.DlvHeaderId == DlvHeaderId
                        select data;
            List<PaperRollEditBarcodeDT> list = query.ToList<PaperRollEditBarcodeDT>();
            return list;
        }

        public List<PaperRollEditBarcodeDT> GetRollPickDT(DeliveryUOW uow, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {

            return uow.GetRollPickDT(DlvHeaderId, DELIVERY_STATUS_NAME);

        }


        public ResultModel AddPickDT(DeliveryUOW uow, long dlvHeaderId, long dlvDetailId, string deliveryName, string barcode, decimal? qty, string addUser, string addUserName)
        {
            return uow.AddPickDT(dlvHeaderId, dlvDetailId, deliveryName, barcode, qty, addUser, addUserName);
        }

        public static void addBarcode123(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            PaperRollEditBarcodeDT paperRollEditBarcodeDT = new PaperRollEditBarcodeDT();
            paperRollEditBarcodeDT.BARCODE = "W2005060001";
            paperRollEditBarcodeDT.ITEM_NUMBER = "4FHIZA03000787RL00";
            paperRollEditBarcodeDT.DlvHeaderId = DlvHeaderId;
            paperRollEditBarcodeDT.DLV_DETAIL_ID = PaperRollEditDT_ID;
            paperRollEditBarcodeDT.PICKED_ID = 1;
            paperRollEditBarcodeDT.PRIMARY_QUANTITY = 1000;
            paperRollEditBarcodeDT.PRIMARY_UOM = "KG";
            //paperRollEditBarcodeDT.ISTMP = "";
            paperRollEditBarcodeDT.CREATED_BY = 1;
            paperRollEditBarcodeDT.CREATION_DATE = DateTime.Now;
            paperRollEditBarcodeDT.LAST_UPDATED_BY = 1;
            paperRollEditBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            //paperRollEditBarcodeDT.REMARK = "";
            model.Add(paperRollEditBarcodeDT);
        }

        public static void addBarcode124(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            PaperRollEditBarcodeDT paperRollEditBarcodeDT = new PaperRollEditBarcodeDT();
            paperRollEditBarcodeDT.BARCODE = "W2005060004";
            paperRollEditBarcodeDT.ITEM_NUMBER = "4FHIZA03000787RL00";
            paperRollEditBarcodeDT.DlvHeaderId = DlvHeaderId;
            paperRollEditBarcodeDT.DLV_DETAIL_ID = PaperRollEditDT_ID;
            paperRollEditBarcodeDT.PICKED_ID = 2;
            paperRollEditBarcodeDT.PRIMARY_QUANTITY = 1000;
            paperRollEditBarcodeDT.PRIMARY_UOM = "KG";
            //paperRollEditBarcodeDT.ISTMP = "";
            paperRollEditBarcodeDT.CREATED_BY = 1;
            paperRollEditBarcodeDT.CREATION_DATE = DateTime.Now;
            paperRollEditBarcodeDT.LAST_UPDATED_BY = 1;
            paperRollEditBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            //paperRollEditBarcodeDT.REMARK = "";
            model.Add(paperRollEditBarcodeDT);
        }

        public static void addBarcode456(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            PaperRollEditBarcodeDT paperRollEditBarcodeDT = new PaperRollEditBarcodeDT();
            paperRollEditBarcodeDT.BARCODE = "W2005060002";
            paperRollEditBarcodeDT.ITEM_NUMBER = "4FHIZA02500787RL00";
            paperRollEditBarcodeDT.DlvHeaderId = DlvHeaderId;
            paperRollEditBarcodeDT.DLV_DETAIL_ID = PaperRollEditDT_ID;
            paperRollEditBarcodeDT.PICKED_ID = 3;
            paperRollEditBarcodeDT.PRIMARY_QUANTITY = 1000;
            paperRollEditBarcodeDT.PRIMARY_UOM = "KG";
            //paperRollEditBarcodeDT.ISTMP = "是";
            paperRollEditBarcodeDT.CREATED_BY = 1;
            paperRollEditBarcodeDT.CREATION_DATE = DateTime.Now;
            paperRollEditBarcodeDT.LAST_UPDATED_BY = 1;
            paperRollEditBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            //paperRollEditBarcodeDT.REMARK = "";
            model.Add(paperRollEditBarcodeDT);
        }

        public static void addBarcode457(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            PaperRollEditBarcodeDT paperRollEditBarcodeDT = new PaperRollEditBarcodeDT();
            paperRollEditBarcodeDT.BARCODE = "W2005060005";
            paperRollEditBarcodeDT.ITEM_NUMBER = "4FHIZA02500787RL00";
            paperRollEditBarcodeDT.DlvHeaderId = DlvHeaderId;
            paperRollEditBarcodeDT.DLV_DETAIL_ID = PaperRollEditDT_ID;
            paperRollEditBarcodeDT.PICKED_ID = 4;
            paperRollEditBarcodeDT.PRIMARY_QUANTITY = 1000;
            paperRollEditBarcodeDT.PRIMARY_UOM = "KG";
            //paperRollEditBarcodeDT.ISTMP = "是";
            paperRollEditBarcodeDT.CREATED_BY = 1;
            paperRollEditBarcodeDT.CREATION_DATE = DateTime.Now;
            paperRollEditBarcodeDT.LAST_UPDATED_BY = 1;
            paperRollEditBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            //paperRollEditBarcodeDT.REMARK = "";
            model.Add(paperRollEditBarcodeDT);
        }

        public static void addBarcode130(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            PaperRollEditBarcodeDT paperRollEditBarcodeDT = new PaperRollEditBarcodeDT();
            paperRollEditBarcodeDT.BARCODE = "W2005060003";
            paperRollEditBarcodeDT.ITEM_NUMBER = "4FHIZA03000787RL00";
            paperRollEditBarcodeDT.DlvHeaderId = DlvHeaderId;
            paperRollEditBarcodeDT.DLV_DETAIL_ID = PaperRollEditDT_ID;
            paperRollEditBarcodeDT.PICKED_ID = 5;
            paperRollEditBarcodeDT.PRIMARY_QUANTITY = 1000;
            paperRollEditBarcodeDT.PRIMARY_UOM = "KG";
            //paperRollEditBarcodeDT.ISTMP = "";
            paperRollEditBarcodeDT.CREATED_BY = 1;
            paperRollEditBarcodeDT.CREATION_DATE = DateTime.Now;
            paperRollEditBarcodeDT.LAST_UPDATED_BY = 1;
            paperRollEditBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            //paperRollEditBarcodeDT.REMARK = "";
            model.Add(paperRollEditBarcodeDT);
        }

        public static void addBarcode131(long PaperRollEditDT_ID, long DlvHeaderId)
        {
            PaperRollEditBarcodeDT paperRollEditBarcodeDT = new PaperRollEditBarcodeDT();
            paperRollEditBarcodeDT.BARCODE = "W2005060006";
            paperRollEditBarcodeDT.ITEM_NUMBER = "4FHIZA02000787RL00";
            paperRollEditBarcodeDT.DlvHeaderId = DlvHeaderId;
            paperRollEditBarcodeDT.DLV_DETAIL_ID = PaperRollEditDT_ID;
            paperRollEditBarcodeDT.PICKED_ID = 6;
            paperRollEditBarcodeDT.PRIMARY_QUANTITY = 1000;
            paperRollEditBarcodeDT.PRIMARY_UOM = "KG";
            //paperRollEditBarcodeDT.ISTMP = "";
            paperRollEditBarcodeDT.CREATED_BY = 1;
            paperRollEditBarcodeDT.CREATION_DATE = DateTime.Now;
            paperRollEditBarcodeDT.LAST_UPDATED_BY = 1;
            paperRollEditBarcodeDT.LAST_UPDATE_DATE = DateTime.Now;
            //paperRollEditBarcodeDT.REMARK = "";
            model.Add(paperRollEditBarcodeDT);
        }

        public static bool checkBarcodeExist(string barcode)
        {
            var query = from barcodeData in PaperRollEditBarcodeData.model
                        where barcode == barcodeData.BARCODE
                        select barcodeData;
            List<PaperRollEditBarcodeDT> list = query.ToList<PaperRollEditBarcodeDT>();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




        public static bool remove(List<long> PICKED_ID)
        {
            bool result = false;
            var query = from barcode in PaperRollEditBarcodeData.model
                        where PICKED_ID.Contains(barcode.PICKED_ID)
                        select barcode;
            List<PaperRollEditBarcodeDT> removeList = query.ToList<PaperRollEditBarcodeDT>();
            foreach (PaperRollEditBarcodeDT barcode in removeList)
            {
                result = model.Remove(barcode);
            }
            return result;

        }

        public static List<string> getItemDescription(List<long> PICKED_ID)
        {
            var query = from barcode in PaperRollEditBarcodeData.model
                        where PICKED_ID.Contains(barcode.PICKED_ID)
                        select barcode.ITEM_NUMBER;

            return query.ToList();
        }

        public static List<PaperRollEditBarcodeDT> getItemList(List<long> PICKED_ID)
        {
            var query = from barcode in PaperRollEditBarcodeData.model
                        where PICKED_ID.Contains(barcode.PICKED_ID)
                        select barcode;

            return query.ToList();
        }


    }

    internal class PaperRollEditBarcodeDTOrder
    {
        public static IOrderedEnumerable<PaperRollEditBarcodeDT> Order(List<Order> orders, IEnumerable<PaperRollEditBarcodeDT> models)
        {
            IOrderedEnumerable<PaperRollEditBarcodeDT> orderedModel = null;
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


        private static IOrderedEnumerable<PaperRollEditBarcodeDT> OrderBy(int column, string dir, IEnumerable<PaperRollEditBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DLV_DETAIL_ID) : models.OrderBy(x => x.DLV_DETAIL_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BARCODE) : models.OrderBy(x => x.BARCODE);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_QUANTITY) : models.OrderBy(x => x.PRIMARY_QUANTITY);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PRIMARY_UOM) : models.OrderBy(x => x.PRIMARY_UOM);
                //case 6:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REMARK) : models.OrderBy(x => x.REMARK);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATE_DATE) : models.OrderBy(x => x.LAST_UPDATE_DATE);

            }
        }

        private static IOrderedEnumerable<PaperRollEditBarcodeDT> ThenBy(int column, string dir, IOrderedEnumerable<PaperRollEditBarcodeDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DLV_DETAIL_ID) : models.ThenBy(x => x.DLV_DETAIL_ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BARCODE) : models.ThenBy(x => x.BARCODE);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_QUANTITY) : models.ThenBy(x => x.PRIMARY_QUANTITY);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PRIMARY_UOM) : models.ThenBy(x => x.PRIMARY_UOM);
                //case 6:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REMARK) : models.ThenBy(x => x.REMARK);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATE_DATE) : models.ThenBy(x => x.LAST_UPDATE_DATE);

            }
        }
    }
}