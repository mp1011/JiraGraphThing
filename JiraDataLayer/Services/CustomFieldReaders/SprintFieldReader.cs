using Atlassian.Jira;
using JiraDataLayer.Models.CustomFieldModels;
using JiraGraphThing.Core.Extensions;

namespace JiraDataLayer.Services.CustomFieldReaders
{
    class SprintFieldReader : ICustomFieldReader<SprintField>
    {
        public string Field => "Sprint";

        public SprintField ReadValue(CustomFieldValue cf)
        {
            if (cf == null || cf.Values.IsNullOrEmpty())
                return null;
            else
                return new SprintField(cf.Values[0]);
        }
    }
}
