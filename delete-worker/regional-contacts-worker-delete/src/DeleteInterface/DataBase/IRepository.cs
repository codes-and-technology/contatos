using DeleteEntitys;

namespace DeleteInterface.DataBase;
public interface IRepository<T> where T : EntityBase
{
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T> FindByIdAsync(Guid id);
}
