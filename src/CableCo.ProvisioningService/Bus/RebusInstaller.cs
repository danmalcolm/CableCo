using CableCo.Accounts.Events;
using CableCo.Common.Rebus;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.Log4Net;
using Rebus.Transports.Msmq;

namespace CableCo.ProvisioningService.Bus
{
    public class RebusInstaller : IWindsorInstaller
    {
        private const string EndpointName = "CableCo.ProvisioningService";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var adapter = new WindsorContainerAdapter(container);

            var bus = Configure.With(adapter)
                .Logging(l => l.Log4Net())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .MessageOwnership(c => c.FromRebusConfigurationSection())
                .ConfigureSqlServerStorage(EndpointName)
                .CreateBus()
                .Start();

            bus.Subscribe<AccountCreated>();
            bus.Subscribe<SubscriptionsChanged>();
        }
    }
}