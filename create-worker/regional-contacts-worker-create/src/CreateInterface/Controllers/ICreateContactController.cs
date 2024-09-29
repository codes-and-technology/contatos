using CreateEntitys;
using Presenters;

namespace CreateInterface.Controllers
{
    public interface ICreateContactController
    {
        Task<CreateResult<ContactEntity>> CreateAsync(ContactEntity entity);
    }
}
