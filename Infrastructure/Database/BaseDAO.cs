using CacheAsidePOC.Infrastructure.Database.Connections;
using System.Data;

namespace CacheAsidePOC.Infrastructure.Database
{
    public abstract class BaseDAO
    {
        protected readonly IDatabaseConnection _dbConnection;

        public BaseDAO(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        protected IDatabaseConnection getDbConnection => _dbConnection;

        protected async Task<T> ExecuteWithConnectionAsync<T>(Func<IDbConnection, Task<T>> operation)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                return await operation(connection);
            }
        }

        protected async Task<T> ExecuteWithConnectionAsync<T>(IDbConnection externalConnection,
            Func<IDbConnection, Task<T>> operation)
        {
            if (externalConnection == null)
                return await ExecuteWithConnectionAsync(operation);

            return await operation(externalConnection);
        }

        protected async Task ExecuteWithConnectionAsync(Func<IDbConnection, Task> operation)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                await operation(connection);
            }
        }

        protected async Task ExecuteWithConnectionAsync(IDbConnection externalConnection,
            Func<IDbConnection, Task> operation)
        {
            if (externalConnection == null)
            {
                await ExecuteWithConnectionAsync(operation);
                return;
            }

            await operation(externalConnection);
        }
    }
}
