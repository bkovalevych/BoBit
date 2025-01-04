using BoBit.Fetcher.Data;
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

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<SeedDb>();
builder.Services.AddLogging(x => x.AddFluentMigratorConsole())
    .AddFluentMigratorCore()
    .ConfigureRunner(x => x.AddSqlServer2016()
        .WithGlobalConnectionString(
        builder.Configuration.GetConnectionString("MasterConnection"))
        .ScanIn(Assembly.GetExecutingAssembly())
        .For
        .Migrations());

builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name:"db", tags:["db"])
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.MigrateDb();
app.Run();
