using FluentMigrator.Runner;

namespace BoBit.Fetcher.Data
{
    public static class MigrationExtensions
    {
        public static IHost MigrateDb(this IHost host) 
        {
            using var scope = host.Services.CreateScope();
            
            var seedDb = scope.ServiceProvider.GetRequiredService<SeedDb>();
            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            
            try
            {
                seedDb.CreateDatabaseIfNoExist();
                migrationService.ListMigrations();
                migrationService.MigrateUp();
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "migration error");
            }
            

            return host;
        }
    }
}
