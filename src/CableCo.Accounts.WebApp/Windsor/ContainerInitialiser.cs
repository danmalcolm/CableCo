using System.Web.Http;
using System.Web.Mvc;
using CableCo.Common.Logging;
using CableCo.Common.Windsor;
using Castle.Windsor;
using Castle.Windsor.Installer;
using log4net;

namespace CableCo.Accounts.WebApp.Windsor
{
    public class ContainerInitialiser
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();

        /// <summary>
        /// Creates Windsor Container and installs components via all installers within application projects
        /// </summary>
        /// <returns></returns>
        public WindsorContainer Create()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(ApplicationAssemblyHelper.AssemblyFilter));
            DependencyResolver.SetResolver(new MvcDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new WebApiDependencyResolver(container);                 return container;
        }
    }
}