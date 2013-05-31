using System;
using System.Collections.Generic;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;

namespace CableCo.Accounts.NHibernate
{
    /// <summary>
    /// Creates NHibernate Configuration for the application's object model
    /// </summary>
    public class ConfigurationBuilder
    {
        private readonly Action<Configuration> customize;

        /// <param name="customize">An optional action to customize the configuration after it has been built with standard settings. Intended to allow settings to be added when running tests, such as enabling statistics or logging sql</param>
        public ConfigurationBuilder(Action<Configuration> customize = null)
        {
            this.customize = customize;
        }

        public Configuration Build()
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008Dialect>();
                db.ConnectionStringName = "accounts";
                db.BatchSize = 100;
            });
            configuration.CurrentSessionContext<CallSessionContext>();

            var props = new Dictionary<string, string>
			{
				{"generate_statistics", "false"},
				{"hbm2ddl.keywords", "auto-quote"}
			};
            configuration.AddProperties(props);

            ModelMapping.Add(configuration);

            if (customize != null)
                customize(configuration);

            return configuration;
        }
    }
}