using Npgsql;
using Polly;

namespace Sebs.Api.PostgreSQL
{
    public static class InitialMigration
    {

        public static IServiceCollection MigrateDatabase<TContext>(this IServiceCollection services)
        {

            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postresql database.");

                var retry = Policy.Handle<NpgsqlException>()
                        .WaitAndRetry(
                            retryCount: 5,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                            onRetry: (exception, retryCount, context) =>
                            {
                                logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                            });

                //if the postgresql server container is not created on run docker compose this
                //migration can't fail for network related exception. The retry options for database operations
                //apply to transient exceptions                    
                retry.Execute(() => ExecuteMigrations(connectionString, logger));

                logger.LogInformation("Migrated postresql database with success!.");

            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the postresql database");
            }


            return services;
        }

        private static void ExecuteMigrations(string connectionString, ILogger logger)
        {
            logger.LogInformation("Open connection with Postgres...");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                logger.LogInformation("Connection opened successfully.");

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };
                command.CommandText = "DROP TABLE IF EXISTS Samples";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Samples(Id SERIAL PRIMARY KEY, 
                                                                Name VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                command.ExecuteNonQuery();


                command.CommandText = "INSERT INTO Samples(Name, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Samples(Name, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();
            }
        }
    }
}