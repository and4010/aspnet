using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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
            var header = ctrHeaderTRepositiory.GetAll().GroupBy(x => x.ContainerNo).Select(x => x.FirstOrDefault()) .ToList();

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
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    generateTestDataCtrOrgT();
                    generateTestDataCtrHeaderDetail();
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
                ctrOrgTRepositiory.Create(ctrorg, true);

                ctrorg.CtrOrgId = 2;
                ctrorg.ProcessCode = "XXIFP217";
                ctrorg.ServerCode = "123";
                ctrorg.BatchId = "20200721141600100000";
                ctrorg.BatchLineId = 2;
                ctrorg.HeaderId = 2;
                ctrorg.OrgId = 2;
                ctrorg.OrgName = "入庫";
                ctrorg.LineId = 2;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "SFG";
                ctrorg.LocatorId = 22016;
                ctrorg.LocatorCode = "SFG";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503117;
                ctrorg.ShipItemNumber = "4FU0ZA022000889RL00";
                ctrorg.PaperType = "FU0Z";
                ctrorg.BasicWeight = "02200";
                ctrorg.ReamWeight = "02200";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "889K502K";
                ctrorg.PackingType = "無";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 0.421M;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 421;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 0;
                ctrorg.SecondaryUom = "";
                ctrorg.LotNumber = "1400110000776875";
                ctrorg.TheoryWeight = "";
                ctrorg.CreatedBy = 1;
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = 1;
                ctrorg.LastUpdateDate = DateTime.Now;
                ctrOrgTRepositiory.Create(ctrorg, true);

                ctrorg.CtrOrgId = 3;
                ctrorg.ProcessCode = "XXIFP217";
                ctrorg.ServerCode = "123";
                ctrorg.BatchId = "20200721141600100000";
                ctrorg.BatchLineId = 3;
                ctrorg.HeaderId = 3;
                ctrorg.OrgId = 3;
                ctrorg.OrgName = "入庫";
                ctrorg.LineId = 3;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "SFG";
                ctrorg.LocatorId = 22016;
                ctrorg.LocatorCode = "SFG";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503117;
                ctrorg.ShipItemNumber = "4FU0ZA022000635RL00";
                ctrorg.PaperType = "FU0Z";
                ctrorg.BasicWeight = "02200";
                ctrorg.ReamWeight = "02200";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "635K502K";
                ctrorg.PackingType = "無";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 0.441M;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 440;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 0;
                ctrorg.SecondaryUom = "";
                ctrorg.LotNumber = "1400120000776904";
                ctrorg.TheoryWeight = "";
                ctrorg.CreatedBy = 1;
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = 1;
                ctrorg.LastUpdateDate = DateTime.Now;
                ctrOrgTRepositiory.Create(ctrorg, true);
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

        

       
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
                    ctrheaderT.HeaderId = org[i].HeaderId;
                    ctrheaderT.OrgId = org[i].OrgId;
                    ctrheaderT.OrgName = org[i].OrgName;
                    ctrheaderT.LineId = org[i].LineId;
                    ctrheaderT.ContainerNo = org[i].ContainerNo;
                    ctrheaderT.MvContainerDate = org[i].MvContainerDate;
                    ctrheaderT.OrganizationId = org[i].OrganizationId;
                    ctrheaderT.OrganizationCode = org[i].OrganizationCode;
                    ctrheaderT.Subinventory = org[i].Subinventory;
                    ctrheaderT.Status = 1;
                    ctrheaderT.CreatedBy = org[i].CreatedBy.ToString();
                    ctrheaderT.CreationDate = org[i].CreationDate;
                    ctrheaderT.LastUpdateBy = org[i].LastUpdateBy.ToString();
                    ctrheaderT.LastUpdateDate = org[i].LastUpdateDate;
                    ctrHeaderTRepositiory.Create(ctrheaderT,true);

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
                    ctrdetailT.CreatedBy = org[i].CreatedBy.ToString();
                    ctrdetailT.CreationDate = org[i].CreationDate;
                    ctrdetailT.LastUpdateBy = org[i].LastUpdateBy.ToString();
                    ctrdetailT.LastUpdateDate = org[i].LastUpdateDate;
                    ctrDetailTRepositiory.Create(ctrdetailT,true);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

        }


        public List<DetailModel.FlatModel> GetFlatHeaderList(string CONTAINER_NO)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"SELECT 
Cast(d.CTR_HEADER_ID AS bigint) as Id,
h.SUBINVENTORY AS Subinventory, 
d.LOCATOR_CODE as Locator,
d.SHIP_ITEM_NUMBER AS Item_No,
d.REAM_WEIGHT AS ReamWeight,
d.ROLL_REAM_QTY AS RollReamQty,
d.PACKING_TYPE AS PackingType,
d.ROLL_REAM_WT AS Pieces_Qty,
d.TRANSACTION_QUANTITY AS TransactionQuantity,
d.TRANSACTION_UOM AS TransactionUom,
d.SECONDARY_QUANTITY AS TtlRollReam,
d.SECONDARY_UOM AS TtlRollReamUom,
d.PRIMARY_QUANTITY AS DeliveryQty,
d.PRIMARY_UOM AS DeliveryUom
FROM dbo.CTR_DETAIL_T d
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
WHERE d.ITEM_CATEGORY = N'平張' and h.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<DetailModel.FlatModel>(query.ToString(),new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

     
        public List<DetailModel.RollModel> GetPaperRollHeaderList(string CONTAINER_NO)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"SELECT 
Cast(d.CTR_HEADER_ID AS bigint) as Id,
h.SUBINVENTORY AS Subinventory, 
d.LOCATOR_CODE as Locator,
d.SHIP_ITEM_NUMBER AS Item_No,
d.PAPER_TYPE AS PaperType,
d.BASIC_WEIGHT AS BaseWeight,
d.SPECIFICATION AS Specification,
d.ROLL_REAM_QTY AS RollReamQty,
d.TRANSACTION_QUANTITY AS TransactionQuantity,
d.TRANSACTION_UOM AS TransactionUom,
d.PRIMARY_QUANTITY AS PrimanyQuantity,
d.PRIMARY_UOM AS PrimaryUom
FROM dbo.CTR_DETAIL_T d
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
WHERE d.ITEM_CATEGORY = N'捲筒' and h.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<DetailModel.RollModel>(query.ToString(),new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

    }
}