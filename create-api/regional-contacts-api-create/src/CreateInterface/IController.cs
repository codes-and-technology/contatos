using CreateDto;
using CreateEntitys;

namespace CreateInterface
{
    public interface IController
    {
        Task<ResultDto<ContactEntity>> CreateAsync(ContactDto contactDto);
    }
}
