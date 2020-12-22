using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class DataTableJsonResultModel<T>
    {
        public int draw { set; get; }

        public int recordsFiltered { set; get; }

        public int recordsTotal { set; get; }

        public List<T> data { set; get; }


        public DataTableJsonResultModel(int draw, int recordsTotal, List<T> data)
        {
            this.draw = draw;
            this.recordsFiltered = recordsTotal;
            this.recordsTotal = recordsTotal;
            this.data = data;
        }

        public DataTableJsonResultModel(int draw, int recordsFiltered, int recordsTotal, List<T> data)
        {
            this.draw = draw;
            this.recordsFiltered = recordsFiltered;
            this.recordsTotal = recordsTotal;
            this.data = data;
        }
    }
}