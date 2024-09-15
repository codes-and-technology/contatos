using ConsultingEntitys;

namespace ConsultingInterface.DbGateway;

public interface IDb
{
    Task<IEnumerable<ContactEntity>> GetAllAsync();

}
