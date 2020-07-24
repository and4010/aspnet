using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.Information
{
    public class OrgSubinventoryDT
    {
        public OrgSubinventoryDT()
        {
        }
        //public OrgSubinventoryDT(
        //    long ORGANIZATION_ID, string ORGANIZATION_CODE, string ORGANIZATION_NAME,
        //    string SUBINVENTORY_CODE, string SUBINVENTORY_NAME, string OSP_FLAG, string BARCODE_PREFIX_CODE, long LOCATOR_ID,
        //    long LOCATOR_TYPE, string LOCATOR_SEGMENTS, string LOCATOR_DESC,
        //    string SEGMENT1, string SEGMENT2, string SEGMENT3, string SEGMENT4,
        //    long CREATED_BY, string CREATED_BY_NAME, DateTime CREATION_DATE, long LAST_UPDATED_BY, string LAST_UPDATED_BY_NAME, DateTime LAST_UPDATE_DATE)
        public OrgSubinventoryDT(
           long ORGANIZATION_ID, string ORGANIZATION_CODE, string ORGANIZATION_NAME,
           string SUBINVENTORY_CODE, string SUBINVENTORY_NAME, string OSP_FLAG, string BARCODE_PREFIX_CODE, long LOCATOR_ID,
           long LOCATOR_TYPE, string LOCATOR_SEGMENTS, string LOCATOR_DESC,
           string SEGMENT1, string SEGMENT2, string SEGMENT3, string SEGMENT4)
        {
            this.ORGANIZATION_ID = ORGANIZATION_ID;
            this.ORGANIZATION_CODE = ORGANIZATION_CODE;
            this.ORGANIZATION_NAME = ORGANIZATION_NAME;
            this.SUBINVENTORY_CODE = SUBINVENTORY_CODE;
            this.SUBINVENTORY_NAME = SUBINVENTORY_NAME;
            this.OSP_FLAG = OSP_FLAG;
            this.BARCODE_PREFIX_CODE = BARCODE_PREFIX_CODE;
            this.LOCATOR_ID = LOCATOR_ID;
            this.LOCATOR_TYPE = LOCATOR_TYPE;
            this.LOCATOR_SEGMENTS = LOCATOR_SEGMENTS;
            this.LOCATOR_DESC = LOCATOR_DESC;
            this.SEGMENT1 = SEGMENT1;
            this.SEGMENT2 = SEGMENT2;
            this.SEGMENT3 = SEGMENT3;
            this.SEGMENT4 = SEGMENT4;
            //this.CREATED_BY = CREATED_BY;
            //this.CREATED_BY_NAME = CREATED_BY_NAME;
            //this.CREATION_DATE = CREATION_DATE;
            //this.LAST_UPDATED_BY = LAST_UPDATED_BY;
            //this.LAST_UPDATED_BY_NAME = LAST_UPDATED_BY_NAME;
            //this.LAST_UPDATE_DATE = LAST_UPDATE_DATE;
        }

        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_CODE { set; get; }
        public string ORGANIZATION_NAME { set; get; }

        public string SUBINVENTORY_CODE { set; get; }
        public string SUBINVENTORY_NAME { set; get; }
        public string OSP_FLAG { set; get; }
        public string BARCODE_PREFIX_CODE { set; get; }
        public long LOCATOR_ID { set; get; }
        public long LOCATOR_TYPE { set; get; }
        public string LOCATOR_SEGMENTS { set; get; }
        public string LOCATOR_DESC { set; get; }
        public string SEGMENT1 { set; get; }
        public string SEGMENT2 { set; get; }
        public string SEGMENT3 { set; get; }
        public string SEGMENT4 { set; get; }

        //public long CREATED_BY { get; set; }
        //public string CREATED_BY_NAME { get; set; }
        //public DateTime? CREATION_DATE { get; set; }

        //public long LAST_UPDATED_BY { get; set; }
        //public string LAST_UPDATED_BY_NAME { get; set; }
        //public DateTime? LAST_UPDATE_DATE { get; set; }

    }

    public class OrgSubinventoryData
    {
        //private static List<OrgSubinventoryDT> dtData = new List<OrgSubinventoryDT>();
        //private List<OrgSubinventoryDT> testSource = new List<OrgSubinventoryDT>();
        private static List<OrgSubinventoryDT> testSource = new List<OrgSubinventoryDT>();

        public OrgSubinventoryData()
        {
            //if (testSource.Count == 0)
            //{
            //    testSource.Add(new OrgSubinventoryDT(265, "FTY", "INV_ORG_華紙總公司", "TB2", "總倉-南崁", "", "A", 0, 1, "", "", "", "", "", "", 1, "一力星1號", Convert.ToDateTime("2020-05-21"), 1, "一力星1號", Convert.ToDateTime("2020-05-21")));
            //    testSource.Add(new OrgSubinventoryDT(265, "FTY", "INV_ORG_華紙總公司", "SFG", "中間倉", "是", "B", 22016, 2, "FTY.SFG.TB2.NA", "總公司.中間倉.TB2", "FTY", "SFG", "TB2", "NA", 1, "一力星1號", Convert.ToDateTime("2020-04-21"), 1, "一力星1號", Convert.ToDateTime("2020-04-21")));
            //    testSource.Add(new OrgSubinventoryDT(265, "FTY", "INV_ORG_華紙總公司", "SFG", "中間倉", "", "B", 22017, 2, "FTY.SFG.TA1.NA", "總公司.中間倉.TA1", "FTY", "SFG", "TA1", "NA", 2, "一力星2號", Convert.ToDateTime("2020-04-20"), 2, "一力星2號", Convert.ToDateTime("2020-04-22")));
            //    testSource.Add(new OrgSubinventoryDT(265, "FTY", "INV_ORG_華紙總公司", "SFG", "中間倉", "", "B", 22018, 2, "FTY.SFG.TCA.NA", "總公司.中間倉.TCA", "FTY", "SFG", "TCA", "NA", 1, "一力星1號", Convert.ToDateTime("2020-04-20"), 3, "華紙1號", Convert.ToDateTime("2020-04-23")));
            //    testSource.Add(new OrgSubinventoryDT(285, "FTS", "INV_ORG_華紙新屋廠", "SA", "新屋內銷", "", "C", 0, 1, "", "", "", "", "", "", 2, "一力星2號", Convert.ToDateTime("2020-04-19"), 2, "一力星2號", Convert.ToDateTime("2020-04-19")));
            //    testSource.Add(new OrgSubinventoryDT(305, "FTA", "INV_ORG_華紙久堂廠", "A1FG", "成品倉(21#平板)", "", "D", 0, 1, "", "", "", "", "", "", 1, " 一力星1號", Convert.ToDateTime("2020-04-22"), 1, "一力星1號", Convert.ToDateTime("2020-04-22")));
            //}

        }

        public List<OrgSubinventoryDT> GetOrgSubinventoryDT()
        {
            return testSource;
            //return dtData;
        }

        public IEnumerable<SelectListItem> GetReasonList()
        {
            List<ListItem> ReasonList = new List<ListItem>();
            ReasonList.Add(new ListItem("請選擇", "請選擇"));
            ReasonList.Add(new ListItem("A-破損", "A-破損"));
            ReasonList.Add(new ListItem("B-汙垢", "B-汙垢"));
            return ReasonList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        private List<SelectListItem> getOrganizationList()
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
            return organizationList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value }).ToList();
        }

        public IEnumerable<SelectListItem> GetSubinventoryList(MasterUOW uow, string ORGANIZATION_ID, MasterUOW.DropDownListType type)
        {
            var subinventoryList = uow.GetSubinventoryDropDownList(ORGANIZATION_ID, type);
            return subinventoryList;
        }

        public string GetBarodePrefixCode(string SUBINVENTORY_CODE)
        {
            var query = from orgSubinventoryDT in testSource
                        where SUBINVENTORY_CODE == orgSubinventoryDT.SUBINVENTORY_CODE
                        select orgSubinventoryDT;
            var list = query.ToList();
            if (list.Count > 0)
            {
                return list[0].BARCODE_PREFIX_CODE;
            }
            else
            {
                return "";
            }
        }

        private List<ListItem> getSubinventoryList(string ORGANIZATION_ID, bool needAll)
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


            List<ListItem> subinventoryList = new List<ListItem>();
            if (needAll)
            {
                subinventoryList.Add(new ListItem("全部", "*"));
            }
            else
            {
                subinventoryList.Add(new ListItem("請選擇", "請選擇"));
            }


            if (ORGANIZATION_ID == "*")
            {
                var query = from orgSubinventoryDT in testSource
                            group orgSubinventoryDT by new { orgSubinventoryDT.SUBINVENTORY_NAME, orgSubinventoryDT.SUBINVENTORY_CODE } into g
                            select new ListItem
                            {
                                //Text =  g.Key.SUBINVENTORY_CODE.ToString() + " " + g.Key.SUBINVENTORY_NAME.ToString(),
                                Text = g.Key.SUBINVENTORY_CODE.ToString(),
                                Value = g.Key.SUBINVENTORY_CODE.ToString()
                            };
                subinventoryList.AddRange(query.ToList());
            }
            else
            {
                var query = from orgSubinventoryDT in testSource
                            where organizationId == orgSubinventoryDT.ORGANIZATION_ID
                            group orgSubinventoryDT by new { orgSubinventoryDT.SUBINVENTORY_NAME, orgSubinventoryDT.SUBINVENTORY_CODE } into g
                            select new ListItem
                            {
                                //Text = g.Key.SUBINVENTORY_CODE.ToString() + " " + g.Key.SUBINVENTORY_NAME.ToString(),
                                Text = g.Key.SUBINVENTORY_CODE.ToString(),
                                Value = g.Key.SUBINVENTORY_CODE.ToString()
                            };
                subinventoryList.AddRange(query.ToList());
            }

            

            //subinventoryList.Add(new ListItem("中間倉", "SFG"));
            //subinventoryList.Add(new ListItem("外購久堂倉", "TA1"));
            //subinventoryList.Add(new ListItem("總倉", "TB1"));
            //subinventoryList.Add(new ListItem("總倉-南崁", "TB2"));
            //subinventoryList.Add(new ListItem("新屋外銷", "SA"));
            //subinventoryList.Add(new ListItem("成品倉(21#平板)", "A1FG"));

            return subinventoryList;
        }

        public List<SelectListItem> GetLocatorList(MasterUOW uow, string ORGANIZATION_ID, string SUBINVENTORY_CODE, MasterUOW.DropDownListType type)
        {
            return uow.GetLocatorDropDownList(ORGANIZATION_ID, SUBINVENTORY_CODE, type);
        }


        public IEnumerable<SelectListItem> GetLocatorList(string ORGANIZATION_ID, string SUBINVENTORY_CODE, bool needAll)
        {
            return getLocatorList(ORGANIZATION_ID, SUBINVENTORY_CODE, needAll);
        }

        private IEnumerable<SelectListItem> getLocatorList(string ORGANIZATION_ID, string SUBINVENTORY_CODE, bool needAll)
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

            //if (string.IsNullOrEmpty(SUBINVENTORY_CODE))
            //{
            //    SUBINVENTORY_CODE = "*";
            //}

            List<ListItem> locatorList = new List<ListItem>();
            if (needAll)
            {
                locatorList.Add(new ListItem("全部", "*"));
            }
            else
            {
                locatorList.Add(new ListItem("請選擇", "請選擇"));
            }

            //locatorList.Add(new ListItem("總公司.中間倉.TB2", "22016"));
            //locatorList.Add(new ListItem("總公司.中間倉.TA1", "22017"));
            //locatorList.Add(new ListItem("總公司.中間倉.TCA", "22018"));

            if (ORGANIZATION_ID == "*" && SUBINVENTORY_CODE == "*")
            {
                var query = from orgSubinventoryDT in testSource
                            where !string.IsNullOrEmpty(orgSubinventoryDT.LOCATOR_SEGMENTS) && orgSubinventoryDT.LOCATOR_TYPE == 2
                            group orgSubinventoryDT by new { orgSubinventoryDT.SEGMENT3, orgSubinventoryDT.LOCATOR_ID } into g
                            select new ListItem
                            {
                                Text = g.Key.SEGMENT3,
                                Value = g.Key.LOCATOR_ID.ToString()
                            };
                locatorList.AddRange(query.ToList());
            }
            else if (ORGANIZATION_ID != "*" && SUBINVENTORY_CODE == "*")
            {
                var query = from orgSubinventoryDT in testSource
                            where organizationId == orgSubinventoryDT.ORGANIZATION_ID && !string.IsNullOrEmpty(orgSubinventoryDT.LOCATOR_SEGMENTS) && orgSubinventoryDT.LOCATOR_TYPE == 2
                            group orgSubinventoryDT by new { orgSubinventoryDT.SEGMENT3, orgSubinventoryDT.LOCATOR_ID } into g
                            select new ListItem
                            {
                                Text = g.Key.SEGMENT3,
                                Value = g.Key.LOCATOR_ID.ToString()
                            };
                locatorList.AddRange(query.ToList());
            }
            else if (ORGANIZATION_ID == "*" && SUBINVENTORY_CODE != "*")
            {
                var query = from orgSubinventoryDT in testSource
                            where SUBINVENTORY_CODE == orgSubinventoryDT.SUBINVENTORY_CODE && !string.IsNullOrEmpty(orgSubinventoryDT.LOCATOR_SEGMENTS) && orgSubinventoryDT.LOCATOR_TYPE == 2
                            group orgSubinventoryDT by new { orgSubinventoryDT.SEGMENT3, orgSubinventoryDT.LOCATOR_ID } into g
                            select new ListItem
                            {
                                Text = g.Key.SEGMENT3,
                                Value = g.Key.LOCATOR_ID.ToString()
                            };
                locatorList.AddRange(query.ToList());
            }
            else
            {
                var query = from orgSubinventoryDT in testSource
                            where organizationId == orgSubinventoryDT.ORGANIZATION_ID && SUBINVENTORY_CODE == orgSubinventoryDT.SUBINVENTORY_CODE && !string.IsNullOrEmpty(orgSubinventoryDT.LOCATOR_SEGMENTS) && orgSubinventoryDT.LOCATOR_TYPE == 2
                            group orgSubinventoryDT by new { orgSubinventoryDT.SEGMENT3, orgSubinventoryDT.LOCATOR_ID } into g
                            select new ListItem
                            {
                                Text = g.Key.SEGMENT3,
                                Value = g.Key.LOCATOR_ID.ToString()
                            };
                locatorList.AddRange(query.ToList());
            }

            return locatorList.Select(i => new SelectListItem() { Text = i.Text, Value = i.Value });
        }

        public List<OrgSubinventoryDT> search(string SUBINVENTORY_CODE, long LOCATOR_ID)
        {
            var query = from orgSubinventoryDT in testSource
                        where SUBINVENTORY_CODE == orgSubinventoryDT.SUBINVENTORY_CODE && LOCATOR_ID == orgSubinventoryDT.LOCATOR_ID
                        select orgSubinventoryDT;
            return query.ToList();
        }

        public List<OrgSubinventoryDT> search(MasterUOW uow, string ORGANIZATION_ID, string SUBINVENTORY_CODE, string LOCATOR_ID)
        {
            return uow.OrgSubinventorySearch(ORGANIZATION_ID, SUBINVENTORY_CODE, LOCATOR_ID);

            //ResultModel result = new ResultModel(true, "搜尋成功");
            //try
            //{
            //    long orgId = 0;
            //    long locId = 0;

            //    try
            //    {
            //        if (!string.IsNullOrEmpty(ORGANIZATION_ID) && ORGANIZATION_ID != "*")
            //        {
            //            orgId = Convert.ToInt64(ORGANIZATION_ID);
            //        }
            //    }
            //    catch
            //    {
            //        ORGANIZATION_ID = "*";
            //    }

            //    //if (string.IsNullOrEmpty(SUBINVENTORY_CODE))
            //    //{
            //    //    SUBINVENTORY_CODE = "*";
            //    //}

            //    try
            //    {
            //        if (!string.IsNullOrEmpty(LOCATOR_ID) && LOCATOR_ID != "*")
            //        {
            //            locId = Convert.ToInt64(LOCATOR_ID);
            //        }
            //    }
            //    catch
            //    {
            //        LOCATOR_ID = "*";
            //    }


            //    var query = testSource.Where(
            //      x =>
            //      (ORGANIZATION_ID == "*" || x.ORGANIZATION_ID == orgId) &&
            //      (SUBINVENTORY_CODE == "*" || x.SUBINVENTORY_CODE != null && x.SUBINVENTORY_CODE.ToLower() == SUBINVENTORY_CODE.ToLower()) &&
            //      (LOCATOR_ID == "*" || x.LOCATOR_ID == locId)
            //      ).ToList();

            //    //dtData = query;
            //    return query;
            //}
            //catch (Exception e)
            //{
            //    //result.Msg = e.Message;
            //    //result.Success = false;
            //    return new List<OrgSubinventoryDT>();
            //}
            ////return result;
        }

        public OrgSubinventoryViewModel getViewModel(MasterUOW uow)
        {
            OrgSubinventoryViewModel viewModel = new OrgSubinventoryViewModel();
            viewModel.SelectedLocator = "*";
            viewModel.SelectedOrganization = "*";
            viewModel.SelectedSubinventory = "*";
            viewModel.OrganizationNameItems = uow.GetOrganizationDropDownList(MasterUOW.DropDownListType.All);
            //viewModel.SubinventoryNameItems = GetSubinventoryList("*", true);
            viewModel.SubinventoryNameItems = uow.GetSubinventoryDropDownList("*", MasterUOW.DropDownListType.All);
            //viewModel.LocatorNameItems = getLocatorList("*", "*", true);
            viewModel.LocatorNameItems = uow.GetLocatorDropDownList("*", "*", MasterUOW.DropDownListType.All);
            return viewModel;
        }

        public string getORGANIZATION_CODE(string SUBINVENTORY_CODE)
        {
            var query = from orgSubinventoryDT in testSource
                        where SUBINVENTORY_CODE == orgSubinventoryDT.SUBINVENTORY_CODE
                        select orgSubinventoryDT;

            List<OrgSubinventoryDT> list = query.ToList();
            if (list.Count > 0)
            {
                return list[0].ORGANIZATION_CODE;
            }
            else
            {
                return "";
            }
        }


    }

    internal class OrgSubinventoryDTOrder
    {
        public static IOrderedEnumerable<OrgSubinventoryDT> Order(List<Order> orders, IEnumerable<OrgSubinventoryDT> models)
        {
            IOrderedEnumerable<OrgSubinventoryDT> orderedModel = null;
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


        private static IOrderedEnumerable<OrgSubinventoryDT> OrderBy(int column, string dir, IEnumerable<OrgSubinventoryDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ORGANIZATION_CODE) : models.OrderBy(x => x.ORGANIZATION_CODE);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ORGANIZATION_NAME) : models.OrderBy(x => x.ORGANIZATION_NAME);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUBINVENTORY_CODE) : models.OrderBy(x => x.SUBINVENTORY_CODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SUBINVENTORY_NAME) : models.OrderBy(x => x.SUBINVENTORY_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OSP_FLAG) : models.OrderBy(x => x.OSP_FLAG);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LOCATOR_SEGMENTS) : models.OrderBy(x => x.LOCATOR_SEGMENTS);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LOCATOR_DESC) : models.OrderBy(x => x.LOCATOR_DESC);
                //case 7:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATED_BY_NAME) : models.OrderBy(x => x.CREATED_BY_NAME);
                //case 8:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATION_DATE) : models.OrderBy(x => x.CREATION_DATE);
                //case 9:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATED_BY_NAME) : models.OrderBy(x => x.LAST_UPDATED_BY_NAME);
                //case 10:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATE_DATE) : models.OrderBy(x => x.LAST_UPDATE_DATE);

            }
        }

        private static IOrderedEnumerable<OrgSubinventoryDT> ThenBy(int column, string dir, IOrderedEnumerable<OrgSubinventoryDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ORGANIZATION_CODE) : models.ThenBy(x => x.ORGANIZATION_CODE);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ORGANIZATION_NAME) : models.ThenBy(x => x.ORGANIZATION_NAME);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUBINVENTORY_CODE) : models.ThenBy(x => x.SUBINVENTORY_CODE);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SUBINVENTORY_NAME) : models.ThenBy(x => x.SUBINVENTORY_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OSP_FLAG) : models.ThenBy(x => x.OSP_FLAG);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LOCATOR_SEGMENTS) : models.ThenBy(x => x.LOCATOR_SEGMENTS);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LOCATOR_DESC) : models.ThenBy(x => x.LOCATOR_DESC);
                //case 7:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATED_BY_NAME) : models.ThenBy(x => x.CREATED_BY_NAME);
                //case 8:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATION_DATE) : models.ThenBy(x => x.CREATION_DATE);
                //case 9:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATED_BY_NAME) : models.ThenBy(x => x.LAST_UPDATED_BY_NAME);
                //case 10:
                //    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATE_DATE) : models.ThenBy(x => x.LAST_UPDATE_DATE);
            }
        }
    }
}