using CreateController;
using CreateInterface;
using Moq;
using Presenters;

namespace RegionalContactsApiCreateTests
{
    public class CreateContactTests
    {
        [Theory]
        [InlineData("usuario teste", "988027555", "teste@teste.com", 13)]
        public async Task When_CreateContact_ShouldBe_Ok(string name, string phoneNumber, string email, short regionNumber)
        {
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();

            mockIContactConsultingGateway.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(new List<ContactConsultingDto>());

            var createContactController = new CreateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.CreateAsync(dto);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("usuario teste", "988027555", "teste@teste.com", 13)]
        public async Task When_CreateContact_ShouldBe_Error_WithContactExists(string name, string phoneNumber, string email, short regionNumber)
        {
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();


            var list = new List<ContactConsultingDto>()
            {
                new ContactConsultingDto
                {
                    Email = email,
                    RegionNumber = regionNumber,
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    PhoneNumber = phoneNumber
                }
            };

            mockIContactConsultingGateway.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(list);

            var createContactController = new CreateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.CreateAsync(dto);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("usuario teste", "988027555", "teste@teste.com", 13)]
        public async Task When_CreateContact_ShouldBe_Error_ApiConsulting(string name, string phoneNumber, string email, short regionNumber)
        {
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();


            var list = new List<ContactConsultingDto>()
            {
                new ContactConsultingDto
                {
                    Email = email,
                    RegionNumber = regionNumber,
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    PhoneNumber = phoneNumber
                }
            };

            mockIContactConsultingGateway.Setup(s => s.Get(It.IsAny<int>())).ThrowsAsync(new Exception("Falha ao tentar consultar contato"));

            var createContactController = new CreateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };            

            await Assert.ThrowsAnyAsync<Exception>(async () => await createContactController.CreateAsync(dto));
        }

        [Theory]
        [InlineData("usuario teste", "988027555", "", 13)]
        [InlineData("usuario teste", "", "", 0)]
        [InlineData("", "57829013457082349", "teste@teste.com", 0)]
        public async Task When_CreateContact_ShouldBe_Error_WithInvalidDto(string name, string phoneNumber, string email, short regionNumber)
        {
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();


            var list = new List<ContactConsultingDto>();            

            mockIContactConsultingGateway.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(list);

            var createContactController = new CreateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.CreateAsync(dto);

            Assert.False(result.Success);
        }
    }
}