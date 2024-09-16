using CreateController;
using CreateEntitys;
using CreateInterface.DataBase;
using CreateUseCases.UseCase;
using Moq;
using Redis;

namespace RegionalContactsWorkerCreate.Unit.Tests
{
    public class CreateContactControllerUnitTest
    {
        [Theory]
        [InlineData("teste", "1243", 0, "teste")]
        [InlineData("teste@gmail.com", "145123", 13, "")]
        [InlineData("teste@gmail.com", "", 13, "teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste")]
        [InlineData("teste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste testeteste teste teste teste teste  teste  teste teste", "988027555", 13, "")]
        public async Task When_AddAsync_ShouldBe_Error(string email, string phone, short region, string name)
        {
            var cache = new Mock<IRedisCache<ContactEntity>>();
            cache.Setup(s => s.SaveCacheAsync(It.IsAny<string>(), It.IsAny<List<ContactEntity>>())).Verifiable();
            cache.Setup(s => s.GetCacheAsync(It.IsAny<string>())).ReturnsAsync(new List<ContactEntity>());
            cache.Setup(s => s.ClearCacheAsync(It.IsAny<string>())).Verifiable();

            var phoneRegionRepositoryMock = new Mock<IPhoneRegionRepository>();
            phoneRegionRepositoryMock.Setup(s => s.GetByRegionNumberAsync(It.IsAny<short>())).ReturnsAsync(new PhoneRegionEntity());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(s => s.PhoneRegions).Returns(phoneRegionRepositoryMock.Object);

            var useCase = new CreateContactUseCase();
            CreateContactController controller = new(unitOfWorkMock.Object, useCase, cache.Object);

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

            var result = await controller.CreateAsync(entity);
            Assert.True(result.Errors.Count > 0);
        }

        [Theory]
        [InlineData("teste@gmail.com", "988027777", 11, "Usuário Teste")]
        public async Task When_AddAsync_ShouldBe_Ok(string email, string phone, short region, string name)
        {
            var cache = new Mock<IRedisCache<ContactEntity>>();
            cache.Setup(s => s.SaveCacheAsync(It.IsAny<string>(), It.IsAny<List<ContactEntity>>()));
            cache.Setup(s => s.GetCacheAsync(It.IsAny<string>())).ReturnsAsync(new List<ContactEntity>());
            cache.Setup(s => s.ClearCacheAsync(It.IsAny<string>())).Verifiable();

            var phoneRegionRepositoryMock = new Mock<IPhoneRegionRepository>();
            phoneRegionRepositoryMock.Setup(s => s.GetByRegionNumberAsync(It.IsAny<short>())).ReturnsAsync(new PhoneRegionEntity());

            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(s => s.AddAsync(It.IsAny<ContactEntity>())).Verifiable();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(s => s.Contacts).Returns(contactRepositoryMock.Object);
            unitOfWorkMock.Setup(s => s.PhoneRegions).Returns(phoneRegionRepositoryMock.Object);

            var useCase = new CreateContactUseCase();
            CreateContactController controller = new(unitOfWorkMock.Object, useCase, cache.Object);

            unitOfWorkMock.Setup(s => s.PhoneRegions.GetByRegionNumberAsync(It.IsAny<short>())).ReturnsAsync(null as PhoneRegionEntity);

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

            var result = await controller.CreateAsync(entity);
            Assert.True(result.Success);
        }
    }
}
