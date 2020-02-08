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
    }
}
