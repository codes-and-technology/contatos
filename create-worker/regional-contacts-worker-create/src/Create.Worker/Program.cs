using Create.Worker;
using MassTransit;
using Rabbit.Consumer.Create;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddRabbitMq();

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}