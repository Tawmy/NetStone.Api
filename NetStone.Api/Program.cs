using System.Text.Json.Serialization;
using NetStone.Api;
using NetStone.Api.Components;
using NetStone.Api.Extensions;
using NetStone.Api.HealthChecks;
using NetStone.Cache;
using NetStone.Cache.Db;
using NetStone.Common.Extensions;
using NetStone.Data;
using NetStone.Queue;
using DependencyInjection = NetStone.Data.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureSwagger();

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(DatabaseContext).Assembly,
    typeof(DependencyInjection).Assembly);

var version = typeof(Program).Assembly.GetName().Version!;
builder.Services.AddSingleton(version);
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddDataProtection(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<DatabaseContext>()
    .AddCheck<DataProtectionCertificateHealthCheck>("cert");

builder.Services.AddCacheServices();
await builder.Services.AddDataServices();
builder.Services.AddQueueServices(builder.Configuration);

var metricsActive = builder.AddOtelMetrics(builder.Configuration);
var tracingActive = builder.AddOtelTracing(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddAuthentication();

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // build a swagger endpoint for each discovered API version -> reversed so most recent is on top
    foreach (var description in app.DescribeApiVersions().Reverse())
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        var name = description.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name);
        options.EnableDeepLinking();
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
}

app.UseExceptionHandler();
app.MapControllers();

if (metricsActive)
{
    app.MapPrometheusScrapingEndpoint();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Logger.LogInformation("NetStone API, version {v}", version.ToVersionString());

if (metricsActive)
{
    app.Logger.LogInformation("Metrics active. Metrics endpoint: {a}", "/metrics");
}

if (tracingActive)
{
    app.Logger.LogInformation("Tracing active. OTLP Endpoint: {a}",
        builder.Configuration.GetGuardedConfiguration(EnvironmentVariables.OtelEndpointUri));
}

app.Run();