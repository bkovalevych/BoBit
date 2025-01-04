using BoBit.Fetcher.Configs;
using BoBit.Fetcher.Data;
using BoBit.Fetcher.Interfaces;
using BoBit.Fetcher.Services;
using FluentMigrator.Runner;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
    .AddTransient<IBitcoinService, BitcoinService>();

builder.Services
    .Configure<FetchSettings>(builder.Configuration.GetSection(nameof(FetchSettings)));

builder.Services.AddLogging(x => x.AddFluentMigratorConsole())
    .AddFluentMigratorCore()
    .ConfigureRunner(x => x.AddSqlServer2016()
        .WithGlobalConnectionString(masterConnectionString)
        .ScanIn(Assembly.GetExecutingAssembly())
        .For
        .Migrations());

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name:"db", tags:["db"])
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

var app = builder.Build();


app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.MigrateDb();
app.Run();
