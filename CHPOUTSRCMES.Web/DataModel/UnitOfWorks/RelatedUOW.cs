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
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class RelatedUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();


        public RelatedUOW(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 取得組成成份料號選單資料
        /// </summary>
        public List<SelectListItem> GetInventoryItemNumber(string ORGANIZATION_ID)
        {
            var InventoryItemList = new List<SelectListItem>();
            InventoryItemList.Add(new SelectListItem() { Text = "全部", Value = "*" });
            try
            {
                using (var mesContext = new MesContext())
                {

                    long organizationId = 0;
                    try
                    {
                        if (ORGANIZATION_ID != "*")
                        {
                            organizationId = Convert.ToInt64(ORGANIZATION_ID);
                        }
                    }
                    catch
                    {
                        ORGANIZATION_ID = "*";
                    }

                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
cast(RT.[INVENTORY_ITEM_ID] as nvarchar) as Value,
RT.[ITEM_NUMBER] as Text
FROM [RELATED_T] RT
LEFT JOIN ORG_ITEMS_T OT ON OT.INVENTORY_ITEM_ID = RT.INVENTORY_ITEM_ID
LEFT JOIN ORGANIZATION_T ORG ON ORG.ORGANIZATION_ID = OT.ORGANIZATION_ID");
                    if (ORGANIZATION_ID != "*")
                    {
                        cond.Add("ORG.ORGANIZATION_ID = @ORGANIZATION_ID");
                        sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", organizationId));
                    }

                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        InventoryItemList.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText, sqlParameterList.ToArray()).ToList());
                    }
                    else
                    {
                        InventoryItemList.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText).ToList());
                    }
                    return InventoryItemList;
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }



        /// <summary>
        /// 取得餘切料號選單資料
        /// </summary>
        public List<SelectListItem> GetRelatedItem(string ORGANIZATION_ID, string INVENTORY_ITEM_ID)
        {
            var RelatedItemList = new List<SelectListItem>();
            RelatedItemList.Add(new SelectListItem() { Text = "全部", Value = "*" });
            long organizationId = 0;
            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }


            long inventoryItemId = 0;
            try
            {
                if (INVENTORY_ITEM_ID != "*")
                {
                    inventoryItemId = Convert.ToInt64(INVENTORY_ITEM_ID);
                }
            }
            catch
            {
                INVENTORY_ITEM_ID = "*";
            }

            using (var mesContext = new MesContext())
            {

                try
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
    @"SELECT 
cast(RT.[RELATED_ITEM_ID] as nvarchar) as Value,
RT.[RELATED_ITEM_NUMBER] as Text
FROM [RELATED_T] RT
LEFT JOIN ORG_ITEMS_T OT ON OT.INVENTORY_ITEM_ID = RT.INVENTORY_ITEM_ID
LEFT JOIN ORGANIZATION_T ORG ON ORG.ORGANIZATION_ID = OT.ORGANIZATION_ID");
                    if (ORGANIZATION_ID != "*")
                    {
                        cond.Add("ORG.ORGANIZATION_ID = @ORGANIZATION_ID");
                        sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", organizationId));
                    }
                    if (INVENTORY_ITEM_ID != "*")
                    {
                        cond.Add("RT.INVENTORY_ITEM_ID = @INVENTORY_ITEM_ID");
                        sqlParameterList.Add(new SqlParameter("@INVENTORY_ITEM_ID", inventoryItemId));
                    }

                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        RelatedItemList.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText, sqlParameterList.ToArray()).ToList());
                    }
                    else
                    {
                        RelatedItemList.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText).ToList());
                    }
                    return RelatedItemList;
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new List<SelectListItem>();
                }

            }



        }



        /// <summary>
        /// 取得基本資料-餘切規格表單資料
        /// </summary>
        public List<OspRelatedDT> search(string ORGANIZATION_ID, string INVENTORY_ITEM_ID, string RELATED_ITEM_ID)
        {
            long organizationId = 0;
            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }


            long inventoryItemId = 0;
            try
            {
                if (INVENTORY_ITEM_ID != "*")
                {
                    inventoryItemId = Convert.ToInt64(INVENTORY_ITEM_ID);
                }
            }
            catch
            {
                INVENTORY_ITEM_ID = "*";
            }
            long relatedItemId = 0;
            try
            {
                if (!string.IsNullOrEmpty(RELATED_ITEM_ID) && RELATED_ITEM_ID != "*")
                {
                    relatedItemId = Convert.ToInt64(RELATED_ITEM_ID);
                }
            }
            catch
            {
                RELATED_ITEM_ID = "*";
            }

            using (var mesContext = new MesContext())
            {

                try
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT 
RT.[RELATED_ID],
RT.[INVENTORY_ITEM_ID],
RT.[ITEM_NUMBER],
RT.[ITEM_DESCRIPTION],
RT.[RELATED_ITEM_ID],
RT.[RELATED_ITEM_NUMBER],
RT.[RELATED_ITEM_DESCRIPTION],
RT.[CONTROL_FLAG],
RT.[CREATED_BY],
RT.[CREATION_DATE],
RT.[LAST_UPDATE_BY],
RT.[LAST_UPDATE_DATE]
FROM [RELATED_T] RT
LEFT JOIN ORG_ITEMS_T OT ON OT.INVENTORY_ITEM_ID = RT.INVENTORY_ITEM_ID
LEFT JOIN ORGANIZATION_T ORG ON ORG.ORGANIZATION_ID = OT.ORGANIZATION_ID");
                    if (ORGANIZATION_ID != "*")
                    {
                        cond.Add("ORG.ORGANIZATION_ID = @ORGANIZATION_ID");
                        sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", organizationId));
                    }
                    if (INVENTORY_ITEM_ID != "*")
                    {
                        cond.Add("RT.INVENTORY_ITEM_ID = @INVENTORY_ITEM_ID");
                        sqlParameterList.Add(new SqlParameter("@INVENTORY_ITEM_ID", inventoryItemId));
                    }
                    if (RELATED_ITEM_ID != "*")
                    {
                        cond.Add("RT.RELATED_ITEM_ID = @RELATED_ITEM_ID");
                        sqlParameterList.Add(new SqlParameter("@RELATED_ITEM_ID", relatedItemId));
                    }

                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        return mesContext.Database.SqlQuery<OspRelatedDT>(commandText, sqlParameterList.ToArray()).ToList();
                    }
                    else
                    {
                        return mesContext.Database.SqlQuery<OspRelatedDT>(commandText).ToList();
                    }

                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new List<OspRelatedDT>();
                }

            }


        }
    }
}