using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rebus;

namespace CableCo.AccountsService.Bus
{
    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly().BasedOn<IHandleMessages>()
                .WithService.AllInterfaces()
                .LifestyleTransient());
        }
    }
}