using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Interfaces
{
    public interface IStatus
    {
        /// <summary>
        /// 取得中文說明
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        string GetDesc(string statusCode);
    }
}