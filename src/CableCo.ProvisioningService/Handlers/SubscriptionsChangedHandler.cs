using System;
using System.IO;
using System.Threading;
using CableCo.Accounts.Events;
using CableCo.Common.Logging;
using CableCo.Provisioning.Events;
using Rebus;
using log4net;

namespace CableCo.ProvisioningService.Handlers
{
    public class SubscriptionsChangedHandler : IHandleMessages<SubscriptionsChanged>
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();
        private readonly IBus bus;

        public SubscriptionsChangedHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(SubscriptionsChanged @event)
        {
            //TODO fan-out different commands to different systems etc, e.g. tv channels, broadband, phone
            foreach (var subscription in @event.Subscriptions)
            {
                var random = new Random();
                Thread.Sleep(random.Next(1000, 5000));
                // Simulating interaction with 3rd party system, hardware etc
                File.AppendAllText("cryptic-system-file.dat", string.Format("{0}->**{1}\r\n", @event.AccountCode, subscription.ProductCode));
                bus.Publish(new ProductProvisioned { AccountCode = @event.AccountCode, ProductCode = subscription.ProductCode } );
            }
        }
    }
}