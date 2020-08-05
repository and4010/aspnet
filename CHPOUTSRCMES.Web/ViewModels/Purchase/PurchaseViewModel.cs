﻿using CHPOUTSRCMES.Web.Models;
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
using System.Web.Services.Protocols;
using Microsoft.Graph;
using NLog;

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
        private ILogger logger = LogManager.GetCurrentClassLogger();


        public DetailModel Detail { set; get; }

        public DetailModel.RollDetailModel RollDetailModel { set; get; }
        public DetailModel.FlatDetailModel FlatDetailModel { set; get; }

        /// <summary>
        /// 已入庫
        /// </summary>
        public const string PurchaseHeaderAlready = "0";

        /// <summary>
        /// 待入庫
        /// </summary>
        public const string PurchaseHeaderPending = "1";

        /// <summary>
        /// 取消
        /// </summary>
        public const string PurchaseHeaderCancel = "2";


        public List<FullCalendarEventModel> GetFullCalendarModel(string Subinventory)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).getFullCalenderList(Subinventory);
            }
        }

        /// <summary>
        /// 取得紙捲表頭資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DetailModel.RollModel> GetRollHeader(string CONTAINER_NO, string Status)
        {
            using (var context = new MesContext())
            {
                if (Status == PurchaseHeaderPending)
                {
                    return new PurchaseUOW(context).GetPaperRollHeaderList(CONTAINER_NO);
                }
                else
                {
                    return new PurchaseUOW(context).GetPaperRollHeaderHtList(CONTAINER_NO);
                }

            }
        }

        /// <summary>
        /// 取得平張表頭資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<DetailModel.FlatModel> GetFlatHeader(string CONTAINER_NO, string Status)
        {

            using (var context = new MesContext())
            {
                if (Status == PurchaseHeaderPending)
                {
                    return new PurchaseUOW(context).GetFlatHeaderList(CONTAINER_NO);
                }
                else
                {
                    return new PurchaseUOW(context).GetFlatHeaderHtList(CONTAINER_NO);
                }


            }
        }


        /// <summary>
        /// 匯入
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <param name="PaperRollModel"></param>
        /// <returns></returns>
        public ResultModel ImportPaperRollPickT(string CONTAINER_NO, List<DetailModel.RollDetailModel> PaperRollModel, string createby, string userName)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).ImportPaperRollDetail(CONTAINER_NO, PaperRollModel, createby, userName);
            }
        }

        /// <summary>
        /// 取得紙捲明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.RollDetailModel> GetPaperRollPickT(string CONTAINER_NO, string Status)
        {
            using (var context = new MesContext())
            {
                if (Status == PurchaseHeaderPending)
                {
                    return new PurchaseUOW(context).GetPaperRollDetailList(CONTAINER_NO);
                }
                else
                {
                    return new PurchaseUOW(context).GetHtPaperRollDetailList(CONTAINER_NO);
                }
            }
        }
        /// <summary>
        /// 取得平張明細資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public List<DetailModel.FlatDetailModel> GetFlatPickT(string CONTAINER_NO, string Status)
        {

            using (var context = new MesContext())
            {
                if (Status == PurchaseHeaderPending)
                {
                    return new PurchaseUOW(context).GetFlatDetailList(CONTAINER_NO);
                }
                else
                {
                    return new PurchaseUOW(context).GetHtFlatDetailList(CONTAINER_NO);
                }
            }

        }

        /// <summary>
        /// 紙捲儲存條碼已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public int SavePaperRollBarcode(String Barcode, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).SavePaperRollBarcode(Barcode, LastUpdateBy, LastUpdateUserName);
            }

        }

        /// <summary>
        /// 平張儲存條碼已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public int SaveFlatBarcode(string Barcode, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).SaveFlatBarcode(Barcode, LastUpdateBy, LastUpdateUserName);
            }
        }

        /// <summary>
        /// 取得紙捲編輯資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DetailModel.RollDetailModel GetPaperRollEdit(string Id)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetPaperRollEdit(Id);
            }
        }

        /// <summary>
        /// 取得紙捲檢視資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DetailModel.RollDetailModel GetPaperRollView(string Id)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetPaperRollView(Id);
            }
        }
        /// <summary>
        /// 編輯紙捲備註照片
        /// </summary>
        /// <param name="File"></param>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="Locator"></param>
        /// <param name="Remark"></param>
        public ResultModel PaperRollEditNote(HttpFileCollectionBase File, long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {
            using (var context = new MesContext())
            {
                //if (File != null || File.Count != 0)
                //{
                //    foreach (string i in File)
                //    {
                //        HttpPostedFileBase hpf = File[i] as HttpPostedFileBase;
                //        new PurchaseUOW(context).SavePhoto(hpf, id, LastUpdateBy);
                //    }
                //}
                return new PurchaseUOW(context).PaperRollEdit(File,id, Reason, Locator, Remark, LastUpdateBy, LastUpdateUserName);
            }
        }

        /// <summary>
        /// 編輯平張備註照片
        /// </summary>
        /// <param name="File"></param>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="Locator"></param>
        /// <param name="Remark"></param>
        public ResultModel FlatEditNote(HttpFileCollectionBase File, long id, string Reason, string Locator, string Remark, string LastUpdateBy, string LastUpdateUserName)
        {

            using (var context = new MesContext())
            {

                //if (File != null || File.Count != 0)
                //{
                //    foreach (string i in File)
                //    {
                //        HttpPostedFileBase hpf = File[i] as HttpPostedFileBase;
                //        new PurchaseUOW(context).SavePhoto(hpf, id, LastUpdateBy);
                //    }
                //}
                return new PurchaseUOW(context).FlatEdit(File,id, Reason, Locator, Remark, LastUpdateBy, LastUpdateUserName);
            }
        }

        /// <summary>
        /// 刪除excel匯入資料
        /// </summary>
        /// <param name="CONTAINER_NO"></param>
        /// <returns></returns>
        public ResultModel ExcelDelete(string CONTAINER_NO)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).DeleteExcel(CONTAINER_NO);
            }
        }


        /// <summary>
        /// 取得平張編輯資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DetailModel.FlatDetailModel GetFlatEdit(string Id)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatEdit(Id);
            }
        }

        /// <summary>
        /// 取得平張檢視資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DetailModel.FlatDetailModel GetFlatView(string Id)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatView(Id);
            }
        }

        /// <summary>
        /// 取得平張頁籤數量
        /// </summary>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public decimal GetFlatNumberTab(string CabinetNumber)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetFlatNumberTab(CabinetNumber);
            }
        }

        /// <summary>
        /// 取得紙捲頁籤數量
        /// </summary>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public decimal GetPaperRollNumberTab(string CabinetNumber)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetPaperRollNumberTab(CabinetNumber);
            }

        }


        //public DetailModel.RollDetailModel CheckLotNumber()
        //{
        //    DetailModel.RollDetailModel paperRollDetail = new DetailModel.RollDetailModel();
        //    paperRollDetail.Item_No = "4FHIZA03000787RL00";
        //    paperRollDetail.Subinventory = "TB2";
        //    paperRollDetail.Locator = "SFG";
        //    return paperRollDetail;
        //}

        /// <summary>
        /// 已入庫完成更改表頭狀態
        /// </summary>
        public ResultModel ChageHeaderStatus(string ContainerNo)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).ChangeHeaderStatus(ContainerNo);
            }
        }

        public List<string> GetPhoto(long id)
        {
            using (var context = new MesContext())
            {
                return new PurchaseUOW(context).GetPhoto(id);
            }

        }


        ///// <summary>
        ///// 儲存照片
        ///// </summary>
        ///// <param name="file"></param>
        //public void SavePhoto(HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        using (var context = new MesContext())
        //        {
        //            new PurchaseUOW(context).SavePhoto(file);
        //        }

        //    }

        //}

        /// <summary>
        /// 入庫行事曆取得月份
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 入庫行事曆取得年份
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 編輯取得原因
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetReason()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetReasonDropDownList(MasterUOW.DropDownListType.Choice);
            }
        }


        /// <summary>
        /// 編輯取得儲位
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetLocator()
        {
            using (var context = new MesContext())
            {
                return new MasterUOW(context).GetLocatorDropDownList("*", "*", MasterUOW.DropDownListType.Choice);
            }

        }

        /// <summary>
        /// 編輯取得儲位
        /// </summary>
        /// <returns></returns>
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
                        || (!string.IsNullOrEmpty(p.TransactionQuantity.ToString()) && p.TransactionQuantity.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.TransactionUom) && p.TransactionUom.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.PrimanyQuantity.ToString()) && p.PrimanyQuantity.ToString().ToLower().Contains(search.ToLower()))
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