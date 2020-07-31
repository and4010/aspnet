using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.Web.DataModel.Interfaces
{
    interface ICategory
    {
        /// <summary>
        /// 取得中文說明
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        string GetDesc(string category);
    }
}
