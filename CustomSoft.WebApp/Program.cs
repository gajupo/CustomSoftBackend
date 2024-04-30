using CustomSoft.WebApp.Server.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // policy.WithOrigins("https://localhost:5173", "http://localhost:5173");
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

// configure the limit of the body lenght, to support bigger files
builder.Services.Configure<KestrelServerOptions>(opt =>
{
    // default size is 256MB, set in the appsettings.json
    opt.Limits.MaxRequestBodySize = builder.Configuration.GetValue<long>("FileSizeLimit");
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiKey = builder.Configuration.GetValue<string>("ApiKey");
builder.Services.AddHttpClient("CustomSoftApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7231/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("x-api-key", apiKey);
});

builder.Services.AddTransient<IWebApiExecuter, WebApiExecuter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors(MyAllowSpecificOrigins);

app.Run();
