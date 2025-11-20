using CacheAsidePOC.Infrastructure.Cache;
using CacheAsidePOC.Infrastructure.Database.Connections;
using CacheAsidePOC.Modules.Cliente;
using CacheAsidePOC.Modules.Cliente.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
    var connectionString = builder.Configuration.GetSection("PostgresConnection").Value;
    var logger = sp.GetRequiredService<ILogger<PostgresConnection>>();
    if (connectionString == null) throw new ArgumentException();
    return new PostgresConnection(connectionString, logger);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis").Value;
    options.InstanceName = "CacheAsidePOC:";
});

builder.Services.AddScoped<CachingService>();
builder.Services.AddScoped<ClientesQueryService>();
builder.Services.AddScoped<ClienteCreateService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/clientes", async (ClientesQueryService service) =>
{
    var clientes = await service.ListarClientes();
    return Results.Ok(clientes);
})
.WithName("ListarClientes")
.Produces<IEnumerable<ClientesDTO>>(StatusCodes.Status200OK);

app.MapPost("/clientes", async (ClientesDTO dto, ClienteCreateService service) =>
{
    await service.AdicionarCliente(dto);
    return Results.Created($"/clientes", dto);
})
.WithName("CriarCliente")
.Produces<ClientesDTO>(StatusCodes.Status201Created);


app.UseSwagger();
app.UseSwaggerUI();
app.Urls.Add("http://0.0.0.0:5000");

app.Run();
