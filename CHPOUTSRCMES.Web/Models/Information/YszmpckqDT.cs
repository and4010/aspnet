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
    public class YszmpckqDT
    {
        public long ORGANIZATION_ID { set; get; }
        public string ORGANIZATION_CODE { set; get; }
        public string ORGANIZATION_NAME { set; get; }
        public long OSP_SUBINVENTORY_ID { set; get; }
        public string OSP_SUBINVENTORY { set; get; }
        public string OSP_SUBINVENTORY_NAME { set; get; }
        public string PSTYP { set; get; }
        public string PSTYP_CHT_NAME { set; get; }
        public string PSTYP_ENG_NAME { set; get; }
        public decimal BWETUP { set; get; }
        public decimal BWETDN { set; get; }
        public decimal RWTUP { set; get; }
        public decimal RWTDN { set; get; }
        public long PCKQ { set; get; }
        public long PAPER_QTY { set; get; }
        public long PIECES_QTY { set; get; }
        public long CREATED_BY { get; set; }
        public string CREATED_BY_NAME { get; set; }
        public DateTime? CREATION_DATE { get; set; }

        public long LAST_UPDATED_BY { get; set; }
        public string LAST_UPDATED_BY_NAME { get; set; }
        public DateTime? LAST_UPDATE_DATE { get; set; }

        //public YszmpckqDT(long ORGANIZATION_ID, string ORGANIZATION_CODE, string ORGANIZATION_NAME, long OSP_SUBINVENTORY_ID, string OSP_SUBINVENTORY, string OSP_SUBINVENTORY_NAME,
        //    string PSTYP, string PSTYP_CHT_NAME, string PSTYP_ENG_NAME, decimal BWETUP, decimal BWETDN, decimal RWTUP, decimal RWTDN, long PCKQ, long PAPER_QTY, long PIECES_QTY,
        //    long CREATED_BY, string CREATED_BY_NAME, DateTime CREATION_DATE, long LAST_UPDATED_BY, string LAST_UPDATED_BY_NAME, DateTime LAST_UPDATE_DATE)
        //{
        //    this.ORGANIZATION_ID = ORGANIZATION_ID;
        //    this.ORGANIZATION_CODE = ORGANIZATION_CODE;
        //    this.ORGANIZATION_NAME = ORGANIZATION_NAME;
        //    this.OSP_SUBINVENTORY_ID = OSP_SUBINVENTORY_ID;
        //    this.OSP_SUBINVENTORY = OSP_SUBINVENTORY;
        //    this.OSP_SUBINVENTORY_NAME = OSP_SUBINVENTORY_NAME;
        //    this.PSTYP = PSTYP;
        //    this.PSTYP_CHT_NAME = PSTYP_CHT_NAME;
        //    this.PSTYP_ENG_NAME = PSTYP_ENG_NAME;
        //    this.BWETUP = BWETUP;
        //    this.BWETDN = BWETDN;
        //    this.RWTUP = RWTUP;
        //    this.RWTDN = RWTDN;
        //    this.PCKQ = PCKQ;
        //    this.PAPER_QTY = PAPER_QTY;
        //    this.PIECES_QTY = PIECES_QTY;
        //    this.CREATED_BY = CREATED_BY;
        //    this.CREATED_BY_NAME = CREATED_BY_NAME;
        //    this.CREATION_DATE = CREATION_DATE;
        //    this.LAST_UPDATED_BY = LAST_UPDATED_BY;
        //    this.LAST_UPDATED_BY_NAME = LAST_UPDATED_BY_NAME;
        //    this.LAST_UPDATE_DATE = LAST_UPDATE_DATE;
        //}
    }

    public class YszmpckqData
    {
        //private static List<YszmpckqDT> dtData = new List<YszmpckqDT>();
        private List<YszmpckqDT> testSource = new List<YszmpckqDT>();

        public List<SelectListItem> GetOrganizationList()
        {
            using (var mes = new MesContext())
            {
                return new MasterUOW(mes).GetOrganizationDropDownList(MasterUOW.DropDownListType.All);
            }
        }

        public List<SelectListItem> GetOspSubinventoryList(string ORGANIZATION_ID) //要加條件OSP_FLAG為Y 只顯示是加工廠的倉庫
        {
            using (var mes = new MesContext())
            {
                return new YszmpckqUOW(mes).GetOspSubinventoryList(ORGANIZATION_ID);
            }
        }

        public List<SelectListItem> GetPstypList(string ORGANIZATION_ID, string OSP_SUBINVENTORY_ID)
        {
            using (var mes = new MesContext())
            {
                return new YszmpckqUOW(mes).GetPstypList(ORGANIZATION_ID, OSP_SUBINVENTORY_ID);
            }
        }

        public List<YszmpckqDT> search(string ORGANIZATION_ID, string OSP_SUBINVENTORY_ID, string PSTYP)
        {
            using (var mes = new MesContext())
            {
                return new YszmpckqUOW(mes).search(ORGANIZATION_ID, OSP_SUBINVENTORY_ID, PSTYP);
            }
        }

        public YszmpckqViewModel getViewModel()
        {
            YszmpckqViewModel viewModel = new YszmpckqViewModel();
            //預設選單為全選
            viewModel.SelectedOspSubinventory = "*";
            viewModel.SelectedOrganization = "*";
            viewModel.SelectedPstyp = "*";

            viewModel.OrganizationNameItems = GetOrganizationList();
            viewModel.OspSubinventoryNameItems = GetOspSubinventoryList("*");
            viewModel.PstypNameItems = GetPstypList("*", "*");
            return viewModel;
        }
    }

    internal class YszmpckqDTOrder
    {
        public static IOrderedEnumerable<YszmpckqDT> Order(List<Order> orders, IEnumerable<YszmpckqDT> models)
        {
            IOrderedEnumerable<YszmpckqDT> orderedModel = null;
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


        private static IOrderedEnumerable<YszmpckqDT> OrderBy(int column, string dir, IEnumerable<YszmpckqDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ORGANIZATION_CODE) : models.OrderBy(x => x.ORGANIZATION_CODE);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ORGANIZATION_NAME) : models.OrderBy(x => x.ORGANIZATION_NAME);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OSP_SUBINVENTORY) : models.OrderBy(x => x.OSP_SUBINVENTORY);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.OSP_SUBINVENTORY_NAME) : models.OrderBy(x => x.OSP_SUBINVENTORY_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PSTYP) : models.OrderBy(x => x.PSTYP);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PSTYP_CHT_NAME) : models.OrderBy(x => x.PSTYP_CHT_NAME);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PSTYP_ENG_NAME) : models.OrderBy(x => x.PSTYP_ENG_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BWETUP) : models.OrderBy(x => x.BWETUP);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BWETDN) : models.OrderBy(x => x.BWETDN);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RWTUP) : models.OrderBy(x => x.RWTUP);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RWTDN) : models.OrderBy(x => x.RWTDN);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PCKQ) : models.OrderBy(x => x.PCKQ);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PAPER_QTY) : models.OrderBy(x => x.PAPER_QTY);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PIECES_QTY) : models.OrderBy(x => x.PIECES_QTY);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATED_BY_NAME) : models.OrderBy(x => x.CREATED_BY_NAME);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.CREATION_DATE) : models.OrderBy(x => x.CREATION_DATE);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATED_BY_NAME) : models.OrderBy(x => x.LAST_UPDATED_BY_NAME);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LAST_UPDATE_DATE) : models.OrderBy(x => x.LAST_UPDATE_DATE);
            }
        }

        private static IOrderedEnumerable<YszmpckqDT> ThenBy(int column, string dir, IOrderedEnumerable<YszmpckqDT> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ORGANIZATION_CODE) : models.ThenBy(x => x.ORGANIZATION_CODE);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ORGANIZATION_NAME) : models.ThenBy(x => x.ORGANIZATION_NAME);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OSP_SUBINVENTORY) : models.ThenBy(x => x.OSP_SUBINVENTORY);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.OSP_SUBINVENTORY_NAME) : models.ThenBy(x => x.OSP_SUBINVENTORY_NAME);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PSTYP) : models.ThenBy(x => x.PSTYP);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PSTYP_CHT_NAME) : models.ThenBy(x => x.PSTYP_CHT_NAME);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PSTYP_ENG_NAME) : models.ThenBy(x => x.PSTYP_ENG_NAME);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BWETUP) : models.ThenBy(x => x.BWETUP);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BWETDN) : models.ThenBy(x => x.BWETDN);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RWTUP) : models.ThenBy(x => x.RWTUP);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RWTDN) : models.ThenBy(x => x.RWTDN);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PCKQ) : models.ThenBy(x => x.PCKQ);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PAPER_QTY) : models.ThenBy(x => x.PAPER_QTY);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.PIECES_QTY) : models.ThenBy(x => x.PIECES_QTY);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATED_BY_NAME) : models.ThenBy(x => x.CREATED_BY_NAME);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.CREATION_DATE) : models.ThenBy(x => x.CREATION_DATE);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATED_BY_NAME) : models.ThenBy(x => x.LAST_UPDATED_BY_NAME);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.LAST_UPDATE_DATE) : models.ThenBy(x => x.LAST_UPDATE_DATE);
            }
        }
    }
}