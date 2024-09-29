using CreateEntitys;
using Presenters;
using RegionalContactsApiCreate.Integration.Tests.Setup;
using System.Net.Http.Json;
using System.Text.Json;

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
    public async Task PostContacts_ReturnsSuccessStatusCode()
    {
        _apiServer.Start();
        _apiServer.SearchContact();
        var contactDto = new ContactDto { Email = "teste@teste.com", Name = "Teste de usuário", PhoneNumber = "966627555", RegionNumber = 11 };
        var response = await _client.PostAsJsonAsync("/api/contact", contactDto);

        _apiServer.Dispose();
        Assert.True(response.IsSuccessStatusCode);            
    }


    [Fact]
    public async Task PostContacts_ReturnsErrorWithContactAlreadyExists()
    {
        _apiServer.Start();
        _apiServer.SearchContact();

        var contactDto = new ContactDto { Email = "teste@teste.com", Name = "Teste de usuário", PhoneNumber = "988888888", RegionNumber = 11 };
        var response = await _client.PostAsJsonAsync("/api/contact", contactDto);
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
