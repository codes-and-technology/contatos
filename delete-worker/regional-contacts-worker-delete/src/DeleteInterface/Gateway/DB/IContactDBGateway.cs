using DeleteEntitys;

namespace DeleteInterface.Gateway.DB
{
    public interface IContactDBGateway : IBaseDB
    {
        Task AddAsync(ContactEntity entity);
        Task DeleteAsync(ContactEntity entity);
        Task<ContactEntity> FindByIdAsync(Guid id);
    }
}
