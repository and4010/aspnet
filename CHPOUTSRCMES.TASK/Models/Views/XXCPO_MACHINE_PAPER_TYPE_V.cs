using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Views
{
    /// <summary>
    /// 紙別機台(N)
    /// </summary>
    public class XXCPO_MACHINE_PAPER_TYPE_V
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
        /// 機台紙別代碼
        /// </summary>
        public string MACHINE_CODE { set; get; }
        /// <summary>
        /// 機台紙別意義
        /// </summary>
        public string MACHINE_MEANING { set; get; }
        /// <summary>
        /// 機台紙別摘要
        /// </summary>
        public string DESCRIPTION { set; get; }
        /// <summary>
        /// 紙別
        /// </summary>
        public string PAPER_TYPE { set; get; }
        /// <summary>
        /// 機台
        /// </summary>
        public string MACHINE_NUM { set; get; }
        /// <summary>
        /// 供應商編號
        /// </summary>
        public string SUPPLIER_NUM { set; get; }
        /// <summary>
        /// 供應商名稱
        /// </summary>
        public string VENDOR_NAME { set; get; }
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
