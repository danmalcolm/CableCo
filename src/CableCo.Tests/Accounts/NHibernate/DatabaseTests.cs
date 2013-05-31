using System;
using CableCo.Accounts.NHibernate;
using CableCo.Common.Utility;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace CableCo.Tests.Accounts.NHibernate
{
    [TestFixture, Category("Database Test")]
    public abstract class DatabaseTests
    {
        protected static ConfigurationStore ConfigurationStore { get; private set; }

        static DatabaseTests()
        {
            log4net.Config.BasicConfigurator.Configure();

            // Initialise configuration shared throughout test run via static ConfigurationStore
            var builder = new ConfigurationBuilder(c =>
            {
                c.DataBaseIntegration(db =>
                {
                    db.LogFormattedSql = true;
                    db.LogSqlInConsole = true;
                });
                c.Properties["generate_statistics"] = "true";
            });

            ConfigurationStore = new ConfigurationStore(builder.Build,
                configurationReady: SetupDatabase);
        }

        private static void SetupDatabase(Configuration configuration)
        {
            // Generate schema ready for tests to run
            DatabaseSetupHelper.RecreateDatabase("CableCo.Accounts", Common.Environment.IntegrationTests);
            new SchemaExport(configuration).Create(false, true);
        }

        [SetUp]
        public void SetUp()
        {
            InTransaction(ResetDatabase);
        }

        // Removes all data from test database, leaving it in a clean state for next test
        public static void ResetDatabase(ISession session)
        {
            const string resetSql = @"delete from dbo.Subscription
delete from dbo.Account";

            var command = session.Connection.CreateCommand();
            command.CommandText = resetSql;
            session.Transaction.Enlist(command);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Starts a session and transaction and executes the specified action
        /// </summary>
        /// <param name="action"></param>
        protected void InTransaction(Action<ISession> action)
        {
            using (var session = ConfigurationStore.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                action(session);
                transaction.Commit();
            }
        }
    }
}