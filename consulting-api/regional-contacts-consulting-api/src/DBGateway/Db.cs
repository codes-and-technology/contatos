using ConsultingEntitys;
using ConsultingInterface.DataBase;
using ConsultingInterface.DbGateway;

namespace DBGateway;

public class Db : IDb
{
    private readonly IContactRepository _contactRepository;

    public Db(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IEnumerable<ContactEntity>> GetAllAsync() => await _contactRepository.GetAllAsync();  
}
