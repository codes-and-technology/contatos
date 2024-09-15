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
            var result = _createContactUseCase.Create(entity);

            if (result.Errors.Count > 0)
                return result;

            if (entity.PhoneRegion?.Id == Guid.Empty)
            {
                PhoneRegionEntity phoneRegionEntity = new PhoneRegionEntity(entity.PhoneRegion.RegionNumber);
                await _unitOfWork.PhoneRegions.AddAsync(phoneRegionEntity);
                entity.PhoneRegion = phoneRegionEntity;
            }

            await _unitOfWork.Contacts.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            await _redis.ClearCacheAsync("Contacts");

            return result;
        }
    }
}
