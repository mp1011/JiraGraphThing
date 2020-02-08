using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Models;
using JiraDataLayer.Services;
using NUnit.Framework;
using System.Threading.Tasks;


namespace Tests.JiraDataLayerTests
{
    class JiraAPITests : TestBase
    {

        [TestCase("DSDE")]
        public async Task CanGetIssuesByProject(string project)
        {
            var service = SimpleIoc.Default.GetInstance<JiraIssueService>();
            bool foundAny = false;

            await foreach(var result in service.GetIssues(new SearchArgs(project: project, take:10)).ConfigureAwait(false))
            {
                foundAny = true;
                result.Project.Should().Be(project);
            }

            foundAny.Should().BeTrue();
        }
    }
}
