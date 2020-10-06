using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entity;
using CHPOUTSRCMES.Web.DataModel.Entity.Process;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.Process
{
    public class ProcessViewModel
    {
        public Invest Invest { set; get; }
        public Cotangent Cotangent { set; get; }
        public CHP_PROCESS_T CHP_PROCESS_T { set; get; }
        public Production Production { set; get; }
        public YieldVariance YieldVariance { set; get; }
        public Boolean Authority { set; get; }

        /// <summary>
        /// 取得畫面資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CHP_PROCESS_T GetViewModel(long id)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetViewModel(id);
            }
        }
        /// <summary>
        /// 寫入備註
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        public ResultModel SetEditNote(long OspHeaderId, string Note, string userId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetEditNote(OspHeaderId, Note, userId);
            }
        }
        /// <summary>
        /// 更改備註裁切日期
        /// </summary>
        /// <returns></returns>
        public ResultModel SetStatusAndCutDate(long OspHeaderId, DateTime Dialog_CuttingDateFrom, DateTime Dialog_CuttingDateTo, 
            string Dialog_MachineNum, string BtnStatus, string UserId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetStatusAndCutDate(OspHeaderId, Dialog_CuttingDateFrom, Dialog_CuttingDateTo, Dialog_MachineNum, BtnStatus, UserId);
            }
        }
        /// <summary>
        /// 關帳用
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public ResultModel SetClose(long OspHeaderId, string BtnStatus)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetClose(OspHeaderId, BtnStatus);
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="BatchNo"></param>
        /// <param name="MachineNum"></param>
        /// <param name="DueDate"></param>
        /// <param name="CuttingDateFrom"></param>
        /// <param name="CuttingDateTo"></param>
        /// <param name="Subinventory"></param>
        /// <returns></returns>
        public List<CHP_PROCESS_T> Search(string Status, string BatchNo, string MachineNum, string DueDate, string CuttingDateFrom, string CuttingDateTo, string Subinventory)
        {
       
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetTable(Status, BatchNo, MachineNum, DueDate, CuttingDateFrom, CuttingDateTo, Subinventory);
            }

        }

        /// <summary>
        /// 投入條碼
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public List<Invest> GetPicketIn(long OspHeaderId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetPicketIn(OspHeaderId);
            }
        }

        /// <summary>
        /// 投入editor刪除編輯
        /// </summary>
        /// <param name="InvestDTList"></param>
        /// <returns></returns>
        public ResultModel SetEditor(ProcessUOW.DetailDTEditor InvestDTList, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetEditor(InvestDTList, UserId, UserName);
            }
        }

        /// <summary>
        /// 檢查工單號
        /// </summary>
        /// <param name="InputBatchNo"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ResultModel CheckBatchNo(string InputBatchNo, long OspHeaderId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).CheckBatchNo(InputBatchNo, OspHeaderId);
            }

        }

        /// <summary>
        /// 檢查庫存條碼
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public ResultDataModel<STOCK_T> CheckStockBarcode(string Barcode, string OspDetailInId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).CheckStockBarcode(Barcode, OspDetailInId);
            }

        }
        
        /// <summary>
        /// 儲存加工撿貨條碼
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="Remnant"></param>
        /// <param name="Remaining_Weight"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public ResultModel SavePickIn(string Barcode, string Remnant, string Remaining_Weight, long OspDetailInId, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SavePickIn(Barcode, Remnant, Remaining_Weight, OspDetailInId, UserId,UserName);
            }
        }

        /// <summary>
        /// 儲存產出條碼
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <param name="Production_Roll_Ream_Qty"></param>
        /// <param name="Production_Roll_Ream_Wt"></param>
        /// <param name="Cotangent"></param>
        /// <param name="OspDetailOutId"></param>
        public ResultModel CreateProduction(string UserId,string UserName,string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Cotangent, long OspDetailOutId)
        {
            using (var context = new MesContext())
            {
              return new ProcessUOW(context).CreateProduction(UserId, UserName, Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId);
            }
        }

        /// <summary>
        /// 產出儲存條碼
        /// </summary>
        /// <param name="PaperRoll_Basic_Weight"></param>
        /// <param name="PaperRoll_Specification"></param>
        /// <param name="PaperRoll_Lot_Number"></param>
        /// <param name="Process_Detail_Id"></param>
        /// <returns></returns>
        public ResultModel PaperRollCreateProduction(string PaperRoll_Basic_Weight, string PaperRoll_Specification, string PaperRoll_Lot_Number, long OspDetailOutId,string UserId,string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).PaperRollCreateProduction(PaperRoll_Basic_Weight, PaperRoll_Specification, PaperRoll_Lot_Number, OspDetailOutId, UserId, UserName);
            }
        }


        /// <summary>
        /// 取得產出檢貨pick
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public List<Production> GetPicketOut(long OspHeaderId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetPicketOut(OspHeaderId);
            }
        }

        /// <summary>
        /// 取得餘切TABLE
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public List<Cotangent> GetCotangents(long OspHeaderId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetCotangents(OspHeaderId);
            }
        }

        /// <summary>
        /// 產出Editor
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        public ResultModel SetProductionEditor(ProcessUOW.ProductionDTEditor ProductionDTEditor, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetProductionEditor(ProductionDTEditor, UserId, UserName);
            }
        }

        /// <summary>
        /// 代紙紙捲產出Editor
        /// </summary>
        /// <param name="ProductionDTEditor"></param>
        /// <returns></returns>
        public ResultModel SetPaperRollerProductionEditor(ProcessUOW.ProductionDTEditor ProductionDTEditor, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetPaperRollerProductionEditor(ProductionDTEditor, UserId, UserName);
            }
        }

        /// <summary>
        /// 餘切Editor
        /// </summary>
        /// <param name="cotangentDTEditor"></param>
        /// <returns></returns>
        public ResultModel SetCotangentEditor(ProcessUOW.CotangentDTEditor cotangentDTEditor, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).SetCotangentEditor(cotangentDTEditor, UserId, UserName);
            }
        }

        /// <summary>
        /// 產出條碼入庫
        /// </summary>
        /// <param name="Production_Barcode"></param>
        /// <param name="OspDetailInId"></param>
        /// <returns></returns>
        public ResultModel ProductionChangeStatus(string Production_Barcode, long OspDetailOutId, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).ProductionChangeStatus(Production_Barcode, OspDetailOutId, UserId, UserName);
            }
        }

        /// <summary>
        /// 餘切條碼入庫
        /// </summary>
        /// <param name="CotangentBarcode"></param>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public ResultModel CotangentChangeStatus(string CotangentBarcode,long OspDetailOutId, string UserId, string UserName)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).CotangentChangeStatus(CotangentBarcode, OspDetailOutId, UserId, UserName);
            }
        }

        /// <summary>
        /// 計算損耗量
        /// </summary>
        /// <param name="OspDetailInId"></param>
        /// <param name="OspDetailOutId"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultDataModel<OSP_YIELD_VARIANCE_T> Loss(long OspDetailInId, long OspDetailOutId, string UserId, string UserName)
        {
            using var context = new MesContext();
            return new ProcessUOW(context).Loss(OspDetailInId, OspDetailOutId, UserId, UserName);
        }

        /// <summary>
        /// 取得損耗得率
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public YieldVariance GetRate(long OspHeaderId)
        {
            using var context = new MesContext();
            return new ProcessUOW(context).GetRate(OspHeaderId);
        }

        public void DeleteRate(long OspHeaderId)
        {
            using var context = new MesContext();
            new ProcessUOW(context).DeleteRate(OspHeaderId);
        }

        /// <summary>
        /// 存檔入庫&&工單號狀態更改
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <param name="Locator"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel ChangeHeaderStauts(long OspHeaderId, long Locator, string UserId, string UserName)
        {
            using var context = new MesContext();
            return new ProcessUOW(context).ChangeHeaderStauts(OspHeaderId, Locator, UserId, UserName);
        }

        /// <summary>
        /// 完工紀錄編輯
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ResultModel EditHeaderStauts(long OspHeaderId, string UserId, string UserName)
        {
            using var context = new MesContext();
            return new ProcessUOW(context).EditHeaderStauts(OspHeaderId, UserId, UserName);
        }

        /// <summary>
        /// 完工紀錄編輯
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ResultDataModel<long> FinisheEdit(string BatchNo, long OspHeaderId, string userId)
        {
            using var context = new MesContext();
            return new ProcessUOW(context).FinisheEdit(BatchNo, OspHeaderId, userId);
        }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBatchStatusDesc()
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetBatchStatusDesc();
            }
        }

        /// <summary>
        /// 取得機台
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetManchine()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetManchine("*");
            }
        }

        public List<SelectListItem> GetSelectMachine(string PaperType)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetSelectMachine(PaperType);
            }
        }

        /// <summary>
        /// 取得公單號
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBatchNo()
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetBatchNo();
            }
        }

        /// <summary>
        /// 取得倉庫
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetSubinventory(string UserId, string OspFlag)
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetSubinventory(UserId, OspFlag);
            }
        }
        /// <summary>
        /// 取得儲位
        /// </summary>
        /// <param name="OspDetailOutId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocator(long OspDetailOutId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetLocator(OspDetailOutId);
            }
        }

        /// <summary>
        /// 有無殘捲
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetRemnantItem()
        {
            List<SelectListItem> Remnant = new List<SelectListItem>();
            Remnant.Add(new SelectListItem()
            {
                Text = "無",
                Value = "0",
                Selected = false,
            });
            Remnant.Add(new SelectListItem()
            {
                Text = "有",
                Value = "1",
                Selected = false,
            });
            return Remnant;
        }

        /// <summary>
        /// 有無餘切下拉選單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCotangentItem()
        {
            List<SelectListItem> GetCotangentItem = new List<SelectListItem>();
            GetCotangentItem.Add(new SelectListItem()
            {
                Text = "無",
                Value = "0",
                Selected = false,
            });
            GetCotangentItem.Add(new SelectListItem()
            {
                Text = "有",
                Value = "1",
                Selected = false,
            });
            return GetCotangentItem;
        }


        /// <summary>
        /// 補印標籤
        /// </summary>
        /// <param name="stockIds"></param>
        /// <param name="userName"></param>
        /// <param name="ItemCategory"></param>
        /// <returns></returns>
        public ActionResult RePrintLabel(List<long> stockIds, string userName ,string ItemCategory)
        {
            using (var context = new MesContext())
            {
                var label = new ProcessUOW(context).RePrintLabel(stockIds, userName, ItemCategory);
                return new MasterUOW(context).PrintLabel(label.Data);
            }
        }

        /// <summary>
        /// 產出條碼列印
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult GeProductLabels(List<long> OspPickedOutId, string userName, List<long> OspCotangentId)
        {
            using (var context = new MesContext())
            {
                List<LabelModel> labels = new List<LabelModel>();
                if (OspPickedOutId != null)
                {
                    var label = new ProcessUOW(context).GeProductLabels(OspPickedOutId, userName);
                    labels.AddRange(label.Data);
                }
                if(OspCotangentId != null)
                {
                    var label = new ProcessUOW(context).GeCotangentLabels(OspCotangentId, userName);
                    labels.AddRange(label.Data);
                }

                return new MasterUOW(context).PrintLabel(labels);
            }
        }

        /// <summary>
        /// 產出代紙紙捲條碼列印
        /// </summary>
        /// <param name="StockId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult GePaperRollerProductLabels(List<long> OspPickedOutId, string userName)
        {
            using (var context = new MesContext())
            {
                List<LabelModel> labels = new List<LabelModel>();
                var label = new ProcessUOW(context).GePaperRollerProductLabels(OspPickedOutId, userName);
                return new MasterUOW(context).PrintLabel(label.Data);
            }
        }

        /// <summary>
        /// 取得權限
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Boolean GetAuthority(List<Claim> roles)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetAuthority(roles);
            }
        }

        public ResultModel CheckInsteadPaperOrderProcess(long SrcOspHeaderId)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).CheckInsteadPaperOrderProcess(SrcOspHeaderId);
            }
        }

        internal class ChpProcessModelDTOrder
        {
            public static IOrderedEnumerable<CHP_PROCESS_T> Order(List<Order> orders, IEnumerable<CHP_PROCESS_T> models)
            {
                IOrderedEnumerable<CHP_PROCESS_T> orderedModel = null;
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

            private static IOrderedEnumerable<CHP_PROCESS_T> OrderBy(int column, string dir, IEnumerable<CHP_PROCESS_T> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspHeaderId) : models.OrderBy(x => x.OspHeaderId);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspDetailInId) : models.OrderBy(x => x.OspDetailInId);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BatchNo) : models.OrderBy(x => x.BatchNo);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DueDate) : models.OrderBy(x => x.DueDate);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CuttingDateFrom) : models.OrderBy(x => x.CuttingDateFrom);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CuttingDateTo) : models.OrderBy(x => x.CuttingDateTo);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.MachineNum) : models.OrderBy(x => x.MachineNum);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CustomerName) : models.OrderBy(x => x.CustomerName);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrderNumber) : models.OrderBy(x => x.OrderNumber);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrderLineNumber) : models.OrderBy(x => x.OrderLineNumber);
                    case 12:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BasicWeight) : models.OrderBy(x => x.BasicWeight);
                    case 13:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                    case 14:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.GrainDirection) : models.OrderBy(x => x.GrainDirection);
                    case 15:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrderWeight) : models.OrderBy(x => x.OrderWeight);
                    case 16:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWt) : models.OrderBy(x => x.ReamWt);
                    case 17:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 18:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 19:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
                    case 20:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspRemark) : models.OrderBy(x => x.OspRemark);
                    case 21:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Loss) : models.OrderBy(x => x.Loss);
                    case 22:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SelectedInventoryItemNumber) : models.OrderBy(x => x.SelectedInventoryItemNumber);
                    case 23:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Product_Item) : models.OrderBy(x => x.Product_Item);
                    case 24:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Note) : models.OrderBy(x => x.Note);
                    case 25:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                    case 26:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Createdby) : models.OrderBy(x => x.Createdby);
                    case 27:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creationdate) : models.OrderBy(x => x.Creationdate);
                    case 28:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LastUpdatedBy) : models.OrderBy(x => x.LastUpdatedBy);
                    case 29:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LastUpdateDate) : models.OrderBy(x => x.LastUpdateDate);
                    case 30:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionUom) : models.OrderBy(x => x.TransactionUom);
                }
            }

            private static IOrderedEnumerable<CHP_PROCESS_T> ThenBy(int column, string dir, IOrderedEnumerable<CHP_PROCESS_T> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspHeaderId) : models.OrderBy(x => x.OspHeaderId);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspDetailInId) : models.OrderBy(x => x.OspDetailInId);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BatchNo) : models.OrderBy(x => x.BatchNo);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DueDate) : models.OrderBy(x => x.DueDate);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CuttingDateFrom) : models.OrderBy(x => x.CuttingDateFrom);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CuttingDateTo) : models.OrderBy(x => x.CuttingDateTo);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.MachineNum) : models.OrderBy(x => x.MachineNum);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CustomerName) : models.OrderBy(x => x.CustomerName);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrderNumber) : models.OrderBy(x => x.OrderNumber);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrderLineNumber) : models.OrderBy(x => x.OrderLineNumber);
                    case 12:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BasicWeight) : models.OrderBy(x => x.BasicWeight);
                    case 13:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                    case 14:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.GrainDirection) : models.OrderBy(x => x.GrainDirection);
                    case 15:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OrderWeight) : models.OrderBy(x => x.OrderWeight);
                    case 16:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWt) : models.OrderBy(x => x.ReamWt);
                    case 17:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 18:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 19:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
                    case 20:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspRemark) : models.OrderBy(x => x.OspRemark);
                    case 21:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Loss) : models.OrderBy(x => x.Loss);
                    case 22:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SelectedInventoryItemNumber) : models.OrderBy(x => x.SelectedInventoryItemNumber);
                    case 23:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Product_Item) : models.OrderBy(x => x.Product_Item);
                    case 24:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Note) : models.OrderBy(x => x.Note);
                    case 25:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                    case 26:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Createdby) : models.OrderBy(x => x.Createdby);
                    case 27:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creationdate) : models.OrderBy(x => x.Creationdate);
                    case 28:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LastUpdatedBy) : models.OrderBy(x => x.LastUpdatedBy);
                    case 29:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LastUpdateDate) : models.OrderBy(x => x.LastUpdateDate);
                    case 30:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionUom) : models.OrderBy(x => x.TransactionUom);
                }
            }

            public static List<CHP_PROCESS_T> Search(DataTableAjaxPostViewModel data, List<CHP_PROCESS_T> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (
                        !string.IsNullOrEmpty(p.OspHeaderId.ToString()) && p.OspHeaderId.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OspDetailInId.ToString()) && p.OspDetailInId.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.BatchNo) && p.BatchNo.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.DueDate.ToString()) && p.DueDate.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.CuttingDateFrom.ToString()) && p.CuttingDateFrom.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.CuttingDateTo.ToString()) && p.CuttingDateTo.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.MachineNum) && p.MachineNum.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.CustomerName) && p.CustomerName.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OrderNumber.ToString()) && p.OrderNumber.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OrderLineNumber) && p.OrderLineNumber.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.BasicWeight) && p.BasicWeight.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Specification) && p.Specification.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.GrainDirection) && p.GrainDirection.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OrderWeight) && p.OrderWeight.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.ReamWt.ToString()) && p.ReamWt.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryQuantity.ToString()) && p.PrimaryQuantity.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryUom) && p.PrimaryUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.TransactionUom) && p.TransactionUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PackingType) && p.PackingType.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OspRemark.ToString()) && p.OspRemark.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PaperType.ToString()) && p.PaperType.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.SelectedInventoryItemNumber.ToString()) && p.SelectedInventoryItemNumber.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Product_Item.ToString()) && p.Product_Item.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Note.ToString()) && p.Note.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Subinventory.ToString()) && p.Subinventory.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Createdby.ToString()) && p.Createdby.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Creationdate.ToString()) && p.Creationdate.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.LastUpdatedBy.ToString()) && p.LastUpdatedBy.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.LocatorCode.ToString()) && p.LocatorCode.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Loss.ToString()) && p.Loss.ToString().ToLower().Contains(search.ToLower()))
                        ).ToList();
                }
                return model;
            }
        }


        internal class InvestModelDTOrder
        {
            public static IOrderedEnumerable<Invest> Order(List<Order> orders, IEnumerable<Invest> models)
            {
                IOrderedEnumerable<Invest> orderedModel = null;
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

            private static IOrderedEnumerable<Invest> OrderBy(int column, string dir, IEnumerable<Invest> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspPickedInId) : models.OrderBy(x => x.OspPickedInId);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspDetailInId) : models.OrderBy(x => x.OspDetailInId);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspHeaderId) : models.OrderBy(x => x.OspHeaderId);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StockId) : models.OrderBy(x => x.StockId);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.HasRemaint) : models.OrderBy(x => x.HasRemaint);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RemainingQuantity) : models.OrderBy(x => x.RemainingQuantity);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BasicWeight) : models.OrderBy(x => x.BasicWeight);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                    case 12:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                    case 13:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemId) : models.OrderBy(x => x.InventoryItemId);
                    case 14:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemNumber) : models.OrderBy(x => x.InventoryItemNumber);
                    case 15:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);
                    
                }
            }

            private static IOrderedEnumerable<Invest> ThenBy(int column, string dir, IOrderedEnumerable<Invest> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspPickedInId) : models.OrderBy(x => x.OspPickedInId);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspDetailInId) : models.OrderBy(x => x.OspDetailInId);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OspHeaderId) : models.OrderBy(x => x.OspHeaderId);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StockId) : models.OrderBy(x => x.StockId);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.HasRemaint) : models.OrderBy(x => x.HasRemaint);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RemainingQuantity) : models.OrderBy(x => x.RemainingQuantity);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BasicWeight) : models.OrderBy(x => x.BasicWeight);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                    case 12:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                    case 13:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemId) : models.OrderBy(x => x.InventoryItemId);
                    case 14:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.InventoryItemNumber) : models.OrderBy(x => x.InventoryItemNumber);
                    case 15:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);

                }
            }

            public static List<Invest> Search(DataTableAjaxPostViewModel data, List<Invest> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (
                        !string.IsNullOrEmpty(p.OspPickedInId.ToString()) && p.OspPickedInId.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OspDetailInId.ToString()) && p.OspDetailInId.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.OspHeaderId.ToString()) && p.OspHeaderId.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.StockId.ToString()) && p.StockId.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.HasRemaint) && p.HasRemaint.Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryQuantity.ToString()) && p.PrimaryQuantity.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.RemainingQuantity.ToString()) && p.RemainingQuantity.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.BasicWeight) && p.BasicWeight.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Specification) && p.Specification.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.LotNumber) && p.LotNumber.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PaperType) && p.PaperType.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.InventoryItemId.ToString()) && p.InventoryItemId.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.InventoryItemNumber) && p.InventoryItemNumber.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.SecondaryQuantity.ToString()) && p.SecondaryQuantity.ToString().ToLower().Contains(search.ToLower()))
                        ).ToList();
                }
                return model;
            }
        }
   

        internal class ProductionModelDTOrder
        {
            public static IOrderedEnumerable<Production> Order(List<Order> orders, IEnumerable<Production> models)
            {
                IOrderedEnumerable<Production> orderedModel = null;
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

            private static IOrderedEnumerable<Production> OrderBy(int column, string dir, IEnumerable<Production> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Product_Item) : models.OrderBy(x => x.Product_Item);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryUom) : models.OrderBy(x => x.SecondaryUom);
                }
            }

            private static IOrderedEnumerable<Production> ThenBy(int column, string dir, IOrderedEnumerable<Production> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Product_Item) : models.OrderBy(x => x.Product_Item);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryUom) : models.OrderBy(x => x.SecondaryUom);
                }
            }

            public static List<Production> Search(DataTableAjaxPostViewModel data, List<Production> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (
                      !string.IsNullOrEmpty(p.Barcode) && p.Barcode.Contains(search.ToLower())
                        || (!string.IsNullOrEmpty(p.Product_Item) && p.Product_Item.Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryQuantity.ToString()) && p.PrimaryQuantity.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.SecondaryQuantity.ToString()) && p.SecondaryQuantity.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryUom) && p.PrimaryUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.SecondaryUom) && p.SecondaryUom.ToLower().Contains(search.ToLower()))
                        )).ToList();
                }
                return model;
            }
        }

        internal class CotangentModelDTOrder
        {
            public static IOrderedEnumerable<Cotangent> Order(List<Order> orders, IEnumerable<Cotangent> models)
            {
                IOrderedEnumerable<Cotangent> orderedModel = null;
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

            private static IOrderedEnumerable<Cotangent> OrderBy(int column, string dir, IEnumerable<Cotangent> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Related_item) : models.OrderBy(x => x.Related_item);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryUom) : models.OrderBy(x => x.SecondaryUom);
                }
            }

            private static IOrderedEnumerable<Cotangent> ThenBy(int column, string dir, IOrderedEnumerable<Cotangent> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Related_item) : models.OrderBy(x => x.Related_item);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryQuantity) : models.OrderBy(x => x.PrimaryQuantity);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryQuantity) : models.OrderBy(x => x.SecondaryQuantity);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SecondaryUom) : models.OrderBy(x => x.SecondaryUom);
                }
            }

            public static List<Cotangent> Search(DataTableAjaxPostViewModel data, List<Cotangent> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (
                      !string.IsNullOrEmpty(p.Barcode) && p.Barcode.Contains(search.ToLower())
                        || (!string.IsNullOrEmpty(p.Related_item) && p.Related_item.Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryQuantity.ToString()) && p.PrimaryQuantity.ToString().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.SecondaryQuantity.ToString()) && p.SecondaryQuantity.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryUom) && p.PrimaryUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.SecondaryUom) && p.SecondaryUom.ToLower().Contains(search.ToLower()))
                        )).ToList();
                }
                return model;
            }
        }

    }
}