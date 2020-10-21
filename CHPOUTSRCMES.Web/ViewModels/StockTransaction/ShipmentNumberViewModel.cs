using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.ViewModels.StockTransaction
{
    public class ShipmentNumberViewModel
    {
        [Display(Name = "出貨編號")]
        public string ShipmentNumber { set; get; }

        /// <summary>
        /// 新增出貨編號方式
        /// </summary>
        public IEnumerable<SelectListItem> CreateTypeList { set; get; }

        [Display(Name = "新增方式")]
        public string CreateType { set; get; }

    }
}