using GalaSoft.MvvmLight;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace JiraGraphThing.ViewModels
{
    public class ItemWorkHistoryViewModel : ViewModelBase
    {
        public ObservableCollection<UIWorkLog> WorkLogs { get; } = new ObservableCollection<UIWorkLog>();

        public void Initialize(JiraGraph issue)
        {
            WorkLogs.Clear();
            TimeSpan totalTimeSpent = TimeSpan.Zero;

            foreach(var log in issue.GetWorkLogs().OrderBy(p=> p.Start))
            {
                totalTimeSpent += log.TimeSpent;
                WorkLogs.Add(new UIWorkLog(log, totalTimeSpent));
            }
        }
    }

}
