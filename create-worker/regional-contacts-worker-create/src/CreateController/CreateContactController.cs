using CreateEntitys;
using CreateInterface.Controllers;
using CreateInterface.Gateway.Cache;
using CreateInterface.Gateway.DB;
using CreateInterface.UseCase;
using Presenters;

namespace CreateController
{
    public class CreateContactController(IContactDBGateway contactDBGateway,
                                         IPhoneRegionDBGateway phoneRegionDBGateway,
                                         ICreateContactUseCase createContactUseCase,
                                         ICacheGateway<ContactEntity> cache) : ICreateContactController
    {
        private readonly IContactDBGateway _contactDBGateway = contactDBGateway;
        private readonly IPhoneRegionDBGateway _phoneRegionDBGateway = phoneRegionDBGateway;
        private readonly ICreateContactUseCase _createContactUseCase = createContactUseCase;
        private readonly ICacheGateway<ContactEntity> _cache = cache;

        public async Task<CreateResult<ContactEntity>> CreateAsync(ContactEntity entity)
        {
            entity.PhoneRegion = await GetOrCreatePhoneRegionAsync(entity.PhoneRegion.RegionNumber);
            var result = _createContactUseCase.Create(entity);

            if (result.Errors.Count > 0)
                return result;

            await _contactDBGateway.AddAsync(entity);
            await _contactDBGateway.CommitAsync();

            await _cache.ClearCacheAsync("Contacts");

            return result;
        }

        private async Task<PhoneRegionEntity> GetOrCreatePhoneRegionAsync(short regionNumber)
        {
            var phoneRegion = await _phoneRegionDBGateway.GetByRegionNumberAsync(regionNumber);

            if (phoneRegion is null)
            {
                phoneRegion = new PhoneRegionEntity { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = regionNumber };
                await _phoneRegionDBGateway.AddAsync(phoneRegion);
            }

            return phoneRegion;
        }
    }
}
