using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionalContactsWorkerUpdate.Integration.Tests.Setup;
using UpdateEntitys;
using UpdateInterface.Gateway.DB;

namespace RegionalContactsWorkerUpdate.Integration.Tests
{
    public class UpdateContactControllerIntegrationTest : IClassFixture<CustomWorkerApplicationFixture>, IClassFixture<DockerFixture>
    {
        private readonly IHost _host;

        public UpdateContactControllerIntegrationTest(CustomWorkerApplicationFixture hostFixture, DockerFixture dockerFixture)
        {
            _host = hostFixture.Host;
        }

        [Theory]
        [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
        public async Task When_UpdateAsync_ShouldBe_Ok(string email, string phone, short region, string name)
        {
            await _host.StartAsync();
            await Task.Delay(20000); // Esperar os container subirem

            var contactToUpdate = new ContactEntity
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

            await contactDBGateway.AddAsync(contactToUpdate);
            await phoneRegionDBGateway.AddAsync(contactToUpdate.PhoneRegion);
            await contactDBGateway.CommitAsync();

            contactToUpdate.Name = "Update_Teste";
            contactToUpdate.PhoneRegion.Contacts = null;

            var publisher = _host.Services.GetRequiredService<IPublishEndpoint>();            

            await publisher.Publish(contactToUpdate);

            // Simular um tempo de processamento e parar o host
            await Task.Delay(10000); // Esperar o Worker processar a mensagem

            var updatedContact = await contactDBGateway.FindByIdAsync(contactToUpdate.Id);

            await _host.StopAsync();

            Assert.NotNull(updatedContact);
            Assert.Equal(updatedContact.Name, contactToUpdate.Name);
        }
    }
}