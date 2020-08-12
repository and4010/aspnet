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
using System.Net;
using Microsoft.Graph;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {

        //
        // GET: /Inbound/
        public ActionResult Index()
        {
            var model = new PurchaseViewModel()
            {
                Year = DateTime.Now.Year.ToString(),
                Month = DateTime.Now.Month.ToString("00")
                //Warehouse = "TB2"
            };
            ViewBag.MonthItems = model.GetMonths();
            ViewBag.YearItems = model.GetYears();
            ViewBag.SubinventoryItems = model.GetSubinventoryList();

            return View(model);
        }

       
        public JsonResult GetEvents(string id)
        {
            ///清除cache fullcalendar才能在執行
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var fullcalendar = purchaseViewModel.GetFullCalendarModel(id);
            return Json(fullcalendar.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReturnIndex(string CabinetNumber)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var resultModel = purchaseViewModel.ChageHeaderStatus(CabinetNumber);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }



        // GET: Purchase
        [HttpGet, ActionName("Detail")]
        public ActionResult Detail(string CONTAINER_NO = "", string Start = "", string Status = "", string Subinventory = "")
        {
            PurchaseViewModel model = new PurchaseViewModel();

            model.CreateDate = Start;
            model.CabinetNumber = CONTAINER_NO;
            model.Subinventory = Subinventory;
            model.Status = Status;
            return View(model);
        }


        [HttpPost, ActionName("RollHeader")]
        public JsonResult RollHeader(DataTableAjaxPostViewModel data, string CabinetNumber,string Status)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.RollModel> model;
            model = purchaseViewModel.GetRollHeader(CabinetNumber, Status);
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }




        [HttpPost, ActionName("RollBody")]
        public JsonResult RollBody(DataTableAjaxPostViewModel data, string status, string CabinetNumber)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            List<DetailModel.RollDetailModel> model = viewModel.GetPaperRollPickT(CabinetNumber,status);
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
        public JsonResult FlatHeader(DataTableAjaxPostViewModel data, string CabinetNumber, string Status)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.FlatModel> model;

            model = purchaseViewModel.GetFlatHeader(CabinetNumber, Status);

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }




        [HttpPost, ActionName("FlatBody")]
        public JsonResult FlatBody(DataTableAjaxPostViewModel data, string status, string CabinetNumber)
        {

            PurchaseViewModel viewModel = new PurchaseViewModel();
            List<DetailModel.FlatDetailModel> model = viewModel.GetFlatPickT(CabinetNumber, status);

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
        public JsonResult CheckCabinetNumber(string InputCabinetNumber, string ViewCabinetNumber)
        {
            var boolean = false;
            if (InputCabinetNumber == ViewCabinetNumber)
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

        /// <summary>
        /// 紙捲檢視畫面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CabinetNumber"></param>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public ActionResult RollView(string Id, string CabinetNumber, string CreateDate)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            model.CabinetNumber = CabinetNumber;
            model.CreateDate = CreateDate;
            model.Subinventory = model.GetPaperRollView(Id).Subinventory;
            model.RollDetailModel = model.GetPaperRollView(Id);


            return View(model);
        }

        /// <summary>
        /// 取得紙捲檢視畫面參數
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cabinetNumber"></param>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public ActionResult RollViewParameter(string id = "", string cabinetNumber = "", string CreateDate = "")
        {
            return Json(new { id, cabinetNumber, CreateDate }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 平張檢視畫面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public ActionResult FlatView(string id,string CabinetNumber)
        {

            

            PurchaseViewModel model = new PurchaseViewModel();

            //ViewBag.LocatorItems = model.GetLocator(Id);
            ViewBag.ReasonItems = model.GetReason();

            model.FlatDetailModel = model.GetFlatView(id);
            //model.CreateDate = "2020-06-08 10:00:00";
            model.CabinetNumber = CabinetNumber;
            //model.Subinventory = "TB2";


            return View(model);
        }

        /// <summary>
        /// 取得平張檢視畫面參數
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cabinetNumber"></param>
        /// <returns></returns>
        public ActionResult FlatViewParameter(string id = "", string cabinetNumber = "")
        {
            return Json(new { id, cabinetNumber }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 紙捲編輯畫面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CabinetNumber"></param>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public ActionResult RollEdit(string Id,string CabinetNumber,string CreateDate)
        {

            PurchaseViewModel model = new PurchaseViewModel();
        
            model.RollDetailModel = model.GetPaperRollEdit(Id);
            ViewBag.LocatorItems = model.GetLocator(Id);
            ViewBag.ReasonItems = model.GetReason();
            model.CabinetNumber = CabinetNumber;
            model.CreateDate = CreateDate;
            return View(model);
        }

        /// <summary>
        /// 取得紙捲編輯畫面參數
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cabinetNumber"></param>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RollEditParameter(string id = "", string cabinetNumber = "",string CreateDate ="")
        {
            return Json(new { id, cabinetNumber , CreateDate }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 平張編輯畫面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public ActionResult FlatEdit(string Id,string CabinetNumber)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            ViewBag.LocatorItems = model.GetLocator(Id);
            ViewBag.ReasonItems = model.GetReason();
            model.FlatDetailModel = model.GetFlatEdit(Id);

            //model.Status = purchaseViewModel.Status;
            //model.CreateDate = purchaseViewModel.CreateDate;
            model.CabinetNumber = CabinetNumber;
            //model.Subinventory = purchaseViewModel.Subinventory;

            return View(model);
        
        }

        /// <summary>
        /// 取得平張編輯畫面參數
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cabinetNumber"></param>
        /// <returns></returns>
        public JsonResult FlatEditParameter(string id = "",string cabinetNumber = "")
        {
            return Json(new { id, cabinetNumber },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RollEditSave(FormCollection formCollection)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var Files = Request.Files;
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            //if(Files != null || Files.Count != 0)
            //{
            //    foreach (string i in Files)
            //    {
            //        HttpPostedFileBase hpf = Files[i] as HttpPostedFileBase;
            //        model.SavePhoto(hpf);
            //    }
            //}
            var resultModel = model.PaperRollEditNote(Files, 
                Int64.Parse(formCollection["id"]),
                formCollection["Reason"], 
                formCollection["Locator"], 
                formCollection["Remark"],
                id,
                name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FlatEditSave(FormCollection formCollection)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var Files = Request.Files;
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            //if (Files != null || Files.Count != 0)
            //{
            //    foreach (string i in Files)
            //    {
            //        HttpPostedFileBase hpf = Files[i] as HttpPostedFileBase;
            //        //model.SavePhoto(hpf);
            //    }
            //}

            //model.GetFlatEditRemak(int.Parse(formCollection["id"]),
            //   formCollection["reason"], formCollection["remak"]);
            var resultModel = model.FlatEditNote(Files,
                Int64.Parse(formCollection["id"]),
                formCollection["Reason"],
                formCollection["Locator"],
                formCollection["Remark"],
                id,
                name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RollSaveBarcode(string Barcode)
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var status = purchaseViewModel.SavePaperRollBarcode(Barcode,id,name);
            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult FlatSaveBarcode(string Barcode)
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var status = purchaseViewModel.SaveFlatBarcode(Barcode,id,name);
            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UploadFileRoll(HttpPostedFileBase file, DataTableAjaxPostViewModel data, FormCollection formCollection)
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
                        ExcelImportRoll(file, ref detail, ref result, formCollection["CabinetNumber"]);
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
        public JsonResult ExcelImportRoll(HttpPostedFileBase file, ref List<DetailModel.RollDetailModel> PaperRollModel, ref ResultModel result, string CONTAINER_NO)
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var Excel = new ExcelImport();
            Excel.PaperRollDetail(file, ref PaperRollModel, CONTAINER_NO, ref result,id,name);
            return Json(new { data = PaperRollModel, result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcelDelete(string CabinetNumber)
        {
            PurchaseViewModel purchaseView = new PurchaseViewModel();
            purchaseView.ExcelDelete(CabinetNumber);
            return Json(JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 頁籤數字
        /// </summary>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaperNumber(string CabinetNumber)
        {
            
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var PaperTotle = viewModel.GetPaperRollNumberTab(CabinetNumber);
            return Json(new { PaperTotle }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 頁籤數字
        /// </summary>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FlatNumber(string CabinetNumber)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var FlatTotle = viewModel.GetFlatNumberTab(CabinetNumber);
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

        /// <summary>
        /// 列印平張標籤
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintFlatLabel(List<long> Id,string Status)
        {
            //取得使用者帳號
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var name = this.User.Identity.GetUserName();
            return viewModel.PritFlatLabel(Id, name, Status);
            //return new JsonResult { Data = new { status = false, result = "" } };
        }

        /// <summary>
        /// 列印紙捲標籤
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintPaperRollLabel(List<long> Id, string Status)
        {
            //取得使用者帳號
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var name = this.User.Identity.GetUserName();
            return viewModel.PritPaperRollLabel(Id, name, Status);
            //return new JsonResult { Data = new { status = false, result = "" } };
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
            Response.AddHeader("Content-Disposition", "Attachment;FileName=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
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

        [HttpPost]
        public JsonResult GetPhoto(int id)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            //Image oImage = null;
            //Bitmap oBitmap = null;
            ////建立副本
            try
            {
                var ListBytePhoto = viewModel.GetPhoto(id);

                if (ListBytePhoto == null || ListBytePhoto.Count == 0)
                {
                    return Json(new {  }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ListBytePhoto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new {  }, JsonRequestBehavior.AllowGet);
            }
           
        }

    }
}