using Atlassian.Jira;
using JiraDataLayer.Models.CustomFieldModels;
using JiraGraphThing.Core.Extensions;

namespace JiraDataLayer.Services.CustomFieldReaders
{
    class EpicLinkFieldReader : ICustomFieldReader<EpicLink>
    {
        public string Field => "Epic Link";

        public EpicLink ReadValue(CustomFieldValue cf)
        {
            if (cf == null || cf.Values.IsNullOrEmpty())
                return null;
            else 
                return new EpicLink(cf.Values[0]);
        }
    }
}
