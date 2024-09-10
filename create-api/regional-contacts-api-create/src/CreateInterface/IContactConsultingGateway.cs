using CreateDto;

namespace CreateInterface;

public interface IContactConsultingGateway
{
    Task<IEnumerable<ContactConsultingDto>> Get(int regionId);
}
