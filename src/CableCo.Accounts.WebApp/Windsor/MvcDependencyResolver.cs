using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;

namespace CableCo.Accounts.WebApp.Windsor
{
    public class MvcDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container;

        public MvcDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.ResolveAll(serviceType).Cast<object>() : new object[] { };
        }
    }
}