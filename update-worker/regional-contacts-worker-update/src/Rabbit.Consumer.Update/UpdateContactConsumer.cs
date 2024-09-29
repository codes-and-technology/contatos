using MassTransit;
using UpdateEntitys;
using UpdateInterface.Gateway.Queue;

namespace Rabbit.Consumer.Update;

public class UpdateContactConsumer(IUpdateContactGateway updateContactGateway) : IConsumer<ContactEntity>
{
    private readonly IUpdateContactGateway _updateContactGateway = updateContactGateway;

    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        var message = context.Message;
        Console.WriteLine($"Received message: {message}");
        var result = await _updateContactGateway.UpdateAsync(message);
    }
}
