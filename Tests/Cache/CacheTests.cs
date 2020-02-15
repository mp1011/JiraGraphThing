using FluentAssertions;
using JiraDataLayer.Cache;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Cache
{
    [TestFixture]
    class CacheTests 
    {

        [Test]
        public async Task BackgroundCacheRefreshesInDifferentThread()
        {
            var cache = new BackgroundRefreshingCache<string>(TimeSpan.FromSeconds(1),
                new ConcurrentInMemoryCache<string>());

            var beforeFirstCache = DateTime.Now;
            await cache.GetOrCompute("TEST", async (s)=> await FastOperation());

            var beforeSecondCache = DateTime.Now;
            await cache.GetOrCompute("TEST", async (s) => await SlowOperation());

            var afterSecondCache = DateTime.Now;

            (afterSecondCache - beforeSecondCache)
                .TotalMilliseconds
                .Should()
                .BeLessThan(1000);

            await Task.Delay(2000);

            var beforeThirdCache = DateTime.Now;
            var result = await cache.GetOrCompute("TEST", async (s) => await SlowOperation());
            var afterThirdCache = DateTime.Now;

            result.Should().Be("Fast");
            (afterThirdCache - beforeThirdCache)
               .TotalMilliseconds
               .Should()
               .BeLessThan(1000);

            await Task.Delay(4000);

            var beforeFourthCache = DateTime.Now;
            result = await cache.GetOrCompute("TEST", async (s) => await SlowOperation());
            var afterFourthCache = DateTime.Now;

            result.Should().Be("Slow");
            (afterThirdCache - beforeThirdCache)
                 .TotalMilliseconds
                 .Should()
                 .BeLessThan(1000);
        }

        private async Task<string> FastOperation()
        {
            return "Fast";
        }

        private async Task<string> SlowOperation()
        {
            System.Threading.Thread.Sleep(3000);
            return "Slow";
        }
    }
}
