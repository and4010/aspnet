using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Delivery;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Util;
using NLog;
using CHPOUTSRCMES.Web.Models.Delivery;
using System.Text;
using System.Data.SqlClient;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.DataModel.Interfaces;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class DeliveryUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<DLV_ORG_T> dlvOrgTRepositiory;
        private readonly IRepository<DLV_HEADER_T> dlvHeaderTRepositiory;
        private readonly IRepository<DLV_DETAIL_T> dlvDetailTRepositiory;
        private readonly IRepository<DLV_DETAIL_HT> dlvDetailHtRepositiory;
        private readonly IRepository<DLV_PICKED_T> dlvPickedTRepositiory;
        private readonly IRepository<DLV_PICKED_HT> dlvPickedHtRepositiory;

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DeliveryUOW(DbContext context)
            : base(context)
        {
            this.dlvOrgTRepositiory = new GenericRepository<DLV_ORG_T>(this);
            this.dlvHeaderTRepositiory = new GenericRepository<DLV_HEADER_T>(this);
            this.dlvDetailTRepositiory = new GenericRepository<DLV_DETAIL_T>(this);
            this.dlvDetailHtRepositiory = new GenericRepository<DLV_DETAIL_HT>(this);
            this.dlvPickedTRepositiory = new GenericRepository<DLV_PICKED_T>(this);
            this.dlvPickedHtRepositiory = new GenericRepository<DLV_PICKED_HT>(this);
        }
        //private List<SelectListItem> deliveryStatusList = new List<SelectListItem>() 
        //{
        //     new SelectListItem() { Text = "取消", Value = "0" },
        //     new SelectListItem() { Text = "未印", Value = "1" },
        //     new SelectListItem() { Text = "待出", Value = "2" },
        //     new SelectListItem() { Text = "已揀", Value = "3" },
        //     new SelectListItem() { Text = "待核准", Value = "4" },
        //     new SelectListItem() { Text = "已出貨", Value = "5" }
        //};

        //public static Dictionary<string, string> DeliveryStatusDictionary = new Dictionary<string, string>()
        //{
        //    {DeliveryStatusCode.Canceled, "已取消"},
        //    {DeliveryStatusCode.Unprinted, "未印"},
        //    {DeliveryStatusCode.UnPicked, "待出"},
        //    {DeliveryStatusCode.Picked, "已揀"},
        //    {DeliveryStatusCode.UnAuthorized, "待核准"},
        //    {DeliveryStatusCode.Authorized, "已出貨"},
        //};

        public IHeader deliveryStatusCode = new DeliveryStatusCode();

        public IDetail pickSatus = new PickStatus();


        /// <summary>
        /// 新增揀貨明細
        /// </summary>
        /// <param name="dlvHeaderId"></param>
        /// <param name="dlvDetailId"></param>
        /// <param name="deliveryName"></param>
        /// <param name="barcode"></param>
        /// <param name="qty">令包數量</param>
        /// <param name="addUser"></param>
        /// <param name="addUserName"></param>
        /// <param name="status">揀貨明細狀態</param>
        /// <param name="transactionUomCode">交易單位</param>
        /// <returns></returns>
        public ResultModel AddPickDT(long dlvHeaderId, long dlvDetailId, string deliveryName, string barcode, decimal? qty, string addUser, string addUserName)
        {
            var addDate = DateTime.Now;
            //庫存檢查
            var checkResult = DeliveryCheckStock(barcode, qty * -1, addDate);
            //var checkResult = CheckStock(barcode, qty, uom);
            if (!checkResult.Success) return new ResultModel(checkResult.Success, checkResult.Msg);
            var stock = checkResult.Data;
            var detailData = dlvDetailTRepositiory.GetAll().AsNoTracking().FirstOrDefault(x => x.DlvDetailId == dlvDetailId);
            if (detailData.OspBatchId == null && detailData.TmpItemId == null)
            {
                if (stock.ItemNumber != detailData.ItemNumber)
                {
                    return new ResultModel(false, "此條碼不符合已選擇的料號");
                }
            }
            else
            {
                //代紙
                if (stock.ItemNumber != detailData.TmpItemNumber)
                {
                    return new ResultModel(false, "此條碼不符合已選擇的料號");
                }
            }
         

            var pickData = dlvPickedTRepositiory.GetAll().AsNoTracking().Where(x => x.Barcode == barcode).ToList();
            if (pickData.Count > 0) return new ResultModel(false, "條碼重複輸入");


            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var status = detailData.OspBatchId == null ? "" : "TMP"; //OspBatchId不是NULL時 為代紙(TMP)
                    string palletStatus;
                    //產生異動記錄
                    STK_TXN_T stkTxnT = CreateStockRecord(stock, null, "", "", null, CategoryCode.Delivery, ActionCode.Picked, deliveryName);

                    decimal? priQty = null;
                    decimal? secQty = null;
                    if (qty != null && qty < stock.SecondaryAvailableQty)
                    {
                        //有令包數量 且 小於庫存次單位數量 時為拆板
                        priQty = null;
                        secQty = qty * -1;
                        palletStatus = PalletStatusCode.Split;
                    }
                    else
                    {
                        //非拆板時
                        priQty = stock.PrimaryAvailableQty * -1;
                        secQty = stock.SecondaryAvailableQty * -1;
                        palletStatus = PalletStatusCode.All;
                    }

                    //更新庫存
                    
                    var updaeStockResult = UpdateStock(stock, stkTxnT, ref priQty, ref secQty, pickSatus, PickStatus.Picked, addUser, addDate, true);
                    if (!updaeStockResult.Success) return new ResultModel(updaeStockResult.Success, updaeStockResult.Msg);

                    var model = uomConversion.Convert(stock.InventoryItemId, priQty != null ? (decimal)priQty * -1 : 0, stock.PrimaryUomCode, detailData.SrcRequestedQuantityUom);

                    if (!model.Success)
                    {
                        //轉換失敗 該如何處理??
                    }
                    //新增一筆PickDT
                    dlvPickedTRepositiory.Create(new DLV_PICKED_T
                    {
                        Stock_Id = stock.StockId,
                        LocatorId = stock.LocatorId,
                        LocatorCode = stock.LocatorSegments,
                        DlvHeaderId = dlvHeaderId,
                        DlvDetailId = dlvDetailId,
                        Barcode = stock.Barcode,
                        InventoryItemId = stock.InventoryItemId,
                        Item_Number = stock.ItemNumber,
                        PackingType = stock.PackingType,
                        LotQuantity = null,
                        LotNumber = stock.LotNumber,
                        ReamWeight = stock.ReamWeight,
                        PrimaryQuantity = priQty != null ? (decimal)priQty * -1 : 0,
                        PrimaryUom = stock.PrimaryUomCode,
                        SecondaryQuantity = secQty * -1,
                        SecondaryUom = stock.SecondaryUomCode,
                        TransactionQuantity = model.Data,
                        TransactionUom = detailData.SrcRequestedQuantityUom,
                        CreatedBy = addUser,
                        CreatedUserName = addUserName,
                        CreationDate = addDate,
                        LastUpdateBy = addUser,
                        LastUpdateUserName = addUserName,
                        LastUpdateDate = addDate,
                        Status = status,
                        PalletStatus = palletStatus
                    });

                    stockTRepositiory.SaveChanges();
                    stkTxnTRepositiory.SaveChanges();
                    dlvPickedTRepositiory.SaveChanges();

                    //更新Header狀態
                    var pickedResult = CheckPicked(dlvHeaderId);
                    if (!pickedResult.Success)
                    {
                        throw new Exception(pickedResult.Msg);
                    }
                    var updateDatas = GetDeliveryHeaderDataListFromHeaderId(dlvHeaderId);
                    if (updateDatas.Count == 0) throw new Exception("找不到交運單資料");
                    foreach (DLV_HEADER_T data in updateDatas)
                    {
                        data.DeliveryStatusCode = pickedResult.Msg;
                        data.DeliveryStatusName = deliveryStatusCode.GetDesc(pickedResult.Msg);
                        dlvHeaderTRepositiory.Update(data);
                        data.LastUpdateBy = addUser;
                        data.LastUpdateUserName = addUserName;
                        data.LastUpdateDate = addDate;
                    }
                    dlvHeaderTRepositiory.SaveChanges();


                    txn.Commit();
                    return new ResultModel(true, "新增揀貨明細成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "新增揀貨明細失敗:" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 檢查是否此Header是否揀畢
        /// </summary>
        /// <param name="dlvHeaderId"></param>
        /// <returns></returns>
        public ResultModel CheckPicked(long dlvHeaderId)
        {
            try
            {
                string cmd = @"
           select 
d.DLV_DETAIL_ID as ID
from DLV_DETAIL_T d
JOIN DLV_HEADER_T h ON h.DLV_HEADER_ID = d.DLV_HEADER_ID
LEFT JOIN DLV_PICKED_T p ON p.DLV_HEADER_ID = d.DLV_HEADER_ID AND p.DLV_DETAIL_ID = d.DLV_DETAIL_ID
where h.DLV_HEADER_ID = @DLV_HEADER_ID
GROUP BY d.DLV_DETAIL_ID
HAVING SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) <> MIN(d.REQUESTED_PRIMARY_QUANTITY) 
OR SUM(ISNULL(p.SECONDARY_QUANTITY, 0)) <> MIN(d.REQUESTED_SECONDARY_QUANTITY)";

                var list = this.Context.Database.SqlQuery<PaperRollEditDT>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();
                if (list.Count == 0) //為0表示應揀量等於已揀量
                {
                    return new ResultModel(true, DeliveryStatusCode.Picked);
                }
                else
                {
                    return new ResultModel(true, DeliveryStatusCode.UnPicked);
                }
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultModel(false, "檢查是否揀畢失敗:" + ex.Message);
            }


        }


        /// <summary>
        /// 刪除揀貨明細
        /// </summary>
        /// <param name="pickedDataList"></param>
        /// <param name="addUser"></param>
        /// <returns></returns>
        public ResultModel DelPickDT(List<DLV_PICKED_T> pickedDataList, string addUser, string addUserName)
        {
            if (pickedDataList == null || pickedDataList.Count == 0) return new ResultModel(false, "沒有揀貨資料");
            long dlvHeaderId = pickedDataList[0].DlvHeaderId;
            var headerData = dlvHeaderTRepositiory.GetAll().AsNoTracking().Where(x => x.DlvHeaderId == dlvHeaderId).ToList();
            if (headerData.Count == 0) return new ResultModel(false, "無法取得交運單資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var addDate = DateTime.Now;
                    foreach (DLV_PICKED_T data in pickedDataList)
                    {
                        var stock = GetStock(data.Barcode);
                        if (stock == null) throw new Exception("找不到庫存資料");
                        STK_TXN_T stkTxnT = CreateStockRecord(stock, null, "", "", null, CategoryCode.Delivery, ActionCode.Deleted, headerData[0].DeliveryName);
                        decimal? priQty = data.PrimaryQuantity;
                        decimal? secQty = data.SecondaryQuantity;
                        var updaeStockResult = UpdateStock(stock, stkTxnT, ref priQty, ref secQty, pickSatus, PickStatus.Deleted, addUser, addDate, true);
                        if (!updaeStockResult.Success) throw new Exception(updaeStockResult.Msg);
                        dlvPickedTRepositiory.Delete(data);
                    }

                    stockTRepositiory.SaveChanges();
                    stkTxnTRepositiory.SaveChanges();
                    dlvPickedTRepositiory.SaveChanges();

                    //更新Header狀態
                    var pickedResult = CheckPicked(dlvHeaderId);
                    if (!pickedResult.Success)
                    {
                        throw new Exception(pickedResult.Msg);
                    }
                    var updateDatas = GetDeliveryHeaderDataListFromHeaderId(dlvHeaderId);
                    if (updateDatas.Count == 0) throw new Exception("找不到交運單資料");
                    foreach (DLV_HEADER_T data in updateDatas)
                    {
                        data.DeliveryStatusCode = pickedResult.Msg;
                        data.DeliveryStatusName = deliveryStatusCode.GetDesc(pickedResult.Msg);
                        dlvHeaderTRepositiory.Update(data);
                        data.LastUpdateBy = addUser;
                        data.LastUpdateUserName = addUserName;
                        data.LastUpdateDate = addDate;
                    }
                    dlvHeaderTRepositiory.SaveChanges();


                    txn.Commit();
                    return new ResultModel(true, "刪除揀貨明細成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "刪除揀貨明細失敗:" + ex.Message);
                }
            }
        }



        public class PickStatus : IDetail
        {
            /// <summary>
            /// 已刪除
            /// </summary>
            public const string Deleted = "DS0";
            /// <summary>
            /// 已揀
            /// </summary>
            public const string Picked = "DS1";
            /// <summary>
            /// 已出貨
            /// </summary>
            public const string Shipped = "DS2";

            public string GetDesc(string statusCode)
            {
                switch (statusCode)
                {
                    case Deleted:
                        return "已刪除";
                    case Picked:
                        return "已揀";
                    case Shipped:
                        return "已出貨";
                    default:
                        return "";
                }
            }

            public string ToStockStatus(string statusCode)
            {
                switch (statusCode)
                {
                    case Deleted:
                        return StockStatusCode.InStock;
                    case Picked:
                        return StockStatusCode.DeliveryPicked;
                    case Shipped:
                        return StockStatusCode.Shipped;
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 單號狀態種類
        /// </summary>
        public class DeliveryStatusCode : IHeader
        {
            /// <summary>
            /// 已取消
            /// </summary>
            public const string Canceled = "DH0";
            /// <summary>
            /// 未印
            /// </summary>
            public const string Unprinted = "DH1";
            /// <summary>
            /// 待出
            /// </summary>
            public const string UnPicked = "DH2";
            /// <summary>
            /// 已揀
            /// </summary>
            public const string Picked = "DH3";
            /// <summary>
            /// 待核准
            /// </summary>
            public const string UnAuthorized = "DH4";
            /// <summary>
            /// 已出貨
            /// </summary>
            public const string Shipped = "DH5";

            public string GetDesc(string statusCode)
            {
                switch (statusCode)
                {
                    case Canceled:
                        return "已取消";
                    case Unprinted:
                        return "未印";
                    case UnPicked:
                        return "待出";
                    case Picked:
                        return "已揀";
                    case UnAuthorized:
                        return "待核准";
                    case Shipped:
                        return "已出貨";
                    default:
                        return "";
                }
            }

            //public string ToStockStatus(string statusCode)
            //{
            //    switch (statusCode)
            //    {
            //        case Canceled:
            //            return StockStatusCode.InStock;
            //        //case Unprinted:
            //        //    return "未印";
            //        //case UnPicked:
            //        //    return "待出";
            //        case Picked:
            //            return StockStatusCode.DeliveryPicked;
            //        //case UnAuthorized:
            //        //    return "待核准";
            //        case Shipped:
            //            return StockStatusCode.Shipped;
            //        default:
            //            return "";
            //    }
            //}
        }

        public void generateTestData()
        {
            try
            {
                #region 第一筆測試資料 平版 令包
                //DliveryHeaderRepositiory.getContext().Configuration.AutoDetectChangesEnabled = false;
                dlvHeaderTRepositiory.Create(new DLV_HEADER_T()
                {
                    DlvHeaderId = 1,
                    OrgId = 1,
                    OrgName = "1",
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "TB3",
                    TripCar = "PN01",
                    TripId = 1,
                    TripName = "Y191226-1036357",
                    TripActualShipDate = Convert.ToDateTime("2019-12-26"),
                    DeliveryId = 1,
                    DeliveryName = "FTY1912000547",
                    ItemCategory = "平版",
                    CustomerId = 1,
                    CustomerNumber = "1",
                    CustomerName = "保吉",
                    CustomerLocationCode = "福安印刷",
                    ShipCustomerId = 1,
                    ShipCustomerNumber = "1",
                    ShipCustomerName = "保吉紙業有限公司",
                    ShipLocationCode = "台南市安南區府安路5段119巷",
                    FreightTermsName = "台南",
                    DeliveryStatusCode = DeliveryStatusCode.Unprinted,
                    DeliveryStatusName = deliveryStatusCode.GetDesc(DeliveryStatusCode.Unprinted),
                    TransactionBy = null,
                    TransactionDate = null,
                    AuthorizeBy = null,
                    AuthorizeDate = null,
                    Note = "FT1.P9B0288",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);


                //DliveryHeaderRepositiory.getContext().Configuration.AutoDetectChangesEnabled = true;

                dlvDetailTRepositiory.Create(new DLV_DETAIL_T()
                {
                    DlvDetailId = 1,
                    DlvHeaderId = 1,
                    OrderNumber = 1192006167,
                    OrderLineId = 1,
                    OrderShipNumber = "1.2",
                    PackingType = "令包",
                    InventoryItemId = 504029,
                    ItemNumber = "4DM00A03500214K512K",
                    ItemDescription = "全塗灰銅卡",
                    ReamWeight = "274.27",
                    ItemCategory = "平版",
                    PaperType = "DM00",
                    BasicWeight = "03500",
                    Specification = "214K512K",
                    GrainDirection = "L",
                    LocatorId = null,
                    LocatorCode = null,
                    SrcRequestedQuantity = 0.1M,
                    SrcRequestedQuantityUom = "MT",
                    RequestedQuantity = 100M,
                    RequestedQuantityUom = "KG",
                    RequestedQuantity2 = 50,
                    RequestedQuantityUom2 = "RE",
                    OspBatchId = null,
                    OspBatchNo = "",
                    OspBatchType = "",
                    TmpItemId = null,
                    TmpItemNumber = "",
                    TmpItemDescription = "",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);
                #endregion

                #region 第二筆測試資料 捲筒
                dlvHeaderTRepositiory.Create(new DLV_HEADER_T()
                {
                    DlvHeaderId = 2,
                    OrgId = 1,
                    OrgName = "1",
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "TB3",
                    TripCar = "PN01",
                    TripId = 1,
                    TripName = "Y191226-1036357",
                    TripActualShipDate = Convert.ToDateTime("2019-12-26"),
                    DeliveryId = 1,
                    DeliveryName = "FTY1912000547",
                    ItemCategory = "捲筒",
                    CustomerId = 1,
                    CustomerNumber = "1",
                    CustomerName = "保吉",
                    CustomerLocationCode = "福安印刷",
                    ShipCustomerId = 1,
                    ShipCustomerNumber = "1",
                    ShipCustomerName = "保吉紙業有限公司",
                    ShipLocationCode = "台南市安南區府安路5段119巷",
                    FreightTermsName = "台南",
                    DeliveryStatusCode = DeliveryStatusCode.Unprinted,
                    DeliveryStatusName = deliveryStatusCode.GetDesc(DeliveryStatusCode.Unprinted),
                    TransactionBy = null,
                    TransactionDate = null,
                    AuthorizeBy = null,
                    AuthorizeDate = null,
                    Note = "FT1.P9B0288",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);

                dlvDetailTRepositiory.Create(new DLV_DETAIL_T()
                {
                    DlvDetailId = 2,
                    DlvHeaderId = 2,
                    OrderNumber = 1192006167,
                    OrderLineId = 1,
                    OrderShipNumber = "1.2",
                    PackingType = "",
                    InventoryItemId = 559299,
                    ItemNumber = "4AK0XA008001320RL00",
                    ItemDescription = "Express捲特級銅版",
                    ReamWeight = "2.2",
                    ItemCategory = "捲筒",
                    PaperType = "AK0X",
                    BasicWeight = "00800",
                    Specification = "1320RL00",
                    GrainDirection = "X",
                    LocatorId = null,
                    LocatorCode = null,
                    SrcRequestedQuantity = 1M,
                    SrcRequestedQuantityUom = "MT",
                    RequestedQuantity = 1000M,
                    RequestedQuantityUom = "KG",
                    RequestedQuantity2 = null,
                    RequestedQuantityUom2 = "",
                    OspBatchId = null,
                    OspBatchNo = "",
                    OspBatchType = "",
                    TmpItemId = null,
                    TmpItemNumber = "",
                    TmpItemDescription = "",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);

                #endregion

                #region 第三筆測試資料 平版 無令打件 代紙
                dlvHeaderTRepositiory.Create(new DLV_HEADER_T()
                {
                    DlvHeaderId = 3,
                    OrgId = 1,
                    OrgName = "1",
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "TB3",
                    TripCar = "PTB2",
                    TripId = 2,
                    TripName = "Y200109-1052058",
                    TripActualShipDate = Convert.ToDateTime("2020-01-09"),
                    DeliveryId = 2,
                    DeliveryName = "FTY2001000140",
                    ItemCategory = "平版",
                    CustomerId = 2,
                    CustomerNumber = "2",
                    CustomerName = "中華彩色",
                    CustomerLocationCode = "中華彩色",
                    ShipCustomerId = 2,
                    ShipCustomerNumber = "2",
                    ShipCustomerName = "中華彩色印刷股份有限公司",
                    ShipLocationCode = "新北市新店區寶橋路229號",
                    FreightTermsName = "台北",
                    DeliveryStatusCode = DeliveryStatusCode.Unprinted,
                    DeliveryStatusName = deliveryStatusCode.GetDesc(DeliveryStatusCode.Unprinted),
                    TransactionBy = null,
                    TransactionDate = null,
                    AuthorizeBy = null,
                    AuthorizeDate = null,
                    Note = "FT1.早上到X002010031大道季刊98期/P2010087",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);

                dlvDetailTRepositiory.Create(new DLV_DETAIL_T()
                {
                    DlvDetailId = 3,
                    DlvHeaderId = 3,
                    OrderNumber = 1202000114,
                    OrderLineId = 2,
                    OrderShipNumber = "1.1",
                    PackingType = "無令打件",
                    InventoryItemId = 505675,
                    ItemNumber = "4DM00P0270008271130",
                    ItemDescription = "全塗灰銅卡",
                    ReamWeight = "278.13",
                    ItemCategory = "平版",
                    PaperType = "DM00",
                    BasicWeight = "02700",
                    Specification = "08271130",
                    GrainDirection = "L",
                    LocatorId = null,
                    LocatorCode = null,
                    SrcRequestedQuantity = 0.1M,
                    SrcRequestedQuantityUom = "MT",
                    RequestedQuantity = 100M,
                    RequestedQuantityUom = "KG",
                    RequestedQuantity2 = 50,
                    RequestedQuantityUom2 = "RE",
                    OspBatchId = 2,
                    OspBatchNo = "P2010087",
                    OspBatchType = "",
                    TmpItemId = 506313,
                    TmpItemNumber = "4DM00P0270007991121",
                    TmpItemDescription = "全塗灰銅卡",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);
                #endregion

                #region 第四筆測試資料 捲筒
                dlvHeaderTRepositiory.Create(new DLV_HEADER_T()
                {
                    DlvHeaderId = 4,
                    OrgId = 1,
                    OrgName = "1",
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "SFG",
                    TripCar = "PTB3",
                    TripId = 3,
                    TripName = "Y200109-1052060",
                    TripActualShipDate = Convert.ToDateTime("2020-04-22"),
                    DeliveryId = 3,
                    DeliveryName = "FTY2001000152",
                    ItemCategory = "捲筒",
                    CustomerId = 2,
                    CustomerNumber = "2",
                    CustomerName = "中華彩色",
                    CustomerLocationCode = "中華彩色",
                    ShipCustomerId = 2,
                    ShipCustomerNumber = "2",
                    ShipCustomerName = "中華彩色印刷股份有限公司",
                    ShipLocationCode = "新北市新店區寶橋路229號",
                    FreightTermsName = "台北",
                    DeliveryStatusCode = DeliveryStatusCode.Unprinted,
                    DeliveryStatusName = deliveryStatusCode.GetDesc(DeliveryStatusCode.Unprinted),
                    TransactionBy = null,
                    TransactionDate = null,
                    AuthorizeBy = null,
                    AuthorizeDate = null,
                    Note = "",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);

                dlvDetailTRepositiory.Create(new DLV_DETAIL_T()
                {
                    DlvDetailId = 4,
                    DlvHeaderId = 4,
                    OrderNumber = 1192006168,
                    OrderLineId = 3,
                    OrderShipNumber = "1.1",
                    PackingType = "",
                    InventoryItemId = 558705,
                    ItemNumber = "4AH00A00900362KRL00",
                    ItemDescription = "捲筒琉麗",
                    ReamWeight = "2.2",
                    ItemCategory = "捲筒",
                    PaperType = "AH00",
                    BasicWeight = "00900",
                    Specification = "362KRL00",
                    GrainDirection = "X",
                    LocatorId = null,
                    LocatorCode = null,
                    SrcRequestedQuantity = 1M,
                    SrcRequestedQuantityUom = "MT",
                    RequestedQuantity = 1000M,
                    RequestedQuantityUom = "KG",
                    RequestedQuantity2 = null,
                    RequestedQuantityUom2 = "",
                    OspBatchId = null,
                    OspBatchNo = "",
                    OspBatchType = "",
                    TmpItemId = null,
                    TmpItemNumber = "",
                    TmpItemDescription = "",
                    CreatedBy = "1",
                    CreatedUserName = "華紙",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateUserName = "華紙",
                    LastUpdateDate = DateTime.Now,
                }, true);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }


        }



        public List<SelectListItem> GetTripNameDropDownList(DropDownListType type)
        {
            var tripNameList = createDropDownList(type);
            tripNameList.AddRange(getTripNameList());
            return tripNameList;
        }

        public List<SelectListItem> GetDeliveryStatusDropDownList(DropDownListType type)
        {
            var deliveryStatusList = createDropDownList(type);
            deliveryStatusList.AddRange(getDeliveryStatusList());
            return deliveryStatusList;
        }

        private List<SelectListItem> getDeliveryStatusList()
        {
            var deliveryStatusList = new List<SelectListItem>();
            try
            {
                deliveryStatusList.Add(new SelectListItem() { Text = deliveryStatusCode.GetDesc(DeliveryStatusCode.Canceled), Value = DeliveryStatusCode.Canceled });
                deliveryStatusList.Add(new SelectListItem() { Text = deliveryStatusCode.GetDesc(DeliveryStatusCode.Unprinted), Value = DeliveryStatusCode.Unprinted });
                deliveryStatusList.Add(new SelectListItem() { Text = deliveryStatusCode.GetDesc(DeliveryStatusCode.UnPicked), Value = DeliveryStatusCode.UnPicked });
                deliveryStatusList.Add(new SelectListItem() { Text = deliveryStatusCode.GetDesc(DeliveryStatusCode.Picked), Value = DeliveryStatusCode.Picked });
                deliveryStatusList.Add(new SelectListItem() { Text = deliveryStatusCode.GetDesc(DeliveryStatusCode.UnAuthorized), Value = DeliveryStatusCode.UnAuthorized });
                deliveryStatusList.Add(new SelectListItem() { Text = deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped), Value = DeliveryStatusCode.Shipped });

                var a = deliveryStatusList.Select(x => new SelectListItem() { Text = x.Text, Value = x.Value }).Where(x => x.Value == "1");
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return deliveryStatusList;
        }



        private List<SelectListItem> getTripNameList()
        {
            var tripNameList = new List<SelectListItem>();
            try
            {
                var tempList = dlvHeaderTRepositiory
                            .GetAll().AsNoTracking()
                            .OrderBy(x => x.TripName)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.TripName,
                                Value = x.TripId.ToString()
                            });
                tripNameList.AddRange(tempList);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return tripNameList;
        }

        public List<TripHeaderDT> DeliverySearch(string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
            string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus)
        {

            DateTime shipBeginDate = new DateTime();
            DateTime shipEndDate = new DateTime();
            DateTime tdate = new DateTime();

            bool shipBeginDateStatus = DateTime.TryParseExact(TripActualShipBeginDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out shipBeginDate);
            bool shipEndDateStatus = DateTime.TryParseExact(TripActualShipEndDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out shipEndDate);
            bool transactionDateStatus = DateTime.TryParseExact(TransactionDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out tdate);

            long tripId = 0;
            try
            {
                if (SelectedTrip != "*")
                {
                    tripId = Convert.ToInt64(SelectedTrip);
                }
            }
            catch
            {
                SelectedTrip = "*";
            }

            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            List<string> cond = new List<string>();
            string prefixCmd = @"
select CONVERT(char(10), AUTHORIZE_DATE,126) as AUTHORIZE_DATE,
CUSTOMER_LOCATION_CODE,
CUSTOMER_NAME,
DELIVERY_NAME,
DELIVERY_STATUS_NAME as DELIVERY_STATUS,
ITEM_CATEGORY as DetailType,
FREIGHT_TERMS_NAME,
DLV_HEADER_ID as Id,
NOTE,
SUBINVENTORY_CODE,
SHIP_CUSTOMER_NAME,
SHIP_LOCATION_CODE,
TRIP_ACTUAL_SHIP_DATE,
TRANSACTION_DATE,
TRIP_CAR,
TRIP_ID,
TRIP_NAME,
ROW_NUMBER() OVER(ORDER BY DLV_HEADER_ID) AS SUB_ID
from DLV_HEADER_T";
            //            string prefixCmd = @"
            //select CONVERT(char(10), h.AUTHORIZE_DATE,126) as AUTHORIZE_DATE,
            //h.CUSTOMER_LOCATION_CODE,
            //h.CUSTOMER_NAME,
            //h.DELIVERY_NAME,
            //h.DELIVERY_STATUS_NAME as DELIVERY_STATUS,
            //h.ITEM_CATEGORY as DetailType,
            //h.FREIGHT_TERMS_NAME,
            //h.DLV_HEADER_ID as Id,
            //h.NOTE,
            //h.SUBINVENTORY_CODE,
            //h.SHIP_CUSTOMER_NAME,
            //h.SHIP_LOCATION_CODE,
            //h.TRIP_ACTUAL_SHIP_DATE,
            //h.TRANSACTION_DATE,
            //h.TRIP_CAR,
            //h.TRIP_ID,
            //h.TRIP_NAME,
            //d.REQUESTED_PRIMARY_QUANTITY,
            //d.REQUESTED_PRIMARY_UOM,
            //d.REQUESTED_SECONDARY_QUANTITY,
            //d.REQUESTED_SECONDARY_UOM,
            //d.REQUESTED_TRANSACTION_QUANTITY,
            //d.REQUESTED_TRANSACTION_UOM,
            //ROW_NUMBER() OVER(ORDER BY h.DLV_HEADER_ID) AS SUB_ID
            //from DLV_HEADER_T h
            //inner join DLV_DETAIL_T d
            //on h.DLV_HEADER_ID = d.DLV_HEADER_ID";

            if (shipBeginDateStatus != false)
            {
                cond.Add("@shipBeginDate <= TRIP_ACTUAL_SHIP_DATE");
                sqlParameterList.Add(SqlParamHelper.GetDataTime("@shipBeginDate", shipBeginDate));
            }

            if (shipEndDateStatus != false)
            {
                cond.Add("TRIP_ACTUAL_SHIP_DATE <= @shipEndDate");
                sqlParameterList.Add(SqlParamHelper.GetDataTime("@shipEndDate", shipEndDate));
            }
            if (DeliveryName != "")
            {
                cond.Add("DELIVERY_NAME = @DeliveryName");
                sqlParameterList.Add(new SqlParameter("@DeliveryName", DeliveryName));
            }
            if (SelectedSubinventory != "*")
            {
                cond.Add("SUBINVENTORY_CODE = @SelectedSubinventory");
                sqlParameterList.Add(SqlParamHelper.R.SubinventoryCode("@SelectedSubinventory", SelectedSubinventory));
            }
            if (SelectedTrip != "*")
            {
                cond.Add("TRIP_ID = @tripId");
                sqlParameterList.Add(new SqlParameter("@tripId", tripId));
            }
            if (transactionDateStatus != false)
            {
                cond.Add("TRANSACTION_DATE = @tdate");
                sqlParameterList.Add(SqlParamHelper.GetDataTime("@tdate", tdate));
            }
            if (SelectedDeliveryStatus != "*")
            {
                cond.Add("DELIVERY_STATUS_CODE = @SelectedDeliveryStatus");
                sqlParameterList.Add(new SqlParameter("@SelectedDeliveryStatus", SelectedDeliveryStatus));
            }

            //string fullCmd = "";
            string commandText = string.Format(prefixCmd + "{0}{1}", cond.Count > 0 ? " WHERE " : "", string.Join(" AND ", cond.ToArray()));


            if (sqlParameterList.Count > 0)
            {
                return this.Context.Database.SqlQuery<TripHeaderDT>(commandText, sqlParameterList.ToArray()).ToList();

            }
            else
            {
                return this.Context.Database.SqlQuery<TripHeaderDT>(commandText).ToList();
            }



            //var query = dlvHeaderTRepositiory.GetAll().AsNoTracking()
            //    .Join(dlvDetailHtRepositiory.GetAll().AsNoTracking(),
            //    h => h.DlvHeaderId,
            //    d => d.DlvHeaderId,
            //     (h, d) => new TripHeaderDT
            //  {
            //      AUTHORIZE_DATE = h.AuthorizeDate != null ? ((DateTime)h.AuthorizeDate).ToString("yyyy-MM-dd") : "",
            //      CUSTOMER_LOCATION_CODE = h.CustomerLocationCode,
            //      CUSTOMER_NAME = h.CustomerName,
            //      DELIVERY_NAME = h.DeliveryName,
            //      DELIVERY_STATUS = h.DeliveryStatusName,
            //      DELIVERY_STATUS_CODE = h.DeliveryStatusCode,
            //      DetailType = h.ItemCategory,
            //      FREIGHT_TERMS_NAME = h.FreightTermsName,
            //      Id = h.DlvHeaderId,
            //      REMARK = h.Note,
            //      SUBINVENTORY_CODE = h.SubinventoryCode,
            //      SHIP_CUSTOMER_NAME = h.ShipCustomerName,
            //      SHIP_LOCATION_CODE = h.ShipLocationCode,
            //      TRIP_ACTUAL_SHIP_DATE = h.TripActualShipDate,
            //      TRANSACTION_DATE = h.TransactionDate,
            //      TRIP_CAR = h.TripCar,
            //      TRIP_ID = h.TripId,
            //      TRIP_NAME = h.TripName,
            //      REQUESTED_QUANTITY = d.RequestedQuantity,
            //      REQUESTED_QUANTITY_UOM = d.RequestedQuantityUom,
            //      REQUESTED_QUANTITY2 = d.RequestedQuantity2,
            //      REQUESTED_QUANTITY_UOM2 = d.RequestedQuantityUom2,
            //      SRC_REQUESTED_QUANTITY = d.SrcRequestedQuantity,
            //      SRC_REQUESTED_QUANTITY_UOM = d.SrcRequestedQuantityUom,
            //  })
            //  .Where(
            //  x =>
            //       (shipBeginDateStatus == false || shipBeginDate <= x.TRIP_ACTUAL_SHIP_DATE) &&
            //      (shipEndDateStatus == false || x.TRIP_ACTUAL_SHIP_DATE <= shipEndDate) &&
            //  (x.DELIVERY_NAME != null && x.DELIVERY_NAME.ToLower().Contains(DeliveryName.ToLower())) &&
            //  (SelectedSubinventory == "*" || x.SUBINVENTORY_CODE == SelectedSubinventory) &&
            //   (SelectedTrip == "*" || x.TRIP_ID == tripId) &&
            //    (transactionDateStatus == false || x.TRANSACTION_DATE == tdate) &&
            //     (SelectedDeliveryStatus == "*" || x.DELIVERY_STATUS_CODE == SelectedDeliveryStatus)
            //  );

            //return tempList;
        }

        /// <summary>
        /// 取不重複的TripId
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<long> GetTripIdList(List<long> dlvHeaderIds)
        {
            return dlvHeaderTRepositiory.GetAll().AsNoTracking().Where(x => dlvHeaderIds.Contains(x.DlvHeaderId)).GroupBy(x => x.TripId).Select(x => x.Key).ToList();
        }

        /// <summary>
        /// 取DLV_HEADER_T 資料
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<DLV_HEADER_T> GetDeliveryHeaderDataListFromTripId(List<long> tripIds)
        {
            return dlvHeaderTRepositiory.GetAll().AsNoTracking().Where(x => tripIds.Contains(x.TripId)).ToList();
        }

        /// <summary>
        /// 取DLV_HEADER_T 資料
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<DLV_HEADER_T> GetDeliveryHeaderDataListFromHeaderId(long dlvHeaderId)
        {
            return dlvHeaderTRepositiory.GetAll().AsNoTracking().Where(x => dlvHeaderId == x.DlvHeaderId).ToList();
        }

        /// <summary>
        /// 取DLV_DETAIL_T 資料
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<DLV_DETAIL_T> GetDeliveryDetailDataListFromHeaderId(long dlvHeaderId)
        {
            return dlvDetailTRepositiory.GetAll().AsNoTracking().Where(x => dlvHeaderId == x.DlvHeaderId).ToList();
        }


        public List<DLV_PICKED_T> GetDeliveryPickDataListFromPickedId(long dlvPickedId)
        {
            return dlvPickedTRepositiory.GetAll().AsNoTracking().Where(x => dlvPickedId == x.DlvPickedId).ToList();
        }

        public List<DLV_PICKED_T> GetDeliveryPickDataListFromPickedId(List<long> dlvPickedId)
        {
            return dlvPickedTRepositiory.GetAll().Where(x => dlvPickedId.Contains(x.DlvPickedId)).ToList();
        }


        /// <summary>
        /// 變更出貨申請
        /// </summary>
        /// <param name="updateDatas"></param>
        /// <param name="deliveryStatusCode"></param>
        /// <returns></returns>
        public ResultModel ChangeDeliveryConfirm(List<DLV_HEADER_T> updateDatas, string statusCode, string userId, string userName)
        {
            if (updateDatas == null) return new ResultModel(false, "沒有交運單資料");
            if (updateDatas.Count == 0) return new ResultModel(false, "沒有交運單資料");
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    foreach (DLV_HEADER_T data in updateDatas)
                    {
                        data.TransactionDate = now;
                        data.TransactionBy = userId;
                        data.TransactionByUserNmae = userName;
                        data.DeliveryStatusCode = statusCode;
                        data.DeliveryStatusName = deliveryStatusCode.GetDesc(statusCode);
                        dlvHeaderTRepositiory.Update(data);
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                    }
                    dlvHeaderTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "變更出貨申請成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "變更出貨申請失敗:" + ex.Message);
                }
            }
        }


        /// <summary>
        /// 出貨核准
        /// </summary>
        /// <param name="updateDatas">要核准的Header資料</param>
        /// <param name="authorizeDate">核准日期</param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultModel DeliveryAuthorize(List<DLV_HEADER_T> updateDatas, DateTime authorizeDate, string userId, string userName)
        {
            if (updateDatas == null || updateDatas.Count == 0) return new ResultModel(false, "沒有交運單資料");
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    string deliveryStatusName = deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped);
                    //改出貨核准日期、狀態改為已出貨
                    foreach (DLV_HEADER_T data in updateDatas)
                    {
                        //更新出貨檔頭
                        data.AuthorizeBy = userId;
                        data.AuthorizeByUserName = userName;
                        data.AuthorizeDate = authorizeDate;
                        data.DeliveryStatusCode = DeliveryStatusCode.Shipped;
                        data.DeliveryStatusName = deliveryStatusName;
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                        dlvHeaderTRepositiory.Update(data);

                        //複製出貨明細資料到出貨歷史明細
                        string cmd = @"
INSERT INTO [DLV_DETAIL_HT]
(
	[DLV_DETAIL_ID]
      ,[DLV_HEADER_ID]
      ,[Order_Number]
      ,[ORDER_LINE_ID]
      ,[ORDER_SHIP_NUMBER]
      ,[PACKING_TYPE]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[REAM_WEIGHT]
      ,[ITEM_CATEGORY]
      ,[PAPER_TYPE]
      ,[BASIC_WEIGHT]
      ,[SPECIFICATION]
      ,[GRAIN_DIRECTION]
      ,[LOCATOR_ID]
      ,[LOCATOR_CODE]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[OSP_BATCH_ID]
      ,[OSP_BATCH_NO]
      ,[OSP_BATCH_TYPE]
      ,[TMP_ITEM_ID]
      ,[TMP_ITEM_NUMBER]
      ,[TMP_ITEM_DESCRIPTION]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [DLV_DETAIL_ID]
      ,[DLV_HEADER_ID]
      ,[Order_Number]
      ,[ORDER_LINE_ID]
      ,[ORDER_SHIP_NUMBER]
      ,[PACKING_TYPE]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[ITEM_DESCRIPTION]
      ,[REAM_WEIGHT]
      ,[ITEM_CATEGORY]
      ,[PAPER_TYPE]
      ,[BASIC_WEIGHT]
      ,[SPECIFICATION]
      ,[GRAIN_DIRECTION]
      ,[LOCATOR_ID]
      ,[LOCATOR_CODE]
      ,[REQUESTED_TRANSACTION_QUANTITY]
      ,[REQUESTED_TRANSACTION_UOM]
      ,[REQUESTED_PRIMARY_QUANTITY]
      ,[REQUESTED_PRIMARY_UOM]
      ,[REQUESTED_SECONDARY_QUANTITY]
      ,[REQUESTED_SECONDARY_UOM]
      ,[OSP_BATCH_ID]
      ,[OSP_BATCH_NO]
      ,[OSP_BATCH_TYPE]
      ,[TMP_ITEM_ID]
      ,[TMP_ITEM_NUMBER]
      ,[TMP_ITEM_DESCRIPTION]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [DLV_DETAIL_T]
  WHERE DLV_HEADER_ID = @DLV_HEADER_ID
";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@DLV_HEADER_ID", data.DlvHeaderId)) <= 0)
                        {
                            throw new Exception("複製出貨明細資料到出貨歷史明細失敗");
                        }
                        //刪除出貨明細資料
                        cmd = @"
  DELETE FROM [DLV_DETAIL_T]
  WHERE DLV_HEADER_ID = @DLV_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@DLV_HEADER_ID", data.DlvHeaderId)) <= 0)
                        {
                            throw new Exception("刪除出貨明細資料失敗");
                        }
                        //複製出貨揀貨資料到出貨歷史揀貨
                        cmd = @"
INSERT INTO [DLV_PICKED_HT](
[DLV_PICKED_ID]
      ,[DLV_DETAIL_ID]
      ,[DLV_HEADER_ID]
      ,[STOCK_ID]
      ,[STATUS]
      ,[PALLET_STATUS]
      ,[LOCATOR_ID]
      ,[LOCATOR_CODE]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[PACKING_TYPE]
      ,[REAM_WEIGHT]
      ,[BARCODE]
      ,[TRANSACTION_QUANTITY]
      ,[TRANSACTION_UOM]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
)
SELECT [DLV_PICKED_ID]
      ,[DLV_DETAIL_ID]
      ,[DLV_HEADER_ID]
      ,[STOCK_ID]
      ,[STATUS]
      ,[PALLET_STATUS] 
      ,[LOCATOR_ID]
      ,[LOCATOR_CODE]
      ,[INVENTORY_ITEM_ID]
      ,[ITEM_NUMBER]
      ,[PACKING_TYPE]
      ,[REAM_WEIGHT]
      ,[BARCODE]
      ,[TRANSACTION_QUANTITY]
      ,[TRANSACTION_UOM]
      ,[PRIMARY_QUANTITY]
      ,[PRIMARY_UOM]
      ,[SECONDARY_QUANTITY]
      ,[SECONDARY_UOM]
      ,[LOT_NUMBER]
      ,[LOT_QUANTITY]
      ,[CREATED_BY]
      ,[CREATED_USER_NAME]
      ,[CREATION_DATE]
      ,[LAST_UPDATE_BY]
      ,[LAST_UPDATE_USER_NAME]
      ,[LAST_UPDATE_DATE]
  FROM [DLV_PICKED_T]
  WHERE DLV_HEADER_ID = @DLV_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@DLV_HEADER_ID", data.DlvHeaderId)) <= 0)
                        {
                            throw new Exception("複製出貨揀貨資料到出貨歷史揀貨失敗");
                        }
                        //刪除出貨明細資料
                        cmd = @"
  DELETE FROM [DLV_PICKED_T]
  WHERE DLV_HEADER_ID = @DLV_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@DLV_HEADER_ID", data.DlvHeaderId)) <= 0)
                        {
                            throw new Exception("刪除出貨揀貨資料失敗");
                        }
                    }

                    dlvHeaderTRepositiory.SaveChanges();

                    txn.Commit();
                    return new ResultModel(true, "出貨核准成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "出貨核准失敗:" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 更新交運單狀態
        /// </summary>
        /// <param name="updateDatas"></param>
        /// <param name="deliveryStatusCode"></param>
        /// <returns></returns>
        public ResultModel UpdateDeliveryStatus(List<DLV_HEADER_T> updateDatas, string statusCode, string userId, string userName)
        {
            if (updateDatas == null) return new ResultModel(false, "沒有交運單資料");
            if (updateDatas.Count == 0) return new ResultModel(false, "沒有交運單資料");
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    foreach (DLV_HEADER_T data in updateDatas)
                    {
                        data.DeliveryStatusCode = statusCode;
                        data.DeliveryStatusName = deliveryStatusCode.GetDesc(statusCode);
                        dlvHeaderTRepositiory.Update(data);
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                    }
                    dlvHeaderTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "交運單狀態更新成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "交運單狀態更新失敗:" + ex.Message);
                }
            }
        }



        /// <summary>
        /// 更新出貨核准日
        /// </summary>
        /// <param name="selectDatas"></param>
        /// <returns></returns>
        public ResultModel UpdateTransactionAuthorizeDates(TripDetailDTEditor selectDatas, string userId, string userName)
        {
            //List<TripHeaderDT> result = new List<TripHeaderDT>();

            //dlvHeaderTRepositiory.GetAll().AsNoTracking().Where(x => data.TripDetailDTList.  .Contains(x.DlvHeaderId)).GroupBy(x => x.TripId).Select(x => x.Key).ToList();

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    foreach (var selectData in selectDatas.TripDetailDTList)
                    {
                        var updateDatas = dlvHeaderTRepositiory.GetAll().AsNoTracking().Where(x => x.TripId == selectData.TRIP_ID).ToList();
                        if (updateDatas.Count == 0)
                        {
                            throw new Exception("無法取得航程號資料");
                        }

                        foreach (DLV_HEADER_T data in updateDatas)
                        {
                            data.AuthorizeDate = Convert.ToDateTime(selectData.AUTHORIZE_DATE);
                            data.LastUpdateBy = userId;
                            data.LastUpdateUserName = userName;
                            data.LastUpdateDate = now;
                            dlvHeaderTRepositiory.Update(data);
                        }
                    }
                    dlvHeaderTRepositiory.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "更新出貨核准日成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "更新出貨核准日失敗:" + ex.Message);
                }
            }
        }

        #region 捲筒

        /// <summary>
        /// 取得捲筒明細表單內容
        /// </summary>
        /// <param name="dlvHeaderId"></param>
        /// <returns></returns>
        public List<PaperRollEditDT> GetRollDetailDT(long dlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            string cmd = "";
            if (DELIVERY_STATUS_NAME == deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped))
            {
                cmd = @"
            select
d.DLV_DETAIL_ID as ID,
ROW_NUMBER() OVER(ORDER BY d.DLV_DETAIL_ID) AS SUB_ID,
MIN(d.ORDER_NUMBER) AS ORDER_NUMBER,
MIN(d.ORDER_SHIP_NUMBER) AS ORDER_SHIP_NUMBER,
MIN(d.OSP_BATCH_ID) AS OSP_BATCH_ID,
MIN(d.OSP_BATCH_NO) AS OSP_BATCH_NO,
MIN(d.INVENTORY_ITEM_ID) AS INVENTORY_ITEM_ID,
MIN(d.ITEM_NUMBER) AS ITEM_NUMBER,
MIN(d.TMP_ITEM_ID) AS TMP_ITEM_ID,
MIN(d.TMP_ITEM_NUMBER) AS TMP_ITEM_NUMBER,

MIN(d.PAPER_TYPE) AS PAPER_TYPE,
MIN(d.BASIC_WEIGHT) AS BASIC_WEIGHT,
MIN(d.SPECIFICATION) AS SPECIFICATION,

MIN(d.REQUESTED_TRANSACTION_QUANTITY) AS SRC_REQUESTED_QUANTITY,
SUM(ISNULL(p.TRANSACTION_QUANTITY, 0)) AS SRC_PICKED_QUANTITY,
MIN(d.REQUESTED_TRANSACTION_UOM) AS SRC_REQUESTED_QUANTITY_UOM,

MIN(d.REQUESTED_PRIMARY_QUANTITY) as REQUESTED_QUANTITY,
SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) AS PICKED_QUANTITY,
MIN(d.REQUESTED_PRIMARY_UOM) AS REQUESTED_QUANTITY_UOM

from DLV_DETAIL_HT d
LEFT JOIN DLV_PICKED_HT p ON p.DLV_HEADER_ID = d.DLV_HEADER_ID AND p.DLV_DETAIL_ID = d.DLV_DETAIL_ID
where d.DLV_HEADER_ID = @DLV_HEADER_ID
GROUP BY d.DLV_DETAIL_ID";
                //string cmd = @"
                //select 
                //DLV_DETAIL_ID as ID,
                //DLV_HEADER_ID as DlvHeaderId,
                //ROW_NUMBER() OVER(ORDER BY DLV_DETAIL_ID) AS SUB_ID,
                //ORDER_NUMBER,
                //ORDER_SHIP_NUMBER,
                //OSP_BATCH_ID,
                //OSP_BATCH_NO,
                //INVENTORY_ITEM_ID,
                //ITEM_NUMBER,
                //TMP_ITEM_ID,
                //TMP_ITEM_NUMBER,
                //PAPER_TYPE,
                //BASIC_WEIGHT,
                //SPECIFICATION,
                //REQUESTED_PRIMARY_QUANTITY as REQUESTED_QUANTITY,
                //(select SUM(PRIMARY_QUANTITY) from DLV_PICKED_T where DLV_HEADER_ID = @DLV_HEADER_ID) as PICKED_QUANTITY,
                //REQUESTED_PRIMARY_UOM as REQUESTED_QUANTITY_UOM,
                //REQUESTED_TRANSACTION_QUANTITY as SRC_REQUESTED_QUANTITY,
                //(select SUM(TRANSACTION_QUANTITY) from DLV_PICKED_T where DLV_HEADER_ID = @DLV_HEADER_ID) as SRC_PICKED_QUANTITY,
                //REQUESTED_TRANSACTION_UOM as SRC_REQUESTED_QUANTITY_UOM
                //from DLV_DETAIL_T
                //where DLV_HEADER_ID = @DLV_HEADER_ID";
            }
            else
            {
                cmd = @"
            select
d.DLV_DETAIL_ID as ID,
ROW_NUMBER() OVER(ORDER BY d.DLV_DETAIL_ID) AS SUB_ID,
MIN(d.ORDER_NUMBER) AS ORDER_NUMBER,
MIN(d.ORDER_SHIP_NUMBER) AS ORDER_SHIP_NUMBER,
MIN(d.OSP_BATCH_ID) AS OSP_BATCH_ID,
MIN(d.OSP_BATCH_NO) AS OSP_BATCH_NO,
MIN(d.INVENTORY_ITEM_ID) AS INVENTORY_ITEM_ID,
MIN(d.ITEM_NUMBER) AS ITEM_NUMBER,
MIN(d.TMP_ITEM_ID) AS TMP_ITEM_ID,
MIN(d.TMP_ITEM_NUMBER) AS TMP_ITEM_NUMBER,

MIN(d.PAPER_TYPE) AS PAPER_TYPE,
MIN(d.BASIC_WEIGHT) AS BASIC_WEIGHT,
MIN(d.SPECIFICATION) AS SPECIFICATION,

MIN(d.REQUESTED_TRANSACTION_QUANTITY) AS SRC_REQUESTED_QUANTITY,
SUM(ISNULL(p.TRANSACTION_QUANTITY, 0)) AS SRC_PICKED_QUANTITY,
MIN(d.REQUESTED_TRANSACTION_UOM) AS SRC_REQUESTED_QUANTITY_UOM,

MIN(d.REQUESTED_PRIMARY_QUANTITY) as REQUESTED_QUANTITY,
SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) AS PICKED_QUANTITY,
MIN(d.REQUESTED_PRIMARY_UOM) AS REQUESTED_QUANTITY_UOM

from DLV_DETAIL_T d
LEFT JOIN DLV_PICKED_T p ON p.DLV_HEADER_ID = d.DLV_HEADER_ID AND p.DLV_DETAIL_ID = d.DLV_DETAIL_ID
where d.DLV_HEADER_ID = @DLV_HEADER_ID
GROUP BY d.DLV_DETAIL_ID";
            }
            return this.Context.Database.SqlQuery<PaperRollEditDT>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();

        }

        public List<PaperRollEditBarcodeDT> GetRollPickDT(long dlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            string cmd = "";
            if (DELIVERY_STATUS_NAME == deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped))
            {
                cmd = @"
select
DLV_PICKED_ID as PICKED_ID,
ROW_NUMBER() OVER(ORDER BY DLV_DETAIL_ID) AS SUB_ID,
DLV_HEADER_ID as DlvHeaderId,
DLV_DETAIL_ID as PaperRollEditDT_ID,
PALLET_STATUS,
ITEM_NUMBER,
BARCODE,
PRIMARY_QUANTITY,
PRIMARY_UOM
from DLV_PICKED_HT
where DLV_HEADER_ID = @DLV_HEADER_ID";
            }
            else
            {
                cmd = @"
select
DLV_PICKED_ID as PICKED_ID,
ROW_NUMBER() OVER(ORDER BY DLV_DETAIL_ID) AS SUB_ID,
DLV_HEADER_ID as DlvHeaderId,
DLV_DETAIL_ID as PaperRollEditDT_ID,
PALLET_STATUS,
ITEM_NUMBER,
BARCODE,
PRIMARY_QUANTITY,
PRIMARY_UOM
from DLV_PICKED_T
where DLV_HEADER_ID = @DLV_HEADER_ID";
            }
            return this.Context.Database.SqlQuery<PaperRollEditBarcodeDT>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();

        }
        #endregion


        #region 平版

        /// <summary>
        /// 取得捲筒明細表單內容
        /// </summary>
        /// <param name="dlvHeaderId"></param>
        /// <returns></returns>
        public List<FlatEditDT> GetFlatDetailDT(long dlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            string cmd = "";
            if (DELIVERY_STATUS_NAME == deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped))
            {
                cmd = @"
select 
d.DLV_DETAIL_ID as ID,
ROW_NUMBER() OVER(ORDER BY d.DLV_DETAIL_ID) AS SUB_ID,
MIN(d.ORDER_NUMBER) AS ORDER_NUMBER,
MIN(d.ORDER_SHIP_NUMBER) AS ORDER_SHIP_NUMBER,
MIN(d.OSP_BATCH_ID) AS OSP_BATCH_ID,
MIN(d.OSP_BATCH_NO) AS OSP_BATCH_NO,
MIN(d.INVENTORY_ITEM_ID) AS INVENTORY_ITEM_ID,
MIN(d.ITEM_NUMBER) AS ITEM_NUMBER,
MIN(d.TMP_ITEM_ID) AS TMP_ITEM_ID,
MIN(d.TMP_ITEM_NUMBER) AS TMP_ITEM_NUMBER,
MIN(d.REAM_WEIGHT) AS REAM_WEIGHT,
MIN(d.PACKING_TYPE) AS PACKING_TYPE,

MIN(d.REQUESTED_TRANSACTION_QUANTITY) AS SRC_REQUESTED_QUANTITY,
SUM(ISNULL(p.TRANSACTION_QUANTITY, 0)) AS SRC_PICKED_QUANTITY,
MIN(d.REQUESTED_TRANSACTION_UOM) AS SRC_REQUESTED_QUANTITY_UOM,

MIN(d.REQUESTED_PRIMARY_QUANTITY) as REQUESTED_QUANTITY,
SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) AS PICKED_QUANTITY,
MIN(d.REQUESTED_PRIMARY_UOM) AS REQUESTED_QUANTITY_UOM,

MIN(d.REQUESTED_SECONDARY_QUANTITY) AS REQUESTED_QUANTITY2,
SUM(ISNULL(p.SECONDARY_QUANTITY, 0)) AS PICKED_QUANTITY2,
MIN(d.REQUESTED_SECONDARY_UOM) AS REQUESTED_QUANTITY_UOM2
from DLV_DETAIL_HT d
LEFT JOIN DLV_PICKED_HT p ON p.DLV_HEADER_ID = d.DLV_HEADER_ID AND p.DLV_DETAIL_ID = d.DLV_DETAIL_ID
where d.DLV_HEADER_ID = @DLV_HEADER_ID
GROUP BY d.DLV_DETAIL_ID";
            }
            else
            {
                cmd = @"
select 
d.DLV_DETAIL_ID as ID,
ROW_NUMBER() OVER(ORDER BY d.DLV_DETAIL_ID) AS SUB_ID,
MIN(d.ORDER_NUMBER) AS ORDER_NUMBER,
MIN(d.ORDER_SHIP_NUMBER) AS ORDER_SHIP_NUMBER,
MIN(d.OSP_BATCH_ID) AS OSP_BATCH_ID,
MIN(d.OSP_BATCH_NO) AS OSP_BATCH_NO,
MIN(d.INVENTORY_ITEM_ID) AS INVENTORY_ITEM_ID,
MIN(d.ITEM_NUMBER) AS ITEM_NUMBER,
MIN(d.TMP_ITEM_ID) AS TMP_ITEM_ID,
MIN(d.TMP_ITEM_NUMBER) AS TMP_ITEM_NUMBER,
MIN(d.REAM_WEIGHT) AS REAM_WEIGHT,
MIN(d.PACKING_TYPE) AS PACKING_TYPE,

MIN(d.REQUESTED_TRANSACTION_QUANTITY) AS SRC_REQUESTED_QUANTITY,
SUM(ISNULL(p.TRANSACTION_QUANTITY, 0)) AS SRC_PICKED_QUANTITY,
MIN(d.REQUESTED_TRANSACTION_UOM) AS SRC_REQUESTED_QUANTITY_UOM,

MIN(d.REQUESTED_PRIMARY_QUANTITY) as REQUESTED_QUANTITY,
SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) AS PICKED_QUANTITY,
MIN(d.REQUESTED_PRIMARY_UOM) AS REQUESTED_QUANTITY_UOM,

MIN(d.REQUESTED_SECONDARY_QUANTITY) AS REQUESTED_QUANTITY2,
SUM(ISNULL(p.SECONDARY_QUANTITY, 0)) AS PICKED_QUANTITY2,
MIN(d.REQUESTED_SECONDARY_UOM) AS REQUESTED_QUANTITY_UOM2
from DLV_DETAIL_T d
LEFT JOIN DLV_PICKED_T p ON p.DLV_HEADER_ID = d.DLV_HEADER_ID AND p.DLV_DETAIL_ID = d.DLV_DETAIL_ID
where d.DLV_HEADER_ID = @DLV_HEADER_ID
GROUP BY d.DLV_DETAIL_ID";
            }


            //            string cmd = @"
            //select 
            //DLV_DETAIL_ID as ID,
            //ROW_NUMBER() OVER(ORDER BY DLV_DETAIL_ID) AS SUB_ID,
            //ORDER_NUMBER,
            //ORDER_SHIP_NUMBER,
            //OSP_BATCH_ID,
            //OSP_BATCH_NO,
            //INVENTORY_ITEM_ID,
            //ITEM_NUMBER,
            //TMP_ITEM_ID,
            //TMP_ITEM_NUMBER,
            //REAM_WEIGHT,
            //PACKING_TYPE,
            //REQUESTED_PRIMARY_QUANTITY as REQUESTED_QUANTITY,
            //(select SUM(PRIMARY_QUANTITY) from DLV_PICKED_T where DLV_HEADER_ID = @DLV_HEADER_ID) as PICKED_QUANTITY,
            //REQUESTED_PRIMARY_UOM as REQUESTED_QUANTITY_UOM,
            //[REQUESTED_SECONDARY_QUANTITY] as REQUESTED_QUANTITY2,
            //(select SUM(SECONDARY_QUANTITY) from DLV_PICKED_T where DLV_HEADER_ID = @DLV_HEADER_ID) as PICKED_QUANTITY2,
            //[REQUESTED_SECONDARY_UOM] as REQUESTED_QUANTITY_UOM2,
            //REQUESTED_TRANSACTION_QUANTITY as SRC_REQUESTED_QUANTITY,
            //(select SUM(TRANSACTION_QUANTITY) from DLV_PICKED_T where DLV_HEADER_ID = @DLV_HEADER_ID) as SRC_PICKED_QUANTITY,
            //REQUESTED_TRANSACTION_UOM as SRC_REQUESTED_QUANTITY_UOM
            //from DLV_DETAIL_T
            //where DLV_HEADER_ID = @DLV_HEADER_ID";

            return this.Context.Database.SqlQuery<FlatEditDT>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();

        }

        public List<FlatEditBarcodeDT> GetFlatPickDT(long dlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            string cmd = "";
            if (DELIVERY_STATUS_NAME == deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped))
            {
                cmd = @"
select 
DLV_PICKED_ID as PICKED_ID,
ROW_NUMBER() OVER(ORDER BY DLV_DETAIL_ID) AS SUB_ID,
DLV_HEADER_ID as DlvHeaderId,
DLV_DETAIL_ID as FlatEditDT_ID,
PALLET_STATUS,
ITEM_NUMBER,
BARCODE,
REAM_WEIGHT,
PACKING_TYPE,
PRIMARY_QUANTITY,
PRIMARY_UOM,
SECONDARY_QUANTITY,
SECONDARY_UOM
from DLV_PICKED_HT
where DLV_HEADER_ID = @DLV_HEADER_ID"; ;

            }
            else
            {
                cmd = @"
select 
DLV_PICKED_ID as PICKED_ID,
ROW_NUMBER() OVER(ORDER BY DLV_DETAIL_ID) AS SUB_ID,
DLV_HEADER_ID as DlvHeaderId,
DLV_DETAIL_ID as FlatEditDT_ID,
PALLET_STATUS,
ITEM_NUMBER,
BARCODE,
REAM_WEIGHT,
PACKING_TYPE,
PRIMARY_QUANTITY,
PRIMARY_UOM,
SECONDARY_QUANTITY,
SECONDARY_UOM
from DLV_PICKED_T
where DLV_HEADER_ID = @DLV_HEADER_ID"; ;
            }
            return this.Context.Database.SqlQuery<FlatEditBarcodeDT>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();

        }


        #endregion


        public ResultDataModel<List<LabelModel>> GetLabels(List<long> PICKED_IDs, string userName)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (PICKED_IDs == null || PICKED_IDs.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                var pickDataList = dlvPickedTRepositiory.GetAll().AsNoTracking().Where(x => PICKED_IDs.Contains(x.DlvPickedId)).ToList();
                if (pickDataList == null || pickDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);

                foreach (DLV_PICKED_T data in pickDataList)
                {
                    StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as BatchNo");

                    if (data.PalletStatus == PalletStatusCode.Split)//判斷是否拆板
                    {
                        if (data.SecondaryQuantity != null) //判斷是否為平版
                        {
                            //拆板 平版
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(s.SECONDARY_AVAILABLE_QTY,'0.##########') as Qty
FROM [DLV_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else
                        {
                            //拆板 捲筒
                            return new ResultDataModel<List<LabelModel>>(false, "捲筒不能拆板", null);
                        }
                    }
                    else
                    {
                        if (data.SecondaryQuantity != null) //判斷是否為平版
                        {
                            //整板 平版
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [DLV_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else
                        {
                            //整板 捲筒
                            cmd.Append(@"
,s.PRIMARY_UOM_CODE as Unit
,FORMAT(p.PRIMARY_QUANTITY,'0.##########') as Qty
FROM [DLV_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                    }
                    //new SqlParameter("@userName", userName);

                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", data.Barcode)).ToList();
                    if (labelModel == null || labelModel.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                    labelModelList.Add(labelModel[0]);
                }
                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);

            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }
           

        }

    }
}