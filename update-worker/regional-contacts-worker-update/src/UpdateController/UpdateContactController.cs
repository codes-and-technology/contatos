using Presenters;
using UpdateEntitys;
using UpdateInterface.Controllers;
using UpdateInterface.Gateway.Cache;
using UpdateInterface.Gateway.DB;
using UpdateInterface.UseCase;

namespace UpdateController
{
    public class UpdateContactController(IContactDBGateway contactDBGateway,
                                         IPhoneRegionDBGateway phoneRegionDBGateway,
                                         IUpdateContactUseCase updateContactUseCase,
                                         ICacheGateway<ContactEntity> cache) : IUpdateContactController
    {
        private readonly IContactDBGateway _contactDBGateway = contactDBGateway;
        private readonly IPhoneRegionDBGateway _phoneRegionDBGateway = phoneRegionDBGateway;
        private readonly IUpdateContactUseCase _updateContactUseCase = updateContactUseCase;
        private readonly ICacheGateway<ContactEntity> _cache = cache;

        public async Task<UpdateResult<ContactEntity>> UpdateAsync(ContactEntity entity)
        {
            entity.PhoneRegion = await GetOrUpdatePhoneRegionAsync(entity.PhoneRegion.RegionNumber);
            var result = _updateContactUseCase.Update(entity);

            if (result.Errors.Count > 0)
                return result;

            await _contactDBGateway.UpdateAsync(entity);
            await _contactDBGateway.CommitAsync();

            await _cache.ClearCacheAsync("Contacts");

            return result;
        }

        private async Task<PhoneRegionEntity> GetOrUpdatePhoneRegionAsync(short regionNumber)
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
