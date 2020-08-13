using System;

namespace CHPOUTSRCMES.TASK.Models.Entity.Temp
{
    public class YSZMPCKQ_TMP_T
    {
        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        public long YSZMPCKQ_ID { set; get; }



        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        public long ORGANIZATION_ID { set; get; }


        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        /// 
        public string ORGANIZATION_CODE { set; get; }


        /// <summary>
        /// 加工廠
        /// </summary>
        /// 
        public string OSP_SUBINVENTORY { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        public string PSTYP { set; get; }


        /// <summary>
        /// 基重上限
        /// </summary>
        /// 
        public decimal BWETUP { set; get; }

        /// <summary>
        /// 基重下限
        /// </summary>
        /// 
        public decimal BWETDN { set; get; }

        /// <summary>
        /// 令重上限
        /// </summary>
        /// 
        public decimal RWTUP { set; get; }

        /// <summary>
        /// 令重下限
        /// </summary>
        /// 
        public decimal RWTDN { set; get; }

        /// <summary>
        /// 包數
        /// </summary>
        /// 
        public long PCKQ { set; get; }

        /// <summary>
        /// 每包張數
        /// </summary>
        /// 
        public long PAPER_QTY { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 
        public long PIECES_QTY { set; get; }

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