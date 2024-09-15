using UpdateController;
using UpdateInterface;
using Microsoft.OpenApi.Models;
using QueueGateway;
using Rabbit.Producer.Update;
using ExternalInterfaceGateway;
using External.Interfaces;

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


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        InstallServices(builder, configuration);

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contatos", Version = "v1" });
        });

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
    }

    private static void InstallServices(WebApplicationBuilder builder, IConfigurationRoot configuration)
    {
        builder.Services.AddRabbitMq(configuration);
        builder.Services.AddRefitServiceExtension(configuration);

        builder.Services.AddScoped<IController, UpdateContactController>();
        builder.Services.AddScoped<IContactProducer, ContactProducer>();
        builder.Services.AddScoped<IContactQueueGateway, ContactQueueGateway>();
        builder.Services.AddScoped<IContactConsultingGateway, ContactConsultingGateway>();
    }
}