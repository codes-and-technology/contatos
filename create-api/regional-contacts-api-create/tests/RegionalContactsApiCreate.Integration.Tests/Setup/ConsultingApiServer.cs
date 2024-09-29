using Presenters;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace RegionalContactsApiCreate.Integration.Tests.Setup;

public class ConsultingApiServer : IDisposable
{
    private WireMockServer _server;

    public string Url => _server.Url;

    public void Start() => _server = WireMockServer.Start(new WireMockServerSettings
    {
        Urls = new[] { "http://localhost:5900" },
    });

    public void SearchContact()
    {
        var list = new List<ContactConsultingDto>
        {
            new ContactConsultingDto
            {
                Email = "teste@tste.com",
                Id = Guid.NewGuid().ToString(),
                Name = "teste",
                PhoneNumber = "988888888",
                RegionNumber = 11
            }
        };
        
        _server.Given(Request.Create()
          .WithPath("/api/Contact")
          .WithParam("regionId", "11")
          .UsingGet())           
          .RespondWith(Response.Create()
          .WithHeader("content-type", "application/json")
          .WithBodyAsJson(list)
          .WithStatusCode(System.Net.HttpStatusCode.OK));
    }
   
    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }
}
