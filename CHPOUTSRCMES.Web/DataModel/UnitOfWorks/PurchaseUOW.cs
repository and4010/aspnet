using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Util;
using DataTables;
using Microsoft.Reporting.WebForms;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class PurchaseUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<CTR_ORG_T> ctrOrgTRepository;
        private readonly IRepository<CTR_HEADER_T> ctrHeaderTRepository;
        private readonly IRepository<CTR_DETAIL_T> ctrDetailTRepository;
        private readonly IRepository<CTR_PICKED_T> ctrPickedTRepository;
        private readonly IRepository<CTR_PICKED_HT> ctrPickedHtRepository;
        private readonly IRepository<CTR_FILEINFO_T> ctrFileInfoTRepository;
        private readonly IRepository<CTR_FILES_T> ctrFilesTRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public PurchaseUOW(DbContext context) : base(context)
        {
            this.ctrOrgTRepository = new GenericRepository<CTR_ORG_T>(this);
            this.ctrHeaderTRepository = new GenericRepository<CTR_HEADER_T>(this);
            this.ctrDetailTRepository = new GenericRepository<CTR_DETAIL_T>(this);
            this.ctrPickedTRepository = new GenericRepository<CTR_PICKED_T>(this);
            this.ctrPickedHtRepository = new GenericRepository<CTR_PICKED_HT>(this);
            this.ctrFileInfoTRepository = new GenericRepository<CTR_FILEINFO_T>(this);
            this.ctrFilesTRepository = new GenericRepository<CTR_FILES_T>(this);
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
                    generateFlatDetail();
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
                ctrorg.BlNo = "123456";
                ctrorg.LineId = 1;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "TB3";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503376;
                ctrorg.ShipItemNumber = "4DM00A03000407K471K";
                ctrorg.PaperType = "DM00";
                ctrorg.BasicWeight = "03000";
                ctrorg.ReamWeight = "299.11";
                ctrorg.RollReamQty = 2;
                ctrorg.RollReamWt = 3000;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "407K471K";
                ctrorg.PackingType = "令包";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 3;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 3000;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 3000;
                ctrorg.SecondaryUom = "RE";
                ctrorg.CreatedBy = "1";
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = "1";
                ctrorg.LastUpdateDate = DateTime.Now;
                ctrOrgTRepository.Create(ctrorg, true);

                ctrorg.CtrOrgId = 2;
                ctrorg.ProcessCode = "XXIFP217";
                ctrorg.ServerCode = "123";
                ctrorg.BatchId = "20200721141600100000";
                ctrorg.BatchLineId = 2;
                ctrorg.HeaderId = 2;
                ctrorg.OrgId = 2;
                ctrorg.OrgName = "入庫";
                ctrorg.BlNo = "123456";
                ctrorg.LineId = 2;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "TB3";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503375;
                ctrorg.ShipItemNumber = "4DM00A03000386K471K";
                ctrorg.PaperType = "DM00";
                ctrorg.BasicWeight = "03000";
                ctrorg.ReamWeight = "02200";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "352K471K";
                ctrorg.PackingType = "無";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 0.616M;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 616;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 0;
                ctrorg.SecondaryUom = "";
                ctrorg.CreatedBy = "1";
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = "1";
                ctrorg.LastUpdateDate = DateTime.Now;
                ctrOrgTRepository.Create(ctrorg, true);

                ctrorg.CtrOrgId = 3;
                ctrorg.ProcessCode = "XXIFP217";
                ctrorg.ServerCode = "123";
                ctrorg.BatchId = "20200721141600100000";
                ctrorg.BatchLineId = 3;
                ctrorg.HeaderId = 3;
                ctrorg.OrgId = 3;
                ctrorg.OrgName = "入庫";
                ctrorg.BlNo = "123456";
                ctrorg.LineId = 3;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "TB3";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503374;
                ctrorg.ShipItemNumber = "4DM00A03000352K471K";
                ctrorg.PaperType = "DM00";
                ctrorg.BasicWeight = "03000";
                ctrorg.ReamWeight = "02200";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "386K471K";
                ctrorg.PackingType = "無";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 0.440M;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 440;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 0;
                ctrorg.SecondaryUom = "";
                ctrorg.CreatedBy = "1";
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = "1";
                ctrorg.LastUpdateDate = DateTime.Now;
                ctrOrgTRepository.Create(ctrorg, true);
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

            var org = ctrOrgTRepository.GetAll().AsNoTracking().ToList();

            try
            {
                for (int i = 0; org.Count() > i; i++)
                {
                    var ctrHeaderTContainerNo = org[i].ContainerNo;
                    var ContainerNo = ctrHeaderTRepository.GetAll()
                        .Where(x => x.ContainerNo == ctrHeaderTContainerNo).SingleOrDefault();
                    if (ContainerNo == null)
                    {
                        ctrheaderT.HeaderId = org[i].HeaderId;
                        ctrheaderT.OrgId = org[i].OrgId;
                        ctrheaderT.OrgName = org[i].OrgName;
                        ctrheaderT.BlNo = org[i].BlNo;
                        ctrheaderT.LineId = org[i].LineId;
                        ctrheaderT.ContainerNo = org[i].ContainerNo;
                        ctrheaderT.MvContainerDate = org[i].MvContainerDate;
                        ctrheaderT.OrganizationId = org[i].OrganizationId;
                        ctrheaderT.OrganizationCode = org[i].OrganizationCode;
                        ctrheaderT.Subinventory = org[i].Subinventory;
                        ctrheaderT.Status = PurchaseStatusCode.GetCode(PurchaseStatusCode.Pending);
                        ctrheaderT.CreatedBy = org[i].CreatedBy.ToString();
                        ctrheaderT.CreatedUserName = org[i].CreatedBy.ToString();
                        ctrheaderT.CreationDate = org[i].CreationDate;
                        ctrHeaderTRepository.Create(ctrheaderT, true);
                    }
                    else if (ContainerNo.ContainerNo != org[i].ContainerNo)
                    {
                        ctrheaderT.HeaderId = org[i].HeaderId;
                        ctrheaderT.OrgId = org[i].OrgId;
                        ctrheaderT.OrgName = org[i].OrgName;
                        ctrheaderT.BlNo = org[i].BlNo;
                        ctrheaderT.LineId = org[i].LineId;
                        ctrheaderT.ContainerNo = org[i].ContainerNo;
                        ctrheaderT.MvContainerDate = org[i].MvContainerDate;
                        ctrheaderT.OrganizationId = org[i].OrganizationId;
                        ctrheaderT.OrganizationCode = org[i].OrganizationCode;
                        ctrheaderT.Subinventory = org[i].Subinventory;
                        ctrheaderT.Status = PurchaseStatusCode.GetCode(PurchaseStatusCode.Pending);
                        ctrheaderT.CreatedBy = org[i].CreatedBy.ToString();
                        ctrheaderT.CreatedUserName = org[i].CreatedBy.ToString();
                        ctrheaderT.CreationDate = org[i].CreationDate;
                        ctrHeaderTRepository.Create(ctrheaderT, true);
                    }


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
                    if (org[i].SecondaryQuantity > 0)
                    {
                        ctrdetailT.ItemCategory = "平版";
                    }
                    else
                    {
                        ctrdetailT.ItemCategory = "捲筒";
                    }
                    ctrdetailT.CreatedBy = org[i].CreatedBy.ToString();
                    ctrdetailT.CreatedUserName = org[i].CreatedBy.ToString();
                    ctrdetailT.CreationDate = org[i].CreationDate;
                    ctrDetailTRepository.Create(ctrdetailT, true);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
            }

        }

        public CTR_HEADER_T GetHeader(long CtrHeaderId)
        {
            try
            {
                return ctrHeaderTRepository.Get(x => x.CtrHeaderId == CtrHeaderId).SingleOrDefault();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }

        }

        /// <summary>
        /// 紙捲匯入pickt資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <param name="PaperRollModel"></param>
        /// <returns></returns>
        public ResultModel ImportPaperRollDetail(long CtrHeaderId, List<DetailModel.RollDetailModel> PaperRollModel, string createby, string userName)
        {

            CTR_PICKED_T cTR_PICKED_T = new CTR_PICKED_T();
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var ctrpick = ctrPickedTRepository.Get(x => x.ItemCategory == "捲筒").ToList();
                    if (ctrpick.Count > 0)
                    {
                        var m = DeleteExcel(CtrHeaderId);
                        if (!m.Success) return m;
                        //return new ResultModel(false, "資料已存在無法匯入");
                    }



                    var ctrDetail = ctrDetailTRepository.GetAll().Join(
                    ctrHeaderTRepository.GetAll(),               //要Join的資料表
                    c => c.CtrHeaderId,    //c代表db.CTR_DETAIL_Ts(c可以自己定義名稱)，這邊放主要資料表要串聯的key
                    cd => cd.CtrHeaderId,  //cd代表db.CTR_HEADER_Ts(cd可以自己定義名稱)，這邊放次資料表要串聯的key
                    (c, cd) => new         //將兩個自定義名稱用小括胡包起來，接著透過 『=>』 可以自己選擇要取用要用資料 
                    {
                        d = c,
                        e = cd
                    }
                    ).Where(x => x.d.ItemCategory == "捲筒" && x.e.CtrHeaderId == CtrHeaderId).ToList();
                    var header = ctrHeaderTRepository.Get(x => x.CtrHeaderId == CtrHeaderId).SingleOrDefault();
                    var barcode = GenerateBarcodes(header.OrganizationId, header.Subinventory, PaperRollModel.Count, userName);
                    for (int j = 0; j < ctrDetail.Count; j++)
                    {

                        for (int i = 0; i < PaperRollModel.Count; i++)
                        {
                            if (ctrDetail[j].d.ShipItemNumber == PaperRollModel[i].Item_No)
                            {
                                cTR_PICKED_T.CtrHeaderId = ctrDetail[j].d.CtrHeaderId;
                                cTR_PICKED_T.CtrDetailId = ctrDetail[j].d.CtrDetailId;
                                cTR_PICKED_T.StockId = null;
                                cTR_PICKED_T.LocatorId = ctrDetail[j].d.LocatorId;
                                cTR_PICKED_T.LocatorCode = ctrDetail[j].d.LocatorCode;
                                cTR_PICKED_T.Barcode = barcode.Data[i];
                                cTR_PICKED_T.InventoryItemId = ctrDetail[j].d.InventoryItemId;
                                cTR_PICKED_T.ShipItemNumber = PaperRollModel[i].Item_No;
                                cTR_PICKED_T.PaperType = ctrDetail[j].d.PaperType;
                                cTR_PICKED_T.BasicWeight = ctrDetail[j].d.BasicWeight;
                                cTR_PICKED_T.ReamWeight = ctrDetail[j].d.ReamWeight;
                                cTR_PICKED_T.RollReamWt = ctrDetail[j].d.RollReamWt;
                                cTR_PICKED_T.Specification = ctrDetail[j].d.Specification;
                                cTR_PICKED_T.PackingType = ctrDetail[j].d.PackingType;
                                cTR_PICKED_T.ShipMtQty = ctrDetail[j].d.ShipMtQty ?? 0;
                                cTR_PICKED_T.TransactionQuantity = ConvertTxnQty(ctrDetail[j].d.InventoryItemId, PaperRollModel[i].PrimanyQuantity, PaperRollModel[i].PrimaryUom, ctrDetail[j].d.TransactionUom);
                                cTR_PICKED_T.TransactionUom = ctrDetail[j].d.TransactionUom;
                                cTR_PICKED_T.PrimaryQuantity = PaperRollModel[i].PrimanyQuantity;
                                cTR_PICKED_T.PrimaryUom = PaperRollModel[i].PrimaryUom;
                                cTR_PICKED_T.SecondaryQuantity = ctrDetail[j].d.SecondaryQuantity;
                                cTR_PICKED_T.SecondaryUom = ctrDetail[j].d.SecondaryUom;
                                cTR_PICKED_T.LotNumber = PaperRollModel[i].LotNumber;
                                cTR_PICKED_T.TheoryWeight = PaperRollModel[i].TheoreticalWeight;
                                cTR_PICKED_T.ItemCategory = ctrDetail[j].d.ItemCategory;
                                cTR_PICKED_T.Status = PickingStatusCode.NOT_PRINTED;
                                cTR_PICKED_T.ReasonCode = "";
                                cTR_PICKED_T.ReasonDesc = "";
                                cTR_PICKED_T.Note = "";
                                cTR_PICKED_T.CreatedBy = createby;
                                cTR_PICKED_T.CreationDate = DateTime.Now;
                                cTR_PICKED_T.CreatedUserName = userName;
                                ctrPickedTRepository.Create(cTR_PICKED_T, true);
                            }
                        }
                    }
                    txn.Commit();
                    return new ResultModel(true, "匯入成功");


                }
                catch (Exception e)
                {
                    txn.Rollback();
                    logger.Error(e.Message);
                    return new ResultModel(false, e.Message.ToString());
                }
            }
        }


        private decimal ConvertTxnQty(long itemId, decimal pryQty, string pryUom, string txnUom)
        {
            if (pryUom.CompareTo(txnUom) == 0)
            {
                return pryQty;
            }

            var model = uomConversion.Convert(itemId, pryQty, pryUom, txnUom);
            if (!model.Success)
                throw new Exception(model.Msg);

            return model.Data;
        }
        /// <summary>
        /// 刪除excel資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public ResultModel DeleteExcel(long CtrHeaderId)
        {

            try
            {

                StringBuilder query = new StringBuilder();
                query.Append(
                @"DELETE p
FROM CTR_PICKED_T p
INNER JOIN CTR_HEADER_T h
INNER JOIN CTR_DETAIL_T d
ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
ON d.CTR_DETAIL_ID = p.CTR_DETAIL_ID
where h.CTR_HEADER_ID = @CTR_HEADER_ID
and d.ITEM_CATEGORY = N'捲筒'");
                this.Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId));
                return new ResultModel(true, "捲筒刪除成功");


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new ResultModel(false, e.Message.ToString());
            }

        }

        /// <summary>
        /// 平版明細資料
        /// </summary>
        public void generateFlatDetail()
        {
            var ctrDetail = ctrDetailTRepository.Get(s => s.ItemCategory == "平版").ToList();

            CTR_PICKED_T cTR_PICKED_T = new CTR_PICKED_T();
            try
            {
                for (int i = 0; i < ctrDetail.Count; i++)
                {
                    var HeaderId = ctrDetail[i].HeaderId;
                    var ctrheader = ctrHeaderTRepository.Get(s => s.HeaderId == HeaderId).SingleOrDefault();
                    for (int j = 0; j < decimal.ToInt32(ctrDetail[i].RollReamQty); j++)
                    {
                        cTR_PICKED_T.CtrHeaderId = HeaderId;
                        cTR_PICKED_T.CtrDetailId = ctrDetail[i].CtrDetailId;
                        cTR_PICKED_T.StockId = null;
                        cTR_PICKED_T.LocatorId = ctrDetail[i].LocatorId;
                        cTR_PICKED_T.LocatorCode = ctrDetail[i].LocatorCode;
                        cTR_PICKED_T.Barcode = GenerateBarcodes(ctrheader.OrganizationId, ctrheader.Subinventory, ctrDetail.Count, "1").Data[i];
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
                        cTR_PICKED_T.LotNumber = "";
                        cTR_PICKED_T.TheoryWeight = "";
                        cTR_PICKED_T.ItemCategory = ctrDetail[i].ItemCategory;
                        cTR_PICKED_T.Status = PickingStatusCode.NOT_PRINTED;
                        cTR_PICKED_T.ReasonCode = "";
                        cTR_PICKED_T.ReasonDesc = "";
                        cTR_PICKED_T.Note = "";
                        cTR_PICKED_T.CreatedBy = ctrDetail[i].CreatedBy;
                        cTR_PICKED_T.CreationDate = DateTime.Now;
                        cTR_PICKED_T.CreatedUserName = ctrDetail[i].CreatedUserName;
                        ctrPickedTRepository.Create(cTR_PICKED_T, true);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                new ResultModel(false, e.Message.ToString());
            }

        }

        /// <summary>
        /// 取得行事曆資料
        /// </summary>
        /// <returns></returns>
        public List<FullCalendarEventModel> getFullCalenderList(string Subinventory)
        {
            var header = ctrHeaderTRepository.Get(x => x.Subinventory == Subinventory).GroupBy(x => x.ContainerNo).Select(x => x.FirstOrDefault()).ToList();

            List<FullCalendarEventModel> fullCalendarEventModel = new List<FullCalendarEventModel>();
            UrlHelper objUrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            for (int i = 0; i < header.Count; i++)
            {
                string headerStatus = header[i].Status.ToString();
                var startTime = ConvertDateTime.ConverYYYY(header[i].MvContainerDate);
                switch (headerStatus)
                {
                    case PurchaseStatusCode.Pending:
                        
                        fullCalendarEventModel.Add(new FullCalendarEventModel()
                        {
                            id = header[i].CtrHeaderId,
                            title = $" {header[i].MvContainerDate:HH:mm}  {header[i].Subinventory} \n {header[i].ContainerNo} 待入庫",
                            start = startTime,
                            end = startTime,
                            allDay = false,
                            url = objUrlHelper.Action("Detail", "Purchase", new
                            {
                                Id = header[i].CtrHeaderId
                            }),
                            Status = header[i].Status
                        });
                        break;
                    case PurchaseStatusCode.Cancel:
                        fullCalendarEventModel.Add(new FullCalendarEventModel()
                        {
                            id = header[i].CtrHeaderId,
                            title = $" {header[i].MvContainerDate:HH:mm}  {header[i].Subinventory} \n {header[i].ContainerNo} 取消",
                            start = startTime,
                            end = startTime,
                            allDay = false,
                            url = "",
                            Status = header[i].Status,
                            color = "#E60000"
                        });
                        break;
                    case PurchaseStatusCode.Already:
                        fullCalendarEventModel.Add(new FullCalendarEventModel()
                        {
                            id = header[i].CtrHeaderId,
                            title = $" {header[i].MvContainerDate:HH:mm}  {header[i].Subinventory} \n {header[i].ContainerNo} 已入庫",
                            start = startTime,
                            end = startTime,
                            allDay = false,
                            url = objUrlHelper.Action("Detail", "Purchase", new
                            {
                                Id = header[i].CtrHeaderId
                            }),
                            Status = header[i].Status,
                        });
                        break;
                    default:
                        break;
                }
            }

            return fullCalendarEventModel;
        }

        /// <summary>
        /// 存檔入庫header轉狀態
        /// </summary>
        /// <param name="ContainerNo"></param>
        /// <returns></returns>
        public ResultModel ChangeHeaderStatus(long CtrHeaderId)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {

                    var pick = ctrPickedTRepository.Get(x => x.CtrHeaderId == CtrHeaderId && x.Status != PickingStatusCode.ALREADY).Count();
                    if (pick > 0)
                    {
                        return new ResultModel(false, "有條碼尚未入庫");
                    }
                    var header = ctrHeaderTRepository.Get(x => x.CtrHeaderId == CtrHeaderId).FirstOrDefault();
                    if (header != null)
                    {
                        //for (int i = 0; i < header.Count; i++)
                        //{
                        header.Status = PurchaseStatusCode.GetCode(PurchaseStatusCode.Already);
                        ctrHeaderTRepository.Update(header, true);
                        ConvertStock(header.CtrHeaderId);
                        StockRecord(header.CtrHeaderId);
                        PickToPickHT(header.CtrHeaderId);
                        PickTDelete(header.CtrHeaderId);
                        DetailToDetailHT(header.CtrHeaderId);
                        //}
                        txn.Commit();
                        return new ResultModel(true, "成功"); ;
                    }
                }
                catch (Exception e)
                {

                    logger.Error(e.Message);
                    return new ResultModel(false, e.Message.ToString());
                }
                finally
                {
                    txn.Rollback();
                }
            }
            return new ResultModel(false, "失敗");
        }

        /// <summary>
        /// 取得平版header資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.FlatModel> GetFlatHeaderList(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                @"SELECT 
ROW_NUMBER() OVER(ORDER BY d.CTR_DETAIL_ID ) AS SubId,
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
FROM CTR_DETAIL_T d
JOIN CTR_HEADER_T h ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
WHERE d.ITEM_CATEGORY = N'平版' and h.CTR_HEADER_ID = @CTR_HEADER_ID");
                string commandText = string.Format(query.ToString());
                commandText = string.Concat(commandText, " UNION ", commandText.Replace("CTR_DETAIL_T", "CTR_DETAIL_HT"));
                return this.Context.Database.SqlQuery<DetailModel.FlatModel>(commandText, new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).ToList();

            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<DetailModel.FlatModel>();
            }
        }

        /// <summary>
        /// 取得紙捲header資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.RollModel> GetPaperRollHeaderList(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                @"SELECT 
ROW_NUMBER() OVER(ORDER BY d.CTR_DETAIL_ID ) AS SubId,
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
FROM CTR_DETAIL_T d
JOIN CTR_HEADER_T h ON h.CTR_HEADER_ID = d.CTR_HEADER_ID
WHERE d.ITEM_CATEGORY = N'捲筒' and h.CTR_HEADER_ID = @CTR_HEADER_ID");
                string commandText = string.Format(query.ToString());
                commandText = string.Concat(commandText, " UNION ", commandText.Replace("CTR_DETAIL_T", "CTR_DETAIL_HT"));
                return Context.Database.SqlQuery<DetailModel.RollModel>(commandText, new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).ToList();

            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<DetailModel.RollModel>();
            }
        }

        /// <summary>
        /// 取得紙捲明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.RollDetailModel> GetPaperRollDetailList(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                @"SELECT 
ROW_NUMBER() OVER(ORDER BY p.CTR_DETAIL_ID ) AS SubId,
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
FROM CTR_PICKED_T p
LEFT JOIN CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h.CTR_HEADER_ID = @CTR_HEADER_ID");
                string commandText = string.Format(query.ToString());
                commandText = string.Concat(commandText, " UNION ", commandText.Replace("CTR_PICKED_T", "CTR_PICKED_HT"));
                return Context.Database.SqlQuery<DetailModel.RollDetailModel>(commandText, new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).ToList();

            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<DetailModel.RollDetailModel>();
            }
        }

        /// <summary>
        /// 取得平版明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.FlatDetailModel> GetFlatDetailList(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                @"SELECT 
ROW_NUMBER() OVER(ORDER BY p.CTR_DETAIL_ID ) AS SubId,
p.CTR_PICKED_ID as Id,
h.SUBINVENTORY as Subinventory, 
p.LOCATOR_CODE as Locator,
p.BARCODE as Barcode,
p.SHIP_ITEM_NUMBER as Item_No,
p.REAM_WEIGHT as ReamWeight,
p.PACKING_TYPE as PackingType,
p.ROLL_REAM_WT as Pieces_Qty,
P.TRANSACTION_QUANTITY as Qty,
p.STATUS as Status,
p.REASON_DESC as Reason,
p.NOTE as Remark
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平版' and h.CTR_HEADER_ID = @CTR_HEADER_ID");
                string commandText = string.Format(query.ToString());
                commandText = string.Concat(commandText, " UNION ", commandText.Replace("CTR_PICKED_T", "CTR_PICKED_HT"));
                return Context.Database.SqlQuery<DetailModel.FlatDetailModel>(commandText, new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).ToList();

            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<DetailModel.FlatDetailModel>();
            }
        }

        /// <summary>
        /// 取得捲筒編輯資料&&檢視資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailModel.RollDetailModel GetPaperRollEditView(long CTR_PICKED_ID)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                @"SELECT 
ROW_NUMBER() OVER(ORDER BY p.CTR_DETAIL_ID ) AS SubId,
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
p.NOTE as Remark,
p.LAST_UPDATE_DATE as CreationDate,
p.LAST_UPDATE_USER_NAME as CreatedUserName 
FROM dbo.CTR_PICKED_T p
LEFT JOIN dbo.CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and p.CTR_PICKED_ID  = @CTR_PICKED_ID");
                string commandText = string.Format(query.ToString());
                commandText = string.Concat(commandText, " UNION ", commandText.Replace("CTR_PICKED_T", "CTR_PICKED_HT").Replace("CTR_DETAIL_T", "CTR_DETAIL_HT"));
                return Context.Database.SqlQuery<DetailModel.RollDetailModel>(commandText, new SqlParameter("@CTR_PICKED_ID", CTR_PICKED_ID)).SingleOrDefault();

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new DetailModel.RollDetailModel();
            }
        }

        /// <summary>
        /// 紙捲寫入原因儲位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="Locator"></param>
        /// <param name="Remark"></param>
        public ResultModel PaperRollEdit(HttpFileCollectionBase File, long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var mes = this.Context.Database.BeginTransaction())
            {
                try
                {
                    if (File != null || File.Count != 0)
                    {
                        foreach (HttpPostedFileBase i in File)
                        {
                            SaveCtrFileInfoT(VaryQualityLevel(i), i.FileName, i.ContentType, id, LastUpdateBy);
                        }
                    }
                    var ctrPickT = ctrPickedTRepository.Get(x => x.CtrPickedId == id).SingleOrDefault();
                    if (Reason != "請選擇")
                    {
                        var reason = stkReasonTRepository.Get(x => x.ReasonCode == Reason).SingleOrDefault();
                        ctrPickT.ReasonDesc = reason.ReasonDesc;
                        ctrPickT.ReasonCode = reason.ReasonCode;
                    }
                    if (Locator != "null")
                    {
                        var LocatorId = Int32.Parse(Locator);
                        var Id = locatorTRepository.Get(x => x.LocatorId == LocatorId).SingleOrDefault();
                        ctrPickT.LocatorId = Id.LocatorId;
                        ctrPickT.LocatorCode = Id.LocatorSegments;
                    }
                    ctrPickT.Note = Remark;
                    ctrPickT.LastUpdateBy = LastUpdateBy;
                    ctrPickT.LastUpdateUserName = LastUpdateUserName;
                    ctrPickT.LastUpdateDate = DateTime.Now;
                    ctrPickedTRepository.Update(ctrPickT, true);
                    mes.Commit();
                    return new ResultModel(true, "");
                }

                catch (Exception e)
                {
                    mes.Rollback();
                    logger.Error(e.Message);
                    return new ResultModel(false, e.Message);
                }
            }
        }

        /// <summary>
        /// 平版寫入原因儲位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="Locator"></param>
        /// <param name="Remark"></param>
        public ResultModel FlatEdit(HttpFileCollectionBase Files, long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {
            using var transaction = this.Context.Database.BeginTransaction();
            try
            {
                var ctrPickT = ctrPickedTRepository.Get(x => x.CtrPickedId == id).SingleOrDefault();

                if (ctrPickT == null)
                {
                    throw new Exception("揀貨資料不存在!!");
                }

                if (Files != null || Files.Count > 0)
                {
                    foreach (HttpPostedFileBase i in Files)
                    {
                        
                        SaveCtrFileInfoT(VaryQualityLevel(i), i.FileName, i.ContentType, ctrPickT.CtrPickedId, LastUpdateBy);
                    }
                }

                if (Reason != "請選擇")
                {
                    var reason = stkReasonTRepository.Get(x => x.ReasonCode == Reason).SingleOrDefault();
                    if (reason != null)
                    {
                        ctrPickT.ReasonDesc = reason.ReasonDesc;
                        ctrPickT.ReasonCode = reason.ReasonCode;
                    }
                }

                if (Locator != "null")
                {
                    var locatorId = Int32.Parse(Locator);
                    var locator = locatorTRepository.Get(x => x.LocatorId == locatorId).SingleOrDefault();
                    ctrPickT.LocatorId = locator.LocatorId;
                    ctrPickT.LocatorCode = locator.LocatorSegments;
                }

                ctrPickT.Note = Remark;
                ctrPickT.LastUpdateBy = LastUpdateBy;
                ctrPickT.LastUpdateUserName = LastUpdateUserName;
                ctrPickT.LastUpdateDate = DateTime.Now;
                ctrPickedTRepository.Update(ctrPickT, true);
                transaction.Commit();
                return new ResultModel(true, "");
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.Error(e.Message);
                return new ResultModel(false, e.Message);
            }


        }

        /// <summary>
        /// 紙捲條碼已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public ResultModel SavePaperRollBarcode(String Barcode, long CtrHeaderId, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var ctrPickT = ctrPickedTRepository.Get(x => x.Barcode == Barcode && x.ItemCategory == "捲筒" && x.CtrHeaderId == CtrHeaderId).SingleOrDefault();
                    if (ctrPickT == null)
                    {
                        return new ResultModel(false, "此無條碼!!");
                    }

                    switch (ctrPickT.Status)
                    {
                        case PickingStatusCode.ALREADY:
                            return new ResultModel(false, $"條碼{ctrPickT.Barcode}-已入庫!!");
                        case PickingStatusCode.NOT_PRINTED:
                            return new ResultModel(false, $"條碼{ctrPickT.Barcode}-未列印標籤!!");
                        case PickingStatusCode.PENDING:
                        default:
                            ctrPickT.Status = PickingStatusCode.ALREADY;
                            ctrPickT.LastUpdateBy = LastUpdateBy;
                            ctrPickT.LastUpdateUserName = LastUpdateUserName;
                            ctrPickT.LastUpdateDate = DateTime.Now;
                            ctrPickedTRepository.Update(ctrPickT, true);
                            txn.Commit();
                            return new ResultModel(true, "");
                    }

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
        /// 平版條碼已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public ResultModel SaveFlatBarcode(String Barcode, long CtrHeaderId, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var ctrPickT = ctrPickedTRepository.Get(x => x.Barcode == Barcode && x.ItemCategory == "平版" && x.CtrHeaderId == CtrHeaderId).SingleOrDefault();
                    if (ctrPickT == null)
                    {
                        return new ResultModel(false, "此無條碼");
                    }
                    switch (ctrPickT.Status)
                    {
                        case PickingStatusCode.ALREADY:
                            return new ResultModel(false, $"條碼{ctrPickT.Barcode}-已入庫!!");
                        case PickingStatusCode.NOT_PRINTED:
                            return new ResultModel(false, $"條碼{ctrPickT.Barcode}-未列印標籤!!");
                        case PickingStatusCode.PENDING:
                        default:
                            ctrPickT.Status = PickingStatusCode.ALREADY;
                            ctrPickedTRepository.Update(ctrPickT, true);
                            ctrPickT.LastUpdateBy = LastUpdateBy;
                            ctrPickT.LastUpdateUserName = LastUpdateUserName;
                            ctrPickT.LastUpdateDate = DateTime.Now;
                            txn.Commit();
                            return new ResultModel(true, "");
                    }
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
        /// 取得編輯平版資料&&檢視資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailModel.FlatDetailModel GetFlatEditView(long CtrPickedId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                @"SELECT 
ROW_NUMBER() OVER(ORDER BY p.CTR_DETAIL_ID ) AS SubId,
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
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h ON h.CTR_HEADER_ID = p.CTR_HEADER_ID
JOIN CTR_DETAIL_T D ON D.CTR_DETAIL_ID = P.CTR_DETAIL_ID
WHERE p.ITEM_CATEGORY = N'平版' and p.CTR_PICKED_ID  = @CTR_PICKED_ID");
                string commandText = string.Format(query.ToString());
                commandText = string.Concat(commandText, " UNION ", commandText.Replace("CTR_PICKED_T", "CTR_PICKED_HT").Replace("CTR_DETAIL_T", "CTR_DETAIL_HT"));
                return Context.Database.SqlQuery<DetailModel.FlatDetailModel>(commandText, new SqlParameter("@CTR_PICKED_ID", CtrPickedId)).SingleOrDefault();
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new DetailModel.FlatDetailModel();
            }
        }

        /// <summary>
        /// 取得頁籤平版數量
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public decimal GetFlatNumberTab(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                $@"SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平版' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
FROM CTR_DETAIL_T d1
JOIN CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'平版' and h1.CTR_HEADER_ID  = @CTR_HEADER_ID");
                return Context.Database.SqlQuery<decimal>(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).SingleOrDefault();


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 取得頁籤紙捲數量
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public decimal GetPaperRollNumberTab(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                $@"SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
FROM CTR_DETAIL_T d1
JOIN CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'捲筒' and h1.CTR_HEADER_ID  = @CTR_HEADER_ID");
                return Context.Database.SqlQuery<decimal>(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).SingleOrDefault();


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return 0;
            }
        }

        //public void SavePhoto(HttpPostedFileBase file, long id, string createby)
        //{
        //    //using (var txn = this.Context.Database.BeginTransaction())
        //    //{
        //    //    try
        //    //    {
        //    //        using (var mescontext = new MesContext())
        //    //        {
        //    //            SaveCtrFileInfoT(VaryQualityLevel(file), file, id, createby);
        //    //        }
        //    //        txn.Commit();
        //    //    }
        //    //    catch (Exception e)
        //    //    {
        //    //        txn.Rollback();
        //    //        logger.Error(e.Message);
        //    //    }
        //    //}
        //    SaveCtrFileInfoT(VaryQualityLevel(file), file, id, createby);
        //}

        /// <summary>
        /// 儲存照片FileInfo Table
        /// </summary>
        /// <param name="filebyte"></param>
        /// <param name="file"></param>
        public void SaveCtrFileInfoT(byte[] filebyte, string filename, string fileType, long id, string CreatedBy)
        {
            CTR_FILES_T cTR_FILES_T = new CTR_FILES_T();
            cTR_FILES_T.FileInstance = filebyte;
            ctrFilesTRepository.Create(cTR_FILES_T, true);

            CTR_FILEINFO_T cTR_FILEINFO_T = new CTR_FILEINFO_T();
            cTR_FILEINFO_T.CtrPickedId = id;
            cTR_FILEINFO_T.CtrFileId = cTR_FILES_T.CtrFileId;
            cTR_FILEINFO_T.FileType = fileType;
            cTR_FILEINFO_T.FileName = filename;
            cTR_FILEINFO_T.Size = filebyte.Length;
            cTR_FILEINFO_T.Seq = 1;
            cTR_FILEINFO_T.CreatedBy = CreatedBy;
            cTR_FILEINFO_T.CreationDate = DateTime.Now;
            ctrFileInfoTRepository.Create(cTR_FILEINFO_T);

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
        //        ctrFilesTRepository.Create(cTR_FILES_T, true);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //    }
        //}

        ///// <summary>
        ///// 壓縮照片大小
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //public byte[] VaryQualityLevel(HttpPostedFileBase file)
        //{
        //    using (var thumb = Image.FromStream(file.InputStream))
        //    {
        //        var jpgInfo = GetEncoder(ImageFormat.Jpeg); /* Returns array of image encoder objects built into GDI+ */
        //        using (var samllfile = new MemoryStream())
        //        {
        //            //    // Create an EncoderParameters object.  
        //            //    // An EncoderParameters object has an array of EncoderParameter  
        //            //    // objects. In this case, there is only one  
        //            //    // EncoderParameter object in the array.  
        //            EncoderParameters myEncoderParameters = new EncoderParameters(1);
        //            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
        //            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 30L);
        //            thumb.Save(samllfile, jpgInfo, myEncoderParameters);
        //            return samllfile.ToArray();
        //        }
        //    };

        //}

        ///// <summary>
        ///// 轉換
        ///// </summary>
        ///// <param name="format"></param>
        ///// <returns></returns>
        //private static ImageCodecInfo GetEncoder(ImageFormat format)
        //{
        //    ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == format.Guid).FirstOrDefault();
        //    if (codec == null)
        //    {
        //        return null;
        //    }
        //    return codec;

        //}

        /// <summary>
        /// 取得相片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetPhoto(long id)
        {
            var db = (MesContext)Context;

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


        public List<long> GetPhotoList(long pickedId)
        {
            return ctrFileInfoTRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.CtrPickedId == pickedId)
                .Select(x=>x.CtrFileinfoId)
                .ToList();
        }

        public string GetPhotoByInfoId(long infoId)
        {
            var db = (MesContext)Context;

            var ctrPhoto = db.CTR_FILES_Ts.Join(
               db.CTR_FILEINFO_Ts,               //要Join的資料表
               c => c.CtrFileId,    //c代表db.CTR_FILEINFO_Ts(c可以自己定義名稱)，這邊放主要資料表要串聯的key
               d => d.CtrFileId,  //d代表db.CTR_FILES_Ts(cd可以自己定義名稱)，這邊放次資料表要串聯的key
               (c, d) => new         //將兩個自定義名稱用小括胡包起來，接著透過 『=>』 可以自己選擇要取用要用資料 
               {
                   x = c.FileInstance,
                   e = d.CtrFileinfoId
               }
               ).Where(x => x.e == infoId).FirstOrDefault();


            return ctrPhoto != null ? Convert.ToBase64String(ctrPhoto.x) : null;
        }


        /// <summary>
        /// 入庫檢貨轉歷史檢貨
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public int PickToPickHT(long CTR_HEADER_ID)
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
            return Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));
        }

        /// <summary>
        /// 入庫檢貨刪除
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public int PickTDelete(long CTR_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
            @"DELETE CTR_PICKED_T WHERE CTR_HEADER_ID = @CTR_HEADER_ID");
            return Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));
        }

        /// <summary>
        /// 存檔入庫明細轉歷史明細
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public int DetailToDetailHT(long CTR_HEADER_ID)
        {

            StringBuilder query = new StringBuilder();
            query.Append(
            @"INSERT INTO [CTR_DETAIL_HT]
			([CTR_DETAIL_ID],[CTR_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
			[BATCH_ID],[BATCH_LINE_ID],[HEADER_ID],[LINE_ID],[DETAIL_ID],
			[LOCATOR_ID],[LOCATOR_CODE],[INVENTORY_ITEM_ID],[SHIP_ITEM_NUMBER],[PAPER_TYPE],
			[BASIC_WEIGHT],[REAM_WEIGHT],[ROLL_REAM_QTY] ,[ROLL_REAM_WT],[TTL_ROLL_REAM],
			[SPECIFICATION],[PACKING_TYPE],[SHIP_MT_QTY],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
			[PRIMARY_QUANTITY] ,[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
			[ITEM_CATEGORY],[ATTRIBUTE1],[ATTRIBUTE2],[ATTRIBUTE3],
			[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],[ATTRIBUTE7],[ATTRIBUTE8],
			[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],[ATTRIBUTE12],[ATTRIBUTE13] ,[ATTRIBUTE14],
		    [ATTRIBUTE15],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],
			[LAST_UPDATE_BY],[LAST_UPDATE_USER_NAME] ,[LAST_UPDATE_DATE])
SELECT [CTR_DETAIL_ID],[CTR_HEADER_ID],[PROCESS_CODE],[SERVER_CODE],
			[BATCH_ID],[BATCH_LINE_ID],[HEADER_ID],[LINE_ID],[DETAIL_ID],
			[LOCATOR_ID],[LOCATOR_CODE],[INVENTORY_ITEM_ID],[SHIP_ITEM_NUMBER],[PAPER_TYPE],
			[BASIC_WEIGHT],[REAM_WEIGHT],[ROLL_REAM_QTY] ,[ROLL_REAM_WT],[TTL_ROLL_REAM],
			[SPECIFICATION],[PACKING_TYPE],[SHIP_MT_QTY],[TRANSACTION_QUANTITY],[TRANSACTION_UOM],
			[PRIMARY_QUANTITY] ,[PRIMARY_UOM],[SECONDARY_QUANTITY],[SECONDARY_UOM],
			[ITEM_CATEGORY],[ATTRIBUTE1],[ATTRIBUTE2],[ATTRIBUTE3],
			[ATTRIBUTE4],[ATTRIBUTE5],[ATTRIBUTE6],[ATTRIBUTE7],[ATTRIBUTE8],
			[ATTRIBUTE9],[ATTRIBUTE10],[ATTRIBUTE11],[ATTRIBUTE12],[ATTRIBUTE13] ,[ATTRIBUTE14],
		    [ATTRIBUTE15],[CREATED_BY],[CREATED_USER_NAME],[CREATION_DATE],
			[LAST_UPDATE_BY],[LAST_UPDATE_USER_NAME] ,[LAST_UPDATE_DATE]
FROM CTR_DETAIL_T D
where D.CTR_HEADER_ID = @CTR_HEADER_ID
delete CTR_DETAIL_T where CTR_HEADER_ID = @CTR_HEADER_ID");
            return Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));


        }

        /// <summary>
        /// 轉入庫存
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public int ConvertStock(long CTR_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
            @"
INSERT INTO [STOCK_T]
([ORGANIZATION_ID],[ORGANIZATION_CODE] ,[SUBINVENTORY_CODE] ,[LOCATOR_ID],[LOCATOR_SEGMENTS]
,[INVENTORY_ITEM_ID] ,[ITEM_NUMBER] ,[ITEM_DESCRIPTION] ,[ITEM_CATEGORY] ,[PAPER_TYPE]
,[BASIC_WEIGHT],[REAM_WEIGHT],[ROLL_REAM_WT],[SPECIFICATION] ,[PACKING_TYPE]
,[OSP_BATCH_NO] ,[LOT_NUMBER] ,[BARCODE],[PRIMARY_UOM_CODE],[PRIMARY_TRANSACTION_QTY]
,[PRIMARY_AVAILABLE_QTY],[PRIMARY_LOCKED_QTY],[SECONDARY_UOM_CODE],[SECONDARY_TRANSACTION_QTY],[SECONDARY_AVAILABLE_QTY]
,[SECONDARY_LOCKED_QTY],[REASON_CODE],[REASON_DESC] ,[NOTE],[STATUS_CODE]
,[CREATED_BY],[CREATION_DATE] ,[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT 
H.ORGANIZATION_ID,H.ORGANIZATION_CODE,H.SUBINVENTORY,P.LOCATOR_ID,P.LOCATOR_CODE,
P.INVENTORY_ITEM_ID,P.SHIP_ITEM_NUMBER,IT.ITEM_DESC_TCH,P.ITEM_CATEGORY,P.PAPER_TYPE,
P.BASIC_WEIGHT,ISNULL(P.REAM_WEIGHT, '') ,P.ROLL_REAM_WT,P.SPECIFICATION,P.PACKING_TYPE,
'',P.LOT_NUMBER,P.BARCODE,P.PRIMARY_UOM,P.PRIMARY_QUANTITY,
P.PRIMARY_QUANTITY,0,P.SECONDARY_UOM,P.SECONDARY_QUANTITY,P.SECONDARY_QUANTITY,
0,P.REASON_CODE,P.REASON_DESC,P.NOTE,@STATUS_CODE,
P.CREATED_BY,GETDATE(),null,null
FROM CTR_PICKED_T P
left join CTR_HEADER_T H on H.CTR_HEADER_ID = P.CTR_HEADER_ID
left join CTR_DETAIL_T D on D.CTR_DETAIL_ID = P.CTR_DETAIL_ID
left join ITEMS_T IT ON IT.INVENTORY_ITEM_ID = P.INVENTORY_ITEM_ID
where P.CTR_HEADER_ID = @CTR_HEADER_ID
UPDATE P
SET P.STOCK_ID = S.STOCK_ID
FROM CTR_PICKED_T P
JOIN STOCK_T S ON P.BARCODE = S.BARCODE
JOIN CTR_DETAIL_T D on D.CTR_DETAIL_ID = P.CTR_DETAIL_ID
JOIN CTR_HEADER_T H on H.CTR_HEADER_ID = P.CTR_HEADER_ID
WHERE P.CTR_HEADER_ID = @CTR_HEADER_ID
");
            return Context.Database.ExecuteSqlCommand(query.ToString()
                , new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID)
                , new SqlParameter("@STATUS_CODE", StockStatusCode.InStock));
        }


        /// <summary>
        /// 庫存異動紀錄
        /// </summary>
        /// <param name="CTR_HEADER_ID"></param>
        public int StockRecord(long CTR_HEADER_ID)
        {
            StringBuilder query = new StringBuilder();
            query.Append(
@"INSERT INTO [dbo].[STK_TXN_T]
           ([STOCK_ID],[ORGANIZATION_ID] ,[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
           ,[LOCATOR_ID],[DST_ORGANIZATION_ID]  ,[DST_ORGANIZATION_CODE],[DST_SUBINVENTORY_CODE] ,[DST_LOCATOR_ID]
           ,[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION] ,[ITEM_CATEGORY] ,[LOT_NUMBER]
           ,[BARCODE],[PRY_UOM_CODE] ,[PRY_BEF_QTY],[PRY_AFT_QTY],[PRY_CHG_QTY]
           ,[SEC_UOM_CODE] ,[SEC_BEF_QTY],[SEC_CHG_QTY],[SEC_AFT_QTY],[CATEGORY]
           ,[DOC],[ACTION],[NOTE],[STATUS_CODE],[CREATED_BY]
           ,[CREATION_DATE],[LAST_UPDATE_BY],[LAST_UPDATE_DATE])
SELECT T.[STOCK_ID],T.[ORGANIZATION_ID] ,T.[ORGANIZATION_CODE],[SUBINVENTORY_CODE]
           ,T.[LOCATOR_ID],''  ,'','' ,''
           ,T.[INVENTORY_ITEM_ID],[ITEM_NUMBER],[ITEM_DESCRIPTION] ,T.[ITEM_CATEGORY] ,T.[LOT_NUMBER]
           ,T.[BARCODE],PRIMARY_UOM_CODE ,PRIMARY_TRANSACTION_QTY,null,null
           ,SECONDARY_UOM_CODE ,SECONDARY_TRANSACTION_QTY,null,null,N'入庫'
           ,H.CONTAINER_NO,N'入庫',T.[NOTE],[STATUS_CODE],T.[CREATED_BY]
           ,GETDATE(),T.[LAST_UPDATE_BY],GETDATE()
FROM STOCK_T T
JOIN CTR_PICKED_T P ON P.STOCK_ID = T.STOCK_ID
JOIN CTR_HEADER_T H on H.CTR_HEADER_ID = P.CTR_HEADER_ID
JOIN CTR_DETAIL_T D on D.CTR_HEADER_ID = H.CTR_HEADER_ID
WHERE D.CTR_HEADER_ID = @CTR_HEADER_ID");
            return Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));
        }

        /// <summary>
        /// 列印平版標籤
        /// </summary>
        /// <param name="PICKED_IDs"></param>
        /// <param name="userName"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GetFlatLabels(List<long> PICKED_IDs, string userName, string Status)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (PICKED_IDs == null || PICKED_IDs.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < PICKED_IDs.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder();
                    cmd.AppendLine(
@"UPDATE CTR_PICKED_T SET STATUS =@TO_STATUS 
WHERE STATUS=@FROM_STATUS AND CTR_PICKED_ID = @CTR_PICKED_ID");

                    string picked =
@"SELECT 
CAST(PT.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(PT.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(PT.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(PT.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(PT.ROLL_REAM_WT,'0.##########') AS nvarchar) AS Qty,
CAST(PT.SECONDARY_UOM AS nvarchar) AS Unit
--CAST(CT.CONTAINER_NO AS nvarchar) AS BatchNo
FROM [CTR_PICKED_T] PT
join CTR_HEADER_T CT ON CT.CTR_HEADER_ID = PT.CTR_HEADER_ID
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = PT.INVENTORY_ITEM_ID
WHERE PT.ITEM_CATEGORY = N'平版'
AND pt.CTR_PICKED_ID = @CTR_PICKED_ID";

                    cmd.AppendLine(picked);
                    cmd.AppendLine(picked.Replace("CTR_PICKED_T", "CTR_PICKED_HT"));


                    string commandText = cmd.ToString();
                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@CTR_PICKED_ID", PICKED_IDs[i]));
                    sqlParameterList.Add(new SqlParameter("@TO_STATUS", PickingStatusCode.PENDING));
                    sqlParameterList.Add(new SqlParameter("@FROM_STATUS", PickingStatusCode.NOT_PRINTED));

                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(commandText, sqlParameterList.ToArray()).SingleOrDefault();
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
        /// 列印紙捲標籤
        /// </summary>
        /// <param name="PICKED_IDs"></param>
        /// <param name="userName"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GetPaperRollLabels(List<long> PICKED_IDs, string userName, string Status)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (PICKED_IDs == null || PICKED_IDs.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < PICKED_IDs.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder();
                    cmd.AppendLine(@"UPDATE CTR_PICKED_T SET STATUS =@TO_STATUS 
WHERE STATUS=@FROM_STATUS AND CTR_PICKED_ID = @CTR_PICKED_ID");
                    string picked = @"
SELECT 
CAST(PT.BARCODE AS nvarchar) AS Barocde,
@userName as PrintBy,
CAST(tt.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(PT.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(PT.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(PT.SPECIFICATION AS nvarchar) AS Specification,
CAST(FORMAT(PT.PRIMARY_QUANTITY,'0.##########') AS nvarchar) AS Qty,
CAST(PT.PRIMARY_UOM AS nvarchar) AS Unit
--CAST(CT.CONTAINER_NO AS nvarchar) AS BatchNo
FROM [CTR_PICKED_T] PT
join CTR_HEADER_T CT ON CT.CTR_HEADER_ID = PT.CTR_HEADER_ID
join ITEMS_T tt on tt.INVENTORY_ITEM_ID = PT.INVENTORY_ITEM_ID
WHERE PT.ITEM_CATEGORY = N'捲筒'
AND pt.CTR_PICKED_ID = @CTR_PICKED_ID
";
                    cmd.AppendLine(picked);
                    cmd.AppendLine(picked.Replace("CTR_PICKED_T", "CTR_PICKED_HT"));


                    string commandText = cmd.ToString();
                    sqlParameterList.Add(new SqlParameter("@userName", userName));
                    sqlParameterList.Add(new SqlParameter("@CTR_PICKED_ID", PICKED_IDs[i]));
                    sqlParameterList.Add(new SqlParameter("@TO_STATUS", PickingStatusCode.PENDING));
                    sqlParameterList.Add(new SqlParameter("@FROM_STATUS", PickingStatusCode.NOT_PRINTED));


                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(commandText, sqlParameterList.ToArray()).SingleOrDefault();
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
        /// 編輯取得儲位
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PickId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocators(long PickId)
        {
            try
            {
                List<SelectListItem> Locatorlist = new List<SelectListItem>();
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                StringBuilder query = new StringBuilder();
                query.Append(
@"
SELECT
CAST (lt.[LOCATOR_ID] AS VARCHAR)AS Value,
lt.[SEGMENT2] + '.' + lt.[SEGMENT3] AS Text
FROM [LOCATOR_T] lt
JOIN SUBINVENTORY_T st ON st.ORGANIZATION_ID = lt.ORGANIZATION_ID AND st.SUBINVENTORY_CODE = lt.SUBINVENTORY_CODE
JOIN CTR_HEADER_T h ON h.SUBINVENTORY = st.SUBINVENTORY_CODE AND h.ORGANIZATION_ID = st.ORGANIZATION_ID
JOIN CTR_PICKED_T pt ON pt.CTR_HEADER_ID = h.CTR_HEADER_ID
WHERE pt.CTR_PICKED_ID = @CTR_PICKED_ID
AND lt.CONTROL_FLAG <> 'D'
AND st.LOCATOR_TYPE = '2'
AND (lt.LOCATOR_DISABLE_DATE >= GETDATE() OR lt.LOCATOR_DISABLE_DATE is null)
");
                sqlParameterList.Add(new SqlParameter("@CTR_PICKED_ID", PickId));
                string commandText = query.ToString();
                if (sqlParameterList.Count > 0)
                {
                    Locatorlist.AddRange(Context.Database.SqlQuery<SelectListItem>(commandText, sqlParameterList.ToArray()).ToList());
                }
                else
                {
                    Locatorlist.AddRange(Context.Database.SqlQuery<SelectListItem>(commandText).ToList());
                }

                return Locatorlist;

            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// 設定原因
        /// </summary>
        /// <param name="PickId"></param>
        /// <returns></returns>
        public ResultDataModel<CTR_PICKED_T> SetSpinnerValue(long PickId)
        {
            try
            {
                var pick = ctrPickedTRepository.Get(x => x.CtrPickedId == PickId).SingleOrDefault();
                return new ResultDataModel<CTR_PICKED_T>(true, "成功", pick);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new ResultDataModel<CTR_PICKED_T>(false, "取得原因失敗:" + e.Message, null);
            }
        }

        public class PurchaseStatusCode : IStatus
        {
            /// <summary>
            /// 已入庫
            /// </summary>
            public const string Already = "0";

            /// <summary>
            /// 待入庫
            /// </summary>
            public const string Pending = "1";

            /// <summary>
            /// 取消
            /// </summary>
            public const string Cancel = "2";


            public static long GetCode(string statusCode)
            {
                return long.Parse(statusCode);
            }

            public string GetDesc(string statusCode)
            {
                var desc = "";
                switch (statusCode)
                {
                    case Already:
                        desc = "已入庫";
                        break;
                    case Cancel:
                        desc = "取消";
                        break;
                    case Pending:
                        desc = "待入庫";
                        break;
                }
                return desc;
            }
        }


        public class PickingStatusCode : IStatus
        {
            public const string ALREADY = "已入庫";

            public const string PENDING = "待入庫";

            public const string NOT_PRINTED = "未印";

            public string GetDesc(string statusCode)
            {
                return statusCode;

            }
        }
    }


}