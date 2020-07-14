using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Subinventory
{
    public class SUBINVENTORY
    {
        public long ORGANIZATION_ID { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public string SUBINVENTORY_NAME { set; get; }
        public string OSP_FLAG { set; get; }
    }
}