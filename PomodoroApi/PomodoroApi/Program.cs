using System.Text.Json.Serialization;
using PomodoroApi.Domain;
using PomodoroApi.Infrastructure;
using PomodoroApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opcoes =>
{
    opcoes.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<ISessaoRepositorio, SessaoRepositorio>();
builder.Services.AddSingleton<IConfiguracaoRepositorio, ConfiguracaoRepositorio>();
builder.Services.AddScoped<SessaoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
