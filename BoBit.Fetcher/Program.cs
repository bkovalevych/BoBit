using BoBit.Fetcher.BackgroundJobs;
using BoBit.Fetcher.Configs;
using BoBit.Fetcher.Data;
using BoBit.Fetcher.Interfaces;
using BoBit.Fetcher.Services;
using FluentMigrator.Runner;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new InvalidOperationException(
                       "Could not find a connection string named 'DefaultConnection'.");
}

var masterConnectionString = builder.Configuration.GetConnectionString("MasterConnection");

if (masterConnectionString == null)
{
    throw new InvalidOperationException(
                       "Could not find a connection string named 'MasterConnection'.");
}

builder.Services.AddSingleton<DapperContext>()
    .AddTransient<SeedDb>()
    .AddSingleton<IDateProvider, DateProvider>()
    .AddHostedService<FetcherJob>();

builder.Services
    .Configure<FetchSettings>(builder.Configuration.GetSection(nameof(FetchSettings)));

builder.Services.AddLogging(x => x.AddFluentMigratorConsole())
    .AddFluentMigratorCore()
    .ConfigureRunner(x => x.AddSqlServer2016()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(Assembly.GetExecutingAssembly())
        .For
        .Migrations());

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "db", tags: ["db"])
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

builder.Services.AddHttpClient<IBitcoinService, BitcoinService>()
    .ConfigureHttpClient((services, client) =>
    {
        client.BaseAddress = new Uri(
            services.GetRequiredService<IOptions<FetchSettings>>().Value.Host);
    })
    .AddStandardResilienceHandler();

var app = builder.Build();


app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.MigrateDb();
app.Run();
