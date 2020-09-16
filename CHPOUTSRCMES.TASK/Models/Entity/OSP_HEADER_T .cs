using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class OSP_HEADER_T
    {
        /// <summary>
        /// 加工檔頭ID
        /// </summary>
        /// 
        public long OSP_HEADER_ID { set; get; }

        /// <summary>
        /// 文件ID(ops工單ID)
        /// </summary>
        /// 
        public long PE_BATCH_ID { set; get; }


        /// <summary>
        /// 文件(工單號碼) 
        /// </summary>
        /// 
        public string BATCH_NO { set; get; }

        /// <summary>
        /// 工單類別(OSP：加工、TMP：代紙)
        /// </summary>
        /// 
        public string BATCH_TYPE { set; get; }


        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        public long BATCH_STATUS { set; get; }

        /// <summary>
        /// 狀態說明
        /// </summary>
        /// 
        public string BATCH_STATUS_DESC { set; get; }

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
        /// 需求完工日期
        /// </summary>
        /// 
        public DateTime DUE_DATE { set; get; }

        /// <summary>
        /// 計劃開工日期
        /// </summary>
        /// 
        public DateTime PLAN_START_DATE { set; get; }

        /// <summary>
        /// 計劃完工日期
        /// </summary>
        /// 
        public DateTime PLAN_CMPLT_DATE { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        public string PE_CREATED_BY { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        public DateTime PE_CREATION_DATE { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        public string PE_LAST_UPDATE_BY { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        public DateTime? PE_LAST_UPDATE_DATE { set; get; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        public string STATUS { set; get; }

        /// <summary>
        /// 裁切日起(起)
        /// </summary>
        /// 
        public DateTime? CUTTING_DATE_FROM { set; get; }

        /// <summary>
        /// 裁切日起(訖)
        /// </summary>
        /// 
        public DateTime? CUTTING_DATE_TO { set; get; }

        /// <summary>
        /// 機台
        /// </summary>
        /// 
        public string MACHINE_CODE { set; get; }

        /// <summary>
        /// 生產備註
        /// </summary>
        /// 
        public string NOTE { set; get; }

        /// <summary>
        /// 修改次數
        /// </summary>
        /// 
        public int MODIFICATIONS { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        public string CREATED_BY { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        public DateTime? CREATION_DATE { set; get; }

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