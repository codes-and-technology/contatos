using ConsultingInterface.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Redis
{
    public static class RedisServiceCollection
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            string redisHost = configuration["Redis:Host"];
            string redisPort = configuration["Redis:Port"];
            string connectionString = $"{redisHost}:{redisPort}";
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

            services.AddSingleton(typeof(IRedisCache<>), typeof(RedisCache<>));
        }
    }
}
