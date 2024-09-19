using Presenters;
using RegionalContactsApiCreate.Integration.Tests.Setup;
using System.Net.Http.Json;

namespace RegionalContactsApiCreateTests;

public class ContactsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<DockerFixture>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ConsultingApiServer _apiServer = new();

    public ContactsControllerTests(CustomWebApplicationFactory<Program> factory, DockerFixture dockerFixture)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task GetContacts_ReturnsSuccessStatusCode()
    {
        _apiServer.Start();

        _apiServer.SearchContact();

        var contactDto = new ContactDto { Email = "teste@teste.com", Name = "Teste de usuário", PhoneNumber = "988027555", RegionNumber = 11 };
        var response = await _client.PostAsJsonAsync("/api/contact", contactDto);

        Assert.True(response.IsSuccessStatusCode);
     
    }
}
