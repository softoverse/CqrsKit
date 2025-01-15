using Microsoft.Extensions.Caching.Distributed;

namespace Softoverse.CqrsKit.WebApi.Services
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value);

        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options);

        void Set<T>(string key, T value);

        void Set<T>(string key, T value, DistributedCacheEntryOptions options);

        Task SetRangeAsync(IDictionary<string, object> data);

        Task SetRangeAsync(IDictionary<string, object> data, DistributedCacheEntryOptions options);

        void SetRange(IDictionary<string, object> data);

        void SetRange(IDictionary<string, object> data, DistributedCacheEntryOptions options);

        bool TryGet<T>(string key, out T? value);

        T? Get<T>(string key);

        Task<T?> GetAsync<T>(string key);

        void Remove(string key);

        Task RemoveAsync(string key);

        void RemoveRange(HashSet<string> keys);

        Task RemoveRangeAsync(HashSet<string> keys);

        Task ClearAllCacheAsync();

        void ClearAllCache();

        #region Cached Keys management

        HashSet<string> GetAllKeys();

        void SetAllKeys(HashSet<string> keys);

        Task SetAllKeysAsync(HashSet<string> keys);

        void AddToCachedKeys(string key);

        Task AddToCachedKeysAsync(string key);

        void AddRangeToCachedKeys(HashSet<string> keys);

        Task AddRangeToCachedKeysAsync(HashSet<string> keys);

        void RemoveFromCachedKeys(string key);

        Task RemoveFromCachedKeysAsync(string key);

        void RemoveRangeFromCachedKeys(HashSet<string> keys);

        Task RemoveRangeFromCachedKeysAsync(HashSet<string> keys);

        #endregion Cached Keys management
    }
}