using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class CTR_PICKED_T
    {
        /// <summary>
        /// 入庫揀貨ID
        /// </summary>
        /// 
        public long CTR_PICKED_ID { set; get; }


        /// <summary>
        /// 表頭ID
        /// </summary>
        /// 
        public long CTR_HEADER_ID { set; get; }

        /// <summary>
        /// 檔頭ID
        /// </summary>
        /// 
        public long CTR_DETAIL_ID { set; get; }

        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        public long? STOCK_ID { set; get; }

        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        public long? LOCATOR_ID { set; get; }


        /// <summary>
        /// 儲位
        /// </summary>
        /// 
        public string LOCATOR_CODE { set; get; }

        /// <summary>
        /// 條碼
        /// </summary>
        /// 
        public string BARCODE { set; get; }

        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        public long INVENTORY_ITEM_ID { set; get; }


        /// <summary>
        /// 料號
        /// </summary>
        /// 
        public string SHIP_ITEM_NUMBER { set; get; }

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
        /// 令重
        /// </summary>
        /// 
        public string REAM_WEIGHT { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 
        public decimal ROLL_REAM_WT { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        public string SPECIFICATION { set; get; }

        /// <summary>
        /// 令包\無令打件
        /// </summary>
        /// 
        public string PACKING_TYPE { set; get; }

        /// <summary>
        /// 出貨數量(MT)
        /// </summary>
        /// 
        public decimal? SHIP_MT_QTY { set; get; }

        /// <summary>
        /// 交易數量
        /// </summary>
        /// 
        public decimal TRANSACTION_QUANTITY { set; get; }

        /// <summary>
        /// 交易單位
        /// </summary>
        /// 
        public string TRANSACTION_UOM { set; get; }

        /// <summary>
        /// 主要數量
        /// </summary>
        /// 
        public decimal PRIMARY_QUANTITY { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        public string PRIMARY_UOM { set; get; }

        /// <summary>
        /// 次要數量
        /// </summary>
        /// 
        public decimal? SECONDARY_QUANTITY { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        public string SECONDARY_UOM { set; get; }


        /// <summary>
        /// 捲號
        /// </summary>
        /// 
        public string LOT_NUMBER { set; get; }

        /// <summary>
        /// 理論重(批號數量)
        /// </summary>
        /// 
        public string THEORY_WEIGHT { set; get; }


        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        public string ITEM_CATEGORY { set; get; }


        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        public string STATUS { set; get; }

        /// <summary>
        /// 原因ID
        /// </summary>
        /// 
        public string REASON_CODE { set; get; }

        /// <summary>
        /// 原因說明
        /// </summary>
        /// 
        public string REASON_DESC { set; get; }

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
        /// 更新時間
        /// </summary>
        /// 
        public DateTime? LAST_UPDATE_DATE { set; get; }


        /// <summary>
        /// 更新人員名稱
        /// </summary>
        /// 
        public string LAST_UPDATE_USER_NAME { set; get; }
    }
}