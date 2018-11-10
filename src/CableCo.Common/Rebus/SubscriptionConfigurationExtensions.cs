using CableCo.Common.Utility;
using Rebus.Config;

namespace CableCo.Common.Rebus
{
    public static class SubscriptionConfigurationExtensions
    {
        public static RebusConfigurer SetupSubscriptions(this RebusConfigurer configurer, 
            string connectionStringName,
            string tableName)
        {
            // BUG in StoreInSqlServer, which expects connection string
            // not name of connection strings
            string connectionString = ConfigurationUtility.ReadConnectionString(connectionStringName);
            return configurer.Subscriptions(x => x.StoreInSqlServer(connectionString, tableName));
        }
    }
}