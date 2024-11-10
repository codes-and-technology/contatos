using External.Interfaces;
using ExternalInterfaceGateway;
using Microsoft.OpenApi.Models;
using Prometheus;
using QueueGateway;
using Rabbit.Producer.Update;
using Update.Api.Helpers.Middlewares;
using UpdateController;
using UpdateInterface;

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

        app.UseSwagger();
        app.UseSwaggerUI();

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

        // Configurar endpoints de sa�de
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/readiness");

        app.Run();
    }

    private static void InstallServices(WebApplicationBuilder builder, IConfigurationRoot configuration)
    {
        builder.Services.AddLogging(builder => builder.AddConsole());
        builder.Services.AddHealthChecks();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddRabbitMq(configuration);
        builder.Services.AddRefitServiceExtension(configuration);

        builder.Services.AddScoped<IController, UpdateContactController>();
        builder.Services.AddScoped<IContactProducer, ContactProducer>();
        builder.Services.AddScoped<IContactQueueGateway, ContactQueueGateway>();
        builder.Services.AddScoped<IContactConsultingGateway, ContactConsultingGateway>();
    }
}