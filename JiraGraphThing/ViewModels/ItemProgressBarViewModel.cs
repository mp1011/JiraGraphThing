using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JiraDataLayer.Models;
using JiraDataLayer.Models.GraphModels;
using JiraGraphThing.Models;
using JiraGraphThing.Services;
using System.Linq;
using System.Windows.Input;

namespace JiraGraphThing.ViewModels
{
    public class ItemProgressBarViewModel : ViewModelBase
    {
        private readonly PageNavigationService _pageNavigationService;

        public ItemProgressBarViewModel(PageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public int MaxBarMinutes { get; set; } = 1;

        private UINode _uiNode;
        private JiraGraph _node => _uiNode?.Node;

        public bool EnableExpand => _uiNode != null && _uiNode.EnableExpand;
        public bool HasChildren => _uiNode != null && _uiNode.Node.GetChildren().Any();

        private bool _expanded = false;

        public bool IsItemCompleted
        {
            get
            {
                if (_node is IssueNode issue)
                {
                    return issue.Status.IsComplete;
                }

                return false;
            }
        }
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

        public UINode[] Children
        {
            get
            {
                if (_uiNode == null)
                    return new UINode[] { };
                else
                {
                    var maxPoints = _uiNode.Node.GetChildren().Max(p => p.GetTotalStoryPoints());
                    return _uiNode.Node
                        .GetChildren()
                        .OrderByDescending(p => p.GetTotalStoryPoints())
                        .Select(p => new UINode(p, _uiNode.Sprint, maxPoints, enableExpand: p.GetChildren().Any()))
                        .ToArray();
                }
            }
        }

        public string Title
        {
            get => _node == null ? string.Empty : _node.Name;
        }

        public int MinutesEstimated => _node == null ? 0 : (int)(_node.GetTotalStoryPoints() * (decimal)_uiNode.Sprint.TimePerStoryPoint.TotalMinutes);

        public int MinutesLogged => _node == null ? 0 : (int)_node.GetTotalTimeSpent().TotalMinutes;

        private ICommand _expandOrCollapse;
        public ICommand ExpandOrCollapse => _expandOrCollapse ?? (_expandOrCollapse =
            new RelayCommand(() =>
            {
                Expanded = !Expanded;
            }));

        public void NavigateToDetails()
        {
            _pageNavigationService.NavigateToWorkHistory(_uiNode);
        }

        public void Initialize(UINode node)
        {
            if(node != null && node != _uiNode)
            {
                MaxBarMinutes = (int)(node.MaxStoryPointsWithinParent * (decimal)node.Sprint.TimePerStoryPoint.TotalMinutes);
                if (MaxBarMinutes <= 0)
                    MaxBarMinutes = 1;

                _uiNode = node;

                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(MinutesLogged));
                RaisePropertyChanged(nameof(MinutesEstimated));
                RaisePropertyChanged(nameof(HasChildren));
                RaisePropertyChanged(nameof(Expanded));
                RaisePropertyChanged(nameof(EnableExpand));
                RaisePropertyChanged(nameof(IsItemCompleted));
                RaisePropertyChanged(nameof(MaxBarMinutes));
            }
        }
    }
}
