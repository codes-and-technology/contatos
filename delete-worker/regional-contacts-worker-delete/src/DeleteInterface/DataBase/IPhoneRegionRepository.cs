using DeleteEntitys;

namespace DeleteInterface.DataBase;

public interface IPhoneRegionRepository : IRepository<PhoneRegionEntity>
{
    Task<PhoneRegionEntity> GetByRegionNumberAsync(short regionNumber);
}
