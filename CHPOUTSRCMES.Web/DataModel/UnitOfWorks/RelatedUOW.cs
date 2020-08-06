using CHPOUTSRCMES.Web.DataModel.Entiy.Information;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
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

        /// <summary>
        /// 餘切規格
        /// </summary>
        private readonly IRepository<RELATED_T> relatedTRepositiory;

        public RelatedUOW(DbContext context) : base(context)
        {
            this.relatedTRepositiory = new GenericRepository<RELATED_T>(this);
        }

        public List<SelectListItem> GetInventoryItemNumber(string ORGANIZATION_ID)
        {
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


                    List<SelectListItem> inventoryItemList = new List<SelectListItem>();
                    inventoryItemList.Add(new SelectListItem() { Text = "全部", Value = "*" });

                    if (ORGANIZATION_ID == "*")
                    {
                        var query = relatedTRepositiory.GetAll().GroupBy(x =>x.ItemNumber).
                                    Select(x => new SelectListItem
                                    {
                                        Text = x.Key,
                                        Value = x.Key
                                    }).ToList();

                        inventoryItemList.AddRange(query);
                    }
                    else
                    {
                        var relate = relatedTRepositiory.GetAll().Join(
                            mesContext.OrgItemsTs,
                            a => a.InventoryItemId,
                            b => b.InventoryItemId,
                            (a, b) => new { c = a, d = b }).Join(
                            mesContext.OrganizationTs.Where(x => x.OrganizationId == organizationId),
                            e => e.d.OrganizationId,
                            x => x.OrganizationId,
                            (e, x) => new { e.c, e.d, g = x }
                            ).Select(f => new SelectListItem{
                              Text =  f.c.ItemNumber,
                              Value = f.c.InventoryItemId.ToString(),
                            }).ToList();

                        inventoryItemList.AddRange(relate);
                    }
                    return inventoryItemList;
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }

        }



        public IEnumerable<SelectListItem> GetRelatedItem(string ORGANIZATION_ID, string INVENTORY_ITEM_ID)
        {
            //long organizationId = 0;
            //try
            //{
            //    if (ORGANIZATION_ID != "*")
            //    {
            //        organizationId = Convert.ToInt64(ORGANIZATION_ID);
            //    }
            //}
            //catch
            //{
            //    ORGANIZATION_ID = "*";
            //}


            //long inventoryItemId = 0;
            //try
            //{
            //    if (INVENTORY_ITEM_ID != "*")
            //    {
            //        inventoryItemId = Convert.ToInt64(INVENTORY_ITEM_ID);
            //    }
            //}
            //catch
            //{
            //    INVENTORY_ITEM_ID = "*";
            //}

            //List<ListItem> relatedItemList = new List<ListItem>();
            //relatedItemList.Add(new ListItem("全部", "*"));

            //if (ORGANIZATION_ID == "*" && INVENTORY_ITEM_ID == "*")
            //{
            //    var query = from ospRelatedDT in testSource
            //                select new ListItem
            //                {
            //                    Text = ospRelatedDT.RELATED_ITEMNUMBER,
            //                    Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
            //                };
            //    relatedItemList.AddRange(query.ToList());
            //}
            //else if (ORGANIZATION_ID != "*" && INVENTORY_ITEM_ID == "*")
            //{

            //    var query = from ospRelatedDT in testSource
            //                where organizationId == ospRelatedDT.ORGANIZATION_ID
            //                select new ListItem
            //                {
            //                    Text = ospRelatedDT.RELATED_ITEMNUMBER,
            //                    Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
            //                };
            //    relatedItemList.AddRange(query.ToList());
            //}
            //else if (ORGANIZATION_ID == "*" && INVENTORY_ITEM_ID != "*")
            //{

            //    var query = from ospRelatedDT in testSource
            //                where inventoryItemId == ospRelatedDT.INVENTORY_ITEM_ID
            //                select new ListItem
            //                {
            //                    Text = ospRelatedDT.RELATED_ITEMNUMBER,
            //                    Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
            //                };
            //    relatedItemList.AddRange(query.ToList());
            //}
            //else
            //{
            //    var query = from ospRelatedDT in testSource
            //                where organizationId == ospRelatedDT.ORGANIZATION_ID && inventoryItemId == ospRelatedDT.INVENTORY_ITEM_ID
            //                select new ListItem
            //                {
            //                    Text = ospRelatedDT.RELATED_ITEMNUMBER,
            //                    Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
            //                };
            //    relatedItemList.AddRange(query.ToList());
            //}


            //return relatedItemList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
            return new List<SelectListItem>();
        }

    }
}