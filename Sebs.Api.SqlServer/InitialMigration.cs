using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Sebs.Api.SqlServer.Data.Contexts;
using Sebs.Api.SqlServer.Data.Entities;

namespace Sebs.Api.SqlServer
{
    public static class InitialMigration
    {

        public static IServiceCollection MigrateDatabase<TContext>(this IServiceCollection services)
             where TContext : SampleContext
        {

            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
            var context = serviceProvider.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                            retryCount: 5,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                            onRetry: (exception, retryCount, context) =>
                            {
                                logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                            });

                // if the sql server container is not created on run docker compose this
                // migration can't fail for network related exception. The retry options for DbContext only 
                // apply to transient exceptions                    
                retry.Execute(async () =>
                    await InvokeSeederAsync(context, serviceProvider));

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }


            return services;
        }

        private static async Task InvokeSeederAsync(SampleContext context, IServiceProvider services)
        {
            context.Database.Migrate();

            var logger = services.GetService<ILogger<SampleContext>>();

            if (!context.Samples.Any())
            {
                context.Samples.Add(new SampleEntity()
                {
                    Name = "sebs",
                });
                await context.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(Context).Name);
            }
        }
    }
}