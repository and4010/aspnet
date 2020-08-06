using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Views
{
    /// <summary>
    /// 組織倉庫儲位
    /// </summary>
    public class XXCINV_SUBINVENTORY_V
    {
        /// <summary>
        /// 庫存組織ID
        /// </summary>
        public long ORGANIZATION_ID { set; get; }
        /// <summary>
        /// 庫存組織
        /// </summary>
        public string ORGANIZATION_CODE { set; get; }
        /// <summary>
        /// 庫存組織名稱
        /// </summary>
        public string ORGANIZATION_NAME { set; get; }
        /// <summary>
        /// 倉庫
        /// </summary>
        public string SUBINVENTORY_CODE { set; get; }
        /// <summary>
        /// 倉庫描述
        /// </summary>
        public string SUBINVENTORY_NAME { set; get; }
        /// <summary>
        /// 加工廠註記(Y：加工廠)
        /// </summary>
        public string OSP_FLAG { set; get; }
        /// <summary>
        /// 儲位控制
        /// </summary>
        public long LOCATOR_TYPE { set; get; }
        /// <summary>
        /// 儲位ID
        /// </summary>
        public long LOCATOR_ID { set; get; }
        /// <summary>
        /// 儲位節段
        /// </summary>
        public string LOCATOR_SEGMENTS { set; get; }
        /// <summary>
        /// 儲位描述
        /// </summary>
        public string LOCATOR_DESC { set; get; }
        /// <summary>
        /// 儲位第一節段
        /// </summary>
        public string SEGMENT1 { set; get; }
        /// <summary>
        /// 儲位第二節段
        /// </summary>
        public string SEGMENT2 { set; get; }
        /// <summary>
        /// 儲位第三節段
        /// </summary>
        public string SEGMENT3 { set; get; }
        /// <summary>
        /// 儲位第四節段
        /// </summary>
        public string SEGMENT4 { set; get; }
        /// <summary>
        /// 儲位狀態ID
        /// </summary>
        public long LOCATOR_STATUS { set; get; }
        /// <summary>
        /// 儲位狀態
        /// </summary>
        public string LOCATOR_STATUS_CODE { set; get; }
        /// <summary>
        /// 儲位撿料順序
        /// </summary>
        public long LOCATOR_PICKING_ORDER { set; get; }
        /// <summary>
        /// 儲位終止日期
        /// </summary>
        public DateTime LOCATOR_DISABLE_DATE { set; get; }
    }
}
