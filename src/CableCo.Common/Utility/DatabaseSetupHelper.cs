using System;
using System.Data.SqlClient;
using System.Text;
using CableCo.Common.Logging;
using log4net;

namespace CableCo.Common.Utility
{
	public class DatabaseSetupHelper
	{
	    private static readonly ILog Log = LogUtility.ForCurrentType();

        private const string DropTemplate = @"
if exists(select * from sysdatabases where name = '{0}')
begin
	alter database [{0}] set offline with rollback immediate 
	alter database [{0}] set online 
	drop database [{0}] 
end
";

        private const string CreateTemplate = @"
if not exists(select * from sysdatabases where name = '{0}')
begin
	create database [{0}]
end	
";
        /// <summary>
        /// Creates a database on the SQL Server specified by the "setup" connection string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="environment"></param>
        public static void CreateDatabase(string name, Environment environment)
        {
            SetupDatabase(name, environment, false, true);
        }

        /// <summary>
        /// Recreates a database on the SQL Server specified by the "setup" connection string. If a database
        /// exists it will be dropped, then created
        /// </summary>
        /// <param name="name"></param>
        /// <param name="environment"></param>
		public static void RecreateDatabase(string name, Environment environment)
		{
            SetupDatabase(name, environment, true, true);
		}

        private static void SetupDatabase(string baseName, Environment environment, bool drop, bool create)
        {
            string databaseName = string.Format("{0}.{1}", baseName, environment);
            var commands = new StringBuilder();
            if (drop)
            {
                Log.InfoFormat("Adding command to drop database '{0}' if it exists", databaseName);
                commands.AppendFormat(DropTemplate, databaseName);
            }
            if (create)
            {
                Log.InfoFormat("Adding command to create database '{0}' if it doesn't exist", databaseName);
                commands.AppendFormat(CreateTemplate, databaseName);
            }
            ExecuteSql(commands.ToString(), databaseName);
		}

		private static void ExecuteSql(string commandText, string databaseName)
		{
            Log.InfoFormat("Executing SQL {0}", commandText);
			var connectionString = ConfigurationUtility.ReadConnectionString("setup");
			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (var command = new SqlCommand(commandText, connection))
					{
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
                throw new Exception(string.Format("An error occurred while setting up database '{0}'", databaseName), e);
			}
		}
	}
}