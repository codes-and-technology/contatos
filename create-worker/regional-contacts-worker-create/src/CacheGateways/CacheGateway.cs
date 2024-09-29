using CreateInterface.Gateway.Cache;
using Redis;

namespace CacheGateways
{
    public class CacheGateway<T>(IRedisCache<T> redis) : ICacheGateway<T> where T : class
    {
        private readonly IRedisCache<T> _redis = redis;

        public async Task ClearCacheAsync(string key)
        {
            await _redis.ClearCacheAsync(key);
        }

        public async Task<List<T>> GetCacheAsync(string key)
        {
            return await _redis.GetCacheAsync(key);
        }

        public async Task SaveCacheAsync(string key, List<T> list)
        {
            await _redis.SaveCacheAsync(key, list);
        }
    }
}
