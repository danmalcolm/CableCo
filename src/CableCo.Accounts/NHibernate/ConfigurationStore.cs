using System;
using CableCo.Common.Utility;
using NHibernate;
using NHibernate.Cfg;

namespace CableCo.Accounts.NHibernate
{
    /// <summary>
    /// Provides access to NHibernate Configuration and ISessionFactory used to load and
    /// save an object model within the application. Ensures that the ISessionFactory 
    /// is built only once during the lifetime of the application.
    /// </summary>
    public class ConfigurationStore
    {
        private readonly ThreadSafeInitializer<Configuration> configurationInitializer;
        private readonly ThreadSafeInitializer<ISessionFactory> factoryInitializer;

        /// <summary>
        /// Creates a new ConfigurationStore
        /// </summary>
        /// <param name="create">Function that creates the configuration</param>
        /// <param name="configurationReady">Action that is executed once after the configuration has been created, suitable for things like creating a demo / dev database</param>
        /// <param name="sessionFactoryReady">Action that is executed once after the session factory has been created, suitable for things like adding demo data to the database</param>
        public ConfigurationStore(Func<Configuration> create, Action<Configuration> configurationReady = null, Action<Configuration, ISessionFactory> sessionFactoryReady = null)
        {
            configurationInitializer = ThreadSafeInitializer.Create(() =>
            {
                var configuration = create();
                if (configurationReady != null)
                    configurationReady(configuration);
                return configuration;
            });

            factoryInitializer = ThreadSafeInitializer.Create(() =>
            {
                var factory = Configuration.BuildSessionFactory();
                if (sessionFactoryReady != null)
                    sessionFactoryReady(Configuration, factory);
                return factory;
            });
        }

        public Configuration Configuration
        {
            get { return configurationInitializer.Value; }
        }

        public ISessionFactory SessionFactory
        {
            get { return factoryInitializer.Value; }
        }
    }
}