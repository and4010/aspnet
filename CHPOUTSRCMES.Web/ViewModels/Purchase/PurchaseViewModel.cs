using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.Purchase
{
    public class PurchaseViewModel
    {
        [Display(Name = "月")]
        public string Month { set; get; }
        [Display(Name = "年")]
        public string Year { set; get; }
        [Display(Name = "倉庫")]
        public string Warehouse { set; get; }
        [Display(Name = "建立時間")]
        public string CreateDate { set; get; }
        [Display(Name = "櫃號")]
        public string CabinetNumber { set; get; }
        [Display(Name = "倉庫")]
        public string Subinventory { set; get; }
        [Display(Name = "狀態")]
        public string Status { set; get; }

        public List<DetailModel> Purchase { get; set; }

   

        public DetailModel Detail { set; get; }

        public DetailModel.RollDetailModel RollDetailModel { set; get; }
        public DetailModel.FlatDetailModel FlatDetailModel { set; get; }





        //測試資料

        public static List<DetailModel.FlatDetailModel> StockInFlat = new List<DetailModel.FlatDetailModel>();

        public static List<DetailModel.RollDetailModel> StockInRoll = new List<DetailModel.RollDetailModel>();

        public static List<FullCalendarEventModel> fullCalendarEventModel = new List<FullCalendarEventModel>();

        public List<DetailModel.RollModel> GetRollHeader()
        {
            List<DetailModel.RollModel> model = new List<DetailModel.RollModel>();

            model.Add(new DetailModel.RollModel()
            {
                Id = 1,
                Subinventory = "TB2",
                Locator = "SFG",
                Item_No = "4FU0ZA025500635RL00",
                PaperType = "FU0Z"
            ,
                BaseWeight = "2550",
                Specification = "635",
                RollReamQty = "1",
                TransactionQuantity = "0.421",
                TransactionUom = "MT",
                PrimanyQuantity = "421",
                PrimaryUom = "KG",
            });
            model.Add(new DetailModel.RollModel()
            {
                Id = 2,
                Subinventory = "TB2",
                Locator = "SFG",
                Item_No = "4FU0ZA022000635RL00",
                PaperType = "FU0Z"
,
                BaseWeight = "2200",
                Specification = "635",
                RollReamQty = "1",
                TransactionQuantity = "0.44",
                TransactionUom = "MT",
                PrimanyQuantity = "440",
                PrimaryUom = "KG",
            });
            model.Add(new DetailModel.RollModel()
            {
                Id = 3,
                Subinventory = "TB2",
                Locator = "SFG",
                Item_No = "4FU0ZA022000889RL00",
                PaperType = "FU0Z"
,
                BaseWeight = "2200",
                Specification = "889",
                RollReamQty = "1",
                TransactionQuantity = "0.889",
                TransactionUom = "MT",
                PrimanyQuantity = "889",
                PrimaryUom = "KG",
            });
            return model;
        }

        public List<DetailModel.FlatModel> GetFlatHeader()
        {
            List<DetailModel.FlatModel> model = new List<DetailModel.FlatModel>();

            model.Add(new DetailModel.FlatModel()
            {
                Id = 1,
                Subinventory = "SFG",
                Locator = "TB2",
                Item_No = "4DM00A03500214K512K",
                ReamWeight = "1000"
            ,
                RollReamQty = "3",
                PackingType = "令包",
                Pieces_Qty = "1000",
                TransactionQuantity = "3",
                TransactionUom = "MT",
                TtlRollReam = "30000",
                TtlRollReamUom = "RE",
                DeliveryQty = "30000",
                DeliveryUom = "KG"
            });

            return model;
        }

        public static void resetData()
        {
            StockInFlat = new List<DetailModel.FlatDetailModel>();
            StockInRoll = new List<DetailModel.RollDetailModel>();
            fullCalendarEventModel = new List<FullCalendarEventModel>();
        }

        public static void GetStockInRoll()
        {
           
        }

        public static void GetStockInFlat()
        {

            StockInFlat.Add(new DetailModel.FlatDetailModel()
            {
                Id = 1,
                Subinventory = "SFG",
                Locator = "TB2",
                Barcode = "P2005060001",
                Item_No = "4DM00A03500214K512K",
                ReamWeight = "1000"
            ,
                PackingType = "令包",
                Pieces_Qty = "1000",
                Qty = "1",
                Status = "待入庫",
                Remark = ""
            });
            StockInFlat.Add(new DetailModel.FlatDetailModel()
            {
                Id = 2,
                Subinventory = "SFG",
                Locator = "TB2",
                Barcode = "P2005060002",
                Item_No = "4DM00A03500214K512K",
                ReamWeight = "1000"
            ,
                PackingType = "令包",
                Pieces_Qty = "1000",
                Qty = "1",
                Status = "待入庫",
                Remark = ""
            });
            StockInFlat.Add(new DetailModel.FlatDetailModel()
            {
                Id = 3,
                Subinventory = "SFG",
                Locator = "TB2",
                Barcode = "P2005060003",
                Item_No = "4DM00A03500214K512K",
                ReamWeight = "1000"
        ,
                PackingType = "令包",
                Pieces_Qty = "1000",
                Qty = "1",
                Status = "待入庫",
                Remark = ""
            });

        }

        public List<DetailModel.RollDetailModel> SaveRollBarcode(String Barcode,ref Boolean Boolean,ref Boolean BarcodeStatus)
        {
            List<DetailModel.RollDetailModel> model = StockInRoll;
            try
            {
                var sr = model.First(r => r.Barcode == Barcode);
                if(sr.Status == "已入庫")
                {
                    BarcodeStatus = false;
                }
                else
                {
                    sr.Status = "已入庫";
                }

            }
            catch(Exception e)
            {
                Boolean = false;
            }
           

            return model;
        }

        public DetailModel.RollDetailModel GetRollEdit(string Id)
        {
            var model = StockInRoll.First(r => r.Id.ToString() == Id);
            DetailModel.RollDetailModel rm = new DetailModel.RollDetailModel();
            rm = model;
            return rm;
        }

        public Boolean GetRollEditRemak(string remak, int id, String status,string Reason)
        {

            var Id = StockInRoll.Single(r => r.Id == id);

            if (Id != null)
            {
                Id.Remark = remak;
                Id.Reason = Reason;
            }

            return true;
        }

        public Boolean GetFlatEditRemak(string remak, int id, string status, string Reason)
        {

            var Id = StockInFlat.Single(r => r.Id == id);

            if (Id != null)
            {
                Id.Remark = remak;
                Id.Reason = Reason;
            }


            return true;
        }


        public DetailModel.FlatDetailModel GetFlatEdit(string Id)
        {
            var model = StockInFlat.First(r => r.Id.ToString() == Id);
            DetailModel.FlatDetailModel fd = new DetailModel.FlatDetailModel();
            fd = model;
            return fd;
        }

        public List<DetailModel.FlatDetailModel> SaveFlatBarcode(string Barcode, ref Boolean Boolean, ref Boolean BarcodeStatus)
        {
            List<DetailModel.FlatDetailModel> model = StockInFlat;
            try
            {
                var sf = model.First(r => r.Barcode == Barcode);
                if (sf.Status == "已入庫")
                {
                    BarcodeStatus = false;
                }
                else
                {
                    sf.Status = "已入庫";
                }

            }
            catch (Exception e)
            {
                Boolean = false;
            }
           
       

            return model;
        }



        public DetailModel.RollDetailModel CheckLotNumber()
        {
            DetailModel.RollDetailModel paperRollDetail = new DetailModel.RollDetailModel();
            paperRollDetail.Item_No = "4FHIZA03000787RL00";
            paperRollDetail.Subinventory = "TB2";
            paperRollDetail.Locator = "SFG";
            return paperRollDetail;
        }






        internal class RollDetailModelDTOrder
        {
            public static IOrderedEnumerable<DetailModel.RollDetailModel> Order(List<Order> orders, IEnumerable<DetailModel.RollDetailModel> models)
            {
                IOrderedEnumerable<DetailModel.RollDetailModel> orderedModel = null;
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

            private static IOrderedEnumerable<DetailModel.RollDetailModel> OrderBy(int column, string dir, IEnumerable<DetailModel.RollDetailModel> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BaseWeight) : models.OrderBy(x => x.BaseWeight);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TheoreticalWeight) : models.OrderBy(x => x.TheoreticalWeight);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionQuantity) : models.OrderBy(x => x.TransactionQuantity);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionUom) : models.OrderBy(x => x.TransactionUom);
                    case 12:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimanyQuantity) : models.OrderBy(x => x.PrimanyQuantity);
                    case 13:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 14:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                    case 15:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 16:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Remark) : models.OrderBy(x => x.Remark);

                }
            }

            private static IOrderedEnumerable<DetailModel.RollDetailModel> ThenBy(int column, string dir, IOrderedEnumerable<DetailModel.RollDetailModel> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BaseWeight) : models.OrderBy(x => x.BaseWeight);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TheoreticalWeight) : models.OrderBy(x => x.TheoreticalWeight);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionQuantity) : models.OrderBy(x => x.TransactionQuantity);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionUom) : models.OrderBy(x => x.TransactionUom);
                    case 12:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimanyQuantity) : models.OrderBy(x => x.PrimanyQuantity);
                    case 13:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PrimaryUom) : models.OrderBy(x => x.PrimaryUom);
                    case 14:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                    case 15:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 16:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Remark) : models.OrderBy(x => x.Remark);

                }
            }


            public static List<DetailModel.RollDetailModel> Search(DataTableAjaxPostViewModel data, List<DetailModel.RollDetailModel> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (
                        !string.IsNullOrEmpty(p.Subinventory) && p.Subinventory.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Id.ToString()) && p.Id.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Locator) && p.Locator.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Item_No) && p.Item_No.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PaperType) && p.PaperType.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.BaseWeight) && p.BaseWeight.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Specification) && p.Specification.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.TheoreticalWeight) && p.TheoreticalWeight.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.TransactionQuantity) && p.TransactionQuantity.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.TransactionUom) && p.TransactionUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimanyQuantity) && p.PrimanyQuantity.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimaryUom) && p.PrimaryUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.LotNumber) && p.LotNumber.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                        ).ToList();
                }
                return model;
            }


        }


        internal class FlatDetailModelDTOrder
        {
            public static IOrderedEnumerable<DetailModel.FlatDetailModel> Order(List<Order> orders, IEnumerable<DetailModel.FlatDetailModel> models)
            {
                IOrderedEnumerable<DetailModel.FlatDetailModel> orderedModel = null;
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

            private static IOrderedEnumerable<DetailModel.FlatDetailModel> OrderBy(int column, string dir, IEnumerable<DetailModel.FlatDetailModel> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWeight) : models.OrderBy(x => x.ReamWeight);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Pieces_Qty) : models.OrderBy(x => x.Pieces_Qty);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Qty) : models.OrderBy(x => x.Qty);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Remark) : models.OrderBy(x => x.Remark);


                }
            }

            private static IOrderedEnumerable<DetailModel.FlatDetailModel> ThenBy(int column, string dir, IOrderedEnumerable<DetailModel.FlatDetailModel> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWeight) : models.OrderBy(x => x.ReamWeight);
                    case 7:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
                    case 8:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Pieces_Qty) : models.OrderBy(x => x.Pieces_Qty);
                    case 9:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Qty) : models.OrderBy(x => x.Qty);
                    case 10:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                    case 11:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Remark) : models.OrderBy(x => x.Remark);
                }
            }

            public static List<DetailModel.FlatDetailModel> Search(DataTableAjaxPostViewModel data, List<DetailModel.FlatDetailModel> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (!string.IsNullOrEmpty(p.Id.ToString()) && p.Id.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Locator) && p.Locator.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Item_No) && p.Item_No.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.ReamWeight) && p.ReamWeight.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PackingType) && p.PackingType.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Pieces_Qty) && p.Pieces_Qty.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Qty) && p.Qty.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Remark) && p.Remark.ToLower().Contains(search.ToLower()))
                        ).ToList();
                }
                return model;
            }
        }
    }
}