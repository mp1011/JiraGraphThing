using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Models;
using System.Linq;
using System.Windows.Input;

namespace JiraGraphThing.ViewModels
{
    public class ItemProgressBarViewModel : ViewModelBase
    {
        public int MaxBarMinutes {get;set;} = 1;

        private JiraGraph _node;
        private Sprint _sprint;

        public bool HasChildren => _node != null && _node.GetChildren().Any();

        private bool _expanded = false;
        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (_expanded != value)
                {
                    Set(nameof(Expanded), ref _expanded, value);
                    RaisePropertyChanged(nameof(Children));
                }
            }
        }

        public NodeWithSprint[] Children
        {
            get
            {
                if (_node == null)
                    return new NodeWithSprint[] { };
                else
                    return _node.GetChildren().Select(p=> new NodeWithSprint(p,_sprint)).ToArray();
            }
        }

        public string Title
        {
            get => _node == null ? string.Empty : _node.Name;           
        }

        public int MinutesEstimated => _node == null ? 0 : (int)(_node.GetTotalStoryPoints() * 60 * 6); //todo, make a setting

        public int MinutesLogged => _node == null ? 0 : (int)_node.GetTotalTimeSpent().TotalMinutes;

        private ICommand _expandOrCollapse;
        public ICommand ExpandOrCollapse => _expandOrCollapse ?? (_expandOrCollapse =
            new RelayCommand(() =>
            {
                Expanded = !Expanded;
            }));

        public void Initialize(JiraGraph node, Sprint sprint)
        {
            if(node != null && node != _node)
            {                                
                if (node is SprintNode || node is UserSprintNode)
                    MaxBarMinutes = (int)(node.GetTotalStoryPoints() * (decimal)sprint.TimePerStoryPoint.TotalMinutes);
                else
                    MaxBarMinutes = 10 * 4 * 6 * 60;//should be computed

                _node = node;
                _sprint = sprint;

                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(MinutesLogged));
                RaisePropertyChanged(nameof(MinutesEstimated));
                RaisePropertyChanged(nameof(HasChildren));
                RaisePropertyChanged(nameof(Expanded));

            }
        }
    }
}
