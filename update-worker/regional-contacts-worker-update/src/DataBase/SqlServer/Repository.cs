using UpdateEntitys;
using UpdateInterface.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DataBase.SqlServer;

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
    public virtual async Task UpdateAsync(T entity) => await Task.Run(() => _dbSet.Update(entity));
    public virtual async Task<T> FindByIdAsync(Guid id) => await _dbSet.FindAsync(id);
}
