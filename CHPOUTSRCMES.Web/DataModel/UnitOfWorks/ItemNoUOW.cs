using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class ItemNoUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<ITEMS_T> ItemTRepositityory;

        /// <summary>
        /// 組織料號
        /// </summary>
        private readonly IRepository<ORG_ITEMS_T> orgItemRepositityory;

        public ItemNoUOW(DbContext context) : base(context)
        {
            this.ItemTRepositityory = new GenericRepository<ITEMS_T>(this);
            this.orgItemRepositityory = new GenericRepository<ORG_ITEMS_T>(this);
        }

        /// <summary>
        /// 取得規格
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCatalog_elem_val_050()
        {
            var SpecList = new List<SelectListItem>();
            try
            {
                var tempList = ItemTRepositityory.GetAll().
                     GroupBy(x => x.CatalogElemVal050).
                     Select(x => new SelectListItem()
                     {
                         Text = x.Key,
                         Value = x.Key,
                     }).ToList();
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
        public List<SelectListItem> GetCatalog_elem_val_020()
        {
            var TypeList = new List<SelectListItem>();
            try
            {
                var tempList = ItemTRepositityory.GetAll().
                     GroupBy(x => x.CatalogElemVal020).
                     Select(x => new SelectListItem()
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
        /// 紙別類型
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCatalog_elem_val_070()
        {
            var TypeList = new List<SelectListItem>();
            try
            {
                var tempList = ItemTRepositityory.GetAll().
                     GroupBy(x => x.CatalogElemVal070).
                     Select(x => new SelectListItem()
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
        public List<SelectListItem> GetOrganization_code()
        {
            var OrgList = new List<SelectListItem>();
            try
            {
                using (var db = new MesContext())
                {
                    var tempList = db.OrgItemsTs.Join(
                        db.OrganizationTs,
                        c => c.OrganizationId,
                        d => d.OrganizationId,
                        (c, d) => new
                        {
                            x = c,
                            e = d,
                        }
                        ).GroupBy(x => x.e.OrganizationCode)
                        .Select(s => new SelectListItem()
                        {
                            Text = s.Key,
                            Value = s.Key,
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

        public List<PartNoModel> GetItemNo(string Catalog_elem_val_050, string Catalog_elem_val_020, string Catalog_elem_val_070, string Organization_code)
        {

            try
            {
                using (var mesContext = new MesContext())
                {
                    var OrganizationId = Organization_code == "*" ? 0 : mesContext.OrganizationTs.Where(x => x.OrganizationCode == Organization_code).SingleOrDefault().OrganizationId;
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"select 
t.[INVENTORY_ITEM_ID] as Inventory_item_id,
t.[ITEM_NUMBER] as Item_number,
t.[CATEGORY_CODE_INV] as Category_code_inv,
t.[CATEGORY_NAME_INV] as Category_name_inv,
t.[CATEGORY_CODE_COST] as Category_code_cost,
t.[CATEGORY_NAME_COST] as Category_name_cost,
t.[CATEGORY_CODE_CONTROL] as Category_code_control,
t.[CATEGORY_NAME_CONTROL] as Category_name_control,
t.[ITEM_DESC_ENG] as Item_desc_eng,
t.[ITEM_DESC_SCH] as Item_desc_sch,
t.[ITEM_DESC_TCH] as Item_desc_tch,
t.[PRIMARY_UOM_CODE] as Primary_uom_code,
t.[SECONDARY_UOM_CODE] as Secondary_uom_code,
t.[INVENTORY_ITEM_STATUS_CODE] as Inventory_item_status_code,
t.[ITEM_TYPE] as Item_type,
t.[CATALOG_ELEM_VAL_010] as Catalog_elem_val_010,
t.[CATALOG_ELEM_VAL_020] as Catalog_elem_val_020,
t.[CATALOG_ELEM_VAL_030] as Catalog_elem_val_030,
t.[CATALOG_ELEM_VAL_040] as Catalog_elem_val_040,
t.[CATALOG_ELEM_VAL_050] as Catalog_elem_val_050,
t.[CATALOG_ELEM_VAL_060] as Catalog_elem_val_060,
t.[CATALOG_ELEM_VAL_070] as Catalog_elem_val_070,
t.[CATALOG_ELEM_VAL_080] as Catalog_elem_val_080,
t.[CATALOG_ELEM_VAL_090] as Catalog_elem_val_090,
t.[CATALOG_ELEM_VAL_100] as Catalog_elem_val_100,
t.[CATALOG_ELEM_VAL_110] as Catalog_elem_val_110,
t.[CATALOG_ELEM_VAL_120] as Catalog_elem_val_120,
t.[CATALOG_ELEM_VAL_130] as Catalog_elem_val_130,
t.[CATALOG_ELEM_VAL_140] as Catalog_elem_val_140,
t.[CREATED_BY] as Created_by,
t.[CREATION_DATE] as Creation_Date,
t.[LAST_UPDATE_BY] as Last_Updated_by,
t.[LAST_UPDATE_DATE] as Last_Update_Date 
FROM [ITEMS_T] t
left join ORG_ITEMS_T ot on ot.INVENTORY_ITEM_ID = t.INVENTORY_ITEM_ID");
                    if (Catalog_elem_val_050 != "*")
                    {
                        cond.Add("t.CATALOG_ELEM_VAL_050 = @CATALOG_ELEM_VAL_050");
                        sqlParameterList.Add(new SqlParameter("@CATALOG_ELEM_VAL_050", Catalog_elem_val_050));
                    }
                    if (Catalog_elem_val_020 != "*")
                    {
                        cond.Add("t.CATALOG_ELEM_VAL_020 = @CATALOG_ELEM_VAL_020");
                        sqlParameterList.Add(new SqlParameter("@CATALOG_ELEM_VAL_020", Catalog_elem_val_020));
                    }
                    if (Catalog_elem_val_070 != "*")
                    {
                        cond.Add("t.CATALOG_ELEM_VAL_070 = @CATALOG_ELEM_VAL_070");
                        sqlParameterList.Add(new SqlParameter("@CATALOG_ELEM_VAL_070", Catalog_elem_val_070));
                    }
                    if (Organization_code != "*")
                    {
                        cond.Add("ot.ORGANIZATION_ID = @ORGANIZATION_ID");
                        sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", OrganizationId));
                    }

                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        return mesContext.Database.SqlQuery<PartNoModel>(commandText, sqlParameterList.ToArray()).ToList();
                    }
                    else
                    {
                        return mesContext.Database.SqlQuery<PartNoModel>(commandText).ToList();
                    }

                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<PartNoModel>();
            }
        }
    }
}