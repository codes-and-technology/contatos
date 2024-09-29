using CreateEntitys;
using CreateInterface.Gateway.DB;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionalContactsWorkerCreate.Integration.Tests.Setup;

namespace RegionalContactsWorkerCreate.Integration.Tests
{
    public class CreateContactControllerIntegrationTest : IClassFixture<CustomWorkerApplicationFixture>, IClassFixture<DockerFixture>
    {
        private readonly IHost _host;

        public CreateContactControllerIntegrationTest(CustomWorkerApplicationFixture hostFixture, DockerFixture dockerFixture)
        {
            _host = hostFixture.Host;
        }

        [Theory]
        [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
        public async Task When_AddAsync_ShouldBe_Ok(string email, string phone, short region, string name)
        {
            await _host.StartAsync();
            await Task.Delay(20000); // Esperar os container subirem

            var publisher = _host.Services.GetRequiredService<IPublishEndpoint>();
            var contactToInsert = new ContactEntity
            {
                CreatedDate = DateTime.Now,
                Email = email,
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phone,
                PhoneRegion = new PhoneRegionEntity
                {
                    RegionNumber = region
                }
            };

            await publisher.Publish(contactToInsert);

            // Simular um tempo de processamento e parar o host
            await Task.Delay(10000); // Esperar o Worker processar a mensagem

            var contactDBGateway = _host.Services.GetRequiredService<IContactDBGateway>();
            var phoneRegionDBGateway = _host.Services.GetRequiredService<IPhoneRegionDBGateway>();

            var insertedContact = await contactDBGateway.FindByIdAsync(contactToInsert.Id);
            insertedContact.PhoneRegion = await phoneRegionDBGateway.GetByRegionNumberAsync(contactToInsert.PhoneRegion.RegionNumber);

            await _host.StopAsync();

            Assert.NotNull(insertedContact);
            Assert.Equal(contactToInsert.Id, insertedContact.Id);
            Assert.Equal(contactToInsert.Name, insertedContact.Name);
            Assert.Equal(contactToInsert.Email, insertedContact.Email);
            Assert.Equal(contactToInsert.PhoneNumber, insertedContact.PhoneNumber);
            Assert.NotNull(insertedContact.PhoneRegion);
            Assert.Equal(contactToInsert.PhoneRegion.RegionNumber, insertedContact.PhoneRegion.RegionNumber);
        }
    }
}