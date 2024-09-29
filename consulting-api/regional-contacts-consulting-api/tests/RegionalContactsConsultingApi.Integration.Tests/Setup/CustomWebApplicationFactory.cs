using ConsultingEntitys;
using DataBase.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presenters;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;

namespace RegionalContactsConsultingApi.Integration.Tests.Setup;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Configura o ambiente de teste
            context.HostingEnvironment.EnvironmentName = "Testing";

            // Adiciona o arquivo de configuração de teste
            config.AddJsonFile("appsettings.Testing.json", optional: true, reloadOnChange: true);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Testing.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 5, // número máximo de tentativas
                        maxRetryDelay: TimeSpan.FromSeconds(10), // atraso máximo entre tentativas
                        errorNumbersToAdd: null); // códigos de erro adicionais para considerar para retry
                });
            });

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
                var list = new List<ContactDto>
                {
                    new ContactDto
                    {
                        Email = "teste@teste.com",
                        Id = Guid.NewGuid().ToString(),
                        Name = "Teste",
                        PhoneNumber = "4123412",
                        RegionNumber = 13
                    }
                };
                var conn = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
                var redis = conn.GetDatabase();
                var result = redis.StringSetAsync("Contacts", JsonSerializer.Serialize(list), TimeSpan.FromMinutes(10)).Result;


            }
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Define o ambiente para "Testing"
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }
}