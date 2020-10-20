using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class CTR_SOA_T
    {
        public long CTR_HEADER_ID { set; get; }

        /// <summary>
        /// XXIFP217
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
        /// 狀態碼
        /// </summary>
        public string STATUS_CODE { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        public string CREATED_BY { set; get; }

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

    }
}