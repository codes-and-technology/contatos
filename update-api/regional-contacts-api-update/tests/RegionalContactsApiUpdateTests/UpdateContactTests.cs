using UpdateController;
using UpdateInterface;
using Moq;
using Presenters;

namespace RegionalContactsApiUpdateTests
{
    public class UpdateContactTests
    {
        [Theory]
        [InlineData("usuario teste", "988027555", "teste@teste.com", 13)]
        public async Task When_UpdateContact_ShouldBe_Ok(string name, string phoneNumber, string email, short regionNumber)
        {
            var id = Guid.NewGuid();
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();

            var createContactController = new UpdateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.UpdateAsync(id, dto);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("usuario teste", "988027555", "teste@teste.com", 13)]
        public async Task When_UpdateContact_ShouldBe_Error_WithContactNotExists(string name, string phoneNumber, string email, short regionNumber)
        {
            var id = Guid.NewGuid();
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();

            var list = new List<ContactConsultingDto>()
            {
                new ContactConsultingDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    RegionNumber = regionNumber,
                    Name = name,
                    PhoneNumber = phoneNumber
                }
            };

            mockIContactConsultingGateway.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(list);

            var createContactController = new UpdateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.UpdateAsync(Guid.NewGuid(), dto);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("usuario teste", "988027555", "teste@teste.com", 13)]
        public async Task When_UpdateContact_ShouldBe_Error_WithAnotherContact(string name, string phoneNumber, string email, short regionNumber)
        {
            var id = Guid.NewGuid();
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();

            var list = new List<ContactConsultingDto>()
            {
                new ContactConsultingDto
                {
                    Id = id.ToString(),
                    Email = email,
                    RegionNumber = regionNumber,
                    Name = name,
                    PhoneNumber = phoneNumber
                },
                 new ContactConsultingDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    RegionNumber = regionNumber,
                    Name = name,
                    PhoneNumber = phoneNumber
                }
            };

            mockIContactConsultingGateway.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(list);

            var createContactController = new UpdateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.UpdateAsync(id, dto);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("usuario teste", "988027555", "", 13)]
        [InlineData("usuario teste", "", "", 0)]
        [InlineData("", "57829013457082349", "teste@teste.com", 0)]
        public async Task When_UpdateContact_ShouldBe_Error_WithInvalidDto(string name, string phoneNumber, string email, short regionNumber)
        {
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();


            var createContactController = new UpdateContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            ContactDto dto = new()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                RegionNumber = regionNumber
            };

            var result = await createContactController.UpdateAsync(Guid.NewGuid(), dto);

            Assert.False(result.Success);
        }
    }
}