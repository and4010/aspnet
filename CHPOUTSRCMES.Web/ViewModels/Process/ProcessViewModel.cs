using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.Process
{
    public class ProcessViewModel
    {
        public Invest Invest { set; get; }
        public Cotangent Cotangent { set; get; }
        public CHP_PROCESS_T CHP_PROCESS_T { set; get; }
        public static List<CHP_PROCESS_T> chp_process_t = new List<CHP_PROCESS_T>();
        public static List<Invest> invest = new List<Invest>();
        public static List<Invest> Invest_Stock = new List<Invest>();
        public static List<Production> ListProductions = new List<Production>();
        public static List<Cotangent> ListCotangent = new List<Cotangent>();
        public static Production Production { set; get; }

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
        public List<SelectListItem> GetSubinventory()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetSubinventory("*");
            }
        }

        public static void ResetData()
        {
            chp_process_t = new List<CHP_PROCESS_T>();
            invest = new List<Invest>();
            Invest_Stock = new List<Invest>();
            ListCotangent = new List<Cotangent>();
            ListProductions = new List<Production>();
        }

        public static List<CHP_PROCESS_T> GetTable()
        {
            chp_process_t.Add(new CHP_PROCESS_T
            {
                OspHeaderId = 1,
                OspDetailInId = 1,
                DueDate = Convert.ToDateTime("2020-05-31"),
                CuttingDateFrom = null,
                CuttingDateTo = null,
                BatchNo = "P9B0288",
                MachineNum = "01",
                Status = "待排單",
                CustomerName = "中華彩色印刷股份有限公司",
                PaperType = "A003",
                BasicWeight = "01000",
                Specification = "310K226K",
                OrderWeight = "58.97",
                GrainDirection = "S",
                ReamWt = "50",
                TransactionUom = "RE",
                PrimaryQuantity = "1.34",
                PrimaryUom = "MT",
                PackingType = "令包",
                OspRemark = "N11/25入倉",
                Note = "",
                SelectedInventoryItemNumber = "4A003A01000310KRL00",
                Product_Item = "4A003A01000310K266K",
                OrderNumber = "1192006167",
                OrderLineNumber = "1.1",
                Subinventory = "TB2",
                Createdby = "一力星",
                Creationdate = DateTime.Now,
            });


            chp_process_t.Add(new CHP_PROCESS_T
            {
                OspHeaderId = 2,
                OspDetailInId = 2,
                DueDate = Convert.ToDateTime("2020-05-31"),
                CuttingDateFrom = null,
                CuttingDateTo = null,
                BatchNo = "F2010087",
                MachineNum = "01",
                Status = "待排單",
                CustomerName = "保吉紙業有限公司",
                PaperType = "AB03",
                BasicWeight = "00699",
                Specification = "350K250K",
                OrderWeight = "43.50",
                GrainDirection = "S",
                ReamWt = "19",
                TransactionUom = "RE",
                PrimaryQuantity = "0.38",
                PrimaryUom = "MT",
                PackingType = "令包",
                OspRemark = "P",
                Note = "",
                SelectedInventoryItemNumber = "4AEH0P0190007871092",
                Product_Item = "4AB23P00699350K250K",
                OrderNumber = "1202000114",
                OrderLineNumber = "1.1",
                Subinventory = "TB2",
                Createdby = "一力星",
                Creationdate = DateTime.Now,
            });

            return chp_process_t;
        }

        public static List<Invest> GetInvest()
        {
            if (invest.Count == 0)
            {
                invest.Add(new Invest
                {
                    Invest_Id = 1,
                    Process_Detail_Id = 1,
                    Barcode = "W2006060001",
                    Original_Weight = "779",
                    Remaining_Weight = "",
                    Remnant = "",
                    Basic_Weight = "02300",
                    Specification = "590MM",
                    Lot_Number = "2618011432040507",
                    Paper_Type = "FHIZ",
                });


                invest.Add(new Invest
                {
                    Invest_Id = 2,
                    Process_Detail_Id = 1,
                    Barcode = "W2006060002",
                    Original_Weight = "669",
                    Remaining_Weight = "",
                    Remnant = "",
                    Basic_Weight = "02000",
                    Specification = "490MM",
                    Lot_Number = "2618011432040508",
                    Paper_Type = "Z300",
                });



                invest.Add(new Invest
                {
                    Invest_Id = 3,
                    Process_Detail_Id = 2,
                    Barcode = "P2007060001",
                    Original_Weight = "800",
                    Remaining_Weight = "",
                    Remnant = "",
                    Ream_Qty = "20",
                    Specification = "400MM",
                    Lot_Number = "2618011432040303",
                    Paper_Type = "Z100",
                    Item_No = "4AEH0P0190007871092",
                });

                invest.Add(new Invest
                {
                    Invest_Id = 4,
                    Process_Detail_Id = 3,
                    Barcode = "W2008060001",
                    Original_Weight = "800",
                    Remaining_Weight = "",
                    Remnant = "",
                    Basic_Weight = "02000",
                    Specification = "400MM",
                    Lot_Number = "2618011432040303",
                    Paper_Type = "Z100",
                    Item_No = "4AEH0P0190007871092",
                });
            }

            return invest;
        }

        public static List<Cotangent> GetCotangents(string Process_Detail_Id)
        {
            if (ListCotangent.Count == 0)
            {
                var d = ListCotangent.FirstOrDefault(r => r.Process_Detail_Id == Int32.Parse(Process_Detail_Id));
                if(d == null)
                {
                    ListCotangent.Add(new Cotangent
                    {
                        Process_Detail_Id = 1,
                        Cotangent_Id = 1,
                        Barcode = "P2009060001",
                        Cotangent_Ttl_Roll_Ream = "",
                        Kg = "",
                        Related_item = "4AEMXA0100007110965",
                        Status = "待入庫",
                    });
                }


            }
            return ListCotangent;
        }



        public List<CHP_PROCESS_T> Search(string Process_Status, string Process_Batch_no, string Manchine_Num, string Demand_Date, string Cutting_Date_From, string Cutting_Date_To, string Subinventory)
        {
            using (var context = new MesContext())
            {
                return new ProcessUOW(context).GetTable();
            }
        }




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
                    || (!string.IsNullOrEmpty(p.OrderNumber) && p.OrderNumber.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.OrderLineNumber) && p.OrderLineNumber.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BasicWeight) && p.BasicWeight.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Specification) && p.Specification.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.GrainDirection) && p.GrainDirection.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.OrderWeight) && p.OrderWeight.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ReamWt) && p.ReamWt.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PrimaryQuantity) && p.PrimaryQuantity.ToLower().Contains(search.ToLower()))
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
}