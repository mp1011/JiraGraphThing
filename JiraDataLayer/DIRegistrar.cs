using GalaSoft.MvvmLight.Ioc;
using JiraDataLayer.Services;
using JiraDataLayer.Services.CustomFieldReaders;
using JiraDataLayer.SqLite;
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

                SimpleIoc.Default.Register<AutoMapperService>();
                SimpleIoc.Default.Register<SQLiteConnectionProvider>();
                SimpleIoc.Default.Register<SQLiteTableCreator>();
                SimpleIoc.Default.RegisterInternal<SQLiteDAO>();
                SimpleIoc.Default.RegisterInternal<SQLiteCacheProvider>();
              
                SimpleIoc.Default.Register<IJiraConfig, JiraConfig>();
                SimpleIoc.Default.Register<JiraRestClientProvider>();
                SimpleIoc.Default.Register<JiraGraphBuilder>();
                SimpleIoc.Default.Register<CustomFieldReader>();
                SimpleIoc.Default.RegisterAllAsArray<ICustomFieldReader>();
                SimpleIoc.Default.RegisterInternal<JiraIssueService>();
            }
        }
    }
}
