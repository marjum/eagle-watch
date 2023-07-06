using Microsoft.Extensions.Caching.Distributed;
using MM.EagleRock.Contract.Cache;
using System.Text.Json;

namespace MM.EagleRock.DAL.Cache
{
    /// <summary>
    /// A simple cachine service using Redis.
    /// </summary>
    /// <remarks>
    /// TODO: Add unit test coverage
    /// </remarks>
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache) 
        {
            _cache = cache;
        }

        /// <inheritdoc/>
        public T? Get<T>(string key)
        {
            var value = _cache.GetString(key);

            if (value != null) 
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        /// <inheritdoc/>
        public T Set<T>(string key, T value)
        {
            // TODO: drive cache item expiration options via service configuration, or through custom logic
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };

            _cache.SetString(key, JsonSerializer.Serialize(value), timeOut);

            return value;
        }
    }
}
