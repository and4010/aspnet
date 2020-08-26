using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class XXIF_CHP_CONTROL_ST
    {
        public string PROCESS_CODE { set; get; }

        public string SERVER_CODE { set; get; }

        public string BATCH_ID { set; get; }

        public int ROW_NUM { set; get; }

        public DateTime PROCESS_DATE { set; get; }

        public long REQUEST_ID { set; get; }

        public string STATUS_CODE { set; get; }

        public string ERROR_MSG { set; get; }

        public string SOA_PULLING_FLAG { set; get; }

        public string SOA_ERROR_MSG { set; get; }

        public string SOA_PROCESS_CODE { set; get; }

        public long CREATED_BY { set; get; }

        public DateTime CREATION_DATE { set; get; }

        public long LAST_UPDATED_BY { set; get; }

        public DateTime LAST_UPDATE_DATE { set; get; }

        public long LAST_UPDATE_LOGIN { set; get; }
    }
}
