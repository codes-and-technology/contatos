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

            List<ContactEntity> list = [];

            if (contactsCache.Count > 0)
            {
                list = contactsCache;
            }
            else
            {
                var dbList = await _dbGateway.GetAllAsync();
                list = dbList.ToList();
            }

            ConsultingContactUseCase useCase = new(list, regionId);

            return useCase.CreateConsulting();
        }
    }
}
