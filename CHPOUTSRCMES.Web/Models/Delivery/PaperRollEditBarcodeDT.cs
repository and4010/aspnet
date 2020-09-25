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

        [Display(Name = "棧板狀態")]
        public string PALLET_STATUS { get; set; }

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
       
        public List<PaperRollEditBarcodeDT> GetRollPickDT(DeliveryUOW uow, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            return uow.GetRollPickDT(DlvHeaderId, DELIVERY_STATUS_NAME);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
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