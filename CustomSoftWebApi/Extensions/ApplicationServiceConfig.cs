using Infrastructure.Config;
using Infrastructure.Repositories.Core;
using Infrastructure.Repositories;
using Application.Mappers;
using Authentication.ApiKeys.Abstractions;
using Authentication.ApiKeys;
using Common;
using Serilog;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;


namespace CustomSoftWebApi.Extensions
{
    public static class ApplicationServiceConfig
    {
        public static void ConfigureDB(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddScoped<IDatabaseService, DatabaseService>(provider => new DatabaseService(connectionString));
        }

        public static void ConfigureInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
            builder.Services.AddScoped<IDBArchivoRepository, DBArchivoRepository>();
            builder.Services.AddScoped<IDiskArchivoRepository, DiskArchivoRepository>();

        }

        public static void ConfigureMappings(this WebApplicationBuilder builder)
        {
            // register mapper profile
            builder.Services.AddAutoMapper(typeof(MappingProfile));
        }

        public static void ConfigureMediatR(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Handlers.QueryHandlers.GetProveedorListHandler).Assembly));
        }

        public static void ConfigureApiKeyAuth(this WebApplicationBuilder builder)
        {
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
        }

        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());
        }

        public static void ConfigureUploadFormOptions(this WebApplicationBuilder builder)
        {
            // configure the limit of the body lenght, to support bigger files
            builder.Services.Configure<KestrelServerOptions>(opt =>
            {
                // default size is 256MB, set in the appsettings.json
                opt.Limits.MaxRequestBodySize = builder.Configuration.GetValue<long>("FileSizeLimit");
            });
        }
    }
}
