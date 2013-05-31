using CableCo.Accounts.Demo;
using CableCo.Accounts.NHibernate;
using CableCo.Common.Utility;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Environment = CableCo.Common.Environment;

namespace CableCo.Accounts.WebApp.NHibernate
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore configurationStore)
        {
            var builder = new ConfigurationBuilder(c => c.CurrentSessionContext<LazyWebSessionContext>());
            var store = new ConfigurationStore(builder.Build, SetUpSchema, CreateDemoData);
            LazyWebSessionContextModule.ConfigurationStore = store;
            container.Register(
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(kernel => store.SessionFactory),
                Component.For<ISession>()
                    .UsingFactoryMethod(kernel =>
                        {
                            var sessionFactory = kernel.Resolve<ISessionFactory>();
                            return sessionFactory.GetCurrentSession();
                        }).LifestyleTransient());

        }

        private void SetUpSchema(Configuration configuration)
        {
            if (ConfigurationUtility.ReadAppSetting<Environment>("Environment") == Environment.Dev)
            {
                DatabaseSetupHelper.RecreateDatabase("CableCo.Accounts", Environment.Dev);
                new SchemaExport(configuration).Create(false, true);
            }
        }

        private void CreateDemoData(Configuration configuration, ISessionFactory factory)
        {
            if (ConfigurationUtility.ReadAppSetting<Environment>("Environment") == Environment.Dev)
            {
                using (var session = factory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    new DemoDataGenerator().Create(session);
                    transaction.Commit();
                }
            }
        }
    }
}