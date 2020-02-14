using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JiraDataLayer.Models.GraphModels;
using JiraDataLayer.Services;
using JiraGraphThing.Models;
using JiraGraphThing.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace JiraGraphThing.ViewModels
{
    public class SprintProgressViewModel : ViewModelBase
    {
        private readonly JiraGraphBuilder _jiraGraphBuilder;
        private readonly SprintService _sprintService;
        public SprintProgressViewModel(JiraGraphBuilder jiraGraphBuilder, SprintService sprintService,
            PageNavigationService navigationService)
        {
            _jiraGraphBuilder = jiraGraphBuilder;
            _jiraGraphBuilder.OnProgressChanged += OnProgressChanged;
            _sprintService = sprintService;

            GoBack = new RelayCommand(() =>
            {
                navigationService.NavigateToMainPage();
            }, keepTargetAlive:true);
        }

        private string _sprintName;
        public string SprintName
        {
            get => _sprintName;
            set
            {
                if (value != _sprintName)
                    Set(nameof(SprintName), ref _sprintName, value);
            }
        }

        public string _progressMessage;
        public string ProgressMessage
        {
            get => _progressMessage;
            set
            {
                if (value != _progressMessage)
                    Set(nameof(ProgressMessage), ref _progressMessage, value);
            }
        }

        public ObservableCollection<NodeWithSprint> SprintNodes { get; } = new ObservableCollection<NodeWithSprint>();

        public ICommand GoBack { get; }

        private void OnProgressChanged(string message, decimal progress)
        {
            ProgressMessage = $"{message} ({(progress * 100).ToString("0")}%)";
        }

        public async void Initialize(string sprintName)
        {
            var sprint = await _sprintService.GetSprint(sprintName);
            SprintName = sprintName;
            SprintNodes.Clear();
            var sprintNode = await _jiraGraphBuilder.LoadUserSprintGraph(sprint);

            SprintNodes.Add(new NodeWithSprint(sprintNode, sprint));
            foreach (var item in sprintNode.Children)            
                SprintNodes.Add(new NodeWithSprint(item, sprint));
        }
    }
}
