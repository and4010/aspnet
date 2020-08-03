using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    /// <summary>
    /// 執行結果及資料
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultDataModel<T> : ResultModel
    {
        /// <summary>
        /// 結果資料
        /// </summary>
        public T Data { set; get; }
        /// <summary>
        /// 建構子
        /// </summary>
        public ResultDataModel()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="success">true:成功 false:失敗</param>
        /// <param name="msg">訊息</param>
        /// <param name="data">資料</param>
        public ResultDataModel(bool success, string msg, T data)
            : base(success, msg)
        {
            this.Data = data;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="code">錯誤碼 > 0:錯誤 0:成功</param>
        /// <param name="msg">訊息</param>
        /// <param name="data">資料</param>
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