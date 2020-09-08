using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CHPOUTSRCMES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (MesContext mesContext = new MesContext())
            {
                //readFromXls(mesContext);
            }
            
            logger.Info("Info");
            logger.Warn("Warn");
            logger.Debug("Debug");
            logger.Error("Error");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            //Response.ContentType = "text/html";
            //Response.StatusCode = 500;
            //if (httpException != null)
            //{
            //    Response.StatusCode = httpException.GetHttpCode();
            //    switch (Response.StatusCode)
            //    {
            //        case 404:
            //            routeData.Values["action"] = "General";
            //            break;
            //    }
            //}

            IController errorsController = new CHPOUTSRCMES.Web.Controllers.ErrorsController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }

        internal static void readFromXls(MesContext context)
        {
            var baseDir = AppDomain.CurrentDomain
                               .BaseDirectory
                               .Replace("\\bin", string.Empty) + "Data\\Excel";

            string initialFile = baseDir + "\\MES_20200903.xlsx";

            if (!string.IsNullOrEmpty(initialFile) && File.Exists(initialFile))
            {
                using (FileStream fs = new FileStream(initialFile, FileMode.Open))
                {
                    IWorkbook workbook = null;
                    if (fs.Length > 0 && initialFile.Substring(initialFile.LastIndexOf(".")).Equals(".xls"))
                    {
                        //把xls文件中的数据写入wk中
                        workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        //把xlsx文件中的数据写入wk中
                        workbook = new XSSFWorkbook(fs);
                    }

                    new IdentityUOW(context).ImportUserMisc(workbook);
                }
            }
        }
    }
}
