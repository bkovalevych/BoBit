using Microsoft.Data.SqlClient;
using System.Data;

namespace BoBit.Fetcher.Data
{
    public class DapperContext(IConfiguration _configuration)
    {
        
        public IDbConnection CreateConnection()
            => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        public IDbConnection CreateMasterConnection()
            => new SqlConnection(_configuration.GetConnectionString("MasterConnection"));
    }
}
