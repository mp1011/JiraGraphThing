using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using JiraGraphThing.Core.Extensions;
using JiraGraphThing.Services;
using JiraGraphThing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraGraphThing.IOC
{
    class DIRegistrar
    {
        public static void RegisterTypes()
        {
            JiraDataLayer.DIRegistrar.RegisterTypes();
            SimpleIoc.Default.Register<PageNavigationService>();

            SimpleIoc.Default.RegisterAllAsArray<ViewModelBase>(typeof(SprintProgressViewModel).Assembly);
        }
    }
}
