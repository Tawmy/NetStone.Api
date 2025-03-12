using System.Text.Json.Serialization;
using NetStone.Api.Components;
using NetStone.Api.Extensions;
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
builder.Services.AddCacheServices();
await builder.Services.AddDataServices();
builder.Services.AddQueueServices(builder.Configuration);

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

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Logger.LogInformation("NetStone API, version {v}", version.ToVersionString());

app.Run();