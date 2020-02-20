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

        [TestCase("DSDE-3291", "To Do")]
        [TestCase("DSDE-3186", "IN PROGRESS")]
        [TestCase("DSDE-3170", "DONE")]
        public async Task CanReadStatus(string key, string expectedStatus)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            var issue = await service.GetIssue(key);
            issue.StatusName.ToString().Should().Be(expectedStatus);

        }

        [TestCase("DSDE-3197", 7)]
        public async Task CanReadLoggedHours(string key, double expectedHours)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();

            var logs = await service.GetWorkLogs(key);
            logs.Sum(p => p.TimeSpent.TotalHours)
                .Should()
                .Be(expectedHours);

            foreach (var log in logs)
                log.Author.Should().NotBeNull();

        }

        [TestCase("DSDE-3197", "Michael Pastore")]
        public async Task CanReadAuthorFromWorkLogs(string key, string expectedAuthor)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();

            var logs = await service.GetWorkLogs(key);

            foreach (var log in logs)
                log.Author.Should().Be(expectedAuthor);

        }
    }
}
