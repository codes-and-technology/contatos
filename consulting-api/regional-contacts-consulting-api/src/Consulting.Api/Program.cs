using CacheGateway;
using ConsultingController;
using ConsultingInterface.CacheGateway;
using ConsultingInterface.Controllers;
using ConsultingInterface.DataBase;
using ConsultingInterface.DbGateway;
using DataBase.SqlServer;
using DBGateway;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Redis;
using System.Text.Json.Serialization;
using Prometheus;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração: Carregar appsettings e arquivos específicos para o ambiente
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        InstallServices(builder, configuration);

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contatos", Version = "v1" });
        });
        builder.Services.UseHttpClientMetrics();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
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
        app.MapControllers();

        app.Run();
    }

    private static void InstallServices(WebApplicationBuilder builder, IConfigurationRoot? configuration)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configuração do DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            }, ServiceLifetime.Scoped);

        builder.Services.AddRedis(configuration);

        builder.Services.AddScoped<IConsultingContactController, ConsultingContactController>();
        builder.Services.AddScoped<ICache, Cache>();
        builder.Services.AddScoped<IDb, Db>();
        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
    }
}