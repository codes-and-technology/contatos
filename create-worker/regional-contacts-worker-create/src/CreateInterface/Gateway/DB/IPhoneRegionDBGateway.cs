using CreateEntitys;

namespace CreateInterface.Gateway.DB
{
    public interface IPhoneRegionDBGateway : IBaseDB
    {
        Task AddAsync(PhoneRegionEntity entity);
        Task<PhoneRegionEntity> GetByRegionNumberAsync(short regionNumber);
    }
}
