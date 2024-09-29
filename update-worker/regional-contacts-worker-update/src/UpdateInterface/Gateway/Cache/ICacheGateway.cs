namespace UpdateInterface.Gateway.Cache
{
    public interface ICacheGateway<T> where T : class
    {
        Task<List<T>> GetCacheAsync(string key);
        Task SaveCacheAsync(string key, List<T> list);
        Task ClearCacheAsync(string key);
    }
}
