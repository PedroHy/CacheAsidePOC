using CacheAsidePOC.DAOs;
using CacheAsidePOC.Infrastructure.Cache;
using CacheAsidePOC.Infrastructure.Caching;
using CacheAsidePOC.Infrastructure.Database.Connections;
using CacheAsidePOC.Modules.Cliente.DTOs;
using System.Text.Json;

namespace CacheAsidePOC.Modules.Cliente
{
    public class ClientesQueryService
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly CachingService _cachingService;

        public ClientesQueryService(PostgresConnection connection, CachingService cachingService)
        {
            _clienteDAO = new ClienteDAO(connection);
            _cachingService = cachingService;
        }

        public async Task<IEnumerable<ClientesDTO?>> ListarClientes()
        {
            try
            {
                // Buscar clientes no cache
                var cached = await _cachingService.GetAsync(CacheKeys.ClientesAll);
                if (!string.IsNullOrWhiteSpace(cached))
                {
                    var fromCache = JsonSerializer.Deserialize<IEnumerable<ClientesDTO>>(cached);
                    if (fromCache is not null)
                        return fromCache;
                }

                // Buscar clientes no db
                var clientes = await _clienteDAO.GetAllAsync();
                var list = clientes?.ToList() ?? new List<ClientesDTO?>();

                var json = JsonSerializer.Serialize(list);
                await _cachingService.SetAsync(CacheKeys.ClientesAll, json);

                return list;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
