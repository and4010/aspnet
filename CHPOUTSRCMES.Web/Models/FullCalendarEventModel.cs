using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    public class FullCalendarEventModel
    {
        public Int64 id { get; set; }

        public String title { get; set; }

        public String start { get; set; }

        public String end { get; set; }

        public bool allDay { get; set; }

        public String url { get; set; }

        public String Status { get; set; }

        public String color { get; set; }
    }
}