using CableCo.Accounts.Events;
using CableCo.Common.Rebus;
using CableCo.Provisioning.Events;
using Castle.Windsor;
using Rebus.Config;
using Rebus.Routing.TypeBased;

namespace CableCo.AccountsService.Bus
{
    public static class RebusConfiguration
    {
        private const string EndpointName = "CableCo.AccountsService";

        public static void Init(IWindsorContainer container)
        {
            var adapter = new CastleWindsorContainerAdapter(container);
            var bus = Configure.With(adapter)
                .Logging(x => x.Log4Net())
                .Transport(x => x.UseMsmq($"{EndpointName}.input"))
                .Routing(t => t.TypeBased().AddEndpointMappingsFromAppConfig())
                .SetupSubscriptions("endpoint", "AccountsServiceSubscriptions")
                .SetupUnitOfWork(container)
                .Options(x => x.SetNumberOfWorkers(5))
                .Start();
            
            bus.Subscribe<ProductProvisioned>();

            DomainEvents.Configure(e => bus.Publish(e));
        }
    }
}