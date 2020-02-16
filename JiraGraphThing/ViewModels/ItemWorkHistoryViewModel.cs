using GalaSoft.MvvmLight;
using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;
using JiraDataLayer.Services;
using JiraGraphThing.Models;
using System.Collections.ObjectModel;

namespace JiraGraphThing.ViewModels
{
    public class ItemWorkHistoryViewModel : ViewModelBase
    {
        private readonly DailyWorkHistoryCalculator _dailyWorkHistoryCalculator;

        public ItemWorkHistoryViewModel(DailyWorkHistoryCalculator dailyWorkHistoryCalculator)
        {
            _dailyWorkHistoryCalculator = dailyWorkHistoryCalculator;
        }

        public ObservableCollection<DailyWorkHistory> WorkLogs { get; } = new ObservableCollection<DailyWorkHistory>();

        public void Initialize(UINode node)
        {
            WorkLogs.Clear();
            
            foreach(var workLog in _dailyWorkHistoryCalculator.CalculateWorkHistory(node.Sprint,node.Node))
            {
                WorkLogs.Add(workLog);
            }
        }
    }

}
