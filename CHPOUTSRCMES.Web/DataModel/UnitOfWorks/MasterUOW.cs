using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Util;
using NLog;
using NPOI.SS.UserModel;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Models;
using System.Data.SqlClient;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using static CHPOUTSRCMES.Web.DataModel.UnitOfWorks.DeliveryUOW;
using System.IO;
using NPOI.OpenXml4Net.OPC.Internal;
using CHPOUTSRCMES.Web.ViewModels;
using System.Text;
using CHPOUTSRCMES.Web.Jsons.Requests;
using System.Web;
using System.Drawing.Imaging;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.Configuration;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class MasterUOW : UnitOfWork
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 料號
        /// </summary>
        public readonly IItemsTRepository itemsTRepository;
        /// <summary>
        /// 組織料號
        /// </summary>
        public readonly IRepository<ORG_ITEMS_T> orgItemRepository;
        /// <summary>
        /// 組織
        /// </summary>
        public readonly IRepository<ORG_UNIT_T> orgUnitRepository;
        /// <summary>
        /// 組織
        /// </summary>
        public readonly IRepository<ORGANIZATION_T> organizationRepository;
        /// <summary>
        /// 倉庫
        /// </summary>
        public readonly IRepository<SUBINVENTORY_T> subinventoryRepository;
        /// <summary>
        /// 儲位
        /// </summary>
        public readonly IRepository<LOCATOR_T> locatorTRepository;
        /// <summary>
        /// 餘切規格
        /// </summary>
        public readonly IRepository<RELATED_T> relatedTRepository;
        /// <summary>
        /// 令重包數
        /// </summary>
        public readonly IRepository<YSZMPCKQ_T> yszmpckqTRepository;
        /// <summary>
        /// 機台紙別
        /// </summary>
        public readonly IRepository<MACHINE_PAPER_TYPE_T> machinePaperTypeRepository;
        /// <summary>
        /// 庫存交易類別
        /// </summary>
        public readonly IRepository<TRANSACTION_TYPE_T> transactionTypeRepository;
        /// <summary>
        /// 條碼設定類別
        /// </summary>
        public readonly IRepository<BCD_MISC_T> bcdMiscRepository;
        /// <summary>
        /// 原因
        /// </summary>
        public readonly IRepository<STK_REASON_T> stkReasonTRepository;
        /// <summary>
        /// 庫存
        /// </summary>
        public readonly IRepository<STOCK_T> stockTRepository;
        /// <summary>
        /// 庫存歷史
        /// </summary>
        public readonly IRepository<STOCK_HT> stockHtRepository;
        /// <summary>
        /// 異動記錄
        /// </summary>
        public readonly IRepository<STK_TXN_T> stkTxnTRepository;

        /// <summary>
        /// 使用者
        /// </summary>
        public readonly IRepository<AppUser> appUserRepository;

        /// <summary>
        /// 使用者
        /// </summary>
        public readonly IRepository<USER_SUBINVENTORY_T> userSubinventoryTRepository;

        public IUomConversion uomConversion;

        public CategoryCode categoryCode = new CategoryCode();

        public ActionCode actionCode = new ActionCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public MasterUOW(DbContext context) : base(context)
        {
            this.orgUnitRepository = new GenericRepository<ORG_UNIT_T>(this);
            this.itemsTRepository = new ItemsTRepository(this);
            this.orgItemRepository = new GenericRepository<ORG_ITEMS_T>(this);
            this.organizationRepository = new GenericRepository<ORGANIZATION_T>(this);
            this.subinventoryRepository = new GenericRepository<SUBINVENTORY_T>(this);
            this.locatorTRepository = new GenericRepository<LOCATOR_T>(this);
            this.relatedTRepository = new GenericRepository<RELATED_T>(this);
            this.yszmpckqTRepository = new GenericRepository<YSZMPCKQ_T>(this);
            this.machinePaperTypeRepository = new GenericRepository<MACHINE_PAPER_TYPE_T>(this);
            this.transactionTypeRepository = new GenericRepository<TRANSACTION_TYPE_T>(this);
            this.bcdMiscRepository = new GenericRepository<BCD_MISC_T>(this);
            this.stkReasonTRepository = new GenericRepository<STK_REASON_T>(this);
            this.stockTRepository = new GenericRepository<STOCK_T>(this);
            this.stockHtRepository = new GenericRepository<STOCK_HT>(this);
            this.stkTxnTRepository = new GenericRepository<STK_TXN_T>(this);
            this.appUserRepository = new GenericRepository<AppUser>(this);
            this.userSubinventoryTRepository = new GenericRepository<USER_SUBINVENTORY_T>(this);
            this.uomConversion = new UomConversion();
        }


        /// <summary>
        /// 異動型態ID
        /// </summary>
        public class TransactionTypeId
        {
            /// <summary>
            /// Intransit Shipment 組織間移轉
            /// </summary>
            public const long IntransitShipment = 21;
            /// <summary>
            /// CHP-16庫存調整(出) 盤虧
            /// </summary>
            public const long Chp16Out = 355;
            /// <summary>
            /// CHP-16庫存調整(入) 盤盈
            /// </summary>
            public const long Chp16In = 356;
            /// <summary>
            /// CHP-26存貨報廢(出) 存貨報廢
            /// </summary>
            public const long Chp26Out = 370;
            /// <summary>
            /// CHP-30倉儲移轉 倉庫移轉
            /// </summary>
            public const long Chp30 = 375;
            /// <summary>
            /// CHP-37出貨數尾差調整(出) 雜發
            /// </summary>
            public const long Chp37Out = 440;
            /// <summary>
            /// CHP-37出貨數尾差調整(入) 雜收
            /// </summary>
            public const long Chp37In = 441;

        }

        /// <summary>
        /// 使用者角色
        /// </summary>
        public class UserRole
        {
            public const string Adm = "系統管理員";
            public const string ChpUser = "華紙使用者";
            public const string User = "使用者";
        }

        /// <summary>
        /// 棧板狀態
        /// </summary>
        public class PalletStatusCode : ICategory
        {
            /// <summary>
            /// 整板
            /// </summary>
            public const string All = "0";
            /// <summary>
            /// 拆板
            /// </summary>
            public const string Split = "1";
            /// <summary>
            /// 併板
            /// </summary>
            public const string Merge = "2";

            public string GetDesc(string palletStatusCode)
            {
                switch (palletStatusCode)
                {
                    case All:
                        return "整板";
                    case Split:
                        return "拆板";
                    case Merge:
                        return "併板";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 儲位控制
        /// </summary>
        public class LocatorType
        {
            /// <summary>
            /// 使用儲位
            /// </summary>
            public const long Used = 2;
            /// <summary>
            /// 無儲位
            /// </summary>
            public const long NotUsed = 1;
        }

        public class PackingType
        {
            public const string Ream = "令包";

            public const string NoReam = "無令打件";
        }

        /// <summary>
        /// 加工廠註記
        /// </summary>
        public class OspFlag
        {
            public const string IsProcessingPlant = "Y";
        }

        /// <summary>
        /// 控制欄位
        /// </summary>
        public class ControlFlag
        {
            public const string Deleted = "D";
        }

        /// <summary>
        /// 入庫揀貨狀態
        /// </summary>
        public class InboundStatus
        {
            /// <summary>
            /// 待列印
            /// </summary>
            public const string WaitPrint = "0";
            /// <summary>
            /// 待入庫
            /// </summary>
            public const string WaitInbound = "1";
            /// <summary>
            /// 已入庫
            /// </summary>
            public const string AlreadyInbound = "2";
        }

        public class ItemCategory
        {
            /// <summary>
            /// 捲筒
            /// </summary>
            public const string Roll = "捲筒";
            /// <summary>
            /// 平版
            /// </summary>
            public const string Flat = "平版";
        }

        /// <summary>
        /// 儲位狀態
        /// </summary>
        public class LocatorStatusCode
        {
            /// <summary>
            /// 可出貨
            /// </summary>
            public const string CanShip = "有效";
            /// <summary>
            /// 不可出貨
            /// </summary>
            public const string NotShip = "Active-no Ship";
        }

        /// <summary>
        /// 庫存異動記錄 Category
        /// </summary>
        public class CategoryCode : ICategory
        {
            public const string Delivery = "C0";
            public const string Process = "C1";
            public const string Purchase = "C2";
            public const string TransferInbound = "C3";
            public const string TransferOutbound = "C4";
            public const string TransferReason = "C5";
            public const string MiscellaneousIn = "C6";
            public const string MiscellaneousOut = "C7";
            public const string Obsolete = "C8";
            public const string InventoryProfit = "C9";
            public const string InventoryLoss = "C10";



            public string GetDesc(string category)
            {
                switch (category)
                {
                    case Delivery:
                        return "出貨";
                    case Process:
                        return "加工";
                    case Purchase:
                        return "進貨";
                    case TransferInbound:
                        return "庫存移轉-入庫";
                    case TransferOutbound:
                        return "庫存移轉-出庫";
                    case TransferReason:
                        return "庫存移轉-貨故";
                    case MiscellaneousIn:
                        return "雜項異動-雜收";
                    case MiscellaneousOut:
                        return "雜項異動-雜發";
                    case Obsolete:
                        return "存貨報廢";
                    case InventoryProfit:
                        return "盤點-盤盈";
                    case InventoryLoss:
                        return "盤點-盤虧";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 庫存異動記錄 ACTION
        /// </summary>
        public class ActionCode : IAction
        {
            /// <summary>
            /// 刪除
            /// </summary>
            public const string Deleted = "A0";
            /// <summary>
            /// 已揀
            /// </summary>
            public const string Picked = "A1";
            /// <summary>
            /// 已出貨
            /// </summary>
            public const string Shipped = "A2";
            public const string Purchse = "A3";

            public const string StockTransfer = "A4";

            public const string ProcessStored = "A5";

            public const string ProcessPicked = "A6";

            public const string ProcessEditStored = "A7";

            public const string ProcessEditDeleted = "A8";

            public string GetDesc(string category)
            {
                switch (category)
                {
                    case Deleted:
                        return "刪除";
                    case Picked:
                        return "已揀";
                    case Shipped:
                        return "已出貨";
                    case Purchse:
                        return "進貨";
                    case StockTransfer:
                        return "庫存異動";
                    case ProcessStored:
                        return "加工入庫";
                    case ProcessPicked:
                        return "加工領料";
                    case ProcessEditStored:
                        return "加工修改入庫";
                    case ProcessEditDeleted:
                        return "加工修改扣帳";
                    default:
                        return "";
                }
            }
        }

        #region 庫存異動

        /// <summary>
        /// 取得庫存移轉型態
        /// </summary>
        public string GetTransferCatalog(long outOrganizationId, long inOrganizationId)
        {
            if (outOrganizationId == inOrganizationId)
            {
                return TransferCatalog.InvTransfer;
            }
            else
            {
                return TransferCatalog.OrgTransfer;
            }
        }

        public class TransferCatalog
        {
            /// <summary>
            /// 組織間移轉
            /// </summary>
            public const string OrgTransfer = "ORG";
            /// <summary>
            /// 倉庫間移轉
            /// </summary>
            public const string InvTransfer = "INV";
        }

        /// <summary>
        /// 取得出貨編號Guid
        /// </summary>
        /// <returns></returns>
        public string GetShipmentNumberGuid()
        {
            return Guid.NewGuid().ToString();
        }


        /// <summary>
        /// 取得異動型態
        /// </summary>
        /// <param name="transactionTypeId"></param>
        /// <returns></returns>
        public TRANSACTION_TYPE_T GetTransactionType(long transactionTypeId)
        {
            return transactionTypeRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.TransactionTypeId == transactionTypeId && x.ControlFlag != ControlFlag.Deleted);
        }

        /// <summary>
        /// 出貨編號狀態
        /// </summary>
        public class NumberStatus
        {
            /// <summary>
            /// 未存檔
            /// </summary>
            public const string NotSaved = "0";
            /// <summary>
            /// 已存檔
            /// </summary>
            public const string Saved = "1";
        }

        /// <summary>
        /// 此筆資料使否傳給ERP
        /// </summary>
        public class ToErp
        {
            /// <summary>
            /// 否; 一般入庫
            /// </summary>
            public const string No = "0";
            /// <summary>
            /// 是; 庫存異動
            /// </summary>
            public const string Yes = "1";
        }

        #endregion


        #region 庫存
        

        /// <summary>
        /// 庫存狀態
        /// </summary>
        public class StockStatusCode : IStatus
        {
            /// <summary>
            /// 在庫
            /// </summary>
            public const string InStock = "S0";
            /// <summary>
            /// 出貨已揀
            /// </summary>
            public const string DeliveryPicked = "S1";
            /// <summary>
            /// 已出貨
            /// </summary>
            public const string Shipped = "S2";

            /// <summary>
            /// 加工領料
            /// </summary>
            public const string ProcessPicked = "S3";
            /// <summary>
            /// 庫存異動至沒庫存
            /// </summary>
            public const string TransferNoneInStock = "S4";


            public string GetDesc(string statusCode)
            {
                switch (statusCode)
                {
                    case InStock:
                        return "在庫";
                    case DeliveryPicked:
                        return "出貨已揀";
                    case Shipped:
                        return "已出貨";
                    case TransferNoneInStock:
                        return "庫存異動至沒庫存";
                    default:
                        return "";
                }
            }

        }

        /// <summary>
        /// 取得庫存資料
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public STOCK_T GetStock(string barcode)
        {
            return stockTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.Barcode == barcode);
        }

        /// <summary>
        /// 共用庫存的倉庫儲位檢查
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="now"></param>
        /// <param name="subinventory"></param>
        /// <param name="locator"></param>
        /// <returns></returns>
        public ResultModel CommonCheckStockLocation(STOCK_T stock, DateTime now, SUBINVENTORY_T subinventory, LOCATOR_T locator)
        {
            if (stock == null) return new ResultModel(false, "沒有庫存資料");
            //庫存的倉庫檢查

            if (subinventory == null) return new ResultModel(false, "沒有庫存所屬倉庫" + stock.SubinventoryCode + "資料");
            if (subinventory.OspFlag != OspFlag.IsProcessingPlant) return new ResultModel(false, "庫存所屬倉庫" + subinventory.SubinventoryCode + "須為加工廠");
            if (subinventory.ControlFlag == ControlFlag.Deleted) return new ResultModel(false, "庫存所屬倉庫" + subinventory.SubinventoryCode + "已刪除");
            if (subinventory.LocatorType != LocatorType.Used)
            {
                return new ResultModel(true, "庫存所屬倉庫" + subinventory.SubinventoryCode + "檢查成功");
            }
            else
            {
                //庫存的儲位檢查
                if (locator == null) return new ResultModel(false, "沒有庫存所屬儲位" + stock.LocatorSegments + "資料");
                if (locator.ControlFlag == ControlFlag.Deleted) return new ResultModel(false, "庫存所屬儲位" + locator.LocatorSegments + "已刪除");
                if (locator.LocatorDisableDate != null && locator.LocatorDisableDate <= now) return new ResultModel(false, "庫存所屬儲位" + locator.LocatorSegments + "不可使用");
                return new ResultModel(true, "庫存所屬儲位" + locator.LocatorSegments + "檢查成功");
            }
        }

        /// <summary>
        /// 出貨庫存數量檢查
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="qty">令包數量(次單位)</param>
        /// <returns></returns>
        public ResultDataModel<STOCK_T> CheckStockQty(string barcode, decimal? qty)
        {
            var stock = stockTRepository.GetAll().FirstOrDefault(x => x.Barcode == barcode);

            if (stock == null)
            {
                return new ResultDataModel<STOCK_T>(false, "查無庫存", stock);
            }

            if (qty != null && stock.ItemCategory == ItemCategory.Flat && stock.PackingType == PackingType.Ream) //判斷是否為拆板(拆板會有令包數量且包裝方式為令包)
            {
                //為拆板，以qty檢查庫存量
                if (stock.StatusCode != StockStatusCode.InStock)
                {
                    return new ResultDataModel<STOCK_T>(false, "不在庫", stock);
                }
                if (stock.SecondaryAvailableQty + qty >= 0) //揀貨時為負qty，刪除時為正qty。刪除時不須檢查庫存
                {
                    return new ResultDataModel<STOCK_T>(true, "庫存足夠", stock);
                }
                else
                {
                    return new ResultDataModel<STOCK_T>(false, "庫存量不足", stock);
                }
            }
            else
            {
                //非拆板(沒有令包數量)
                if (stock.StatusCode != StockStatusCode.InStock)
                {
                    return new ResultDataModel<STOCK_T>(false, "不在庫", null);
                }
                if (stock.ItemCategory == ItemCategory.Roll && stock.PrimaryAvailableQty == 0)
                {
                    return new ResultDataModel<STOCK_T>(false, "沒有庫存量", null);
                }
                if (stock.ItemCategory == ItemCategory.Flat && stock.SecondaryAvailableQty == 0)
                {
                    return new ResultDataModel<STOCK_T>(false, "沒有庫存量", null);
                }
                if (stock.ItemCategory != ItemCategory.Roll && stock.ItemCategory != ItemCategory.Flat)
                {
                    return new ResultDataModel<STOCK_T>(false, "無法識別貨品類別", null);
                }
                return new ResultDataModel<STOCK_T>(true, "有庫存", stock);
            }
        }

        /// <summary>
        /// 出貨庫存檢查
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="qty"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public ResultDataModel<STOCK_T> DeliveryCheckStock(string barcode, decimal? qty, DateTime now)
        {
            var checkStockQtyResult = CheckStockQty(barcode, qty);

            if (!checkStockQtyResult.Success) return checkStockQtyResult;


            var stock = checkStockQtyResult.Data;
            var subinventoryList = subinventoryRepository.GetAll().AsNoTracking().Where(x =>
            x.OrganizationId == stock.OrganizationId &&
            x.SubinventoryCode == stock.SubinventoryCode).ToList();


            if (subinventoryList.Count == 0) return new ResultDataModel<STOCK_T>(false, "找不到庫存所屬倉庫" + stock.SubinventoryCode + "資料", stock);

            List<LOCATOR_T> locatorList = null;

            if (subinventoryList[0].LocatorType == LocatorType.Used)
            {
                locatorList = locatorTRepository.GetAll().AsNoTracking().Where(x =>
                x.OrganizationId == stock.OrganizationId &&
                x.SubinventoryCode == stock.SubinventoryCode &&
                x.LocatorId == stock.LocatorId).ToList();
            }

            if (locatorList == null || locatorList.Count == 0) return new ResultDataModel<STOCK_T>(false, "找不到庫存所屬儲位" + stock.LocatorSegments + "資料", stock);

            if (locatorList[0].LocatorStatusCode != LocatorStatusCode.CanShip) return new ResultDataModel<STOCK_T>(false, "庫存所屬儲位" + locatorList[0].LocatorSegments + "狀態不是有效", stock);

            var commonCheckStockLocationResult = CommonCheckStockLocation(stock, now, subinventoryList[0], locatorList[0]);

            return new ResultDataModel<STOCK_T>(commonCheckStockLocationResult.Success, commonCheckStockLocationResult.Msg, stock);

        }

        /// <summary>
        /// 更新庫存鎖定量及狀態
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="stkTxnT"></param>
        /// <param name="addPriLockQty"></param>
        /// <param name="addSecLockQty"></param>
        /// <param name="detail"></param>
        /// <param name="statusCode"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="addDate"></param>
        /// <returns></returns>
        public ResultDataModel<STOCK_T> UpdateStockLockQty(STOCK_T stock, STK_TXN_T stkTxnT, decimal addPriLockQty, decimal? addSecLockQty, IDetail detail, string statusCode, string lastUpdatedBy, DateTime now)
        {
            try
            {
                stock.PrimaryLockedQty = stock.PrimaryLockedQty.HasValue ? stock.PrimaryLockedQty + addPriLockQty : 0 + addPriLockQty;
                stock.SecondaryLockedQty = stock.SecondaryLockedQty.HasValue ? stock.SecondaryLockedQty + addSecLockQty : 0 + addSecLockQty;

                stock.StatusCode = stock.PrimaryAvailableQty == 0 ? detail.ToStockStatus(statusCode) : StockStatusCode.InStock;

                stock.LastUpdateBy = lastUpdatedBy;
                stock.LastUpdateDate = now;
                stkTxnT.PryAftQty = stock.PrimaryAvailableQty;
                stkTxnT.SecAftQty = stock.SecondaryAvailableQty;
                stkTxnT.CreatedBy = lastUpdatedBy;
                stkTxnT.CreationDate = now;
                stkTxnT.LastUpdateBy = null;
                stkTxnT.LastUpdateDate = null;
                stkTxnT.StatusCode = stock.StatusCode;

                stockTRepository.Update(stock);
                stkTxnTRepository.Create(stkTxnT);
                return new ResultDataModel<STOCK_T>(true, "庫存鎖定量更新成功", stock);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<STOCK_T>(false, "庫存鎖定量更新失敗:" + ex.Message, null);
            }

        }

        /// <summary>
        /// 更新庫存量及狀態
        /// </summary>
        /// <param name="stock">庫存</param>
        /// <param name="stkTxnT">異動記錄</param>
        /// <param name="priQty">主單位異動量</param>
        /// <param name="secQty">次單位異動量</param>
        /// <param name="detail">作業狀態轉換介面</param>
        /// <param name="statusCode">作業狀態碼</param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="addDate"></param>
        /// <param name="lockQty">鎖單量，揀貨用</param>
        /// <returns>更新後庫存</returns>
        public ResultDataModel<STOCK_T> UpdateStock(STOCK_T stock, STK_TXN_T stkTxnT, ref decimal? priQty, ref decimal? secQty, IDetail detail, string statusCode, string lastUpdatedBy, DateTime addDate, bool lockQty = false)
        {

            //主要單位及次要單位 其一必須有值
            if (!priQty.HasValue && !secQty.HasValue)
            {
                return new ResultDataModel<STOCK_T>(-1, "主要單位及次要單位兩者其中之一必須要有值!!", null);
            }

            //捲筒主要單位計算必須為公斤
            //平版為次要單位(雜項異動例外為主單位，因此不判斷)
            if (stock.isRoll() && (!priQty.HasValue || priQty.Value == 0))
            {
                return new ResultDataModel<STOCK_T>(-2, $"{stock.ItemCategory}的主要單位必須要有值!!", null);
            }

            //雜項異動處理 平版+主要單位，先換成次單位
            if (!stock.isRoll() && priQty.HasValue && !secQty.HasValue)
            {
                var model = uomConversion.Convert(stock.InventoryItemId, priQty.Value, stock.PrimaryUomCode, stock.SecondaryUomCode);

                if (!model.Success) return new ResultDataModel<STOCK_T>(-3, model.Msg, null);

                secQty = model.Data;
            }

            //平版次要單位計算必須有值
            if (!stock.isRoll() && (!secQty.HasValue || secQty.Value == 0))
            {
                return new ResultDataModel<STOCK_T>(-2, $"{stock.ItemCategory}的次要單位必須要有值!!", null);
            }

            //平版 + 次單位，先換算主單位
            if (!stock.isRoll() && secQty.HasValue && !priQty.HasValue)
            {
                var model = uomConversion.Convert(stock.InventoryItemId, secQty.Value, stock.SecondaryUomCode, stock.PrimaryUomCode);

                if (!model.Success) return new ResultDataModel<STOCK_T>(-4, model.Msg, null);

                priQty = model.Data;
            }

            //計算主單位數量
            var pryBeforeValue = stock.PrimaryAvailableQty;
            var pryAfterValue = pryBeforeValue + priQty.Value;

            stock.PrimaryAvailableQty = pryAfterValue;
            //是揀貨時 計算鎖單量
            if (lockQty) stock.PrimaryLockedQty = stock.PrimaryLockedQty.HasValue ? stock.PrimaryLockedQty + (-1 * priQty.Value) : 0 + (-1 * priQty.Value); //如果是原鎖定量是null要改為0，否則相加後仍為null

            //記錄異動表
            stkTxnT.PryChgQty = priQty;
            stkTxnT.PryBefQty = pryBeforeValue;
            stkTxnT.PryAftQty = pryAfterValue;

            //平版次單位計算
            if (!stock.isRoll() && secQty.HasValue)
            {
                var secBeforeValue = stock.SecondaryAvailableQty;
                var secAfterValue = secBeforeValue + secQty.Value;

                stock.SecondaryAvailableQty = secAfterValue;
                //是揀貨時 計算鎖單量
                if (lockQty) stock.SecondaryLockedQty = stock.SecondaryLockedQty.HasValue ? stock.SecondaryLockedQty + (-1 * secQty.Value) : 0 + (-1 * secQty.Value);

                stkTxnT.SecChgQty = secQty;
                stkTxnT.SecBefQty = secBeforeValue;
                stkTxnT.SecAftQty = secAfterValue;
            }

            //記錄異動人員
            stock.LastUpdateBy = lastUpdatedBy;
            stock.LastUpdateDate = addDate;
            //記錄庫存狀態
            stock.StatusCode = stock.PrimaryAvailableQty == 0 ? detail.ToStockStatus(statusCode) : StockStatusCode.InStock; //數量為0時為指定的庫存狀態，非0時為在庫
            stkTxnT.CreatedBy = lastUpdatedBy;
            stkTxnT.CreationDate = addDate;
            stkTxnT.LastUpdateBy = null;
            stkTxnT.LastUpdateDate = null;
            stkTxnT.StatusCode = stock.StatusCode;

            stockTRepository.Update(stock);
            stkTxnTRepository.Create(stkTxnT);

            return new ResultDataModel<STOCK_T>(true, "庫存更新成功", stock);
        }


        public STK_TXN_T CreateStockRecord(STOCK_T stock, long? dstOrganizationId, string dstOrganizationCode, string dstSubinventoryCode, long? dstLocatorId, string categoryCode, string actionCode, string doc)
        {
            return new STK_TXN_T
            {
                StockId = stock.StockId,
                OrganizationId = stock.OrganizationId,
                OrganizationCode = stock.OrganizationCode,
                SubinventoryCode = stock.SubinventoryCode,
                LocatorId = stock.LocatorId,
                Barcode = stock.Barcode,
                InventoryItemId = stock.InventoryItemId,
                ItemNumber = stock.ItemNumber,
                ItemDescription = stock.ItemDescription,
                ItemCategory = stock.ItemCategory,
                DstOrganizationId = dstOrganizationId,
                DstOrganizationCode = dstOrganizationCode,
                DstSubinventoryCode = dstSubinventoryCode,
                DstLocatorId = dstLocatorId,
                Category = this.categoryCode.GetDesc(categoryCode),
                Action = this.actionCode.GetDesc(actionCode),
                Note = stock.Note,
                Doc = doc,
                LotNumber = stock.LotNumber,
                PryUomCode = stock.PrimaryUomCode,
                SecUomCode = stock.SecondaryUomCode
            };
        }

        public STK_TXN_T CreateStockRecord(STOCK_T stock, long? dstOrganizationId, string dstOrganizationCode,
            string dstSubinventoryCode, long? dstLocatorId, string categoryCode, string actionCode, string doc,
            decimal? pryBefQty, decimal? pryChgQty, decimal? pryAftQty, decimal? secBefQty, decimal? secChgQty, decimal? secAftQty,
            string statusCode, string userId, DateTime createDate)
        {
            return new STK_TXN_T
            {
                StockId = stock.StockId,
                OrganizationId = stock.OrganizationId,
                OrganizationCode = stock.OrganizationCode,
                SubinventoryCode = stock.SubinventoryCode,
                LocatorId = stock.LocatorId,
                Barcode = stock.Barcode,
                InventoryItemId = stock.InventoryItemId,
                ItemNumber = stock.ItemNumber,
                ItemDescription = stock.ItemDescription,
                ItemCategory = stock.ItemCategory,
                DstOrganizationId = dstOrganizationId,
                DstOrganizationCode = dstOrganizationCode,
                DstSubinventoryCode = dstSubinventoryCode,
                DstLocatorId = dstLocatorId,
                Category = this.categoryCode.GetDesc(categoryCode),
                Action = this.actionCode.GetDesc(actionCode),
                Note = stock.Note,
                Doc = doc,
                LotNumber = stock.LotNumber,
                PryUomCode = stock.PrimaryUomCode,
                PryBefQty = pryBefQty,
                PryChgQty = pryChgQty,
                PryAftQty = pryAftQty,
                SecBefQty = secBefQty,
                SecChgQty = secChgQty,
                SecAftQty = secAftQty,
                SecUomCode = stock.SecondaryUomCode,
                StatusCode = statusCode,
                CreatedBy = userId,
                CreationDate = createDate,
                LastUpdateBy = null,
                LastUpdateDate = null
            };
        }

        public STK_TXN_T CreateStockRecord(STOCK_T stock, long organizationId, string organizationCode, string subinventoryCode, long? locatorId,
            long? dstOrganizationId, string dstOrganizationCode,
           string dstSubinventoryCode, long? dstLocatorId, string categoryCode, string actionCode, string doc,
           decimal? pryBefQty, decimal? pryChgQty, decimal? pryAftQty, decimal? secBefQty, decimal? secChgQty, decimal? secAftQty,
           string statusCode, string userId, DateTime createDate)
        {
            return new STK_TXN_T
            {
                StockId = stock.StockId,
                OrganizationId = organizationId,
                OrganizationCode = organizationCode,
                SubinventoryCode = subinventoryCode,
                LocatorId = locatorId,
                Barcode = stock.Barcode,
                InventoryItemId = stock.InventoryItemId,
                ItemNumber = stock.ItemNumber,
                ItemDescription = stock.ItemDescription,
                ItemCategory = stock.ItemCategory,
                DstOrganizationId = dstOrganizationId,
                DstOrganizationCode = dstOrganizationCode,
                DstSubinventoryCode = dstSubinventoryCode,
                DstLocatorId = dstLocatorId,
                Category = this.categoryCode.GetDesc(categoryCode),
                Action = this.actionCode.GetDesc(actionCode),
                Note = stock.Note,
                Doc = doc,
                LotNumber = stock.LotNumber,
                PryUomCode = stock.PrimaryUomCode,
                PryBefQty = pryBefQty,
                PryChgQty = pryChgQty,
                PryAftQty = pryAftQty,
                SecBefQty = secBefQty,
                SecChgQty = secChgQty,
                SecAftQty = secAftQty,
                SecUomCode = stock.SecondaryUomCode,
                StatusCode = statusCode,
                CreatedBy = userId,
                CreationDate = createDate,
                LastUpdateBy = null,
                LastUpdateDate = null
            };
        }

        #endregion


        #region 原因

        /// <summary>
        /// 取得原因
        /// </summary>
        public List<ReasonModel> GetReason()
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(
                    @"
select
 [REASON_CODE] as Reason_code,
 [REASON_DESC] as Reason_desc,
 u.UserName as Create_by,
 [CREATION_DATE] as Create_date,
 l.UserName as Last_update_by,
 [LAST_UPDATE_DATE] as Last_Create_date
 from STK_REASON_T s
 left join USER_T u on u.Id = CREATED_BY
 left join USER_T l on l.Id = LAST_UPDATE_BY");
                    return mesContext.Database.SqlQuery<ReasonModel>(query.ToString()).ToList();
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message.ToString());
                return new List<ReasonModel>();
            }
        }

        /// <summary>
        /// 新增編輯刪除原因
        /// </summary>
        public ResultModel SetReasonValue(ReasonViewModel.ReasonEditor ReasonEditor, string id, string name)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    if (ReasonEditor.ReasonModel.Reason_code.Length == 0 && ReasonEditor.ReasonModel.Reason_desc.Length == 0)
                    {
                        return new ResultModel(false, "不得空白");
                    }
                    else
                    {
                        if (ReasonEditor.Action == "edit")
                        {
                            var ID = stkReasonTRepository.Get(r => r.ReasonCode == ReasonEditor.ReasonModel.Reason_code).SingleOrDefault();

                            if (ID != null)
                            {
                                ID.ReasonDesc = ReasonEditor.ReasonModel.Reason_desc;
                                stkReasonTRepository.Update(ID, true);
                                return new ResultModel(true, "");
                            }
                        }

                        if (ReasonEditor.Action == "create")
                        {
                            var d = stkReasonTRepository.Get(r => r.ReasonCode.ToString() == ReasonEditor.ReasonModel.Reason_code).SingleOrDefault();
                            if (d == null)
                            {
                                STK_REASON_T sTK_REASON_T = new STK_REASON_T();
                                sTK_REASON_T.ReasonCode = ReasonEditor.ReasonModel.Reason_code;
                                sTK_REASON_T.ReasonDesc = ReasonEditor.ReasonModel.Reason_desc;
                                sTK_REASON_T.CreatedBy = id;
                                sTK_REASON_T.CreationDate = DateTime.Now;
                                stkReasonTRepository.Create(sTK_REASON_T, true);
                                return new ResultModel(true, "");
                            }
                            else
                            {
                                return new ResultModel(false, "原因ID代碼已存在");
                            }

                        }

                        if (ReasonEditor.Action == "remove")
                        {
                            var ID = stkReasonTRepository.Get(r => r.ReasonCode.ToString() == ReasonEditor.ReasonModel.Reason_code).SingleOrDefault();
                            stkReasonTRepository.Delete(ID, true);
                            return new ResultModel(true, "");
                        }
                        return new ResultModel(false, "");
                    }
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return new ResultModel(false, e.Message);
            }
        }

        #endregion

        #region 組織
        /// <summary>
        /// 取得組織資料
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public ORGANIZATION_T GetOrganization(long organizationId)
        {
            return organizationRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.OrganizationId == organizationId && x.ControlFlag != ControlFlag.Deleted);
        }

        #endregion

        #region 條碼

        /// <summary>
        /// 產生條碼清單 (請用交易TRANSACTION)
        /// </summary>
        /// <param name="organiztionId">組織ID</param>
        /// <param name="subinventoryCode">倉庫</param>
        /// <param name="requestQty">數量</param>
        /// <param name="userId">使用者ID</param>
        /// <param name="prefix">前置碼</param>
        /// <returns>ResultDataModel 條碼清單</returns>
        public ResultDataModel<List<string>> GenerateBarcodes(long organiztionId, string subinventoryCode, int requestQty, string userId, string prefix = "")
        {
            ResultDataModel<List<string>> result = null;
            try
            {
                var pOrg = SqlParamHelper.GetBigInt("@organizationId", organiztionId);
                var pSub = SqlParamHelper.R.SubinventoryCode("@subinventory", subinventoryCode);
                var pPrefix = SqlParamHelper.GetNVarChar("@prefix", prefix);
                var pReqQty = SqlParamHelper.GetInt("@requestQty", requestQty);
                var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
                var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
                var pUser = SqlParamHelper.GetNVarChar("@user", userId, 128);

                var list = this.Context.Database.SqlQuery<string>("dbo.SP_GenerateBarcodes @organizationId, @subinventory, @prefix, @requestQty, @code output, @message output, @user",
                    pOrg, pSub, pPrefix, pReqQty, pCode, pMsg, pUser).ToList();

                result = new ResultDataModel<List<string>>(Convert.ToInt32(pCode.Value), Convert.ToString(pMsg.Value), list);
            }
            catch (Exception ex)
            {
                result = new ResultDataModel<List<string>>(-1, ex.Message, null);
                logger.Error(ex, "產生條碼出現例外!!");
            }
            return result;
        }


        #endregion


        #region 單號

        /// <summary>
        /// 產生出貨單號 (請用交易TRANSACTION)
        /// </summary>
        /// <param name="subinventoryCode"></param>
        /// <param name="dstSubinventoryCode"></param>
        /// <param name="createDate"></param>
        /// <param name="digit">流水號長度</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<string> GenerateShipmentNo(string subinventoryCode, string dstSubinventoryCode, DateTime createDate, int digit, string userId)
        {
            ResultDataModel<string> result = null;
            try
            {
                var pSub = SqlParamHelper.R.SubinventoryCode("@subinventory", subinventoryCode);
                var pDstSub = SqlParamHelper.R.SubinventoryCode("@dstSubinventory", dstSubinventoryCode);
                var pDigit = SqlParamHelper.GetInt("@digit", digit);
                var pCreateDate = SqlParamHelper.GetDataTime("@createDate", createDate);
                var pDocNo = SqlParamHelper.GetVarChar("@docNo", "", 50, System.Data.ParameterDirection.Output);
                var pCode = SqlParamHelper.GetInt("@code", 0, System.Data.ParameterDirection.Output);
                var pMsg = SqlParamHelper.GetNVarChar("@message", "", 500, System.Data.ParameterDirection.Output);
                var pUser = SqlParamHelper.GetNVarChar("@user", userId, 128);

                Context.Database.ExecuteSqlCommand("dbo.SP_GenerateDocNum @subinventory, @dstSubinventory, @createDate, @digit, @docNo output, @code output, @message output, @user",
                    pSub, pDstSub, pCreateDate, pDigit, pDocNo, pCode, pMsg, pUser);


                result = new ResultDataModel<string>(Convert.ToInt32(pCode.Value), Convert.ToString(pMsg.Value), Convert.ToString(pDocNo.Value));
            }
            catch (Exception ex)
            {
                result = new ResultDataModel<string>(-1, ex.Message, null);
                logger.Error(ex, "產生單號出現例外!!");
            }
            return result;
        }

        #endregion 

        /// <summary>
        /// 產生下拉選單內容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> createDropDownList(DropDownListType type)
        {
            var organizationList = new List<SelectListItem>();
            switch (type)
            {
                case DropDownListType.All:
                    organizationList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                    break;
                case DropDownListType.Choice:
                    organizationList.Add(new SelectListItem() { Text = "請選擇", Value = "請選擇" });
                    break;
                case DropDownListType.Add:
                    organizationList.Add(new SelectListItem() { Text = "新增編號", Value = "新增編號" });
                    break;
                default://無Header
                    break;
            }
            return organizationList;
        }

        /// <summary>
        /// 取得組織下拉選單,
        /// 條件: CONTROL_FLAG不為D,
        /// 適用作業: 組織倉庫、庫存移轉出入庫
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetOrganizationDropDownList(DropDownListType type)
        {
            var organizationList = createDropDownList(type);
            organizationList.AddRange(getOrganizationList());
            return organizationList;
        }

        /// <summary>
        /// 取得使用者組織下拉選單,
        /// 條件: 使用者Id 和 CONTROL_FLAG不為D,
        /// 適用作業: 板令對照、餘切規格、、庫存移轉出入庫
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetOrganizationDropDownListForUserId(string userId, DropDownListType type)
        {
            var organizationList = createDropDownList(type);
            organizationList.AddRange(getOrganizationListForUserId(userId));
            return organizationList;
        }

        /// <summary>
        /// 取得使用者倉庫下拉選單,
        /// 條件:使用者Id 和 OSP_FLAG為Y 和 CONTROL_FLAG不為D,
        /// 適用作業: 入庫、出貨、加工、庫存移轉-入庫 收貨倉、庫存移轉-出庫 發貨倉、基本資料-板令對照
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventoryDropDownListForUserId(string userId, DropDownListType type)
        {
            var subinventoryList = createDropDownList(type);
            subinventoryList.AddRange(getSubinventoryListForUserId(userId));
            return subinventoryList;
        }

        /// <summary>
        /// 取得使用者倉庫下拉選單 Value為組織Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventoryDropDownListForUserId2(string userId, DropDownListType type)
        {
            var subinventoryList = createDropDownList(type);
            subinventoryList.AddRange(getSubinventoryListForUserId2(userId));
            return subinventoryList;
        }

        /// <summary>
        /// 取得使用者倉庫下拉選單 Value為組織Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventoryDropDownListForUserId2(string userId, string ORGANIZATION_ID, DropDownListType type)
        {
            var subinventoryList = createDropDownList(type);
            subinventoryList.AddRange(getSubinventoryListForUserId2(userId, ORGANIZATION_ID));
            return subinventoryList;
        }

        /// <summary>
        /// 取得倉庫下拉選單,
        /// 條件: 組織Id 和 CONTROL_FLAG不為D,
        /// 適用作業: 庫存移轉-入庫 發貨倉、庫存移轉-出庫 收貨倉、基本資料-組織倉庫
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventoryDropDownList(string ORGANIZATION_ID, DropDownListType type)
        {
            var subinventoryList = createDropDownList(type);
            subinventoryList.AddRange(getSubinventoryList(ORGANIZATION_ID));
            return subinventoryList;
        }

        /// <summary>
        /// 取得儲位下拉式選單內容
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocatorDropDownList(string ORGANIZATION_ID, string SUBINVENTORY_CODE, DropDownListType type)
        {
            var locatorList = createDropDownList(type);
            locatorList.AddRange(getLocatorList(ORGANIZATION_ID, SUBINVENTORY_CODE));
            return locatorList;
        }

        /// <summary>
        /// 取得使用者儲位下拉式選單內容
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocatorDropDownListForUserId(string userId, string SUBINVENTORY_CODE, DropDownListType type)
        {
            var locatorList = createDropDownList(type);
            locatorList.AddRange(getLocatorListForUserId(userId, SUBINVENTORY_CODE));
            return locatorList;
        }

        /// <summary>
        /// 取得原因下拉式選單內容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetReasonDropDownList(DropDownListType type)
        {
            var reasonList = createDropDownList(type);
            reasonList.AddRange(getReasonList());
            return reasonList;
        }

        /// <summary>
        /// 取得庫存異動紀錄原因選單
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetStkTxnReasonDropDownList(string userId, DropDownListType type)
        {
            var reasonList = createDropDownList(type);
            reasonList.AddRange(getStkTxnReasonListForUserId(userId));
            return reasonList;
        }

        /// <summary>
        /// 取得原因SelectListItem
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> getReasonList()
        {
            var reasonList = new List<SelectListItem>();
            var tempList = stkReasonTRepository
                          .GetAll().AsNoTracking()
                          .Select(x => new SelectListItem()
                          {
                              Text = x.ReasonCode + "-" + x.ReasonDesc,
                              Value = x.ReasonCode
                          });

            reasonList.AddRange(tempList);
            return reasonList;


        }

        /// <summary>
        /// 取得組織SelectListItem
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> getOrganizationList()
        {
            var organizationList = new List<SelectListItem>();
            try
            {
                var tempList = organizationRepository
                            .GetAll().AsNoTracking()
                            .Where(x => x.ControlFlag != ControlFlag.Deleted)
                            .OrderBy(x => x.OrganizationCode)
                            .Select(x => new SelectListItem()
                            {
                                Text = x.OrganizationCode,
                                Value = x.OrganizationId.ToString()
                            });
                organizationList.AddRange(tempList);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return organizationList;
        }

        /// <summary>
        /// 取得使用者的組織SelectListItem
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> getOrganizationListForUserId(string userId)
        {
            return userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(organizationRepository.GetAll().AsNoTracking(),
                us => new { us.OrganizationId },
                o => new { o.OrganizationId },
                (us, o) => new
                {
                    UserId = us.UserId,
                    OrganizationId = us.OrganizationId,
                    OrganizationCode = o.OrganizationCode,
                    ControlFlag = o.ControlFlag
                })
                .Where(x => x.UserId == userId &&
                x.ControlFlag != ControlFlag.Deleted)
                .GroupBy(x => new { x.OrganizationCode, x.OrganizationId })
                .OrderBy(x => x.Key.OrganizationCode)
                  .Select(x => new SelectListItem()
                  {
                      Text = x.Key.OrganizationCode,
                      Value = x.Key.OrganizationId.ToString()
                  }).ToList();
        }


        /// <summary>
        /// 取得倉庫SelectListItem
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <returns></returns>
        private List<SelectListItem> getSubinventoryList(string ORGANIZATION_ID)
        {
            long organizationId = 0;
            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            var subinventoryList = new List<SelectListItem>();
            if (ORGANIZATION_ID == "*")
            {
                var tempList = subinventoryRepository
                           .GetAll().AsNoTracking()
                           .Where(x =>
                           x.ControlFlag != ControlFlag.Deleted
                           )
                           .OrderBy(x => x.SubinventoryCode)
                           .Select(x => new SelectListItem()
                           {
                               Text = x.SubinventoryCode,
                               Value = x.OrganizationId.ToString()
                           });
                subinventoryList.AddRange(tempList);
            }
            else
            {
                var tempList = subinventoryRepository
                           .GetAll().AsNoTracking()
                           .Where(x => x.OrganizationId == organizationId &&
                           x.ControlFlag != ControlFlag.Deleted
                           )
                           .OrderBy(x => x.SubinventoryCode)
                           .Select(x => new SelectListItem()
                           {
                               Text = x.SubinventoryCode,
                               Value = x.OrganizationId.ToString()
                           });
                subinventoryList.AddRange(tempList);
            }
            return subinventoryList;
        }



        /// <summary>
        /// 取得使用者的倉庫SelectListItem
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<SelectListItem> getSubinventoryListForUserId(string userId)
        {
            return userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(subinventoryRepository.GetAll().AsNoTracking(),
                us => new { us.OrganizationId, us.SubinventoryCode },
                s => new { s.OrganizationId, s.SubinventoryCode },
                (us, s) => new
                {
                    UserId = us.UserId,
                    OrganizationId = us.OrganizationId,
                    SubinventoryCode = us.SubinventoryCode,
                    OspFlag = s.OspFlag,
                    ControlFlag = s.ControlFlag,
                })
                .Where(x => x.UserId == userId &&
                x.OspFlag == OspFlag.IsProcessingPlant &&
                x.ControlFlag != ControlFlag.Deleted)
               .OrderBy(x => x.SubinventoryCode)
               .Select(x => new SelectListItem()
               {
                   Text = x.SubinventoryCode,
                   Value = x.SubinventoryCode
               }).ToList();
        }

        /// <summary>
        /// 取得使用者的倉庫SelectListItem Value為組織Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<SelectListItem> getSubinventoryListForUserId2(string userId, string ORGANIZATION_ID)
        {
            long organizationId = 0;
            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            var subinventoryList = new List<SelectListItem>();
            if (ORGANIZATION_ID == "*")
            {
                var tempList = userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(subinventoryRepository.GetAll().AsNoTracking(),
                us => new { us.OrganizationId, us.SubinventoryCode },
                s => new { s.OrganizationId, s.SubinventoryCode },
                (us, s) => new
                {
                    UserId = us.UserId,
                    OrganizationId = us.OrganizationId,
                    SubinventoryCode = us.SubinventoryCode,
                    OspFlag = s.OspFlag,
                    ControlFlag = s.ControlFlag,
                })
                .Where(x => x.UserId == userId &&
                x.OspFlag == OspFlag.IsProcessingPlant &&
                x.ControlFlag != ControlFlag.Deleted)
               .OrderBy(x => x.SubinventoryCode)
               .Select(x => new SelectListItem()
               {
                   Text = x.SubinventoryCode,
                   Value = x.OrganizationId.ToString()
               }).ToList();
                subinventoryList.AddRange(tempList);
            }
            else
            {
                var tempList = userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(subinventoryRepository.GetAll().AsNoTracking(),
                us => new { us.OrganizationId, us.SubinventoryCode },
                s => new { s.OrganizationId, s.SubinventoryCode },
                (us, s) => new
                {
                    UserId = us.UserId,
                    OrganizationId = us.OrganizationId,
                    SubinventoryCode = us.SubinventoryCode,
                    OspFlag = s.OspFlag,
                    ControlFlag = s.ControlFlag,
                })
                .Where(x => x.UserId == userId &&
                x.OspFlag == OspFlag.IsProcessingPlant &&
                x.ControlFlag != ControlFlag.Deleted &&
                x.OrganizationId == organizationId)
               .OrderBy(x => x.SubinventoryCode)
               .Select(x => new SelectListItem()
               {
                   Text = x.SubinventoryCode,
                   Value = x.OrganizationId.ToString()
               }).ToList();
                subinventoryList.AddRange(tempList);
            }

            return subinventoryList;

        }

        /// <summary>
        /// 取得使用者的倉庫SelectListItem Value為組織Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<SelectListItem> getSubinventoryListForUserId2(string userId)
        {
            return userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(subinventoryRepository.GetAll().AsNoTracking(),
                us => new { us.OrganizationId, us.SubinventoryCode },
                s => new { s.OrganizationId, s.SubinventoryCode },
                (us, s) => new
                {
                    UserId = us.UserId,
                    OrganizationId = us.OrganizationId,
                    SubinventoryCode = us.SubinventoryCode,
                    OspFlag = s.OspFlag,
                    ControlFlag = s.ControlFlag,
                })
                .Where(x => x.UserId == userId &&
                x.OspFlag == OspFlag.IsProcessingPlant &&
                x.ControlFlag != ControlFlag.Deleted)
               .OrderBy(x => x.SubinventoryCode)
               .Select(x => new SelectListItem()
               {
                   Text = x.SubinventoryCode,
                   Value = x.OrganizationId.ToString()
               }).ToList();
        }

        /// <summary>
        /// 取得儲位SelectListItem
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <returns></returns>
        private List<SelectListItem> getLocatorList(string ORGANIZATION_ID, string SUBINVENTORY_CODE)
        {
            long organizationId = 0;

            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            var locatorList = new List<SelectListItem>();

            if (ORGANIZATION_ID == "*" && SUBINVENTORY_CODE == "全部")
            {
                var tempList = locatorTRepository.GetAll().AsNoTracking()
                          .Join(subinventoryRepository.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment2 = l.Segment2,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId,
                              LocatorControlFlag = l.ControlFlag,
                              LocatorDisableDate = l.LocatorDisableDate
                          })
                          .Where(x => x.LocatorType == LocatorType.Used &&
                          x.LocatorControlFlag != ControlFlag.Deleted &&
                          x.LocatorDisableDate == null || x.LocatorDisableDate > DateTime.Now
                          )
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment2 + "." + x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }
            else if (ORGANIZATION_ID != "*" && SUBINVENTORY_CODE == "全部")
            {
                var tempList = locatorTRepository.GetAll().AsNoTracking()
                          .Join(subinventoryRepository.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment2 = l.Segment2,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId,
                              LocatorControlFlag = l.ControlFlag,
                              LocatorDisableDate = l.LocatorDisableDate
                          })
                          .Where(x => x.LocatorType == LocatorType.Used &&
                          x.OrganizationId == organizationId &&
                           x.LocatorControlFlag != ControlFlag.Deleted &&
                          x.LocatorDisableDate == null || x.LocatorDisableDate > DateTime.Now
                          )
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment2 + "." + x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }
            else if (ORGANIZATION_ID == "*" && SUBINVENTORY_CODE != "全部")
            {
                var tempList = locatorTRepository.GetAll().AsNoTracking()
                          .Join(subinventoryRepository.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment2 = l.Segment2,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId,
                              LocatorControlFlag = l.ControlFlag,
                              LocatorDisableDate = l.LocatorDisableDate
                          })
                          .Where(x => x.LocatorType == LocatorType.Used &&
                          x.SubinventoryCode == SUBINVENTORY_CODE &&
                          x.LocatorControlFlag != ControlFlag.Deleted &&
                          x.LocatorDisableDate == null || x.LocatorDisableDate > DateTime.Now
                          )
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment2 + "." + x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }
            else
            {
                var tempList = locatorTRepository.GetAll().AsNoTracking()
                          .Join(subinventoryRepository.GetAll().AsNoTracking(),
                          l => new { l.SubinventoryCode, l.OrganizationId },
                          s => new { s.SubinventoryCode, s.OrganizationId },
                          (l, s) => new
                          {
                              OrganizationId = l.OrganizationId,
                              SubinventoryCode = l.SubinventoryCode,
                              Segment2 = l.Segment2,
                              Segment3 = l.Segment3,
                              LocatorType = s.LocatorType,
                              LocatorId = l.LocatorId,
                              LocatorControlFlag = l.ControlFlag,
                              LocatorDisableDate = l.LocatorDisableDate
                          })
                          .Where(x => x.LocatorType == LocatorType.Used &&
                          x.OrganizationId == organizationId &&
                          x.SubinventoryCode == SUBINVENTORY_CODE &&
                          x.LocatorControlFlag != ControlFlag.Deleted &&
                          x.LocatorDisableDate == null || x.LocatorDisableDate > DateTime.Now
                          )
                          .OrderBy(x => x.Segment3)
                          .Select(x => new SelectListItem()
                          {
                              Text = x.Segment2 + "." + x.Segment3,
                              Value = x.LocatorId.ToString()
                          });
                locatorList.AddRange(tempList);
            }

            return locatorList;
        }

        /// <summary>
        /// 取得使用者儲位SelectListItem
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <returns></returns>
        public List<SelectListItem> getLocatorListForUserId(string userId, string SUBINVENTORY_CODE)
        {

            var tmp = userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(locatorTRepository.GetAll().AsNoTracking(),
                    us => new { us.OrganizationId, us.SubinventoryCode },
                    l => new { l.OrganizationId, l.SubinventoryCode },
                    (us, l) => new
                    {
                        UserId = us.UserId,
                        OrganizationId = us.OrganizationId,
                        SubinventoryCode = us.SubinventoryCode,
                        Segment2 = l.Segment2,
                        Segment3 = l.Segment3,
                        LocatorId = l.LocatorId,
                        LocatorControlFlag = l.ControlFlag,
                        LocatorDisableDate = l.LocatorDisableDate
                    })
                .Where(x =>
                    x.UserId == userId
                    && x.LocatorControlFlag != ControlFlag.Deleted
                    && (x.LocatorDisableDate == null || x.LocatorDisableDate > DateTime.Now)
                    );

            if (SUBINVENTORY_CODE != "*" && SUBINVENTORY_CODE != "")
            {
                tmp = tmp.Where(x => x.SubinventoryCode == SUBINVENTORY_CODE);
            }

            return tmp.OrderBy(x => x.Segment2).ThenBy(x => x.Segment3)
                     .Select(x => new SelectListItem()
                     {
                         Text = x.Segment2 + "." + x.Segment3,
                         Value = x.LocatorId.ToString()
                     }).ToList();
        }

        public List<SelectListItem> getLocatorListForUserId(string userId, string SUBINVENTORY_CODE, long? selectedLocatorId)
        {

            var tmp = userSubinventoryTRepository.GetAll().AsNoTracking()
                .Join(locatorTRepository.GetAll().AsNoTracking(),
                    us => new { us.OrganizationId, us.SubinventoryCode },
                    l => new { l.OrganizationId, l.SubinventoryCode },
                    (us, l) => new
                    {
                        UserId = us.UserId,
                        OrganizationId = us.OrganizationId,
                        SubinventoryCode = us.SubinventoryCode,
                        Segment2 = l.Segment2,
                        Segment3 = l.Segment3,
                        LocatorSegments = l.LocatorSegments,
                        LocatorId = l.LocatorId,
                        LocatorControlFlag = l.ControlFlag,
                        LocatorDisableDate = l.LocatorDisableDate
                    })
                .Where(x =>
                    x.UserId == userId
                    && x.LocatorControlFlag != ControlFlag.Deleted
                    && (x.LocatorDisableDate == null || x.LocatorDisableDate > DateTime.Now)
                    );

            if (!string.IsNullOrEmpty(SUBINVENTORY_CODE))
            {
                tmp = tmp.Where(x => x.SubinventoryCode == SUBINVENTORY_CODE);
            }

            if (selectedLocatorId != null)
            {
                return tmp.OrderBy(x => x.Segment3)
                     .Select(x => new SelectListItem()
                     {
                         Text = x.Segment2 + "." + x.Segment3,
                         Value = x.LocatorId.ToString(),
                         Selected = x.LocatorId == selectedLocatorId
                     }).ToList();
            }
            else
            {
                return tmp.OrderBy(x => x.Segment3)
                     .Select(x => new SelectListItem()
                     {
                         Text = x.Segment2 + "." + x.Segment3,
                         Value = x.LocatorId.ToString(),
                         Selected = x.LocatorSegments == "FTY.TB3.TFG.NA"
                     }).ToList();
            }
        }

        /// <summary>
        /// 取得使用者異動記錄原因SelectListItem
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SelectListItem> getStkTxnReasonListForUserId(string userId)
        {
            try
            {
                var subinventoryList = GetSubinventoryListForUser(userId);
                if (subinventoryList == null || subinventoryList.Count == 0)
                {
                    throw new Exception("找不到使用者倉庫");
                }

                List<string> newSubinventoryList = new List<string>();
                foreach(string subinventory in subinventoryList)
                {
                    newSubinventoryList.Add("'" + subinventory + "'");
                }

                StringBuilder cmd = new StringBuilder();
                cmd.AppendLine(
@"SELECT s.reason as Text, s.reason as Value FROM (SELECT (CATEGORY + '-(' + ACTION + ')') as reason  FROM  [STK_TXN_T] WHERE 
SUBINVENTORY_CODE IN ({subinventoryList}) OR DST_SUBINVENTORY_CODE IN ({subinventoryList}) ) as s group by s.reason order by s.reason");
                var subinventoryCondition = string.Join(",", newSubinventoryList);
                cmd.Replace("{subinventoryList}", subinventoryCondition);
                string commandText = cmd.ToString();

                var list = Context.Database.SqlQuery<SelectListItem>(commandText).ToList();

                return list;

            }
            catch(Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// 取得SubinventoryCodeList
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ospFlag"></param>
        /// <returns></returns>
        public List<string> GetSubinventoryListForUser(string userId, string ospFlag = OspFlag.IsProcessingPlant)
        {
            return subinventoryRepository.GetAll().AsNoTracking().Join(
                             userSubinventoryTRepository.GetAll().AsNoTracking(),
                             s => new { s.SubinventoryCode },
                             us => new { us.SubinventoryCode },
                             (s, us) => new
                             {
                                 UserId = us.UserId,
                                 SubinventoryCode = s.SubinventoryCode,
                                 OspFlag = s.OspFlag,
                                 ControlFlag = s.ControlFlag
                             }).Where(x => x.UserId == userId &&
                             x.OspFlag == OspFlag.IsProcessingPlant &&
                              x.ControlFlag != ControlFlag.Deleted
                             )
                             .Select(x => x.SubinventoryCode).ToList();
        }


        public class DropDownListTypeValue
        {
            public const string NoHeader = "";
            public const string All = "*";
            public const string Choice = "請選擇";
            public const string Add = "新增編號";
        }

        /// <summary>
        /// 下拉選單OPTION種類
        /// </summary>
        public enum DropDownListType
        {
            NoHeader = 0, //沒有額外添加第一項
            All = 1, //添加第一項為全部
            Choice = 2, //添加第一項為請選擇
            Add = 3 //添加第一項為新增編號
        }

        /// <summary>
        /// 取得 基本資料-庫存組織 查詢結果
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="SUBINVENTORY_CODE"></param>
        /// <param name="LOCATOR_ID"></param>
        /// <returns></returns>
        public List<OrgSubinventoryDT> OrgSubinventorySearch(string ORGANIZATION_ID, string SUBINVENTORY_CODE, string LOCATOR_ID)
        {
            try
            {
                long orgId = 0;
                long locId = 0;

                try
                {
                    if (!string.IsNullOrEmpty(ORGANIZATION_ID) && ORGANIZATION_ID != "*")
                    {
                        orgId = Convert.ToInt64(ORGANIZATION_ID);
                    }
                }
                catch
                {
                    ORGANIZATION_ID = "*";
                }

                try
                {
                    if (!string.IsNullOrEmpty(LOCATOR_ID) && LOCATOR_ID != "*")
                    {
                        locId = Convert.ToInt64(LOCATOR_ID);
                    }
                }
                catch
                {
                    LOCATOR_ID = "*";
                }


                List<string> cond = new List<string>();
                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                string prefixCmd = @"
select 
o.ORGANIZATION_ID,
o.ORGANIZATION_CODE,
o.ORGANIZATION_NAME,
s.SUBINVENTORY_CODE,
s.SUBINVENTORY_NAME,
s.OSP_FLAG,
'A',
l.LOCATOR_ID,
s.LOCATOR_TYPE,
l.LOCATOR_SEGMENTS,
l.LOCATOR_DESC,
l.SEGMENT1,
l.SEGMENT2,
l.SEGMENT3,
l.SEGMENT4
from ORGANIZATION_T o
inner join SUBINVENTORY_T s on o.ORGANIZATION_ID = s.ORGANIZATION_ID
left join LOCATOR_T l on s.ORGANIZATION_ID = l.ORGANIZATION_ID and s.SUBINVENTORY_CODE = l.SUBINVENTORY_CODE ";

                if (ORGANIZATION_ID != "*")
                {
                    cond.Add("o.ORGANIZATION_ID = @ORGANIZATION_ID");
                    sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", orgId.ToString()));

                }
                if (SUBINVENTORY_CODE != "全部")
                {
                    cond.Add("s.SUBINVENTORY_CODE = @SUBINVENTORY_CODE");
                    sqlParameterList.Add(new SqlParameter("@SUBINVENTORY_CODE", SUBINVENTORY_CODE));
                }
                if (LOCATOR_ID != "*")
                {
                    cond.Add("l.LOCATOR_ID = @LOCATOR_ID");
                    sqlParameterList.Add(new SqlParameter("@LOCATOR_ID", locId.ToString()));
                }

                string commandText = string.Format(prefixCmd + "{0}{1}", cond.Count > 0 ? " WHERE " : "", string.Join(" AND ", cond.ToArray()));

                commandText += " AND s.CONTROL_FLAG <> 'D' AND l.CONTROL_FLAG <> 'D' AND o.CONTROL_FLAG <> 'D'";

                if (sqlParameterList.Count > 0)
                {
                    return this.Context.Database.SqlQuery<OrgSubinventoryDT>(commandText, sqlParameterList.ToArray()).ToList();

                }
                else
                {
                    return this.Context.Database.SqlQuery<OrgSubinventoryDT>(commandText).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new List<OrgSubinventoryDT>();
            }
        }

        /// <summary>
        /// 取得庫存標籤資料
        /// </summary>
        /// <param name="stockIdList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultDataModel<List<LabelModel>> GetStockLabels(List<long> stockIdList, string userId)
        {
            try
            {

                List<LabelModel> labelModelList = new List<LabelModel>();
                if (stockIdList == null || stockIdList.Count == 0)
                {
                    return new ResultDataModel<List<LabelModel>>(false, "找不到揀貨資料", null);
                }

                List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                StringBuilder cmd = new StringBuilder();
                cmd.AppendLine(
@"SELECT 
CAST(T.BARCODE AS nvarchar) AS Barocde,
U.UserName as PrintBy,
CAST(I.ITEM_DESC_TCH AS nvarchar) AS BarocdeName, 
CAST(T.PAPER_TYPE AS nvarchar) AS PapaerType,
CAST(T.BASIC_WEIGHT AS nvarchar) AS BasicWeight,
CAST(T.SPECIFICATION AS nvarchar) AS Specification,
CASE ITEM_CATEGORY 
WHEN '捲筒' THEN CAST(FORMAT(T.PRIMARY_AVAILABLE_QTY,'0.##########') AS nvarchar) 
ELSE  CAST(FORMAT(T.SECONDARY_AVAILABLE_QTY,'0.##########') AS nvarchar) 
END AS Qty,
CASE ITEM_CATEGORY 
WHEN '捲筒' THEN CAST(T.PRIMARY_UOM_CODE AS nvarchar) 
ELSE  CAST(T.SECONDARY_UOM_CODE AS nvarchar) 
END AS Unit,
CAST(T.LOT_NUMBER AS nvarchar) AS LotNumber,
CAST(T.OSP_BATCH_NO AS nvarchar) AS OspBatchNo
FROM STOCK_T T
JOIN ITEMS_T I on I.INVENTORY_ITEM_ID = T.INVENTORY_ITEM_ID
JOIN USER_T U ON U.Id = @userId 
WHERE T.STOCK_ID IN");


                var stockCondition = string.Join(",", stockIdList);

                cmd.AppendFormat(" ({0}) ", stockCondition);


                string commandText = cmd.ToString();
                sqlParameterList.Add(new SqlParameter("@userId", userId));

                var list = Context.Database.SqlQuery<LabelModel>(commandText, sqlParameterList.ToArray()).ToList();
                if (list == null || list.Count() == 0)
                {
                    return new ResultDataModel<List<LabelModel>>(false, "找不到標籤資料", null);
                }

                labelModelList.AddRange(list);

                return new ResultDataModel<List<LabelModel>>(true, "取得標籤資料成功", labelModelList);

            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<List<LabelModel>>(false, "取得標籤資料失敗:" + ex.Message, null);
            }
        }

        /// <summary>
        /// 標籤列印
        /// </summary>
        /// <param name="labels">標籤內容</param>
        /// <returns></returns>
        public ActionResult PrintLabel(List<LabelModel> labels)
        {
            Util.PdfLableUtil pdf = new PdfLableUtil();
            var result = pdf.GeneratePdfLabels2(labels);
            if (!result.Success)
            {
                throw new Exception("產生PDF發生錯誤:" + result.Msg);
            }
            string labelFullPath = result.Msg;
            var fileStream = new FileStream(labelFullPath,
                                FileMode.Open,
                                FileAccess.Read
                                );
            return new FileStreamResult(fileStream, "application/pdf");
        }

        #region 料號

        /// <summary>
        /// 取得自動完成料號List
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public List<AutoCompletedItem> GetAutoCompleteItemNumberList(string Prefix, int count = 30)
        {
            return itemsTRepository.GetAll().AsNoTracking().Where(x =>
            x.ItemNumber.Contains(Prefix) &&
            x.ControlFlag != ControlFlag.Deleted)
                .Take(count)
                .OrderBy(x => x.ItemNumber)
                .Select(x => new AutoCompletedItem
                {
                    Description = x.ItemDescTch,
                    Value = x.ItemNumber
                })
                .ToList();
        }

        /// <summary>
        /// 取得料號資料
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public ITEMS_T GetItemNumber(string itemNumber)
        {
            return itemsTRepository.GetAll().AsNoTracking().Join(
                orgItemRepository.GetAll().AsNoTracking(),
                i => i.InventoryItemId,
                oi => oi.InventoryItemId,
                (i, oi) => i)
                .FirstOrDefault(x => x.ItemNumber == itemNumber);
        }

        /// <summary>
        /// 取得料號資料
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public ITEMS_T GetItemNumber(long InventoryItemId)
        {
            return itemsTRepository.GetAll().AsNoTracking().Join(
                orgItemRepository.GetAll().AsNoTracking(),
                i => i.InventoryItemId,
                oi => oi.InventoryItemId,
                (i, oi) => i)
                .FirstOrDefault(x => x.InventoryItemId == InventoryItemId);
        }

        /// <summary>
        /// 取得料號組織
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public List<long> GetItemNumberOrganizationId(long inventoryItemId)
        {
            return itemsTRepository.GetAll().AsNoTracking().
                Join(orgItemRepository.GetAll().AsNoTracking(),
                i => new { i.InventoryItemId },
                o => new { o.InventoryItemId },
                (i, o) => new
                {
                    InventoryItemId = i.InventoryItemId,
                    OrganizationId = o.OrganizationId
                }
                ).Where(x => x.InventoryItemId == inventoryItemId).
                Select(x => x.OrganizationId).ToList();
        }

        #endregion

        /// <summary>
        /// 取得機台
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetManchine(string OrganizationId)
        {
            List<SelectListItem> ManchineNum = new List<SelectListItem>();
            ManchineNum.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });

            if (OrganizationId != "*")
            {
                var id = Int32.Parse(OrganizationId);
                var MachineCode = machinePaperTypeRepository.Get(x => x.ControlFlag != ControlFlag.Deleted && x.OrganizationId == id)
                                  .GroupBy(x => x.MachineNum)
                    .Select(y => new SelectListItem
                    {
                        Text = y.Key,
                        Value = y.Key
                    }).ToList();
                ManchineNum.AddRange(MachineCode);
            }
            else
            {
                var MachineCode = machinePaperTypeRepository.Get(c => c.ControlFlag != ControlFlag.Deleted)
                     .GroupBy(x => x.MachineNum)
                    .Select(y => new SelectListItem
                    {
                        Text = y.Key,
                        Value = y.Key
                    }).ToList();
                ManchineNum.AddRange(MachineCode);
            }

            return ManchineNum;
        }

        /// <summary>
        /// 取得倉庫
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OspFlag"></param>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventory(string UserId, string OspFlag)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SelectListItem> sublist = new List<SelectListItem>();
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"
SELECT
st.SUBINVENTORY_CODE as Text,
st.SUBINVENTORY_CODE as Value
FROM SUBINVENTORY_T st
join USER_SUBINVENTORY_T usb on usb.SUBINVENTORY_CODE = st.SUBINVENTORY_CODE
WHERE CONTROL_FLAG != 'D'
and usb.UserId = @UserId
");
                    if (OspFlag == "Y")
                    {
                        cond.Add("and OSP_FLAG = @OSP_FLAG");
                        sqlParameterList.Add(new SqlParameter("@OSP_FLAG", OspFlag));
                        sublist.Add(new SelectListItem { Text = "全部", Value = "*" });
                    }

                    sqlParameterList.Add(new SqlParameter("@UserId", UserId));
                    string commandText = string.Format(query + "{0}", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        sublist.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText, sqlParameterList.ToArray()).ToList());
                    }
                    else
                    {
                        sublist.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText).ToList());
                    }

                    return sublist;
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// 取得令重包數資料
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="organizationCode"></param>
        /// <param name="ospSubinventory"></param>
        /// <param name="pstyp"></param>
        /// <param name="sBasicWeight"></param>
        /// <returns></returns>
        public YSZMPCKQ_T GetYszmpckq(long organizationId, string organizationCode, string ospSubinventory, string pstyp, string sBasicWeight)
        {
            try
            {
                decimal basicWeight = Convert.ToDecimal(sBasicWeight) / 10;
                using (var mesContext = new MesContext())
                {
                    return yszmpckqTRepository.GetAll().AsNoTracking().FirstOrDefault(x => x.OrganizationId == organizationId &&
                        x.OrganizationCode == organizationCode && x.OspSubinventory == ospSubinventory && x.Pstyp == pstyp
                        && basicWeight >= x.Bwetdn && basicWeight <= x.Bwetup);
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return null;
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
            /// <summary>
            /// 庫存移轉-已出庫
            /// </summary>
            public const string TransferOutOfStock = "DS3";



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
                    case TransferOutOfStock:
                        return "庫存移轉-已出庫";
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
                    case TransferOutOfStock:
                        return StockStatusCode.TransferNoneInStock;
                    default:
                        return "";
                }
            }
        }


        /// <summary>
        /// 壓縮照片大小
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] VaryQualityLevel(HttpPostedFileBase file)
        {
            using (var thumb = System.Drawing.Image.FromStream(file.InputStream))
            {
                var jpgInfo = GetEncoder(ImageFormat.Jpeg); /* Returns array of image encoder objects built into GDI+ */
                using (var samllfile = new MemoryStream())
                {
                    //    // Create an EncoderParameters object.  
                    //    // An EncoderParameters object has an array of EncoderParameter  
                    //    // objects. In this case, there is only one  
                    //    // EncoderParameter object in the array.  
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 30L);
                    thumb.Save(samllfile, jpgInfo, myEncoderParameters);
                    return samllfile.ToArray();
                }
            };

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

    }
}