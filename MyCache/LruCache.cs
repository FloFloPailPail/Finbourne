using System.Collections.Concurrent;

namespace LruCache
{
    public class LruCache<TKey, TValue> : ICache<TKey, TValue> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly ConcurrentDictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>> _cache;
        private readonly LinkedList<CacheItem<TKey, TValue>> _lruList;

        private const int DefaultCacheSize = 1000;
        private const int MinimumCacheSize = 1;

        public LruCache(int capacity)
        {
            if (capacity < MinimumCacheSize)
            {
                throw new ArgumentException($"Cache size must be at least {MinimumCacheSize}", nameof(capacity));
            }

            _capacity = capacity;
            _cache = new ConcurrentDictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>>();
            _lruList = new LinkedList<CacheItem<TKey, TValue>>();
        }

        public LruCache()
        {
            _capacity = DefaultCacheSize;
            _cache = new ConcurrentDictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>>();
            _lruList = new LinkedList<CacheItem<TKey, TValue>>();
        }

        public TValue? Get(TKey key)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                if (DateTime.Now > node.Value.ExpirationDate)
                {
                    if (_cache.TryRemove(key, out var itemNode))
                    {
                        _lruList.Remove(itemNode);
                    }

                    return default;
                }

                _lruList.Remove(node);
                _lruList.AddFirst(node);

                return node.Value.Value;
            }

            return default;
        }

        public void Put(TKey key, TValue value, TimeSpan expiration)
        {
            if (_cache.Count >= _capacity
                && _cache.TryRemove(_lruList.Last.Value.Key, out var node))
            {
                _lruList.Remove(node);
            }

            var newCacheItem = new CacheItem<TKey, TValue>(key, value, expiration);
            var newNode = new LinkedListNode<CacheItem<TKey, TValue>>(newCacheItem);
            _lruList.AddFirst(newNode);
            _cache[key] = newNode;
        }
    }
}
