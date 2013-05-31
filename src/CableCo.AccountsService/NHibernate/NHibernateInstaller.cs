using CableCo.Accounts.NHibernate;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using Rebus;
using Rebus.Castle.Windsor;

namespace CableCo.AccountsService.NHibernate
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var builder = new ConfigurationBuilder();
            var configurationStore = new ConfigurationStore(builder.Build);
            container.Register(
                Component.For<ISessionFactory>()
                         .UsingFactoryMethod(kernel => configurationStore.SessionFactory),
                Component.For<ISession>()
                        .UsingFactoryMethod(kernel => MessageContext.GetCurrent().Items["session"] as ISession)
                        .LifestyleScoped<PerTransportMessage>());
        }
    }
}