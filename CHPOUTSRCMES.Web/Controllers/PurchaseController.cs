using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using System.Web;
using System.Linq;
using CHPOUTSRCMES.Web.Models.Information;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class PurchaseController : Controller
    {

        //
        // GET: /Inbound/
        public ActionResult Index()
        {
            var model = new PurchaseViewModel()
            {
                Year = DateTime.Now.Year.ToString(),
                Month = DateTime.Now.Month.ToString("00"),
                Warehouse = "TB2"
            };
            ViewBag.MonthItems = GetMonths();
            ViewBag.YearItems = GetYears();
            ViewBag.WarehouseItems = GetWarehouses();

            return View(model);
        }


        public JsonResult GetEvents(string id)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var events = new List<FullCalendarEventModel>();
            if (PurchaseViewModel.fullCalendarEventModel.Count == 0)
            {
                events.Add(new FullCalendarEventModel()
                {
                    id = 1,
                    title = "TB2" + "\nWHLU5321157" + " 待入庫",
                    start = "2020-02-25 10:00:00",
                    end = "2020-02-25 24:00:00",
                    allDay = false,
                    url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "WHLU5321157" }),
                    Status = "1"
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 3,
                    title = "TB2" + "\nWHAU5231488" + " 已取消",
                    start = "2020-02-25 14:00:00",
                    end = "2020-02-25 24:00:00",
                    allDay = false,

                    Status = "2",
                    color = "#E60000",
                });



                events.Add(new FullCalendarEventModel()
                {
                    id = 1,
                    title = "TB2" + "\nWHLU5321157" + " 待入庫",
                    start = "2020-03-06 10:00:00",
                    end = "2020-03-06 24:00:00",
                    allDay = false,
                    url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "WHLU5321157", Status = "1", start = "2020-03-06 10:00:00" }),
                    Status = "1"
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 3,
                    title = "TB2" + "\nWHAU5231488" + " 已取消",
                    start = "2020-03-07 14:00:00",
                    end = "2020-03-07 24:00:00",
                    allDay = false,
                    url = "",
                    Status = "2",
                    color = "#E60000",
                });




                events.Add(new FullCalendarEventModel()
                {
                    id = 1,
                    title = "TB2" + "\nWHLU5321157" + " 待入庫",
                    start = "2020-04-08 10:00:00",
                    end = "2020-04-08 24:00:00",
                    allDay = false,
                    url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "WHLU5321157", Status = "1", start = "2020-04-08 10:00:00" }),
                    Status = "1"
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 3,
                    title = "TB2" + "\nWHAU5231488" + " 已取消",
                    start = "2020-04-09 14:00:00",
                    end = "2020-04-09 24:00:00",
                    allDay = false,
                    url = "",
                    Status = "2",
                    color = "#E60000",
                });




                events.Add(new FullCalendarEventModel()
                {
                    id = 1,
                    title = "TB2"+ "\nWHLU5321157" + " 待入庫",
                    start = "2020-05-08 10:00:00",
                    end = "2020-05-08 24:00:00",
                    allDay = false,
                    Status = "1",
                    url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "WHLU5321157", Status = "1", start = "2020-05-08 10:00:00" }),
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 2,
                    title = "TB2" + "\nWHAU5231488" + " 已取消",
                    start = "2020-05-09 14:00:00",
                    end = "2020-05-09 24:00:00",
                    allDay = false,
                    url = "",
                    Status = "2",
                    color = "#E60000",
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 1,
                    title = "TB2" + "\nTGBU6882663" + " 待入庫",
                    start = "2020-06-08 10:00:00",
                    end = "2020-06-08 24:00:00",
                    allDay = false,
                    Status = "1",
                    url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "TGBU6882663", Status = "1", start = "2020-06-08 10:00:00" }),
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 2,
                    title = "TB2" + "\nWHAU5231488" + " 已取消",
                    start = "2020-06-09 14:00:00",
                    end = "2020-06-09 24:00:00",
                    allDay = false,
                    url = "",
                    Status = "2",
                    color = "#E60000",
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 1,
                    title = "TB2" + "\nTGBU6882663" + " 待入庫",
                    start = "2020-07-08 10:00:00",
                    end = "2020-07-08 24:00:00",
                    allDay = false,
                    Status = "1",
                    url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "TGBU6882663", Status = "1", start = "2020-07-08 10:00:00" }),
                });

                events.Add(new FullCalendarEventModel()
                {
                    id = 2,
                    title = "TB2" + "\nWHAU5231488" + " 已取消",
                    start = "2020-07-09 14:00:00",
                    end = "2020-07-09 24:00:00",
                    allDay = false,
                    url = "",
                    Status = "2",
                    color = "#E60000",
                });

                PurchaseViewModel.fullCalendarEventModel = events;
            }





            return Json(PurchaseViewModel.fullCalendarEventModel.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReturnIndex(string CabinetNumber, string CreateDate)
        {
            var boolean = true;
            var creatdate = PurchaseViewModel.fullCalendarEventModel.First(r => r.start == CreateDate);
            if (creatdate != null)
            {
                creatdate.title = "TB2" + "\nTGBU6882663" + " 已入庫";
                creatdate.Status = "0";
                creatdate.url = @Url.Action("Detail", "Purchase", new { CONTAINER_NO = "TGBU6882663", Status = "0", start = CreateDate });
            }
            return Json(new { boolean },JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                string month = i.ToString();
                month = month.PadLeft(2, '0');
                months.Add(new SelectListItem()
                {
                    Text = month,
                    Value = month,
                    Selected = false,
                });
            }
            return months;
        }

        private List<SelectListItem> GetYears()
        {
            List<SelectListItem> years = new List<SelectListItem>();
            int nowYear = DateTime.Now.Year;

            if (nowYear < 100)
            {
                nowYear = 100; //fullcendar支援的最小年份
            }

            int maxYear = nowYear + 1;
            int minYear = nowYear - 4;

            if (minYear < 100)
            {
                minYear = 100; //fullcendar支援的最小年份
            }

            for (int i = minYear; i <= maxYear; i++)
            {
                string year = i.ToString();
                year = year.PadLeft(4, '0');
                years.Add(new SelectListItem()
                {
                    Text = year,
                    Value = year,
                    Selected = false,
                });
            }
            return years;
        }

        private List<SelectListItem> GetWarehouses()
        {
            List<SelectListItem> warehouses = new List<SelectListItem>();

            warehouses.Add(new SelectListItem()
            {
                Text = "TB2",
                Value = "TB2",
                Selected = false,
            });
            return warehouses;
        }

        private List<SelectListItem> GetReason()
        {
            List<SelectListItem> reason = new List<SelectListItem>();
            List<ReasonModel> models = new List<ReasonModel>();
            reason.Add(new SelectListItem()
            {
                Text = "A-破損",
                Value = "A-破損",
                Selected = false,
            });
            reason.Add(new SelectListItem()
            {
                Text = "B-汙垢",
                Value = "B-汙垢",
                Selected = false,
            });
            return reason;
        }

        // GET: Purchase
        [HttpGet, ActionName("Detail")]
        public ActionResult Detail(string CONTAINER_NO = "", string Status = "", string start = "")
        {
            PurchaseViewModel model = new PurchaseViewModel();

            model.CreateDate = start;
            model.CabinetNumber = CONTAINER_NO;
            model.Subinventory = "TB2";
            model.Status = Status;
            return View(model);
        }


        [HttpPost, ActionName("RollHeader")]
        public JsonResult RollHeader(DataTableAjaxPostViewModel data)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.RollModel> model;
            model = purchaseViewModel.GetRollHeader();


            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }




        [HttpPost, ActionName("RollBody")]
        public JsonResult RollBody(DataTableAjaxPostViewModel data, string status)
        {
            List<DetailModel.RollDetailModel> model;
            if (status == "1")
            {
                model = new List<DetailModel.RollDetailModel>();
            }
            else if (status == "4")
            {
                if (PurchaseViewModel.StockInRoll.Count == 0)
                {
                    PurchaseViewModel.GetStockInRoll();
                }
            }

            model = PurchaseViewModel.StockInRoll;

            model = PurchaseViewModel.RollDetailModelDTOrder.Search(data, model);
            model = PurchaseViewModel.RollDetailModelDTOrder.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }



        [HttpPost, ActionName("FlatHeader")]
        public JsonResult FlatHeader(DataTableAjaxPostViewModel data)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.FlatModel> model;

            model = purchaseViewModel.GetFlatHeader();

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }




        [HttpPost, ActionName("FlatBody")]
        public JsonResult FlatBody(DataTableAjaxPostViewModel data, string status)
        {


            List<DetailModel.FlatDetailModel> model;

            if (PurchaseViewModel.StockInFlat.Count == 0)
            {
                PurchaseViewModel.GetStockInFlat();
            }
            model = PurchaseViewModel.StockInFlat;

            model = PurchaseViewModel.FlatDetailModelDTOrder.Search(data, model);
            model = PurchaseViewModel.FlatDetailModelDTOrder.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }

        [HttpPost]
        public JsonResult CheckCabinetNumber(string CabinetNumber)
        {
            var boolean = false;
             if(CabinetNumber == "TGBU6882663")
            {
                boolean = true;
            }
            else
            {
                boolean = false;
            }
            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _ImportBodyRoll()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _ImportBodyFlat()
        {
            return PartialView();
        }


        public ActionResult RollView(string id)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            model.CreateDate = "2020-06-08 10:00:00";
            model.CabinetNumber = "TGBU6882663";
            model.Subinventory = "TB2";

            ViewBag.WarehouseItems = GetWarehouses();
            ViewBag.ReasonItems = GetReason();


            model.RollDetailModel = model.GetRollEdit(id);

            return View(model);
        }

        public ActionResult FlatView(string id)
        {
            PurchaseViewModel model = new PurchaseViewModel();


            model.CreateDate = "2020-06-08 10:00:00";
            model.CabinetNumber = "TGBU6882663";
            model.Subinventory = "TB2";
            ViewBag.WarehouseItems = GetWarehouses();
            ViewBag.ReasonItems = GetReason();

            model.FlatDetailModel = model.GetFlatEdit(id);

            return View(model);
        }


        public ActionResult RollEdit(string id)
        {

            PurchaseViewModel model = new PurchaseViewModel();
            string[] sarry = id.ToString().Split(new char[1] { '-' });
            var Id = sarry[0];
            var status = sarry[1];
            model.CreateDate = "2020-06-08 10:00:00";
            model.CabinetNumber = "TGBU6882663";
            model.Subinventory = "TB2";
            model.Status = status;

            ViewBag.WarehouseItems = GetWarehouses();
            ViewBag.ReasonItems = GetReason();


            model.RollDetailModel = model.GetRollEdit(Id);



            return View(model);
        }


        public ActionResult FlatEdit(string id)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            string[] sarry = id.ToString().Split(new char[1] { '-' });
            var Id = sarry[0];
            var status = sarry[1];

            model.Status = status;
            model.CreateDate = "2020-06-08 10:00:00";
            model.CabinetNumber = "TGBU6882663";
            model.Subinventory = "TB2";
            ViewBag.WarehouseItems = GetWarehouses();
            ViewBag.ReasonItems = GetReason();

            model.FlatDetailModel = model.GetFlatEdit(Id);


            return View(model);
        }

        [HttpPost]
        public JsonResult RollEditSave(string Remak, int id, string Status,string Reason)
        {
            PurchaseViewModel model = new PurchaseViewModel();

            model.GetRollEditRemak(Remak, id, Status,Reason);
            var boolean = true;

            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FlatEditSave(string Remak, int id, string Status, string Reason)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            model.GetFlatEditRemak(Remak, id, Status, Reason);
            var boolean = true;

            return Json(new { boolean }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RollSaveBarcode(DataTableAjaxPostViewModel data, string Barcode)
        {
            var Boolean = true;
            var BarcodeStatus = true;
            List<DetailModel.RollDetailModel> model;
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            model = purchaseViewModel.SaveRollBarcode(Barcode, ref Boolean, ref BarcodeStatus);


            return Json(new { Boolean, BarcodeStatus }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult FlatSaveBarcode(DataTableAjaxPostViewModel data, string Barcode)
        {
            var Boolean = true;
            var BarcodeStatus = true;
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.FlatDetailModel> model;
            model = purchaseViewModel.SaveFlatBarcode(Barcode, ref Boolean, ref BarcodeStatus);

            return Json(new { Boolean, BarcodeStatus }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UploadFileRoll(HttpPostedFileBase file, DataTableAjaxPostViewModel data)
        {
            var result = new ResultModel();
            var detail = new List<DetailModel.RollDetailModel>();
            if (file == null || file.ContentLength == 0)
            {
                result.Msg = "檔案不得空白";
                result.Success = false;
            }
            else
            {
                string extension = Path.GetExtension(file.FileName);
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    //string filelocation = Server.MapPath("~/Content/");

                    //if (System.IO.File.Exists(filelocation))
                    //{
                    //    System.IO.File.Delete(filelocation);
                    //};
                    try
                    {
                        //file.SaveAs(Path.Combine(filelocation, file.FileName));
                        ExcelImportRoll(file, data, ref detail, ref result);
                    }
                    catch (Exception e)
                    {
                        result.Msg = e.Message;
                        result.Success = false;
                    }

                }
                else
                {
                    result.Msg = "只能上傳excel文件";
                    result.Success = false;
                }
            }
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcelImportRoll(HttpPostedFileBase file, DataTableAjaxPostViewModel data, ref List<DetailModel.RollDetailModel> detail, ref ResultModel result)
        {

            PurchaseViewModel purchaseView = new PurchaseViewModel();
            List<DetailModel.RollModel> RollHeader = purchaseView.GetRollHeader();
            var papper = new ExcelImport();
            papper.PaperRollDetail(file, ref data, ref detail, ref result, RollHeader);
            PurchaseViewModel.StockInRoll = detail;
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcelDelete()
        {
            PurchaseViewModel.StockInRoll = new List<DetailModel.RollDetailModel>();
            return Json(JsonRequestBehavior.AllowGet);
        }

        //頁籤數字
        [HttpPost]
        public JsonResult PaperNumber()
        {
            var PR = 0;
            var PH = 0;
            PurchaseViewModel purchaseView = new PurchaseViewModel();
            List <DetailModel.RollModel> header = purchaseView.GetRollHeader();
            for(int i =0; i<header.Count; i++)
            {
                PH += Int32.Parse(header[i].RollReamQty);
            }
            var model = PurchaseViewModel.StockInRoll;
            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].Status == "已入庫")
                {
                    PR++;
                }
            }
            var PaperTotle = PH - PR;
            return Json(new { PaperTotle }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FlatNumber()
        {
            var FB = 0;
            var FH = 0;
            PurchaseViewModel purchaseView = new PurchaseViewModel();
            List<DetailModel.FlatModel> header = purchaseView.GetFlatHeader();
            for (int i = 0; i < header.Count; i++)
            {
                FH += Int32.Parse(header[i].RollReamQty);
            }
            var model = PurchaseViewModel.StockInFlat;
            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].Status == "已入庫")
                {
                    FB++;
                }
            }
            var FlatTotle = FH - FB;
            return Json(new { FlatTotle }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadFileFlat(HttpPostedFileBase file, DataTableAjaxPostViewModel data)
        {
            var result = new ResultModel();
            var detail = new List<DetailModel.FlatDetailModel>();
            if (file == null || file.ContentLength == 0)
            {
                result.Msg = "檔案不得空白";
                result.Success = false;
            }
            else
            {
                string extension = Path.GetExtension(file.FileName);
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    try
                    {
                        ExcelImportFlat(file, data, ref detail, ref result);
                    }
                    catch (Exception e)
                    {
                        result.Msg = e.Message;
                        result.Success = false;
                    }

                }
                else
                {
                    result.Msg = "只能上傳excel文件";
                    result.Success = false;
                }
            }
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ExcelImportFlat(HttpPostedFileBase file, DataTableAjaxPostViewModel data, ref List<DetailModel.FlatDetailModel> detail, ref ResultModel result)
        {
            var papper = new ExcelImport();
            //papper.FlatDetail(file, ref detail, ref result);
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }


      
        public void DownloadFile()
        {
            //用戶端的物件
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] file = null;
            string filepath = Server.MapPath("../File/detail.xls");
            try
            {
                //用戶端下載檔案到byte陣列
                file = wc.DownloadData(filepath);
            }
            catch (Exception ex)
            {
                Response.Write("ASP.net禁止下載此敏感檔案(通常為：.cs、.vb、微軟資料庫mdb、mdf和config組態檔等)。<br/>檔案路徑：" + filepath + "<br/>錯誤訊息：" + ex.ToString());
                return;
            }
            Response.ClearHeaders();
            Response.Clear();
            Response.Buffer = true;
            string fileName = System.IO.Path.GetFileName(filepath);
            //跳出視窗，讓用戶端選擇要儲存的地方                         //使用Server.UrlEncode()編碼中文字才不會下載時，檔名為亂碼
            Response.AddHeader("Content-Disposition", "Attachment;FileName=" + System.Web.HttpUtility.UrlEncode(fileName,System.Text.Encoding.UTF8));
            //設定MIME類型為二進位檔案
            Response.ContentType = "Application/xls";

            try
            {
                //檔案有各式各樣，所以用BinaryWrite
               Response.BinaryWrite(file);

            }
            catch (Exception ex)
            {
                Response.Write("檔案輸出有誤，您可以在瀏覽器的URL網址貼上以下路徑嘗試看看。<br/>檔案路徑：" + filepath + "<br/>錯誤訊息：" + ex.ToString());
                return;
            }

            //這是專門寫文字的
            //HttpContext.Current.Response.Write();
           Response.End();
        }
    }
}