using Rebus.Config;

namespace CableCo.Common.Rebus
{
    public static class RebusConfigurationExtensions
    {
        /// <summary>
        /// Initialises SQL Server storage for subscriptions and sagas, using a common convention for
        /// database and table names. The connection string called 'endpoint' will be used to connect
        /// to the database.
        /// </summary>
        /// <param name="configurer"></param>
        /// <param name="endpointName"></param>
        /// <param name="subscriptions"></param>
        /// <param name="sagas"></param>
        /// <returns></returns>
        public static RebusConfigurer ConfigureSqlServerStorage(this RebusConfigurer configurer, string endpointName, bool subscriptions = true, bool sagas = true)
        {
//            var environment = ConfigurationUtility.ReadAppSetting<Environment>("Environment");
//            string databaseName = endpointName + "EndPoint";
//            DatabaseSetupHelper.CreateDatabase(databaseName, environment);
//
//            string subscriptionTable = string.Format("{0}.Subscription", endpointName);
//            string sagaTable = string.Format("{0}.Saga", endpointName);
//            string sagaIndexTable = string.Format("{0}.SagaIndex", endpointName);
//            string connectionString = ConfigurationUtility.ReadConnectionString("endpoint");
//
//            if (subscriptions)
//            {
//               configurer.Subscriptions(x => x.StoreInSqlServer(connectionString, subscriptionTable)
//                    .EnsureTableIsCreated());
//            }
//            if (sagas)
//            {
//                configurer.Sagas(x => x.StoreInSqlServer(connectionString, sagaTable, sagaIndexTable)
//                    .EnsureTablesAreCreated());
//            }
            return configurer;
        }

    }
}