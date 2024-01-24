using System.Configuration;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NetStone;
using NetStone.Api;
using NetStone.Api.Interfaces;
using NetStone.Api.Messages;
using NetStone.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
ConfigureSwagger(builder.Services);

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<LodestoneClient>(sp =>
{
    // Assuming that GetClientAsync returns a Task<LodestoneClient>
    var clientTask = LodestoneClient.GetClientAsync();
    clientTask.Wait();
    return clientTask.Result;
});

builder.Services.AddTransient<ICharacterService, CharacterService>();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

AddAuthentication(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

return;

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options => { options.SupportNonNullableReferenceTypes(); });
    services.ConfigureOptions<ConfigureSwaggerOptions>();
}

void AddAuthentication(IServiceCollection services)
{
    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = Environment.GetEnvironmentVariable(EnvironmentVariables.AuthAuthority) ??
                                throw new ConfigurationErrorsException(
                                    Errors.Environment.EnvironmentVariableNotSet(EnvironmentVariables.AuthAuthority));
            options.Audience = Environment.GetEnvironmentVariable(EnvironmentVariables.AuthAudience) ??
                               throw new ConfigurationErrorsException(
                                   Errors.Environment.EnvironmentVariableNotSet(EnvironmentVariables.AuthAudience));

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