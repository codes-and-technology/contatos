using DeleteEntitys;
using Presenters;

namespace DeleteInterface
{
    public interface IController
    {
        Task<ResultDto<DeleteContactEntity>> DeleteAsync(Guid id);
    }
}
