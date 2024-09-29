using DeleteEntitys;
using Presenters;

namespace DeleteInterface.Controllers
{
    public interface IDeleteContactController
    {
        Task<DeleteResult<ContactEntity>> DeleteAsync(ContactEntity entity);
    }
}
