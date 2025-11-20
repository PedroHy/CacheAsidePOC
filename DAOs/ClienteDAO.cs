using CacheAsidePOC.Infrastructure.Database;
using CacheAsidePOC.Infrastructure.Database.Connections;
using CacheAsidePOC.Modules.Cliente.DTOs;
using Dapper;

namespace CacheAsidePOC.DAOs
{
    public class ClienteDAO : BaseDAO
    {
        public ClienteDAO(IDatabaseConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<int> InsertAsync(ClientesDTO cliente)
        {
            return await ExecuteWithConnectionAsync(async conn =>
            {
                const string query = @"
                    INSERT INTO cliente (
                        name, 
                        email, 
                        cpf, 
                        cpf_raw) 
                    VALUES (
                        @Name, 
                        @Email, 
                        @Cpf, 
                        @CpfRaw
                    );
                ";
                return await conn.ExecuteScalarAsync<int>(query, cliente);
            });
        }

        public async Task<IEnumerable<ClientesDTO?>> GetAllAsync()
        {
            return await ExecuteWithConnectionAsync(async conn =>
            {
                const string query = @"
                    SELECT
                        id as Id,
                        name as Name,
                        email as Email,
                        cpf as Cpf,
                        cpf_raw as CpfRaw
                    FROM cliente;
                ";

                return await conn.QueryAsync<ClientesDTO>(query);
            });
        }
    }
}
