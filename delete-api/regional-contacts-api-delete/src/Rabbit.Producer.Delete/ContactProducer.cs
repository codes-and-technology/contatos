﻿using DeleteInterface;
using MassTransit;
using Microsoft.Extensions.Configuration;
using DeleteEntitys;

namespace Rabbit.Producer.Delete;

public class ContactProducer : IContactProducer
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IConfiguration _configuration;

    public ContactProducer(ISendEndpointProvider sendEndpointProvider, IConfiguration configuration)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _configuration = configuration;
    }

    public async Task SendMessage(DeleteContactEntity entity)
    {
        var host = _configuration["Rabbit:Host"];
        
        var queue = "delete-contact";
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"rabbitmq://{host}/{queue}"));

        await sendEndpoint.Send(entity);      
    }
}