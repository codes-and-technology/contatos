using Presenters;
using CreateEntitys;

namespace CreateInterface.Gateway.Queue
{
    public interface ICreateContactGateway
    {
        Task<CreateResult<ContactEntity>> CreateAsync(ContactEntity entity);
    }
}
