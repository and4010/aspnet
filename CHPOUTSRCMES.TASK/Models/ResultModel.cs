using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.TASK.Models
{
    public class ResultModel
    {
        public const int CODE_SUCCESS = 0;

        public bool Success { get; set; }

        public int Code { set; get; }

        public string Msg { get; set; }


        public ResultModel()
        {
        }

        public ResultModel (bool success, string msg)
        {
            this.Success = success;
            this.Code = success ? CODE_SUCCESS : -1;
            this.Msg = msg;
        }

        public ResultModel(int code, string msg)
        {
            this.Success = code == CODE_SUCCESS;
            this.Code = code;
            this.Msg = msg;
        }

        /// <summary>
        /// LOG用途
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("RESULTMODEL (SUCCESS : {0}, CODE:{1}, MSG:{2})", this.Success, this.Code, this.Msg);
        }
    }
}