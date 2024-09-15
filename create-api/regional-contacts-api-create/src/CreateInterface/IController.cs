using CreateEntitys;
using Presenters;

namespace CreateInterface
{
    public interface IController
    {
        Task<ResultDto<ContactEntity>> CreateAsync(ContactDto contactDto);
    }
}
