using Docker.DotNet;
using Docker.DotNet.Models;

namespace RegionalContactsWorkerUpdate.Integration.Tests.Setup
{
    public class DockerFixture : IAsyncLifetime
    {
        private const string SQL_SERVER_IMAGE = "mcr.microsoft.com/mssql/server:2019-latest";
        private const string SQL_SERVER_CONTAINER_NAME = "DB_TESTE";
        private const string REDIS_IMAGE = "redis:latest";
        private const string REDIS_CONTAINER_NAME = "REDIS_TESTE";
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
            await StartSqlServerContainerAsync();
            await StartRedisContainerAsync();
            await StartRabbitMQContainerAsync();
        }

        public async Task DisposeAsync()
        {
            await StopAndRemoveContainerAsync(SQL_SERVER_CONTAINER_NAME);
            await StopAndRemoveContainerAsync(REDIS_CONTAINER_NAME);
            await StopAndRemoveContainerAsync(RABBITMQ_CONTAINER_NAME);
        }

        private async Task StartSqlServerContainerAsync()
        {
            var sqlServerExists = await ContainerExistsAsync(SQL_SERVER_CONTAINER_NAME);
            if (sqlServerExists) return;

            var sqlServerContainer = new CreateContainerResponse();
            try
            {
                sqlServerContainer = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
                {
                    Image = SQL_SERVER_IMAGE,
                    Name = SQL_SERVER_CONTAINER_NAME,
                    Env = new[] { "ACCEPT_EULA=Y", "SA_PASSWORD=sql@123456" },
                    ExposedPorts = new Dictionary<string, EmptyStruct> { { "1433/tcp", new EmptyStruct() } },
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1533" } } }
                    }
                    }
                });
                await _dockerClient.Containers.StartContainerAsync(sqlServerContainer.ID, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SQL Server container: {ex.Message}");
            }
        }

        private async Task StartRedisContainerAsync()
        {
            var redisExists = await ContainerExistsAsync(REDIS_CONTAINER_NAME);
            if (redisExists) return;

            var redisContainer = new CreateContainerResponse();
            try
            {
                redisContainer = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
                {
                    Image = REDIS_IMAGE,
                    Name = REDIS_CONTAINER_NAME,
                    ExposedPorts = new Dictionary<string, EmptyStruct> { { "6379/tcp", new EmptyStruct() } },
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "6379/tcp", new List<PortBinding> { new PortBinding { HostPort = "6380" } } }
                    }
                    }
                });
                await _dockerClient.Containers.StartContainerAsync(redisContainer.ID, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Redis container: {ex.Message}");
            }
        }

        private async Task StartRabbitMQContainerAsync()
        {
            // Defina as portas customizadas para os testes
            int amqpTestPort = 5673;
            int managementTestPort = 15673;

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
    }
}
