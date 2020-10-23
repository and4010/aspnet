using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.ViewModels.Account
{
    public class AccountViewModel
    {
        public static void RestData()
        {
            model = new List<AccountModel>();
        }
 
        public static List<AccountModel> model = new List<AccountModel>();

        
        public static IOrderedEnumerable<AccountModel> Order(List<Order> orders, IEnumerable<AccountModel> models)
        {
            IOrderedEnumerable<AccountModel> orderedModel = null;
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

        private static IOrderedEnumerable<AccountModel> OrderBy(int column, string dir, IEnumerable<AccountModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RoleId) : models.OrderBy(x => x.RoleId);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RoleName) : models.OrderBy(x => x.RoleName);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Account) : models.OrderBy(x => x.Account);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Password) : models.OrderBy(x => x.Password);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Name) : models.OrderBy(x => x.Name);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Email) : models.OrderBy(x => x.Email);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SubinventoryCount) : models.OrderBy(x => x.SubinventoryCount);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);


            }
        }

        private static IOrderedEnumerable<AccountModel> ThenBy(int column, string dir, IOrderedEnumerable<AccountModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RoleId) : models.OrderBy(x => x.RoleId);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RoleName) : models.OrderBy(x => x.RoleName);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Account) : models.OrderBy(x => x.Account);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Password) : models.OrderBy(x => x.Password);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Name) : models.OrderBy(x => x.Name);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Email) : models.OrderBy(x => x.Email);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SubinventoryCount) : models.OrderBy(x => x.SubinventoryCount);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);


            }
        }

        public static List<AccountModel> Search(DataTableAjaxPostViewModel data, List<AccountModel> model)
        {
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (!string.IsNullOrEmpty(p.Id.ToString()) && p.Id.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.RoleId.ToString()) && p.RoleId.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.RoleName) && p.RoleName.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Account) && p.Account.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Password) && p.Password.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Email) && p.Email.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Status) && p.Status.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.SubinventoryCount.ToString()) && p.SubinventoryCount.ToString().ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }

    }
}