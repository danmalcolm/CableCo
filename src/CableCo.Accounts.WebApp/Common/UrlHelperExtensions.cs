using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CableCo.Accounts.WebApp.Common
{
    public static class UrlHelperExtensions
    {
        public static string Action<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expression, object additionalRouteValues = null, string protocol = "http")
             where TController : Controller
        {
            var routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(expression);
            var controller = routeValues["controller"].ToString();
            var action = routeValues["action"].ToString();

            routeValues.MergeFrom(additionalRouteValues);

            return urlHelper.Action(action, controller, routeValues, protocol, null);
        }

        /// <summary>
        /// Gets an absolute URL within site for HttpRequest
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="path"></param>
        /// <param name="request"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static string Content(this UrlHelper urlHelper, string path, HttpRequestBase request, string scheme)
        {
            path = urlHelper.Content(path);
            var builder = new UriBuilder(request.Url.AbsoluteUri)
            {
                Path = path,
                Scheme = scheme
            };
            string url = builder.ToString();
            return url;
        }
    }
}