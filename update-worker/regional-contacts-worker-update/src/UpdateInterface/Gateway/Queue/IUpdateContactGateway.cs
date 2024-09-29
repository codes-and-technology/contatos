using Presenters;
using UpdateEntitys;

namespace UpdateInterface.Gateway.Queue
{
    public interface IUpdateContactGateway
    {
        Task<UpdateResult<ContactEntity>> UpdateAsync(ContactEntity entity);
    }
}
