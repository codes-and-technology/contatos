using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Consumer.Create;

public static class RabbitServiceExtension
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration["Rabbit:Host"];
        ushort port = Convert.ToUInt16(configuration["Rabbit:Port"]);
        var user = configuration["Rabbit:User"];
        var password = configuration["Rabbit:Password"];

        services.AddMassTransit(register =>
        {
            register.AddConsumer<CreateContactConsumer>();

            register.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, port, "/", h =>
                {
                    
                    h.Username(user);
                    h.Password(password);
                });
                cfg.ReceiveEndpoint("create-contact", receiver =>
                {
                    receiver.ConfigureConsumer<CreateContactConsumer>(context);
                    receiver.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));                    
                });
            });          
        });
    }
}
