using ConsultingEntitys;

namespace ConsultingInterface.DataBase;

public interface IContactRepository : IRepository<ContactEntity>
{
    Task<IEnumerable<ContactEntity>> GetAllAsync(); 
}
