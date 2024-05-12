using Microsoft.EntityFrameworkCore;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Domain.Interfaces.Repositories;

namespace RegionalContacts.Infrastructure.Repositories.SqlServer;

public class PhoneRegionRepository(ApplicationDbContext context) : Repository<PhoneRegion>(context), IPhoneRegionRepository
{
    public async Task<PhoneRegion> GetByRegionNumberAsync(short number) => await context.PhoneRegions.Where(w => w.RegionNumber == number).FirstOrDefaultAsync();
}
