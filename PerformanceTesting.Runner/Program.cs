// See https://aka.ms/new-console-template for more information
using PerformanceTester;
using Serilog;
using SnowStorm.Extensions;
using System.Net.Http.Headers;

Helper.Write("******************************");

Helper.Write("Hello, World!");

Helper.Write("Setup logging ...");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.File(path: @"C:\Dev\PerformanceTesting\PerformanceTesting.Runner\Logs\logs.txt", rollingInterval: RollingInterval.Minute, retainedFileCountLimit: 10)
    .CreateLogger();
Helper.Write("logging setup done.");

string rootPath = "https://localhost:44330/";

//rootPath = Helper.Read("Provide root url:");

Helper.Write("Targets:");
Helper.Write("  0 : Get without db");
Helper.Write("  1 : Get without db WITG Caching");
Helper.Write("  2 : Get");
Helper.Write("  3 : Post");
Helper.Write("  4 : Get with payload");
Helper.Write("  5 : Post with payload");
Helper.Write("  6 : Get Direct");
Helper.Write("  7 : Combined");
string target = Helper.Read("Select target:");

bool isRunning = true;
while (isRunning)
{
    _ = int.TryParse(Helper.Read("How many workers?: "), out int workerCount);

    WorkerManager w = new(rootPath, target, workerCount);
    await w.RunAsync();

    string again = Helper.Read("Run again? Y/N:");
    if (again.HasValue() && again.ToUpper() != "Y")
        isRunning = false;
}

Helper.Write("******************************");