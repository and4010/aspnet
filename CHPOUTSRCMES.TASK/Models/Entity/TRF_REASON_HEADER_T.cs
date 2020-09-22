using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{

    public class TRF_REASON_HEADER_T
    {
        /// <summary>
        /// 庫存移轉貨故檔頭ID
        /// </summary>
        /// 
        public long TRANSFER_REASON_HEADER_ID { set; get; }


        /// <summary>
        /// 作業單元ID(OU)
        /// </summary>
        /// 
        public long ORG_ID { set; get; }


        ///// <summary>
        ///// 作業單元(OU)
        ///// </summary>
        ///// 
        //public string ORG_NAME { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        public long ORGANIZATION_ID { set; get; }


        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        public string ORGANIZATION_CODE { set; get; }

        /// <summary>
        /// 出貨編號 Guid
        /// </summary>
        /// 
        public string SHIPMENT_NUMBER { set; get; }

        /// <summary>
        /// 出庫倉庫
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
        /// 儲位第三節段
        /// </summary>
        /// 
        public string SEGMENT3 { set; get; }

        /// <summary>
        /// 出貨編號狀態
        /// </summary>
        /// 
        public string NUMBER_STATUS { set; get; }

        /// <summary>
        /// 交易日期
        /// </summary>
        /// 
        public DateTime TRANSACTION_DATE { set; get; }

        /// <summary>
        /// 異動型態ID
        /// </summary>
        /// 
        public long TRANSACTION_TYPE_ID { set; get; }

        /// <summary>
        /// 異動型態
        /// </summary>
        /// 
        public string TRANSACTION_TYPE_NAME { set; get; }

        /// <summary>
        /// 目標作業單元ID(OU)
        /// </summary>
        /// 
        public long? TRANSFER_ORG_ID { set; get; }

        /// <summary>
        /// 目標作業單元(OU)
        /// </summary>
        /// 
        //public string TRANSFER_ORG_NAME { set; get; }

        /// <summary>
        /// 目標庫存組織ID
        /// </summary>
        /// 
        public long? TRANSFER_ORGANIZATION_ID { set; get; }

        /// <summary>
        /// 目標庫存組織
        /// </summary>
        /// 
        public string TRANSFER_ORGANIZATION_CODE { set; get; }

        /// <summary>
        /// 目標倉庫
        /// </summary>
        /// 
        public string TRANSFER_SUBINVENTORY_CODE { set; get; }

        /// <summary>
        /// 目標儲位ID
        /// </summary>
        /// 
        public long? TRANSFER_LOCATOR_ID { set; get; }

        /// <summary>
        /// 目標儲位
        /// </summary>
        /// 
        public string TRANSFER_LOCATOR_CODE { set; get; }

        
        /// <summary>
        /// 是否傳給ERP
        /// </summary>
        public string TO_ERP { set; get; }

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