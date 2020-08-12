using System;

namespace CHPOUTSRCMES.TASK.Models.Entity.Shadowed
{
    public class TRANSACTION_TYPE_SHADOWED_T
    {

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
        /// 異動型態摘要
        /// </summary>
        /// 
        public string DESCRIPTION { set; get; }


        /// <summary>
        /// 異動作業id
        /// </summary>
        /// 
        public long TRANSACTION_ACTION_ID { set; get; }


        /// <summary>
        /// 異動作業
        /// </summary>
        /// 

        public string TRANSACTION_ACTION_NAME { set; get; }


        /// <summary>
        /// 來源型態ID
        /// </summary>
        /// 
        public long TRANSACTION_SOURCE_TYPE_ID { set; get; }


        /// <summary>
        /// 來源型態
        /// </summary>
        /// 
        public string TRANSACTION_SOURCE_TYPE_NAME { set; get; }

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