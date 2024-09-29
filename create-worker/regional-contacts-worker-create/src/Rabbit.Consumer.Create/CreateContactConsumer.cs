using CreateEntitys;
using CreateInterface.Gateway.Queue;
using MassTransit;

namespace Rabbit.Consumer.Create;

public class CreateContactConsumer(ICreateContactGateway createContactGateway) : IConsumer<ContactEntity>
{
    private readonly ICreateContactGateway _createContactGateway = createContactGateway;

    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        var message = context.Message;
        Console.WriteLine($"Received message: {message}");
        var result = await _createContactGateway.CreateAsync(message);
    }
}
