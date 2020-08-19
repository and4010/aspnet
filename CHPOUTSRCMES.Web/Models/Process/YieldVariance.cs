using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class YieldVariance
    {
        public long OspYieldVarianceId { set; get; }

        public long OspHeaderId { set; get; }
        [DisplayName("重量(KG)")]
        public decimal LossWeight { set; get; }
        [DisplayName("得率")]
        public decimal Rate { set; get; }
    }
}