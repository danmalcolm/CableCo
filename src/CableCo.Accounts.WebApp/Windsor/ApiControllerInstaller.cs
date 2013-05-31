using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using CableCo.Common.Utility;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CableCo.Accounts.WebApp.Windsor
{
    public class ApiControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped());
        }
    }
}