using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models.SoaQuery;
using CHPOUTSRCMES.Web.Models.StockQuery;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.SoaQuery;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class SoaController : Controller
    {
        /// <summary>
        /// SOA傳輸記錄 View
        /// </summary>
        /// <returns></returns>
        // GET: /Soa/
        public ActionResult Index()
        {
            var model = new SoaQueryViewModel();
            model.ProcessCodeList = SoaQueryViewModel.getProcessCodeList();
            model.ErrorOptionList = SoaQueryViewModel.getErrorOptionList();
            model.Fields = new SoaQueryModel();
            return View(model);
        }

        /// <summary>
        /// SOA傳輸記錄明細
        /// </summary>
        /// <param name="subinventoryCode"></param>
        /// <param name="locatorId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        // GET: /Soa/Detail
        public ActionResult Detail(string processCode, string serverCode, string batchId)
        {
            var model = new SoaQueryDetailViewModel();
            model.HeaderField = SoaQueryModel.getModel(processCode, serverCode, batchId);
            model.TableFields = new SoaDetailQueryModel();

            if(model.HeaderField != null)
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 清除上傳資料註記
        /// </summary>
        /// <param name="data">DataTableAjaxPostViewModel</param>
        /// <param name="processCode">傳輸類型</param>
        /// <param name="processDate">傳輸日期</param>
        /// <param name="hasError">錯誤訊息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ClearSoaStage(string processCode, string serverCode, string batchId)
        {
            var userId = User.Identity.GetUserId();
            var model = SoaDetailQueryModel.ClearSoaStage(processCode, serverCode, batchId, userId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 傳輸記錄查詢
        /// </summary>
        /// <param name="data">DataTableAjaxPostViewModel</param>
        /// <param name="processCode">傳輸類型</param>
        /// <param name="processDate">傳輸日期</param>
        /// <param name="hasError">錯誤訊息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SoaQuery(DataTableAjaxPostViewModel data, 
            string processCode, string processDate, string hasError)
        {
            var models = SoaQueryModel.getModels(data, processCode, processDate, hasError);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 庫存明細查詢
        /// </summary>
        /// <param name="data">DataTableAjaxPostViewModel</param>
        /// <param name="processCode">倉庫代號</param>
        /// <param name="serverCode">儲位ID</param>
        /// <param name="batchId">料號ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SoaDetailQuery(DataTableAjaxPostViewModel data,
            string processCode, string serverCode, string batchId)
        {

            var userId = User.Identity.GetUserId();
            var models = SoaDetailQueryModel.getModels(data, processCode, serverCode, batchId);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

    }
}