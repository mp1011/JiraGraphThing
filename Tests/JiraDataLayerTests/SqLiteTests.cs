﻿using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Cache;
using JiraDataLayer.Models;
using JiraDataLayer.Services;
using JiraDataLayer.SqLite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.JiraDataLayerTests
{
    class SqLiteTests : TestBase
    {
        [TestCase("DSDE-2995")]
        public async Task CanCacheIssueToSQLite(string key)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            var cacheProvider = SimpleIoc.Default.GetInstance<SQLiteCacheProvider>();
            var sqliteCache = cacheProvider.CreateCache<JiraIssue>();
            var issue = await sqliteCache.GetOrCompute(key, async () => await service.GetIssue(key));
            var loaded = await sqliteCache.GetOrCompute(key, async () => (JiraIssue)null);

            loaded.Key.Should().Be(key);
        }
    }
}