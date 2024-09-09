using CreateEntitys;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Producer.Create;

public static class RabbitMqServiceExtension
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {                
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);

                // Configura o envio de mensagens com roteamento apropriado
                cfg.Send<ContactEntity>(s =>
                {
                    s.UseRoutingKeyFormatter(ctx => "create-contact");
                });

                // Configura a troca para ser durável
                cfg.ExchangeType = "direct"; // Configura a troca para o tipo `direct`
                cfg.Durable = true; // Define a troca como durável

            });

            
        });
    }
}
