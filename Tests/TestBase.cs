using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class TestBase
    {
        [SetUp]
        public void Setup()
        {
            OneConfig.Services.FileHelper.ApplicationDirectory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
           // CopyDatabase();
            JiraDataLayer.DIRegistrar.RegisterTypes();
        }

        private void CopyDatabase()
        {
            string name = "LocalDB.sqlite";

            if (File.Exists(name))
                return;

            var asm = Assembly.GetExecutingAssembly();
            var resourceName = asm.GetManifestResourceNames()
                .First(p => p.Contains(name));

            using (var stream = asm.GetManifestResourceStream(resourceName))
            {
                using (var file = File.OpenWrite(name))
                {
                    stream.CopyTo(file);
                }
            }
        }
    }
}