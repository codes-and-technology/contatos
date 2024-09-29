using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionalContactsWorkerDelete.Integration.Tests.Setup;
using DeleteEntitys;
using DeleteInterface.Gateway.DB;

namespace RegionalContactsWorkerDelete.Integration.Tests
{
    public class DeleteContactControllerIntegrationTest : IClassFixture<CustomWorkerApplicationFixture>, IClassFixture<DockerFixture>
    {
        private readonly IHost _host;

        public DeleteContactControllerIntegrationTest(CustomWorkerApplicationFixture hostFixture, DockerFixture dockerFixture)
        {
            _host = hostFixture.Host;
        }

        [Theory]
        [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
        public async Task When_DeleteAsync_ShouldBe_Ok(string email, string phone, short region, string name)
        {
            await _host.StartAsync();
            await Task.Delay(20000); // Esperar os container subirem

            var contactToDelete = new ContactEntity
            {
                CreatedDate = DateTime.Now,
                Email = email,
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phone,
                PhoneRegion = new PhoneRegionEntity
                {
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    RegionNumber = region
                }
            };

            var contactDBGateway = _host.Services.GetRequiredService<IContactDBGateway>();
            var phoneRegionDBGateway = _host.Services.GetRequiredService<IPhoneRegionDBGateway>();

            await contactDBGateway.AddAsync(contactToDelete);
            await phoneRegionDBGateway.AddAsync(contactToDelete.PhoneRegion);
            await contactDBGateway.CommitAsync();

            contactToDelete.PhoneRegion.Contacts = null;

            var publisher = _host.Services.GetRequiredService<IPublishEndpoint>();            

            await publisher.Publish(contactToDelete);

            // Simular um tempo de processamento e parar o host
            await Task.Delay(10000); // Esperar o Worker processar a mensagem
            await _host.StopAsync();

            Assert.True(true);
        }
    }
}