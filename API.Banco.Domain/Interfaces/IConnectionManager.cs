using System.Data;

namespace API.Banco.Domain.Interfaces
{
    public interface IConnectionManager
    {
        IDbConnection GetConnection(string key);
    }
}
