using GalaSoft.MvvmLight;
using JiraDataLayer.Models.GraphModels;
using System.Linq;

namespace JiraGraphThing.ViewModels
{
    public class ItemProgressBarViewModel : ViewModelBase
    {
        private IssueNode _issueNode;

        public string Title
        {
            get => _issueNode?.Issue?.Key ?? string.Empty;           
        }

        public decimal PercentTimeUsed
        {
            get
            {
                if (_issueNode == null)
                    return 0;

                var totalTimeUsed = _issueNode.GetTotalTimeSpent().TotalHours;
                var timeExpected = _issueNode.GetTotalStoryPoints() * 6; //todo, makea setting
                if (timeExpected > 0)
                    return (decimal)totalTimeUsed / timeExpected;
                else
                    return 0;
            }
        }

        public void Initialize(IssueNode issueNode)
        {
            if(issueNode != null && issueNode != _issueNode)
            {
                _issueNode = issueNode;
                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(PercentTimeUsed));
            }
        }
    }
}
