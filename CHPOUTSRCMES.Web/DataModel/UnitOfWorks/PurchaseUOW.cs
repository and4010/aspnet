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
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Util;
using DataTables;
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
                ctrorg.LocatorCode = "TB2";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503117;
                ctrorg.ShipItemNumber = "4DM00E02700310K502K";
                ctrorg.PaperType = "DM00";
                ctrorg.BasicWeight = "02700";
                ctrorg.ReamWeight = "299.11";
                ctrorg.RollReamQty = 2;
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
                ctrorg.LocatorCode = "TB2";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 1990193;
                ctrorg.ShipItemNumber = "4A001A006000315RL00";
                ctrorg.PaperType = "A0001";
                ctrorg.BasicWeight = "00600";
                ctrorg.ReamWeight = "02200";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "0315RL00";
                ctrorg.PackingType = "無";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 0.616M;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 616;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 0;
                ctrorg.SecondaryUom = "";
                ctrorg.LotNumber = "1400110000776875";
                ctrorg.TheoryWeight = "616";
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
                ctrorg.LocatorCode = "TB2";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 2096018;
                ctrorg.ShipItemNumber = "4A001A006000400RL00";
                ctrorg.PaperType = "A0001";
                ctrorg.BasicWeight = "00600";
                ctrorg.ReamWeight = "02200";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "0400RL00";
                ctrorg.PackingType = "無";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 0.440M;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 440;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 0;
                ctrorg.SecondaryUom = "";
                ctrorg.LotNumber = "1400120000776904";
                ctrorg.TheoryWeight = "440";
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
                    ctrheaderT.Status = Int64.Parse(PurchaseStatusCode.PurchaseHeaderPending);
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

        /// <summary>
        /// 紙捲匯入pickt資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <param name="PaperRollModel"></param>
        /// <returns></returns>
        public ResultModel ImportPaperRollDetail(string CONTAINER_NO, List<DetailModel.RollDetailModel> PaperRollModel, string createby, string userName)
        {

            CTR_PICKED_T cTR_PICKED_T = new CTR_PICKED_T();
            try
            {
                var ctrpick = ctrPickedTRepositiory.Get(x => x.ItemCategory == "捲筒").ToList();
                if (ctrpick.Count == PaperRollModel.Count)
                {
                    return new ResultModel(false, "資料已存在無法匯入");
                }

                using (var db = new MesContext())
                {
                    var ctrDetail = db.CTR_DETAIL_Ts.Join(
                   db.CTR_HEADER_Ts,               //要Join的資料表
                   c => c.CtrHeaderId,    //c代表db.CTR_DETAIL_Ts(c可以自己定義名稱)，這邊放主要資料表要串聯的key
                   cd => cd.CtrHeaderId,  //cd代表db.CTR_HEADER_Ts(cd可以自己定義名稱)，這邊放次資料表要串聯的key
                   (c, cd) => new         //將兩個自定義名稱用小括胡包起來，接著透過 『=>』 可以自己選擇要取用要用資料 
                   {
                       d = c,
                       e = cd
                   }
                   ).Where(x => x.d.ItemCategory == "捲筒" && x.e.ContainerNo == CONTAINER_NO).ToList();

                    for (int i = 0; i < ctrDetail.Count; i++)
                    {
                        cTR_PICKED_T.CtrHeaderId = ctrDetail[i].d.HeaderId;
                        cTR_PICKED_T.CtrDetailId = ctrDetail[i].d.CtrDetailId;
                        cTR_PICKED_T.StockId = i + 1;
                        cTR_PICKED_T.LocatorId = ctrDetail[i].d.LocatorId;
                        cTR_PICKED_T.LocatorCode = ctrDetail[i].d.LocatorCode;
                        cTR_PICKED_T.Barcode = GenerateBarcodes(ctrDetail[i].e.OrganizationId, ctrDetail[i].e.Subinventory, "W", PaperRollModel.Count, "伊麗星").Data[i];
                        cTR_PICKED_T.InventoryItemId = ctrDetail[i].d.InventoryItemId;
                        cTR_PICKED_T.ShipItemNumber = PaperRollModel[i].Item_No;
                        cTR_PICKED_T.PaperType = PaperRollModel[i].PaperType;
                        cTR_PICKED_T.BasicWeight = PaperRollModel[i].BaseWeight;
                        cTR_PICKED_T.ReamWeight = ctrDetail[i].d.ReamWeight;
                        cTR_PICKED_T.RollReamWt = ctrDetail[i].d.RollReamWt;
                        cTR_PICKED_T.Specification = PaperRollModel[i].Specification;
                        cTR_PICKED_T.PackingType = ctrDetail[i].d.PackingType;
                        cTR_PICKED_T.ShipMtQty = ctrDetail[i].d.ShipMtQty;
                        cTR_PICKED_T.TransactionQuantity = ctrDetail[i].d.TransactionQuantity;
                        cTR_PICKED_T.TransactionUom = ctrDetail[i].d.TransactionUom;
                        cTR_PICKED_T.PrimaryQuantity = PaperRollModel[i].PrimanyQuantity;
                        cTR_PICKED_T.PrimaryUom = PaperRollModel[i].PrimaryUom;
                        cTR_PICKED_T.SecondaryQuantity = ctrDetail[i].d.SecondaryQuantity;
                        cTR_PICKED_T.SecondaryUom = ctrDetail[i].d.SecondaryUom;
                        cTR_PICKED_T.LotNumber = PaperRollModel[i].LotNumber;
                        cTR_PICKED_T.TheoryWeight = ctrDetail[i].d.TheoryWeight;
                        cTR_PICKED_T.ItemCategory = ctrDetail[i].d.ItemCategory;
                        cTR_PICKED_T.Status = "待入庫";
                        cTR_PICKED_T.ReasonCode = "";
                        cTR_PICKED_T.ReasonDesc = "";
                        cTR_PICKED_T.Note = "";
                        cTR_PICKED_T.CreatedBy = createby;
                        cTR_PICKED_T.CreationDate = DateTime.Now;
                        cTR_PICKED_T.CreatedUserName = userName;
                        cTR_PICKED_T.LastUpdateBy = createby;
                        cTR_PICKED_T.LastUpdateDate = DateTime.Now;
                        cTR_PICKED_T.LastUpdateUserName = userName;
                        ctrPickedTRepositiory.Create(cTR_PICKED_T, true);

                    }
                    return new ResultModel(true, "匯入成功");
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new ResultModel(false, e.Message.ToString());
            }

        }

        /// <summary>
        /// 刪除excel資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public ResultModel DeleteExcel(string CONTAINER_NO)
        {

            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"DELETE p
FROM CTR_PICKED_T p
INNER JOIN CTR_HEADER_T h
INNER JOIN CTR_DETAIL_T d
ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
ON d.CTR_DETAIL_ID = p.CTR_DETAIL_ID
where h.CONTAINER_NO = @CONTAINER_NO
and d.ITEM_CATEGORY = N'捲筒'");
                    mesContext.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO));
                    return new ResultModel(true, "捲筒刪除成功");
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new ResultModel(false, e.Message.ToString());
            }

        }

        /// <summary>
        /// 平張明細資料
        /// </summary>
        public void generateTestFlatDetail()
        {
            var ctrDetail = ctrDetailTRepositiory.Get(s => s.ItemCategory == "平張").ToList();

            CTR_PICKED_T cTR_PICKED_T = new CTR_PICKED_T();
            try
            {
                for (int i = 0; i < ctrDetail.Count; i++)
                {
                    var HeaderId = ctrDetail[i].HeaderId;
                    var ctrheader = ctrHeaderTRepositiory.Get(s => s.HeaderId == HeaderId).SingleOrDefault();
                    for (int j = 0; j < decimal.ToInt32(ctrDetail[i].RollReamQty); j++)
                    {
                        cTR_PICKED_T.CtrHeaderId = HeaderId;
                        cTR_PICKED_T.CtrDetailId = ctrDetail[i].CtrDetailId;
                        cTR_PICKED_T.StockId = i + 1;
                        cTR_PICKED_T.LocatorId = ctrDetail[i].LocatorId;
                        cTR_PICKED_T.LocatorCode = ctrDetail[i].LocatorCode;
                        cTR_PICKED_T.Barcode = GenerateBarcodes(ctrheader.OrganizationId, ctrheader.Subinventory, "P", ctrDetail.Count, decimal.ToInt32(ctrDetail[i].RollReamQty).ToString()).Data[i];
                        cTR_PICKED_T.InventoryItemId = ctrDetail[i].InventoryItemId;
                        cTR_PICKED_T.ShipItemNumber = ctrDetail[i].ShipItemNumber;
                        cTR_PICKED_T.PaperType = ctrDetail[i].PaperType;
                        cTR_PICKED_T.BasicWeight = ctrDetail[i].BasicWeight;
                        cTR_PICKED_T.ReamWeight = ctrDetail[i].ReamWeight;
                        cTR_PICKED_T.RollReamWt = ctrDetail[i].RollReamWt;
                        cTR_PICKED_T.Specification = ctrDetail[i].Specification;
                        cTR_PICKED_T.PackingType = ctrDetail[i].PackingType;
                        cTR_PICKED_T.ShipMtQty = ctrDetail[i].ShipMtQty;
                        cTR_PICKED_T.TransactionQuantity = ctrDetail[i].TransactionQuantity;
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
                        cTR_PICKED_T.CreatedBy = ctrDetail[i].CreatedBy;
                        cTR_PICKED_T.CreationDate = DateTime.Now;
                        cTR_PICKED_T.CreatedUserName = ctrDetail[i].CreatedUserName;
                        cTR_PICKED_T.LastUpdateBy = ctrDetail[i].LastUpdateBy;
                        cTR_PICKED_T.LastUpdateDate = DateTime.Now;
                        cTR_PICKED_T.LastUpdateUserName = ctrDetail[i].LastUpdateUserName;
                        ctrPickedTRepositiory.Create(cTR_PICKED_T, true);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }

        }

        /// <summary>
        /// 取得行事曆資料
        /// </summary>
        /// <returns></returns>
        public List<FullCalendarEventModel> getFullCalenderList(string Subinventory)
        {
            var header = ctrHeaderTRepositiory.Get(x => x.Subinventory == Subinventory).GroupBy(x => x.ContainerNo).Select(x => x.FirstOrDefault()).ToList();

            List<FullCalendarEventModel> fullCalendarEventModel = new List<FullCalendarEventModel>();
            UrlHelper objUrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            for (int i = 0; i < header.Count; i++)
            {
                if (header[i].Status == Int64.Parse(PurchaseStatusCode.PurchaseHeaderPending))
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
                if (header[i].Status == Int64.Parse(PurchaseStatusCode.PurchaseHeaderCancel))
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
                if (header[i].Status == Int64.Parse(PurchaseStatusCode.PurchaseHeaderAlready))
                {
                    fullCalendarEventModel.Add(new FullCalendarEventModel()
                    {
                        id = header[i].CtrHeaderId,
                        title = header[i].Subinventory + "\n" + header[i].ContainerNo + "已入庫",
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
                        Status = header[i].Status,
                    });
                }

            }
            return fullCalendarEventModel;
        }

        /// <summary>
        /// 存檔入庫header轉狀態
        /// </summary>
        /// <param name="ContainerNo"></param>
        /// <returns></returns>
        public Boolean ChangeHeaderStatus(string ContainerNo)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var header = ctrHeaderTRepositiory.Get(x => x.ContainerNo == ContainerNo).ToList();
                    if (header != null)
                    {
                        for (int i = 0; i < header.Count; i++)
                        {
                            header[i].Status = 0;
                            ctrHeaderTRepositiory.Update(header[i], true);
                            PickToPickHT(header[i].HeaderId);
                            PickTDelete(header[i].HeaderId);
                        }
                        txn.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                }
                return false;
            }
        }

        /// <summary>
        /// 取得平張header資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得紙捲header資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得紙捲明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.RollDetailModel> GetPaperRollDetailList(string CONTAINER_NO)
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
p.PAPER_TYPE as PaperType,
p.BASIC_WEIGHT as BaseWeight,
p.SPECIFICATION as Specification,
p.THEORY_WEIGHT as TheoreticalWeight,
p.TRANSACTION_QUANTITY as TransactionQuantity,
p.TRANSACTION_UOM as TransactionUom,
p.PRIMARY_QUANTITY as PrimanyQuantity,
p.PRIMARY_UOM as PrimaryUom,
p.LOT_NUMBER as LotNumber,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d ON d.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<DetailModel.RollDetailModel>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// 取得紙捲歷史明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.RollDetailModel> GetHtPaperRollDetailList(string CONTAINER_NO)
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
p.PAPER_TYPE as PaperType,
p.BASIC_WEIGHT as BaseWeight,
p.SPECIFICATION as Specification,
p.THEORY_WEIGHT as TheoreticalWeight,
p.TRANSACTION_QUANTITY as TransactionQuantity,
p.TRANSACTION_UOM as TransactionUom,
p.PRIMARY_QUANTITY as PrimanyQuantity,
p.PRIMARY_UOM as PrimaryUom,
p.LOT_NUMBER as LotNumber,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark
FROM dbo.CTR_PICKED_HT p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d ON d.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h.CONTAINER_NO  = @CONTAINER_NO");
                    return mesContext.Database.SqlQuery<DetailModel.RollDetailModel>(query.ToString(), new SqlParameter("@CONTAINER_NO", CONTAINER_NO)).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// 取得平張明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得平張歷史明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.FlatDetailModel> GetHtFlatDetailList(string CONTAINER_NO)
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
FROM dbo.CTR_PICKED_HT p
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

        /// <summary>
        /// 取得捲筒編輯資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailModel.RollDetailModel GetPaperRollEdit(string id)
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
p.PAPER_TYPE as PaperType,
p.BASIC_WEIGHT as BaseWeight,
p.SPECIFICATION as Specification,
d.THEORY_WEIGHT as TheoreticalWeight,
d.TRANSACTION_QUANTITY as TransactionQuantity,
d.TRANSACTION_UOM as TransactionUom,
d.PRIMARY_QUANTITY as PrimanyQuantity,
d.PRIMARY_UOM as PrimaryUom,
d.LOT_NUMBER as LotNumber,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark,
p.LAST_UPDATE_DATE as CreationDate,
p.LAST_UPDATE_USER_NAME as CreatedUserName 
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d ON d.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and p.CTR_PICKED_ID  = @CTR_PICKED_ID");
                    return mesContext.Database.SqlQuery<DetailModel.RollDetailModel>(query.ToString(), new SqlParameter("@CTR_PICKED_ID", id)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 取得捲筒檢視資料pickt歷史資料
        /// </summary>
        /// <returns></returns>
        public DetailModel.RollDetailModel GetPaperRollView(string id)
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
p.PAPER_TYPE as PaperType,
p.BASIC_WEIGHT as BaseWeight,
p.SPECIFICATION as Specification,
d.THEORY_WEIGHT as TheoreticalWeight,
d.TRANSACTION_QUANTITY as TransactionQuantity,
d.TRANSACTION_UOM as TransactionUom,
d.PRIMARY_QUANTITY as PrimanyQuantity,
d.PRIMARY_UOM as PrimaryUom,
d.LOT_NUMBER as LotNumber,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark,
p.LAST_UPDATE_DATE as CreationDate,
p.LAST_UPDATE_USER_NAME as CreatedUserName 
FROM dbo.CTR_PICKED_HT p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
LEFT JOIN dbo.CTR_DETAIL_T d ON d.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and p.CTR_PICKED_ID  = @CTR_PICKED_ID");
                    return mesContext.Database.SqlQuery<DetailModel.RollDetailModel>(query.ToString(), new SqlParameter("@CTR_PICKED_ID", id)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 紙捲寫入原因儲位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="Locator"></param>
        /// <param name="Remark"></param>
        public Boolean PaperRollEdit(long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var mes = this.Context.Database.BeginTransaction())
            {
                try
                {
                    using (var db = new MesContext())
                    {
                        var ctrPickT = ctrPickedTRepositiory.Get(x => x.CtrPickedId == id).SingleOrDefault();
                        if (Reason != "請選擇")
                        {
                            ctrPickT.ReasonDesc = Reason;
                        }
                        if (Locator != "請選擇")
                        {
                            var LocatorId = db.LocatorTs.Where(x => x.Segment3 == Locator).SingleOrDefault().LocatorId;
                            ctrPickT.LocatorId = LocatorId;
                            ctrPickT.LocatorCode = Locator;
                            var ctrdetail = ctrDetailTRepositiory.Get(x => x.DetailId == ctrPickT.CtrDetailId).SingleOrDefault();
                            ctrdetail.LocatorCode = Locator;
                            ctrdetail.LocatorId = LocatorId;
                            ctrDetailTRepositiory.Update(ctrdetail, true);
                        }
                        ctrPickT.Note = Remark;
                        ctrPickT.LastUpdateBy = LastUpdateBy;
                        ctrPickT.LastUpdateUserName = LastUpdateUserName;
                        ctrPickT.LastUpdateDate = DateTime.Now;
                        ctrPickedTRepositiory.Update(ctrPickT, true);
                        mes.Commit();
                        return true;
                    }

                }

                catch (Exception e)
                {
                    mes.Rollback();
                    logger.Error(e.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 平張寫入原因儲位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="Locator"></param>
        /// <param name="Remark"></param>
        public Boolean FlatEdit(long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var mes = this.Context.Database.BeginTransaction())
            {
                try
                {
                    using (var db = new MesContext())
                    {
                        var ctrPickT = ctrPickedTRepositiory.Get(x => x.CtrPickedId == id).SingleOrDefault();
                        if (Reason != "請選擇")
                        {
                            ctrPickT.ReasonDesc = Reason;
                        }
                        if (Locator != "請選擇")
                        {
                            var LocatorId = db.LocatorTs.Where(x => x.Segment3 == Locator).SingleOrDefault().LocatorId;
                            ctrPickT.LocatorId = LocatorId;
                            ctrPickT.LocatorCode = Locator;
                            var ctrdetail = ctrDetailTRepositiory.Get(x => x.DetailId == ctrPickT.CtrDetailId).SingleOrDefault();
                            ctrdetail.LocatorCode = Locator;
                            ctrdetail.LocatorId = LocatorId;
                            ctrDetailTRepositiory.Update(ctrdetail, true);
                        }
                        ctrPickT.Note = Remark;
                        ctrPickT.LastUpdateBy = LastUpdateBy;
                        ctrPickT.LastUpdateUserName = LastUpdateUserName;
                        ctrPickT.LastUpdateDate = DateTime.Now;
                        ctrPickedTRepositiory.Update(ctrPickT, true);
                        mes.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    mes.Rollback();
                    logger.Error(e.Message);
                    return false;
                }
            }


        }

        /// <summary>
        /// 紙捲條碼已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public int SavePaperRollBarcode(String Barcode, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var ctrPickT = ctrPickedTRepositiory.Get(x => x.Barcode == Barcode).SingleOrDefault();
                    if (ctrPickT != null)
                    {
                        if (ctrPickT.Status == "已入庫")
                        {
                            return 1;
                        }
                        else
                        {
                            ctrPickT.Status = "已入庫";
                            ctrPickT.LastUpdateBy = LastUpdateBy;
                            ctrPickT.LastUpdateUserName = LastUpdateUserName;
                            ctrPickT.LastUpdateDate = DateTime.Now;
                            ctrPickedTRepositiory.Update(ctrPickT, true);
                            txn.Commit();
                            return 0;
                        }

                    }
                    else
                    {
                        return 2;
                    }

                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                    return 3;
                }
            }
        }

        /// <summary>
        /// 平張條碼已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public int SaveFlatBarcode(String Barcode, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var ctrPickT = ctrPickedTRepositiory.Get(x => x.Barcode == Barcode).SingleOrDefault();
                    if (ctrPickT != null)
                    {
                        if (ctrPickT.Status == "已入庫")
                        {
                            return 1;
                        }
                        else
                        {
                            ctrPickT.Status = "已入庫";
                            ctrPickedTRepositiory.Update(ctrPickT, true);
                            ctrPickT.LastUpdateBy = LastUpdateBy;
                            ctrPickT.LastUpdateUserName = LastUpdateUserName;
                            ctrPickT.LastUpdateDate = DateTime.Now;
                            txn.Commit();
                            return 0;
                        }

                    }
                    else
                    {
                        return 2;
                    }

                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                    return 3;
                }
            }
        }

        /// <summary>
        /// 取得編輯平張資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailModel.FlatDetailModel GetFlatEdit(string id)
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
p.NOTE as Remark,
p.LAST_UPDATE_DATE as CreationDate,
p.LAST_UPDATE_USER_NAME as CreatedUserName 
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

        /// <summary>
        /// 取得平張檢視歷史pcikt資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailModel.FlatDetailModel GetFlatView(string id)
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
p.NOTE as Remark,
p.LAST_UPDATE_DATE as CreationDate,
p.LAST_UPDATE_USER_NAME as CreatedUserName 
FROM dbo.CTR_PICKED_HT p
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

        /// <summary>
        /// 取得頁籤平張數量
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
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
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            return 0;
        }

        /// <summary>
        /// 取得頁籤紙捲數量
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
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

        public void SavePhoto(HttpPostedFileBase file, long id, string createby)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    using (var mescontext = new MesContext())
                    {
                        SaveCtrFileInfoT(VaryQualityLevel(file), file, id, createby);
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

        /// <summary>
        /// 儲存照片FileInfo Table
        /// </summary>
        /// <param name="filebyte"></param>
        /// <param name="file"></param>
        public void SaveCtrFileInfoT(byte[] filebyte, HttpPostedFileBase file, long id, string CreatedBy)
        {
            try
            {
                if (filebyte != null)
                {
                    CTR_FILES_T cTR_FILES_T = new CTR_FILES_T();
                    cTR_FILES_T.FileInstance = filebyte;
                    ctrFilesTRepositiory.Create(cTR_FILES_T, true);

                    CTR_FILEINFO_T cTR_FILEINFO_T = new CTR_FILEINFO_T();
                    cTR_FILEINFO_T.CtrPickedId = id;
                    cTR_FILEINFO_T.CtrFileId = cTR_FILES_T.CtrFileId;
                    cTR_FILEINFO_T.FileType = file.ContentType;
                    cTR_FILEINFO_T.FileName = file.FileName;
                    cTR_FILEINFO_T.Size = filebyte.Length;
                    cTR_FILEINFO_T.Seq = 1;
                    cTR_FILEINFO_T.CreatedBy = CreatedBy;
                    cTR_FILEINFO_T.CreationDate = DateTime.Now;
                    ctrFileInfoTRepositiory.Create(cTR_FILEINFO_T);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }

        }

        /// <summary>
        /// 儲存照片file table
        /// </summary>
        /// <param name="filebyte"></param>
        /// <param name="file"></param>
        //public void SaveCtrFileT(byte[] filebyte, HttpPostedFileBase file,long id)
        //{
        //    try
        //    {
        //        CTR_FILES_T cTR_FILES_T = new CTR_FILES_T();
        //        cTR_FILES_T.FileInstance = filebyte;
        //        ctrFilesTRepositiory.Create(cTR_FILES_T, true);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //    }
        //}

        /// <summary>
        /// 壓縮照片大小
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 轉換
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == format.Guid).FirstOrDefault();
            if (codec == null)
            {
                return null;
            }
            return codec;

        }

        /// <summary>
        /// 取得相片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetPhoto(long id)
        {
            using (var db = new MesContext())
            {
                var ctrPhoto = db.CTR_FILES_Ts.Join(
                   db.CTR_FILEINFO_Ts,               //要Join的資料表
                   c => c.CtrFileId,    //c代表db.CTR_FILEINFO_Ts(c可以自己定義名稱)，這邊放主要資料表要串聯的key
                   d => d.CtrFileId,  //d代表db.CTR_FILES_Ts(cd可以自己定義名稱)，這邊放次資料表要串聯的key
                   (c, d) => new         //將兩個自定義名稱用小括胡包起來，接著透過 『=>』 可以自己選擇要取用要用資料 
                   {
                       x = c.FileInstance,
                       e = d.CtrPickedId
                   }
                   ).Where(x => x.e == id).ToList();
                List<string> vs = new List<string>();
                for (int i = 0; i < ctrPhoto.Count; i++)
                {
                    vs.Add(Convert.ToBase64String(ctrPhoto[i].x));
                }
                return vs;
            }
        }

        /// <summary>
        /// 入庫檢貨轉歷史檢貨
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public void PickToPickHT(long CTR_HEADER_ID)
        {

            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"INSERT INTO [CTR_PICKED_HT]
           ([CTR_PICKED_ID],[CTR_HEADER_ID],[CTR_DETAIL_ID],[STOCK_ID],[LOCATOR_ID]
           ,[LOCATOR_CODE],[BARCODE],[INVENTORY_ITEM_ID],[SHIP_ITEM_NUMBER],[PAPER_TYPE]
		   ,[BASIC_WEIGHT],[REAM_WEIGHT],[ROLL_REAM_WT],[SPECIFICATION],[PACKING_TYPE]
           ,[SHIP_MT_QTY],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],[PRIMARY_QUANTITY],[PRIMARY_UOM]
           ,[SECONDARY_QUANTITY],[SECONDARY_UOM],[LOT_NUMBER],[THEORY_WEIGHT],[ITEM_CATEGORY]
           ,[STATUS],[REASON_CODE],[REASON_DESC],[NOTE],[CREATED_BY]
           ,[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME])
SELECT [CTR_PICKED_ID],[CTR_HEADER_ID],[CTR_DETAIL_ID],[STOCK_ID],[LOCATOR_ID]
           ,[LOCATOR_CODE],[BARCODE],[INVENTORY_ITEM_ID],[SHIP_ITEM_NUMBER],[PAPER_TYPE]
		   ,[BASIC_WEIGHT],[REAM_WEIGHT],[ROLL_REAM_WT],[SPECIFICATION],[PACKING_TYPE]
           ,[SHIP_MT_QTY],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],[PRIMARY_QUANTITY],[PRIMARY_UOM]
           ,[SECONDARY_QUANTITY],[SECONDARY_UOM],[LOT_NUMBER],[THEORY_WEIGHT],[ITEM_CATEGORY]
           ,[STATUS],[REASON_CODE],[REASON_DESC],[NOTE],[CREATED_BY]
           ,[CREATED_USER_NAME],[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[LAST_UPDATE_USER_NAME]
FROM CTR_PICKED_T p
where p.CTR_HEADER_ID = @CTR_HEADER_ID");
                    mesContext.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }

        }

        /// <summary>
        /// 入庫檢或刪除
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public void PickTDelete(long CTR_HEADER_ID)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"delete CTR_PICKED_T 
where CTR_HEADER_ID = @CTR_HEADER_ID");
                    mesContext.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }



        public class PurchaseStatusCode
        {
            /// <summary>
            /// 已入庫
            /// </summary>
            public const string PurchaseHeaderAlready = "0";

            /// <summary>
            /// 待入庫
            /// </summary>
            public const string PurchaseHeaderPending = "1";

            /// <summary>
            /// 取消
            /// </summary>
            public const string PurchaseHeaderCancel = "2";
        }

    }


}