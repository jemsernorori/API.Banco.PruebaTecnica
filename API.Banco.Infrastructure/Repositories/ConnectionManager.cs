using API.Banco.Domain.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace API.Banco.Infrastructure.Repositories
{
    public class ConnectionManager : IConnectionManager
    {
        public const string BD_KEY = "BancaDB";
        private readonly IConfiguration _configuration;

        public ConnectionManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection(string key)
        {
            var connectionString = _configuration.GetConnectionString(key);
            return new SqliteConnection(connectionString);
        }
    }
}
