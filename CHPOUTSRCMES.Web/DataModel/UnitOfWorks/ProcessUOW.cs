using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Process;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Process;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Ajax.Utilities;
using Microsoft.Graph;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
                oSP.ProcessCode = "XXIFP219";
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

                oSP.ProcessCode = "XXIFP219";
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
                oSP.BasicWeight = "02000";
                oSP.Specification = "889RL00";
                oSP.GrainDirection = "S";
                oSP.ReamWt = "20";
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


                //代紙平張

                oSP.ProcessCode = "XXIFP219";
                oSP.ServerCode = "124";
                oSP.BatchId = "20201112141600100000";
                oSP.BatchLineId = 2;
                oSP.PeBatchId = 2;
                oSP.BatchNo = "P100000";
                oSP.BatchType = "TMP";
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
                oSP.LineNo = 2;
                oSP.InventoryItemId = 504124;
                oSP.InventoryItemNumber = "4DM00P02700252K512K";
                oSP.PackingType = "無令打件";
                oSP.PaperType = "DM00";
                oSP.OrderWeight = "27";
                oSP.BasicWeight = "2700";
                oSP.Specification = "252K512K";
                oSP.GrainDirection = "S";
                oSP.ReamWt = "27";
                oSP.PlanQty = 2;
                oSP.WipPLAN_QTY = 3;
                oSP.DtlUom = "RE";
                oSP.OrderHeaderId = 2;
                oSP.OrderNumber = 654321;
                oSP.OrderLineId = 2;
                oSP.OrderLineNumber = "9536852";
                oSP.CustomerId = 2;
                oSP.CustomerNumber = "321";
                oSP.CustomerName = "中華彩色印刷股份有限公司";
                oSP.PrNumber = 2;
                oSP.PrLineNumber = 2;
                oSP.RequisitionLineId = 2;
                oSP.PoNumber = 2;
                oSP.PoLineNumber = 2;
                oSP.PoLineId = 2;
                oSP.PoUnitPrice = 40;
                oSP.PoRevisionNum = 2;
                oSP.PoStatus = "待印";
                oSP.PoVendorNum = "2";
                oSP.OspRemark = "N11/25入倉";
                oSP.Subinventory = "TB3";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 2;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 2000;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 2000;
                oSP.SecondaryQuantity = 1000;
                oSP.SecondaryUom = "RE";
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = "1";
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = "1";
                oSP.LastUpdateDate = DateTime.Now;
                OspOrgTRepositiory.Create(oSP, true);


                oSP.ProcessCode = "XXIFP219";
                oSP.ServerCode = "124";
                oSP.BatchId = "20201112141600100000";
                oSP.BatchLineId = 2;
                oSP.PeBatchId = 2;
                oSP.BatchNo = "P100000";
                oSP.BatchType = "TMP";
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
                oSP.LineNo = 2;
                oSP.InventoryItemId = 504125;
                oSP.InventoryItemNumber = "4DM00P02700330K512K";
                oSP.PackingType = "無令打件";
                oSP.PaperType = "DM00";
                oSP.OrderWeight = "27";
                oSP.BasicWeight = "2700";
                oSP.Specification = "330K512K";
                oSP.GrainDirection = "S";
                oSP.ReamWt = "27";
                oSP.PlanQty = 2;
                oSP.WipPLAN_QTY = 3;
                oSP.DtlUom = "RE";
                oSP.OrderHeaderId = 2;
                oSP.OrderNumber = 654321;
                oSP.OrderLineId = 2;
                oSP.OrderLineNumber = "9536852";
                oSP.CustomerId = 2;
                oSP.CustomerNumber = "321";
                oSP.CustomerName = "中華彩色印刷股份有限公司";
                oSP.PrNumber = 2;
                oSP.PrLineNumber = 2;
                oSP.RequisitionLineId = 2;
                oSP.PoNumber = 2;
                oSP.PoLineNumber = 2;
                oSP.PoLineId = 2;
                oSP.PoUnitPrice = 40;
                oSP.PoRevisionNum = 2;
                oSP.PoStatus = "待印";
                oSP.PoVendorNum = "2";
                oSP.OspRemark = "N11/25入倉";
                oSP.Subinventory = "TB3";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 2;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 2000;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 2000;
                oSP.SecondaryQuantity = 1000;
                oSP.SecondaryUom = "RE";
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = "1";
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = "1";
                oSP.LastUpdateDate = DateTime.Now;
                OspOrgTRepositiory.Create(oSP, true);

                //代紙平張

                oSP.ProcessCode = "XXIFP219";
                oSP.ServerCode = "124";
                oSP.BatchId = "20201112141600100000";
                oSP.BatchLineId = 3;
                oSP.PeBatchId = 3;
                oSP.BatchNo = "K100000";
                oSP.BatchType = "TMP";
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
                oSP.LineNo = 2;
                oSP.InventoryItemId = 558789;
                oSP.InventoryItemNumber = "4AH00C01055294KRL00";
                oSP.PackingType = "";
                oSP.PaperType = "AH00";
                oSP.OrderWeight = "22";
                oSP.BasicWeight = "1055";
                oSP.Specification = "294KRL00";
                oSP.GrainDirection = "A";
                oSP.ReamWt = "22";
                oSP.PlanQty = 3;
                oSP.WipPLAN_QTY = 4;
                oSP.DtlUom = "RE";
                oSP.OrderHeaderId = 32;
                oSP.OrderNumber = 9513542;
                oSP.OrderLineId = 3;
                oSP.OrderLineNumber = "1111536";
                oSP.CustomerId = 3;
                oSP.CustomerNumber = "321";
                oSP.CustomerName = "彩色印刷股份有限公司";
                oSP.PrNumber = 3;
                oSP.PrLineNumber = 3;
                oSP.RequisitionLineId = 3;
                oSP.PoNumber = 3;
                oSP.PoLineNumber = 3;
                oSP.PoLineId = 3;
                oSP.PoUnitPrice = 50;
                oSP.PoRevisionNum = 3;
                oSP.PoStatus = "待印";
                oSP.PoVendorNum = "3";
                oSP.OspRemark = "N11/25入倉";
                oSP.Subinventory = "TB3";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 2;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 3000;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 3000;
                oSP.PrimaryUom = "KG";
                oSP.CreatedBy = "1";
                oSP.CreationDate = DateTime.Now;
                oSP.LastUpdateBy = "1";
                oSP.LastUpdateDate = DateTime.Now;
                OspOrgTRepositiory.Create(oSP, true);


                oSP.ProcessCode = "XXIFP219";
                oSP.ServerCode = "124";
                oSP.BatchId = "20201112141600100000";
                oSP.BatchLineId = 3;
                oSP.PeBatchId = 3;
                oSP.BatchNo = "K100000";
                oSP.BatchType = "TMP";
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
                oSP.LineNo = 2;
                oSP.InventoryItemId = 558790;
                oSP.InventoryItemNumber = "4AH00E01000340KRL00";
                oSP.PackingType = "";
                oSP.PaperType = "AH00";
                oSP.OrderWeight = "22";
                oSP.BasicWeight = "1000";
                oSP.Specification = "340KRL00";
                oSP.GrainDirection = "C";
                oSP.ReamWt = "22";
                oSP.PlanQty = 3;
                oSP.WipPLAN_QTY = 4;
                oSP.DtlUom = "RE";
                oSP.OrderHeaderId = 3;
                oSP.OrderNumber = 4357213;
                oSP.OrderLineId = 3;
                oSP.OrderLineNumber = "4357213";
                oSP.CustomerId = 3;
                oSP.CustomerNumber = "753";
                oSP.CustomerName = "彩色印刷股份有限公司";
                oSP.PrNumber = 3;
                oSP.PrLineNumber = 3;
                oSP.RequisitionLineId = 3;
                oSP.PoNumber = 3;
                oSP.PoLineNumber = 3;
                oSP.PoLineId = 3;
                oSP.PoUnitPrice = 50;
                oSP.PoRevisionNum = 3;
                oSP.PoStatus = "待印";
                oSP.PoVendorNum = "3";
                oSP.OspRemark = "N11/25入倉";
                oSP.Subinventory = "TB3";
                oSP.LocatorId = 23265;
                oSP.LocatorCode = "FTY.REVT.A11.B12";
                oSP.ReservationUomCode = "RE";
                oSP.ReservationQuantity = 3;
                oSP.LineCreatedBy = 1;
                oSP.LineCreationDate = DateTime.Now;
                oSP.LineLastUpdateBy = 1;
                oSP.LineLastUpdateDate = DateTime.Now;
                oSP.TransactionQuantity = 3000;
                oSP.TransactionUom = "KG";
                oSP.PrimaryQuantity = 3000;
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
                        OspHeaderT.CreatedBy = "SYS";
                        OspHeaderT.CreationDate = DateTime.Now;
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
                        OspHeaderT.CreatedBy = "SYS";
                        OspHeaderT.CreationDate = DateTime.Now;
                        OspHeaderTRepositiory.Create(OspHeaderT, true);
                    }


                    if (org[i].LineType == "I")
                    {
                        OspDetailInT.OspHeaderId = OspHeaderT.OspHeaderId;
                        OspDetailInT.ProcessCode = org[i].ProcessCode;
                        OspDetailInT.ServerCode = org[i].ServerCode;
                        OspDetailInT.BatchId = org[i].BatchId;
                        OspDetailInT.BatchLineId = org[i].BatchLineId;
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
                        OspDetailOutT.ProcessCode = org[i].ProcessCode;
                        OspDetailOutT.ServerCode = org[i].ServerCode;
                        OspDetailOutT.BatchId = org[i].BatchId;
                        OspDetailOutT.BatchLineId = org[i].BatchLineId;
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
DO.TRANSACTION_QUANTITY AS ReamWt,
DO.PRIMARY_QUANTITY AS PrimaryQuantity,
DO.PRIMARY_UOM AS PrimaryUom,
DO.TRANSACTION_UOM AS TransactionUom,
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
WHERE DI.OSP_HEADER_ID = @OSP_HEADER_ID");
                    string commandText = string.Format(query.ToString());
                    commandText = string.Concat(commandText, " UNION ", commandText.Replace("OSP_DETAIL_IN_T", "OSP_DETAIL_IN_HT").Replace("OSP_DETAIL_OUT_T", "OSP_DETAIL_OUT_HT"));
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
DO.TRANSACTION_QUANTITY AS ReamWt,
DO.PRIMARY_QUANTITY AS PrimaryQuantity,
DO.PRIMARY_UOM AS PrimaryUom,
DO.TRANSACTION_UOM AS TransactionUom,
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
        /// 投入條碼
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
                        var id = OspPickedInTRepositiory.Get(x => x.OspPickedInId == InvestDTListId.OspPickedInId).SingleOrDefault();
                        var stock = stockTRepositiory.Get(x => x.StockId == id.StockId).SingleOrDefault();
                        var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                        if (id != null)
                        {
                            id.HasRemaint = InvestDTListId.HasRemaint;
                            id.RemainingQuantity = InvestDTListId.RemainingQuantity;
                            id.LastUpdateBy = UserId;
                            id.LastUpdateUserName = UserName;
                            id.LastUpdateDate = DateTime.Now;
                            OspPickedInTRepositiory.Update(id, true);
                            var aft = InvestDTListId.RemainingQuantity;
                            var chg = stock.PrimaryTransactionQty - InvestDTListId.RemainingQuantity;
                            if (stock.PrimaryTransactionQty >= aft)
                            {
                                CheckStock(stock.StockId, UserId, aft ?? 0, ActionCode.Picked);
                                StockRecord(id.StockId, aft ?? 0, chg ?? 0, 0, 0, CategoryCode.Process, ActionCode.Picked, header.BatchNo, UserId);
                                txn.Commit();
                                return new ResultModel(true, "");
                            }
                            else
                            {
                                txn.Rollback();
                                return new ResultModel(false, "庫存數量不對");
                            }


                        }
                    }

                    if (InvestDTList.Action == "remove")
                    {
                        var id = OspPickedInTRepositiory.Get(x => x.OspPickedInId == InvestDTListId.OspPickedInId).SingleOrDefault();
                        var stock = stockTRepositiory.Get(x => x.StockId == id.StockId).SingleOrDefault();
                        var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == id.OspHeaderId).SingleOrDefault();
                        if (id != null)
                        {
                            OspPickedInTRepositiory.Delete(id, true);
                            CheckStock(stock.StockId, UserId, stock.PrimaryTransactionQty, ActionCode.Picked);
                            StockRecord(id.StockId, stock.PrimaryTransactionQty, 0, 0, 0, CategoryCode.Process, ActionCode.Picked, header.BatchNo, UserId);
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
                    var data = Context.Database.SqlQuery<STOCK_T>(query.ToString(), sqlParameterList.ToArray()).SingleOrDefault();

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
                        var aft = Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight);
                        var chg = data.PrimaryAvailableQty - (Remaining_Weight == "" ? 0 : decimal.Parse(Remaining_Weight));
                        if (data.PrimaryAvailableQty >= aft)
                        {
                            CheckStock(data.StockId, UserId, aft, ActionCode.Picked);
                            StockRecord(data.StockId, aft, chg, 0, 0, CategoryCode.Process, ActionCode.Picked, Header.BatchNo, UserId);
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
                var relate = OspDetailOutTRepositiory.Get().
                    Join(relatedTRepositiory.GetAll(),
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

                if(relate == null)
                {
                    return new ResultModel(false, "無餘切資料");
                }
                //尋找餘切
                var Relateitem = itemsTRepositiory.Get(x => x.InventoryItemId == relate.b.RelatedItemId).SingleOrDefault();


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
        public ResultModel PaperRollCreateProduction(string PaperRoll_Basic_Weight, string PaperRoll_Specification, string PaperRoll_Lot_Number, long OspDetailOutId, string UserId, string UserName)
        {
            if (PaperRoll_Basic_Weight == null)
            {
                return new ResultModel(false, "令數不得空白");
            }
            if (PaperRoll_Specification == null)
            {
                return new ResultModel(false, "棧板數不得空白");
            }
            if (PaperRoll_Lot_Number == null)
            {
                return new ResultModel(false, "捲號不得空白");
            }
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var detailout = OspDetailOutTRepositiory.Get(x => x.OspDetailOutId == OspDetailOutId).SingleOrDefault();
                    var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == detailout.OspHeaderId).SingleOrDefault();
                    if (detailout == null)
                    {
                        return new ResultModel(false, "ID錯誤");
                    }
                    OSP_PICKED_OUT_T ospPickOut = new OSP_PICKED_OUT_T();
                    OSP_COTANGENT_T ospCotanget = new OSP_COTANGENT_T();

                    ospPickOut.OspDetailOutId = OspDetailOutId;
                    ospPickOut.OspHeaderId = detailout.OspHeaderId;
                    ospPickOut.StockId = null;
                    ospPickOut.Barcode = GenerateBarcodes(header.OrganizationId, detailout.Subinventory, 1, UserName).Data[0];
                    ospPickOut.InventoryItemId = detailout.InventoryItemId;
                    ospPickOut.InventoryItemNumber = detailout.InventoryItemNumber;
                    ospPickOut.BasicWeight = detailout.BasicWeight;
                    ospPickOut.Specification = detailout.Specification;
                    ospPickOut.PaperType = detailout.PaperType;
                    ospPickOut.LotNumber = PaperRoll_Lot_Number;
                    ospPickOut.PrimaryQuantity = decimal.Parse(PaperRoll_Basic_Weight);
                    ospPickOut.PrimaryUom = "KG";
                    ospPickOut.SecondaryQuantity = 0;
                    ospPickOut.SecondaryUom = "";
                    ospPickOut.Status = "待入庫";
                    ospPickOut.Cotangent = "N";
                    ospPickOut.OspCotangentId = ospCotanget.OspCotangentId;
                    ospPickOut.CreatedBy = UserId;
                    ospPickOut.CreatedUserName = UserName;
                    ospPickOut.CreationDate = DateTime.Now;
                    OspPickedOutTRepositiory.Create(ospPickOut, true);
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
        /// 取得產出檢貨picket
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
                        var id = OspPickedOutTRepositiory.Get(r => r.OspPickedOutId == OspPickedOutId).SingleOrDefault();
                        if (id != null)
                        {
                            id.PrimaryQuantity = ProductionDTEditor.ProductionList[0].PrimaryQuantity;
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
[LOSS_WEIGHT] as LossWeight,
[PRIMARY_UOM],
[RATE] as Rate
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
        /// 存檔入庫&&工單號狀態更改
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <param name="Locator"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel ChangeHeaderStauts(long OspHeaderId, long Locator, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailOut = OspDetailOutTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailIn = OspDetailInTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();

                    if (header == null)
                    {
                        return new ResultModel(false, "找不到id");
                    }

                    if (header.Status == "已完工")
                    {
                        header.Status = "待核准";
                        header.PeLastUpdateBy = UserId;
                        header.PeLastUpdateDate = DateTime.Now;
                        OspHeaderTRepositiory.Update(header, true);
                    }
                    else if (header.Status == "待核准")
                    {
                        header.Status = "已完工";
                        header.PeLastUpdateBy = UserId;
                        header.PeLastUpdateDate = DateTime.Now;
                        header.Modifications = 1;
                        OspHeaderTRepositiory.Update(header, true);
                        SaveStock(header.OspHeaderId, StockStatusCode.InStock);
                        PickInToHt(header.OspHeaderId);
                        PirckOutToHt(header.OspHeaderId);
                        DetailInToHt(header.OspHeaderId);
                        DetailOutToHt(header.OspHeaderId);
                        ContangetToHt(header.OspHeaderId);
                        YieldToHt(header.OspHeaderId);
                    }
                    else
                    {
                        header.Status = "已完工";
                        header.PeLastUpdateBy = UserId;
                        header.PeLastUpdateDate = DateTime.Now;
                        OspHeaderTRepositiory.Update(header, true);
                        SaveStock(header.OspHeaderId, new StockStatusCode().GetDesc(StockStatusCode.InStock));
                        PickInToHt(header.OspHeaderId);
                        PirckOutToHt(header.OspHeaderId);
                        DetailInToHt(header.OspHeaderId);
                        DetailOutToHt(header.OspHeaderId);
                        ContangetToHt(header.OspHeaderId);
                        YieldToHt(header.OspHeaderId);
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
        /// 完工紀錄工單號狀態更改
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel EditHeaderStauts(long OspHeaderId, string UserId, string UserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var header = OspHeaderTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailOut = OspDetailOutTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    var DetailIn = OspDetailInTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();

                    if (header == null)
                    {
                        return new ResultModel(false, "找不到id");
                    }

                    header.Status = "待核准";
                    header.PeLastUpdateBy = UserId;
                    header.PeLastUpdateDate = DateTime.Now;
                    OspHeaderTRepositiory.Update(header, true);

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
        public ResultModel FinisheEdit(string BatchNo, long OspHeaderId)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var batchno = OspHeaderTRepositiory.Get(x => x.OspHeaderId == OspHeaderId).SingleOrDefault();
                    if (batchno == null)
                    {
                        return new ResultModel(false, "單號錯誤");
                    }
                    if (batchno.BatchNo != BatchNo)
                    {
                        return new ResultModel(false, "工單號輸入不對請重新輸入");
                    }

                    if (batchno.Status == "已完工")
                    {
                        if (batchno.Modifications == 1)
                        {
                            return new ResultModel(false, "已修改過，無法再修改");
                        }
                        HtToTStockDelete(OspHeaderId);
                        PickInHtToT(OspHeaderId);
                        PirckOutHtToT(OspHeaderId);
                        DetailInHtToT(OspHeaderId);
                        DetailOutHtToT(OspHeaderId);
                        ContangetHtToT(OspHeaderId);
                        YieldHtToT(OspHeaderId);
                        txn.Commit();
                    }

                    return new ResultModel(true, "成功");

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
        /// 轉入新增庫存
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
            this.Context.Database.ExecuteSqlCommand("[SP_SaveCotangentStock] @headerId, @statusCode ,@code output, @message output, @user",
                pheaderId, pstatusCode, pCode, pMsg, pUser);
        }


        /// <summary>
        /// 庫存異動紀錄
        /// </summary>
        /// <param name="Barcode"></param>
        public void StockRecord(long StockId, decimal PryAftQty, decimal PryChgQty, decimal SecAftQty, decimal SecChgQty,
            string Category, string Action, string Doc, string Createdby)
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
            Context.Database.ExecuteSqlCommand("[SP_ProcessSaveStkTxn] @StockId, @PRY_AFT_QTY ,@PRY_CHG_QTY,@SEC_AFT_QTY,@SEC_CHG_QTY," +
                "@CATEGORY,@ACTION,@DOC,@CREATED_BY,@code output, @message output, @user",
                pStockId, pPryAftQty, pPryChgQty, pSecAftQty, pSecChgQty, pCategory, pAction, pDoc, pCreatedby, pCode, pMsg, pUser);

        }

        /// <summary>
        /// 檢查庫存
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userid"></param>
        /// <param name="afg"></param>
        /// <param name="StatusCode"></param>
        public void CheckStock(long StockId, string userid, decimal aft, string StatusCode)
        {
            var stockid = stockTRepositiory.Get(x => x.StockId == StockId).SingleOrDefault();
            stockid.PrimaryAvailableQty = aft;
            stockid.PrimaryLockedQty = stockid.PrimaryTransactionQty - aft;
            stockid.StatusCode = StatusCode;
            stockid.LastUpdateBy = userid;
            stockid.LastUpdateDate = DateTime.Now;
            stockTRepositiory.Update(stockid, true);
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
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[HAS_REMAINT],[REMAINING_QUANTITY],[REMAINING_UOM],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_PICKED_IN_ID],[OSP_DETAIL_IN_ID] ,[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
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
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[COTANGENT],[OSP_COTANGENT_ID],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_PICKED_OUT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
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
([OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
[OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
([OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],
[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_COTANGENT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
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
@"INSERT INTO [OSP_YIELD_VARIANCE_HT]
([OSP_YIELD_VARIANCE_ID],[OSP_HEADER_ID],[DETAIL_IN_QUANTITY],[COTANGENT_QUANTITY],[DETAIL_OUT_QUANTITY],
[LOSS_WEIGHT],[PRIMARY_UOM],[RATE],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT
[OSP_YIELD_VARIANCE_ID],[OSP_HEADER_ID],[DETAIL_IN_QUANTITY],[COTANGENT_QUANTITY],[DETAIL_OUT_QUANTITY],
[LOSS_WEIGHT],[PRIMARY_UOM],[RATE],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM [OSP_YIELD_VARIANCE_T]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_YIELD_VARIANCE_T WHERE OSP_HEADER_ID = @OSP_HEADER_ID");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }






        /// <summary>
        /// PICKIN歷史轉撿貨&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void PickInHtToT(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"
SET IDENTITY_INSERT [OSP_PICKED_IN_T] ON
INSERT INTO [OSP_PICKED_IN_T]
([OSP_PICKED_IN_ID],[OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[HAS_REMAINT],[REMAINING_QUANTITY],[REMAINING_UOM],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_PICKED_IN_ID],[OSP_DETAIL_IN_ID] ,[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[HAS_REMAINT],[REMAINING_QUANTITY] ,[REMAINING_UOM],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM [OSP_PICKED_IN_HT] 
where OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_PICKED_IN_HT WHERE OSP_HEADER_ID = @OSP_HEADER_ID
SET IDENTITY_INSERT [OSP_PICKED_IN_T] OFF");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// PickOut歷史轉撿貨&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void PirckOutHtToT(long OSP_HEADER_ID)
        {

            StringBuilder query = new StringBuilder();
            query.Append(
@"
SET IDENTITY_INSERT [OSP_PICKED_OUT_T] ON
INSERT INTO [OSP_PICKED_OUT_T]
([OSP_PICKED_OUT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[COTANGENT],[OSP_COTANGENT_ID],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_PICKED_OUT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[COTANGENT],[OSP_COTANGENT_ID],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM　[OSP_PICKED_OUT_HT]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = @OSP_HEADER_ID
SET IDENTITY_INSERT [OSP_PICKED_OUT_T] OFF");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 投入歷史轉投入明細&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void DetailInHtToT(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"
SET IDENTITY_INSERT [OSP_DETAIL_IN_T] ON
INSERT INTO [OSP_DETAIL_IN_T]
([OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
[OSP_DETAIL_IN_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
FROM [OSP_DETAIL_IN_HT]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
DELETE OSP_DETAIL_IN_HT WHERE OSP_HEADER_ID = @OSP_HEADER_ID
SET IDENTITY_INSERT [OSP_DETAIL_IN_T] OFF");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }
        /// <summary>
        /// 產出歷史轉產出明細&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void DetailOutHtToT(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"
SET IDENTITY_INSERT [OSP_DETAIL_OUT_T] ON
INSERT INTO [OSP_DETAIL_OUT_T]
([OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[LINE_TYPE],[LINE_NO],[INVENTORY_ITEM_ID],
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
FROM [OSP_DETAIL_OUT_HT]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
DELETE OSP_DETAIL_OUT_HT WHERE OSP_HEADER_ID = @OSP_HEADER_ID
SET IDENTITY_INSERT [OSP_DETAIL_OUT_T] OFF");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 餘切歷史轉餘切明細&刪除歷史
        /// </summary>
        public void ContangetHtToT(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"
SET IDENTITY_INSERT [OSP_COTANGENT_T] ON
INSERT INTO [OSP_COTANGENT_T]
([OSP_COTANGENT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],
[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT 
[OSP_COTANGENT_ID],[OSP_DETAIL_OUT_ID],[OSP_HEADER_ID],[STOCK_ID],[BARCODE],
[INVENTORY_ITEM_ID],[INVENTORY_ITEM_NUMBER],[PAPER_TYPE],[BASIC_WEIGHT],[SPECIFICATION],
[LOT_NUMBER],[PRIMARY_QUANTITY],[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
[STATUS],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],
[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM [OSP_COTANGENT_HT]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_COTANGENT_HT WHERE OSP_HEADER_ID = @OSP_HEADER_ID
SET IDENTITY_INSERT [OSP_COTANGENT_T] OFF
");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 損耗歷史轉損耗明細&刪除歷史
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void YieldHtToT(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"
SET IDENTITY_INSERT [OSP_YIELD_VARIANCE_T] ON
INSERT INTO [OSP_YIELD_VARIANCE_T]
([OSP_YIELD_VARIANCE_ID],[OSP_HEADER_ID],[DETAIL_IN_QUANTITY],[COTANGENT_QUANTITY],[DETAIL_OUT_QUANTITY],
[LOSS_WEIGHT],[PRIMARY_UOM],[RATE],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT
[OSP_YIELD_VARIANCE_ID],[OSP_HEADER_ID],[DETAIL_IN_QUANTITY],[COTANGENT_QUANTITY],[DETAIL_OUT_QUANTITY],
[LOSS_WEIGHT],[PRIMARY_UOM],[RATE],[CREATED_BY],[CREATED_USER_NAME],
[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM [OSP_YIELD_VARIANCE_HT]
WHERE OSP_HEADER_ID = @OSP_HEADER_ID
Delete OSP_YIELD_VARIANCE_HT WHERE OSP_HEADER_ID = @OSP_HEADER_ID
SET IDENTITY_INSERT [OSP_YIELD_VARIANCE_T] OFF");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
        }

        /// <summary>
        /// 歷史轉修改資料庫存資料刪除
        /// </summary>
        /// <param name="OSP_HEADER_ID"></param>
        public void HtToTStockDelete(long OSP_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"DELETE ST FROM STOCK_T ST
JOIN OSP_PICKED_OUT_HT POH ON POH.STOCK_ID = ST.STOCK_ID
WHERE POH.OSP_HEADER_ID = @OSP_HEADER_ID

DELETE ST FROM STOCK_T ST
JOIN OSP_COTANGENT_HT CHT ON CHT.STOCK_ID = ST.STOCK_ID
WHERE CHT.OSP_HEADER_ID = @OSP_HEADER_ID");
            Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@OSP_HEADER_ID", OSP_HEADER_ID));
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
CAST(ST.SECONDARY_UOM_CODE AS nvarchar) AS Unit
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
CAST(ST.PRIMARY_UOM_CODE AS nvarchar) AS Unit
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
CAST(OPO.SECONDARY_UOM AS nvarchar) AS Unit
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
CAST(FORMAT(OCT.PRIMARY_QUANTITY,'0.##########') AS nvarchar) AS Qty,
CAST(OCT.PRIMARY_UOM AS nvarchar) AS Unit
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
CAST(OPO.PRIMARY_UOM AS nvarchar) AS Unit
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
    }
}