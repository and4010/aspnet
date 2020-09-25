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
        public List<FlatEditDT> GetFlatDetailDT(DeliveryUOW uow, long DlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            return uow.GetFlatDetailDT(DlvHeaderId, DELIVERY_STATUS_NAME);
        }
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUB_ID) : models.OrderBy(x => x.SUB_ID);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUB_ID) : models.ThenBy(x => x.SUB_ID);
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