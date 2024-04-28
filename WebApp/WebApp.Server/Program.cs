using WebApp.Server.Data;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:5173", "http://localhost:5173");
                      });
});
builder.Services.AddControllers();

var apiKey = builder.Configuration.GetValue<string>("ApiKey");
builder.Services.AddHttpClient("CustomSoftApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7231/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("x-api-key", apiKey);
});

builder.Services.AddTransient<IWebApiExecuter, WebApiExecuter>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);

app.MapFallbackToFile("/index.html");

app.Run();
