using LruCache;

namespace LruCacheTests
{
    internal class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLruLogic()
        {
            LruCache<int, int?> cache = new(2);

            cache.Put(1, 1, new TimeSpan(0,0,10));
            cache.Put(2, 2, new TimeSpan(0, 0, 10));
            Assert.That(cache.Get(1), Is.EqualTo(1));
            cache.Put(3, 3, new TimeSpan(0, 0, 10));
            Assert.That(cache.Get(2), Is.EqualTo(null));
            cache.Put(4, 4, new TimeSpan(0, 0, 10));
            Assert.That(cache.Get(1), Is.EqualTo(null));
            Assert.That(cache.Get(3), Is.EqualTo(3));
            Assert.That(cache.Get(4), Is.EqualTo(4));
        }

        [Test]
        public void TestGetWithEmptyCache()
        {
            LruCache<string, string> cache = new(2);
            Assert.That(cache.Get("Test"), Is.EqualTo(null));
        }

        [Test]
        public void TestAddAndGet()
        {
            LruCache<int, int?> cache = new(2);
            cache.Put(1, 1, new TimeSpan(0, 0, 10));
            Assert.That(cache.Get(1), Is.EqualTo(1));
        }

        [Test]
        public void TestGetAfterExpiration()
        {
            LruCache<int, int?> cache = new(2);
            cache.Put(1, 1, new TimeSpan(0, 0, 10));
            Thread.Sleep(11000);
            Assert.That(cache.Get(1), Is.EqualTo(null));
        }
    }
}