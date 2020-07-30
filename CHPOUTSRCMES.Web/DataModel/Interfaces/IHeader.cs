using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Interfaces
{
    /// <summary>
    /// 單號狀態介面
    /// </summary>
    public interface IHeader : IStatus
    {
        ///// <summary>
        ///// 轉成庫存狀態
        ///// </summary>
        ///// <param name="statusCode"></param>
        ///// <returns></returns>
        //string ToStockStatus(string statusCode);
    }
}