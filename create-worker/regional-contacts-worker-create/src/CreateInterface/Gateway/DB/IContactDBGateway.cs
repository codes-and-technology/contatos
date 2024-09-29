using CreateEntitys;

namespace CreateInterface.Gateway.DB
{
    public interface IContactDBGateway : IBaseDB
    {
        Task AddAsync(ContactEntity entity);
        Task<ContactEntity> FindByIdAsync(Guid id);
    }
}
