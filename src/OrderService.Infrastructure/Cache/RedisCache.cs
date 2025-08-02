using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.Domain.Contracts;
using StackExchange.Redis;

namespace OrderService.Infrastructure.Cache;

public class RedisCache : IRedisCache
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisCache> _logger;
    private const int DEFAULT_EXPIRATION_TIME_IN_MINUTES = 15;

    public RedisCache(IConnectionMultiplexer redis, ILogger<RedisCache> logger)
    {
        _db = redis.GetDatabase();
        _logger = logger;
    }

    public async Task SetAsync(string key, string value, int expiredInMinutes = DEFAULT_EXPIRATION_TIME_IN_MINUTES)
    {
        await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(expiredInMinutes));
    }

    public async Task SetAsync(string key, object value, int expiredInMinutes = DEFAULT_EXPIRATION_TIME_IN_MINUTES)
    {
        await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), TimeSpan.FromMinutes(expiredInMinutes));
    }

    public async Task<string?> GetAsync(string key)
    {
        var redisValue = await _db.StringGetAsync(key);
        if (redisValue.IsNull) { return null; }
        return redisValue.ToString();
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var redisValue = await _db.StringGetAsync(key);
        if (redisValue.IsNull) { return null; }

        try
        {
            return JsonConvert.DeserializeObject<T>(redisValue.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can not deserialize cached value");
            return null;
        }
    }
}
