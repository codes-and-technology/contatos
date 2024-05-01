using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Infrastructure.Repository;

namespace RegionalContacts.Infrastructure.Repositories;

public class PhoneRegionRepository(ApplicationDbContext context) : Repository<PhoneRegion>(context), IPhoneRegionRepository
{
    public async Task<PhoneRegion> GetByRegionNumberAsync(short number) => await context.PhoneRegions.Where(w => w.RegionNumber == number).FirstOrDefaultAsync();    
}
