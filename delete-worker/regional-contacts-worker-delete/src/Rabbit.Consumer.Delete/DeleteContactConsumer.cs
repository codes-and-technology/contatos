using MassTransit;
using DeleteEntitys;
using DeleteInterface.Gateway.Queue;

namespace Rabbit.Consumer.Delete;

public class DeleteContactConsumer(IDeleteContactGateway deleteContactGateway) : IConsumer<ContactEntity>
{
    private readonly IDeleteContactGateway _deleteContactGateway = deleteContactGateway;

    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        var message = context.Message;
        Console.WriteLine($"Received message: {message}");
        var result = await _deleteContactGateway.DeleteAsync(message);
    }
}
