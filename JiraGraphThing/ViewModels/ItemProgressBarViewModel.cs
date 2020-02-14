using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JiraDataLayer.Models.GraphModels;
using System.Linq;
using System.Windows.Input;

namespace JiraGraphThing.ViewModels
{
    public class ItemProgressBarViewModel : ViewModelBase
    {
        public const int MinutesPerStoryPoint = 6 * 60;
        public int MaxBarMinutes {get;set;} = 1;

        private JiraGraph _node;

        public bool HasChildren => _node != null && _node.GetChildren().Any();

        private bool _expanded = false;
        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (_expanded != value)
                    Set(nameof(Expanded), ref _expanded, value);
            }
        }

        public JiraGraph[] Children
        {
            get
            {
                if (_node == null)
                    return new JiraGraph[] { };
                else
                    return _node.GetChildren().ToArray();
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
                RaisePropertyChanged(nameof(HasChildren));
                RaisePropertyChanged(nameof(Expanded));

            }
        }
    }
}
