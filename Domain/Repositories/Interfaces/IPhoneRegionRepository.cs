using RegionalContacts.Core.Entity;

namespace RegionalContacts.Core.Repositories.Interfaces;

public interface IPhoneRegionRepository
{
    Task<PhoneRegion> GetByRegionNumberAsync(short number);
    Task AddAsync(PhoneRegion entity);
}
