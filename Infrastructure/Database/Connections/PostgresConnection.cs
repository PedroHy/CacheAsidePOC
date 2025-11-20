using Npgsql;
using System.Data;

namespace CacheAsidePOC.Infrastructure.Database.Connections
{
    public class PostgresConnection : IDatabaseConnection
    {
        private readonly string _postgresConnectionString;
        private readonly ILogger<PostgresConnection> _logger;
        private static NpgsqlDataSourceBuilder? _dataSourceBuilder;
        private static NpgsqlDataSource? _dataSource;

        public PostgresConnection(string postgresConnectionString, ILogger<PostgresConnection> logger)
        {
            _postgresConnectionString = postgresConnectionString;
            _logger = logger;
            InitializeDataSource();
        }


        private void InitializeDataSource()
        {
            if (_dataSource == null)
            {
                lock (typeof(PostgresConnection))
                {
                    if (_dataSource == null)
                    {
                        _dataSourceBuilder = new NpgsqlDataSourceBuilder(_postgresConnectionString);
                        _dataSourceBuilder.ConnectionStringBuilder.MinPoolSize = 10;
                        _dataSourceBuilder.ConnectionStringBuilder.MaxPoolSize = 100;
                        _dataSourceBuilder.ConnectionStringBuilder.Pooling = true;
                        _dataSourceBuilder.ConnectionStringBuilder.ConnectionLifetime = 0;
                        _dataSourceBuilder.ConnectionStringBuilder.ConnectionIdleLifetime = 300;
                        _dataSourceBuilder.ConnectionStringBuilder.CommandTimeout = 60;
                        _dataSourceBuilder.ConnectionStringBuilder.IncludeErrorDetail = true;
                        _dataSource = _dataSourceBuilder.Build();
                    }
                }
            }
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                if (_dataSource != null)
                    return _dataSource.CreateConnection();
                else throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar conexão com o banco de dados");
                throw;
            }
        }
    }
}
