using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Process;
using CHPOUTSRCMES.Web.DataModel.Entity.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Process;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Ajax.Utilities;
using Microsoft.Graph;
using Microsoft.Reporting.WebForms;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class ProcessUOW : MasterUOW
    {

        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<OSP_COTANGENT_T> OspCotangenTRepository;
        private readonly IRepository<OSP_COTANGENT_HT> OspCotangentHTRepository;
        private readonly IRepository<OSP_DETAIL_IN_T> OspDetailInTRepository;
        private readonly IRepository<OSP_DETAIL_IN_HT> OspDetailInHTRepository;
        private readonly IRepository<OSP_DETAIL_OUT_T> OspDetailOutTRepository;
        private readonly IRepository<OSP_DETAIL_OUT_HT> OspDetailOutHTRepository;
        private readonly IRepository<OSP_HEADER_T> OspHeaderTRepository;
        private readonly IRepository<OSP_ORG_T> OspOrgTRepository;
        private readonly IRepository<OSP_PICKED_IN_T> OspPickedInTRepository;
        private readonly IRepository<OSP_PICKED_IN_HT> OspPickedInHTRepository;
        private readonly IRepository<OSP_PICKED_OUT_T> OspPickedOutTRepository;
        private readonly IRepository<OSP_PICKED_OUT_HT> OspPickedOutHTRepository;
        private readonly IRepository<OSP_YIELD_VARIANCE_T> OspYieldVarianceTRepository;
        private readonly IRepository<OSP_YIELD_VARIANCE_HT> OspYieldVarianceHTRepository;
        private readonly IRepository<OSP_HEADER_MOD_T> OspHeaderModTRepository;
        public ProcessUOW(DbContext context) : base(context)
        {
            this.OspCotangenTRepository = new GenericRepository<OSP_COTANGENT_T>(this);
            this.OspCotangentHTRepository = new GenericRepository<OSP_COTANGENT_HT>(this);
            this.OspDetailInTRepository = new GenericRepository<OSP_DETAIL_IN_T>(this);
            this.OspDetailInHTRepository = new GenericRepository<OSP_DETAIL_IN_HT>(this);
            this.OspDetailOutTRepository = new GenericRepository<OSP_DETAIL_OUT_T>(this);
            this.OspDetailOutHTRepository = new GenericRepository<OSP_DETAIL_OUT_HT>(this);
            this.OspHeaderTRepository = new GenericRepository<OSP_HEADER_T>(this);
            this.OspOrgTRepository = new GenericRepository<OSP_ORG_T>(this);
            this.OspPickedInTRepository = new GenericRepository<OSP_PICKED_IN_T>(this);
            this.OspPickedInHTRepository = new GenericRepository<OSP_PICKED_IN_HT>(this);
            this.OspPickedOutTRepository = new GenericRepository<OSP_PICKED_OUT_T>(this);
            this.OspPickedOutHTRepository = new GenericRepository<OSP_PICKED_OUT_HT>(this);
            this.OspYieldVarianceTRepository = new GenericRepository<OSP_YIELD_VARIANCE_T>(this);
            this.OspYieldVarianceHTRepository = new GenericRepository<OSP_YIELD_VARIANCE_HT>(this);
            this.OspHeaderModTRepository = new GenericRepository<OSP_HEADER_MOD_T>(this);

        }




        /// <summary>
        /// 取得單一筆加工排程資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CHP_PROCESS_T GetViewModel(long id)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
DI.OSP_HEADER_ID AS OspHeaderId,
DI.OSP_DETAIL_IN_ID AS OspDetailInId,
DO.OSP_DETAIL_OUT_ID AS OspDetailOutId,
H.STATUS AS Status,
H.BATCH_NO AS BatchNo,
H.PLAN_START_DATE AS PlanStartDate,
H.BATCH_TYPE AS BatchType,
H.DUE_DATE AS DueDate,
H.CUTTING_DATE_FROM AS CuttingDateFrom,
H.CUTTING_DATE_TO AS CuttingDateTo,
H.MACHINE_CODE AS MachineNum,
DO.CUSTOMER_NAME AS CustomerName,
ISNULL(DO.ORDER_NUMBER, ISNULL(DI.ORDER_NUMBER, 0)) AS OrderNumber,
ISNULL(DO.ORDER_LINE_NUMBER, ISNULL(DI.ORDER_LINE_NUMBER, '')) AS OrderLineNumber,
DI.BASIC_WEIGHT AS BasicWeight,
DI.SPECIFICATION AS Specification,
DI.GRAIN_DIRECTION AS GrainDirection,
DI.ORDER_WEIGHT AS OrderWeight,
DI.REAM_WT AS ReamWt,
DO.PRIMARY_QUANTITY AS PrimaryQuantity,
DO.PRIMARY_UOM AS PrimaryUom,
DO.TRANSACTION_UOM AS TransactionUom,
DI.PACKING_TYPE AS PackingType,
DI.PAPER_TYPE AS PaperType,
DI.OSP_REMARK AS OspRemark,
DI.INVENTORY_ITEM_NUMBER AS SelectedInventoryItemNumber,
DO.INVENTORY_ITEM_NUMBER AS Product_Item,
DO.PAPER_TYPE AS DoPaperType,
DO.SPECIFICATION AS DoSpecification,
DO.BASIC_WEIGHT AS DoBasicWeight,
DO.GRAIN_DIRECTION AS DoGrainDirection,
DO.PRIMARY_QUANTITY AS DoPrimaryQuantity,
DO.REAM_WT as DoReamWt,
DO.PACKING_TYPE AS DoPackingType,
H.NOTE AS Note,
DI.SUBINVENTORY AS Subinventory,
DI.LOCATOR_CODE AS LocatorCode,
OYV.LOSS_WEIGHT AS Loss,
DI.CREATED_BY AS Createdby,
DI.CREATION_DATE AS Creationdate,
DI.LAST_UPDATE_BY AS LastUpdatedBy,
DI.LAST_UPDATE_DATE AS LastUpdateDate
FROM [OSP_HEADER_T] H
JOIN OSP_DETAIL_IN_T DI ON DI.OSP_HEADER_ID = H.OSP_HEADER_ID
JOIN OSP_DETAIL_OUT_T DO ON DO.OSP_HEADER_ID = H.OSP_HEADER_ID
left JOIN OSP_YIELD_VARIANCE_T OYV ON OYV.OSP_HEADER_ID = H.OSP_HEADER_ID
WHERE DI.OSP_HEADER_ID = @OSP_HEADER_ID");
                    string commandText = string.Format(query.ToString());
                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_DETAIL_IN_T", "OSP_DETAIL_IN_HT").Replace("OSP_DETAIL_OUT_T", "OSP_DETAIL_OUT_HT").Replace("OSP_YIELD_VARIANCE_T", "OSP_YIELD_VARIANCE_HT"));
                    return mesContext.Database.SqlQuery<CHP_PROCESS_T>(commandText, new SqlParameter("@OSP_HEADER_ID", id)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new CHP_PROCESS_T();
            }
        }

        /// <summary>
        /// 寫入編輯備註
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ResultModel SetEditNote(long OspHeaderId, string note, string userId)
        {
            try
            {
                var header = OspHeaderTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (header != null)
                {
                    header.Note = note;
                    header.LastUpdateBy = userId;
                    header.LastUpdateDate = DateTime.Now;
                    OspHeaderTRepository.Update(header, true);
                    return new ResultModel(true, "成功");
                }
                else
                {
                    return new ResultModel(false, "找不到ID");
                }

            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }
        }

        /// <summary>
        /// 取得加工排程資料
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="BatchNo"></param>
        /// <param name="MachineNum"></param>
        /// <param name="DueDate"></param>
        /// <param name="CuttingDateFrom"></param>
        /// <param name="CuttingDateTo"></param>
        /// <param name="Subinventory"></param>
        /// <returns></returns>
        public List<CHP_PROCESS_T> GetTable(string Status, string BatchNo, string MachineNum, string DueDateFrom, string DueDateTo,
            string CuttingDateFrom, string CuttingDateTo, string Subinventory, string PlanStartDateFrom, string PlanStartDateTo, string UserId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
DI.OSP_HEADER_ID AS OspHeaderId,
DI.OSP_DETAIL_IN_ID AS OspDetailInId,
DO.OSP_DETAIL_OUT_ID AS OspDetailOutId,
H.STATUS AS Status,
H.BATCH_NO AS BatchNo,
H.PLAN_START_DATE AS PlanStartDate,
H.BATCH_TYPE AS BatchType,
H.DUE_DATE AS DueDate,
H.CUTTING_DATE_FROM AS CuttingDateFrom,
H.CUTTING_DATE_TO AS CuttingDateTo,
H.MACHINE_CODE AS MachineNum,
DO.CUSTOMER_NAME AS CustomerName,
ISNULL(DO.ORDER_NUMBER, 0) AS OrderNumber,
ISNULL(DO.ORDER_LINE_NUMBER, 0) AS OrderLineNumber,
DI.BASIC_WEIGHT AS BasicWeight,
DI.SPECIFICATION AS Specification,
DI.GRAIN_DIRECTION AS GrainDirection,
DO.ORDER_WEIGHT AS OrderWeight,
DI.REAM_WT AS ReamWt,
DO.PRIMARY_QUANTITY AS PrimaryQuantity,
DO.PRIMARY_UOM AS PrimaryUom,
DO.TRANSACTION_UOM AS TransactionUom,
DI.PACKING_TYPE AS PackingType,
DI.PAPER_TYPE AS PaperType,
DI.OSP_REMARK AS OspRemark,
DO.PAPER_TYPE AS DoPaperType,
DO.SPECIFICATION AS DoSpecification,
DO.BASIC_WEIGHT AS DoBasicWeight,
DO.GRAIN_DIRECTION AS DoGrainDirection,
DO.PRIMARY_QUANTITY AS DoPrimaryQuantity,
DO.REAM_WT as DoReamWt,
DO.PACKING_TYPE AS DoPackingType,
DO.INVENTORY_ITEM_NUMBER AS Product_Item,
DI.INVENTORY_ITEM_NUMBER AS SelectedInventoryItemNumber,
H.NOTE AS Note,
ISNULL(H.SRC_OSP_HEADER_ID, 0) AS SrcOspHeaderId,
DI.SUBINVENTORY AS Subinventory,
DI.LOCATOR_CODE AS LocatorCode,
OYV.LOSS_WEIGHT AS Loss,
DI.CREATED_BY AS Createdby,
DI.CREATION_DATE AS Creationdate,
DI.LAST_UPDATE_BY AS LastUpdatedBy,
DI.LAST_UPDATE_DATE AS LastUpdateDate,
ISNULL(HM.ORG_OSP_HEADER_ID, 0) AS OrgOspHeaderId
FROM [OSP_HEADER_T] H
JOIN OSP_DETAIL_IN_T DI ON DI.OSP_HEADER_ID = H.OSP_HEADER_ID
JOIN OSP_DETAIL_OUT_T DO ON DO.OSP_HEADER_ID = H.OSP_HEADER_ID
left JOIN OSP_YIELD_VARIANCE_T OYV ON OYV.OSP_HEADER_ID = H.OSP_HEADER_ID
left JOIN OSP_HEADER_MOD_T HM ON HM.OSP_HEADER_ID = H.OSP_HEADER_ID
");
                    if (Status != "*")
                    {
                        cond.Add("H.STATUS = @STATUS");
                        sqlParameterList.Add(new SqlParameter("@STATUS", Status));
                    }

                    if (!string.IsNullOrEmpty(BatchNo))
                    {
                        cond.Add("H.BATCH_NO LIKE @BATCH_NO");
                        sqlParameterList.Add(new SqlParameter("@BATCH_NO", BatchNo + '%'));
                    }

                    if (MachineNum != "*")
                    {
                        cond.Add("MACHINE_CODE = @MACHINE_CODE");
                        sqlParameterList.Add(new SqlParameter("@MACHINE_CODE", MachineNum));
                    }
                    DateTime dueDateFrom = new DateTime();
                    if (DueDateFrom != "" && DateTime.TryParseExact(DueDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dueDateFrom))
                    {
                        cond.Add("H.DUE_DATE >= @DUE_DATE");
                        sqlParameterList.Add(SqlParamHelper.GetDataTime("@DUE_DATE", dueDateFrom, ParameterDirection.Input));
                    }

                    DateTime dueDateTo = new DateTime();
                    if (DueDateTo != "" && DateTime.TryParseExact(DueDateTo, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dueDateTo))
                    {
                        cond.Add("H.DUE_DATE <= @DUE_END_DATE");
                        sqlParameterList.Add(SqlParamHelper.GetDataTime("@DUE_END_DATE", dueDateTo.AddDays(1).AddMilliseconds(-1), ParameterDirection.Input));
                    }

                    if (CuttingDateFrom != "" && CuttingDateTo != "")
                    {
                        DateTime dueDate1 = new DateTime();
                        DateTime.TryParseExact(CuttingDateTo, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dueDate1);
                        cond.Add("((H.CUTTING_DATE_FROM >= @CUTTING_DATE_FROM AND H.CUTTING_DATE_TO <= @CUTTING_DATE_TO) or " +
"(H.CUTTING_DATE_FROM >= @CUTTING_DATE_FROM AND H.CUTTING_DATE_TO <= @CUTTING_DATE_TO) or" +
"(H.CUTTING_DATE_FROM >= @CUTTING_DATE_FROM AND H.CUTTING_DATE_TO <= @CUTTING_DATE_TO))");
                        sqlParameterList.Add(new SqlParameter("@CUTTING_DATE_FROM", CuttingDateFrom));
                        sqlParameterList.Add(SqlParamHelper.GetDataTime("@CUTTING_DATE_TO", dueDate1.AddDays(1).AddMilliseconds(-1), ParameterDirection.Input));
                    }

                    if (CuttingDateFrom != "" && CuttingDateTo == "")
                    {
                        cond.Add("H.CUTTING_DATE_FROM >= @CUTTING_DATE_FROM");
                        sqlParameterList.Add(new SqlParameter("@CUTTING_DATE_FROM", CuttingDateFrom));
                    }

                    if (CuttingDateTo != "" && CuttingDateFrom == "")
                    {
                        DateTime dueDate1 = new DateTime();
                        DateTime.TryParseExact(CuttingDateTo, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dueDate1);
                        cond.Add("H.CUTTING_DATE_TO <= @CUTTING_DATE_TO");
                        sqlParameterList.Add(SqlParamHelper.GetDataTime("@CUTTING_DATE_TO", dueDate1.AddDays(1).AddMilliseconds(-1), ParameterDirection.Input));
                    }
                    if (Subinventory != "*")
                    {
                        cond.Add("DI.SUBINVENTORY = @SUBINVENTORY");
                        sqlParameterList.Add(new SqlParameter("@SUBINVENTORY", Subinventory));
                    }
                    else
                    {
                        var subinventoryCodeList = GetSubinventoryListForUser(UserId);

                        string temp = "";
                        foreach (string subinventoryCode in subinventoryCodeList)
                        {
                            temp += "'" + subinventoryCode + "'" + ',';
                        }
                        temp = temp.TrimEnd(',');
                        temp = string.Format("DI.SUBINVENTORY IN({0})", temp);
                        cond.Add(temp);
                    }

                    DateTime planStartDateFrom = new DateTime();
                    if (PlanStartDateFrom != "" && DateTime.TryParseExact(PlanStartDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out planStartDateFrom))
                    {
                        cond.Add("H.PLAN_START_DATE >= @PLAN_START_DATE_FROM");
                        sqlParameterList.Add(SqlParamHelper.GetDataTime("@PLAN_START_DATE_FROM", planStartDateFrom, ParameterDirection.Input));
                    }

                    DateTime planStartDateTo = new DateTime();
                    if (PlanStartDateTo != "" && DateTime.TryParseExact(PlanStartDateTo, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out planStartDateTo))
                    {
                        cond.Add("H.PLAN_START_DATE <= @PLAN_START_DATE_TO");
                        sqlParameterList.Add(SqlParamHelper.GetDataTime("@PLAN_START_DATE_TO", planStartDateTo.AddDays(1).AddMilliseconds(-1), ParameterDirection.Input));
                    }

                    cond.Add("H.STATUS <> '5'");

                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));

                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_DETAIL_IN_T", "OSP_DETAIL_IN_HT").Replace("OSP_DETAIL_OUT_T", "OSP_DETAIL_OUT_HT"));

                    if (sqlParameterList.Count > 0)
                    {
                        return mesContext.Database.SqlQuery<CHP_PROCESS_T>(commandText, sqlParameterList.ToArray()).ToList();
                    }
                    else
                    {
                        return mesContext.Database.SqlQuery<CHP_PROCESS_T>(commandText).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<CHP_PROCESS_T>();
            }
        }

        /// <summary>
        /// 取得投入條碼資料
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public List<Invest> GetPicketIn(long OspHeaderId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT
[OSP_PICKED_IN_ID] as OspPickedInId,
[OSP_DETAIL_IN_ID] as OspDetailInId,
[OSP_HEADER_ID] as OspHeaderId,
[STOCK_ID] as StockId,
[BARCODE] as Barcode,
[INVENTORY_ITEM_ID] as InventoryItemId,
[INVENTORY_ITEM_NUMBER] as InventoryItemNumber,
[PAPER_TYPE] as PaperType,
[BASIC_WEIGHT] as BasicWeight,
[SPECIFICATION] as Specification,
[LOT_NUMBER] as LotNumber,
[LOT_QUANTITY] as LotQuantity,
[PRIMARY_QUANTITY] as PrimaryQuantity,
--[PRIMARY_UOM] as PRIMARY_UOM,
[SECONDARY_QUANTITY] as SecondaryQuantity,
--[SECONDARY_UOM] as SECONDARY_UOM,
[HAS_REMAINT] as HasRemaint,
[REMAINING_QUANTITY] as RemainingQuantity
--[REMAINING_UOM] as REMAINING_UOM,
--[CREATED_BY] as CREATED_BY,
--[CREATED_USER_NAME] as CREATED_USER_NAME,
--[CREATION_DATE] as CREATION_DATE,
--[LAST_UPDATE_BY] as LAST_UPDATE_BY,
--[LAST_UPDATE_DATE] as LAST_UPDATE_DATE,
--[LAST_UPDATE_USER_NAME] as LAST_UPDATE_USER_NAME
FROM [OSP_PICKED_IN_T]
where OSP_HEADER_ID = @OSP_HEADER_ID");
                    string commandText = string.Format(query.ToString());
                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_PICKED_IN_T", "OSP_PICKED_IN_HT"));
                    return mesContext.Database.SqlQuery<Invest>(commandText, new SqlParameter("@OSP_HEADER_ID", OspHeaderId)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<Invest>();
            }
        }

        /// <summary>
        /// 投入Editor
        /// </summary>
        /// <param name="InvestDTList"></param>
        /// <returns></returns>
        public ResultModel SetEditor(DetailDTEditor InvestDTList, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var InvestDTListId = InvestDTList.InvestList[0];
                    if (InvestDTList.Action == "edit")
                    {
                        var id = OspPickedInTRepository.Get(x => x.OspPickedInId == InvestDTListId.OspPickedInId).SingleOrDefault();
                        var stock = stockTRepository.Get(x => x.StockId == id.StockId).SingleOrDefault();
                        var header = OspHeaderTRepository.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                        if (id != null)
                        {
                            if (InvestDTListId.HasRemaint == "有" && (InvestDTListId.RemainingQuantity ?? 0) <= 0)
                            {
                                return new ResultModel(false, "請輸入餘重");
                            }
                            id.HasRemaint = InvestDTListId.HasRemaint;
                            id.RemainingQuantity = InvestDTListId.RemainingQuantity;
                            id.LastUpdateBy = UserId;
                            id.LastUpdateUserName = UserName;
                            id.LastUpdateDate = DateTime.Now;
                            OspPickedInTRepository.Update(id, true);
                            //var aft = InvestDTListId.RemainingQuantity ?? 0;
                            var bef = stock.PrimaryAvailableQty;
                            var chg = stock.PrimaryAvailableQty - (InvestDTListId.RemainingQuantity ?? 0);
                            var lockQty = (stock.PrimaryLockedQty ?? 0) + chg;
                            var aft = stock.PrimaryAvailableQty - chg;
                            if (id.PrimaryQuantity > aft)
                            {
                                CheckStock(stock.StockId, UserId, aft, lockQty, StockStatusCode.ProcessPicked);
                                var m1 = StockRecord(id.StockId, bef, aft, -1*chg, 0, 0, 0, CategoryCode.Process, ActionCode.Picked, header.BatchNo, UserId);
                                if (!m1.Success)
                                {
                                    throw new Exception($"CODE:{m1.Code} MSG:{m1.Msg}");
                                }
                                txn.Commit();
                                return new ResultModel(true, "");
                            }
                            else
                            {
                                txn.Rollback();
                                return new ResultModel(false, "餘重須小於原重量");
                            }


                        }
                    }

                    if (InvestDTList.Action == "remove")
                    {
                        var id = OspPickedInTRepository.Get(x => x.OspPickedInId == InvestDTListId.OspPickedInId).SingleOrDefault();
                        var stock = stockTRepository.Get(x => x.StockId == id.StockId).SingleOrDefault();
                        var header = OspHeaderTRepository.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                        if (id != null)
                        {
                            var bef = stock.PrimaryAvailableQty;
                            var chg = id.PrimaryQuantity - (id.RemainingQuantity ?? 0);
                            var lockQty = (stock.PrimaryLockedQty ?? 0) - chg;
                            var aft = stock.PrimaryAvailableQty + chg;
                            CheckStock(stock.StockId, UserId, aft, lockQty, StockStatusCode.InStock);
                            OspPickedInTRepository.Delete(id, true);
                            var m1 = StockRecord(id.StockId, bef, aft, chg, 0, 0, 0, CategoryCode.Process, ActionCode.Deleted, header.BatchNo, UserId);
                            if (!m1.Success)
                            {
                                throw new Exception($"CODE:{m1.Code} MSG:{m1.Msg}");
                            }
                            txn.Commit();
                            return new ResultModel(true, "");
                        }

                    }
                    return new ResultModel(false, "找不到ID");
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }
            }
        }

        /// <summary>
        /// 加工投入勾選刪除
        /// </summary>
        /// <returns></returns>
        public ResultModel ChooseDelete(long[] OspPickedInId, string UserId)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < OspPickedInId.Length; i++)
                    {
                        var OspPickedInId1 = OspPickedInId[i];
                        var id = OspPickedInTRepository.Get(x => x.OspPickedInId == OspPickedInId1).SingleOrDefault();
                        var stock = stockTRepository.Get(x => x.StockId == id.StockId).SingleOrDefault();
                        var header = OspHeaderTRepository.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                        if (id != null)
                        {
                            var bef = stock.PrimaryAvailableQty;
                            var chg = id.PrimaryQuantity - (id.RemainingQuantity ?? 0);
                            var lockQty = (stock.PrimaryLockedQty ?? 0) - chg;
                            var aft = stock.PrimaryAvailableQty + chg;
                            CheckStock(stock.StockId, UserId, aft, lockQty, StockStatusCode.InStock);
                            OspPickedInTRepository.Delete(id, true);
                            var m1 = StockRecord(id.StockId, bef, stock.PrimaryAvailableQty, chg, 0, 0, 0, CategoryCode.Process, ActionCode.Deleted, header.BatchNo, UserId);
                            if (!m1.Success)
                            {
                                throw new Exception($"CODE:{m1.Code} MSG:{m1.Msg}");
                            }
                        }
                    }
                    txn.Commit();
                    return new ResultModel(true, "");
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }

            }

        }

        /// <summary>
        /// 設定裁切日期
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Dialog_CuttingDateFrom"></param>
        /// <param name="Dialog_CuttingDateTo"></param>
        /// <param name="Dialog_MachineNum"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public ResultModel SetStatusAndCutDate(long OspHeaderId, DateTime Dialog_CuttingDateFrom, DateTime Dialog_CuttingDateTo,
            string Dialog_MachineNum, string BtnStatus, string UserId)
        {
            try
            {
                var header = OspHeaderTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (header != null)
                {
                    header.CuttingDateFrom = Dialog_CuttingDateFrom;
                    header.CuttingDateTo = Dialog_CuttingDateTo;
                    header.MachineCode = Dialog_MachineNum == "*" ? "" : Dialog_MachineNum;
                    header.Status = BtnStatus;
                    header.LastUpdateBy = UserId;
                    header.LastUpdateDate = DateTime.Now;
                    OspHeaderTRepository.Update(header, true);
                    return new ResultModel(true, "成功");
                }
                else
                {
                    return new ResultModel(false, "找不到ID");
                }

            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }
        }

        /// <summary>
        /// 關帳用
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public ResultModel SetClose(long OspHeaderId, string BtnStatus)
        {
            try
            {
                var header = OspHeaderTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (header != null)
                {
                    header.Status = BtnStatus;
                    OspHeaderTRepository.Update(header, true);
                    return new ResultModel(true, "成功");
                }
                else
                {
                    return new ResultModel(false, "找不到ID");
                }

            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }
        }

        /// <summary>
        /// 檢查工單號是否正確
        /// </summary>
        /// <param name="InputBatchNo"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ResultModel CheckBatchNo(string InputBatchNo, long OspHeaderId)
        {
            try
            {
                var batchno = OspHeaderTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (batchno == null)
                {
                    return new ResultModel(false, "單號錯誤");
                }
                else
                {
                    if (batchno.BatchNo == InputBatchNo)
                    {
                        return new ResultModel(true, "");
                    }
                    else
                    {
                        return new ResultModel(false, "工單號輸入不對請重新輸入");
                    }
                }


            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }

        }

        /// <summary>
        /// 加工投入檢查庫存條碼
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public ResultDataModel<STOCK_T> CheckStockBarcode(string Barcode, long OspDetailInId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT
ST.[STOCK_ID] as StockId
,ST.[ORGANIZATION_ID] as OrganizationId
,ST.[ORGANIZATION_CODE] as OrganizationCode
,ST.[SUBINVENTORY_CODE] as SubinventoryCode
,ST.[LOCATOR_ID] as LocatorId
,ST.[LOCATOR_SEGMENTS] as LocatorSegments
,ST.[INVENTORY_ITEM_ID] as InventoryItemId
,ST.[ITEM_NUMBER] as ItemNumber
,ST.[ITEM_DESCRIPTION] as ItemDescription
,ST.[ITEM_CATEGORY] as ItemCategory
,ST.[PAPER_TYPE] as PaperType
,ST.[BASIC_WEIGHT] as BasicWeight
,ST.[REAM_WEIGHT] as ReamWeight
,ST.[ROLL_REAM_WT] as RollReamWt
,ST.[SPECIFICATION] as Specification
,ST.[PACKING_TYPE] as PackingType
,ST.[OSP_BATCH_NO] as OspBatchNo
,ST.[LOT_NUMBER] as LotNumber
,ST.[LOT_QUANTITY] as LotQuantity
,ST.[BARCODE] as Barcode
,ST.[PRIMARY_UOM_CODE] as PrimaryUomCode
,ST.[PRIMARY_TRANSACTION_QTY] as PrimaryTransactionQty
,ST.[PRIMARY_AVAILABLE_QTY] as PrimaryAvailableQty
,ST.[PRIMARY_LOCKED_QTY] as PrimaryLockedQty
,ST.[SECONDARY_UOM_CODE] as SecondaryUomCode
,ST.[SECONDARY_TRANSACTION_QTY] as SecondaryTransactionQty
,ST.[SECONDARY_AVAILABLE_QTY] as SecondaryAvailableQty
,ST.[SECONDARY_LOCKED_QTY] as SecondaryLockedQty
,ST.[REASON_CODE] as ReasonCode
,ST.[REASON_DESC] as ReasonDesc
,ST.[NOTE] as Note
,ST.[STATUS_CODE] as StatusCode
,ST.[CREATED_BY] as CreatedBy
,ST.[CREATION_DATE] as CreationDate
,ST.[LAST_UPDATE_BY] as LastUpdateBy
,ST.[LAST_UPDATE_DATE] as LastUpdateDate
,ST.[CONTAINER_NO] as ContainerNo
FROM [STOCK_T] ST
join OSP_DETAIL_IN_T DT ON DT.OSP_DETAIL_IN_ID = @OSP_DETAIL_IN_ID
WHERE ST.ITEM_NUMBER = DT.INVENTORY_ITEM_NUMBER
AND ST.BARCODE = @BARCODE");
                    sqlParameterList.Add(new SqlParameter("@OSP_DETAIL_IN_ID", OspDetailInId));
                    sqlParameterList.Add(new SqlParameter("@BARCODE", Barcode));
                    var data = mesContext.Database.SqlQuery<STOCK_T>(query.ToString(), sqlParameterList.ToArray()).SingleOrDefault();
                    if (data == null)
                    {
                        return new ResultDataModel<STOCK_T>(false, "無條碼資料", null);
                    }

                    var detailin = OspDetailInTRepository.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault();
                    if (detailin == null) return new ResultDataModel<STOCK_T>(false, "沒有加工投入明細資料", null);

                    if (detailin.LocatorId != null && data.LocatorId != detailin.LocatorId)
                    {
                        return new ResultDataModel<STOCK_T>(false, "請投入儲位為" + detailin.LocatorCode + "的條碼", null);
                    }

                    return new ResultDataModel<STOCK_T>(true, "", data);
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultDataModel<STOCK_T>(false, e.Message, null);
            }

        }

        /// <summary>
        /// 儲存加工撿貨條碼
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="Remnant"></param>
        /// <param name="Remaining_Weight"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public ResultModel SavePickIn(string Barcode, string Remnant, string Remaining_Weight, long OspDetailInId, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT
ST.[STOCK_ID] as StockId
,ST.[ORGANIZATION_ID] as OrganizationId
,ST.[ORGANIZATION_CODE] as OrganizationCode
,ST.[SUBINVENTORY_CODE] as SubinventoryCode
,ST.[LOCATOR_ID] as LocatorId
,ST.[LOCATOR_SEGMENTS] as LocatorSegments
,ST.[INVENTORY_ITEM_ID] as InventoryItemId
,ST.[ITEM_NUMBER] as ItemNumber
,ST.[ITEM_DESCRIPTION] as ItemDescription
,ST.[ITEM_CATEGORY] as ItemCategory
,ST.[PAPER_TYPE] as PaperType
,ST.[BASIC_WEIGHT] as BasicWeight
,ST.[REAM_WEIGHT] as ReamWeight
,ST.[ROLL_REAM_WT] as RollReamWt
,ST.[SPECIFICATION] as Specification
,ST.[PACKING_TYPE] as PackingType
,ST.[OSP_BATCH_NO] as OspBatchNo
,ST.[LOT_NUMBER] as LotNumber
,ST.[LOT_QUANTITY] as LotQuantity
,ST.[BARCODE] as Barcode
,ST.[PRIMARY_UOM_CODE] as PrimaryUomCode
,ST.[PRIMARY_TRANSACTION_QTY] as PrimaryTransactionQty
,ST.[PRIMARY_AVAILABLE_QTY] as PrimaryAvailableQty
,ST.[PRIMARY_LOCKED_QTY] as PrimaryLockedQty
,ST.[SECONDARY_UOM_CODE] as SecondaryUomCode
,ST.[SECONDARY_TRANSACTION_QTY] as SecondaryTransactionQty
,ST.[SECONDARY_AVAILABLE_QTY] as SecondaryAvailableQty
,ST.[SECONDARY_LOCKED_QTY] as SecondaryLockedQty
,ST.[REASON_CODE] as ReasonCode
,ST.[REASON_DESC] as ReasonDesc
,ST.[NOTE] as Note
,ST.[STATUS_CODE] as StatusCode
,ST.[CREATED_BY] as CreatedBy
,ST.[CREATION_DATE] as CreationDate
,ST.[LAST_UPDATE_BY] as LastUpdateBy
,ST.[LAST_UPDATE_DATE] as LastUpdateDate
,ST.[CONTAINER_NO] as ContainerNo
FROM [STOCK_T] ST
join OSP_DETAIL_IN_T DT ON DT.OSP_DETAIL_IN_ID = @OSP_DETAIL_IN_ID
WHERE ST.ITEM_NUMBER = DT.INVENTORY_ITEM_NUMBER
AND ST.BARCODE = @BARCODE");
                    sqlParameterList.Add(new SqlParameter("@OSP_DETAIL_IN_ID", OspDetailInId));
                    sqlParameterList.Add(new SqlParameter("@BARCODE", Barcode));
                    var data = Context.Database.SqlQuery<STOCK_T>(query.ToString(), sqlParameterList.ToArray()).SingleOrDefault();

                    if (data == null)
                    {
                        return new ResultModel(false, "無條碼資料");
                    }

                    if (data.StatusCode != StockStatusCode.InStock)
                    {
                        return new ResultModel(false, "條碼狀態須為在庫才可投入");
                    }

                    var detailin = OspDetailInTRepository.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault();
                    if (detailin == null) return new ResultModel(false, "沒有加工投入明細資料");

                    if (detailin.LocatorId != null && data.LocatorId != detailin.LocatorId)
                    {
                        return new ResultModel(false, "請投入儲位為" + detailin.LocatorCode + "的條碼");
                    }

                    var OspPickIn = OspPickedInTRepository.Get(x => x.Barcode == Barcode && x.OspDetailInId == OspDetailInId).SingleOrDefault();
                    if (OspPickIn == null)
                    {
                        //var detailin = OspDetailInTRepository.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault();
                        var Header = OspHeaderTRepository.Get(x => x.OspHeaderId == detailin.OspHeaderId).SingleOrDefault();
                        OSP_PICKED_IN_T oSP_PICKED_IN_T = new OSP_PICKED_IN_T();
                        oSP_PICKED_IN_T.OspDetailInId = OspDetailInId;
                        oSP_PICKED_IN_T.OspHeaderId = detailin == null ? 0 : detailin.OspHeaderId;
                        oSP_PICKED_IN_T.StockId = data.StockId;
                        oSP_PICKED_IN_T.Barcode = Barcode;
                        oSP_PICKED_IN_T.InventoryItemId = data.InventoryItemId;
                        oSP_PICKED_IN_T.InventoryItemNumber = data.ItemNumber;
                        oSP_PICKED_IN_T.PaperType = data.PaperType;
                        oSP_PICKED_IN_T.BasicWeight = data.BasicWeight;
                        oSP_PICKED_IN_T.Specification = data.Specification;
                        oSP_PICKED_IN_T.LotNumber = data.LotNumber;
                        oSP_PICKED_IN_T.LotQuantity = data.LotQuantity;
                        oSP_PICKED_IN_T.PrimaryQuantity = data.PrimaryAvailableQty;
                        oSP_PICKED_IN_T.PrimaryUom = data.PrimaryUomCode;
                        oSP_PICKED_IN_T.SecondaryQuantity = data.SecondaryAvailableQty;
                        oSP_PICKED_IN_T.SecondaryUom = data.SecondaryUomCode;
                        oSP_PICKED_IN_T.HasRemaint = Remnant == "1" ? "有" : "無";
                        oSP_PICKED_IN_T.RemainingQuantity = Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight);
                        oSP_PICKED_IN_T.RemainingUom = Remaining_Weight == "" ? null : "KG";
                        oSP_PICKED_IN_T.CreatedBy = UserId;
                        oSP_PICKED_IN_T.CreatedUserName = UserName;
                        oSP_PICKED_IN_T.CreationDate = DateTime.Now;
                        OspPickedInTRepository.Create(oSP_PICKED_IN_T, true);
                        //var aft = Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight);
                        var bef = data.PrimaryAvailableQty;
                        var chg = data.PrimaryAvailableQty - (Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight));
                        var lockQty = (data.PrimaryLockedQty ?? 0) + chg;
                        var aft = data.PrimaryAvailableQty - chg;
                        if (data.PrimaryAvailableQty >= aft)
                        {
                            CheckStock(data.StockId, UserId, aft, lockQty, StockStatusCode.ProcessPicked);
                            var m1 = StockRecord(data.StockId, bef, aft, -1*chg, 0, 0, 0, CategoryCode.Process, ActionCode.Picked, Header.BatchNo, UserId);
                            if (!m1.Success)
                            {
                                throw new Exception($"CODE:{m1.Code} MSG:{m1.Msg}");
                            }
                            var delteRateResult = DeleteRateNoTransaction(oSP_PICKED_IN_T.OspHeaderId);
                            if (!delteRateResult.Success) throw new Exception(delteRateResult.Msg);
                            txn.Commit();
                            return new ResultModel(true, "寫入成功");
                        }
                        else
                        {
                            txn.Rollback();
                            return new ResultModel(false, "庫存數量不正確");
                        }

                    }
                    else
                    {
                        return new ResultModel(false, "條碼資料已存在");
                    }

                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }
            }
        }

        /// <summary>
        /// 儲存產出條碼
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <param name="Production_Roll_Ream_Qty"></param>
        /// <param name="Production_Roll_Ream_Wt"></param>
        /// <param name="Cotangent"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultModel CreateProduction(string UserId, string UserName, string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId, long OspDetailInId, long OspHeaderId)
        {
            if (Production_Roll_Ream_Qty == null)
            {
                return new ResultModel(false, "令數不得空白");
            }
            if (Production_Roll_Ream_Wt == null)
            {
                return new ResultModel(false, "棧板數不得空白");
            }
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var save = InsertPickOut(UserId, UserName, Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId, OspDetailInId);
                    var delteRateResult = DeleteRateNoTransaction(OspHeaderId);
                    if (!delteRateResult.Success) throw new Exception(delteRateResult.Msg);
                    if (save.Success == false)
                    {
                        txn.Rollback();
                    }
                    else
                    {
                        txn.Commit();
                    }
                    return new ResultModel(save.Success, save.Msg);
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                    return new ResultModel(false, e.Message);
                }

            }

        }

        /// <summary>
        /// 儲存產出和餘切檢貨Table PICK
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <param name="Production_Roll_Ream_Qty"></param>
        /// <param name="Production_Roll_Ream_Wt"></param>
        /// <param name="Cotangent"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultModel InsertPickOut(string UserId, string UserName, string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId, long OspDetailInId)
        {

            var detailout = OspDetailOutTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
            var header = OspHeaderTRepository.Get(x => x.OspHeaderId == detailout.OspHeaderId).SingleOrDefault();
            if (detailout == null)
            {
                return new ResultModel(false, "ID錯誤");
            }
            OSP_PICKED_OUT_T ospPickOut = new OSP_PICKED_OUT_T();
            OSP_COTANGENT_T ospCotanget = new OSP_COTANGENT_T();

            if (Cotangent == "1")
            {
                var relate = OspDetailInTRepository.Get().
                    Join(relatedTRepository.GetAll(),
                    s => s.InventoryItemId,
                    c => c.InventoryItemId,
                    (s, c) => new
                    {
                        a = s,
                        b = c

                    }).Where(
                    x => x.a.OspDetailInId == OspDetailInId &&
                    x.b.InventoryItemId == x.a.InventoryItemId &&
                    x.b.ControlFlag != "D").SingleOrDefault();

                if (relate == null)
                {
                    return new ResultModel(false, "無餘切資料");
                }
                //尋找餘切
                var Relateitem = itemsTRepository.Get(x => x.InventoryItemId == relate.b.RelatedItemId).SingleOrDefault();

                var barcodesResult = GenerateBarcodes(header.OrganizationId, detailout.Subinventory, 1, UserName);
                if (!barcodesResult.Success) throw new Exception(barcodesResult.Msg);

                ospCotanget.OspDetailOutId = OspDetailOutId;
                ospCotanget.OspHeaderId = detailout.OspHeaderId;
                ospCotanget.StockId = null;
                ospCotanget.Barcode = barcodesResult.Data[0];
                ospCotanget.InventoryItemId = Relateitem.InventoryItemId;
                ospCotanget.InventoryItemNumber = Relateitem.ItemNumber;
                ospCotanget.BasicWeight = Relateitem.CatalogElemVal040;
                ospCotanget.Specification = Relateitem.CatalogElemVal050;
                ospCotanget.PaperType = Relateitem.CatalogElemVal020;
                ospCotanget.LotNumber = "";
                ospCotanget.LotQuantity = null;
                ospCotanget.PrimaryQuantity = 0M;
                ospCotanget.PrimaryUom = Relateitem.PrimaryUomCode;
                ospCotanget.SecondaryQuantity = 0M;
                ospCotanget.SecondaryUom = Relateitem.SecondaryUomCode;
                ospCotanget.Status = "待入庫";
                ospCotanget.CreatedBy = UserId;
                ospCotanget.CreatedUserName = UserName;
                ospCotanget.CreationDate = DateTime.Now;
                OspCotangenTRepository.Create(ospCotanget, true);

            }

            var generateBarcodesResult = GenerateBarcodes(header.OrganizationId, detailout.Subinventory, int.Parse(Production_Roll_Ream_Qty), UserName);
            if (!generateBarcodesResult.Success) throw new Exception(generateBarcodesResult.Msg);

            for (int i = 0; i < int.Parse(Production_Roll_Ream_Qty); i++)
            {
                ospPickOut.OspDetailOutId = OspDetailOutId;
                ospPickOut.OspHeaderId = detailout.OspHeaderId;
                ospPickOut.StockId = null;
                ospPickOut.Barcode = generateBarcodesResult.Data[i];
                ospPickOut.InventoryItemId = detailout.InventoryItemId;
                ospPickOut.InventoryItemNumber = detailout.InventoryItemNumber;
                ospPickOut.BasicWeight = detailout.BasicWeight;
                ospPickOut.Specification = detailout.Specification;
                ospPickOut.PaperType = detailout.PaperType;
                ospPickOut.LotNumber = "";
                ospPickOut.LotQuantity = null;
                ospPickOut.PrimaryQuantity = uomConversion.Convert(detailout.InventoryItemId, decimal.Parse(Production_Roll_Ream_Wt), "RE", "KG").Data;
                ospPickOut.PrimaryUom = "KG";
                ospPickOut.SecondaryQuantity = decimal.Parse(Production_Roll_Ream_Wt);
                ospPickOut.SecondaryUom = "RE";
                ospPickOut.Status = "待入庫";
                ospPickOut.Cotangent = null;
                ospPickOut.OspCotangentId = null;
                //ospPickOut.Cotangent = Cotangent == "1" ? "Y" : "N";
                //ospPickOut.OspCotangentId = ospCotanget.OspCotangentId;
                ospPickOut.CreatedBy = UserId;
                ospPickOut.CreatedUserName = UserName;
                ospPickOut.CreationDate = DateTime.Now;
                OspPickedOutTRepository.Create(ospPickOut, true);
            }

            return new ResultModel(true, "");


        }

        /// <summary>
        /// 代工-紙捲產出條碼
        /// </summary>
        /// <param name="PaperRoll_Basic_Weight"></param>
        /// <param name="PaperRoll_Specification"></param>
        /// <param name="PaperRoll_Lot_Number"></param>
        /// <param name="OspDetailOutId"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel PaperRollCreateProduction(string PaperRoll_Weight, string PaperRoll_Lot_Number, long OspDetailOutId, string UserId, string UserName)
        {
            if (PaperRoll_Weight == null)
            {
                return new ResultModel(false, "重量不得空白");
            }

            if (PaperRoll_Lot_Number == null)
            {
                return new ResultModel(false, "捲號不得空白");
            }

            var tempDetail = OspPickedOutTRepository.GetAll().FirstOrDefault(x => x.LotNumber == PaperRoll_Lot_Number);
            if (tempDetail != null) return new ResultModel(false, "捲號不可重複");
            var stock = stockTRepository.GetAll().FirstOrDefault(x => x.LotNumber == PaperRoll_Lot_Number);
            if (stock != null) return new ResultModel(false, "此捲號" + PaperRoll_Lot_Number + "已入庫");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var detailout = OspDetailOutTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                    var header = OspHeaderTRepository.Get(x => x.OspHeaderId == detailout.OspHeaderId).SingleOrDefault();
                    if (detailout == null)
                    {
                        return new ResultModel(false, "ID錯誤");
                    }
                    OSP_PICKED_OUT_T ospPickOut = new OSP_PICKED_OUT_T();
                    //OSP_COTANGENT_T ospCotanget = new OSP_COTANGENT_T();

                    var barcodesResult = GenerateBarcodes(header.OrganizationId, detailout.Subinventory, 1, UserName);
                    if (!barcodesResult.Success) throw new Exception(barcodesResult.Msg);

                    ospPickOut.OspDetailOutId = OspDetailOutId;
                    ospPickOut.OspHeaderId = detailout.OspHeaderId;
                    ospPickOut.StockId = null;
                    ospPickOut.Barcode = barcodesResult.Data[0];
                    ospPickOut.InventoryItemId = detailout.InventoryItemId;
                    ospPickOut.InventoryItemNumber = detailout.InventoryItemNumber;
                    ospPickOut.BasicWeight = detailout.BasicWeight;
                    ospPickOut.Specification = detailout.Specification;
                    ospPickOut.PaperType = detailout.PaperType;
                    ospPickOut.LotNumber = PaperRoll_Lot_Number;
                    ospPickOut.LotQuantity = decimal.Parse(PaperRoll_Weight);
                    ospPickOut.PrimaryQuantity = decimal.Parse(PaperRoll_Weight);
                    ospPickOut.PrimaryUom = "KG";
                    ospPickOut.SecondaryQuantity = 0;
                    ospPickOut.SecondaryUom = "";
                    ospPickOut.Status = "待入庫";
                    //ospPickOut.Cotangent = "N";
                    ospPickOut.Cotangent = null;
                    ospPickOut.OspCotangentId = null;
                    //ospPickOut.OspCotangentId = ospCotanget.OspCotangentId;
                    ospPickOut.CreatedBy = UserId;
                    ospPickOut.CreatedUserName = UserName;
                    ospPickOut.CreationDate = DateTime.Now;
                    OspPickedOutTRepository.Create(ospPickOut, true);
                    var delteRateResult = DeleteRateNoTransaction(ospPickOut.OspHeaderId);
                    if (!delteRateResult.Success) throw new Exception(delteRateResult.Msg);
                    txn.Commit();
                    return new ResultModel(true, "");
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                    return new ResultModel(false, e.Message);
                }

            }
        }


        /// <summary>
        /// 取得產出檢貨資料
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public List<Production> GetPicketOut(long OspHeaderId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT
[OSP_PICKED_OUT_ID] as OspPickedOutId,
[OSP_DETAIL_OUT_ID] as OspDetailOutId,
[OSP_HEADER_ID] as OspHeaderId,
[STOCK_ID] as StockId,
[BARCODE] as Barcode,
[INVENTORY_ITEM_ID] as InventoryItemId,
[INVENTORY_ITEM_NUMBER] as Product_Item,
[PAPER_TYPE] as PaperType,
[BASIC_WEIGHT] as BasicWeight,
[SPECIFICATION] as Specification,
[LOT_NUMBER] as LotNumber,
[LOT_QUANTITY] as LotQuantity,
[PRIMARY_QUANTITY] as PrimaryQuantity,
[PRIMARY_UOM] as PrimaryUom,
[SECONDARY_QUANTITY] as SecondaryQuantity,
[SECONDARY_UOM] as SecondaryUom,
[STATUS] as Status
FROM OSP_PICKED_OUT_T
WHERE OSP_HEADER_ID = @OSP_HEADER_ID");
                    string commandText = string.Format(query.ToString());
                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_PICKED_OUT_T", "OSP_PICKED_OUT_HT"));
                    return mesContext.Database.SqlQuery<Production>(commandText, new SqlParameter("@OSP_HEADER_ID", OspHeaderId)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<Production>();
            }
        }

        /// <summary>
        /// 取得餘切資料
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public List<Cotangent> GetCotangents(long OspHeaderId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
[OSP_COTANGENT_ID] AS OspCotangentId,
[OSP_DETAIL_OUT_ID] AS OspDetailOutId,
[OSP_HEADER_ID] AS OspHeaderId,
[BARCODE] AS Barcode,
[INVENTORY_ITEM_ID] as InventoryItemId,
[INVENTORY_ITEM_NUMBER] as Related_item,
[PRIMARY_QUANTITY] AS PrimaryQuantity,
[PRIMARY_UOM] as PrimaryUom,
[SECONDARY_QUANTITY] AS SecondaryQuantity ,
[SECONDARY_UOM] as SecondaryUom,
[STATUS] AS Status
FROM [OSP_COTANGENT_T]
where OSP_HEADER_ID = @OSP_HEADER_ID");
                    string commandText = string.Format(query.ToString());
                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_COTANGENT_T", "OSP_COTANGENT_HT"));
                    return mesContext.Database.SqlQuery<Cotangent>(commandText, new SqlParameter("@OSP_HEADER_ID", OspHeaderId)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<Cotangent>();
            }
        }

        /// <summary>
        /// 產出Editor
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        public ResultModel SetProductionEditor(ProductionDTEditor ProductionDTEditor, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    if (ProductionDTEditor.Action == "edit")
                    {
                        var OspPickedOutId = ProductionDTEditor.ProductionList[0].OspPickedOutId;
                        var id = OspPickedOutTRepository.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            id.SecondaryQuantity = ProductionDTEditor.ProductionList[0].SecondaryQuantity;
                            id.PrimaryQuantity = uomConversion.Convert(id.InventoryItemId, ProductionDTEditor.ProductionList[0].SecondaryQuantity, "RE", "KG").Data;
                            id.LastUpdateBy = UserId;
                            id.LastUpdateUserName = UserName;
                            id.LastUpdateDate = DateTime.Now;
                            OspPickedOutTRepository.Update(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "找不到ID");
                    }
                    else if (ProductionDTEditor.Action == "remove")
                    {
                        var OspPickedOutId = ProductionDTEditor.ProductionList[0].OspPickedOutId;
                        var id = OspPickedOutTRepository.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            OspPickedOutTRepository.Delete(id, true);
                            //if (id.Cotangent == "N")
                            //{
                            //    //產出資料沒有餘切時，直接刪除產出資料
                            //    OspPickedOutTRepository.Delete(id, true);
                            //}
                            //else
                            //{
                            //    var PickedOutList = OspPickedOutTRepository.GetAll().AsNoTracking().Where(x => x.OspCotangentId == id.OspCotangentId).ToList(); //取得同餘切id的pick產出資料
                            //    if (PickedOutList == null)
                            //    {
                            //        throw new Exception("找不到產出資料");
                            //    }
                            //    else if (PickedOutList.Count == 1)
                            //    {
                            //        //當產出資料只有一個時，刪除對映的餘切
                            //        var cotangent = OspCotangenTRepository.GetAll().FirstOrDefault(x => x.OspCotangentId == id.OspCotangentId);
                            //        if (cotangent == null) throw new Exception("找不到餘切資料");
                            //        OspCotangenTRepository.Delete(cotangent, true);
                            //        OspPickedOutTRepository.Delete(id, true);
                            //    }
                            //    else
                            //    {
                            //        //當產出資料有多個時，保留餘切資料
                            //        OspPickedOutTRepository.Delete(id, true);
                            //    }
                            //}

                            //var PickedOut = OspPickedOutTRepository.GetAll().ToList();
                            //if (PickedOut.Count == 1)
                            //{
                            //    //var cotangent = OspCotangenTRepository.Get(x => x.OspCotangentId == id.OspCotangentId).SingleOrDefault();
                            //    var cotangent = OspCotangenTRepository.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                            //    if (cotangent != null)
                            //    {
                            //        OspCotangenTRepository.Delete(cotangent, true);
                            //    }
                            //}
                            //OspPickedOutTRepository.Delete(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "找不到ID");
                    }
                    else
                    {
                        return new ResultModel(false, "無法識別作業項目");
                    }
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }

            }
        }

        /// <summary>
        /// 產出選擇刪除
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel ProductionChooseDelete(long[] OspPickedOutId)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    for(int i=0; i < OspPickedOutId.Length; i++)
                    {
                        var OspPickedOutId1 = OspPickedOutId[i];
                        var id = OspPickedOutTRepository.Get(r => r.OspPickedOutId == OspPickedOutId1).SingleOrDefault();
                        if (id == null)
                        {
                            return new ResultModel(false, "找不到ID");
                        }
                        OspPickedOutTRepository.Delete(id, true);
                    }
                    txn.Commit();
                    return new ResultModel(true, "");
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }

            }
        }

        /// <summary>
        /// 代紙紙捲產出Editor
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        public ResultModel SetPaperRollerProductionEditor(ProductionDTEditor ProductionDTEditor, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    if (ProductionDTEditor.Action == "edit")
                    {
                        var OspPickedOutId = ProductionDTEditor.ProductionList[0].OspPickedOutId;
                        var id = OspPickedOutTRepository.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            id.PrimaryQuantity = ProductionDTEditor.ProductionList[0].PrimaryQuantity;
                            id.LastUpdateBy = UserId;
                            id.LastUpdateUserName = UserName;
                            id.LastUpdateDate = DateTime.Now;
                            OspPickedOutTRepository.Update(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "找不到ID");
                    }
                    else if (ProductionDTEditor.Action == "remove")
                    {
                        var OspPickedOutId = ProductionDTEditor.ProductionList[0].OspPickedOutId;
                        var id = OspPickedOutTRepository.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            var PickedOut = OspPickedOutTRepository.GetAll().ToList();
                            if (PickedOut.Count == 1)
                            {
                                //var cotangent = OspCotangenTRepository.Get(x => x.OspCotangentId == id.OspCotangentId).SingleOrDefault();
                                var cotangent = OspCotangenTRepository.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                                if (cotangent != null)
                                {
                                    OspCotangenTRepository.Delete(cotangent, true);
                                }
                            }
                            OspPickedOutTRepository.Delete(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "找不到ID");
                    }
                    else
                    {
                        return new ResultModel(false, "無法識別作業項目");
                    }
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }

            }
        }

        /// <summary>
        /// 餘切Editor
        /// </summary>
        /// <param name="cotangentDTEditor"></param>
        /// <returns></returns>
        public ResultModel SetCotangentEditor(CotangentDTEditor cotangentDTEditor, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    if (cotangentDTEditor.Action == "edit")
                    {
                        var OspCotangentId = cotangentDTEditor.CotangentList[0].OspCotangentId;
                        var id = OspCotangenTRepository.Get(r => r.OspCotangentId == OspCotangentId).SingleOrDefault();
                        if (id != null)
                        {
                            id.SecondaryQuantity = cotangentDTEditor.CotangentList[0].SecondaryQuantity;
                            id.PrimaryQuantity = uomConversion.Convert(id.InventoryItemId, cotangentDTEditor.CotangentList[0].SecondaryQuantity, "RE", "KG").Data;
                            id.LastUpdateBy = UserId;
                            id.LastUpdateUserName = UserName;
                            id.LastUpdateDate = DateTime.Now;
                            OspCotangenTRepository.Update(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "找不到ID");
                    }
                    else if (cotangentDTEditor.Action == "remove")
                    {
                        var OspCotangentId = cotangentDTEditor.CotangentList[0].OspCotangentId;
                        var id = OspCotangenTRepository.Get(r => r.OspCotangentId == OspCotangentId).SingleOrDefault();
                        if (id != null)
                        {
                            //var osppickList = OspPickedOutTRepository.Get(x => x.OspCotangentId == id.OspCotangentId).ToList();
                            //foreach (var osppick in osppickList)
                            //{
                            //    osppick.Cotangent = "N";
                            //    osppick.OspCotangentId = null;
                            //    osppick.LastUpdateBy = UserId;
                            //    osppick.LastUpdateUserName = UserName;
                            //    osppick.LastUpdateDate = DateTime.Now;
                            //    OspPickedOutTRepository.Update(osppick, true);
                            //}
                            OspCotangenTRepository.Delete(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "找不到ID");
                    }
                    else
                    {
                        return new ResultModel(false, "無法識別作業項目");
                    }
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }
            }
        }

        /// <summary>
        /// 餘切選擇刪除
        /// </summary>
        /// <param name="OspCotangentId"></param>
        /// <returns></returns>
        public ResultModel CotangentChooseDelete(long[] OspCotangentId)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    for(int i=0;i< OspCotangentId.Length; i++)
                    {
                        var OspCotangentId1 = OspCotangentId[i];
                        var id = OspCotangenTRepository.Get(r => r.OspCotangentId == OspCotangentId1).SingleOrDefault();
                        if (id == null)
                        {
                            return new ResultModel(false, "找不到ID");
                        }
                        OspCotangenTRepository.Delete(id, true);
                    }
                    txn.Commit();
                    return new ResultModel(true, "");
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }
            }
        }

        /// <summary>
        /// 產出條碼入庫
        /// </summary>
        /// <param name="Production_Barcode"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultModel ProductionChangeStatus(string Production_Barcode, long OspDetailOutId, string UserId, string UserName)
        {
            try
            {
                var ProductionId = OspPickedOutTRepository.Get(r => r.Barcode == Production_Barcode && r.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                if (ProductionId != null)
                {

                    if (ProductionId.Status == "已入庫")
                    {
                        return new ResultModel(false, "已入庫");
                    }
                    else
                    {
                        ProductionId.Status = "已入庫";
                        ProductionId.LastUpdateBy = UserId;
                        ProductionId.LastUpdateUserName = UserName;
                        ProductionId.LastUpdateDate = DateTime.Now;
                        OspPickedOutTRepository.Update(ProductionId, true);
                        return new ResultModel(true, "");
                    }

                }
                else
                {
                    return new ResultModel(false, "無條碼存在");
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }


        }

        /// <summary>
        /// 餘切條碼入庫
        /// </summary>
        /// <param name="CotangentBarcode"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultModel CotangentChangeStatus(string CotangentBarcode, long OspDetailOutId, string UserId, string UserName)
        {

            try
            {
                var CotangentId = OspCotangenTRepository.Get(r => r.Barcode == CotangentBarcode && r.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                if (CotangentId != null)
                {

                    if (CotangentId.Status == "已入庫")
                    {
                        return new ResultModel(false, "已入庫");
                    }
                    else if (CotangentId.SecondaryQuantity == null || CotangentId.SecondaryQuantity == 0M)
                    {
                        return new ResultModel(false, "請先輸入令數");
                    }
                    else
                    {
                        CotangentId.Status = "已入庫";
                        CotangentId.LastUpdateBy = UserId;
                        CotangentId.LastUpdateUserName = UserName;
                        CotangentId.LastUpdateDate = DateTime.Now;
                        OspCotangenTRepository.Update(CotangentId, true);
                        return new ResultModel(true, "");
                    }

                }
                else
                {
                    return new ResultModel(false, "無條碼存在");
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }

        }

        /// <summary>
        /// 計算損耗
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultDataModel<YieldVariance> Loss(long OspDetailInId, long OspDetailOutId, string UserId, string UserName)
        {
            try
            {
                var PickIn = OspPickedInTRepository.Get(x => x.OspDetailInId == OspDetailInId).ToList();
                var PickOut = OspPickedOutTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).ToList();
                var DetailIn = OspDetailInTRepository.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault();
                var DetailOut = OspDetailOutTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                var Cotangent = OspCotangenTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).ToList();
                var PickOutWeight = 0M; //產出主單位重量
                var PickOutSecondaryQty = 0M; //產出次單位數量
                var RemainWeight = 0M;
                var PickInWeight = 0M;
                var CotangetnWeight = 0M; //餘切主單位重量
                var CotangetnSecondaryQty = 0M; //餘切次單位數量
                var InvestSecondaryQty = 0M; //投入次單位數量
                string CotangentPrimaryUom = null;
                string CotangentSecondaryUom = null;
                string CotangentItemNumber = null;

                for (int i = 0; i < PickOut.Count; i++)
                {
                    var status = PickOut[i].Status;
                    if (status == "待入庫")
                    {
                        return new ResultDataModel<YieldVariance>(false, "產出請先入庫", null);
                    }
                    PickOutWeight += +PickOut[i].PrimaryQuantity;
                    PickOutSecondaryQty += PickOut[i].SecondaryQuantity ?? 0;
                }
                if (Cotangent != null && Cotangent.Count > 0)
                {
                    CotangentItemNumber = Cotangent[0].InventoryItemNumber;
                    CotangentPrimaryUom = Cotangent[0].PrimaryUom;
                    CotangentSecondaryUom = Cotangent[0].SecondaryUom;
                    for (int i = 0; i < Cotangent.Count; i++)
                    {
                        if (Cotangent[i].SecondaryQuantity == 0M || Cotangent[i].SecondaryQuantity == null)
                        {
                            return new ResultDataModel<YieldVariance>(false, "餘切請先輸入令數", null);
                        }

                        if (Cotangent[i].Status == "待入庫")
                        {
                            return new ResultDataModel<YieldVariance>(false, "餘切請先入庫", null);
                        }
                        CotangetnWeight += Cotangent[i].PrimaryQuantity;
                        CotangetnSecondaryQty += Cotangent[i].SecondaryQuantity ?? 0;
                    }
                }
                //總產出重量：產出重量+餘切重量
                var ProductWeight = PickOutWeight + CotangetnWeight;

                for (int i = 0; i < PickIn.Count; i++)
                {
                    RemainWeight += PickIn[i].RemainingQuantity ?? 0;
                    PickInWeight += PickIn[i].PrimaryQuantity;
                    InvestSecondaryQty += PickIn[i].SecondaryQuantity ?? 0;
                }
                //投入重量：領料量 - 餘捲
                var investWeight = PickInWeight - RemainWeight;

                //var investItem = itemsTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.InventoryItemId == DetailIn.InventoryItemId);
                //if (investItem == null) { throw new Exception("找不到投入料號資料"); }

                //if (investItem.CatalogElemVal070 == ItemCategory.)
                //var investUomConversionResult = uomConversion.Convert(DetailIn.InventoryItemId, investWeight, DetailIn.PrimaryUom, DetailIn.SecondaryUom);
                //if (!investUomConversionResult.Success) throw new Exception(investUomConversionResult.Msg);

                //var Totle = Math.Round(investWeight - ProductWeight, 2);
                //var rate = Math.Round((ProductWeight / investWeight * 100), 2);
                var Totle = investWeight - ProductWeight;
                var rate = ProductWeight / investWeight * 100;
                var loss = OspYieldVarianceTRepository.Get(x => x.OspHeaderId == DetailOut.OspHeaderId).SingleOrDefault();
                if (loss == null)
                {
                    OSP_YIELD_VARIANCE_T oSP_YIELD_VARIANCE_T = new OSP_YIELD_VARIANCE_T();
                    oSP_YIELD_VARIANCE_T.OspHeaderId = DetailOut.OspHeaderId;
                    oSP_YIELD_VARIANCE_T.DetailInItemNumber = DetailIn.InventoryItemNumber;
                    oSP_YIELD_VARIANCE_T.DetailInQuantity = investWeight;
                    oSP_YIELD_VARIANCE_T.DetailInSecondaryQuantity = InvestSecondaryQty;
                    oSP_YIELD_VARIANCE_T.DetailInPrimaryUom = DetailIn.PrimaryUom;
                    oSP_YIELD_VARIANCE_T.DetailInSecondaryUom = DetailIn.SecondaryUom;

                    oSP_YIELD_VARIANCE_T.DetailOutItemNumber = DetailOut.InventoryItemNumber;
                    oSP_YIELD_VARIANCE_T.DetailOutQuantity = ProductWeight;
                    oSP_YIELD_VARIANCE_T.DetailOutPrimaryQuantity = PickOutWeight;
                    oSP_YIELD_VARIANCE_T.DetailOutSecondaryQuantity = PickOutSecondaryQty;
                    oSP_YIELD_VARIANCE_T.DetailOutPrimaryUom = DetailOut.PrimaryUom;
                    oSP_YIELD_VARIANCE_T.DetailOutSecondaryUom = DetailOut.SecondaryUom;

                    oSP_YIELD_VARIANCE_T.CotangentItemNumber = CotangentItemNumber;
                    oSP_YIELD_VARIANCE_T.CotangentQuantity = CotangetnWeight;
                    oSP_YIELD_VARIANCE_T.CotangentSecondaryQuantity = CotangetnSecondaryQty;
                    oSP_YIELD_VARIANCE_T.CotangentPrimaryUom = CotangentPrimaryUom;
                    oSP_YIELD_VARIANCE_T.CotangentSecondaryUom = CotangentSecondaryUom;

                    //oSP_YIELD_VARIANCE_T.PrimaryUom = "KG";
                    oSP_YIELD_VARIANCE_T.LossWeight = Totle;
                    oSP_YIELD_VARIANCE_T.Rate = rate;
                    oSP_YIELD_VARIANCE_T.CreatedBy = UserId;
                    oSP_YIELD_VARIANCE_T.CreatedUserName = UserName;
                    oSP_YIELD_VARIANCE_T.CreationDate = DateTime.Now;
                    OspYieldVarianceTRepository.Create(oSP_YIELD_VARIANCE_T, true);
                    return new ResultDataModel<YieldVariance>(true, "成功",
                        new YieldVariance
                        {
                            OspYieldVarianceId = oSP_YIELD_VARIANCE_T.OspYieldVarianceId,
                            OspHeaderId = oSP_YIELD_VARIANCE_T.OspHeaderId,
                            InvestWeight = Math.Round(oSP_YIELD_VARIANCE_T.DetailInQuantity, 5),
                            ProductWeight = Math.Round(oSP_YIELD_VARIANCE_T.DetailOutQuantity, 5),
                            LossWeight = Math.Round(oSP_YIELD_VARIANCE_T.LossWeight, 5),
                            Rate = Math.Round(oSP_YIELD_VARIANCE_T.Rate, 2)
                        });
                }
                else
                {
                    loss.DetailInQuantity = investWeight;
                    loss.DetailInItemNumber = DetailIn.InventoryItemNumber;
                    loss.DetailInSecondaryQuantity = InvestSecondaryQty;
                    loss.DetailInPrimaryUom = DetailIn.PrimaryUom;
                    loss.DetailInSecondaryUom = DetailIn.SecondaryUom;

                    loss.DetailOutItemNumber = DetailOut.InventoryItemNumber;
                    loss.DetailOutQuantity = ProductWeight;
                    loss.DetailOutPrimaryQuantity = PickOutWeight;
                    loss.DetailOutSecondaryQuantity = PickOutSecondaryQty;
                    loss.DetailOutPrimaryUom = DetailOut.PrimaryUom;
                    loss.DetailOutSecondaryUom = DetailOut.SecondaryUom;

                    loss.CotangentItemNumber = CotangentItemNumber;
                    loss.CotangentQuantity = CotangetnWeight;
                    loss.CotangentSecondaryQuantity = CotangetnSecondaryQty;
                    loss.CotangentPrimaryUom = CotangentPrimaryUom;
                    loss.CotangentSecondaryUom = CotangentSecondaryUom;

                    loss.LossWeight = Totle;
                    loss.Rate = rate;
                    loss.LastUpdateBy = UserId;
                    loss.LastUpdateUserName = UserName;
                    loss.LastUpdateDate = DateTime.Now;
                    OspYieldVarianceTRepository.Update(loss, true);
                    return new ResultDataModel<YieldVariance>(true, "成功",
                        new YieldVariance
                        {
                            OspYieldVarianceId = loss.OspYieldVarianceId,
                            OspHeaderId = loss.OspHeaderId,
                            InvestWeight = Math.Round(loss.DetailInQuantity, 5),
                            ProductWeight = Math.Round(loss.DetailOutQuantity, 5),
                            LossWeight = Math.Round(loss.LossWeight, 5),
                            Rate = Math.Round(loss.Rate, 2)
                        });
                }


            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultDataModel<YieldVariance>(false, e.Message, null);
            }

        }

        /// <summary>
        /// 取得得率重量
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public YieldVariance GetRate(long OspHeaderId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
[OSP_YIELD_VARIANCE_ID] as OspYieldVarianceId,
[OSP_HEADER_ID] as OspHeaderId,
ROUND([LOSS_WEIGHT], 5) as LossWeight,
ROUND([RATE], 2) as Rate,
ROUND([DETAIL_IN_QUANTITY], 5) as InvestWeight,
ROUND([DETAIL_OUT_QUANTITY], 5) as ProductWeight
FROM [OSP_YIELD_VARIANCE_T]
where OSP_HEADER_ID = @OSP_HEADER_ID");
                    string commandText = string.Format(query.ToString());
                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_YIELD_VARIANCE_T", "OSP_YIELD_VARIANCE_HT"));
                    return mesContext.Database.SqlQuery<YieldVariance>(commandText, new SqlParameter("@OSP_HEADER_ID", OspHeaderId)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new YieldVariance();
            }



        }
        /// <summary>
        /// 刪除得率
        /// </summary>
        /// <param name="OspHeaderId"></param>
        public void DeleteRate(long OspHeaderId)
        {

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var op = OspYieldVarianceTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    if (op != null)
                    {
                        OspYieldVarianceTRepository.Delete(op, true);
                        txn.Commit();
                    }

                }

                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));

                }
            }
        }

        /// <summary>
        /// 清除損耗記錄 使用時要在外面加Transaction
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ResultModel DeleteRateNoTransaction(long OspHeaderId)
        {
            try
            {
                var op = OspYieldVarianceTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (op != null)
                {
                    OspYieldVarianceTRepository.Delete(op, true);
                }
                return new ResultModel(true, "清除損耗記錄成功");
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, "清除損耗記錄發生錯誤: " + e.Message);
            }
        }

        /// <summary>
        /// 存檔入庫&&工單號狀態更改
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <param name="Locator"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel ChangeHeaderStatus(long OspHeaderId, string LocatorId, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var header = OspHeaderTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailOut = OspDetailOutTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailIn = OspDetailInTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();

                    if (header == null)
                    {
                        return new ResultModel(false, "找不到id");
                    }
                    else if (header.Status == ProcessStatusCode.CompletedBatch)
                    {
                        return new ResultModel(false, $"此單號 {header.BatchNo} 已完工!!");
                    }
                    else if (header.Status == ProcessStatusCode.PendingBatch) //待核淮存檔
                    {
                        header.Modifications += 1;
                    }

                    header.Status = ProcessStatusCode.CompletedBatch;
                    header.LastUpdateBy = UserId;
                    header.LastUpdateDate = DateTime.Now;

                    OspHeaderTRepository.Update(header, true);
                    var m1 = SaveInvestStock(header.OspHeaderId, StockStatusCode.InStock, StockStatusCode.ProcessPicked, CategoryCode.Process, ActionCode.ProcessPicked, header.BatchNo, UserId);
                    if (!m1.Success)
                    {
                        throw new Exception($"CODE:{m1.Code} MSG:{m1.Msg}");
                    }

                    var m2 = SaveProductStock(header.OspHeaderId, StockStatusCode.InStock, LocatorId, CategoryCode.Process, ActionCode.ProcessStored, header.BatchNo, UserId);
                    if (!m2.Success)
                    {
                        throw new Exception($"CODE:{m2.Code} MSG:{m2.Msg}");
                    }
                    var m3 = SaveContangetStock(header.OspHeaderId, StockStatusCode.InStock, LocatorId, CategoryCode.Process, ActionCode.ProcessStored, header.BatchNo, UserId);
                    if (!m3.Success)
                    {
                        throw new Exception($"CODE:{m3.Code} MSG:{m3.Msg}");
                    }
                    PickInToHt(header.OspHeaderId);
                    PirckOutToHt(header.OspHeaderId);
                    DetailInToHt(header.OspHeaderId);
                    DetailOutToHt(header.OspHeaderId);
                    ContangetToHt(header.OspHeaderId);
                    YieldToHt(header.OspHeaderId);

                    txn.Commit();
                    return new ResultModel(true, "");

                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }
            }

        }

        /// <summary>
        /// 完工紀錄工單號狀態更改
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel EditHeaderStatus(long OspHeaderId, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var header = OspHeaderTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailOut = OspDetailOutTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailIn = OspDetailInTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();

                    if (header == null)
                    {
                        return new ResultModel(false, "找不到id");
                    }

                    header.Status = ProcessStatusCode.PendingBatch;
                    header.LastUpdateBy = UserId;
                    header.LastUpdateDate = DateTime.Now;
                    OspHeaderTRepository.Update(header, true);

                    txn.Commit();
                    return new ResultModel(true, "");

                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultModel(false, e.Message);
                }
            }

        }

        /// <summary>
        /// 完工紀錄編輯
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ResultDataModel<long> FinishedEdit(string BatchNo, long OspHeaderId, string userId)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {

                    var header = OspHeaderTRepository.GetAll().Where(x => x.OspHeaderId == OspHeaderId).FirstOrDefault();

                    if (header == null)
                    {
                        return new ResultDataModel<long>(false, "單號錯誤", 0);
                    }
                    if (header.BatchNo != BatchNo)
                    {
                        return new ResultDataModel<long>(false, "工單號輸入不對請重新輸入", 0);
                    }

                    if (header.Status != ProcessStatusCode.CompletedBatch)
                    {
                        return new ResultDataModel<long>(false, "僅完工狀態可進行修改!!", 0);
                    }

                    List<string> notInStockBarocdeList = new List<string>();

                    var inNotInStockBarocdeList = OspPickedInHTRepository.GetAll().AsNoTracking().Join(
                        stockTRepository.GetAll().AsNoTracking(),
                        o => new { o.StockId },
                        s => new { s.StockId },
                        (o, s) => new
                        {
                            OspHeaderId = o.OspHeaderId,
                            Barcode = s.Barcode,
                            StatusCode = s.StatusCode
                        })
                        .Where(x => x.OspHeaderId == OspHeaderId && x.StatusCode != StockStatusCode.InStock)
                        .Select(x => x.Barcode)
                        .ToList();
                     
                    if (inNotInStockBarocdeList != null && inNotInStockBarocdeList.Count > 0)
                    {
                        notInStockBarocdeList.AddRange(inNotInStockBarocdeList);
                    }

                    var outNotInStockBarocdeList = OspPickedOutHTRepository.GetAll().AsNoTracking().Join(
                        stockTRepository.GetAll().AsNoTracking(),
                        o => o.StockId,
                        s => s.StockId,
                        (o, s) => new
                        {
                            OspHeaderId = o.OspHeaderId,
                            Barcode = s.Barcode,
                            StatusCode = s.StatusCode
                        })
                        .Where(x => x.OspHeaderId == OspHeaderId && x.StatusCode != StockStatusCode.InStock)
                        .Select(x => x.Barcode)
                        .ToList();

                    if (outNotInStockBarocdeList != null && outNotInStockBarocdeList.Count > 0)
                    {
                        notInStockBarocdeList.AddRange(outNotInStockBarocdeList);
                    }
                    
                    var cotangentNotInStockBarocdeList = OspCotangentHTRepository.GetAll().AsNoTracking().Join(
                        stockTRepository.GetAll().AsNoTracking(),
                        o => o.StockId,
                        s => s.StockId,
                        (o, s) => new
                        {
                            OspHeaderId = o.OspHeaderId,
                            Barcode = s.Barcode,
                            StatusCode = s.StatusCode
                        })
                        .Where(x => x.OspHeaderId == OspHeaderId && x.StatusCode != StockStatusCode.InStock)
                        .Select(x => x.Barcode)
                        .ToList();

                    if (cotangentNotInStockBarocdeList != null && cotangentNotInStockBarocdeList.Count > 0)
                    {
                        notInStockBarocdeList.AddRange(cotangentNotInStockBarocdeList);
                    }

                    if (notInStockBarocdeList != null && notInStockBarocdeList.Count > 0)
                    {
                        return new ResultDataModel<long>(false, "以下條碼不在庫無法修改:" + string.Join(",", notInStockBarocdeList), 0);
                    }

                    var newHeader = OspHeaderTRepository.GetAll().Where(x => x.OspHeaderId == OspHeaderId).AsNoTracking().FirstOrDefault();
                    if (newHeader.Status == ProcessStatusCode.CompletedBatch)
                    {
                        if (newHeader.Modifications >= 1)
                        {
                            return new ResultDataModel<long>(false, "已修改過，無法再修改", 0);
                        }

                        newHeader.Status = ProcessStatusCode.PendingBatch;
                        newHeader.LastUpdateBy = userId;
                        newHeader.LastUpdateDate = DateTime.Now;
                        newHeader.OspHeaderId = 0;
                        OspHeaderTRepository.Create(newHeader, true);

                        header.Status = ProcessStatusCode.Modified;
                        header.LastUpdateBy = userId;
                        header.LastUpdateDate = DateTime.Now;
                        OspHeaderTRepository.Update(header, true);

                        var headerMod = new OSP_HEADER_MOD_T();
                        headerMod.OrgOspHeaderId = header.OspHeaderId;
                        headerMod.OspHeaderId = newHeader.OspHeaderId;
                        OspHeaderModTRepository.Create(headerMod, true);

                        var m1 = ReturnInvestStock(header.OspHeaderId, StockStatusCode.InStock, StockStatusCode.ProcessPicked, CategoryCode.Process, ActionCode.ProcessEditStored, header.BatchNo, userId);
                        if (!m1.Success)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} CODE:{m1.Code} MSG:{m1.Msg}");
                        }

                        if (DeleteStock(OspHeaderId, userId) <= 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 刪除產品庫存資料失敗");
                        }

                        if (CopyDetailInHT(header.OspHeaderId) <= 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 搬移加工組成成份資料失敗");
                        }

                        if (CopyDetailOutHT(header.OspHeaderId) <= 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 搬移加工產品資料失敗");
                        }

                        if (CopyPickedInHT(header.OspHeaderId) <= 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 搬移加工組成成份揀貨資料失敗");
                        }

                        if (CopyPickedOut(header.OspHeaderId) <= 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 搬移加工產品揀貨資料失敗");
                        }

                        if (CopyContangetHT(header.OspHeaderId) < 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 搬移加工餘切資料失敗");
                        }

                        if (CopyYieldHT(header.OspHeaderId) < 0)
                        {
                            throw new Exception($"OSP_HEADER_ID:{OspHeaderId} 搬移得率資料失敗");
                        }

                        txn.Commit();
                    }

                    return new ResultDataModel<long>(true, "成功", newHeader.OspHeaderId);

                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new ResultDataModel<long>(false, e.Message, 0);
                }
            }
        }

        /// <summary>
        /// 扣除投入庫存已揀量
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="statusCode"></param>
        public ResultModel SaveInvestStock(long headerId, string statusInStockCode, string statusProcessPicked, string Category, string Action, string Doc, string userId)
        {
            var resultModel = new ResultModel(false, "");

            var pHeaderId = SqlParamHelper.GetBigInt("@headerId", headerId);
            var pInStockStatusCode = SqlParamHelper.GetNVarChar("@inStockStatusCode", statusInStockCode);
            var pProcessPickedStatusCode = SqlParamHelper.GetNVarChar("@processPickedStatusCode", statusProcessPicked);

            var pCategory = SqlParamHelper.GetNVarChar("@category", categoryCode.GetDesc(Category));
            var pAction = SqlParamHelper.GetNVarChar("@action", actionCode.GetDesc(Action));
            var pDoc = SqlParamHelper.GetNVarChar("@doc", Doc);
            var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
            var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
            var pUser = SqlParamHelper.GetNVarChar("@user", userId, 128);

            this.Context.Database.ExecuteSqlCommand("[SP_SaveOspPickedIn] @headerId, @inStockStatusCode, @processPickedStatusCode, @category, @action, @doc, @code output, @message output, @user",
                pHeaderId, pInStockStatusCode, pProcessPickedStatusCode, pCategory, pAction, pDoc, pCode, pMsg, pUser);

            if (pCode.Value != DBNull.Value)
            {
                resultModel.Code = Convert.ToInt32(pCode.Value);
                resultModel.Success = resultModel.Code == ResultModel.CODE_SUCCESS;
            }

            if (pMsg.Value != DBNull.Value)
            {
                resultModel.Msg = Convert.ToString(pMsg.Value);
            }

            return resultModel;
        }

        /// <summary>
        /// 還原投入庫存已揀量
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="statusCode"></param>
        public ResultModel ReturnInvestStock(long headerId, string statusInStockCode, string statusProcessPicked, string Category, string Action, string Doc, string userId)
        {
            var resultModel = new ResultModel(false, "");

            var pHeaderId = SqlParamHelper.GetBigInt("@headerId", headerId);
            var pInStockStatusCode = SqlParamHelper.GetNVarChar("@inStockStatusCode", statusInStockCode);
            var pProcessPickedStatusCode = SqlParamHelper.GetNVarChar("@processPickedStatusCode", statusProcessPicked);

            var pCategory = SqlParamHelper.GetNVarChar("@category", categoryCode.GetDesc(Category));
            var pAction = SqlParamHelper.GetNVarChar("@action", actionCode.GetDesc(Action));
            var pDoc = SqlParamHelper.GetNVarChar("@doc", Doc);
            var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
            var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
            var pUser = SqlParamHelper.GetNVarChar("@user", userId, 128);

            this.Context.Database.ExecuteSqlCommand("[SP_ReturnOspPickedIn] @headerId, @inStockStatusCode, @processPickedStatusCode, @category, @action, @doc, @code output, @message output, @user",
                pHeaderId, pInStockStatusCode, pProcessPickedStatusCode, pCategory, pAction, pDoc, pCode, pMsg, pUser);

            if (pCode.Value != DBNull.Value)
            {
                resultModel.Code = Convert.ToInt32(pCode.Value);
                resultModel.Success = resultModel.Code == ResultModel.CODE_SUCCESS;
            }

            if (pMsg.Value != DBNull.Value)
            {
                resultModel.Msg = Convert.ToString(pMsg.Value);
            }

            return resultModel;
        }

        /// <summary>
        /// 轉入新增產品庫存
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="statusCode"></param>
        public ResultModel SaveProductStock(long headerId, string statusCode, string LocatorId, string Category, string Action, string Doc, string userId)
        {
            var resultModel = new ResultModel(false, "");

            if (!long.TryParse(LocatorId, out long locatorId))
            {
                locatorId = 0;
            }
            var locator = locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == locatorId);
            string locatorCode = "";
            if (locator != null) locatorCode = locator.LocatorSegments;

            var pheaderId = SqlParamHelper.GetBigInt("@headerId", headerId);
            var pstatusCode = SqlParamHelper.GetNVarChar("@statusCode", statusCode);

            var pLoc = SqlParamHelper.GetBigInt("@locatorId", locatorId);
            var pLocatorCode = SqlParamHelper.GetNVarChar("@locatorCode", locatorCode);
            var pCategory = SqlParamHelper.GetNVarChar("@category", categoryCode.GetDesc(Category));
            var pAction = SqlParamHelper.GetNVarChar("@action", actionCode.GetDesc(Action));
            var pDoc = SqlParamHelper.GetNVarChar("@doc", Doc);
            var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
            var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
            var pUser = SqlParamHelper.GetNVarChar("@user", userId, 128);

            this.Context.Database.ExecuteSqlCommand("[SP_SaveOspDetailOut] @headerId, @statusCode, @locatorId, @locatorCode, @category, @action, @doc, @code output, @message output, @user",
                pheaderId, pstatusCode, pLoc, pLocatorCode, pCategory, pAction, pDoc, pCode, pMsg, pUser);
            if (pCode.Value != DBNull.Value)
            {
                resultModel.Code = Convert.ToInt32(pCode.Value);
                resultModel.Success = resultModel.Code == ResultModel.CODE_SUCCESS;
            }

            if (pMsg.Value != DBNull.Value)
            {
                resultModel.Msg = Convert.ToString(pMsg.Value);
            }

            return resultModel;
        }

        /// <summary>
        /// 轉入新增餘切庫存
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="statusCode"></param>
        public ResultModel SaveContangetStock(long headerId, string statusCode, string LocatorId, string Category, string Action, string Doc, string userId)
        {
            var resultModel = new ResultModel(false, "");

            if (!long.TryParse(LocatorId, out long locatorId))
            {
                locatorId = 0;
            }
            var locator = locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == locatorId);
            string locatorCode = "";
            if (locator != null) locatorCode = locator.LocatorSegments;

            var pheaderId = SqlParamHelper.GetBigInt("@headerId", headerId);
            var pstatusCode = SqlParamHelper.GetNVarChar("@statusCode", statusCode);
            var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
            var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
            var pUser = SqlParamHelper.GetNVarChar("@user", userId, 128);
            var pLoc = SqlParamHelper.GetBigInt("@locatorId", locatorId);
            var pLocatorCode = SqlParamHelper.GetNVarChar("@locatorCode", locatorCode);
            var pCategory = SqlParamHelper.GetNVarChar("@category", categoryCode.GetDesc(Category));
            var pAction = SqlParamHelper.GetNVarChar("@action", actionCode.GetDesc(Action));
            var pDoc = SqlParamHelper.GetNVarChar("@doc", Doc);

            this.Context.Database.ExecuteSqlCommand("[SP_SaveCotangentStock] @headerId, @statusCode, @locatorId, @locatorCode, @category, @action, @doc ,@code output, @message output, @user",
                pheaderId, pstatusCode, pLoc, pLocatorCode, pCategory, pAction, pDoc, pCode, pMsg, pUser);

            if (pCode.Value != DBNull.Value)
            {
                resultModel.Code = Convert.ToInt32(pCode.Value);
                resultModel.Success = resultModel.Code == ResultModel.CODE_SUCCESS;
            }

            if (pMsg.Value != DBNull.Value)
            {
                resultModel.Msg = Convert.ToString(pMsg.Value);
            }

            return resultModel;
        }

        /// <summary>
        /// 庫存異動紀錄
        /// </summary>
        /// <param name="Barcode"></param>
        public ResultModel StockRecord(long StockId, decimal PryBefQty , decimal PryAftQty, decimal PryChgQty, decimal SecBefQty, decimal SecAftQty, decimal SecChgQty,
            string Category, string Action, string Doc, string Createdby)
        {
            var resultModel = new ResultModel(false, "");

            var pStockId = SqlParamHelper.GetBigInt("@StockId", StockId);
            var pPryBefQty = SqlParamHelper.GetDecimal("@PRY_BEF_QTY", PryBefQty);
            var pPryAftQty = SqlParamHelper.GetDecimal("@PRY_AFT_QTY", PryAftQty);
            var pPryChgQty = SqlParamHelper.GetDecimal("@PRY_CHG_QTY", PryChgQty);
            var pSecBefQty = SqlParamHelper.GetDecimal("@SEC_BEF_QTY", SecBefQty);
            var pSecAftQty = SqlParamHelper.GetDecimal("@SEC_AFT_QTY", SecAftQty);
            var pSecChgQty = SqlParamHelper.GetDecimal("@SEC_CHG_QTY", SecChgQty);
            var pCategory = SqlParamHelper.GetNVarChar("@CATEGORY", categoryCode.GetDesc(Category));
            var pAction = SqlParamHelper.GetNVarChar("@ACTION", actionCode.GetDesc(Action));
            var pDoc = SqlParamHelper.GetNVarChar("@DOC", Doc);
            var pCreatedby = SqlParamHelper.GetNVarChar("@CREATED_BY", Createdby);
            var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
            var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
            var pUser = SqlParamHelper.GetNVarChar("@user", "", 128);
            Context.Database.ExecuteSqlCommand("[SP_ProcessSaveStkTxn] @StockId, @PRY_BEF_QTY, @PRY_AFT_QTY ,@PRY_CHG_QTY, @SEC_BEF_QTY, @SEC_AFT_QTY,@SEC_CHG_QTY," +
                "@CATEGORY,@ACTION,@DOC,@CREATED_BY,@code output, @message output, @user",
                pStockId, pPryBefQty, pPryAftQty, pPryChgQty, pSecBefQty, pSecAftQty, pSecChgQty, pCategory, pAction, pDoc, pCreatedby, pCode, pMsg, pUser);

            if (pCode.Value != DBNull.Value)
            {
                resultModel.Code = Convert.ToInt32(pCode.Value);
                resultModel.Success = resultModel.Code == ResultModel.CODE_SUCCESS;
            }

            if (pMsg.Value != DBNull.Value)
            {
                resultModel.Msg = Convert.ToString(pMsg.Value);
            }

            return resultModel;
        }

        /// <summary>
        /// 檢查庫存
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userid"></param>
        /// <param name="chg 揀貨量"></param>
        /// <param name="StatusCode"></param>
        public void CheckStock(long StockId, string userid, decimal afterQty, decimal lockQty, string StatusCode)
        {
            var stockid = stockTRepository.Get(x => x.StockId == StockId).SingleOrDefault();
            stockid.PrimaryAvailableQty = afterQty;
            stockid.PrimaryLockedQty = lockQty;
            stockid.StatusCode = StatusCode;
            stockid.LastUpdateBy = userid;
            stockid.LastUpdateDate = DateTime.Now;
            stockTRepository.Update(stockid, true);
        }

        /// <summary>
        /// PICKIN揀貨轉歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void PickInToHt(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"INSERT INTO [OSP_PICKED_IN_HT]
([OSP_PICKED_IN_ID],[OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[HAS_REMAINT],[REMAINING_QUANTITY],[REMAINING_UOM],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_PICKED_IN_ID],[OSP_DETAIL_IN_ID] ,[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[HAS_REMAINT],[REMAINING_QUANTITY] ,[REMAINING_UOM],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM [OSP_PICKED_IN_T]
where OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_PICKED_IN_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// PickOut撿貨轉歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void PirckOutToHt(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"INSERT INTO [OSP_PICKED_OUT_HT]
([OSP_PICKED_OUT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[COTANGENT],[OSP_COTANGENT_ID],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_PICKED_OUT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[COTANGENT],[OSP_COTANGENT_ID],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM　[OSP_PICKED_OUT_T]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_PICKED_OUT_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 投入明細轉歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void DetailInToHt(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"INSERT INTO [OSP_DETAIL_IN_HT]
([OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[PROCESS_CODE] ,[SERVER_CODE],[BATCH_ID],
[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT 
[OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[PROCESS_CODE] ,[SERVER_CODE],[BATCH_ID],
[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE]
FROM [OSP_DETAIL_IN_T]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
DELETE OSP_DETAIL_IN_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }
        /// <summary>
        /// 產出明細轉歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void DetailOutToHt(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"INSERT INTO [OSP_DETAIL_OUT_HT]
([OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
[BATCH_ID],[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT
[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
[BATCH_ID],[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE]
FROM [OSP_DETAIL_OUT_T]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
DELETE OSP_DETAIL_OUT_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 餘切轉歷史明細
        /// </summary>
        public void ContangetToHt(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"INSERT INTO [OSP_COTANGENT_HT]
([OSP_COTANGENT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],
[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_COTANGENT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],
[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM [OSP_COTANGENT_T]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_COTANGENT_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID
");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 損耗轉歷史明細
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void YieldToHt(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"
INSERT INTO [OSP_YIELD_VARIANCE_HT]
([OSP_YIELD_VARIANCE_ID]
,[OSP_HEADER_ID]
      ,[DETAIL_IN_QUANTITY]
      ,[COTANGENT_QUANTITY]
      ,[DETAIL_OUT_QUANTITY]
      ,[LOSS_WEIGHT]
      ,[RATE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_DATE]
      ,[LAST_UPDATE_USER_NAME]
      ,[DETAIL_IN_SECONDARY_QUANTITY]
      ,[DETAIL_IN_PRIMARY_UOM]
      ,[DETAIL_IN_SECONDARY_UOM]
      ,[DETAIL_OUT_PRIMARY_QUANTITY]
      ,[DETAIL_OUT_SECONDARY_QUANTITY]
      ,[DETAIL_OUT_PRIMARY_UOM]
      ,[DETAIL_OUT_SECONDARY_UOM]
      ,[COTANGENT_SECONDARY_QUANTITY]
      ,[COTANGENT_PRIMARY_UOM]
      ,[COTANGENT_SECONDARY_UOM]
      ,[DETAIL_IN_ITEM_NUMBER]
      ,[DETAIL_OUT_ITEM_NUMBER]
      ,[COTANGENT_ITEM_NUMBER])
SELECT
[OSP_YIELD_VARIANCE_ID]
,[OSP_HEADER_ID]
      ,[DETAIL_IN_QUANTITY]
      ,[COTANGENT_QUANTITY]
      ,[DETAIL_OUT_QUANTITY]
      ,[LOSS_WEIGHT]
      ,[RATE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_DATE]
      ,[LAST_UPDATE_USER_NAME]
      ,[DETAIL_IN_SECONDARY_QUANTITY]
      ,[DETAIL_IN_PRIMARY_UOM]
      ,[DETAIL_IN_SECONDARY_UOM]
      ,[DETAIL_OUT_PRIMARY_QUANTITY]
      ,[DETAIL_OUT_SECONDARY_QUANTITY]
      ,[DETAIL_OUT_PRIMARY_UOM]
      ,[DETAIL_OUT_SECONDARY_UOM]
      ,[COTANGENT_SECONDARY_QUANTITY]
      ,[COTANGENT_PRIMARY_UOM]
      ,[COTANGENT_SECONDARY_UOM]
      ,[DETAIL_IN_ITEM_NUMBER]
      ,[DETAIL_OUT_ITEM_NUMBER]
      ,[COTANGENT_ITEM_NUMBER]
FROM [OSP_YIELD_VARIANCE_T]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_YIELD_VARIANCE_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID
");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// PICKIN歷史轉撿貨&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public int CopyPickedInHT(long ospHeaderId)
        {
            return Context.Database.ExecuteSqlCommand(
@"
INSERT INTO [OSP_PICKED_IN_T]
([OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[HAS_REMAINT],[REMAINING_QUANTITY],[REMAINING_UOM],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
T.[OSP_DETAIL_IN_ID], T.OSP_HEADER_ID AS [OSP_HEADER_ID], P.[STOCK_ID] AS [STOCK_ID], P.[BARCODE],
P.[INVENTORY_ITEM_ID], P.[INVENTORY_ITEM_NUMBER], P.[PAPER_TYPE], P.[BASIC_WEIGHT], P.[SPECIFICATION],
P.[LOT_NUMBER], P.[LOT_QUANTITY], P.[PRIMARY_QUANTITY], P.[PRIMARY_UOM], P.[SECONDARY_QUANTITY], P.[SECONDARY_UOM],
P.[HAS_REMAINT], P.[REMAINING_QUANTITY] , P.[REMAINING_UOM], P.[CREATED_BY], P.[CREATED_USER_NAME],
P.[CREATION_DATE], P.[LAST_UPDATE_BY], P.[LAST_UPDATE_DATE], P.[LAST_UPDATE_USER_NAME]
FROM [OSP_PICKED_IN_HT] P
JOIN [OSP_HEADER_MOD_T] M ON M.ORG_OSP_HEADER_ID = P.OSP_HEADER_ID
JOIN [OSP_DETAIL_IN_HT] D ON D.OSP_DETAIL_IN_ID = P.OSP_DETAIL_IN_ID AND D.OSP_HEADER_ID = P.OSP_HEADER_ID
LEFT JOIN [OSP_DETAIL_IN_T] T ON T.OSP_HEADER_ID = M.OSP_HEADER_ID AND T.PROCESS_CODE = D.PROCESS_CODE AND T.SERVER_CODE = D.SERVER_CODE AND T.BATCH_ID = D.BATCH_ID AND T.BATCH_LINE_ID = D.BATCH_LINE_ID
where P.OSP_HEADER_ID = @OSP_HEADER_ID"
                    , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
                );

        }



        /// <summary>
        /// PickOut歷史轉撿貨&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public int CopyPickedOut(long ospHeaderId)
        {

            return Context.Database.ExecuteSqlCommand(
@"
INSERT INTO [OSP_PICKED_OUT_T]
([OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[COTANGENT],[OSP_COTANGENT_ID],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
T.[OSP_DETAIL_OUT_ID], T.OSP_HEADER_ID AS [OSP_HEADER_ID], P.STOCK_ID, P.[BARCODE],
P.[INVENTORY_ITEM_ID], P.[INVENTORY_ITEM_NUMBER], P.[PAPER_TYPE], P.[BASIC_WEIGHT], P.[SPECIFICATION],
P.[LOT_NUMBER], P.[LOT_QUANTITY], P.[PRIMARY_QUANTITY], P.[PRIMARY_UOM], P.[SECONDARY_QUANTITY], P.[SECONDARY_UOM],
P.[STATUS], P.[COTANGENT], P.[OSP_COTANGENT_ID], P.[CREATED_BY], P.[CREATED_USER_NAME],
P.[CREATION_DATE], P.[LAST_UPDATE_BY], P.[LAST_UPDATE_DATE], P.[LAST_UPDATE_USER_NAME]
FROM　[OSP_PICKED_OUT_HT] P
JOIN [OSP_HEADER_MOD_T] M ON M.ORG_OSP_HEADER_ID = P.OSP_HEADER_ID
JOIN [OSP_DETAIL_OUT_HT] D ON D.OSP_DETAIL_OUT_ID = P.OSP_DETAIL_OUT_ID AND D.OSP_HEADER_ID = P.OSP_HEADER_ID
LEFT JOIN [OSP_DETAIL_OUT_T] T ON T.OSP_HEADER_ID = M.OSP_HEADER_ID AND T.PROCESS_CODE = D.PROCESS_CODE AND T.SERVER_CODE = D.SERVER_CODE AND T.BATCH_ID = D.BATCH_ID AND T.BATCH_LINE_ID = D.BATCH_LINE_ID
WHERE P.OSP_HEADER_ID = @OSP_HEADER_ID"
                    , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
                );

        }



        /// <summary>
        /// 投入歷史轉投入明細&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public int CopyDetailInHT(long ospHeaderId)
        {
            return Context.Database.ExecuteSqlCommand(
@"
INSERT INTO [OSP_DETAIL_IN_T]
([OSP_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
[BATCH_ID],[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT 
M.OSP_HEADER_ID AS [OSP_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
[BATCH_ID],[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE]
FROM [OSP_DETAIL_IN_HT] D
JOIN [OSP_HEADER_MOD_T] M ON M.ORG_OSP_HEADER_ID = D.OSP_HEADER_ID
WHERE D.OSP_HEADER_ID = @OSP_HEADER_ID"
                , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
              );


        }



        /// <summary>
        /// 產出歷史轉產出明細&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public int CopyDetailOutHT(long ospHeaderId)
        {
            return Context.Database.ExecuteSqlCommand(
@"
INSERT INTO [OSP_DETAIL_OUT_T]
([OSP_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
[BATCH_ID],[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT
M.OSP_HEADER_ID AS [OSP_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
[BATCH_ID],[BATCH_LINE_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
[INVENTORY_ITEM_NUMBER],[BASIC_WEIGHT],[SPECIFICATION],[GRAIN_DIRECTION],[ORDER_WEIGHT],
[REAM_WT],[PAPER_TYPE],[PACKING_TYPE],[PLAN_QTY],[WIP_PLAN_QTY],
[DTL_UOM],[ORDER_HEADER_ID],[ORDER_NUMBER],[ORDER_LINE_ID],[ORDER_LINE_NUMBER],
[CUSTOMER_ID],[CUSTOMER_NUMBER],[CUSTOMER_NAME],[PR_NUMBER],[PR_LINE_NUMBER],
[REQUISITION_LINE_ID],[PO_NUMBER],[PO_LINE_NUMBER],[PO_LINE_ID],[PO_UNIT_PRICE],
[PO_REVISION_NUM],[PO_STATUS],[PO_VENDOR_NUM],[OSP_REMARK],[SUBINVENTORY],
[LOCATOR_ID],[LOCATOR_CODE],[RESERVATION_UOM_CODE],[RESERVATION_QUANTITY],[LINE_CREATED_BY],
[LINE_CREATION_DATE],[LINE_LAST_UPDATE_BY],[LINE_LAST_UPDATE_DATE],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],[ATTRIBUTE1],
[ATTRIBUTE2],[ATTRIBUTE3],[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],
[ATTRIBUTE7],[ATTRIBUTE8],[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],
[ATTRIBUTE12],[ATTRIBUTE13],[ATTRIBUTE14],[ATTRIBUTE15],[REQUEST_ID],
[CREATED_BY],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE]
FROM [OSP_DETAIL_OUT_HT] D
JOIN [OSP_HEADER_MOD_T] M ON M.ORG_OSP_HEADER_ID = D.OSP_HEADER_ID
WHERE D.OSP_HEADER_ID = @ORG_OSP_HEADER_ID"
                    , new SqlParameter("@ORG_OSP_HEADER_ID", ospHeaderId)
                );

        }



        /// <summary>
        /// 餘切歷史轉餘切明細&刪除歷史
        /// </summary>
        public int CopyContangetHT(long ospHeaderId)
        {
            return Context.Database.ExecuteSqlCommand(
@"
INSERT INTO [OSP_COTANGENT_T]
([OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[LOT_QUANTITY],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],
[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
T.[OSP_DETAIL_OUT_ID], M.[OSP_HEADER_ID], C.[STOCK_ID], C.[BARCODE],
C.[INVENTORY_ITEM_ID], C.[INVENTORY_ITEM_NUMBER], C.[PAPER_TYPE], C.[BASIC_WEIGHT], C.[SPECIFICATION],
C.[LOT_NUMBER], C.[LOT_QUANTITY], C.[PRIMARY_QUANTITY], C.[PRIMARY_UOM], C.[SECONDARY_QUANTITY], C.[SECONDARY_UOM],
C.[STATUS], C.[CREATED_BY], C.[CREATED_USER_NAME], C.[CREATION_DATE], C.[LAST_UPDATE_BY],
C.[LAST_UPDATE_DATE], C.[LAST_UPDATE_USER_NAME]
FROM [OSP_COTANGENT_HT] C
JOIN OSP_HEADER_MOD_T M ON M.ORG_OSP_HEADER_ID = C.OSP_HEADER_ID
JOIN [OSP_DETAIL_OUT_HT] D ON D.OSP_DETAIL_OUT_ID = C.OSP_DETAIL_OUT_ID
LEFT JOIN [OSP_DETAIL_OUT_T] T ON T.OSP_HEADER_ID = M.OSP_HEADER_ID AND T.PROCESS_CODE = D.PROCESS_CODE AND T.SERVER_CODE = D.SERVER_CODE AND T.BATCH_ID = D.BATCH_ID AND T.BATCH_LINE_ID = D.BATCH_LINE_ID
WHERE C.OSP_HEADER_ID = @OSP_HEADER_ID"
                    , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
                );


        }


        /// <summary>
        /// 損耗歷史轉損耗明細&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public int CopyYieldHT(long ospHeaderId)
        {
            return Context.Database.ExecuteSqlCommand(
@"
INSERT INTO [OSP_YIELD_VARIANCE_T]
([OSP_HEADER_ID]
      ,[DETAIL_IN_QUANTITY]
      ,[COTANGENT_QUANTITY]
      ,[DETAIL_OUT_QUANTITY]
      ,[LOSS_WEIGHT]
      ,[RATE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_DATE]
      ,[LAST_UPDATE_USER_NAME]
      ,[DETAIL_IN_SECONDARY_QUANTITY]
      ,[DETAIL_IN_PRIMARY_UOM]
      ,[DETAIL_IN_SECONDARY_UOM]
      ,[DETAIL_OUT_PRIMARY_QUANTITY]
      ,[DETAIL_OUT_SECONDARY_QUANTITY]
      ,[DETAIL_OUT_PRIMARY_UOM]
      ,[DETAIL_OUT_SECONDARY_UOM]
      ,[COTANGENT_SECONDARY_QUANTITY]
      ,[COTANGENT_PRIMARY_UOM]
      ,[COTANGENT_SECONDARY_UOM]
      ,[DETAIL_IN_ITEM_NUMBER]
      ,[DETAIL_OUT_ITEM_NUMBER]
      ,[COTANGENT_ITEM_NUMBER])
SELECT
M.OSP_HEADER_ID
,[DETAIL_IN_QUANTITY]
      ,[COTANGENT_QUANTITY]
      ,[DETAIL_OUT_QUANTITY]
      ,[LOSS_WEIGHT]
      ,[RATE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_DATE]
      ,[LAST_UPDATE_USER_NAME]
      ,[DETAIL_IN_SECONDARY_QUANTITY]
      ,[DETAIL_IN_PRIMARY_UOM]
      ,[DETAIL_IN_SECONDARY_UOM]
      ,[DETAIL_OUT_PRIMARY_QUANTITY]
      ,[DETAIL_OUT_SECONDARY_QUANTITY]
      ,[DETAIL_OUT_PRIMARY_UOM]
      ,[DETAIL_OUT_SECONDARY_UOM]
      ,[COTANGENT_SECONDARY_QUANTITY]
      ,[COTANGENT_PRIMARY_UOM]
      ,[COTANGENT_SECONDARY_UOM]
      ,[DETAIL_IN_ITEM_NUMBER]
      ,[DETAIL_OUT_ITEM_NUMBER]
      ,[COTANGENT_ITEM_NUMBER]
FROM [OSP_YIELD_VARIANCE_HT] Y
JOIN OSP_HEADER_MOD_T M ON M.ORG_OSP_HEADER_ID = Y.OSP_HEADER_ID
WHERE Y.OSP_HEADER_ID = @OSP_HEADER_ID
"
                    , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
                );


        }


        /// <summary>
        /// 歷史轉修改資料庫存資料刪除
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public int DeleteStock(long ospHeaderId, string userId)
        {
            return DeleteStockFromOspPickedHT(ospHeaderId, userId) + DeleteStockFromOspContangetHT(ospHeaderId, userId);
        }

        /// <summary>
        /// 刪除庫存來自撿貨歷史表格
        /// </summary>
        /// <param name="ospHeaderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteStockFromOspPickedHT(long ospHeaderId, string userId)
        {
            int createStockRecord = Context.Database.ExecuteSqlCommand(
            @"INSERT INTO[dbo].[STK_TXN_T]
            ([STOCK_ID],[ORGANIZATION_ID],[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
       ,[LOCATOR_ID],[DST_ORGANIZATION_ID],[DST_ORGANIZATION_CODE],[DST_SUBINVENTORY_CODE],[DST_LOCATOR_ID]
       ,[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION],[ITEM_CATEGORY],[LOT_NUMBER]
       ,[BARCODE],[PRY_UOM_CODE],[PRY_BEF_QTY],[PRY_AFT_QTY],[PRY_CHG_QTY]
       ,[SEC_UOM_CODE],[SEC_BEF_QTY],[SEC_CHG_QTY],[SEC_AFT_QTY],[CATEGORY]
       ,[DOC],[ACTION],[NOTE],[STATUS_CODE],[CREATED_BY]
       ,[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT T.[STOCK_ID],T.[ORGANIZATION_ID] ,T.[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
        ,T.[LOCATOR_ID],''  ,'','' ,''
        ,T.[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION] ,T.[ITEM_CATEGORY] ,T.[LOT_NUMBER]
        ,T.[BARCODE],PRIMARY_UOM_CODE ,PRIMARY_AVAILABLE_QTY,0,-1*PRIMARY_AVAILABLE_QTY
        ,SECONDARY_UOM_CODE,SECONDARY_AVAILABLE_QTY,-1*SECONDARY_AVAILABLE_QTY,0,@CATEGORY
        ,H.BATCH_NO,@ACTION,T.[NOTE],[STATUS_CODE],@CREATED_BY
        ,GETDATE(),NULL,NULL
FROM STOCK_T T
JOIN OSP_PICKED_OUT_HT POH ON POH.STOCK_ID = T.STOCK_ID
JOIN OSP_HEADER_T H ON H.OSP_HEADER_ID = POH.OSP_HEADER_ID
WHERE POH.OSP_HEADER_ID = @OSP_HEADER_ID",
            new SqlParameter("@OSP_HEADER_ID", ospHeaderId),
            new SqlParameter("@CATEGORY", categoryCode.GetDesc(CategoryCode.Process)),
            new SqlParameter("@ACTION", actionCode.GetDesc(ActionCode.ProcessEditDeleted)),
            new SqlParameter("@CREATED_BY", userId));

            int delStock = Context.Database.ExecuteSqlCommand(
@"DELETE ST FROM STOCK_T ST
JOIN OSP_PICKED_OUT_HT POH ON POH.STOCK_ID = ST.STOCK_ID
WHERE POH.OSP_HEADER_ID = @OSP_HEADER_ID"
                    , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
                );

            return createStockRecord + delStock;
        }

        /// <summary>
        /// 刪除庫存來自餘切歷史紀錄
        /// </summary>
        /// <param name="ospHeaderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteStockFromOspContangetHT(long ospHeaderId, string userId)
        {
            int createStockRecord = Context.Database.ExecuteSqlCommand(
            @"INSERT INTO[dbo].[STK_TXN_T]
            ([STOCK_ID],[ORGANIZATION_ID],[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
       ,[LOCATOR_ID],[DST_ORGANIZATION_ID],[DST_ORGANIZATION_CODE],[DST_SUBINVENTORY_CODE],[DST_LOCATOR_ID]
       ,[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION],[ITEM_CATEGORY],[LOT_NUMBER]
       ,[BARCODE],[PRY_UOM_CODE],[PRY_BEF_QTY],[PRY_AFT_QTY],[PRY_CHG_QTY]
       ,[SEC_UOM_CODE],[SEC_BEF_QTY],[SEC_CHG_QTY],[SEC_AFT_QTY],[CATEGORY]
       ,[DOC],[ACTION],[NOTE],[STATUS_CODE],[CREATED_BY]
       ,[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT T.[STOCK_ID],T.[ORGANIZATION_ID] ,T.[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
        ,T.[LOCATOR_ID],''  ,'','' ,''
        ,T.[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION] ,T.[ITEM_CATEGORY] ,T.[LOT_NUMBER]
        ,T.[BARCODE],PRIMARY_UOM_CODE ,PRIMARY_AVAILABLE_QTY,0,-1*PRIMARY_AVAILABLE_QTY
        ,SECONDARY_UOM_CODE,SECONDARY_AVAILABLE_QTY,-1*SECONDARY_AVAILABLE_QTY,0,@CATEGORY
        ,H.BATCH_NO,@ACTION,T.[NOTE],[STATUS_CODE],@CREATED_BY
        ,GETDATE(),NULL,NULL
FROM STOCK_T T
JOIN OSP_COTANGENT_HT CHT ON CHT.STOCK_ID = T.STOCK_ID
JOIN OSP_HEADER_T H ON H.OSP_HEADER_ID = CHT.OSP_HEADER_ID
WHERE CHT.OSP_HEADER_ID = @OSP_HEADER_ID",
            new SqlParameter("@OSP_HEADER_ID", ospHeaderId),
            new SqlParameter("@CATEGORY", categoryCode.GetDesc(CategoryCode.Process)),
            new SqlParameter("@ACTION", actionCode.GetDesc(ActionCode.ProcessEditDeleted)),
            new SqlParameter("@CREATED_BY", userId));

            int delStock = Context.Database.ExecuteSqlCommand(
@"DELETE ST FROM STOCK_T ST
JOIN OSP_COTANGENT_HT CHT ON CHT.STOCK_ID = ST.STOCK_ID
WHERE CHT.OSP_HEADER_ID = @OSP_HEADER_ID"
                    , new SqlParameter("@OSP_HEADER_ID", ospHeaderId)
                );

            return createStockRecord + delStock;
        }


        /// <summary>
        ///  補印標籤
        /// </summary>
        /// <param name="stockIds"></param>
        /// <param name="userName"></param>
        /// <param name="ItemCategory"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> RePrintLabel(List<long> stockIds, string userName, string ItemCategory)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (stockIds == null || stockIds.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < stockIds.Count; i++)
                {
                    StringBuilder cmd = new StringBuilder();
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    if (ItemCategory == "平版")
                    {
                        cmd.Append(
@"
SELECT 
CAST(ST.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(ST.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(ST.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(ST.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(ST.SECONDARY_TRANSACTION_QTY,'0.##########') AS nvarchar) AS Qty,
CAST(ST.SECONDARY_UOM_CODE AS nvarchar) AS Unit,
CAST(ST.OSP_BATCH_NO AS nvarchar) AS OspBatchNo
--CAST(CT.CONTAINER_NO AS nvarchar) AS BatchNo
FROM STOCK_T ST
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = ST.INVENTORY_ITEM_ID
WHERE ST.ITEM_CATEGORY = N'平版'
AND ST.STOCK_ID = @STOCK_ID
");
                    }
                    else
                    {
                        cmd.Append(
@"
SELECT 
CAST(ST.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(ST.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(ST.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(ST.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(ST.PRIMARY_TRANSACTION_QTY,'0.##########') AS nvarchar) AS Qty,
CAST(ST.PRIMARY_UOM_CODE AS nvarchar) AS Unit,
CAST(ST.LOT_NUMBER AS nvarchar) as LotNumber
--CAST(CT.CONTAINER_NO AS nvarchar) AS BatchNo
FROM STOCK_T ST
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = ST.INVENTORY_ITEM_ID
WHERE ST.ITEM_CATEGORY = N'捲筒'
AND ST.STOCK_ID = @STOCK_ID
");
                    }

                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@STOCK_ID", stockIds[i]));
                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), sqlParameterList.ToArray()).SingleOrDefault(); ;
                    if (labelModel == null) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                    labelModelList.Add(labelModel);
                }
                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);

            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }


        }

        /// <summary>
        /// 列印產出成品標籤
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GeProductLabels(List<long> OspPickedOutId, string userName)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (OspPickedOutId == null || OspPickedOutId.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < OspPickedOutId.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder(
@"
SELECT 
CAST(OPO.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(OPO.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(OPO.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(OPO.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(OPO.SECONDARY_QUANTITY,'0.##########') AS nvarchar) AS Qty,
CAST(OPO.SECONDARY_UOM AS nvarchar) AS Unit,
CAST(OH.BATCH_NO AS nvarchar) AS OspBatchNo
--CAST(OH.BATCH_NO AS nvarchar) AS BatchNo
FROM OSP_PICKED_OUT_T OPO
join OSP_HEADER_T OH ON OH.OSP_HEADER_ID = OPO.OSP_HEADER_ID
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = OPO.INVENTORY_ITEM_ID
AND OPO.OSP_PICKED_OUT_ID = @OSP_PICKED_OUT_ID
");
                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@OSP_PICKED_OUT_ID", OspPickedOutId[i]));
                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), sqlParameterList.ToArray()).SingleOrDefault();
                    if (labelModel == null) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                    labelModelList.Add(labelModel);
                }
                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);


            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }


        }

        /// <summary>
        /// 列印餘切成品標籤
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GeCotangentLabels(List<long> OspCotangentId, string userName)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (OspCotangentId == null || OspCotangentId.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < OspCotangentId.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder(
@"
SELECT 
CAST(OCT.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(OCT.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(OCT.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(OCT.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(OCT.SECONDARY_QUANTITY,'0.##########') AS nvarchar) AS Qty,
CAST(OCT.SECONDARY_UOM AS nvarchar) AS Unit,
CAST(OH.BATCH_NO AS nvarchar) AS OspBatchNo
--CAST(OH.BATCH_NO AS nvarchar) AS BatchNo
FROM OSP_COTANGENT_T OCT
join OSP_HEADER_T OH ON OH.OSP_HEADER_ID = OCT.OSP_HEADER_ID
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = OCT.INVENTORY_ITEM_ID
AND OCT.OSP_COTANGENT_ID = @OSP_COTANGENT_ID
");
                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@OSP_COTANGENT_ID", OspCotangentId[i]));
                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), sqlParameterList.ToArray()).SingleOrDefault();
                    if (labelModel == null) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                    labelModelList.Add(labelModel);
                }
                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);


            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }


        }

        /// <summary>
        /// 列印代紙紙捲產出成品標籤
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GePaperRollerProductLabels(List<long> OspPickedOutId, string userName)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (OspPickedOutId == null || OspPickedOutId.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < OspPickedOutId.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder(
@"
SELECT 
CAST(OPO.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(OPO.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(OPO.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(OPO.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(OPO.PRIMARY_QUANTITY,'0.##########') AS nvarchar) AS Qty,
CAST(OPO.PRIMARY_UOM AS nvarchar) AS Unit,
CAST(OPO.LOT_NUMBER AS nvarchar) as LotNumber
--CAST(OH.BATCH_NO AS nvarchar) AS BatchNo
FROM OSP_PICKED_OUT_T OPO
join OSP_HEADER_T OH ON OH.OSP_HEADER_ID = OPO.OSP_HEADER_ID
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = OPO.INVENTORY_ITEM_ID
AND OPO.OSP_PICKED_OUT_ID = @OSP_PICKED_OUT_ID
");
                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@OSP_PICKED_OUT_ID", OspPickedOutId[i]));
                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), sqlParameterList.ToArray()).SingleOrDefault();
                    if (labelModel == null) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                    labelModelList.Add(labelModel);
                }
                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);


            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }


        }

        /// <summary>
        /// 取得工單號
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBatchNo()
        {
            List<SelectListItem> BatchNo = new List<SelectListItem>();
            BatchNo.Add(new SelectListItem
            {
                Text = "全部",
                Value = "*",
            });

            var batchNo = OspHeaderTRepository.GetAll().GroupBy(x => x.BatchNo)
                  .Select(x => new SelectListItem
                  {
                      Text = x.Key,
                      Value = x.Key

                  }).ToList();

            BatchNo.AddRange(batchNo);
            return BatchNo;
        }

        /// <summary>
        /// 取得儲位
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocator(long OspDetailOutId, string UserId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    var DetailOut = OspDetailOutTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                    return getLocatorListForUserId(UserId, DetailOut.Subinventory, DetailOut.LocatorId);

                    //                    List<SelectListItem> locator = new List<SelectListItem>();
                    //                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    //                    StringBuilder query = new StringBuilder();
                    //                    query.Append(
                    //@"SELECT 
                    //[SEGMENT3] as Text,
                    //cast([LOCATOR_ID] as nvarchar) as Value
                    //FROM [LOCATOR_T] LT
                    //where CONTROL_FLAG <> 'D'
                    //and SUBINVENTORY_CODE = @SUBINVENTORY_CODE");
                    //                    sqlParameterList.Add(new SqlParameter("@SUBINVENTORY_CODE", DetailOut.Subinventory));
                    //                    var data = mesContext.Database.SqlQuery<SelectListItem>(query.ToString(), sqlParameterList.ToArray()).ToList();
                    //                    if (data == null)
                    //                    {
                    //                        locator.AddRange(data);
                    //                    }
                    //                    else
                    //                    {
                    //                        locator.AddRange(data);
                    //                    }
                    //                    return locator;
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// 取得機台
        /// </summary>
        /// <param name="PaperType"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSelectMachine(string PaperType)
        {
            try
            {
                List<SelectListItem> ManchineNum = new List<SelectListItem>();
                var MachineCode = machinePaperTypeRepository.Get(x => x.ControlFlag != ControlFlag.Deleted && x.PaperType == PaperType)
                             .Select(x => new SelectListItem
                             {
                                 Text = x.MachineNum,
                                 Value = x.MachineNum
                             }).ToList();
                ManchineNum.AddRange(MachineCode);


                return ManchineNum;
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// 取得工單號狀態
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBatchStatusDesc()
        {
            List<SelectListItem> BatchStatusDesc = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "全部",
                    Value = "*",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "待排單",
                    Value = "0",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "已排單",
                    Value = "1",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "待核准",
                    Value = "2",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "已完工",
                    Value = "3",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "關帳",
                    Value = "4",
                    Selected = false,
                },
                 new SelectListItem()
                {
                    Text = "已取消",
                    Value = "6",
                    Selected = false,
                }
            };
            return BatchStatusDesc;
        }

        /// <summary>
        /// 取得權限
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Boolean GetAuthority(List<Claim> roles)
        {
            var boolean = false;
            try
            {
                if (roles != null && roles.Count > 0)
                {
                    foreach (Claim role in roles)
                    {
                        if (role.Value == UserRole.Adm || role.Value == UserRole.ChpUser)
                        {
                            boolean = true;
                            break;
                        }
                        else
                        {
                            boolean = false;
                        }
                    }
                }
                return boolean;
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return boolean;
            }

        }

        public class DetailDTEditor
        {
            public string Action { get; set; }
            public List<Invest> InvestList { get; set; }
        }

        public class ProductionDTEditor
        {
            public string Action { get; set; }
            public List<Production> ProductionList { get; set; }
        }

        public class CotangentDTEditor
        {
            public string Action { get; set; }
            public List<Cotangent> CotangentList { get; set; }
        }


        public class ProcessStatusCode : IStatus
        {
            /// <summary>
            /// 待排單
            /// </summary>
            public const string WaitBatch = "0";
            /// <summary>
            /// 已排單
            /// </summary>
            public const string DwellBatch = "1";
            /// <summary>
            /// 待核准
            /// </summary>
            public const string PendingBatch = "2";
            /// <summary>
            /// 已完工
            /// </summary>
            public const string CompletedBatch = "3";
            /// <summary>
            /// 關帳
            /// </summary>
            public const string CloseBatch = "4";
            /// <summary>
            /// 已修改
            /// </summary>
            public const string Modified = "5";
            /// <summary>
            /// 已取消
            /// </summary>
            public const string Canceled = "6";

            public string GetDesc(string statusCode)
            {
                switch (statusCode)
                {
                    case WaitBatch:
                        return "待排單";
                    case DwellBatch:
                        return "已排單";
                    case PendingBatch:
                        return "待核准";
                    case CompletedBatch:
                        return "已完工";
                    case CloseBatch:
                        return "關帳";
                    case Modified:
                        return "已修改";
                    case Canceled:
                        return "已取消";
                    default:
                        return "";
                }
            }
        }


        /// <summary>
        /// 取得領料單報表
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="OspHeaderId"></param>
        public void OspMaterial(ref ReportDataSource Header, string OspHeaderId)
        {

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();

                    if (!long.TryParse(OspHeaderId, out long ospHeaderId))
                    {
                        throw new ArgumentException("HEADER ID ERROR");
                    }

                    DataSet dataset = new DataSet("OSP");
                    string Header1 = "SELECT * from OspCutMaterial(@OSP_HEADER_ID)";
                    SqlCommand command = new SqlCommand(Header1, connection);
                    command.Parameters.Add(new SqlParameter("@OSP_HEADER_ID", ospHeaderId) { DbType = DbType.Int64 });
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "DataSet1");
                    Header.Name = "DataSet1";
                    Header.Value = dataset.Tables["DataSet1"];

                    connection.Close();
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                }
            }

        }


        /// <summary>
        /// 取得外場P裁切單報表
        /// </summary>
        /// <param name="Detail"></param>
        /// <param name="LabelKnife"></param>
        /// <param name="LabelDesc"></param>
        /// <param name="LabelSize"></param>
        /// <param name="OspHeaderId"></param>
        public void OspCutReceiptReport(ref ReportDataSource Detail, ref ReportDataSource LabelKnife, ref ReportDataSource LabelDesc, ref ReportDataSource LabelSize, string OspHeaderId)
        {

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    if (!long.TryParse(OspHeaderId, out long ospHeaderId))
                    {
                        throw new ArgumentException("HEADER ID ERROR");
                    }

                    var osp = OspDetailOutTRepository.Get(x => x.OspHeaderId == ospHeaderId).FirstOrDefault();

                    DataSet dataset = new DataSet("Receipt");
                    GetDetail(connection, ref dataset, ospHeaderId);
                    Detail.Name = "DataSet1";
                    Detail.Value = dataset.Tables["DataSet1"];


                    GetLabelKnife(connection, ref dataset, osp.Specification);
                    LabelKnife.Name = "LabelKnife";
                    LabelKnife.Value = dataset.Tables["LabelKnife"];


                    GetLabelDesc(connection, ref dataset, osp.PackingType ?? "");
                    LabelDesc.Name = "LabelDesc";
                    LabelDesc.Value = dataset.Tables["LabelDesc"];

                    GetLabelSize(connection, ref dataset, osp.Specification);
                    LabelSize.Name = "LabelSize";
                    LabelSize.Value = dataset.Tables["LabelSize"];


                    connection.Close();
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                }
            }

        }

        /// <summary>
        /// 取得裁切單資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dsDetail"></param>
        /// <param name="ospHeaderId"></param>
        public void GetDetail(SqlConnection connection, ref DataSet dsDetail, long ospHeaderId)
        {
            string Header = "SELECT * FROM dbo.OspOutSourcCut(@OSP_HEADER_ID)";
            SqlCommand command = new SqlCommand(Header, connection);
            command.Parameters.Add(new SqlParameter("@OSP_HEADER_ID", ospHeaderId) { DbType = DbType.Int64 });
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(dsDetail, "DataSet1");
        }

        /// <summary>
        /// 取得標籤園刀資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="LabelKnife"></param>
        /// <param name="spec"></param>
        public void GetLabelKnife(SqlConnection connection, ref DataSet LabelKnife, string spec)
        {
            string Header = "SELECT dbo.OspLabelKnife(dbo.CheckOspLabelSpec(@SPECIFICATION)) AS Knife";
            SqlCommand command = new SqlCommand(Header, connection);
            command.Parameters.Add(new SqlParameter("@SPECIFICATION", spec));
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(LabelKnife, "LabelKnife");
        }

        /// <summary>
        /// 取得標籤摘要
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="LabelDesc"></param>
        /// <param name="packingType"></param>
        public void GetLabelDesc(SqlConnection connection, ref DataSet LabelDesc, string packingType)
        {
            string Header = "select dbo.ConvertOspLabelDesc(@packingType) as LabelDesc";
            SqlCommand command = new SqlCommand(Header, connection);
            command.Parameters.Add(new SqlParameter("@packingType", packingType) { IsNullable = true });
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(LabelDesc, "LabelDesc");
        }

        /// <summary>
        /// 取得標籤大小
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="LabelSize"></param>
        /// <param name="spec"></param>
        public void GetLabelSize(SqlConnection connection, ref DataSet LabelSize, string spec)
        {
            string Header = "select dbo.CheckOspLabelSize(dbo.CheckOspLabelSpec(@SPECIFICATION)) as LabelSize";
            SqlCommand command = new SqlCommand(Header, connection);
            command.Parameters.Add(new SqlParameter("@SPECIFICATION", spec));
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(LabelSize, "LabelSize");
        }


        /// <summary>
        /// 成品入庫報表
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="OspHeaderId"></param>
        public void OspStock(ref ReportDataSource stock, ref ReportDataSource Countangent, ref ReportDataSource Remain, string OspHeaderId)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    if (!long.TryParse(OspHeaderId, out long ospHeaderId))
                    {
                        throw new ArgumentException("HEADER ID ERROR");
                    }
                    DataSet dataset = new DataSet("stock");
                    OspStockData(connection, ref dataset, ospHeaderId);
                    stock.Name = "DataSet1";
                    stock.Value = dataset.Tables["DataSet1"];
                    OspStockCotangentData(connection, ref dataset, ospHeaderId);
                    Countangent.Name = "DataSet2";
                    Countangent.Value = dataset.Tables["DataSet2"];
                    OspStockRemainData(connection, ref dataset, ospHeaderId);
                    Remain.Name = "DataSet3";
                    Remain.Value = dataset.Tables["DataSet3"];
                    connection.Close();
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                }
            }
        }

        /// <summary>
        /// 取得成品入庫資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dsDetail"></param>
        /// <param name="ospHeaderId"></param>
        public void OspStockData(SqlConnection connection, ref DataSet dsDetail, long ospHeaderId)
        {
            string Header1 = "select * from OspCutStock(@OSP_HEADER_ID)";
            SqlCommand command = new SqlCommand(Header1, connection);
            command.Parameters.Add(new SqlParameter("@OSP_HEADER_ID", ospHeaderId) { DbType = DbType.Int64 });
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(dsDetail, "DataSet1");
        }

        /// <summary>
        /// 取得成品入庫產出資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dsCotangent"></param>
        /// <param name="ospHeaderId"></param>
        public void OspStockCotangentData(SqlConnection connection, ref DataSet dsCotangent, long ospHeaderId)
        {
            string Header1 = "select * from OspCutCotangentStock(@OSP_HEADER_ID)";
            SqlCommand command = new SqlCommand(Header1, connection);
            command.Parameters.Add(new SqlParameter("@OSP_HEADER_ID", ospHeaderId) { DbType = DbType.Int64 });
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(dsCotangent, "DataSet2");
        }

        /// <summary>
        /// 取得成品入庫殘捲資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Remain"></param>
        /// <param name="ospHeaderId"></param>
        public void OspStockRemainData(SqlConnection connection, ref DataSet Remain, long ospHeaderId)
        {
            string Header1 = "select * from OspRemainQty(@OSP_HEADER_ID)";
            SqlCommand command = new SqlCommand(Header1, connection);
            command.Parameters.Add(new SqlParameter("@OSP_HEADER_ID", ospHeaderId) { DbType = DbType.Int64 });
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(Remain, "DataSet3");
        }

        /// <summary>
        /// 加工代紙紙捲成品入庫
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="OspHeaderId"></param>
        public void OspPaperRollerStock(ref ReportDataSource stock, string OspHeaderId)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    if (!long.TryParse(OspHeaderId, out long ospHeaderId))
                    {
                        throw new ArgumentException("HEADER ID ERROR");
                    }
                    DataSet dataset = new DataSet("PaperRollerstock");
                    string Header1 = "SELECT * FROM OspCutStock(@OSP_HEADER_ID)";
                    SqlCommand command = new SqlCommand(Header1, connection);
                    command.Parameters.Add(new SqlParameter("@OSP_HEADER_ID", ospHeaderId) { DbType = DbType.Int64 });
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "DataSet1");
                    stock.Name = "DataSet1";
                    stock.Value = dataset.Tables["DataSet1"];

                    connection.Close();
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                }
            }

        }

        /// <summary>
        /// 取得工單得率報表資料
        /// </summary>
        /// <param name="cuttingDateFrom"></param>
        /// <param name="cuttingDateTo"></param>
        /// <param name="batchNo"></param>
        /// <param name="machineNum"></param>
        /// <param name="itemNumber"></param>
        /// <param name="barcode"></param>
        /// <param name="subinventory"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<ReportDataSource> GetOspYieldReportDataSource(string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum, string itemNumber, string barcode, string subinventory, string userId)
        {

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    string cmd = @"EXEC dbo.SP_OspYieldReport 
@cuttingDateFrom, @cuttingDateTo, @batchNo, @machineNum, @itemNumber, @barcode, @subinventory, @dateFormStatus, @dateToStatus, @code, @message, @user
";
                    connection.Open();
                    SqlCommand command = new SqlCommand(cmd, connection);
                    DataSet dataset = new DataSet("OSP");
                    command.Parameters.AddRange(GetOspYieldSqlParameterList(cuttingDateFrom, cuttingDateTo, batchNo, machineNum, itemNumber, barcode, subinventory, userId).ToArray());
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "DataSet1");
                    ReportDataSource dataSource = new ReportDataSource();
                    dataSource.Name = "DataSet1";
                    dataSource.Value = dataset.Tables["DataSet1"];

                    connection.Close();

                    return new ResultDataModel<ReportDataSource>(true, "取得工單得率報表資料來源成功", dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    return new ResultDataModel<ReportDataSource>(false, "取得工單得率報表資料來源失敗:" + ex.Message, null);
                }
            }

        }

        /// <summary>
        /// 裁切資料匯總報
        /// </summary>
        /// <param name="planStartDateFrom"></param>
        /// <param name="planStartDateTo"></param>
        /// <param name="batchNo"></param>
        /// <param name="paperType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<ReportDataSource> GetOspCutSumReportDataSource(string planStartDateFrom, string planStartDateTo, string batchNo, string paperType, string userId)
        {

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    string cmd = @"EXEC dbo.SP_OspCutSumReport 
@planStartDateFrom, @planStartDateTo, @batchNo, @paperType, @dateFormStatus, @dateToStatus, @code, @message, @user
";
                    connection.Open();
                    SqlCommand command = new SqlCommand(cmd, connection);
                    DataSet dataset = new DataSet("OSP");
                    command.Parameters.AddRange(GetOspCutSumSqlParameterList(planStartDateFrom, planStartDateTo, batchNo, paperType, userId).ToArray());
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "DataSet1");
                    ReportDataSource dataSource = new ReportDataSource();
                    dataSource.Name = "DataSet1";
                    dataSource.Value = dataset.Tables["DataSet1"];

                    connection.Close();

                    return new ResultDataModel<ReportDataSource>(true, "取得裁切資料彙總報表資料來源成功", dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    return new ResultDataModel<ReportDataSource>(false, "取得裁切資料彙總報表資料來源失敗:" + ex.Message, null);
                }
            }

        }

        /// <summary>
        /// 取得工單得率報表時間範圍資料
        /// </summary>
        /// <param name="cuttingDateFrom"></param>
        /// <param name="cuttingDateTo"></param>
        /// <param name="batchNo"></param>
        /// <param name="machineNum"></param>
        /// <param name="itemNumber"></param>
        /// <param name="barcode"></param>
        /// <param name="subinventory"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SqlParameter> GetOspYieldSqlParameterList(string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum, string itemNumber, string barcode, string subinventory, string userId)
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            string sDateFormStatus = "1";
            string sDateToStatus = "1";

            if (!string.IsNullOrEmpty(cuttingDateTo))
            {
                cuttingDateTo += " 23:59:59";
            }

            var dateFormStatus = DateTime.TryParseExact(cuttingDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateFrom);
            var dateToStatus = DateTime.TryParseExact(cuttingDateTo, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateTo);
            if (!dateFormStatus)
            {
                cuttingDateFrom = "1900-01-01";
                sDateFormStatus = "0";
            }
            if (!dateToStatus)
            {
                cuttingDateTo = "9999-12-31 23:59:59";
                sDateToStatus = "0";
            }

            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@cuttingDateFrom", cuttingDateFrom, 30));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@cuttingDateTo", cuttingDateTo, 30));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@batchNo", batchNo, 32));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@machineNum", machineNum, 30));
            sqlParameterList.Add(SqlParamHelper.R.ItemNo("@itemNumber", itemNumber));
            sqlParameterList.Add(SqlParamHelper.R.Barcode("@barcode", barcode));
            sqlParameterList.Add(SqlParamHelper.R.SubinventoryCode("@subinventory", subinventory));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@dateFormStatus", sDateFormStatus, 1));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@dateToStatus", sDateToStatus, 1));
            sqlParameterList.Add(SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output));
            sqlParameterList.Add(SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@user", userId, 128));

            return sqlParameterList;
        }

        /// <summary>
        /// 裁切資料匯總報表時間範圍資料
        /// </summary>
        /// <param name="planStartDateFrom"></param>
        /// <param name="planStartDateTo"></param>
        /// <param name="batchNo"></param>
        /// <param name="paperType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SqlParameter> GetOspCutSumSqlParameterList(string planStartDateFrom, string planStartDateTo, string batchNo, string paperType, string userId)
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            string sDateFormStatus = "1";
            string sDateToStatus = "1";

            if (!string.IsNullOrEmpty(planStartDateTo))
            {
                planStartDateTo += " 23:59:59";
            }

            var dateFormStatus = DateTime.TryParseExact(planStartDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateFrom);
            var dateToStatus = DateTime.TryParseExact(planStartDateTo, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateTo);
            if (!dateFormStatus)
            {
                planStartDateFrom = "1900-01-01";
                sDateFormStatus = "0";
            }
            if (!dateToStatus)
            {
                planStartDateTo = "9999-12-31 23:59:59";
                sDateToStatus = "0";
            }

            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@planStartDateFrom", planStartDateFrom, 30));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@planStartDateTo", planStartDateTo, 30));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@batchNo", batchNo, 32));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@paperType", paperType, 4));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@dateFormStatus", sDateFormStatus, 1));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@dateToStatus", sDateToStatus, 1));
            sqlParameterList.Add(SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output));
            sqlParameterList.Add(SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output));
            sqlParameterList.Add(SqlParamHelper.GetVarChar("@user", userId, 128));

            return sqlParameterList;
        }

        /// <summary>
        /// 報表時間範圍資料參數
        /// </summary>
        /// <param name="cuttingDateFrom"></param>
        /// <param name="cuttingDateTo"></param>
        /// <param name="batchNo"></param>
        /// <param name="machineNum"></param>
        /// <param name="itemNumber"></param>
        /// <param name="barcode"></param>
        /// <param name="subinventory"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ReportParameter> GetOspYieldReportParameterList(string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum, string itemNumber, string barcode, string subinventory, string userId)
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            string sDateFormStatus = "1";
            string sDateToStatus = "1";

            if (!string.IsNullOrEmpty(cuttingDateTo))
            {
                cuttingDateTo += " 23:59:59";
            }

            var dateFormStatus = DateTime.TryParseExact(cuttingDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateFrom);
            var dateToStatus = DateTime.TryParseExact(cuttingDateTo, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateTo);
            if (!dateFormStatus)
            {
                cuttingDateFrom = "1900-01-01";
                sDateFormStatus = "0";
            }
            if (!dateToStatus)
            {
                cuttingDateTo = "9999-12-31 23:59:59";
                sDateToStatus = "0";
            }
            
            List<ReportParameter> reportParameterList = new List<ReportParameter>();
            reportParameterList.Add(new ReportParameter("cuttingDateFrom", cuttingDateFrom, false));
            reportParameterList.Add(new ReportParameter("cuttingDateTo", cuttingDateTo, false));
            reportParameterList.Add(new ReportParameter("batchNo", batchNo, false));
            reportParameterList.Add(new ReportParameter("machineNum", machineNum, false));
            reportParameterList.Add(new ReportParameter("itemNumber", itemNumber, false));
            reportParameterList.Add(new ReportParameter("barcode", barcode, false));
            reportParameterList.Add(new ReportParameter("subinventory", subinventory, false));
            reportParameterList.Add(new ReportParameter("dateFormStatus", sDateFormStatus, false));
            reportParameterList.Add(new ReportParameter("dateToStatus", sDateToStatus, false));
            reportParameterList.Add(new ReportParameter("code", "0", false));
            reportParameterList.Add(new ReportParameter("message", "", false));
            reportParameterList.Add(new ReportParameter("user", userId, false));

            return reportParameterList;
        }

        /// <summary>
        /// 裁切資料匯總報資料參數
        /// </summary>
        /// <param name="planStartDateFrom"></param>
        /// <param name="planStartDateTo"></param>
        /// <param name="batchNo"></param>
        /// <param name="paperType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ReportParameter> GetOspCutSumReportParameterList(string planStartDateFrom, string planStartDateTo, string batchNo, string paperType, string userId)
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            string sDateFormStatus = "1";
            string sDateToStatus = "1";

            if (!string.IsNullOrEmpty(planStartDateTo))
            {
                planStartDateTo += " 23:59:59";
            }

            var dateFormStatus = DateTime.TryParseExact(planStartDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateFrom);
            var dateToStatus = DateTime.TryParseExact(planStartDateTo, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateTo);
            if (!dateFormStatus)
            {
                planStartDateFrom = "1900-01-01";
                sDateFormStatus = "0";
            }
            if (!dateToStatus)
            {
                planStartDateTo = "9999-12-31 23:59:59";
                sDateToStatus = "0";
            }

            List<ReportParameter> reportParameterList = new List<ReportParameter>();
            reportParameterList.Add(new ReportParameter("planStartDateFrom", planStartDateFrom, false));
            reportParameterList.Add(new ReportParameter("planStartDateTo", planStartDateTo, false));
            reportParameterList.Add(new ReportParameter("batchNo", batchNo, false));
            reportParameterList.Add(new ReportParameter("paperType", paperType, false));
            reportParameterList.Add(new ReportParameter("dateFormStatus", sDateFormStatus, false));
            reportParameterList.Add(new ReportParameter("dateToStatus", sDateToStatus, false));
            reportParameterList.Add(new ReportParameter("code", "0", false));
            reportParameterList.Add(new ReportParameter("message", "", false));
            reportParameterList.Add(new ReportParameter("user", userId, false));

            return reportParameterList;
        }

        /// <summary>
        /// 取得明細產出資料
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public OSP_DETAIL_OUT_T GetDetailOut(long OspHeaderId)
        {
            try
            {
                return OspDetailOutTRepository.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new OSP_DETAIL_OUT_T();
            }

        }

        /// <summary>
        /// 檢查代紙工單是否可以排單
        /// </summary>
        /// <param name="SrcOspHeaderId">來源裁切工單HeaderId</param>
        /// <returns></returns>
        public ResultModel CheckInsteadPaperOrderProcess(long SrcOspHeaderId)
        {
            var header = OspHeaderTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.OspHeaderId == SrcOspHeaderId);
            if (header.Status != ProcessStatusCode.CompletedBatch) return new ResultModel(false, "此工單" + header.BatchNo + "尚未完工");
            return new ResultModel(true, "可以排單");
        }

        /// <summary>
        /// 取得使用者倉庫資料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<int> GetOspPendingCount(string userId)
        {
            var resultDataModel = new ResultDataModel<int>(false, "", 0);
            try
            {
                var subinventoryList = GetSubinventoryListForUser(userId);
                if (subinventoryList == null || subinventoryList.Count == 0)
                {
                    throw new Exception("找不到使用者倉庫");
                }

                resultDataModel.Data = OspHeaderTRepository.GetAll().AsNoTracking().Join(
                    OspDetailInTRepository.GetAll().AsNoTracking(),
                    h => new { h.OspHeaderId },
                    i => new { i.OspHeaderId },
                    (h, i) => new
                    {
                        Status = h.Status,
                        Subinventory = i.Subinventory
                    })
                    .Where(x => x.Status != ProcessStatusCode.Modified
                        && x.Status != ProcessStatusCode.Canceled
                        && x.Status != ProcessStatusCode.CompletedBatch
                        && x.Status != ProcessStatusCode.CloseBatch
                        && subinventoryList.Contains(x.Subinventory))
                    .Count();
                resultDataModel.Code = ResultModel.CODE_SUCCESS;
                resultDataModel.Msg = "";
            }
            catch (Exception ex)
            {
                resultDataModel.Code = -1;
                resultDataModel.Msg = $"取得加工單未完成數量時發生錯誤 EX:{ex.Message}";
            }

            return resultDataModel;
        }
    }
}