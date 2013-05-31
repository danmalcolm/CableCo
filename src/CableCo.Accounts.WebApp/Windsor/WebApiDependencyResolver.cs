using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace CableCo.Accounts.WebApp.Windsor
{
    public class WebApiDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container;

        public WebApiDependencyResolver(IWindsorContainer container)
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

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(this.container.Kernel);
        }

        public void Dispose()
        {
            container.Dispose();
        }

       
    }
}