using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class OspRelatedDT
    {
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_CODE { set; get; }
        public string ORGANIZATION_NAME { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_NUMBER { set; get; }
        public string ITEM_DESCRIPTION { set; get; }
        public long RELATED_ITEM_ID { set; get; }
        public string RELATED_ITEMNUMBER { set; get; }
        public string RELATED_ITEM_DESCRIPTION { set; get; }
        public long CREATED_BY { get; set; }
        public string CREATED_BY_NAME { get; set; }
        public DateTime? CREATION_DATE { get; set; }
        public long LAST_UPDATED_BY { get; set; }
        public string LAST_UPDATED_BY_NAME { get; set; }
        public DateTime? LAST_UPDATE_DATE { get; set; }


        public OspRelatedDT(long ORGANIZATION_ID, string ORGANIZATION_CODE, string ORGANIZATION_NAME,
            long INVENTORY_ITEM_ID, string ITEM_NUMBER, string ITEM_DESCRIPTION,
            long RELATED_ITEM_ID, string RELATED_ITEMNUMBER, string RELATED_ITEM_DESCRIPTION,
            long CREATED_BY, string CREATED_BY_NAME, DateTime CREATION_DATE, long LAST_UPDATED_BY, string LAST_UPDATED_BY_NAME, DateTime LAST_UPDATE_DATE)
        {
            this.ORGANIZATION_ID = ORGANIZATION_ID;
            this.ORGANIZATION_CODE = ORGANIZATION_CODE;
            this.ORGANIZATION_NAME = ORGANIZATION_NAME;
            this.INVENTORY_ITEM_ID = INVENTORY_ITEM_ID;
            this.ITEM_NUMBER = ITEM_NUMBER;
            this.ITEM_DESCRIPTION = ITEM_DESCRIPTION;
            this.RELATED_ITEM_ID = RELATED_ITEM_ID;
            this.RELATED_ITEMNUMBER = RELATED_ITEMNUMBER;
            this.RELATED_ITEM_DESCRIPTION = RELATED_ITEM_DESCRIPTION;
            this.CREATED_BY = CREATED_BY;
            this.CREATED_BY_NAME = CREATED_BY_NAME;
            this.CREATION_DATE = CREATION_DATE;
            this.LAST_UPDATED_BY = LAST_UPDATED_BY;
            this.LAST_UPDATED_BY_NAME = LAST_UPDATED_BY_NAME;
            this.LAST_UPDATE_DATE = LAST_UPDATE_DATE;
        }

    }

    public class OspRelatedData
    {

        //private static List<OspRelatedDT> dtData = new List<OspRelatedDT>();
        private List<OspRelatedDT> testSource = new List<OspRelatedDT>();

        public OspRelatedData()
        {
            if (testSource.Count == 0)
            {
                testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 614391, "4AKMXA01000280KRL00", "捲筒特級雙面銅版紙(M)", 749947, "4AEMXA0100007110965", "特級雙面銅版紙(M)", 1, "一力星1號", Convert.ToDateTime("2020-04-21"), 1, "一力星1號", Convert.ToDateTime("2020-04-21")));
                testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 614424, "4AKMXA01200350KRL00", "捲筒特級雙面銅版紙(M)", 749948, "4AEMXA0120008890635", "特級雙面銅版紙(M)", 2, "一力星2號", Convert.ToDateTime("2020-04-22"), 2, "一力星2號", Convert.ToDateTime("2020-04-22")));
                //testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 614428, "4AKMXA01500280KRL00", "捲筒特級雙面銅版紙(M)", 554395, "4AEMXA0150007110965", "特級雙面銅版紙(M)", 3, " 華紙1號", Convert.ToDateTime("2020-04-23"), 3, " 華紙1號", Convert.ToDateTime("2020-04-23")));
                //testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 1126128, "4AKMXA01800350KRL00", "捲筒特級雙面銅版紙(M)", 1275214, "4AEMXA0180008890635", "特級雙面銅版紙(M)", 1, "一力星1號", Convert.ToDateTime("2020-04-21"), 2, "一力星2號", Convert.ToDateTime("2020-04-21")));
                //testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 614570, "4AN23A006020889RL00", "捲筒高白雪面雜誌紙", 1078391, "4AM23A00602350K250K", "高白雪面雜誌紙", 2, "一力星2號", Convert.ToDateTime("2020-04-25"), 1, "一力星1號", Convert.ToDateTime("2020-04-26")));
                //testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 614597, "4AN2ZA006400787RL00", "捲筒高白雜誌", 553337, "4AM2ZA00640310K430K", "高白雜誌紙", 1, "一力星1號", Convert.ToDateTime("2020-04-24"), 1, "一力星1號", Convert.ToDateTime("2020-04-27")));
                //testSource.Add(new OspRelatedDT(265, "FTY", "INV_ORG_華紙總公司", 1118757, "4AEH0P0190007871092", "特級銅版紙(H)", 1228678, "4AEH0P01900310K430K", "特級雙面銅版紙(AEH0)", 3, " 華紙1號", Convert.ToDateTime("2020-04-21"), 1, "一力星1號", Convert.ToDateTime("2020-04-21")));
            }
        }

        //public List<OspRelatedDT> GetOspRelatedDT()
        //{
        //    return dtData;
        //}



        private IEnumerable<SelectListItem> getOrganizationList()
        {
            List<ListItem> organizationList = new List<ListItem>();
            organizationList.Add(new ListItem("全部", "*"));

            var query = from orgSubinventoryDT in testSource
                        group orgSubinventoryDT by new { orgSubinventoryDT.ORGANIZATION_CODE, orgSubinventoryDT.ORGANIZATION_ID } into g
                        select new ListItem
                        {
                            Text = g.Key.ORGANIZATION_CODE,
                            Value = g.Key.ORGANIZATION_ID.ToString()
                        };
            //organizationList.Add(new ListItem("INV_ORG_華紙總公司", "265"));
            //organizationList.Add(new ListItem("INV_ORG_華紙新屋廠", "285"));
            //organizationList.Add(new ListItem("INV_ORG_華紙久堂廠", "305"));
            organizationList.AddRange(query.ToList());
            return organizationList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public IEnumerable<SelectListItem> GetInventoryItemList(string ORGANIZATION_ID)
        {
            return getInventoryItemList(ORGANIZATION_ID);
        }

        private IEnumerable<SelectListItem> getInventoryItemList(string ORGANIZATION_ID)
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


            List<ListItem> inventoryItemList = new List<ListItem>();
            inventoryItemList.Add(new ListItem("全部", "*"));

            if (ORGANIZATION_ID == "*")
            {
                var query = from ospRelatedDT in testSource
                            select new ListItem
                            {
                                Text = ospRelatedDT.ITEM_NUMBER,
                                Value = ospRelatedDT.INVENTORY_ITEM_ID.ToString()
                            };

                inventoryItemList.AddRange(query.ToList());
            }
            else
            {
                var query = from ospRelatedDT in testSource
                            where organizationId == ospRelatedDT.ORGANIZATION_ID
                            select new ListItem
                            {
                                Text = ospRelatedDT.ITEM_NUMBER,
                                Value = ospRelatedDT.INVENTORY_ITEM_ID.ToString()
                            };

                inventoryItemList.AddRange(query.ToList());
            }

            return inventoryItemList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }



        public IEnumerable<SelectListItem> GetRelatedItemList(string ORGANIZATION_ID, string INVENTORY_ITEM_ID)
        {
            return getRelatedItemList(ORGANIZATION_ID, INVENTORY_ITEM_ID);
        }

        private IEnumerable<SelectListItem> getRelatedItemList(string ORGANIZATION_ID, string INVENTORY_ITEM_ID)
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

            List<ListItem> relatedItemList = new List<ListItem>();
            relatedItemList.Add(new ListItem("全部", "*"));

            if (ORGANIZATION_ID == "*" && INVENTORY_ITEM_ID == "*")
            {
                var query = from ospRelatedDT in testSource
                            select new ListItem
                            {
                                Text = ospRelatedDT.RELATED_ITEMNUMBER,
                                Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
                            };
                relatedItemList.AddRange(query.ToList());
            }
            else if (ORGANIZATION_ID != "*" && INVENTORY_ITEM_ID == "*")
            {

                var query = from ospRelatedDT in testSource
                            where organizationId == ospRelatedDT.ORGANIZATION_ID
                            select new ListItem
                            {
                                Text = ospRelatedDT.RELATED_ITEMNUMBER,
                                Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
                            };
                relatedItemList.AddRange(query.ToList());
            }
            else if (ORGANIZATION_ID == "*" && INVENTORY_ITEM_ID != "*")
            {

                var query = from ospRelatedDT in testSource
                            where inventoryItemId == ospRelatedDT.INVENTORY_ITEM_ID
                            select new ListItem
                            {
                                Text = ospRelatedDT.RELATED_ITEMNUMBER,
                                Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
                            };
                relatedItemList.AddRange(query.ToList());
            }
            else
            {
                var query = from ospRelatedDT in testSource
                            where organizationId == ospRelatedDT.ORGANIZATION_ID && inventoryItemId == ospRelatedDT.INVENTORY_ITEM_ID
                            select new ListItem
                            {
                                Text = ospRelatedDT.RELATED_ITEMNUMBER,
                                Value = ospRelatedDT.RELATED_ITEM_ID.ToString()
                            };
                relatedItemList.AddRange(query.ToList());
            }


            return relatedItemList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }



        public List<OspRelatedDT> search(string ORGANIZATION_ID, string INVENTORY_ITEM_ID, string RELATED_ITEM_ID)
        {
            //ResultModel result = new ResultModel(true, "搜尋成功");
            try
            {
                long orgId = 0;
                long inventoryItemId = 0;
                long relatedItemId = 0;

                try
                {
                    if (!string.IsNullOrEmpty(ORGANIZATION_ID) && ORGANIZATION_ID != "*")
                    {
                        orgId = Convert.ToInt64(ORGANIZATION_ID);
                    }
                }
                catch
                {
                    ORGANIZATION_ID = "*";
                }

                try
                {
                    if (!string.IsNullOrEmpty(INVENTORY_ITEM_ID) && INVENTORY_ITEM_ID != "*")
                    {
                        inventoryItemId = Convert.ToInt64(INVENTORY_ITEM_ID);
                    }
                }
                catch
                {
                    INVENTORY_ITEM_ID = "*";
                }

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



                var query = testSource.Where(
                  x =>
                  (ORGANIZATION_ID == "*" || x.ORGANIZATION_ID == orgId) &&
                  (INVENTORY_ITEM_ID == "*" || x.INVENTORY_ITEM_ID == inventoryItemId) &&
                  (RELATED_ITEM_ID == "*" || x.RELATED_ITEM_ID == relatedItemId)
                  ).ToList();

                //dtData = query;
                return query;
            }
            catch (Exception e)
            {
                //result.Msg = e.Message;
                //result.Success = false;
                return new List<OspRelatedDT>();
            }
            //return result;
        }

        public OspRelatedViewModel getViewModel()
        {
            OspRelatedViewModel viewModel = new OspRelatedViewModel();
            viewModel.SelectedOrganization = "*";
            viewModel.SelectedInventoryItemNumber = "*";
            viewModel.SelectedRelatedItemNumber = "*";

            viewModel.OrganizationNameItems = getOrganizationList();
            viewModel.InventoryItemNumberItems = getInventoryItemList("*");
            viewModel.RelatedItemNumberItems = getRelatedItemList("*", "*");

            return viewModel;
        }
    }

    internal class OspRelatedDTOrder
    {
        public static IOrderedEnumerable<OspRelatedDT> Order(List<Order> orders, IEnumerable<OspRelatedDT> models)
        {
            IOrderedEnumerable<OspRelatedDT> orderedModel = null;
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


        private static IOrderedEnumerable<OspRelatedDT> OrderBy(int column, string dir, IEnumerable<OspRelatedDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_NUMBER) : models.OrderBy(x => x.ITEM_NUMBER);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ITEM_DESCRIPTION) : models.OrderBy(x => x.ITEM_DESCRIPTION);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RELATED_ITEMNUMBER) : models.OrderBy(x => x.RELATED_ITEMNUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RELATED_ITEM_DESCRIPTION) : models.OrderBy(x => x.RELATED_ITEM_DESCRIPTION);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATED_BY_NAME) : models.OrderBy(x => x.CREATED_BY_NAME);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATION_DATE) : models.OrderBy(x => x.CREATION_DATE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATED_BY_NAME) : models.OrderBy(x => x.LAST_UPDATED_BY_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATE_DATE) : models.OrderBy(x => x.LAST_UPDATE_DATE);
            }
        }

        private static IOrderedEnumerable<OspRelatedDT> ThenBy(int column, string dir, IOrderedEnumerable<OspRelatedDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_NUMBER) : models.ThenBy(x => x.ITEM_NUMBER);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ITEM_DESCRIPTION) : models.ThenBy(x => x.ITEM_DESCRIPTION);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RELATED_ITEMNUMBER) : models.ThenBy(x => x.RELATED_ITEMNUMBER);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RELATED_ITEM_DESCRIPTION) : models.ThenBy(x => x.RELATED_ITEM_DESCRIPTION);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATED_BY_NAME) : models.ThenBy(x => x.CREATED_BY_NAME);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATION_DATE) : models.ThenBy(x => x.CREATION_DATE);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATED_BY_NAME) : models.ThenBy(x => x.LAST_UPDATED_BY_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATE_DATE) : models.ThenBy(x => x.LAST_UPDATE_DATE);
            }
        }
    }
}