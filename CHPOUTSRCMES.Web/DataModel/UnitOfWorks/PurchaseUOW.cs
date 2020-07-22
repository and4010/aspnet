using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class PurchaseUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<CTR_ORG_T> ctrOrgTRepositiory;
        private readonly IRepository<CTR_HEADER_T> ctrHeaderTRepositiory;
        private readonly IRepository<CTR_DETAIL_T> ctrDetailTRepositiory;
        private readonly IRepository<CTR_PICKED_T> ctrPickedTRepositiory;
        private readonly IRepository<CTR_PICKED_HT> ctrPickedHtRepositiory;

        //還有幾個TABLE未加入

                /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PurchaseUOW(DbContext context) : base(context)
        {
            this.ctrOrgTRepositiory = new GenericRepository<CTR_ORG_T>(this);
            this.ctrHeaderTRepositiory = new GenericRepository<CTR_HEADER_T>(this);
            this.ctrDetailTRepositiory = new GenericRepository<CTR_DETAIL_T>(this);
            this.ctrPickedTRepositiory = new GenericRepository<CTR_PICKED_T>(this);
            this.ctrPickedHtRepositiory = new GenericRepository<CTR_PICKED_HT>(this);
        }


        public List<FullCalendarEventModel> getFullCalenderList()
        {
            var header = ctrHeaderTRepositiory.GetAll().ToList();

            List<FullCalendarEventModel> fullCalendarEventModel = new List<FullCalendarEventModel>();
            UrlHelper objUrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            for (int i = 0; i < header.Count; i++)
            {
                if (header[i].Status == 1)
                {
                    fullCalendarEventModel.Add(new FullCalendarEventModel()
                    {
                        id = header[i].CtrHeaderId,
                        title = header[i].Subinventory + "\n" + header[i].ContainerNo + " 待入庫",
                        start = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        end = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        allDay = false,
                        url = objUrlHelper.Action("Detail", "Purchase", new
                        {
                            CONTAINER_NO = header[i].ContainerNo,
                            Start = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                            Status = header[i].Status,
                            Subinventory = header[i].Subinventory
                        }),
                        Status = header[i].Status
                    });
                }
                if (header[i].Status == 2)
                {
                    fullCalendarEventModel.Add(new FullCalendarEventModel()
                    {
                        id = header[i].CtrHeaderId,
                        title = header[i].Subinventory + "\n" + header[i].ContainerNo + "取消",
                        start = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        end = ConvertDateTime.ConverYYYY(header[i].MvContainerDate),
                        allDay = false,
                        Status = header[i].Status,
                        color = "#E60000"
                    });
                }

            }
            return fullCalendarEventModel;
        }

        public void generateTestData()
        {
            this.Context.Configuration.AutoDetectChangesEnabled = false;
            using (this.Context.Database.BeginTransaction())
            {
                try
                {
                    generateTestDataCtrOrgT();
                    generateTestDataCtrHeaderDetail();

                    this.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                }
            }
            this.Context.Configuration.AutoDetectChangesEnabled = true;
        }

        private void generateTestDataCtrOrgT()
        {
            CTR_ORG_T ctrorg = new CTR_ORG_T();
            try
            {
                
                ctrorg.CtrOrgId = 1;
                ctrorg.ProcessCode = "XXIFP217";
                ctrorg.ServerCode = "123";
                ctrorg.BatchId = "20200721141600100000";
                ctrorg.BatchLineId = 1;
                ctrorg.HeaderId = 1;
                ctrorg.OrgId = 1;
                ctrorg.OrgName = "入庫";
                ctrorg.LineId = 1;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "SFG";
                ctrorg.LocatorId = 22016;
                ctrorg.LocatorCode = "SFG";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503117;
                ctrorg.ShipItemNumber = "4DM00E02700310K502K";
                ctrorg.PaperType = "DM00";
                ctrorg.BasicWeight = "02700";
                ctrorg.ReamWeight = "299.11";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "310K502K";
                ctrorg.PackingType = "令包";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 3;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 3000;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 3000;
                ctrorg.SecondaryUom = "RE";
                ctrorg.LotNumber = "";
                ctrorg.TheoryWeight = "";
                ctrorg.CreatedBy = 1;
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = 1;
                ctrorg.LastUpdateDate = DateTime.Now;
                
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

            ctrOrgTRepositiory.Create(ctrorg);
        }

        public void generateTestDataCtrHeaderDetail()
        {
            CTR_HEADER_T ctrheaderT = new CTR_HEADER_T();
            CTR_DETAIL_T ctrdetailT = new CTR_DETAIL_T();

            var org = ctrOrgTRepositiory.GetAll().AsNoTracking().ToList();

            try
            {
                for (int i = 0; org.Count() > i; i++)
                {

                    ctrheaderT.OrgId = org[i].OrgId;
                    ctrheaderT.OrgName = org[i].OrgName;
                    ctrheaderT.LineId = org[i].LineId;
                    ctrheaderT.ContainerNo = org[i].ContainerNo;
                    ctrheaderT.MvContainerDate = org[i].MvContainerDate;
                    ctrheaderT.OrganizationId = org[i].OrganizationId;
                    ctrheaderT.OrganizationCode = org[i].OrganizationCode;
                    ctrheaderT.Subinventory = org[i].Subinventory;
                    ctrheaderT.Status = 1;
                    ctrheaderT.CreatedBy = org[i].CreatedBy;
                    ctrheaderT.CreationDate = org[i].CreationDate;
                    ctrheaderT.LastUpdateBy = org[i].LastUpdateBy;
                    ctrheaderT.LastUpdateDate = org[i].LastUpdateDate;


                    ctrdetailT.CtrHeaderId = ctrheaderT.CtrHeaderId;
                    ctrdetailT.ProcessCode = org[i].ProcessCode;
                    ctrdetailT.ServerCode = org[i].ServerCode;
                    ctrdetailT.BatchId = org[i].BatchId;
                    ctrdetailT.BatchLineId = org[i].BatchLineId;
                    ctrdetailT.HeaderId = org[i].HeaderId;
                    ctrdetailT.LineId = org[i].LineId;
                    ctrdetailT.DetailId = org[i].DetailId;
                    ctrdetailT.LocatorId = org[i].LocatorId;
                    ctrdetailT.LocatorCode = org[i].LocatorCode;
                    ctrdetailT.InventoryItemId = org[i].InventoryItemId;
                    ctrdetailT.ShipItemNumber = org[i].ShipItemNumber;
                    ctrdetailT.PaperType = org[i].PaperType;
                    ctrdetailT.BasicWeight = org[i].BasicWeight;
                    ctrdetailT.ReamWeight = org[i].ReamWeight;
                    ctrdetailT.RollReamQty = org[i].RollReamQty;
                    ctrdetailT.RollReamWt = org[i].RollReamWt;
                    ctrdetailT.TtlRollReam = org[i].TtlRollReam;
                    ctrdetailT.Specification = org[i].Specification;
                    ctrdetailT.PackingType = org[i].PackingType;
                    ctrdetailT.ShipMtQty = org[i].ShipMtQty;
                    ctrdetailT.TransactionQuantity = org[i].TransactionQuantity;
                    ctrdetailT.TransactionUom = org[i].TransactionUom;
                    ctrdetailT.PrimaryQuantity = org[i].PrimaryQuantity;
                    ctrdetailT.PrimaryUom = org[i].PrimaryUom;
                    ctrdetailT.SecondaryQuantity = org[i].SecondaryQuantity;
                    ctrdetailT.SecondaryUom = org[i].SecondaryUom;
                    ctrdetailT.LotNumber = org[i].LotNumber;
                    ctrdetailT.TheoryWeight = org[i].TheoryWeight;
                    if (org[i].LotNumber == "")
                    {
                        ctrdetailT.ItemCategory = "平張";
                    }
                    else
                    {
                        ctrdetailT.ItemCategory = "捲筒";
                    }
                    ctrdetailT.CreatedBy = org[i].CreatedBy;
                    ctrdetailT.CreationDate = org[i].CreationDate;
                    ctrdetailT.LastUpdateBy = org[i].LastUpdateBy;
                    ctrdetailT.LastUpdateDate = org[i].LastUpdateDate;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

            ctrHeaderTRepositiory.Create(ctrheaderT);
            ctrDetailTRepositiory.Create(ctrdetailT);
        }
    }
}