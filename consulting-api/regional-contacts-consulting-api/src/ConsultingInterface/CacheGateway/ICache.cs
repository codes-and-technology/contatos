using ConsultingEntitys;
using Presenters;

namespace ConsultingInterface.CacheGateway;

public interface ICache
{
    Task<List<ContactDto>> GetCacheAsync(string key);
    Task SaveCacheAsync(string key, List<ContactDto> list);

}
