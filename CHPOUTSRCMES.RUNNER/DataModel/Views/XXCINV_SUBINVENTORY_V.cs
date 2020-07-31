using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.DataModel.Views
{
    public class XXCINV_SUBINVENTORY_V
    {
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_CODE { set; get; }
        public string ORGANIZATION_NAME { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public string SUBINVENTORY_NAME { set; get; }
        public string OSP_FLAG { set; get; }
        public long LOCATOR_TYPE { set; get; }
        public long LOCATOR_ID { set; get; }
        public string LOCATOR_SEGMENTS { set; get; }
        public string LOCATOR_DESC { set; get; }
        public string SEGMENT1 { set; get; }
        public string SEGMENT2 { set; get; }
        public string SEGMENT3 { set; get; }
        public string SEGMENT4 { set; get; }
        public long LOCATOR_STATUS { set; get; }
        public string LOCATOR_STATUS_CODE { set; get; }
        public long LOCATOR_PICKING_ORDER { set; get; }
        public DateTime LOCATOR_DISABLE_DATE { set; get; }
    }
}
