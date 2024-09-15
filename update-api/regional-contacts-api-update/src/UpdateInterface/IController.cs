using UpdateEntitys;
using Presenters;

namespace UpdateInterface
{
    public interface IController
    {
        Task<ResultDto<UpdateContactEntity>> UpdateAsync(Guid id, ContactDto contactDto);
    }
}
