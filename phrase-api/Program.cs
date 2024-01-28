using Azure.Identity;
using phrase_api.Contracts.Workers;
using phrase_api.Workers;
using phrase_api.Repos;
using phrase_api.Contracts.Repos;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var context = builder.Configuration;

context.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


// Add services to the container.
services.AddLogging(configure => configure
        .AddConsole()
        .AddApplicationInsights());

services.AddHttpClient();

builder.Logging.AddConfiguration(context.GetSection("Logging"));

context.AddAzureAppConfiguration(config => config
        .Connect(Environment.GetEnvironmentVariable("APPCONFIG"))
        .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential())));

builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = context.GetSection("insights").Value,
            configureApplicationInsightsLoggerOptions: (options) => { }
    );

services.AddSingleton<IMongoClient>(new MongoClient(context.GetConnectionString("Mongo")));

services.AddTransient<IPhraseRepository, PhraseRepository>();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddApplicationInsightsTelemetry(config =>
            config.ConnectionString = context.GetSection("insights").Value
    );

services.AddTransient<IWordsWoker, WordsWorker>();
services.AddTransient<IPhraseWorker, PhraseWorker>();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
               builder =>
               {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
