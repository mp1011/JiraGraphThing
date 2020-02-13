using GalaSoft.MvvmLight;
using JiraDataLayer.Models.GraphModels;
using System.Linq;

namespace JiraGraphThing.ViewModels
{
    public class ItemProgressBarViewModel : ViewModelBase
    {
        public const int MinutesPerStoryPoint = 6 * 60;
        public int MaxBarMinutes {get;set;} = 1;

        private JiraGraph _node;

        public string Title
        {
            get => _node == null ? string.Empty : _node.Name;           
        }

        public int MinutesEstimated => _node == null ? 0 : (int)(_node.GetTotalStoryPoints() * 60 * 6); //todo, make a setting

        public int MinutesLogged => _node == null ? 0 : (int)_node.GetTotalTimeSpent().TotalMinutes;

        public void Initialize(JiraGraph node)
        {
            if(node != null && node != _node)
            {                                
                if (node is SprintNode || node is UserSprintNode)
                    MaxBarMinutes = (int)(node.GetTotalStoryPoints() * MinutesPerStoryPoint);
                else
                    MaxBarMinutes = 10 * 4 * 6 * 60;//should be computed

                _node = node;
                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(MinutesLogged));
                RaisePropertyChanged(nameof(MinutesEstimated));

            }
        }
    }
}
