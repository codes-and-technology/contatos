using CacheGateways;
using CreateController;
using CreateInterface.Controllers;
using CreateInterface.DataBase;
using CreateInterface.Gateway.Cache;
using CreateInterface.Gateway.DB;
using CreateInterface.Gateway.Queue;
using CreateInterface.UseCase;
using CreateUseCases.UseCase;
using DataBase.SqlServer;
using DataBase.SqlServer.Configurations;
using DBGateways;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueGateways;
using Rabbit.Consumer.Create;
using Redis;

namespace RegionalContactsWorkerCreate.Integration.Tests.Setup
{
    public class CustomWorkerApplicationFixture : IAsyncLifetime
    {
        public IHost Host { get; private set; }

        public async Task InitializeAsync() => await Task.Run(ConfigureWebHost);

        public async Task DisposeAsync() => await Task.Run(Host.Dispose);

        public void ConfigureWebHost()
        {
            // Configura o Host para inicializar o Worker com o ambiente "Testing"
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        hostingContext.HostingEnvironment.EnvironmentName = "Testing";

                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.Testing.json")
                                .Build();

                        // Configura o banco de dados para testes
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            var connectionString = configuration.GetConnectionString("ConnectionString");
                            options.UseSqlServer(connectionString, sqlServerOptions =>
                            {
                                sqlServerOptions.EnableRetryOnFailure(
                                    maxRetryCount: 5, // número máximo de tentativas
                                    maxRetryDelay: TimeSpan.FromSeconds(10), // atraso máximo entre tentativas
                                    errorNumbersToAdd: null); // códigos de erro adicionais para considerar para retry
                            });
                        });

                        services.AddRabbitMq(configuration);
                        services.AddRedis(configuration);

                        services.AddScoped<IUnitOfWork, UnitOfWork>();
                        services.AddScoped<IContactRepository, ContactRepository>();
                        services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
                        services.AddScoped<IContactDBGateway, ContactDBGateway>();
                        services.AddScoped<IPhoneRegionDBGateway, PhoneRegionDBGateway>();
                        services.AddScoped(typeof(ICacheGateway<>), typeof(CacheGateway<>));
                        services.AddScoped<ICreateContactGateway, CreateContactGateway>();
                        services.AddScoped<ICreateContactController, CreateContactController>();
                        services.AddScoped<ICreateContactUseCase, CreateContactUseCase>();

                        //Realizar o migration
                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            Thread.Sleep(10000);
                            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                            {
                                try
                                {
                                    appContext.Database.EnsureDeleted();
                                    appContext.Database.Migrate();
                                }
                                catch (Exception ex)
                                {
                                    // Log errors or do anything you think it's needed
                                    throw;
                                }
                            }
                        }
                    })
                    .Build();
        }
    }
}
