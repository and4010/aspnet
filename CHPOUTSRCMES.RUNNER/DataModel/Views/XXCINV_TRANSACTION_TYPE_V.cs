using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.DataModel.Views
{
    /// <summary>
    /// 庫存交易類別(N)
    /// </summary>
    public class XXCINV_TRANSACTION_TYPE_V
    {
        /// <summary>
        /// 異動型態ID
        /// </summary>
        public long TRANSACTION_TYPE_ID { set; get; }
        /// <summary>
        /// 異動型態
        /// </summary>
        public string TRANSACTION_TYPE_NAME { set; get; }
        /// <summary>
        /// 異動型態摘要
        /// </summary>
        public string DESCRIPTION { set; get; }
        /// <summary>
        /// 異動作業ID
        /// </summary>
        public long TRANSACTION_ACTION_ID { set; get; }
        /// <summary>
        /// 異動作業
        /// </summary>
        public string TRANSACTION_ACTION_NAME { set; get; }
        /// <summary>
        /// 來源型態ID
        /// </summary>
        public long TRANSACTION_SOURCE_TYPE_ID { set; get; }
        /// <summary>
        /// 來源型態
        /// </summary>
        public string TRANSACTION_SOURCE_TYPE_NAME { set; get; }
        /// <summary>
        /// 建立人員ID
        /// </summary>
        public long CREATED_BY { set; get; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CREATION_DATE { set; get; }
        /// <summary>
        /// 最後更新人員ID
        /// </summary>
        public long LAST_UPDATED_BY { set; get; }
        /// <summary>
        /// 最後更新日期
        /// </summary>
        public DateTime LAST_UPDATE_DATE { set; get; }
    }
}
