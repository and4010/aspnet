using CHPOUTSRCMES.Web.Models.Process;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class ProcessController : Controller
    {
        //
        // GET: /Process/
        public ActionResult Index()
        {
            ViewBag.Process_Status = GetProcess_Status();
            ViewBag.Process_Batch_no = GetProcess_Batch_no();
            ViewBag.Manchine_Num = GetManchine_Num();
            ViewBag.Subinventory = GetSubinventory();
            return View();
        }

        public ActionResult Schedule(String id)
        {
            ViewBag.RemnantItem = GetRemnantItem();
            ViewBag.CotangentItem = GetCotangentItem();
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            ProcessIndexViewModel procesIndexViewModel = new ProcessIndexViewModel();
            var cpt = model.First(r => r.Process_Heard_Id.ToString() == id);
            procesIndexViewModel.CHP_PROCESS_T = cpt;
            procesIndexViewModel.Production = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Process_Detail_Id.ToString() == id);

            return View(procesIndexViewModel);
        }

        public ActionResult Edit(String id)
        {
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            ProcessIndexViewModel procesIndexViewModel = new ProcessIndexViewModel();
            var cpt = model.First(r => r.Process_Heard_Id.ToString() == id);
            procesIndexViewModel.CHP_PROCESS_T = cpt;
            procesIndexViewModel.Production = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Production_Id.ToString() == id);

            return View(procesIndexViewModel);
        }

        public JsonResult EditSave(string Process_Detail_Id,string remark)
        {
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            var cpt = model.First(r => r.Process_Heard_Id.ToString() == Process_Detail_Id);
            cpt.Produce_Remark = remark;
            var Boolean = true;
            return Json(new { Boolean },JsonRequestBehavior.AllowGet);
        }

        public ActionResult Flat(String id)
        {
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            ProcessIndexViewModel procesIndexViewModel = new ProcessIndexViewModel();
            var cpt = model.First(r => r.Process_Heard_Id.ToString() == id);
            procesIndexViewModel.CHP_PROCESS_T = cpt;
            procesIndexViewModel.Production = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Process_Detail_Id.ToString() == id);

            return View(procesIndexViewModel);
        }

        public ActionResult PaperRoll(String id)
        {
            ViewBag.RemnantItem = GetRemnantItem();
            ViewBag.CotangentItem = GetCotangentItem();
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            ProcessIndexViewModel procesIndexViewModel = new ProcessIndexViewModel();
            var cpt = model.First(r => r.Process_Heard_Id.ToString() == id);
            procesIndexViewModel.CHP_PROCESS_T = cpt;
            procesIndexViewModel.Production = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Process_Detail_Id.ToString() == id);

            return View(procesIndexViewModel);
        }


        [HttpPost]
        public ActionResult _ProcessIndex()
        {
            ViewBag.MachineItems = GetManchine();
            return PartialView();
        }

        [HttpPost]
        public ActionResult _Subinventory()
        {
            ViewBag.LocatorItem = GetLocator();
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChagneIndexStatus(string Process_Batch_no,string Locator,string Status)
        {
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            var cpt = model.FirstOrDefault(r => r.Process_Batch_no == Process_Batch_no);
            if (cpt != null)
            {
                if(Status == "完工紀錄")
                {
                    cpt.Process_Status = "待核准";
                } else if (Status == "核准")
                {
                    cpt.Process_Status = "已完工";
                }
                else
                {
                    cpt.Process_Status = "已完工";
                    cpt.Locator = Locator;
                }
            
            }
            return Json(JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult _BtnDailog(DataTableAjaxPostViewModel data, string Process_Detail_Id, string dialog_Cutting_Date_From, string dialog_Cutting_Date_To,string dialg_Manchine_Num, string BtnStatus)
        {
            List<CHP_PROCESS_T> model = ProcessIndexViewModel.chp_process_t;
            var ID = model.First(r => r.Process_Detail_Id.ToString() == Process_Detail_Id);
            if (ID != null)
            {
                if(dialog_Cutting_Date_To != null)
                {
                    ID.Cutting_Date_To = Convert.ToDateTime(dialog_Cutting_Date_To);
                    ID.Cutting_Date_From = Convert.ToDateTime(dialog_Cutting_Date_From);
                    ID.Manchine_Num = dialg_Manchine_Num;
                }
                ID.Process_Status = BtnStatus;
            }


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TableResult(DataTableAjaxPostViewModel data, string Process_Status, string Process_Batch_no, string Manchine_Num,
            string Demand_Date, string Cutting_Date_From, string Cutting_Date_To, string Subinventory)
        {
            if (ProcessIndexViewModel.chp_process_t.Count == 0)
            {
                ProcessIndexViewModel.GetTable();

            }
            ProcessIndexViewModel viewModel = new ProcessIndexViewModel();
            List<CHP_PROCESS_T> model = viewModel.Search(Process_Status, Process_Batch_no, Manchine_Num, Demand_Date, Cutting_Date_From, Cutting_Date_To, Subinventory);
            model = ProcessIndexViewModel.Search(data, model);
            model = ProcessIndexViewModel.Order(data.Order, model).ToList();
            var model1 = model.Skip(data.Start).Take(data.Length).ToList();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckOrderNumber(string ProcessBatchNo)
        {
            var boolean = false;
            var orde = ProcessIndexViewModel.chp_process_t.FirstOrDefault(r => r.Process_Batch_no == ProcessBatchNo);
            if(orde == null)
            {
                boolean = false;
            }
            else
            {
                boolean = true;
            }
            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvestLoadTable(DataTableAjaxPostViewModel data, string Process_Detail_Id)
        {
            List<Invest> model = new List<Invest>();
            var list =
                    from invent in ProcessIndexViewModel.Invest_Stock
                    where invent.Process_Detail_Id == int.Parse(Process_Detail_Id)
                    select invent;

            model.AddRange(list);
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Barcode(string Barcode, string Process_Detail_Id)
        {
            ProcessIndexViewModel.GetInvest();
            var result = false;
            ProcessIndexViewModel procesIndexViewModel = new ProcessIndexViewModel();

            List<Invest> model = ProcessIndexViewModel.invest;

            var cpd = model.FirstOrDefault(r => r.Barcode == Barcode && r.Process_Detail_Id.ToString() == Process_Detail_Id);
            if (cpd == null)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return Json(new { result, cpd }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvestTable(string Barcode, string Remnant, string Remaining_Weight, string Process_Detail_Id)
        {
            List<Invest> chp_process_detail_t = ProcessIndexViewModel.invest;
            List<Invest> model = ProcessIndexViewModel.Invest_Stock;
            var check = 0;


            var checkbarode = chp_process_detail_t.FirstOrDefault(r => r.Barcode == Barcode && r.Process_Detail_Id.ToString() == Process_Detail_Id);


            if (checkbarode == null)
            {
                //資料不存在
                check = 2;
            }


            if (Barcode != null && Barcode != "")
            {
                var bd = model.FirstOrDefault(r => r.Barcode == Barcode && r.Process_Detail_Id.ToString() == Process_Detail_Id);
                if (bd != null)
                {
                    check = 1;
                }
                else
                {
                    var list =
                              from CHP_PROCESS_DETAIL_T in chp_process_detail_t
                              where CHP_PROCESS_DETAIL_T.Barcode == Barcode
                              select CHP_PROCESS_DETAIL_T;

                    ProcessIndexViewModel.Invest_Stock.AddRange(list);
                    //1 有殘捲 0 無殘捲
                    if (Remnant != null)
                    {
                        checkbarode.Remnant = Remnant == "1" ? "有" : "無";
                        checkbarode.Remaining_Weight = Remaining_Weight;
                    }

                }
            }





            return Json(new { check }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvestEdit(DetailDTEditor InvestDTList)
        {
            if(InvestDTList.Action == "edit")
            {
                List<Invest> model = ProcessIndexViewModel.Invest_Stock;
                var id = model.FirstOrDefault(r => r.Invest_Id == InvestDTList.InvestList[0].Invest_Id);
                if (id != null)
                {
                    id.Remaining_Weight = InvestDTList.InvestList[0].Remaining_Weight;
                    id.Remnant = InvestDTList.InvestList[0].Remnant;
                }
        
            }
          
            if(InvestDTList.Action == "remove")
            {
                var barcode = ProcessIndexViewModel.Invest_Stock.FirstOrDefault(r => r.Invest_Id == InvestDTList.InvestList[0].Invest_Id);
                if (barcode != null)
                {
                    ProcessIndexViewModel.Invest_Stock.Remove(barcode);
                }
  
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvestDelete(string Barcode, string Process_Detail_Id)
        {
            var boolean = true;
            var barcode = ProcessIndexViewModel.Invest_Stock.FirstOrDefault(r => r.Barcode == Barcode && r.Process_Detail_Id == int.Parse(Process_Detail_Id));
            if (barcode != null)
            {
                ProcessIndexViewModel.Invest_Stock.Remove(barcode);
            }
            else
            {
                boolean = false;
            }

            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ProductionLoadDataTables(DataTableAjaxPostViewModel data, string Process_Detail_Id)
        {
            List<Production> model = new List<Production>();
            var list =
                   from Productions in ProcessIndexViewModel.ListProductions
                   where Productions.Process_Detail_Id == int.Parse(Process_Detail_Id)
                   select Productions;

            model.AddRange(list);
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionDetail(string Production_Roll_Ream_Qty, string Production_Roll_Ream_Wt, string Product_Item, string Process_Detail_Id)
        {
            List<Production> model = ProcessIndexViewModel.ListProductions;
            var boolean = true;
            var msg = "";
            if (Production_Roll_Ream_Qty != null)
            {
                var count = Convert.ToInt32(Production_Roll_Ream_Qty);
                var weight = Convert.ToInt32(Production_Roll_Ream_Wt) / count;
                if (model.Count != 0)
                {
                    var d = model.FirstOrDefault(r => r.Barcode != "");
                }
                for (int i = 0; i < count; i++)
                {
                    model.Add(new Production
                    {
                        Process_Detail_Id = int.Parse(Process_Detail_Id),
                        Production_Id = model.Count == 0 ? (i + 1) : (model.Count + 1),
                        Barcode = "P201006000" + (model.Count == 0 ? (i + 1).ToString() : (model.Count + 1).ToString()),
                        Roll_Ream_Wt = Production_Roll_Ream_Wt,
                        Weight = weight.ToString(),
                        Status = "待入庫",
                        Item_No = Product_Item,
                        Roll_Ream_Qty = Production_Roll_Ream_Qty
                    });
                }
            }
            else
            {
                msg = "令數不得空白";
                boolean = false;
            }


            return Json(new { boolean , msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionDelete(string Production_Id)
        {
            var boolean = true;
            var ProductionId = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Production_Id.ToString() == Production_Id);
            if (ProductionId != null)
            {
                ProcessIndexViewModel.ListProductions.Remove(ProductionId);
            }
            else
            {
                boolean = false;
            }

            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionEdit(DataTableAjaxPostViewModel data,ProductionDTEditor ProductionDTEditor)
        {
        
            List<Production> model = new List<Production>();
            if (ProductionDTEditor.Action == "edit")
            {
                List<Production> model1 = ProcessIndexViewModel.ListProductions;
                var id = model1.FirstOrDefault(r => r.Production_Id == ProductionDTEditor.ProductionList[0].Production_Id);
                if (id != null)
                {
                    id.Roll_Ream_Wt = ProductionDTEditor.ProductionList[0].Roll_Ream_Wt;
                    id.Weight = ProductionDTEditor.ProductionList[0].Weight;
                }
             
                model.Add(id);
            }
            if(ProductionDTEditor.Action == "remove")
            {
                var ProductionId = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Production_Id == ProductionDTEditor.ProductionList[0].Production_Id);
                if (ProductionId != null)
                {
                    ProcessIndexViewModel.ListProductions.Remove(ProductionId);
                }
            }
          
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionChangeStatus(string Production_Barcode, string Process_Detail_Id)
        {
            var check = 0;
            var ProductionId = ProcessIndexViewModel.ListProductions.FirstOrDefault(r => r.Barcode == Production_Barcode && r.Process_Detail_Id == int.Parse(Process_Detail_Id));
            if (ProductionId != null)
            {
                if (ProductionId.Status == "已入庫")
                {
                    check = 1;
                }
                else
                {
                    ProductionId.Status = "已入庫";
                    check = 2;
                }

            }
            else
            {
                //無條碼
                check = 3;
            }

            return Json(new { check }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CotangentDataTables(DataTableAjaxPostViewModel data, string Production_Cotangent,string Process_Detail_Id)
        {
            List<Cotangent> model = new List<Cotangent>();
            if (Production_Cotangent == "1")
            {
                ProcessIndexViewModel.GetCotangents(Process_Detail_Id);
            }

            model = ProcessIndexViewModel.ListCotangent;
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CotangentEdit(DataTableAjaxPostViewModel data, CotangentDTEditor cotangentDTEditor)
        {
            var model = ProcessIndexViewModel.ListCotangent;
            if (cotangentDTEditor.Action == "edit")
            {
                var id = model.FirstOrDefault(r => r.Cotangent_Id == cotangentDTEditor.CotangentList[0].Cotangent_Id);
                if(id != null)
                {
                    id.Cotangent_Ttl_Roll_Ream = cotangentDTEditor.CotangentList[0].Cotangent_Ttl_Roll_Ream;
                    id.Kg = (int.Parse(cotangentDTEditor.CotangentList[0].Cotangent_Ttl_Roll_Ream) * 2).ToString();
                }
               
            }
           if(cotangentDTEditor.Action == "remove")
            {
                
                var id = model.FirstOrDefault(r => r.Cotangent_Id == cotangentDTEditor.CotangentList[0].Cotangent_Id);
                if(id != null)
                {
                    ProcessIndexViewModel.ListCotangent.Remove(id);
                }
               
            }
         
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CotangentDelete(string Cotangent_Id)
        {
            var boolean = true;
            var CotangentId = ProcessIndexViewModel.ListCotangent.FirstOrDefault(r => r.Cotangent_Id.ToString() == Cotangent_Id);
            if (CotangentId != null)
            {
                ProcessIndexViewModel.ListCotangent.Remove(CotangentId);
            }
            else
            {
                boolean = false;
            }

            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CotangentChangeStatus(string CotangentBarcode)
        {
            var check = 0;
            var CotangentId = ProcessIndexViewModel.ListCotangent.FirstOrDefault(r => r.Barcode == CotangentBarcode);
            if (CotangentId != null)
            {
                if (CotangentId.Status == "已入庫")
                {
                    check = 1;
                }
                else
                {
                    CotangentId.Status = "已入庫";
                    check = 2;
                }

            }
            else
            {
                //無條碼
                check = 3;
            }

            return Json(new { check }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RecordCotangentDataTables(DataTableAjaxPostViewModel data)
        {
            List<Cotangent> model = new List<Cotangent>();
            model = ProcessIndexViewModel.ListCotangent;
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Loss(string TotalWeight, string Percentage, string Process_Detail_Id)
        {
            List<Production> model = ProcessIndexViewModel.ListProductions;
            for(int i = 0; i < model.Count; i++)
            {
                if (model[i].Process_Detail_Id == int.Parse(Process_Detail_Id))
                {
                    model[i].Loss = "重量(KG)" + TotalWeight + "得率" + Percentage + "%";
                }
            }

            //var id = model.FirstOrDefault(r => r.Process_Detail_Id == int.Parse(Process_Detail_Id));
            //if (id != null)
            //{
            //    id.Loss = "重量(KG)" + TotalWeight + "得率" + Percentage + "%";
            //}
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult PaperRollProductionDetail(string PaperRoll_Basic_Weight, string PaperRoll_Specification, string PaperRoll_Lot_Number, string Product_Item, string Process_Detail_Id)
        {
            List<Production> model = ProcessIndexViewModel.ListProductions;
            var boolean = true;
            if (PaperRoll_Lot_Number != null)
            {
                model.Add(new Production
                {
                    Process_Detail_Id = int.Parse(Process_Detail_Id),
                    Production_Id = model.Count == 0 ? (1) : (model.Count + 1),
                    Barcode = "W201006000" + (model.Count == 0 ? (1).ToString() : (model.Count + 1).ToString()),
                    Weight = PaperRoll_Basic_Weight.ToString(),
                    Status = "待入庫",
                    Item_No = Product_Item,
                    Lot_Number = PaperRoll_Lot_Number
                });

            }


            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }


        private List<SelectListItem> GetProcess_Status()
        {
            List<SelectListItem> Process_Status = new List<SelectListItem>();


            Process_Status.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            Process_Status.Add(new SelectListItem()
            {
                Text = "待排單",
                Value = "待排單",
                Selected = false,
            });
            Process_Status.Add(new SelectListItem()
            {
                Text = "已排單",
                Value = "已排單",
                Selected = false,
            });
            Process_Status.Add(new SelectListItem()
            {
                Text = "待核准",
                Value = "待核准",
                Selected = false,
            });
            Process_Status.Add(new SelectListItem()
            {
                Text = "已完工",
                Value = "已完工",
                Selected = false,
            });
            Process_Status.Add(new SelectListItem()
            {
                Text = "關帳",
                Value = "關帳",
                Selected = false,
            });
            return Process_Status;
        }

        private List<SelectListItem> GetProcess_Batch_no()
        {
            List<SelectListItem> Process_Batch_no = new List<SelectListItem>();
            Process_Batch_no.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            Process_Batch_no.Add(new SelectListItem()
            {
                Text = "P9B0288",
                Value = "P9B0288",
                Selected = false,
            });
            Process_Batch_no.Add(new SelectListItem()
            {
                Text = "F2010087",
                Value = "F2010087",
                Selected = false,
            });
            Process_Batch_no.Add(new SelectListItem()
            {
                Text = "R9B0287",
                Value = "R9B0287",
                Selected = false,
            });
            return Process_Batch_no;
        }

        private List<SelectListItem> GetManchine_Num()
        {
            List<SelectListItem> GetManchine_Num = new List<SelectListItem>();
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "*",
                Selected = false,
            });
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "01",
                Value = "01",
                Selected = false,
            });
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "02",
                Value = "02",
                Selected = false,
            });
            return GetManchine_Num;
        }

        private List<SelectListItem> GetRemnantItem()
        {
            List<SelectListItem> GetManchine_Num = new List<SelectListItem>();
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "無",
                Value = "0",
                Selected = false,
            });
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "有",
                Value = "1",
                Selected = false,
            });
            return GetManchine_Num;
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

        private List<SelectListItem> GetLocator()
        {
            List<SelectListItem> GetLocator = new List<SelectListItem>();
            GetLocator.Add(new SelectListItem()
            {
                Text = "SFG",
                Value = "SFG",
                Selected = false,
            });
            GetLocator.Add(new SelectListItem()
            {
                Text = "TB3",
                Value = "TB3",
                Selected = false,
            });
            return GetLocator;
        }

        private List<SelectListItem> GetCotangentItem()
        {
            List<SelectListItem> GetCotangentItem = new List<SelectListItem>();
            GetCotangentItem.Add(new SelectListItem()
            {
                Text = "無",
                Value = "0",
                Selected = false,
            });
            GetCotangentItem.Add(new SelectListItem()
            {
                Text = "有",
                Value = "1",
                Selected = false,
            });
            return GetCotangentItem;
        }

        private List<SelectListItem> GetManchine()
        {
            List<SelectListItem> GetManchine_Num = new List<SelectListItem>();
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "01",
                Value = "01",
                Selected = false,
            });
            GetManchine_Num.Add(new SelectListItem()
            {
                Text = "02",
                Value = "02",
                Selected = false,
            });
            return GetManchine_Num;
        }

        public class DetailDTEditor
        {
            public string Action { get; set; }
            //public List<long> TripDetailDT_IDs { get; set; }
            //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
            public List<Invest> InvestList { get; set; }
        }

        public class ProductionDTEditor
        {
            public string Action { get; set; }
            //public List<long> TripDetailDT_IDs { get; set; }
            //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
            public List<Production> ProductionList { get; set; }
        }

        public class CotangentDTEditor
        {
            public string Action { get; set; }
            //public List<long> TripDetailDT_IDs { get; set; }
            //public List<string> TRANSACTION_AUTHORIZE_DATEs { get; set; }
            public List<Cotangent> CotangentList { get; set; }
        }

    }
}