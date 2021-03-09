using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class ItemNoUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public ItemNoUOW(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 取得規格
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCatalog_elem_val_050(int count = 100)
        {
            var SpecList = new List<SelectListItem>();
            try
            {
                var tempList = itemsTRepository.GetAll()
                    .AsNoTracking()
                    .GroupBy(x => x.CatalogElemVal050)
                    .Take(count)
                    .Select(x => new SelectListItem()
                     {
                         Text = x.Key,
                         Value = x.Key,
                     })
                    .OrderBy(x => x.Value)
                    .ToList();
                SpecList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                SpecList.AddRange(tempList);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
            return SpecList;
        }

        /// <summary>
        /// 取得紙別
        /// </summary>
        public List<SelectListItem> GetCatalog_elem_val_020(int count = 100)
        {
            var TypeList = new List<SelectListItem>();
            try
            {
                var tempList = itemsTRepository.GetAll()
                    .AsNoTracking()
                    .GroupBy(x => x.CatalogElemVal020)
                    .Take(count)
                    .Select(x => new SelectListItem()
                     {
                         Text = x.Key,
                         Value = x.Key,
                     })
                    .OrderBy(x => x.Value)
                    .ToList();
                TypeList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                TypeList.AddRange(tempList);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
            return TypeList;
        }

        /// <summary>
        /// 紙別類型
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCatalog_elem_val_070(int count = 100)
        {
            var TypeList = new List<SelectListItem>();
            try
            {
                var tempList = itemsTRepository.GetAll()
                    .AsNoTracking()
                    .GroupBy(x => x.CatalogElemVal070)
                    .Take(count)
                    .Select(x => new SelectListItem()
                     {
                         Text = x.Key,
                         Value = x.Key,
                     }).ToList();
                TypeList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                TypeList.AddRange(tempList);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
            return TypeList;
        }

        /// <summary>
        /// 取得料號組織
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetOrganization_code(int count = 100)
        {
            var OrgList = new List<SelectListItem>();
            try
            {
                using (var db = new MesContext())
                {
                    var tempList = db.OrgItemsTs
                        .Join(db.OrganizationTs, c => c.OrganizationId, d => d.OrganizationId,
                        (c, d) => d)
                        .AsNoTracking()
                        .GroupBy(x => x.OrganizationCode)
                        .Select(s => new SelectListItem()
                        {
                            Text = s.FirstOrDefault().OrganizationCode,
                            Value = s.FirstOrDefault().OrganizationId.ToString(),
                        }).ToList();
                    OrgList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                    OrgList.AddRange(tempList);
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
            return OrgList;
        }

        /// <summary>
        /// 取得基本資料-料號表單資料
        /// </summary>
        public ResultPageModel<PartNoModel> GetItemNo(DataTableAjaxPostViewModel data, string Catalog_elem_val_050, string Catalog_elem_val_020, string Catalog_elem_val_070, string OrganizationId)
        {
            ResultPageModel<PartNoModel> model = new ResultPageModel<PartNoModel>();
            model.Draw = data.Draw;
            long organId = 0;
            long.TryParse(OrganizationId, out organId);
            try
            {
                var items = itemsTRepository.GetAll().Join(orgItemRepository.GetAll(), x => x.InventoryItemId, y => y.InventoryItemId, (x, y) => new { Item = x, OrganId = y.OrganizationId });

                if (!string.IsNullOrEmpty(Catalog_elem_val_050)
                    && Catalog_elem_val_050.CompareTo("*") != 0)
                {
                    items = items.Where(x => x.Item.CatalogElemVal050 == Catalog_elem_val_050);
                }
                if (!string.IsNullOrEmpty(Catalog_elem_val_020)
                    && Catalog_elem_val_020.CompareTo("*") != 0)
                {
                    items = items.Where(x => x.Item.CatalogElemVal020 == Catalog_elem_val_020);
                }
                if (!string.IsNullOrEmpty(Catalog_elem_val_070)
                    && Catalog_elem_val_070.CompareTo("*") != 0)
                {
                    items = items.Where(x => x.Item.CatalogElemVal070 == Catalog_elem_val_070);
                }
                if (organId > 0)
                {
                    items = items.Where(x => x.OrganId == organId);
                }

                var list = items.Select(x => new PartNoModel()
                {
                    Catalog_elem_val_010 = x.Item.CatalogElemVal010,
                    Catalog_elem_val_020 = x.Item.CatalogElemVal020,
                    Catalog_elem_val_030 = x.Item.CatalogElemVal030,
                    Catalog_elem_val_040 = x.Item.CatalogElemVal040,
                    Catalog_elem_val_050 = x.Item.CatalogElemVal050,
                    Catalog_elem_val_060 = x.Item.CatalogElemVal060,
                    Catalog_elem_val_070 = x.Item.CatalogElemVal070,
                    Catalog_elem_val_080 = x.Item.CatalogElemVal080,
                    Catalog_elem_val_090 = x.Item.CatalogElemVal090,
                    Catalog_elem_val_100 = x.Item.CatalogElemVal100,
                    Catalog_elem_val_110 = x.Item.CatalogElemVal110,
                    Catalog_elem_val_120 = x.Item.CatalogElemVal120,
                    Catalog_elem_val_130 = x.Item.CatalogElemVal130,
                    Catalog_elem_val_140 = x.Item.CatalogElemVal140,
                    Inventory_item_id = x.Item.InventoryItemId,
                    Item_number = x.Item.ItemNumber,
                    Item_type = x.Item.ItemType,
                    Item_desc_eng = x.Item.ItemDescEng,
                    Item_desc_sch = x.Item.ItemDescSch,
                    Item_desc_tch = x.Item.ItemDescTch,
                    Category_code_control = x.Item.CategoryCodeControl,
                    Category_code_cost = x.Item.CategoryCodeCost,
                    Category_code_inv = x.Item.CategoryCodeInv,
                    Category_name_control = x.Item.CategoryNameControl,
                    Category_name_cost = x.Item.CategoryNameCost,
                    Category_name_inv = x.Item.CategoryNameInv,
                    Primary_uom_code = x.Item.PrimaryUomCode,
                    Secondary_uom_code = x.Item.SecondaryUomCode,
                    Inventory_item_status_code = x.Item.InventoryItemStatusCode,
                    Created_by = x.Item.CreatedBy,
                    Creation_Date = x.Item.CreationDate,
                    Last_Updated_by = x.Item.LastUpdateBy,
                    Last_Update_Date = x.Item.LastUpdateDate,
                    Organization_code = x.OrganId,
                    Id = 0
                });

                string search = data.Search.Value;
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    list = list.Where(p => (
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
                        || (!string.IsNullOrEmpty(p.Catalog_elem_val_130) && p.Catalog_elem_val_130.ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Created_by.ToString()) && p.Created_by.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Creation_Date.ToString()) && p.Creation_Date.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Last_Update_Date.ToString()) && p.Last_Update_Date.ToString().ToLower().Contains(search.ToLower()))
                        || (!string.IsNullOrEmpty(p.Last_Updated_by.ToString()) && p.Last_Updated_by.ToString().ToLower().Contains(search.ToLower()))
                        );
                }
                else
                {

                }
                model.RecordTotal = list.Count();
                model.RecordFiltered = list.Count();

                var records = Order(data.Order, list).Skip(data.Start).Take(data.Length).ToList();


                model.List = records;
                model.Success = true;
                model.Code = ResultModel.CODE_SUCCESS;
                model.Msg = "";


            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));


                model.RecordFiltered = 0;
                model.RecordTotal = 0;
                model.List = new List<PartNoModel>();
                model.Code = -99;
                model.Success = false;
                model.Msg = e.Message;


            }
            return model;

        }

        /// <summary>
        /// 基本資料-料號表格排序
        /// </summary>
        public static IOrderedQueryable<PartNoModel> Order(List<Order> orders, IQueryable<PartNoModel> models)
        {
            IOrderedQueryable<PartNoModel> orderedModel = null;
            if (orders.Count() > 0)
            {
                orderedModel = OrderBy(orders[0].Column, orders[0].Dir, models);


                for (int i = 1; i < orders.Count(); i++)
                {
                    orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
                }
            }
            return orderedModel;
        }

        /// <summary>
        /// 基本資料-料號表格排序
        /// </summary>
        private static IOrderedQueryable<PartNoModel> OrderBy(int column, string dir, IQueryable<PartNoModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_inv) : models.OrderBy(x => x.Category_code_inv);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_cost) : models.OrderBy(x => x.Category_code_cost);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_eng) : models.OrderBy(x => x.Item_desc_eng);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_tch) : models.OrderBy(x => x.Item_desc_tch);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Primary_uom_code) : models.OrderBy(x => x.Primary_uom_code);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Secondary_uom_code) : models.OrderBy(x => x.Secondary_uom_code);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_type) : models.OrderBy(x => x.Item_type);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_010) : models.OrderBy(x => x.Catalog_elem_val_010);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_020) : models.OrderBy(x => x.Catalog_elem_val_020);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_030) : models.OrderBy(x => x.Catalog_elem_val_030);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_040) : models.OrderBy(x => x.Catalog_elem_val_040);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_050) : models.OrderBy(x => x.Catalog_elem_val_050);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_060) : models.OrderBy(x => x.Catalog_elem_val_060);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_070) : models.OrderBy(x => x.Catalog_elem_val_070);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_100) : models.OrderBy(x => x.Catalog_elem_val_100);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_110) : models.OrderBy(x => x.Catalog_elem_val_110);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_130) : models.OrderBy(x => x.Catalog_elem_val_130);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creation_Date) : models.OrderBy(x => x.Creation_Date);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_by) : models.OrderBy(x => x.Last_Updated_by);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Update_Date) : models.OrderBy(x => x.Last_Update_Date);
            }
        }

        /// <summary>
        /// 基本資料-料號表格排序
        /// </summary>
        private static IOrderedQueryable<PartNoModel> ThenBy(int column, string dir, IOrderedQueryable<PartNoModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_inv) : models.OrderBy(x => x.Category_code_inv);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Category_code_cost) : models.OrderBy(x => x.Category_code_cost);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_eng) : models.OrderBy(x => x.Item_desc_eng);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_desc_tch) : models.OrderBy(x => x.Item_desc_tch);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Primary_uom_code) : models.OrderBy(x => x.Primary_uom_code);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Secondary_uom_code) : models.OrderBy(x => x.Secondary_uom_code);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_type) : models.OrderBy(x => x.Item_type);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_010) : models.OrderBy(x => x.Catalog_elem_val_010);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_020) : models.OrderBy(x => x.Catalog_elem_val_020);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_030) : models.OrderBy(x => x.Catalog_elem_val_030);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_040) : models.OrderBy(x => x.Catalog_elem_val_040);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_050) : models.OrderBy(x => x.Catalog_elem_val_050);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_060) : models.OrderBy(x => x.Catalog_elem_val_060);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_070) : models.OrderBy(x => x.Catalog_elem_val_070);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_100) : models.OrderBy(x => x.Catalog_elem_val_100);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_110) : models.OrderBy(x => x.Catalog_elem_val_110);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Catalog_elem_val_130) : models.OrderBy(x => x.Catalog_elem_val_130);
                case 18:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 19:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Creation_Date) : models.OrderBy(x => x.Creation_Date);
                case 20:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_by) : models.OrderBy(x => x.Last_Updated_by);
                case 21:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Update_Date) : models.OrderBy(x => x.Last_Update_Date);
            }
        }


    }
}