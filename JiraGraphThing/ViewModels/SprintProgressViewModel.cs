using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JiraDataLayer.Models.GraphModels;
using JiraDataLayer.Services;
using JiraGraphThing.Models;
using JiraGraphThing.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JiraGraphThing.ViewModels
{
    public class SprintProgressViewModel : ViewModelBase
    {
        private readonly JiraGraphBuilder _jiraGraphBuilder;
        private readonly SprintService _sprintService;
        public SprintProgressViewModel(JiraGraphBuilder jiraGraphBuilder, SprintService sprintService)
        {
            _jiraGraphBuilder = jiraGraphBuilder;
            _jiraGraphBuilder.OnProgressChanged += OnProgressChanged;
            _sprintService = sprintService;
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

        public ObservableCollection<UINode> SprintNodes { get; } = new ObservableCollection<UINode>();
     
        private void OnProgressChanged(string message, decimal progress)
        {
            ProgressMessage = $"{message} ({(progress * 100).ToString("0")}%)";
        }

        public async Task Initialize(string sprintName)
        {
            if (sprintName == SprintName)
                return;

            var sprint = await _sprintService.GetSprint(sprintName);
            SprintName = sprintName;
            SprintNodes.Clear();
            var sprintNode = await _jiraGraphBuilder.LoadUserSprintGraph(sprint);

            SprintNodes.Add(new UINode(sprintNode, sprint, sprintNode.GetTotalStoryPoints(), enableExpand: false));
            foreach (var item in sprintNode.Children.OrderByDescending(p=>p.GetTotalStoryPoints()))
            {
                var maxEstimated = sprintNode.Children.Max(p => p.GetTotalStoryPoints());
                SprintNodes.Add(new UINode(item, sprint, maxEstimated, enableExpand: item.Children.Any()));
            }
        }
    }
}
