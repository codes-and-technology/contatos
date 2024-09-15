﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Consumer.Create;

public static class RabbitServiceExtension
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration["Rabbit:Host"];
        //var user = configuration["Rabbit:User"];
        //var password = configuration["Rabbit:Password"];

        services.AddMassTransit(register =>
        {
            register.AddConsumer<CreateContactConsumer>();
            register.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
                cfg.ReceiveEndpoint("create-contact", receiver =>
                {
                    receiver.ConfigureConsumer<CreateContactConsumer>(context);
                    receiver.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });


                cfg.ReceiveEndpoint("create-contact-error", receiverError =>
                {
                    receiverError.Consumer<CreateContactConsumerDeadLetter>();
                    receiverError.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                });
            });
          
        });
    }
}