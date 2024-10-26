using System.Data;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetStone.Api;
using NetStone.Api.Components;
using NetStone.Cache;
using NetStone.Cache.Db;
using NetStone.Common.Extensions;
using NetStone.Data;
using NetStone.Queue;
using Npgsql;
using DependencyInjection = NetStone.Data.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
ConfigureSwagger(builder.Services);

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(DatabaseContext).Assembly,
    typeof(DependencyInjection).Assembly);

var version = typeof(Program).Assembly.GetName().Version!;
builder.Services.AddSingleton(version);
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddCacheServices();
await builder.Services.AddDataServices();
builder.Services.AddQueueServices(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

AddAuthentication(builder);

var app = builder.Build();

await MigrateDatabaseAsync(app.Services);

// Configure the HTTP request pipeline.

// always make swagger definition available
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
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
}
else
{
    app.UseExceptionHandler("/Error", true);
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions());
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Logger.LogInformation("NetStone API, version {v}", version.ToVersionString());

app.Run();

return;

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();

    services.AddApiVersioning(x =>
        {
            x.ApiVersionReader = new HeaderApiVersionReader("X-API-Version"); // read version from request headers
            x.AssumeDefaultVersionWhenUnspecified = true; // assume V1 if request is sent without version
            x.ReportApiVersions = true; // respond with supported versions in response header
        })
        .AddMvc()
        .AddApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

    services.AddSwaggerGen(options =>
    {
        options.SupportNonNullableReferenceTypes();
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
            $"{typeof(Program).Assembly.GetName().Name}.xml"));
        options.CustomSchemaIds(type => type.ToString());
    });
    services.ConfigureOptions<ConfigureSwaggerOptions>();
}

void AddAuthentication(WebApplicationBuilder webAppBuilder)
{
    webAppBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = webAppBuilder.Configuration.GetGuardedConfiguration(EnvironmentVariables.AuthAuthority);
            options.Audience = webAppBuilder.Configuration.GetGuardedConfiguration(EnvironmentVariables.AuthAudience);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateActor = true,
                ValidateAudience = true,
                ValidateTokenReplay = true
            };
        });
}

async Task MigrateDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    await dbContext.Database.MigrateAsync();

    if (dbContext.Database.GetDbConnection() is NpgsqlConnection npgsqlConnection)
    {
        if (npgsqlConnection.State != ConnectionState.Open)
        {
            await npgsqlConnection.OpenAsync();
        }

        try
        {
            await npgsqlConnection.ReloadTypesAsync();
        }
        finally
        {
            await npgsqlConnection.CloseAsync();
        }
    }
}