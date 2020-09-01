using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Miscellaneous;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class MiscellaneousUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<TRF_MISCELLANEOUS_HEADER_T> trfMiscellaneousHeaderTRepositiory;
        private readonly IRepository<TRF_MISCELLANEOUS_T> trfMiscellaneousTRepositiory;
        private readonly IRepository<TRF_MISCELLANEOUS_HT> trfMiscellaneousHtRepositiory;

        public MiscellaneousUOW(DbContext context)
          : base(context)
        {
            this.trfMiscellaneousHeaderTRepositiory = new GenericRepository<TRF_MISCELLANEOUS_HEADER_T>(this);
            this.trfMiscellaneousTRepositiory = new GenericRepository<TRF_MISCELLANEOUS_T>(this);
            this.trfMiscellaneousHtRepositiory = new GenericRepository<TRF_MISCELLANEOUS_HT>(this);
        }

        MiscellaneousType miscellaneousType = new MiscellaneousType();

        /// <summary>
        /// 雜項異動類別
        /// </summary>
        public class MiscellaneousType
        {
            /// <summary>
            /// 雜收
            /// </summary>
            public const long Receive = TransactionTypeId.Chp37In;
            /// <summary>
            /// 雜發
            /// </summary>
            public const long Send = TransactionTypeId.Chp37Out;

            public string GetDesc(long type)
            {
                switch (type)
                {
                    case Receive:
                        return "雜收";
                    case Send:
                        return "雜發";
                    default:
                        return "";
                }
            }
        }

        #region 雜項異動類別下拉選單

        /// <summary>
        /// 取得雜項異動類別下拉選單
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetMiscellaneousTypeDropDownList()
        {
            var transferTypeList = createDropDownList(DropDownListType.Choice);
            transferTypeList.AddRange(getMiscellaneousTypeList());
            return transferTypeList;
        }

        private List<SelectListItem> getMiscellaneousTypeList()
        {
            var miscellaneousTypeList = new List<SelectListItem>();
            try
            {
                miscellaneousTypeList.Add(new SelectListItem() { Text = miscellaneousType.GetDesc(MiscellaneousType.Send), Value = MiscellaneousType.Send.ToString() });
                miscellaneousTypeList.Add(new SelectListItem() { Text = miscellaneousType.GetDesc(MiscellaneousType.Receive), Value = MiscellaneousType.Receive.ToString() });
                
                var a = miscellaneousTypeList.Select(x => new SelectListItem() { Text = x.Text, Value = x.Value }).Where(x => x.Value == "1");
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return miscellaneousTypeList;
        }

        #endregion


        public List<StockDT> GetStockTList(long organizationId, string subinventoryCode, long? locatorId, string itemNumber, decimal primaryQty, decimal percentageError)
        {
            percentageError = percentageError * 0.01m;
            decimal errorQty = primaryQty * percentageError;
            decimal maxQty = primaryQty + errorQty;
            decimal minQty = primaryQty - errorQty;

            if (locatorId == null)
            {
                string cmd = @"
SELECT [STOCK_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY [STOCK_ID]) AS SUB_ID
      ,s.[ORGANIZATION_ID] AS ORGANIZATION_ID
	  ,'' AS SEGMENT3
      ,s.[SUBINVENTORY_CODE] AS SUBINVENTORY_CODE
      ,[ITEM_NUMBER] AS ITEM_NO
      ,[BARCODE] AS BARCODE
      ,[PRIMARY_UOM_CODE] AS PRIMARY_UOM_CODE
      ,[PRIMARY_AVAILABLE_QTY] AS PRIMARY_AVAILABLE_QTY
      ,[SECONDARY_UOM_CODE] AS SECONDARY_UOM_CODE
      ,[SECONDARY_AVAILABLE_QTY] AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE
  FROM [STOCK_T] s
  WHERE s.ORGANIZATION_ID = @organizationId 
  AND s.SUBINVENTORY_CODE = @subinventoryCode
  AND s.PRIMARY_AVAILABLE_QTY >= @minQty
  AND s.PRIMARY_AVAILABLE_QTY <= @maxQty
  AND s.ITEM_NUMBER = @itemNumber
";

                var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                var pSub = SqlParamHelper.R.SubinventoryCode("@subinventoryCode", subinventoryCode);
                var pMinQty = SqlParamHelper.GetDecimal("@minQty", minQty);
                var pMaxQty = SqlParamHelper.GetDecimal("@maxQty", maxQty);
                var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                return this.Context.Database.SqlQuery<StockDT>(cmd, pOrg, pSub, pMinQty, pMaxQty, pItemNo).ToList();
            }
            else
            {
                string cmd = @"
SELECT [STOCK_ID] as ID
	  ,ROW_NUMBER() OVER(ORDER BY [STOCK_ID]) AS SUB_ID
      ,s.[ORGANIZATION_ID] AS ORGANIZATION_ID
	  ,l.SEGMENT3 AS SEGMENT3
      ,s.[SUBINVENTORY_CODE]  AS SUBINVENTORY_CODE
      ,[ITEM_NUMBER] AS ITEM_NO
      ,[BARCODE] AS BARCODE
      ,[PRIMARY_UOM_CODE] AS PRIMARY_UOM_CODE
      ,[PRIMARY_AVAILABLE_QTY] AS PRIMARY_AVAILABLE_QTY
      ,[SECONDARY_UOM_CODE] AS SECONDARY_UOM_CODE
      ,[SECONDARY_AVAILABLE_QTY] AS SECONDARY_AVAILABLE_QTY
      ,[NOTE] AS NOTE
      ,[STATUS_CODE] AS STATUS_CODE
  FROM [STOCK_T] s
  LEFT JOIN [LOCATOR_T] l on s.ORGANIZATION_ID = l.ORGANIZATION_ID 
  AND s.SUBINVENTORY_CODE = l.SUBINVENTORY_CODE 
  AND s.LOCATOR_ID = l.LOCATOR_ID
  WHERE s.ORGANIZATION_ID = @organizationId 
  AND s.SUBINVENTORY_CODE = @subinventoryCode 
  AND s.LOCATOR_ID = @locatorId 
  AND s.PRIMARY_AVAILABLE_QTY >= @minQty
  AND s.PRIMARY_AVAILABLE_QTY <= @maxQty
  AND s.ITEM_NUMBER = @itemNumber
";

                var pOrg = SqlParamHelper.GetBigInt("@organizationId", organizationId);
                var pSub = SqlParamHelper.R.SubinventoryCode("@subinventory", subinventoryCode);
                var pLoc = SqlParamHelper.GetBigInt("@locatorId", (long)locatorId);
                var pMinQty = SqlParamHelper.GetDecimal("@minQty", minQty);
                var pMaxQty = SqlParamHelper.GetDecimal("@maxQty", maxQty);
                var pItemNo = SqlParamHelper.R.ItemNo("@itemNumber", itemNumber);

                return this.Context.Database.SqlQuery<StockDT>(cmd, pOrg, pSub, pLoc, pMinQty, pMaxQty, pItemNo).ToList();
            }

        }
    }
}