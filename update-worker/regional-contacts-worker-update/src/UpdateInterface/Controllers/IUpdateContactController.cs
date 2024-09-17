using UpdateEntitys;
using Presenters;

namespace UpdateInterface.Controllers
{
    public interface IUpdateContactController
    {
        Task<UpdateResult<ContactEntity>> UpdateAsync(ContactEntity entity);
    }
}
