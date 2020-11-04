using CHPOUTSRCMES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Util
{
    public static class ConvertEx
    {
        /// <summary>
        /// string轉decimal
        /// </summary>
        /// <param name="s"></param>
        /// <returns>回傳true/false 和 結果(轉換失敗為0)</returns>
        public static ResultDataModel<decimal> StringToDecimal(string s)
        {
            decimal i = 0;
            bool result = decimal.TryParse(s, out i);
            if (result)
            {
                return new ResultDataModel<decimal>(result, "轉換成功", i);
            }
            else
            {
                return new ResultDataModel<decimal>(result, "轉換失敗", i);
            }
        }

        /// <summary>
        /// 去掉decimal尾數無用的0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
    }
}