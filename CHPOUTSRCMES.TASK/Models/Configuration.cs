using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models
{
    class Configuration
    {
        /// <summary>
        /// 主檔同步間隔(分)
        /// </summary>
        public int MasterTaskInterval { set; get; }
        /// <summary>
        /// 進櫃入庫同步間隔(分)
        /// </summary>
        public int CtrTaskInterval { set; get; }
        /// <summary>
        /// 出貨同步間隔(分)
        /// </summary>
        public int DlvTaskInterval { set; get; }
        /// <summary>
        /// 加工同步間隔(分)
        /// </summary>
        public int OspTaskInterval { set; get; }
        /// <summary>
        /// 庫位異動同步間隔(分)
        /// </summary>
        public int TrfTaskInterval { set; get; }


        public Configuration()
        {
            MasterTaskInterval = 25;
            CtrTaskInterval = 5;
            DlvTaskInterval = 5;
            OspTaskInterval = 5;
            TrfTaskInterval = 5;
        }
    }
}
