using System;
using System.IO;
using CableCo.Accounts.Events;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus.Castle.Windsor;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Rebus.Log4Net;

namespace CableCo.Accounts.WebApp.Bus
{
    public class RebusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var adapter = new WindsorContainerAdapter(container);
            
            string subscriptionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rebus_subscriptions.xml");
            var bus = Configure.With(adapter)
                .Logging(l => l.Log4Net())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .Subscriptions(s => s.StoreInXmlFile(subscriptionsFilePath))
                .CreateBus()
                .Start();
            bus.Subscribe<AccountCreated>();
            bus.Subscribe<SubscriptionsChanged>(); 
            bus.Subscribe<SubscriptionProvisioned>();
        }
    }
}