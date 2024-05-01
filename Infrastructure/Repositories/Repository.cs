using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;

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

    public async Task AddAsync(T entity) => _dbSet.AddAsync(entity);
    
    public async Task DeleteAsync(T entity) => await Task.Run(() => _dbSet.Remove(entity));    

    public async Task<IList<T>> FindAllAsync() => await _dbSet.ToListAsync();    

    public async Task<T> FindByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task UpdateAsync(T entity) => await Task.Run(() => _dbSet.Update(entity));
}
