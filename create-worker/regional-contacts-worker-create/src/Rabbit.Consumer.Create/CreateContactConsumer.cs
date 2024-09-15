using CreateEntitys;
using CreateInterface.Controllers;
using MassTransit;

namespace Rabbit.Consumer.Create;

public class CreateContactConsumer(ICreateContactController createContactController) : IConsumer<ContactEntity>
{
    private readonly ICreateContactController _createContactController = createContactController;

    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        var message = context.Message;
        Console.WriteLine($"Received message: {message}");
        var result = await _createContactController.CreateAsync(message);
    }
}
