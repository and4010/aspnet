using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class DLV_DETAIL_T
    {
        /// <summary>
        /// 出庫明細ID
        /// </summary>
        /// 
        public long DLV_DETAIL_ID { set; get; }

        /// <summary>
        /// 出庫檔頭ID
        /// </summary>
        /// 
        public long DLV_HEADER_ID { set; get; }

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
        /// 令重
        /// </summary>
        /// 
        public string REAM_WEIGHT { set; get; }

        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        public string ITEM_CATEGORY { set; get; }

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
        public decimal REQUESTED_TRANSACTION_QUANTITY { set; get; }

        /// <summary>
        /// 訂單交易單位
        /// </summary>
        /// 
        public string REQUESTED_TRANSACTION_UOM { set; get; }

        /// <summary>
        /// 預計出庫主要數量
        /// </summary>
        /// 
        public decimal REQUESTED_PRIMARY_QUANTITY { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        public string REQUESTED_PRIMARY_UOM { set; get; }

        /// <summary>
        /// 預計出庫次要數量
        /// </summary>
        /// 
        public decimal? REQUESTED_SECONDARY_QUANTITY { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        public string REQUESTED_SECONDARY_UOM { set; get; }

        /// <summary>
        /// 文件ID(OPS工單ID)
        /// </summary>
        /// 
        public long? OSP_BATCH_ID { set; get; }

        /// <summary>
        /// 文件(工單號碼) 
        /// </summary>
        /// 
        public string OSP_BATCH_NO { set; get; }

        /// <summary>
        /// 工單類別
        /// </summary>
        /// 
        public string OSP_BATCH_TYPE { set; get; }

        /// <summary>
        /// 代紙料浩ID
        /// </summary>
        /// 
        public long? TMP_ITEM_ID { set; get; }

        /// <summary>
        /// 代紙料號 
        /// </summary>
        /// 
        public string TMP_ITEM_NUMBER { set; get; }

        /// <summary>
        /// 代紙料號名稱
        /// </summary>
        /// 
        public string TMP_ITEM_DESCRIPTION { set; get; }


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