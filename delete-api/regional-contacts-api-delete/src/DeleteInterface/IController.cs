using DeleteEntitys;
using Presenters;

namespace DeleteInterface
{
    public interface IController
    {
        Task<ResultDto<ContactEntity>> DeleteAsync(Guid id);
    }
}
