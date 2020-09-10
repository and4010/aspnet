using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;

namespace CHPOUTSRCMES.Web.ViewModels
{
    public class StockQueryModel
    {
        [Display(Name = "倉庫")]
        public string SubinventoryCode { set; get; }

        [Display(Name = "儲位ID")]
        public long LocatorId { set; get; }

        [Display(Name = "儲位")]
        public string LocatorSegments { set; get; }

        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }

        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "主單位")]
        public string PrimaryUomCode { set; get; }


        public decimal PrimaryAvailableQty { set; get; }


        public decimal PrimarySumQty { set; get; }

        [Display(Name = "次單位")]
        public string SecondaryUomCode { set; get; }


        public decimal? SecondaryAvailableQty { set; get; }


        public decimal? SecondarySumQty { set; get; }

        public static List<StockQueryModel> getModels(DataTableAjaxPostViewModel data,
            string subinventory, long locatorId, string itemCategory, string itemNo)
        {

            using var mesContext = new CHPOUTSRCMES.Web.DataModel.MesContext();

            using var masterUow = new CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW(mesContext);
            try
            {


                var list = masterUow.stockTRepository.GetAll().AsNoTracking();

                if (!string.IsNullOrEmpty(subinventory) && subinventory.CompareTo("全部") != 0)
                {
                    list = list.Where(x => x.SubinventoryCode == subinventory);
                }

                if (locatorId > 0)
                {
                    list = list.Where(x => x.LocatorId == locatorId);
                }

                if (!string.IsNullOrEmpty(itemCategory) && itemCategory.CompareTo("全部") != 0)
                {
                    list = list.Where(x => x.ItemCategory == itemCategory);
                }

                if (!string.IsNullOrEmpty(itemNo) && itemNo.CompareTo("全部") != 0)
                {
                    list = list.Where(x => x.ItemNumber.Contains(itemNo));
                }

                var models = list.GroupBy(x => new { x.SubinventoryCode, x.LocatorId, x.InventoryItemId })
                    .Select(x => new StockQueryModel()
                    {
                        SubinventoryCode = x.FirstOrDefault().SubinventoryCode,
                        LocatorId = x.FirstOrDefault().LocatorId ?? 0,
                        LocatorSegments = x.FirstOrDefault().LocatorSegments,
                        InventoryItemId = x.FirstOrDefault().InventoryItemId,
                        ItemNumber = x.FirstOrDefault().ItemNumber,
                        PrimaryUomCode = x.FirstOrDefault().PrimaryUomCode,
                        PrimaryAvailableQty = x.Sum(y => y.PrimaryAvailableQty),
                        PrimarySumQty = x.Sum(y => y.PrimaryAvailableQty) + (x.Sum(x => x.PrimaryLockedQty) ?? 0),
                        SecondaryUomCode = x.FirstOrDefault().SecondaryUomCode,
                        SecondaryAvailableQty = x.Sum(y => y.SecondaryAvailableQty) + x.Sum(y => y.SecondaryLockedQty)
                    });
                //var count = models.Count();
                return models.Skip(data.Start).Take(data.Length).ToList();
            }
            catch (Exception ex)
            {

            }

            return new List<StockQueryModel>();
        }
    }
}