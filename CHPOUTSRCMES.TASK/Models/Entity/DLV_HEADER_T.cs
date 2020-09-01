using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class DLV_HEADER_T
    {
        /// <summary>
        /// 出庫檔頭ID
        /// </summary>
        /// 
        public long DLV_HEADER_ID { set; get; }

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
        /// 出貨倉庫
        /// </summary>
        /// 
        public string SUBINVENTORY_CODE { set; get; }

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
        /// 捲筒\平板
        /// </summary>
        /// 
        public string ITEM_CATEGORY { set; get; }

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
        /// 交貨單狀態
        /// </summary>
        /// 
        public string DELIVERY_STATUS_CODE { set; get; }

        /// <summary>
        /// 交貨單狀態名稱
        /// </summary>
        /// 
        public string DELIVERY_STATUS_NAME { set; get; }

        /// <summary>
        /// 出貨確認人員
        /// </summary>
        /// 
        public string TRANSACTION_BY { set; get; }

        /// <summary>
        /// 出貨確認人員名稱
        /// </summary>
        /// 
        public string TRANSACTION_USER_NAME { set; get; }

        /// <summary>
        /// 出貨確認日期
        /// </summary>
        /// 
        public DateTime? TRANSACTION_DATE { set; get; }


        /// <summary>
        /// 出貨核准人員
        /// </summary>
        /// 
        public string AUTHORIZE_BY { set; get; }

        /// <summary>
        /// 出貨核准人員名稱
        /// </summary>
        /// 
        public string AUTHORIZE_USER_NAME { set; get; }

        /// <summary>
        /// 出貨核准日期
        /// </summary>
        /// 
        public DateTime? AUTHORIZE_DATE { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        public string NOTE { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        public string CREATED_BY { set; get; }

        /// <summary>
        /// 建立人員名稱
        /// </summary>
        /// 
        public string CREATED_USER_NAME { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        public DateTime CREATION_DATE { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        public string LAST_UPDATE_BY { set; get; }

        /// <summary>
        /// 更新人員名稱
        /// </summary>
        /// 
        public string LAST_UPDATE_USER_NAME { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        public DateTime? LAST_UPDATE_DATE { set; get; }
    }
}