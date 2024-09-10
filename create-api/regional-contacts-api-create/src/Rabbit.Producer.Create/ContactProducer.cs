using CreateEntitys;
using CreateInterface;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Rabbit.Producer.Create;

public class ContactProducer : IContactProducer
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IConfiguration _configuration;

    public ContactProducer(ISendEndpointProvider sendEndpointProvider, IConfiguration configuration)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _configuration = configuration;
    }

    public async Task SendMessage(ContactEntity entity)
    {
        var host = _configuration["Rabbit:Host"];
        var queue = "create-contact";
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"rabbitmq://{host}/{queue}"));

        await sendEndpoint.Send(entity);      
    }
}
