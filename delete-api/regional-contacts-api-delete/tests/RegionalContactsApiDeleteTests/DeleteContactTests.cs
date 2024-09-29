using Controller;
using DeleteInterface;
using Moq;
using Presenters;

namespace RegionalContactsApiDeleteTests
{
    public class DeleteContactTests
    {
        [Fact]
        public async Task When_DeleteContact_ShouldBe_Ok()
        {
            var id = Guid.NewGuid();
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();

            mockIContactConsultingGateway.Setup(s => s.Get()).ReturnsAsync(new List<ContactConsultingDto>());

            var createContactController = new DeleteContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            var result = await createContactController.DeleteAsync(id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task When_DeleteContact_ShouldBe_Error()
        {
            var id = Guid.NewGuid();
            var mockIContactQueueGateway = new Mock<IContactQueueGateway>();
            var mockIContactConsultingGateway = new Mock<IContactConsultingGateway>();

            var list = new List<ContactConsultingDto>()
            {
               new ContactConsultingDto
               {
                   Id = Guid.NewGuid().ToString(),
                   Email = "teste@teste.com",
                   Name = "Usuario Teste",
                   PhoneNumber = "988027555",
                   RegionNumber = 15
               }
            };
            mockIContactConsultingGateway.Setup(s => s.Get()).ReturnsAsync(list);

            var createContactController = new DeleteContactController(mockIContactQueueGateway.Object, mockIContactConsultingGateway.Object);

            var result = await createContactController.DeleteAsync(id);

            Assert.False(result.Success);
        }
    }
}