using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Services;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.JiraDataLayerTests
{
    class JiraGraphBuilderTests : TestBase
    {
        [TestCase("DSDE-2995",5)]
        public async Task CanLoadEpicWithIssues(string epicKey, int expectedChildren)
        {
            var graphBuilder = SimpleIoc.Default.GetInstance<JiraGraphBuilder>();
            var graph = await graphBuilder.LoadItemGraph(epicKey);
            graph.GetChildren().Count().Should().Be(expectedChildren);
        }

        [TestCase("DSDE-2995",6,22)]
        public async Task CanLoadMetricsFromEpic(string epicKey, decimal expectedPoints, double expectedLoggedHours)
        {
            var graphBuilder = SimpleIoc.Default.GetInstance<JiraGraphBuilder>();
            var graph = await graphBuilder.LoadItemGraph(epicKey);
            graph.GetTotalStoryPoints().Should().Be(expectedPoints);
            graph.GetWorkLogs().Sum(w => w.TimeSpent.TotalHours).Should().Be(expectedLoggedHours);
        }

        [TestCase("DSDE Sprint 21")]
        public async Task CanLoadGraphBySprint(string sprint)
        {
            var graphBuilder = SimpleIoc.Default.GetInstance<JiraGraphBuilder>();
            var graph = await graphBuilder.LoadSprintGraph(sprint);
            graph.Children.Length.Should().BeGreaterThan(0);
        }

        [TestCase("DSDE Sprint 21")]
        public async Task CanLoadUserSprintGraph(string sprint)
        {
            var graphBuilder = SimpleIoc.Default.GetInstance<JiraGraphBuilder>();
            var graph = await graphBuilder.LoadUserSprintGraph(sprint);
            graph.Children.Length.Should().BeGreaterThan(0);
        }
    }
}
