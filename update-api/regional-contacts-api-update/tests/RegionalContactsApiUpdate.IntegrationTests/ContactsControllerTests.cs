using Presenters;
using RegionalContactsApiUpdate.IntegrationTests.Setup;
using System.Net.Http.Json;
using System.Text.Json;
using UpdateEntitys;

namespace RegionalContactsApiUpdate.IntegrationTests;

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
    public async Task UpdateContact_ReturnsSuccessStatusCode()
    {
        _apiServer.Start();
        _apiServer.SearchContact();

        var id = "3e97c338-7cfe-4670-8055-6d79accff0c5";

        var contactDto = new ContactDto { Email = "teste@teste.com", Name = "Teste de usuário", PhoneNumber = "966627555", RegionNumber = 11 };
        var response = await _client.PutAsJsonAsync($"/api/contact/{id}", contactDto);

        _apiServer.Dispose();
        Assert.True(response.IsSuccessStatusCode);
    }


    [Fact]
    public async Task UpdateContacts_ReturnsError()
    {
        _apiServer.Start();
        _apiServer.SearchContact();

        var contactDto = new ContactDto { Email = "teste@teste.com", Name = "Teste de usuário", PhoneNumber = "978888888", RegionNumber = 11 };
        var response = await _client.PutAsJsonAsync($"/api/contact/{Guid.NewGuid()}", contactDto);
        _apiServer.Dispose();

        var resultString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var result = JsonSerializer.Deserialize<ResultDto<ContactEntity>>(resultString, options);

        Assert.True(result.Errors.Count() > 0);

    }
}

