using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.StockQuery
{
    public class StockDetailQueryModel
    {
        [Display(Name = "庫存ID")]
        public long StockId { set; get; }

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

        [Display(Name = "主單位")]
        public string PrimaryUomCode { set; get; }

        [Display(Name = "主可用量")]
        public decimal PrimaryAvailableQty { set; get; }

        [Display(Name = "次單位")]
        public string SecondaryUomCode { set; get; }

        [Display(Name = "次可用量")]
        public decimal? SecondaryAvailableQty { set; get; }

        [Display(Name = "基重")]
        public string BasicWeight { set; get; }

        [Display(Name = "令重")]
        public string ReamWeight { set; get; }

        [Display(Name = "規格")]
        public string Specification { set; get; }

        [Display(Name = "條碼")]
        public string Barcode { set; get; }

        [Display(Name = "捲號")]
        public string LotNumber { set; get; }

        [Display(Name = "紙別")]
        public string PaperType { set; get; }

        [Display(Name = "包裝方式")]
        public string PackingType { set; get; }

        public static List<StockDetailQueryModel> getModels(DataTableAjaxPostViewModel data,
            string subinventory, long locatorId, long itemId, string userId)
        {

            using var mesContext = new MesContext();

            using var masterUow = new MasterUOW(mesContext);
            try
            {
                var list = masterUow.stockTRepository.GetAll().AsNoTracking()
                    .Join(masterUow.userSubinventoryTRepository.GetAll().AsNoTracking(), x => x.SubinventoryCode, y => y.SubinventoryCode, (x, y) => new { user = y, stock = x })
                    .Where(x => x.user.UserId == userId && x.stock.StatusCode == MasterUOW.StockStatusCode.InStock)
                    .Select(x => x.stock);

                if (!string.IsNullOrEmpty(subinventory) && subinventory.CompareTo("*") != 0)
                {
                    list = list.Where(x => x.SubinventoryCode == subinventory);
                }

                if (locatorId > 0)
                {
                    list = list.Where(x => x.LocatorId == locatorId);
                }

                if (itemId > 0)
                {
                    list = list.Where(x => x.InventoryItemId == itemId);
                }

                var models = list
                    .Select(x => new StockDetailQueryModel()
                    {
                        StockId = x.StockId,
                        SubinventoryCode = x.SubinventoryCode,
                        LocatorId = x.LocatorId ?? 0,
                        LocatorSegments = x.LocatorSegments,
                        InventoryItemId = x.InventoryItemId,
                        ItemNumber = x.ItemNumber,
                        PrimaryUomCode = x.PrimaryUomCode,
                        PrimaryAvailableQty = x.PrimaryAvailableQty,
                        SecondaryUomCode = x.SecondaryUomCode,
                        SecondaryAvailableQty = x.SecondaryAvailableQty,
                        Barcode = x.Barcode,
                        BasicWeight = x.BasicWeight,
                        ReamWeight = x.ReamWeight, 
                        LotNumber = x.LotNumber, 
                        PackingType = x.PackingType, 
                        Specification = x.Specification
                    });
                //var count = models.Count();
                return models.OrderBy(x=> new { x.SubinventoryCode, x.LocatorSegments, x.InventoryItemId}).Skip(data.Start).Take(data.Length).ToList();
            }
            catch (Exception ex)
            {

            }

            return new List<StockDetailQueryModel>();
        }
    }
}