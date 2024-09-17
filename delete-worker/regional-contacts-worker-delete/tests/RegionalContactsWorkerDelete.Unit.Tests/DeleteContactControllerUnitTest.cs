using Moq;
using DeleteController;
using DeleteEntitys;
using DeleteInterface.Gateway.Cache;
using DeleteInterface.Gateway.DB;
using DeleteUseCases.UseCase;

namespace RegionalContactsWorkerDelete.Unit.Tests
{
    public class DeleteContactControllerUnitTest
    {
        [Theory]
        [InlineData("teste", "1243", 0, "teste")]
        [InlineData("teste@gmail.com", "145123", 13, "")]
        [InlineData("teste@gmail.com", "", 13, "teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste")]
        [InlineData("teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste", "988027555", 13, "")]
        public async Task When_DeleteAsync_ShouldBe_Error(string email, string phone, short region, string name)
        {
            var cache = new Mock<ICacheGateway<ContactEntity>>();
            cache.Setup(s => s.SaveCacheAsync(It.IsAny<string>(), It.IsAny<List<ContactEntity>>())).Verifiable();
            cache.Setup(s => s.GetCacheAsync(It.IsAny<string>())).ReturnsAsync(new List<ContactEntity>());
            cache.Setup(s => s.ClearCacheAsync(It.IsAny<string>())).Verifiable();

            var entity = new ContactEntity
            {
                CreatedDate = DateTime.Now,
                Email = email,
                Name = name,
                PhoneNumber = phone,
                PhoneRegion = new PhoneRegionEntity
                {
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    RegionNumber = region
                }
            };

            entity.Id = Guid.Empty;

            var contactDBGatewayMock = new Mock<IContactDBGateway>();
            contactDBGatewayMock.Setup(s => s.AddAsync(entity)).Verifiable();

            var useCase = new DeleteContactUseCase();
            DeleteContactController controller = new(contactDBGatewayMock.Object, useCase, cache.Object);

            var result = await controller.DeleteAsync(entity);
            Assert.True(result.Errors.Count > 0);
        }

        [Theory]
        [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
        public async Task When_DeleteAsync_ShouldBe_Ok(string email, string phone, short region, string name)
        {
            var cache = new Mock<ICacheGateway<ContactEntity>>();
            cache.Setup(s => s.SaveCacheAsync(It.IsAny<string>(), It.IsAny<List<ContactEntity>>())).Verifiable();
            cache.Setup(s => s.GetCacheAsync(It.IsAny<string>())).ReturnsAsync(new List<ContactEntity>());
            cache.Setup(s => s.ClearCacheAsync(It.IsAny<string>())).Verifiable();

            var entity = new ContactEntity
            {
                CreatedDate = DateTime.Now,
                Email = email,
                Id = Guid.NewGuid(),
                Name = name,
                PhoneNumber = phone,
                PhoneRegion = new PhoneRegionEntity
                {
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    RegionNumber = region
                }
            };

            var contactDBGatewayMock = new Mock<IContactDBGateway>();
            contactDBGatewayMock.Setup(s => s.AddAsync(entity)).Verifiable();

            var useCase = new DeleteContactUseCase();
            DeleteContactController controller = new(contactDBGatewayMock.Object, useCase, cache.Object);

            var result = await controller.DeleteAsync(entity);
            Assert.True(result.Success);
        }
    }
}
