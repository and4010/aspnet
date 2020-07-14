using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Subinventory
{
    public class LOCATOR
    {
        public long ORGANIZATION_ID { set; get; }
        public string SUBINVENTORY_CODE { set; get; }
        public long LOCATOR_ID { set; get; }
        public long LOCATOR_TYPE { set; get; }
        public string LOCATOR_SEGMENTS { set; get; }
        public string LOCATOR_DESC { set; get; }
        public string SEGMENT1 { set; get; }
        public string SEGMENT2 { set; get; }
        public string SEGMENT3 { set; get; }
        public string SEGMENT4 { set; get; }
    }
}