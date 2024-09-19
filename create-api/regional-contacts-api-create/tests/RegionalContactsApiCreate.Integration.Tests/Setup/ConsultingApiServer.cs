using Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using static MassTransit.ValidationResultExtensions;
using static System.Reflection.Metadata.BlobBuilder;

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
                RegionNumber = 13
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
