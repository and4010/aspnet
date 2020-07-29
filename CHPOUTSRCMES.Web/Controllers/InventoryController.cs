using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            ViewBag.Subinventory = GetSubinventory();
            ViewBag.TypeItem = GetPaperType();
            ViewBag.Item_No = GetItemNo();
            ViewBag.PackingTypeItem = GetPackintType();
            ViewBag.PaperTypeItem = GetPaperType();
            ViewBag.LotNumberItem = GetLotNumber();
            return View();
        }

        public ActionResult PaperRollRecord(string Id)
        {
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> model = InventoryViewModel.paperRollModels;
            var m1 = model.FirstOrDefault(r => r.Id == int.Parse(Id));
            return View(m1);
        }

        public ActionResult FlatRecord(string Id)
        {
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> model = InventoryViewModel.flatModels;
            var m1 = model.FirstOrDefault(r => r.Id == int.Parse(Id));
            return View(m1);
        }

        [HttpPost]
        public JsonResult LoadPaperRollTable(DataTableAjaxPostViewModel data, string Subinventory, string Item_No, string PaperType, string LotNumber)
        {
            if (InventoryViewModel.paperRollModels.Count == 0)
            {
                InventoryViewModel.GetPaperRoll();

            }
            InventoryViewModel viewModel = new InventoryViewModel();
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> model = viewModel.Search(Subinventory, Item_No, PaperType, LotNumber);
            model = InventoryViewModel.Search(data, model);
            model = InventoryViewModel.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadFlatTable(DataTableAjaxPostViewModel data, string Subinventory, string Item_No, string PackingType)
        {
            if (InventoryViewModel.flatModels.Count == 0)
            {
                InventoryViewModel.GetFlat();

            }
            InventoryViewModel viewModel = new InventoryViewModel();
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> model = viewModel.Search(Subinventory, Item_No, PackingType);
            model = InventoryViewModel.Search(data, model);
            model = InventoryViewModel.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EditorPaperRoll(DataTableAjaxPostViewModel data, PaperRollDTEditor paperRollDTEditor)
        {
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> model = InventoryViewModel.paperRollModels;
            var paperRoll = paperRollDTEditor.PaperRollList[0];
            var m1 = model.FirstOrDefault(r => r.Id == paperRoll.Id);
            if (m1 != null)
            {
                if (paperRoll.FirmStock == "是")
                {
                    m1.Panying = "盤盈";
                }
                else
                {
                    m1.Panying = "盤虧";
                }
                m1.TransactionQuantity = paperRoll.TransactionQuantity;
                m1.FirmStock = paperRoll.FirmStock;
                m1.Reason = paperRoll.Reason;
                m1.Created_by = "一力星一號";
                m1.Last_Updated_Date = DateTime.Now;
            }

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditorFlat(DataTableAjaxPostViewModel data, FlatDTEditor flatDTEditor)
        {
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> model = InventoryViewModel.flatModels;
            var flatDT = flatDTEditor.FlatList[0];
            var m1 = model.FirstOrDefault(r => r.Id == flatDT.Id);
            if (m1 != null)
            {
                if(m1.Ream_Qty == flatDT.StockReam_Qty)
                {
                    m1.Panying = "正常";
                }else if(int.Parse(m1.Ream_Qty)> int.Parse(flatDT.StockReam_Qty))
                {
                    m1.Panying = "盤虧";
                }else if(int.Parse(m1.Ream_Qty) < int.Parse(flatDT.StockReam_Qty))
                {
                    m1.Panying = "盤盈";
                }
                m1.StockReam_Qty = flatDT.StockReam_Qty;
                m1.Reason = flatDT.Reason;
                m1.Created_by = "一力星一號";
                m1.Last_Updated_Date = DateTime.Now;
            }

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadRecordPaperRollTable(DataTableAjaxPostViewModel data,string Id)
        {
            if (InventoryViewModel.paperRollModels.Count == 0)
            {
                InventoryViewModel.GetPaperRoll();

            }
            InventoryViewModel viewModel = new InventoryViewModel();
            var m1 = InventoryViewModel.paperRollModels.Where(l => Id.Contains(l.Id.ToString()));
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> model = new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel>();
            model.AddRange(m1);
            model = InventoryViewModel.Search(data, model);
            model = InventoryViewModel.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadRecordFlatTable(DataTableAjaxPostViewModel data,string Id)
        {
            if (InventoryViewModel.flatModels.Count == 0)
            {
                InventoryViewModel.GetFlat();

            }
            InventoryViewModel viewModel = new InventoryViewModel();
            var m1 = InventoryViewModel.flatModels.Where(l => Id.Contains(l.Id.ToString()));
            List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> model = new List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel>();
            model.AddRange(m1);
            model = InventoryViewModel.Search(data, model);
            model = InventoryViewModel.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }



        private List<SelectListItem> GetSubinventory()
        {
            List<SelectListItem> GetSubinventory = new List<SelectListItem>();
            GetSubinventory.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            GetSubinventory.Add(new SelectListItem()
            {
                Text = "TB2",
                Value = "TB2",
                Selected = false,
            });
            GetSubinventory.Add(new SelectListItem()
            {
                Text = "TB3",
                Value = "TB3",
                Selected = false,
            });
            return GetSubinventory;
        }

        private List<SelectListItem> GetPaperType()
        {
            List<SelectListItem> GetLocator = new List<SelectListItem>();
            GetLocator.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            GetLocator.Add(new SelectListItem()
            {
                Text = "FHIZ",
                Value = "FHIZ",
                Selected = false,
            });
            GetLocator.Add(new SelectListItem()
            {
                Text = "FU0S",
                Value = "FU0S",
                Selected = false,
            });
            return GetLocator;
        }

        private List<SelectListItem> GetItemNo()
        {
            List<SelectListItem> GetItemNo = new List<SelectListItem>();
            GetItemNo.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            GetItemNo.Add(new SelectListItem()
            {
                Text = "4FHIZA025500635RL00",
                Value = "4FHIZA025500635RL00",
                Selected = false,
            });
            GetItemNo.Add(new SelectListItem()
            {
                Text = "4FU0SA025500635RL00",
                Value = "4FU0SA025500635RL00",
                Selected = false,
            });
            GetItemNo.Add(new SelectListItem()
            {
                Text = "4F202020080",
                Value = "4F202020080",
                Selected = false,
            });

            GetItemNo.Add(new SelectListItem()
            {
                Text = "4F202020090",
                Value = "4F202020090",
                Selected = false,
            });

            return GetItemNo;
        }

        private List<SelectListItem> GetPackintType()
        {
            List<SelectListItem> GetPackintType = new List<SelectListItem>();
            GetPackintType.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            GetPackintType.Add(new SelectListItem()
            {
                Text = "令包",
                Value = "令包",
                Selected = false,
            });
            GetPackintType.Add(new SelectListItem()
            {
                Text = "打件",
                Value = "打件",
                Selected = false,
            });

            return GetPackintType;
        }

        private List<SelectListItem> GetLotNumber()
        {
            List<SelectListItem> GetLotNumber = new List<SelectListItem>();
            GetLotNumber.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            GetLotNumber.Add(new SelectListItem()
            {
                Text = "1400110000776782",
                Value = "1400110000776782",
                Selected = false,
            });
            GetLotNumber.Add(new SelectListItem()
            {
                Text = "1400110000776845",
                Value = "1400110000776845",
                Selected = false,
            });

            return GetLotNumber;
        }


        public class PaperRollDTEditor
        {
            public string Action { get; set; }
            //public List<long> TripDetailDT_IDs { get; set; }
            //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
            public List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.PaperRollModel> PaperRollList { get; set; }
        }

        public class FlatDTEditor
        {
            public string Action { get; set; }
            //public List<long> TripDetailDT_IDs { get; set; }
            //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
            public List<CHPOUTSRCMES.Web.Models.Inventory.Inventory.FlatModel> FlatList { get; set; }
        }





    }

}