using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Process;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.Process;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Graph;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
                oSP.PeCreatedBy = 1;
                oSP.PeCreationDate = DateTime.Now;
                oSP.PeLastUpdateBy = 1;
                oSP.PeLastUpdateDate = DateTime.Now;
                oSP.LineType = "I";
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
                oSP.Subinventory = "REVT";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 1;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 30;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 30;
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = 1;
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = 1;
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
                oSP.PeCreatedBy = 1;
                oSP.PeCreationDate = DateTime.Now;
                oSP.PeLastUpdateBy = 1;
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
                oSP.Subinventory = "REVT";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 1;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 30;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 30;
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = 1;
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = 1;
                oSP.LastUpdateDate = DateTime.Now;
                OspOrgTRepositiory.Create(oSP, true);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
            }
        }

        public void OspOrgToDetail()
        {
            OSP_HEADER_T OspHeaderT = new OSP_HEADER_T();
            OSP_DETAIL_IN_T OspDetailInT = new OSP_DETAIL_IN_T();
            OSP_DETAIL_OUT_T OspDetailOutT = new OSP_DETAIL_OUT_T();
            var org = OspOrgTRepositiory.GetAll().AsNoTracking().ToList();

            try
            {
                for(int i = 0; org.Count() > i; i++)
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
                        OspHeaderT.Stauts = "待排單";
                        OspHeaderT.Modifications = 0;
                        OspHeaderTRepositiory.Create(OspHeaderT, true);
                    }else if (batch.BatchNo != org[i].BatchNo)
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
                        OspHeaderT.Stauts = "待排單";
                        OspHeaderT.Modifications = 0;
                        OspHeaderTRepositiory.Create(OspHeaderT, true);
                    }


                    if(org[i].LineType == "I")
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

                    if(org[i].LineType == "P")
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
            }catch(Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
            }
        }

        public List<CHP_PROCESS_T> GetTable(string Status, string BatchNo, string MachineCode, string DueDate, string CuttingDateFrom, string CuttingDateTo, string Subinventory)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
DI.OSP_HEADER_ID,
DI.OSP_DETAIL_IN_ID,
H.STATUS,
H.BATCH_NO,
H.BATCH_TYPE,
H.DUE_DATE,
H.CUTTING_DATE_FROM,
H.CUTTING_DATE_TO,
H.MACHINE_CODE,
DI.CUSTOMER_NAME,
DI.ORDER_NUMBER,
DI.ORDER_LINE_NUMBER,
DI.BASIC_WEIGHT,
DI.SPECIFICATION,
DI.GRAIN_DIRECTION,
DI.ORDER_WEIGHT,
DI.REAM_WT,
DI.PRIMARY_QUANTITY,
DI.PRIMARY_UOM,
DI.TRANSACTION_QUANTITY,
DI.TRANSACTION_UOM,
DI.PACKING_TYPE,
DI.PAPER_TYPE,
DI.OSP_REMARK,
DI.INVENTORY_ITEM_NUMBER,
DO.INVENTORY_ITEM_NUMBER,
H.NOTE,
DI.SUBINVENTORY,
DI.LOCATOR_CODE,
OYV.LOSS_WEIGHT,
DI.CREATED_BY,
DI.CREATED_BY,
DI.LAST_UPDATE_BY,
DI.LAST_UPDATE_DATE
FROM [OSP_HEADER_T] H
JOIN OSP_DETAIL_IN_T DI ON DI.OSP_HEADER_ID = H.OSP_HEADER_ID
JOIN OSP_DETAIL_OUT_T DO ON DO.OSP_HEADER_ID = H.OSP_HEADER_ID
left JOIN OSP_YIELD_VARIANCE_T OYV ON OYV.OSP_HEADER_ID = H.OSP_HEADER_ID");
                    return mesContext.Database.SqlQuery<CHP_PROCESS_T>(query.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<CHP_PROCESS_T>();
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

  
    }
}