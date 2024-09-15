using ConsultingEntitys;
using Presenters;

namespace ConsultingUseCases.UseCase
{
    public class ConsultingContactUseCase
    {
        private List<ContactEntity> _list;
        private readonly short? _regionId;

        public ConsultingContactUseCase(List<ContactEntity> list, short? regionId)
        {
            _list = list;
            _regionId = regionId;
        }

        public List<ContactDto> CreateConsulting()
        {
            var list = new List<ContactDto>();

            if (_regionId.HasValue)
                _list = _list.Where(w => w.PhoneRegion != null && w.PhoneRegion.RegionNumber == _regionId.Value).ToList();

            foreach (var item in _list)
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
    }
}
