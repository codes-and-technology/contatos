using Controller;
using DeleteInterface;
using Microsoft.OpenApi.Models;
using QueueGateway;
using Rabbit.Producer.Delete;
using External.Interfaces;
using ExternalInterfaceGateway;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configura��o: Carregar appsettings e arquivos espec�ficos para o ambiente
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

        builder.Services.AddScoped<IController, DeleteContactController>();
        builder.Services.AddScoped<IContactProducer, ContactProducer>();
        builder.Services.AddScoped<IContactQueueGateway, ContactQueueGateway>();
        builder.Services.AddScoped<IContactConsultingGateway, ContactConsultingGateway>();
    }
}