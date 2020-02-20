using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Services;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.JiraDataLayerTests
{
    class DailyWorkHistoryCalculatorTests :TestBase
    {
        [TestCase("DSDE Sprint 21","Michael Pastore")]
        public async Task CanComputeWorkHistory(string sprintName, string user)
        {
            var calculator = SimpleIoc.Default.GetInstance<DailyWorkHistoryCalculator>();
            var sprint = await SimpleIoc.Default.GetInstance<SprintService>().GetSprint(sprintName);
            var sprintGraph = await SimpleIoc.Default.GetInstance<JiraGraphBuilder>().LoadUserSprintGraph(sprint);
            var userNode = sprintGraph.Children.Single(p => p.Name == user);

            var logs = calculator.CalculateWorkHistory(sprint, userNode).ToArray();
            logs.Length.Should().Be(10);
        }
    }
}
