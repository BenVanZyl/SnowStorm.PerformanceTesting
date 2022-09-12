using JkaPerth.Web.Services.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog.Events;
using Serilog;
using SnowStorm;
using System;
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
    connectionString.Append($"Pooling=true;Min Pool Size=1;Max Pool Size={poolSize*2};");
    connectionString.Append("Timeout=35;");
    connectionString.Append("Connection Lifetime=1;");  
    //connectionString.Append("ConnectRetryCount=5;ConnectRetryInterval=5");
    builder.Services.AddSnowStorm(connectionString.ToString(), poolSize: poolSize);

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

