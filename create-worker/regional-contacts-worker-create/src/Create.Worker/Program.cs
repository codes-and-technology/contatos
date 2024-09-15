using Create.Worker;
using CreateController;
using CreateInterface.Controllers;
using CreateInterface.DataBase;
using CreateInterface.UseCase;
using CreateUseCases.UseCase;
using DataBase.SqlServer;
using DataBase.SqlServer.Configurations;
using Microsoft.EntityFrameworkCore;
using Rabbit.Consumer.Create;
using Redis;

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
        builder.Services.AddScoped<ICreateContactController, CreateContactController>();
        builder.Services.AddScoped<ICreateContactUseCase, CreateContactUseCase>();

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