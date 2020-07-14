using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }

        public string Msg { get; set; }

        public ResultModel()
        {
        }

        public ResultModel (bool success, string msg)
        {
            this.Success = success;
            this.Msg = msg;
        }
    }
}