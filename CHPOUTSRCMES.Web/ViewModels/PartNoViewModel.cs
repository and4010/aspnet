using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class PartNoViewModel
    {
       
        public PartNoModel partNoModel { set; get; }

        public IEnumerable<SelectListItem> GetSpec()
        {
            using (var context = new MesContext())
            {
                return new ItemNoUOW(context).GetCatalog_elem_val_050();
            }
        }

        public IEnumerable<SelectListItem> GetTypePaper()
        {
            using (var context = new MesContext())
            {
                return new ItemNoUOW(context).GetCatalog_elem_val_020();
            }
        }

        public IEnumerable<SelectListItem> Get070()
        {
            using (var context = new MesContext())
            {
                return new ItemNoUOW(context).GetCatalog_elem_val_070();
            }
        }

        public IEnumerable<SelectListItem> GetOrganization_code()
        {
            using (var context = new MesContext())
            {
                return new ItemNoUOW(context).GetOrganization_code();
            }
        }

        public List<PartNoModel> GetItemNo(DataTableAjaxPostViewModel data, string Catalog_elem_val_050, string Catalog_elem_val_020, string Catalog_elem_val_070,string Organization_code)
        {
            using (var context = new MesContext())
            {
                return new ItemNoUOW(context).GetItemNo(data, Catalog_elem_val_050, Catalog_elem_val_020, Catalog_elem_val_070, Organization_code);
            }
        }
    }
}