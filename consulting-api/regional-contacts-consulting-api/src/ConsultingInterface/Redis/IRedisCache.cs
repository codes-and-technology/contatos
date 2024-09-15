namespace ConsultingInterface.Redis
{
    public interface IRedisCache<T>
    {
        Task<List<T>> GetCacheAsync(string key);
    }
}
