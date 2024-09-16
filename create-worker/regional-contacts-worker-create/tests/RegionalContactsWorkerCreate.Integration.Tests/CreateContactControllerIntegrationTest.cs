using CreateController;
using CreateEntitys;
using CreateInterface.Controllers;
using CreateInterface.DataBase;
using CreateInterface.UseCase;
using CreateUseCases.UseCase;
using DataBase.SqlServer;
using DataBase.SqlServer.Configurations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rabbit.Consumer.Create;
using Redis;
using RegionalContactsWorkerCreate.Integration.Tests.Setup;

namespace RegionalContactsWorkerCreate.Integration.Tests
{
    public class CreateContactControllerIntegrationTest : IClassFixture<DockerFixture>
    {
        private DockerFixture _fixture;
        private readonly IHost _host;

        public CreateContactControllerIntegrationTest(DockerFixture dockerFixture)
        {
            _fixture = dockerFixture;

            // Configura o Host para inicializar o Worker com o ambiente "Testing"
            _host = Host.CreateDefaultBuilder()
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
                    services.AddScoped<ICreateContactController, CreateContactController>();
                    services.AddScoped<ICreateContactUseCase, CreateContactUseCase>();

                    services.AddMassTransitHostedService();

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

        [Theory]
        [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
        public async Task When_AddAsync_ShouldBe_Ok(string email, string phone, short region, string name)
        {
            await _host.StartAsync();
            await Task.Delay(20000); // Esperar os container subirem

            var publisher = _host.Services.GetRequiredService<IPublishEndpoint>();
            var contactToInsert = new ContactEntity
            {
                CreatedDate = DateTime.Now,
                Email = email,
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phone,
                PhoneRegion = new PhoneRegionEntity
                {
                    RegionNumber = region
                }
            };

            await publisher.Publish(contactToInsert);

            // Simular um tempo de processamento e parar o host
            await Task.Delay(10000); // Esperar o Worker processar a mensagem

            var contactRepository = _host.Services.GetRequiredService<IContactRepository>();
            var regionNumberRepository = _host.Services.GetRequiredService<IPhoneRegionRepository>();

            var insertedContact = await contactRepository.FindByIdAsync(contactToInsert.Id);
            insertedContact.PhoneRegion = await regionNumberRepository.GetByRegionNumberAsync(contactToInsert.PhoneRegion.RegionNumber);

            await _host.StopAsync();

            Assert.NotNull(insertedContact);
            Assert.Equal(contactToInsert.Id, insertedContact.Id);
            Assert.Equal(contactToInsert.Name, insertedContact.Name);
            Assert.Equal(contactToInsert.Email, insertedContact.Email);
            Assert.Equal(contactToInsert.PhoneNumber, insertedContact.PhoneNumber);
            Assert.NotNull(insertedContact.PhoneRegion);
            Assert.Equal(contactToInsert.PhoneRegion.RegionNumber, insertedContact.PhoneRegion.RegionNumber);
        }
    }
}