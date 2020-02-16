using FluentAssertions;
using JiraGraphThing.Core.Extensions;
using NUnit.Framework;
using System;

namespace Tests.ExtensionsTests
{
    class TimespanExtensionsTests
    {
        [TestCase("5:30","5h30m")]
        [TestCase("3:00", "3h")]
        [TestCase("0:15", "15m")]
        [TestCase("0:00", "")]
        public void CanFormatTime(string timespan, string compact)
        {
            var formatted = TimeSpan.Parse(timespan).FormatCompact();
            formatted.Should().Be(compact);
        }
    }
}
