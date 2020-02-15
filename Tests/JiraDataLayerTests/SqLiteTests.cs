using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.Models.DTO;
using JiraDataLayer.Services;
using JiraDataLayer.SqLite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.JiraDataLayerTests
{
    class SqLiteTests : TestBase
    {
        [Test]
        public void CanUpdateValueInSqlLite()
        {
            var dao = SimpleIoc.Default.GetInstance<SQLiteDAO>();

            var key = "TEST" + Guid.NewGuid().ToString();
            var testModel = new JQLSearchDTO { JQL = key, Key = key, Take = 10 };

            dao.Write(testModel);
            dao.Read<JQLSearchDTO>("JQL=@JQL", new { testModel.JQL })
                .Length
                .Should()
                .Be(1);

            testModel.Take = 20;
            dao.Write(testModel);
            var read = dao.Read<JQLSearchDTO>("JQL=@JQL", new { testModel.JQL });

            read.Length
                .Should()
                .Be(1);

            read[0].Take.Should().Be(20);
        }

        [TestCase("DSDE-2995")]
        public async Task CanCacheIssueToSQLite(string key)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            var cacheProvider = SimpleIoc.Default.GetInstance<SQLiteCacheProvider>();
            var sqliteCache = cacheProvider.CreateCache<JiraIssue>();
            var issue = await sqliteCache.GetOrCompute(key, async (s) => await service.GetIssue(key));
            var loaded = await sqliteCache.GetOrCompute(key, async (s) => (JiraIssue)null);

            loaded.Key.Should().Be(key);
        }

        [Test]
        public async Task CanCacheWorkLogs()
        {
            var fakeLogs = Enumerable.Range(0, 5)
                .Select(x => new WorkLog("UnitTest", DateTime.Now.AddHours(-1 * x), TimeSpan.FromMinutes(30 * (x + 1))))
                .ToArray();

            var cache = SimpleIoc.Default.GetInstance<SQLiteCacheProvider>()
                .CreateArrayCache<WorkLog>();

            var cached = await cache.GetOrCompute("FAKE", async (s) => fakeLogs);

            var cached2 = await cache.GetOrCompute("FAKE", async (s) => null);

            cached2.Length.Should().Be(fakeLogs.Length);
            cached2.Sum(p => p.TimeSpent.TotalSeconds)
                .Should()
                .Be(fakeLogs.Sum(p => p.TimeSpent.TotalSeconds));
        }
    }
}
