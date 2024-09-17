using UpdateEntitys;

namespace UpdateInterface.Gateway.DB
{
    public interface IBaseDB
    {
        Task CommitAsync();
    }
}
