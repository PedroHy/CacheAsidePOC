using System.Data;

namespace CacheAsidePOC.Infrastructure.Database.Connections
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();
    }
}

