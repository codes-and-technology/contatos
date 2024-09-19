using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Diagnostics;

namespace RegionalContactsApiCreate.Integration.Tests.Setup;

public class DockerFixture : IAsyncLifetime
{
    private const string RABBITMQ_IMAGE = "rabbitmq:3-management";
    private const string RABBITMQ_CONTAINER_NAME = "RABBITMQ_TESTE";

    private readonly DockerClient _dockerClient;
    public string RabbitMQContainerId { get; private set; }
    public string RabbitMQUri { get; private set; }

    public DockerFixture()
    {
        _dockerClient = new DockerClientConfiguration().CreateClient();
    }

    public async Task InitializeAsync()
    {
        await StartRabbitMQContainerAsync();
    }

    public async Task DisposeAsync()
    {
        await StopAndRemoveContainerAsync(RABBITMQ_CONTAINER_NAME);
    }

    private async Task StartRabbitMQContainerAsync()
    {
        // Defina as portas customizadas para os testes
        int amqpTestPort = 5673;
        int managementTestPort = 15673;

  //      await _dockerClient.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = "rabbitmq" }, new AuthConfig(), new Progress())
  //.ConfigureAwait(false);

        // Cria o container RabbitMQ com as portas modificadas
        var createResponse = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = RABBITMQ_IMAGE,
            Name = RABBITMQ_CONTAINER_NAME,
            ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { $"{amqpTestPort}/tcp", default },
                    { $"{managementTestPort}/tcp", default }
                },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "5672/tcp", new List<PortBinding> { new PortBinding { HostPort = amqpTestPort.ToString() } } }, // AMQP
                        { "15672/tcp", new List<PortBinding> { new PortBinding { HostPort = managementTestPort.ToString() } } } // Management
                    }
            },
            Env = new List<string>
                {
                    "RABBITMQ_DEFAULT_USER=guest",
                    "RABBITMQ_DEFAULT_PASS=guest"
                }
        });

        RabbitMQContainerId = createResponse.ID;

        // Inicia o container
        await _dockerClient.Containers.StartContainerAsync(RabbitMQContainerId, new ContainerStartParameters());

        // Define o URI de conexão para o RabbitMQ
        RabbitMQUri = $"amqp://guest:guest@localhost:{amqpTestPort}";
    }

    private async Task<bool> ContainerExistsAsync(string containerName)
    {
        try
        {
            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true });
            return containers.Any(c => c.Names.Contains($"/{containerName}"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking container existence: {ex.Message}");
            return false;
        }
    }

    private async Task StopAndRemoveContainerAsync(string containerName)
    {
        try
        {
            var containerId = await GetContainerIdAsync(containerName);
            if (containerId == null) return;

            await _dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
            await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping/removing container: {ex.Message}");
        }
    }

    private async Task<string> GetContainerIdAsync(string containerName)
    {
        try
        {
            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true });
            return containers.FirstOrDefault(c => c.Names.Contains($"/{containerName}"))?.ID;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting container ID: {ex.Message}");
            return null;
        }
    }

    private sealed class Progress : IProgress<JSONMessage>
    {
        public void Report(JSONMessage value)
        {
            Debug.WriteLine(value.ProgressMessage);
        }
    }
}
