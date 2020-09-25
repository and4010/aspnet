using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using static CHPOUTSRCMES.Web.DataModel.UnitOfWorks.DeliveryUOW;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class FlatEditBarcodeDT
    {
        public long PICKED_ID { get; set; }

        public long SUB_ID { get; set; }

        public long DLV_DETAIL_ID { get; set; } //DLV_DETAIL_ID

        public long DlvHeaderId { get; set; } //DLV_HEADER_ID

        [Display(Name = "棧板狀態")]
        public string PALLET_STATUS { get; set; }

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
        public List<FlatEditBarcodeDT> GetFlatPickDT(DeliveryUOW uow, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            return uow.GetFlatPickDT(DlvHeaderId, DELIVERY_STATUS_NAME);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
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