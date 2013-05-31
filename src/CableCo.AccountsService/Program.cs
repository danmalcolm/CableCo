using System;
using System.Threading;
using CableCo.Accounts.Events;
using CableCo.AccountsService.Windsor;
using CableCo.Common.Logging;
using CableCo.Provisioning.Events;
using Rebus;
using ILog = log4net.ILog;

namespace CableCo.AccountsService
{
    public class Program
    {
        private static ILog log;

        static void Main()
        {
            Console.WriteLine("Application has started. Ctrl-C to end");

            LogUtility.Initialise();
            log = LogUtility.ForCurrentType();

            var autoResetEvent = new AutoResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                // cancel the cancellation to allow the program to shutdown cleanly
                eventArgs.Cancel = true;
                autoResetEvent.Set();
            };

            using (var container = new ContainerInitialiser().Create())
            {
                log.DebugFormat("Initialised container");
                var bus = container.Resolve<IStartableBus>().Start();
                bus.Subscribe<ProductProvisioned>();
                DomainEvents.Configure(bus.Publish);
                
                // main blocks here waiting for ctrl-C
                autoResetEvent.WaitOne();
                log.InfoFormat("Shutting down");
            }
        }
    }
}
