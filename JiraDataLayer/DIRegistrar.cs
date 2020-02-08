using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Services;
using JiraDataLayer.Services.CustomFieldReaders;
using JiraGraphThing.Core.Extensions;

namespace JiraDataLayer
{
    public class DIRegistrar
    {
        private static bool _registered = false;
        public static void RegisterTypes()
        {
            if (!_registered)
            {
                _registered = true;
                SimpleIoc.Default.Register<IJiraConfig, JiraConfig>();
                SimpleIoc.Default.Register<JiraRestClientProvider>();
                SimpleIoc.Default.Register<JiraGraphBuilder>();
                SimpleIoc.Default.Register<CustomFieldReader>();
                SimpleIoc.Default.RegisterAllAsArray<ICustomFieldReader>();
                SimpleIoc.Default.Register(() => new JiraIssueService(SimpleIoc.Default.GetInstance<JiraRestClientProvider>(), SimpleIoc.Default.GetInstance<CustomFieldReader>()));
            }
        }
    }
}
