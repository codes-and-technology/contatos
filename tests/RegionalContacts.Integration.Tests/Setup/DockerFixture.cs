using Docker.DotNet;
using Docker.DotNet.Models;

namespace RegionalContacts.Integration.Tests.Setup
{
    public class DockerFixture : IAsyncLifetime
    {
        private const string SqlServerImage = "mcr.microsoft.com/mssql/server:latest";
        private const string RedisImage = "redis:latest";
        private const string SqlServerContainerName = "DB_TESTE";
        private const string RedisContainerName = "REDIS_TESTE";

        private readonly DockerClient _dockerClient;

        public DockerFixture()
        {
            _dockerClient = new DockerClientConfiguration().CreateClient();
        }

        public async Task InitializeAsync()
        {
            await StartSqlServerContainerAsync();
            await StartRedisContainerAsync();
        }

        public async Task DisposeAsync()
        {
            await StopAndRemoveContainerAsync(SqlServerContainerName);
            await StopAndRemoveContainerAsync(RedisContainerName);
        }

        private async Task StartSqlServerContainerAsync()
        {
            var sqlServerExists = await ContainerExistsAsync(SqlServerContainerName);
            if (sqlServerExists) return;

            var sqlServerContainer = new CreateContainerResponse();
            try
            {
                sqlServerContainer = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
                {
                    Image = SqlServerImage,
                    Name = SqlServerContainerName,
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
            var redisExists = await ContainerExistsAsync(RedisContainerName);
            if (redisExists) return;

            var redisContainer = new CreateContainerResponse();
            try
            {
                redisContainer = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
                {
                    Image = RedisImage,
                    Name = RedisContainerName,
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
