using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class CTR_HEADER_T
    {

        /// <summary>
        /// 檔頭ID
        /// </summary>
        /// 
        public long CTR_HEADER_ID { set; get; }



        /// <summary>
        /// 櫃表維護 Header ID
        /// </summary>
        /// 
        public long HEADER_ID { set; get; }


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
        /// 提單號碼
        /// </summary>
        /// 
        public string BL_NO { set; get; }


        /// <summary>
        /// 櫃表維護 Line ID
        /// </summary>
        /// 
        public long LINE_ID { set; get; }


        /// <summary>
        /// 櫃號
        /// </summary>
        /// 
        public string CONTAINER_NO { set; get; }

        /// <summary>
        /// 拖櫃日期時間
        /// </summary>
        /// 
        public DateTime MV_CONTAINER_DATE { set; get; }


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
        /// 倉庫
        /// </summary>
        /// 
        public string SUBINVENTORY { set; get; }


        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        public long STATUS { set; get; }

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
        /// 更新人員名稱
        /// </summary>
        /// 
        public string LAST_UPDATE_USER_NAME { set; get; }

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

       
    }
}