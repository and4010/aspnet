using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class YieldVariance
    {
        public long OspYieldVarianceId { set; get; }

        public long OspHeaderId { set; get; }

        [DisplayName("損耗重量(KG)")]
        [DisplayFormat(DataFormatString = "{0:0.#####}", ApplyFormatInEditMode = true)]
        public decimal LossWeight { set; get; }

        [DisplayName("得率(%)")]
        public decimal Rate { set; get; }

        [DisplayName("總用紙重(KG)")]
        [DisplayFormat(DataFormatString = "{0:0.#####}", ApplyFormatInEditMode = true)]
        public decimal InvestWeight { set; get; }

        [DisplayName("總產出重(KG)")]
        [DisplayFormat(DataFormatString = "{0:0.#####}", ApplyFormatInEditMode = true)]
        public decimal ProductWeight { set; get; }
    }
}