using CreateEntitys;
using CreateInterface.DataBase;
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
}
