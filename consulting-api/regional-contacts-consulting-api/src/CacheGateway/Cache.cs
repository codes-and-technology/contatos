using ConsultingEntitys;
using ConsultingInterface.CacheGateway;
using ConsultingInterface.Redis;

namespace CacheGateway;

public class Cache : ICache
{
    private readonly IRedisCache<ContactEntity> _redis;

    public Cache(IRedisCache<ContactEntity> redis)
    {
        _redis = redis;
    }

    public async Task<List<ContactEntity>> GetCacheAsync(string key) => await _redis.GetCacheAsync(key);
}
