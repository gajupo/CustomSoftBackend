using Serilog;
using CustomSoftWebApi.Extensions;
using CustomSoftWebApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

    // global unhlandel exception in case some controller or action does not handle some error
    app.UseExceptionHandler(a => a.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature.Error;

        var problemDetails = new ProblemDetails
        {
            Type = "Global.error",
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred!",
            Detail = exception.Message,
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }));

    // middlewares section
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles();
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