using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CHPOUTSRCMES.Web.DataModel;

namespace CHPOUTSRCMES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (MesContext mesContext = new MesContext())
            {
            }
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
    }
}
