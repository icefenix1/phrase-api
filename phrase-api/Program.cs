using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(configure => configure
        .AddConsole()
        .AddApplicationInsights());
builder.Services.AddHttpClient();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Configuration.AddAzureAppConfiguration(config => config
        .Connect(Environment.GetEnvironmentVariable("APPCONFIG"))
        .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential())));

builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = builder.Configuration.GetSection("insights").Value,
            configureApplicationInsightsLoggerOptions: (options) => { }
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(config =>
            config.ConnectionString = builder.Configuration.GetSection("insights").Value
    );

var app = builder.Build();

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
