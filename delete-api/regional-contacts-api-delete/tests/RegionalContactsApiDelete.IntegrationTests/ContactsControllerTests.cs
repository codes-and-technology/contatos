using Presenters;
using RegionalContactsApiDelete.IntegrationTests.Setup;
using System.Net.Http.Json;
using System.Text.Json;

namespace RegionalContactsApiDelete.IntegrationTests;

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
    public async Task DeleteContact_ReturnsSuccessStatusCode()
    {
        var id = "93801677-b494-4a82-98a9-444e9fa1250e";

        _apiServer.Start();
        _apiServer.SearchContact();
        var response = await _client.DeleteAsync($"/api/contact/{id}");

        _apiServer.Dispose();
        Assert.True(response.IsSuccessStatusCode);
    }


    [Fact]
    public async Task DeleteContacts_ReturnErrorWithContactNotExists()
    {
        _apiServer.Start();
        _apiServer.SearchContact();
        var id = "682e2f01-9cf9-453e-b3f2-c1e8e0bfd149";
        var response = await _client.DeleteAsync($"/api/contact/{id}");
        _apiServer.Dispose();

        var resultString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var result = JsonSerializer.Deserialize<ResultDto<ContactConsultingDto>>(resultString, options);

        Assert.True(result.Errors.Count() > 0);

    }
}