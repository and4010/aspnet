using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.DataModel.Views
{
    /// <summary>
    /// 餘切規格(N)
    /// </summary>
    public class XXCINV_OSP_RELATED_ITEM_V
    {
        /// <summary>
        /// 組成成份料號ID
        /// </summary>
        public long INVENTORY_ITEM_ID { set; get; }
        /// <summary>
        /// 組成成份料號
        /// </summary>
        public string ITEM_NUMBER { set; get; }
        /// <summary>
        /// 組成成份料號摘要
        /// </summary>
        public string ITEM_DESCRIPTION { set; get; }
        /// <summary>
        /// 餘切料號ID
        /// </summary>
        public long RELATED_ITEM_ID { set; get; }
        /// <summary>
        /// 餘切料號
        /// </summary>
        public string RELATED_ITEM_NUMBER { set; get; }
        /// <summary>
        /// 餘切料號摘要
        /// </summary>
        public string RELATED_ITEM_DESCRIPTION { set; get; }
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
