using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class MachinePaperTypeViewModel
    {
        public MachinePaperType MachinePaperType { set; get; }


        public IEnumerable<SelectListItem> GetOrganization_code()
        {
            using (var context = new MesContext())
            {
                return new MaincheTypeUOW(context).GetOrganization_code();
            }
        }

        public List<MachinePaperType> GetMachinePaperTypes(string Organization_code)
        {
            using (var context = new MesContext())
            {
                return new MaincheTypeUOW(context).GetMachinePaperTypes(Organization_code);
            }
        }

        public static IOrderedEnumerable<MachinePaperType> Order(List<Order> orders, IEnumerable<MachinePaperType> models)
        {
            IOrderedEnumerable<MachinePaperType> orderedModel = null;
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

        private static IOrderedEnumerable<MachinePaperType> OrderBy(int column, string dir, IEnumerable<MachinePaperType> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Machine_code) : models.OrderBy(x => x.Machine_code);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Machine_meaning) : models.OrderBy(x => x.Machine_meaning);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Description) : models.OrderBy(x => x.Description);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Paper_type) : models.OrderBy(x => x.Paper_type);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Machine_num) : models.OrderBy(x => x.Machine_num);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Supplier_num) : models.OrderBy(x => x.Supplier_num);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Supplier_name) : models.OrderBy(x => x.Supplier_name);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creation_date) : models.OrderBy(x => x.Creation_date);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_updated_by) : models.OrderBy(x => x.Last_updated_by);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_update_date) : models.OrderBy(x => x.Last_update_date);


            }
        }

        private static IOrderedEnumerable<MachinePaperType> ThenBy(int column, string dir, IOrderedEnumerable<MachinePaperType> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Organization_code) : models.OrderBy(x => x.Organization_code);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Machine_code) : models.OrderBy(x => x.Machine_code);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Machine_meaning) : models.OrderBy(x => x.Machine_meaning);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Description) : models.OrderBy(x => x.Description);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Paper_type) : models.OrderBy(x => x.Paper_type);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Machine_num) : models.OrderBy(x => x.Machine_num);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Supplier_num) : models.OrderBy(x => x.Supplier_num);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Supplier_name) : models.OrderBy(x => x.Supplier_name);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creation_date) : models.OrderBy(x => x.Creation_date);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_updated_by) : models.OrderBy(x => x.Last_updated_by);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_update_date) : models.OrderBy(x => x.Last_update_date);


            }
        }


        public static List<MachinePaperType> Search(DataTableAjaxPostViewModel data, List<MachinePaperType> model)
        {
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (
                    !string.IsNullOrEmpty(p.Organization_id.ToString()) && p.Organization_id.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Organization_code.ToString()) && p.Organization_code.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Machine_code) && p.Machine_code.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Machine_meaning) && p.Machine_meaning.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Paper_type) && p.Paper_type.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Machine_num) && p.Machine_num.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Supplier_num) && p.Supplier_num.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Supplier_name) && p.Supplier_name.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Created_by.ToString()) && p.Created_by.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_updated_by.ToString()) && p.Last_updated_by.ToString().ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }

    }
}