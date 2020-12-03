using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;
using Microsoft.Reporting.WebForms;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using System.Drawing;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.Report
{
    public class YieldQueryModel
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        [Display(Name = "工單號")]
        public string BatchNo { set; get; }

        [Display(Name = "狀態")]
        public string Status { set; get; }

        [Display(Name = "組成成分料號")] //LINE_TYPE (I)
        public string DiItemNumber { set; get; }

        [Display(Name = "噸數")]
        public decimal DiQty { set; get; }

        [Display(Name = "產品料號")]  //LINE_TYPE  (P)
        public string DoItemNumber { set; get; }

        [Display(Name = "令數")]
        public decimal DoReQty { set; get; }

        [Display(Name = "噸數")]
        public decimal DoQty { set; get; }

        [Display(Name = "得率%")]
        public decimal Yield { set; get; }


        public DataTableJsonResultModel<YieldQueryModel> getModels(DataTableAjaxPostViewModel data,string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum, string userId)
        {
            var sqlParameterList = new List<SqlParameter>();
            using var mesContext = new MesContext();

            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();

            try
            {
                var dateFormStatus = DateTime.TryParseExact(cuttingDateFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateFrom);
                var dateToStatus = DateTime.TryParseExact(cuttingDateTo, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out dateTo);
                if (!dateFormStatus) dateFrom = new DateTime(1900, 1, 1);
                if (!dateToStatus) dateTo = new DateTime(9999, 12, 31);

                string prefixCmd = @"
SELECT h.BATCH_NO AS BatchNo,
dbo.GetOspStatusNameByCode(h.STATUS) AS Status,
iht.INVENTORY_ITEM_NUMBER AS DiItemNumber,
ROUND(ISNULL(yht.DETAIL_IN_QUANTITY, 0) / 1000, 5) AS DiQty,
oht.INVENTORY_ITEM_NUMBER AS DoItemNumber,
(SELECT ISNULL(SUM(SECONDARY_QUANTITY), 0) FROM OSP_PICKED_OUT_HT WHERE OSP_HEADER_ID = h.OSP_HEADER_ID) AS DoReQty,
ROUND(ISNULL(yht.DETAIL_OUT_QUANTITY, 0) /1000, 5) AS DoQty,
ROUND(yht.RATE, 2) AS Yield
FROM OSP_HEADER_T h
JOIN OSP_YIELD_VARIANCE_HT yht on h.OSP_HEADER_ID = yht.OSP_HEADER_ID
JOIN OSP_DETAIL_IN_HT iht on h.OSP_HEADER_ID = iht.OSP_HEADER_ID
JOIN OSP_DETAIL_OUT_HT oht on h.OSP_HEADER_ID = oht.OSP_HEADER_ID
JOIN USER_SUBINVENTORY_T us ON us.ORGANIZATION_ID = h.ORGANIZATION_ID AND us.SUBINVENTORY_CODE = iht.SUBINVENTORY AND us.SUBINVENTORY_CODE = oht.SUBINVENTORY
";
                List<string> cond = new List<string>();

                if (!string.IsNullOrEmpty(userId))
                {
                    cond.Add("us.UserId = @userId");
                    sqlParameterList.Add(SqlParamHelper.GetVarChar("@userId", userId, 128));
                }

                //cond.Add("(h.STATUS = '3' OR h.STATUS = '4')");


                if (dateFormStatus || dateToStatus)
                {
                    cond.Add("((h.CUTTING_DATE_FROM BETWEEN @CUTTING_DATE_FROM AND @CUTTING_DATE_TO) or (h.CUTTING_DATE_TO BETWEEN @CUTTING_DATE_FROM AND @CUTTING_DATE_TO))");
                    sqlParameterList.Add(SqlParamHelper.GetDataTime("@CUTTING_DATE_FROM", dateFrom));
                    sqlParameterList.Add(SqlParamHelper.GetDataTime("@CUTTING_DATE_TO", dateTo));
                }

                if (batchNo != "*")
                {
                    cond.Add("h.BATCH_NO = @BATCH_NO");
                    sqlParameterList.Add(new SqlParameter("@BATCH_NO", batchNo));
                }
                if (machineNum != "*")
                {
                    cond.Add("h.MACHINE_CODE = @MACHINE_CODE");
                    sqlParameterList.Add(new SqlParameter("@MACHINE_CODE", machineNum));
                }

                string commandText = $"{prefixCmd} WHERE {string.Join(" AND ", cond.ToArray())}";

                var model = mesContext.Database.SqlQuery<YieldQueryModel>(commandText, sqlParameterList.ToArray()).ToList();
                //var totalCount = model.Count;

                var list = Search(model, data.Search.Value);
                //var filteredCount = list.Count();

                list = Order(data.Order, list);

                list = list.Skip(data.Start).Take(data.Length);

                return new DataTableJsonResultModel<YieldQueryModel>(data.Draw, 0, list.ToList());
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }
            return new DataTableJsonResultModel<YieldQueryModel>(data.Draw, 0, new List<YieldQueryModel>());
        }


        public static IOrderedEnumerable<YieldQueryModel> Order(List<Order> orders, IEnumerable<YieldQueryModel> models)
        {
            IOrderedEnumerable<YieldQueryModel> orderedModel = null;

            for (int i = 0; i < orders.Count(); i++)
            {
                if (i == 0)
                {
                    orderedModel = OrderBy(orders[i].Column, orders[i].Dir, models);
                }
                else
                {
                    orderedModel = ThenBy(orders[i].Column, orders[i].Dir, orderedModel);
                }
            }
            return orderedModel;
        }

        private static IOrderedEnumerable<YieldQueryModel> OrderBy(int column, string dir, IEnumerable<YieldQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BatchNo) : models.OrderBy(x => x.BatchNo);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Status) : models.OrderBy(x => x.Status);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DiItemNumber) : models.OrderBy(x => x.DiItemNumber);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DiQty) : models.OrderBy(x => x.DiQty);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DoItemNumber) : models.OrderBy(x => x.DoItemNumber);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DoReQty) : models.OrderBy(x => x.DoReQty);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.DoQty) : models.OrderBy(x => x.DoQty);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Yield) : models.OrderBy(x => x.Yield);
            }
        }

        private static IOrderedEnumerable<YieldQueryModel> ThenBy(int column, string dir, IOrderedEnumerable<YieldQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BatchNo) : models.ThenBy(x => x.BatchNo);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Status) : models.ThenBy(x => x.Status);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DiItemNumber) : models.ThenBy(x => x.DiItemNumber);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DiQty) : models.ThenBy(x => x.DiQty);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DoItemNumber) : models.ThenBy(x => x.DoItemNumber);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DoReQty) : models.ThenBy(x => x.DoReQty);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.DoQty) : models.ThenBy(x => x.DoQty);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Yield) : models.ThenBy(x => x.Yield);
            }
        }

        private static IEnumerable<YieldQueryModel> Search(IEnumerable<YieldQueryModel> models, string search)
        {
            if (string.IsNullOrEmpty(search)) return models;

            return models.Where(x =>
                    (!string.IsNullOrEmpty(x.BatchNo) && x.BatchNo.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.Status) && x.Status.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.DiItemNumber) && x.DiItemNumber.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.DiQty.ToString()) && x.DiQty.Normalize().ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.DoItemNumber.ToLower()) && x.DoItemNumber.ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.DoReQty.ToString()) && x.DoReQty.Normalize().ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.DoQty.ToString()) && x.DoQty.Normalize().ToString().ToLower().Contains(search.ToLower()))
                    || (!string.IsNullOrEmpty(x.Yield.ToString()) && x.Yield.Normalize().ToString().ToLower().Contains(search.ToLower()))
                );
        }


        public ResultDataModel<ReportViewer> LocalOspYieldReportViewer(ProcessUOW uow, string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum, string itemNumber, string barcode, string subinventory, string userId)
        {
            try
            {
                var report = new ReportViewer();
                // Set the processing mode for the ReportViewer to Local  
                report.ProcessingMode = ProcessingMode.Local;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;

                LocalReport localReport = report.LocalReport;
                localReport.ReportPath = "Report/OspYield.rdlc";

                var reportDataSourceResult = uow.GetOspYieldReportDataSource(cuttingDateFrom, cuttingDateTo, batchNo, machineNum, itemNumber, barcode, subinventory, userId);
                if (!reportDataSourceResult.Success) return new ResultDataModel<ReportViewer>(false, reportDataSourceResult.Msg, null);
                localReport.DataSources.Add(reportDataSourceResult.Data);

                // Set the report parameters for the report  
                localReport.SetParameters(uow.GetOspYieldReportParameterList(cuttingDateFrom, cuttingDateTo, batchNo, machineNum, itemNumber, barcode, subinventory, userId));

                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得工單得率報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得工單得率報表失敗:" + ex.Message, null);
            }
        }


        public ResultDataModel<ReportViewer> RemoteOspYieldReportViewer(ProcessUOW uow, string cuttingDateFrom, string cuttingDateTo, string batchNo, string machineNum, string itemNumber, string barcode, string subinventory, string userId)
        {
            try
            {
                string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];
               

                var report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.Solid;
                report.ServerReport.ReportPath = KeyName + "/OspYield";
                report.ServerReport.ReportServerUrl = new Uri("http://rs.yfy.com/ReportServer");
                report.ServerReport.SetParameters(uow.GetOspYieldReportParameterList(cuttingDateFrom, cuttingDateTo, batchNo, machineNum, itemNumber, barcode, subinventory, userId));
                report.ServerReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得工單得率報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得工單得率報表失敗:" + ex.Message, null);
            }
        }

    }
}