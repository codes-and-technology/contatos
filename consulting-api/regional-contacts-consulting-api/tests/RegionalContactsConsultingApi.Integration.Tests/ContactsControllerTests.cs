using Presenters;
using RegionalContactsConsultingApi.Integration.Tests.Setup;
using System.Net.Http.Json;

namespace RegionalContactsConsultingApi.Integration.Tests;

public class ContactsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<DockerFixture>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ContactsControllerTests(CustomWebApplicationFactory<Program> factory, DockerFixture dockerFixture)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetContacts_ReturnsSuccessStatusCode()
    {
        var getResponse = await _client.GetFromJsonAsync<List<ContactDto>>("/api/contact");

        Assert.True(getResponse.Count > 0);  
    }
}
