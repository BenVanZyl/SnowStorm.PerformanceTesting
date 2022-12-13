using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using SnowStorm;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.File(path: @"C:\Dev\PerformanceTesting\PerformanceTesting\Server\Logs\logs.txt", rollingInterval: RollingInterval.Minute, retainedFileCountLimit: 10)
            .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    builder.Services.AddHttpContextAccessor();

    int poolSize = 32;
    StringBuilder connectionString = new();
    connectionString.Append("Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true;");
    connectionString.Append("Enlist=true;");
    connectionString.Append($"Pooling=true;Min Pool Size=1;Max Pool Size={poolSize};");
    connectionString.Append("Timeout=35;");
    connectionString.Append("Connection Lifetime=1;");
    //connectionString.Append("ConnectRetryCount=5;ConnectRetryInterval=5");

    builder.Services.AddSnowStorm("PerformanceTesting.Server", connectionString.ToString(), poolSize: poolSize);

    //swagger
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SnowStorm Performance Testing API V1");
    });


    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

