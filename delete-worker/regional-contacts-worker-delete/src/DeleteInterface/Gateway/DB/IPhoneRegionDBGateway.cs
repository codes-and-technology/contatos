using DeleteEntitys;

namespace DeleteInterface.Gateway.DB
{
    public interface IPhoneRegionDBGateway : IBaseDB
    {
        Task AddAsync(PhoneRegionEntity entity);
        Task<PhoneRegionEntity> GetByRegionNumberAsync(short regionNumber);
    }
}
