using NHibernate.Cfg;
using NUnit.Framework;
using CableCo.Accounts.NHibernate;

namespace CableCo.Tests.Accounts.NHibernate
{
    [TestFixture]
    public class ConfigurationBuilderTests
    {
        [Test]
        public void should_use_default_properties_if_no_customization()
        {
            var configuration = new ConfigurationBuilder().Build();
            Assert.AreEqual("accounts", configuration.Properties["connection.connection_string_name"]);
        }

        [Test]
        public void should_customize_default_configuration_if_specified()
        {
            var configuration = new ConfigurationBuilder(c =>
            {
                c.Properties["generate_statistics"] = "true";
                c.DataBaseIntegration(db => db.ConnectionStringName = "core2");
            })
                .Build();
            Assert.AreEqual("true", configuration.Properties["generate_statistics"]);
            Assert.AreEqual("core2", configuration.Properties["connection.connection_string_name"]);
        }
    }
}