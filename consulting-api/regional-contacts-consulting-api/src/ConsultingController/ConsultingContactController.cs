using ConsultingEntitys;
using ConsultingInterface.CacheGateway;
using ConsultingInterface.Controllers;
using ConsultingInterface.DbGateway;
using ConsultingUseCases.UseCase;
using Presenters;

namespace ConsultingController
{
    public class ConsultingContactController(IDb dbGateway, ICache cacheGateway) : IConsultingContactController
    {
        private readonly IDb _dbGateway = dbGateway;
        private readonly ICache _cacheGateway = cacheGateway;

        public async Task<List<ContactDto>> ConsultingAsync(short? regionId)
        {
            ConsultingResult<IEnumerable<ContactEntity>> result = new();
            var contactsCache = await _cacheGateway.GetCacheAsync("Contacts");
            ConsultingContactUseCase useCase = new(regionId);

            List<ContactEntity> list = [];

            if (contactsCache is not null && contactsCache.Count > 0)
            {
                return useCase.CreateConsultingFromCache(contactsCache);
            }

            var dbList = await _dbGateway.GetAllAsync();
            list = dbList.ToList();

            await _cacheGateway.SaveCacheAsync("Contacts", list.Select(f => new ContactDto
            {
                Id = f.Id.ToString(),
                Email = f.Email,
                Name = f.Name,
                PhoneNumber = f.PhoneNumber,
                RegionNumber = f.PhoneRegion.RegionNumber
            }).ToList());

            return useCase.CreateConsultingFromDb(list);
        }
    }
}
