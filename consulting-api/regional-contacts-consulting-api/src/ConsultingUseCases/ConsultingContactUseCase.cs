using ConsultingEntitys;
using Presenters;

namespace ConsultingUseCases.UseCase
{
    public class ConsultingContactUseCase
    {
        private readonly short? _regionId;

        public ConsultingContactUseCase(short? regionId)
        {
            _regionId = regionId;
        }

        public List<ContactDto> CreateConsultingFromDb(List<ContactEntity> listDb)
        {
            var list = new List<ContactDto>();

            if (_regionId.HasValue)
                listDb = listDb.Where(w => w.PhoneRegion != null && w.PhoneRegion.RegionNumber == _regionId.Value).ToList();

            foreach (var item in listDb)
            {
                list.Add(new ContactDto
                {
                    Id = item.Id.ToString(),
                    Email = item.Email,
                    Name = item.Name,
                    PhoneNumber = item.PhoneNumber,
                    RegionNumber = item.PhoneRegion.RegionNumber
                });
            }

            return list;
        }

        public List<ContactDto> CreateConsultingFromCache(List<ContactDto> listFromCache)
        {
            if (_regionId.HasValue)
                listFromCache = listFromCache.Where(w => w.RegionNumber == _regionId.Value).ToList();

            return listFromCache;
        }
    }
}
