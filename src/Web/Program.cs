using Core.Interfaces;
using Core.Services;
using Infra.FunTranslations;
using Infra.PokeApi;
using Prometheus;
using Serilog;
using Serilog.Debugging;

ConfigureLogging();

ConfigureMetrics();

try
{
    var builder = WebApplication.CreateBuilder(args);
    ConfigureBuilder(builder);

    var app = builder.Build();
    ConfigureRequestPipeline(app);

    Log.Information("Starting web host");
    app.Run();
    Log.Information("Shutting down");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


static void ConfigureRequestPipeline(WebApplication app)
{
    app.Map("/metrics", app => app.UseRouting().UseEndpoints(x => x.MapMetrics("")));
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.DefaultModelsExpandDepth(-1);
        x.DisplayRequestDuration();
    });
    app.UseSerilogRequestLogging();
    app.UseHttpMetrics();
    app.MapControllers();
}

static void ConfigureBuilder(WebApplicationBuilder builder)
{
    builder.WebHost.UseKestrel(x => x.AddServerHeader = false);
    builder.Host.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromMinutes(1))
        .UseSerilog();


    builder.Services.AddRouting(x => x.LowercaseUrls = true);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IPokemonService, PokemonService>();
    builder.Services.AddScoped<IPokemonInfoProvider, PokemonInfoProvider>();
    builder.Services.AddScoped<ITranslator, FunTranslator>();
    builder.Services.AddHttpClient();

}

static void ConfigureMetrics()
{
    Metrics.SuppressDefaultMetrics();
}

static void ConfigureLogging()
{
    var cfg = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: false)
             .AddEnvironmentVariables()
             .Build();
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(cfg).CreateLogger();
}

namespace Web
{
    //needed for https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
    public partial class Program { }
}