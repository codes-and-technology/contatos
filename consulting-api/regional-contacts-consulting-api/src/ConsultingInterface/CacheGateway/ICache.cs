using ConsultingEntitys;

namespace ConsultingInterface.CacheGateway;

public interface ICache
{
    Task<List<ContactEntity>> GetCacheAsync(string key);
}
