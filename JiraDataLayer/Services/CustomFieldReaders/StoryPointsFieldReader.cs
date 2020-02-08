using Atlassian.Jira;
using JiraDataLayer.Models.CustomFieldModels;
using JiraGraphThing.Core.Extensions;

namespace JiraDataLayer.Services.CustomFieldReaders
{
    class StoryPointsFieldReader : ICustomFieldReader<StoryPoints>
    {
        public string Field => "Story Points";

        public StoryPoints ReadValue(CustomFieldValue cf)
        {
            if (cf == null || cf.Values.IsNullOrEmpty())
                return null;
            else 
                return new StoryPoints(decimal.Parse(cf.Values[0]));
        }
    }
}
