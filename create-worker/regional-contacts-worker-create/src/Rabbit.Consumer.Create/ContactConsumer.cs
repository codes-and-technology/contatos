using CreateEntitys;
using MassTransit;

namespace Rabbit.Consumer.Create;

public class ContactConsumer : IConsumer<ContactEntity>
{
    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        var message = context.Message;

        // Lógica de processamento da mensagem
        Console.WriteLine($"Received message: {message}");

        await Task.CompletedTask; // Simula trabalho assíncrono
    }
}
