using System;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.Models.Information
{

    public class ReasonModel
    {

        [Display(Name = "原因ID")]
        public string Reason_code { set; get; }


        [Display(Name = "原因說明")]
        public string Reason_desc { set; get; }


        [Display(Name = "建立人員")]
        public string Create_by { set; get; }


        [Display(Name = "建立日期")]
        public DateTime Create_date { set; get; }


        [Display(Name = "更新人員")]
        public string Last_update_by { set; get; }

        [Display(Name = "建立日期")]
        public DateTime? Last_Create_date { set; get; }

    }
}