using CreateEntitys;
using MassTransit;

namespace Rabbit.Consumer.Create;

public class ContactConsumerDeadLetter : IConsumer<ContactEntity>
{
    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
        try
        {
            var message = context.Message;

            // Lógica de processamento da mensagem
            Console.WriteLine($"Received message: {message}");

            throw new Exception("error");
            await Task.CompletedTask; // Simula trabalho assíncrono
        }
        catch (Exception)
        {
            throw;
        }
    }

   
}
