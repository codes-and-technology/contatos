using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Domain.Interfaces.Repositories;
using RegionalContacts.Infrastructure.Repository;

namespace RegionalContacts.Infrastructure.Repositories;

public class PhoneRegionRepository(ApplicationDbContext context) : Repository<PhoneRegion>(context), IPhoneRegionRepository
{
    public async Task<PhoneRegion> GetByRegionNumberAsync(short number) => await context.PhoneRegions.Where(w => w.RegionNumber == number).FirstOrDefaultAsync();    
}
