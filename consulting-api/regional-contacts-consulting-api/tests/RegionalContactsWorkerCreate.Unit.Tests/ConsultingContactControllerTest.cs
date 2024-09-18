using ConsultingController;
using Moq;
using ConsultingInterface.DbGateway;
using ConsultingInterface.CacheGateway;
using Presenters;
using ConsultingEntitys;

namespace RegionalContactsWorkerConsulting.Unit.Tests;


public class ConsultingContactControllerTest
{

    [Theory]
    [InlineData(null)]
    [InlineData(13)]
    public async Task When_Consulting_ShouldBe_Ok_WithCache(int? regionId)
    {
        Mock<IDb> mockDb = new();
        Mock<ICache> mockCache = new();

        mockCache.Setup(s => s.GetCacheAsync(It.IsAny<string>())).ReturnsAsync(CreateList());

        var controller = new ConsultingContactController(mockDb.Object, mockCache.Object);

        var result = await controller.ConsultingAsync(regionId.HasValue ? short.Parse(regionId.Value.ToString()) : null);

        Assert.True(result.Any());
    }

    [Theory]
    [InlineData(null)]
    public async Task When_Consulting_ShouldBe_Ok_WithDatabase(short? regionId)
    {
        Mock<IDb> mockDb = new();
        Mock<ICache> mockCache = new();


        mockDb.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ContactEntity>
        {
            new ContactEntity
            {
                Email = "teste@teste.com",
                Id = Guid.NewGuid(),
                Name = "Test",
                PhoneNumber = "1234567890",
                CreatedDate = DateTime.UtcNow,
                PhoneRegion = new PhoneRegionEntity { CreatedDate = DateTime.UtcNow, Id = Guid.NewGuid(), RegionNumber = 13 }
            }
        });

        var controller = new ConsultingContactController(mockDb.Object, mockCache.Object);

        var result = await controller.ConsultingAsync(regionId);

        Assert.True(result.Any());
    }

    private List<ContactDto> CreateList()
    {
        return new List<ContactDto>
        {
            new ContactDto
            {
                Email = "teste@teste.com",
                Id = Guid.NewGuid().ToString(),
                Name = "Test",
                PhoneNumber = "1234567890",
                RegionNumber = 13
            }
        };
    }
}
