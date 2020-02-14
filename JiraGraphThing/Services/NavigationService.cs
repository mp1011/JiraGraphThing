using JiraGraphThing.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JiraGraphThing.Services
{
    public class PageNavigationService
    {
        private static Frame AppFrame => Window.Current.Content as Frame;

        public void NavigateToMainPage()
        {
            AppFrame.Navigate(typeof(SprintPicker));
        }

        public void NavigateToSprint(string sprintName)
        {
            AppFrame.Navigate(typeof(SprintProgress), sprintName);
        }
    }
}
