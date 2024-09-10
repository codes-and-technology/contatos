using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Consumer.Create;

public static class RabbitServiceExtension
{
    public static void AddRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            // Registra o consumidor
            x.AddConsumer<ContactConsumer>();

            // Configura RabbitMQ
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost"); // URL do RabbitMQ

                // Configura o endpoint de recebimento para a fila específica
                cfg.ReceiveEndpoint("create-contact", e =>
                {
                    e.ConfigureConsumer<ContactConsumer>(context);                    
                    e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });                             


                cfg.ReceiveEndpoint("create-contact_error", e =>
                {
                    e.Consumer<ContactConsumerDeadLetter>();
                    e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });

            });

          
        });
    }
}
