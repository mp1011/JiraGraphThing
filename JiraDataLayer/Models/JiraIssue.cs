using Atlassian.Jira;
using JiraDataLayer.Models.CustomFieldModels;
using JiraDataLayer.Services;

namespace JiraDataLayer.Models
{
    public class JiraIssue
    {
        public string Key { get; }

        public string EpicKey { get; }

        public string ParentKey { get; }

        public string Project { get; }

        public string TypeName { get; }

        public string Sprint { get; }

        public decimal? StoryPoints { get; }

        internal JiraIssue(Issue issue, CustomFieldReader customFieldReader)
        {
            Key = issue.Key.Value;
            Project = issue.Project;

            EpicKey = customFieldReader.ReadCustomField<EpicLink>(issue)?.Key;
            StoryPoints = customFieldReader.ReadCustomField<StoryPoints>(issue)?.Value;
            Sprint = customFieldReader.ReadCustomField<Sprint>(issue)?.Name;

            ParentKey = issue.ParentIssueKey ?? EpicKey;
            TypeName = issue.Type.Name;
            
        }

        public override string ToString()
        {
            return $"{TypeName} {Key}";
        }
    }
}
