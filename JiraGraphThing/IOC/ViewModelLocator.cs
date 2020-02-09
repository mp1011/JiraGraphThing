using GalaSoft.MvvmLight.Ioc;
using System;

namespace JiraGraphThing.IOC
{
    public class ViewModelLocator
    {
        public object this[string viewModelName]
        {
            get
            {
                try
                {
                    var viewModelType = GetViewModelType(viewModelName);
                    var viewModel = SimpleIoc.Default.GetInstance(viewModelType);
                    return viewModel ?? throw new NullReferenceException();
                }
                catch (Exception e)
                {
                    throw new Exception($"Unable to resolve view model {viewModelName}", e);
                }
            }
        }

        private Type GetViewModelType(string viewModelName)
        {
            var assembly = typeof(ViewModelLocator).Assembly;
            var fullName = $"JiraGraphThing.ViewModels.{viewModelName}";
            var type = assembly.GetType(fullName);
            return type;
        }
    }

}
