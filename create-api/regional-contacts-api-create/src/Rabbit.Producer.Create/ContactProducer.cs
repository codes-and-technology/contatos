using CreateEntitys;
using CreateInterface;
using MassTransit;
using MassTransit.Transports;

namespace Rabbit.Producer.Create;

public class ContactProducer : IContactProducer
{
    private readonly IPublishEndpoint _publisher;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public ContactProducer(IPublishEndpoint publisher, ISendEndpointProvider sendEndpointProvider)
    {
        _publisher = publisher;
        _sendEndpointProvider = sendEndpointProvider;

    }

    public async Task SendMessage(ContactEntity entity)
    {
       await _publisher.Publish<ContactEntity>(entity, context =>
       {
           context.Headers.Set("RoutingKey", "create-contact");           
       });
    }
}
