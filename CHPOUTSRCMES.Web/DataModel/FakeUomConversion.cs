using CHPOUTSRCMES.Web.DataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel
{
    public class FakeUomConversion : IUomConversion
    {
        public decimal Convert(long P_ITEM_ID, decimal P_FROM_QTY, string P_FROM_UOM, string P_TO_UOM, int Round = 5)
        {
            if (P_FROM_UOM == "RE") return P_FROM_QTY * 1.1m;
            if (P_FROM_UOM == "KG") return P_FROM_QTY * 2m;
            return P_FROM_QTY * 1m;
        }
    }
}