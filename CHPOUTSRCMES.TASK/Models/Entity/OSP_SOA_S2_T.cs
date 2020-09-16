using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class OSP_SOA_S2_T
    {
        /// <summary>
        /// 加工檔頭ID
        /// </summary>
        /// 
        public long OSP_HEADER_ID { set; get; }

        public string PROCESS_CODE { set; get; }

        public string SERVER_CODE { set; get; }

        public string BATCH_ID { set; get; }

        public string STATUS_CODE { set; get; }

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