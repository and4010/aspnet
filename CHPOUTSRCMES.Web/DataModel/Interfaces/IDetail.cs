using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Interfaces
{
    /// <summary>
    /// 明細狀態介面
    /// </summary>
    public interface IDetail : IStatus
    {
        /// <summary>
        /// 轉成庫存狀態
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        string ToStockStatus(string statusCode);
    }
}