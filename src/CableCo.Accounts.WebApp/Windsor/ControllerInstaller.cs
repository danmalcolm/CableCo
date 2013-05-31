using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CableCo.Accounts.WebApp.Windsor
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // MVC controllers
            container.Register(Classes.FromThisAssembly().BasedOn<ControllerBase>().LifestyleTransient());
            // WebAPI controllers
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped());
        }
    }
}