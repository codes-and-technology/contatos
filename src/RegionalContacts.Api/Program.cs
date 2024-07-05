using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RegionalContacts.Api;
using RegionalContacts.Domain.Interfaces.Repositories;
using RegionalContacts.Infrastructure.Repositories.SqlServer;
using RegionalContacts.Infrastructure.Repositories.SqlServer.Configurations;
using RegionalContacts.Service;
using RegionalContacts.Service.Services.Interfaces;
using System.Reflection;
using System.Text.Json.Serialization;
using RegionalContacts.Infrastructure.Repositories.Redis;
using Prometheus;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      });

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddRedis(configuration);
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.UseHttpClientMetrics();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contatos", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
    }, ServiceLifetime.Scoped);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contatos API V1");
    });
}

/*INICIO DA CONFIGURAÇÃO - PROMETHEUS*/
// Custom Metrics to count requests for each endpoint and the method
var counter = Metrics.CreateCounter("webapimetric", "Counts requests to the WebApiMetrics API endpoints",
    new CounterConfiguration
    {
        LabelNames = new[] { "method", "endpoint" }
    });

var gauge = Metrics.CreateGauge(
        "myapp_http_request_duration_seconds",
        "Tempo médio de resposta das requisições HTTP em segundos.");

app.Use(async (context, next) =>
{
    counter.WithLabels(context.Request.Method, context.Request.Path).Inc();

    var stopwatch = Stopwatch.StartNew();

    try
    {
        await next();
    }
    finally
    {
        stopwatch.Stop();
        var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

        // Atualizar o medidor de tempo de resposta
        gauge.Set(elapsedSeconds);
    }
});

// Use the prometheus middleware
app.UseMetricServer();
app.UseHttpMetrics();

/*FIM DA CONFIGURAÇÃO - PROMETHEUS*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseLoggingApi();

app.MapControllers();

app.UseMetricServer();
app.UseHttpMetrics();

app.Run();
