using Moq;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Service.Services;

namespace RegionalContacts.Tests.Service;

public class ContactServiceTest
{
    [Fact]
    public async Task When_FindById_ShouldBe_Ok()
    {
        Contact contact = new()
        {
            Id = Guid.NewGuid(),
            Name = "teste",
            Email = "teste@teste.com",
            PhoneNumber = "123453523",
            CreatedDate = DateTime.Now,
            PhoneRegion = new PhoneRegion { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = 33 }
        };

        var list = new List<Contact>
        {
            contact
        };
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(s => s.Contacts.FindAllAsync()).ReturnsAsync(list);

        ContactService service = new(unitOfWorkMock.Object);

        var result = await service.FindAsync(null);

        Assert.Equal(list.Count >  1, result.Count > 1);
    }
}
