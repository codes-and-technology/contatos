using ConsultingInterface.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace Redis
{
    public class RedisCache<T> : IRedisCache<T> where T : class
    {
        private readonly IDatabase _cache;

        public RedisCache(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task<List<T>> GetCacheAsync(string key)
        {
            var cache = await _cache.StringGetAsync(key);

            if (cache.IsNullOrEmpty)
            {
                return [];
            }
            else
            {
                return Deserialize<List<T>>(cache);
            }
        }

        private byte[] Serialize<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        private T Deserialize<T>(byte[] bytes)
        {
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
