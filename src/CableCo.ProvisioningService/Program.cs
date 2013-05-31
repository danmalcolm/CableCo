using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CableCo.Common.Logging;
using CableCo.ProvisioningService.Windsor;
using Castle.Windsor;
using log4net;

namespace CableCo.ProvisioningService
{
    public class Program
    {
        private static WindsorContainer container;
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
                // main blocks here waiting for ctrl-C
                autoResetEvent.WaitOne();
                log.InfoFormat("Shutting down service");
            }
        }
    }
}
