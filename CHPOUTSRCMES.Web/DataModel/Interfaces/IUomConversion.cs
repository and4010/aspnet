using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.Web.DataModel.Interfaces
{
    //"ROUND(YFY_DIS_PKG_UTIL.UOM_CONVERSION(P_ITEM_ID       => 553916
    //                                 ,P_FROM_QTY      => 0.27144
    //                                 ,P_FROM_UOM      => 'RE'
    //                                 ,P_TO_UOM        => 'KG'
    //                                 ) ,5) QUANTITY"


    internal interface IUomConversion
    {
       decimal Convert(long P_ITEM_ID, decimal P_FROM_QTY, string P_FROM_UOM, string P_TO_UOM, int Round = 5);
    }
}
