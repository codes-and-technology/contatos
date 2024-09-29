using ConsultingEntitys;
using ConsultingInterface.CacheGateway;
using ConsultingInterface.Redis;
using Presenters;

namespace CacheGateway;

public class Cache : ICache
{
    private readonly IRedisCache<ContactDto> _redis;

    public Cache(IRedisCache<ContactDto> redis)
    {
        _redis = redis;
    }

    public async Task<List<ContactDto>> GetCacheAsync(string key) => await _redis.GetCacheAsync(key);

    public async Task SaveCacheAsync(string key, List<ContactDto> list) => await _redis.SaveCacheAsync(key,list);
}
