using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Purchase
{
    public class PurchaseCalendar
    {
        public string CONTAINER_NO = "";
        public string DATE = "";
        public string STATUSE = "";
    }


    public class Chp_Container_Header_T
    {
        [Display(Name = "櫃表維護")]
        public Int64 Header_Id;
        [Display(Name = "作業單元ID")]
        public Int64 Org_Id;
        [Display(Name = "作業單元")]
        public string Org_Name;
        [Display(Name = "櫃表維護")]
        public Int64 Line_Id;
        [Display(Name = "櫃號")]
        public string ContainerNo;
        [Display(Name = "拖櫃日期時間")]
        public DateTime Mv_Container_Date;
        [Display(Name = "廠別ID")]
        public Int64 Organization_Id;
        [Display(Name = "廠別")]
        public string Organization_Code;
        [Display(Name = "倉庫")]
        public string Subinventory;
        [Display(Name = "狀態")]
        public string Status;

        public string color;


        [Display(Name = "建立人員")]
        public Int64 Created_By;
        [Display(Name = "建立日期")]
        public DateTime Creation_Date;
        [Display(Name = "更新人員")]
        public Int64 Last_Updated_By;
        [Display(Name = "更新日期")]
        public DateTime Last_Updated_Date;
    }
}