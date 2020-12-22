using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using NLog;
using CHPOUTSRCMES.Web.Util;

namespace CHPOUTSRCMES.Web.Models.Report
{
    public class CutSumQueryModel
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public ResultDataModel<ReportViewer> LocalOspCutSumReportViewer(ProcessUOW uow, string planStartDateFrom, string planStartDateTo, string batchNo, string paperType, string userId)
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
                localReport.ReportPath = "Report/OspCutSum.rdlc";

                var reportDataSourceResult = uow.GetOspCutSumReportDataSource(planStartDateFrom, planStartDateTo, batchNo, paperType, userId);
                if (!reportDataSourceResult.Success) return new ResultDataModel<ReportViewer>(false, reportDataSourceResult.Msg, null);
                localReport.DataSources.Add(reportDataSourceResult.Data);

                // Set the report parameters for the report  
                localReport.SetParameters(uow.GetOspCutSumReportParameterList(planStartDateFrom, planStartDateTo, batchNo, paperType, userId));

                report.LocalReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得裁切資料匯總報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得裁切資料匯總報表失敗:" + ex.Message, null);
            }
        }


        public ResultDataModel<ReportViewer> RemoteOspCutSumReportViewer(ProcessUOW uow, string planStartDateFrom, string planStartDateTo, string batchNo, string paperType, string userId)
        {
            try
            {
                string KeyName = System.Web.Configuration.WebConfigurationManager.AppSettings["reportServerUrl"];


                var report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;
                report.BackColor = Color.LightGray;
                report.SizeToReportContent = true;
                report.AsyncRendering = false; 
                report.BorderWidth = 1;
                report.BorderStyle = BorderStyle.None;
                report.ServerReport.ReportPath = KeyName + "/OspCutSum";
                report.ServerReport.ReportServerUrl = new Uri("https://rs.yfy.com/ReportServer");
                report.ServerReport.SetParameters(uow.GetOspCutSumReportParameterList(planStartDateFrom, planStartDateTo, batchNo, paperType, userId));
                report.ServerReport.Refresh();

                return new ResultDataModel<ReportViewer>(true, "取得裁切資料匯總報表成功", report);
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
                return new ResultDataModel<ReportViewer>(false, "取得裁切資料匯總報表失敗:" + ex.Message, null);
            }
        }
    }
}