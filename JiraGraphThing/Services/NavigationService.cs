using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JiraGraphThing.Services
{
    public class PageNavigationService
    {
        private Frame _contentFrame;

        public Frame ContentFrame 
        { 
            get
            {
                if (_contentFrame != null)
                    return _contentFrame;

                var content = Window.Current.Content;
                if(content is Frame frame)
                {
                    if(frame.Content is Main main)
                    {
                        _contentFrame = main.GetContentHolder();
                        return _contentFrame;
                    }
                }

                return content as Frame;
            } 
        }

        public void NavigateToMainPage()
        {
            ContentFrame.Navigate(typeof(Main));
        }

        public void NavigateToSprint(string sprintName)
        {
            ContentFrame.Navigate(typeof(SprintProgress), sprintName);
        }

        public void NavigateToWorkHistory(JiraGraph node)
        {
            ContentFrame.Navigate(typeof(ItemWorkHistory), node);
        }
    }
}
