using CHPOUTSRCMES.Web.Models.SoaQuery;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CHPOUTSRCMES.Web.ViewModels.SoaQuery
{
    public class SoaQueryDetailViewModel
    {
        public SoaQueryModel HeaderField { set; get; }

        public SoaDetailQueryModel TableFields { set; get; }
    }
}