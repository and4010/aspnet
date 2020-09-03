using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    public class ResultPageModel<T> : ResultModel
    {
        /// <summary>
        /// 結果資料
        /// </summary>
        public List<T> List { set; get; }

        public int Draw { set; get; }


        public int RecordTotal { set; get; }

        public int RecordFiltered { set; get; }

        public ResultPageModel()
        {

        }


        public ResultPageModel(bool success, string msg, List<T> list, int recordTotal)
            : base(success, msg)
        {
            Success = success;
            Msg = msg;
            Code = success ? CODE_SUCCESS : -1;
            List = list;
        }
    }
}