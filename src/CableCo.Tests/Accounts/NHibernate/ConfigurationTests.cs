using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace CableCo.Tests.Accounts.NHibernate
{
    [TestFixture]
    public class ConfigurationTests : DatabaseTests
    {
        [Test]
        public void configuration_should_be_valid()
        {
            // Tests overall configuration
            var configuration = ConfigurationStore.Configuration;
            // This will catch any non-virtual properties on lazy entities
            var sessionFactory = ConfigurationStore.SessionFactory;
        }

        [Test]
        public void generated_schema_should_be_valid()
        {
            // Ensures that schema is valid - useful for 2 reasons:
            // 1. It catches any invalid column names, e.g. unescaped SQL keywords
            // 2. Generates sql script, useful for code-first approach
            var configuration = ConfigurationStore.Configuration;
            new SchemaExport(configuration).SetOutputFile("..\\..\\Generated\\accounts-schema.sql").Create(true, true);
        }

    }
}