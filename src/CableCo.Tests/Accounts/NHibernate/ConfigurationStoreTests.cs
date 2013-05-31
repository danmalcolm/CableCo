using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CableCo.Accounts.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;

namespace CableCo.Tests.Accounts.NHibernate
{
    [TestFixture]
    public class ConfigurationStoreTests
    {
        public class TestConfigurationBuilder
        {
            public int BuildCount;

            public virtual Configuration Build()
            {
                var configuration = new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.Dialect<MsSql2008Dialect>();
                    db.ConnectionStringName = "accounts";
                });

                Interlocked.Increment(ref BuildCount);

                return configuration;
            }
        }

        public class FailingConfigurationBuilder : TestConfigurationBuilder
        {
            public int AttemptedBuildCount = 0;
            private readonly int failCount;

            public FailingConfigurationBuilder(int failCount)
            {
                this.failCount = failCount;
            }

            public override Configuration Build()
            {
                AttemptedBuildCount++;
                if (AttemptedBuildCount <= failCount)
                    throw new TestException();
                return base.Build();
            }

            public class TestException : Exception
            {

            }
        }

        [Test]
        public void should_not_initialise_configuration_and_session_factory_until_accessed()
        {
            var builder = new TestConfigurationBuilder();
            var store = new ConfigurationStore(builder.Build);
            Assert.AreEqual(0, builder.BuildCount);
        }

        [Test]
        public void should_initialise_configuration_only_once_when_accessed()
        {
            var builder = new TestConfigurationBuilder();
            var store = new ConfigurationStore(builder.Build);
            Assert.AreEqual(0, builder.BuildCount);
            var configuration = store.Configuration;
            var sessionFactory = store.SessionFactory;
            Assert.AreEqual(1, builder.BuildCount);
            var configuration2 = store.Configuration;
            var sessionFactory2 = store.SessionFactory;
            Assert.AreEqual(1, builder.BuildCount);
        }

        [Test]
        public void should_return_same_configuration_and_session_factory_each_time_accessed()
        {
            var builder = new TestConfigurationBuilder();
            var store = new ConfigurationStore(builder.Build);
            Assert.AreEqual(0, builder.BuildCount);

            var configuration = store.Configuration;
            var sessionFactory = store.SessionFactory;
            var configuration2 = store.Configuration;
            var sessionFactory2 = store.SessionFactory;

            Assert.AreSame(configuration, configuration2);
            Assert.AreSame(sessionFactory, sessionFactory2);
        }

        [Test]
        public void configuration_and_session_factory_should_be_initialised_once_when_accessed_concurrently_by_multiple_threads()
        {
            var values = new ConcurrentDictionary<int, Tuple<Configuration, ISessionFactory>>();
            var builder = new TestConfigurationBuilder();
            var store = new ConfigurationStore(builder.Build);

            Parallel.ForEach(Enumerable.Range(0, 100), index =>
            {
                var value = Tuple.Create(store.Configuration, store.SessionFactory);
                values.AddOrUpdate(index, value, (key, existing) => value);
            });

            Assert.AreEqual(1, builder.BuildCount);
            Assert.AreEqual(1, values.Values.Select(x => x.Item1).Distinct().Count());
            Assert.AreEqual(1, values.Values.Select(x => x.Item2).Distinct().Count());
        }

        [Test]
        public void should_retry_when_initialisation_fails_until_successful()
        {
            const int failCount = 2;

            var builder = new FailingConfigurationBuilder(failCount);
            var store = new ConfigurationStore(builder.Build);

            Configuration configuration1;
            for (var i = 0; i < failCount; i++)
            {
                try
                {
                    configuration1 = store.Configuration;
                    Assert.Fail("Exception should be thrown on first 2 attempts to initialise configuration");
                }
                catch { }
            }
            configuration1 = store.Configuration;
            var configuration2 = store.Configuration;
            Assert.IsNotNull(configuration1);
            Assert.AreSame(configuration1, configuration2);
            Assert.AreEqual(3, builder.AttemptedBuildCount);
            Assert.AreEqual(1, builder.BuildCount);
        }
    }
}