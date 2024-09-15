using CreateEntitys;
using CreateInterface.Controllers;
using CreateInterface.DataBase;
using CreateInterface.UseCase;
using Presenters;
using Redis;

namespace CreateController
{
    public class CreateContactController(IUnitOfWork unitOfWork, ICreateContactUseCase createContactUseCase, IRedisCache<ContactEntity> redis) : ICreateContactController
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICreateContactUseCase _createContactUseCase = createContactUseCase;
        private readonly IRedisCache<ContactEntity> _redis = redis;

        public async Task<CreateResult<ContactEntity>> CreateAsync(ContactEntity entity)
        {
            entity.PhoneRegion = await GetOrCreatePhoneRegionAsync(entity.PhoneRegion.RegionNumber);
            var result = _createContactUseCase.Create(entity);

            if (result.Errors.Count > 0)
                return result;

            await _unitOfWork.Contacts.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            await _redis.ClearCacheAsync("Contacts");

            return result;
        }

        private async Task<PhoneRegionEntity> GetOrCreatePhoneRegionAsync(short regionNumber)
        {
            var phoneRegion = await _unitOfWork.PhoneRegions.GetByRegionNumberAsync(regionNumber);

            if (phoneRegion is null)
            {
                phoneRegion = new PhoneRegionEntity { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = regionNumber };
                await _unitOfWork.PhoneRegions.AddAsync(phoneRegion);
            }

            return phoneRegion;
        }
    }
}
