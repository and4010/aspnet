using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Delivery
{
    public class DeliveryORG
    {

        //作業單元ID(OU)
        public long ORG_ID { get; set; }

        //作業單元(OU)
        public string ORG_NAME { get; set; }

        //庫存組織ID
        public long ORGANIZATION_ID { get; set; }

        //庫存組織
        public string ORGANIZATION_CODE { get; set; }

        //車次
        public string TRIP_CAR { get; set; }

        //航程號ID
        public long TRIP_ID { get; set; }

        //航程號
        public string TRIP_NAME { get; set; }

        //組車日
        public DateTime TRIP_ACTUAL_SHIP_DATE { get; set; }

        //交貨單ID
        public long DELIVERY_ID { get; set; }

        //交貨單號
        public string DELIVERY_NAME { get; set; }

        //客戶ID
        public long CUSTOMER_ID { get; set; }

        //訂單客戶編號
        public string CUSTOMER_NUMBER { get; set; }

        //客戶名稱
        public string CUSTOMER_NAME { get; set; }

        //送貨地點
        public string CUSTOMER_LOCATION_CODE { get; set; }

        //送貨客戶ID
        public long SHIP_CUSTOMER_ID { get; set; }

        //送貨客戶編號
        public string SHIP_CUSTOMER_NUMBER { get; set; }

        //送貨客戶名稱
        public string SHIP_CUSTOMER_NAME { get; set; }

        //送貨客戶地點
        public string SHIP_LOCATION_CODE { get; set; }

        //內銷地區別
        public string FREIGHT_TERMS_NAME { get; set; }

        //訂單ID
        public long ORDER_HEADER_ID { get; set; }

        //訂單編號
        public long ORDER_NUMBER { get; set; }

        //訂單明細ID
        public long ORDER_LINE_ID { get; set; }

        //訂單行號
        public string ORDER_SHIP_NUMBER { get; set; }

        //出貨明細ID
        public long DELIVERY_DETAIL_ID { get; set; }

        //包裝方式
        public string PACKING_TYPE { get; set; }

        //料號ID
        public long INVENTORY_ITEM_ID { get; set; }

        //料號
        public string ITEM_NUMBER { get; set; }

        //料號名稱
        public string ITEM_DESCRIPTION { get; set; }

        //紙別
        public string PAPER_TYPE { get; set; }

        //基重
        public string BASIC_WEIGHT { get; set; }

        //規格
        public string SPECIFICATION { get; set; }

        //絲向
        public string GRAIN_DIRECTION { get; set; }

        //出貨倉庫
        public string SUBINVENTORY_CODE { get; set; }

        //出貨儲位ID
        public long LOCATOR_ID { get; set; }

        //出貨儲位
        public string LOCATOR_CODE { get; set; }

        //訂單原始數量
        public decimal SRC_REQUESTED_QUANTITY { get; set; }

        //訂單主單位
        public string SRC_REQUESTED_QUANTITY_UOM { get; set; }

        //預計出庫量
        public decimal REQUESTED_QUANTITY { get; set; }

        //庫存單位
        public string REQUESTED_QUANTITY_UOM { get; set; }

        //預計出庫輔數量
        public decimal REQUESTED_QUANTITY2 { get; set; }

        //輔單位
        public string SRC_REQUESTED_QUANTITY_UOM2 { get; set; }

        //捲號
        public string LOT_NUMBER { get; set; }

        //捲號數量
        public decimal LOT_QUANTITY { get; set; }

        //備註
        public string REMARK { get; set; }

        //建立人員
        public long CREATED_BY { get; set; }

        //建立日期
        public DateTime CREATION_DATE { get; set; }

        //更新人員
        public long LAST_UPDATED_BY { get; set; }

        //更新日期
        public DateTime LAST_UPDATE_DATE { get; set; }

        //代紙料號ID
        public long TMP_INVENTORY_ITEM_ID { get; set; }

        //代紙料號
        public string TMP_ITEM_NUMBER { get; set; }

        //代紙料號名稱
        public string TMP_ITEM_DESCRIPTION { get; set; }

    }

    public class DeliveryTestData
    {
        public static int a = 2;
        public static List<DeliveryORG> orgList = new List<DeliveryORG>();

        public static List<DeliveryHeader> headerList = new List<DeliveryHeader>();

        public static List<DeliveryDetail> detailList = new List<DeliveryDetail>();

        public static List<DeliveryPicked> pickedList = new List<DeliveryPicked>();

        public static void AddDefaultData()
        {
            #region 新增ORG
            DeliveryORG org = new DeliveryORG();
            //作業單元ID(OU)
            org.ORG_ID = 1;
            //作業單元(OU)
            org.ORG_NAME = "";
            //庫存組織ID
            org.ORGANIZATION_ID = 1;

            //庫存組織
            org.ORGANIZATION_CODE = "TB2";

            //車次
            org.TRIP_CAR = "123";

            //航程號ID
            org.TRIP_ID = 1;

            //航程號
            org.TRIP_NAME = "Y191226-1036357";

            //組車日
            org.TRIP_ACTUAL_SHIP_DATE = DateTime.Now;

            //交貨單ID
            org.DELIVERY_ID = 1;

            //交貨單號
            org.DELIVERY_NAME = "FTY1910000150";

            //客戶ID
            org.CUSTOMER_ID = 1;

            //訂單客戶編號
            org.CUSTOMER_NUMBER = "1";

            //客戶名稱
            org.CUSTOMER_NAME = "小明";

            //送貨地點
            org.CUSTOMER_LOCATION_CODE = "台北";

            //送貨客戶ID
            org.SHIP_CUSTOMER_ID = 1;

            //送貨客戶編號
            org.SHIP_CUSTOMER_NUMBER = "1";

            //送貨客戶名稱
            org.SHIP_CUSTOMER_NAME = "小明";

            //送貨客戶地點
            org.SHIP_LOCATION_CODE = "台北";

            //內銷地區別
            org.FREIGHT_TERMS_NAME = "A";

            //訂單ID
            org.ORDER_HEADER_ID = 1;

            //訂單編號
            org.ORDER_NUMBER = 123;

            //訂單明細ID
            org.ORDER_LINE_ID = 1;

            //訂單行號
            org.ORDER_SHIP_NUMBER = "1";

            //出貨明細ID
            org.DELIVERY_DETAIL_ID = 1;

            //包裝方式
            org.PACKING_TYPE = "令包";

            //料號ID
            org.INVENTORY_ITEM_ID = 1;

            //料號
            org.ITEM_NUMBER = "123";

            //料號名稱
            org.ITEM_DESCRIPTION = "123";

            //紙別
            org.PAPER_TYPE = "塗佈白紙板";

            //基重
            org.BASIC_WEIGHT = "03500";

            //規格
            org.SPECIFICATION = "345K512K";

            //絲向
            org.GRAIN_DIRECTION = "L";

            //出貨倉庫
            org.SUBINVENTORY_CODE = "TB2";

            //出貨儲位ID
            org.LOCATOR_ID = 1;

            //出貨儲位
            org.LOCATOR_CODE = "123";

            //訂單原始數量
            org.SRC_REQUESTED_QUANTITY = 1;

            //訂單主單位
            org.SRC_REQUESTED_QUANTITY_UOM = "頓";

            //預計出庫量
            org.REQUESTED_QUANTITY = 1000;

            //庫存單位
            org.REQUESTED_QUANTITY_UOM = "KG";

            //預計出庫輔數量
            org.REQUESTED_QUANTITY2 = 100;

            //輔單位
            org.SRC_REQUESTED_QUANTITY_UOM2 = "令";

            //捲號
            org.LOT_NUMBER = "123";

            //捲號數量
            org.LOT_QUANTITY = 1;

            //備註
            org.REMARK = "ABC";

            //建立人員
            org.CREATED_BY = 1;

            //建立日期
            org.CREATION_DATE = DateTime.Now;

            //更新人員
            org.LAST_UPDATED_BY = 2;

            //更新日期
            org.LAST_UPDATE_DATE = DateTime.Now;

            //代紙料號ID
            org.TMP_INVENTORY_ITEM_ID = 1;

            //代紙料號
            org.TMP_ITEM_NUMBER = "T123";

            //代紙料號名稱
            org.TMP_ITEM_DESCRIPTION = "T123";


            orgList.Add(org);

            #endregion

            #region 新增header
            DeliveryHeader header = new DeliveryHeader();
            header.DELIVERY_HEADER_ID = 1;
            header.ORG_ID = 1;
            header.ORG_NAME = "";
            header.ORGANIZATION_ID = 1;
            header.ORGANIZATION_CODE = "TB2";
            header.TRIP_CAR = "123";
            header.TRIP_ID = 1;
            header.TRIP_NAME = "Y191226-1036357";
            header.TRIP_ACTUAL_SHIP_DATE = DateTime.Now;
            header.DELIVERY_ID = 1;
            header.DELIVERY_NAME = "FTY1910000150";
            header.CUSTOMER_ID = 1;
            header.CUSTOMER_NUMBER = "1";
            header.CUSTOMER_NAME = "小明";
            header.CUSTOMER_LOCATION_CODE = "台北";
            header.SHIP_CUSTOMER_ID = 1;
            header.SHIP_CUSTOMER_NUMBER = "1";
            header.SHIP_CUSTOMER_NAME = "小明";
            header.SHIP_LOCATION_CODE = "台北";
            header.FREIGHT_TERMS_NAME = "A";
            header.ORDER_HEADER_ID = 1;
            header.ORDER_NUMBER = 123;
            header.ORDER_LINE_ID = 1;
            header.ORDER_SHIP_NUMBER = "1";
            header.DELIVERY_STATUS = "已揀";
            header.TRANSACTION_BY = 1;
            header.TRANSACTION_DATE = DateTime.Now;
            header.AUTHORIZE_BY = 1;
            header.AUTHORIZE_DATE = DateTime.Now;
            header.REMARK = "ABC";
            header.CREATED_BY = 1;
            header.CREATION_DATE = DateTime.Now;
            header.LAST_UPDATED_BY = 1;
            header.LAST_UPDATE_DATE = DateTime.Now;
            headerList.Add(header);
            #endregion

            #region 新增detail
            DeliveryDetail detail = new DeliveryDetail();
            detail.DELIVERY_DETAIL_ID = 1;
            detail.DELIVERY_HEADER_ID = 1;
            detail.PACKING_TYPE = "令包";
            detail.INVENTORY_ITEM_ID = 1;
            detail.ITEM_NUMBER = "123";
            detail.ITEM_DESCRIPTION = "123";
            detail.PAPER_TYPE = "塗佈白紙板";
            detail.BASIC_WEIGHT = "03500";
            detail.SPECIFICATION = "345K512K";
            detail.GRAIN_DIRECTION = "L";
            detail.SUBINVENTORY_CODE = "TB2";
            detail.LOCATOR_ID = 1;
            detail.LOCATOR_CODE = "123";
            detail.SRC_REQUESTED_QUANTITY = 100;
            detail.SRC_REQUESTED_QUANTITY_UOM = "頓";
            detail.REQUESTED_QUANTITY = 50;
            detail.REQUESTED_QUANTITY_UOM = "KG";
            detail.REQUESTED_QUANTITY2 = 30;
            detail.SRC_REQUESTED_QUANTITY_UOM2 = "令";
            detail.OSP_BATCH_ID = 1;
            detail.OSP_BATCH_NO = "123";
            detail.OSP_BATCH_TYPE = "OSP";
            detail.TMP_INVENTORY_ITEM_ID = 0;
            detail.TMP_ITEM_NUMBER = "";
            detail.TMP_ITEM_DESCRIPTION = "";
            detail.CREATED_BY = 1;
            detail.CREATION_DATE = DateTime.Now;
            detail.LAST_UPDATED_BY = 1;
            detail.LAST_UPDATE_DATE = DateTime.Now;
            detailList.Add(detail);

            #endregion

            #region 新增picked
            DeliveryPicked picked = new DeliveryPicked();
            picked.PICKED_ID = 1;
            picked.DELIVERY_DETAIL_ID = 1;
            picked.STATUS = "";
            picked.INVENTORY_ITEM_ID = 1;
            picked.BARCODE = "123";
            picked.PRIMARY_QUANTITY = 100;
            picked.PRIMARY_UOM = "公斤";
            picked.SECONDARY_QUANTITY = 10;
            picked.SECONDARY_UOM = "令";
            picked.LOT_NUMBER = "123";
            picked.LOT_QUANTITY = 1;
            picked.LOCATOR_ID = 1;
            picked.LOCATOR_CODE = "123";
            picked.CREATED_BY = 1;
            picked.CREATION_DATE = DateTime.Now;
            picked.LAST_UPDATED_BY = 1;
            picked.LAST_UPDATE_DATE = DateTime.Now;
            pickedList.Add(picked);
            DeliveryPicked picked2 = new DeliveryPicked();
            picked2.PICKED_ID = 2;
            picked2.DELIVERY_DETAIL_ID = 1;
            picked2.STATUS = "";
            picked2.INVENTORY_ITEM_ID = 1;
            picked2.BARCODE = "124";
            picked2.PRIMARY_QUANTITY = 100;
            picked2.PRIMARY_UOM = "公斤";
            picked2.SECONDARY_QUANTITY = 10;
            picked2.SECONDARY_UOM = "令";
            picked2.LOT_NUMBER = "124";
            picked2.LOT_QUANTITY = 1;
            picked2.LOCATOR_ID = 2;
            picked2.LOCATOR_CODE = "124";
            picked2.CREATED_BY = 1;
            picked2.CREATION_DATE = DateTime.Now;
            picked2.LAST_UPDATED_BY = 1;
            picked2.LAST_UPDATE_DATE = DateTime.Now;
            pickedList.Add(picked2);
            #endregion
        }

        public static List<PaperRollEditDT> GetPaperRollEditDT(string TRIP_NAME, string DELIVERY_NAME)
        {
            List<PaperRollEditDT> paperRollEditDTList = new List<PaperRollEditDT>();
            var query = from org in orgList
                        join header in headerList
                        on org.ORG_ID equals header.ORG_ID into headerGroup
                        from h in headerGroup.DefaultIfEmpty()
                        join detail in detailList
                        on (h == null ? 0 : h.DELIVERY_HEADER_ID) equals detail.DELIVERY_HEADER_ID into detailGroup
                        from d in detailGroup.DefaultIfEmpty()
                        join picked in pickedList
                        on (d == null ? 0 : d.DELIVERY_DETAIL_ID) equals picked.DELIVERY_DETAIL_ID into pickedGroup
                        from p in pickedGroup.DefaultIfEmpty()
                        where org.TRIP_NAME == TRIP_NAME && org.DELIVERY_NAME == DELIVERY_NAME

                        select new PaperRollEditDT
                        {

                            ORDER_NUMBER = org.ORDER_NUMBER,
                            OSP_BATCH_NO = d.OSP_BATCH_NO,

                            ITEM_DESCRIPTION = org.ITEM_DESCRIPTION,

                            TMP_ITEM_NUMBER = org.TMP_ITEM_DESCRIPTION,

                            PAPER_TYPE = org.PAPER_TYPE,

                            BASIC_WEIGHT = org.BASIC_WEIGHT,

                            SPECIFICATION = org.SPECIFICATION,

                            REQUESTED_QUANTITY = org.REQUESTED_QUANTITY,


                            //主單位已揀數合計
                            PICKED_QUANTITY = pickedGroup.Sum(x => x.PRIMARY_QUANTITY),

                            REQUESTED_QUANTITY_UOM = org.REQUESTED_QUANTITY_UOM,

                            SRC_REQUESTED_QUANTITY = org.SRC_REQUESTED_QUANTITY,

                            //交易單位已揀數合計 由主單位已揀數合計 換算過來
                            SRC_PICKED_QUANTITY = (pickedGroup.Sum(x => x.PRIMARY_QUANTITY)) / 1000,

                            SRC_REQUESTED_QUANTITY_UOM = org.SRC_REQUESTED_QUANTITY_UOM

                        };

          

            return query.ToList();
        }
    }
}