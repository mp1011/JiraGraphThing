using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Services;

namespace JiraDataLayer
{
    public class DIRegistrar
    {
        public static void RegisterTypes()
        {
            SimpleIoc.Default.Register<IJiraConfig, JiraConfig>();
            SimpleIoc.Default.Register<JiraRestClientProvider>();
            SimpleIoc.Default.Register(()=> new JiraIssueService(SimpleIoc.Default.GetInstance<JiraRestClientProvider>()));
        }
    }
}
