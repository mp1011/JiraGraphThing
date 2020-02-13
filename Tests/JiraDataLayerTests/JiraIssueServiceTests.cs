using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Models;
using JiraDataLayer.Services;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;


namespace Tests.JiraDataLayerTests
{
    class JiraIssueServiceTests : TestBase
    {

        [TestCase("DSDE")]
        public async Task CanGetIssuesByProject(string project)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            bool foundAny = false;

            foreach(var result in await service.GetIssues(new SearchArgs(project: project, take:10)))
            {
                foundAny = true;
                result.Project.Should().Be(project);
            }

            foundAny.Should().BeTrue();
        }

        [TestCase("DSDE-2995")]
        [TestCase("DSDE-3001")]
        [TestCase("DSDE-3193")]
        [TestCase("DSDE-3195")]
        [TestCase("DSDE-3243")]
        public async Task CanGetSpecificIssue(string key)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            int count = 0;

            foreach (var result in await service.GetIssues(new SearchArgs(key: key)).ConfigureAwait(false))
            {
                count++;
                result.Key.Should().Be(key);
            }

            count.Should().Be(1);
        }

        [TestCase("DSDE-2997", "DSDE Sprint 21")]
        public async Task CanReadSprint(string key, string expectedSprint)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            (await service.GetIssue(key)).Sprint.Should().Be(expectedSprint);
        }

        [TestCase("DSDE-2997", 2.0)]
        public async Task CanReadStoryPoints(string key, decimal expectedStoryPoints)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            (await service.GetIssue(key)).StoryPoints.Should().Be(expectedStoryPoints);
        }

        [TestCase("DSDE-2995",5)]
        public async Task CanGetIssuesByEpicLink(string parentKey, int expectedChildren)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            int count = 0;

            foreach (var result in await service.GetIssues(new SearchArgs(parentKey: parentKey)).ConfigureAwait(false))
            {
                count++;
                result.EpicKey.Should().Be(parentKey);
            }

            count.Should().Be(expectedChildren);
        }

        [TestCase("DSDE-3193", 1)]
        public async Task CanGetSubtasksByParentKey(string parentKey, int expectedChildren)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            int count = 0;

            foreach (var result in await service.GetIssues(new SearchArgs(parentKey: parentKey)).ConfigureAwait(false))
            {
                count++;
                result.ParentKey.Should().Be(parentKey);
            }

            count.Should().Be(expectedChildren);
        }

        [TestCase("DSDE-3197", 7)]
        public async Task CanReadLoggedHours(string key, double expectedHours)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();

            var logs = await service.GetWorkLogs(key);
            logs.Sum(p => p.TimeSpent.TotalHours)
                .Should()
                .Be(expectedHours);
        }
    }
}
