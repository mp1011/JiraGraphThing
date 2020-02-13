using Atlassian.Jira;
using JiraDataLayer.Models.CustomFieldModels;
using JiraDataLayer.Models.DTO;
using JiraDataLayer.Services;

namespace JiraDataLayer.Models
{
    public class JiraIssue 
    {
        public string Key { get; private set; }

        public string EpicKey { get; private set; }

        public string ParentKey { get; private set; }

        public string Project { get; private set; }

        public string TypeName { get; private set; }

        public string Sprint { get; private set; }

        public string Assignee { get; private set; }

        public string Summary { get; private set; }

        public decimal? StoryPoints { get; private set; }

        internal JiraIssue()
        {
        }

        internal JiraIssue(Issue issue, CustomFieldReader customFieldReader)
        {
            Key = issue.Key.Value;
            Summary = issue.Summary;
            Project = issue.Project;
            Assignee = issue.Assignee;
            EpicKey = customFieldReader.ReadCustomField<EpicLink>(issue)?.Key;
            StoryPoints = customFieldReader.ReadCustomField<StoryPoints>(issue)?.Value;
            Sprint = customFieldReader.ReadCustomField<Sprint>(issue)?.Name;

            ParentKey = issue.ParentIssueKey ?? EpicKey;
            TypeName = issue.Type.Name;
            
        }

        //hack
        public JiraIssue RemoveStoryPoints()
        {
            return new JiraIssue
            {
                Assignee = Assignee,
                EpicKey = EpicKey,
                ParentKey = ParentKey,
                Project = Project,
                Sprint = Sprint,
                Summary = Summary,
                TypeName = TypeName,
                Key = Key
            };
        }

        public override string ToString()
        {
            return $"{Key} {Summary}";
        }
    }
}
