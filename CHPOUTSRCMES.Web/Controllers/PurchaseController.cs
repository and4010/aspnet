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
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CHPOUTSRCMES.Web.ActionFilter;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {

        /// <summary>
        /// 入庫-月曆首頁 View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new PurchaseViewModel()
            {
                Year = DateTime.Now.Year.ToString(),
                Month = DateTime.Now.Month.ToString("00")
                //Warehouse = "TB2"
            };
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            ViewBag.MonthItems = model.GetMonths();
            ViewBag.YearItems = model.GetYears();
            ViewBag.SubinventoryItems = model.GetSubinventoryList(id,"");
            ViewBag.StatusItems = model.GetStatus();
            return View(model);
        }

        /// <summary>
        /// 查詢櫃號
        /// </summary>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public JsonResult SearchCabinetNumber(string CabinetNumber)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var resultModel = purchaseViewModel.SearchCabinetNumber(CabinetNumber);
            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 取得入庫月曆資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult GetEvents(string id, string status)
        {
            ///清除cache fullcalendar才能在執行
            
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var fullcalendar = purchaseViewModel.GetFullCalendarModel(id, status);
            return Json(fullcalendar.ToArray(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 入庫存檔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReturnIndex(long id)
        {
            //取得使用者ID
            var userId = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var resultModel = purchaseViewModel.ChageHeaderStatus(id, userId, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 取得進櫃明細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Purchase
        [HttpGet, ActionName("Detail")]
        public ActionResult Detail(long id)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var Header = model.GetHeader(id);
            model.CtrHeaderId = id;
            model.CreateDate = Header.MvContainerDate; 
            model.CabinetNumber = Header.ContainerNo;
            model.Subinventory = Header.Subinventory;
            model.Status = Header.Status;
            return View(model);
        }

        /// <summary>
        /// 紙捲表頭表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("RollHeader")]
        public JsonResult RollHeader(DataTableAjaxPostViewModel data, long id)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.RollModel> model;
            model = purchaseViewModel.GetRollHeader(id);
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }



        /// <summary>
        /// 紙捲表身表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("RollBody")]
        public JsonResult RollBody(DataTableAjaxPostViewModel data, long id)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            List<DetailModel.RollDetailModel> model = viewModel.GetPaperRollPickT(id);
            model = PurchaseViewModel.RollDetailModelDTOrder.Search(data, model);
            model = PurchaseViewModel.RollDetailModelDTOrder.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }


        /// <summary>
        /// 平張表頭表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("FlatHeader")]
        public JsonResult FlatHeader(DataTableAjaxPostViewModel data, long id)
        {
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            List<DetailModel.FlatModel> model;

            model = purchaseViewModel.GetFlatHeader(id);

            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = model }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }



        /// <summary>
        /// 平張表身表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("FlatBody")]
        public JsonResult FlatBody(DataTableAjaxPostViewModel data,long id)
        {

            PurchaseViewModel viewModel = new PurchaseViewModel();
            List<DetailModel.FlatDetailModel> model = viewModel.GetFlatPickT(id);

            model = PurchaseViewModel.FlatDetailModelDTOrder.Search(data, model);
            model = PurchaseViewModel.FlatDetailModelDTOrder.Order(data.Order, model).ToList();
            var data1 = model.Skip(data.Start).Take(data.Length).ToList();
            return Json(new { draw = data.Draw, recordsFiltered = model.Count, recordsTotal = model.Count, data = data1 }, JsonRequestBehavior.AllowGet);
            //draw：為避免XSS攻擊，內建的控制。 
            //recordsTotal：篩選前的總資料數 
            //recordsFiltered：篩選後的總資料數(jQuery DataTable內建的篩選，因本案例沒用到，與recordsTotal相同
            //data：該分頁所需要的資料。
        }

        /// <summary>
        /// 比對櫃號
        /// </summary>
        /// <param name="InputCabinetNumber"></param>
        /// <param name="ViewCabinetNumber"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 紙捲匯入View
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ImportBodyRoll()
        {
            return PartialView();
        }

        /// <summary>
        /// 平張匯入View
        /// </summary>
        /// <returns></returns>
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
        public ActionResult RollView(long CtrPickedId, long CtrHeaderId)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var herader = model.GetHeader(CtrHeaderId);
            model.CabinetNumber = herader.ContainerNo;
            model.CreateDate = herader.MvContainerDate;
            model.Subinventory = model.GetPaperRollView(CtrPickedId).Subinventory;
            model.RollDetailModel = model.GetPaperRollView(CtrPickedId);


            return View(model);
        }

        /// <summary>
        /// 平張檢視畫面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public ActionResult FlatView(long CtrPickedId, long CtrHeaderId)
        {

            PurchaseViewModel model = new PurchaseViewModel();

            //ViewBag.LocatorItems = model.GetLocator(Id);
            ViewBag.ReasonItems = model.GetReason();
            model.FlatDetailModel = model.GetFlatView(CtrPickedId);
            //model.CreateDate = "2020-06-08 10:00:00";
            model.CabinetNumber = model.GetHeader(CtrHeaderId).ContainerNo;
            //model.Subinventory = "TB2";


            return View(model);
        }

        /// <summary>
        /// 紙捲編輯畫面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CabinetNumber"></param>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public ActionResult RollEdit(long CtrPickedId, long CtrHeaderId)
        {

            PurchaseViewModel model = new PurchaseViewModel();
            var header = model.GetHeader(CtrHeaderId);
            model.RollDetailModel = model.GetPaperRollEdit(CtrPickedId);
            ViewBag.LocatorItems = model.GetLocator(CtrPickedId);
            ViewBag.ReasonItems = model.GetReason();
            model.CabinetNumber = header.ContainerNo;
            model.CreateDate = header.MvContainerDate;
            return View(model);
        }

        /// <summary>
        /// 平張編輯畫面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CabinetNumber"></param>
        /// <returns></returns>
        public ActionResult FlatEdit(long CtrPickedId, long CtrHeaderId)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            ViewBag.LocatorItems = model.GetLocator(CtrPickedId);
            ViewBag.ReasonItems = model.GetReason();
            model.FlatDetailModel = model.GetFlatEdit(CtrPickedId);
            model.CabinetNumber = model.GetHeader(CtrHeaderId).ContainerNo;
            return View(model);
        }

        /// <summary>
        /// 紙捲編輯存檔
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RollEditSave(FormCollection formCollection)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var Files = Request.Files;
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var resultModel = model.PaperRollEditNote(Files, 
                Int64.Parse(formCollection["id"]),
                formCollection["Reason"], 
                formCollection["Locator"], 
                formCollection["Remark"],
                id,
                name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 平張編輯存檔
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FlatEditSave(FormCollection formCollection)
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var Files = Request.Files;
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            
            var resultModel = model.FlatEditNote(Files,
                Int64.Parse(formCollection["id"]),
                formCollection["Reason"],
                formCollection["Locator"],
                formCollection["Remark"],
                id,
                name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 紙捲轉已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RollSaveBarcode(string Barcode,long id)
        {
            //取得使用者ID
            var userId = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var resultModel = purchaseViewModel.SavePaperRollBarcode(Barcode, id, userId, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 平張轉已入庫
        /// </summary>
        /// <param name="Barcode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FlatSaveBarcode(string Barcode, long id)
        {
            //取得使用者ID
            var userId = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            var resultModel = purchaseViewModel.SaveFlatBarcode(Barcode,id, userId, name);
            return Json(new { resultModel }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 紙捲Excel匯入資料預覽表格
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <param name="formCollection"></param>
        /// <returns></returns>
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
                    try
                    {
                        ExcelImportRoll(file, ref detail, ref result, long.Parse(formCollection["id"]));
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
        /// 紙捲Excel匯入資料預覽表格
        /// </summary>
        /// <param name="file"></param>
        /// <param name="PaperRollModel"></param>
        /// <param name="result"></param>
        /// <param name="CtrHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcelImportRoll(HttpPostedFileBase file, ref List<DetailModel.RollDetailModel> PaperRollModel, ref ResultModel result, long CtrHeaderId)
        {
            //取得使用者ID
            var id = this.User.Identity.GetUserId();
            //取得使用者帳號
            var name = this.User.Identity.GetUserName();
            var Excel = new ExcelImport();
            Excel.PaperRollDetail(file, ref PaperRollModel, CtrHeaderId, ref result,id,name);
            return Json(new { data = PaperRollModel, result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消Excell匯入
        /// </summary>
        /// <param name="CtrHeaderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcelDelete(long CtrHeaderId)
        {
            PurchaseViewModel purchaseView = new PurchaseViewModel();
            purchaseView.ExcelDelete(CtrHeaderId);
            return Json(JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 頁籤紙捲數字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaperNumber(long id)
        {
            
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var PaperTotle = viewModel.GetPaperRollNumberTab(id);
            return Json(new { PaperTotle }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 頁籤紙捲已入庫數字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaperNumberIn(long id)
        {

            PurchaseViewModel viewModel = new PurchaseViewModel();
            var PaperTotleIn = viewModel.GetPaperRollNumberInTab(id);
            return Json(new { PaperTotleIn }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 頁籤平板數字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FlatNumber(long id)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var FlatTotle = viewModel.GetFlatNumberTab(id);
            return Json(new { FlatTotle }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 頁籤平板已入庫數字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FlatInNumberIn(long id)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            var FlatTotleIn = viewModel.GetFlatNumberInTab(id);
            return Json(new { FlatTotleIn }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 平張Excel匯入資料預覽表格
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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
            var userId = this.User.Identity.GetUserId();
            return viewModel.PritFlatLabel(Id, userId, name, Status);
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
            var userId = this.User.Identity.GetUserId();
            return viewModel.PritPaperRollLabel(Id, userId, name, Status);
        }

        /// <summary>
        /// 平張Excel匯入資料預覽表格
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcelImportFlat(HttpPostedFileBase file, DataTableAjaxPostViewModel data, ref List<DetailModel.FlatDetailModel> detail, ref ResultModel result)
        {
            var papper = new ExcelImport();
            return Json(new { draw = data.Draw, recordsFiltered = detail.Count, recordsTotal = detail.Count, data = detail, result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Excell範例檔案下載
        /// </summary>
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
            Response.End();
        }

        /// <summary>
        /// 取得照片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPhotos(long id)
        {
            PurchaseViewModel viewModel = new PurchaseViewModel();
            ////建立副本
            try
            {
                var ListBytePhoto = viewModel.GetPhotos(id);

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

        /// <summary>
        /// 取得照片清單ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPhotoList(long id)
        {
            int code = 0;
            string message = "";
            PurchaseViewModel viewModel = new PurchaseViewModel();
            ////建立副本
            List<long> list = new List<long>();
            try
            {
                list = viewModel.GetPhotoList(id);
            }
            catch (Exception ex)
            {
                code = -1;
                message = ex.Message;
            }
            return Json(new { Result = list, Code = code, Msg = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得照片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetPhoto(long id)
        {
            int code = 0;
            string message = "";
            PurchaseViewModel viewModel = new PurchaseViewModel();
            string photo = "";
            try
            {
                photo = viewModel.GetPhoto(id);
            }
            catch (Exception ex)
            {
                code = -1;
                message = ex.Message;
            }
            return Json(new { Result = photo, Code = code, Msg = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得編輯頁面的異常原因和儲位的Value
        /// </summary>
        /// <param name="PickId"></param>
        /// <returns></returns>
        public JsonResult SetSpinnerValue(long PickId)
        {
            PurchaseViewModel ViewModel = new PurchaseViewModel();
            var resultDataModel = ViewModel.SetSpinnerValue(PickId);
            return Json(new { resultDataModel }, JsonRequestBehavior.AllowGet);
        }

        




    }
}