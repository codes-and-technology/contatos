using Presenters;

namespace DeleteInterface;

public interface IContactConsultingGateway
{
    Task<IEnumerable<ContactConsultingDto>> Get();

}
