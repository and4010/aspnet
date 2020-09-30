using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.Util;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CHPOUTSRCMES.Web.Models.SoaQuery
{
    public class SoaQueryModel
    {
        [Display(Name = "項次")]
        public long Id { set; get; }

        [Display(Name = "傳輸代號")]
        public string ProcessCode { set; get; }

        [Display(Name = "傳輸類型")]
        public string ProcessName { set; get; }

        [Display(Name = "傳輸日期")]
        public DateTime ProcessDate { set; get; }

        [Display(Name = "SERVER CODE")]
        public string ServerCode { set; get; }

        [Display(Name = "傳輸ID")]
        public string BatchId { set; get; }

        [Display(Name = "傳輸筆數")]

        public int RowNum { set; get; }

        [Display(Name = "狀態")]
        public string StatusCode { set; get; }

        [Display(Name = "錯誤訊息")]
        public string ErrorMsg { set; get; }

        [Display(Name = "SOA傳輸狀態")]
        public string SoaPullingFlag { set; get; }

        [Display(Name = "SOA錯誤訊息")]
        public string SoaErrorMsg { set; get; }

        [Display(Name = "SOA狀態")]
        public string SoaProcessCode { set; get; }


        public static DataTableJsonResultModel<SoaQueryModel> getModels(DataTableAjaxPostViewModel data,
            string processCode, string processDate, string hasError)
        {
            var paramList = new List<SqlParameter>();
            using var mesContext = new MesContext();
            try
            {
                StringBuilder builder = new StringBuilder(@"
SELECT 
ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS Id
, PROCESS_CODE AS ProcessCode
, PROCESS_DATE AS ProcessDate
, dbo.GetProcessNameByCode(PROCESS_CODE) AS ProcessName 
, SERVER_CODE AS ServerCode
, BATCH_ID AS BatchId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, ROW_NUM AS RowNum
, SOA_PULLING_FLAG AS SoaPullingFlag
, SOA_ERROR_MSG AS SoaErrorMsg
, SOA_PROCESS_CODE AS SoaProcessCode
FROM XXIF_CHP_CONTROL_ST 
WHERE 1=1
");

                if (!string.IsNullOrEmpty(processCode) && processCode.CompareTo("*") != 0)
                {
                    builder.AppendLine(" AND PROCESS_CODE= @processCode");
                    paramList.Add(SqlParamHelper.GetNVarChar("@processCode", processCode, 20));
                }

                DateTime startDate = new DateTime();
                if(!string.IsNullOrEmpty(processDate) && DateTime.TryParseExact(processDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDate))
                {
                    builder.AppendLine(" AND PROCESS_DATE BETWEEN @startDate AND @endDate");
                    
                    paramList.Add(SqlParamHelper.GetDataTime("@startDate", startDate));
                    paramList.Add(SqlParamHelper.GetDataTime("@endDate", startDate.AddDays(1)));
                }

                
                switch(hasError)
                {
                    default:
                        break;
                    case "1":
                        builder.AppendLine(" AND ( ( STATUS_CODE IS NOT NULL AND STATUS_CODE <> 'S') OR (SOA_PROCESS_CODE IS NOT NULL AND SOA_PROCESS_CODE <> 'S') )");
                        break;
                    case "0":
                        builder.AppendLine(" AND ( STATUS_CODE = 'S' AND SOA_PROCESS_CODE = 'S' )");
                        break;
                }

                var models = mesContext.Database.SqlQuery<SoaQueryModel>(builder.ToString(), paramList.ToArray()).ToList();


                var list = Search(models, data.Search.Value);
                list = Order(data.Order, models);

                list = list.Skip(data.Start).Take(data.Length);

                return new DataTableJsonResultModel<SoaQueryModel>(data.Draw, models.Count, list.ToList());

            }
            catch (Exception ex)
            {

            }

            return new DataTableJsonResultModel<SoaQueryModel>(data.Draw, 0, new List<SoaQueryModel>());
        }

        public static IOrderedEnumerable<SoaQueryModel> Order(List<Order> orders, IEnumerable<SoaQueryModel> models)
        {
            IOrderedEnumerable<SoaQueryModel> orderedModel = null;

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

        private static IOrderedEnumerable<SoaQueryModel> OrderBy(int column, string dir, IEnumerable<SoaQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.Id) : models.OrderBy(x => x.Id);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ProcessName) : models.OrderBy(x => x.ProcessName);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ProcessDate) : models.OrderBy(x => x.ProcessDate);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.BatchId) : models.OrderBy(x => x.BatchId);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.RowNum) : models.OrderBy(x => x.RowNum);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.StatusCode) : models.OrderBy(x => x.StatusCode);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.ErrorMsg) : models.OrderBy(x => x.ErrorMsg);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SoaPullingFlag) : models.OrderBy(x => x.SoaPullingFlag);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SoaErrorMsg) : models.OrderBy(x => x.SoaErrorMsg);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.OrderByDescending(x => x.SoaProcessCode) : models.OrderBy(x => x.SoaProcessCode);
            }
        }

        private static IOrderedEnumerable<SoaQueryModel> ThenBy(int column, string dir, IOrderedEnumerable<SoaQueryModel> models)
        {
            switch (column)
            {
                default:
                case 0:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.Id) : models.ThenBy(x => x.Id);
                case 1:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ProcessName) : models.ThenBy(x => x.ProcessName);
                case 2:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ProcessDate) : models.ThenBy(x => x.ProcessDate);
                case 3:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.BatchId) : models.ThenBy(x => x.BatchId);
                case 4:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.RowNum) : models.ThenBy(x => x.RowNum);
                case 5:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.StatusCode) : models.ThenBy(x => x.StatusCode);
                case 6:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.ErrorMsg) : models.ThenBy(x => x.ErrorMsg);
                case 7:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SoaPullingFlag) : models.ThenBy(x => x.SoaPullingFlag);
                case 8:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SoaErrorMsg) : models.ThenBy(x => x.SoaErrorMsg);
                case 9:
                    return string.Compare(dir, "DESC", true) == 0 ? models.ThenByDescending(x => x.SoaProcessCode) : models.ThenBy(x => x.SoaProcessCode);
            }
        }

        private static IEnumerable<SoaQueryModel> Search(IEnumerable<SoaQueryModel> models, string search)
        {
            if (string.IsNullOrEmpty(search)) return models;

            return models.Where(x => 
                    (!string.IsNullOrEmpty(x.ProcessName) && x.ProcessName.Contains(search))
                    || (!string.IsNullOrEmpty(x.BatchId) && x.BatchId.Contains(search))
                    || (!string.IsNullOrEmpty(x.StatusCode) && x.StatusCode.Contains(search))
                    || (!string.IsNullOrEmpty(x.ErrorMsg) && x.ErrorMsg.Contains(search))
                    || (!string.IsNullOrEmpty(x.SoaPullingFlag) && x.SoaPullingFlag.Contains(search))
                    || (!string.IsNullOrEmpty(x.SoaErrorMsg) && x.SoaErrorMsg.Contains(search))
                    || (!string.IsNullOrEmpty(x.SoaProcessCode) && x.SoaProcessCode.Contains(search))
                );
        }

        public static SoaQueryModel getModel(string processCode, string serverCode, string batchId)
        {
            var paramList = new List<SqlParameter>();
            using var mesContext = new MesContext();
            try
            {
                StringBuilder builder = new StringBuilder(@"
SELECT 
ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS Id
, PROCESS_CODE AS ProcessCode
, PROCESS_DATE AS ProcessDate
, dbo.GetProcessNameByCode(PROCESS_CODE) AS ProcessName 
, SERVER_CODE AS ServerCode
, BATCH_ID AS BatchId
, STATUS_CODE AS StatusCode
, ERROR_MSG AS ErrorMsg
, ROW_NUM AS RowNum
, SOA_PULLING_FLAG AS SoaPullingFlag
, SOA_ERROR_MSG AS SoaErrorMsg
, SOA_PROCESS_CODE AS SoaProcessCode
FROM XXIF_CHP_CONTROL_ST 
WHERE 1=1 AND PROCESS_CODE= @processCode AND SERVER_CODE= @serverCode AND BATCH_ID= @batchId
");
                paramList.Add(SqlParamHelper.GetNVarChar("@processCode", processCode, 20));
                paramList.Add(SqlParamHelper.GetNVarChar("@serverCode", serverCode, 20));
                paramList.Add(SqlParamHelper.GetNVarChar("@batchId", batchId, 20));


                var model = mesContext.Database.SqlQuery<SoaQueryModel>(builder.ToString(), paramList.ToArray()).FirstOrDefault();



                return model;

            }
            catch (Exception ex)
            {

            }

            return new SoaQueryModel();
        }
    }
}