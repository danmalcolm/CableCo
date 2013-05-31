using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Rebus.Log4Net;
using CableCo.Common.Rebus;

namespace CableCo.AccountsService.Bus
{
    public class RebusInstaller : IWindsorInstaller
    {
        private const string EndpointName = "CableCo.AccountsService";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var adapter = new WindsorContainerAdapter(container);
            
            var bus = Configure.With(adapter)
                .Logging(x => x.Log4Net())
                .Transport(x => x.UseMsmqAndGetInputQueueNameFromAppConfig())
                .MessageOwnership(x => x.FromRebusConfigurationSection())
                .ConfigureSqlServerStorage(EndpointName)
                .Events(x => x.AddUnitOfWorkManager(new UnitOfWorkManager(container)))
                .CreateBus();

            container.Register(Component.For<IStartableBus>().Instance(bus));
        }
    }
}