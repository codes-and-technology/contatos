using Presenters;
using DeleteEntitys;

namespace DeleteInterface.Gateway.Queue
{
    public interface IDeleteContactGateway
    {
        Task<DeleteResult<ContactEntity>> DeleteAsync(ContactEntity entity);
    }
}
