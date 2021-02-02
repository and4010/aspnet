using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.Models.Delivery;
using CHPOUTSRCMES.Web.Models.Stock;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using CHPOUTSRCMES.Web.ViewModels.Account;
using CHPOUTSRCMES.Web.ViewModels.Process;
using CHPOUTSRCMES.Web.ViewModels.Purchase;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public const string Flat = "平版";

        public const string PaperRoller = "捲筒";

        StockTransferBarcodeData stockTransferBarcodeData = new StockTransferBarcodeData();

        /// <summary>
        /// 登入後的首頁View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var id = this.User.Identity.GetUserId();
            using var mesContext = new MesContext();
            using var processUow = new ProcessUOW(mesContext);
            using var purchaseUow = new PurchaseUOW(mesContext);
            using var deliveryUow = new DeliveryUOW(mesContext);

            HomeViewModel viewModel = new HomeViewModel();
            var model1 = purchaseUow.GetCtrPendingCount(id);
            var model2 = deliveryUow.GetDlvPendingCount(id);
            var model3 = processUow.GetOspPendingCount(id);
            viewModel.CtrPendingCount = model1.Data;
            viewModel.DlvPendingCount = model2.Data;
            viewModel.OspPendingCount = model3.Data;

            return View(viewModel);
        }

        
        /// <summary>
        /// 入庫單
        /// </summary>
        /// <param name="CtrHeaderId"></param>
        /// <param name="ItemCategory"></param>
        /// <returns></returns>
        public ActionResult CtrReport(string CtrHeaderId, string ItemCategory)
        {
#if DEBUG
            return LocalReport(CtrHeaderId, ItemCategory);
#else
           return RemoteReport(CtrHeaderId, ItemCategory);
#endif

        }

        public ActionResult LocalReport(string CtrHeaderId, string ItemCategory)
        {
            using (var context = new MesContext())
            {
                using (PurchaseUOW uow = new PurchaseUOW(context))
                {
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("CTR_HEADER_ID", CtrHeaderId, false));
                    paramList.Add(new ReportParameter("ITEM_CATEGORY", ItemCategory, false));
                    var report = new ReportViewer();

                    // Set the processing mode for the ReportViewer to Local  
                    report.ProcessingMode = ProcessingMode.Local;
                    report.BackColor = Color.LightGray;
                    report.SizeToReportContent = true;
                    report.BorderWidth = 1;
                    report.BorderStyle = BorderStyle.Solid;
                    LocalReport localReport = report.LocalReport;
                    if(ItemCategory == Flat)
                    {
                        localReport.ReportPath = "Report/PurchaseFlat.rdlc";
                    }
                    else
                    {
                        localReport.ReportPath = "Report/PurchasePaperRoll.rdlc";
                    }
                   
                    ReportDataSource Header = new ReportDataSource();
                    ReportDataSource Detail = new ReportDataSource();
                    ReportDataSource Reason = new ReportDataSource();
                    uow.PurchaseReceipt(ref Header, ref Detail, ref Reason, CtrHeaderId, ItemCategory);
                    localReport.DataSources.Add(Header);
                    localReport.DataSources.Add(Detail);
                    localReport.DataSources.Add(Reason);
                    // Set the report parameters for the report  
                    localReport.SetParameters(paramList);

                    report.LocalReport.Refresh();

                    ViewBag.ReportViewer = report;
                }
            }
          
            return View("Report");
        }

        public ActionResult RemoteReport(string CtrHeaderId, string ItemCategory)
        {
            string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("CTR_HEADER_ID", CtrHeaderId, false));
            paramList.Add(new ReportParameter("ITEM_CATEGORY", ItemCategory, false));
            var report = new ReportViewer();
            report.ProcessingMode = ProcessingMode.Remote;
            report.SizeToReportContent = true;
            report.BorderStyle = BorderStyle.Solid;
            report.BorderWidth = 1;
            report.BackColor = Color.LightGray;
            if (ItemCategory == Flat)
            {
                report.ServerReport.ReportPath = KeyName + "/PurchaseFlat";
            }
            else
            {
                report.ServerReport.ReportPath = KeyName + "/PurchasePaperRoll";
            }
            report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
            report.ServerReport.SetParameters(paramList);
            report.ServerReport.Refresh();
            ViewBag.ReportViewer = report;
            return View("Report");
        }

        /// <summary>
        /// 加工領料單
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ActionResult OspReport(string OspHeaderId)
        {
#if DEBUG
            return OspLocalReport(OspHeaderId);
#else
            return OspRemoteReport(OspHeaderId);
#endif
        }

        public ActionResult OspLocalReport(string OspHeaderId)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId,false));
                    var report = new ReportViewer();

                    // Set the processing mode for the ReportViewer to Local  
                    report.ProcessingMode = ProcessingMode.Local;
                    report.BackColor = Color.LightGray;
                    report.SizeToReportContent = true;
                    report.BorderWidth = 1;
                    report.BorderStyle = BorderStyle.Solid;
                    LocalReport localReport = report.LocalReport;
                    localReport.ReportPath = "Report/ProcessCutMaterial.rdlc";

                    ReportDataSource CutMaterial = new ReportDataSource();
                    uow.OspMaterial(ref CutMaterial, OspHeaderId);
                    localReport.DataSources.Add(CutMaterial);
                    // Set the report parameters for the report  
                    localReport.SetParameters(paramList);

                    report.LocalReport.Refresh();

                    ViewBag.ReportViewer = report;
                }
            }

            return View("Report");
        }

        public ActionResult OspRemoteReport(string OspHeaderId)
        {
            string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
            var report = new ReportViewer();
            report.ProcessingMode = ProcessingMode.Remote;
            report.SizeToReportContent = true;
            report.BorderStyle = BorderStyle.Solid;
            report.BorderWidth = 1;
            report.BackColor = Color.LightGray;
            report.ServerReport.ReportPath = KeyName + "/ProcessCutMaterial";
            report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
            report.ServerReport.SetParameters(paramList);
            report.ServerReport.Refresh();
            report.ServerReport.Timeout = 120;
            report.KeepSessionAlive = true;
            ViewBag.ReportViewer = report;
            return View("Report");
        }

        /// <summary>
        /// 加工裁切單
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>

        public ActionResult OspCutReceiptReport(string OspHeaderId)
        {
#if DEBUG
            return OspLocalCutReceiptReport(OspHeaderId);
#else
            return OspRemoteCutReceiptReport(OspHeaderId);
#endif
        }

        public ActionResult OspLocalCutReceiptReport(string OspHeaderId)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
                    paramList.Add(new ReportParameter("SPECIFICATION", OspHeaderId, false));
                    paramList.Add(new ReportParameter("packingType", OspHeaderId, false));
                    var report = new ReportViewer();

                    // Set the processing mode for the ReportViewer to Local  
                    report.ProcessingMode = ProcessingMode.Local;
                    report.BackColor = Color.LightGray;
                    report.SizeToReportContent = true;
                    report.BorderWidth = 1;
                    report.BorderStyle = BorderStyle.Solid;
                    LocalReport localReport = report.LocalReport;
                    localReport.ReportPath = "Report/ProcessCutReceipt.rdlc";


                    ReportDataSource dsDetail = new ReportDataSource();
                    ReportDataSource LabelKnife = new ReportDataSource();
                    ReportDataSource LabelDesc = new ReportDataSource();
                    ReportDataSource LabelSize = new ReportDataSource();
                    uow.OspCutReceiptReport(ref dsDetail, ref LabelKnife, ref LabelDesc, ref LabelSize, OspHeaderId);
                    localReport.DataSources.Add(dsDetail);
                    localReport.DataSources.Add(LabelKnife);
                    localReport.DataSources.Add(LabelDesc);
                    localReport.DataSources.Add(LabelSize);
                    // Set the report parameters for the report  
                    localReport.SetParameters(paramList);

                    report.LocalReport.Refresh();

                    ViewBag.ReportViewer = report;
                }
            }

            return View("Report");
        }

        public ActionResult OspRemoteCutReceiptReport(string OspHeaderId)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
                    if (!long.TryParse(OspHeaderId, out long ospHeaderId))
                    {
                        throw new ArgumentException("HEADER ID ERROR");
                    }
                    var osp = uow.GetDetailOut(ospHeaderId);
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
                    paramList.Add(new ReportParameter("SPECIFICATION", osp.Specification, false));
                    paramList.Add(new ReportParameter("packingType", osp.PackingType, false));
                    var report = new ReportViewer();
                    report.ProcessingMode = ProcessingMode.Remote;
                    report.SizeToReportContent = true;
                    report.BorderStyle = BorderStyle.Solid;
                    report.BorderWidth = 1;
                    report.BackColor = Color.LightGray;
                    report.ServerReport.ReportPath = KeyName + "/ProcessCutReceipt";
                    report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
                    report.ServerReport.SetParameters(paramList);
                    report.ServerReport.Refresh();
                    ViewBag.ReportViewer = report;
                }
            }
            return View("Report");
        }

        /// <summary>
        /// 加工成品入庫
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ActionResult OspStock(string OspHeaderId)
        {
#if DEBUG
            return OspLocalStockReport(OspHeaderId);
#else
            return OspRemoteStockReport(OspHeaderId);
#endif
        }

        public ActionResult OspLocalStockReport(string OspHeaderId)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
                    var report = new ReportViewer();

                    // Set the processing mode for the ReportViewer to Local  
                    report.ProcessingMode = ProcessingMode.Local;
                    report.BackColor = Color.LightGray;
                    report.SizeToReportContent = true;
                    report.BorderWidth = 1;
                    report.BorderStyle = BorderStyle.Solid;
                    LocalReport localReport = report.LocalReport;
                    localReport.ReportPath = "Report/CutStock.rdlc";

                    ReportDataSource stock = new ReportDataSource();
                    ReportDataSource Countangent = new ReportDataSource();
                    ReportDataSource Remain = new ReportDataSource();
                    uow.OspStock(ref stock, ref Countangent,ref Remain, OspHeaderId);
                    localReport.DataSources.Add(stock);
                    localReport.DataSources.Add(Countangent);
                    localReport.DataSources.Add(Remain);
                    // Set the report parameters for the report  
                    localReport.SetParameters(paramList);

                    report.LocalReport.Refresh();

                    ViewBag.ReportViewer = report;
                }
            }

            return View("Report");
        }

        public ActionResult OspRemoteStockReport(string OspHeaderId)
        {
            string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
            var report = new ReportViewer();
            report.ProcessingMode = ProcessingMode.Remote;
            report.SizeToReportContent = true;
            report.BorderStyle = BorderStyle.Solid;
            report.BorderWidth = 1;
            report.BackColor = Color.LightGray;
            report.ServerReport.ReportPath = KeyName + "/CutStock";
            report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
            report.ServerReport.SetParameters(paramList);
            report.ServerReport.Refresh();
            ViewBag.ReportViewer = report;
            return View("Report");
        }

        /// <summary>
        /// 加工紙捲成品入庫
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ActionResult OspPaperRollerStock(string OspHeaderId)
        {
#if DEBUG
            return OspLocalPaperRollerStockReport(OspHeaderId);
#else
            return OspRemotePaperRollerStockReport(OspHeaderId);
#endif
        }

        public ActionResult OspLocalPaperRollerStockReport(string OspHeaderId)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
                    var report = new ReportViewer();

                    // Set the processing mode for the ReportViewer to Local  
                    report.ProcessingMode = ProcessingMode.Local;
                    report.BackColor = Color.LightGray;
                    report.SizeToReportContent = true;
                    report.BorderWidth = 1;
                    report.BorderStyle = BorderStyle.Solid;
                    LocalReport localReport = report.LocalReport;
                    localReport.ReportPath = "Report/OspCutPaperRollStock.rdlc";

                    ReportDataSource stock = new ReportDataSource();
                    uow.OspPaperRollerStock(ref stock, OspHeaderId);
                    localReport.DataSources.Add(stock);
                    // Set the report parameters for the report  
                    localReport.SetParameters(paramList);

                    report.ServerReport.Refresh();

                    ViewBag.ReportViewer = report;
                }
            }

            return View("Report");
        }

        public ActionResult OspRemotePaperRollerStockReport(string OspHeaderId)
        {
            string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
            var report = new ReportViewer();
            report.ProcessingMode = ProcessingMode.Remote;
            report.SizeToReportContent = true;
            report.BorderStyle = BorderStyle.Solid;
            report.BorderWidth = 1;
            report.BackColor = Color.LightGray;
            report.ServerReport.ReportPath = KeyName + "/OspCutPaperRollStock";
            report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
            report.ServerReport.SetParameters(paramList);
            report.ServerReport.Refresh();
            ViewBag.ReportViewer = report;
            return View("Report");
        }

        /// <summary>
        /// 加工平張成品入庫
        /// </summary>
        /// <param name="OspHeaderId"></param>
        /// <returns></returns>
        public ActionResult OspFlatStock(string OspHeaderId)
        {
#if DEBUG
            return OspLocalPaperRollerStockReport(OspHeaderId);
#else
            return OspRemotePaperRollerStockReport(OspHeaderId);
#endif
        }

        public ActionResult OspLocalFlatStockReport(string OspHeaderId)
        {
            using (var context = new MesContext())
            {
                using (ProcessUOW uow = new ProcessUOW(context))
                {
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
                    var report = new ReportViewer();

                    // Set the processing mode for the ReportViewer to Local  
                    report.ProcessingMode = ProcessingMode.Local;
                    report.BackColor = Color.LightGray;
                    report.SizeToReportContent = true;
                    report.BorderWidth = 1;
                    report.BorderStyle = BorderStyle.Solid;
                    LocalReport localReport = report.LocalReport;
                    localReport.ReportPath = "Report/OspReplaceFlatStock.rdlc";

                    ReportDataSource stock = new ReportDataSource();
                    uow.OspPaperRollerStock(ref stock, OspHeaderId);
                    localReport.DataSources.Add(stock);
                    // Set the report parameters for the report  
                    localReport.SetParameters(paramList);

                    report.ServerReport.Refresh();

                    ViewBag.ReportViewer = report;
                }
            }

            return View("Report");
        }

        public ActionResult OspRemoteFlatStockReport(string OspHeaderId)
        {
            string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("OSP_HEADER_ID", OspHeaderId, false));
            var report = new ReportViewer();
            report.ProcessingMode = ProcessingMode.Remote;
            report.SizeToReportContent = true;
            report.BorderStyle = BorderStyle.Solid;
            report.BorderWidth = 1;
            report.BackColor = Color.LightGray;
            report.ServerReport.ReportPath = KeyName + "/OspReplaceFlatStock";
            report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
            report.ServerReport.SetParameters(paramList);
            report.ServerReport.Refresh();
            ViewBag.ReportViewer = report;
            return View("Report");
        }

        //[HttpPost]
        //public JsonResult GetOspCount()
        //{
            
        //    using var mesContext = new MesContext();
        //    using var processUow = new ProcessUOW(mesContext);

        //    var model = processUow.GetOspPendingCount();

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult GetDlvCount()
        //{
        //    using var mesContext = new MesContext();
        //    using var deliveryUow = new DeliveryUOW(mesContext);

        //    var model = deliveryUow.GetDlvPendingCount();

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult GetCtrCount()
        //{
        //    using var mesContext = new MesContext();
        //    using var purchaseUOW = new PurchaseUOW(mesContext);

        //    var model = purchaseUOW.GetCtrPendingCount();

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult massPrint(long id)
        //{
        //    string paperType = "";

        //    switch (id)
        //    {
        //        case 1:
        //            paperType = "平版";
        //            break;
        //        default:
        //            paperType = "捲筒";

        //            break;
        //    }

        //    var name = this.User.Identity.GetUserName();
        //    using var mesContext = new MesContext();
        //    using var transferUow = new TransferUOW(mesContext);
        //    return transferUow.PrintMassLabel(transferUow, id, paperType, name);
        //}

    }
}