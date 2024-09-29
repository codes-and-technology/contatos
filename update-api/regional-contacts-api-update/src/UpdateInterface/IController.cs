using UpdateEntitys;
using Presenters;

namespace UpdateInterface
{
    public interface IController
    {
        Task<ResultDto<ContactEntity>> UpdateAsync(Guid id, ContactDto contactDto);
    }
}
