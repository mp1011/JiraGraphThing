using GalaSoft.MvvmLight;
using JiraDataLayer.Models.GraphModels;
using System.Linq;

namespace JiraGraphThing.ViewModels
{
    public class ItemProgressBarViewModel : ViewModelBase
    {
        public const int MinutesPerStoryPoint = 6 * 60;
        public const int MaxBarMinutes = 60 * 6 * 5;

        private JiraGraph _node;

        public string Title
        {
            get => _node == null ? string.Empty : _node.Name;           
        }

        public int MinutesEstimated => _node == null ? 0 : (int)(_node.GetTotalStoryPoints() * 60 * 6); //todo, make a setting

        public int MinutesLogged => _node == null ? 0 : (int)_node.GetTotalTimeSpent().TotalMinutes;

        public bool IsIssue => _node is IssueNode;
        public bool IsSprint => _node is SprintNode;

        public void Initialize(JiraGraph node)
        {
            if(node != null && node != _node)
            {
                _node = node;
                RaisePropertyChanged(nameof(Title));
            }
        }
    }
}
