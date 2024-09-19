using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Producer.Delete;

public static class RabbitMqServiceExtension
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration["Rabbit:Host"];
        var user = configuration["Rabbit:User"];
        ushort port = Convert.ToUInt16(configuration["Rabbit:Port"]);

        var password = configuration["Rabbit:Password"];

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {                
                cfg.Host(host, port, "/", h => {
                    h.Username(user);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
              
                cfg.ExchangeType = "direct"; 
                cfg.Durable = true; 

            });

            
        });
    }
}
