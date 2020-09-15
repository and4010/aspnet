using CHPOUTSRCMES.Web.Models.StockQuery;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CHPOUTSRCMES.Web.ViewModels.StockQuery
{
    public class QueryDetailViewModel
    {
        [Display(Name = "倉庫代碼")]
        public string SubinventoryCode { set; get; }

        [Display(Name = "倉庫名稱")]
        public string SubinventoryName { set; get; }

        [Display(Name = "儲位ID")]
        public long LocatorId { set; get; }
        
        [Display(Name = "儲位")]
        public string LocatorSegments { set; get; }
        
        [Display(Name = "料號ID")]
        public long InventoryItemId { set; get; }
        
        [Display(Name = "料號")]
        public string ItemNumber { set; get; }

        [Display(Name = "捲筒/平版")]
        public string ItemCategory { set; get; }

        public StockDetailQueryModel TableFields { set; get; }


        public static QueryDetailViewModel getModel(string subinventoryCode, long locatorId, long itemId)
        {
            var model = new QueryDetailViewModel();

            using var mesContext = new CHPOUTSRCMES.Web.DataModel.MesContext();

            using var masterUow = new CHPOUTSRCMES.Web.DataModel.UnitOfWorks.MasterUOW(mesContext);
            try
            {

                var subinventory = masterUow.subinventoryRepository.Get(x => x.SubinventoryCode == subinventoryCode).FirstOrDefault();

                var locator = masterUow.locatorTRepository.Get(x => x.LocatorId == locatorId).FirstOrDefault();

                var item = masterUow.itemsTRepository.Get(x => x.InventoryItemId == itemId).FirstOrDefault();

                
                model.SubinventoryCode = subinventory.SubinventoryCode;
                model.SubinventoryName = subinventory.SubinventoryName;
                model.LocatorId = locator.LocatorId;
                model.LocatorSegments = locator.LocatorSegments;
                model.InventoryItemId = item.InventoryItemId;
                model.ItemNumber = item.ItemNumber;
                model.ItemCategory = item.CatalogElemVal070;
                model.TableFields = new StockDetailQueryModel();

                
            }
            catch (Exception ex)
            {

            }

            return model;
        }

            

    }
}