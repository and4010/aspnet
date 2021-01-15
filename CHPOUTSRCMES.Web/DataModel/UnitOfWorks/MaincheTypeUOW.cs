using CHPOUTSRCMES.Web.DataModel.Entity.Information;
using CHPOUTSRCMES.Web.DataModel.Entity.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entity.Repositorys;
using CHPOUTSRCMES.Web.Models.Information;
using CHPOUTSRCMES.Web.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class MaincheTypeUOW : MasterUOW
    {

        private ILogger logger = LogManager.GetCurrentClassLogger();


        public MaincheTypeUOW(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 取得機台組織
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetOrganization_code()
        {
            var OrgList = new List<SelectListItem>();
            try
            {
                var tempList = machinePaperTypeRepository.GetAll().
                     GroupBy(x => x.OrganizationCode).
                     Select(x => new SelectListItem()
                     {
                         Text = x.Key,
                         Value = x.Key,
                     });
                OrgList.Add(new SelectListItem() { Text = "全部", Value = "*" });
                OrgList.AddRange(tempList);
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
            return OrgList;
        }


        /// <summary>
        /// 取得基本資料-紙別機台表單資料
        /// </summary>
        public List<MachinePaperType> GetMachinePaperTypes(string Organization_code)
        {
            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"
Select
[MACHINE_CODE] as Machine_code,
[ORGANIZATION_ID] as Organization_id,
[ORGANIZATION_CODE] as Organization_code,
[MACHINE_MEANING] as Machine_meaning,
[DESCRIPTION] as Description,
[PAPER_TYPE] as Paper_type,
[MACHINE_NUM] as Machine_num,
[SUPPLIER_NUM] as Supplier_num,
[SUPPLIER_NAME] as Supplier_name,
[CREATED_BY] as Created_by,
[CREATION_DATE] as Creation_date,
[LAST_UPDATE_BY] as Last_updated_by,
[LAST_UPDATE_DATE] as Last_update_date
FROM [MACHINE_PAPER_TYPE_T]");
                    if (Organization_code != "*")
                    {
                        cond.Add("ORGANIZATION_CODE = @ORGANIZATION_CODE");
                        sqlParameterList.Add(new SqlParameter("@ORGANIZATION_CODE", Organization_code));
                    }
                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        return mesContext.Database.SqlQuery<MachinePaperType>(commandText, sqlParameterList.ToArray()).ToList();
                    }
                    else
                    {
                        return mesContext.Database.SqlQuery<MachinePaperType>(commandText).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<MachinePaperType>();
            }
            //return result;
        }


    }
}