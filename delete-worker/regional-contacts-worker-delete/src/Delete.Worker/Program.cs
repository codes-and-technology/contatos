using CacheGateways;
using DataBase.SqlServer;
using DataBase.SqlServer.Configurations;
using DBGateways;
using Microsoft.EntityFrameworkCore;
using QueueGateways;
using Rabbit.Consumer.Delete;
using Redis;
using Delete.Worker;
using DeleteController;
using DeleteInterface.Controllers;
using DeleteInterface.DataBase;
using DeleteInterface.Gateway.Cache;
using DeleteInterface.Gateway.DB;
using DeleteInterface.Gateway.Queue;
using DeleteInterface.UseCase;
using DeleteUseCases.UseCase;
using Microsoft.AspNetCore.Builder;
using Prometheus;
using Microsoft.Extensions.Diagnostics.HealthChecks;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração: Carregar appsettings e arquivos específicos para o ambiente
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        builder.Services.AddRabbitMq(configuration);
        builder.Services.AddRedis(configuration);

        builder.Services.UseHttpClientMetrics();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
        builder.Services.AddScoped<IContactDBGateway, ContactDBGateway>();
        builder.Services.AddScoped<IPhoneRegionDBGateway, PhoneRegionDBGateway>();
        builder.Services.AddScoped(typeof(ICacheGateway<>), typeof(CacheGateway<>));
        builder.Services.AddScoped<IDeleteContactGateway, DeleteContactGateway>();
        builder.Services.AddScoped<IDeleteContactController, DeleteContactController>();
        builder.Services.AddScoped<IDeleteContactUseCase, DeleteContactUseCase>();

        builder.Services.AddHostedService<Worker>();

        // Configuração do DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            }, ServiceLifetime.Scoped);

        // Adiciona health checks
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        var host = builder.Build();

        /* INICIO DA CONFIGURAÇÃO - PROMETHEUS */
        host.UseMetricServer();
        host.UseHttpMetrics(options =>
        {
            options.AddCustomLabel("host", context => context.Request.Host.Host);
        });
        /* FIM DA CONFIGURAÇÃO - PROMETHEUS */

        // Configura os endpoints de health check
        host.MapHealthChecks("/health");
        host.MapHealthChecks("/readiness");

        host.Run();
    }
}