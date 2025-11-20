using CacheAsidePOC.DAOs;
using CacheAsidePOC.Infrastructure.Cache;
using CacheAsidePOC.Infrastructure.Caching;
using CacheAsidePOC.Infrastructure.Database.Connections;
using CacheAsidePOC.Modules.Cliente.DTOs;

namespace CacheAsidePOC.Modules.Cliente
{
    public class ClienteCreateService
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly CachingService _cachingService;

        public ClienteCreateService(PostgresConnection connection, CachingService cachingService)
        {
            _clienteDAO = new ClienteDAO(connection);
            _cachingService = cachingService;
        }

        public async Task AdicionarCliente(ClientesDTO clienteDTO)
        {
            try
            {
                await _clienteDAO.InsertAsync(clienteDTO);

                // invalidação do cache
                await _cachingService.RemoveAsync(CacheKeys.ClientesAll);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
