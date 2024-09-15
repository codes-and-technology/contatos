using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace External.Interfaces;

public static class RefitServiceExtension
{
    public static void AddRefitServiceExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IContactExternal>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(configuration["ContactConsulting:Uri"]);
            c.Timeout = TimeSpan.FromSeconds(1);
        });        
    }
}
