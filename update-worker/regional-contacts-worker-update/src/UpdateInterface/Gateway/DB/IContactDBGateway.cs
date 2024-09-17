using UpdateEntitys;

namespace UpdateInterface.Gateway.DB
{
    public interface IContactDBGateway : IBaseDB
    {
        Task AddAsync(ContactEntity entity);
        Task UpdateAsync(ContactEntity entity);
        Task<ContactEntity> FindByIdAsync(Guid id);
    }
}
