namespace OrderService.Domain.Contracts;

public interface IRedisCache
{
    Task<string?> GetAsync(string key);
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SetAsync(string key, object value, int expiredInMinutes = 15);
    Task SetAsync(string key, string value, int expiredInMinutes = 15);
}
