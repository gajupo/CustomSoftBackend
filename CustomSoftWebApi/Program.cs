using Serilog;
using CustomSoftWebApi.Extensions;

// creating a initial log instance, to record messages before the host starts
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.ConfigureSerilog();

    builder.ConfigureDB();

    builder.ConfigureInfrastructure();

    builder.ConfigureMappings();

    builder.ConfigureMediatR();

    builder.ConfigureApiKeyAuth();
    
    builder.ConfigureUploadFormOptions();
    
    builder.Services.AddControllers();

        
    // build the host
    await using var app = builder.Build();

    // middlewares section
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // runing the app async
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