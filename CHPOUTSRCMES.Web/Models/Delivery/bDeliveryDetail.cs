using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public static class bDeliveryDetail
    {
        public class Model
        {
            public long Id { get; set; }

            [Display(Name = "作業單元ID(OU)")]
            public long ORG_ID { get; set; }

            [Display(Name = "作業單元(OU)")]
            public string ORG_NAME { get; set; }

            [Display(Name = "庫存組織ID")]
            public long ORGANIZATION_ID { get; set; }

            [Display(Name = "庫存組織")]
            public string ORGANIZATION_CODE { get; set; }

            [Display(Name = "車次")]
            public string TRIP_CAR { get; set; }

            [Display(Name = "航程號ID")]
            public long TRIP_ID { get; set; }

            [Display(Name = "航程號")]
            public string TRIP_NAME { get; set; }

            [Display(Name = "組車日")]
            public string TRIP_ACTUAL_SHIP_DATE { get; set; }

            [Display(Name = "交貨單ID")]
            public long DELIVERY_ID { get; set; }

            [Display(Name = "交貨單號")]
            public string DELIVERY_NAME { get; set; }

            [Display(Name = "客戶ID")]
            public long CUSTOMER_ID { get; set; }

            [Display(Name = "訂單客戶編號")]
            public string CUSTOMER_NUMBER { get; set; }

            [Display(Name = "客戶名稱")]
            public string CUSTOMER_NAME { get; set; }

            [Display(Name = "送貨地點")]
            public string CUSTOMER_LOCATION_CODE { get; set; }

            [Display(Name = "送貨客戶ID")]
            public long SHIP_CUSTOMER_ID { get; set; }

            [Display(Name = "送貨客戶編號")]
            public string SHIP_CUSTOMER_NUMBER { get; set; }

            [Display(Name = "送貨客戶名稱")]
            public string SHIP_CUSTOMER_NAME { get; set; }

            [Display(Name = "送貨客戶地點")]
            public string SHIP_LOCATION_CODE { get; set; }

            [Display(Name = "內銷區域別")]
            public string FREIGHT_TERMS_NAME { get; set; }

            [Display(Name = "訂單ID")]
            public long ORDER_HEADER_ID { get; set; }

            [Display(Name = "訂單編號")]
            public long ORDER_NUMBER { get; set; }

            [Display(Name = "訂單明細ID")]
            public long ORDER_LINE_ID { get; set; }

            [Display(Name = "訂單行號")]
            public string ORDER_SHIP_NUMBER { get; set; }

            [Display(Name = "出貨明細ID")]
            public long DELIVERY_DETAIL_ID { get; set; }

            [Display(Name = "交貨單狀態")]
            public string DELIVERY_STATUS { get; set; }

            [Display(Name = "出貨申請日期")]
            public string TRANSACTION_DATE { get; set; }
            



            [Display(Name = "包裝方式")]
            public string PACKING_TYPE { get; set; }

            [Display(Name = "料號ID")]
            public long INVENTORY_ITEM_ID { get; set; }

            [Display(Name = "料號")]
            public string ITEM_NUMBER { get; set; }

            [Display(Name = "料號名稱")]
            public string ITEM_DESCRIPTION { get; set; }

            [Display(Name = "條碼號")]
            public string BARCODE { get; set; }

            [Display(Name = "紙別")]
            public string PAPER_TYPE { get; set; }

            [Display(Name = "基重")]
            public string BASIC_WEIGHT { get; set; }

            [Display(Name = "規格")]
            public string SPECIFICATION { get; set; }

            [Display(Name = "絲向")]
            public string GRAIN_DIRECTION { get; set; }

            [Display(Name = "出貨倉庫")]
            public string SUBINVENTORY_CODE { get; set; }

            [Display(Name = "出貨儲位ID")]
            public long LOCATOR_ID { get; set; }

            [Display(Name = "出貨儲位")]
            public string LOCATOR_CODE { get; set; }

            [Display(Name = "訂單原始數量")]
            public decimal SRC_REQUESTED_QUANTITY { get; set; }

            [Display(Name = "訂單主單位")]
            public string SRC_REQUESTED_QUANTITY_UOM { get; set; }

            [Display(Name = "預計出庫量")]
            public string REQUESTED_QUANTITY { get; set; }

            [Display(Name = "庫存單位")]
            public string REQUESTED_QUANTITY_UOM { get; set; }

            [Display(Name = "預計出庫輔數量")]
            public decimal REQUESTED_QUANTITY2 { get; set; }

            [Display(Name = "輔單位")]
            public string SRC_REQUESTED_QUANTITY_UOM2 { get; set; }

            [Display(Name = "批號")]
            public string LOT_NUMBER { get; set; }

            [Display(Name = "批號數量")]
            public decimal LOT_QUANTITY { get; set; }

            [Display(Name = "備註")]
            public string REMARK { get; set; }

            [Display(Name = "建立人員")]
            public long CREATED_BY { get; set; }

            [Display(Name = "建立日期")]
            public string CREATION_DATE { get; set; }

            [Display(Name = "更新人員")]
            public long LAST_UPDATED_BY { get; set; }

            [Display(Name = "更新日期")]
            public string LAST_UPDATE_DATE { get; set; }


        }

        public static List<Model> model = new List<Model>();


        public static void AddData(Model deliveryDetail)
        {
            model.Add(deliveryDetail);
        }

        public static List<Model> GetData(long DELIVERY_DETAIL_ID)
        {
            var query = from deliveryDetail in model
                        where deliveryDetail.DELIVERY_DETAIL_ID == DELIVERY_DETAIL_ID
                        select deliveryDetail;
            return query.ToList<Model>();
        }

        public static List<Model> GetData()
        {
            var query = from deliveryDetail in model
                        select deliveryDetail;
            return query.ToList<Model>();
        }
    }
}