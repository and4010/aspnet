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
        /// 主檔啟用
        /// </summary>
        public bool MasterTaskEnabled { set; get; }

        /// <summary>
        /// 進櫃入庫同步間隔(分)
        /// </summary>
        public int CtrTaskInterval { set; get; }
        
        /// <summary>
        /// 進櫃入庫啟用
        /// </summary>
        public bool CtrTaskEnabled { set; get; }

        /// <summary>
        /// 出貨同步間隔(分)
        /// </summary>
        public int DlvTaskInterval { set; get; }
        
        /// <summary>
        /// 出貨啟用
        /// </summary>
        public bool DlvTaskEnabled { set; get; }

        /// <summary>
        /// 加工啟用
        /// </summary>
        public bool OspTaskEnabled { set; get; }

        /// <summary>
        /// 加工同步間隔(分)
        /// </summary>
        public int OspTaskInterval { set; get; }
        
        /// <summary>
        /// 庫位異動同步間隔(分)
        /// </summary>
        public int TrfTaskInterval { set; get; }
        
        /// <summary>
        /// 庫位異動啟用
        /// </summary>
        public bool TrfTaskEnabled { set; get; }


        public Configuration()
        {
            MasterTaskEnabled = false;
            MasterTaskInterval = 25;

            CtrTaskEnabled = false;
            CtrTaskInterval = 5;

            DlvTaskEnabled = false;
            DlvTaskInterval = 5;

            OspTaskEnabled = false;
            OspTaskInterval = 5;

            TrfTaskEnabled = false;
            TrfTaskInterval = 5;
        }
    }
}
