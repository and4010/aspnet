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
using System.Web.Configuration;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.DataModel.Entity.Transfer;
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
        private readonly IRepository<TRF_REASON_HEADER_T> trfReasonHeaderTRepository;
        private readonly IRepository<TRF_REASON_T> trfReasonTRepository;



        public PurchaseUOW(DbContext context) : base(context)
        {
            this.ctrOrgTRepository = new GenericRepository<CTR_ORG_T>(this);
            this.ctrHeaderTRepository = new GenericRepository<CTR_HEADER_T>(this);
            this.ctrDetailTRepository = new GenericRepository<CTR_DETAIL_T>(this);
            this.ctrPickedTRepository = new GenericRepository<CTR_PICKED_T>(this);
            this.ctrPickedHtRepository = new GenericRepository<CTR_PICKED_HT>(this);
            this.ctrFileInfoTRepository = new GenericRepository<CTR_FILEINFO_T>(this);
            this.ctrFilesTRepository = new GenericRepository<CTR_FILES_T>(this);
            this.trfReasonHeaderTRepository = new GenericRepository<TRF_REASON_HEADER_T>(this);
            this.trfReasonTRepository = new GenericRepository<TRF_REASON_T>(this);
        }


        /// <summary>
        /// 取得表頭資料
        /// </summary>
        /// <param name="CtrHeaderId"></param>
        /// <returns></returns>
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
                    if (!barcode.Success) throw new Exception(barcode.Msg);
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

        /// <summary>
        /// 重量換算
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="pryQty"></param>
        /// <param name="pryUom"></param>
        /// <param name="txnUom"></param>
        /// <returns></returns>
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
        /// 取得行事曆資料
        /// </summary>
        /// <returns></returns>
        public List<FullCalendarEventModel> getFullCalenderList(string Subinventory, string status)
        {
            var header = new List<CTR_HEADER_T>();
            if (status == "*")
            {
                header = ctrHeaderTRepository.Get(x => x.Subinventory == Subinventory).GroupBy(x => x.ContainerNo).Select(x => x.FirstOrDefault()).ToList();
            }
            else
            {
                long status1 = Int64.Parse(status);
                header = ctrHeaderTRepository.Get(x => x.Subinventory == Subinventory && x.Status == status1).GroupBy(x => x.ContainerNo).Select(x => x.FirstOrDefault()).ToList();
            }

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
        /// 日歷搜尋櫃號
        /// </summary>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public ResultModel SearchCabinetNumber(string CabinetNumber)
        {
            var header = ctrHeaderTRepository.Get(x => x.ContainerNo == CabinetNumber).SingleOrDefault();
            if(header == null)
            {
                return new ResultModel(false, "無此櫃號");
            }
            if (header.Status == 2)
            {
                return new ResultModel(false, "此櫃號已取消");
            }
            else
            {
                return new ResultModel(true, header.CtrHeaderId.ToString());
            }
        }

        /// <summary>
        /// 存檔入庫header轉狀態
        /// </summary>
        /// <param name="ContainerNo"></param>
        /// <returns></returns>
        public ResultModel ChangeHeaderStatus(long CtrHeaderId, string userId, string userName)
        {
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    var pick = ctrPickedTRepository.Get(x => x.CtrHeaderId == CtrHeaderId && x.Status != PickingStatusCode.ALREADY).Count();
                    if (pick > 0)
                    {
                        return new ResultModel(false, "有條碼尚未入庫");
                    }
                    var header = ctrHeaderTRepository.Get(x => x.CtrHeaderId == CtrHeaderId).FirstOrDefault();
                    if (header != null)
                    {
                        header.Status = PurchaseStatusCode.GetCode(PurchaseStatusCode.Already);
                        header.LastUpdateBy = userId;
                        header.LastUpdateDate = now;
                        header.LastUpdateUserName = userName;
                        ctrHeaderTRepository.Update(header, true);
                        ConvertStock(header.CtrHeaderId);
                        StockRecord(header.CtrHeaderId);
                        SaveTrfReason(header.CtrHeaderId, now);
                        PickToPickHT(header.CtrHeaderId);
                        PickTDelete(header.CtrHeaderId);
                        DetailToDetailHT(header.CtrHeaderId);
                        txn.Commit();
                        return new ResultModel(true, "成功"); ;
                    }
                }
                catch (Exception e)
                {

                    logger.Error(e.Message);
                    return new ResultModel(false, e.Message.ToString());
                }
                txn.Rollback();
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
        public ResultModel PaperRollEdit(HttpFileCollectionBase Files, long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var ctrPickT = ctrPickedTRepository.Get(x => x.CtrPickedId == id).SingleOrDefault();

                    if (ctrPickT == null)
                    {
                        throw new Exception("揀貨資料不存在!!");
                    }

                    if (Files != null || Files.Count > 0)
                    {
                        //檢查檔案是否已上傳
                        foreach (string i in Files)
                        {
                            HttpPostedFileBase file = Files[i];
                            string fileNmae = Path.GetFileName(file.FileName);
                            var fileInfo = ctrFileInfoTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.CtrPickedId == id && x.FileName == fileNmae);
                            if (fileInfo != null) return new ResultModel(false, fileNmae + "此檔案已上傳");
                        }

                        foreach (string i in Files)
                        {
                            HttpPostedFileBase file = Files[i];
                            string fileNmae = Path.GetFileName(file.FileName);
                            SaveCtrFileInfoT(VaryQualityLevel(file), fileNmae, file.ContentType, ctrPickT.CtrPickedId, LastUpdateBy);
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
                    //檢查檔案是否已上傳
                    foreach (string i in Files)
                    {
                        HttpPostedFileBase file = Files[i];
                        string fileNmae = Path.GetFileName(file.FileName);
                        var fileInfo = ctrFileInfoTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.CtrPickedId == id && x.FileName == fileNmae);
                        if (fileInfo != null) return new ResultModel(false, fileNmae + "此檔案已上傳");
                    }

                    foreach (string i in Files)
                    {
                        HttpPostedFileBase file = Files[i];
                        string fileNmae = Path.GetFileName(file.FileName);
                        SaveCtrFileInfoT(VaryQualityLevel(file), fileNmae, file.ContentType, ctrPickT.CtrPickedId, LastUpdateBy);
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
                        return new ResultModel(false, "無此條碼!!");
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
                        return new ResultModel(false, "無此條碼!!");
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
        /// 取得頁籤未入庫平版數量
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public decimal GetFlatNumberTab(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                $@"SELECT (CASE h.STATUS
            WHEN  0
            THEN (SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM CTR_PICKED_HT p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平版' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
FROM CTR_DETAIL_HT d1
JOIN CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'平版' and h1.CTR_HEADER_ID  = @CTR_HEADER_ID)
            ELSE (SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平版' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
FROM CTR_DETAIL_T d1
JOIN CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'平版' and h1.CTR_HEADER_ID  = @CTR_HEADER_ID)
        END)
FROM CTR_HEADER_T h where h.CTR_HEADER_ID = @CTR_HEADER_ID");
                return Context.Database.SqlQuery<decimal>(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).SingleOrDefault();


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 取得頁籤已入庫平版數量
        /// </summary>
        /// <param name="CtrHeaderId"></param>
        /// <returns></returns>
        public Int32 GetFlatNumberInTab(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
$@"SELECT (CASE h.STATUS
            WHEN  0
            THEN (SELECT COUNT(p.CTR_PICKED_ID)
FROM CTR_PICKED_HT p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平版' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
            ELSE (SELECT COUNT(p.CTR_PICKED_ID)
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'平版' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
        END)
FROM CTR_HEADER_T h where h.CTR_HEADER_ID = @CTR_HEADER_ID");
                return Context.Database.SqlQuery<Int32>(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).SingleOrDefault();


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 取得頁籤未入庫紙捲數量
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public decimal GetPaperRollNumberTab(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
                $@"SELECT (CASE h.STATUS
            WHEN  0
            THEN (SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM CTR_PICKED_HT p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
FROM CTR_DETAIL_HT d1
JOIN CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'捲筒' and h1.CTR_HEADER_ID  = @CTR_HEADER_ID)
            ELSE (SELECT 
(SELECT 
sum(d1.ROLL_REAM_QTY)- 
count(p.CTR_PICKED_ID)
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
FROM CTR_DETAIL_T d1
JOIN CTR_HEADER_T h1 ON h1.CTR_HEADER_ID = d1.CTR_HEADER_ID
WHERE d1.ITEM_CATEGORY = N'捲筒' and h1.CTR_HEADER_ID  = @CTR_HEADER_ID)
        END)
FROM CTR_HEADER_T h where h.CTR_HEADER_ID = @CTR_HEADER_ID
");
                return Context.Database.SqlQuery<decimal>(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).SingleOrDefault();


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 取得頁籤已入庫紙捲數量
        /// </summary>
        /// <param name="CtrHeaderId"></param>
        /// <returns></returns>
        public Int32 GetPaperRollNumberInTab(long CtrHeaderId)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(
$@"SELECT (CASE h.STATUS
            WHEN  0
            THEN (SELECT COUNT(p.CTR_PICKED_ID)
FROM CTR_PICKED_HT p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
            ELSE (SELECT COUNT(p.CTR_PICKED_ID)
FROM CTR_PICKED_T p
JOIN CTR_HEADER_T h2 ON h2.CTR_HEADER_ID = p.CTR_HEADER_ID
WHERE p.ITEM_CATEGORY = N'捲筒' and h2.CTR_HEADER_ID  = @CTR_HEADER_ID and p.STATUS = N'{PickingStatusCode.ALREADY}')
        END)
FROM CTR_HEADER_T h where h.CTR_HEADER_ID = @CTR_HEADER_ID");
                return Context.Database.SqlQuery<Int32>(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CtrHeaderId)).SingleOrDefault();


            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return 0;
            }
        }


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
        /// 照片轉換base64
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

        /// <summary>
        /// 取得所有照片ID
        /// </summary>
        /// <param name="pickedId"></param>
        /// <returns></returns>
        public List<long> GetPhotoList(long pickedId)
        {
            return ctrFileInfoTRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.CtrPickedId == pickedId)
                .Select(x => x.CtrFileinfoId)
                .ToList();
        }

        /// <summary>
        /// 取得照片檔案
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
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
,[CREATED_BY],[CREATION_DATE] ,[LAST_UPDATE_BY],[LAST_UPDATE_DATE],[CONTAINER_NO])
SELECT 
H.ORGANIZATION_ID,H.ORGANIZATION_CODE,H.SUBINVENTORY,P.LOCATOR_ID,P.LOCATOR_CODE,
P.INVENTORY_ITEM_ID,P.SHIP_ITEM_NUMBER,IT.ITEM_DESC_TCH,P.ITEM_CATEGORY,P.PAPER_TYPE,
P.BASIC_WEIGHT,ISNULL(P.REAM_WEIGHT, '') ,P.ROLL_REAM_WT,P.SPECIFICATION,P.PACKING_TYPE,
'',P.LOT_NUMBER,P.BARCODE,P.PRIMARY_UOM,P.PRIMARY_QUANTITY,
P.PRIMARY_QUANTITY,0,P.SECONDARY_UOM,P.SECONDARY_QUANTITY,P.SECONDARY_QUANTITY,
0,P.REASON_CODE,P.REASON_DESC,P.NOTE,@STATUS_CODE,
P.CREATED_BY,GETDATE(),null,null,H.CONTAINER_NO
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
           ,T.[BARCODE],PRIMARY_UOM_CODE ,0,PRIMARY_TRANSACTION_QTY,PRIMARY_TRANSACTION_QTY
           ,SECONDARY_UOM_CODE ,0,SECONDARY_TRANSACTION_QTY,SECONDARY_TRANSACTION_QTY,N'入庫'
           ,H.CONTAINER_NO,N'入庫',T.[NOTE],[STATUS_CODE],T.[CREATED_BY]
           ,GETDATE(),NULL,NULL
FROM STOCK_T T
JOIN CTR_PICKED_T P ON P.STOCK_ID = T.STOCK_ID
JOIN CTR_HEADER_T H on H.CTR_HEADER_ID = P.CTR_HEADER_ID
--JOIN CTR_DETAIL_T D on D.CTR_HEADER_ID = H.CTR_HEADER_ID
WHERE P.CTR_HEADER_ID = @CTR_HEADER_ID");
            return Context.Database.ExecuteSqlCommand(query.ToString(), new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID));
        }

        /// <summary>
        /// 列印平版標籤
        /// </summary>
        /// <param name="PICKED_IDs"></param>
        /// <param name="userName"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GetFlatLabels(List<long> PICKED_IDs, string userId, string userName, string Status)
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
@"UPDATE CTR_PICKED_T SET STATUS =@TO_STATUS, LAST_UPDATE_BY =@userId, LAST_UPDATE_USER_NAME =@userName, LAST_UPDATE_DATE =@now
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
CAST(PT.SECONDARY_UOM AS nvarchar) AS Unit,
CAST(ISNULL((SELECT SubId FROM
(SELECT ROW_NUMBER() OVER(ORDER BY p.CTR_DETAIL_ID ) AS SubId,
	CTR_PICKED_ID 
	FROM CTR_PICKED_T P
	JOIN CTR_HEADER_T H ON H.CTR_HEADER_ID = P.CTR_HEADER_ID
	JOIN CTR_DETAIL_T D ON D.CTR_DETAIL_ID = P.CTR_DETAIL_ID
	WHERE H.CTR_HEADER_ID = PT.CTR_HEADER_ID
	and p.ITEM_CATEGORY = N'平版')
AS TEMP WHERE CTR_PICKED_ID = @CTR_PICKED_ID
), 0) AS nvarchar)AS SubId
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
                    sqlParameterList.Add(SqlParamHelper.GetDataTime("@now", DateTime.Now));
                    sqlParameterList.Add(SqlParamHelper.GetNVarChar("@userId", userId));

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
        public ResultDataModel<List<LabelModel>> GetPaperRollLabels(List<long> PICKED_IDs, string userId, string userName, string Status)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (PICKED_IDs == null || PICKED_IDs.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                for (int i = 0; i < PICKED_IDs.Count; i++)
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    StringBuilder cmd = new StringBuilder();
                    cmd.AppendLine(@"UPDATE CTR_PICKED_T SET STATUS =@TO_STATUS, LAST_UPDATE_BY =@userId, LAST_UPDATE_USER_NAME =@userName, LAST_UPDATE_DATE =@now 
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
CAST(PT.PRIMARY_UOM AS nvarchar) AS Unit,
CAST(PT.LOT_NUMBER AS nvarchar) as LotNumber,
CAST(ISNULL((SELECT SubId FROM
(SELECT ROW_NUMBER() OVER(ORDER BY p.CTR_DETAIL_ID ) AS SubId,
	CTR_PICKED_ID 
	FROM CTR_PICKED_T P
	JOIN CTR_HEADER_T H ON H.CTR_HEADER_ID = P.CTR_HEADER_ID
	JOIN CTR_DETAIL_T D ON D.CTR_DETAIL_ID = P.CTR_DETAIL_ID
	WHERE H.CTR_HEADER_ID = PT.CTR_HEADER_ID
	and p.ITEM_CATEGORY = N'捲筒')
AS TEMP WHERE CTR_PICKED_ID = @CTR_PICKED_ID
), 0) AS nvarchar)AS SubId
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
                    sqlParameterList.Add(SqlParamHelper.GetDataTime("@now", DateTime.Now));
                    sqlParameterList.Add(SqlParamHelper.GetNVarChar("@userId", userId));


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
        /// 取的日曆下拉狀態
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetStatus()
        {
            List<SelectListItem> Locatorlist = new List<SelectListItem>();
            Locatorlist.Add(new SelectListItem
            {
                Text = "待入庫",
                Value = "1",
            });
            Locatorlist.Add(new SelectListItem
            {
                Text = "已入庫",
                Value = "0",
            });
            Locatorlist.Add(new SelectListItem
            {
                Text = "取消",
                Value = "2",
            });
            Locatorlist.Insert(0, new SelectListItem { Text = "全部", Value = "*" });
            return Locatorlist;
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

        /// <summary>
        /// 入庫單
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="Detail"></param>
        /// <param name="Reason"></param>
        /// <param name="CtrHeaderId"></param>
        /// <param name="ItemCategory"></param>
        public void PurchaseReceipt(ref ReportDataSource Header, ref ReportDataSource Detail, ref ReportDataSource Reason, string CtrHeaderId, string ItemCategory)
        {

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    if (!long.TryParse(CtrHeaderId, out long ctrHeaderId))
                    {
                        throw new ArgumentException("HEADER ID ERROR");
                    }

                    DataSet dataset = new DataSet("Flat");
                    GetHeaderReceipt(connection, ref dataset, ctrHeaderId);
                    Header.Name = "Header";
                    Header.Value = dataset.Tables["Header"];


                    GetDetailReceipt(connection, ref dataset, ctrHeaderId, ItemCategory);
                    Detail.Name = "Detail";
                    Detail.Value = dataset.Tables["Detail"];


                    GetReasonReceipt(connection, ref dataset);
                    Reason.Name = "Reason";
                    Reason.Value = dataset.Tables["Reason"];
                    connection.Close();
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                }
            }

        }

        /// <summary>
        /// 取得入庫單表頭資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dsSalesOrder"></param>
        /// <param name="CTR_HEADER_ID"></param>
        public void GetHeaderReceipt(SqlConnection connection, ref DataSet dsSalesOrder, long CTR_HEADER_ID)
        {
            string Header = "SELECT * FROM dbo.PurchaseHeadaer(@CTR_HEADER_ID)";
            SqlCommand command = new SqlCommand(Header, connection);
            command.Parameters.Add(new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID) { DbType = DbType.Int64 });
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(dsSalesOrder, "Header");
        }

        /// <summary>
        /// 取得報表表身資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Detail"></param>
        /// <param name="CTR_HEADER_ID"></param>
        /// <param name="ITEM_CATEGORY"></param>
        public void GetDetailReceipt(SqlConnection connection, ref DataSet Detail, long CTR_HEADER_ID, string ITEM_CATEGORY)
        {
            string Detail1 = "SELECT * FROM dbo.PurchaseDetail(@CTR_HEADER_ID,@ITEM_CATEGORY)";
            SqlCommand command = new SqlCommand(Detail1, connection);
            command.Parameters.Add(new SqlParameter("@CTR_HEADER_ID", CTR_HEADER_ID) { DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter("@ITEM_CATEGORY", ITEM_CATEGORY));
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(Detail, "Detail");
        }

        /// <summary>
        /// 取得入庫單原因資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Reason"></param>
        public void GetReasonReceipt(SqlConnection connection, ref DataSet Reason)
        {
            string reason = "SELECT * FROM dbo.PurchaseReason()";
            SqlCommand command = new SqlCommand(reason, connection);
            SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
            salesOrderAdapter.Fill(Reason, "Reason");

        }
        /// <summary>
        /// 貨故轉移
        /// </summary>
        /// <param name="ctrHeaderId"></param>
        /// <param name="now"></param>
        public void SaveTrfReason(long ctrHeaderId, DateTime now)
        {

            var pickList = ctrPickedTRepository.GetAll().AsNoTracking().Where(x => x.CtrHeaderId == ctrHeaderId && !string.IsNullOrEmpty(x.ReasonCode)).ToList();
            if (pickList == null || pickList.Count == 0) return;

            foreach (var pick in pickList)
            {
                var trfLocator = locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == pick.LocatorId);
                if (trfLocator == null) throw new Exception("找不到目標儲位資料");

                var ctrDetail = ctrDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.CtrDetailId == pick.CtrDetailId);
                if (ctrDetail == null) throw new Exception("找不到明細資料");

                var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.StockId);
                if (stock == null) throw new Exception("找不到庫存資料");

                var locator = locatorTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.LocatorId == ctrDetail.LocatorId);
                if (locator == null) throw new Exception("找不到原本儲位資料");

                var organization = GetOrganization(stock.OrganizationId);
                if (organization == null) throw new Exception("找不到組織資料");

                var trfOrganization = GetOrganization(trfLocator.OrganizationId);
                if (trfOrganization == null) throw new Exception("找不到目標組織資料");

                var reason = stkReasonTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.ReasonCode == pick.ReasonCode);
                if (reason == null) throw new Exception("找不到貨故資料");

                long transactionTypeId;
                var transferCatalog = GetTransferCatalog(stock.OrganizationId, trfLocator.OrganizationId);
                if (transferCatalog == TransferCatalog.OrgTransfer)
                {
                    throw new Exception("貨故不可為組織間移轉");
                }
                else
                {
                    transactionTypeId = TransferUOW.TransactionTypeId.Chp30;
                }

                var transactionType = GetTransactionType(transactionTypeId);
                if (trfOrganization == null) throw new Exception("找不到庫存交易類別資料");


                TRF_REASON_HEADER_T header = new TRF_REASON_HEADER_T
                {
                    OrgId = organization.OrgUnitId,
                    OrganizationId = stock.OrganizationId,
                    OrganizationCode = stock.OrganizationCode,
                    ShipmentNumber = GetShipmentNumberGuid(),
                    SubinventoryCode = stock.SubinventoryCode,
                    LocatorId = locator.LocatorId,
                    LocatorCode = locator.LocatorSegments,
                    Segment3 = locator.Segment3,
                    TransactionDate = now,
                    TransactionTypeId = transactionTypeId,
                    TransactionTypeName = transactionType.TransactionTypeName,
                    TransferOrgId = trfOrganization.OrgUnitId,
                    TransferOrganizationId = trfLocator.OrganizationId,
                    TransferOrganizationCode = trfOrganization.OrganizationCode,
                    TransferSubinventoryCode = trfLocator.SubinventoryCode,
                    TransferLocatorId = trfLocator.LocatorId,
                    TransferLocatorCode = trfLocator.LocatorSegments,
                    NumberStatus = NumberStatus.Saved,
                    ToErp = ToErp.Yes,
                    CreatedBy = pick.CreatedBy,
                    CreatedUserName = pick.CreatedUserName,
                    CreationDate = pick.CreationDate,
                    CtrHeaderId = ctrHeaderId,
                    LastUpdateBy = null,
                    LastUpdateUserName = null,
                    LastUpdateDate = null
                };
                trfReasonHeaderTRepository.Create(header, true);

                TRF_REASON_T detail = new TRF_REASON_T
                {
                    TransferReasonHeaderId = header.TransferReasonHeaderId,
                    InventoryItemId = stock.InventoryItemId,
                    ItemNumber = stock.ItemNumber,
                    ItemDescription = stock.ItemDescription,
                    Barcode = stock.Barcode,
                    StockId = stock.StockId,
                    PrimaryUom = stock.PrimaryUomCode,
                    PrimaryQuantity = stock.PrimaryAvailableQty,
                    SecondaryUom = stock.SecondaryUomCode,
                    SecondaryQuantity = stock.SecondaryAvailableQty,
                    LotNumber = stock.LotNumber,
                    LotQuantity = stock.LotQuantity,
                    ReasonCode = reason.ReasonCode,
                    ReasonDesc = reason.ReasonDesc,
                    Note = pick.Note,
                    CreatedBy = pick.CreatedBy,
                    CreatedUserName = pick.CreatedUserName,
                    CreationDate = pick.CreationDate,
                    LastUpdateBy = null,
                    LastUpdateUserName = null,
                    LastUpdateDate = null
                };
                trfReasonTRepository.Create(detail, true);

                //複製貨故明細資料到貨故歷史明細
                string cmd = @"
INSERT INTO TRF_REASON_HT
(
      [TRANSFER_REASON_ID]
      ,[TRANSFER_REASON_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[BARCODE]
      ,[STOCK_ID]
      ,[PRIMARY_UOM]
      ,[PRIMARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[REASON_CODE]
      ,[REASON_DESC]
      ,[NOTE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [TRANSFER_REASON_ID]
      ,[TRANSFER_REASON_HEADER_ID]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[BARCODE]
      ,[STOCK_ID]
      ,[PRIMARY_UOM]
      ,[PRIMARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[REASON_CODE]
      ,[REASON_DESC]
      ,[NOTE]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [TRF_REASON_T]
  WHERE [TRANSFER_REASON_HEADER_ID] = @TRANSFER_REASON_HEADER_ID
";
                if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_REASON_HEADER_ID", header.TransferReasonHeaderId)) <= 0)
                {
                    throw new Exception("複製貨故明細資料到貨故歷史明細失敗");
                }

                //刪除貨故明細資料
                cmd = @"
  DELETE FROM [TRF_REASON_T]
  WHERE TRANSFER_REASON_HEADER_ID = @TRANSFER_REASON_HEADER_ID";
                if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@TRANSFER_REASON_HEADER_ID", header.TransferReasonHeaderId)) <= 0)
                {
                    throw new Exception("刪除貨故明細資料失敗");
                }
            }


        }

        /// <summary>
        /// 取得主頁未入庫數量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<int> GetCtrPendingCount(string userId)
        {
            var resultDataModel = new ResultDataModel<int>(false, "", 0);
            try
            {
                var subinventoryList = GetSubinventoryListForUser(userId);
                if (subinventoryList == null || subinventoryList.Count == 0)
                {
                    throw new Exception("找不到使用者倉庫");
                }

                var code = PurchaseStatusCode.GetCode(PurchaseStatusCode.Pending);
                resultDataModel.Data = ctrHeaderTRepository.GetAll().AsNoTracking()
                    .Where(x => x.Status == code 
                    && subinventoryList.Contains(x.Subinventory)
                    ).Count();

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

        /// <summary>
        /// 取得庫存%入庫所有卷號
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllLotNumber()
        {
            try
            {
                List<String> vs = new List<string>();
                StringBuilder query = new StringBuilder();
                query.Append(
                @"select
  LOT_NUMBER
  from CTR_PICKED_T 
  where ITEM_CATEGORY = '捲筒'");
                vs.AddRange(Context.Database.SqlQuery<String>(query.ToString()).ToList());

                StringBuilder query1 = new StringBuilder();
                query1.Append(
                @"select
  LOT_NUMBER
  from STOCK_T 
  where ITEM_CATEGORY = '捲筒'");
                vs.AddRange(Context.Database.SqlQuery<String>(query1.ToString()).ToList());

                return vs;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new List<String>();
            }
        }


    }


}