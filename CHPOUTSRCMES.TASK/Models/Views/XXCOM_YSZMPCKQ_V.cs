using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Views
{
    /// <summary>
    /// 令重包數(N)
    /// </summary>
    public class XXCOM_YSZMPCKQ_V
    {
        /// <summary>
        /// 庫存組織
        /// </summary>
        public long ORGANIZATION_ID { set; get; }
        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        public string ORGANIZATION_CODE { set; get; }
        /// <summary>
        /// 加工廠
        /// </summary>
        public string OSP_SUBINVENTORY { set; get; }
        /// <summary>
        /// 紙別
        /// </summary>
        public string PSTYP { set; get; }
        /// <summary>
        /// 基重上限
        /// </summary>
        public decimal? BWETUP { set; get; }
        /// <summary>
        /// 基重下限
        /// </summary>
        public decimal? BWETDN { set; get; }
        /// <summary>
        /// 令重上限
        /// </summary>
        public decimal? RWTUP { set; get; }
        /// <summary>
        /// 令重下限
        /// </summary>
        public decimal? RWTDN { set; get; }
        /// <summary>
        /// 包數
        /// </summary>
        public decimal? PCKQ { set; get; }
        /// <summary>
        /// 每包張數
        /// </summary>
        public decimal? PAPER_QTY { set; get; }
        /// <summary>
        /// 每件令數
        /// </summary>
        public decimal? PIECES_QTY { set; get; }
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
