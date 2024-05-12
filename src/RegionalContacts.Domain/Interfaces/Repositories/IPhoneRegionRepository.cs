using RegionalContacts.Domain.Entity;

namespace RegionalContacts.Domain.Interfaces.Repositories;

public interface IPhoneRegionRepository : IRepository<PhoneRegion>
{
    Task<PhoneRegion> GetByRegionNumberAsync(short number);
}
