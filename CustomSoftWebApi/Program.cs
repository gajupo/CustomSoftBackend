using Application.Mappers;
using Infrastructure.Config;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Core;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDatabaseService, DatabaseService>(provider => new DatabaseService(connectionString));
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();

// register mapper profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Handlers.QueryHandlers.GetProveedorListHandler).Assembly));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
