using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace CHPOUTSRCMES.Web.ViewModels.Purchase
{
    public class PurchaseViewModel
    {
        [Display(Name = "月")]
        public string Month { set; get; }
        [Display(Name = "年")]
        public string Year { set; get; }
        [Display(Name = "儲位")]
        public string Locator { set; get; }
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


        public static List<DetailModel.RollDetailModel> StockInRoll = new List<DetailModel.RollDetailModel>();


        public List<FullCalendarEventModel> GetFullCalendarModel()
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).getFullCalenderList();
            }
        }

        public List<DetailModel.RollModel> GetRollHeader(string CONTAINER_NO)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetPaperRollHeaderList(CONTAINER_NO);
            }
        }

        public List<DetailModel.FlatModel> GetFlatHeader(string CONTAINER_NO)
        {

            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatHeaderList(CONTAINER_NO);
            }
        }



        public static void GetStockInRoll()
        {

        }

        public List<DetailModel.FlatDetailModel> GetFlatPickT(string CONTAINER_NO)
        {

            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatDetailList(CONTAINER_NO);
            }

        }

        public List<DetailModel.RollDetailModel> SaveRollBarcode(String Barcode, ref Boolean Boolean, ref Boolean BarcodeStatus)
        {
            List<DetailModel.RollDetailModel> model = StockInRoll;
            try
            {
                var sr = model.First(r => r.Barcode == Barcode);
                if (sr.Status == "已入庫")
                {
                    BarcodeStatus = false;
                }
                else
                {
                    sr.Status = "已入庫";
                }

            }
            catch (Exception e)
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

        public Boolean GetRollEditRemak(string remak, int id, String status, string Reason)
        {

            var Id = StockInRoll.Single(r => r.Id == id);

            if (Id != null)
            {
                Id.Remark = remak;
                Id.Reason = Reason;
            }

            return true;
        }

        public Boolean GetFlatEditRemak(int id, string Reason, string remak)
        {

            //var Id = StockInFlat.Single(r => r.Id == id);

            //if (Id != null)
            //{
            //    Id.Remark = remak;
            //    Id.Reason = Reason;
            //}


            return true;
        }


        public DetailModel.FlatDetailModel GetFlatEdit(string Id)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatEdit(Id);
            }
        }

        public List<DetailModel.FlatDetailModel> SaveFlatBarcode(string Barcode, ref Boolean Boolean, ref Boolean BarcodeStatus)
        {
            //List<DetailModel.FlatDetailModel> model = StockInFlat;
            //try
            //{
            //    var sf = model.First(r => r.Barcode == Barcode);
            //    if (sf.Status == "已入庫")
            //    {
            //        BarcodeStatus = false;
            //    }
            //    else
            //    {
            //        sf.Status = "已入庫";
            //    }

            //}
            //catch (Exception e)
            //{
            //    Boolean = false;
            //}



            return null;
        }

        public decimal GetFlatNumberTab(string CabinetNumber)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatNumberTab(CabinetNumber);
            }
        }

        public decimal GetPaperRollNumberTab(string CabinetNumber)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetPaperRollNumberTab(CabinetNumber);
            }
            
        }

        public DetailModel.RollDetailModel CheckLotNumber()
        {
            DetailModel.RollDetailModel paperRollDetail = new DetailModel.RollDetailModel();
            paperRollDetail.Item_No = "4FHIZA03000787RL00";
            paperRollDetail.Subinventory = "TB2";
            paperRollDetail.Locator = "SFG";
            return paperRollDetail;
        }

        //儲存照片
        public void SavePhoto(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (var context = new MesContext())
                {
                    new PurchaseUOW(context).SavePhoto(file);
                }

            }

        }

        //入庫行事曆取得月份
        public List<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                string month = i.ToString();
                month = month.PadLeft(2, '0');
                months.Add(new SelectListItem()
                {
                    Text = month,
                    Value = month,
                    Selected = false,
                });
            }
            return months;
        }

        //入庫行事曆取得年份
        public List<SelectListItem> GetYears()
        {
            List<SelectListItem> years = new List<SelectListItem>();
            int nowYear = DateTime.Now.Year;

            if (nowYear < 100)
            {
                nowYear = 100; //fullcendar支援的最小年份
            }

            int maxYear = nowYear + 1;
            int minYear = nowYear - 4;

            if (minYear < 100)
            {
                minYear = 100; //fullcendar支援的最小年份
            }

            for (int i = minYear; i <= maxYear; i++)
            {
                string year = i.ToString();
                year = year.PadLeft(4, '0');
                years.Add(new SelectListItem()
                {
                    Text = year,
                    Value = year,
                    Selected = false,
                });
            }
            return years;
        }


        //編輯取得原因
        public List<SelectListItem> GetReason()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetReasonDropDownList(MasterUOW.DropDownListType.Choice);
            }
        }


        //編輯取得儲位
        public List<SelectListItem> GetLocator()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetLocatorDropDownList("*", "*", MasterUOW.DropDownListType.Choice);
            }

        }

        //編輯取得儲位
        public List<SelectListItem> GetSubinventoryList()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetSubinventoryDropDownList("*", MasterUOW.DropDownListType.Choice);
            }

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
                        || (!string.IsNullOrEmpty(p.Pieces_Qty.ToString()) && p.Pieces_Qty.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Qty.ToString()) && p.Qty.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Remark) && p.Remark.ToLower().Contains(search.ToLower()))
                        ).ToList();
                }
                return model;
            }
        }
    }
}