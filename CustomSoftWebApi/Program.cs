using Application.Mappers;
using Authentication.ApiKeys.Abstractions;
using Authentication.ApiKeys;
using Infrastructure.Config;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Core;
using Common;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

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

    await using var app = builder.Build();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();

    Log.Information("Api stopped");
    return 0;

}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}