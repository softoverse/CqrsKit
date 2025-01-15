using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Caching.Distributed;

namespace Softoverse.CqrsKit.WebApi.Extensions
{
    public static class DistributedCacheExtensions
    {
        private static readonly TimeSpan _absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60 * 60);
        private static readonly TimeSpan _slidingExpiration = TimeSpan.FromSeconds(30);

        private static readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _absoluteExpirationRelativeToNow,
            SlidingExpiration = _slidingExpiration
        };

        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public static Task SetValueAsync<T>(this IDistributedCache cache, string key, T value)
        {
            return cache.SetValueAsync(key, value, _distributedCacheEntryOptions);
        }

        public static Task SetValueAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
            return cache.SetAsync(key, bytes, options);
        }

        public static void SetValue<T>(this IDistributedCache cache, string key, T value)
        {
            cache.SetValue(key, value, _distributedCacheEntryOptions);
        }

        public static void SetValue<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
            cache.Set(key, bytes, options);
        }

        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
        {
            var val = cache.Get(key);
            value = default;

            if (val == null) return false;

            value = JsonSerializer.Deserialize<T>(val, _serializerOptions);

            return true;
        }

        public static T? GetValue<T>(this IDistributedCache cache, string key)
        {
            var val = cache.Get(key);
            var value = JsonSerializer.Deserialize<T>(val, _serializerOptions);
            return value;
        }

        public static async Task<T?> GetValueAsync<T>(this IDistributedCache cache, string key)
        {
            var val = await cache.GetAsync(key);
            var value = JsonSerializer.Deserialize<T>(val, _serializerOptions);
            return value;
        }
    }
}