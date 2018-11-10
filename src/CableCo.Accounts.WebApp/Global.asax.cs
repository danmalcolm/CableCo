using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CableCo.Accounts.WebApp.App_Start;
using CableCo.Accounts.WebApp.Bus;
using CableCo.Accounts.WebApp.Windsor;
using CableCo.Common.Logging;
using Castle.Windsor;
using log4net;
using Rebus.Startup;

namespace CableCo.Accounts.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog log;
        private IWindsorContainer container;

        protected void Application_Start()
        {
            SetupLogging();
            log.InfoFormat("Application starting");

            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            container = new ContainerInitialiser().Create();
            RebusConfiguration.Init(container);
        }

        private void SetupLogging()
        {
            LogUtility.Initialise();
            log = LogManager.GetLogger(this.GetType());
        }
    }
}