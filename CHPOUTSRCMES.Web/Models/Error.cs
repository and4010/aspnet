using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models
{
    public class Error
    {
        public int Id { get; set; }
        [Display(Name = "錯誤訊息")]
        public string Message { get; set; }
    }
}