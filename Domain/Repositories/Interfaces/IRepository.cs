using RegionalContacts.Core.Entity;

namespace RegionalContacts.Core.Repositories.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    Task<IList<T>> FindAllAsync();
    Task<T> FindByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
