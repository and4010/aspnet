using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{

    public class RELATED_T
    {
        /// <summary>
        /// 餘切規格ID
        /// </summary>
        /// 
        public long RELATED_ID { set; get; }

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
        /// 料號說明
        /// </summary>
        /// 
        public string ITEM_DESCRIPTION { set; get; }

        /// <summary>
        /// 餘切料號ID
        /// </summary>
        /// 
        public long RELATED_ITEM_ID { set; get; }

        /// <summary>
        /// 餘切料號
        /// </summary>
        /// 
        public string RELATED_ITEM_NUMBER { set; get; }

        /// <summary>
        /// 餘切料號說明
        /// </summary>
        /// 
        public string RELATED_ITEM_DESCRIPTION { set; get; }


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
        public long LAST_UPDATED_BY { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        public DateTime LAST_UPDATED_DATE { set; get; }
    }
}