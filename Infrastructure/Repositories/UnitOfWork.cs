using Infrastructure.Repository;
using RegionalContacts.Core.Repositories.Interfaces;

namespace RegionalContacts.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{

    private readonly ApplicationDbContext _dbContext;
    public IContactRepository Contacts { get; }
    public IPhoneRegionRepository PhoneRegions { get; }

    public UnitOfWork(ApplicationDbContext dbContext,
                        IContactRepository contactRepository,
                        IPhoneRegionRepository phoneRegions)
    {
        _dbContext = dbContext;
        Contacts = contactRepository;
        PhoneRegions = phoneRegions;
    }

    public async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
    }
}
