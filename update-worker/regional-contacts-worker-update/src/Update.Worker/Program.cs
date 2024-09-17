using CacheGateways;
using DataBase.SqlServer;
using DataBase.SqlServer.Configurations;
using DBGateways;
using Microsoft.EntityFrameworkCore;
using Rabbit.Consumer.Update;
using Redis;
using Update.Worker;
using UpdateController;
using UpdateInterface.Controllers;
using UpdateInterface.DataBase;
using UpdateInterface.Gateway.Cache;
using UpdateInterface.Gateway.DB;
using UpdateInterface.UseCase;
using UpdateUseCases.UseCase;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Configuração: Carregar appsettings e arquivos específicos para o ambiente
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        builder.Services.AddRabbitMq(configuration);
        builder.Services.AddRedis(configuration);

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
        builder.Services.AddScoped<IContactDBGateway, ContactDBGateway>();
        builder.Services.AddScoped<IPhoneRegionDBGateway, PhoneRegionDBGateway>();
        builder.Services.AddScoped(typeof(ICacheGateway<>), typeof(CacheGateway<>));
        builder.Services.AddScoped<IUpdateContactController, UpdateContactController>();
        builder.Services.AddScoped<IUpdateContactUseCase, UpdateContactUseCase>();

        builder.Services.AddHostedService<Worker>();

        // Configuração do DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            }, ServiceLifetime.Scoped);

        var host = builder.Build();
        host.Run();
    }
}