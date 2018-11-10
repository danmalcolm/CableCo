using System;
using System.Threading;
using CableCo.Common.Logging;
using CableCo.ProvisioningService.Bus;
using CableCo.ProvisioningService.Windsor;
using log4net;

namespace CableCo.ProvisioningService
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
        }
    }
}
