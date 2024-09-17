using MassTransit;
using UpdateEntitys;
using UpdateInterface.Controllers;

namespace Rabbit.Consumer.Update;

public class UpdateContactConsumer(IUpdateContactController updateContactController) : IConsumer<ContactEntity>
{
    private readonly IUpdateContactController _updateContactController = updateContactController;

    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        var message = context.Message;
        Console.WriteLine($"Received message: {message}");
        var result = await _updateContactController.UpdateAsync(message);
    }
}
