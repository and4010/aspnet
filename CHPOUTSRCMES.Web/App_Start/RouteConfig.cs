using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CHPOUTSRCMES.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "StockQuery",
                url: "Stock/Detail/{subinventoryCode}/{locatorId}/{itemId}",
                defaults: new { controller = "Stock", action = "Detail", subinventoryCode = UrlParameter.Optional, locatorId = UrlParameter.Optional,itemId = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
