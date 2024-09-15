using CreateEntitys;
using MassTransit;

namespace Rabbit.Consumer.Create;

public class CreateContactConsumerDeadLetter : IConsumer<ContactEntity>
{
    public async Task Consume(ConsumeContext<ContactEntity> context)
    {
		try
		{
			string errorMessage = $"Erro ao tentar criar um novo registro: {context.Message}";
			Console.WriteLine($"Received message: {errorMessage}");
			await Task.CompletedTask;
		}
		catch (Exception ex)
		{
            Console.WriteLine($"Erro ao tentar processar uma falha de consumo da fila. ERROR: {ex}");
        }
    }
}
