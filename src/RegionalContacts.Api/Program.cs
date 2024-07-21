using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using RegionalContacts.Api;
using RegionalContacts.Domain.Interfaces.Repositories;
using RegionalContacts.Infrastructure.Repositories.Redis;
using RegionalContacts.Infrastructure.Repositories.SqlServer;
using RegionalContacts.Infrastructure.Repositories.SqlServer.Configurations;
using RegionalContacts.Service;
using RegionalContacts.Service.Services.Interfaces;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração: Carregar appsettings e arquivos específicos para o ambiente
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

// Configuração Redis
builder.Services.AddRedis(configuration);

// Logging e outros serviços
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

// Configuração do DbContext
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

/* INICIO DA CONFIGURAÇÃO - PROMETHEUS */
app.UseMetricServer();
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});
/* FIM DA CONFIGURAÇÃO - PROMETHEUS */

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseLoggingApi();
app.MapControllers();
app.UseMetricServer();
app.UseHttpMetrics();

app.Run();

public partial class Program { }
