using NUnit.Framework;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class TestBase
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            OneConfig.Services.FileHelper.ApplicationDirectory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            JiraDataLayer.DIRegistrar.RegisterTypes();
        }
    }
}