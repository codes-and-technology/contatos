using DeleteEntitys;

namespace DeleteInterface.Gateway.DB
{
    public interface IBaseDB
    {
        Task CommitAsync();
    }
}
