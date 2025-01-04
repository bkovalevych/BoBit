using Dapper;

namespace BoBit.Fetcher.Data
{
    public class SeedDb(DapperContext _ctx)
    {
        public void CreateDatabaseIfNoExist()
        {
            using var defaultConnection = _ctx.CreateConnection();
            var dbName = defaultConnection.Database;
            var query = "SELECT * FROM sys.databases WHERE name = @name";
            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);
            using var connection = _ctx.CreateMasterConnection();
            
            var records = connection.Query(query, parameters);

            if (!records.Any())
            {
                connection.Execute($"CREATE DATABASE {dbName}");
            }

        }
    }
}
