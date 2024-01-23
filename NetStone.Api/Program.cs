using NetStone;
using NetStone.Api.Interfaces;
using NetStone.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<LodestoneClient>(sp =>
{
    // Assuming that GetClientAsync returns a Task<LodestoneClient>
    var clientTask = LodestoneClient.GetClientAsync();
    clientTask.Wait();
    return clientTask.Result;
});

builder.Services.AddTransient<ICharacterService, CharacterService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();