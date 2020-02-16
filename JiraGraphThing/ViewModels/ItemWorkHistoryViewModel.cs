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

        public UINode Node { get; private set; }

        public ObservableCollection<DailyWorkHistory> WorkLogs { get; } = new ObservableCollection<DailyWorkHistory>();

        public void Initialize(UINode node)
        {
            Node = node;
            WorkLogs.Clear();
            
            foreach(var workLog in _dailyWorkHistoryCalculator.CalculateWorkHistory(node.Sprint,node.Node))
            {
                WorkLogs.Add(workLog);
            }

            RaisePropertyChanged(nameof(Node));
        }
    }

}
