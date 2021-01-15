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
    public class YszmpckqUOW : MasterUOW
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();


        public YszmpckqUOW(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 取得加工廠選單資料
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <returns></returns>
        public List<SelectListItem> GetOspSubinventoryList(string ORGANIZATION_ID) //要加條件OSP_FLAG為Y 只顯示是加工廠的倉庫
        {
            long organizationId = 0;
            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            List<SelectListItem> ospSubinventoryList = new List<SelectListItem>();
            ospSubinventoryList.Add(new SelectListItem { Text = "全部", Value = "*" });

            try
            {
                using (var db = new MesContext())
                {
                    if (ORGANIZATION_ID == "*")
                    {
                        var query = db.YszmpckqTs.Join(
                           db.SubinventoryTs,
                           a => a.OrganizationId,
                           d => d.OrganizationId,
                           (a, b) => new
                           {
                               c = a,
                               d = b,
                           }
                           )
                           .Where(x => x.d.OspFlag == "Y")
                           .GroupBy(x => x.c.OspSubinventory)
                           .Select(s => new SelectListItem()
                           {
                               Text = s.Key,
                               Value = s.Key,
                           }).ToList();
                        ospSubinventoryList.AddRange(query);
                    }
                    else
                    {

                        var query = db.YszmpckqTs.Join(
                            db.SubinventoryTs,
                            a => a.OrganizationId,
                            d => d.OrganizationId,
                            (a, b) => new
                            {
                                c = a,
                                d = b,
                            }
                            )
                            .Where(x => x.d.OrganizationId == organizationId && x.d.OspFlag == "Y")
                            .GroupBy(x => x.c.OspSubinventory)
                            .Select(s => new SelectListItem()
                            {
                                Text = s.Key,
                                Value = s.Key,
                            }).ToList();
                        ospSubinventoryList.AddRange(query);
                    }

                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
            return ospSubinventoryList;
        }

        /// <summary>
        /// 取得紙別選單資料
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="OSP_SUBINVENTORY_ID"></param>
        /// <returns></returns>
        public List<SelectListItem> GetPstypList(string ORGANIZATION_ID, string OSP_SUBINVENTORY_ID)
        {
            long organizationId = 0;

            try
            {
                if (ORGANIZATION_ID != "*")
                {
                    organizationId = Convert.ToInt64(ORGANIZATION_ID);
                }
            }
            catch
            {
                ORGANIZATION_ID = "*";
            }

            try
            {
                using (var mesContext = new MesContext())
                {
                    List<SelectListItem> PstypList = new List<SelectListItem>();
                    PstypList.Add(new SelectListItem { Text = "全部", Value = "*" });

                    List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                    List<string> cond = new List<string>();
                    StringBuilder query = new StringBuilder();
                    query.Append(
@"SELECT DISTINCT 
[PSTYP] AS Text,
[PSTYP] AS Value
FROM [YSZMPCKQ_T]");
                    if (ORGANIZATION_ID != "*")
                    {
                        cond.Add("ORGANIZATION_ID = @ORGANIZATION_ID");
                        sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", organizationId));
                    }
                    if (OSP_SUBINVENTORY_ID != "全部")
                    {
                        cond.Add("OSP_SUBINVENTORY =@OSP_SUBINVENTORY");
                        sqlParameterList.Add(new SqlParameter("@OSP_SUBINVENTORY", OSP_SUBINVENTORY_ID));
                    }


                    string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                    if (sqlParameterList.Count > 0)
                    {
                        PstypList.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText, sqlParameterList.ToArray()).ToList());
                    }
                    else
                    {
                        PstypList.AddRange(mesContext.Database.SqlQuery<SelectListItem>(commandText).ToList());
                    }
                    return PstypList;
                }
            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<SelectListItem>();
            }
        }


        /// <summary>
        /// 取得板令對照表單資料
        /// </summary>
        /// <param name="ORGANIZATION_ID"></param>
        /// <param name="OSP_SUBINVENTORY_ID"></param>
        /// <param name="PSTYP"></param>
        /// <returns></returns>
        public List<YszmpckqDT> search(string ORGANIZATION_ID, string OSP_SUBINVENTORY_ID, string PSTYP)
        {
            try
            {
                long orgId = 0;

                try
                {
                    if (!string.IsNullOrEmpty(ORGANIZATION_ID) && ORGANIZATION_ID != "*")
                    {
                        orgId = Convert.ToInt64(ORGANIZATION_ID);
                    }
                }
                catch
                {
                    ORGANIZATION_ID = "*";
                }

                try
                {
                    using (var mesContext = new MesContext())
                    {

                        List<SqlParameter> sqlParameterList = new List<SqlParameter>();
                        List<string> cond = new List<string>();
                        StringBuilder query = new StringBuilder();
                        query.Append(
    @"SELECT y.[YSZMPCKQ_ID]
      ,y.[ORGANIZATION_ID] as ORGANIZATION_ID
      ,y.[ORGANIZATION_CODE]
	  ,ot.[ORGANIZATION_NAME] 
	  ,st.SUBINVENTORY_CODE as OSP_SUBINVENTORY
	  ,st.SUBINVENTORY_NAME as OSP_SUBINVENTORY_NAME
      ,y.[OSP_SUBINVENTORY]
      ,y.[PSTYP]
      ,y.[BWETUP]
      ,y.[BWETDN]
      ,y.[RWTUP]
      ,y.[RWTDN]
      ,y.[PCKQ]
      ,y.[PAPER_QTY]
      ,y.[PIECES_QTY]
      ,y.[CONTROL_FLAG]
      ,y.[CREATED_BY]
      ,y.[CREATION_DATE]
      ,y.[LAST_UPDATE_BY]
      ,y.[LAST_UPDATE_DATE]
FROM [YSZMPCKQ_T] y
left join ORGANIZATION_T ot on ot.ORGANIZATION_ID = y.ORGANIZATION_ID
left join SUBINVENTORY_T st on st.SUBINVENTORY_CODE = y.OSP_SUBINVENTORY");
                        if (ORGANIZATION_ID != "*")
                        {
                            cond.Add("y.ORGANIZATION_ID = @ORGANIZATION_ID");
                            sqlParameterList.Add(new SqlParameter("@ORGANIZATION_ID", orgId));
                        }
                        if (OSP_SUBINVENTORY_ID != "全部")
                        {
                            cond.Add("OSP_SUBINVENTORY =@OSP_SUBINVENTORY");
                            sqlParameterList.Add(new SqlParameter("@OSP_SUBINVENTORY", OSP_SUBINVENTORY_ID));
                        }
                        if (PSTYP != "*")
                        {
                            cond.Add("PSTYP = @PSTYP");
                            sqlParameterList.Add(new SqlParameter("@PSTYP", PSTYP));
                        }


                        string commandText = string.Format(query + "{0}{1}", cond.Count > 0 ? " where " : "", string.Join(" and ", cond.ToArray()));
                        if (sqlParameterList.Count > 0)
                        {
                            return mesContext.Database.SqlQuery<YszmpckqDT>(commandText, sqlParameterList.ToArray()).ToList();
                        }
                        else
                        {
                            return mesContext.Database.SqlQuery<YszmpckqDT>(commandText).ToList();
                        }

                    }
                }
                catch (Exception e)
                {
                    logger.Error(LogUtilities.BuildExceptionMessage(e));
                    return new List<YszmpckqDT>();
                }

            }
            catch (Exception e)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(e));
                return new List<YszmpckqDT>();
            }

        }
    }
}