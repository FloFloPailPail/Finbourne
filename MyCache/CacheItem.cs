namespace LruCache
{
    public class CacheItem<TKey, TValue>(TKey key, TValue value, TimeSpan expirationDate)
    {
        public TKey Key { get; } = key;
        public TValue Value { get; } = value;
        public DateTime ExpirationDate { get; } = DateTime.Now.Add(expirationDate);
    }
}
