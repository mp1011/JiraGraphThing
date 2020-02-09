using GalaSoft.MvvmLight;
using JiraDataLayer.Models.GraphModels;
using JiraDataLayer.Services;
using System.Collections.ObjectModel;

namespace JiraGraphThing.ViewModels
{
    public class SprintProgressViewModel : ViewModelBase
    {
        private readonly JiraGraphBuilder _jiraGraphBuilder;

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

        public ObservableCollection<IssueNode> SprintIssues { get; } = new ObservableCollection<IssueNode>();

        public SprintProgressViewModel(JiraGraphBuilder jiraGraphBuilder)
        {
            _jiraGraphBuilder = jiraGraphBuilder;
            _jiraGraphBuilder.OnProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(string message, decimal progress)
        {
            ProgressMessage = $"{message} ({(progress * 100).ToString("0")}%)";
        }

        public async void Initialize(string sprintName)
        {
            SprintName = sprintName;
            SprintIssues.Clear();
            var sprintNode = await _jiraGraphBuilder.LoadSprintGraph(sprintName);
            foreach (var item in sprintNode.Children)
                SprintIssues.Add(item);
        }
    }
}
