using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Domain.Interfaces.Repositories;

namespace RegionalContacts.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    protected ApplicationDbContext _context;
    protected DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    
    public virtual async Task DeleteAsync(T entity) => await Task.Run(() => _dbSet.Remove(entity));    

    public virtual async Task<IList<T>> FindAllAsync() => await _dbSet.ToListAsync();

    public virtual async Task<T> FindByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public virtual async Task UpdateAsync(T entity) => await Task.Run(() => _dbSet.Update(entity));
}
