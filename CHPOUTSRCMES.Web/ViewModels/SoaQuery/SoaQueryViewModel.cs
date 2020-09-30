using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Jsons.Requests;
using CHPOUTSRCMES.Web.Models.SoaQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels.SoaQuery
{
    public class SoaQueryViewModel
    {
        public SoaQueryModel Fields { set; get; }

        public IEnumerable<SelectListItem> ProcessCodeList { set; get; }

        public IEnumerable<SelectListItem> ErrorOptionList { set; get; }


        public static IEnumerable<SelectListItem> getProcessCodeList()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "全部", Value = "*" },
                new SelectListItem() { Text = "進櫃入庫-資料轉入", Value = "XXIFP217" },
                new SelectListItem() { Text = "進櫃入庫-資料回傳", Value = "XXIFP218" },
                new SelectListItem() { Text = "出貨-資料轉入", Value = "XXIFP220" },
                new SelectListItem() { Text = "出貨-資料回傳", Value = "XXIFP221" },
                new SelectListItem() { Text = "加工-資料轉入", Value = "XXIFP219" },
                new SelectListItem() { Text = "加工-生產批物料耗用", Value = "XXIFP210" },
                new SelectListItem() { Text = "加工-生產批完工入庫且保留", Value = "XXIFP211" },
                new SelectListItem() { Text = "加工-生產批完工狀態變更", Value = "XXIFP213" },
                new SelectListItem() { Text = "庫存異動", Value = "XXIFP222" }
            };
        }

        public static IEnumerable<SelectListItem> getErrorOptionList()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "全部", Value = "*" },
                new SelectListItem() { Text = "有", Value = "1" },
                new SelectListItem() { Text = "無", Value = "0" }
            };
        }

    }
}