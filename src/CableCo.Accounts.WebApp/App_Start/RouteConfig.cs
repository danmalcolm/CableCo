using System.Web.Mvc;
using System.Web.Routing;

namespace CableCo.Accounts.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{hellotxt}", new { hellotxt = @"hello\.txt?" });

            routes.MapRoute(
                name: "accounts-index",
                url: "accounts",
                defaults: new {controller = "Accounts", action = "Index" }
            );

            routes.MapRoute(
                name: "accounts-details",
                url: "accounts/{code}",
                defaults: new { controller = "Accounts", action = "Details" }
            );

            routes.MapRoute(
                name: "accounts-detail-collection",
                url: "accounts/{code}/subscriptions",
                defaults: new { controller = "AccountSubscriptions", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}