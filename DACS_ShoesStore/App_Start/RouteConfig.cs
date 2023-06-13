using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DACS_ShoesStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 namespaces: new[] { "DACS_ShoesStore.Controllers" },
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "ProductDetails",
                url: "ProductDetails/{id}",
                defaults: new { controller = "UserProduct", action = "ProductDetails" }
            );



        }

    }
}
