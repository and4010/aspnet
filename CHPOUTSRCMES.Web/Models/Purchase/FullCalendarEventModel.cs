using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Purchase
{
    public class FullCalendarEventModel
    {
        public long id { get; set; }

        public String title { get; set; }

        public String start { get; set; }

        public String end { get; set; }

        public bool allDay { get; set; }

        public String url { get; set; }

        public long Status { get; set; }

        public String color { get; set; }
    }
}