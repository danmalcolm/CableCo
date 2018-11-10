using System;
using System.Threading;
using CableCo.Accounts.Events;
using CableCo.AccountsService.Bus;
using CableCo.AccountsService.Windsor;
using CableCo.Common.Logging;
using Rebus.Startup;
using ILog = log4net.ILog;

namespace CableCo.AccountsService
{
    public class Program
    {
        private static ILog log;

        static void Main()
        {
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
                RebusConfiguration.Init(container);
                Console.WriteLine("Application has started. Ctrl-C to end");
                autoResetEvent.WaitOne();
                log.InfoFormat("Shutting down service");
            }

            using (var container = new ContainerInitialiser().Create())
            {
                log.DebugFormat("Initialised container");
                var starter = container.Resolve<IBusStarter>();
                starter.Start();
                
                DomainEvents.Configure(e => starter.Bus.Publish(e));
                
                // main blocks here waiting for ctrl-C
                autoResetEvent.WaitOne();
                log.InfoFormat("Shutting down");
            }
        }
    }
}
