using Presenters;

namespace UpdateInterface;

public interface IContactConsultingGateway
{
    Task<IEnumerable<ContactConsultingDto>> Get(int regionId);

}
