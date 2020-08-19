using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Delivery;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels.Account;
using CHPOUTSRCMES.Web.ViewModels.Inventory;
using CHPOUTSRCMES.Web.ViewModels.Process;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        StockTransferBarcodeData stockTransferBarcodeData = new StockTransferBarcodeData();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }



        public ActionResult TestData()
        {
            return View("TestData");
        }

        [HttpPost]
        public ActionResult RecoverTestData()
        {
            FlatEditBarcodeData.resetData();
            FlatEditData.resetData();
            PaperRollEditBarcodeData.resetData();
            PaperRollEditData.resetData();
            TripHeaderData.resetData();
            AccountViewModel.RestData();
            StockTransferBarcodeData.resetData();
            StockTransferData.resetData();
            StockData.resetData();
            InventoryViewModel.ResetData();
            StockMiscellaneousData.resetData();
            StockObsoleteData.resetData();
            StockInventoryData.resetData();
            return new JsonResult { Data = new { status = true, result = "測試資料還原成功" } };
        }

        [HttpPost]
        public ActionResult GetLabel(List<string> BARCODE)
        //public ActionResult GetLabel(List<PurchaseDetailModel> model)
        {
            string msg = "";
            LabelData labelData = new LabelData();
            var lables = labelData.GetLabels(BARCODE);
            Util.PdfLableUtil pdf = new PdfLableUtil();
            string labelFullPath = pdf.GeneratePdfLabels2(lables, ref msg);
            if (string.IsNullOrEmpty(labelFullPath))
            {
                throw new Exception("產生PDF發生錯誤:" + msg);
            }

            var fileStream = new FileStream(labelFullPath,
                                 FileMode.Open,
                                 FileAccess.Read
                               );
            return new FileStreamResult(fileStream, "application/pdf");

        }

        [HttpPost]
        public ActionResult GetLabel2(List<string> BARCODE)
        //public ActionResult GetLabel(List<PurchaseDetailModel> model)
        {
            string msg = "";
            LabelData labelData = new LabelData();
            var lables = labelData.GetLabels2(BARCODE, stockTransferBarcodeData);
            Util.PdfLableUtil pdf = new PdfLableUtil();
            string labelFullPath = pdf.GeneratePdfLabels2(lables, ref msg);
            if (string.IsNullOrEmpty(labelFullPath))
            {
                throw new Exception("產生PDF發生錯誤:" + msg);
            }

            var fileStream = new FileStream(labelFullPath,
                                 FileMode.Open,
                                 FileAccess.Read
                               );
            return new FileStreamResult(fileStream, "application/pdf");

        }

        [HttpPost]
        public ActionResult GetLabels3(List<string> BARCODE)
        //public ActionResult GetLabel(List<PurchaseDetailModel> model)
        {
            string msg = "";
            LabelData labelData = new LabelData();
            var lables = labelData.GetLabels3(BARCODE);
            Util.PdfLableUtil pdf = new PdfLableUtil();
            string labelFullPath = pdf.GeneratePdfLabels2(lables, ref msg);
            if (string.IsNullOrEmpty(labelFullPath))
            {
                throw new Exception("產生PDF發生錯誤:" + msg);
            }

            var fileStream = new FileStream(labelFullPath,
                                 FileMode.Open,
                                 FileAccess.Read
                               );
            return new FileStreamResult(fileStream, "application/pdf");

        }

        public void DownloadFile()
        {
            //用戶端的物件
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] file = null;
            string filepath = Server.MapPath("../File/StockTransaction.zip");
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
    }
}