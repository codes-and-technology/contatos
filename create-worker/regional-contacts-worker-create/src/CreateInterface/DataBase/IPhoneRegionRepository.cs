using CreateEntitys;

namespace CreateInterface.DataBase;

public interface IPhoneRegionRepository : IRepository<PhoneRegionEntity>
{
    Task<PhoneRegionEntity> GetByRegionNumberAsync(short regionNumber);
}
