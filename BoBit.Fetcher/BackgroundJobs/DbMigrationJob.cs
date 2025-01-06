
using BoBit.Fetcher.Data;
using FluentMigrator.Runner;

namespace BoBit.Fetcher.BackgroundJobs
{
    public class DbMigrationJob(IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            using var scope = serviceProvider.CreateScope();

            var seedDb = scope.ServiceProvider.GetRequiredService<SeedDb>();
            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            
            for (int i = 0; i < 5; i++) 
            {
                try
                {
                    seedDb.CreateDatabaseIfNoExist();
                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                    logger.LogInformation("Migrated");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "migration error, attempt: {attempt}", i);
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
            
        }
    }
}
