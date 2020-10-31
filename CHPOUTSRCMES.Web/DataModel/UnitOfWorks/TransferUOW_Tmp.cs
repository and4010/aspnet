using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Entity.Transfer;
using CHPOUTSRCMES.Web.DataModel.Entiy.Transfer;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.WebForms;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public partial class TransferUOW
    {
        public ResultDataModel<List<LabelModel>> GetLabelForHeaderId(long id, string paperType, string userName)
        {
            var model = new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
            try
            {
                string cmd = "";

                switch (paperType)
                {
                    case "平版":
                        cmd = @"select
st.BARCODE as Barcode,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName,
CAST(st.PAPER_TYPE AS nvarchar) AS PaperType,
CAST(st.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(st.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(st.ROLL_REAM_WT, '0.##########') AS nvarchar) AS Qty,
 CAST(st.SECONDARY_UOM_CODE AS nvarchar) AS Unit,
 @userName as PrintBy
from STOCK_T st
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = st.INVENTORY_ITEM_ID
JOIN TRF_INBOUND_PICKED_HT P ON P.BARCODE = st.BARCODE
JOIN TRF_HEADER_T H ON H.TRANSFER_HEADER_ID = P.TRANSFER_HEADER_ID
join tmp_label2 t on t.BATCH_ID = H.TRANSFER_HEADER_ID AND t.ITEM_NUMBER = tt.ITEM_NUMBER
WHERE st.ITEM_CATEGORY = @paperType
AND H.TRANSFER_HEADER_ID = @id
ORDER BY t.id";
                        break;
                    default:
                    case "捲筒":
                        cmd = @"SELECT
st.BARCODE as Barcode,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName,
CAST(st.PAPER_TYPE AS nvarchar) AS PaperType,
CAST(st.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(st.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(st.PRIMARY_TRANSACTION_QTY, '0.##########') AS nvarchar) AS Qty,
 CAST(st.PRIMARY_UOM_CODE AS nvarchar) AS Unit,
 CAST(st.LOT_NUMBER AS nvarchar) as LotNumber,
@userName as PrintBy
from STOCK_T st
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = st.INVENTORY_ITEM_ID
JOIN TRF_INBOUND_PICKED_HT P ON P.BARCODE = st.BARCODE
JOIN TRF_HEADER_T H ON H.TRANSFER_HEADER_ID = P.TRANSFER_HEADER_ID
join tmp_label2 t on t.BATCH_ID = H.TRANSFER_HEADER_ID AND t.ITEM_NUMBER = tt.ITEM_NUMBER AND t.LOT_NUMBER = st.LOT_NUMBER
WHERE 1 = 1
AND st.ITEM_CATEGORY = @paperType
AND H.TRANSFER_HEADER_ID = @id
ORDER BY t.id";
                        break;
                }


                var labelModelList = this.Context.Database.SqlQuery<LabelModel>(cmd
                    , SqlParamHelper.GetNVarChar("@paperType", paperType)
                    , SqlParamHelper.GetNVarChar("@userName", userName, ParameterDirection.Input)
                    , SqlParamHelper.GetBigInt("@id", id, ParameterDirection.Input)
                    ).ToList();
                if (labelModelList != null && labelModelList.Count > 0)
                {
                    model = new ResultDataModel<List<LabelModel>>(true, "", labelModelList);
                }
            }
            catch(Exception ex)
            {
                model = new ResultDataModel<List<LabelModel>>(false, ex.Message, null);
            }

            return model;
        }


        public ActionResult PrintMassLabel(TransferUOW uow, long id, string paperType, string userName)
        {
            var resultData = uow.GetLabelForHeaderId(id, paperType, userName);
            return uow.PrintLabel(resultData.Data);
        }
    }

}