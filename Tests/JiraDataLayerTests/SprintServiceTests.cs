using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.JiraDataLayerTests
{
    class SprintServiceTests :TestBase
    {
        [TestCase("DSDE Sprint 21", "1/15/2020")]
        public async Task CanLoadSprint(string sprint, string expectedDate)
        {
            var svc = SimpleIoc.Default.GetInstance<SprintService>();
            var sprintModel = await svc.GetSprint("DSDE board",sprint);
            sprintModel.Start
                .ToShortDateString()
                .Should()
                .Be(expectedDate);
        }

        [TestCase("DSDE board")]
        public async Task CanLoadSprints(string board)
        {
            var svc = SimpleIoc.Default.GetInstance<SprintService>();
            var sprints = await svc.GetSprints("DSDE board");
            sprints.Length.Should().BeGreaterThan(0);
        }
    }
}
