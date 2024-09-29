using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RegionalContactsApiDelete.IntegrationTests.Setup;
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
            var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.Testing.json")
                        .Build();
            var sp = services.BuildServiceProvider();
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Define o ambiente para "Testing"
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }
}
