using System;

namespace CHPOUTSRCMES.TASK.Models.Entity.Shadowed
{
    public class MACHINE_PAPER_TYPE_SHADOWED_T
    {
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
        /// 機台紙別代碼
        /// </summary>
        /// 
        public string MACHINE_CODE { set; get; }

        /// <summary>
        /// 機台紙別意義
        /// </summary>
        /// 
        public string MACHINE_MEANING { set; get; }

        /// <summary>
        /// 機台紙別摘要
        /// </summary>
        /// 
        public string DESCRIPTION { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        public string PAPER_TYPE { set; get; }

        /// <summary>
        /// 機台
        /// </summary>
        /// 
        public string MACHINE_NUM { set; get; }

        /// <summary>
        /// 供應商編號
        /// </summary>
        /// 
        public string SUPPLIER_NUM { set; get; }

        /// <summary>
        /// 供應商名稱
        /// </summary>
        /// 
        public string SUPPLIER_NAME { set; get; }


        /// <summary>
        /// 控制欄位  D:刪除
        /// </summary>
        /// 
        public string CONTROL_FLAG { set; get; }

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