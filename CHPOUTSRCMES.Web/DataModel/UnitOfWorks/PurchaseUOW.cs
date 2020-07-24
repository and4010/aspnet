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
        private readonly IRepository<CTR_FILEINFO_T> ctrFileInfoTRepositiory;
        private readonly IRepository<CTR_FILES_T> ctrFilesTRepositiory;
     

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
            this.ctrFileInfoTRepositiory = new GenericRepository<CTR_FILEINFO_T>(this);
            this.ctrFilesTRepositiory = new GenericRepository<CTR_FILES_T>(this);
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
                    generateTestFlatDetail();
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
                ctrorg.RollReamWt = 3000;
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
                    ctrheaderT.CreatedUserName = org[i].CreatedBy.ToString();
                    ctrheaderT.CreationDate = org[i].CreationDate;
                    ctrheaderT.LastUpdateBy = org[i].LastUpdateBy.ToString();
                    ctrheaderT.LastUpdateDate = org[i].LastUpdateDate;
                    ctrheaderT.LastUpdateUserName = org[i].LastUpdateBy.ToString();
                    ctrHeaderTRepositiory.Create(ctrheaderT, true);

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
                    ctrdetailT.CreatedUserName = org[i].CreatedBy.ToString();
                    ctrdetailT.CreationDate = org[i].CreationDate;
                    ctrdetailT.LastUpdateBy = org[i].LastUpdateBy.ToString();
                    ctrdetailT.LastUpdateUserName = org[i].LastUpdateBy.ToString();
                    ctrdetailT.LastUpdateDate = org[i].LastUpdateDate;
                    ctrDetailTRepositiory.Create(ctrdetailT, true);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

        }

        public void generateTestFlatDetail()
        {
            var ctrDetail = ctrDetailTRepositiory.Get(s => s.ItemCategory == "平張").ToList();
            CTR_PICKED_T cTR_PICKED_T = new CTR_PICKED_T();
            try
            {
                for (int i = 0; i < ctrDetail.Count; i++)
                {
                    cTR_PICKED_T.CtrHeaderId = ctrDetail[i].CtrHeaderId;
                    cTR_PICKED_T.CtrDetailId = ctrDetail[i].CtrHeaderId;
                    cTR_PICKED_T.StockId = 0;
                    cTR_PICKED_T.LocatorId = ctrDetail[i].LocatorId;
                    cTR_PICKED_T.LocatorCode = ctrDetail[i].LocatorCode;
                    cTR_PICKED_T.Barcode = "P200506000" + i + 1;
                    cTR_PICKED_T.InventoryItemId = ctrDetail[i].InventoryItemId;
                    cTR_PICKED_T.ShipItemNumber = ctrDetail[i].ShipItemNumber;
                    cTR_PICKED_T.PaperType = ctrDetail[i].PaperType;
                    cTR_PICKED_T.BasicWeight = ctrDetail[i].BasicWeight;
                    cTR_PICKED_T.ReamWeight = ctrDetail[i].ReamWeight;
                    cTR_PICKED_T.RollReamWt = ctrDetail[i].RollReamWt;
                    cTR_PICKED_T.Specification = ctrDetail[i].Specification;
                    cTR_PICKED_T.PackingType = ctrDetail[i].PackingType;
                    cTR_PICKED_T.ShipMtQty = ctrDetail[i].ShipMtQty;
                    cTR_PICKED_T.TransactionQuantity = ctrDetail[i].CtrHeaderId;
                    cTR_PICKED_T.TransactionUom = ctrDetail[i].TransactionUom;
                    cTR_PICKED_T.PrimaryQuantity = ctrDetail[i].PrimaryQuantity;
                    cTR_PICKED_T.PrimaryUom = ctrDetail[i].PrimaryUom;
                    cTR_PICKED_T.SecondaryQuantity = ctrDetail[i].SecondaryQuantity;
                    cTR_PICKED_T.SecondaryUom = ctrDetail[i].SecondaryUom;
                    cTR_PICKED_T.LotNumber = ctrDetail[i].LotNumber;
                    cTR_PICKED_T.TheoryWeight = ctrDetail[i].TheoryWeight;
                    cTR_PICKED_T.ItemCategory = ctrDetail[i].ItemCategory;
                    cTR_PICKED_T.Status = "待入庫";
                    cTR_PICKED_T.ReasonCode = "";
                    cTR_PICKED_T.ReasonDesc = "";
                    cTR_PICKED_T.Note = "";
                    cTR_PICKED_T.CreatedBy = "1";
                    cTR_PICKED_T.CreationDate = DateTime.Now;
                    cTR_PICKED_T.CreatedUserName = "伊麗星";
                    cTR_PICKED_T.LastUpdateBy = "1";
                    cTR_PICKED_T.LastUpdateDate = DateTime.Now;
                    cTR_PICKED_T.LastUpdateUserName = "伊麗星";
                    ctrPickedTRepositiory.Create(cTR_PICKED_T);
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
            }
            this.SaveChanges();
       
        }

        public List<FullCalendarEventModel> getFullCalenderList()
        {
            var header = ctrHeaderTRepositiory.GetAll().GroupBy(x => x.ContainerNo).Select(x => x.FirstOrDefault()).ToList();

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
                    return mesContext.Database.SqlQuery<DetailModel.FlatModel>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
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
                    return mesContext.Database.SqlQuery<DetailModel.RollModel>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

        public List<DetailModel.FlatDetailModel> GetFlatDetailList(string CONTAINER_NO)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"SELECT 
p.CTR_PICKED_ID as Id,
h.SUBINVENTORY as Subinventory, 
p.LOCATOR_CODE as Locator,
p.BARCODE as Barcode,
p.SHIP_ITEM_NUMBER as Item_No,
p.REAM_WEIGHT as ReamWeight,
p.PACKING_TYPE as PackingType,
p.ROLL_REAM_WT as Pieces_Qty,
d.ROLL_REAM_QTY as Qty,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d ON d.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平張' and h.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<DetailModel.FlatDetailModel>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

        public DetailModel.FlatDetailModel GetFlatEdit(string id)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"  SELECT 
p.CTR_PICKED_ID as Id,
h.SUBINVENTORY as Subinventory, 
p.LOCATOR_CODE as Locator,
p.BARCODE as Barcode,
p.SHIP_ITEM_NUMBER as Item_No,
p.REAM_WEIGHT as ReamWeight,
p.PACKING_TYPE as PackingType,
p.ROLL_REAM_WT as Pieces_Qty,
d.ROLL_REAM_QTY as Qty,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d ON d.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平張' and p.CTR_PICKED_ID  = @CTR_PICKED_ID");
                    return mesContext.Database.SqlQuery<DetailModel.FlatDetailModel>(query.ToString(), new SqlParameter("@CTR_PICKED_ID", id)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

        public decimal GetFlatNumberTab(string CONTAINER_NO)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d2 ON d2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平張' and h2.CONTAINER_NO  = @CONTAINER_NO and p.STATUS = N'已入庫')
FROM dbo.CTR_DETAIL_T d1
LEFT JOIN dbo.CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'平張' and h1.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<decimal>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).SingleOrDefault();
                }
          
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
            }
            return 0;
        }

        public decimal GetPaperRollNumberTab(string CONTAINER_NO)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d2 ON d2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h2.CONTAINER_NO  = @CONTAINER_NO and p.STATUS = N'已入庫')
FROM dbo.CTR_DETAIL_T d1
LEFT JOIN dbo.CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'捲筒' and h1.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<decimal>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).SingleOrDefault();
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            return 0;
        }

        public void SavePhoto(HttpPostedFileBase file)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    using (var mescontext = new MesContext())
                    {
                        SaveCtrFileInfoT(VaryQualityLevel(file), file);
                    }
                    txn.Commit();
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                }
            }
        }

        public void SaveCtrFileInfoT(byte[] filebyte,HttpPostedFileBase file)
        {
            try
            {
                if(filebyte != null)
                {
                    CTR_FILEINFO_T cTR_FILEINFO_T = new CTR_FILEINFO_T();
                    cTR_FILEINFO_T.CtrPickedId = 1;
                    cTR_FILEINFO_T.CtrFileId = 1;
                    cTR_FILEINFO_T.FileType = file.ContentType;
                    cTR_FILEINFO_T.FileName = file.FileName;
                    cTR_FILEINFO_T.Size = filebyte.Length;
                    cTR_FILEINFO_T.Seq = 1;
                    cTR_FILEINFO_T.CreatedBy = "1";
                    cTR_FILEINFO_T.CreationDate = DateTime.Now;
                    ctrFileInfoTRepositiory.Create(cTR_FILEINFO_T);
                    SaveCtrFileT(filebyte, file);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }

        }

        public void SaveCtrFileT(byte[] filebyte, HttpPostedFileBase file)
        {
            try
            {
                CTR_FILES_T cTR_FILES_T = new CTR_FILES_T();
                cTR_FILES_T.CtrFileId = 1;
                cTR_FILES_T.FileInstance = filebyte;
                ctrFilesTRepositiory.Create(cTR_FILES_T,true);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }

        public byte[] VaryQualityLevel(HttpPostedFileBase file)
        {
            try
            {
                //string tempPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString() + "/"));
                //if (!Directory.Exists(tempPath))
                //{
                //    Directory.CreateDirectory(tempPath);
                //}
                using (var thumb = Image.FromStream(file.InputStream))
                {
                    var jpgInfo = GetEncoder(ImageFormat.Jpeg); /* Returns array of image encoder objects built into GDI+ */
                    //using (var encParams = new EncoderParameters(1))
                    //{
                    using (var samllfile = new MemoryStream())
                    {
                        //    // Create an EncoderParameters object.  
                        //    // An EncoderParameters object has an array of EncoderParameter  
                        //    // objects. In this case, there is only one  
                        //    // EncoderParameter object in the array.  
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 10L);
                        thumb.Save(samllfile, jpgInfo, myEncoderParameters);
                        return samllfile.ToArray();
                    }
                    //};
                };
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            return null;
        }
        //照片解析
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == format.Guid).FirstOrDefault();
            if (codec == null)
            {
                return null;
            }
            return codec;

        }




    }
}