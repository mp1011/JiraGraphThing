using Atlassian.Jira;
using JiraDataLayer.Models.CustomFieldModels;
using JiraGraphThing.Core.Extensions;

namespace JiraDataLayer.Services.CustomFieldReaders
{
    class SprintFieldReader : ICustomFieldReader<Sprint>
    {
        public string Field => "Sprint";

        public Sprint ReadValue(CustomFieldValue cf)
        {
            if (cf == null || cf.Values.IsNullOrEmpty())
                return null;
            else
                return new Sprint(cf.Values[0]);
        }
    }
}
