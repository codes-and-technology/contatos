using CreateEntitys;

namespace CreateInterface.DataBase;
public interface IRepository<T> where T : EntityBase
{
    Task AddAsync(T entity);
    Task<T> FindByIdAsync(Guid id);
}
