using CreateEntitys;

namespace CreateInterface.Gateway.DB
{
    public interface IBaseDB
    {
        Task CommitAsync();
    }
}
