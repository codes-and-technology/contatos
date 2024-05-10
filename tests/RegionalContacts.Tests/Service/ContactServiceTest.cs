using Moq;
using RegionalContacts.Domain.Dto.Contato;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Domain.Interfaces.Repositories;
using RegionalContacts.Service;
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


    [Theory]
    [InlineData("teste", "1243", 0, "teste")]
    [InlineData("teste@gmail.com", "145123", 13, "")]
    [InlineData("teste@gmail.com", "", 13, "teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste")]
    [InlineData("teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste", "988027555", 13, "")]
    public async Task When_AddAsync_ShouldBe_Error(string email, string phone, short region, string name)
    {
      
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        ContactService service = new(unitOfWorkMock.Object);

        unitOfWorkMock.Setup(s => s.Contacts
        .FindByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<short>()))
            .ReturnsAsync(new Contact {CreatedDate = DateTime.Now,
            Email = "tste@gmail.com",
            Id = Guid.NewGuid(),
            Name = "teste",
            PhoneNumber = "1243",
            PhoneRegion = new PhoneRegion
            {
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid(),
                RegionNumber = region
            }
            });

        
        var dto = new ContactCreateDto
        {
            Email = email,
            PhoneNumber = phone,
            RegionNumber = region,
            Name = name
        };

        var result = await service.CreateAsync(dto);

        Assert.True(result.Errors.Count > 0);
    }

    [Theory]
    [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
    public async Task When_AddAsync_ShouldBe_Ok(string email, string phone, short region, string name)
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        ContactService service = new(unitOfWorkMock.Object);

        unitOfWorkMock.Setup(s => s.Contacts
        .FindByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<short>()))
            .ReturnsAsync(null as Contact);

        unitOfWorkMock.Setup(s => s.PhoneRegions.GetByRegionNumberAsync(It.IsAny<short>())).ReturnsAsync(null as PhoneRegion);

        var dto = new ContactCreateDto
        {
            Email = email,
            PhoneNumber = phone,
            RegionNumber = region,
            Name = name
        };

        var result = await service.CreateAsync(dto);

        Assert.True(result.Success);
    }


    [Fact]
    public async Task When_DeleteAsync_ShouldBe_Ok()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        ContactService service = new(unitOfWorkMock.Object);


        unitOfWorkMock.Setup(s => s.Contacts.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Contact { Id =  Guid.NewGuid()});

        var result = await service.DeleteAsync(Guid.NewGuid());

        Assert.True(result.Success);
    }

    [Fact]
    public async Task When_DeleteAsync_ShouldBe_Error()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        ContactService service = new(unitOfWorkMock.Object);


        unitOfWorkMock.Setup(s => s.Contacts.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Contact);

        var result = await service.DeleteAsync(Guid.NewGuid());

        Assert.False(result.Success);
    }

    [Theory]
    [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
    public async Task When_UpdateAsync_ShouldBe_Ok(string email, string phone, short region, string name)
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        ContactService service = new(unitOfWorkMock.Object);

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Email = "teste@teste.com",
            Name = name,
            PhoneNumber = phone,
            PhoneRegion = new PhoneRegion { CreatedDate = DateTime.Now, Id =  Guid.NewGuid(), RegionNumber = 11 }
        };


        unitOfWorkMock.Setup(s => s.Contacts
        .FindByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<short>()))
            .ReturnsAsync(null as Contact);

        unitOfWorkMock.Setup(s => s.PhoneRegions.GetByRegionNumberAsync(It.IsAny<short>())).ReturnsAsync(null as PhoneRegion);
        unitOfWorkMock.Setup(s => s.Contacts.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(contact);

        var dto = new ContactUpdateDto
        {
            Email = email,
            PhoneNumber = phone,
            RegionNumber = region,
            Name = name
        };

        var id = Guid.NewGuid();

        var result = await service.UpdateAsync(id, dto);

        Assert.True(result.Success);
    }

    [Theory]
    [InlineData("teste", "1243", 0, "teste")]
    [InlineData("teste@gmail.com", "145123", 13, "")]
    [InlineData("teste@gmail.com", "", 13, "teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste")]
    [InlineData("teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste", "988027555", 13, "")]
    public async Task When_UpdateAsync_ShouldBe_Error(string email, string phone, short region, string name)
    {

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        ContactService service = new(unitOfWorkMock.Object);

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Email = "teste@teste.com",
            Name = name,
            PhoneNumber = phone,
            PhoneRegion = new PhoneRegion { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = 11 }
        };


        unitOfWorkMock.Setup(s => s.Contacts
        .FindByPhoneNumberAsync(It.IsAny<string>(), It.IsAny<short>()))
            .ReturnsAsync(null as Contact);

        unitOfWorkMock.Setup(s => s.PhoneRegions.GetByRegionNumberAsync(It.IsAny<short>())).ReturnsAsync(null as PhoneRegion);
        unitOfWorkMock.Setup(s => s.Contacts.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(contact);

        var dto = new ContactUpdateDto
        {
            Email = email,
            PhoneNumber = phone,
            RegionNumber = region,
            Name = name
        };

        var id = Guid.NewGuid();

        var result = await service.UpdateAsync(id, dto);

        Assert.False(result.Success);
    }


}
