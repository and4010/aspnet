using CHPOUTSRCMES.Web.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.ViewModels.Process
{
    public class ProcessIndexViewModel
    {
        public Invest Invest { set; get; }
        public Cotangent Cotangent { set; get; }
        public CHP_PROCESS_T CHP_PROCESS_T { set; get; }
        public static List<CHP_PROCESS_T> chp_process_t = new List<CHP_PROCESS_T>();
        public static List<Invest> invest = new List<Invest>();
        public static List<Invest> Invest_Stock = new List<Invest>();
        public static List<Production> ListProductions = new List<Production>();
        public static List<Cotangent> ListCotangent = new List<Cotangent>();
        public Production Production { set; get; }


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
                Process_Heard_Id = 1,
                Process_Detail_Id = 1,
                Demand_Date = Convert.ToDateTime("2020-05-31"),
                Cutting_Date_From = null,
                Cutting_Date_To = null,
                Process_Batch_no = "P9B0288",
                Manchine_Num = "01",
                Process_Status = "待排單",
                Cosutomer_Num = "中華彩色印刷股份有限公司",
                Paper_Type = "A003",
                Basic_Weight = "01000",
                Specification = "310K226K",
                Ream_Weight = "58.97",
                Grain_Direction = "S",
                Ream_Qty = "50",
                TransactionUom = "RE",
                Weight = "1.34",
                PrimaryUom = "MT",
                Packing_Type = "令包",
                Outsourching_Remark = "N11/25入倉",
                Produce_Remark = "",
                SelectedInventoryItemNumber = "4A003A01000310KRL00",
                Product_Item = "4A003A01000310K266K",
                Order_Number = "1192006167",
                Detail_Line = "1.1",
                Produce_Item = "FFF123",
                Subinventory = "TB2",
                Created_by = "一力星",
                Creation_date = DateTime.Now.ToString(),
            });


            chp_process_t.Add(new CHP_PROCESS_T
            {
                Process_Heard_Id = 2,
                Process_Detail_Id = 2,
                Demand_Date = Convert.ToDateTime("2020-05-31"),
                Cutting_Date_From = null,
                Cutting_Date_To = null,
                Process_Batch_no = "F2010087",
                Manchine_Num = "01",
                Process_Status = "待排單",
                Cosutomer_Num = "保吉紙業有限公司",
                Paper_Type = "AB03",
                Basic_Weight = "00699",
                Specification = "350K250K",
                Ream_Weight = "43.50",
                Grain_Direction = "S",
                Ream_Qty = "19",
                TransactionUom = "RE",
                Weight = "0.38",
                PrimaryUom = "MT",
                Packing_Type = "令包",
                Outsourching_Remark = "P",
                Produce_Remark = "",
                SelectedInventoryItemNumber = "4AEH0P0190007871092",
                Product_Item = "4AB23P00699350K250K",
                Order_Number = "1202000114",
                Detail_Line = "1.1",
                Produce_Item = "FFF123",
                Subinventory = "TB2",
                Created_by = "一力星",
                Creation_date = DateTime.Now.ToString(),
            });

            chp_process_t.Add(new CHP_PROCESS_T
            {
                Process_Heard_Id = 3,
                Process_Detail_Id = 3,
                Demand_Date = Convert.ToDateTime("2020-05-31"),
                Cutting_Date_From = null,
                Cutting_Date_To = null,
                Process_Batch_no = "R9B0287",
                Manchine_Num = "01",
                Process_Status = "待排單",
                Cosutomer_Num = "中華彩色印刷股份有限公司",
                Paper_Type = "A003",
                Basic_Weight = "01000",
                Specification = "310K226K",
                Ream_Weight = "58.97",
                Grain_Direction = "S",
                Ream_Qty = "50",
                TransactionUom = "RE",
                Weight = "1.34",
                PrimaryUom = "MT",
                Packing_Type = "無令打件",
                Outsourching_Remark = "N11/25入倉",
                Produce_Remark = "",
                SelectedInventoryItemNumber = "4AEH0P0190007871092",
                Product_Item = "4FV0TP0230005900787",
                Order_Number = "1192006167",
                Detail_Line = "1.1",
                Produce_Item = "FFF123",
                Subinventory = "TB2",
                Created_by = "一力星",
                Creation_date = DateTime.Now.ToString(),
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
            //ResultModel result = new ResultModel(true, "搜尋成功");
            try
            {

                var query = chp_process_t.Where(
                  x =>
                  (Process_Status == "*" || x.Process_Status != null && x.Process_Status.ToLower() == Process_Status.ToLower()) &&
                  (Process_Batch_no == "*" || x.Process_Batch_no != null && x.Process_Batch_no.ToLower() == Process_Batch_no.ToLower()) &&
                  (Manchine_Num == "*" || x.Manchine_Num != null && x.Manchine_Num.ToLower() == Manchine_Num.ToLower()) &&
                  (Demand_Date == "" || x.Demand_Date != null && x.Demand_Date.ToString("yyyy-MM-dd").ToLower() == Demand_Date.ToLower()) &&
                  (Cutting_Date_From == "" || x.Cutting_Date_From != null && x.Cutting_Date_From.Value.ToString("yyyy-MM-dd").ToLower() == Cutting_Date_From.ToLower()) &&
                  (Cutting_Date_To == "" || x.Cutting_Date_To != null && x.Cutting_Date_To.Value.ToString("yyyy-MM-dd").ToLower() == Cutting_Date_To.ToLower()) &&
                  (Subinventory == "*" || x.Subinventory != null && x.Subinventory.ToLower() == Subinventory.ToLower())
                  ).ToList();

                //dtData = query;
                return query;
            }
            catch (Exception e)
            {
                //result.Msg = e.Message;
                //result.Success = false;
                return new List<CHP_PROCESS_T>();
            }
            //return result;
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Heard_Id) : models.OrderBy(x => x.Process_Heard_Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Detail_Id) : models.OrderBy(x => x.Process_Detail_Id);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Status) : models.OrderBy(x => x.Process_Status);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Batch_no) : models.OrderBy(x => x.Process_Batch_no);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Demand_Date) : models.OrderBy(x => x.Demand_Date);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Cutting_Date_From) : models.OrderBy(x => x.Cutting_Date_From);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Cutting_Date_To) : models.OrderBy(x => x.Cutting_Date_To);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Manchine_Num) : models.OrderBy(x => x.Manchine_Num);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Cosutomer_Num) : models.OrderBy(x => x.Cosutomer_Num);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Order_Number) : models.OrderBy(x => x.Order_Number);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Detail_Line) : models.OrderBy(x => x.Detail_Line);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Basic_Weight) : models.OrderBy(x => x.Basic_Weight);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Grain_Direction) : models.OrderBy(x => x.Grain_Direction);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Ream_Weight) : models.OrderBy(x => x.Ream_Weight);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Ream_Qty) : models.OrderBy(x => x.Ream_Qty);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Weight) : models.OrderBy(x => x.Weight);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Packing_Type) : models.OrderBy(x => x.Packing_Type);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Outsourching_Remark) : models.OrderBy(x => x.Outsourching_Remark);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Produce_Item) : models.OrderBy(x => x.Produce_Item);
                case 22:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SelectedInventoryItemNumber) : models.OrderBy(x => x.SelectedInventoryItemNumber);
                case 23:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Product_Item) : models.OrderBy(x => x.Product_Item);
                case 24:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Produce_Remark) : models.OrderBy(x => x.Produce_Remark);
                case 25:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                case 26:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 27:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creation_date) : models.OrderBy(x => x.Creation_date);
                case 28:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_updated_by) : models.OrderBy(x => x.Last_updated_by);
                case 29:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_update_date) : models.OrderBy(x => x.Last_update_date);
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
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Heard_Id) : models.OrderBy(x => x.Process_Heard_Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Detail_Id) : models.OrderBy(x => x.Process_Detail_Id);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Status) : models.OrderBy(x => x.Process_Status);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Process_Batch_no) : models.OrderBy(x => x.Process_Batch_no);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Demand_Date) : models.OrderBy(x => x.Demand_Date);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Cutting_Date_From) : models.OrderBy(x => x.Cutting_Date_From);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Cutting_Date_To) : models.OrderBy(x => x.Cutting_Date_To);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Manchine_Num) : models.OrderBy(x => x.Manchine_Num);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Cosutomer_Num) : models.OrderBy(x => x.Cosutomer_Num);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Order_Number) : models.OrderBy(x => x.Order_Number);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Detail_Line) : models.OrderBy(x => x.Detail_Line);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Basic_Weight) : models.OrderBy(x => x.Basic_Weight);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Grain_Direction) : models.OrderBy(x => x.Grain_Direction);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Ream_Weight) : models.OrderBy(x => x.Ream_Weight);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Ream_Qty) : models.OrderBy(x => x.Ream_Qty);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Weight) : models.OrderBy(x => x.Weight);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Packing_Type) : models.OrderBy(x => x.Packing_Type);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Outsourching_Remark) : models.OrderBy(x => x.Outsourching_Remark);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Produce_Item) : models.OrderBy(x => x.Produce_Item);
                case 22:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SelectedInventoryItemNumber) : models.OrderBy(x => x.SelectedInventoryItemNumber);
                case 23:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Product_Item) : models.OrderBy(x => x.Product_Item);
                case 24:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Produce_Remark) : models.OrderBy(x => x.Produce_Remark);
                case 25:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                case 26:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 27:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creation_date) : models.OrderBy(x => x.Creation_date);
                case 28:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_updated_by) : models.OrderBy(x => x.Last_updated_by);
                case 29:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_update_date) : models.OrderBy(x => x.Last_update_date);
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
                    !string.IsNullOrEmpty(p.Process_Heard_Id.ToString()) && p.Process_Heard_Id.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Process_Detail_Id.ToString()) && p.Process_Detail_Id.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Process_Status) && p.Process_Status.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Process_Batch_no) && p.Process_Batch_no.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Demand_Date.ToString()) && p.Demand_Date.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Cutting_Date_From.ToString()) && p.Cutting_Date_From.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Cutting_Date_To.ToString()) && p.Cutting_Date_To.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Manchine_Num) && p.Manchine_Num.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Cosutomer_Num) && p.Cosutomer_Num.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Order_Number) && p.Order_Number.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Detail_Line) && p.Detail_Line.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Basic_Weight) && p.Basic_Weight.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Specification) && p.Specification.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Grain_Direction) && p.Grain_Direction.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Ream_Weight) && p.Ream_Weight.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Ream_Qty) && p.Ream_Qty.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Weight) && p.Weight.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PrimaryUom) && p.PrimaryUom.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.TransactionUom) && p.TransactionUom.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Packing_Type) && p.Packing_Type.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Outsourching_Remark.ToString()) && p.Outsourching_Remark.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Produce_Item.ToString()) && p.Produce_Item.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SelectedInventoryItemNumber.ToString()) && p.SelectedInventoryItemNumber.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Product_Item.ToString()) && p.Product_Item.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Produce_Remark.ToString()) && p.Produce_Remark.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Subinventory.ToString()) && p.Subinventory.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Created_by.ToString()) && p.Created_by.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Creation_date.ToString()) && p.Creation_date.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_updated_by.ToString()) && p.Last_updated_by.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_update_date.ToString()) && p.Last_update_date.ToString().ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }



    }
}