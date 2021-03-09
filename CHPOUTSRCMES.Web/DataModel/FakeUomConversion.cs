using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel
{
    /// <summary>
    /// 模擬單位轉換，現已沒用到
    /// </summary>
    public class FakeUomConversion : IUomConversion
    {
        public ResultDataModel<decimal> Convert(long P_ITEM_ID, decimal P_FROM_QTY, string P_FROM_UOM, string P_TO_UOM, int Round = 5)
        {
            ResultDataModel<decimal> model = new ResultDataModel<decimal>(true, "", 0m);
           
            if (P_FROM_UOM == "RE") model.Data =  P_FROM_QTY * 1.1m;
            else if (P_FROM_UOM == "KG") model.Data = P_FROM_QTY * 2m;
            else model.Data = P_FROM_QTY * 1m;

            return model;
        }
    }
}