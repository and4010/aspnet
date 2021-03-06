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
using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Web.Configuration;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class DeliveryUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<DLV_ORG_T> dlvOrgTRepository;
        private readonly IRepository<DLV_HEADER_T> dlvHeaderTRepository;
        private readonly IRepository<DLV_DETAIL_T> dlvDetailTRepository;
        private readonly IRepository<DLV_DETAIL_HT> dlvDetailHtRepository;
        private readonly IRepository<DLV_PICKED_T> dlvPickedTRepository;
        private readonly IRepository<DLV_PICKED_HT> dlvPickedHtRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DeliveryUOW(DbContext context)
            : base(context)
        {
            dlvOrgTRepository = new GenericRepository<DLV_ORG_T>(this);
            dlvHeaderTRepository = new GenericRepository<DLV_HEADER_T>(this);
            dlvDetailTRepository = new GenericRepository<DLV_DETAIL_T>(this);
            dlvDetailHtRepository = new GenericRepository<DLV_DETAIL_HT>(this);
            dlvPickedTRepository = new GenericRepository<DLV_PICKED_T>(this);
            dlvPickedHtRepository = new GenericRepository<DLV_PICKED_HT>(this);
        }

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
            var detailData = dlvDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.DlvDetailId == dlvDetailId);
            if (detailData == null) return new ResultModel(false, "找不到明細資料");
            if (string.IsNullOrEmpty(detailData.ItemNumber)) return new ResultModel(false, "找不到明細料號");
            if (string.IsNullOrEmpty(stock.ItemNumber)) return new ResultModel(false, "找不到條碼料號");

            if (stock.ItemNumber != detailData.ItemNumber)
            {
               return new ResultModel(false, "此條碼不符合已選擇的料號");
            }

            if (detailData.LocatorId != null && detailData.LocatorId != stock.LocatorId)
            {
                var stockLocator = locatorTRepository.GetAll().FirstOrDefault(x => x.LocatorId == stock.LocatorId);
                var detailLocator = locatorTRepository.GetAll().FirstOrDefault(x => x.LocatorId == detailData.LocatorId);
                string stockLocatorSegment3 = "";
                string detailLocatorSegment3 = "";
                if (stockLocator != null) stockLocatorSegment3 = stockLocator.Segment3;
                if (detailLocator != null) detailLocatorSegment3 = detailLocator.Segment3;
                return new ResultModel(false, "此條碼的儲位" + stockLocatorSegment3 + "不符合出貨儲位" + detailLocatorSegment3);
            }

            //平版且為令包時 不檢查條碼重複
            if (detailData.ItemCategory == ItemCategory.Roll || (detailData.ItemCategory == ItemCategory.Flat && detailData.PackingType == PackingType.NoReam))
            {
                var pickData = dlvPickedTRepository.GetAll().AsNoTracking().Where(x => x.Barcode == barcode && x.DlvHeaderId == dlvHeaderId).ToList();
                if (pickData.Count > 0) return new ResultModel(false, "條碼重複輸入");
            }

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
                        throw new Exception(model.Msg);
                    }
                    //新增一筆PickDT
                    dlvPickedTRepository.Create(new DLV_PICKED_T
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
                        LotQuantity = stock.LotQuantity,
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
                        LastUpdateBy = null,
                        LastUpdateUserName = null,
                        LastUpdateDate = null,
                        Status = status,
                        PalletStatus = palletStatus
                    });


                    stockTRepository.SaveChanges();
                    stkTxnTRepository.SaveChanges();
                    dlvPickedTRepository.SaveChanges();

                    //更新Header狀態
                    var pickedResult = CheckPicked(dlvHeaderId);
                    if (!pickedResult.Success)
                    {
                        throw new Exception(pickedResult.Msg);
                    }
                    var updateDatas = GetDeliveryHeaderDataListFromHeaderId(dlvHeaderId);
                    if (updateDatas == null || updateDatas.Count == 0) throw new Exception("找不到交運單資料");
                    foreach (DLV_HEADER_T data in updateDatas)
                    {
                        data.DeliveryStatusCode = pickedResult.Data;
                        data.DeliveryStatusName = deliveryStatusCode.GetDesc(pickedResult.Data);
                        dlvHeaderTRepository.Update(data);
                        data.LastUpdateBy = addUser;
                        data.LastUpdateUserName = addUserName;
                        data.LastUpdateDate = addDate;
                    }
                    dlvHeaderTRepository.SaveChanges();


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
        public ResultDataModel<string> CheckPicked(long dlvHeaderId)
        {
            try
            {
                string cmd = @"
          select 
 case when ((SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) > MIN(d.REQUESTED_PRIMARY_QUANTITY * 1.05) AND MIN(d.ITEM_CATEGORY) = '捲筒' AND MIN(d.REQUESTED_TRANSACTION_UOM) = 'LB')
 OR (SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) > MIN(d.REQUESTED_PRIMARY_QUANTITY) AND MIN(d.ITEM_CATEGORY) = '捲筒' AND MIN(d.REQUESTED_TRANSACTION_UOM) != 'LB')
 OR (SUM(ISNULL(p.SECONDARY_QUANTITY, 0)) > MIN(d.REQUESTED_SECONDARY_QUANTITY) AND MIN(d.ITEM_CATEGORY) = '平版' ))
 then 1
 when ((SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) < MIN(d.REQUESTED_PRIMARY_QUANTITY * 0.95) AND MIN(d.ITEM_CATEGORY) = '捲筒' AND MIN(d.REQUESTED_TRANSACTION_UOM) = 'LB')
 OR (SUM(ISNULL(p.PRIMARY_QUANTITY, 0)) < MIN(d.REQUESTED_PRIMARY_QUANTITY) AND MIN(d.ITEM_CATEGORY) = '捲筒' AND MIN(d.REQUESTED_TRANSACTION_UOM) != 'LB' )
 OR (SUM(ISNULL(p.SECONDARY_QUANTITY, 0)) < MIN(d.REQUESTED_SECONDARY_QUANTITY) AND MIN(d.ITEM_CATEGORY) = '平版' ))
 then -1
 else 0
 end
from DLV_DETAIL_T d
JOIN DLV_HEADER_T h ON h.DLV_HEADER_ID = d.DLV_HEADER_ID
LEFT JOIN DLV_PICKED_T p ON p.DLV_HEADER_ID = d.DLV_HEADER_ID AND p.DLV_DETAIL_ID = d.DLV_DETAIL_ID
where h.DLV_HEADER_ID = @DLV_HEADER_ID
GROUP BY d.DLV_DETAIL_ID";

                var list = this.Context.Database.SqlQuery<int>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();
                if (list.Count == 0) //為0表示應揀量等於已揀量
                {
                    throw new Exception("找不到明細資料");
                }
                else
                {
                    foreach(var status in list)
                    {
                        if (status == 1)
                        {
                            return new ResultDataModel<string>(false, "已揀數量超過需求量", null);
                        }
                        else if (status == -1)
                        {
                            return new ResultDataModel<string>(true, "檢查是否揀畢成功", DeliveryStatusCode.UnPicked);
                        }
                        else
                        {
                            //此dtail為已揀，繼續檢查下一筆dtail
                        }
                    }
                    //全部detail為已揀，回傳已揀
                    return new ResultDataModel<string>(true, "檢查是否揀畢成功", DeliveryStatusCode.Picked);
                }
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<string>(false, "檢查是否揀畢失敗:" + ex.Message, null);
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
            var headerData = dlvHeaderTRepository.GetAll().AsNoTracking().Where(x => x.DlvHeaderId == dlvHeaderId).ToList();
            if (headerData.Count == 0) return new ResultModel(false, "無法取得交運單資料");

            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var addDate = DateTime.Now;
                    foreach (DLV_PICKED_T data in pickedDataList)
                    {
                        var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == data.Stock_Id);
                        if (stock == null) throw new Exception("找不到庫存資料");
                        STK_TXN_T stkTxnT = CreateStockRecord(stock, null, "", "", null, CategoryCode.Delivery, ActionCode.Deleted, headerData[0].DeliveryName);
                        decimal? priQty = data.PrimaryQuantity;
                        decimal? secQty = data.SecondaryQuantity;
                        var updaeStockResult = UpdateStock(stock, stkTxnT, ref priQty, ref secQty, pickSatus, PickStatus.Deleted, addUser, addDate, true);
                        if (!updaeStockResult.Success) throw new Exception(updaeStockResult.Msg);
                        dlvPickedTRepository.Delete(data);
                    }

                    stockTRepository.SaveChanges();
                    stkTxnTRepository.SaveChanges();
                    dlvPickedTRepository.SaveChanges();

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
                        data.DeliveryStatusCode = pickedResult.Data;
                        data.DeliveryStatusName = deliveryStatusCode.GetDesc(pickedResult.Data);
                        data.LastUpdateBy = addUser;
                        data.LastUpdateUserName = addUserName;
                        data.LastUpdateDate = addDate;
                        dlvHeaderTRepository.Update(data);
                    }
                    dlvHeaderTRepository.SaveChanges();
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
        }

        /// <summary>
        /// 取得航程號選單資料
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetTripNameDropDownList(DropDownListType type, string userId)
        {
            var tripNameList = createDropDownList(type);
            tripNameList.AddRange(getTripNameList(userId));
            return tripNameList;
        }

        /// <summary>
        /// 取得交運單狀態選單資料
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetDeliveryStatusDropDownList(DropDownListType type)
        {
            var deliveryStatusList = createDropDownList(type);
            deliveryStatusList.AddRange(getDeliveryStatusList());
            return deliveryStatusList;
        }

        /// <summary>
        /// 取得交運單狀態選單資料
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 取得航程號選單資料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<SelectListItem> getTripNameList(string userId)
        {
            var tripNameList = new List<SelectListItem>();
            try
            {
                //取得符合使用者倉庫365天內的航程號
                string cmd = @"
SELECT TOP 1000 CONVERT(varchar(10), t.TRIP_ID) as Value
,t.TRIP_NAME as Text
FROM DLV_HEADER_T t
JOIN USER_SUBINVENTORY_T ut on t.SUBINVENTORY_CODE = ut.SUBINVENTORY_CODE AND t.ORGANIZATION_ID = ut.ORGANIZATION_ID
WHERE 
ut.UserId = @userId AND
DateDiff(dd, t.CREATION_DATE ,getdate())<=365
GROUP BY t.TRIP_ID, t.TRIP_NAME
ORDER BY t.TRIP_ID DESC";

                tripNameList = this.Context.Database.SqlQuery<SelectListItem>(cmd,
                    new SqlParameter("@userId", userId)).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return tripNameList;
        }

        /// <summary>
        /// 取得出貨查詢表單資料
        /// </summary>
        public List<TripHeaderDT> DeliverySearch(string TripActualShipBeginDate, string TripActualShipEndDate, string DeliveryName, string SelectedSubinventory,
                    string SelectedTrip, string TransactionDate, string SelectedDeliveryStatus, string userId)
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
SELECT 
CONVERT(CHAR(10), AUTHORIZE_DATE,126) AS AUTHORIZE_DATE,
CUSTOMER_LOCATION_CODE,
CUSTOMER_NAME,
SHIP_CUSTOMER_NAME,
DELIVERY_NAME,
DELIVERY_STATUS_NAME AS DELIVERY_STATUS,
ITEM_CATEGORY AS DETAILTYPE,
FREIGHT_TERMS_NAME,
DLV_HEADER_ID AS ID,
NOTE,
h.SUBINVENTORY_CODE,
SHIP_CUSTOMER_NAME,
SHIP_LOCATION_CODE,
TRIP_ACTUAL_SHIP_DATE,
TRANSACTION_DATE,
TRIP_CAR,
TRIP_ID,
TRIP_NAME,
ROW_NUMBER() OVER(ORDER BY DLV_HEADER_ID) AS SUB_ID,
CASE WHEN (h.DELIVERY_STATUS_CODE = 'DH0' OR h.DELIVERY_STATUS_CODE = 'DH5' ) THEN
 (SELECT ISNULL(SUM(REQUESTED_PRIMARY_QUANTITY),0) FROM DLV_DETAIL_HT WHERE DLV_HEADER_ID = h.DLV_HEADER_ID) ELSE
 (SELECT ISNULL(SUM(REQUESTED_PRIMARY_QUANTITY),0) FROM DLV_DETAIL_T WHERE DLV_HEADER_ID = h.DLV_HEADER_ID)
 END AS RP_SUM,
CASE WHEN (h.DELIVERY_STATUS_CODE = 'DH0' OR h.DELIVERY_STATUS_CODE = 'DH5' ) THEN
 (SELECT TOP 1 REQUESTED_PRIMARY_UOM FROM DLV_DETAIL_HT WHERE DLV_HEADER_ID = h.DLV_HEADER_ID) ELSE
 (SELECT TOP 1 REQUESTED_PRIMARY_UOM FROM DLV_DETAIL_T WHERE DLV_HEADER_ID = h.DLV_HEADER_ID)
 END AS REQUESTED_PRIMARY_UOM,
CASE WHEN (h.DELIVERY_STATUS_CODE = 'DH0' OR h.DELIVERY_STATUS_CODE = 'DH5' ) THEN
(SELECT ISNULL(SUM(REQUESTED_SECONDARY_QUANTITY),0) FROM DLV_DETAIL_HT WHERE DLV_HEADER_ID = h.DLV_HEADER_ID) ELSE
(SELECT ISNULL(SUM(REQUESTED_SECONDARY_QUANTITY),0) FROM DLV_DETAIL_T WHERE DLV_HEADER_ID = h.DLV_HEADER_ID)
END AS RS_SUM,
CASE WHEN (h.DELIVERY_STATUS_CODE = 'DH0' OR h.DELIVERY_STATUS_CODE = 'DH5' ) THEN
(SELECT TOP 1 REQUESTED_SECONDARY_UOM FROM DLV_DETAIL_HT WHERE DLV_HEADER_ID = h.DLV_HEADER_ID) ELSE
(SELECT TOP 1 REQUESTED_SECONDARY_UOM FROM DLV_DETAIL_T WHERE DLV_HEADER_ID = h.DLV_HEADER_ID)
END AS REQUESTED_SECONDARY_UOM
FROM DLV_HEADER_T h
JOIN USER_SUBINVENTORY_T s ON s.SUBINVENTORY_CODE = h.SUBINVENTORY_CODE";

            cond.Add("s.UserId = @userId");
            sqlParameterList.Add(SqlParamHelper.GetNVarChar("@userId", userId));


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
            if (SelectedSubinventory != "全部")
            {
                cond.Add("h.SUBINVENTORY_CODE = @SelectedSubinventory");
                sqlParameterList.Add(SqlParamHelper.R.SubinventoryCode("@SelectedSubinventory", SelectedSubinventory));
            }
            else
            {
                var subinventoryCodeList = GetSubinventoryListForUser(userId);

                string temp = "";
                foreach (string subinventoryCode in subinventoryCodeList)
                {
                    temp += "'" + subinventoryCode + "'" + ',';
                }
                temp = temp.TrimEnd(',');
                temp = string.Format("h.SUBINVENTORY_CODE IN({0})", temp);
                cond.Add(temp);
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

            string commandText = $"{prefixCmd} WHERE {string.Join(" AND ", cond.ToArray())}";
            return this.Context.Database.SqlQuery<TripHeaderDT>(commandText, sqlParameterList.ToArray()).ToList();

        }

        /// <summary>
        /// 取不重複的TripId
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<long> GetTripIdList(List<long> dlvHeaderIds)
        {
            return dlvHeaderTRepository.GetAll().AsNoTracking().Where(x => dlvHeaderIds.Contains(x.DlvHeaderId)).GroupBy(x => x.TripId).Select(x => x.Key).ToList();
        }

        /// <summary>
        /// 取得出貨表頭資料
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<DLV_HEADER_T> GetDeliveryHeaderDataListFromTripId(List<long> tripIds)
        {
            return dlvHeaderTRepository.GetAll().AsNoTracking().Where(x => tripIds.Contains(x.TripId)).ToList();
        }

        /// <summary>
        /// 取得出貨表頭資料
        /// </summary>
        /// <param name="dlvHeaderIds"></param>
        /// <returns></returns>
        public List<DLV_HEADER_T> GetDeliveryHeaderDataListFromHeaderId(long dlvHeaderId)
        {
            return dlvHeaderTRepository.GetAll().AsNoTracking().Where(x => dlvHeaderId == x.DlvHeaderId).ToList();
        }

        /// <summary>
        /// 取得出貨揀貨資料
        /// </summary>
        /// <param name="dlvPickedId"></param>
        /// <returns></returns>
        public List<DLV_PICKED_T> GetDeliveryPickDataListFromPickedId(List<long> dlvPickedId)
        {
            return dlvPickedTRepository.GetAll().Where(x => dlvPickedId.Contains(x.DlvPickedId)).ToList();
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
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                        dlvHeaderTRepository.Update(data);
                    }
                    dlvHeaderTRepository.SaveChanges();
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
                        dlvHeaderTRepository.Update(data);

                        
                        var pickList = dlvPickedTRepository.GetAll().AsNoTracking().Where(x => x.DlvHeaderId == data.DlvHeaderId).ToList();
                        if (pickList == null || pickList.Count == 0) throw new Exception("找不到揀貨資料");

                        foreach(DLV_PICKED_T pick in pickList)
                        {
                            //更新庫存鎖定量
                            var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.Stock_Id);
                            if (stock == null) throw new Exception("找不到庫存資料");
                            STK_TXN_T stkTxnT = CreateStockRecord(stock, null, "", "", null, CategoryCode.Delivery, ActionCode.Shipped, data.DeliveryName);
                            var updateStockLockQtyResult = UpdateStockLockQty(stock, stkTxnT, -1 * pick.PrimaryQuantity, -1 * pick.SecondaryQuantity, pickSatus, PickStatus.Shipped, userId, now);
                            if (!updateStockLockQtyResult.Success) throw new Exception(updateStockLockQtyResult.Msg);
                        }
                        
                        //複製出貨明細資料到出貨歷史明細
                        string cmd = @"
INSERT INTO [DLV_DETAIL_HT]
(
	[DLV_DETAIL_ID]
      ,[DLV_HEADER_ID]
      ,[PROCESS_CODE]
      ,[SERVER_CODE]
      ,[BATCH_ID]
      ,[BATCH_LINE_ID]
      ,[Order_Number]
      ,[ORDER_LINE_ID]
      ,[ORDER_SHIP_NUMBER]
      ,[DELIVERY_DETAIL_ID]
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
      ,[PROCESS_CODE]
      ,[SERVER_CODE]
      ,[BATCH_ID]
      ,[BATCH_LINE_ID]
      ,[Order_Number]
      ,[ORDER_LINE_ID]
      ,[ORDER_SHIP_NUMBER]
      ,[DELIVERY_DETAIL_ID]
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

                    this.SaveChanges();
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
        /// 取消航程號
        /// </summary>
        public ResultModel CancelTrip(List<DLV_HEADER_T> updateDatas, string userId, string userName)
        {
            if (updateDatas == null || updateDatas.Count == 0) return new ResultModel(false, "沒有交運單資料");
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    foreach (DLV_HEADER_T header in updateDatas)
                    {
                        //更新出貨檔頭
                        header.DeliveryStatusCode = DeliveryStatusCode.Canceled;
                        header.DeliveryStatusName = deliveryStatusCode.GetDesc(DeliveryStatusCode.Canceled);
                        header.LastUpdateBy = userId;
                        header.LastUpdateUserName = userName;
                        header.LastUpdateDate = now;
                        dlvHeaderTRepository.Update(header);

                        //複製出貨明細資料到出貨歷史明細
                        string cmd = @"
INSERT INTO [DLV_DETAIL_HT]
(
	[DLV_DETAIL_ID]
      ,[DLV_HEADER_ID]
      ,[PROCESS_CODE]
      ,[SERVER_CODE]
      ,[BATCH_ID]
      ,[BATCH_LINE_ID]
      ,[Order_Number]
      ,[ORDER_LINE_ID]
      ,[ORDER_SHIP_NUMBER]
      ,[DELIVERY_DETAIL_ID]
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
      ,[PROCESS_CODE]
      ,[SERVER_CODE]
      ,[BATCH_ID]
      ,[BATCH_LINE_ID]
      ,[Order_Number]
      ,[ORDER_LINE_ID]
      ,[ORDER_SHIP_NUMBER]
      ,[DELIVERY_DETAIL_ID]
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
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@DLV_HEADER_ID", header.DlvHeaderId)) <= 0)
                        {
                            throw new Exception("複製出貨明細資料到出貨歷史明細失敗");
                        }
                        //刪除出貨明細資料
                        cmd = @"
  DELETE FROM [DLV_DETAIL_T]
  WHERE DLV_HEADER_ID = @DLV_HEADER_ID";
                        if (this.Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("@DLV_HEADER_ID", header.DlvHeaderId)) <= 0)
                        {
                            throw new Exception("刪除出貨明細資料失敗");
                        }

                        var pickList = dlvPickedTRepository.GetAll().Where(x => x.DlvHeaderId == header.DlvHeaderId).ToList();
                        if (pickList != null && pickList.Count > 0)
                        {
                            foreach (DLV_PICKED_T pick in pickList)
                            {
                                //還原庫存
                                var stock = stockTRepository.GetAll().FirstOrDefault(x => x.StockId == pick.Stock_Id);
                                if (stock == null) throw new Exception("找不到庫存資料");

                                STK_TXN_T stkTxnT = CreateStockRecord(stock, null, "", "", null, CategoryCode.Delivery, ActionCode.Deleted, header.DeliveryName);
                                decimal? priQty = pick.PrimaryQuantity;
                                decimal? secQty = pick.SecondaryQuantity;
                                var updaeStockResult = UpdateStock(stock, stkTxnT, ref priQty, ref secQty, pickSatus, PickStatus.Deleted, userId, now, true);
                                if (!updaeStockResult.Success) throw new Exception(updaeStockResult.Msg);
                                dlvPickedTRepository.Delete(pick);
                            }
                        }
                    }

                    this.SaveChanges();
                    txn.Commit();
                    return new ResultModel(true, "取消航程號成功");
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    txn.Rollback();
                    return new ResultModel(false, "取消航程號失敗:" + ex.Message);
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
                        data.LastUpdateBy = userId;
                        data.LastUpdateUserName = userName;
                        data.LastUpdateDate = now;
                        dlvHeaderTRepository.Update(data);
                    }
                    dlvHeaderTRepository.SaveChanges();
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
            using (var txn = this.Context.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    foreach (var selectData in selectDatas.TripDetailDTList)
                    {
                        var updateDatas = dlvHeaderTRepository.GetAll().AsNoTracking().Where(x => x.TripId == selectData.TRIP_ID).ToList();
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
                            dlvHeaderTRepository.Update(data);
                        }
                    }
                    dlvHeaderTRepository.SaveChanges();
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
        /// 取得紙捲明細表單資料
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

        /// <summary>
        /// 取得紙捲揀貨表單資料
        /// </summary>
        /// <param name="dlvHeaderId"></param>
        /// <param name="DELIVERY_STATUS_NAME"></param>
        /// <returns></returns>
        public List<PaperRollEditBarcodeDT> GetRollPickDT(long dlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            string cmd = "";
            if (DELIVERY_STATUS_NAME == deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped))
            {
                cmd = @"
select
DLV_PICKED_ID as PICKED_ID,
ROW_NUMBER() OVER(ORDER BY DLV_PICKED_ID) AS SUB_ID,
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
ROW_NUMBER() OVER(ORDER BY DLV_PICKED_ID) AS SUB_ID,
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
        /// 取得平張明細表單資料
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
            return this.Context.Database.SqlQuery<FlatEditDT>(cmd, new SqlParameter("@DLV_HEADER_ID", dlvHeaderId)).ToList();
        }

        /// <summary>
        /// 取得平張揀貨表單資料
        /// </summary>
        /// <param name="dlvHeaderId"></param>
        /// <param name="DELIVERY_STATUS_NAME"></param>
        /// <returns></returns>
        public List<FlatEditBarcodeDT> GetFlatPickDT(long dlvHeaderId, string DELIVERY_STATUS_NAME)
        {
            string cmd = "";
            if (DELIVERY_STATUS_NAME == deliveryStatusCode.GetDesc(DeliveryStatusCode.Shipped))
            {
                cmd = @"
select 
DLV_PICKED_ID as PICKED_ID,
ROW_NUMBER() OVER(ORDER BY DLV_PICKED_ID) AS SUB_ID,
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
ROW_NUMBER() OVER(ORDER BY DLV_PICKED_ID) AS SUB_ID,
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

        /// <summary>
        /// 取得條碼標籤資料
        /// </summary>
        /// <param name="PICKED_IDs"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GetLabels(List<long> PICKED_IDs, string userName)
        {
            try
            {
                List<LabelModel> labelModelList = new List<LabelModel>();
                if (PICKED_IDs == null || PICKED_IDs.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                var pickDataList = dlvPickedTRepository.GetAll().AsNoTracking().Where(x => PICKED_IDs.Contains(x.DlvPickedId)).ToList();
                if (pickDataList == null || pickDataList.Count == 0) return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                if (pickDataList.Count != PICKED_IDs.Count) throw new Exception("找不到部分揀貨資料");
                foreach (DLV_PICKED_T pick in pickDataList)
                {
                    var detail = dlvDetailTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.DlvHeaderId == pick.DlvHeaderId && x.DlvDetailId == pick.DlvDetailId);
                    if (detail == null) return new ResultDataModel<List<LabelModel>>(false, "找不到明細資料", null);

                    StringBuilder cmd = new StringBuilder(@"
SELECT p.BARCODE as Barocde
,@userName as PrintBy
,s.ITEM_DESCRIPTION as BarocdeName
,s.PAPER_TYPE as PapaerType
,s.BASIC_WEIGHT as BasicWeight
,s.SPECIFICATION as Specification
,s.OSP_BATCH_NO as OspBatchNo
,s.LOT_NUMBER as LotNumber
");

                    if (pick.PalletStatus == PalletStatusCode.Split) //判斷是否拆板
                    {
                        if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
                        {
                            //拆板 平版 數量為庫存數量(拆板後的剩餘數量)
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(s.SECONDARY_AVAILABLE_QTY,'0.##########') as Qty
FROM [DLV_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else if (detail.ItemCategory == ItemCategory.Roll)
                        {
                            //拆板 捲筒
                            return new ResultDataModel<List<LabelModel>>(false, "捲筒不能拆板", null);
                        }
                        else
                        {
                            throw new Exception("無法識別貨品類別");
                        }
                    }
                    else if (pick.PalletStatus == PalletStatusCode.All) //判斷是否整版
                    {
                        if (detail.ItemCategory == ItemCategory.Flat) //判斷是否為平版
                        {
                            //整板 平版 數量為揀貨的數量
                            cmd.Append(@"
,s.SECONDARY_UOM_CODE as Unit
,FORMAT(p.SECONDARY_QUANTITY,'0.##########') as Qty
FROM [DLV_PICKED_T] p
INNER JOIN STOCK_T s ON p.BARCODE = s.BARCODE
WHERE p.BARCODE = @Barcode
");
                        }
                        else if (detail.ItemCategory == ItemCategory.Roll)
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
                        else
                        {
                            throw new Exception("無法識別貨品類別");
                        }
                    }
                    else
                    {
                        //棧板狀態為併板 在出貨不會遇到
                        throw new Exception("出貨棧板狀態不可為併板");
                    }

                    var labelModel = this.Context.Database.SqlQuery<LabelModel>(cmd.ToString(), new SqlParameter("@userName", userName), new SqlParameter("@Barcode", pick.Barcode)).ToList();
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

        /// <summary>
        /// 取得出貨備貨單ReportDataSource
        /// </summary>
        /// <param name="tripName"></param>
        /// <returns></returns>
        public ResultDataModel<ReportDataSource> GetPickingListReportDataSource(string tripName)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MesContext"].ConnectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    ReportDataSource dataSource = new ReportDataSource();
                    DataSet dataset = new DataSet("dataset");
                    string cmd = "SELECT * From DeliveryPickingList(@TRIP_NAME)";
                    SqlCommand command = new SqlCommand(cmd, connection);
                    command.Parameters.Add(new SqlParameter("@TRIP_NAME", tripName));
                    SqlDataAdapter salesOrderAdapter = new SqlDataAdapter(command);
                    salesOrderAdapter.Fill(dataset, "Detail");
                    dataSource.Name = "Detail";
                    dataSource.Value = dataset.Tables["Detail"];

                    connection.Close();
                    return new ResultDataModel<ReportDataSource>(true, "取得備貨單報表資料來源成功", dataSource);
                }
                catch (Exception ex)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(ex));
                    return new ResultDataModel<ReportDataSource>(false, "取得備貨單報表資料來源失敗:" + ex.Message, null);
                }
            }

        }

        /// <summary>
        /// 取得出貨未完成數量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<int> GetDlvPendingCount(string userId)
        {
            var resultDataModel = new ResultDataModel<int>(false, "", 0);
            try
            {
                var subinventoryList = GetSubinventoryListForUser(userId);
                if (subinventoryList == null || subinventoryList.Count == 0)
                {
                    throw new Exception("找不到使用者倉庫");
                }

                resultDataModel.Data = dlvHeaderTRepository.GetAll()
                .Where(x => 
                    x.DeliveryStatusCode != DeliveryStatusCode.Canceled
                    && x.DeliveryStatusCode != DeliveryStatusCode.Shipped
                    && subinventoryList.Contains(x.SubinventoryCode)
                    )
                .Count();
                resultDataModel.Code = ResultModel.CODE_SUCCESS;
                resultDataModel.Msg = "";
            }
            catch (Exception ex)
            {
                resultDataModel.Code = -1;
                resultDataModel.Msg = $"取得出貨未完成數量時發生錯誤 EX:{ex.Message}";
            }

            return resultDataModel;
        }

    }
}