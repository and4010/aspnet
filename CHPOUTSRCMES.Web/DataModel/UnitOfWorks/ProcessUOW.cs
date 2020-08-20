﻿using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Process;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Process;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Graph;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class ProcessUOW : MasterUOW
    {

        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<OSP_COTANGENT_T> OspCotangenTRepository;
        private readonly IRepository<OSP_COTANGENT_HT> OspCotangentHTRepository;
        private readonly IRepository<OSP_DETAIL_IN_T> OspDetailInTRepositiory;
        private readonly IRepository<OSP_DETAIL_IN_HT> OspDetailInHTRepositiory;
        private readonly IRepository<OSP_DETAIL_OUT_T> OspDetailOutTRepositiory;
        private readonly IRepository<OSP_DETAIL_OUT_HT> OspDetailOutHTRepositiory;
        private readonly IRepository<OSP_HEADER_T> OspHeaderTRepositiory;
        private readonly IRepository<OSP_ORG_T> OspOrgTRepositiory;
        private readonly IRepository<OSP_PICKED_IN_T> OspPickedInTRepositiory;
        private readonly IRepository<OSP_PICKED_IN_HT> OspPickedInHTRepositiory;
        private readonly IRepository<OSP_PICKED_OUT_T> OspPickedOutTRepositiory;
        private readonly IRepository<OSP_PICKED_OUT_HT> OspPickedOutHTRepositiory;
        private readonly IRepository<OSP_YIELD_VARIANCE_T> OspYieldVarianceTRepositiory;
        private readonly IRepository<OSP_YIELD_VARIANCE_HT> OspYieldVarianceHTRepositiory;

        public ProcessUOW(DbContext context) : base(context)
        {
            this.OspCotangenTRepository = new GenericRepository<OSP_COTANGENT_T>(this);
            this.OspCotangentHTRepository = new GenericRepository<OSP_COTANGENT_HT>(this);
            this.OspDetailInTRepositiory = new GenericRepository<OSP_DETAIL_IN_T>(this);
            this.OspDetailInHTRepositiory = new GenericRepository<OSP_DETAIL_IN_HT>(this);
            this.OspDetailOutTRepositiory = new GenericRepository<OSP_DETAIL_OUT_T>(this);
            this.OspDetailOutHTRepositiory = new GenericRepository<OSP_DETAIL_OUT_HT>(this);
            this.OspHeaderTRepositiory = new GenericRepository<OSP_HEADER_T>(this);
            this.OspOrgTRepositiory = new GenericRepository<OSP_ORG_T>(this);
            this.OspPickedInTRepositiory = new GenericRepository<OSP_PICKED_IN_T>(this);
            this.OspPickedInHTRepositiory = new GenericRepository<OSP_PICKED_IN_HT>(this);
            this.OspPickedOutTRepositiory = new GenericRepository<OSP_PICKED_OUT_T>(this);
            this.OspPickedOutHTRepositiory = new GenericRepository<OSP_PICKED_OUT_HT>(this);
            this.OspYieldVarianceTRepositiory = new GenericRepository<OSP_YIELD_VARIANCE_T>(this);
            this.OspYieldVarianceHTRepositiory = new GenericRepository<OSP_YIELD_VARIANCE_HT>(this);


        }

        public void generateTestData()
        {
            this.Context.Configuration.AutoDetectChangesEnabled = false;
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    generateTestDataOspOrgT();
                    OspOrgToDetail();
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                }
            }
            this.Context.Configuration.AutoDetectChangesEnabled = true;
        }

        public void generateTestDataOspOrgT()
        {
            OSP_ORG_T oSP = new OSP_ORG_T();
            try
            {
                oSP.ProcessCode = "XXIFP220";
                oSP.ServerCode = "123";
                oSP.BatchId = "20191112141600100000";
                oSP.BatchLineId = 1;
                oSP.PeBatchId = 1;
                oSP.BatchNo = "W100000";
                oSP.BatchType = "OSP";
                oSP.BatchStatus = 0;
                oSP.BatchStatusDesc = "待排單";
                oSP.OrgId = 1;
                oSP.OrgName = "加工";
                oSP.OrganizationId = 265;
                oSP.OrganizationCode = "FTY";
                oSP.DueDate = DateTime.Now;
                oSP.PlanStartDate = DateTime.Now;
                oSP.PlanCmpltDate = DateTime.Now;
                oSP.PeCreatedBy = "1";
                oSP.PeCreationDate = DateTime.Now;
                oSP.PeLastUpdateBy = "1";
                oSP.PeLastUpdateDate = DateTime.Now;
                oSP.LineType = "I";
                oSP.LineNo = 1;
                oSP.InventoryItemId = 558705;
                oSP.InventoryItemNumber = "4AH00A00900362KRL00";
                oSP.PackingType = "無令打包";
                oSP.PaperType = "AH00";
                oSP.OrderWeight = "20";
                oSP.BasicWeight = "00900";
                oSP.Specification = "362KRL00";
                oSP.GrainDirection = "S";
                oSP.ReamWt = "20";
                oSP.PackingType = "令";
                oSP.PlanQty = 1;
                oSP.WipPLAN_QTY = 1;
                oSP.DtlUom = "KG";
                oSP.OrderHeaderId = 1;
                oSP.OrderNumber = 123456;
                oSP.OrderLineId = 1;
                oSP.OrderLineNumber = "1112315";
                oSP.CustomerId = 1;
                oSP.CustomerNumber = "123";
                oSP.CustomerName = "中華彩色印刷股份有限公司";
                oSP.PrNumber = 1;
                oSP.PrLineNumber = 1;
                oSP.RequisitionLineId = 1;
                oSP.PoNumber = 1;
                oSP.PoLineNumber = 1;
                oSP.PoLineId = 1;
                oSP.PoUnitPrice = 20;
                oSP.PoRevisionNum = 1;
                oSP.PoStatus = "待印";
                oSP.PoVendorNum = "1";
                oSP.OspRemark = "N11/25入倉";
                oSP.Subinventory = "TB3";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 1;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 1000;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 1000;
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = "1";
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = "1";
                oSP.LastUpdateDate = DateTime.Now;
                OspOrgTRepositiory.Create(oSP, true);

                oSP.ProcessCode = "XXIFP220";
                oSP.ServerCode = "123";
                oSP.BatchId = "20191112141600100000";
                oSP.BatchLineId = 1;
                oSP.PeBatchId = 1;
                oSP.BatchNo = "W100000";
                oSP.BatchType = "OSP";
                oSP.BatchStatus = 0;
                oSP.BatchStatusDesc = "待排單";
                oSP.OrgId = 1;
                oSP.OrgName = "加工";
                oSP.OrganizationId = 265;
                oSP.OrganizationCode = "FTY";
                oSP.DueDate = DateTime.Now;
                oSP.PlanStartDate = DateTime.Now;
                oSP.PlanCmpltDate = DateTime.Now;
                oSP.PeCreatedBy = "1";
                oSP.PeCreationDate = DateTime.Now;
                oSP.PeLastUpdateBy = "1";
                oSP.PeLastUpdateDate = DateTime.Now;
                oSP.LineType = "P";
                oSP.LineNo = 1;
                oSP.InventoryItemId = 728485;
                oSP.InventoryItemNumber = "4FU0SA020000889RL00";
                oSP.PackingType = "無令打包";
                oSP.PaperType = "FU0S";
                oSP.OrderWeight = "20";
                oSP.BasicWeight = "020000";
                oSP.Specification = "889RL00";
                oSP.GrainDirection = "S";
                oSP.ReamWt = "20";
                oSP.PackingType = "令";
                oSP.PlanQty = 1;
                oSP.WipPLAN_QTY = 1;
                oSP.DtlUom = "KG";
                oSP.OrderHeaderId = 1;
                oSP.OrderNumber = 123456;
                oSP.OrderLineId = 1;
                oSP.OrderLineNumber = "1112315";
                oSP.CustomerId = 1;
                oSP.CustomerNumber = "123";
                oSP.CustomerName = "中華彩色印刷股份有限公司";
                oSP.PrNumber = 1;
                oSP.PrLineNumber = 1;
                oSP.RequisitionLineId = 1;
                oSP.PoNumber = 1;
                oSP.PoLineNumber = 1;
                oSP.PoLineId = 1;
                oSP.PoUnitPrice = 20;
                oSP.PoRevisionNum = 1;
                oSP.PoStatus = "待印";
                oSP.PoVendorNum = "1";
                oSP.OspRemark = "N11/25入倉";
                oSP.Subinventory = "TB3";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 1;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 1000;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 1000;
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = "1";
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = "1";
                oSP.LastUpdateDate = DateTime.Now;
                OspOrgTRepositiory.Create(oSP, true);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
            }
        }

        /// <summary>
        /// 主檔轉各自detail
        /// </summary>
        public void OspOrgToDetail()
        {
            OSP_HEADER_T OspHeaderT = new OSP_HEADER_T();
            OSP_DETAIL_IN_T OspDetailInT = new OSP_DETAIL_IN_T();
            OSP_DETAIL_OUT_T OspDetailOutT = new OSP_DETAIL_OUT_T();
            var org = OspOrgTRepositiory.GetAll().AsNoTracking().ToList();

            try
            {
                for (int i = 0; org.Count() > i; i++)
                {
                    var OspHeaderBatchNo = org[i].BatchNo;
                    var batch = OspHeaderTRepositiory.GetAll().Where(x => x.BatchNo == OspHeaderBatchNo).SingleOrDefault();
                    if (batch == null)
                    {
                        OspHeaderT.PeBatchId = org[i].PeBatchId;
                        OspHeaderT.BatchNo = org[i].BatchNo;
                        OspHeaderT.BatchType = org[i].BatchType;
                        OspHeaderT.BatchStatus = org[i].BatchStatus;
                        OspHeaderT.BatchStatusDesc = org[i].BatchStatusDesc;
                        OspHeaderT.OrgId = org[i].OrgId;
                        OspHeaderT.OrgName = org[i].OrgName;
                        OspHeaderT.OrganizationId = org[i].OrganizationId;
                        OspHeaderT.OrganizationCode = org[i].OrganizationCode;
                        OspHeaderT.DueDate = org[i].DueDate;
                        OspHeaderT.PlanStartDate = org[i].PlanStartDate;
                        OspHeaderT.PlanCmpltDate = org[i].PlanCmpltDate;
                        OspHeaderT.PeCreatedBy = org[i].PeCreatedBy;
                        OspHeaderT.PeCreationDate = org[i].PeCreationDate;
                        OspHeaderT.PeLastUpdateBy = org[i].PeLastUpdateBy;
                        OspHeaderT.PeLastUpdateDate = org[i].PeLastUpdateDate;
                        OspHeaderT.Status = "待排單";
                        OspHeaderT.Modifications = 0;
                        OspHeaderTRepositiory.Create(OspHeaderT, true);
                    }
                    else if (batch.BatchNo != org[i].BatchNo)
                    {
                        OspHeaderT.PeBatchId = org[i].PeBatchId;
                        OspHeaderT.BatchNo = org[i].BatchNo;
                        OspHeaderT.BatchType = org[i].BatchType;
                        OspHeaderT.BatchStatus = org[i].BatchStatus;
                        OspHeaderT.BatchStatusDesc = org[i].BatchStatusDesc;
                        OspHeaderT.OrgId = org[i].OrgId;
                        OspHeaderT.OrgName = org[i].OrgName;
                        OspHeaderT.OrganizationId = org[i].OrganizationId;
                        OspHeaderT.OrganizationCode = org[i].OrganizationCode;
                        OspHeaderT.DueDate = org[i].DueDate;
                        OspHeaderT.PlanStartDate = org[i].PlanStartDate;
                        OspHeaderT.PlanCmpltDate = org[i].PlanCmpltDate;
                        OspHeaderT.PeCreatedBy = org[i].PeCreatedBy;
                        OspHeaderT.PeCreationDate = org[i].PeCreationDate;
                        OspHeaderT.PeLastUpdateBy = org[i].PeLastUpdateBy;
                        OspHeaderT.PeLastUpdateDate = org[i].PeLastUpdateDate;
                        OspHeaderT.Status = "待排單";
                        OspHeaderT.Modifications = 0;
                        OspHeaderTRepositiory.Create(OspHeaderT, true);
                    }


                    if (org[i].LineType == "I")
                    {
                        OspDetailInT.OspHeaderId = OspHeaderT.OspHeaderId;
                        OspDetailInT.LineType = org[i].LineType;
                        OspDetailInT.LineNo = org[i].LineNo;
                        OspDetailInT.InventoryItemId = org[i].InventoryItemId;
                        OspDetailInT.InventoryItemNumber = org[i].InventoryItemNumber;
                        OspDetailInT.BasicWeight = org[i].BasicWeight;
                        OspDetailInT.Specification = org[i].Specification;
                        OspDetailInT.GrainDirection = org[i].GrainDirection;
                        OspDetailInT.OrderWeight = org[i].OrderWeight;
                        OspDetailInT.ReamWt = org[i].ReamWt;
                        OspDetailInT.PaperType = org[i].PaperType;
                        OspDetailInT.PackingType = org[i].PackingType;
                        OspDetailInT.PlanQty = org[i].PlanQty;
                        OspDetailInT.WipPLAN_QTY = org[i].LineNo;
                        OspDetailInT.DtlUom = org[i].DtlUom;
                        OspDetailInT.OrderHeaderId = org[i].OrderHeaderId;
                        OspDetailInT.OrderNumber = org[i].OrderNumber;
                        OspDetailInT.OrderLineId = org[i].OrderLineId;
                        OspDetailInT.OrderLineNumber = org[i].OrderLineNumber;
                        OspDetailInT.CustomerId = org[i].CustomerId;
                        OspDetailInT.CustomerNumber = org[i].CustomerNumber;
                        OspDetailInT.CustomerName = org[i].CustomerName;
                        OspDetailInT.PrNumber = org[i].PrNumber;
                        OspDetailInT.PrLineNumber = org[i].PrLineNumber;
                        OspDetailInT.RequisitionLineId = org[i].RequisitionLineId;
                        OspDetailInT.PoNumber = org[i].PoNumber;
                        OspDetailInT.PoLineNumber = org[i].PoLineNumber;
                        OspDetailInT.PoLineId = org[i].PoLineId;
                        OspDetailInT.PoUnitPrice = org[i].PoUnitPrice;
                        OspDetailInT.PoRevisionNum = org[i].PoRevisionNum;
                        OspDetailInT.PoStatus = org[i].PoStatus;
                        OspDetailInT.PoVendorNum = org[i].PoVendorNum;
                        OspDetailInT.OspRemark = org[i].OspRemark;
                        OspDetailInT.Subinventory = org[i].Subinventory;
                        OspDetailInT.LocatorId = org[i].LocatorId;
                        OspDetailInT.LocatorCode = org[i].LocatorCode;
                        OspDetailInT.ReservationUomCode = org[i].ReservationUomCode;
                        OspDetailInT.ReservationQuantity = org[i].ReservationQuantity;
                        OspDetailInT.LineCreatedBy = org[i].LineCreatedBy;
                        OspDetailInT.LineCreationDate = org[i].LineCreationDate;
                        OspDetailInT.LineLastUpdateBy = org[i].LineLastUpdateBy;
                        OspDetailInT.LineLastUpdateDate = org[i].LineLastUpdateDate;
                        OspDetailInT.TransactionQuantity = org[i].TransactionQuantity;
                        OspDetailInT.TransactionUom = org[i].TransactionUom;
                        OspDetailInT.PrimaryQuantity = org[i].PrimaryQuantity;
                        OspDetailInT.PrimaryUom = org[i].PrimaryUom;
                        OspDetailInT.SecondaryQuantity = org[i].SecondaryQuantity;
                        OspDetailInT.SecondaryUom = org[i].SecondaryUom;
                        OspDetailInT.RequestId = org[i].RequestId;
                        OspDetailInT.CreatedBy = org[i].CreatedBy;
                        OspDetailInT.CreationDate = org[i].CreationDate;
                        OspDetailInT.LastUpdateBy = org[i].LastUpdateBy;
                        OspDetailInT.LastUpdateDate = org[i].LastUpdateDate;
                        OspDetailInTRepositiory.Create(OspDetailInT, true);

                    }

                    if (org[i].LineType == "P")
                    {
                        OspDetailOutT.OspHeaderId = OspHeaderT.OspHeaderId;
                        OspDetailOutT.LineType = org[i].LineType;
                        OspDetailOutT.LineNo = org[i].LineNo;
                        OspDetailOutT.InventoryItemId = org[i].InventoryItemId;
                        OspDetailOutT.InventoryItemNumber = org[i].InventoryItemNumber;
                        OspDetailOutT.BasicWeight = org[i].BasicWeight;
                        OspDetailOutT.Specification = org[i].Specification;
                        OspDetailOutT.GrainDirection = org[i].GrainDirection;
                        OspDetailOutT.OrderWeight = org[i].OrderWeight;
                        OspDetailOutT.ReamWt = org[i].ReamWt;
                        OspDetailOutT.PaperType = org[i].PaperType;
                        OspDetailOutT.PackingType = org[i].PackingType;
                        OspDetailOutT.PlanQty = org[i].PlanQty;
                        OspDetailOutT.WipPLAN_QTY = org[i].LineNo;
                        OspDetailOutT.DtlUom = org[i].DtlUom;
                        OspDetailOutT.OrderHeaderId = org[i].OrderHeaderId;
                        OspDetailOutT.OrderNumber = org[i].OrderNumber;
                        OspDetailOutT.OrderLineId = org[i].OrderLineId;
                        OspDetailOutT.OrderLineNumber = org[i].OrderLineNumber;
                        OspDetailOutT.CustomerId = org[i].CustomerId;
                        OspDetailOutT.CustomerNumber = org[i].CustomerNumber;
                        OspDetailOutT.CustomerName = org[i].CustomerName;
                        OspDetailOutT.PrNumber = org[i].PrNumber;
                        OspDetailOutT.PrLineNumber = org[i].PrLineNumber;
                        OspDetailOutT.RequisitionLineId = org[i].RequisitionLineId;
                        OspDetailOutT.PoNumber = org[i].PoNumber;
                        OspDetailOutT.PoLineNumber = org[i].PoLineNumber;
                        OspDetailOutT.PoLineId = org[i].PoLineId;
                        OspDetailOutT.PoUnitPrice = org[i].PoUnitPrice;
                        OspDetailOutT.PoRevisionNum = org[i].PoRevisionNum;
                        OspDetailOutT.PoStatus = org[i].PoStatus;
                        OspDetailOutT.PoVendorNum = org[i].PoVendorNum;
                        OspDetailOutT.OspRemark = org[i].OspRemark;
                        OspDetailOutT.Subinventory = org[i].Subinventory;
                        OspDetailOutT.LocatorId = org[i].LocatorId;
                        OspDetailOutT.LocatorCode = org[i].LocatorCode;
                        OspDetailOutT.ReservationUomCode = org[i].ReservationUomCode;
                        OspDetailOutT.ReservationQuantity = org[i].ReservationQuantity;
                        OspDetailOutT.LineCreatedBy = org[i].LineCreatedBy;
                        OspDetailOutT.LineCreationDate = org[i].LineCreationDate;
                        OspDetailOutT.LineLastUpdateBy = org[i].LineLastUpdateBy;
                        OspDetailOutT.LineLastUpdateDate = org[i].LineLastUpdateDate;
                        OspDetailOutT.TransactionQuantity = org[i].TransactionQuantity;
                        OspDetailOutT.TransactionUom = org[i].TransactionUom;
                        OspDetailOutT.PrimaryQuantity = org[i].PrimaryQuantity;
                        OspDetailOutT.PrimaryUom = org[i].PrimaryUom;
                        OspDetailOutT.SecondaryQuantity = org[i].SecondaryQuantity;
                        OspDetailOutT.SecondaryUom = org[i].SecondaryUom;
                        OspDetailOutT.RequestId = org[i].RequestId;
                        OspDetailOutT.CreatedBy = org[i].CreatedBy;
                        OspDetailOutT.CreationDate = org[i].CreationDate;
                        OspDetailOutT.LastUpdateBy = org[i].LastUpdateBy;
                        OspDetailOutT.LastUpdateDate = org[i].LastUpdateDate;
                        OspDetailOutTRepositiory.Create(OspDetailOutT, true);

                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
            }
        }

        /// <summary>
        /// 取得畫面Model
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
H.BATCH_TYPE AS BatchType,
H.DUE_DATE AS DueDate,
H.CUTTING_DATE_FROM AS CuttingDateFrom,
H.CUTTING_DATE_TO AS CuttingDateTo,
H.MACHINE_CODE AS MachineNum,
DI.CUSTOMER_NAME AS CustomerName,
DI.ORDER_NUMBER AS OrderNumber,
DI.ORDER_LINE_NUMBER AS OrderLineNumber,
DI.BASIC_WEIGHT AS BasicWeight,
DI.SPECIFICATION AS Specification,
DI.GRAIN_DIRECTION AS GrainDirection,
DI.ORDER_WEIGHT AS OrderWeight,
DI.REAM_WT AS ReamWt,
DI.PRIMARY_QUANTITY AS PrimaryQuantity,
DI.PRIMARY_UOM AS PrimaryUom,
DI.TRANSACTION_UOM AS TransactionUom,
DI.PACKING_TYPE AS PackingType,
DI.PAPER_TYPE AS PaperType,
DI.OSP_REMARK AS OspRemark,
DI.INVENTORY_ITEM_NUMBER AS SelectedInventoryItemNumber,
DO.INVENTORY_ITEM_NUMBER AS Product_Item,
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
WHERE DI.OSP_DETAIL_IN_ID = @OSP_DETAIL_IN_ID");
                    return mesContext.Database.SqlQuery<CHP_PROCESS_T>(query.ToString(), new SqlParameter("@OSP_DETAIL_IN_ID", id)).SingleOrDefault();
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
        public ResultModel SetEditNote(long OspHeaderId, string note)
        {
            try
            {
                var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (header != null)
                {
                    header.Note = note;
                    OspHeaderTRepositiory.Update(header, true);
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
        /// 取得table資料
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="BatchNo"></param>
        /// <param name="MachineNum"></param>
        /// <param name="DueDate"></param>
        /// <param name="CuttingDateFrom"></param>
        /// <param name="CuttingDateTo"></param>
        /// <param name="Subinventory"></param>
        /// <returns></returns>
        public List<CHP_PROCESS_T> GetTable(string Status, string BatchNo, string MachineNum, string DueDate, string CuttingDateFrom, string CuttingDateTo, string Subinventory)
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
H.BATCH_TYPE AS BatchType,
H.DUE_DATE AS DueDate,
H.CUTTING_DATE_FROM AS CuttingDateFrom,
H.CUTTING_DATE_TO AS CuttingDateTo,
H.MACHINE_CODE AS MachineNum,
DI.CUSTOMER_NAME AS CustomerName,
DI.ORDER_NUMBER AS OrderNumber,
DI.ORDER_LINE_NUMBER AS OrderLineNumber,
DI.BASIC_WEIGHT AS BasicWeight,
DI.SPECIFICATION AS Specification,
DI.GRAIN_DIRECTION AS GrainDirection,
DI.ORDER_WEIGHT AS OrderWeight,
DI.REAM_WT AS ReamWt,
DI.PRIMARY_QUANTITY AS PrimaryQuantity,
DI.PRIMARY_UOM AS PrimaryUom,
DI.TRANSACTION_UOM AS TransactionUom,
DI.PACKING_TYPE AS PackingType,
DI.PAPER_TYPE AS PaperType,
DI.OSP_REMARK AS OspRemark,
DI.INVENTORY_ITEM_NUMBER AS SelectedInventoryItemNumber,
DO.INVENTORY_ITEM_NUMBER AS Product_Item,
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
left JOIN OSP_YIELD_VARIANCE_T OYV ON OYV.OSP_HEADER_ID = H.OSP_HEADER_ID");
                    if (Status != "*")
                    {
                        cond.Add("H.STATUS = @STATUS");
                        sqlParameterList.Add(new SqlParameter("@STATUS", Status));
                    }
                    if (BatchNo != "*")
                    {
                        cond.Add("H.BATCH_NO = @BATCH_NO");
                        sqlParameterList.Add(new SqlParameter("@BATCH_NO", BatchNo));
                    }
                    if (MachineNum != "*")
                    {
                        cond.Add("MACHINE_CODE = @MACHINE_CODE");
                        sqlParameterList.Add(new SqlParameter("@MACHINE_CODE", MachineNum));
                    }
                    if (DueDate != "")
                    {
                        cond.Add("H.DUE_DATE = @DUE_DATE");
                        sqlParameterList.Add(new SqlParameter("@CUTTING_DATE_FROM", DueDate));
                    }
                    if (CuttingDateFrom != "")
                    {
                        cond.Add("H.CUTTING_DATE_FROM = @CUTTING_DATE_FROM");
                        sqlParameterList.Add(new SqlParameter("@CUTTING_DATE_FROM", CuttingDateFrom));
                    }
                    if (CuttingDateTo != "")
                    {
                        cond.Add("H.CUTTING_DATE_TO = @CUTTING_DATE_TO");
                        sqlParameterList.Add(new SqlParameter("@CUTTING_DATE_TO", CuttingDateTo));
                    }
                    if (Subinventory != "*")
                    {
                        cond.Add("DI.SUBINVENTORY = @SUBINVENTORY");
                        sqlParameterList.Add(new SqlParameter("@SUBINVENTORY", Subinventory));
                    }
                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
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
        /// 投入條碼
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public List<Invest> GetPicketIn(long OspDetailInId)
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
where OSP_DETAIL_IN_ID = @OSP_DETAIL_IN_ID");
                    return mesContext.Database.SqlQuery<Invest>(query.ToString(), new SqlParameter("@OSP_DETAIL_IN_ID", OspDetailInId)).ToList();
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
            try
            {
                var InvestDTListId = InvestDTList.InvestList[0];
                if (InvestDTList.Action == "edit")
                {
                    var id = OspPickedInTRepositiory.Get(x => x.OspPickedInId == InvestDTListId.OspPickedInId).SingleOrDefault();

                    var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                    if (id != null)
                    {
                        id.HasRemaint = InvestDTListId.HasRemaint;
                        id.RemainingQuantity = InvestDTListId.RemainingQuantity;
                        id.LastUpdateBy = UserId;
                        id.LastUpdateUserName = UserName;
                        id.LastUpdateDate = DateTime.Now;
                        OspPickedInTRepositiory.Update(id, true);
                        var aft = id.PrimaryQuantity - (id.PrimaryQuantity - (InvestDTListId.RemainingQuantity));
                        var chg = id.PrimaryQuantity - (InvestDTListId.RemainingQuantity);
                        StockRecord(id.StockId, aft ?? 0, chg??0, 0, 0, CategoryCode.Process, ActionCode.Picked, header.BatchNo, UserId);
                        return new ResultModel(true, "");
                    }
                }

                if (InvestDTList.Action == "remove")
                {
                    var barcode = OspPickedInTRepositiory.Get(x => x.OspPickedInId == InvestDTListId.OspPickedInId).SingleOrDefault();
                    if (barcode != null)
                    {
                        OspPickedInTRepositiory.Delete(barcode, true);
                        return new ResultModel(true, "");
                    }

                }
                return new ResultModel(false, "找不到ID");
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
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
            string Dialog_MachineNum, string BtnStatus)
        {
            try
            {
                var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                if (header != null)
                {
                    header.CuttingDateFrom = Dialog_CuttingDateFrom;
                    header.CuttingDateTo = Dialog_CuttingDateTo;
                    header.MachineCode = Dialog_MachineNum == "*" ? "" : Dialog_MachineNum;
                    header.Status = BtnStatus;
                    OspHeaderTRepositiory.Update(header, true);
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
                var batchno = OspHeaderTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
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
        public ResultDataModel<STOCK_T> CheckStockBarcode(string Barcode, string OspDetailInId)
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
FROM [STOCK_T] ST
join OSP_DETAIL_IN_T DT ON DT.OSP_DETAIL_IN_ID = @OSP_DETAIL_IN_ID
WHERE ST.ITEM_NUMBER = DT.INVENTORY_ITEM_NUMBER
AND ST.BARCODE = @BARCODE");
                    sqlParameterList.Add(new SqlParameter("@OSP_DETAIL_IN_ID", OspDetailInId));
                    sqlParameterList.Add(new SqlParameter("@BARCODE", Barcode));
                    var data = mesContext.Database.SqlQuery<STOCK_T>(query.ToString(), sqlParameterList.ToArray()).SingleOrDefault();
                    if (data == null)
                    {
                        return new ResultDataModel<STOCK_T>(false, "無條碼資料", data);
                    }
                    else
                    {
                        return new ResultDataModel<STOCK_T>(true, "", data);
                    }

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
            using var txn = this.Context.Database.BeginTransaction();
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
FROM [STOCK_T] ST
join OSP_DETAIL_IN_T DT ON DT.OSP_DETAIL_IN_ID = @OSP_DETAIL_IN_ID
WHERE ST.ITEM_NUMBER = DT.INVENTORY_ITEM_NUMBER
AND ST.BARCODE = @BARCODE");
                    sqlParameterList.Add(new SqlParameter("@OSP_DETAIL_IN_ID", OspDetailInId));
                    sqlParameterList.Add(new SqlParameter("@BARCODE", Barcode));
                    var data = mesContext.Database.SqlQuery<STOCK_T>(query.ToString(), sqlParameterList.ToArray()).SingleOrDefault();

                    if (data == null)
                    {
                        return new ResultModel(false, "無條碼資料");
                    }

                    var OspPickIn = OspPickedInTRepositiory.Get(x => x.Barcode == Barcode && x.OspDetailInId == OspDetailInId).SingleOrDefault();
                    if (OspPickIn == null)
                    {
                        var detailin = OspDetailInTRepositiory.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault();
                        var Header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == detailin.OspHeaderId).SingleOrDefault();
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
                        OspPickedInTRepositiory.Create(oSP_PICKED_IN_T, true);
                        var aft = data.PrimaryAvailableQty-(data.PrimaryAvailableQty - (Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight)));
                        var chg = data.PrimaryAvailableQty - (Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight));
                        StockRecord(data.StockId, aft,chg, 0,0, CategoryCode.Process,ActionCode.Picked, Header.BatchNo, UserId);
                        txn.Commit();
                        return new ResultModel(true, "寫入成功");
                    }
                    else
                    {
                        return new ResultModel(false, "條碼資料已存在");
                    }


                }

            }
            catch (Exception e)
            {
                txn.Rollback();
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
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
        public ResultModel CreateProduction(string UserId, string UserName, string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId)
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
                    var save = InsertPickOut(UserId, UserName, Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId);
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
        public ResultModel InsertPickOut(string UserId, string UserName, string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId)
        {
            try
            {
                using (var db = new MesContext())
                {
                    var detailout = OspDetailOutTRepositiory.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                    var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == detailout.OspHeaderId).SingleOrDefault();
                    if (detailout == null)
                    {
                        return new ResultModel(false, "ID錯誤");
                    }
                    OSP_PICKED_OUT_T ospPickOut = new OSP_PICKED_OUT_T();
                    OSP_COTANGENT_T ospCotanget = new OSP_COTANGENT_T();

                    if (Cotangent == "1")
                    {
                        var relate = db.OspDetailOutTs.
                            Join(db.RelatedTs,
                            s => s.InventoryItemId,
                            c => c.InventoryItemId,
                            (s, c) => new
                            {
                                a = s,
                                b = c

                            }).Where(
                            x => x.a.OspDetailOutId == OspDetailOutId &&
                            x.b.InventoryItemId == x.a.InventoryItemId &&
                            x.b.ControlFlag != "D").SingleOrDefault();

                        //尋找餘切
                        var Relateitem = db.ItemsTs.Where(x => x.InventoryItemId == relate.b.RelatedItemId).SingleOrDefault();


                        ospCotanget.OspDetailOutId = OspDetailOutId;
                        ospCotanget.OspHeaderId = detailout.OspHeaderId;
                        ospCotanget.StockId = null;
                        ospCotanget.Barcode = GenerateBarcodes(header.OrganizationId, detailout.Subinventory, 1, UserName).Data[0];
                        ospCotanget.InventoryItemId = Relateitem.InventoryItemId;
                        ospCotanget.InventoryItemNumber = Relateitem.ItemNumber;
                        ospCotanget.BasicWeight = Relateitem.CatalogElemVal040;
                        ospCotanget.Specification = Relateitem.CatalogElemVal050;
                        ospCotanget.PaperType = Relateitem.CatalogElemVal020;
                        ospCotanget.LotNumber = "";
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
                    for (int i = 0; i < int.Parse(Production_Roll_Ream_Qty); i++)
                    {
                        ospPickOut.OspDetailOutId = OspDetailOutId;
                        ospPickOut.OspHeaderId = detailout.OspHeaderId;
                        ospPickOut.StockId = null;
                        ospPickOut.Barcode = GenerateBarcodes(header.OrganizationId, detailout.Subinventory, int.Parse(Production_Roll_Ream_Qty), UserName).Data[i];
                        ospPickOut.InventoryItemId = detailout.InventoryItemId;
                        ospPickOut.InventoryItemNumber = detailout.InventoryItemNumber;
                        ospPickOut.BasicWeight = detailout.BasicWeight;
                        ospPickOut.Specification = detailout.Specification;
                        ospPickOut.PaperType = detailout.PaperType;
                        ospPickOut.LotNumber = "";
                        ospPickOut.PrimaryQuantity = uomConversion.Convert(detailout.InventoryItemId, decimal.Parse(Production_Roll_Ream_Wt), "RE", "KG").Data;
                        ospPickOut.PrimaryUom = "KG";
                        ospPickOut.SecondaryQuantity = decimal.Parse(Production_Roll_Ream_Wt);
                        ospPickOut.SecondaryUom = "RE";
                        ospPickOut.Status = "待入庫";
                        ospPickOut.Cotangent = Cotangent == "1" ? "Y" : "N";
                        ospPickOut.OspCotangentId = ospCotanget.OspCotangentId;
                        ospPickOut.CreatedBy = UserId;
                        ospPickOut.CreatedUserName = UserName;
                        ospPickOut.CreationDate = DateTime.Now;
                        OspPickedOutTRepositiory.Create(ospPickOut, true);
                    }

                    return new ResultModel(true, "");
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new ResultModel(false, e.Message);
            }

        }

        /// <summary>
        /// 取得產出檢貨picket
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public List<Production> GetPicketOut(long OspDetailOutId)
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
[PRIMARY_QUANTITY] as PrimaryQuantity,
[PRIMARY_UOM] as PrimaryUom,
[SECONDARY_QUANTITY] as SecondaryQuantity,
[SECONDARY_UOM] as SecondaryUom,
[STATUS] as Status
FROM OSP_PICKED_OUT_T
WHERE OSP_DETAIL_OUT_ID = @OSP_DETAIL_OUT_ID");
                    return mesContext.Database.SqlQuery<Production>(query.ToString(), new SqlParameter("@OSP_DETAIL_OUT_ID", OspDetailOutId)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<Production>();
            }
        }

        /// <summary>
        /// 餘切資料
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public List<Cotangent> GetCotangents(long OspDetailOutId)
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
where OSP_DETAIL_OUT_ID = @OSP_DETAIL_OUT_ID");
                    return mesContext.Database.SqlQuery<Cotangent>(query.ToString(), new SqlParameter("@OSP_DETAIL_OUT_ID", OspDetailOutId)).ToList();
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
                        var id = OspPickedOutTRepositiory.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            id.SecondaryQuantity = ProductionDTEditor.ProductionList[0].SecondaryQuantity;
                            id.PrimaryQuantity = uomConversion.Convert(id.InventoryItemId, ProductionDTEditor.ProductionList[0].SecondaryQuantity, "RE", "KG").Data;
                            id.LastUpdateBy = UserId;
                            id.LastUpdateUserName = UserName;
                            id.LastUpdateDate = DateTime.Now;
                            OspPickedOutTRepositiory.Update(id, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                        }
                    }
                    if (ProductionDTEditor.Action == "remove")
                    {
                        var OspPickedOutId = ProductionDTEditor.ProductionList[0].OspPickedOutId;
                        var id = OspPickedOutTRepositiory.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            var PickedOut = OspPickedOutTRepositiory.GetAll().ToList();
                            if (PickedOut.Count == 1)
                            {
                                var cotangent = OspCotangenTRepository.Get(x => x.OspCotangentId == id.OspCotangentId).SingleOrDefault();
                                if (cotangent != null)
                                {
                                    OspCotangenTRepository.Delete(cotangent, true);
                                }
                            }
                            OspPickedOutTRepositiory.Delete(id, true);
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

                    }
                    if (cotangentDTEditor.Action == "remove")
                    {
                        var OspCotangentId = cotangentDTEditor.CotangentList[0].OspCotangentId;
                        var id = OspCotangenTRepository.Get(r => r.OspCotangentId == OspCotangentId).SingleOrDefault();
                        if (id != null)
                        {
                            var osppick = OspPickedOutTRepositiory.Get(x => x.OspCotangentId == id.OspCotangentId).FirstOrDefault();
                            osppick.Cotangent = "N";
                            osppick.OspCotangentId = null;
                            OspCotangenTRepository.Delete(id, true);
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
        /// 產出條碼入庫
        /// </summary>
        /// <param name="Production_Barcode"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultModel ProductionChangeStatus(string Production_Barcode, long OspDetailOutId, string UserId, string UserName)
        {
            try
            {
                var ProductionId = OspPickedOutTRepositiory.Get(r => r.Barcode == Production_Barcode && r.OspDetailOutId == OspDetailOutId).SingleOrDefault();
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
                        OspPickedOutTRepositiory.Update(ProductionId, true);
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
        /// 計算損號
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultDataModel<OSP_YIELD_VARIANCE_T> Loss(long OspDetailInId, long OspDetailOutId, string UserId, string UserName)
        {
            try
            {
                var PickIn = OspPickedInTRepositiory.Get(x => x.OspDetailInId == OspDetailInId).ToList();
                var PickOut = OspPickedOutTRepositiory.Get(x => x.OspDetailOutId == OspDetailOutId).ToList();
                var DetailIn = OspDetailInTRepositiory.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault();
                var DetailOut = OspDetailOutTRepositiory.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                var Cotangent = OspCotangenTRepository.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                var PickOutWeight = 0M;
                var RemainWeight = 0M;
                var PickInWeight = 0M;
                var CotangetnWeight = 0M;
                for (int i = 0; i < PickOut.Count; i++)
                {
                    var status = PickOut[i].Status;
                    if (status == "待入庫")
                    {
                        return new ResultDataModel<OSP_YIELD_VARIANCE_T>(false, "產出請先入庫", null);
                    }
                }
                if (Cotangent != null)
                {
                    if (Cotangent.SecondaryQuantity == 0M || Cotangent.SecondaryQuantity == null)
                    {
                        return new ResultDataModel<OSP_YIELD_VARIANCE_T>(false, "餘切請先輸入令數", null);
                    }

                    if (Cotangent.Status == "待入庫")
                    {
                        return new ResultDataModel<OSP_YIELD_VARIANCE_T>(false, "餘切請先入庫", null);
                    }
                    CotangetnWeight = Cotangent.PrimaryQuantity;
                }
                //產出重量：訂單+餘切重量
                var ProductWeight = DetailOut.PrimaryQuantity + CotangetnWeight;

                for (int i = 0; i < PickIn.Count; i++)
                {
                    RemainWeight += PickIn[i].RemainingQuantity ?? 0;
                }
                //投入重量：領料量 - 餘捲
                var investWeight = DetailIn.PrimaryQuantity - RemainWeight;
                var Totle = Math.Round(ProductWeight / investWeight, 2);
                var rate = Math.Round((Totle * 100), 2);
                var loss = OspYieldVarianceTRepositiory.Get(x => x.OspHeaderId == DetailOut.OspHeaderId).SingleOrDefault();
                if (loss == null)
                {
                    OSP_YIELD_VARIANCE_T oSP_YIELD_VARIANCE_T = new OSP_YIELD_VARIANCE_T();
                    oSP_YIELD_VARIANCE_T.OspHeaderId = DetailOut.OspHeaderId;
                    oSP_YIELD_VARIANCE_T.DetailInQuantity = DetailIn.PrimaryQuantity;
                    oSP_YIELD_VARIANCE_T.CotangentQuantity = CotangetnWeight;
                    oSP_YIELD_VARIANCE_T.DetailOutQuantity = DetailOut.PrimaryQuantity;
                    oSP_YIELD_VARIANCE_T.PrimaryUom = "KG";
                    oSP_YIELD_VARIANCE_T.LossWeight = Totle;
                    oSP_YIELD_VARIANCE_T.Rate = rate;
                    oSP_YIELD_VARIANCE_T.CreatedBy = UserId;
                    oSP_YIELD_VARIANCE_T.CreatedUserName = UserName;
                    oSP_YIELD_VARIANCE_T.CreationDate = DateTime.Now;
                    OspYieldVarianceTRepositiory.Create(oSP_YIELD_VARIANCE_T, true);
                    return new ResultDataModel<OSP_YIELD_VARIANCE_T>(true, "成功", oSP_YIELD_VARIANCE_T);
                }
                else
                {
                    loss.DetailInQuantity = DetailIn.PrimaryQuantity;
                    loss.CotangentQuantity = CotangetnWeight;
                    loss.DetailOutQuantity = DetailOut.PrimaryQuantity;
                    loss.LastUpdateBy = UserId;
                    loss.LastUpdateUserName = UserName;
                    loss.LastUpdateDate = DateTime.Now;
                    OspYieldVarianceTRepositiory.Update(loss, true);
                    return new ResultDataModel<OSP_YIELD_VARIANCE_T>(true, "成功", loss);
                }


            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultDataModel<OSP_YIELD_VARIANCE_T>(false, e.Message, null);
            }

        }

        /// <summary>
        /// 列印標籤
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GetStockLabels(List<long> OspPickedInId, string userName)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (OspPickedInId == null || OspPickedInId.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                var pickDataList = OspPickedInTRepositiory.GetAll().AsNoTracking().Where(x => OspPickedInId.Contains(x.OspPickedInId)).ToList();
                if (pickDataList == null || pickDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < pickDataList.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder(
@"
SELECT 
CAST(PTI.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(PTI.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(PTI.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(PTI.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(PTI.PRIMARY_QUANTITY,'0.##########') AS nvarchar) AS Qty,
CAST(PTI.PRIMARY_UOM AS nvarchar) AS Unit
--CAST(OH.BATCH_NO AS nvarchar) AS BatchNo
FROM OSP_PICKED_IN_T PTI
join OSP_HEADER_T OH ON OH.OSP_HEADER_ID = PTI.OSP_HEADER_ID
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = PTI.INVENTORY_ITEM_ID
AND PTI.OSP_PICKED_IN_ID = @OSP_PICKED_IN_ID
");
                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@OSP_PICKED_IN_ID", pickDataList[0].StockId));
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
        /// 取得得率重量
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public YieldVariance GetRate(long OspDetailInId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    var OspHeaderId = OspDetailInTRepositiory.Get(x => x.OspDetailInId == OspDetailInId).SingleOrDefault().OspHeaderId;
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
[OSP_YIELD_VARIANCE_ID] as OspYieldVarianceId,
[OSP_HEADER_ID] as OspHeaderId,
[LOSS_WEIGHT] as LossWeight,
[PRIMARY_UOM],
[RATE] as Rate
FROM [OSP_YIELD_VARIANCE_T]
where OSP_HEADER_ID = @OSP_HEADER_ID");
                    return mesContext.Database.SqlQuery<YieldVariance>(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OspHeaderId)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new YieldVariance();
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
        public ResultModel ChangeHeaderStauts(long OspDetailOutId, long Locator, string UserId, string UserName)
        {
            using var txn = this.Context.Database.BeginTransaction();
            try
            {
                var DetailOut = OspDetailOutTRepositiory.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == DetailOut.OspHeaderId).SingleOrDefault();
                var DetailIn = OspDetailInTRepositiory.Get(x => x.OspHeaderId == header.OspHeaderId).SingleOrDefault();
                if (header != null)
                {
                    if (header.Status == "已完工")
                    {
                        header.Status = "待核准";
                        header.PeLastUpdateBy = UserId;
                        header.PeLastUpdateDate = DateTime.Now;
                        OspHeaderTRepositiory.Update(header, true);
                        txn.Commit();
                        return new ResultModel(true, "");
                    }
                    else if (header.Status == "待核准")
                    {
                        header.Status = "已完工";
                        header.PeLastUpdateBy = UserId;
                        header.PeLastUpdateDate = DateTime.Now;
                        OspHeaderTRepositiory.Update(header, true);
                        txn.Commit();
                        return new ResultModel(true, "");
                    }
                    else
                    {
                        header.Status = "已完工";
                        header.PeLastUpdateBy = UserId;
                        header.PeLastUpdateDate = DateTime.Now;
                        OspHeaderTRepositiory.Update(header, true);
                        var locator = locatorTRepositiory.Get(x => x.LocatorId == Locator).SingleOrDefault();
                        DetailOut.LocatorId = Locator;
                        DetailOut.LocatorCode = locator.Segment3;
                        DetailOut.LastUpdateBy = UserId;
                        DetailOut.LastUpdateDate = DateTime.Now;
                        OspDetailOutTRepositiory.Update(DetailOut, true);

                        DetailIn.LocatorId = Locator;
                        DetailIn.LocatorCode = locator.Segment3;
                        DetailIn.LastUpdateBy = UserId;
                        DetailIn.LastUpdateDate = DateTime.Now;
                        OspDetailInTRepositiory.Update(DetailIn, true);

                        SaveStock(header.OspHeaderId, StockStatusCode.InStock);
                      

                        txn.Commit();
                        return new ResultModel(true, "");

                    }
                }
                return new ResultModel(false, "找不到id");
            }
            catch (Exception e)
            {
                txn.Rollback();
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultModel(false, e.Message);
            }


        }

        /// <summary>
        /// 新增庫存
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="statusCode"></param>
        public void SaveStock(long headerId, string statusCode)
        {
            var pheaderId = SqlParamHelper.GetBigInt("@headerId", headerId);
            var pstatusCode = SqlParamHelper.GetNVarChar("@statusCode", statusCode);
            var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
            var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
            var pUser = SqlParamHelper.GetNVarChar("@user", "", 128);
            this.Context.Database.ExecuteSqlCommand("[SP_SaveOspDetailOut] @headerId, @statusCode ,@code output, @message output, @user",
                pheaderId, pstatusCode, pCode, pMsg, pUser);
        }


        /// <summary>
        /// 庫存異動紀錄
        /// </summary>
        /// <param name="Barcode"></param>
        public void StockRecord(long StockId,decimal PryAftQty ,decimal PryChgQty, decimal SecAftQty, decimal SecChgQty,
            string Category,string Action,string Doc,string Createdby)
        {

            using (var mesContext = new MesContext())
            {
                var pStockId = SqlParamHelper.GetBigInt("@StockId", StockId);
                var pPryAftQty = SqlParamHelper.GetDecimal("@PRY_AFT_QTY", PryAftQty);
                var pPryChgQty = SqlParamHelper.GetDecimal("@PRY_CHG_QTY", PryChgQty);
                var pSecAftQty = SqlParamHelper.GetDecimal("@SEC_AFT_QTY", SecAftQty);
                var pSecChgQty = SqlParamHelper.GetDecimal("@SEC_CHG_QTY", SecChgQty);
                var pCategory = SqlParamHelper.GetNVarChar("@CATEGORY", Category);
                var pAction = SqlParamHelper.GetNVarChar("@ACTION", Action);
                var pDoc = SqlParamHelper.GetNVarChar("@DOC", Doc);
                var pCreatedby = SqlParamHelper.GetNVarChar("@CREATED_BY", Createdby);
                var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
                var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
                var pUser = SqlParamHelper.GetNVarChar("@user", "", 128);
                mesContext.Database.ExecuteSqlCommand("[SP_ProcessSaveStkTxn] @StockId, @PRY_AFT_QTY ,@PRY_CHG_QTY,@SEC_AFT_QTY,@SEC_CHG_QTY," +
                    "@CATEGORY,@ACTION,@DOC,@CREATED_BY,@code output, @message output, @user",
                    pStockId, pPryAftQty, pPryChgQty, pSecAftQty, pSecChgQty, pCategory, pAction, pDoc, pCreatedby, pCode, pMsg, pUser);
            }                                                                            
                                                                                    
        }

        public void CheckStock(long StockId,string userid)
        {
            var stockid = stockTRepositiory.Get(x => x.StockId == StockId).SingleOrDefault();
            STOCK_T stock = new STOCK_T();
            stock.LastUpdateBy = userid;
            stock.LastUpdateDate = DateTime.Now;

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

            var batchNo = OspHeaderTRepositiory.GetAll().GroupBy(x => x.BatchNo)
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
        public List<SelectListItem> GetLocator(long OspDetailOutId)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    var DetailOut = OspDetailOutTRepositiory.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                    List<SelectListItem> locator = new List<SelectListItem>();
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
[SEGMENT3] as Text,
cast([LOCATOR_ID] as nvarchar) as Value
FROM [LOCATOR_T] LT
where CONTROL_FLAG <> 'D'
and SUBINVENTORY_CODE = @SUBINVENTORY_CODE");
                    sqlParameterList.Add(new SqlParameter("@SUBINVENTORY_CODE", DetailOut.Subinventory));
                    var data = mesContext.Database.SqlQuery<SelectListItem>(query.ToString(), sqlParameterList.ToArray()).ToList();
                    if (data == null)
                    {
                        locator.AddRange(data);
                    }
                    else
                    {
                        locator.AddRange(data);
                    }
                    return locator;
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// 狀態
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
                    Value = "待排單",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "已排單",
                    Value = "已排單",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "待核准",
                    Value = "待核准",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "已完工",
                    Value = "已完工",
                    Selected = false,
                },
                new SelectListItem()
                {
                    Text = "關帳",
                    Value = "關帳",
                    Selected = false,
                }
            };
            return BatchStatusDesc;
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
    }
}