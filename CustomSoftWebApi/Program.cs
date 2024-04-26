using Application.Mappers;
using Authentication.ApiKeys.Abstractions;
using Authentication.ApiKeys;
using Infrastructure.Config;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Core;
using Common;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDatabaseService, DatabaseService>(provider => new DatabaseService(connectionString));
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();

// register mapper profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Handlers.QueryHandlers.GetProveedorListHandler).Assembly));

builder.Services.AddControllers();

builder.Services
    .AddDefaultApiKeyGenerator(new ApiKeyGenerationOptions
    {
        KeyPrefix = "KP-",
        GenerateUrlSafeKeys = true,
        LengthOfKey = 32
    })
    .AddDefaultClaimsPrincipalFactory()
    .AddApiKeys()
    .AddSingleton<IClientsService, InMemoryClientsService>()
    .AddMemoryCache()
    .AddSingleton<IApiKeysCacheService, CacheService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
