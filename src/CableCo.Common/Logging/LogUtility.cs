using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using CableCo.Common.Utility;
using log4net;
using log4net.Config;

namespace CableCo.Common.Logging
{
    public static class LogUtility
    {
        private static bool initialised = false;

        /// <summary>
        /// Creates a Log based on the type calling this method. This method is intended for use once per class, 
        /// such as in a static field initialiser.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ILog ForCurrentType()
        {
            var callingType = new StackFrame(1, false).GetMethod().DeclaringType;
            return LogManager.GetLogger(callingType);
        }

        /// <summary>
        /// Should be called at application startup to set up log4net
        /// </summary>
        public static void Initialise()
        {
            if (!initialised)
            {
                string configFilePath = PathUtility.CombineWithAppDomainPath("log4net.config");
                if (File.Exists(configFilePath))
                {
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));
                }
                else
                {
                    BasicConfigurator.Configure();
                }
                AddGlobalPropertiesIfWebApp();
                initialised = true;
            }
        }

        private static void AddGlobalPropertiesIfWebApp()
        {
            if (HttpRuntime.AppDomainAppId != null) // http://stackoverflow.com/questions/3179716/how-determine-if-application-is-web-application
            {
                GlobalContext.Properties["request-sessionid"] = HttpContextLogProperty.CreateForSession(session => session.SessionID);
                GlobalContext.Properties["request-url"] = HttpContextLogProperty.CreateForRequest(request => request.Url.AbsolutePath);
                GlobalContext.Properties["username"] = new UserNameLogProperty();
            }
        }
    }
}