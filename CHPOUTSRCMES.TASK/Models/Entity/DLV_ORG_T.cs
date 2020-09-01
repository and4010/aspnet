using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class DLV_ORG_T
    {
        /// <summary>
        /// 出庫主檔ID
        /// </summary>
        /// 
        public long DLV_ORG_ID { set; get; }
        	
        /// <summary>
        /// XXIFP220
        /// </summary>
        /// 
        public string PROCESS_CODE { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string SERVER_CODE { set; get; }

        /// <summary>
        /// 20191112141600100000
        /// </summary>
        /// 
        public string BATCH_ID { set; get; }

        /// <summary>
        /// 1
        /// </summary>
        /// 
        public long BATCH_LINE_ID { set; get; }

        /// <summary>
        /// 作業單元ID(OU)
        /// </summary>
        /// 
        public long ORG_ID { set; get; }


        /// <summary>
        /// 作業單元(OU)
        /// </summary>
        /// 
        public string ORG_NAME { set; get; }

         /// <summary>
        /// 組織ID
        /// </summary>
        /// 
        public long ORGANIZATION_ID { set; get; }


        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        public string ORGANIZATION_CODE { set; get; }

        /// <summary>
        /// 車次
        /// </summary>
        /// 
        public string TRIP_CAR { set; get; }

        /// <summary>
        /// 航程號ID
        /// </summary>
        /// 
        public long TRIP_ID { set; get; }

         /// <summary>
        /// 航程號
        /// </summary>
        /// 
        public string TRIP_NAME { set; get; }

         /// <summary>
        /// 組車日
        /// </summary>
        /// 
        public DateTime TRIP_ACTUAL_SHIP_DATE { set; get; }

        /// <summary>
        /// 交貨單ID
        /// </summary>
        /// 
        public long DELIVERY_ID { set; get; }

        /// <summary>
        /// 交運單號
        /// </summary>
        /// 
        public string DELIVERY_NAME { set; get; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        /// 
        public long CUSTOMER_ID { set; get; }

        /// <summary>
        /// 訂單客戶編號
        /// </summary>
        /// 
        public string CUSTOMER_NUMBER { set; get; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        /// 
        public string CUSTOMER_NAME { set; get; }

        /// <summary>
        /// 送貨地點
        /// </summary>
        /// 
        public string CUSTOMER_LOCATION_CODE { set; get; }

        /// <summary>
        /// 送貨客戶ID
        /// </summary>
        /// 
        public long SHIP_CUSTOMER_ID { set; get; }

        /// <summary>
        /// 送貨客戶編號
        /// </summary>
        /// 
        public string SHIP_CUSTOMER_NUMBER { set; get; }

        /// <summary>
        /// 送貨客戶名稱
        /// </summary>
        /// 
        public string SHIP_CUSTOMER_NAME { set; get; }

        /// <summary>
        /// 送貨客戶地點
        /// </summary>
        /// 
        public string SHIP_LOCATION_CODE { set; get; }

        /// <summary>
        /// 內銷地區別
        /// </summary>
        /// 
        public string FREIGHT_TERMS_NAME { set; get; }

        /// <summary>
        /// 訂單ID
        /// </summary>
        /// 
        public long ORDER_HEADER_ID { set; get; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        /// 
        public long Order_Number { set; get; }

        /// <summary>
        /// 訂單明細ID
        /// </summary>
        /// 
        public long ORDER_LINE_ID { set; get; }

        /// <summary>
        /// 訂單行號
        /// </summary>
        /// 
        public string ORDER_SHIP_NUMBER { set; get; }

        /// <summary>
        /// 出貨明細ID
        /// </summary>
        /// 
        public long DELIVERY_DETAIL_ID { set; get; }

         /// <summary>
        /// 包裝方式
        /// </summary>
        /// 
        public string PACKING_TYPE { set; get; }

        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        public long INVENTORY_ITEM_ID { set; get; }

        /// <summary>
        /// 料號
        /// </summary>
        /// 
        public string ITEM_NUMBER { set; get; }

        /// <summary>
        /// 料號名稱
        /// </summary>
        /// 
        public string ITEM_DESCRIPTION { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        public string PAPER_TYPE { set; get; }

        /// <summary>
        /// 基重
        /// </summary>
        /// 
        public string BASIC_WEIGHT { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        public string SPECIFICATION { set; get; }

        /// <summary>
        /// 絲向
        /// </summary>
        /// 
        public string GRAIN_DIRECTION { set; get; }

        /// <summary>
        /// 出貨倉庫
        /// </summary>
        /// 
        public string SUBINVENTORY_CODE { set; get; }

        /// <summary>
        /// 出貨儲位ID
        /// </summary>
        /// 
        public long? LOCATOR_ID { set; get; }

        /// <summary>
        /// 出貨儲位
        /// </summary>
        /// 
        public string LOCATOR_CODE { set; get; }

        /// <summary>
        /// 訂單原始交易數量
        /// </summary>
        /// 
        public decimal SRC_REQUESTED_QUANTITY { set; get; }

        /// <summary>
        /// 訂單交易單位
        /// </summary>
        /// 
        public string SRC_REQUESTED_QUANTITY_UOM { set; get; }

        /// <summary>
        /// 預計出庫主要數量
        /// </summary>
        /// 
        public decimal REQUESTED_QUANTITY { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        public string REQUESTED_QUANTITY_UOM { set; get; }

        /// <summary>
        /// 預計出庫次要數量
        /// </summary>
        /// 
        public decimal REQUESTED_QUANTITY2 { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        public string REQUESTED_QUANTITY_UOM2 { set; get; }

        /// <summary>
        /// 文件(工單號碼) 
        /// </summary>
        /// 
        public string BATCH_NO { set; get; }

        /// <summary>
        /// 代紙料號 
        /// </summary>
        /// 
        public string INVENTORY_ITEM_NUMBER { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        public string REMARK { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE1 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE2 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE3 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE4 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE5 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE6 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE7 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE8 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE9 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE10 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE11 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE12 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE13 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE14 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        public string ATTRIBUTE15 { set; get; }

        /// <summary>
        /// ERP請求識別碼
        /// </summary>
        /// 
        public long? REQUEST_ID { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        public long CREATED_BY { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        public DateTime CREATION_DATE { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        public long LAST_UPDATE_BY { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        public DateTime LAST_UPDATE_DATE { set; get; }
    }
}