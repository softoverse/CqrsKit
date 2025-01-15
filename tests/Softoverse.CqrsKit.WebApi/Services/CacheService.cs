using Microsoft.Extensions.Caching.Distributed;

using Softoverse.CqrsKit.WebApi.Extensions;

namespace Softoverse.CqrsKit.WebApi.Services
{
    public class CacheService(IDistributedCache cache) : ICacheService
    {
        public Task SetAsync<T>(string key, T value)
        {
            cache.SetValueAsync(key, value);
            return AddToCachedKeysAsync(key);
        }

        public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            cache.SetValueAsync(key, value, options);
            return AddToCachedKeysAsync(key);
        }

        public void Set<T>(string key, T value)
        {
            cache.SetValue(key, value);
            AddToCachedKeys(key);
        }

        public void Set<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            cache.SetValue(key, value, options);
            AddToCachedKeys(key);
        }

        public Task SetRangeAsync(IDictionary<string, object> data)
        {
            var tasks = data.Select(item =>
            {
                return Task.WhenAll(new List<Task>
                {
                    cache.SetValueAsync(item.Key, item.Value),
                    AddToCachedKeysAsync(item.Key)
                });
            });

            return Task.WhenAll(tasks);
        }

        public Task SetRangeAsync(IDictionary<string, object> data, DistributedCacheEntryOptions options)
        {
            var tasks = data.Select(item =>
            {
                return Task.WhenAll(new List<Task>
                {
                    cache.SetValueAsync(item.Key, item.Value, options),
                    AddToCachedKeysAsync(item.Key)
                });
            });

            return Task.WhenAll(tasks);
        }

        public void SetRange(IDictionary<string, object> data)
        {
            Parallel.ForEach(data, item =>
            {
                cache.SetValue(item.Key, item.Value);
                AddToCachedKeys(item.Key);
            });
        }

        public void SetRange(IDictionary<string, object> data, DistributedCacheEntryOptions options)
        {
            Parallel.ForEach(data, item =>
            {
                cache.SetValue(item.Key, item.Value, options);
                AddToCachedKeys(item.Key);
            });
        }

        public bool TryGet<T>(string key, out T? value)
        {
            return cache.TryGetValue(key, out value);
        }

        public T? Get<T>(string key)
        {
            return cache.GetValue<T>(key);
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return cache.GetValueAsync<T>(key);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
            RemoveFromCachedKeys(key);
        }

        public Task RemoveAsync(string key)
        {
            cache.RemoveAsync(key);
            return RemoveFromCachedKeysAsync(key);
        }

        public void RemoveRange(HashSet<string> keys)
        {
            foreach (var key in keys)
            {
                cache.Remove(key);
                RemoveFromCachedKeys(key);
            }
        }

        public Task RemoveRangeAsync(HashSet<string> keys)
        {
            var tasks = keys.Select(key =>
            {
                return Task.WhenAll(new List<Task>
                {
                    cache.RemoveAsync(key),
                    RemoveFromCachedKeysAsync(key)
                });
            });

            return Task.WhenAll(tasks);
        }

        public Task ClearAllCacheAsync()
        {
            var keys = GetAllKeys();
            return RemoveRangeAsync(keys);
        }

        public void ClearAllCache()
        {
            var keys = GetAllKeys();
            RemoveRange(keys);
        }

        #region Cached Keys management

        //TODO: Optimize cached_keys_list values with corresponding options provided for cachings

        private readonly string _allCachedItemKeysListName = "cached_keys_list";

        private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(45),
            SlidingExpiration = TimeSpan.FromMinutes(15)
        };

        public HashSet<string> GetAllKeys()
        {
            if (TryGet(_allCachedItemKeysListName, out HashSet<string>? value))
            {
                return value ?? new();
            }
            else
            {
                return new();
            }
        }

        public Task SetAllKeysAsync(HashSet<string> keys)
        {
            return cache.SetValueAsync(_allCachedItemKeysListName, keys, _distributedCacheEntryOptions);
        }

        public void SetAllKeys(HashSet<string> keys)
        {
            cache.SetValue(_allCachedItemKeysListName, keys, _distributedCacheEntryOptions);
        }

        public Task AddToCachedKeysAsync(string key)
        {
            HashSet<string> allKeys = GetAllKeys() ?? new();
            allKeys.Add(key);
            return SetAllKeysAsync(allKeys);
        }

        public void AddToCachedKeys(string key)
        {
            HashSet<string> allKeys = GetAllKeys() ?? new();
            allKeys.Add(key);
            SetAllKeys(allKeys);
        }

        public void AddRangeToCachedKeys(HashSet<string> keys)
        {
            HashSet<string> allKeys = GetAllKeys() ?? new();
            keys = allKeys.Concat(keys).ToHashSet();
            SetAllKeys(allKeys);
            cache.SetValue(_allCachedItemKeysListName, keys, _distributedCacheEntryOptions);
        }

        public async Task AddRangeToCachedKeysAsync(HashSet<string> keys)
        {
            HashSet<string> allKeys = GetAllKeys() ?? new();
            keys = allKeys.Concat(keys).ToHashSet();
            await SetAllKeysAsync(allKeys);
            await cache.SetValueAsync(_allCachedItemKeysListName, keys, _distributedCacheEntryOptions);
        }

        public void RemoveFromCachedKeys(string key)
        {
            HashSet<string> allKeys = (GetAllKeys() ?? new()).Where(x => x != key).ToHashSet();
            SetAllKeys(allKeys);
        }

        public async Task RemoveFromCachedKeysAsync(string key)
        {
            HashSet<string> allKeys = (GetAllKeys() ?? new()).Where(x => x != key).ToHashSet();
            await SetAllKeysAsync(allKeys);
        }

        public void RemoveRangeFromCachedKeys(HashSet<string> keys)
        {
            HashSet<string> allKeys = (GetAllKeys() ?? new()).Where(x => !keys.Contains(x)).ToHashSet();
            SetAllKeys(allKeys);
        }

        public async Task RemoveRangeFromCachedKeysAsync(HashSet<string> keys)
        {
            HashSet<string> allKeys = (GetAllKeys() ?? new()).Where(x => !keys.Contains(x)).ToHashSet();
            await SetAllKeysAsync(allKeys);
        }

        #endregion Cached Keys management
    }
}