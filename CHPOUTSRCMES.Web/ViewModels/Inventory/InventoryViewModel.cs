using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.Models.Inventory;

namespace CHPOUTSRCMES.Web.ViewModels.Inventory
{
    public class InventoryViewModel
    {

        public static List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> flatModels = new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel>();
        public static List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> paperRollModels = new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel>();


        public static void ResetData()
        {
            flatModels = new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel>();
            paperRollModels = new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel>();
        }


        public static List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> GetPaperRoll()
        {
            if(paperRollModels.Count == 0)
            {
                paperRollModels.Add(new CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel
                {
                    Id = 1,
                    Subinventory = "TB2",
                    Locator = "SFG",
                    Barcode = "W2006060001",
                    Item_No = "4FHIZA025500635RL00",
                    PaperType = "FHIZ",
                    BaseWeight = "04000",
                    Specification = "310K",
                    TheoreticalWeight = "700",
                    LotNumber = "1400110000776782",
                    Stock = "是",
                    TransactionQuantity = "-100",
                    Reason = "",
                    Created_by = "一力星一號",
                    Last_Updated_Date = DateTime.Now,
                    Panying = "盤虧"
                }) ;

                paperRollModels.Add(new CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel
                {
                    Id = 2,
                    Subinventory = "TB2",
                    Locator = "SFG",
                    Barcode = "W2006060002",
                    Item_No = "4FU0SA025500635RL00",
                    PaperType = "FU0S",
                    BaseWeight = "04000",
                    Specification = "100K",
                    TheoreticalWeight = "800",
                    LotNumber = "1400110000776845",
                    Stock = "是",
                    TransactionQuantity = "200",
                    Reason = "",
                    Created_by = "一力星一號",
                    Last_Updated_Date = DateTime.Now,
                    Panying = "盤盈"
                });

            }


            return paperRollModels;
        }


        public static List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> GetFlat()
        {
            if (flatModels.Count == 0)
            {
                flatModels.Add(new CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel
                {
                    Id = 1,
                    Subinventory = "TB2",
                    Locator = "SFG",
                    Barcode = "P2006060001",
                    Item_No = "4F202020080",
                    ReamWeight = "50",
                    PackingType = "令包",
                    Ream_Qty = "20",
                    StockReam_Qty = "",
                    Reason = "",
                    Created_by = "一力星一號",
                    Last_Updated_Date = DateTime.Now,
                    Panying = "盤虧"
                });

                flatModels.Add(new CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel
                {
                    Id = 2,
                    Subinventory = "TB2",
                    Locator = "SFG",
                    Barcode = "P2006060001",
                    Item_No = "4F202020090",
                    ReamWeight = "40",
                    PackingType = "打件",
                    Ream_Qty = "10",
                    StockReam_Qty = "",
                    Reason = "",
                    Created_by = "一力星一號",
                    Last_Updated_Date = DateTime.Now,
                    Panying = "盤盈"
                });

            }


            return flatModels;
        }



        public List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> Search(string Subinventory, string Item_No, string PaperType, string LotNumber)
        {
            //ResultModel result = new ResultModel(true, "搜尋成功");
            try
            {

                var query = paperRollModels.Where(
                  x =>
                  (Item_No == "*" || x.Item_No != null && x.Item_No.ToLower() == Item_No.ToLower()) &&
                  (PaperType == "*" || x.PaperType != null && x.PaperType.ToLower() == PaperType.ToLower()) &&
                  (LotNumber == "*" || x.LotNumber != null && x.LotNumber.ToLower() == LotNumber.ToLower()) &&
                  (Subinventory == "*" || x.Subinventory != null && x.Subinventory.ToLower() == Subinventory.ToLower())
                  ).ToList();

                //dtData = query;
                return query;
            }
            catch (Exception e)
            {
                //result.Msg = e.Message;
                //result.Success = false;
                return new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel>();
            }
            //return result;
        }

        public List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> Search(string Subinventory, string Item_No, string PackingType)
        {
            //ResultModel result = new ResultModel(true, "搜尋成功");
            try
            {

                var query = flatModels.Where(
                  x =>
                  (Item_No == "*" || x.Item_No != null && x.Item_No.ToLower() == Item_No.ToLower()) &&
                  (PackingType == "*" || x.PackingType != null && x.PackingType.ToLower() == PackingType.ToLower()) &&
                  (Subinventory == "*" || x.Subinventory != null && x.Subinventory.ToLower() == Subinventory.ToLower())
                  ).ToList();

                //dtData = query;
                return query;
            }
            catch (Exception e)
            {
                //result.Msg = e.Message;
                //result.Success = false;
                return new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel>();
            }
            //return result;
        }





        public static IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> Order(List<Order> orders, IEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> models)
        {
            IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> orderedModel = null;
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

        private static IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> OrderBy(int column, string dir, IEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BaseWeight) : models.OrderBy(x => x.BaseWeight);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TheoreticalWeight) : models.OrderBy(x => x.TheoreticalWeight);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Stock) : models.OrderBy(x => x.Stock);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.FirmStock) : models.OrderBy(x => x.FirmStock);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionQuantity) : models.OrderBy(x => x.TransactionQuantity);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason) : models.OrderBy(x => x.Reason);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Panying) : models.OrderBy(x => x.Panying);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_Date) : models.OrderBy(x => x.Last_Updated_Date);
            }
        }

        private static IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> ThenBy(int column, string dir, IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PaperType) : models.OrderBy(x => x.PaperType);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BaseWeight) : models.OrderBy(x => x.BaseWeight);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Specification) : models.OrderBy(x => x.Specification);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TheoreticalWeight) : models.OrderBy(x => x.TheoreticalWeight);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.LotNumber) : models.OrderBy(x => x.LotNumber);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Stock) : models.OrderBy(x => x.Stock);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.FirmStock) : models.OrderBy(x => x.FirmStock);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.TransactionQuantity) : models.OrderBy(x => x.TransactionQuantity);
                case 14:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason) : models.OrderBy(x => x.Reason);
                case 15:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Panying) : models.OrderBy(x => x.Panying);
                case 16:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 17:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_Date) : models.OrderBy(x => x.Last_Updated_Date);
            }
        }


        public static List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> Search(DataTableAjaxPostViewModel data, List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> model)
        {
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (
                    !string.IsNullOrEmpty(p.Id.ToString()) && p.Id.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Subinventory.ToString()) && p.Subinventory.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Locator) && p.Locator.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_No.ToString()) && p.Item_No.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PaperType.ToString()) && p.PaperType.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.BaseWeight.ToString()) && p.BaseWeight.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Specification) && p.Specification.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.TheoreticalWeight) && p.TheoreticalWeight.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.LotNumber) && p.LotNumber.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Stock) && p.Stock.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.FirmStock) && p.FirmStock.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.TransactionQuantity) && p.TransactionQuantity.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Reason) && p.Reason.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Created_by) && p.Created_by.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_Updated_Date.ToString()) && p.Last_Updated_Date.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Panying) && p.Panying.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }



        public static IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> Order(List<Order> orders, IEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> models)
        {
            IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> orderedModel = null;
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

        private static IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> OrderBy(int column, string dir, IEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWeight) : models.OrderBy(x => x.ReamWeight);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Ream_Qty) : models.OrderBy(x => x.Ream_Qty);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StockReam_Qty) : models.OrderBy(x => x.StockReam_Qty);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason) : models.OrderBy(x => x.Reason);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_Date) : models.OrderBy(x => x.Last_Updated_Date);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Panying) : models.OrderBy(x => x.Panying);
              
            }
        }

        private static IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> ThenBy(int column, string dir, IOrderedEnumerable<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> models)
        {
            switch (column)
            {
                default:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Subinventory) : models.OrderBy(x => x.Subinventory);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Locator) : models.OrderBy(x => x.Locator);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Barcode) : models.OrderBy(x => x.Barcode);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Item_No) : models.OrderBy(x => x.Item_No);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ReamWeight) : models.OrderBy(x => x.ReamWeight);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.PackingType) : models.OrderBy(x => x.PackingType);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Ream_Qty) : models.OrderBy(x => x.Ream_Qty);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StockReam_Qty) : models.OrderBy(x => x.StockReam_Qty);
                case 10:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Reason) : models.OrderBy(x => x.Reason);
                case 11:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Created_by) : models.OrderBy(x => x.Created_by);
                case 12:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Last_Updated_Date) : models.OrderBy(x => x.Last_Updated_Date);
                case 13:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Panying) : models.OrderBy(x => x.Panying);

            }
        }


        public static List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> Search(DataTableAjaxPostViewModel data, List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> model)
        {
            string search = data.Search.Value;
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search   
                model = model.Where(p => (
                    !string.IsNullOrEmpty(p.Id.ToString()) && p.Id.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Subinventory.ToString()) && p.Subinventory.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Locator) && p.Locator.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Item_No.ToString()) && p.Item_No.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.ReamWeight.ToString()) && p.ReamWeight.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.PackingType.ToString()) && p.PackingType.ToString().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Ream_Qty) && p.Ream_Qty.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.StockReam_Qty) && p.StockReam_Qty.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Reason) && p.Reason.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Created_by) && p.Created_by.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Last_Updated_Date.ToString()) && p.Last_Updated_Date.ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(p.Panying) && p.Panying.ToLower().Contains(search.ToLower()))
                    ).ToList();
            }
            return model;
        }


    }
}