using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CHPOUTSRCMES.Web.ViewModels.StockTransaction;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.Jsons.Requests;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entiy.Transfer;
using System.Security.Claims;
using CHPOUTSRCMES.Web.DataModel.Entity;
using NLog;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Reporting.WebForms;
using System.Drawing;

namespace CHPOUTSRCMES.Web.Models.Stock
{
    public class StockTransferDT
    {
        public long ID { get; set; }

        public long SUB_ID { get; set; }

        [Display(Name = "料號")]
        public string ITEM_NUMBER { get; set; }

        //[Display(Name = "基重")]
        //public string Base_Weight { get; set; }

        //[Display(Name = "紙別")]
        //public string PAPERTYPE { get; set; }

        //[Display(Name = "規格")]
        //public string Specification { get; set; }

        [Display(Name = "包裝方式")]
        public string PACKING_TYPE { get; set; }

        [Display(Name = "捲數/板數")]
        public decimal ROLL_REAM_QTY { get; set; }

        [Display(Name = "捲板單位")]
        public string ROLL_REAM_UOM { get; set; }

        [Display(Name = "需求數量")] //預計出庫量 主要數量
        public decimal REQUESTED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //主單位已揀數合計
        public decimal PICKED_QUANTITY { get; set; }

        [Display(Name = "已揀數量")] //入庫主單位已揀數合計
        public decimal INBOUND_PICKED_QUANTITY { get; set; }

        [Display(Name = "單位")] //主單位
        public string REQUESTED_QUANTITY_UOM { get; set; }

        [Display(Name = "需求數量")] //預計出庫輔數量 次要數量
        public decimal REQUESTED_QUANTITY2 { get; set; }

        [Display(Name = "已揀數量")] //出庫已揀輔數量
        public decimal PICKED_QUANTITY2 { get; set; }

        [Display(Name = "已揀數量")] //入庫已揀輔數量
        public decimal INBOUND_PICKED_QUANTITY2 { get; set; }

        [Display(Name = "單位")] //輔單位
        public string REQUESTED_QUANTITY_UOM2 { get; set; }

        [Display(Name = "每棧令數")]
        public decimal ROLL_REAM_WT { get; set; }


        //[Display(Name = "需求數量")] //訂單原始數量 交易數量
        //public decimal SRC_REQUESTED_QUANTITY { get; set; }

        //[Display(Name = "已揀數量")] //交易單位已揀數合計 由主單位已揀數合計 換算過來
        //public decimal SRC_PICKED_QUANTITY { get; set; }

        //[Display(Name = "單位")] //交易單位
        //public string SRC_REQUESTED_QUANTITY_UOM { get; set; }

        //[Display(Name = "備註")]
        //public string REMARK { get; set; }



        [Display(Name = "建立人員ID")]
        public long CREATED_BY { get; set; }
        [Display(Name = "建立人員名稱")]
        public string CREATE_BY_USERNAME { set; get; }
        [Display(Name = "建立日期")]
        public DateTime CREATION_DATE { get; set; }


        [Display(Name = "更新人員ID")]
        public long LAST_UPDATED_BY { get; set; }
        [Display(Name = "更新人員名稱")]
        public long LAST_UPDATED_BY_USERNAME { get; set; }
        [Display(Name = "更新日期")]
        public DateTime LAST_UPDATE_DATE { get; set; }

        [Display(Name = "出貨編號")]
        public string SHIPMENT_NUMBER { get; set; }

        [Display(Name = "移轉編號")]
        public string SUBINVENTORY_TRANSFER_NUMBER { get; set; }

        [Display(Name = "編號狀態")]
        public string NUMBER_STATUS { get; set; }

        [Display(Name = "發貨倉庫")]
        public string OUT_SUBINVENTORY_CODE { get; set; }

        [Display(Name = "發貨儲位")]
        public string OUT_LOCATOR_ID { get; set; }

        [Display(Name = "收貨倉庫")]
        public string IN_SUBINVENTORY_CODE { get; set; }

        [Display(Name = "收貨儲位")]
        public string IN_LOCATOR_ID { get; set; }
    }

    public class StockTransferData
    {
        public static List<StockTransferDT> model = new List<StockTransferDT>();
        public List<StockTransferDT> importModel = new List<StockTransferDT>();
        public OrgSubinventoryData orgData = new OrgSubinventoryData();

        private ILogger logger = LogManager.GetCurrentClassLogger();
        public static void resetData()
        {
            model = new List<StockTransferDT>();
        }


        public StockTransferViewModel GetViewModel(TransferUOW uow)
        {
            StockTransferViewModel viewModel = new StockTransferViewModel();
            //viewModel.SelectedTransferType = "請選擇";
            viewModel.TransferTypeItems = uow.GetTransferTypeDropDownList();
            return viewModel;
        }

        public OutBoundViewModel GetOutBoundViewModel(TransferUOW uow, string userId)
        {
            OutBoundViewModel viewModel = new OutBoundViewModel();

            viewModel.OutSubinventoryItems = orgData.GetSubinventoryListForUserId(uow, userId, MasterUOW.DropDownListType.Choice);

            //viewModel.OutLocatorItems = orgData.GetLocatorList(uow, "265", viewModel.SelectedOutSubinventory, MasterUOW.DropDownListType.Choice);
            viewModel.OutLocatorItems = uow.createDropDownList(MasterUOW.DropDownListType.Choice);

            viewModel.InSubinventoryItems = orgData.GetSubinventoryList(uow, "*", MasterUOW.DropDownListType.Choice);

            //viewModel.InLocatorItems = orgData.GetLocatorList(uow, "*", viewModel.SelectedInSubinventory, MasterUOW.DropDownListType.Choice);
            viewModel.InLocatorItems = uow.createDropDownList(MasterUOW.DropDownListType.Choice);

            viewModel.ShipmentNumberItems = uow.createDropDownList(MasterUOW.DropDownListType.Add);

            //viewModel.SubinventoryTransferNumberItems = GetSubinventoryTransferNumberList(viewModel.SelectedOutSubinventory, viewModel.SelectedOutLocator, viewModel.SelectedInSubinventory, viewModel.SelectedInLocator);

            //viewModel.ItemNumberItems = StockData.GetItemNumberList(viewModel.SelectedOutSubinventory, Convert.ToInt64(viewModel.SelectedOutLocator));

            //ResultModel result = CheckTransactionType(viewModel.SelectedOutSubinventory, viewModel.SelectedInSubinventory);
            //if (result.Success == false)
            //{
            //    viewModel.DisplayDetail = false;
            //    viewModel.DisplayShipmentNumberArea = false;
            //    viewModel.DisplaySubinventoryTransferNumberArea = false;
            //}
            //else
            //{
            //    viewModel.DisplayDetail = true;
            //    if (result.Msg == "倉庫間移轉")
            //    {
            //        viewModel.DisplayShipmentNumberArea = false;
            //        viewModel.DisplaySubinventoryTransferNumberArea = true;
            //    }
            //    else
            //    {
            //        viewModel.DisplayShipmentNumberArea = true;
            //        viewModel.DisplaySubinventoryTransferNumberArea = false;
            //    }
            //}

            return viewModel;
        }

        public InBoundViewModel GetInBoundViewModel(TransferUOW uow, List<Claim> roles, string userId)
        {
            InBoundViewModel viewModel = new InBoundViewModel();

            viewModel.OutSubinventoryItems = orgData.GetSubinventoryList(uow, "*", MasterUOW.DropDownListType.Choice);

            //viewModel.OutLocatorItems = orgData.GetLocatorList(uow, "*", viewModel.SelectedOutSubinventory, MasterUOW.DropDownListType.Choice);
            viewModel.OutLocatorItems = uow.createDropDownList(MasterUOW.DropDownListType.Choice);

            viewModel.InSubinventoryItems = orgData.GetSubinventoryListForUserId(uow, userId, MasterUOW.DropDownListType.Choice);

            //viewModel.InLocatorItems = orgData.GetLocatorList(uow, "265", viewModel.SelectedInSubinventory, MasterUOW.DropDownListType.Choice);
            viewModel.InLocatorItems = uow.createDropDownList(MasterUOW.DropDownListType.Choice);

            viewModel.ShipmentNumberItems = uow.createDropDownList(MasterUOW.DropDownListType.Add);

            if (roles != null && roles.Count > 0)
            {
                foreach (Claim role in roles)
                {
                    if (role.Value == MasterUOW.UserRole.Adm || role.Value == MasterUOW.UserRole.ChpUser)
                    {
                        viewModel.Advanced = true;
                        break;
                    }
                    else
                    {
                        viewModel.Advanced = false;
                    }
                }
            }

            return viewModel;
        }

        public TransferReasonViewModel GetTransferReasonViewModel(TransferUOW uow)
        {
            TransferReasonViewModel viewModel = new TransferReasonViewModel();
            viewModel.ReasonItems = uow.GetReasonDropDownList(MasterUOW.DropDownListType.Choice);
            viewModel.LocatorItems = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            return viewModel;
        }



        public List<SelectListItem> GetLocatorListForUserId(MasterUOW uow, string userId, string SUBINVENTORY_CODE, MasterUOW.DropDownListType type)
        {
            //return orgData.GetLocatorList(uow, "265", SUBINVENTORY_CODE, type);
            return orgData.GetLocatorListForUserId(uow, userId, SUBINVENTORY_CODE, type);
        }

        /// <summary>
        /// 取得儲位下拉選單,
        /// 條件:ORGANIZATION_ID 和 SUBINVENTORY_CODE 和 LOCATOR_TYPE為2 和 CONTROL_FLAG 不為D 和 LOCATOR_DISABLE_DATE為NULL或大於系統時間,
        /// 適用作業: 庫存移轉-入庫 發貨儲位、庫存移轉-出庫 收貨儲位、基本資料-組織倉庫
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="ORGANIZATION_ID">組織Id, *為全部組織</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocatorList(MasterUOW uow, string ORGANIZATION_ID, string SUBINVENTORY_CODE, MasterUOW.DropDownListType type)
        {
            return uow.GetLocatorDropDownList(ORGANIZATION_ID, SUBINVENTORY_CODE, type);
        }

        /// <summary>
        /// 取得自動完成料號List
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public List<AutoCompletedItem> GetAutoCompleteItemNumberList(TransferUOW uow, string Prefix)
        {
            return uow.GetAutoCompleteItemNumberList(Prefix);
        }

        public ResultDataModel<ITEMS_T> GetItemNumberData(TransferUOW uow, string itemNumber)
        {
            var item = uow.GetItemNumber(itemNumber);
            if (item == null) return new ResultDataModel<ITEMS_T>(false, "找不到料號資料", null);
            return new ResultDataModel<ITEMS_T>(true, "取得料號資料成功", item);

        }

        public ResultModel CheckTransactionType(string outSubinventory, string inSubinventory)
        {
            if (outSubinventory == null || outSubinventory == "請選擇") return new ResultModel(false, "");
            if (inSubinventory == null || inSubinventory == "請選擇") return new ResultModel(false, "");

            if (orgData.getORGANIZATION_CODE(outSubinventory) == orgData.getORGANIZATION_CODE(inSubinventory))
            {
                return new ResultModel(true, "倉庫間移轉"); //倉庫間移轉
            }
            else
            {
                return new ResultModel(true, "組織間移轉"); //組織間移轉
            }
        }

        public List<StockTransferDT> GetModelFromShipmentNumber(string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string shipmentNumber)
        {
            var query = from stockTransferDT in model
                        where OutSubinventoryCode == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OutLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        InSubinventoryCode == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        InLocator == stockTransferDT.IN_LOCATOR_ID &&
                        shipmentNumber == stockTransferDT.SHIPMENT_NUMBER
                        select stockTransferDT;

            return query.ToList();
        }

        public List<StockTransferDT> GetModelFromSubinventoryTransferNumber(string OutSubinventoryCode, string OutLocator, string InSubinventoryCode, string InLocator, string subinventoryTransferNumber)
        {
            var query = from stockTransferDT in model
                        where OutSubinventoryCode == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OutLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        InSubinventoryCode == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        InLocator == stockTransferDT.IN_LOCATOR_ID &&
                        subinventoryTransferNumber == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                        select stockTransferDT;

            return query.ToList();
        }

        public class GetShipmentNumberListResult
        {
            public bool status { get; set; }

            public string result { get; set; }
            public List<SelectListItem> items { get; set; }
            public string transferCatalog { get; set; }

            public string transferType { get; set; }
            public string numberStatus { get; set; }
            public string isMes { get; set; }

        }

        public GetShipmentNumberListResult GetInboundShipmentNumberList(TransferUOW uow, long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            var headerList = uow.GetInBoundShipmentNumberDropDownList(outOrganizationId, outSubinventoryCode, inOrganizationId, inSubinventoryCode);
            if (headerList == null || headerList.Count == 0) return new GetShipmentNumberListResult { status = true, result = "找不到出貨編號", items = uow.createDropDownList(MasterUOW.DropDownListType.Add) };
            //List<SelectListItem> items = headerList.Select(i => new SelectListItem() { Text = i.ShipmentNumber, Value = i.ShipmentNumber }).ToList();

            return new GetShipmentNumberListResult
            {
                status = true,
                result = "取得出貨編號成功",
                items = headerList
            };
        }

        public GetShipmentNumberListResult GetOutboundShipmentNumberList(TransferUOW uow, long outOrganizationId, string outSubinventoryCode, long inOrganizationId, string inSubinventoryCode)
        {
            var headerList = uow.GetOutBoundShipmentNumberDropDownList(outOrganizationId, outSubinventoryCode, inOrganizationId, inSubinventoryCode);
            if (headerList == null || headerList.Count == 0) return new GetShipmentNumberListResult { status = true, result = "找不到出貨編號", items = uow.createDropDownList(MasterUOW.DropDownListType.Add) };

            return new GetShipmentNumberListResult
            {
                status = true,
                result = "取得出貨編號成功",
                items = headerList
            };
        }

        //public GetShipmentNumberListResult GetShipmentNumberList(TransferUOW uow, string transferType, string outSubinventoryCode, string inSubinventoryCode)
        //{
        //    var outSubinventory = uow.GetSubinventoryT(outSubinventoryCode);
        //    if (outSubinventory == null || outSubinventory.Count == 0) return new GetShipmentNumberListResult { status = false, result = "找不到出貨倉庫資料" };
        //    var inSubinventory = uow.GetSubinventoryT(inSubinventoryCode);
        //    if (inSubinventory == null || inSubinventory.Count == 0) return new GetShipmentNumberListResult { status = false, result = "找不到出貨倉庫資料" };

        //    string TRANSFER_CATALOG = "";
        //    if (outSubinventory[0].OrganizationId == inSubinventory[0].OrganizationId)
        //    {
        //        TRANSFER_CATALOG = TransferUOW.TransferCatalog.InvTransfer;
        //    }
        //    else
        //    {
        //        TRANSFER_CATALOG = TransferUOW.TransferCatalog.OrgTransfer;
        //    }

        //    //string transferCatalog = uow.GetTransferCatalog(outSubinventoryCode, inSubinventoryCode);
        //    //List<SelectListItem> items = uow.GetShipmentNumberDropDownList(TRANSFER_CATALOG, transferType, outSubinventoryCode, inSubinventoryCode);
        //    //if (items == null || items.Count == 0) return new GetShipmentNumberListResult { status = false, msg = "找不到出貨倉庫資料" };

        //    var headerList = uow.GetTrfHeaderList(TRANSFER_CATALOG, outSubinventory[0].OrganizationId, outSubinventoryCode, inSubinventory[0].OrganizationId, inSubinventoryCode);
        //    if (headerList == null || headerList.Count == 0) return new GetShipmentNumberListResult { status = true, result = "找不到出貨編號", items = uow.createDropDownList(MasterUOW.DropDownListType.Add) };

        //    List<SelectListItem> items = headerList.Select(i => new SelectListItem() { Text = i.ShipmentNumber, Value = i.ShipmentNumber }).ToList();

        //    return new GetShipmentNumberListResult
        //    {
        //        status = true,
        //        result = "取得出貨編號成功",
        //        items = items,
        //        isMes = headerList[0].IsMes,
        //        numberStatus = headerList[0].NumberStatus,
        //        transferType = headerList[0].TransferType,
        //        transferCatalog = headerList[0].TransferCatalog
        //    };
        //}

        //public List<AutoCompletedItem> AutoCompleteShipmentNumber(string TransactionType, string outSubInventory, string outLocator, string inSubInventory, string inLocator, string Prefix)
        //{
        //    if (TransactionType == "出貨編號")
        //    {
        //        var query = from stockTransferDT in model
        //                    where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
        //                    outLocator == stockTransferDT.OUT_LOCATOR_ID &&
        //                    inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
        //                    inLocator == stockTransferDT.IN_LOCATOR_ID &&
        //                    "非MES入庫手動新增" != stockTransferDT.NUMBER_STATUS &&
        //                    "非MES入庫檔案匯入" != stockTransferDT.NUMBER_STATUS &&
        //                    "非MES已入庫" != stockTransferDT.NUMBER_STATUS &&
        //                    stockTransferDT.SHIPMENT_NUMBER.Contains(Prefix)
        //                    group stockTransferDT by new { stockTransferDT.SHIPMENT_NUMBER } into g
        //                    select new AutoCompletedItem
        //                    {
        //                        Description = g.Key.SHIPMENT_NUMBER,
        //                        Value = g.Key.SHIPMENT_NUMBER
        //                    };

        //        return query.ToList();
        //    }
        //    else if (TransactionType == "移轉編號")
        //    {
        //        var query = from stockTransferDT in model
        //                    where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
        //                    outLocator == stockTransferDT.OUT_LOCATOR_ID &&
        //                    inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
        //                    inLocator == stockTransferDT.IN_LOCATOR_ID &&
        //                    "非MES入庫手動新增" != stockTransferDT.NUMBER_STATUS &&
        //                    "非MES入庫檔案匯入" != stockTransferDT.NUMBER_STATUS &&
        //                    "非MES已入庫" != stockTransferDT.NUMBER_STATUS &&
        //                    stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER.Contains(Prefix)
        //                    group stockTransferDT by new { stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER } into g
        //                    select new AutoCompletedItem
        //                    {
        //                        Description = g.Key.SUBINVENTORY_TRANSFER_NUMBER,
        //                        Value = g.Key.SUBINVENTORY_TRANSFER_NUMBER
        //                    };

        //        return query.ToList();
        //    }
        //    else
        //    {
        //        return new List<AutoCompletedItem>();
        //    }

        //}

        //public List<AutoCompletedItem> AutoCompleteShipmentNumber(string TransactionType, string outSubInventory, string outLocator, string inSubInventory, string inLocator, string Prefix, string NumberStatus)
        //{
        //    if (TransactionType == "出貨編號")
        //    {
        //        var query = from stockTransferDT in model
        //                    where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
        //                    outLocator == stockTransferDT.OUT_LOCATOR_ID &&
        //                    inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
        //                    inLocator == stockTransferDT.IN_LOCATOR_ID &&
        //                    NumberStatus != stockTransferDT.NUMBER_STATUS &&
        //                    stockTransferDT.SHIPMENT_NUMBER.Contains(Prefix)
        //                    group stockTransferDT by new { stockTransferDT.SHIPMENT_NUMBER } into g
        //                    select new AutoCompletedItem
        //                    {
        //                        Description = g.Key.SHIPMENT_NUMBER,
        //                        Value = g.Key.SHIPMENT_NUMBER
        //                    };

        //        return query.ToList();
        //    }
        //    else if (TransactionType == "移轉編號")
        //    {
        //        var query = from stockTransferDT in model
        //                    where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
        //                    outLocator == stockTransferDT.OUT_LOCATOR_ID &&
        //                    inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
        //                    inLocator == stockTransferDT.IN_LOCATOR_ID &&
        //                    NumberStatus != stockTransferDT.NUMBER_STATUS &&
        //                    stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER.Contains(Prefix)
        //                    group stockTransferDT by new { stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER } into g
        //                    select new AutoCompletedItem
        //                    {
        //                        Description = g.Key.SUBINVENTORY_TRANSFER_NUMBER,
        //                        Value = g.Key.SUBINVENTORY_TRANSFER_NUMBER
        //                    };

        //        return query.ToList();
        //    }
        //    else
        //    {
        //        return new List<AutoCompletedItem>();
        //    }

        //}



        public IEnumerable<SelectListItem> GetSubinventoryTransferNumberList(string outSubInventory, string outLocator, string inSubInventory, string inLocator)
        {
            List<ListItem> list = new List<ListItem>();

            list.Add(new ListItem("新增編號", "新增編號"));

            var query = from stockTransferDT in model
                        where outSubInventory == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        outLocator == stockTransferDT.OUT_LOCATOR_ID &&
                        inSubInventory == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        inLocator == stockTransferDT.IN_LOCATOR_ID
                        group stockTransferDT by new { stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER } into g
                        select new ListItem
                        {
                            Text = g.Key.SUBINVENTORY_TRANSFER_NUMBER,
                            Value = g.Key.SUBINVENTORY_TRANSFER_NUMBER
                        };
            list.AddRange(query.ToList());

            return list.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        /// <summary>
        /// 庫存移轉新增明細
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="shipmentNumber"></param>
        /// <param name="transferType"></param>
        /// <param name="itemNumber"></param>
        /// <param name="outOrganizationId"></param>
        /// <param name="outSubinventoryCode"></param>
        /// <param name="outLocatorId"></param>
        /// <param name="inOrganizationId"></param>
        /// <param name="inSubinventoryCode"></param>
        /// <param name="inLocatorId"></param>
        /// <param name="dataUpadteAuthority"></param>
        /// <param name="dataWriteType"></param>
        /// <param name="requestedQty"></param>
        /// <param name="rollReamWt"></param>
        /// <param name="lotNumber"></param>
        /// <param name="createUser"></param>
        /// <param name="createUserName"></param>
        /// <returns></returns>
        public ResultDataModel<TRF_HEADER_T> InboundCreateDetail(TransferUOW uow, string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
            string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId,
            string requestedQty, string rollReamWt, string lotNumber, string createUser, string createUserName)
        {
            var requestedQtyConvertResult = ConvertEx.StringToDecimal(requestedQty);
            if (!requestedQtyConvertResult.Success) return new ResultDataModel<TRF_HEADER_T>(false, "數量須為數字", null);

            var rollReamWtConvertResult = ConvertEx.StringToDecimal(rollReamWt);
            if (!rollReamWtConvertResult.Success) return new ResultDataModel<TRF_HEADER_T>(false, "每棧令數須為數字", null);

            return uow.InboundCreateDetail(shipmentNumber, transferType, itemNumber, outOrganizationId,
                outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId,
                TransferUOW.DataUpdateAuthority.Permit, TransferUOW.DataWriteType.KeyIn, requestedQtyConvertResult.Data, rollReamWtConvertResult.Data, lotNumber, createUser, createUserName);
        }

        public ResultDataModel<TRF_HEADER_T> OutboundCreateDetail(TransferUOW uow, string shipmentNumber, string transferType, string itemNumber, long outOrganizationId,
           string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId,
           string requestedQty, string rollReamQty, string createUser, string createUserName)
        {
            var requestedQtyConvertResult = ConvertEx.StringToDecimal(requestedQty);
            if (!requestedQtyConvertResult.Success) return new ResultDataModel<TRF_HEADER_T>(false, "數量須為數字", null);

            var rollReamQtyConvertResult = ConvertEx.StringToDecimal(rollReamQty);
            if (!rollReamQtyConvertResult.Success) return new ResultDataModel<TRF_HEADER_T>(false, "棧板數、捲數須為數字", null);

            return uow.OutboundCreateDetail(shipmentNumber, transferType, itemNumber, outOrganizationId,
                outSubinventoryCode, outLocatorId, inOrganizationId, inSubinventoryCode, inLocatorId,
                TransferUOW.DataUpdateAuthority.Permit, TransferUOW.DataWriteType.KeyIn, requestedQtyConvertResult.Data, rollReamQtyConvertResult.Data, createUser, createUserName);
        }

        public ResultModel OutboundCreatePick(TransferUOW uow, long transferHeaderId, long transferDetailId, string barcode,
        decimal reamQty, string createUser, string createUserName)
        {
            return uow.OutboundCreatePick(transferHeaderId, transferDetailId, barcode, reamQty, createUser, createUserName);
        }

        //public ResultModel CheckCreateDetail(TransferUOW uow, string shipmentNumber, string transferType, string itemNumber, long outOrganizationId, string outSubinventoryCode, long outLocatorId, long inOrganizationId, string inSubinventoryCode, long inLocatorId)
        //{
        //    var itemNumberOrganizationIdList = uow.GetItemNumberOrganizationId(itemNumber);
        //    if (itemNumberOrganizationIdList == null || itemNumberOrganizationIdList.Count == 0) return new ResultModel(false, "找不到料號所屬組織");

        //    if (transferType == TransferUOW.TransferType.InBound)
        //    {
        //        if (itemNumberOrganizationIdList.Where(x => x == inOrganizationId).ToList().Count == 0) return new ResultModel(false, "入庫組織沒有此料號");
        //    }
        //    else
        //    {
        //        if (itemNumberOrganizationIdList.Where(x => x == outOrganizationId).ToList().Count == 0) return new ResultModel(false, "出庫組織沒有此料號");
        //    }

        //    if (shipmentNumber == TransferUOW.DropDownListTypeValue.Add) return new ResultModel(true, "為新增編號");

        //    var trfHeaderList = uow.GetTrfHeader(shipmentNumber, transferType);
        //    if (trfHeaderList == null) return new ResultModel(false, "找不到出貨單資料");
        //    if (trfHeaderList.OrganizationId != outOrganizationId) return new ResultModel(false, "出庫組織比對錯誤，請檢查出庫倉庫是否選擇正確");
        //    if (trfHeaderList.SubinventoryCode != outSubinventoryCode) return new ResultModel(false, "出庫倉庫比對錯誤，請檢查出庫倉庫是否選擇正確");
        //    if (trfHeaderList.LocatorId != outLocatorId) return new ResultModel(false, "出庫儲位比對錯誤，請檢查出庫儲位是否選擇正確");
        //    if (trfHeaderList.TransferOrganizationId != inOrganizationId) return new ResultModel(false, "入庫組織比對錯誤，請檢查入庫倉庫是否選擇正確");
        //    if (trfHeaderList.TransferSubinventoryCode != inSubinventoryCode) return new ResultModel(false, "入庫倉庫比對錯誤，請檢查入庫倉庫是否選擇正確");
        //    if (trfHeaderList.TransferLocatorId != inLocatorId) return new ResultModel(false, "入庫儲位比對錯誤，請檢查入庫儲位是否選擇正確");

        //    var trfDeatilList = uow.GetTrfDetailList(trfHeaderList.TransferHeaderId, itemNumber);
        //    if (trfDeatilList != null && trfDeatilList.Count > 0) return new ResultModel(false, "料號重複輸入");

        //    return new ResultModel(true, "新增明細檢查正確");
        //}

        public ResultModel CheckNumber(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number)
        {
            //檢查編號是否重複
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SHIPMENT_NUMBER
                            select stockTransferDT;
                List<StockTransferDT> datalist = query.ToList();
                if (datalist.Count > 0)
                {
                    if (!(datalist[0].OUT_SUBINVENTORY_CODE == OUT_SUBINVENTORY_CODE &&
                        datalist[0].OUT_LOCATOR_ID == OUT_LOCATOR_ID &&
                        datalist[0].IN_SUBINVENTORY_CODE == IN_SUBINVENTORY_CODE &&
                        datalist[0].IN_LOCATOR_ID == IN_LOCATOR_ID))
                    {
                        return new ResultModel(false, "出貨編號不可重複輸入");
                    }
                    else
                    {
                        return new ResultModel(true, "通過檢查");
                    }
                }
                else
                {
                    return new ResultModel(true, "是否新增編號?");
                }
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                            select stockTransferDT;
                List<StockTransferDT> datalist = query.ToList();
                if (datalist.Count > 0)
                {
                    if (!(datalist[0].OUT_SUBINVENTORY_CODE == OUT_SUBINVENTORY_CODE &&
                        datalist[0].OUT_LOCATOR_ID == OUT_LOCATOR_ID &&
                        datalist[0].IN_SUBINVENTORY_CODE == IN_SUBINVENTORY_CODE &&
                        datalist[0].IN_LOCATOR_ID == IN_LOCATOR_ID))
                    {
                        return new ResultModel(false, "移轉編號不可重複輸入");
                    }
                    else
                    {
                        return new ResultModel(true, "通過檢查");
                    }

                }
                else
                {
                    return new ResultModel(true, "是否新增編號?");
                }
            }
            else
            {
                return new ResultModel(false, "無法識別異動類別");
            }
        }


        public string GetShipmentNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID)
        {
            var query = from stockTransferDT in model
                        where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                        IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID
                        orderby stockTransferDT.SHIPMENT_NUMBER descending
                        select stockTransferDT.SHIPMENT_NUMBER;
            var list = query.ToList();
            int lastNumber = 0;
            if (list.Count > 0)
            {
                lastNumber = Convert.ToInt32(list[0].Substring((list[0].Length - 3) > 0 ? list[0].Length - 3 : 0)); //取後三碼流水號
            }
            lastNumber = lastNumber + 1;

            return "(" + OUT_SUBINVENTORY_CODE + "-" + IN_SUBINVENTORY_CODE + ")" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", lastNumber);
        }

        public string GetSubinventoryTransferNumber(string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID)
        {
            var query = from stockTransferDT in model
                        where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                        OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                        IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                        IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID
                        orderby stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER descending
                        select stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER;
            var list = query.ToList();
            int lastNumber = 0;
            if (list.Count > 0)
            {
                lastNumber = Convert.ToInt32(list[0].Substring((list[0].Length - 3) > 0 ? list[0].Length - 3 : 0)); //取後三碼流水號
            }
            lastNumber = lastNumber + 1;

            return "(" + OUT_SUBINVENTORY_CODE + "-" + IN_SUBINVENTORY_CODE + ")" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", lastNumber);
        }

        public ResultModel SaveStockTransferDT(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
            string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal PICKED_QTY, string UNIT, decimal ROLL_REAM_QTY)
        {
            ////檢查編號是否重複
            //if (TransactionType == "出貨編號")
            //{
            //    var query = from stockTransferDT in model
            //                where Number == stockTransferDT.SHIPMENT_NUMBER 
            //                select stockTransferDT;
            //    List<StockTransferDT> datalist = query.ToList();
            //    if (datalist.Count > 0)
            //    {
            //        if (!(datalist[0].OUT_SUBINVENTORY_CODE == OUT_SUBINVENTORY_CODE &&
            //            datalist[0].OUT_LOCATOR_ID == OUT_LOCATOR_ID &&
            //            datalist[0].IN_SUBINVENTORY_CODE == IN_SUBINVENTORY_CODE &&
            //            datalist[0].IN_LOCATOR_ID == IN_LOCATOR_ID))
            //        {
            //            return new ResultModel(false, "出貨編號不可重複輸入");
            //        }
            //    }
            //    else
            //    {
            //        return new ResultModel(true, "是否新增出貨編號?");
            //    }
            //}
            if (Number == "新增編號" && TransactionType == "出貨編號")
            {
                Number = GetShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }
            else if (Number == "新增編號" && TransactionType == "移轉編號")
            {
                Number = GetSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }

            List<StockTransferDT> list = new List<StockTransferDT>();

            //檢查備貨單的料號是否存在
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SHIPMENT_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }

            if (list.Count > 0)
            {
                return new ResultModel(false, "此料號已存在備貨單中");
                //foreach(StockTransferDT stockTransferDT in model)
                //{
                //    if (list[0].ID == stockTransferDT.ID)
                //    {
                //        if (UNIT == stockTransferDT.REQUESTED_QUANTITY_UOM)
                //        {
                //            stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                //            stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                //        }
                //        else if (UNIT == stockTransferDT.SRC_REQUESTED_QUANTITY_UOM2)
                //        {
                //            stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY;
                //            stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY;
                //        }
                //        stockTransferDT.ROLL_REAM_QTY = ROLL_REAM_QTY;
                //    }
                //}
            }
            else
            {
                List<StockDT> itemList = StockData.GetStockItemData(OUT_SUBINVENTORY_CODE, ITEM_NUMBER);
                if (itemList.Count == 0)
                {
                    return new ResultModel(false, "搜尋不到料號" + ITEM_NUMBER + "資料");
                }

                var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                StockTransferDT stockTransferDT = new StockTransferDT();
                stockTransferDT.ID = highestId + 1;
                stockTransferDT.OUT_SUBINVENTORY_CODE = OUT_SUBINVENTORY_CODE;
                stockTransferDT.OUT_LOCATOR_ID = OUT_LOCATOR_ID;
                stockTransferDT.IN_SUBINVENTORY_CODE = IN_SUBINVENTORY_CODE;
                stockTransferDT.IN_LOCATOR_ID = IN_LOCATOR_ID;
                if (TransactionType == "出貨編號")
                {
                    stockTransferDT.SHIPMENT_NUMBER = Number;
                }
                else if (TransactionType == "移轉編號")
                {
                    stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER = Number;
                }
                stockTransferDT.ITEM_NUMBER = ITEM_NUMBER;
                stockTransferDT.PACKING_TYPE = itemList[0].PACKING_TYPE;
                if (UNIT == "KG")
                {
                    if (itemList[0].ITEM_CATEGORY == "捲筒")
                    {
                        stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                        stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.REQUESTED_QUANTITY2 = 0;
                        stockTransferDT.PICKED_QUANTITY2 = 0;
                        stockTransferDT.INBOUND_PICKED_QUANTITY2 = 0;
                        stockTransferDT.ROLL_REAM_UOM = "捲";
                    }
                    else if (itemList[0].ITEM_CATEGORY == "平版")
                    {
                        stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                        stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                        stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY / 10;
                        stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY / 10;
                        stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY / 10;
                        stockTransferDT.ROLL_REAM_UOM = "板";
                    }
                }
                else if (UNIT == "RE")
                {
                    stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY * 10;
                    stockTransferDT.PICKED_QUANTITY = PICKED_QTY * 10;
                    stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY * 10;
                    stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY;
                    stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY;
                    stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY;
                    stockTransferDT.ROLL_REAM_UOM = "板";
                }
                stockTransferDT.REQUESTED_QUANTITY_UOM = itemList[0].PRIMARY_UOM_CODE;
                stockTransferDT.REQUESTED_QUANTITY_UOM2 = itemList[0].SECONDARY_UOM_CODE;
                stockTransferDT.ROLL_REAM_QTY = ROLL_REAM_QTY;
                stockTransferDT.NUMBER_STATUS = "MES未出庫";
                model.Add(stockTransferDT);
                return new ResultModel(true, Number);
            }
        }

        public ResultModel CreateInbound(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID,
          ref string Number, string ITEM_NUMBER, decimal REQUESTED_QTY, decimal PICKED_QTY, string UNIT, decimal ROLL_REAM_QTY, string ITEM_CATEGORY, string NUMBER_STATUS)
        {
            if (Number == "新增編號" && TransactionType == "出貨編號")
            {
                Number = GetShipmentNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }
            else if (Number == "新增編號" && TransactionType == "移轉編號")
            {
                Number = GetSubinventoryTransferNumber(OUT_SUBINVENTORY_CODE, OUT_LOCATOR_ID, IN_SUBINVENTORY_CODE, IN_LOCATOR_ID);
            }
            List<StockTransferDT> list = new List<StockTransferDT>();

            string newNumber = Number;
            //檢查入庫單的料號是否存在
            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where newNumber == stockTransferDT.SHIPMENT_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where newNumber == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER &&
                            ITEM_NUMBER == stockTransferDT.ITEM_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }

            if (list.Count > 0)
            {
                var data = model.FirstOrDefault(d => d.ID == list[0].ID);
                if (data != null)
                {
                    if (UNIT == "KG")
                    {
                        if (ITEM_CATEGORY == "捲筒")
                        {
                            data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + REQUESTED_QTY;
                            //data.INBOUND_PICKED_QUANTITY = data.INBOUND_PICKED_QUANTITY + PICKED_QTY;
                            //data.PICKED_QUANTITY = PICKED_QTY;
                            //data.REQUESTED_QUANTITY2 = 0;
                            //data.PICKED_QUANTITY2 = 0;
                            //data.INBOUND_PICKED_QUANTITY2 = 0;
                        }
                        else if (ITEM_CATEGORY == "平版")
                        {
                            data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + REQUESTED_QTY;
                            //stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                            //data.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                            data.REQUESTED_QUANTITY2 = data.REQUESTED_QUANTITY2 + (REQUESTED_QTY / 10);
                            //data.PICKED_QUANTITY2 = PICKED_QTY / 10;
                            //data.INBOUND_PICKED_QUANTITY2 = PICKED_QTY / 10;
                        }
                    }
                    else if (UNIT == "RE")
                    {
                        data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + (REQUESTED_QTY * 10);
                        //data.PICKED_QUANTITY = PICKED_QTY * 10;
                        //data.INBOUND_PICKED_QUANTITY = PICKED_QTY * 10;
                        data.REQUESTED_QUANTITY2 = data.REQUESTED_QUANTITY2 + REQUESTED_QTY;
                        //data.PICKED_QUANTITY2 = PICKED_QTY;
                        //data.INBOUND_PICKED_QUANTITY2 = PICKED_QTY;
                    }
                    data.ROLL_REAM_QTY = data.ROLL_REAM_QTY + ROLL_REAM_QTY;
                    //data.NUMBER_STATUS = "非MES入庫手動新增";
                    data.NUMBER_STATUS = NUMBER_STATUS;
                    return new ResultModel(true, list[0].ID.ToString());
                }
                else
                {
                    return new ResultModel(false, "無法取得入庫單料號");
                }
            }
            else
            {
                List<StockDT> itemList = StockData.GetStockItemData(IN_SUBINVENTORY_CODE, ITEM_NUMBER);
                if (itemList.Count == 0)
                {
                    return new ResultModel(false, "搜尋不到料號" + ITEM_NUMBER + "資料");
                }
                else
                {
                    var highestId = model.Any() ? model.Select(x => x.ID).Max() : 0;
                    StockTransferDT stockTransferDT = new StockTransferDT();
                    stockTransferDT.ID = highestId + 1;
                    stockTransferDT.OUT_SUBINVENTORY_CODE = OUT_SUBINVENTORY_CODE;
                    stockTransferDT.OUT_LOCATOR_ID = OUT_LOCATOR_ID;
                    stockTransferDT.IN_SUBINVENTORY_CODE = IN_SUBINVENTORY_CODE;
                    stockTransferDT.IN_LOCATOR_ID = IN_LOCATOR_ID;
                    if (TransactionType == "出貨編號")
                    {
                        stockTransferDT.SHIPMENT_NUMBER = Number;
                    }
                    else if (TransactionType == "移轉編號")
                    {
                        stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER = Number;
                    }
                    stockTransferDT.ITEM_NUMBER = ITEM_NUMBER;
                    stockTransferDT.PACKING_TYPE = itemList[0].PACKING_TYPE;
                    if (UNIT == "KG")
                    {
                        if (itemList[0].ITEM_CATEGORY == "捲筒")
                        {
                            stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                            stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.REQUESTED_QUANTITY2 = 0;
                            stockTransferDT.PICKED_QUANTITY2 = 0;
                            stockTransferDT.INBOUND_PICKED_QUANTITY2 = 0;
                        }
                        else if (itemList[0].ITEM_CATEGORY == "平版")
                        {
                            stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY;
                            stockTransferDT.PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY;
                            stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY / 10;
                            stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY / 10;
                            stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY / 10;
                        }
                    }
                    else if (UNIT == "RE")
                    {
                        stockTransferDT.REQUESTED_QUANTITY = REQUESTED_QTY * 10;
                        stockTransferDT.PICKED_QUANTITY = PICKED_QTY * 10;
                        stockTransferDT.INBOUND_PICKED_QUANTITY = PICKED_QTY * 10;
                        stockTransferDT.REQUESTED_QUANTITY2 = REQUESTED_QTY;
                        stockTransferDT.PICKED_QUANTITY2 = PICKED_QTY;
                        stockTransferDT.INBOUND_PICKED_QUANTITY2 = PICKED_QTY;
                    }
                    stockTransferDT.REQUESTED_QUANTITY_UOM = itemList[0].PRIMARY_UOM_CODE;
                    stockTransferDT.REQUESTED_QUANTITY_UOM2 = itemList[0].SECONDARY_UOM_CODE;
                    stockTransferDT.ROLL_REAM_QTY = ROLL_REAM_QTY;
                    //stockTransferDT.NUMBER_STATUS = "非MES入庫手動新增";
                    stockTransferDT.NUMBER_STATUS = NUMBER_STATUS;
                    model.Add(stockTransferDT);
                    return new ResultModel(true, stockTransferDT.ID.ToString());
                }
            }

        }

        public ResultModel UpdateStockTransferDT(long ID, decimal PRIMARY_QUANTITY, decimal SECONDARY_QUANTITY, bool remove, bool isInbound, string barcodeStatus)
        {
            foreach (StockTransferDT stockTransferDT in model)
            {
                if (isInbound)
                {
                    if (stockTransferDT.ID == ID)
                    {
                        stockTransferDT.REQUESTED_QUANTITY = stockTransferDT.REQUESTED_QUANTITY + PRIMARY_QUANTITY;
                        stockTransferDT.REQUESTED_QUANTITY2 = stockTransferDT.REQUESTED_QUANTITY2 + SECONDARY_QUANTITY;

                        if (barcodeStatus == "已入庫")
                        {
                            stockTransferDT.INBOUND_PICKED_QUANTITY = stockTransferDT.INBOUND_PICKED_QUANTITY + PRIMARY_QUANTITY;
                            stockTransferDT.INBOUND_PICKED_QUANTITY2 = stockTransferDT.INBOUND_PICKED_QUANTITY2 + SECONDARY_QUANTITY;
                        }
                        if (remove)
                        {
                            if (stockTransferDT.INBOUND_PICKED_QUANTITY == 0 && stockTransferDT.INBOUND_PICKED_QUANTITY2 == 0)
                            {
                                model.Remove(stockTransferDT);
                            }
                        }

                        //更新棧板數
                        if (PRIMARY_QUANTITY > 0)
                        {
                            stockTransferDT.ROLL_REAM_QTY = stockTransferDT.ROLL_REAM_QTY + 1;
                        }
                        else
                        {
                            stockTransferDT.ROLL_REAM_QTY = stockTransferDT.ROLL_REAM_QTY - 1;
                        }
                        return new ResultModel(true, "更新入庫單成功");
                    }
                }
                else
                {
                    if (stockTransferDT.ID == ID)
                    {
                        stockTransferDT.PICKED_QUANTITY = stockTransferDT.PICKED_QUANTITY + PRIMARY_QUANTITY;
                        stockTransferDT.PICKED_QUANTITY2 = stockTransferDT.PICKED_QUANTITY2 + SECONDARY_QUANTITY;
                        if (remove)
                        {
                            if (stockTransferDT.PICKED_QUANTITY == 0 && stockTransferDT.PICKED_QUANTITY2 == 0)
                            {
                                model.Remove(stockTransferDT);
                            }
                        }

                        return new ResultModel(true, "更新備貨單成功");
                    }
                }
            }
            return new ResultModel(false, "找不到備貨單更新");
        }

        public ResultModel DeleteNumber(string TransactionType, string Number)
        {
            if (TransactionType == "出貨編號")
            {
                var stockTransferDTquery = from stockTransferDT in model
                                           where Number == stockTransferDT.SHIPMENT_NUMBER
                                           select stockTransferDT;
                var stockTransferDTList = stockTransferDTquery.ToList();

                if (stockTransferDTList.Count == 0)
                {
                    return new ResultModel(true, "沒有編號資料可以刪除");
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    StockTransferBarcodeData.model.RemoveAll((x) => x.TransferDetailId == data.ID);
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    model.Remove(data);
                }

                return new ResultModel(true, "此編號所有資料刪除成功");
            }
            else if (TransactionType == "移轉編號")
            {
                var stockTransferDTquery = from stockTransferDT in model
                                           where Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                                           select stockTransferDT;
                var stockTransferDTList = stockTransferDTquery.ToList();

                if (stockTransferDTList.Count == 0)
                {
                    return new ResultModel(true, "沒有編號資料可以刪除");
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    StockTransferBarcodeData.model.RemoveAll((x) => x.TransferDetailId == data.ID);
                }

                foreach (StockTransferDT data in stockTransferDTList)
                {
                    model.Remove(data);
                }

                return new ResultModel(true, "此編號所有資料刪除成功");
            }
            else
            {
                return new ResultModel(false, "無法識別異動類別");
            }
        }

        public ResultModel DeleteItemNumber(long ID, bool isInbound)
        {
            var query = from data in StockTransferBarcodeData.model
                        where ID == data.TransferDetailId
                        select data.ID;
            var removeBarcodeIDs = query.ToList();
            if (removeBarcodeIDs.Count > 0)
            {
                bool removeResult = false;
                bool removeResult2 = false;
                var barcodeQuery = from data in StockTransferBarcodeData.model
                                   where removeBarcodeIDs.Contains(data.ID)
                                   select data;
                var removeList = barcodeQuery.ToList();
                if (removeList.Count == 0)
                {
                    return new ResultModel(false, "找不到條碼刪除");
                }

                foreach (StockTransferBarcodeDT barcode in removeList)
                {
                    removeResult = UpdateStockTransferDT(barcode.TransferDetailId, -barcode.PRIMARY_QUANTITY, -barcode.SECONDARY_QUANTITY, true, isInbound, barcode.Status).Success;
                    removeResult2 = StockTransferBarcodeData.model.Remove(barcode);
                }
                if (removeResult && removeResult2)
                {
                    if (model.Count > 0)
                    {
                        return new ResultModel(true, "刪除料號成功");
                    }
                    else
                    {
                        return new ResultModel(true, "刪除料號成功，備貨單已沒任何資料");
                    }

                }
                else
                {
                    return new ResultModel(false, "刪除料號失敗");
                }
            }
            else
            {
                var itemQuery = from data in model
                                where ID == data.ID
                                select data;
                var removeList = itemQuery.ToList();
                if (removeList.Count > 0)
                {
                    if (model.Remove(removeList[0]))
                    {
                        if (model.Count > 0)
                        {
                            return new ResultModel(true, "刪除料號成功");
                        }
                        else
                        {
                            return new ResultModel(true, "刪除料號成功，備貨單已沒任何資料");
                        }
                    }
                    else
                    {
                        return new ResultModel(false, "料號刪除失敗");
                    }
                }
                else
                {
                    return new ResultModel(false, "料號刪除失敗");
                }
            }


        }



        public List<StockTransferBarcodeDT> GetInboundPickedData(TransferUOW uow, long transferHeaderId, string numberStatus)
        {
            return uow.GetTrfInboundPickedTList(transferHeaderId, numberStatus);
        }

        public List<StockTransferDT> GetOutboundDetailData(TransferUOW uow, long transferHeaderId, string numberStatus)
        {
            return uow.GetTrfOutboundDetailTList(transferHeaderId, numberStatus);
        }

        public List<StockTransferBarcodeDT> GetOutboundPickedData(TransferUOW uow, long transferHeaderId, string numberStatus)
        {
            return uow.GetTrfOutboundPickedTList(transferHeaderId, numberStatus);
        }

        public ResultDataModel<TRF_HEADER_T> GetShipmentNumberData(TransferUOW uow, long transferHeaderId)
        {
            if (transferHeaderId == 0)
            {
                return new ResultDataModel<TRF_HEADER_T>(true, "新增編號", null);
            }

            var trfHeader = uow.GetTrfHeader(transferHeaderId);
            if (trfHeader == null)
            {
                return new ResultDataModel<TRF_HEADER_T>(false, "找不到出貨編號資料", null);
            }
            else
            {
                return new ResultDataModel<TRF_HEADER_T>(true, "找到出貨編號資料", trfHeader);
            }
        }

        public ResultModel GetNumberStatus(string TransactionType, string OUT_SUBINVENTORY_CODE, string OUT_LOCATOR_ID, string IN_SUBINVENTORY_CODE, string IN_LOCATOR_ID, string Number)
        {
            List<StockTransferDT> list = new List<StockTransferDT>();

            if (TransactionType == "出貨編號")
            {
                var query = from stockTransferDT in model
                            where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                            IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&
                            Number == stockTransferDT.SHIPMENT_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else if (TransactionType == "移轉編號")
            {
                var query = from stockTransferDT in model
                            where OUT_SUBINVENTORY_CODE == stockTransferDT.OUT_SUBINVENTORY_CODE &&
                            OUT_LOCATOR_ID == stockTransferDT.OUT_LOCATOR_ID &&
                            IN_SUBINVENTORY_CODE == stockTransferDT.IN_SUBINVENTORY_CODE &&
                            IN_LOCATOR_ID == stockTransferDT.IN_LOCATOR_ID &&
                            Number == stockTransferDT.SUBINVENTORY_TRANSFER_NUMBER
                            select stockTransferDT;
                list = query.ToList();
            }
            else
            {
                return new ResultModel(false, "無法識別異動類別");
            }

            if (list.Count > 0)
            {
                return new ResultModel(true, list[0].NUMBER_STATUS);
            }
            else
            {
                return new ResultModel(true, "新增"); //沒編號資料回傳狀態回新增
            }

        }


        public List<StockTransferDT> GetStockTransferData(long ID)
        {
            var query = from stockTransferDT in model
                        where ID == stockTransferDT.ID
                        select stockTransferDT;
            return query.ToList();
        }

        /// <summary>
        /// 出庫存檔
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="transferHeaderId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultModel OutBoundSaveTransfer(TransferUOW uow, long transferHeaderId, string userId, string userName)
        {
            return uow.OutBoundSaveTransfer(transferHeaderId, userId, userName);
        }

        /// <summary>
        /// 入庫存檔
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="transferHeaderId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultModel InBoundSaveTransfer(TransferUOW uow, long transferHeaderId, string userId, string userName)
        {
            return uow.InBoundSaveTransfer(transferHeaderId, userId, userName, true);
        }

        public ResultModel InBoundSaveTransferNoCheckStockStatus(TransferUOW uow, long transferHeaderId, string userId, string userName)
        {
            return uow.InBoundSaveTransfer(transferHeaderId, userId, userName, false);
        }

        
        

        public ResultModel MergeBarcode(long ID, decimal PRIMARY_QUANTITY, decimal SECONDARY_QUANTITY, decimal addROLL_REAM_QTY)
        {
            var data = model.FirstOrDefault(d => d.ID == ID);
            if (data != null)
            {
                data.REQUESTED_QUANTITY = data.REQUESTED_QUANTITY + PRIMARY_QUANTITY;
                data.REQUESTED_QUANTITY2 = data.REQUESTED_QUANTITY2 + SECONDARY_QUANTITY;
                data.ROLL_REAM_QTY = data.ROLL_REAM_QTY + addROLL_REAM_QTY;

                return new ResultModel(true, "更改入庫單數量成功");
            }
            else
            {
                return new ResultModel(true, "更改入庫單數量失敗，找不到ID");
            }
        }

        //public ResultModel DelROLL_REAM_QTY(long ID, decimal PRIMARY_QUANTITY, decimal SECONDARY_QUANTITY, bool addNewBarcode)
        //{

        //}

        public ResultModel InboundPickEditor(TransferUOW uow, PickEditor pickEditor, string userId, string userName)
        {
            if (pickEditor.Action == "remove")
            {
                var transferPickedIdList = pickEditor.StockTransferBarcodeDTList.Select(x => x.ID).ToList();
                return uow.DelInboundPickData(transferPickedIdList, userId, userName);
            }
            else if (pickEditor.Action == "edit")
            {
                var transferPickedIdList = pickEditor.StockTransferBarcodeDTList.Select(x => x.ID).ToList();
                string note = pickEditor.StockTransferBarcodeDTList[0].REMARK;
                return uow.UpdateInboundPickNote(transferPickedIdList, note, userId, userName);
            }
            else
            {
                return new ResultModel(false, "無法識別作業項目");
            }
        }

        public ResultModel OutboundDetailEditor(TransferUOW uow, DetailEditor detailEditor, string userId, string userName)
        {
            if (detailEditor.Action == "remove")
            {
                var transferDetailIdList = detailEditor.StockTransferDTList.Select(x => x.ID).ToList();
                return uow.DelOutboundDetailData(transferDetailIdList, userId, userName);
            }
            else
            {
                return new ResultModel(false, "無法識別作業項目");
            }
        }

        public ResultModel OutboundPickEditor(TransferUOW uow, PickEditor pickEditor, string userId, string userName)
        {
            if (pickEditor.Action == "remove")
            {
                var transferDetailIdList = pickEditor.StockTransferBarcodeDTList.Select(x => x.ID).ToList();
                return uow.DelOutboundPickedData(transferDetailIdList, userId, userName);
            }
            else if (pickEditor.Action == "edit")
            {
                var transferPickedIdList = pickEditor.StockTransferBarcodeDTList.Select(x => x.ID).ToList();
                string note = pickEditor.StockTransferBarcodeDTList[0].REMARK;
                return uow.UpdateOutboundPickNote(transferPickedIdList, note, userId, userName);
            }
            else
            {
                return new ResultModel(false, "無法識別作業項目");
            }
        }

        public ResultModel ChangeToAlreadyInBound(TransferUOW uow, long transferHeaderId, string barcode, string userId, string userName)
        {
            return uow.ChangeToAlreadyInBound(transferHeaderId, barcode, userId, userName);
        }

        public ActionResult PrintInboundLabel(TransferUOW uow, List<long> transferPickedIdList, string userName)
        {
            var resultData = uow.GetInboundLabels(transferPickedIdList, userName);
            return uow.PrintLabel(resultData.Data);
        }

        public ActionResult PrintOutboundLabel(TransferUOW uow, List<long> transferPickedIdList, string userName)
        {
            var resultData = uow.GetOutboundLabels(transferPickedIdList, userName);
            return uow.PrintLabel(resultData.Data);
        }

        public ResultModel WaitPrintToWaitInbound(TransferUOW uow, List<long> transferPickedIdList, string userId, string userName)
        {
            return uow.WaitPrintToWaitInbound(transferPickedIdList, userId, userName);
        }

        public ResultDataModel<TRF_HEADER_T> OutBoundToInbound(TransferUOW uow, long transferHeaderId, string userId, string userName)
        {
            return uow.OutBoundToInbound(transferHeaderId, userId, userName);
        }

            #region 併板

            public MergeBarcodeViewModel GetMergeBarcodeViewModel(TransferUOW uow, List<long> transferPickedIdList)
        {
            MergeBarcodeViewModel vieModel = new MergeBarcodeViewModel();
            vieModel.WaitMergeBarcodeList = new List<TRF_INBOUND_PICKED_T>();

            var waitMergeBarcodeList = uow.GetTrfInboundPickedList(transferPickedIdList);

            if (waitMergeBarcodeList.Count != 0)
            {
                vieModel.WaitMergeBarcodeList = waitMergeBarcodeList;
            }
            return vieModel;

        }

        public JsonResult GetMergeBarocdeStatus(TransferUOW uow, string MergeBarocde, List<long> waitMergeIDs)
        {
            try
            {
                var mergeBarocdeData = uow.GetStock(MergeBarocde);
                var waitMergeBarcodeDataList = uow.GetTrfInboundPickedList(waitMergeIDs);

                ResultModel checkResult = checkMergeBarcode(mergeBarocdeData, waitMergeBarcodeDataList);
                if (!checkResult.Success)
                {
                    return new JsonResult { Data = new { status = checkResult.Success, result = checkResult.Msg } };
                }

                decimal waitMergeTotalQty = 0;
                foreach (TRF_INBOUND_PICKED_T data in waitMergeBarcodeDataList)
                {
                    waitMergeTotalQty = waitMergeTotalQty + (decimal)data.SecondaryQuantity;
                }

                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        OriginalBarcode = mergeBarocdeData.Barcode,
                        OriginalQty = mergeBarocdeData.SecondaryAvailableQty,
                        OriginalUnit = mergeBarocdeData.SecondaryUomCode,
                        AfterBarcode = mergeBarocdeData.Barcode,
                        AfterQty = mergeBarocdeData.SecondaryAvailableQty + waitMergeTotalQty,
                        AfterUnit = mergeBarocdeData.SecondaryUomCode
                    }
                };
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new JsonResult { Data = new { status = false, result = "失敗:" + ex.Message } };
            }


        }

        public ResultModel checkMergeBarcode(STOCK_T mergeBarocdeData, List<TRF_INBOUND_PICKED_T> waitMergeBarcodeDataList)
        {
            if (mergeBarocdeData == null)
            {
                return new ResultModel(false, "找不到條碼資料");
            }

            if (mergeBarocdeData.StatusCode != TransferUOW.StockStatusCode.InStock)
            {
                return new ResultModel(false, "此條碼不在庫");
            }

            if (waitMergeBarcodeDataList.Count == 0)
            {
                return new ResultModel(false, "找不到待併板條碼資料");
            }

            if (mergeBarocdeData.ItemCategory == TransferUOW.ItemCategory.Roll)
            {
                return new ResultModel(false, "捲筒不可併板");
            }

            if (mergeBarocdeData.PackingType != TransferUOW.PackingType.Ream)
            {
                return new ResultModel(false, "打包方式不是令包不可併板");
            }

            foreach (TRF_INBOUND_PICKED_T data in waitMergeBarcodeDataList)
            {
                if (data.ItemNumber != mergeBarocdeData.ItemNumber)
                {
                    return new ResultModel(false, "併板料號須相同");
                }
            }

            return new ResultModel(true, "併板條碼檢查成功");
        }

        public ResultModel MergeBarcode(TransferUOW uow, string MergeBarocde, List<long> waitMergeIDs, string userId, string userName)
        {
            var mergeBarocdeData = uow.GetStock(MergeBarocde);
            var waitMergeBarcodeDataList = uow.GetTrfInboundPickedList2(waitMergeIDs);

            ResultModel checkResult = checkMergeBarcode(mergeBarocdeData, waitMergeBarcodeDataList);
            if (!checkResult.Success)
            {
                return checkResult;
            }

            return uow.MergeBarcode(mergeBarocdeData, waitMergeBarcodeDataList, userId, userName);
        }

        #endregion

        #region 庫存移轉-入庫 Excel匯入
        public ResultDataModel<TRF_HEADER_T> InboundImportExcel(TransferUOW uow, List<InboundImportExcelModel> excelList, string shipmentNumber, string transferType,
            long outOrganizationId, string outSubinventoryCode, long? outLocatorId, long inOrganizationId, string inSubinventoryCode, long? inLocatorId, string userId, string userName)
        {
            return uow.InboundImportExcel(shipmentNumber, transferType, outOrganizationId, outSubinventoryCode, outLocatorId, inOrganizationId,
               inSubinventoryCode, inLocatorId, excelList, userId, userName);

        }

        #endregion


        public List<StockDT> SearchStock(TransferUOW uow, long organizationId, string subinventoryCode, long? locatorId, string itemNumber)
        {
            return uow.GetStockTList(organizationId, subinventoryCode, locatorId, itemNumber);
        }

        public ResultModel SaveReason(TransferUOW uow, HttpFileCollectionBase file, long stockId, string reasonCode, long? transferLocatorId, string note, string userId, string userName)
        {
            return uow.SaveReason(file, stockId, reasonCode, transferLocatorId, note, userId, userName);
        }

        #region 報表

        public ResultDataModel<ReportViewer> LocalOutboundPickingReportViewer(TransferUOW uow, string shipmentNumber)
        {
            try
            {
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("SHIPMENT_NUMBER", shipmentNumber, false));

                var report = new ReportViewer();
                // Set the processing mode for the ReportViewer to Local  
                report.ProcessingMode = ProcessingMode.Local;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;

                LocalReport localReport = report.LocalReport;
                localReport.ReportPath = "Report/OutboundPickingList.rdlc";

                var reportDataSourceResult = uow.GetOutboundPickingListReportDataSource(shipmentNumber);
                if (!reportDataSourceResult.Success) return new ResultDataModel<ReportViewer>(false, reportDataSourceResult.Msg, null);
                localReport.DataSources.Add(reportDataSourceResult.Data);

                // Set the report parameters for the report  
                localReport.SetParameters(paramList);

                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得庫存移轉-備貨單報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得庫存移轉-備貨單報表失敗:" + ex.Message, null);
            }
        }

        public ResultDataModel<ReportViewer> RemoteOutboundPickingReportViewer(string shipmentNumber)
        {
            try
            {
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("SHIPMENT_NUMBER", shipmentNumber, false));

                var report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;
                report.ServerReport.ReportPath = "/開發區/CHPOUSMES/OutboundPickingList.rdl";
                report.ServerReport.ReportServerUrl = new Uri("http://rs.yfy.com/reports/");
                report.ServerReport.SetParameters(paramList);
                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得庫存移轉-備貨單報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得庫存移轉-備貨單報表失敗:" + ex.Message, null);
            }
        }

        public ResultDataModel<ReportViewer> LocalInboundPickingReportViewer(TransferUOW uow, string shipmentNumber)
        {
            try
            {
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("SHIPMENT_NUMBER", shipmentNumber, false));

                var report = new ReportViewer();
                // Set the processing mode for the ReportViewer to Local  
                report.ProcessingMode = ProcessingMode.Local;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;

                LocalReport localReport = report.LocalReport;
                localReport.ReportPath = "Report/InboundPickingList.rdlc";

                var reportDataSourceResult = uow.GetInboundPickingListReportDataSource(shipmentNumber);
                if (!reportDataSourceResult.Success) return new ResultDataModel<ReportViewer>(false, reportDataSourceResult.Msg, null);
                localReport.DataSources.Add(reportDataSourceResult.Data);

                // Set the report parameters for the report  
                localReport.SetParameters(paramList);

                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得庫存移轉-入庫單報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得庫存移轉-入庫單報表失敗:" + ex.Message, null);
            }
        }

        public ResultDataModel<ReportViewer> RemoteInboundPickingReportViewer(string shipmentNumber)
        {
            try
            {
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("SHIPMENT_NUMBER", shipmentNumber, false));

                var report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;
                report.ServerReport.ReportPath = "/開發區/CHPOUSMES/InboundPickingList.rdl";
                report.ServerReport.ReportServerUrl = new Uri("http://rs.yfy.com/reports/");
                report.ServerReport.SetParameters(paramList);
                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得庫存移轉-入庫單報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得庫存移轉-入庫單報表失敗:" + ex.Message, null);
            }
        }

        #endregion
    }


    internal class StockTransferDTOrder
    {
        public static IOrderedEnumerable<StockTransferDT> Order(List<Order> orders, IEnumerable<StockTransferDT> models)
        {
            IOrderedEnumerable<StockTransferDT> orderedModel = null;
            if (orders.Count() > 0)
            {
                orderedModel = OrderBy(orders[0].Column, orders[0].Dir, models);
            }

            for (int i = 1; i < orders.Count(); i++)
            {
                orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
            }
            return orderedModel;
        }


        private static IOrderedEnumerable<StockTransferDT> OrderBy(int column, string dir, IEnumerable<StockTransferDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ID) : models.OrderBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PACKING_TYPE) : models.OrderBy(x => x.PACKING_TYPE);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ROLL_REAM_QTY) : models.OrderBy(x => x.ROLL_REAM_QTY);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY) : models.OrderBy(x => x.REQUESTED_QUANTITY);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY) : models.OrderBy(x => x.PICKED_QUANTITY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY2) : models.OrderBy(x => x.REQUESTED_QUANTITY2);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PICKED_QUANTITY2) : models.OrderBy(x => x.PICKED_QUANTITY2);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.REQUESTED_QUANTITY_UOM2) : models.OrderBy(x => x.REQUESTED_QUANTITY_UOM2);

            }
        }

        private static IOrderedEnumerable<StockTransferDT> ThenBy(int column, string dir, IOrderedEnumerable<StockTransferDT> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ID) : models.ThenBy(x => x.ID);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PACKING_TYPE) : models.ThenBy(x => x.PACKING_TYPE);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ROLL_REAM_QTY) : models.ThenBy(x => x.ROLL_REAM_QTY);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY) : models.ThenBy(x => x.REQUESTED_QUANTITY);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY) : models.ThenBy(x => x.PICKED_QUANTITY);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY_UOM) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY2) : models.ThenBy(x => x.REQUESTED_QUANTITY2);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PICKED_QUANTITY2) : models.ThenBy(x => x.PICKED_QUANTITY2);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.REQUESTED_QUANTITY_UOM2) : models.ThenBy(x => x.REQUESTED_QUANTITY_UOM2);

            }
        }
    }

    public class PickEditor
    {
        public string Action { get; set; }
        public List<StockTransferBarcodeDT> StockTransferBarcodeDTList { get; set; }
    }

    public class DetailEditor
    {

        public string Action { get; set; }
        public List<StockTransferDT> StockTransferDTList { get; set; }
    }

    public class InboundImportExcelModel
    {
        /// <summary>
        /// 料號
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// 數量;平版時填總令數,捲筒時為重量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 捲號;平版時填空字串("")
        /// </summary>
        public string LotNumber { get; set; }
        /// <summary>
        /// 每件令數;紙捲時填0
        /// </summary>
        public decimal RollReamWt { get; set; }


    }
}