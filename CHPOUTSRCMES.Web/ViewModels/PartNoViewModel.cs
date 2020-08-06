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

        public List<PartNoModel> GetItemNo(string Catalog_elem_val_050, string Catalog_elem_val_020, string Catalog_elem_val_070,string Organization_code)
        {
            using (var context = new MesContext())
            {
                return new ItemNoUOW(context).GetItemNo(Catalog_elem_val_050, Catalog_elem_val_020, Catalog_elem_val_070, Organization_code);
            }
        }


        public static IOrderedEnumerable<PartNoModel> Order(List<Order> orders, IEnumerable<PartNoModel> models)
        {
            IOrderedEnumerable<PartNoModel> orderedModel = null;
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

        private static IOrderedEnumerable<PartNoModel> OrderBy(int column, string dir, IEnumerable<PartNoModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_inv) : models.OrderBy(x => x.Category_code_inv);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_cost) : models.OrderBy(x => x.Category_code_cost);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_control) : models.OrderBy(x => x.Category_code_control);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_tch) : models.OrderBy(x => x.Item_desc_tch);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Primary_uom_code) : models.OrderBy(x => x.Primary_uom_code);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Secondary_uom_code) : models.OrderBy(x => x.Secondary_uom_code);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_number) : models.OrderBy(x => x.Item_number);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Inventory_item_status_code) : models.OrderBy(x => x.Inventory_item_status_code);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_type) : models.OrderBy(x => x.Item_type);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_010) : models.OrderBy(x => x.Catalog_elem_val_010);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_020) : models.OrderBy(x => x.Catalog_elem_val_020);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_030) : models.OrderBy(x => x.Catalog_elem_val_030);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_040) : models.OrderBy(x => x.Catalog_elem_val_040);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_050) : models.OrderBy(x => x.Catalog_elem_val_050);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_060) : models.OrderBy(x => x.Catalog_elem_val_060);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_070) : models.OrderBy(x => x.Catalog_elem_val_070);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_100) : models.OrderBy(x => x.Catalog_elem_val_100);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_110) : models.OrderBy(x => x.Catalog_elem_val_110);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_by) : models.OrderBy(x => x.Last_Updated_by);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Update_Date) : models.OrderBy(x => x.Last_Update_Date);
   
            }
        }

        private static IOrderedEnumerable<PartNoModel> ThenBy(int column, string dir, IOrderedEnumerable<PartNoModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_inv) : models.OrderBy(x => x.Category_code_inv);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_cost) : models.OrderBy(x => x.Category_code_cost);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_control) : models.OrderBy(x => x.Category_code_control);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_tch) : models.OrderBy(x => x.Item_desc_tch);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Primary_uom_code) : models.OrderBy(x => x.Primary_uom_code);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Secondary_uom_code) : models.OrderBy(x => x.Secondary_uom_code);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_number) : models.OrderBy(x => x.Item_number);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Inventory_item_status_code) : models.OrderBy(x => x.Inventory_item_status_code);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_type) : models.OrderBy(x => x.Item_type);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_010) : models.OrderBy(x => x.Catalog_elem_val_010);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_020) : models.OrderBy(x => x.Catalog_elem_val_020);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_030) : models.OrderBy(x => x.Catalog_elem_val_030);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_040) : models.OrderBy(x => x.Catalog_elem_val_040);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_050) : models.OrderBy(x => x.Catalog_elem_val_050);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_060) : models.OrderBy(x => x.Catalog_elem_val_060);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_070) : models.OrderBy(x => x.Catalog_elem_val_070);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_100) : models.OrderBy(x => x.Catalog_elem_val_100);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_110) : models.OrderBy(x => x.Catalog_elem_val_110);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_by) : models.OrderBy(x => x.Last_Updated_by);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Update_Date) : models.OrderBy(x => x.Last_Update_Date);

            }
        }


        public static List<PartNoModel> Search(DataTableAjaxPostViewModel data, List<PartNoModel> model)
        {
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (
                    !string.IsNullOrEmpty(p.Organization_code.ToString()) && p.Organization_code.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Category_code_inv.ToString()) && p.Category_code_inv.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Category_code_cost) && p.Category_code_cost.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Category_code_control) && p.Category_code_control.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_desc_tch) && p.Item_desc_tch.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Primary_uom_code) && p.Primary_uom_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Secondary_uom_code) && p.Secondary_uom_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_number) && p.Item_number.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Inventory_item_status_code) && p.Inventory_item_status_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_type) && p.Item_type.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_010) && p.Catalog_elem_val_010.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_020) && p.Catalog_elem_val_020.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_030) && p.Catalog_elem_val_030.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_040) && p.Catalog_elem_val_040.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_050) && p.Catalog_elem_val_050.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_060) && p.Catalog_elem_val_060.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_070) && p.Catalog_elem_val_070.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_100) && p.Catalog_elem_val_100.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Catalog_elem_val_110) && p.Catalog_elem_val_110.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_Updated_by.ToString()) && p.Last_Updated_by.ToString().ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }





    }
}