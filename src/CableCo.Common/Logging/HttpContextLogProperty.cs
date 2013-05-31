using System;
using System.Web;
using System.Web.SessionState;

namespace CableCo.Common.Logging
{
    /// <summary>
    /// Used as log4net property to add information from HttpContext in log
    /// <code>
    /// GlobalContext.Properties["request-sessionid"] = HttpContextInfoProvider.FromSession((c) => c.Session.SessionID);
    /// </code>
    /// </summary>
    public class HttpContextLogProperty
    {

        public static HttpContextLogProperty CreateForRequest(Func<HttpRequest, object> getInfo)
        {
            return new HttpContextLogProperty(context =>
            {
                HttpRequest request = null;
                // Forced to do hacky try-catch because Request not available in code executing
                // within Application_Start. See http://mvolo.com/blogs/serverside/archive/2007/11/10/Integrated-mode-Request-is-not-available-in-this-context-in-Application_5F00_Start.aspx
                try
                {
                    request = context.Request;
                }
                catch (HttpException ) { }

                if (request != null)
                {
                    return getInfo(request);
                }
                else
                {
                    return null;
                }
            });
        }

        public static HttpContextLogProperty CreateForSession(Func<HttpSessionState, object> getInfo)
        {
            return new HttpContextLogProperty(context =>
            {
                if (context.Session != null)
                {
                    return getInfo(context.Session);
                }
                else
                {
                    return null;
                }
            });
        }

        private readonly Func<HttpContext, object> getInfo;

        public HttpContextLogProperty(Func<HttpContext, object> getInfo)
        {
            this.getInfo = getInfo;
        }

        public override string ToString()
        {
            if (HttpContext.Current != null)
            {
                object info = getInfo(HttpContext.Current);
                if (info != null) return info.ToString();
            }
            return "";
        }
    }
}