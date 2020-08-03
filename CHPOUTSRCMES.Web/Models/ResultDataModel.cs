using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    public class ResultDataModel<T> : ResultModel
    {
        public const int CODE_SUCCESS = 0;

        //public bool Success { get; set; }

        //public int Code { set; get; }

        //public string Msg { get; set; }

        public T Data { set; get; }

        public ResultDataModel()
        {
        }

        public ResultDataModel(bool success, string msg, T data)
            : base(success, msg)
        {
            this.Data = data;
        }

        public ResultDataModel(int code, string msg, T data)
            : base(code, msg)
        {
            this.Data = data;
        }

        /// <summary>
        /// LOG用途
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("RESULTMODEL ( SUCCESS : {0}, CODE:{1}, MSG:{2}, DATA:{3} )", this.Success, this.Code, this.Msg, this.Data);
        }
    }
}