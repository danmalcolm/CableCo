using CableCo.Accounts.Events;
using CableCo.Common.Rebus;
using Castle.Windsor;
using Rebus.Config;
using Rebus.Routing.TypeBased;

namespace CableCo.Accounts.WebApp.Bus
{
    public static class RebusConfiguration 
    {
        private const string EndpointName = "CableCo.Accounts.WebApp";

        public static void Init(IWindsorContainer container)
        {
            var adapter = new CastleWindsorContainerAdapter(container);
            var bus = Configure.With(adapter)
                .Logging(x => x.Log4Net())
                .Transport(x => x.UseMsmq($"{EndpointName}.input"))
                .Routing(t => t.TypeBased().AddEndpointMappingsFromAppConfig())
                .SetupSubscriptions("accounts", "AccountsWebAppSubscriptions")
                .SetupUnitOfWork(container)
                .Options(x => x.SetNumberOfWorkers(5))
                .Start();

            bus.Subscribe<AccountCreated>();
            bus.Subscribe<SubscriptionsChanged>();
            bus.Subscribe<SubscriptionProvisioned>();
        }
    }
}