using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Process;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Graph;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private readonly IRepository<OSP_ORG_T> Osp_orgTRepositiory;
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
            this.Osp_orgTRepositiory = new GenericRepository<OSP_ORG_T>(this);
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
                oSP.DueDate = Convert.ToDateTime("2020/09/17");
                oSP.PlanStartDate = Convert.ToDateTime("2020/09/17");
                oSP.PlanCmpltDate = Convert.ToDateTime("2020/09/17");
                oSP.PeCreatedBy = 1;
                oSP.PeCreationDate = DateTime.Now;
                oSP.PeLastUpdateBy = 1;
                oSP.PeLastUpdateDate = DateTime.Now;
                oSP.LineType = "I";
                oSP.LineNo = 1;
                oSP.InventoryItemId = 728485;
                oSP.InventoryItemNumber = "4FU0SA020000889RL00";
                oSP.BasicWeight = "020000";
                oSP.Specification = "889RL00";
                oSP.GrainDirection = "11111";
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
                oSP.LocatorCode = "A11";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 1;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 10;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 30;
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = 1;
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = 1;
                oSP.LastUpdateDate = DateTime.Now; 
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
            }
        }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBatchStatusDesc()
        {
            List<SelectListItem> BatchStatusDesc = new List<SelectListItem>();


            BatchStatusDesc.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            BatchStatusDesc.Add(new SelectListItem()
            {
                Text = "待排單",
                Value = "待排單",
                Selected = false,
            });
            BatchStatusDesc.Add(new SelectListItem()
            {
                Text = "已排單",
                Value = "已排單",
                Selected = false,
            });
            BatchStatusDesc.Add(new SelectListItem()
            {
                Text = "待核准",
                Value = "待核准",
                Selected = false,
            });
            BatchStatusDesc.Add(new SelectListItem()
            {
                Text = "已完工",
                Value = "已完工",
                Selected = false,
            });
            BatchStatusDesc.Add(new SelectListItem()
            {
                Text = "關帳",
                Value = "關帳",
                Selected = false,
            });
            return BatchStatusDesc;
        }

  
    }
}