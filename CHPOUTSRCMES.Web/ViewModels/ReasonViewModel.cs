using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class ReasonViewModel
    {
        public ReasonModel ReasonModel { set; get; }

        public static List<ReasonModel> models = new List<ReasonModel>(); 



        public List<ReasonModel> GetReason()
        {
            models.Add(new ReasonModel {
                Reason_code= "A",
                Reason_desc = "破損",
                Create_by = "華紙管理員",
                Create_date = DateTime.Now,
                Last_update_by = "華紙管理員",
                Last_Create_date = DateTime.Now,
            });


            return models;
        }


        internal class ReasonModelDTOrder
        {
            public static IOrderedEnumerable<ReasonModel> Order(List<Order> orders, IEnumerable<ReasonModel> models)
            {
                IOrderedEnumerable<ReasonModel> orderedModel = null;
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

            private static IOrderedEnumerable<ReasonModel> OrderBy(int column, string dir, IEnumerable<ReasonModel> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason_code) : models.OrderBy(x => x.Reason_code);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason_desc) : models.OrderBy(x => x.Reason_desc);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Create_by) : models.OrderBy(x => x.Create_by);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Create_date) : models.OrderBy(x => x.Create_date);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_update_by) : models.OrderBy(x => x.Last_update_by);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Create_date) : models.OrderBy(x => x.Last_Create_date);


                }
            }

            private static IOrderedEnumerable<ReasonModel> ThenBy(int column, string dir, IOrderedEnumerable<ReasonModel> models)
            {
                switch (column)
                {
                    default:
                    case 1:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason_code) : models.OrderBy(x => x.Reason_code);
                    case 2:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason_desc) : models.OrderBy(x => x.Reason_desc);
                    case 3:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Create_by) : models.OrderBy(x => x.Create_by);
                    case 4:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Create_date) : models.OrderBy(x => x.Create_date);
                    case 5:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_update_by) : models.OrderBy(x => x.Last_update_by);
                    case 6:
                        return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Create_date) : models.OrderBy(x => x.Last_Create_date);

                }
            }

            public static List<ReasonModel> Search(DataTableAjaxPostViewModel data, List<ReasonModel> model)
            {
                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    model = model.Where(p => (!string.IsNullOrEmpty(p.Reason_code.ToString()) && p.Reason_code.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Reason_desc) && p.Reason_desc.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Create_by) && p.Create_by.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Last_update_by) && p.Last_update_by.ToLower().Contains(search.ToLower()))).ToList();
                }
                return model;
            }
        }
    }
}