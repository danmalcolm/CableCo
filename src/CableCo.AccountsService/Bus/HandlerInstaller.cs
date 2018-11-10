using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus.CastleWindsor;

namespace CableCo.AccountsService.Bus
{
    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AutoRegisterHandlersFromAssemblyOf<HandlerInstaller>();
        }
    }
}